using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoRichiestaApprovazione : int
        {
            /// <summary>
        /// La richiesta non è stata ancora inoltrata
        /// </summary>
        /// <remarks></remarks>
            NONCHIESTA = -1,

            /// <summary>
        /// Nessun supervisore ha ancora preso in carico la richiesta
        /// </summary>
        /// <remarks></remarks>
            ATTESA = 0,

            /// <summary>
        /// Un supervisore ha preso in carico la richiesta
        /// </summary>
        /// <remarks></remarks>
            PRESAINCARICO = 1,

            /// <summary>
        /// Il supervisore ha approvato la richiesta
        /// </summary>
        /// <remarks></remarks>
            APPROVATA = 2,

            /// <summary>
        /// Il supervisore ha negato la richiesta
        /// </summary>
        /// <remarks></remarks>
            NEGATA = 3,

            /// <summary>
        /// La richiesta è stata annullata perché l'offerta è stata modificata
        /// </summary>
        /// <remarks></remarks>
            ANNULLATA = 4
        }

        /// <summary>
    /// Rappresenta la specifica di un obiettivo per un ufficio
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CRichiestaApprovazione : Databases.DBObjectPO, IComparable, ICloneable
        {
            private int m_IDCausa;                    // ID del vincolo non rispettato
            private CDocumentoPraticaCaricato m_Causa;    // Vincolo non rispettato
            private string m_DescrizioneCausa;            // Descrizione d
            private int m_IDOggettoApprovabile;
            private string m_TipoOggettoApprovabile;
            private object m_OggettoApprovabile;
            private DateTime m_DataRichiestaApprovazione;
            private int m_IDUtenteRichiestaApprovazione;
            private string m_NomeUtenteRichiestaApprovazione;
            private Sistema.CUser m_UtenteRichiestaApprovazione;
            private int m_IDMotivoRichiesta;
            private CMotivoScontoPratica m_MotivoRichiesta;
            private string m_NomeMotivoRichiesta;
            private string m_DescrizioneRichiesta;
            private string m_ParametriRichiesta;
            private StatoRichiestaApprovazione m_StatoRichiesta;
            private DateTime? m_DataPresaInCarico;
            private int m_IDPresaInCaricoDa;
            private string m_NomePresaInCaricoDa;
            private Sistema.CUser m_PresaInCaricoDa;
            private DateTime? m_DataConferma;
            private int m_IDConfermataDa;
            private string m_NomeConfermataDa;
            private Sistema.CUser m_ConfermataDa;
            private int m_Flags;
            private string m_MotivoConferma;
            private string m_DettaglioConferma;
            private int m_IDCliente;
            private string m_NomeCliente;
            private Anagrafica.CPersona m_Cliente;
            private int m_IDGruppo;
            private RichiestaApprovazioneGroup m_Gruppo;

            public CRichiestaApprovazione()
            {
                m_IDOggettoApprovabile = 0;
                m_TipoOggettoApprovabile = "";
                m_OggettoApprovabile = null;
                m_DataRichiestaApprovazione = DMD.DateUtils.Now();
                m_IDUtenteRichiestaApprovazione = 0;
                m_NomeUtenteRichiestaApprovazione = "";
                m_UtenteRichiestaApprovazione = null;
                m_IDMotivoRichiesta = 0;
                m_MotivoRichiesta = null;
                m_NomeMotivoRichiesta = "";
                m_DescrizioneRichiesta = "";
                m_ParametriRichiesta = "";
                m_StatoRichiesta = StatoRichiestaApprovazione.NONCHIESTA;
                m_DataPresaInCarico = default;
                m_IDPresaInCaricoDa = 0;
                m_NomePresaInCaricoDa = "";
                m_PresaInCaricoDa = null;
                m_DataConferma = default;
                m_IDConfermataDa = 0;
                m_NomeConfermataDa = "";
                m_ConfermataDa = null;
                m_MotivoConferma = "";
                m_DettaglioConferma = "";
                m_Flags = 0;
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_Cliente = null;
                m_IDGruppo = 0;
                m_Gruppo = null;
            }

            public int IDGruppo
            {
                get
                {
                    return DBUtils.GetID(m_Gruppo, m_IDGruppo);
                }

                set
                {
                    int oldValue = IDGruppo;
                    if (oldValue == value)
                        return;
                    m_IDGruppo = value;
                    m_Gruppo = null;
                    DoChanged("IDGruppo", value, oldValue);
                }
            }

            public RichiestaApprovazioneGroup Gruppo
            {
                get
                {
                    if (m_Gruppo is null)
                        m_Gruppo = RichiesteApprovazioneGroups.GetItemById(m_IDGruppo);
                    return m_Gruppo;
                }

                set
                {
                    var oldValue = m_Gruppo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Gruppo = value;
                    m_IDGruppo = DBUtils.GetID(value);
                    DoChanged("Gruppo", value, oldValue);
                }
            }

            protected internal void SetGruppo(RichiestaApprovazioneGroup value)
            {
                m_Gruppo = value;
                m_IDGruppo = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta una stringa che specifica il motivo della conferma o della negazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MotivoConferma
            {
                get
                {
                    return m_MotivoConferma;
                }

                set
                {
                    string oldValue = m_MotivoConferma;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoConferma = value;
                    DoChanged("MotivoConferma", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che specifica meglio il motivo della conferma o della negazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DettaglioConferma
            {
                get
                {
                    return m_DettaglioConferma;
                }

                set
                {
                    string oldValue = m_DettaglioConferma;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioConferma = value;
                    DoChanged("DettaglioConferma", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto approvabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOggettoApprovabile
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_OggettoApprovabile, m_IDOggettoApprovabile);
                }

                set
                {
                    int oldValue = IDOggettoApprovabile;
                    if (oldValue == value)
                        return;
                    m_IDOggettoApprovabile = value;
                    m_OggettoApprovabile = null;
                    DoChanged("IDOggettoApprovabile", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo dell'oggetto approvabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoOggettoApprovabile
            {
                get
                {
                    if (m_OggettoApprovabile is object)
                        return DMD.RunTime.vbTypeName(m_OggettoApprovabile);
                    return m_TipoOggettoApprovabile;
                }

                set
                {
                    string oldValue = TipoOggettoApprovabile;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoOggettoApprovabile = value;
                    m_OggettoApprovabile = null;
                    DoChanged("TipoOggettoApprovabile", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto approvabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public object OggettoApprovabile
            {
                get
                {
                    if (m_OggettoApprovabile is null)
                        m_OggettoApprovabile = Sistema.Types.GetItemByTypeAndId(m_TipoOggettoApprovabile, m_IDOggettoApprovabile);
                    return m_OggettoApprovabile;
                }

                set
                {
                    var oldValue = OggettoApprovabile;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    SetOggettoApprovabile(value);
                    DoChanged("OggettoApprovabile", value, oldValue);
                }
            }

            protected internal void SetOggettoApprovabile(object value)
            {
                m_OggettoApprovabile = value;
                m_IDOggettoApprovabile = DBUtils.GetID((Databases.IDBObjectBase)value);
                if (value is object)
                {
                    m_TipoOggettoApprovabile = DMD.RunTime.vbTypeName(value);
                }
                else
                {
                    m_TipoOggettoApprovabile = "";
                }
            }

            /// <summary>
        /// Restituisce o imposta la data e l'ora della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime DataRichiestaApprovazione
            {
                get
                {
                    return m_DataRichiestaApprovazione;
                }

                set
                {
                    var oldValue = m_DataRichiestaApprovazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiestaApprovazione = value;
                    DoChanged("DataRichiestaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha effettuato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDUtenteRichiestaApprovazione
            {
                get
                {
                    return DBUtils.GetID(m_UtenteRichiestaApprovazione, m_IDUtenteRichiestaApprovazione);
                }

                set
                {
                    int oldValue = IDUtenteRichiestaApprovazione;
                    if (oldValue == value)
                        return;
                    m_IDUtenteRichiestaApprovazione = value;
                    m_UtenteRichiestaApprovazione = null;
                    DoChanged("IDUtenteRichiestaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha effettuato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeUtenteRichiestaApprovazione
            {
                get
                {
                    return m_NomeUtenteRichiestaApprovazione;
                }

                set
                {
                    string oldValue = m_NomeUtenteRichiestaApprovazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUtenteRichiestaApprovazione = value;
                    DoChanged("NomeUtenteRichiestaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha effettuato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser UtenteRichiestaApprovazione
            {
                get
                {
                    if (m_UtenteRichiestaApprovazione is null)
                        m_UtenteRichiestaApprovazione = Sistema.Users.GetItemById(m_IDUtenteRichiestaApprovazione);
                    return m_UtenteRichiestaApprovazione;
                }

                set
                {
                    var oldValue = UtenteRichiestaApprovazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDUtenteRichiestaApprovazione = DBUtils.GetID(value);
                    m_UtenteRichiestaApprovazione = value;
                    if (value is object)
                        m_NomeUtenteRichiestaApprovazione = value.Nominativo;
                    DoChanged("UtenteRichiestaApprovazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del motivo della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDMotivoRichiesta
            {
                get
                {
                    return DBUtils.GetID(m_MotivoRichiesta, m_IDMotivoRichiesta);
                }

                set
                {
                    int oldValue = IDMotivoRichiesta;
                    if (oldValue == value)
                        return;
                    m_MotivoRichiesta = null;
                    m_IDMotivoRichiesta = value;
                    DoChanged("IDMotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il motivo della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CMotivoScontoPratica MotivoRichiesta
            {
                get
                {
                    if (m_MotivoRichiesta is null)
                        m_MotivoRichiesta = MotiviSconto.GetItemById(m_IDMotivoRichiesta);
                    return m_MotivoRichiesta;
                }

                set
                {
                    var oldValue = MotivoRichiesta;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_MotivoRichiesta = value;
                    m_IDMotivoRichiesta = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeMotivoRichiesta = value.Nome;
                    DoChanged("MotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del motivo della richiesta di approvazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeMotivoRichiesta
            {
                get
                {
                    return m_NomeMotivoRichiesta;
                }

                set
                {
                    string oldValue = m_NomeMotivoRichiesta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeMotivoRichiesta = value;
                    DoChanged("NomeMotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che descrive la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DescrizioneRichiesta
            {
                get
                {
                    return m_DescrizioneRichiesta;
                }

                set
                {
                    string oldValue = m_DescrizioneRichiesta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneRichiesta = value;
                    DoChanged("DescrizioneRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta dei parametri aggiuntivi per la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ParametriRichiesta
            {
                get
                {
                    return m_ParametriRichiesta;
                }

                set
                {
                    string oldValue = m_ParametriRichiesta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ParametriRichiesta = value;
                    DoChanged("ParametriRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoRichiestaApprovazione StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }

                set
                {
                    var oldValue = m_StatoRichiesta;
                    if (oldValue == value)
                        return;
                    m_StatoRichiesta = value;
                    DoChanged("StatoRichiesta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data della prima presa in carico della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }

                set
                {
                    var oldValue = m_DataPresaInCarico;
                    if (oldValue == value == true)
                        return;
                    m_DataPresaInCarico = value;
                    DoChanged("DataPresaInCarico", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del primo utente che ha preso in carico la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPresaInCaricoDa
            {
                get
                {
                    return DBUtils.GetID(m_PresaInCaricoDa, m_IDPresaInCaricoDa);
                }

                set
                {
                    int oldValue = IDPresaInCaricoDa;
                    if (oldValue == value)
                        return;
                    m_IDPresaInCaricoDa = value;
                    m_PresaInCaricoDa = null;
                    DoChanged("IDPresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del primo utente che ha preso in carico la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
                }

                set
                {
                    string oldValue = m_NomePresaInCaricoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePresaInCaricoDa = value;
                    DoChanged("NomePresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il primo utente che ha preso in carico la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser PresaInCaricoDa
            {
                get
                {
                    if (m_PresaInCaricoDa is null)
                        m_PresaInCaricoDa = Sistema.Users.GetItemById(m_IDPresaInCaricoDa);
                    return m_PresaInCaricoDa;
                }

                set
                {
                    var oldValue = PresaInCaricoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PresaInCaricoDa = value;
                    m_IDPresaInCaricoDa = DBUtils.GetID(value);
                    if (value is object)
                        m_NomePresaInCaricoDa = value.Nominativo;
                    DoChanged("PresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di approvazione o negazione della richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataConferma
            {
                get
                {
                    return m_DataConferma;
                }

                set
                {
                    var oldValue = m_DataConferma;
                    if (oldValue == value == true)
                        return;
                    m_DataConferma = value;
                    DoChanged("DataConferma", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha approvato o negato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDConfermataDa
            {
                get
                {
                    return DBUtils.GetID(m_ConfermataDa, m_IDConfermataDa);
                }

                set
                {
                    int oldValue = IDConfermataDa;
                    if (oldValue == value)
                        return;
                    m_IDConfermataDa = value;
                    m_ConfermataDa = null;
                    DoChanged("IDConfermataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'utente che ha approvato o negato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeConfermataDa
            {
                get
                {
                    return m_NomeConfermataDa;
                }

                set
                {
                    string oldValue = m_NomeConfermataDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeConfermataDa = value;
                    DoChanged("NomeConfermataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha approvato o negato la richiesta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser ConfermataDa
            {
                get
                {
                    if (m_ConfermataDa is null)
                        m_ConfermataDa = Sistema.Users.GetItemById(m_IDConfermataDa);
                    return m_ConfermataDa;
                }

                set
                {
                    var oldValue = ConfermataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ConfermataDa = value;
                    m_IDConfermataDa = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeConfermataDa = value.Nominativo;
                    DoChanged("ConfermataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta i flags
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return RichiesteApprovazione.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDRichiesteApprovazione";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDGruppo = reader.Read("IDGruppo", this.m_IDGruppo);
                m_IDOggettoApprovabile = reader.Read("IDOggettoApprovabile", this.m_IDOggettoApprovabile);
                m_TipoOggettoApprovabile = reader.Read("TipoOggettoApprovabile", this.m_TipoOggettoApprovabile);
                m_DataRichiestaApprovazione = reader.Read("DataRichiestaApprovazione", this.m_DataRichiestaApprovazione);
                m_IDUtenteRichiestaApprovazione = reader.Read("IDUtenteRichiestaApprovazione", this.m_IDUtenteRichiestaApprovazione);
                m_NomeUtenteRichiestaApprovazione = reader.Read("NomeUtenteRichiestaApprovazione", this.m_NomeUtenteRichiestaApprovazione);
                m_IDMotivoRichiesta = reader.Read("IDMotivoRichiesta", this.m_IDMotivoRichiesta);
                m_NomeMotivoRichiesta = reader.Read("NomeMotivoRichiesta", this.m_NomeMotivoRichiesta);
                m_DescrizioneRichiesta = reader.Read("DescrizioneRichiesta", this.m_DescrizioneRichiesta);
                m_ParametriRichiesta = reader.Read("ParametriRichiesta", this.m_ParametriRichiesta);
                m_StatoRichiesta = reader.Read("StatoRichiesta", this.m_StatoRichiesta);
                m_DataPresaInCarico = reader.Read("DataPresaInCarico", this.m_DataPresaInCarico);
                m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", this.m_IDPresaInCaricoDa);
                m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa", this.m_NomePresaInCaricoDa);
                m_DataConferma = reader.Read("DataConferma", this.m_DataConferma);
                m_IDConfermataDa = reader.Read("IDConfermataDa", this.m_IDConfermataDa);
                m_NomeConfermataDa = reader.Read("NomeConfermataDa", this.m_NomeConfermataDa);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_MotivoConferma = reader.Read("MotivoConferma", this.m_MotivoConferma);
                m_DettaglioConferma = reader.Read("DettaglioConferma", this.m_DettaglioConferma);
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomePersona", this.m_NomeCliente);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDGruppo", IDGruppo);
                writer.Write("IDOggettoApprovabile", IDOggettoApprovabile);
                writer.Write("TipoOggettoApprovabile", TipoOggettoApprovabile);
                writer.Write("DataRichiestaApprovazione", m_DataRichiestaApprovazione);
                writer.Write("IDUtenteRichiestaApprovazione", IDUtenteRichiestaApprovazione);
                writer.Write("NomeUtenteRichiestaApprovazione", m_NomeUtenteRichiestaApprovazione);
                writer.Write("IDMotivoRichiesta", IDMotivoRichiesta);
                writer.Write("NomeMotivoRichiesta", m_NomeMotivoRichiesta);
                writer.Write("DescrizioneRichiesta", m_DescrizioneRichiesta);
                writer.Write("ParametriRichiesta", m_ParametriRichiesta);
                writer.Write("StatoRichiesta", m_StatoRichiesta);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.Write("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.Write("DataConferma", m_DataConferma);
                writer.Write("IDConfermataDa", IDConfermataDa);
                writer.Write("NomeConfermataDa", m_NomeConfermataDa);
                writer.Write("Flags", m_Flags);
                writer.Write("MotivoConferma", m_MotivoConferma);
                writer.Write("DettaglioConferma", m_DettaglioConferma);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomePersona", m_NomeCliente);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDGruppo", IDGruppo);
                writer.WriteAttribute("IDOggettoApprovabile", IDOggettoApprovabile);
                writer.WriteAttribute("TipoOggettoApprovabile", TipoOggettoApprovabile);
                writer.WriteAttribute("DataRichiestaApprovazione", m_DataRichiestaApprovazione);
                writer.WriteAttribute("IDUtenteRichiestaApprovazione", IDUtenteRichiestaApprovazione);
                writer.WriteAttribute("NomeUtenteRichiestaApprovazione", m_NomeUtenteRichiestaApprovazione);
                writer.WriteAttribute("IDMotivoRichiesta", IDMotivoRichiesta);
                writer.WriteAttribute("NomeMotivoRichiesta", m_NomeMotivoRichiesta);
                writer.WriteAttribute("StatoRichiesta", (int?)m_StatoRichiesta);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.WriteAttribute("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.WriteAttribute("DataConferma", m_DataConferma);
                writer.WriteAttribute("IDConfermataDa", IDConfermataDa);
                writer.WriteAttribute("NomeConfermataDa", m_NomeConfermataDa);
                writer.WriteAttribute("MotivoConferma", m_MotivoConferma);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                base.XMLSerialize(writer);
                writer.WriteTag("DescrizioneRichiesta", m_DescrizioneRichiesta);
                writer.WriteTag("ParametriRichiesta", m_ParametriRichiesta);
                writer.WriteTag("DettaglioConferma", m_DettaglioConferma);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDGruppo":
                        {
                            m_IDGruppo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOggettoApprovabile":
                        {
                            m_IDOggettoApprovabile = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoOggettoApprovabile":
                        {
                            m_TipoOggettoApprovabile = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRichiestaApprovazione":
                        {
                            m_DataRichiestaApprovazione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDUtenteRichiestaApprovazione":
                        {
                            m_IDUtenteRichiestaApprovazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUtenteRichiestaApprovazione":
                        {
                            m_NomeUtenteRichiestaApprovazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDMotivoRichiesta":
                        {
                            m_IDMotivoRichiesta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeMotivoRichiesta":
                        {
                            m_NomeMotivoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoRichiesta":
                        {
                            m_StatoRichiesta = (StatoRichiestaApprovazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPresaInCaricoDa":
                        {
                            m_IDPresaInCaricoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePresaInCaricoDa":
                        {
                            m_NomePresaInCaricoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataConferma":
                        {
                            m_DataConferma = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDConfermataDa":
                        {
                            m_IDConfermataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConfermataDa":
                        {
                            m_NomeConfermataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DescrizioneRichiesta":
                        {
                            m_DescrizioneRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ParametriRichiesta":
                        {
                            m_ParametriRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoConferma":
                        {
                            m_MotivoConferma = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioConferma":
                        {
                            m_DettaglioConferma = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public int CompareTo(CRichiestaApprovazione obj)
            {
                return DMD.DateUtils.Compare(m_DataRichiestaApprovazione, obj.m_DataRichiestaApprovazione);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CRichiestaApprovazione)obj);
            }

            protected override void OnCreate(SystemEvent e)
            {
                base.OnCreate(e);
                RichiesteApprovazione.doItemCreated(new ItemEventArgs(this));
            }

            protected override void OnDelete(SystemEvent e)
            {
                base.OnDelete(e);
                RichiesteApprovazione.doItemDeleted(new ItemEventArgs(this));
            }

            protected override void OnModified(SystemEvent e)
            {
                base.OnModified(e);
                RichiesteApprovazione.doItemModified(new ItemEventArgs(this));
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}