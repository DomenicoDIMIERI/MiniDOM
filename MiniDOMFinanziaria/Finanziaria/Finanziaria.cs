using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Databases;
using static minidom.Sistema;

namespace minidom
{
    public sealed partial class Finanziaria
    {

        /// <summary>
    /// Evento generato quando viene impostato il DB del modulo Finanziaria
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
        public static event DatabaseChangedEventHandler DatabaseChanged;

        public delegate void DatabaseChangedEventHandler(object sender, EventArgs e);

        private static bool m_Initialized = false;
        private static Sistema.CModule m_Module;
        private static Sistema.CGroup m_GruppoOperatori = null;
        private static Sistema.CGroup m_GruppoReferenti = null;
        private static Sistema.CGroup m_GruppoSupervisori = null;
        private static Sistema.CGroup m_GruppoAutorizzatori = null;
        private static Sistema.CGroup m_GruppoConsulenti = null;
        private static Databases.CDBConnection m_Database;

        private Finanziaria()
        {
            DMDObject.IncreaseCounter(this);
        }




        // Private Shared m_Lock As New Object
        private static DBObjectBase GetConfigProvider(int id)
        {
            return Configuration;
        }

        /// <summary>
    /// Inizializza il modulo
    /// </summary>
    /// <remarks></remarks>
        private static void Initialize()
        {
            // SyncLock m_Lock
            if (m_Initialized)
                return;
            m_Initialized = true;
            Anagrafica.PersonaMerged += HandlePersonaMerged;
            Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
            Anagrafica.PersonaModified += HandlePersonaModified;
            Anagrafica.PersonaCreated += HandlePersonaCreated;
            CustomerCalls.Visite.ItemCreated += HandleContatto;
            CustomerCalls.Visite.ItemDeleted += HandleContatto;
            CustomerCalls.Visite.ItemModified += HandleContatto;
            CustomerCalls.Telefonate.ItemCreated += HandleContatto;
            CustomerCalls.Telefonate.ItemDeleted += HandleContatto;
            CustomerCalls.Telefonate.ItemModified += HandleContatto;
            Sistema.Attachments.ItemCreated += HandleAttachment;
            Sistema.Attachments.ItemDeleted += HandleAttachment;
            Sistema.Attachments.ItemModified += HandleAttachment;
            Office.RichiesteCERQ.ItemCreated += HandleRichiesta;
            Office.RichiesteCERQ.ItemDeleted += HandleRichiesta;
            Office.RichiesteCERQ.ItemModified += HandleRichiesta;
            Estinzioni.ItemCreated += HandleAltroPrestito;
            Estinzioni.ItemDeleted += HandleAltroPrestito;
            Estinzioni.ItemModified += HandleAltroPrestito;
            RichiesteFinanziamento.ItemCreated += HandleRichiestaFinanziamento;
            RichiesteFinanziamento.ItemDeleted += HandleRichiestaFinanziamento;
            RichiesteFinanziamento.ItemModified += HandleRichiestaFinanziamento;
            RichiesteConteggi.ItemCreated += HandleRichiestaConteggi;
            RichiesteConteggi.ItemDeleted += HandleRichiestaConteggi;
            RichiesteConteggi.ItemModified += HandleRichiestaConteggi;
            Consulenze.ItemCreated += HandleOfferta;
            Consulenze.ItemDeleted += HandleOfferta;
            Consulenze.ItemModified += HandleOfferta;
            Pratiche.ItemCreated += HandlePratica;
            Pratiche.ItemDeleted += HandlePratica;
            Pratiche.ItemModified += HandlePratica;
            Sistema.Types.RegisteredTypeProviders.Add("CEstinzione", Estinzioni.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CPraticaCQSPD", Pratiche.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CQSPDConsulenza", Consulenze.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CCQSPDConfig", Finanziaria.GetConfigProvider);
            Sistema.Types.RegisteredTypeProviders.Add("CImportExportSource", ImportExportSources.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CImportExport", ImportExport.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CCQSPDCessionarioClass", Cessionari.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CConsulentePratica", Consulenti.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CCollaboratore", Collaboratori.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CProfilo", Profili.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("COffertaCQS", Offerte.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CStatoPratica", StatiPratica.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CCQSPDProdotto", Prodotti.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CGruppoProdotti", GruppiProdotto.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CCQSPDTipoProvvigione", CCQSPDTipoProvvigione_GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("CMotivoScontoPratica", minidom.Finanziaria.MotiviSconto.GetItemById);
            Sistema.Types.RegisteredTypeProviders.Add("RichiestaApprovazioneGroup", RichiesteApprovazione.GetItemById);
            var lista = Anagrafica.ListeRicontatto.GetItemByName("Altri Prestiti Rinnovabili");
            if (lista is null)
            {
                lista = new Anagrafica.CListaRicontatti();
                lista.Name = "Altri Prestiti Rinnovabili";
                lista.Stato = ObjectStatus.OBJECT_VALID;
                lista.Descrizione = "Lista predefinita per i rinnovi degli altri prestiti";
                lista.Save();
            }

            int cnt = 0;
            cnt += GruppoOperatori.Members.Count;
            cnt += GruppoAutorizzatori.Members.Count;
            cnt += GruppoReferenti.Members.Count;
            cnt += GruppoSupervisori.Members.Count;
            cnt += GruppoConsulenti.Members.Count;
            Estinzioni.Initialize();
            RichiesteConteggi.Initialize();
            StatiPratica.Initialize();
            Pratiche.Initialize();
            ImportExportSources.Initialize();
            ImportExport.Initialize();


            // End SyncLock
        }

        private static CCQSPDTipoProvvigione CCQSPDTipoProvvigione_GetItemById(int id)
        {
            var cursor = new CCQSPDTipoProvvigioneCursor();
            cursor.ID.Value = id;
            cursor.IgnoreRights = true;
            var ret = cursor.Item;
            cursor.Dispose();
            return ret;
        }

        /// <summary>
    /// Inizializza il modulo
    /// </summary>
    /// <remarks></remarks>
        private static void Terminate()
        {
            // SyncLock m_Lock
            if (!m_Initialized)
                return;
            Anagrafica.PersonaMerged -= HandlePersonaMerged;
            Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
            Anagrafica.PersonaModified -= HandlePersonaModified;
            Anagrafica.PersonaCreated -= HandlePersonaCreated;
            CustomerCalls.Visite.ItemCreated -= HandleContatto;
            CustomerCalls.Visite.ItemDeleted -= HandleContatto;
            CustomerCalls.Visite.ItemModified -= HandleContatto;
            CustomerCalls.Telefonate.ItemCreated -= HandleContatto;
            CustomerCalls.Telefonate.ItemDeleted -= HandleContatto;
            CustomerCalls.Telefonate.ItemModified -= HandleContatto;
            Sistema.Attachments.ItemCreated -= HandleAttachment;
            Sistema.Attachments.ItemDeleted -= HandleAttachment;
            Sistema.Attachments.ItemModified -= HandleAttachment;
            Office.RichiesteCERQ.ItemCreated -= HandleRichiesta;
            Office.RichiesteCERQ.ItemDeleted -= HandleRichiesta;
            Office.RichiesteCERQ.ItemModified -= HandleRichiesta;
            Estinzioni.ItemCreated -= HandleAltroPrestito;
            Estinzioni.ItemDeleted -= HandleAltroPrestito;
            Estinzioni.ItemModified -= HandleAltroPrestito;
            RichiesteFinanziamento.ItemCreated -= HandleRichiestaFinanziamento;
            RichiesteFinanziamento.ItemDeleted -= HandleRichiestaFinanziamento;
            RichiesteFinanziamento.ItemModified -= HandleRichiestaFinanziamento;
            RichiesteConteggi.ItemCreated -= HandleRichiestaConteggi;
            RichiesteConteggi.ItemDeleted -= HandleRichiestaConteggi;
            RichiesteConteggi.ItemModified -= HandleRichiestaConteggi;
            Consulenze.ItemCreated -= HandleOfferta;
            Consulenze.ItemDeleted -= HandleOfferta;
            Consulenze.ItemModified -= HandleOfferta;
            Pratiche.ItemCreated -= HandlePratica;
            Pratiche.ItemDeleted -= HandlePratica;
            Pratiche.ItemModified -= HandlePratica;
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CEstinzione");
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CPraticaCQSPD");
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CQSPDConsulenza");
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CCQSPDConfig");
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CImportExportSource");
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CImportExport");
            ImportExportSources.Terminate();
            ImportExport.Terminate();
            Pratiche.Terminate();
            Estinzioni.Terminate();
            // End SyncLock
        }

        /// <summary>
    /// Restituisce o imposta il database specifico per il modulo Finanziaria
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static Databases.CDBConnection Database
        {
            get
            {
                if (m_Database is null)
                    return Databases.APPConn;
                return m_Database;
            }

            set
            {
                if (ReferenceEquals(value, m_Database))
                    return;
                Terminate();
                m_Database = value;
                Initialize();
                DatabaseChanged?.Invoke(null, new EventArgs());
            }
        }

        private static bool IsDocumentoDiReddito(Sistema.CAttachment a)
        {
            string cat = Strings.LCase(Strings.Trim(a.Categoria));
            switch (cat ?? "")
            {
                case "busta paga":
                case "obis m":
                case "cud inpdap":
                case "cud":
                    {
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }

        private static void HandleAttachment(object sender, ItemEventArgs e)
        {
            // Dim att As CAttachment = e.Item
            // If (att.Stato = ObjectStatus.OBJECT_VALID AndAlso IsDocumentoDiReddito(att) AndAlso att.IDOwner <> 0 AndAlso att.OwnerType = "CPersonaFisica") Then
            // Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(att.IDOwner)
            // Dim w As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(p)
            // If (w Is Nothing) Then w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(p)
            // If (w IsNot Nothing) Then
            // If w.StatoFinestra = StatoFinestraLavorazione.NonAperta Then
            // If (Calendar.CheckBetween(att.DataInizio, w.DataInizioLavorabilita, w.DataFineLavorabilita, 3600 * 24)) Then
            // w.StatoFinestra = StatoFinestraLavorazione.Aperta
            // w.DataInizioLavorazione = Calendar.Now
            // w.DataUltimoAggiornamento = Calendar.Now
            // w.BustaPaga = att
            // w.DataBustaPaga = Calendar.Now
            // w.StatoBustaPaga = StatoOfferteFL.Liquidata
            // w.Save()
            // End If
            // ElseIf w.StatoFinestra <> StatoFinestraLavorazione.Chiusa Then
            // If (Calendar.CheckBetween(att.DataInizio, w.DataInizioLavorazione, w.DataFineLavorazione, 3600 * 24)) Then
            // w.StatoFinestra = StatoFinestraLavorazione.Aperta
            // w.DataUltimoAggiornamento = Calendar.Now
            // w.BustaPaga = att
            // w.DataBustaPaga = Calendar.Now
            // w.StatoBustaPaga = StatoOfferteFL.Liquidata
            // w.Save()
            // End If
            // End If
            // End If

            // 'Dim cursor As New ClientiLavoratiStatsItemCursor
            // 'Dim data As Date
            // 'If (att.DataInizio.HasValue) Then
            // '    data = att.DataInizio.Value
            // 'Else
            // '    data = att.CreatoIl
            // 'End If

            // 'cursor.IDCliente.Value = att.IDOwner
            // 'cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
            // 'cursor.Stato.Value = ObjectStatus.OBJECT_VALID

            // 'Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
            // 'cursor.Dispose()
            // 'If (statsItem Is Nothing) Then
            // '    statsItem = New ClientiLavoratiStatsItem
            // '    statsItem.Cliente = Anagrafica.Persone.GetItemById(att.IDOwner)
            // '    statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '    statsItem.DataInizioLavorazione = data
            // '    statsItem.DataUltimoAggiornamento = data
            // 'Else
            // '    Select Case statsItem.SottostatoLavorazione
            // '        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
            // '        Case Else
            // '            'FIX
            // '            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

            // '            If (Maths.Abs(Calendar.DateDiff(DateTimeInterval.Month, statsItem.DataUltimoAggiornamento.Value, data)) >= 3) Then
            // '                statsItem = New ClientiLavoratiStatsItem
            // '                statsItem.Cliente = Anagrafica.Persone.GetItemById(att.IDOwner)
            // '                statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '                statsItem.DataInizioLavorazione = data
            // '                statsItem.DataUltimoAggiornamento = data
            // '            End If
            // '    End Select
            // 'End If
            // 'statsItem.NotifyBustaPaga(att)
            // 'statsItem.Save()

            // End If
        }

        private static bool IsRichiestaCE(Office.RichiestaCERQ rich)
        {
            switch (DMD.Strings.LCase(Strings.Trim(rich.TipoRichiesta)))
            {
                case "richiesta ce":
                case "certificato di stipendio":
                case "cud":
                    {
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }

        private static StatoOfferteFL WGetStatoRichestaCERQ(Office.RichiestaCERQ rich)
        {
            switch (rich.StatoOperazione)
            {
                case var @case when @case == Office.StatoRichiestaCERQ.ANNULLATA:
                    {
                        return StatoOfferteFL.BocciataAgenzia;
                    }

                case var case1 when case1 == Office.StatoRichiestaCERQ.DA_RICHIEDERE:
                    {
                        return StatoOfferteFL.InLavorazione;
                    }

                case var case2 when case2 == Office.StatoRichiestaCERQ.RICHIESTA:
                    {
                        return StatoOfferteFL.InLavorazione;
                    }

                case var case3 when case3 == Office.StatoRichiestaCERQ.RIFIUTATA:
                    {
                        return StatoOfferteFL.BocciataCessionario;
                    }

                case var case4 when case4 == Office.StatoRichiestaCERQ.RITIRATA:
                    {
                        return StatoOfferteFL.Liquidata;
                    }
            }

            return StatoOfferteFL.Sconosciuto;
        }

        private static void HandleRichiesta(object sender, ItemEventArgs e)
        {
            // Dim rich As Office.RichiestaCERQ = e.Item

            // If rich.Stato <> ObjectStatus.OBJECT_VALID OrElse Not IsRichiestaCE(rich) Then Exit Sub

            // Dim p As CPersonaFisica = rich.Cliente
            // If (p Is Nothing) Then Exit Sub

            // Dim w As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(p)
            // If (w Is Nothing) Then w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(p)
            // If (w IsNot Nothing) Then
            // If w.StatoFinestra = StatoFinestraLavorazione.NonAperta Then
            // If (Calendar.CheckBetween(rich.Data, w.DataInizioLavorabilita, w.DataFineLavorabilita, 3600 * 24)) Then
            // w.StatoFinestra = StatoFinestraLavorazione.Aperta
            // w.DataInizioLavorazione = Calendar.Now
            // w.DataUltimoAggiornamento = Calendar.Now
            // w.RichiestaCertificato = rich
            // w.DataRichiestaCertificato = Calendar.Now
            // w.StatoRichiestaCertificato = WGetStatoRichestaCERQ(rich)
            // w.Save()
            // End If
            // ElseIf w.StatoFinestra <> StatoFinestraLavorazione.Chiusa Then
            // If (Calendar.CheckBetween(rich.Data, w.DataInizioLavorazione, w.DataFineLavorazione, 3600 * 24)) Then
            // w.StatoFinestra = StatoFinestraLavorazione.Aperta
            // w.DataUltimoAggiornamento = Calendar.Now
            // w.RichiestaCertificato = rich
            // w.DataRichiestaCertificato = Calendar.Now
            // w.StatoRichiestaCertificato = WGetStatoRichestaCERQ(rich)
            // w.Save()
            // End If
            // End If
            // End If
        }

        private static void HandleContatto(object sender, ItemEventArgs e)
        {
            // 'If (TypeOf (e.Item) Is CVisita) Then
            // Dim visita As CContattoUtente = e.Item
            // If (visita.Stato <> ObjectStatus.OBJECT_VALID) Then Return

            // Dim persona As CPersona
            // If (visita.IDPerContoDi <> 0) Then
            // persona = visita.PerContoDi
            // Else
            // persona = visita.Persona
            // End If
            // If Not (TypeOf (persona) Is CPersonaFisica) Then Return

            // Dim w As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(persona)
            // If (w Is Nothing) Then w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(persona)

            // If (w Is Nothing) Then Return

            // w.NotificaContatto(visita)

            // 'Dim cursor As New ClientiLavoratiStatsItemCursor
            // 'cursor.IDCliente.Value = GetID(persona)
            // 'cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
            // 'cursor.Stato.Value = ObjectStatus.OBJECT_VALID

            // 'Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
            // 'cursor.Dispose()
            // 'If (statsItem Is Nothing) Then
            // '    statsItem = New ClientiLavoratiStatsItem
            // '    statsItem.Cliente = persona
            // '    statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '    statsItem.DataInizioLavorazione = visita.Data
            // '    statsItem.DataUltimoAggiornamento = visita.Data
            // 'Else
            // '    Select Case statsItem.SottostatoLavorazione
            // '        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
            // '        Case Else
            // '            'FIX
            // '            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

            // '            If (Maths.Abs(Calendar.DateDiff(DateTimeInterval.Month, statsItem.DataUltimoAggiornamento.Value, visita.Data)) >= 3) Then
            // '                statsItem = New ClientiLavoratiStatsItem
            // '                statsItem.Cliente = visita.Persona
            // '                statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '                statsItem.DataInizioLavorazione = visita.Data
            // '                statsItem.DataUltimoAggiornamento = visita.Data
            // '            End If
            // '    End Select
            // 'End If
            // 'statsItem.NotifyVisita(visita)
            // 'statsItem.Save()

            // 'End If
        }

        private static void HandleAltroPrestito(object sender, ItemEventArgs e)
        {
            // Dim est As CEstinzione = e.Item
            // If est.Persona Is Nothing OrElse Not TypeOf (est.Persona) Is CPersonaFisica Then Exit Sub
            // Dim persona As CPersonaFisica = est.Persona
            // Dim w As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(persona)
            // If (w Is Nothing) Then w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(persona)
            // If (w Is Nothing) Then Exit Sub


            // Dim dataRinnovo As Date? = Nothing
            // Dim isRinnovo As Boolean = False
            // Dim dIni As Date? = Nothing

            // Dim ultima As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetUltimaFinestraLavorata(persona)
            // If Not (ultima Is Nothing) Then dataRinnovo = ultima.DataFineLavorazione

            // Dim prestiti As CCollection(Of CEstinzione) = Finanziaria.Estinzioni.GetPrestitiAttivi(persona)
            // Dim minPrestito As Date? = Nothing

            // If (w IsNot Nothing) Then
            // dIni = w.DataInizioLavorazione
            // If (dIni.HasValue = False) Then dIni = w.DataInizioLavorabilita
            // End If

            // isRinnovo = prestiti.Count > 0
            // For Each p As CEstinzione In prestiti
            // If (dIni.HasValue) Then
            // If (p.DataRinnovo.HasValue AndAlso p.DataRinnovo.Value > dIni.Value) Then
            // minPrestito = Calendar.Min(minPrestito, p.DataRinnovo)
            // End If
            // Else
            // minPrestito = Calendar.Min(minPrestito, p.DataRinnovo)
            // End If
            // Next

            // dataRinnovo = Calendar.Max(minPrestito, dataRinnovo)
            // If (dataRinnovo.HasValue = False) Then Return

            // w.SetFlag(FinestraLavorazioneFlags.Rinnovo, isRinnovo)
            // w.DataInizioLavorabilita = dataRinnovo.Value
            // w.Save()
        }

        private static void HandleRichiestaFinanziamento(object sender, ItemEventArgs e)
        {
            // Dim rich As CRichiestaFinanziamento = e.Item
            // Dim cliente As CPersona = rich.Cliente
            // If (cliente Is Nothing) Then Exit Sub
            // Dim w As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetFinestra(cliente, rich.Data)
            // If (w Is Nothing) Then w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(cliente)
            // If (w Is Nothing) Then Exit Sub

            // w.NotificaRichiesta(rich)

            // 'Dim cursor As New ClientiLavoratiStatsItemCursor
            // 'cursor.IDCliente.Value = rich.IDCliente
            // 'cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
            // 'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // 'Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
            // 'cursor.Dispose()
            // 'If (statsItem Is Nothing) Then
            // '    statsItem = New ClientiLavoratiStatsItem
            // '    statsItem.Cliente = rich.Cliente
            // '    statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '    statsItem.DataInizioLavorazione = rich.Data
            // '    statsItem.DataUltimoAggiornamento = rich.Data
            // 'Else
            // '    Select Case statsItem.SottostatoLavorazione
            // '        Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
            // '        Case Else
            // '            'FIX
            // '            If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

            // '            If (Maths.Abs(Calendar.DateDiff(DateTimeInterval.Month, statsItem.DataUltimoAggiornamento.Value, rich.Data)) >= 3) Then
            // '                statsItem = New ClientiLavoratiStatsItem
            // '                statsItem.Cliente = rich.Cliente
            // '                statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '                statsItem.DataInizioLavorazione = rich.Data
            // '                statsItem.DataUltimoAggiornamento = rich.Data
            // '            End If
            // '    End Select
            // 'End If
            // 'statsItem.NotifyRichiestaFinanziamento(rich)
            // 'statsItem.Save()
        }

        private static void HandleRichiestaConteggi(object sender, ItemEventArgs e)
        {
            CRichiestaConteggio rich = (CRichiestaConteggio)e.Item;
            // Dim cursor As New ClientiLavoratiStatsItemCursor
            // Dim data As Date
            // If (rich.DataRichiesta.HasValue) Then
            // data = rich.DataRichiesta.Value
            // Else
            // data = rich.CreatoIl
            // End If

            // cursor.IDCliente.Value = rich.IDCliente
            // cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
            // cursor.Dispose()
            // If (statsItem Is Nothing) Then
            // statsItem = New ClientiLavoratiStatsItem
            // statsItem.Cliente = rich.Cliente
            // statsItem.Stato = ObjectStatus.OBJECT_VALID
            // statsItem.DataInizioLavorazione = data
            // statsItem.DataUltimoAggiornamento = data
            // Else
            // Select Case statsItem.SottostatoLavorazione
            // Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
            // Case Else
            // 'FIX
            // If (statsItem.DataUltimoAggiornamento.HasValue = False) Then statsItem.DataUltimoAggiornamento = statsItem.DataInizioLavorazione

            // If (Maths.Abs(Calendar.DateDiff(DateTimeInterval.Month, statsItem.DataUltimoAggiornamento.Value, data)) >= 3) Then
            // statsItem = New ClientiLavoratiStatsItem
            // statsItem.Cliente = rich.Cliente
            // statsItem.Stato = ObjectStatus.OBJECT_VALID
            // statsItem.DataInizioLavorazione = data
            // statsItem.DataUltimoAggiornamento = data
            // End If
            // End Select
            // End If
            // statsItem.NotifyRichiestaConteggio(rich)
            // statsItem.Save()
        }

        private static void HandleOfferta(object sender, ItemEventArgs e)
        {
            CQSPDConsulenza cons = (CQSPDConsulenza)e.Item;
            var conn = Database;
            if (conn.IsRemote())
            {
            }
            else if (cons.Stato == ObjectStatus.OBJECT_DELETED)
            {
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" + ((int)StatoRichiestaApprovazione.ANNULLATA).ToString() + ", [DettaglioConferma]='Studio di fattibilità eliminato' WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_DELETED).ToString() + " AND [StatoRichiesta]<=" + ((int)StatoRichiestaApprovazione.PRESAINCARICO).ToString() + " AND [IDOggettoApprovabile]=" + DBUtils.GetID(cons) + " AND [TipoOggettoApprovabile]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(cons)));
            }
            else if (cons.StatoConsulenza == StatiConsulenza.BOCCIATA)
            {
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" + ((int)StatoRichiestaApprovazione.ANNULLATA).ToString() + ", [DettaglioConferma]='Studio di fattibilità bocciato' WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_DELETED).ToString() + " AND [StatoRichiesta]<=" + ((int)StatoRichiestaApprovazione.PRESAINCARICO).ToString() + " AND [IDOggettoApprovabile]=" + DBUtils.GetID(cons) + " AND [TipoOggettoApprovabile]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(cons)));
            }
            else if (cons.StatoConsulenza == StatiConsulenza.RIFIUTATA)
            {
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" + ((int)StatoRichiestaApprovazione.ANNULLATA).ToString() + ", [DettaglioConferma]='Studio di fattibilità rifiutato' WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_DELETED).ToString() + " AND [StatoRichiesta]<=" + ((int)StatoRichiestaApprovazione.PRESAINCARICO).ToString() + " AND [IDOggettoApprovabile]=" + DBUtils.GetID(cons) + " AND [TipoOggettoApprovabile]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(cons)));
            }
            else if (cons.StatoConsulenza == StatiConsulenza.NONFATTIBILE)
            {
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" + ((int)StatoRichiestaApprovazione.ANNULLATA).ToString() + ", [DettaglioConferma]='Studio di fattibilità non fattibile' WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_DELETED).ToString() + " AND [StatoRichiesta]<=" + ((int)StatoRichiestaApprovazione.PRESAINCARICO).ToString() + " AND [IDOggettoApprovabile]=" + DBUtils.GetID(cons) + " AND [TipoOggettoApprovabile]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(cons)));
            }

            // Dim w As FinestraLavorazione = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(cons.IDCliente)
            // If w Is Nothing Then w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(cons.IDCliente)
            // If (w IsNot Nothing) Then
            // w.NotificaProposta(cons)
            // End If

            // 'Dim cursor As New ClientiLavoratiStatsItemCursor
            // 'cursor.IDCliente.Value = cons.IDCliente
            // 'cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
            // 'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // 'Dim statsItem As ClientiLavoratiStatsItem = cursor.Item
            // 'cursor.Dispose()
            // 'If (statsItem Is Nothing) Then
            // '    statsItem = New ClientiLavoratiStatsItem
            // '    statsItem.Cliente = cons.Cliente
            // '    statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '    statsItem.DataInizioLavorazione = cons.DataConsulenza
            // '    statsItem.DataUltimoAggiornamento = cons.DataConsulenza
            // 'Else
            // '    'Select Case statsItem.SottostatoLavorazione
            // '    '    Case SottostatoLavorazione.None, SottostatoLavorazione.InLavorazione, SottostatoLavorazione.InAttesa
            // '    '    Case Else
            // '    '        If (Maths.Abs(Calendar.DateDiff(DateTimeInterval.Month, statsItem.DataUltimoAggiornamento.Value, Data)) >= 3) Then
            // '    '            statsItem = New ClientiLavoratiStatsItem
            // '    '            statsItem.Cliente = rich.Cliente
            // '    '            statsItem.Stato = ObjectStatus.OBJECT_VALID
            // '    '            statsItem.DataInizioLavorazione = Data
            // '    '        End If
            // '    'End Select
            // 'End If
            // 'statsItem.NotifyConsulenza(cons)
            // 'statsItem.Save()
        }

        private static void HandlePratica(object sender, ItemEventArgs e)
        {
            CPraticaCQSPD pratica = (CPraticaCQSPD)e.Item;
            var conn = Database;
            if (conn.IsRemote())
            {
            }
            else if (pratica.Stato == ObjectStatus.OBJECT_DELETED)
            {
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" + ((int)StatoRichiestaApprovazione.ANNULLATA).ToString() + ", [DettaglioConferma]='La pratica è stata eliminata' WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_DELETED).ToString() + " AND [StatoRichiesta]<=" + ((int)StatoRichiestaApprovazione.PRESAINCARICO).ToString() + " AND [IDOggettoApprovabile]=" + DBUtils.GetID(pratica) + " AND [TipoOggettoApprovabile]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(pratica)));
                conn.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] SET [IDCQS]=0, [StatoCQS]=0, [DataCQS]=Null WHERE [IDCQS]=" + DBUtils.GetID(pratica));
                conn.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] SET [IDPD]=0, [StatoPD]=0, [DataPD]=Null WHERE [IDPD]=" + DBUtils.GetID(pratica));
                conn.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] SET [IDCQSI]=0, [StatoCQSI]=0, [DataCQSI]=Null WHERE [IDCQSI]=" + DBUtils.GetID(pratica));
                conn.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] SET [IDPDI]=0, [StatoPDI]=0, [DataPDI]=Null WHERE [IDPDI]=" + DBUtils.GetID(pratica));
            }
            else if (
                   (pratica.StatoAttuale is object)
                && (pratica.StatoAttuale.MacroStato.HasValue && pratica.StatoAttuale.MacroStato.Value == StatoPraticaEnum.STATO_ANNULLATA) 
                   )
            {
                conn.ExecuteCommand("UPDATE [tbl_CQSPDRichiesteApprovazione] SET [StatoRichiesta]=" + ((int)StatoRichiestaApprovazione.ANNULLATA).ToString() + ", [DettaglioConferma]='La pratica è stata annullata' WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_DELETED).ToString() + " AND [StatoRichiesta]<=" + ((int)StatoRichiestaApprovazione.PRESAINCARICO).ToString() + " AND [IDOggettoApprovabile]=" + DBUtils.GetID(pratica) + " AND [TipoOggettoApprovabile]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(pratica)));
            }
        }

        /// <summary>
    /// Gestisce l'evento "PersonaMerged" del modulo anagrafica in modo da mantenere consistenti i dati nelle tabelle
    /// </summary>
    /// <param name="e"></param>
    /// <remarks></remarks>
        private static void HandlePersonaMerged(Anagrafica.MergePersonaEventArgs e)
        {
            lock (Anagrafica.@lock)
            {
                var mi = e.MI;
                var persona = mi.Persona1;
                var persona1 = mi.Persona2;
                string dbSQL;
                IDataReader dbRis;
                Anagrafica.CMergePersonaRecord rec;

                // Tabella tbl_CQSPDGrpRichApp 
                dbSQL = "select [ID] FROM [tbl_CQSPDGrpRichApp] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDGrpRichApp";
                    rec.FieldName = "IDCliente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpRichApp] Set [IDCliente]=" + DBUtils.GetID(persona) + ", [NomeCliente]=" + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_AreaManagers 
                dbSQL = "Select [ID] FROM [tbl_AreaManagers] WHERE [Persona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_AreaManagers";
                    rec.FieldName = "Persona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_AreaManagers] Set [Persona]=" + DBUtils.GetID(persona) + " WHERE [Persona]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDNoteCliXCollab 
                dbSQL = "Select [ID] FROM [tbl_CQSPDNoteCliXCollab] WHERE [IDPersona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDNoteCliXCollab";
                    rec.FieldName = "IDPersona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDNoteCliXCollab] Set [IDPersona]=" + DBUtils.GetID(persona) + " WHERE [IDPersona]=" + DBUtils.GetID(persona1));


                // Tabella tbl_Collaboratori 
                dbSQL = "Select [ID] FROM [tbl_Collaboratori] WHERE [Persona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_Collaboratori";
                    rec.FieldName = "Persona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_Collaboratori] Set [Persona]=" + DBUtils.GetID(persona) + ", NomePersona = " + DBUtils.DBString(persona.Nominativo) + " WHERE [Persona]=" + DBUtils.GetID(persona1));

                // Tabella tbl_EstinzioniXPersona 
                dbSQL = "Select [ID] FROM [tbl_EstinzioniXPersona] WHERE [Persona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_EstinzioniXPersona";
                    rec.FieldName = "Persona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_EstinzioniXPersona] Set [Persona]=" + DBUtils.GetID(persona) + " WHERE [Persona]=" + DBUtils.GetID(persona1));

                // Tabella tbl_TeamManagers 
                dbSQL = "Select [ID] FROM [tbl_TeamManagers] WHERE [Persona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_TeamManagers";
                    rec.FieldName = "Persona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_TeamManagers] Set [Persona]=" + DBUtils.GetID(persona) + " WHERE [Persona]=" + DBUtils.GetID(persona1));
                if (persona is Anagrafica.CPersonaFisica)
                {
                    {
                        var withBlock = (Anagrafica.CPersonaFisica)persona;
                        // Tabella tbl_Pratiche 
                        dbSQL = "Select [ID] FROM [tbl_Pratiche] WHERE [Cliente]=" + DBUtils.GetID(persona1);
                        dbRis = Database.ExecuteReader(dbSQL);
                        while (dbRis.Read())
                        {
                            rec = new Anagrafica.CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Pratiche";
                            rec.FieldName = "Cliente";
                            rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                            mi.TabelleModificate.Add(rec);
                        }

                        dbRis.Dispose();
                        Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [Cliente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(withBlock.Nome) + ", [CognomeCliente] = " + DBUtils.DBString(withBlock.Cognome) + " WHERE [Cliente]=" + DBUtils.GetID(persona1));
                    }

                    // Tabella tbl_CQSPDRichCERQ 
                    dbSQL = "Select [ID] FROM [tbl_CQSPDRichCERQ] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_CQSPDRichCERQ";
                        rec.FieldName = "IDCliente";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDCliente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));
                }
                else
                {

                    // tbl_CQSPDValutazioniAzienda
                    dbSQL = "Select [ID] FROM [tbl_CQSPDValutazioniAzienda] WHERE [IDAzienda]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_CQSPDValutazioniAzienda";
                        rec.FieldName = "IDAzienda";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDValutazioniAzienda] Set [IDAzienda]=" + DBUtils.GetID(persona) + ", [NomeAzienda] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDAzienda]=" + DBUtils.GetID(persona1));


                    // Tabella tbl_Pratiche 
                    dbSQL = "Select [ID] FROM [tbl_Pratiche] WHERE [IDAmministrazione]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_Pratiche";
                        rec.FieldName = "IDAmministrazione";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [IDAmministrazione]=" + DBUtils.GetID(persona) + ", [Ente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDAmministrazione]=" + DBUtils.GetID(persona1));

                    // Tabella tbl_Pratiche 
                    dbSQL = "Select [ID] FROM [tbl_Pratiche] WHERE [IDEntePagante]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_Pratiche";
                        rec.FieldName = "IDEntePagante";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [IDEntePagante]=" + DBUtils.GetID(persona) + ", [NomeEntePagante] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDEntePagante]=" + DBUtils.GetID(persona1));

                    // Tabella tbl_Pratiche 
                    dbSQL = "Select [ID] FROM [tbl_CQSPDRichCERQ] WHERE [IDAmministrazione]=" + DBUtils.GetID(persona1);
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        rec = new Anagrafica.CMergePersonaRecord();
                        rec.NomeTabella = "tbl_CQSPDRichCERQ";
                        rec.FieldName = "IDAmministrazione";
                        rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                        mi.TabelleModificate.Add(rec);
                    }

                    dbRis.Dispose();
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDAmministrazione]=" + DBUtils.GetID(persona) + ", [NomeAmministrazione] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDAmministrazione]=" + DBUtils.GetID(persona1));
                }

                // Tabella tbl_RichiesteFinanziamenti 
                dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamenti] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_RichiesteFinanziamenti";
                    rec.FieldName = "IDCliente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamenti] Set [IDCliente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_Preventivi_Offerte 
                dbSQL = "Select [ID] FROM [tbl_Preventivi_Offerte] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_Preventivi_Offerte";
                    rec.FieldName = "IDCliente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_Preventivi_Offerte] Set [IDCliente]=" + DBUtils.GetID(persona) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_Estinzioni 
                dbSQL = "Select [ID] FROM [tbl_Estinzioni] WHERE [IDPersona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_Estinzioni";
                    rec.FieldName = "IDPersona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_Estinzioni] Set [IDPersona]=" + DBUtils.GetID(persona) + " WHERE [IDPersona]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDConsulenze 
                dbSQL = "Select [ID] FROM [tbl_CQSPDConsulenze] WHERE [IDCLiente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDConsulenze";
                    rec.FieldName = "IDCLiente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDConsulenze] Set [IDCLiente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDGrpConsulenze 
                dbSQL = "Select [ID] FROM [tbl_CQSPDGrpConsulenze] WHERE [IDCLiente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDGrpConsulenze";
                    rec.FieldName = "IDCLiente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpConsulenze] Set [IDCLiente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_RichiesteFinanziamentiC 
                dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_RichiesteFinanziamentiC";
                    rec.FieldName = "IDCliente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDCliente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_RichiesteFinanziamentiC 
                dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDIstituto]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_RichiesteFinanziamentiC";
                    rec.FieldName = "IDIstituto";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDIstituto]=" + DBUtils.GetID(persona) + ", [NomeIstituto] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDIstituto]=" + DBUtils.GetID(persona1));

                // Tabella tbl_RichiesteFinanziamentiC 
                dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDAgenziaR]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_RichiesteFinanziamentiC";
                    rec.FieldName = "IDAgenziaR";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgenziaR]=" + DBUtils.GetID(persona) + ", [NomeAgenziaR] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDAgenziaR]=" + DBUtils.GetID(persona1));

                // Tabella tbl_RichiesteFinanziamentiC 
                dbSQL = "Select [ID] FROM [tbl_RichiesteFinanziamentiC] WHERE [IDAgente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_RichiesteFinanziamentiC";
                    rec.FieldName = "IDAgente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgente]=" + DBUtils.GetID(persona) + ", [NomeAgente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDAgente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDClientiLavorati 
                dbSQL = "Select [ID] FROM [tbl_CQSPDClientiLavorati] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDClientiLavorati";
                    rec.FieldName = "IDCliente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDClientiLavorati] Set [IDCliente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDImportExport 
                dbSQL = "Select [ID] FROM [tbl_CQSPDImportExport] WHERE [Esportazione]=True And [IDPersonaEsportata]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDImportExport";
                    rec.FieldName = "IDPersonaEsportata";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaEsportata]=" + DBUtils.GetID(persona) + ", [NomePersonaEsportata] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [Esportazione]=True And [IDPersonaEsportata]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDImportExport 
                dbSQL = "Select [ID] FROM [tbl_CQSPDImportExport] WHERE [Esportazione]=False And [IDPersonaImportata]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDImportExport";
                    rec.FieldName = "IDPersonaImportata";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaImportata]=" + DBUtils.GetID(persona) + ", [NomePersonaImportata] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [Esportazione]=False And [IDPersonaImportata]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDFinestreLavorazione 
                dbSQL = "Select [ID] FROM [tbl_CQSPDFinestreLavorazione] WHERE [IDCliente]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDFinestreLavorazione";
                    rec.FieldName = "IDCliente";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] Set [IDCliente]=" + DBUtils.GetID(persona) + ", [NomeCliente] = " + DBUtils.DBString(persona.Nominativo) + " WHERE [IDCliente]=" + DBUtils.GetID(persona1));

                // Tabella tbl_CQSPDCliXCollab 
                dbSQL = "Select [ID] FROM [tbl_CQSPDCliXCollab] WHERE [IDPersona]=" + DBUtils.GetID(persona1);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    rec = new Anagrafica.CMergePersonaRecord();
                    rec.NomeTabella = "tbl_CQSPDCliXCollab";
                    rec.FieldName = "IDPersona";
                    rec.RecordID = Sistema.Formats.ToInteger(dbRis["ID"]);
                    mi.TabelleModificate.Add(rec);
                }

                dbRis.Dispose();
                Database.ExecuteCommand("UPDATE [tbl_CQSPDCliXCollab] Set [IDPersona]=" + DBUtils.GetID(persona) + " WHERE [IDPersona]=" + DBUtils.GetID(persona1));
                if (e.Persona is Anagrafica.CPersonaFisica)
                {
                    var ra1 = RichiesteApprovazioneGroups.GetRichiestaByPersona((Anagrafica.CPersonaFisica)persona);
                    var ra2 = RichiesteApprovazioneGroups.GetRichiestaByPersona((Anagrafica.CPersonaFisica)persona1);
                    if (ra1 is object && ra2 is object)
                    {
                        if (ra1.DataRichiesta < ra2.DataRichiesta)
                        {
                            ra1.DettaglioRichiesta = DMD.Strings.Combine(ra1.DettaglioRichiesta, ra2.DettaglioRichiesta, "<br/>");
                            foreach (CRichiestaApprovazione ra in ra2.Richieste)
                            {
                                ra1.Richieste.Add(ra);
                                ra.Save(true);
                            }

                            ra1.Save();
                            ra2.Delete();
                        }
                        else
                        {
                            ra2.DettaglioRichiesta = DMD.Strings.Combine(ra2.DettaglioRichiesta, ra1.DettaglioRichiesta, "<br/>");
                            foreach (CRichiestaApprovazione ra in ra1.Richieste)
                            {
                                ra2.Richieste.Add(ra);
                                ra.Save(true);
                            }

                            ra2.Save();
                            ra1.Delete();
                        }
                    }

                    var w1 = FinestreDiLavorazione.GetFinestraCorrente((Anagrafica.CPersonaFisica)persona);
                    var w2 = FinestreDiLavorazione.GetFinestraCorrente((Anagrafica.CPersonaFisica)persona1);
                    if (w1 is null)
                    {
                        if (w2 is object)
                        {
                            w1 = w2;
                        }
                    }
                    else if (w2 is object)
                    {
                        w1.MergeWith(w2);
                    }

                    if (w1 is null)
                    {
                        w1 = FinestreDiLavorazione.GetProssimaFinestra((Anagrafica.CPersonaFisica)e.Persona);
                    }
                    else
                    {
                        w1.Cliente = (Anagrafica.CPersonaFisica)e.Persona;
                        w1.Ricalcola = true;
                        w1.Save();
                    }

                    if (w2 is object && !ReferenceEquals(w2, w1))
                    {
                        w2.Delete();
                    }

                    var finestre = FinestreDiLavorazione.GetFinestreByPersona(persona);
                    bool foundAttiva = false;
                    finestre.Sort();
                    int i;
                    var loopTo = finestre.Count - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        w1 = finestre[i];
                        if (w1.StatoFinestra == StatoFinestraLavorazione.Aperta)
                        {
                            if (foundAttiva)
                            {
                                w1.StatoFinestra = StatoFinestraLavorazione.Chiusa;
                                w1.Ricalcola = true;
                                w1.Save();
                            }

                            foundAttiva = true;
                        }
                    }

                    i = 0;
                    while (i < finestre.Count)
                    {
                        w1 = finestre[i];
                        if (w1.StatoFinestra == StatoFinestraLavorazione.NonAperta)
                        {
                            int j = i + 1;
                            while (j < finestre.Count)
                            {
                                w2 = finestre[j];
                                if (w2.StatoFinestra == StatoFinestraLavorazione.NonAperta)
                                {
                                    w1.MergeWith(w2);
                                    w1.Ricalcola = true;
                                    w1.Save();
                                    w2.Delete();
                                }

                                j += 1;
                            }

                            break;
                        }

                        i += 1;
                    }
                }
            }
        }

        private static void HandlePeronaUnMerged(Anagrafica.MergePersonaEventArgs e)
        {
            lock (Anagrafica.@lock)
            {
                var mi = e.MI;
                var persona = mi.Persona1;
                var persona1 = mi.Persona2;
                string items;

                // tbl_CQSPDGrpRichApp
                items = mi.GetAffectedRecors("tbl_CQSPDGrpRichApp", "IDCliente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpRichApp] Set [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_AreaManagers
                items = mi.GetAffectedRecors("tbl_AreaManagers", "Persona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_AreaManagers] Set [Persona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDNoteCliXCollab
                items = mi.GetAffectedRecors("tbl_CQSPDNoteCliXCollab", "IDPersona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDNoteCliXCollab] Set [IDPersona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_Collaboratori 
                items = mi.GetAffectedRecors("tbl_Collaboratori", "Persona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_Collaboratori] Set [Persona]= " + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_EstinzioniXPersona 
                items = mi.GetAffectedRecors("tbl_EstinzioniXPersona", "Persona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_EstinzioniXPersona] Set [Persona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_TeamManagers 
                items = mi.GetAffectedRecors("tbl_TeamManagers", "Persona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_TeamManagers] Set [Persona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");
                if (persona is Anagrafica.CPersonaFisica)
                {
                    {
                        var withBlock = (Anagrafica.CPersonaFisica)persona;
                        // Tabella tbl_Pratiche 
                        items = mi.GetAffectedRecors("tbl_Pratiche", "Cliente");
                        if (!string.IsNullOrEmpty(items))
                            Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [Cliente]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");
                    }

                    // Tabella tbl_CQSPDRichCERQ 
                    items = mi.GetAffectedRecors("tbl_CQSPDRichCERQ", "IDCliente");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }
                else
                {
                    // tbl_CQSPDValutazioniAzienda
                    items = mi.GetAffectedRecors("tbl_CQSPDValutazioniAzienda", "IDAzienda");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_CQSPDValutazioniAzienda] Set [IDAzienda]=" + DBUtils.GetID(persona1) + ", [NomeAzienda]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                    // Tabella tbl_Pratiche 
                    items = mi.GetAffectedRecors("tbl_Pratiche", "IDAmministrazione");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [IDAmministrazione]=" + DBUtils.GetID(persona1) + ", [Ente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                    // Tabella tbl_Pratiche 
                    items = mi.GetAffectedRecors("tbl_Pratiche", "IDEntePagante");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [IDEntePagante]=" + DBUtils.GetID(persona1) + ", [NomeEntePagante]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                    // Tabella tbl_CQSPDRichCERQ           
                    items = mi.GetAffectedRecors("tbl_CQSPDRichCERQ", "IDAmministrazione");
                    if (!string.IsNullOrEmpty(items))
                        Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] Set [IDAmministrazione]=" + DBUtils.GetID(persona1) + ", [NomeAmministrazione]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                }

                // Tabella tbl_RichiesteFinanziamenti 
                items = mi.GetAffectedRecors("tbl_RichiesteFinanziamenti", "IDCliente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamenti] Set [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_Preventivi_Offerte 
                items = mi.GetAffectedRecors("tbl_Preventivi_Offerte", "IDCliente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_Preventivi_Offerte] Set [IDCliente]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_Estinzioni 
                items = mi.GetAffectedRecors("tbl_Estinzioni", "IDPersona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_Estinzioni] Set [IDPersona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDConsulenze 
                items = mi.GetAffectedRecors("tbl_CQSPDConsulenze", "IDCLiente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDConsulenze] Set [IDCLiente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDGrpConsulenze 
                items = mi.GetAffectedRecors("tbl_CQSPDGrpConsulenze", "IDCLiente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpConsulenze] Set [IDCLiente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_RichiesteFinanziamentiC 
                items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDCliente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_RichiesteFinanziamentiC 
                items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDIstituto");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDIstituto]=" + DBUtils.GetID(persona1) + ", [NomeIstituto]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_RichiesteFinanziamentiC 
                items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDAgenziaR");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgenziaR]=" + DBUtils.GetID(persona1) + ", [NomeAgenziaR]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_RichiesteFinanziamentiC 
                items = mi.GetAffectedRecors("tbl_RichiesteFinanziamentiC", "IDAgente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_RichiesteFinanziamentiC] Set [IDAgente]=" + DBUtils.GetID(persona1) + ", [NomeAgente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDClientiLavorati 
                items = mi.GetAffectedRecors("tbl_CQSPDClientiLavorati", "IDCliente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDClientiLavorati] Set [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDImportExport 
                items = mi.GetAffectedRecors("tbl_CQSPDImportExport", "IDPersonaEsportata");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaEsportata]=" + DBUtils.GetID(persona1) + ", [NomePersonaEsportata]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDImportExport 
                items = mi.GetAffectedRecors("tbl_CQSPDImportExport", "IDPersonaImportata");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDImportExport] Set [IDPersonaImportata]=" + DBUtils.GetID(persona1) + ", [NomePersonaImportata]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDFinestreLavorazione 
                items = mi.GetAffectedRecors("tbl_CQSPDFinestreLavorazione", "IDCliente");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDFinestreLavorazione] Set [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");

                // Tabella tbl_CQSPDFinestreLavorazione 
                items = mi.GetAffectedRecors("tbl_CQSPDCliXCollab", "IDPersona");
                if (!string.IsNullOrEmpty(items))
                    Database.ExecuteCommand("UPDATE [tbl_CQSPDCliXCollab] Set [IDPersona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");
            }
        }

        private static void HandlePersonaModified(Anagrafica.PersonaEventArgs e)
        {
            lock (Anagrafica.@lock)
            {
                Database.ExecuteCommand("UPDATE [tbl_Pratiche] Set [TipoFonteCliente]=" + DBUtils.DBString(e.Persona.TipoFonte) + ", [IDFonteCliente]=" + DBUtils.DBNumber(e.Persona.IDFonte) + " WHERE [Cliente]=" + DBUtils.GetID(e.Persona));
                Database.ExecuteCommand("UPDATE [tbl_CQSPDClientiLavorati] Set [IconaCliente]=" + DBUtils.DBString(e.Persona.IconURL) + ", [NomeCliente]=" + DBUtils.DBString(e.Persona.Nominativo) + ", [IDPuntoOperativo]=" + e.Persona.IDPuntoOperativo + ", [NomePuntoOperativo]=" + DBUtils.DBString(e.Persona.NomePuntoOperativo) + " WHERE [IDCliente]=" + DBUtils.GetID(e.Persona));
                Database.ExecuteCommand("UPDATE [tbl_Estinzioni] Set [IDPuntoOperativo]=" + DBUtils.DBNumber(e.Persona.IDPuntoOperativo) + ", [NomePuntoOperativo]=" + DBUtils.DBString(e.Persona.NomePuntoOperativo) + " WHERE [IDPersona]=" + DBUtils.GetID(e.Persona));
                if (e.Persona is Anagrafica.CPersonaFisica)
                {
                    var wl = FinestreDiLavorazione.GetFinestraCorrente((Anagrafica.CPersonaFisica)e.Persona);
                    if (wl is object)
                    {
                        wl.Ricalcola = true;
                        wl.Cliente = (Anagrafica.CPersonaFisica)e.Persona;
                        wl.NomeCliente = e.Persona.Nominativo;
                        wl.IconaCliente = e.Persona.IconURL;
                        wl.Stato = ObjectStatus.OBJECT_VALID;
                        // w.DataInizioLavorabilita = Me.CalcolaDataLavorabilita(persona)
                        // w.DataUltimoAggiornamento = Calendar.Now
                        wl.SetFlag(FinestraLavorazioneFlags.Disponibile_CQS, FinestreDiLavorazione.HaCQS((Anagrafica.CPersonaFisica)e.Persona, wl.DataInizioLavorabilita));
                        wl.SetFlag(FinestraLavorazioneFlags.Disponibile_PD, FinestreDiLavorazione.HaPD((Anagrafica.CPersonaFisica)e.Persona, wl.DataInizioLavorabilita));
                        wl.Save();
                    }
                    else
                    {
                        wl = FinestreDiLavorazione.GetProssimaFinestra((Anagrafica.CPersonaFisica)e.Persona);
                    }
                }
            }
        }

        private static void HandlePersonaCreated(Anagrafica.PersonaEventArgs e)
        {
            lock (Anagrafica.@lock)
            {
                if (e.Persona is Anagrafica.CPersonaFisica)
                {
                    var col = Collaboratori.GetItemByUser(Sistema.Users.CurrentUser);
                    if (col is object && col.IsValid())
                    {
                        var cli = Collaboratori.ClientiXCollaboratori.GetItemByPersonaECollaboratore((Anagrafica.CPersonaFisica)e.Persona, col);
                        if (cli is null)
                        {
                            cli = new ClienteXCollaboratore();
                            cli.Persona = (Anagrafica.CPersonaFisica)e.Persona;
                            cli.Collaboratore = col;
                            cli.Stato = ObjectStatus.OBJECT_VALID;
                            cli.StatoLavorazione = StatoClienteCollaboratore.CONTATTO;
                            cli.DataAcquisizione = DMD.DateUtils.Now();
                            cli.MotivoAssegnazione = "Creato dal collaboratore";
                        }

                        cli.FromPersona();
                        cli.Save(true);
                    }

                    var wl = FinestreDiLavorazione.GetProssimaFinestra((Anagrafica.CPersonaFisica)e.Persona);
                    wl.Stato = ObjectStatus.OBJECT_VALID;
                    wl.Save();
                }
            }
        }

        public static Sistema.CModule Module
        {
            get
            {
                if (m_Module is null)
                    m_Module = Sistema.Modules.GetItemByName("modCQSPD");
                return m_Module;
            }
        }

        /// <summary>
    /// Gruppo dei consulenti (i membri vengono aggiunti e rimossi automaticamente in funzione della definizione dei consulenti)
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static Sistema.CGroup GruppoConsulenti
        {
            get
            {
                m_GruppoConsulenti = Sistema.Groups.GetItemByName("Finanziaria Consulenti");
                if (m_GruppoConsulenti is null)
                {
                    m_GruppoConsulenti = new Sistema.CGroup("Finanziaria Consulenti");
                    m_GruppoConsulenti.Notes = "Gruppo di operatori abilitati alle consulenze per le pratiche";
                    m_GruppoConsulenti.Stato = ObjectStatus.OBJECT_VALID;
                    m_GruppoConsulenti.Save();
                }

                return m_GruppoConsulenti;
            }
        }

        /// <summary>
    /// Rappresenta il gruppo degli operatori abilitati alla visualizzazione ed alla lavorazione delle pratiche
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static Sistema.CGroup GruppoOperatori
        {
            get
            {
                if (m_GruppoOperatori is null)
                    m_GruppoOperatori = Sistema.Groups.GetItemByName("Finanziaria");
                if (m_GruppoOperatori is null)
                {
                    m_GruppoOperatori = new Sistema.CGroup("Finanziaria");
                    m_GruppoOperatori.Stato = ObjectStatus.OBJECT_VALID;
                    m_GruppoOperatori.Save();
                }

                return m_GruppoOperatori;
            }
        }

        /// <summary>
    /// Rappresenta il gruppo dei responsabili di ufficio che hanno poteri su tutte le pratiche dell'ufficio
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static Sistema.CGroup GruppoReferenti
        {
            get
            {
                if (m_GruppoReferenti is null)
                    m_GruppoReferenti = Sistema.Groups.GetItemByName("Finanziaria Referenti");
                if (m_GruppoReferenti is null)
                {
                    m_GruppoReferenti = new Sistema.CGroup("Finanziaria Referenti");
                    m_GruppoReferenti.Stato = ObjectStatus.OBJECT_VALID;
                    m_GruppoReferenti.Save();
                }

                return m_GruppoReferenti;
            }
        }

        /// <summary>
    /// Rappresenta il gruppo dei supervisori.
    /// Questo gruppo ha potere globale su tutte le pratiche e può autorizzare gli sconti (se con motivo non privilegiato)
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static Sistema.CGroup GruppoSupervisori
        {
            get
            {
                if (m_GruppoSupervisori is null)
                    m_GruppoSupervisori = Sistema.Groups.GetItemByName("Finanziaria Supervisori");
                if (m_GruppoSupervisori is null)
                {
                    m_GruppoSupervisori = new Sistema.CGroup("Finanziaria Supervisori");
                    m_GruppoSupervisori.Stato = ObjectStatus.OBJECT_VALID;
                    m_GruppoSupervisori.Save();
                }

                return m_GruppoSupervisori;
            }
        }

        /// <summary>
    /// Questo gruppo ha potere su tutte le pratiche e può autorizzare tutti gli sconti
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static Sistema.CGroup GruppoAutorizzatori
        {
            get
            {
                if (m_GruppoAutorizzatori is null)
                    m_GruppoAutorizzatori = Sistema.Groups.GetItemByName("Finanziaria Autorizzatori");
                if (m_GruppoAutorizzatori is null)
                {
                    m_GruppoAutorizzatori = new Sistema.CGroup("Finanziaria Autorizzatori");
                    m_GruppoAutorizzatori.Stato = ObjectStatus.OBJECT_VALID;
                    m_GruppoAutorizzatori.Save();
                }

                return m_GruppoAutorizzatori;
            }
        }

        private static CCollection<Sistema.ICalendarProvider> m_Providers;

        public static CCollection<Sistema.ICalendarProvider> Providers
        {
            get
            {
                if (m_Providers is null)
                    m_Providers = new CCollection<Sistema.ICalendarProvider>();
                return new CCollection<Sistema.ICalendarProvider>(m_Providers);
            }
        }

        public static CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            var ret = new CCollection<Sistema.ICalendarActivity>();
            var providers = Providers;
            foreach (Sistema.ICalendarProvider p in providers)
                ret.AddRange(p.GetToDoList(user));
            ret.Sort();
            return ret;
        }

        public static Sistema.ICalendarProvider GetProviderByName(string name)
        {
            var providers = Providers;
            name = DMD.Strings.Trim(name);
            if (string.IsNullOrEmpty(name))
                return null;
            foreach (Sistema.ICalendarProvider p in providers)
            {
                if ((p.UniqueName ?? "") == (name ?? ""))
                    return p;
            }

            return null;
        }

        public static void AddProvider(Sistema.ICalendarProvider p)
        {
            if (GetProviderByName(p.UniqueName) is object)
                throw new ArgumentException("Provider già registrato");
            m_Providers.Add(p);
        }

        public static void RemoveProvider(Sistema.ICalendarProvider p)
        {
            p = GetProviderByName(p.UniqueName);
            if (p is null)
                throw new KeyNotFoundException("Provider non trovato");
            m_Providers.Remove(p);
        }

        ~Finanziaria()
        {
            DMDObject.DecreaseCounter(this);
        }

        public class CTMProduttoriCollection : CCollaboratoreCollection
        {
            private CTeamManager m_TeamManager;

            public CTMProduttoriCollection()
            {
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_TeamManager is object)
                    ((CCollaboratore)value).Referente = m_TeamManager;
                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CTeamManager value)
            {
                Clear();
                m_TeamManager = value;
                if (DBUtils.GetID(value) != 0)
                {
                    var cursor = new CCollaboratoriCursor();
                    cursor.ReferenteID.Value = DBUtils.GetID(value);
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                }

                return true;
            }
        }

        public class CTMPraticheCollection : CPraticheCollection
        {
            private CTeamManager m_TeamManager;

            public CTMPraticheCollection()
            {
                m_TeamManager = null;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_TeamManager is object)
                {
                    var info = ((CPraticaCQSPD)value).Info;
                    info.Commerciale = m_TeamManager;
                }

                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CTeamManager value)
            {
                Clear();
                m_TeamManager = value;
                if (DBUtils.GetID(value) != 0)
                {
                    var cursor = new CPraticheCQSPDCursor();
                    cursor.IDCommerciale.Value = DBUtils.GetID(value);
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                }

                return true;
            }
        }

        public class CAMTeamManagersCollection : CTeamManagersCollection
        {
            private CAreaManager m_AreaManager;

            public CAMTeamManagersCollection()
            {
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_AreaManager is object)
                    ((CTeamManager)value).Referente = m_AreaManager;
                base.OnInsert(index, value);
            }

            protected internal bool Initialize(CAreaManager value)
            {
                Clear();
                m_AreaManager = value;
                if (DBUtils.GetID(value) != 0)
                {
                    var cursor = new CTeamManagersCursor();
                    cursor.IDReferente.Value = DBUtils.GetID(value);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                    Sort();
                }

                return true;
            }
        }

        public class CTeamManagersCollection : CCollection<CTeamManager>, IComparer
        {
            public CTeamManagersCollection()
            {
            }

            public int ContaValidi()
            {
                int cnt = 0;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var tm = this[i];
                    switch (tm.StatoTeamManager)
                    {
                        case StatoTeamManager.STATO_ATTIVO:
                        case StatoTeamManager.STATO_INATTIVAZIONE:
                        case StatoTeamManager.STATO_SOSPESO:
                            {
                                cnt = cnt + 1;
                                break;
                            }
                    }
                }

                return cnt;
            }

            public int ContaAttivi()
            {
                int cnt = 0;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var tm = this[i];
                    if (tm.StatoTeamManager == StatoTeamManager.STATO_ATTIVO)
                        cnt = cnt + 1;
                }

                return cnt;
            }

            public int ContaInAttivazione()
            {
                int cnt = 0;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var tm = this[i];
                    if (tm.StatoTeamManager == StatoTeamManager.STATO_INATTIVAZIONE)
                        cnt = cnt + 1;
                }

                return cnt;
            }

            public int Compare1(CTeamManager x, CTeamManager y)
            {
                return DMD.Strings.Compare(x.Nominativo, y.Nominativo, true);
            }
        }

        public class CFsbPrev_StampaOfferta : Databases.DBObject
        {
            private int m_OffertaID;
            private COffertaCQS m_Offerta;
            private DateTime m_DataStampa;
            private string m_OutputFile;

            public CFsbPrev_StampaOfferta()
            {
                m_OffertaID = 0;
                m_Offerta = null;
                m_DataStampa = default;
                m_OutputFile = "";
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public DateTime DataStampa
            {
                get
                {
                    return m_DataStampa;
                }

                set
                {
                    var oldValue = m_DataStampa;
                    if (oldValue == value)
                        return;
                    m_DataStampa = value;
                    DoChanged("DataStampa", value, oldValue);
                }
            }

            public string OutputFile
            {
                get
                {
                    return m_OutputFile;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_OutputFile;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_OutputFile = value;
                    DoChanged("OutputFile", value, oldValue);
                }
            }

            public int OffertaID
            {
                get
                {
                    return DBUtils.GetID(m_Offerta, m_OffertaID);
                }

                set
                {
                    int oldValue = OffertaID;
                    if (oldValue == value)
                        return;
                    m_Offerta = null;
                    m_OffertaID = value;
                    DoChanged("OffertaID", value, oldValue);
                }
            }

            public COffertaCQS Offerta
            {
                get
                {
                    if (m_Offerta is null)
                        m_Offerta = Offerte.GetItemById(m_OffertaID);
                    return m_Offerta;
                }

                set
                {
                    var oldValue = Offerta;
                    if (oldValue == value)
                        return;
                    m_Offerta = value;
                    m_OffertaID = DBUtils.GetID(value);
                    DoChanged("Offerta", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_Preventivi_Stampe";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_OffertaID = reader.Read("Offerta", m_OffertaID);
                m_DataStampa = reader.Read("DataStampa", m_DataStampa);
                m_OutputFile = reader.Read("OutputFile", m_OutputFile);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Offerta", DBUtils.GetID(m_Offerta, m_OffertaID));
                writer.Write("DataStampa", m_DataStampa);
                writer.Write("OutputFile", m_OutputFile);
                return base.SaveToRecordset(writer);
            }
        }



        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
    /// Riga della tabella dei coefficienti assicurativi
    /// </summary>
    /// <remarks></remarks>
        public class CCoefficienteAssicurativo : Databases.DBObjectBase
        {
            private int m_IDTabella;
            private CTabellaAssicurativa m_Tabella;
            private string m_Sesso;
            private int m_Anni;
            private double?[] m_C;

            public CCoefficienteAssicurativo()
            {
                m_IDTabella = 0;
                m_Tabella = null;
                m_Sesso = "";
                m_Anni = 0;
                m_C = new double?[11];
            }

            public override CModulesClass GetModule()
            {
                return TabelleAssicurative.Module;
            }

            public int IDTabella
            {
                get
                {
                    return DBUtils.GetID(m_Tabella, m_IDTabella);
                }

                set
                {
                    int oldValue = IDTabella;
                    if (value == oldValue)
                        return;
                    m_IDTabella = value;
                    m_Tabella = null;
                    DoChanged("IDTabella", value, oldValue);
                }
            }

            public CTabellaAssicurativa Tabella
            {
                get
                {
                    if (m_Tabella is null)
                        m_Tabella = TabelleAssicurative.GetItemById(m_IDTabella);
                    return m_Tabella;
                }

                set
                {
                    var oldValue = Tabella;
                    if (oldValue == value)
                        return;
                    m_Tabella = value;
                    m_IDTabella = DBUtils.GetID(value);
                    DoChanged("Tabella", value, oldValue);
                }
            }

            public string Sesso
            {
                get
                {
                    return m_Sesso;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_Sesso;
                    m_Sesso = value;
                    DoChanged("Sesso", value, oldValue);
                }
            }

            public int Anni
            {
                get
                {
                    return m_Anni;
                }

                set
                {
                    int oldValue = m_Anni;
                    if (oldValue == value)
                        return;
                    m_Anni = value;
                    DoChanged("Anni", value, oldValue);
                }
            }

            public double? get_Coefficiente(int durata)
            {
                return m_C[(int)Maths.Floor(durata / 12d)];
            }

            public void set_Coefficiente(int durata, double? value)
            {
                var oldValue = m_C[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value == true)
                    return;
                m_C[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("Coefficiente", value, oldValue);
            }

            public override string GetTableName()
            {
                return "tbl_CoefficientiAssicurativi";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDTabella = reader.Read("Tabella", this.m_IDTabella);
                this.m_Sesso = reader.Read("Sesso", this.m_Sesso);
                this.m_Anni = reader.Read("Anni", this.m_Anni);
                for (int i = 24; i <= 120; i += 12)
                    this.m_C[(int)Maths.Floor(i / 12d)] = reader.Read("C" + i, this.m_C[(int)Maths.Floor(i / 12d)]);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Tabella", IDTabella);
                writer.Write("Sesso", m_Sesso);
                writer.Write("Anni", m_Anni);
                for (int i = 24; i <= 120; i += 12)
                    writer.Write("C" + i, m_C[(int)Maths.Floor(i / 12d)]);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("IDTabella", IDTabella);
                writer.WriteTag("Sesso", m_Sesso);
                writer.WriteTag("Anni", m_Anni);
                writer.WriteTag("C", m_C);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDTabella":
                        {
                            m_IDTabella = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Sesso":
                        {
                            m_Sesso = Strings.UCase(Strings.Left(Strings.Trim(DMD.XML.Utils.Serializer.DeserializeString(fieldValue)), 1));
                            break;
                        }

                    case "Anni":
                        {
                            m_Anni = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "C":
                        {
                            m_C = (double?[])fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                string ret = "(" + m_Anni + ", '" + m_Sesso + ", {";
                for (int i = 24; i <= 120; i += 12)
                {
                    if (i > 24)
                        ret += ", ";
                    ret += Sistema.Formats.FormatPercentage(m_C, 4);
                }

                ret += "})";
                return ret;
            }
        }


        /// <summary>
    /// Cursore sulla tabella dei coefficienti assicurativi
    /// </summary>
    /// <remarks></remarks>
        public class CCoefficientiAssicurativiCursor : Databases.DBObjectCursorBase<CCoefficienteAssicurativo>
        {
            private DBCursorField<int> m_Tabella;
            private DBCursorStringField m_Sesso;
            private DBCursorField<int> m_Anni;

            public CCoefficientiAssicurativiCursor()
            {
                m_Tabella = new DBCursorField<int>("Tabella");
                m_Sesso = new DBCursorStringField("Sesso");
                m_Anni = new DBCursorField<int>("Anni");
            }

            public DBCursorField<int> Tabella
            {
                get
                {
                    return m_Tabella;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public DBCursorStringField Sesso
            {
                get
                {
                    return m_Sesso;
                }
            }

            public DBCursorField<int> Anni
            {
                get
                {
                    return m_Anni;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CCoefficienteAssicurativo();
            }

            public override string GetTableName()
            {
                return "tbl_CoefficientiAssicurativi";
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }
        }


        /// <summary>
    /// Collezione di righe di coefficienti assicurativi appartenenti ad una tabella
    /// </summary>
    /// <remarks></remarks>
        public class CCoefficientiAssicurativiCollection : CCollection<CCoefficienteAssicurativo>
        {
            private CTabellaAssicurativa m_Tabella;

            public CCoefficientiAssicurativiCollection()
            {
                m_Tabella = null;
            }

            public CCoefficientiAssicurativiCollection(CTabellaAssicurativa tabella)
            {
                if (tabella is null)
                    throw new ArgumentNullException("tabella");
                Initialize(tabella);
            }

            public CTabellaAssicurativa Tabella
            {
                get
                {
                    return m_Tabella;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Tabella is object)
                    ((CCoefficienteAssicurativo)value).Tabella = m_Tabella;
                base.OnInsert(index, value);
            }

            public CCoefficienteAssicurativo Add(string sesso, int anni)
            {
                var item = new CCoefficienteAssicurativo();
                item.Sesso = sesso;
                item.Anni = anni;
                Add(item);
                return item;
            }

            public double? GetCoefficiente(string sesso, int anni, int durata)
            {
                int i;
                var Item = new CCoefficienteAssicurativo();
                Item.Sesso = sesso;
                Item.Anni = anni;
                // Response.Write("Tabella: " & GetID(m_Tabella, 0) & ", sesso: " & sesso & ", anni: " & anni & ", durata: " & durata & "<br/>")
                i = IndexOf(Item);
                // i = Me.IndexOf2(item, 0, Me.Count-1)
                if (i >= 0)
                {
                    return this[i].get_Coefficiente(durata);
                }
                else
                {
                    return default;
                }
                // Response.Write("i: " & i & "<br/>")
            }

            protected internal bool Initialize(CTabellaAssicurativa tabella)
            {
                Clear();
                Sorted = false;
                m_Tabella = tabella;
                var cursor = new CCoefficientiAssicurativiCursor();
                cursor.Tabella.Value = DBUtils.GetID(tabella, 0);
                cursor.Sesso.SortOrder = SortEnum.SORT_ASC;
                cursor.Anni.SortOrder = SortEnum.SORT_ASC;
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                Comparer = this;
                Sorted = true;
                return true;
            }

            protected override int CompareWithType(CCoefficienteAssicurativo a, CCoefficienteAssicurativo b)
            {
                int ret = a.Anni - b.Anni;
                if (ret == 0)
                    ret = DMD.Strings.Compare(a.Sesso, b.Sesso, true);
                return ret;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public static DateTime CalcolaDecorrenza(DateTime d)
        {
            int giorno = DMD.DateUtils.GetDay(d);
            int mAdd = Sistema.IIF(giorno >= 15, 2, 1);
            return (DateTime)DMD.DateUtils.GetMonthFirstDay(DMD.DateUtils.DateAdd("M", mAdd, DMD.DateUtils.GetMonthFirstDay(d)));
        }
    }
}