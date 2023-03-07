using System;
using System.Collections;
using DMD;
using minidom;
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
    /// Gestione delle pratiche di cessione del quinto e delega
    /// </summary>
    /// <remarks></remarks>
        public sealed class CPraticheClass : CModulesClass<Finanziaria.CPraticaCQSPD>
        {

            /// <summary>
        /// Evento generato quando viene generata una condizione di attenzione per la pratica
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaWatchEventHandler PraticaWatch;

            public delegate void PraticaWatchEventHandler(ItemEventArgs e);



            /// <summary>
        /// Evento generato quando viene apportata una correzione alla pratica
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaCorrettaEventHandler PraticaCorretta;

            public delegate void PraticaCorrettaEventHandler(ItemEventArgs e);


            /// <summary>
        /// Evento generato quando viene formulata un'offerta che richiede l'approvazione
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaRequireApprovationEventHandler PraticaRequireApprovation;

            public delegate void PraticaRequireApprovationEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la pratica subisce un pasaggio di stato
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaChangeStatusEventHandler PraticaChangeStatus;

            public delegate void PraticaChangeStatusEventHandler(ItemEventArgs e);


            /// <summary>
        /// Evento generato quando viene memorizzata una nuova pratica
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaCreatedEventHandler PraticaCreated;

            public delegate void PraticaCreatedEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando viene eliminata una pratica esistente
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaDeletedEventHandler PraticaDeleted;

            public delegate void PraticaDeletedEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando viene modificata una pratica
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaModifiedEventHandler PraticaModified;

            public delegate void PraticaModifiedEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato per notificare un evento relativo ad una pratica
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event NotifyPraticaEventHandler NotifyPratica;

            public delegate void NotifyPraticaEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando una pratica in attesa di conferma viene autorizzata
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event PraticaAutorizzataEventHandler PraticaAutorizzata;

            public delegate void PraticaAutorizzataEventHandler(ItemEventArgs e);

            /// <summary>
            /// Evento generato quando una pratica in attesa di conferma viene rifiutata
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event PraticaRifiutataEventHandler PraticaRifiutata;

            public delegate void PraticaRifiutataEventHandler(ItemEventArgs e);

            public event PraticaPresaInCaricoEventHandler PraticaPresaInCarico;

            public delegate void PraticaPresaInCaricoEventHandler(ItemEventArgs e);

            internal CPraticheClass() : base("modPraticheCQSPD", typeof(Finanziaria.CPraticheCQSPDCursor))
            {
            }

            private bool m_EventsEnabled = true;


            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se abilitare o meno gli eventi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool EventEnabled
            {
                get
                {
                    return m_EventsEnabled;
                }

                set
                {
                    m_EventsEnabled = value;
                }
            }

            public CCollection<OggettoAnomalo> GetAnomalie(int idUfficio, int idOperatore, DateTime? dal, DateTime? al, int ritardoConsentito = 1)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    var ret = new CCollection<OggettoAnomalo>();
                    Finanziaria.CPraticaCQSPD pratica;
                    // Dim operatore As CUser

                    var stato = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO);
                    while (stato is object)
                    {
                        if (stato.GiorniStallo.HasValue)
                        {
                            cursor.IgnoreRights = true;
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDStatoAttuale.Value = DBUtils.GetID(stato);
                            cursor.Flags.Value = Finanziaria.PraticaFlags.HIDDEN | Finanziaria.PraticaFlags.TRASFERITA;
                            cursor.Flags.Operator = Databases.OP.OP_NE;
                            // cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                            if (idUfficio != 0)
                                cursor.IDPuntoOperativo.Value = idUfficio;
                            if (idOperatore != 0)
                                cursor.StatoPreventivoAccettato.IDOperatore = idOperatore;
                            if (dal.HasValue)
                                cursor.StatoPreventivoAccettato.Inizio = dal.Value;
                            if (al.HasValue)
                                cursor.StatoPreventivoAccettato.Fine = al.Value;
                            while (!cursor.EOF())
                            {
                                pratica = cursor.Item;
                                DateTime d1 = default;
                                if (pratica.StatoDiLavorazioneAttuale.Data.HasValue)
                                    d1 = (DateTime)pratica.StatoDiLavorazioneAttuale.Data;
                                if (Maths.Abs(DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Day, d1, DMD.DateUtils.Now())) > stato.GiorniStallo == true)
                                {
                                    // operatore = pratica.StatoDiLavorazioneAttuale.Operatore
                                    // If (operatore Is Nothing) Then operatore = pratica.StatoContatto.Operatore
                                    // If (operatore Is Nothing) Then operatore = pratica.CreatoDa
                                    var op = new OggettoAnomalo();
                                    op.Oggetto = pratica;
                                    op.AggiungiAnomalia("La pratica è in stato " + stato.Nome + " da più di " + stato.GiorniStallo + " giorni", 0);
                                    ret.Add(op);
                                }

                                cursor.MoveNext();
                            }
                        }

                        stato = stato.DefaultTarget;
                    }

                    return ret;
                }
            }

            /// <summary>
        /// Analizza la stringa template e sostituisce i valori
        /// </summary>
        /// <param name="template">Testo da elaborare</param>
        /// <param name="context">Contesto in cui avviene l'elaborazione</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ParseTemplate(string template, CKeyCollection context)
            {
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)context["Pratica"];
                Sistema.CUser currentUser = (Sistema.CUser)context["CurrentUser"];
                var info = pratica.Info;
                template = Strings.Replace(template, "%%ID%%", pratica.ID.ToString());
                template = Strings.Replace(template, "%%NUMEROPRATICA%%", pratica.NumeroPratica);
                template = Strings.Replace(template, "%%USERNAME%%", currentUser.Nominativo);
                template = Strings.Replace(template, "%%NOMINATIVOCLIENTE%%", pratica.NominativoCliente);
                template = Strings.Replace(template, "%%NOMEPRODOTTO%%", pratica.NomeProdotto);
                template = Strings.Replace(template, "%%NOMEPROFILO%%", pratica.NomeProfilo);
                template = Strings.Replace(template, "%%NOMECESSIONARIO%%", pratica.NomeCessionario);
                template = Strings.Replace(template, "%%NOMEAMMINISTRAZIONE%%", pratica.Impiego.NomeAzienda);
                template = Strings.Replace(template, "%%MONTANTELORDO%%", Sistema.Formats.FormatValuta(pratica.MontanteLordo));
                template = Strings.Replace(template, "%%SPREAD%%", Sistema.Formats.FormatPercentage(pratica.Spread, 3));
                template = Strings.Replace(template, "%%VALORESPREAD%%", Sistema.Formats.FormatValuta(pratica.ValoreSpread));
                template = Strings.Replace(template, "%%UPFRONT%%", Sistema.Formats.FormatPercentage(pratica.UpFront, 3));
                template = Strings.Replace(template, "%%VALOREUPFRONT%%", Sistema.Formats.FormatValuta(pratica.ValoreUpFront));
                template = Strings.Replace(template, "%%RUNNING%%", Sistema.Formats.FormatPercentage(pratica.Running, 3));
                template = Strings.Replace(template, "%%VALORERUNNING%%", Sistema.Formats.FormatValuta(pratica.ValoreRunning));
                template = Strings.Replace(template, "%%PROVVIGIONEMASSIMA%%", Sistema.Formats.FormatPercentage(pratica.ProvvigioneMassima, 3));
                template = Strings.Replace(template, "%%VALOREPROVVIGIONEMASSIMA%%", Sistema.Formats.FormatValuta(pratica.ValoreProvvigioneMassima));
                template = Strings.Replace(template, "%%PROVVIGIONEBROKER%%", Sistema.Formats.FormatPercentage(pratica.Provvigionale.get_PercentualeSu(pratica.MontanteLordo), 3));
                template = Strings.Replace(template, "%%VALOREPROVVIGIONEBROKER%%", Sistema.Formats.FormatValuta(pratica.Provvigionale.ValoreTotale()));
                template = Strings.Replace(template, "%%MOTIVORICHIESTASCONTO%%", pratica.Info.MotivoSconto);
                template = Strings.Replace(template, "%%DETTAGLIORICHIESTASCONTO%%", pratica.Info.MotivoScontoDettaglio);
                template = Strings.Replace(template, "%%ANZIANITA%%", Sistema.Formats.FormatInteger(pratica.Anzianita));
                template = Strings.Replace(template, "%%NUMEROCELLULARE%%", Sistema.Formats.FormatPhoneNumber(pratica.Cellulare));
                template = Strings.Replace(template, "%%CODICEFISCALE%%", Sistema.Formats.FormatCodiceFiscale(pratica.CodiceFiscale));
                template = Strings.Replace(template, "%%COGNOMECLIENTE%%", pratica.CognomeCliente);
                template = Strings.Replace(template, "%%NOMECONSULENTE%%", pratica.NomeConsulente);
                template = Strings.Replace(template, "%%COSTO%%", Sistema.Formats.FormatValuta(info.Costo));
                template = Strings.Replace(template, "%%CREATODA%%", pratica.CreatoDa.Nominativo);
                template = Strings.Replace(template, "%%CREATOIL%%", Sistema.Formats.FormatUserDateTime(pratica.CreatoIl));
                if (pratica.OffertaCorrente is object)
                {
                    template = Strings.Replace(template, "%%MOTIVOCONFERMASCONTO%%", pratica.OffertaCorrente.MotivoConfermaSconto);
                    template = Strings.Replace(template, "%%DETTAGLIOCONFERMASCONTO%%", pratica.OffertaCorrente.DettaglioConfermaSconto);
                }
                else
                {
                    template = Strings.Replace(template, "%%MOTIVOCONFERMASCONTO%%", "");
                    template = Strings.Replace(template, "%%DETTAGLIOCONFERMASCONTO%%", "");
                }

                if (info is object)
                {
                    if (info.Commerciale is object)
                    {
                        template = Strings.Replace(template, "%%NOMECOMMERCIALE%%", info.Commerciale.Nominativo);
                    }
                    else
                    {
                        template = Strings.Replace(template, "%%NOMECOMMERCIALE%%", "");
                    }

                    template = Strings.Replace(template, "%%DATAAGGIORNAMENTOPRATICA%%", Sistema.Formats.FormatUserDateTime(info.DataAggiornamentoPT));
                }
                else
                {
                    template = Strings.Replace(template, "%%NOMECOMMERCIALE%%", "");
                    template = Strings.Replace(template, "%%DATAAGGIORNAMENTOPRATICA%%", "");
                }

                template = Strings.Replace(template, "%%DATAASSUNZIONE%%", Sistema.Formats.FormatUserDate(pratica.Impiego.DataAssunzione));
                template = Strings.Replace(template, "%%DATADECORRENZA%%", Sistema.Formats.FormatUserDate(pratica.DataDecorrenza));
                // template = Replace(template, "%%DATALICENZIAMENTO%%", Formats.FormatUserDate(pratica.DataLicenziamento))
                template = Strings.Replace(template, "%%DATATRASFERIMENTO%%", Sistema.Formats.FormatUserDateTime(info.DataTrasferimento));
                template = Strings.Replace(template, "%%DAVEDERE%%", Sistema.IIF(pratica.DaVedere, "Sì", "No"));
                template = Strings.Replace(template, "%%EMAIL%%", pratica.eMail);
                if (pratica.Impiego.EntePagante is object)
                {
                    template = Strings.Replace(template, "%%NOMEENTEPAGANTE%%", pratica.Impiego.EntePagante.Nominativo);
                }
                else
                {
                    template = Strings.Replace(template, "%%NOMEENTEPAGANTE%%", "");
                }

                template = Strings.Replace(template, "%%ETA%%", Sistema.Formats.FormatInteger(pratica.Eta));
                template = Strings.Replace(template, "%%ETAFF%%", Sistema.Formats.FormatInteger(pratica.EtaFineFinanziamento));
                template = Strings.Replace(template, "%%FAX%%", Sistema.Formats.FormatPhoneNumber(pratica.Fax));
                template = Strings.Replace(template, "%%STATOATTUALE%%", pratica.StatoAttuale.Nome);
                if (pratica.StatoDiLavorazioneAttuale is object && pratica.StatoDiLavorazioneAttuale.FromStato is object)
                {
                    template = Strings.Replace(template, "%%STATOPRECEDENTE%%", pratica.StatoDiLavorazioneAttuale.FromStato.DescrizioneStato);
                    template = Strings.Replace(template, "%%NOTESTATOATTUALE%%", pratica.StatoDiLavorazioneAttuale.Note);
                }
                else
                {
                    template = Strings.Replace(template, "%%STATOPRECEDENTE%%", "");
                    template = Strings.Replace(template, "%%NOTESTATOATTUALE%%", "");
                }

                template = Strings.Replace(template, "%%BASEURL%%", Sistema.ApplicationContext.BaseURL);
                if (pratica.StatoAnnullata is object)
                {
                    template = Strings.Replace(template, "%%MOTIVOANNULLAMENTO%%", pratica.StatoAnnullata.Note);
                }
                else
                {
                    template = Strings.Replace(template, "%%MOTIVOANNULLAMENTO%%", "");
                }

                template = Strings.Replace(template, "%%CONTEXTMESSAGE%%", DMD.Strings.CStr(context.GetItemByKey("Message")));
                return template;
            }

            /// <summary>
        /// Invia una notifica agli utenti abilitati
        /// </summary>
        /// <param name="pratica"></param>
        /// <remarks></remarks>
            public void Notifica(Finanziaria.CPraticaCQSPD pratica)
            {
                if (m_EventsEnabled)
                    NotifyPratica?.Invoke(new ItemEventArgs(pratica));
            }

            /// <summary>
        /// Invia una notifica agli utenti abilitati
        /// </summary>
        /// <param name="pratica"></param>
        /// <remarks></remarks>
            public void Autorizza(Finanziaria.CPraticaCQSPD pratica)
            {
                if (m_EventsEnabled)
                    PraticaAutorizzata?.Invoke(new ItemEventArgs(pratica));
            }

            internal void DoOnCreate(ItemEventArgs e)
            {
                doItemCreated(e);
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                Anagrafica.CPersona cliente = pratica.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.isClienteInAcquisizione = false;
                info.isClienteAcquisito = true;
                info.AggiornaOperazione(pratica, "Pratica " + pratica.NumeroPratica + " caricata");
                PraticaCreated?.Invoke(new ItemEventArgs(pratica));
                Module.DispatchEvent(new Sistema.EventDescription("Create", "Creata la pratica N°" + pratica.NumeroPratica, pratica));
            }

            internal void DoOnDelete(ItemEventArgs e)
            {
                doItemDeleted(e);
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                PraticaDeleted?.Invoke(new ItemEventArgs(pratica));
                Module.DispatchEvent(new Sistema.EventDescription("Delete", "Eliminata la pratica N°" + pratica.NumeroPratica, pratica));
            }

            // Friend Sub DoOnSave(ByVal pratica As CPraticaCQSPD)
            // If (m_EventsEnabled) Then RaiseEvent PraticaModified(New ItemEventArgs(pratica))
            // End Sub

            /// <summary>
        /// Restituisce un oggetto CAllowedRemoteIPs che specifica se l'IP è autorizzato o meno a inviare una pratica
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CAllowedRemoteIPs GetRemoteIPInfo(string ip)
            {
                using (var cursor = new Finanziaria.CAllowedRemoteIPsCursor())
                {
                    Finanziaria.CAllowedRemoteIPs ret;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.RemoteIP.Value = ip;
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    ret = cursor.Item;
                    return ret;
                }
            }

            /// <summary>
        /// Formatta il dettaglio
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string FormatDettaglioStato(int value)
            {
                switch (value)
                {
                    case 0:
                        {
                            return "Altro";
                        }

                    case 1:
                        {
                            return "Annullato dal cliente";
                        }

                    case 2:
                        {
                            return "Non fattibile";
                        }

                    default:
                        {
                            return "Sconosciuto";
                        }
                }
            }



            /// <summary>
        /// Restituisce la pratica in base al suo ID nel sistema del cessionario
        /// </summary>
        /// <param name="cessionario"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CPraticaCQSPD GetItemByNumeroEsterno(Finanziaria.CCQSPDCessionarioClass cessionario, string numero)
            {
                numero = Strings.Trim(numero);
                if (string.IsNullOrEmpty(numero))
                    return null;
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCessionario.Value = DBUtils.GetID(cessionario);
                    cursor.WhereClauses.Add("[StatRichD_Params]=" + DBUtils.DBString(numero));
                    var ret = cursor.Item;
                    return ret;
                }
            }

            public CCollection<Finanziaria.CProfilo> GetArrayProfiliEsterni(Finanziaria.CCQSPDCessionarioClass cessionario, bool onlyValid = true)
            {
                if (cessionario is null)
                    throw new ArgumentNullException("cessionartio");
                CCollection<Finanziaria.CProfilo> items;
                var ret = new CCollection<Finanziaria.CProfilo>();
                items = Finanziaria.Profili.GetPreventivatoriUtente(onlyValid);
                for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                {
                    var item = items[i];
                    if (item.IDCessionario == DBUtils.GetID(cessionario))
                        ret.Add(item);
                }

                return ret;
            }

            public CCollection<Finanziaria.CProfilo> GetArrayProfiliEsterni(Finanziaria.CCQSPDCessionarioClass cessionario, DateTime dataDecorrenza, bool onlyValid = true)
            {
                if (cessionario is null)
                    throw new ArgumentNullException("cessionartio");
                CCollection<Finanziaria.CProfilo> items;
                var ret = new CCollection<Finanziaria.CProfilo>();
                items = Finanziaria.Profili.GetPreventivatoriUtente(dataDecorrenza, onlyValid);
                for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                {
                    var item = items[i];
                    if (item.IDCessionario == DBUtils.GetID(cessionario))
                        ret.Add(item);
                }

                return ret;
            }

            public string FormatStatoPratica(Finanziaria.StatoPraticaEnum? value)
            {
                if (value.HasValue == false)
                    return "";
                switch (value.Value)
                {
                    case Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO:
                        {
                            return "Preventivo";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO:
                        {
                            return "Preventivo accettato";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_STAMPATO:
                        {
                            return "Contratto stampato";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_FIRMATO:
                        {
                            return "Contratto firmato";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_PRATICA_CARICATA:
                        {
                            return "Pratica caricata";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_RICHIESTADELIBERA:
                        {
                            return "Richiesta delibera";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE:
                        {
                            return "Pronta per liquidazione";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_LIQUIDATA:
                        {
                            return "Liquidata";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_DELIBERATA:
                        {
                            return "Deliberata";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA:
                        {
                            return "Archiviata";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_ANNULLATA:
                        {
                            return "Annullata";
                        }

                    case Finanziaria.StatoPraticaEnum.STATO_ESTINTAANTICIPATAMENTE:
                        {
                            return "Estinta anticipatamente";
                        }

                    default:
                        {
                            return ((int)value.Value).ToString();
                        }
                }
            }

            public Finanziaria.StatoPraticaEnum? ParseStatoPratica(string value)
            {
                switch (DMD.Strings.LCase(DMD.Strings.Trim(value)) ?? "")
                {
                    case "preventivo":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO;
                        }

                    case "preventivo accettato":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO;
                        }

                    case "contratto stampato":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_STAMPATO;
                        }

                    case "contratto firmato":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_CONTRATTO_FIRMATO;
                        }

                    case "pratica caricata":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_PRATICA_CARICATA;
                        }

                    case "richiesta delibera":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_RICHIESTADELIBERA;
                        }

                    case "pronta per liquidazione":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE;
                        }

                    case "liquidata":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_LIQUIDATA;
                        }

                    case "deliberata":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_DELIBERATA;
                        }

                    case "archiviata":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA;
                        }

                    case "annullata":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_ANNULLATA;
                        }

                    case "estinta anticipatamente":
                        {
                            return Finanziaria.StatoPraticaEnum.STATO_ESTINTAANTICIPATAMENTE;
                        }

                    default:
                        {
                            return default;
                        }
                }
            }

            public bool GetStatistichePerStatoCL(string dbSQL, int numRows, int conta, ref decimal sommaML, ref double aveProvvBrk, ref double aveSpread, ref double aveProvvTotale, ref decimal sommaMLfil, ref decimal sommaMLBrk, ref decimal costoFisso, ref decimal rappel)
            {
                string dbSQL1 = "SELECT Count(*) As [conta], Sum(IIF([MontanteLordo] Is Null, 0, [MontanteLordo])*IIF([Rappel] Is Null, 0, [Rappel])/100) As [Rappel], Sum([Costo]) As [CostoFisso], Sum(IIF([MontanteLordo] Is Null, 0, [MontanteLordo])*IIF([ProvvBroker] Is Null, 0, [ProvvBroker])/100) As [sommaMLBrk],  Sum(IIF([MontanteLordo] Is Null, 0, [MontanteLordo])*(IIF([Spread] Is Null, 0, [Spread])+IIF([SpreadSotto] Is Null,0,[SpreadSotto]))/100) As [sommaMLfil], Sum([MontanteLordo]) As [sommaML], Sum([ProvvBroker]) As [aveProvvBrk], Sum([Spread]) As [aveSpread], Sum(IIF([ProvvBroker] Is Null, 0, [ProvvBroker])+IIF([Spread] Is Null, 0, [Spread])) As [aveProvvTotale] FROM (" + dbSQL + ") WHERE ([StatoPratica]>=" + ((int)Finanziaria.StatoPraticaEnum.STATO_RICHIESTADELIBERA).ToString() + ") And ([StatoPratica]<=" + ((int)Finanziaria.StatoPraticaEnum.STATO_LIQUIDATA).ToString() + ")";
                using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL1))
                {
                    if (dbRis.Read())
                    {
                        sommaML = Sistema.Formats.ToValuta(dbRis["sommaML"]);
                        aveProvvBrk = Sistema.Formats.ToDouble(dbRis["aveProvvBrk"]) / numRows;
                        aveSpread = Sistema.Formats.ToDouble(dbRis["aveSpread"]) / numRows;
                        aveProvvTotale = Sistema.Formats.ToDouble(dbRis["aveProvvTotale"]) / numRows;
                        sommaMLfil = Sistema.Formats.ToValuta(dbRis["sommaMLfil"]);
                        sommaMLBrk = Sistema.Formats.ToValuta(dbRis["sommaMLBrk"]);
                        costoFisso = Sistema.Formats.ToValuta(dbRis["CostoFisso"]);
                        rappel = (decimal)Sistema.Formats.ToDouble(dbRis["Rappel"]);
                        conta = (int)Sistema.Formats.ToDouble(dbRis["conta"]);
                    }
                    else
                    {
                        sommaML = 0m;
                        aveProvvBrk = 0d;
                        aveSpread = 0d;
                        aveProvvTotale = 0d;
                        sommaMLfil = 0m;
                        sommaMLBrk = 0m;
                        costoFisso = 0m;
                        rappel = 0m;
                        conta = 0;
                    }

                    return true;
                }
            }

            /// <summary>
        /// Ottiene le statistiche annuali suddivise per stato e per mese
        /// </summary>
        /// <returns></returns>
            public CKeyCollection<CCollection<StatsItem>> GetPraticheAnnualiPerStato(int anno)
            {
                var stato = new Finanziaria.CInfoStato();
                var ret = new CKeyCollection<CCollection<StatsItem>>();
                stato.Inizio = DMD.DateUtils.GetYearFirstDay(anno);
                stato.Fine = DMD.DateUtils.GetLastSecond(DMD.DateUtils.GetYearLastDay(anno));
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.PassaggiDiStato.Add(stato);
                    while (!cursor.EOF())
                    {
                        var pratica = cursor.Item;
                        int idStato = pratica.IDStatoAttuale;
                        CCollection<StatsItem> mesi;
                        StatsItem item;
                        if (!ret.ContainsKey("K" + idStato))
                        {
                            mesi = new CCollection<StatsItem>();
                            for (int i = 1; i <= 12; i++)
                            {
                                item = new StatsItem();
                                item.Name = pratica.StatoAttuale.Nome;
                                mesi.Add(item);
                            }

                            ret.Add("K" + idStato, mesi);
                        }
                        else
                        {
                            mesi = ret.GetItemByKey("K" + idStato);
                        }

                        int mese = DMD.DateUtils.Month((DateTime)pratica.StatoDiLavorazioneAttuale.Data) - 1;
                        item = mesi[mese];
                        item.LastRun = (DateTime)DMD.DateUtils.Max(item.LastRun, pratica.StatoDiLavorazioneAttuale.Data);
                        item.Count += 1;
                        item.ExecTime = (float)(item.ExecTime + (float?)pratica.MontanteLordo);
                        cursor.MoveNext();
                    }
                }

                return ret;
            }

            /// <summary>
        /// Ottiene le statistiche annuali suddivise per stato e per mese
        /// </summary>
        /// <returns></returns>
            public CKeyCollection<StatsItem> GetPraticheAnnualiPerTipo(int anno)
            {
                var stato = new Finanziaria.CInfoStato();
                var ret = new CKeyCollection<StatsItem>();
                stato.Inizio = DMD.DateUtils.GetYearFirstDay(anno);
                stato.Fine = DMD.DateUtils.GetLastSecond(DMD.DateUtils.GetYearLastDay(anno));
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.PassaggiDiStato.Add(stato);
                    while (!cursor.EOF())
                    {
                        var pratica = cursor.Item;
                        string tipo = "";
                        if (pratica.Prodotto is null)
                        {
                            tipo = "sconosciuto";
                        }
                        else
                        {
                            tipo = pratica.Prodotto.Categoria;
                            if (string.IsNullOrEmpty(tipo))
                                tipo = pratica.Prodotto.Nome;
                        }

                        var item = ret.GetItemByKey(tipo);
                        if (item is null)
                        {
                            item = new StatsItem();
                            item.Name = tipo;
                            ret.Add(tipo, item);
                        }

                        item.LastRun = (DateTime)DMD.DateUtils.Max(item.LastRun, pratica.StatoDiLavorazioneAttuale.Data);
                        item.Count += 1;
                        item.ExecTime = (float)(item.ExecTime + (float?)pratica.MontanteLordo);
                        cursor.MoveNext();
                    }
                }

                return ret;
            }


            /// <summary>
        /// Restituisce una collezione di pratiche
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheByPersona(Anagrafica.CPersonaFisica persona)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    if (persona is null)
                        throw new ArgumentNullException("persona");
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    if (DBUtils.GetID(persona) == 0)
                        return ret;
                    cursor.IDCliente.Value = DBUtils.GetID(persona);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        var p = cursor.Item;
                        p.SetCliente(persona);
                        ret.Add(p);
                        cursor.MoveNext();
                    }

                    return ret;
                }
            }

            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheByPersona(int idPersona)
            {
                return GetPraticheByPersona((Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(idPersona));
            }

            /// <summary>
        /// Restituisce una collezione di pratiche
        /// </summary>
        /// <param name="azienda"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheByAzienda(Anagrafica.CAzienda azienda)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    if (azienda is null)
                        throw new ArgumentNullException("persona");
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    if (DBUtils.GetID(azienda) != 0)
                    {
                        cursor.IDAmministrazione.Value = DBUtils.GetID(azienda);
                        cursor.StatoPratica.SortOrder = SortEnum.SORT_ASC;
                        cursor.StatoPratica.SortPriority = 99;
                        cursor.Nominativo.SortOrder = SortEnum.SORT_ASC;
                        cursor.Nominativo.SortPriority = 98;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        while (!cursor.EOF())
                        {
                            ret.Add(cursor.Item);
                            cursor.MoveNext();
                        }
                    }

                    return ret;
                }
            }

            protected internal void DoOnInCarico(ItemEventArgs e)
            {
                PraticaPresaInCarico?.Invoke(e);
            }

            internal void DoOnApprovata(ItemEventArgs e)
            {
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                Anagrafica.CPersona cliente = pratica.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.isClienteInAcquisizione = false;
                info.isClienteAcquisito = true;
                info.AggiornaOperazione(pratica, "Pratica " + pratica.NumeroPratica + " offerta autorizzata");
                PraticaAutorizzata?.Invoke(e);
            }

            internal void DoOnRifiutata(ItemEventArgs e)
            {
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                Anagrafica.CPersona cliente = pratica.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.isClienteInAcquisizione = false;
                info.isClienteAcquisito = true;
                info.Note = "Pratica " + pratica.NumeroPratica + " offerta negata";
                info.Save();
                PraticaRifiutata?.Invoke(e);
            }

            internal void DoOnModified(ItemEventArgs e)
            {
                doItemModified(e);
                PraticaModified?.Invoke(e);
            }

            internal void DoOnWatch(ItemEventArgs e)
            {
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                Anagrafica.CPersona cliente = pratica.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.isClienteInAcquisizione = false;
                info.isClienteAcquisito = true;
                info.AggiornaOperazione(pratica, "Pratica " + pratica.NumeroPratica + " in condizione di attenzione");
                PraticaWatch?.Invoke(e);
            }

            internal void DoOnChangeStatus(ItemEventArgs e)
            {
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                Anagrafica.CPersona cliente = pratica.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(pratica, "Pratica " + pratica.NumeroPratica + " in stato: " + pratica.StatoAttuale.Nome + ", Motivo: " + pratica.StatoDiLavorazioneAttuale.Note);
                PraticaChangeStatus?.Invoke(e);
                Module.DispatchEvent(new Sistema.EventDescription("change_status", "Pratica N°" + pratica.NumeroPratica + " in stato " + pratica.StatoDiLavorazioneAttuale.DescrizioneStato, pratica));
            }

            internal void DoOnRequireApprovation(ItemEventArgs e)
            {
                Finanziaria.CPraticaCQSPD pratica = (Finanziaria.CPraticaCQSPD)e.Item;
                Anagrafica.CPersona cliente = pratica.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(pratica, "Pratica " + pratica.NumeroPratica + " in richiesta approvazione");
                PraticaRequireApprovation?.Invoke(e);
                Module.DispatchEvent(new Sistema.EventDescription("require_approvation", Sistema.Users.CurrentUser.Nominativo + " richiede l'approvazione dell'offerta fatta per la pratica N°" + pratica.NumeroPratica, pratica));
            }

            internal void DoOnCorretta(ItemEventArgs e)
            {
                PraticaCorretta?.Invoke(e);
            }

            public DateTime CalcolaProssimaDecorrenza()
            {
                return CalcolaProssimaDecorrenza(DMD.DateUtils.ToDay());
            }

            public DateTime CalcolaProssimaDecorrenza(DateTime fromDate)
            {
                var ret = fromDate;
                if (ret.Day <= 15)
                {
                    ret = DMD.DateUtils.MakeDate(ret.Year, ret.Month, 15);
                }
                else
                {
                    ret = DMD.DateUtils.GetNextMonthFirstDay(ret);
                }

                return ret;
            }

            public void RielaboraRinnovabili()
            {
            }

            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheByProposta(Finanziaria.CQSPDConsulenza proposta)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    if (proposta is null)
                        throw new ArgumentNullException("proposta");
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    if (DBUtils.GetID(proposta) == 0)
                        return ret;

                    // If Not (Users.Module.UserCanDoAction("bypess_azienda")) Then cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDConsulenza.Value = DBUtils.GetID(proposta);
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    return ret;
                }
            }

            public Finanziaria.CQSFastStats GetFastStats(Finanziaria.CQSFilter filter)
            {
                return new Finanziaria.CQSFastStats(filter);
            }

            public CCollection<Finanziaria.CPraticaCQSPD> GetPreventivi(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? di, DateTime? df)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoPratica.Value = Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO;
                    // cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                    cursor.Trasferita.Value = false;
                    cursor.Flags.Value = Finanziaria.PraticaFlags.HIDDEN;
                    cursor.Flags.Operator = Databases.OP.OP_NE;
                    if (ufficio is object)
                        cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio);
                    if (operatore is object)
                        cursor.StatoPreventivo.IDOperatore = DBUtils.GetID(operatore);
                    if (di.HasValue || df.HasValue)
                    {
                        cursor.StatoPreventivo.Tipo = "Tra";
                        cursor.StatoPreventivo.Inizio = di;
                        cursor.StatoPreventivo.Fine = df;
                    }

                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    return ret;
                }
            }

            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheCaricate(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? di, DateTime? df)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoPratica.Value = Finanziaria.StatoPraticaEnum.STATO_PRATICA_CARICATA;
                    // cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                    cursor.Trasferita.Value = false;
                    cursor.Flags.Value = Finanziaria.PraticaFlags.HIDDEN;
                    cursor.Flags.Operator = Databases.OP.OP_NE;
                    if (ufficio is object)
                        cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio);
                    if (operatore is object)
                        cursor.StatoPreventivo.IDOperatore = DBUtils.GetID(operatore);
                    if (di.HasValue || df.HasValue)
                    {
                        cursor.StatoPreventivo.Tipo = "Tra";
                        cursor.StatoPreventivo.Inizio = di;
                        cursor.StatoPreventivo.Fine = df;
                    }

                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    return ret;
                }
            }

            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheInCorso(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? dataInizio, DateTime? dataFine)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    var stato = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_PREVENTIVO);
                    var stArch = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA);
                    var stAnn = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ANNULLATA);
                    DateTime? d1;
                    while (stato is object)
                    {
                        if (ReferenceEquals(stato, stArch))
                        {
                        }
                        // Pratica archiviata


                        else if (ReferenceEquals(stato, stAnn))
                        {
                        }
                        // Pratica annullata


                        else if (stato.GiorniStallo.HasValue)
                        {
                            cursor.IgnoreRights = true;
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDStatoAttuale.Value = DBUtils.GetID(stato);
                            cursor.Trasferita.Value = false;
                            cursor.Flags.Value = Finanziaria.PraticaFlags.HIDDEN;
                            cursor.Flags.Operator = Databases.OP.OP_NE;
                            // cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                            // cursor.StatoPreventivo.MacroStato = Nothing
                            cursor.StatoPreventivo.Tipo = "Tra";
                            cursor.StatoPreventivo.Inizio = dataInizio;
                            cursor.StatoPreventivo.Fine = dataFine;
                            if (operatore is object)
                                cursor.StatoPreventivo.IDOperatore = DBUtils.GetID(operatore);
                            if (ufficio is object)
                                cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio);
                            while (!cursor.EOF())
                            {
                                var pratica = cursor.Item;
                                d1 = pratica.StatoDiLavorazioneAttuale.Data;
                                if ((d1.HasValue && Maths.Abs(DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Day, d1.Value, DMD.DateUtils.Now())) > stato.GiorniStallo) == true)
                                {
                                    ret.Add(pratica);
                                }

                                cursor.MoveNext();
                            }
                        }

                        stato = stato.DefaultTarget;
                    }

                    return ret;
                }
            }

            public CCollection<Finanziaria.CPraticaCQSPD> GetPraticheConcluse(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? dataInizio, DateTime? dataFine)
            {
                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    var ret = new CCollection<Finanziaria.CPraticaCQSPD>();
                    var stArch = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ARCHIVIATA);
                    var stAnn = Finanziaria.StatiPratica.GetItemByCompatibleID(Finanziaria.StatoPraticaEnum.STATO_ANNULLATA);
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStatoAttuale.ValueIn(new[] { DBUtils.GetID(stAnn), DBUtils.GetID(stArch) });
                    cursor.Trasferita.Value = false;
                    cursor.Flags.Value = Finanziaria.PraticaFlags.HIDDEN;
                    cursor.Flags.Operator = Databases.OP.OP_NE;
                    // cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                    // cursor.StatoContatto.MacroStato = Nothing
                    // cursor.StatoContatto.Tipo = "Tra"
                    // cursor.StatoContatto.Inizio = dataInizio
                    // cursor.StatoContatto.Fine = dataFine
                    // If (operatore IsNot Nothing) Then cursor.StatoContatto.IDOperatore = GetID(operatore)
                    if (ufficio is object)
                        cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio);
                    while (!cursor.EOF())
                    {
                        Finanziaria.CPraticaCQSPD pratica;
                        pratica = cursor.Item;
                        ret.Add(pratica);
                        cursor.MoveNext();
                    }

                    return ret;
                }
            }

            public override Finanziaria.CPraticaCQSPD GetItemById(int id)
            {
                return base.GetItemById(id);
            }

            /// <summary>
        /// Effettua le statistiche aggregate sul cursore
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CQSPDMLInfo CalcolaSomme(Finanziaria.CPraticheCQSPDCursor cursor)
            {
                var ret = new Finanziaria.CQSPDMLInfo();
                string dbSQL;
                dbSQL = "SELECT Count(*) As [Num], " + "Sum([MontanteLordo]) As [SML], " + "Sum([UpFront]) As [SUpFront], " + "Sum([UpFront]+[Running]) As [SSpread], " + "Sum([Running]) As [SRunning], " + "Sum(iif([ProvvMax] Is Null Or [ProvvMax]<=[UpFront], 0, [ProvvMax]-[UpFront])) As [SSconto], " + "Sum([Rappel]) As [SRappel] " + "FROM (" + cursor.GetSQL() + ")";
                using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL))
                {
                    if (dbRis.Read())
                    {
                        ret.Conteggio = Sistema.Formats.ToInteger(dbRis["Num"]);
                        ret.ML = Sistema.Formats.ToValuta(dbRis["SML"]);
                        ret.UpFront = Sistema.Formats.ToValuta(dbRis["SUpFront"]);
                        ret.Spread = Sistema.Formats.ToValuta(dbRis["SSPread"]);
                        ret.Running = Sistema.Formats.ToValuta(dbRis["SRunning"]);
                        ret.Sconto = Sistema.Formats.ToValuta(dbRis["SSconto"]);
                        ret.Rappel = Sistema.Formats.ToValuta(dbRis["SRappel"]);
                    }

                    return ret;
                }
            }

            public void SyncStatiLav(CCollection<Finanziaria.CPraticaCQSPD> pratiche)
            {
                var buffer = new ArrayList();
                foreach (Finanziaria.CPraticaCQSPD r in pratiche)
                    buffer.Add(DBUtils.GetID(r));
                if (buffer.Count > 0)
                {
                    var col = new CCollection<Finanziaria.CStatoLavorazionePratica>();
                    var cursor = new Finanziaria.CStatiLavorazionePraticaCursor();
                    int[] arrp = (int[])buffer.ToArray(typeof(int));
                    cursor.IDPratica.ValueIn(arrp);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        var stl = cursor.Item;
                        col.Add(stl);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                    foreach (Finanziaria.CPraticaCQSPD r in pratiche)
                    {
                        var statilav = new Finanziaria.CStatiLavorazionePraticaCollection();
                        statilav.SetPratica(r);
                        foreach (var stl in col)
                        {
                            if (stl.IDPratica == DBUtils.GetID(r))
                            {
                                statilav.Add(stl);
                            }
                        }

                        statilav.Sort();
                        r.SetStatiDiLavorazione(statilav);
                        var sta = statilav.GetItemById(r.IDStatoDiLavorazioneAttuale);
                        if (sta is object)
                            r.SetStatoDiLavorazioneAttuale(sta);
                    }
                }
            }

            public void SyncOffertaCorrente(CCollection<Finanziaria.CPraticaCQSPD> pratiche)
            {
                var idList = new ArrayList();
                foreach (Finanziaria.CPraticaCQSPD p in pratiche)
                {
                    if (p.IDOffertaCorrente != 0)
                        idList.Add(p.IDOffertaCorrente);
                }

                if (idList.Count > 0)
                {
                    int[] arr = (int[])idList.ToArray(typeof(int));
                    using (var cursor = new Finanziaria.CCQSPDOfferteCursor())
                    {
                        cursor.ID.ValueIn(arr);
                        cursor.IgnoreRights = true;
                        while (!cursor.EOF())
                        {
                            var o = cursor.Item;
                            foreach (Finanziaria.CPraticaCQSPD p in pratiche)
                            {
                                if (p.IDOffertaCorrente == DBUtils.GetID(o))
                                {
                                    p.SetOffertaCorrente(o);
                                    o.SetPratica(p);
                                }
                            }

                            cursor.MoveNext();
                        }
                    }
                }
            }
        }
    }

    public partial class Finanziaria
    {
        private static CPraticheClass m_Pratiche = null;

        /// <summary>
    /// Accede al modulo delle pratiche
    /// </summary>
    /// <returns></returns>
        public static CPraticheClass Pratiche
        {
            get
            {
                if (m_Pratiche is null)
                    m_Pratiche = new CPraticheClass();
                return m_Pratiche;
            }
        }
    }
}