using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoProduttore : int
        {
            STATO_ATTIVO = Sistema.UserStatus.USER_ENABLED,
            STATO_DISABILITATO = Sistema.UserStatus.USER_DISABLED,
            STATO_ELIMINATO = Sistema.UserStatus.USER_DELETED,
            STATO_INATTIVAZIONE = Sistema.UserStatus.USER_NEW,
            STATO_SOSPESO = Sistema.UserStatus.USER_SUSPENDED
            // STATO_INVALID = UserStatus.USER_TEMP
        }

        [Serializable]
        public class CCollaboratore 
            : Databases.DBObjectPO, 
              IFonte, 
              ICloneable
        {
            
            [NonSerialized] private Sistema.CUser m_User;
            private int m_UserID;
            private string m_NomeUtente;
            private int m_PersonaID;
            [NonSerialized] private Anagrafica.CPersona m_Persona;
            private string m_NomePersona;
            private string m_Rapporto;
            private bool m_Attivo;
            private DateTime? m_DataInizioRapporto;
            private DateTime? m_DataFineRapporto;
            private string m_Tipo;
            private int m_ReferenteID;
            [NonSerialized] private CTeamManager m_Referente;
            private int m_AttivatoDaID;
            [NonSerialized] private Sistema.CUser m_AttivatoDa;
            private DateTime? m_DataAttivazione;
            [NonSerialized] private CTrattativeCollaboratore m_Trattative;
            private int m_ListinoPredefinitoID;
            [NonSerialized] private CProfilo m_ListinoPredefinito;
            // Private  As CPratichePerCollabCollection

            private string m_NumeroIscrizioneUIC;
            private DateTime? m_DataIscrizioneUIC;
            private string m_TipoIscrizioneUIC;
            private DateTime? m_DataRinnovoUIC;
            private DateTime? m_DataVisuraProtesti;
            private int m_EsitoVisuraProtesti;
            private string m_NumeroIscrizioneRUI;
            private DateTime? m_DataIscrizioneRUI;
            private DateTime? m_DataRinnovoRUI;
            private string m_NumeroIscrizioneISVAP;
            private DateTime? m_DataIscrizioneISVAP;
            private DateTime? m_DataRinnovoISVAP;

            // Private m_ProdottiDistribuiti
            // Private m_Acconti

            // Private m_Commercializza_cqs_delega
            // Private m_Commercializza_cqs_delega_con_chi
            // Private m_Commercializza_cqs_delega_con_acconti
            // Private m_Netto_erogato_per_pratica
            // Private m_Teg_per_pratica_60_mesi
            // Private m_Teg_per_pratica_120_mesi
            // Private m_Tempi_erogazione_gg
            // Private m_notifica_pratica_a_cura_CQSPD
            // Private m_notifica_pratica_a_cura_broker
            // Private m_procura
            // Private m_collegamento_per_preventivi_IP_dinamico
            // Private m_collegamento_per_preventivi_IP_statico
            // Private m_Tempi_liquidazione_per_tipo_prodotto_gg
            // Private m_Distribuzione_produzione_Statale_Perc
            // Private m_Distribuzione_produzione_Pubblico_Perc
            // Private m_Distribuzione_produzione_Privato_Perc
            // Private m_Distribuzione_produzione_Pensionato_Perc
            // Private m_Fatturato_montante_lordo_mensile
            // Private m_Fatturato_montante_lordo_annuo
            // Private m_Max_provvigionale_caricato
            // Private m_Note_Provvigionale_Max
            // Private m_Proposte
            // Private m_MAIL_referente
            // Private m_Tipologia_Med_A
            // Private m_Tipologia_Med_B
            // Private m_Tipologia_Med_C
            // Private m_SCHEDA
            // Private m_Esito
            // Private m_Raccolta_Preventivi_competitor
            // Private m_Allegato_preventivo_competitor
            // Private m_Stampa_Preventivi_finsab_confronto
            // Private m_Allegato_preventivi_finsab
            // Private m_Riepilogo_esigenze_mediatore
            // Private m_Spread_finsab_da_assegnare_conv_inpdap
            // Private m_Provv_Mediatore_da_assegnare_conv_inpdap
            // Private m_Spread_finsab_da_assegnare_ordinario
            // Private m_Provv_Mediatore_da_assegnare_ordinario
            // Private m_ProfiloFuturo

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCollaboratore()
            {
                m_PersonaID = 0;
                m_Persona = null;
                m_NomePersona = Strings.vbNullString;
                m_UserID = 0;
                m_User = null;
                m_NomeUtente = Strings.vbNullString;
                m_ReferenteID = 0;
                m_Referente = null;
                m_Trattative = null;
                m_ListinoPredefinitoID = 0;
                m_ListinoPredefinito = null;
                m_Attivo = true;
                // Me.m_BUIs = Nothing
                // Me.m_Acconti = False
                // Me.m_Commercializza_cqs_delega = False
                // Me.m_Commercializza_cqs_delega_con_acconti = False
                // Me.m_notifica_pratica_a_cura_CQSPD = False
                // Me.m_notifica_pratica_a_cura_broker = False
                // Me.m_procura = False
                // Me.m_collegamento_per_preventivi_IP_dinamico = False
                // Me.m_collegamento_per_preventivi_IP_statico = False
                // Me.m_Tipologia_Med_A = False
                // Me.m_Tipologia_Med_B = False
                // Me.m_Tipologia_Med_C = False
                // Me.m_Raccolta_Preventivi_competitor = False
                // Me.m_Stampa_Preventivi_finsab_confronto = False
                // Me.m_Pratiche = Nothing
                m_AttivatoDa = null;
            }

            /// <summary>
            /// Restituisce un riferimento al modulo  Collaboratori
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return Finanziaria.Collaboratori.Module;
            }

            /// <summary>
            /// Restituisce vero se il collaboratore è attivo alla data specifica
            /// </summary>
            /// <param name="atDate"></param>
            /// <returns></returns>
            public bool IsValid(DateTime atDate)
            {
                return DMD.DateUtils.CheckBetween(atDate, m_DataInizioRapporto, m_DataFineRapporto);
            }

            /// <summary>
            /// Restituisce vero se il collaboratore è attivo alla data odierna
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DateTime.Now);
            }



            // Public ReadOnly Property Pratiche As CPratichePerCollabCollection
            // Get
            // If (Me.m_Pratiche Is Nothing) Then
            // Me.m_Pratiche = New CPratichePerCollabCollection
            // Me.m_Pratiche.Initialize(Me)
            // End If
            // Return Me.m_Pratiche
            // End Get
            // End Property

            /// <summary>
            /// Restituisce o importa un riferimento al listino predefinito
            /// </summary>
            public CProfilo ListinoPredefinito
            {
                get
                {
                    if (m_ListinoPredefinito is null)
                        m_ListinoPredefinito = Profili.GetItemById(m_ListinoPredefinitoID);
                    return m_ListinoPredefinito;
                }

                set
                {
                    var oldValue = ListinoPredefinito;
                    if (oldValue == value)
                        return;
                    m_ListinoPredefinito = value;
                    m_ListinoPredefinitoID = DBUtils.GetID(value);
                    DoChanged("ListinoPredefinito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del listino predefinito
            /// </summary>
            public int ListinoPredefinitoID
            {
                get
                {
                    return DBUtils.GetID(m_ListinoPredefinito, m_ListinoPredefinitoID);
                }

                set
                {
                    int oldValue = ListinoPredefinitoID;
                    if (oldValue == value)
                        return;
                    m_ListinoPredefinitoID = value;
                    m_ListinoPredefinito = null;
                    DoChanged("ListinoPredefinitoID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultima visura protesti
            /// </summary>
            public DateTime? DataVisuraProtesti
            {
                get
                {
                    return m_DataVisuraProtesti;
                }

                set
                {
                    var oldValue = m_DataVisuraProtesti;
                    if (oldValue == value == true)
                        return;
                    m_DataVisuraProtesti = value;
                    DoChanged("DataVisuraProtesti", value, oldValue);
                }
            }

            protected internal void InvalidateTrattative()
            {
                m_Trattative = null;
            }

            public int EsitoVisuraProtesti
            {
                get
                {
                    return m_EsitoVisuraProtesti;
                }

                set
                {
                    int oldValue = m_EsitoVisuraProtesti;
                    if (oldValue == value)
                        return;
                    m_EsitoVisuraProtesti = value;
                    DoChanged("EsitoVisuraProtesti", value, oldValue);
                }
            }

            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_PersonaID);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_PersonaID = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_PersonaID);
                    return m_Persona;
                }

                set
                {
                    var oldValue = Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_PersonaID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            public CTeamManager Referente
            {
                get
                {
                    if (m_Referente is null)
                        m_Referente = TeamManagers.GetItemById(m_ReferenteID);
                    return m_Referente;
                }

                set
                {
                    var oldValue = Referente;
                    if (oldValue == value)
                        return;
                    m_Referente = value;
                    m_ReferenteID = DBUtils.GetID(value);
                    DoChanged("Referente", value, oldValue);
                }
            }

            public int ReferenteID
            {
                get
                {
                    return DBUtils.GetID(m_Referente, m_ReferenteID);
                }

                set
                {
                    int oldValue = ReferenteID;
                    if (oldValue == value)
                        return;
                    m_ReferenteID = value;
                    m_Referente = null;
                    DoChanged("ReferenteID", value, oldValue);
                }
            }

            public Sistema.CUser AttivatoDa
            {
                get
                {
                    if (m_AttivatoDa is null)
                        m_AttivatoDa = Sistema.Users.GetItemById(m_AttivatoDaID);
                    return m_AttivatoDa;
                }

                set
                {
                    var oldValue = AttivatoDa;
                    if (oldValue == value)
                        return;
                    m_AttivatoDa = value;
                    m_AttivatoDaID = DBUtils.GetID(value);
                    DoChanged("AttivatoDa", value, oldValue);
                }
            }

            public int AttivatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_AttivatoDa, m_AttivatoDaID);
                }

                set
                {
                    int oldValue = AttivatoDaID;
                    if (oldValue == value)
                        return;
                    m_AttivatoDaID = value;
                    m_AttivatoDa = null;
                    DoChanged("AttivatoDaID", value, oldValue);
                }
            }

            public DateTime? DataAttivazione
            {
                get
                {
                    return m_DataAttivazione;
                }

                set
                {
                    var oldValue = m_DataAttivazione;
                    if (oldValue == value == true)
                        return;
                    m_DataAttivazione = value;
                    DoChanged("DataAttivazione", value, oldValue);
                }
            }

            public int UserID
            {
                get
                {
                    return DBUtils.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_UserID = value;
                    m_User = null;
                    DoChanged("UserID", value, oldValue);
                }
            }

            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (oldValue == value)
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeUtente = value.Nominativo;
                    DoChanged("User", value, oldValue);
                }
            }

            public string NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeUtente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUtente = value;
                    DoChanged("NomeUtente", value, oldValue);
                }
            }

            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            public CTrattativeCollaboratore Trattative
            {
                get
                {
                    if (m_Trattative is null)
                        m_Trattative = new CTrattativeCollaboratore(this);
                    return m_Trattative;
                }
            }

            public string Rapporto
            {
                get
                {
                    return m_Rapporto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Rapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Rapporto = value;
                    DoChanged("Rapporto", value, oldValue);
                }
            }

            public DateTime? DataInizioRapporto
            {
                get
                {
                    return m_DataInizioRapporto;
                }

                set
                {
                    var oldValue = m_DataInizioRapporto;
                    if (oldValue == value == true)
                        return;
                    m_DataInizioRapporto = value;
                    DoChanged("DataInizioRapporto", value, oldValue);
                }
            }

            public DateTime? DataFineRapporto
            {
                get
                {
                    return m_DataFineRapporto;
                }

                set
                {
                    var oldValue = m_DataFineRapporto;
                    if (oldValue == value == true)
                        return;
                    m_DataFineRapporto = value;
                    DoChanged("DataFineRapporto", value, oldValue);
                }
            }

            public DateTime? DataIscrizioneUIC
            {
                get
                {
                    return m_DataIscrizioneUIC;
                }

                set
                {
                    var oldValue = m_DataIscrizioneUIC;
                    if (oldValue == value == true)
                        return;
                    m_DataIscrizioneUIC = value;
                    DoChanged("DataIscrizioneUIC", value, oldValue);
                }
            }

            public string NumeroIscrizioneUIC
            {
                get
                {
                    return m_NumeroIscrizioneUIC;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NumeroIscrizioneUIC;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroIscrizioneUIC = value;
                    DoChanged("NumeroIscrizioneUIC", value, oldValue);
                }
            }

            public DateTime? DataIscrizioneRUI
            {
                get
                {
                    return m_DataIscrizioneRUI;
                }

                set
                {
                    var oldValue = m_DataIscrizioneRUI;
                    if (oldValue == value == true)
                        return;
                    m_DataIscrizioneRUI = value;
                    DoChanged("DataIscrizioneRUI", value, oldValue);
                }
            }

            public string NumeroIscrizioneRUI
            {
                get
                {
                    return m_NumeroIscrizioneRUI;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NumeroIscrizioneRUI;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroIscrizioneRUI = value;
                    DoChanged("NumeroIscrizioneUIC", value, oldValue);
                }
            }

            public DateTime? DataRinnovoRUI
            {
                get
                {
                    return m_DataRinnovoRUI;
                }

                set
                {
                    var oldValue = m_DataRinnovoRUI;
                    if (oldValue == value == true)
                        return;
                    m_DataRinnovoRUI = value;
                    DoChanged("DataRinnovoRUI", value, oldValue);
                }
            }

            public string NumeroIscrizioneISVAP
            {
                get
                {
                    return m_NumeroIscrizioneISVAP;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NumeroIscrizioneISVAP;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroIscrizioneISVAP = value;
                    DoChanged("NumeroIscrizioneISVAP", value, oldValue);
                }
            }

            public DateTime? DataIscrizioneISVAP
            {
                get
                {
                    return m_DataIscrizioneISVAP;
                }

                set
                {
                    var oldValue = m_DataIscrizioneISVAP;
                    if (oldValue == value == true)
                        return;
                    m_DataIscrizioneISVAP = value;
                    DoChanged("DataIscrizioneISVAP", value, oldValue);
                }
            }

            public DateTime? DataRinnovoISVAP
            {
                get
                {
                    return m_DataRinnovoISVAP;
                }

                set
                {
                    var oldValue = m_DataRinnovoISVAP;
                    if (oldValue == value == true)
                        return;
                    m_DataRinnovoISVAP = value;
                    DoChanged("DataRinnovoISVAP", value, oldValue);
                }
            }

            public string TipoIscrizioneUIC
            {
                get
                {
                    return m_TipoIscrizioneUIC;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TipoIscrizioneUIC;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoIscrizioneUIC = value;
                    DoChanged("TipoIscrizioneUIC", value, oldValue);
                }
            }

            public DateTime? DataRinnovoUIC
            {
                get
                {
                    return m_DataRinnovoUIC;
                }

                set
                {
                    var oldValue = m_DataRinnovoUIC;
                    if (oldValue == value == true)
                        return;
                    m_DataRinnovoUIC = value;
                    DoChanged("DataRinnovoUIC", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_Collaboratori";
            }

            private Sistema.CGroup EnsureGroup(string grpName)
            {
                var grp = Sistema.Groups.GetItemByName(grpName);
                if (grp is null)
                {
                    grp = new Sistema.CGroup(grpName);
                }

                grp.Stato = ObjectStatus.OBJECT_VALID;
                grp.Save();
                return grp;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                Sistema.CGroup grp;
                bool ret;
                if (m_User is object)
                    m_User.Save(force);
                if (m_ListinoPredefinito is object)
                    m_ListinoPredefinito.Save(force);
                if (m_Persona is object)
                {
                    m_Persona.Stato = ObjectStatus.OBJECT_VALID;
                    m_Persona.Save();
                }

                ret = base.SaveToDatabase(dbConn, force);
                if (m_Trattative is object)
                    m_Trattative.Save(force);
                // If Not (Me.m_Pratiche Is Nothing) Then Me.m_Pratiche.Save(force)

                // Assicuriamoci che l'oggetto appartenga al gruppo corretto
                if (UserID != 0)
                {
                    switch (Strings.LCase(m_Tipo) ?? "")
                    {
                        case "mediatore":
                            {
                                grp = EnsureGroup("Collaboratori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Agenti");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Segnalatori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Mediatori");
                                if (!grp.Members.Contains(UserID))
                                    grp.Members.Add(UserID);
                                break;
                            }

                        case "agente":
                            {
                                grp = EnsureGroup("Collaboratori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Mediatori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Segnalatori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Agenti");
                                if (!grp.Members.Contains(UserID))
                                    grp.Members.Add(UserID);
                                break;
                            }

                        case "segnalatore":
                            {
                                grp = EnsureGroup("Collaboratori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Mediatori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Agenti");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Segnalatori");
                                if (!grp.Members.Contains(UserID))
                                    grp.Members.Add(UserID);
                                break;
                            }

                        case "collaboratore":
                            {
                                grp = EnsureGroup("Mediatori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Agenti");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Segnalatori");
                                if (grp.Members.Contains(UserID))
                                    grp.Members.Remove(UserID);
                                grp = EnsureGroup("Collaboratori");
                                if (!grp.Members.Contains(UserID))
                                    grp.Members.Add(UserID);
                                break;
                            }

                        default:
                            {
                                throw new ArgumentException("Tipo collaboratore non supportato: " + m_Tipo);
                                break;
                            }
                    }

                    grp = Sistema.Groups.GetItemByName("Users");
                    if (!grp.Members.Contains(UserID))
                    {
                        grp.Members.Add(UserID);
                    }
                }

                Anagrafica.CFonte fonte;
                // Dim canale As CCanale

                switch (Strings.LCase(m_Tipo) ?? "")
                {
                    case "mediatore":
                        {
                            fonte = (Anagrafica.CFonte)Anagrafica.Fonti.GetItemByName("Mediatori", "Mediatori", NomePersona);
                            break;
                        }

                    case "agente":
                        {
                            fonte = (Anagrafica.CFonte)Anagrafica.Fonti.GetItemByName("Agenti", "Agenti", NomePersona);
                            break;
                        }

                    case "segnalatore":
                        {
                            fonte = (Anagrafica.CFonte)Anagrafica.Fonti.GetItemByName("Segnalatori", "Segnalatori", NomePersona);
                            break;
                        }

                    case "collaboratore":
                        {
                            fonte = (Anagrafica.CFonte)Anagrafica.Fonti.GetItemByName("Collaboratori", "Collaboratori", NomePersona);
                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Tipo collaboratore non supportato: " + m_Tipo);
                            break;
                        }
                }

                if (fonte is null)
                    fonte = new Anagrafica.CFonte();
                fonte.Nome = NomePersona;
                fonte.Tipo = Tipo;
                fonte.Stato = ObjectStatus.OBJECT_VALID;
                fonte.IconURL = ((IFonte)this).IconURL;
                fonte.Save();
                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_UserID = reader.Read("Utente", this.m_UserID);
                m_NomeUtente = reader.Read("NomeUtente", this.m_NomeUtente);
                m_PersonaID = reader.Read("Persona", this.m_PersonaID);
                m_NomePersona = reader.Read("NomePersona", this.m_NomePersona);
                m_DataIscrizioneUIC = reader.Read("DataIscrizioneUIC", this.m_DataIscrizioneUIC);
                m_NumeroIscrizioneUIC = reader.Read("NumeroIscrizioneUIC", this.m_NumeroIscrizioneUIC);
                m_TipoIscrizioneUIC = reader.Read("TipoIscrizioneUIC", this.m_TipoIscrizioneUIC);
                m_DataRinnovoUIC = reader.Read("DataRinnovoUIC", this.m_DataRinnovoUIC);
                m_NumeroIscrizioneRUI = reader.Read("NumeroIscrizioneRUI", this.m_NumeroIscrizioneRUI);
                m_DataIscrizioneRUI = reader.Read("DataIscrizioneRUI", this.m_DataIscrizioneRUI);
                m_DataRinnovoRUI = reader.Read("DataRinnovoRUI", this.m_DataRinnovoRUI);
                m_NumeroIscrizioneISVAP = reader.Read("NumeroIscrizioneISVAP", this.m_NumeroIscrizioneISVAP);
                m_DataIscrizioneISVAP = reader.Read("DataIscrizioneISVAP", this.m_DataIscrizioneISVAP);
                m_DataRinnovoISVAP = reader.Read("DataRinnovoISVAP", this.m_DataRinnovoISVAP);
                m_DataVisuraProtesti = reader.Read("DataVisuraProtesti", this.m_DataVisuraProtesti);
                m_EsitoVisuraProtesti = reader.Read("EsitoVisuraProtesti", this.m_EsitoVisuraProtesti);
                m_AttivatoDaID = reader.Read("AttivatoDa", this.m_AttivatoDaID);
                m_DataAttivazione = reader.Read("DataAttivazione", this.m_DataAttivazione);
                m_Tipo = reader.Read("Tipo", this.m_Tipo);
                m_ReferenteID = reader.Read("Referente", this.m_ReferenteID);
                m_ListinoPredefinitoID = reader.Read("ListinoPredefinito", this.m_ListinoPredefinitoID);
                m_Rapporto = reader.Read("Rapporto", this.m_Rapporto);
                m_DataInizioRapporto = reader.Read("DataInizioRapporto", this.m_DataInizioRapporto);
                m_DataFineRapporto = reader.Read("DataFineRapporto", this.m_DataFineRapporto);

                // m_Commercializza_cqs_delega = dbRis ("Commercializza_cqs_delega")
                // m_Commercializza_cqs_delega_con_chi = dbRis ("Commercializza_cqs_delega_con_chi")
                // m_Commercializza_cqs_delega_con_acconti = dbRis ("Commercializza_cqs_delega_con_acconti")
                // m_Netto_erogato_per_pratica = dbRis ("Netto_erogato_per_pratica")
                // m_Teg_per_pratica_60_mesi = dbRis ("Teg_per_pratica_60_mesi")
                // m_Teg_per_pratica_120_mesi = dbRis ("Teg_per_pratica_120_mesi")
                // m_Tempi_erogazione_gg = dbRis ("Tempi_erogazione_gg")
                // m_notifica_pratica_a_cura_CQSPD = dbRis ("notifica_pratica_a_cura_CQSPD")
                // m_notifica_pratica_a_cura_broker = dbRis ("notifica_pratica_a_cura_broker")
                // m_procura = dbRis ("procura")
                // m_collegamento_per_preventivi_IP_dinamico = dbRis ("collegamento_per_preventivi_IP_dinamico")
                // m_collegamento_per_preventivi_IP_statico = dbRis ("collegamento_per_preventivi_IP_statico")
                // m_Tempi_liquidazione_per_tipo_prodotto_gg = dbRis ("Tempi_liquidazione_per_tipo_prodotto_gg")
                // m_Distribuzione_produzione_Statale_Perc = dbRis ("Distribuzione_produzione_Statale_Perc")
                // m_Distribuzione_produzione_Pubblico_Perc = dbRis ("Distribuzione_produzione_Pubblico_Perc")
                // m_Distribuzione_produzione_Privato_Perc = dbRis ("Distribuzione_produzione_Privato_Perc")
                // m_Distribuzione_produzione_Pensionato_Perc = dbRis ("Distribuzione_produzione_Pensionato_Perc")
                // m_Fatturato_montante_lordo_mensile = dbRis ("Fatturato_montante_lordo_mensile")
                // m_Fatturato_montante_lordo_annuo = dbRis ("Fatturato_montante_lordo_annuo")
                // m_Max_provvigionale_caricato = dbRis ("Max_provvigionale_caricato")
                // m_Note_Provvigionale_Max = dbRis ("Note_Provvigionale_Max")
                // m_Proposte = dbRis ("Proposte")
                // m_MAIL_referente = dbRis ("MAIL_referente")
                // m_Tipologia_Med_A = dbRis ("Tipologia_Med_A")
                // m_Tipologia_Med_B = dbRis ("Tipologia_Med_B")
                // m_Tipologia_Med_C = dbRis ("Tipologia_Med_C")
                // m_SCHEDA = dbRis ("SCHEDA")
                // m_Esito = dbRis ("Esito")
                // m_Raccolta_Preventivi_competitor = dbRis ("Raccolta_Preventivi_competitor")
                // m_Allegato_preventivo_competitor = dbRis ("Allegato_preventivo_competitor")
                // m_Stampa_Preventivi_finsab_confronto = dbRis ("Stampa_Preventivi_finsab_confronto")
                // m_Allegato_preventivi_finsab = dbRis ("Allegato_preventivi_finsab")
                // m_Riepilogo_esigenze_mediatore = dbRis ("Riepilogo_esigenze_mediatore")
                // m_Spread_finsab_da_assegnare_conv_inpdap = dbRis ("Spread_finsab_da_assegnare_conv_inpdap")
                // m_Provv_Mediatore_da_assegnare_conv_inpdap = dbRi s("Provv_Mediatore_da_assegnare_conv_inpdap")
                // m_Spread_finsab_da_assegnare_ordinario = dbRis ("Spread_finsab_da_assegnare_ordinario")
                // m_Provv_Mediatore_da_assegnare_ordinario = dbRis ("Provv_Mediatore_da_assegnare_ordinario")
                // m_ProfiloFuturo = dbRis ("ProfiloFuturo")

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Utente", DBUtils.GetID(m_User, m_UserID));
                writer.Write("NomeUtente", m_NomeUtente);
                writer.Write("Persona", DBUtils.GetID(m_Persona, m_PersonaID));
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("NumeroIscrizioneUIC", m_NumeroIscrizioneUIC);
                writer.Write("TipoIscrizioneUIC", m_TipoIscrizioneUIC);
                writer.Write("DataIscrizioneUIC", m_DataIscrizioneUIC);
                writer.Write("DataRinnovoUIC", m_DataRinnovoUIC);
                writer.Write("NumeroIscrizioneRUI", m_NumeroIscrizioneRUI);
                writer.Write("DataIscrizioneRUI", m_DataIscrizioneRUI);
                writer.Write("DataRinnovoRUI", m_DataRinnovoRUI);
                writer.Write("NumeroIscrizioneISVAP", m_NumeroIscrizioneISVAP);
                writer.Write("DataIscrizioneISVAP", m_DataIscrizioneISVAP);
                writer.Write("DataRinnovoISVAP", m_DataRinnovoISVAP);
                writer.Write("DataVisuraProtesti", m_DataVisuraProtesti);
                writer.Write("EsitoVisuraProtesti", m_EsitoVisuraProtesti);
                writer.Write("Tipo", m_Tipo);
                writer.Write("Referente", DBUtils.GetID(m_Referente, m_ReferenteID));
                writer.Write("AttivatoDa", DBUtils.GetID(m_AttivatoDa, m_AttivatoDaID));
                writer.Write("DataAttivazione", m_DataAttivazione);
                writer.Write("ListinoPredefinito", DBUtils.GetID(m_ListinoPredefinito, m_ListinoPredefinitoID));
                writer.Write("Rapporto", m_Rapporto);
                writer.Write("DataInizioRapporto", m_DataInizioRapporto);
                writer.Write("DataFineRapporto", m_DataFineRapporto);
                // dbRis("ProdottiDistribuiti") = m_ProdottiDistribuiti
                // dbRis("Acconti") = m_Acconti


                // dbRis("Commercializza_cqs_delega") = m_Commercializza_cqs_delega
                // dbRis("Commercializza_cqs_delega_con_chi") = m_Commercializza_cqs_delega_con_chi
                // dbRis("Commercializza_cqs_delega_con_acconti") = m_Commercializza_cqs_delega_con_acconti
                // dbRis("Netto_erogato_per_pratica") = m_Netto_erogato_per_pratica
                // dbRis("Teg_per_pratica_60_mesi") = m_Teg_per_pratica_60_mesi
                // dbRis("Teg_per_pratica_120_mesi") = m_Teg_per_pratica_120_mesi
                // dbRis("Tempi_erogazione_gg") = m_Tempi_erogazione_gg
                // dbRis("notifica_pratica_a_cura_CQSPD") = m_notifica_pratica_a_cura_CQSPD
                // dbRis("notifica_pratica_a_cura_broker") = m_notifica_pratica_a_cura_broker
                // dbRis("procura") = m_procura
                // dbRis("collegamento_per_preventivi_IP_dinamico") = m_collegamento_per_preventivi_IP_dinamico
                // dbRis("collegamento_per_preventivi_IP_statico") = m_collegamento_per_preventivi_IP_statico
                // dbRis("Tempi_liquidazione_per_tipo_prodotto_gg") = m_Tempi_liquidazione_per_tipo_prodotto_gg
                // dbRis("Distribuzione_produzione_Statale_Perc") = m_Distribuzione_produzione_Statale_Perc
                // dbRis("Distribuzione_produzione_Pubblico_Perc") = m_Distribuzione_produzione_Pubblico_Perc
                // dbRis("Distribuzione_produzione_Privato_Perc") = m_Distribuzione_produzione_Privato_Perc
                // dbRis("Distribuzione_produzione_Pensionato_Perc") = m_Distribuzione_produzione_Pensionato_Perc
                // dbRis("Fatturato_montante_lordo_mensile") = m_Fatturato_montante_lordo_mensile
                // dbRis("Fatturato_montante_lordo_annuo") = m_Fatturato_montante_lordo_annuo
                // dbRis("Max_provvigionale_caricato") = m_Max_provvigionale_caricato
                // dbRis("Note_Provvigionale_Max") = m_Note_Provvigionale_Max
                // dbRis("Proposte") = m_Proposte
                // dbRis("MAIL_referente") = m_MAIL_referente
                // dbRis("Tipologia_Med_A") = m_Tipologia_Med_A
                // dbRis("Tipologia_Med_B") = m_Tipologia_Med_B
                // dbRis("Tipologia_Med_C") = m_Tipologia_Med_C
                // dbRis("SCHEDA") = m_SCHEDA
                // dbRis("Esito") = m_Esito
                // dbRis("Raccolta_Preventivi_competitor") = m_Raccolta_Preventivi_competitor
                // dbRis("Allegato_preventivo_competitor") = m_Allegato_preventivo_competitor
                // dbRis("Stampa_Preventivi_finsab_confronto") = m_Stampa_Preventivi_finsab_confronto
                // dbRis("Allegato_preventivi_finsab") = m_Allegato_preventivi_finsab
                // dbRis("Riepilogo_esigenze_mediatore") = m_Riepilogo_esigenze_mediatore
                // dbRis("Spread_finsab_da_assegnare_conv_inpdap") = m_Spread_finsab_da_assegnare_conv_inpdap
                // dbRis("Provv_Mediatore_da_assegnare_conv_inpdap") = m_Provv_Mediatore_da_assegnare_conv_inpdap
                // dbRis("Spread_finsab_da_assegnare_ordinario") = m_Spread_finsab_da_assegnare_ordinario
                // dbRis("Provv_Mediatore_da_assegnare_ordinario") = m_Provv_Mediatore_da_assegnare_ordinario
                // dbRis("ProfiloFuturo") = m_ProfiloFuturo

                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.NomePersona;
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("User", UserID);
                writer.WriteAttribute("NomeUtente", m_NomeUtente);
                writer.WriteAttribute("Persona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("Rapporto", m_Rapporto);
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("DataInizioRapporto", m_DataInizioRapporto);
                writer.WriteAttribute("DataFineRapporto", m_DataFineRapporto);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Referente", ReferenteID);
                writer.WriteAttribute("AttivatoDa", AttivatoDaID);
                writer.WriteAttribute("DataAttivazione", m_DataAttivazione);
                writer.WriteAttribute("ListinoPredefinito", ListinoPredefinitoID);
                writer.WriteAttribute("NumeroUIC", m_NumeroIscrizioneUIC);
                writer.WriteAttribute("DataUIC", m_DataIscrizioneUIC);
                writer.WriteAttribute("TipoUIC", m_TipoIscrizioneUIC);
                writer.WriteAttribute("RinnovoUIC", m_DataRinnovoUIC);
                writer.WriteAttribute("DataVisura", m_DataVisuraProtesti);
                writer.WriteAttribute("EsitoVisura", m_EsitoVisuraProtesti);
                writer.WriteAttribute("NumeroRUI", m_NumeroIscrizioneRUI);
                writer.WriteAttribute("DataRUI", m_DataIscrizioneRUI);
                writer.WriteAttribute("RinnovoRUI", m_DataRinnovoRUI);
                writer.WriteAttribute("NumeroISVAP", m_NumeroIscrizioneISVAP);
                writer.WriteAttribute("DataISVAP", m_DataIscrizioneISVAP);
                writer.WriteAttribute("RinnovoISVAP", m_DataRinnovoISVAP);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "User":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUtente":
                        {
                            m_NomeUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Persona":
                        {
                            m_PersonaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Rapporto":
                        {
                            m_Rapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataInizioRapporto":
                        {
                            m_DataInizioRapporto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFineRapporto":
                        {
                            m_DataFineRapporto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Referente":
                        {
                            m_ReferenteID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AttivatoDa":
                        {
                            m_AttivatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataAttivazione":
                        {
                            m_DataAttivazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ListinoPredefinito":
                        {
                            m_ListinoPredefinitoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroUIC":
                        {
                            m_NumeroIscrizioneUIC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataUIC":
                        {
                            m_DataIscrizioneUIC = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoUIC":
                        {
                            m_TipoIscrizioneUIC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RinnovoUIC":
                        {
                            m_DataRinnovoUIC = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataVisura":
                        {
                            m_DataVisuraProtesti = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "EsitoVisura":
                        {
                            m_EsitoVisuraProtesti = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroRUI":
                        {
                            m_NumeroIscrizioneRUI = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRUI":
                        {
                            m_DataIscrizioneRUI = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RinnovoRUI":
                        {
                            m_DataRinnovoRUI = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NumeroISVAP":
                        {
                            m_NumeroIscrizioneISVAP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataISVAP":
                        {
                            m_DataIscrizioneISVAP = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RinnovoISVAP":
                        {
                            m_DataRinnovoISVAP = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            string IFonte.Nome
            {
                get
                {
                    return NomePersona;
                }
            }

            string IFonte.IconURL
            {
                get
                {
                    return "/widgets/images/default.gif";
                }
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                Collaboratori.UpdateCached(this);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}