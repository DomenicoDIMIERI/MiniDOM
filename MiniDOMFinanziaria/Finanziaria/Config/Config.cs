using System;
using minidom;
using minidom.internals;
using static minidom.Databases;

namespace minidom
{
    namespace internals
    {
        [Flags]
        public enum CQSPDConfigFlags : int
        {
            None = 0,

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le richiesta
        /// </summary>
        /// <remarks></remarks>
            NORMALUSER_ALLOWEDITRICHIESTA = 1,

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le proposte
        /// </summary>
        /// <remarks></remarks>
            NORMALUSER_ALLOWEDITPROPOSTA = 2,

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le pratiche
        /// </summary>
        /// <remarks></remarks>
            NORMALUSER_ALLOWEDITPRATICA = 4,

            /// <summary>
        /// Consenti agli utenti che non dispongono del diritto change_status di modificare gli altri prestiti
        /// </summary>
        /// <remarks></remarks>
            NORMALUSRE_ALLOWEDITALTRIPRESTITI = 8,

            /// <summary>
        /// Consente la correzione della decorrenza delle proposte
        /// </summary>
        /// <remarks></remarks>
            NORMALUSER_ALLOWEDITDECORRENZAPROP = 16,

            /// <summary>
        /// Indica al sistema che deve essere effettuata l'archiviazione automatica delle pratiche liquidate dopo N giorni
        /// </summary>
        /// <remarks></remarks>
            ARCHIVIA_AUTOMATICAMENTE = 32,

            /// <summary>
        /// Indica al sistema di utilizzare l'interfaccia multicessionario
        /// </summary>
        /// <remarks></remarks>
            SISTEMA_MULTICESSIONARIO = 64,

            /// <summary>
        /// Consente di proporre al cliente degli studi di fattibilità bypassando il controllo del supervisore.
        /// La proposta sarà comunque inviata al supervisore se lo studio di fattibilità viene convertito in pratica.
        /// </summary>
        /// <remarks></remarks>
            CONSENTI_PROPOSTE_NONAPPROVATE = 256
        }

        /// <summary>
    /// Configurazione del modulo CreditoV
    /// </summary>
    /// <remarks></remarks>
        public class CCQSPDConfig
            : DBObjectBase
        {
            public void DoChanged(string paramName, object newValue, object oldValue)
            {
                throw new NotImplementedException();
            }

            public DateTime? GetValueDate(string paramName, DateTime? defValue)
            {
                return Finanziaria.Module.Settings.GetValueDate(paramName, defValue);
            }

            public void SetValueDate(string paramName, DateTime? value)
            {
                Finanziaria.Module.Settings.SetValueDate(paramName, value);
            }

            public int? GetValueInt(string paramName, int? defValue)
            {
                return Finanziaria.Module.Settings.GetValueInt(paramName, (int)defValue);
            }

            public void SetValueInt(string paramName, int? value)
            {
                Finanziaria.Module.Settings.SetValueInt(paramName, value);
            }

            public double? GetValueDouble(string paramName, int? defValue)
            {
                return Finanziaria.Module.Settings.GetValueDouble(paramName, (double)defValue);
            }

            public void SetValueDouble(string paramName, double? value)
            {
                Finanziaria.Module.Settings.SetValueDouble(paramName, value);
            }

            public string GetValueString(string paramName, string defValue)
            {
                return Finanziaria.Module.Settings.GetValueString(paramName, defValue);
            }

            public void SetValueString(string paramName, string value)
            {
                Finanziaria.Module.Settings.SetValueString(paramName, value);
            }

            public CCQSPDConfig()
            {
            }

            public bool ConsentiProposteSenzaSupervisore
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.CONSENTI_PROPOSTE_NONAPPROVATE);
                }

