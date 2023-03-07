using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public class COffertaCQSCalculator
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public delegate void PropertyChangedEventHandler(object sender, Databases.PropertyChangedEventArgs e);

            private int m_TANDigits;
            private int m_TAEGDigits;
            private decimal m_OneriErariali;
            private bool m_Diretta;
            private decimal m_NettoAllaMano;
            private double m_TAN;
            private double m_TAEG;
            private decimal m_Rata;
            private int m_Durata;
            private decimal m_CapitaleFinanziato;
            private decimal m_Interessi;
            private decimal m_NettoErogato;
            private decimal m_ProvvMax;
            private decimal m_Commissioni;
            private decimal m_ProvvTAN;
            private decimal m_Spese;
            private double m_Sconto;
            private decimal m_ValoreSconto;
            private decimal m_CommissioniTotaliAgenzia;
            private CCQSPDCessionarioClass m_Cessionario;
            private CCollection<CCQSPDCessionarioClass> m_Cessionari;
            private CCollection<CProfilo> m_Profili;
            private CCollection<CCQSPDProdotto> m_Prodotti;
            private CCollection<CProdottoXTabellaFin> m_Tabelle;
            private CCollection<CProdottoXTabellaAss> m_Assicurazioni;
            private CCollection<CProdottoXTabellaSpesa> m_TabelleSpese;
            private COffertaCQS m_Offerta;
            private CProfilo m_Profilo;
            private CCQSPDProdotto m_Prodotto;
            private CProdottoXTabellaFin m_TabellaFinRel;
            private CProdottoXTabellaAss m_TabellaAssRel;
            private CProdottoXTabellaSpesa m_TabellaSpeseRel;
            private DateTime m_DataCaricamento;
            private DateTime m_Decorrenza;
            private Anagrafica.CPersonaFisica m_Cliente;
            private CCollection<EstinzioneXEstintore> m_Estinzioni;
            private FinestraLavorazione m_W;
            private CCollection<ClienteXCollaboratore> m_Collaboratori;
            private CCollaboratore m_Collaboratore;
            private ClienteXCollaboratore m_ClienteXCollaboratore;
            private CCollection<ClienteXCollaboratore> m_CollaboratoriXCliente;
            private decimal m_SommaMontanteResiduo;
            private decimal m_SommaEstinzioni;
            private bool m_Calculated;
            private Sistema.CUser m_User;

            public COffertaCQSCalculator()
            {
                m_TANDigits = 2;
                m_TAEGDigits = 2;
                m_OneriErariali = 0m;
                m_Diretta = true;
                m_Cessionario = null;
                m_Profili = null;
                m_Prodotti = null;
                m_Tabelle = null;
                m_Assicurazioni = null;
                m_TabelleSpese = null;
                m_Offerta = null;
                m_Profilo = null;
                m_Prodotto = null;
                m_TabellaFinRel = null;
                m_TabellaAssRel = null;
                m_TabellaSpeseRel = null;
                m_CommissioniTotaliAgenzia = 0m;
                m_ValoreSconto = 0m;
                m_DataCaricamento = DMD.DateUtils.ToDay();
                m_Decorrenza = CalcolaDecorrenza(m_DataCaricamento);
                m_Cliente = null;
                m_Estinzioni = null;
                m_W = null;
                m_Collaboratori = null;
                m_Collaboratore = null;
                m_ClienteXCollaboratore = null;
                m_CollaboratoriXCliente = null;
                m_SommaMontanteResiduo = 0m;
                m_SommaEstinzioni = 0m;
                m_Calculated = false;
                m_User = Sistema.Users.CurrentUser;
            }

            public decimal ValoreSconto
            {
                get
                {
                    Validate();
                    return m_ValoreSconto;
                }

                set
                {
                    decimal oldValue = m_ValoreSconto;
                    if (oldValue == value)
                        return;
                    m_ValoreSconto = value;
                    DoChanged("ValoreSconto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente configurato per il calcolatore
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser User
            {
                get
                {
                    return m_User;
                }

                set
                {
                    var oldValue = m_User;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_User = value;
                    DoChanged("User", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il cliente
        /// </summary>
        /// <returns></returns>
            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    SetCliente(value);
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la durata del prestito in mesi
        /// </summary>
        /// <returns></returns>
            public int Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    int oldValue = Durata;
                    if (oldValue == value)
                        return;
                    m_Durata = value;
                    DoChanged("Durata", value, oldValue);
                }
            }

            public double CalcolaEta(DateTime data)
            {
                return m_Cliente.get_Eta(data);
            }

            public double CalcolaAnzianita(DateTime data)
            {
                return m_Cliente.ImpiegoPrincipale.Anzianita(data);
            }

            public double CalcolaEtaAInizioFinanziamento()
            {
                return CalcolaEta(DataDecorrenza);
            }

            public double CalcolaEtaAFineFinanziamento()
            {
                int durata = Durata;
                var decorrenza = DataDecorrenza;
                return CalcolaEta((DateTime)DMD.DateUtils.DateAdd("M", durata, decorrenza));
            }

            public double CalcolaAnzianitaAInizioFinanziamento()
            {
                return CalcolaAnzianita(DataDecorrenza);
            }

            public double CalcolaAnzianitaAFineFinanziamento()
            {
                int durata = Durata;
                var decorrenza = DataDecorrenza;
                return CalcolaAnzianita((DateTime)DMD.DateUtils.DateAdd("M", durata, decorrenza));
            }

            public void SetOfferta(COffertaCQS value)
            {
                // Dim i As Integer
                var ddec = DataDecorrenza;
                m_Collaboratore = value.Collaboratore;
                m_ClienteXCollaboratore = value.ClienteXCollaboratore;
                m_Estinzioni = new CEstinzioniXEstintoreCollection(value);
                m_Cessionario = value.Cessionario;
                m_Profili = new CCollection<CProfilo>();
                m_Profilo = value.Profilo;
                var profili = Finanziaria.Profili.GetPreventivatoriUtente(ddec, true);
                foreach (CProfilo prof in profili)
                {
                    if (prof.IDCessionario == DBUtils.GetID(m_Cessionario) && prof.IsValid(ddec))
                    {
                        m_Profili.Add(prof);
                    }
                }

                m_Prodotto = value.Prodotto;
                m_Tabelle = new CCollection<CProdottoXTabellaFin>();
                m_Assicurazioni = null;
                m_TabelleSpese = null;
                m_TabellaAssRel = value.TabellaAssicurativaRel;
                m_TabellaSpeseRel = value.TabellaSpese;
                m_TabellaFinRel = value.TabellaFinanziariaRel;
                m_OneriErariali = value.OneriErariali;
                m_TAN = value.TAN;
                m_TAEG = value.TAEG;
                m_Rata = value.Rata;
                m_Durata = value.Durata;
                m_CapitaleFinanziato = (decimal)value.CapitaleFinanziato;
                m_Interessi = value.Interessi;
                m_NettoErogato = value.NettoRicavo;
                m_NettoAllaMano = m_NettoErogato;
                m_ProvvMax = value.ValoreProvvigioneMassima;
                m_Commissioni = value.ValoreSpread;
                m_ProvvTAN = (decimal)value.ValoreProvvTAN;
                m_Spese = value.Imposte;
                m_Sconto = (double)value.ValoreRiduzioneProvvigionale;
                decimal daEstinguere = 0m;
                decimal residuo = (decimal)value.getSommaDebitiResidui(m_Estinzioni);
                decimal ml = value.MontanteLordo - residuo;
                foreach (EstinzioneXEstintore est in m_Estinzioni)
                {
                    if (est.Selezionata)
                    {
                        daEstinguere += est.TotaleDaRimborsare;
                    }
                }

                m_NettoAllaMano = m_NettoErogato - daEstinguere;
                if (m_Prodotto is object)
                {
                    foreach (CProdottoXTabellaFin tbl in m_Prodotto.TabelleFinanziarieRelations)
                    {
                        if (tbl.Stato == ObjectStatus.OBJECT_VALID && tbl.Tabella is object && tbl.Tabella.Stato == ObjectStatus.OBJECT_VALID && tbl.Tabella.IsValid(ddec))
                        {
                            m_Tabelle.Add(tbl);
                        }
                    }
                }

                m_Diretta = Sistema.TestFlag(value.Flags, OffertaFlags.DirettaCollaboratore);
                decimal provvcoll = value.getSommaProvvigioniA(CQSPDTipoSoggetto.Collaboratore) - value.ValoreRiduzioneProvvigionale;
                m_Estinzioni = null;
            }

            protected void DoChanged(string propName, object newValue, object oldValue)
            {
                m_Calculated = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName, newValue, oldValue));
            }

            public DateTime DataDecorrenza
            {
                get
                {
                    return m_Decorrenza;
                }

                set
                {
                    var oldValue = m_Decorrenza;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Decorrenza = value;
                    doDecorrenzaChanged();
                    DoChanged("DataDecorrenza", value, oldValue);
                }
            }

            public bool Diretta
            {
                get
                {
                    return m_Diretta;
                }

                set
                {
                    if (Diretta == value)
                        return;
                    m_Diretta = value;
                    DoChanged("Diretta", value, !value);
                }
            }

            public CCollaboratore Collaboratore
            {
                get
                {
                    if (m_Collaboratore is null)
                    {
                        m_Collaboratore = Collaboratori.GetItemByUser(User);
                    }

                    if (m_Collaboratore is null)
                    {
                        var w = FinestraCorrente;
                        if (w is object)
                            m_Collaboratore = w.Collaboratore;
                    }

                    return m_Collaboratore;
                }

                set
                {
                    var oldValue = m_Collaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Collaboratore = value;
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            // Public Property Collaboratore As CCollaboratore
            // Get
            // Return Me.m_Collaboratore
            // End Get
            // Set(value As CCollaboratore)
            // Dim oldValue As CCollaboratore = Me.m_Collaboratore
            // If (value Is oldValue) Then Return
            // Me.m_Collaboratore = value
            // 'If (cbo.options.length === 2) Then cbo.selectedIndex = 1
            // 'document.getElementById(Me.getCUID() + "trCessionario").style.display = (cbo.options.length > 2) ? "" : "none"
            // Me.doCessionarioChanged()
            // Me._checkButtons()
            // Me.DoChanged("Collaboratore", value, oldValue)
            // End Set
            // End Property

            public FinestraLavorazione FinestraCorrente
            {
                get
                {
                    if (m_W is null && m_Cliente is object)
                    {
                        m_W = FinestreDiLavorazione.GetFinestraCorrente(m_Cliente);
                    }

                    return m_W;
                }

                set
                {
                    var oldValue = m_W;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_W = value;
                    DoChanged("FinestraCorrente", value, oldValue);
                }
            }

            protected void doProfiloChanged()
            {
                // Me.m_Profilo = Me.m_Profili.GetItemById(GetFieldValue(Me.getCUID() + "profilo"))
                m_Prodotti = null;
                m_Prodotto = null;
                // If (Me.m_Prodotti.Count = 1) Then Me.m_Prodotto = Me.m_Prodotti(0)
                doProdottoChanged();
            }

            protected void doProdottoChanged()
            {
                var ddec = DataDecorrenza;

                // Me.m_Prodotto = Me.m_Prodotti.GetItemById(GetFieldValue(Me.getCUID() + "prod"))
                m_Tabelle = null;
                m_TabellaFinRel = null;
                m_Assicurazioni = null;
                m_TabellaAssRel = null;
                m_TabelleSpese = null;
                m_TabellaSpeseRel = null;


                // If (Me.m_Cliente IsNot Nothing OrElse Me.m_ClienteXCollaboratore IsNot Nothing) Then
                // Me.ChangeEstinzione(0, False)
                // End If

                // Me.doSpeseChanged()
            }

            public CCollection<CCQSPDProdotto> Prodotti
            {
                get
                {
                    if (m_Prodotti is null)
                    {
                        m_Prodotti = GetElencoProdotti(Profilo, DataCaricamento);
                        if (m_Prodotti.Count == 1)
                        {
                            m_Prodotto = m_Prodotti[0];
                        }
                    }

                    return m_Prodotti;
                }
            }

            public CProdottoXTabellaAss TabellaAssicurativaRel
            {
                get
                {
                    if (m_TabellaAssRel is null && TabelleAssicurative.Count == 1)
                    {
                        m_TabellaAssRel = TabelleAssicurative[0];
                    }

                    return m_TabellaAssRel;
                }

                set
                {
                    var oldValue = TabellaAssicurativaRel;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_TabellaAssRel = value;
                    DoChanged("TabellaAssicurativaRel", value, oldValue);
                }
            }

            public CProdottoXTabellaFin TabellaFinanziariaRel
            {
                get
                {
                    if (m_TabellaFinRel is null && TabelleFinanziarie.Count == 1)
                    {
                        m_TabellaFinRel = TabelleFinanziarie[0];
                    }

                    return m_TabellaFinRel;
                }

                set
                {
                    var oldValue = TabellaFinanziariaRel;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_TabellaFinRel = value;
                    DoChanged("TabellaFinanziariaRel", value, oldValue);
                }
            }

            public CProdottoXTabellaSpesa TabellaSpeseRel
            {
                get
                {
                    if (m_TabellaSpeseRel is null && TabelleSpese.Count == 1)
                    {
                        m_TabellaSpeseRel = m_TabelleSpese[0];
                    }

                    return m_TabellaSpeseRel;
                }

                set
                {
                    var oldValue = TabellaSpeseRel;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_TabellaSpeseRel = value;
                    DoChanged("TabellaSpeseRel", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la collezione di profili validi per la configurazione corrente
        /// </summary>
        /// <returns></returns>
            public CCollection<CProfilo> Profili
            {
                get
                {
                    if (m_Profili is null)
                    {
                        m_Profili = new CCollection<CProfilo>();
                        var ddec = DataCaricamento;
                        CCollection items = null;
                        var col = Collaboratore;
                        if (col is object)
                        {
                            items = new CCollection();
                            if (col.ListinoPredefinito is object)
                            {
                                items.Add(col.ListinoPredefinito);
                            }
                        }
                        else
                        {
                            items = Finanziaria.Profili.GetPreventivatoriUtente(ddec, true);
                        }

                        foreach (CProfilo prof in items)
                        {
                            if (prof.IDCessionario == DBUtils.GetID(m_Cessionario) && prof.IsValid(ddec))
                            {
                                m_Profili.Add(prof);
                            }
                        }

                        if (m_Profili.Count == 1)
                            m_Profilo = m_Profili[0];
                    }

                    return m_Profili;
                }
            }

            /// <summary>
        /// Restituisce la collezione delle tabelle finanziarie valide per la configurazione corrente
        /// </summary>
        /// <returns></returns>
            public CCollection<CProdottoXTabellaFin> TabelleFinanziarie
            {
                get
                {
                    if (m_Tabelle is null)
                    {
                        m_Tabelle = new CCollection<CProdottoXTabellaFin>();
                        if (Prodotto is object)
                        {
                            var ddec = DataCaricamento;
                            foreach (CProdottoXTabellaFin tbl in Prodotto.TabelleFinanziarieRelations)
                            {
                                if (tbl.Stato == ObjectStatus.OBJECT_VALID && tbl.Tabella is object && tbl.Tabella.Stato == ObjectStatus.OBJECT_VALID && tbl.Tabella.IsValid(ddec))
                                {
                                    m_Tabelle.Add(tbl);
                                }
                            }
                        }

                        if (m_Tabelle.Count == 1)
                            m_TabellaFinRel = m_Tabelle[0];
                    }

                    return m_Tabelle;
                }
            }

            /// <summary>
        /// Restituisce la collezione delle tabelle assicurative valide per la configurazione corrente
        /// </summary>
        /// <returns></returns>
            public CCollection<CProdottoXTabellaAss> TabelleAssicurative
            {
                get
                {
                    if (m_Assicurazioni is null)
                    {
                        m_Assicurazioni = new CCollection<CProdottoXTabellaAss>();
                        if (Prodotto is object)
                        {
                            var ddec = DataCaricamento;
                            foreach (CProdottoXTabellaAss tbl in Prodotto.TabelleAssicurativeRelations)
                            {
                                if (tbl.Stato == ObjectStatus.OBJECT_VALID)
                                {
                                    m_Assicurazioni.Add(tbl);
                                }
                            }
                        }

                        if (m_Assicurazioni.Count == 1)
                            m_TabellaAssRel = m_Assicurazioni[0];
                    }

                    return m_Assicurazioni;
                }
            }

            /// <summary>
        /// Restituisce la collezione delle tabelle spese valide per la configurazione corrente
        /// </summary>
        /// <returns></returns>
            public CCollection<CProdottoXTabellaSpesa> TabelleSpese
            {
                get
                {
                    if (m_TabelleSpese is null)
                    {
                        m_TabelleSpese = new CCollection<CProdottoXTabellaSpesa>();
                        if (Prodotto is object)
                        {
                            var ddec = DataCaricamento;
                            foreach (CProdottoXTabellaSpesa tbl in Prodotto.TabelleSpese)
                            {
                                if (tbl.Stato == ObjectStatus.OBJECT_VALID && tbl.IsValid(ddec) && tbl.TabellaSpese is object && tbl.TabellaSpese.IsValid(ddec))
                                {
                                    m_TabelleSpese.Add(tbl);
                                }
                            }
                        }

                        if (m_TabelleSpese.Count == 1)
                            m_TabellaSpeseRel = m_TabelleSpese[0];
                    }

                    return m_TabelleSpese;
                }
            }

            /// <summary>
        /// Restituisce o imposta il cessionario selezionato per il calcolo
        /// </summary>
        /// <returns></returns>
            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null && Cessionari.Count == 1)
                    {
                        m_Cessionario = Cessionari[0];
                    }

                    return m_Cessionario;
                }

                set
                {
                    var oldValue = m_Cessionario;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_Cessionario = value;
                    doCessionarioChanged();
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce l'elenco dei cessionari validi per la configurazione corrente
        /// </summary>
        /// <returns></returns>
            public CCollection<CCQSPDCessionarioClass> Cessionari
            {
                get
                {
                    if (m_Cessionari is null)
                    {
                        m_Cessionari = new CCollection<CCQSPDCessionarioClass>();
                        var ddec = DataDecorrenza;
                        var items = Finanziaria.Cessionari.GetAllCessionari();
                        foreach (CCQSPDCessionarioClass c in items)
                        {
                            if (c.Stato == ObjectStatus.OBJECT_VALID && c.IsValid(ddec) && c.Visibile && c.UsabileInPratiche)
                            {
                                m_Cessionari.Add(c);
                            }
                        }
                    }

                    return m_Cessionari;
                }
            }

            /// <summary>
        /// Restituisce l'elenco dei collaboratori a cui è stato assegnato il cliente nella data specificata
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
            public CCollection<CCollaboratore> GetCollaboratori(DateTime d)
            {
                var ret = new CCollection<CCollaboratore>();
                var items = CollaboratoriXCliente;
                foreach (ClienteXCollaboratore c in items)
                {
                    CCollaboratore col = null;
                    if (c.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        col = c.Collaboratore;
                    }

                    if (col is object && col.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        ret.Add(col);
                    }
                }

                return ret;
            }

            /// <summary>
        /// Restituisce la collezione dei prodotti inclusi nel profilo e validi alla data specificata
        /// </summary>
        /// <param name="profilo"></param>
        /// <param name="ddec"></param>
        /// <returns></returns>
            public CCollection<CCQSPDProdotto> GetElencoProdotti(CProfilo profilo, DateTime ddec)
            {
                var ret = new CCollection<CCQSPDProdotto>();
                if (profilo is object)
                {
                    var prodotti = profilo.ProdottiXProfiloRelations.GetProdotti();
                    foreach (CCQSPDProdotto prodotto in prodotti)
                    {
                        if (prodotto.IsValid(ddec))
                        {
                            ret.Add(prodotto);
                        }
                    }
                }

                return ret;
            }

            public bool IsRapportini()
            {
                return Consulenze.Module.UserCanDoAction("showprovvigioni");
            }


            /// <summary>
        /// Restituisce o imposta la relazione tra il cliente ed il collaboratore
        /// </summary>
        /// <returns></returns>
            public ClienteXCollaboratore ClienteXCollaboratore
            {
                get
                {
                    if (m_ClienteXCollaboratore is null)
                    {
                        if (m_Cliente is object && m_Collaboratore is object)
                        {
                            m_ClienteXCollaboratore = Collaboratori.ClientiXCollaboratori.GetItemByPersonaECollaboratore(m_Cliente, m_Collaboratore);
                        }
                    }

                    return m_ClienteXCollaboratore;
                }

                set
                {
                    var oldValue = m_ClienteXCollaboratore;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_ClienteXCollaboratore = value;
                    m_Diretta = !Sistema.TestFlag(value.Flags, ClienteCollaboratoreFlags.AssegnatoDaAgenzia); // value.getAssegnatoDa() === Nothing
                    m_Estinzioni = null;
                    // Me.chengeEstinzione(0, False)
                    DoChanged("ClienteXCollaboratore", value, oldValue);
                }
            }

            protected void Inizializza()
            {
                m_W = null;
                m_Estinzioni = null;
                if (CollaboratoriXCliente.Count == 1)
                    m_ClienteXCollaboratore = CollaboratoriXCliente[0];
                // Me.doCollaboratoreChanged()
            }

            protected void doDecorrenzaChanged()
            {
                m_Profili = null;
                m_Prodotti = null;
                m_Cessionari = null;
            }

            protected void doCessionarioChanged()
            {
                m_Profili = null;
                m_Prodotti = null;
            }

            private void _aggiornaEstinzione()
            {
                if (m_Cliente is object || m_ClienteXCollaboratore is object)
                    return;
                var items = EstinzioniXEstintore;
                if (items.Count == 0)
                    return;
                var est = items[0];
                if (m_Prodotto is object)
                {
                    est.Parametro = m_Prodotto.IdTipoContratto;
                }

                est.Tipo = Sistema.IIF(est.Parametro == "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA);
                est.DataCaricamento = DataCaricamento;
                est.DataEstinzione = DataDecorrenza;
                est.Rata = m_SommaMontanteResiduo;
                est.NumeroQuoteResidue = 1;
                est.TotaleDaEstinguere = m_SommaEstinzioni;
            }

            private void doResiduoChanged()
            {
                // Me.Ricalcola()
                Invalidate();
            }

            private void doValoreEstinzioneChanged()
            {
                // Me.Ricalcola()
                Invalidate();
            }

            public CCollection<ClienteXCollaboratore> CollaboratoriXCliente
            {
                get
                {
                    if (m_CollaboratoriXCliente is null)
                    {
                        m_CollaboratoriXCliente = new CCollection<ClienteXCollaboratore>();
                        if (DBUtils.GetID(m_Cliente) != 0)
                        {
                            var col = Collaboratori.GetItemByUser(User);
                            var cursor = new ClienteXCollaboratoreCursor();
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IgnoreRights = true;
                            cursor.IDPersona.Value = DBUtils.GetID(m_Cliente);
                            if (col is object)
                                cursor.IDCollaboratore.Value = DBUtils.GetID(col);
                            while (!cursor.EOF())
                            {
                                var cli = cursor.Item;
                                m_CollaboratoriXCliente.Add(cli);
                                cursor.MoveNext();
                            }

                            cursor.Dispose();
                        }
                    }

                    return m_CollaboratoriXCliente;
                }
            }

            protected internal void SetCliente(Anagrafica.CPersonaFisica p)
            {
                m_Cliente = p;
                m_CollaboratoriXCliente = null;
                m_Cessionari = null;
                m_Profili = null;
                m_TabellaAssRel = null;
                m_Estinzioni = null;

                // Me.chengeEstinzione(0, False)
            }

            public DateTime DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }

                set
                {
                    var oldValue = m_DataCaricamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataCaricamento = value;
                    m_Estinzioni = null;
                    m_Cessionari = null;
                    m_Cessionario = null;
                    DoChanged("DataCaricamento", value, oldValue);
                }
            }

            public void ChangeEstinzione(int i, bool selezionata)
            {
                decimal sum = 0m;
                decimal sum1 = 0m;
                var items = EstinzioniXEstintore;
                items[i].Selezionata = selezionata;
                var dcar = DataCaricamento;
                var ddec = DataDecorrenza;
                string nomeCess = "";
                if (Cessionario is object)
                    nomeCess = Cessionario.Nome;
                foreach (EstinzioneXEstintore est in items)
                {
                    if (DBUtils.GetID(est) != 0)
                    {
                        var dest = ddec;
                        bool IsInterno = !string.IsNullOrEmpty(nomeCess) && DMD.Strings.Compare(nomeCess, est.NomeCessionario, true) == 0;
                        int resid = 0;
                        if (est.DataFine.HasValue)
                            resid = (int)Maths.Max(0L, DMD.DateUtils.DateDiff("M", ddec, est.DataFine.Value) + 1L);
                        if (Sistema.Formats.ToInteger(est.Durata) > 0)
                            resid = (int)Maths.Min(resid, est.Durata);
                        if (IsInterno)
                            resid += 1;
                        est.Parametro = "";
                        if (m_Prodotto is object)
                            est.Parametro = m_Prodotto.IdTipoContratto;
                        est.DataCaricamento = dcar;
                        est.DataEstinzione = dest;
                        est.AggiornaValori();
                        est.NumeroQuoteResidue = resid;
                        est.AggiornaCalcolo();
                        if (est.Selezionata)
                        {
                            sum = (decimal)(sum + est.MontanteResiduo);
                            sum1 += est.TotaleDaRimborsare;
                        }
                    }
                }

                m_SommaMontanteResiduo = sum;
                m_SommaEstinzioni = sum1;
                Invalidate();
            }

            private bool isValid(CEstinzione est)
            {
                bool ret;
                ret = est is object && est.Stato == ObjectStatus.OBJECT_VALID && est.Scadenza.HasValue;
                if (!ret)
                    return ret;
                // var durata = //Formats.ToInteger(est.getDurata()) > 0
                switch (est.Tipo)
                {
                    case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO:
                    case TipoEstinzione.ESTINZIONE_PRESTITODELEGA:
                    case TipoEstinzione.ESTINZIONE_CQP:
                        {
                            // case TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                            return true;
                        }

                    default:
                        {
                            return false;
                        }
                }
            }

            private decimal CalcolaResiduo(CEstinzione est)
            {
                string nomeCess = "";
                if (Cessionario is object)
                    nomeCess = Cessionario.Nome;
                string tipoContratto = "";
                if (m_Prodotto is object)
                    tipoContratto = m_Prodotto.IdTipoContratto;
                if (string.IsNullOrEmpty(tipoContratto))
                    tipoContratto = "C";
                var tp = Sistema.IIF(tipoContratto == "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA);
                var dec = DataDecorrenza;
                if (!isValid(est))
                    return 0m;
                string nmCess = est.NomeIstituto;
                int nRate = (int)Maths.Max(0L, 1L + DMD.DateUtils.DateDiff("M", dec, est.Scadenza.Value));
                var estTP = est.Tipo;
                if (estTP == TipoEstinzione.ESTINZIONE_CQP || estTP == TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO)
                {
                    estTP = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO;
                }

                if (estTP != tp)
                    return 0m;
                if (
                        tp != TipoEstinzione.ESTINZIONE_NO 
                        && 
                        estTP == tp 
                        && !string.IsNullOrEmpty(nomeCess) 
                        && DMD.Strings.Compare(nomeCess, nmCess, true) == 0
                        )
                {
                    int DeltaML_AggiuntiRate = Sistema.Formats.ToInteger(Configuration.GetValueInt("DeltaML_AggiuntiRate", 0));
                    nRate = nRate + DeltaML_AggiuntiRate;
                }

                var calculator = new CEstinzioneCalculator();
                calculator.Rata = Sistema.Formats.ToValuta(est.Rata);
                calculator.Durata = Sistema.Formats.ToInteger(est.Durata);
                calculator.TAN = Sistema.Formats.ToDouble(est.TAN);
                calculator.PenaleEstinzione = Sistema.Formats.ToDouble(est.PenaleEstinzione);
                calculator.NumeroRateResidue = nRate;
                decimal res = (decimal)calculator.DebitoResiduo;
                return Maths.Max(0m, res);
            }

            protected internal void RimuoviCliente()
            {
                SetCliente(null);
            }

            protected internal void doSave()
            {
                if (m_Cliente is null)
                {
                    _doSaveCollab();
                }
                else
                {
                    _doSaveCiente();
                }
            }

            protected internal void _doSaveCiente()
            {
                // If (Calendar.Compare(Calendar.ToDay(), Me.txtDecorrenza.getValue()) >= 0)Then Throw "Decorrenza non valida!"
                var cu = User;
                var p = m_Cliente;
                if (p is null)
                    throw new ArgumentNullException("Cliente non selezionato");
                var po = p.PuntoOperativo;
                var w = FinestraCorrente;
                if (w is null)
                    throw new ArgumentNullException("Cliente non lavorabile (finestra di lavorazione nulla)");
                var ric = w.RichiestaFinanziamento;
                if (ric is null)
                {
                    ric = new CRichiestaFinanziamento();
                    ric.Cliente = p;
                    ric.Data = DMD.DateUtils.Now();
                    ric.AssegnatoA = cu;
                    ric.PresaInCaricoDa = cu;
                    ric.PuntoOperativo = po;
                    ric.StatoRichiesta = StatoRichiestaFinanziamento.EVASA;
                    ric.Stato = ObjectStatus.OBJECT_VALID;
                    ric.FinestraLavorazione = w;
                    ric.Note = "Richiesta creata automaticamente nel Preventivatore 2018";
                    ric.Save();
                }

                w.RichiestaFinanziamento = ric;
                var off = m_Offerta;
                off.Cliente = p;
                // off.setPuntoOperativo(po)
                off.Stato = ObjectStatus.OBJECT_VALID;
                // off.setFinestraCorrente(w)
                off.Save();
                var cons = new CQSPDConsulenza();
                if (m_Prodotto.IdTipoContratto == "C")
                {
                    cons.OffertaCQS = off;
                }
                else
                {
                    cons.OffertaPD = off;
                }

                CQSPDStudioDiFattibilita sf = null; // (w.getStudioDiFattibilita() === Nothing) ? null : w.getStudioDiFattibilita().getStudioDiFattibilita()
                if (sf is null)
                    sf = StudiDiFattibilita.GetUltimoStudioDiFattibilita(p);
                if (sf is null || DMD.DateUtils.Compare(DMD.DateUtils.GetDatePart(sf.Data), DMD.DateUtils.ToDay()) < 0)
                {
                    sf = new CQSPDStudioDiFattibilita();
                    sf.Cliente = p;
                    sf.Data = off.CreatoIl;
                    sf.Richiesta = ric;
                    sf.PuntoOperativo = p.PuntoOperativo;
                    sf.Consulente = Consulenti.GetItemByUser(cu);
                    sf.OraInizio = sf.Data;
                    sf.Stato = ObjectStatus.OBJECT_VALID;
                    sf.Save();
                }

                sf.Proposte.Add(cons);
                cons.DataConsulenza = off.CreatoIl;
                cons.InseritoDa = cu;
                cons.StatoConsulenza = StatiConsulenza.INSERITA;
                cons.Cliente = p;
                cons.PuntoOperativo = p.PuntoOperativo;
                cons.Consulente = Consulenti.GetItemByUser(cu);
                cons.Descrizione = "Preventivo generato dal Preventivatore Prestitalia 2018";
                cons.FinestraLavorazione = w;
                cons.Collaboratore = Collaboratore;
                // cons.setStato(ObjectStatus.OBJECT_VALID)
                cons.Save();
                // cons.Estinzioni().PreparaEstinzini(Me.txtDecorrenza.getValue())
                decimal sum = 0m;
                var items = EstinzioniXEstintore;
                foreach (EstinzioneXEstintore ex in items)
                {
                    if (ex.Selezionata)
                    {
                        ex.Estintore = cons;
                        ex.Parametro = m_Prodotto.IdTipoContratto;
                        // ex.setDataEstinzione(Me.txtDecorrenza.getValue())
                        // ex.AggiornaValori()
                        cons.Estinzioni.Add(ex);
                        ex.Save(true);
                        sum += ex.TotaleDaRimborsare;
                    }
                }

                cons.NettoRicavo = off.NettoRicavo;
                cons.SommaEstinzioni = sum;
                cons.SommaTrattenuteVolontarie = 0m;
                cons.SommaPignoramenti = 0m;
                cons.Save();
                w.StudioDiFattibilita = cons;
                w.Save();
            }

            private void _doSaveCollab()
            {
                // If (Calendar.Compare(Calendar.ToDay(), Me.txtDecorrenza.getValue()) >= 0)Then Throw "Decorrenza non valida!"
                var cu = User;
                var p = m_ClienteXCollaboratore;
                if (p is null)
                    throw new ArgumentNullException("Cliente non selezionato");
                var off = m_Offerta;
                off.ClienteXCollaboratore = p;
                off.Cliente = p.Persona;
                // if (p.getPersona() !== null) off.setPuntoOperativo(p.getPersona().getPuntoOperativo())
                off.Stato = ObjectStatus.OBJECT_VALID;
                off.Save();

                // var i, exi, prov, 
                var items = EstinzioniXEstintore;
                foreach (EstinzioneXEstintore exi in items)
                {
                    exi.Estintore = (IEstintore)off;
                    // exi.AggiornaValori()
                    exi.Save(true);
                }

                foreach (CCQSPDProvvigioneXOfferta prov in off.Provvigioni)
                    prov.Save(true);
            }

            private string NomeCessionario
            {
                get
                {
                    if (Cessionario is null)
                    {
                        return "";
                    }
                    else
                    {
                        return Cessionario.Nome;
                    }
                }
            }

            private string Parametro
            {
                get
                {
                    if (Prodotto is null)
                        return "";
                    return Prodotto.IdTipoContratto;
                }
            }

            public CCollection<EstinzioneXEstintore> EstinzioniXEstintore
            {
                get
                {
                    // var i, items, est, ex
                    CCollection<CEstinzione> items;
                    var ddec = DataDecorrenza;
                    var dcar = DataCaricamento;
                    var dest = ddec;
                    string nomeCess = NomeCessionario;
                    EstinzioneXEstintore ex;
                    if (m_Estinzioni is null)
                    {
                        if (m_Cliente is object)
                        {
                            items = Estinzioni.GetEstinzioniByPersona(m_Cliente);
                        }
                        else if (m_ClienteXCollaboratore is object)
                        {
                            items = new CCollection<CEstinzione>();
                            var cursor = new CEstinzioniCursor();
                            cursor.IDClienteXCollaboratore.Value = DBUtils.GetID(m_ClienteXCollaboratore);
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IgnoreRights = true;
                            while (!cursor.EOF())
                            {
                                items.Add(cursor.Item);
                                cursor.MoveNext();
                            }

                            cursor.Dispose();
                        }
                        else
                        {
                            m_Estinzioni = new CCollection<EstinzioneXEstintore>();
                            ex = new EstinzioneXEstintore();
                            ex.Selezionata = true;
                            ex.Stato = ObjectStatus.OBJECT_VALID;
                            ex.Parametro = Parametro;
                            ex.Tipo = Sistema.IIF(ex.Parametro == "C", TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA);
                            ex.DataEstinzione = dest;
                            ex.Rata = 0;
                            ex.NumeroQuoteResidue = 1;
                            ex.TotaleDaEstinguere = 0;
                            m_Estinzioni.Add(ex);
                            return m_Estinzioni;
                        }

                        m_Estinzioni = new CCollection<EstinzioneXEstintore>();
                        foreach (CEstinzione est in items)
                        {
                            if (est.Stato == ObjectStatus.OBJECT_VALID && !est.Estinta && est.IsInCorso(dcar))
                            {
                                dest = ddec; // Calendar.Max(est.getDataEstinzione(), ddec)
                                bool IsInterno = DMD.Strings.Compare(nomeCess, est.NomeIstituto, true) == 0;
                                if (est.Scadenza.HasValue == false && est.DataInizio.HasValue && Sistema.Formats.ToInteger(est.Durata) > 0)
                                {
                                    est.Scadenza = DMD.DateUtils.GetLastMonthDay(DMD.DateUtils.DateAdd("M", est.Durata.Value, est.DataInizio.Value));
                                }

                                int resid = 0;
                                if (est.Scadenza.HasValue)
                                    resid = (int)Maths.Max(0L, DMD.DateUtils.DateDiff("M", ddec, est.Scadenza.Value) + 1L);
                                if (Sistema.Formats.ToInteger(est.Durata) > 0)
                                    resid = (int)Maths.Min(resid, est.Durata);
                                if (IsInterno)
                                    resid += 1;
                                ex = new EstinzioneXEstintore();
                                ex.Estinzione = est;
                                ex.Estintore = null;
                                ex.Parametro = Parametro;
                                ex.Stato = ObjectStatus.OBJECT_VALID;
                                ex.DataCaricamento = dcar; // //(IsInterno)? dcar : Calendar.DateAdd("M", -1, ddec))
                                ex.DataEstinzione = dest;
                                ex.AggiornaCalcolo();
                                m_Estinzioni.Add(ex);
                                ex.Save();
                            }
                        }
                    }

                    return m_Estinzioni;
                }
            }

            private decimal CalcolaProvvigioneCollaboratore(COffertaCQS o)
            {
                decimal ret = 0m;
                foreach (CCQSPDProvvigioneXOfferta item in o.Provvigioni)
                {
                    if (item.PagataA == CQSPDTipoSoggetto.Collaboratore)
                    {
                        ret += DMD.Decimals.TryParse(item.Valore, 0);
                    }
                }

                return ret;
            }

            public CProfilo Profilo
            {
                get
                {
                    if (m_Profilo is null && Profili.Count == 1)
                    {
                        m_Profilo = Profili[0];
                    }

                    return m_Profilo;
                }

                set
                {
                    var oldValue = m_Profilo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Profilo = value;
                    DoChanged("Profilo", value, oldValue);
                }
            }

            public CCQSPDProdotto Prodotto
            {
                get
                {
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = m_Prodotto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Prodotto = value;
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            public decimal Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    decimal oldValue = m_Rata;
                    if (oldValue == value)
                        return;
                    m_Rata = value;
                    DoChanged("Rata", value, oldValue);
                }
            }

            public double TAN
            {
                get
                {
                    Validate();
                    return m_TAN;
                }

                set
                {
                    double oldValue = m_TAN;
                    if (oldValue == value)
                        return;
                    m_TAN = value;
                    DoChanged("TAN", value, oldValue);
                }
            }

            public double TAEG
            {
                get
                {
                    Validate();
                    return m_TAEG;
                }

                set
                {
                    double oldValue = m_TAEG;
                    if (oldValue == value)
                        return;
                    m_TAEG = value;
                    DoChanged("TAEG", value, oldValue);
                }
            }

            public decimal ValoreProvvigioneMassima
            {
                get
                {
                    Validate();
                    return m_ProvvMax;
                }

                set
                {
                    decimal oldValue = m_ProvvMax;
                    if (oldValue == value)
                        return;
                    m_ProvvMax = value;
                    DoChanged("ValoreProvvigioneMassima", value, oldValue);
                }
            }

            public decimal Commissioni
            {
                get
                {
                    return m_Commissioni;
                }

                set
                {
                    decimal oldValue = m_Commissioni;
                    if (oldValue == value)
                        return;
                    m_Commissioni = value;
                    DoChanged("Commissioni", value, oldValue);
                }
            }

            public decimal CapitaleFinanziato
            {
                get
                {
                    Validate();
                    return m_CapitaleFinanziato;
                }

                set
                {
                    decimal oldValue = m_CapitaleFinanziato;
                    if (oldValue == value)
                        return;
                    m_CapitaleFinanziato = value;
                    DoChanged("CapitaleFinanziato", value, oldValue);
                }
            }

            public decimal NettoErogato
            {
                get
                {
                    Validate();
                    return m_NettoErogato;
                }

                set
                {
                    decimal oldValue = m_NettoErogato;
                    if (oldValue == value)
                        return;
                    m_NettoErogato = value;
                    DoChanged("NettoErogato", value, oldValue);
                }
            }

            public decimal NettoAllaMano
            {
                get
                {
                    return NettoErogato - SommaEstinzioni;
                }
            }

            public decimal OneriErariali
            {
                get
                {
                    Validate();
                    return m_OneriErariali;
                }

                set
                {
                    decimal oldValue = m_OneriErariali;
                    if (oldValue == value)
                        return;
                    m_OneriErariali = value;
                    DoChanged("OneriErariali", value, oldValue);
                }
            }

            public decimal Spese
            {
                get
                {
                    return m_Spese;
                }

                set
                {
                    decimal oldValue = m_Spese;
                    if (oldValue == value)
                        return;
                    m_Spese = value;
                    DoChanged("Spese", value, oldValue);
                }
            }

            public decimal ValoreProvvigioneTAN
            {
                get
                {
                    Validate();
                    return m_ProvvTAN;
                }

                set
                {
                    decimal oldValue = m_ProvvTAN;
                    if (oldValue == value)
                        return;
                    m_ProvvTAN = value;
                    DoChanged("ValoreProvvigioneTAN", value, oldValue);
                }
            }

            private COffertaCQS PrepareOfferta()
            {
                var o = new COffertaCQS();
                o.DataCaricamento = DataCaricamento;
                o.DataDecorrenza = DataDecorrenza;
                o.OffertaLibera = true;
                o.Cessionario = Cessionario;
                o.Profilo = Profilo;
                o.Prodotto = Prodotto;
                o.Rata = Rata;
                o.Durata = Durata;
                o.TAN = TAN;
                o.TAEG = TAEG;
                o.ValoreProvvigioneMassima = ValoreProvvigioneMassima;
                o.ValoreSpread = Commissioni;
                o.ValoreUpFront = Commissioni;
                o.ValoreRiduzioneProvvigionale = ValoreSconto;
                o.PremioDaCessionario = 0;
                o.PremioDaCessionario1 = 0;
                o.CapitaleFinanziato = CapitaleFinanziato;
                o.NettoRicavo = NettoErogato;
                o.OneriErariali = OneriErariali;
                o.TabellaFinanziariaRel = TabellaFinanziariaRel;
                o.TabellaAssicurativaRel = TabellaAssicurativaRel;
                o.TabellaSpese = TabellaSpeseRel;
                o.Imposte = Spese;
                o.ValoreProvvTAN = ValoreProvvigioneTAN;
                // Me.m_ClienteXCollaboratore = Finanziaria.Collaboratori.ClientiXCollaboratori().GetItemById(colID)
                CCollaboratore col = null;
                if (ClienteXCollaboratore is object)
                    col = ClienteXCollaboratore.Collaboratore;
                o.Collaboratore = col;
                o.ClienteXCollaboratore = ClienteXCollaboratore;
                o.Flags = Sistema.SetFlag(o.Flags, OffertaFlags.DirettaCollaboratore, m_Diretta);
                o.PuntoOperativo = null;
                if (m_Cliente is object)
                    o.PuntoOperativo = m_Cliente.PuntoOperativo;
                o.Save();
                var estinzioni = EstinzioniXEstintore;
                o.SincronizzaProvvigioni(estinzioni);
                foreach (CCQSPDProvvigioneXOfferta provv1 in o.Provvigioni)
                {
                    provv1.Aggiorna(o, estinzioni);
                    switch (provv1.PagataDa)
                    {
                        case CQSPDTipoSoggetto.Cessionario:
                            {
                                break;
                            }

                        case CQSPDTipoSoggetto.Agenzia:
                            {
                                switch (provv1.PagataA)
                                {
                                    case CQSPDTipoSoggetto.Cessionario:
                                        {
                                            break;
                                        }

                                    case CQSPDTipoSoggetto.Agenzia:
                                        {
                                            break;
                                        }

                                    case CQSPDTipoSoggetto.Collaboratore:
                                        {
                                            break;
                                        }

                                    case CQSPDTipoSoggetto.Cliente:
                                        {
                                            break;
                                        }
                                }

                                break;
                            }

                        case CQSPDTipoSoggetto.Collaboratore:
                            {
                                break;
                            }

                        case CQSPDTipoSoggetto.Cliente:
                            {
                                switch (provv1.PagataA)
                                {
                                    case CQSPDTipoSoggetto.Cessionario:
                                        {
                                            break;
                                        }

                                    case CQSPDTipoSoggetto.Agenzia:
                                        {
                                            break;
                                        }

                                    case CQSPDTipoSoggetto.Collaboratore:
                                        {
                                            m_NettoErogato = (decimal)(m_NettoErogato - provv1.Valore);
                                            o.NettoRicavo = m_NettoErogato;
                                            var c = new CTAEGCalculator();
                                            c.Rata = (double)o.Rata;
                                            c.Durata = o.Durata;
                                            c.NettoRicavo = (double)m_NettoErogato;
                                            m_TAEG = c.Calc();
                                            o.TAEG = m_TAEG;
                                            break;
                                        }

                                    case CQSPDTipoSoggetto.Cliente:
                                        {
                                            break;
                                        }
                                }

                                break;
                            }
                    }
                }

                // Me.ctrlProvv.setItem(o)
                // Me.ctrlProvv.setEstinzioni(Me.m_Estinzioni)

                return o;
            }

            private bool CheckExiType(EstinzioneXEstintore exi)
            {
                if (exi.Tipo == TipoEstinzione.ESTINZIONE_CQP || exi.Tipo == TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO)
                {
                    return Parametro == "C";
                }
                else if (exi.Tipo == TipoEstinzione.ESTINZIONE_PRESTITODELEGA)
                {
                    return Parametro == "D";
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
        /// Restituisce o imposta la somma dei montanti residui da restituire
        /// </summary>
        /// <returns></returns>
            public decimal SommaMontantiResidui
            {
                get
                {
                    if (m_Cliente is null && m_ClienteXCollaboratore is null)
                    {
                        return m_SommaMontanteResiduo;
                    }
                    else
                    {
                        decimal ret = 0m;
                        foreach (EstinzioneXEstintore exi in EstinzioniXEstintore)
                        {
                            if (exi.Selezionata && exi.MontanteResiduo.HasValue && CheckExiType(exi))
                            {
                                ret = (decimal)(ret + exi.MontanteResiduo);
                            }
                        }

                        return ret;
                    }
                }

                set
                {
                    decimal oldValue = m_SommaMontanteResiduo;
                    if (oldValue == value)
                        return;
                    m_SommaMontanteResiduo = value;
                    DoChanged("SommaMontanteResiduo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la somma dei prestiti da rimborsare
        /// </summary>
        /// <returns></returns>
            public decimal SommaEstinzioni
            {
                get
                {
                    if (m_Cliente is null && m_ClienteXCollaboratore is null)
                    {
                        return m_SommaEstinzioni;
                    }
                    else
                    {
                        decimal ret = 0m;
                        foreach (EstinzioneXEstintore exi in EstinzioniXEstintore)
                        {
                            if (exi.Selezionata)
                            {
                                ret += exi.TotaleDaRimborsare;
                            }
                        }

                        return ret;
                    }
                }

                set
                {
                    decimal oldValue = m_SommaEstinzioni;
                    if (oldValue == value)
                        return;
                    m_SommaEstinzioni = value;
                    DoChanged("SommaEstinzioni", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale di sconto applicata
        /// </summary>
        /// <returns></returns>
            public double Sconto
            {
                get
                {
                    return m_Sconto;
                }

                set
                {
                    double oldValue = m_Sconto;
                    if (oldValue == value)
                        return;
                    m_Sconto = value;
                    DoChanged("Sconto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il montante lordo dell'operazione (rata * durata)
        /// </summary>
        /// <returns></returns>
            public decimal MontanteLordo
            {
                get
                {
                    return Rata * Durata;
                }
            }

            public decimal CommissioniTotaliAgenzia
            {
                get
                {
                    Validate();
                    return m_CommissioniTotaliAgenzia;
                }
            }

            public void Invalidate()
            {
                m_Calculated = false;
            }

            public void Validate()
            {
                if (m_Calculated)
                    return;
                Ricalcola();
            }

            private void Ricalcola()
            {
                // Me.m_ClienteXCollaboratore = Finanziaria.Collaboratori.ClientiXCollaboratori.GetItemById(Me.m_)
                // Me.m_Diretta = True
                m_Calculated = true;
                if (ClienteXCollaboratore is object)
                {
                    m_Diretta = !Sistema.TestFlag(m_ClienteXCollaboratore.Flags, ClienteCollaboratoreFlags.AssegnatoDaAgenzia);
                }

                var cess = Cessionario;
                var prof = Profilo;
                var p = Prodotto;
                var relFin = TabellaFinanziariaRel;
                var relAss = TabellaAssicurativaRel;
                var relSpe = TabellaSpeseRel;
                var decorrenza = DataDecorrenza;
                CTabellaFinanziaria tabella = null;
                if (relFin is object)
                    tabella = relFin.Tabella;
                double TAN = 0d;
                if (tabella is object && !tabella.TANVariabile)
                {
                    TAN = (double)tabella.TAN;
                }

                double pmax = 0d;
                if (tabella is object)
                {
                    pmax = (double)tabella.ProvvMax;
                }

                decimal upfrontmax = 0m;
                if (tabella is object)
                {
                    upfrontmax = (decimal)tabella.UpFrontMax;
                }

                decimal rata = Rata;
                if (rata < 0m)
                    throw new ArgumentOutOfRangeException("Rata non valida");
                int durata = Durata;
                if (durata < 12 || durata > 120)
                    throw new ArgumentOutOfRangeException("Durata non valida");

                // Sincronizziamo il valore residuo (per nessun cliente selezionato)
                _aggiornaEstinzione();
                decimal residuo = SommaMontantiResidui;
                decimal daEstinguere = SommaEstinzioni;
                decimal ml = rata * durata;
                decimal cf = 0m;
                if (rata > 0m && durata > 0 && TAN > 0d)
                {
                    var ti = new CTANInverter();
                    ti.Rata = (double)rata;
                    ti.Durata = durata;
                    ti.TAN = TAN;
                    cf = (decimal)ti.Calc();
                }

                decimal interessi = ml - cf;
                // var residuo = Formats.ParseValuta(GetFieldValue(Me.getCUID() + "estinzione"))
                if (residuo < 0m)
                    throw new ArgumentOutOfRangeException("Montante residuo non valido");
                double sconto = Sconto;
                if (sconto < 0d || sconto > pmax)
                    throw new ArgumentNullException("Riduzione provvigionale non valida");
                decimal commissioni = (decimal)Maths.Max(0d, (pmax - sconto) * (double)(ml - residuo) / 100d);
                m_ValoreSconto = 0m;
                if (upfrontmax > 0m)
                {
                    commissioni = (decimal)Maths.Min((double)upfrontmax, (pmax - sconto) * (double)(ml - residuo) / 100d);
                    m_ValoreSconto = (decimal)Sistema.IIF(ml - residuo > 0m, sconto * (double)(ml - residuo) / 100d, 0d);
                }

                double pTAN = 0d;
                decimal baseML = ml;
                if (tabella is object)
                {
                    if (residuo == 0m)
                    {
                        pTAN = (double)tabella.Sconto;
                    }
                    else
                    {
                        pTAN = tabella.ProvvTANR;
                    }

                    if (tabella.TipoCalcoloProvvTAN == TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI)
                    {
                        baseML = ml - residuo;
                    }
                }

                decimal provvagee = (decimal)((double)commissioni + (double)baseML * pTAN / 100d);
                decimal provvage = Sistema.IIF(baseML > 0m, provvagee * 100m / baseML, 0m);
                CTabellaSpese tabellaSpese = null;
                if (TabellaSpeseRel is object)
                {
                    tabellaSpese = m_TabellaSpeseRel.TabellaSpese;
                }

                decimal spese = 0m;
                if (tabella is object)
                {
                    spese = (decimal)tabellaSpese.get_SpeseFisse(durata);
                }

                decimal oneri = 0m;
                if (tabella is object)
                {
                    oneri = tabellaSpese.get_Rivalsa(durata);
                }

                decimal nettoErogato = Maths.Max(0m, cf - commissioni - spese);
                double TAEG = 0d;
                if (rata > 0m && durata > 0 && nettoErogato > 0m)
                {
                    var c = new CTAEGCalculator();
                    c.Rata = (double)rata;
                    c.Durata = durata;
                    c.NettoRicavo = (double)nettoErogato;
                    TAEG = c.Calc();
                }

                // ***
                decimal nettoAllaMano = nettoErogato;
                // Me.m_DaEstinguere = 0
                var estinzioni = EstinzioniXEstintore;
                nettoAllaMano = nettoErogato - SommaEstinzioni;
                m_OneriErariali = oneri;
                m_TAN = TAN;
                m_TAEG = TAEG;
                m_Rata = rata;
                m_Durata = durata;
                m_CapitaleFinanziato = cf;
                m_Interessi = interessi;
                m_NettoErogato = nettoErogato;
                m_NettoAllaMano = nettoAllaMano;
                m_ProvvMax = (decimal)Maths.Max(0d, pmax * (double)(ml - residuo) / 100d);
                m_Commissioni = commissioni;
                m_ProvvTAN = provvagee - commissioni;
                m_Spese = spese;
                m_CommissioniTotaliAgenzia = provvagee;
                var o = PrepareOfferta();
                m_NettoAllaMano = m_NettoErogato - SommaEstinzioni;
                nettoErogato = m_NettoErogato;
                nettoAllaMano = m_NettoAllaMano;
                TAEG = m_TAEG;
                m_Offerta = o;
                if (m_Collaboratore is object)
                {
                    decimal scontoe = (decimal)(sconto * (double)(ml - residuo) / 100d);
                    decimal provvcoll = CalcolaProvvigioneCollaboratore(o) - scontoe;
                }
            }

            public CCollaboratore GetCollaboratoreByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (CCollaboratore c in GetCollaboratori(DataCaricamento))
                {
                    if (DMD.Strings.Compare(c.NomePersona, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectCollaboratore(string nome)
            {
                Collaboratore = GetCollaboratoreByName(nome);
            }

            public CCQSPDCessionarioClass GetCessionarioByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (CCQSPDCessionarioClass c in Cessionari)
                {
                    if (DMD.Strings.Compare(c.Nome, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectCessionario(string nome)
            {
                Cessionario = GetCessionarioByName(nome);
            }

            public CProfilo GetProfiloByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (var c in Profili)
                {
                    if (DMD.Strings.Compare(c.Nome, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectProfilo(string nome)
            {
                Profilo = GetProfiloByName(nome);
            }

            public CCQSPDProdotto GetProdottoByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (var c in Prodotti)
                {
                    if (DMD.Strings.Compare(c.Nome, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectProdotto(string nome)
            {
                Prodotto = GetProdottoByName(nome);
            }

            public CProdottoXTabellaFin GetTabellaFinanziariaByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (var c in TabelleFinanziarie)
                {
                    if (c.Tabella is object && DMD.Strings.Compare(c.Tabella.Nome, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectTabellaFinanziaria(string nome)
            {
                TabellaFinanziariaRel = GetTabellaFinanziariaByName(nome);
            }

            public CProdottoXTabellaAss GetTabellaAssicurativaByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (var c in TabelleAssicurative)
                {
                    if (DMD.Strings.Compare(c.Descrizione, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectTabellaAssicurativa(string nome)
            {
                TabellaAssicurativaRel = GetTabellaAssicurativaByName(nome);
            }

            public CProdottoXTabellaSpesa GetTabellaSpeseByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                foreach (CProdottoXTabellaSpesa c in TabelleSpese)
                {
                    if (DMD.Strings.Compare(c.Nome, nome, true) == 0)
                        return c;
                }

                return null;
            }

            public void SelectTabellaSpese(string nome)
            {
                TabellaSpeseRel = GetTabellaSpeseByName(nome);
            }
        }
    }
}