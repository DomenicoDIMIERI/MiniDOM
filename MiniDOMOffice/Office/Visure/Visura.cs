using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {
        /// <summary>
        /// Stato di una visura
        /// </summary>
        public enum StatoVisura : int
        {
            /// <summary>
            /// Visura non chiesta
            /// </summary>
            DA_RICHIEDERE = 0,

            /// <summary>
            /// Visura richiesta
            /// </summary>
            RICHIESTA = 1,

            /// <summary>
            /// Visura ritirata
            /// </summary>
            RITIRATA = 3,

            /// <summary>
            /// Visura annullata dal richiedente
            /// </summary>
            ANNULLATA = 4,

            /// <summary>
            /// Visura annullata dall'ente
            /// </summary>
            RIFIUTATA = 2
        }



        /// <summary>
        /// Rappresenta una richiesta di visura/censimento
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Visura 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<Visura>
        {
            private DateTime m_Data;                      // Data ed ora della richiesta
            [NonSerialized] private Sistema.CUser m_Richiedente;                // Utente che ha effettuato la richiesta
            private int m_IDRichiedente;            // ID del richiedente che ha effettuato la richiesta
            private string m_NomeRichiedente;           // Nome del richiedente che ha effettuato la richiesta
            [NonSerialized] private Sistema.CUser m_PresaInCaricoDa;
            private int m_IDPresaInCaricoDa;
            private string m_NomePresaInCaricoDa;
            private DateTime? m_DataPresaInCarico;
            private DateTime? m_DataCompletamento;
            private StatoVisura m_StatoVisura;
            private bool m_ValutazioneAmministrazione;
            private bool m_CensimentoDatoreDiLavoro;
            private bool m_CensimentoSedeOperativa;
            private bool m_VariazioneDenominazione;
            private bool m_Sblocco;
            private int m_IDAmministrazione;
            [NonSerialized] private Anagrafica.CAzienda m_Amministrazione;
            private string m_RagioneSociale;
            private string m_OggettoSociale;
            private string m_CodiceFiscale;
            private string m_PartitaIVA;
            private string m_ResponsabileDaContattare;
            private string m_Qualifica;
            private CIndirizzo m_Indirizzo;
            private string m_Telefono;
            private string m_Fax;
            private string m_IndirizzoeMail;
            private CIndirizzo m_IndirizzoDiNotifica;
            private string m_TelefonoDiNotifica;
            private string m_FaxDiNotifica;
            private bool m_ConvenzionePresente;
            private string m_CodiceODescrizioneConvenzione;
            private int? m_NumeroDipendenti;
            private bool m_AmministrazioneSottoscriveMODPREST_008;
            private string m_NoteOInfoSullaSocieta;
            private int m_IDBustaPaga;
            [NonSerialized] private CAttachment m_BustaPaga;
            private int m_IDMotivoRichiestaSblocco;
            [NonSerialized] private CAttachment m_MotivoRichiestaSblocco;
            [NonSerialized] private CAttachmentsCollection m_AltriAllegati;
            [NonSerialized] private CAttachmentsCollection m_DocumentiProdotti;
            private string m_CodiceAmministrazioneCL;
            private string m_StatoAmministrazioneCL;
            private string m_CodiceDatoreLavoroCL;
            private string m_RagioneSocialeSOP;
            private string m_ResponsabileDaContattareSOP;
            private string m_QualificaSOP;
            private CIndirizzo m_IndirizzoSO;
            private string m_TelefonoSO;
            private string m_FaxSO;
            private bool m_ConvenzionePresenteSO;
            private string m_CodiceODescrizioneConvenzioneSO;
            private int m_IDBustaPagaSO;
            [NonSerialized] private CAttachment m_BustaPagaSO;
            [NonSerialized] private CAttachmentsCollection m_AltriAllegatiSO;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Visura()
            {
                m_Data = default;
                m_Richiedente = null;
                m_IDRichiedente = 0;
                m_NomeRichiedente = DMD.Strings.vbNullString;
                m_PresaInCaricoDa = null;
                m_IDPresaInCaricoDa = 0;
                m_NomePresaInCaricoDa = DMD.Strings.vbNullString;
                m_DataPresaInCarico = default;
                m_DataCompletamento = default;
                m_StatoVisura = StatoVisura.DA_RICHIEDERE;
                m_ValutazioneAmministrazione = false;
                m_CensimentoDatoreDiLavoro = false;
                m_CensimentoSedeOperativa = false;
                m_VariazioneDenominazione = false;
                m_Sblocco = false;
                m_IDAmministrazione = 0;
                m_Amministrazione = null;
                m_RagioneSociale = DMD.Strings.vbNullString;
                m_OggettoSociale = DMD.Strings.vbNullString;
                m_CodiceFiscale = DMD.Strings.vbNullString;
                m_PartitaIVA = DMD.Strings.vbNullString;
                m_ResponsabileDaContattare = DMD.Strings.vbNullString;
                m_Qualifica = DMD.Strings.vbNullString;
                m_Indirizzo = new Anagrafica.CIndirizzo();
                m_Telefono = DMD.Strings.vbNullString;
                m_Fax = DMD.Strings.vbNullString;
                m_IndirizzoeMail = DMD.Strings.vbNullString;
                m_IndirizzoDiNotifica = new Anagrafica.CIndirizzo();
                m_TelefonoDiNotifica = DMD.Strings.vbNullString;
                m_FaxDiNotifica = DMD.Strings.vbNullString;
                m_ConvenzionePresente = false;
                m_CodiceODescrizioneConvenzione = DMD.Strings.vbNullString;
                m_NumeroDipendenti = default;
                m_AmministrazioneSottoscriveMODPREST_008 = false;
                m_NoteOInfoSullaSocieta = DMD.Strings.vbNullString;
                m_IDBustaPaga = 0;
                m_BustaPaga = null;
                m_IDMotivoRichiestaSblocco = 0;
                m_MotivoRichiestaSblocco = null;
                m_AltriAllegati = null;
                m_DocumentiProdotti = null;
                m_CodiceAmministrazioneCL = DMD.Strings.vbNullString;
                m_StatoAmministrazioneCL = DMD.Strings.vbNullString;
                m_CodiceDatoreLavoroCL = DMD.Strings.vbNullString;
                m_RagioneSocialeSOP = DMD.Strings.vbNullString;
                m_ResponsabileDaContattareSOP = DMD.Strings.vbNullString;
                m_QualificaSOP = DMD.Strings.vbNullString;
                m_IndirizzoSO = new Anagrafica.CIndirizzo();
                m_TelefonoSO = DMD.Strings.vbNullString;
                m_FaxSO = DMD.Strings.vbNullString;
                m_ConvenzionePresenteSO = false;
                m_CodiceODescrizioneConvenzioneSO = DMD.Strings.vbNullString;
                m_IDBustaPagaSO = 0;
                m_BustaPagaSO = null;
                m_AltriAllegatiSO = null;
            }

            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    m_Richiedente = Sistema.Users.CurrentUser;
            //    m_IDRichiedente = DBUtils.GetID(m_Richiedente);
            //    m_NomeRichiedente = m_Richiedente.Nominativo;
            //    m_StatoVisura = StatoVisura.DA_RICHIEDERE;
            //    m_DataCompletamento = default;
            //    m_DataPresaInCarico = default;
            //    m_PresaInCaricoDa = null;
            //    m_IDPresaInCaricoDa = 0;
            //    m_NomePresaInCaricoDa = DMD.Strings.vbNullString;
            //    m_AltriAllegati = null;
            //    m_Data = DMD.DateUtils.Now();
            //}

            /// <summary>
            /// Codice del datore di lavoro
            /// </summary>
            public string CodiceDatoreLavoroCL
            {
                get
                {
                    return m_CodiceDatoreLavoroCL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceDatoreLavoroCL;
                    if ((value ?? "") == (value ?? ""))
                        return;
                    m_CodiceDatoreLavoroCL = value;
                    DoChanged("CodiceDatoreLavoroCL", value, oldValue);
                }
            }

            /// <summary>
            /// Ragione sociale
            /// </summary>
            public string RagioneSocialeSOP
            {
                get
                {
                    return m_RagioneSocialeSOP;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_RagioneSocialeSOP;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RagioneSocialeSOP = value;
                    DoChanged("RagioneSocialeSOP", value, oldValue);
                }
            }

            /// <summary>
            /// Responsabile da contattare
            /// </summary>
            public string ResponsabileDaContattareSOP
            {
                get
                {
                    return m_ResponsabileDaContattareSOP;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ResponsabileDaContattareSOP;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ResponsabileDaContattareSOP = value;
                    DoChanged("ResponsabileDaContattareSOP", value, oldValue);
                }
            }

            /// <summary>
            /// Qualifica
            /// </summary>
            public string QualificaSOP
            {
                get
                {
                    return m_QualificaSOP;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_QualificaSOP;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_QualificaSOP = value;
                    DoChanged("QualificaSOP", value, oldValue);
                }
            }

            /// <summary>
            /// Indirizzo
            /// </summary>
            public Anagrafica.CIndirizzo IndirizzoSO
            {
                get
                {
                    return m_IndirizzoSO;
                }
            }

            /// <summary>
            /// Telefono
            /// </summary>
            public string TelefonoSO
            {
                get
                {
                    return m_TelefonoSO;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TelefonoSO;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TelefonoSO = value;
                    DoChanged("TelefonoSO", value, oldValue);
                }
            }

            /// <summary>
            /// Fax
            /// </summary>
            public string FaxSO
            {
                get
                {
                    return m_FaxSO;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_FaxSO;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FaxSO = value;
                    DoChanged("FaxSO", value, oldValue);
                }
            }

            /// <summary>
            /// Conversione Presente
            /// </summary>
            public bool ConvenzionePresenteSO
            {
                get
                {
                    return m_ConvenzionePresenteSO;
                }

                set
                {
                    if (m_ConvenzionePresenteSO == value)
                        return;
                    m_ConvenzionePresenteSO = value;
                    DoChanged("ConvenzionePresenteSO", value, !value);
                }
            }

            /// <summary>
            /// Codice o descrizione
            /// </summary>
            public string CodiceODescrizioneConvenzioneSO
            {
                get
                {
                    return m_CodiceODescrizioneConvenzioneSO;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceODescrizioneConvenzioneSO;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceODescrizioneConvenzioneSO = value;
                    DoChanged("CodiceODescrizioneConvenzioneSO", value, oldValue);
                }
            }

            /// <summary>
            /// ID della busta paga
            /// </summary>
            public int IDBustaPagaSO
            {
                get
                {
                    return DBUtils.GetID(m_BustaPagaSO, m_IDBustaPagaSO);
                }

                set
                {
                    int oldValue = IDBustaPagaSO;
                    if (oldValue == value)
                        return;
                    m_IDBustaPagaSO = value;
                    m_BustaPagaSO = null;
                    DoChanged("IDBustaPagaSO", value, oldValue);
                }
            }

            /// <summary>
            /// Busta paga
            /// </summary>
            public Sistema.CAttachment BustaPagaSO
            {
                get
                {
                    if (m_BustaPagaSO is null)
                        m_BustaPagaSO = Sistema.Attachments.GetItemById(m_IDBustaPagaSO);
                    return m_BustaPagaSO;
                }

                set
                {
                    var oldValue = m_BustaPagaSO;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_BustaPagaSO = value;
                    m_IDBustaPagaSO = DBUtils.GetID(value, 0);
                    DoChanged("IDBustaPagaSO", value, oldValue);
                }
            }

            /// <summary>
            /// Altri allegati
            /// </summary>
            public Sistema.CAttachmentsCollection AltriAllegatiSO
            {
                get
                {
                    if (m_AltriAllegatiSO is null)
                        m_AltriAllegatiSO = new Sistema.CAttachmentsCollection(this, "AltriAllegatiSO", 0);
                    return m_AltriAllegatiSO;
                }
            }

            /// <summary>
            /// Codice amministrazione del cliente
            /// </summary>
            public string CodiceAmministrazioneCL
            {
                get
                {
                    return m_CodiceAmministrazioneCL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceAmministrazioneCL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceAmministrazioneCL = value;
                    DoChanged("CodiceAmministrazioneCL", value, oldValue);
                }
            }

            /// <summary>
            /// Stato amministrazione
            /// </summary>
            public string StatoAmministrazioneCL
            {
                get
                {
                    return m_StatoAmministrazioneCL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_StatoAmministrazioneCL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StatoAmministrazioneCL = value;
                    DoChanged("StatoAmministrazioneCL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data della richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha effettuato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Richiedente
            {
                get
                {
                    if (m_Richiedente is null)
                        m_Richiedente = Sistema.Users.GetItemById(m_IDRichiedente);
                    return m_Richiedente;
                }

                set
                {
                    object oldValue = m_Richiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiedente = value;
                    m_IDRichiedente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeRichiedente = value.Nominativo;
                    DoChanged("Richiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del richiedente a cui è stata assegnata la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_Richiedente, m_IDRichiedente);
                }

                set
                {
                    int oldValue = IDRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDRichiedente = value;
                    m_Richiedente = null;
                    DoChanged("IDRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del richiedente che ha effettuato la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiedente = value;
                    DoChanged("NomeRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Presa in carico da
            /// </summary>
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
                    var oldValue = m_PresaInCaricoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PresaInCaricoDa = value;
                    m_IDPresaInCaricoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePresaInCaricoDa = value.Nominativo;
                    DoChanged("PresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente che ha preso in carico la richiesta di visura
            /// </summary>
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
            /// Nome dell'utente che ha preso in carico la richiesta
            /// </summary>
            public string NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePresaInCaricoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePresaInCaricoDa = value;
                    DoChanged("NomePresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Data di presa in carico della richiesta
            /// </summary>
            public DateTime? DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }

                set
                {
                    var oldValue = m_DataPresaInCarico;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPresaInCarico = value;
                    DoChanged("DataPresaInCarico", value, oldValue);
                }
            }

            /// <summary>
            /// Data di esito della richiesta
            /// </summary>
            public DateTime? DataCompletamento
            {
                get
                {
                    return m_DataCompletamento;
                }

                set
                {
                    var oldvalue = m_DataCompletamento;
                    if (DMD.DateUtils.Compare(value, oldvalue) == 0)
                        return;
                    m_DataCompletamento = value;
                    DoChanged("DataCompletamento", value, oldvalue);
                }
            }

            /// <summary>
            /// Stato della richiesta di visura
            /// </summary>
            public StatoVisura StatoVisura
            {
                get
                {
                    return m_StatoVisura;
                }

                set
                {
                    var oldValue = m_StatoVisura;
                    if (oldValue == value)
                        return;
                    m_StatoVisura = value;
                    DoChanged("StatoVisura", value, oldValue);
                }
            }

            /// <summary>
            /// Se vero indica che l'amministrazione é stata valutata
            /// </summary>
            public bool ValutazioneAmministrazione
            {
                get
                {
                    return m_ValutazioneAmministrazione;
                }

                set
                {
                    if (m_ValutazioneAmministrazione == value)
                        return;
                    m_ValutazioneAmministrazione = value;
                    DoChanged("ValutazioneAmministrazione", value, !value);
                }
            }

            /// <summary>
            /// CensimentoDatoreDiLavoro
            /// </summary>
            public bool CensimentoDatoreDiLavoro
            {
                get
                {
                    return m_CensimentoDatoreDiLavoro;
                }

                set
                {
                    if (m_CensimentoDatoreDiLavoro == value)
                        return;
                    m_CensimentoDatoreDiLavoro = value;
                    DoChanged("CensimentoDatoreDiLavoro", value, !value);
                }
            }

            /// <summary>
            /// CensimentoSedeOperativa
            /// </summary>
            public bool CensimentoSedeOperativa
            {
                get
                {
                    return m_CensimentoSedeOperativa;
                }

                set
                {
                    if (m_CensimentoSedeOperativa == value)
                        return;
                    m_CensimentoSedeOperativa = value;
                    DoChanged("CensimentoSedeOperativa", value, !value);
                }
            }

            /// <summary>
            /// VariazioneDenominazione
            /// </summary>
            public bool VariazioneDenominazione
            {
                get
                {
                    return m_VariazioneDenominazione;
                }

                set
                {
                    if (m_VariazioneDenominazione == value)
                        return;
                    m_VariazioneDenominazione = value;
                    DoChanged("VariazioneDenominazione", value, !value);
                }
            }

            /// <summary>
            /// Sblocco
            /// </summary>
            public bool Sblocco
            {
                get
                {
                    return m_Sblocco;
                }

                set
                {
                    if (m_Sblocco == value)
                        return;
                    m_Sblocco = value;
                    DoChanged("Sblocco", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'amministrazione a cui si è stata inviata la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda Amministrazione
            {
                get
                {
                    if (m_Amministrazione is null)
                        m_Amministrazione = Anagrafica.Aziende.GetItemById(m_IDAmministrazione);
                    return m_Amministrazione;
                }

                set
                {
                    var oldValue = m_Amministrazione;
                    if (oldValue == value)
                        return;
                    m_Amministrazione = value;
                    m_IDAmministrazione = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_RagioneSociale = value.Nominativo;
                    }

                    DoChanged("Amministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'amministrazione a cui è stata fatta la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAmministrazione
            {
                get
                {
                    return DBUtils.GetID(m_Amministrazione, m_IDAmministrazione);
                }

                set
                {
                    int oldValue = IDAmministrazione;
                    if (oldValue == value)
                        return;
                    m_IDAmministrazione = value;
                    m_Amministrazione = null;
                    DoChanged("IDAmministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Ragione sociale
            /// </summary>
            public string RagioneSociale
            {
                get
                {
                    return m_RagioneSociale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_RagioneSociale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RagioneSociale = value;
                    DoChanged("RagioneSociale", value, oldValue);
                }
            }

            /// <summary>
            /// Oggetto sociale
            /// </summary>
            public string OggettoSociale
            {
                get
                {
                    return m_OggettoSociale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_OggettoSociale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_OggettoSociale = value;
                    DoChanged("OggettoSociale", value, oldValue);
                }
            }

            /// <summary>
            /// Codice fiscale
            /// </summary>
            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    string oldValue = m_CodiceFiscale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            /// <summary>
            /// Partita IVA
            /// </summary>
            public string PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }

                set
                {
                    value = Sistema.Formats.ParsePartitaIVA(value);
                    string oldValue = m_PartitaIVA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PartitaIVA = value;
                    DoChanged("PartitaIVA", value, oldValue);
                }
            }

            /// <summary>
            /// Responsabile da contattare
            /// </summary>
            public string ResponsabileDaContattare
            {
                get
                {
                    return m_ResponsabileDaContattare;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ResponsabileDaContattare;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ResponsabileDaContattare = value;
                    DoChanged("ResponsabileDaContattare", value, oldValue);
                }
            }

            /// <summary>
            /// Qualifica
            /// </summary>
            public string Qualifica
            {
                get
                {
                    return m_Qualifica;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Qualifica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Qualifica = value;
                    DoChanged("Qualifica", value, oldValue);
                }
            }

            /// <summary>
            /// Indirizzo
            /// </summary>
            public Anagrafica.CIndirizzo Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            /// <summary>
            /// Telefono
            /// </summary>
            public string Telefono
            {
                get
                {
                    return m_Telefono;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Telefono;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Telefono = value;
                    DoChanged("Telefono", value, oldValue);
                }
            }

            /// <summary>
            /// FAX
            /// </summary>
            public string Fax
            {
                get
                {
                    return m_Fax;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Fax;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Fax = value;
                    DoChanged("Fax", value, oldValue);
                }
            }

            /// <summary>
            /// Indirizzo eMail
            /// </summary>
            public string IndirizzoeMail
            {
                get
                {
                    return m_IndirizzoeMail;
                }

                set
                {
                    value = Sistema.Formats.ParseEMailAddress(value);
                    string oldValue = m_IndirizzoeMail;
                    m_IndirizzoeMail = value;
                    DoChanged("IndirizzoeMail", value, oldValue);
                }
            }

            /// <summary>
            /// Indirizzo di notifica
            /// </summary>
            public Anagrafica.CIndirizzo IndirizzoDiNotifica
            {
                get
                {
                    return m_IndirizzoDiNotifica;
                }
            }

            /// <summary>
            /// Telefono per la notifica
            /// </summary>
            public string TelefonoDiNotifica
            {
                get
                {
                    return m_TelefonoDiNotifica;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_TelefonoDiNotifica;
                    m_TelefonoDiNotifica = value;
                    DoChanged("TelefonoDiNotifica", value, oldValue);
                }
            }

            /// <summary>
            /// Fax per la notifica
            /// </summary>
            public string FaxDiNotifica
            {
                get
                {
                    return m_FaxDiNotifica;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_FaxDiNotifica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FaxDiNotifica = value;
                    DoChanged("FaxDiNotifica", value, oldValue);
                }
            }

            /// <summary>
            /// ConvenzionePresente
            /// </summary>
            public bool ConvenzionePresente
            {
                get
                {
                    return m_ConvenzionePresente;
                }

                set
                {
                    if (m_ConvenzionePresente == value)
                        return;
                    m_ConvenzionePresente = value;
                    DoChanged("ConvenzionePresente", value, !value);
                }
            }

            /// <summary>
            /// CodiceODescrizioneConvenzione
            /// </summary>
            public string CodiceODescrizioneConvenzione
            {
                get
                {
                    return m_CodiceODescrizioneConvenzione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceODescrizioneConvenzione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceODescrizioneConvenzione = value;
                    DoChanged("CodiceODescrizioneConvenzione", value, oldValue);
                }
            }

            /// <summary>
            /// Numero Dipendenti
            /// </summary>
            public int? NumeroDipendenti
            {
                get
                {
                    return m_NumeroDipendenti;
                }

                set
                {
                    var oldValue = m_NumeroDipendenti;
                    if (oldValue == value == true)
                        return;
                    m_NumeroDipendenti = value;
                    DoChanged("NumeroDipendenti", value, oldValue);
                }
            }

            /// <summary>
            /// AmministrazioneSottoscriveMODPREST_008
            /// </summary>
            public bool AmministrazioneSottoscriveMODPREST_008
            {
                get
                {
                    return m_AmministrazioneSottoscriveMODPREST_008;
                }

                set
                {
                    if (m_AmministrazioneSottoscriveMODPREST_008 == value)
                        return;
                    m_AmministrazioneSottoscriveMODPREST_008 = value;
                    DoChanged("AmministrazioneSottoscriveMODPREST_008", value, !value);
                }
            }

            /// <summary>
            /// NoteOInfoSullaSocieta
            /// </summary>
            public string NoteOInfoSullaSocieta
            {
                get
                {
                    return m_NoteOInfoSullaSocieta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NoteOInfoSullaSocieta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NoteOInfoSullaSocieta = value;
                    DoChanged("NoteOInfoSullaSocieta", value, oldValue);
                }
            }

            /// <summary>
            /// IDBustaPaga
            /// </summary>
            public int IDBustaPaga
            {
                get
                {
                    return DBUtils.GetID(m_BustaPaga, m_IDBustaPaga);
                }

                set
                {
                    int oldValue = IDBustaPaga;
                    if (oldValue == value)
                        return;
                    m_IDBustaPaga = value;
                    m_BustaPaga = null;
                    DoChanged("IDBustaPaga", value, oldValue);
                }
            }

            /// <summary>
            /// BustaPaga
            /// </summary>
            public Sistema.CAttachment BustaPaga
            {
                get
                {
                    if (m_BustaPaga is null)
                        m_BustaPaga = Sistema.Attachments.GetItemById(m_IDBustaPaga);
                    return m_BustaPaga;
                }

                set
                {
                    var oldValue = m_BustaPaga;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_BustaPaga = value;
                    m_IDBustaPaga = DBUtils.GetID(value, 0);
                    DoChanged("BustaPaga", value, oldValue);
                }
            }

            /// <summary>
            /// IDMotivoRichiestaSblocco
            /// </summary>
            public int IDMotivoRichiestaSblocco
            {
                get
                {
                    return DBUtils.GetID(m_MotivoRichiestaSblocco, m_IDMotivoRichiestaSblocco);
                }

                set
                {
                    int oldValue = IDMotivoRichiestaSblocco;
                    if (oldValue == value)
                        return;
                    m_IDMotivoRichiestaSblocco = value;
                    m_MotivoRichiestaSblocco = null;
                    DoChanged("IDMotivoRichiestaSblocco", value, oldValue);
                }
            }

            /// <summary>
            /// MotivoRichiestaSblocco
            /// </summary>
            public Sistema.CAttachment MotivoRichiestaSblocco
            {
                get
                {
                    if (m_MotivoRichiestaSblocco is null)
                        m_MotivoRichiestaSblocco = Sistema.Attachments.GetItemById(m_IDMotivoRichiestaSblocco);
                    return m_MotivoRichiestaSblocco;
                }

                set
                {
                    var oldValue = m_MotivoRichiestaSblocco;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_MotivoRichiestaSblocco = value;
                    m_IDMotivoRichiestaSblocco = DBUtils.GetID(value, 0);
                    DoChanged("MotivoRichiestaSblocco", value, oldValue);
                }
            }

            /// <summary>
            /// AltriAllegati
            /// </summary>
            public Sistema.CAttachmentsCollection AltriAllegati
            {
                get
                {
                    if (m_AltriAllegati is null)
                        m_AltriAllegati = new Sistema.CAttachmentsCollection(this, "AltriAllegati", 0);
                    return m_AltriAllegati;
                }
            }

            /// <summary>
            /// DocumentiProdotti
            /// </summary>
            public Sistema.CAttachmentsCollection DocumentiProdotti
            {
                get
                {
                    if (m_DocumentiProdotti is null)
                        m_DocumentiProdotti = new Sistema.CAttachmentsCollection(this, "DocumentiProdotti", 0);
                    return m_DocumentiProdotti;
                }
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Visure;
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged()
                    || m_Indirizzo.IsChanged() 
                    || m_IndirizzoDiNotifica.IsChanged() 
                    || m_IndirizzoSO.IsChanged();
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CQSPDVisure";
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_Indirizzo.SetChanged(false);
                m_IndirizzoDiNotifica.SetChanged(false);
                m_IndirizzoSO.SetChanged(false);
            }

            /// <summary>
            /// Salva nel recordset
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Data", m_Data);
                writer.Write("IDRichiedente", IDRichiedente);
                writer.Write("NomeRichiedente", m_NomeRichiedente);
                writer.Write("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.Write("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("DataCompletamento", m_DataCompletamento);
                writer.Write("StatoVisura", m_StatoVisura);
                writer.Write("VALAMM", m_ValutazioneAmministrazione);
                writer.Write("CENSDATLAV", m_CensimentoDatoreDiLavoro);
                writer.Write("CENSSEDOP", m_CensimentoSedeOperativa);
                writer.Write("VARIAZDENOM", m_VariazioneDenominazione);
                writer.Write("SBLOCCO", m_Sblocco);
                writer.Write("IDAmministrazione", IDAmministrazione);
                writer.Write("RagioneSociale", m_RagioneSociale);
                writer.Write("OggettoSociale", m_OggettoSociale);
                writer.Write("CodiceFiscale", m_CodiceFiscale);
                writer.Write("PartitaIVA", m_PartitaIVA);
                writer.Write("NomeResponsabile", m_ResponsabileDaContattare);
                writer.Write("QualificaResponsabile", m_Qualifica);
                writer.Write("Indirizzo_Provincia", m_Indirizzo.Provincia);
                writer.Write("Indirizzo_Citta", m_Indirizzo.Citta);
                writer.Write("Indirizzo_CAP", m_Indirizzo.CAP);
                writer.Write("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                writer.Write("Telefono", m_Telefono);
                writer.Write("Fax", m_Fax);
                writer.Write("eMail", m_IndirizzoeMail);
                writer.Write("IndirizzoN_Provincia", m_IndirizzoDiNotifica.Provincia);
                writer.Write("IndirizzoN_Citta", m_IndirizzoDiNotifica.Citta);
                writer.Write("IndirizzoN_CAP", m_IndirizzoDiNotifica.CAP);
                writer.Write("IndirizzoN_Via", m_IndirizzoDiNotifica.ToponimoViaECivico);
                writer.Write("TelefonoN", m_TelefonoDiNotifica);
                writer.Write("FaxN", m_FaxDiNotifica);
                writer.Write("CONVSINO", m_ConvenzionePresente);
                writer.Write("CODCONV", m_CodiceODescrizioneConvenzione);
                writer.Write("NumeroDipendenti", m_NumeroDipendenti);
                writer.Write("AMMMODPRST008", m_AmministrazioneSottoscriveMODPREST_008);
                writer.Write("NoteSocieta", m_NoteOInfoSullaSocieta);
                writer.Write("IDBustaPaga", IDBustaPaga);
                writer.Write("IDMotivoSblocco", IDMotivoRichiestaSblocco);
                writer.Write("CODAMMCL", m_CodiceAmministrazioneCL);
                writer.Write("STATOAMMCL", m_StatoAmministrazioneCL);
                writer.Write("CODDLAVCL", m_CodiceDatoreLavoroCL);
                writer.Write("RAGSOCSOP", m_RagioneSocialeSOP);
                writer.Write("RESPCONTSOP", m_ResponsabileDaContattareSOP);
                writer.Write("QUALIFSOP", m_QualificaSOP);
                writer.Write("IndirizzoSO_Provincia", m_IndirizzoSO.Provincia);
                writer.Write("IndirizzoSO_Citta", m_IndirizzoSO.Citta);
                writer.Write("IndirizzoSO_CAP", m_IndirizzoSO.CAP);
                writer.Write("IndirizzoSO_Via", m_IndirizzoSO.ToponimoViaECivico);
                writer.Write("TelefonoSO", m_TelefonoSO);
                writer.Write("FaxSO", m_FaxSO);
                writer.Write("CONVSINOSO", m_ConvenzionePresenteSO);
                writer.Write("CODCONVSO", m_CodiceODescrizioneConvenzioneSO);
                writer.Write("IDBustaPagaSO", IDBustaPagaSO);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Data = reader.Read("Data", this.m_Data);
                this.m_IDRichiedente = reader.Read("IDRichiedente", this.m_IDRichiedente);
                this.m_NomeRichiedente = reader.Read("NomeRichiedente", this.m_NomeRichiedente);
                this.m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", this.m_IDPresaInCaricoDa);
                this.m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa", this.m_NomePresaInCaricoDa);
                this.m_DataPresaInCarico = reader.Read("DataPresaInCarico", this.m_DataPresaInCarico);
                this.m_DataCompletamento = reader.Read("DataCompletamento", this.m_DataCompletamento);
                this.m_StatoVisura = reader.Read("StatoVisura", this.m_StatoVisura);
                this.m_ValutazioneAmministrazione = reader.Read("VALAMM", this.m_ValutazioneAmministrazione);
                this.m_CensimentoDatoreDiLavoro = reader.Read("CENSDATLAV", this.m_CensimentoDatoreDiLavoro);
                this.m_CensimentoSedeOperativa = reader.Read("CENSSEDOP", this.m_CensimentoSedeOperativa);
                this.m_VariazioneDenominazione = reader.Read("VARIAZDENOM", this.m_VariazioneDenominazione);
                this.m_Sblocco = reader.Read("SBLOCCO", this.m_Sblocco);
                this.m_IDAmministrazione = reader.Read("IDAmministrazione", this.m_IDAmministrazione);
                this.m_RagioneSociale = reader.Read("RagioneSociale", this.m_RagioneSociale);
                this.m_OggettoSociale = reader.Read("OggettoSociale", this.m_OggettoSociale);
                this.m_CodiceFiscale = reader.Read("CodiceFiscale", this.m_CodiceFiscale);
                this.m_PartitaIVA = reader.Read("PartitaIVA", this.m_PartitaIVA);
                this.m_ResponsabileDaContattare = reader.Read("NomeResponsabile", this.m_ResponsabileDaContattare);
                this.m_Qualifica = reader.Read("QualificaResponsabile", this.m_Qualifica);
                m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", m_Indirizzo.Provincia);
                m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", m_Indirizzo.Citta);
                m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", m_Indirizzo.CAP);
                m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                m_Indirizzo.SetChanged(false);
                m_Telefono = reader.Read("Telefono", this.m_Telefono);
                m_Fax = reader.Read("Fax", this.m_Fax);
                m_IndirizzoeMail = reader.Read("eMail", this.m_IndirizzoeMail);
                m_IndirizzoDiNotifica.Provincia = reader.Read("IndirizzoN_Provincia", m_IndirizzoDiNotifica.Provincia);
                m_IndirizzoDiNotifica.Citta = reader.Read("IndirizzoN_Citta", m_IndirizzoDiNotifica.Citta);
                m_IndirizzoDiNotifica.CAP = reader.Read("IndirizzoN_CAP", m_IndirizzoDiNotifica.CAP);
                m_IndirizzoDiNotifica.ToponimoViaECivico = reader.Read("IndirizzoN_Via", m_IndirizzoDiNotifica.ToponimoViaECivico);
                m_IndirizzoDiNotifica.SetChanged(false);
                m_TelefonoDiNotifica = reader.Read("TelefonoN", this.m_TelefonoDiNotifica);
                m_FaxDiNotifica = reader.Read("FaxN", this.m_FaxDiNotifica);
                m_ConvenzionePresente = reader.Read("CONVSINO", this.m_ConvenzionePresente);
                m_CodiceODescrizioneConvenzione = reader.Read("CODCONV", this.m_CodiceODescrizioneConvenzione);
                m_NumeroDipendenti = reader.Read("NumeroDipendenti", this.m_NumeroDipendenti);
                m_AmministrazioneSottoscriveMODPREST_008 = reader.Read("AMMMODPRST008", this.m_AmministrazioneSottoscriveMODPREST_008);
                m_NoteOInfoSullaSocieta = reader.Read("NoteSocieta", this.m_NoteOInfoSullaSocieta);
                m_IDBustaPaga = reader.Read("IDBustaPaga", this.m_IDBustaPaga);
                m_IDMotivoRichiestaSblocco = reader.Read("IDMotivoSblocco", this.m_IDMotivoRichiestaSblocco);
                m_CodiceAmministrazioneCL = reader.Read("CODAMMCL", this.m_CodiceAmministrazioneCL);
                m_StatoAmministrazioneCL = reader.Read("STATOAMMCL", this.m_StatoAmministrazioneCL);
                m_CodiceDatoreLavoroCL = reader.Read("CODDLAVCL", this.m_CodiceDatoreLavoroCL);
                m_RagioneSocialeSOP = reader.Read("RAGSOCSOP", this.m_RagioneSocialeSOP);
                m_ResponsabileDaContattareSOP = reader.Read("RESPCONTSOP", this.m_ResponsabileDaContattareSOP);
                m_QualificaSOP = reader.Read("QUALIFSOP", this.m_QualificaSOP);
                m_IndirizzoSO.Provincia = reader.Read("IndirizzoSO_Provincia", m_IndirizzoSO.Provincia);
                m_IndirizzoSO.Citta = reader.Read("IndirizzoSO_Citta", m_IndirizzoSO.Citta);
                m_IndirizzoSO.CAP = reader.Read("IndirizzoSO_CAP", m_IndirizzoSO.CAP);
                m_IndirizzoSO.ToponimoViaECivico = reader.Read("IndirizzoSO_Via", m_IndirizzoSO.ToponimoViaECivico);
                m_IndirizzoSO.SetChanged(false);
                m_TelefonoSO = reader.Read("TelefonoSO", this.m_TelefonoSO);
                m_FaxSO = reader.Read("FaxSO", this.m_FaxSO);
                m_ConvenzionePresenteSO = reader.Read("CONVSINOSO", this.m_ConvenzionePresenteSO);
                m_CodiceODescrizioneConvenzioneSO = reader.Read("CODCONVSO", this.m_CodiceODescrizioneConvenzioneSO);
                m_IDBustaPagaSO = reader.Read("IDBustaPagaSO", this.m_IDBustaPagaSO);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDRichiedente", typeof(int), 1);
                c = table.Fields.Ensure("NomeRichiedente", typeof(string), 255);
                c = table.Fields.Ensure("IDPresaInCaricoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomePresaInCaricoDa", typeof(string), 255);
                c = table.Fields.Ensure("DataPresaInCarico", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataCompletamento", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoVisura", typeof(int), 1);
                c = table.Fields.Ensure("VALAMM", typeof(bool), 1);
                c = table.Fields.Ensure("CENSDATLAV", typeof(bool), 1);
                c = table.Fields.Ensure("CENSSEDOP", typeof(bool), 1);
                c = table.Fields.Ensure("VARIAZDENOM", typeof(bool), 1);
                c = table.Fields.Ensure("SBLOCCO", typeof(bool), 1);
                c = table.Fields.Ensure("IDAmministrazione", typeof(int), 1);
                c = table.Fields.Ensure("RagioneSociale", typeof(string), 255);
                c = table.Fields.Ensure("OggettoSociale", typeof(string), 0);
                c = table.Fields.Ensure("CodiceFiscale", typeof(string), 255);
                c = table.Fields.Ensure("PartitaIVA", typeof(string), 255);
                c = table.Fields.Ensure("NomeResponsabile", typeof(string), 255);
                c = table.Fields.Ensure("QualificaResponsabile", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Citta", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_CAP", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Via", typeof(string), 255);
                c = table.Fields.Ensure("Telefono", typeof(string), 255);
                c = table.Fields.Ensure("Fax", typeof(string), 255);
                c = table.Fields.Ensure("eMail", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoN_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoN_Citta", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoN_CAP", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoN_Via", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoN", typeof(string), 255);
                c = table.Fields.Ensure("FaxN", typeof(string), 255);
                c = table.Fields.Ensure("CONVSINO", typeof(bool), 1);
                c = table.Fields.Ensure("CODCONV", typeof(string), 255);
                c = table.Fields.Ensure("NumeroDipendenti", typeof(int), 1);
                c = table.Fields.Ensure("AMMMODPRST008", typeof(bool), 1);
                c = table.Fields.Ensure("NoteSocieta", typeof(string), 0);
                c = table.Fields.Ensure("IDBustaPaga", typeof(int), 1);
                c = table.Fields.Ensure("IDMotivoSblocco", typeof(int), 1);
                c = table.Fields.Ensure("CODAMMCL", typeof(string), 255);
                c = table.Fields.Ensure("STATOAMMCL", typeof(string), 255);
                c = table.Fields.Ensure("CODDLAVCL", typeof(string), 255);
                c = table.Fields.Ensure("RAGSOCSOP", typeof(string), 255);
                c = table.Fields.Ensure("RESPCONTSOP", typeof(string), 255);
                c = table.Fields.Ensure("QUALIFSOP", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoSO_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoSO_Citta", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoSO_CAP", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoSO_Via", typeof(string), 255);
                c = table.Fields.Ensure("TelefonoSO", typeof(string), 255);
                c = table.Fields.Ensure("FaxSO", typeof(string), 255);
                c = table.Fields.Ensure("CONVSINOSO", typeof(bool), 1);
                c = table.Fields.Ensure("CODCONVSO", typeof(string), 255);
                c = table.Fields.Ensure("IDBustaPagaSO", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDate", new string[] { "Data", "DataPresaInCarico", "DataCompletamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRichiedente", new string[] { "IDRichiedente", "NomeRichiedente", "IDPresaInCaricoDa", "NomePresaInCaricoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoVisura", new string[] { "StatoVisura", "VALAMM", "CENSDATLAV", "CENSSEDOP" , "VARIAZDENOM", "SBLOCCO" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAmministrazione", new string[] { "IDAmministrazione", "RagioneSociale", "CodiceFiscale", "PartitaIVA" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxResponsabile", new string[] { "NomeResponsabile", "OggettoSociale", "QualificaResponsabile" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo", new string[] { "Indirizzo_Provincia", "Indirizzo_Citta", "Indirizzo_CAP", "Indirizzo_Via" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTelefoni", new string[] { "Telefono", "Fax", "eMail" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzoN", new string[] { "IndirizzoN_Provincia", "IndirizzoN_Citta", "IndirizzoN_CAP", "IndirizzoN_Via" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTelefoniN", new string[] { "TelefonoN", "FaxN" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri2", new string[] { "CONVSINO", "CODCONV", "AMMMODPRST008" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatistiche", new string[] { "NumeroDipendenti", "NoteSocieta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri3", new string[] { "CODAMMCL", "STATOAMMCL", "CODDLAVCL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri4", new string[] { "IDBustaPaga", "IDMotivoSblocco"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri5", new string[] { "RAGSOCSOP", "RESPCONTSOP", "QUALIFSOP" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzoSO", new string[] { "IndirizzoSO_Provincia", "IndirizzoSO_Citta", "IndirizzoSO_CAP", "IndirizzoSO_Via" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTelefonoSO", new string[] { "TelefonoSO", "FaxSO", "CONVSINOSO", "CODCONVSO", "IDBustaPagaSO" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("IDRichiedente", IDRichiedente);
                writer.WriteAttribute("NomeRichiedente", m_NomeRichiedente);
                writer.WriteAttribute("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.WriteAttribute("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("DataCompletamento", m_DataCompletamento);
                writer.WriteAttribute("StatoVisura", (int?)m_StatoVisura);
                writer.WriteAttribute("VALAMM", m_ValutazioneAmministrazione);
                writer.WriteAttribute("CENSDATLAV", m_CensimentoDatoreDiLavoro);
                writer.WriteAttribute("CENSSEDOP", m_CensimentoSedeOperativa);
                writer.WriteAttribute("VARIAZDENOM", m_VariazioneDenominazione);
                writer.WriteAttribute("SBLOCCO", m_Sblocco);
                writer.WriteAttribute("IDAmministrazione", IDAmministrazione);
                writer.WriteAttribute("RagioneSociale", m_RagioneSociale);
                writer.WriteAttribute("OggettoSociale", m_OggettoSociale);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                writer.WriteAttribute("PartitaIVA", m_PartitaIVA);
                writer.WriteAttribute("NomeResponsabile", m_ResponsabileDaContattare);
                writer.WriteAttribute("QualificaResponsabile", m_Qualifica);
                writer.WriteAttribute("Telefono", m_Telefono);
                writer.WriteAttribute("Fax", m_Fax);
                writer.WriteAttribute("eMail", m_IndirizzoeMail);
                writer.WriteAttribute("TelefonoN", m_TelefonoDiNotifica);
                writer.WriteAttribute("FaxN", m_FaxDiNotifica);
                writer.WriteAttribute("CONVSINO", m_ConvenzionePresente);
                writer.WriteAttribute("CODCONV", m_CodiceODescrizioneConvenzione);
                writer.WriteAttribute("NumeroDipendenti", m_NumeroDipendenti);
                writer.WriteAttribute("AMMMODPRST008", m_AmministrazioneSottoscriveMODPREST_008);
                writer.WriteAttribute("NoteSocieta", m_NoteOInfoSullaSocieta);
                writer.WriteAttribute("IDBustaPaga", IDBustaPaga);
                writer.WriteAttribute("IDMotivoSblocco", IDMotivoRichiestaSblocco);
                writer.WriteAttribute("CODAMMCL", m_CodiceAmministrazioneCL);
                writer.WriteAttribute("STATOAMMCL", m_StatoAmministrazioneCL);
                writer.WriteAttribute("CODDLAVCL", m_CodiceDatoreLavoroCL);
                writer.WriteAttribute("RAGSOCSOP", m_RagioneSocialeSOP);
                writer.WriteAttribute("RESPCONTSOP", m_ResponsabileDaContattareSOP);
                writer.WriteAttribute("QUALIFSOP", m_QualificaSOP);
                writer.WriteAttribute("TelefonoSO", m_TelefonoSO);
                writer.WriteAttribute("FaxSO", m_FaxSO);
                writer.WriteAttribute("CONVSINOSO", m_ConvenzionePresenteSO);
                writer.WriteAttribute("CODCONVSO", m_CodiceODescrizioneConvenzioneSO);
                writer.WriteAttribute("IDBustaPagaSO", IDBustaPagaSO);
                base.XMLSerialize(writer);
                writer.WriteTag("IndirizzoSO", m_IndirizzoSO);
                writer.WriteTag("Indirizzo", m_Indirizzo);
                writer.WriteTag("IndirizzoN", m_IndirizzoDiNotifica);
                writer.WriteTag("AltriAllegatiSO", AltriAllegatiSO);
                writer.WriteTag("AltriAllegati", AltriAllegati);
                writer.WriteTag("DocumentiProdotti", DocumentiProdotti);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDRichiedente":
                        {
                            m_IDRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRichiedente":
                        {
                            m_NomeRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataCompletamento":
                        {
                            m_DataCompletamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoVisura":
                        {
                            m_StatoVisura = (StatoVisura)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "VALAMM":
                        {
                            m_ValutazioneAmministrazione = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "CENSDATLAV":
                        {
                            m_CensimentoDatoreDiLavoro = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "CENSSEDOP":
                        {
                            m_CensimentoSedeOperativa = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "VARIAZDENOM":
                        {
                            m_VariazioneDenominazione = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "SBLOCCO":
                        {
                            m_Sblocco = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDAmministrazione":
                        {
                            m_IDAmministrazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RagioneSociale":
                        {
                            m_RagioneSociale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "OggettoSociale":
                        {
                            m_OggettoSociale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceFiscale":
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PartitaIVA":
                        {
                            m_PartitaIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeResponsabile":
                        {
                            m_ResponsabileDaContattare = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "QualificaResponsabile":
                        {
                            m_Qualifica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "Telefono":
                        {
                            m_Telefono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Fax":
                        {
                            m_Fax = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "eMail":
                        {
                            m_IndirizzoeMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IndirizzoN":
                        {
                            m_IndirizzoDiNotifica = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "TelefonoN":
                        {
                            m_TelefonoDiNotifica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FaxN":
                        {
                            m_FaxDiNotifica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CONVSINO":
                        {
                            m_ConvenzionePresente = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "CODCONV":
                        {
                            m_CodiceODescrizioneConvenzione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroDipendenti":
                        {
                            m_NumeroDipendenti = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AMMMODPRST008":
                        {
                            m_AmministrazioneSottoscriveMODPREST_008 = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "NoteSocieta":
                        {
                            m_NoteOInfoSullaSocieta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDBustaPaga":
                        {
                            m_IDBustaPaga = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDMotivoSblocco":
                        {
                            m_IDMotivoRichiestaSblocco = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AltriAllegati":
                        {
                            m_AltriAllegati = (Sistema.CAttachmentsCollection)fieldValue;
                            m_AltriAllegati.SetOwner(this);
                            m_AltriAllegati.SetContesto("AltriAllegati", 0);
                            break;
                        }

                    case "DocumentiProdotti":
                        {
                            m_DocumentiProdotti = (Sistema.CAttachmentsCollection)fieldValue;
                            m_DocumentiProdotti.SetOwner(this);
                            m_DocumentiProdotti.SetContesto("DocumentiProdotti", 0);
                            break;
                        }

                    case "CODAMMCL":
                        {
                            m_CodiceAmministrazioneCL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "STATOAMMCL":
                        {
                            m_StatoAmministrazioneCL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CODDLAVCL":
                        {
                            m_CodiceDatoreLavoroCL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RAGSOCSOP":
                        {
                            m_RagioneSocialeSOP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RESPCONTSOP":
                        {
                            m_ResponsabileDaContattareSOP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "QUALIFSOP":
                        {
                            m_QualificaSOP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IndirizzoSO":
                        {
                            m_IndirizzoSO = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "TelefonoSO":
                        {
                            m_TelefonoSO = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FaxSO":
                        {
                            m_FaxSO = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CONVSINOSO":
                        {
                            m_ConvenzionePresenteSO = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "CODCONVSO":
                        {
                            m_CodiceODescrizioneConvenzioneSO = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDBustaPagaSO":
                        {
                            IDBustaPagaSO = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AltriAllegatiSO":
                        {
                            m_AltriAllegatiSO = (Sistema.CAttachmentsCollection)fieldValue;
                            m_AltriAllegatiSO.SetOwner(this);
                            m_AltriAllegatiSO.SetContesto("AltriAllegatiSO", 0);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta loggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return
                    DMD.Strings.ConcatArray(
                            "Visura del " , Sistema.Formats.FormatUserDate(m_Data) ,
                            " per " + m_RagioneSociale
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.IDAmministrazione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Visura) && this.Equals((Visura)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Visura obj)
            {
                return base.Equals(obj)
                        && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                        && DMD.Integers.EQ(this.m_IDRichiedente, obj.m_IDRichiedente)
                        && DMD.Strings.EQ(this.m_NomeRichiedente, obj.m_NomeRichiedente)
                        && DMD.Integers.EQ(this.m_IDPresaInCaricoDa, obj.m_IDPresaInCaricoDa)
                        && DMD.Strings.EQ(this.m_NomePresaInCaricoDa, obj.m_NomePresaInCaricoDa)
                        && DMD.DateUtils.EQ(this.m_DataPresaInCarico, obj.m_DataPresaInCarico)
                        && DMD.DateUtils.EQ(this.m_DataCompletamento, obj.m_DataCompletamento)
                        && DMD.Integers.EQ((int)this.m_StatoVisura, (int)obj.m_StatoVisura)
                        && DMD.Booleans.EQ(this.m_ValutazioneAmministrazione, obj.m_ValutazioneAmministrazione)
                        && DMD.Booleans.EQ(this.m_CensimentoDatoreDiLavoro, obj.m_CensimentoDatoreDiLavoro)
                        && DMD.Booleans.EQ(this.m_CensimentoSedeOperativa, obj.m_CensimentoSedeOperativa)
                        && DMD.Booleans.EQ(this.m_VariazioneDenominazione, obj.m_VariazioneDenominazione)
                        && DMD.Booleans.EQ(this.m_Sblocco, obj.m_Sblocco)
                        && DMD.Integers.EQ(this.m_IDAmministrazione, obj.m_IDAmministrazione)
                        && DMD.Strings.EQ(this.m_RagioneSociale, obj.m_RagioneSociale)
                        && DMD.Strings.EQ(this.m_OggettoSociale, obj.m_OggettoSociale)
                        && DMD.Strings.EQ(this.m_CodiceFiscale, obj.m_CodiceFiscale)
                        && DMD.Strings.EQ(this.m_PartitaIVA, obj.m_PartitaIVA)
                        && DMD.Strings.EQ(this.m_ResponsabileDaContattare, obj.m_ResponsabileDaContattare)
                        && DMD.Strings.EQ(this.m_Qualifica, obj.m_Qualifica)
                        && this.m_Indirizzo.Equals(obj.m_Indirizzo)
                        && DMD.Strings.EQ(this.m_Telefono, obj.m_Telefono)
                        && DMD.Strings.EQ(this.m_Fax, obj.m_Fax)
                        && DMD.Strings.EQ(this.m_IndirizzoeMail, obj.m_IndirizzoeMail)
                        && this.m_IndirizzoDiNotifica.Equals(obj.m_IndirizzoDiNotifica)
                        && DMD.Strings.EQ(this.m_TelefonoDiNotifica, obj.m_TelefonoDiNotifica)
                        && DMD.Strings.EQ(this.m_FaxDiNotifica, obj.m_FaxDiNotifica)
                        && DMD.Booleans.EQ(this.m_ConvenzionePresente, obj.m_ConvenzionePresente)
                        && DMD.Strings.EQ(this.m_CodiceODescrizioneConvenzione, obj.m_CodiceODescrizioneConvenzione)
                        && DMD.Integers.EQ(this.m_NumeroDipendenti, obj.m_NumeroDipendenti)
                        && DMD.Booleans.EQ(this.m_AmministrazioneSottoscriveMODPREST_008, obj.m_AmministrazioneSottoscriveMODPREST_008)
                        && DMD.Strings.EQ(this.m_NoteOInfoSullaSocieta, obj.m_NoteOInfoSullaSocieta)
                        && DMD.Integers.EQ(this.m_IDBustaPaga, obj.m_IDBustaPaga)
                        && DMD.Integers.EQ(this.m_IDMotivoRichiestaSblocco, obj.m_IDMotivoRichiestaSblocco)
                        //private int ;
                        //[NonSerialized] private Sistema.CAttachment m_MotivoRichiestaSblocco;
                        //[NonSerialized] private Sistema.CAttachmentsCollection m_AltriAllegati;
                        //[NonSerialized] private Sistema.CAttachmentsCollection m_DocumentiProdotti;
                        && DMD.Strings.EQ(this.m_CodiceAmministrazioneCL, obj.m_CodiceAmministrazioneCL)
                        && DMD.Strings.EQ(this.m_StatoAmministrazioneCL, obj.m_StatoAmministrazioneCL)
                        && DMD.Strings.EQ(this.m_CodiceDatoreLavoroCL, obj.m_CodiceDatoreLavoroCL)
                        && DMD.Strings.EQ(this.m_RagioneSocialeSOP, obj.m_RagioneSocialeSOP)
                        && DMD.Strings.EQ(this.m_ResponsabileDaContattareSOP, obj.m_ResponsabileDaContattareSOP)
                        && DMD.Strings.EQ(this.m_QualificaSOP, obj.m_QualificaSOP)
                        && this.m_IndirizzoSO.Equals(obj.m_IndirizzoSO)
                        && DMD.Strings.EQ(this.m_TelefonoSO, obj.m_TelefonoSO)
                        && DMD.Strings.EQ(this.m_FaxSO, obj.m_FaxSO)
                        && DMD.Booleans.EQ(this.m_ConvenzionePresenteSO, obj.m_ConvenzionePresenteSO)
                        && DMD.Strings.EQ(this.m_CodiceODescrizioneConvenzioneSO, obj.m_CodiceODescrizioneConvenzioneSO)
                        && DMD.Integers.EQ(this.m_IDBustaPagaSO, obj.m_IDBustaPagaSO)
                        ;
            //[NonSerialized] private Sistema.CAttachment m_BustaPagaSO;
            //[NonSerialized] private Sistema.CAttachmentsCollection m_AltriAllegatiSO;
            }

            /// <summary>
            /// Compara due visure per data richiesta
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Visura other)
            {
                return DMD.DateUtils.Compare(this.Data, other.Data);
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((Visura)obj); }
        }
    }
}