                set
                {
                    if (ConsentiProposteSenzaSupervisore == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.CONSENTI_PROPOSTE_NONAPPROVATE, value);
                    DoChanged("ConentiProposteSenzaSupervisore", value, !value);
                }
            }



            /// <summary>
        /// Restituisce o imposta un valore booleano che indica al sistema se deve essere utilizzata l'interfaccia per i sistema multicessionari
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool SistemaMulticessionario
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.SISTEMA_MULTICESSIONARIO);
                }

                set
                {
                    if (SistemaMulticessionario == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.SISTEMA_MULTICESSIONARIO, value);
                    DoChanged("SistemaMulticessionario", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultima archiviazione automatica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? UltimaArchiviazione
            {
                get
                {
                    return GetValueDate("UltimaArchiviazione", default);
                }

                set
                {
                    var oldValue = UltimaArchiviazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    SetValueDate("UltimaArchiviazione", value);
                    DoChanged("UltimaArchiviazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve archiviare automaticamente le pratiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool ArchiviaAutomaticamente
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.ARCHIVIA_AUTOMATICAMENTE);
                }

                set
                {
                    if (ArchiviaAutomaticamente == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.ARCHIVIA_AUTOMATICAMENTE, value);
                    DoChanged("ArchiviaAutomaticamente", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di giorni dopo cui archiviare automaticamente la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int GiorniArchiviazione
            {
                get
                {
                    return (int)GetValueInt("GiorniArchiviazione", 120);
                }

                set
                {
                    int oldValue = GiorniArchiviazione;
                    if (oldValue == value)
                        return;
                    SetValueInt("GiorniArchiviazione", value);
                    DoChanged("GiorniArchiviazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta i flags
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CQSPDConfigFlags Flags
            {
                get
                {
                    return (CQSPDConfigFlags)DMD.Integers.ValueOf(Finanziaria.Module.Settings.GetValueInt("Flags", 0));
                }

                set
                {
                    var oldValue = Flags;
                    if (oldValue == value)
                        return;
                    Finanziaria.Module.Settings.SetValueInt("Flags", (int)value);
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le richieste di finanziamento già salvate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool AllowNormalUsersToEditRichieste
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITRICHIESTA);
                }

                set
                {
                    if (AllowNormalUsersToEditRichieste == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITRICHIESTA, value);
                }
            }

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool AllowNormalUsersToEditProposte
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPROPOSTA);
                }

                set
                {
                    if (AllowNormalUsersToEditProposte == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPROPOSTA, value);
                }
            }

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool AllowNormalUsersToEditPratiche
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPRATICA);
                }

                set
                {
                    if (AllowNormalUsersToEditPratiche == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITPRATICA, value);
                }
            }

            /// <summary>
        /// Consente agli utenti che non dispongono del diritto change_status di modificare le proposte già salvate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool AllowNormalUsersToEditAltriPrestiti
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.NORMALUSRE_ALLOWEDITALTRIPRESTITI);
                }

                set
                {
                    if (AllowNormalUsersToEditAltriPrestiti == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.NORMALUSRE_ALLOWEDITALTRIPRESTITI, value);
                }
            }

            /// <summary>
        /// Consente agli utenti normali di modificare la decorrenza delle proposte
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool AllowNormalUsersToCorrectDecorrenzaProposte
            {
                get
                {
                    return Sistema.TestFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITDECORRENZAPROP);
                }

                set
                {
                    if (AllowNormalUsersToCorrectDecorrenzaProposte == value)
                        return;
                    Flags = Sistema.SetFlag(Flags, CQSPDConfigFlags.NORMALUSER_ALLOWEDITDECORRENZAPROP, value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore minimo caricabile per l'UpFront da parte di un utente normale.
        /// Sotto questo valore solo gli operatori con il diritto change_status possono caricare le offerte
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ValoreMinimoUpFront
            {
                get
                {
                    return (decimal)GetValueDouble("ValoreMinimoUpfront", 0);
                }

                set
                {
                    decimal oldValue = ValoreMinimoUpFront;
                    if (oldValue == value)
                        return;
                    SetValueDouble("ValoreMinimoUpfront", (double?)value);
                    DoChanged("ValoreMinimoUpFront", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la soglia sotto la quale viene generata una codinzione di attenzione per la pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal SogliaNotificaUpFront
            {
                get
                {
                    return (decimal)GetValueDouble("SogliaNotificaUpFront", 0);
                }

                set
                {
                    decimal oldValue = SogliaNotificaUpFront;
                    if (oldValue == value)
                        return;
                    SetValueDouble("SogliaNotificaUpFront", (double?)value);
                    DoChanged("SogliaNotificaUpFront", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di giorni da sottrarre alla data in cui un prestito diventa rifinanziabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int GiorniAnticipoRifin
            {
                get
                {
                    return (int)GetValueInt("GiorniAnticipoRifin", 120);
                }

                set
                {
                    int oldValue = GiorniAnticipoRifin; // 
                    if (oldValue == value)
                        return;
                    SetValueInt("GiorniAnticipoRifin", value);
                    DoChanged("GiorniAnticipoRifin", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imosta il tempo (in percentuale rispetto alla durata) in cui un prestito diventa rifinanziabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double PercCompletamentoRifn
            {
                get
                {
                    return (double)GetValueDouble("PercCompletamentoRifn", 40);
                }

                set
                {
                    double oldValue = PercCompletamentoRifn;
                    if (oldValue == value)
                        return;
                    SetValueDouble("PercCompletamentoRifn", value);
                    DoChanged("PercCompletamentoRifn", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dello stato predefinito (quando viene caricata una nuova pratica)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDStatoPredefinito
            {
                get
                {
                    return (int)GetValueInt("IDStatoPredefinito", 0);
                }

                set
                {
                    int oldValue = IDStatoPredefinito;
                    if (oldValue == value)
                        return; // If (GetID(Me.m_StatoPredefinito) = value) Then Exit Property
                    SetValueInt("IDStatoPredefinito", value);
                    DoChanged("IDStatoPredefinito", value, oldValue);
                }
            }

            /// <summary>
        /// Restituiscec o imposta lo stato predefinito per le nuove pratiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CStatoPratica StatoPredefinito
            {
                get
                {
                    return Finanziaria.StatiPratica.GetItemById(IDStatoPredefinito);
                }

                set
                {
                    IDStatoPredefinito = DBUtils.GetID(value);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'indirizzo da cui vengono inviate le email provenienti da questo modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string BCC
            {
                get
                {
                    return GetValueString("BCC", "");
                }

                set
                {
                    string oldValue = BCC;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("BCC", value);
                    DoChanged("BCC", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'indirizzo da cui vengono inviate le email provenienti da questo modulo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string FromAddress
            {
                get
                {
                    return GetValueString("FromAddress", "");
                }

                set
                {
                    string oldValue = FromAddress;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("FromAddress", value);
                    DoChanged("FromAddress", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome visualizzato come mittente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string FromDisplayName
            {
                get
                {
                    return GetValueString("FromDisplayName", "");
                }

                set
                {
                    string oldValue = FromDisplayName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("FromDisplayName", value);
                    DoChanged("FromDisplayName", value, oldValue);
                }
            }


            /// <summary>
        /// Elenco di indirizzi separati da "," a cui inviare gli avvisi generati per le pratiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NotifyChangesTo
            {
                get
                {
                    return GetValueString("NotifyChangesTo", "");
                }

                set
                {
                    string oldValue = NotifyChangesTo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("NotifyChangesTo", value);
                    DoChanged("NotifyChangesTo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail che viene inviata per notificare le condizioni di avviso
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string WatchSubject
            {
                get
                {
                    return GetValueString("WatchSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = WatchSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("WatchSubject", value);
                    DoChanged("WatchSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il modello utilizzato per le email inviate per notificare delle condizioni di attenzione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string WatchTemplate
            {
                get
                {
                    return GetValueString("WatchTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = WatchTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("WatchTemplate", value);
                    DoChanged("WatchTemplat", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare l'inserimento di una nuova richiesta di finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string RichiestaFinanziamentoSubject
            {
                get
                {
                    return GetValueString("RichiestaFinanziamentoSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = RichiestaFinanziamentoSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("RichiestaFinanziamentoSubject", value);
                    DoChanged("RichiestaFinanziamentoSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail utilizzata per notificare l'inserimento di una nuova richiesta di finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string RichiestaFinanziamentoTemplate
            {
                get
                {
                    return GetValueString("RichiestaFinanziamentoTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = RichiestaFinanziamentoTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("RichiestaFinanziamentoTemplate", value);
                    DoChanged("RichiestaFinanziamentoTemplate", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta il soggetto della mail che viene inviata per notificare l'eliminazione di una pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DeleteSubject
            {
                get
                {
                    return GetValueString("DeleteSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = DeleteSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("DeleteSubject", value);
                    DoChanged("DeleteSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del modello di mail utilizzato per notificare l'eliminazione di una pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DeleteTemplate
            {
                get
                {
                    return GetValueString("DeleteTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = DeleteTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("DeleteTemplate", value);
                    DoChanged("DeleteTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'elenco degli indirizzi email a cui notificare le modifiche fatte alle pratiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NotifyWarningsTo
            {
                get
                {
                    return GetValueString("NotifyChangesTo", "");
                }

                set
                {
                    string oldValue = NotifyChangesTo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("NotifyChangesTo", value);
                    DoChanged("NotifyWarningsTo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'elenco degli indirizzi email a cui inviare le richieste di approvazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NotifyRichiesteApprovazioneTo
            {
                get
                {
                    return GetValueString("NotifyRichiesteApprovazioneTo", "");
                }

                set
                {
                    string oldValue = NotifyRichiesteApprovazioneTo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("NotifyRichiesteApprovazioneTo", value);
                    DoChanged("NotifyRichiesteApprovazioneTo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del template della mail inviata per notificare le richieste di approvazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TemplateRichiesteApprovazione
            {
                get
                {
                    return GetValueString("TemplateRichiesteApprovazione", "");
                }

                set
                {
                    string oldValue = TemplateRichiesteApprovazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("TemplateRichiesteApprovazione", value);
                    DoChanged("TemplateRichiesteApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del template della mail che viene inviata per notificare l'approvazione di una pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TemplateConfermaApprovazione
            {
                get
                {
                    return GetValueString("TemplateConfermaApprovazione", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = TemplateConfermaApprovazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("TemplateConfermaApprovazione", value);
                    DoChanged("TemplateConfermaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del template usato per le email inviate per notificare la mancata approvazione di una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TemplateNegaApprovazione
            {
                get
                {
                    return GetValueString("TemplateNegaApprovazione", "");
                }

                set
                {
                    string oldValue = TemplateNegaApprovazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("TemplateNegaApprovazione", value);
                    DoChanged("TemplateNegaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del template utilizzato per le email inviate per notificare la modifica di una pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TemplateChangeStatus
            {
                get
                {
                    return GetValueString("TemplateChangeStatus", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = TemplateChangeStatus;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("TemplateChangeStatus", value);
                    DoChanged("TemplateChangeStatus", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare le modifiche fatte alle pratiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ChangeStatusSubject
            {
                get
                {
                    return GetValueString("ChangeStatusSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ChangeStatusSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ChangeStatusSubject", value);
                    DoChanged("ChangeStatusSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per le richieste di approvazione di una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ReqApprSubject
            {
                get
                {
                    return GetValueString("ReqApprSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ReqApprSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ReqApprSubject", value);
                    DoChanged("ReqApprSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o impsota il soggetto delle mail inviate per notificare l'approvazione di una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ApprSubject
            {
                get
                {
                    return GetValueString("ApprSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ApprSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ApprSubject", value);
                    DoChanged("ApprSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per negare una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NegaSubject
            {
                get
                {
                    return GetValueString("NegaSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = NegaSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("NegaSubject", value);
                    DoChanged("NegaSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare le proposte caricate sotto la soglia minima dell'UpFront
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NotificaSogliaSubject
            {
                get
                {
                    return GetValueString("NotificaSogliaSubject", "");
                }

                set
                {
                    string oldValue = NotificaSogliaSubject;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("NotificaSogliaSubject", value);
                    DoChanged("NotificaSogliaSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del templete utilizzato per le email inviate per notificare una proposta caricata sotto la soglia minima dell'UpFront
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NotificaSogliaTemplate
            {
                get
                {
                    return GetValueString("NotificaSogliaTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = NotificaSogliaTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("NotificaSogliaTemplate", value);
                    DoChanged("NotificaSogliaTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare la creazione di una nuova pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PraticaCreataSubject
            {
                get
                {
                    return GetValueString("PraticaCreataSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = PraticaCreataSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("PraticaCreataSubject", value);
                    DoChanged("PraticaCreataSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare la creazione di una nuova pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PraticaCreataTemplate
            {
                get
                {
                    return GetValueString("PraticaCreataTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = PraticaCreataTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("PraticaCreataSubject", value);
                    DoChanged("PraticaCreataTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare l'annullamento di una pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PraticaAnnullataSubject
            {
                get
                {
                    return GetValueString("PraticaAnnullataSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = PraticaAnnullataSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("PraticaAnnullataSubject", value);
                    DoChanged("PraticaAnnullataSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare l'annullamento di una nuova pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PraticaAnnullataTemplate
            {
                get
                {
                    return GetValueString("PraticaAnnullataTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = PraticaAnnullataTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("PraticaAnnullataSubject", value);
                    DoChanged("PraticaAnnullataTemplate", value, oldValue);
                }
            }

            // Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            // Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

            // Me.Overflow.SetValueString("ConsulenzaRichiestaApprovazioneSubject", Me.m_ConsulenzaRichiestaApprovazioneSubject)
            // Me.Overflow.SetValueString("ConsulenzaRichiestaApprovazioneTemplate", Me.m_ConsulenzaRichiestaApprovazioneTemplate)
            // Me.Overflow.SetValueString("ConsulenzaConfermaApprovazioneSubject", Me.m_ConsulenzaConfermaApprovazioneSubject)
            // Me.Overflow.SetValueString("ConsulenzaConfermaApprovazioneTemplate", Me.m_ConsulenzaConfermaApprovazioneTemplate)
            // Me.Overflow.SetValueString("ConsulenzaNegaApprovazioneSubject", Me.m_ConsulenzaNegaApprovazioneSubject)
            // Me.Overflow.SetValueString("ConsulenzaNegaApprovazioneTemplate", Me.m_ConsulenzaNegaApprovazioneTemplate)

            // If (ret) Then
            // 'Finanziaria.Pratiche.Module.Settings.Save(True)
            // SetConfiguration(Me)
            // End If
            // Return ret
            // End Function

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare l'inserimento di un nuovo studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaInseritaSubject
            {
                get
                {
                    return GetValueString("ConsulenzaInseritaSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaInseritaSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaInseritaSubject", value);
                    DoChanged("ConsulenzaInseritaSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il percorso del modello della mail utilizzata per notificare l'inserimento di un nuovo studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaInseritaTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaInseritaTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaInseritaTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaInseritaTemplate", value);
                    DoChanged("ConsulenzaInseritaTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata quando un cliente accetta uno studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaAccettataSubject
            {
                get
                {
                    return GetValueString("ConsulenzaAccettataSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaAccettataSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaAccettataSubject", value);
                    DoChanged("ConsulenzaAccettataSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata quando un cliente accetta uno studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaAccettataTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaAccettataTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaAccettataTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaAccettataTemplate", value);
                    DoChanged("ConsulenzaAccettataTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata quando un operatore boccia lo studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaBocciataSubject
            {
                get
                {
                    return GetValueString("ConsulenzaBocciataSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaBocciataSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaBocciataSubject", value);
                    DoChanged("ConsulenzaBocciataSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata quando un operatore boccia uno studio di fattibilità
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaBocciataTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaBocciataTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaBocciataTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaBocciataTemplate", value);
                    DoChanged("ConsulenzaBocciataTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per notificare che un operatore ha proposto uno studio di fattibilità ad un cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaPropostaSubject
            {
                get
                {
                    return GetValueString("ConsulenzaPropostaSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaPropostaSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaPropostaSubject", value);
                    DoChanged("ConsulenzaPropostaSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata per notificare che un operatore ha proposto uno studio di fattibilità ad un cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaPropostaTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaPropostaTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaPropostaTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaPropostaTemplate", value);
                    DoChanged("ConsulenzaPropostaTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto della mail inviata per noficiare che un cliente ha rifiutato uno studio di fattibilità proposto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaRifiutataSubject
            {
                get
                {
                    return GetValueString("ConsulenzaRifiutataSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaRifiutataSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaRifiutataSubject", value);
                    DoChanged("ConsulenzaRifiutataSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata per notificare che un cliente ha rifiutato uno studio di fattibilità proposto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaRifiutataTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaRifiutataTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaRifiutataTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaRifiutataTemplate", value);
                    DoChanged("ConsulenzaRifiutataTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto delle mail inviate per notificare le richieste di approvazione per una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaRichiestaApprovazioneSubject
            {
                get
                {
                    return GetValueString("ConsulenzaRichiestaApprovazioneSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaRichiestaApprovazioneSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaRichiestaApprovazioneSubject", value);
                    DoChanged("ConsulenzaRichiestaApprovazioneSubject", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata per notificare le richieste di approvazione di una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaRichiestaApprovazioneTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaRichiestaApprovazioneTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaRichiestaApprovazioneTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaRichiestaApprovazioneTemplate", value);
                    DoChanged("ConsulenzaRichiestaApprovazioneTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto delle mail inviate per notificare le autorizzazioni delle richieste di approvazione per una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaConfermaApprovazioneSubject
            {
                get
                {
                    return GetValueString("ConsulenzaConfermaApprovazioneSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaConfermaApprovazioneSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaConfermaApprovazioneSubject", value);
                    DoChanged("ConsulenzaConfermaApprovazioneSubject ", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata per notificare le autorizzazioni delle  richieste di approvazione di una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaConfermaApprovazioneTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaConfermaApprovazioneTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaConfermaApprovazioneTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaConfermaApprovazioneTemplate", value);
                    DoChanged("ConsulenzaConfermaApprovazioneTemplate", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il soggetto delle mail inviate per notificare le negazioni delle richieste di approvazione per una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaNegaApprovazioneSubject
            {
                get
                {
                    return GetValueString("ConsulenzaNegaApprovazioneSubject", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaNegaApprovazioneSubject;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaNegaApprovazioneSubject", value);
                    DoChanged("ConsulenzaNegaApprovazioneSubject ", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il template della mail inviata per notificare le negazioni delle  richieste di approvazione di una proposta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ConsulenzaNegaApprovazioneTemplate
            {
                get
                {
                    return GetValueString("ConsulenzaNegaApprovazioneTemplate", "");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ConsulenzaNegaApprovazioneTemplate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    SetValueString("ConsulenzaNegaApprovazioneTemplate", value);
                    DoChanged("ConsulenzaNegaApprovazioneTemplate", value, oldValue);
                }
            }

            public override string ToString()
            {
                return "Configurazione Modulo Credito V";
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            protected override CDBConnection GetConnection()
            {
                return Finanziaria.Database;
            }

            public override string GetTableName()
            {
                return "";
            }
        }
    }

    public partial class Finanziaria
    {
        private static CCQSPDConfig m_Config = null;

        /// <summary>
    /// Oggetto che contiene alcuni parametri relativi alla configurazione del modulo
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public static CCQSPDConfig Configuration
        {
            get
            {
                if (m_Config is null)
                    m_Config = new CCQSPDConfig();
                return m_Config;
            }
        }
    }
}