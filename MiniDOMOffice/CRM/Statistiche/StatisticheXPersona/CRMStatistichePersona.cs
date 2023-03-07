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
using static minidom.CustomerCalls;


namespace minidom
{
    public partial class CustomerCalls
    {
        /// <summary>
        /// Flag per le statistiche CRM sulle persone
        /// </summary>
        [Flags]
        public enum PersonFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Questo flag indica che la riga è relativa ad una persona fisica
            /// </summary>
            /// <remarks></remarks>
            PersonaFisica = 1,

            /// <summary>
            /// Questo flag indica che la riga è relativa ad una persona da ricontattabile
            /// </summary>
            /// <remarks></remarks>
            Ricontattabile = 2,

            /// <summary>
            /// Questo flag indica che la persona è deceduta
            /// </summary>
            /// <remarks></remarks>
            Deceduto = 4,

            /// <summary>
            /// Flag che indica che si tratta di un cliente non ancora acquisito
            /// </summary>
            /// <remarks></remarks>
            ClienteInAquisizione = 8,

            /// <summary>
            /// Flag che indica che si tratta di un cliente acquisito (almeno una pratica)
            /// </summary>
            /// <remarks></remarks>
            ClienteAcquisito = 16
        }

        /// <summary>
        /// Statistiche sulle persone
        /// </summary>
        [Serializable]
        public class CRMStatistichePersona
            : minidom.Databases.DBObjectBase
        {
            private int m_IDPersona;
            [NonSerialized] private Anagrafica.CPersona m_Persona;
            private string m_NomePersona;
            private string m_DettaglioEsito;
            private string m_DettaglioEsito1;
            private int m_ConteggioVisiteRicevute;
            private int m_ConteggioVisiteEffettuate;
            private int m_IDUltimaVisita;
            [NonSerialized] private CVisita m_UltimaVisita;
            private DateTime? m_DataUltimaVisita;
            [NonSerialized] private CContattoUtente m_UltimoContattoNo;
            private int m_UltimoContattoNoID;
            private DateTime? m_DataUltimoContattoNo;
            private int m_ConteggioNoRispo;
            [NonSerialized] private CContattoUtente m_UltimoContattoOk;
            private int m_UltimoContattoOkID;
            private DateTime? m_DataUltimoContattoOk;
            private int m_ConteggioRisp;
            private string m_Note;
            //private PersonFlags m_Flags;

            // Private m_ProssimoRicontatto As CRicontatto
            // Private m_ProssimoRicontattoID As Integer
            private DateTime? m_DataProssimoRicontatto;
            private string m_MotivoProssimoRicontatto;
            private int m_IDPuntoOperativo;
            private string m_NomePuntoOperativo;
            [NonSerialized] private Anagrafica.CUfficio m_PuntoOperativo;
            private string m_DescrizioneStato;
            private string m_IconURL;
            private DateTime? m_DataAggiornamento;
            private DateTime? m_DataUltimaOperazione;
            private CCollection<CPersonWatchCond> m_CondizioniAttenzione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatistichePersona()
            {
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_DettaglioEsito = "";
                m_DettaglioEsito1 = "";
                m_UltimoContattoNo = null;
                m_UltimoContattoNoID = 0;
                m_DataUltimoContattoNo = default;
                m_IDUltimaVisita = 0;
                m_UltimaVisita = null;
                m_DataUltimaVisita = default;
                m_UltimoContattoOk = null;
                m_UltimoContattoOkID = 0;
                m_DataUltimoContattoOk = default;
                m_ConteggioRisp = 0;
                m_ConteggioNoRispo = 0;
                m_Flags = (int) PersonFlags.None;
                m_NomePersona = "";
                m_DataProssimoRicontatto = default;
                m_MotivoProssimoRicontatto = "";
                m_IDPuntoOperativo = 0;
                m_NomePuntoOperativo = "";
                m_PuntoOperativo = null;
                m_DescrizioneStato = "";
                m_IconURL = "";
                m_DataAggiornamento = default;
                m_DataUltimaOperazione = default;
                m_ConteggioVisiteRicevute = 0;
                m_ConteggioVisiteEffettuate = 0;
            }

            /// <summary>
            /// Restituisce o imposta una stringa che riporta il valore omonimo impostato per l'oggetto CPersona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioEsito
            {
                get
                {
                    return m_DettaglioEsito;
                }

                set
                {
                    string oldValue = m_DettaglioEsito;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsito = value;
                    DoChanged("DettaglioEsito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che riporta il valore omonimo impostato per l'oggetto CPersona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioEsito1
            {
                get
                {
                    return m_DettaglioEsito1;
                }

                set
                {
                    string oldValue = m_DettaglioEsito1;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsito1 = value;
                    DoChanged("DettaglioEsito1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'ultima visita (effettuata o ricevuta) registrata per questa persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDUltimaVisita
            {
                get
                {
                    return DBUtils.GetID(m_UltimaVisita, m_IDUltimaVisita);
                }

                set
                {
                    int oldValue = IDUltimaVisita;
                    if (oldValue == value)
                        return;
                    m_IDUltimaVisita = value;
                    m_UltimaVisita = null;
                    DoChanged("IDUltimaVisita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ultima visita
            /// </summary>
            public CVisita UltimaVisita
            {
                get
                {
                    if (m_UltimaVisita is null)
                        m_UltimaVisita = minidom.CustomerCalls.Visite.GetItemById(m_IDUltimaVisita);
                    return m_UltimaVisita;
                }

                set
                {
                    var oldValue = UltimaVisita;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_UltimaVisita = value;
                    m_IDUltimaVisita = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_DataUltimaVisita = value.Data;
                    DoChanged("UltimaVisita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora dell'ultima visita ricevuta o effettuata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataUltimaVisita
            {
                get
                {
                    return m_DataUltimaVisita;
                }

                set
                {
                    var oldValue = m_DataUltimaVisita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataUltimaVisita = value;
                    DoChanged("DataUltimaVisita", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice la collezione delle condizioni di attenzione registrate per questa anagrafica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CPersonWatchCond> CondizioniAttenzione
            {
                get
                {
                    if (m_CondizioniAttenzione is null)
                        m_CondizioniAttenzione = new CCollection<CPersonWatchCond>();
                    return m_CondizioniAttenzione;
                }
            }

            /// <summary>
            /// Restituisce il watchdog generato dalla sorgente
            /// </summary>
            /// <param name="source"></param>
            /// <param name="tag"></param>
            /// <returns></returns>
            public CPersonWatchCond GetAttenzione(object source, string tag)
            {
                string sType = "";
                int sID = 0;
                if (source is object)
                {
                    sType = DMD.RunTime.vbTypeName(source);
                    sID = DBUtils.GetID(source, 0);
                }

                foreach (CPersonWatchCond c in CondizioniAttenzione)
                {
                    if ((c.SourceType ?? "") == (sType ?? "") && c.SourceID == sID && (c.Tag ?? "") == (tag ?? ""))
                    {
                        return c;
                    }
                }

                return null;
            }

            /// <summary>
            /// Aggiunge una condizione di attenzione
            /// </summary>
            /// <param name="source"></param>
            /// <param name="descrizione"></param>
            /// <param name="tag"></param>
            /// <returns></returns>
            public CPersonWatchCond AggiungiAttenzione(object source, string descrizione, string tag)
            {
                var c = GetAttenzione(source, tag);
                bool hasChanged = false;
                if (c is null)
                {
                    c = new CPersonWatchCond();
                    c.Source = source;
                    c.Data = DMD.DateUtils.Now();
                    c.Tag = tag;
                    CondizioniAttenzione.Add(c);
                    hasChanged = true;
                }

                if ((c.Descrizione ?? "") != (DMD.Strings.Trim(descrizione) ?? ""))
                {
                    hasChanged = true;
                    c.Descrizione = descrizione;
                }

                if (hasChanged)
                {
                    DoChanged("CondizioniAttenzione");
                }

                return c;
            }

            /// <summary>
            /// Rimuove la condizione generata dalla sorgente
            /// </summary>
            /// <param name="source"></param>
            /// <param name="tag"></param>
            /// <returns></returns>
            public CPersonWatchCond RimuoviAttenzione(object source, string tag)
            {
                string sType = "";
                int sID = 0;
                if (source is object)
                {
                    sType = DMD.RunTime.vbTypeName(source);
                    sID = DBUtils.GetID(source, 0);
                }

                CPersonWatchCond c = null;
                int i = 0;
                while (i < CondizioniAttenzione.Count)
                {
                    c = CondizioniAttenzione[i];
                    if ((c.SourceType ?? "") == (sType ?? "") && c.SourceID == sID && (c.Tag ?? "") == (tag ?? ""))
                    {
                        CondizioniAttenzione.RemoveAt(i);
                        DoChanged("CondizioniAttenzione");
                        break;
                    }

                    i += 1;
                }

                return c;
            }

            /// <summary>
            /// Restituisce il valore della gravita maggiore tra le condizioni di attenzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Gravita
            {
                get
                {
                    int ret = 0;
                    foreach (CPersonWatchCond c in CondizioniAttenzione)
                    {
                        if (c.Gravita > ret)
                            ret = c.Gravita;
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultimo aggiornamento di stato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataUltimaOperazione
            {
                get
                {
                    return m_DataUltimaOperazione;
                }
                // Set(value As Date?)
                // Dim oldValue As Date? = Me.m_DataUltimaOperazione
                // If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                // Me.m_DataUltimaOperazione = value
                // Me.DoChanged("DataUltimaOperazione", value, oldValue)
                // End Set
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultimo aggiornamento di questo record
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataAggiornamento
            {
                get
                {
                    return m_DataAggiornamento;
                }

                set
                {
                    var oldValue = m_DataAggiornamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAggiornamento = value;
                    DoChanged("DataAggiornamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso dell'immagine associata alla persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    string oldValue = m_IconURL;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la prossima data di ricontatto impostata per la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataProssimoRicontatto
            {
                get
                {
                    return m_DataProssimoRicontatto;
                }

                set
                {
                    var oldValue = m_DataProssimoRicontatto;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataProssimoRicontatto = value;
                    DoChanged("DataProssimoRicontatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo del prossimo ricontattol
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string MotivoProssimoRicontatto
            {
                get
                {
                    return m_MotivoProssimoRicontatto;
                }

                set
                {
                    string oldValue = m_MotivoProssimoRicontatto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoProssimoRicontatto = value;
                    DoChanged("MotivoProssimoRicontatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta L'ID del punto operativo di appartenenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPuntoOperativo
            {
                get
                {
                    return DBUtils.GetID(m_PuntoOperativo, m_IDPuntoOperativo);
                }

                set
                {
                    int oldValue = IDPuntoOperativo;
                    if (oldValue == value)
                        return;
                    m_IDPuntoOperativo = value;
                    m_PuntoOperativo = null;
                    DoChanged("IDPuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il punto operativo di appartenenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CUfficio PuntoOperativo
            {
                get
                {
                    if (m_PuntoOperativo is null)
                        m_PuntoOperativo = Anagrafica.Uffici.GetItemById(m_IDPuntoOperativo);
                    return m_PuntoOperativo;
                }

                set
                {
                    var oldValue = PuntoOperativo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PuntoOperativo = value;
                    m_IDPuntoOperativo = DBUtils.GetID(value, 0);
                    m_NomePuntoOperativo = (value is object)? value.Nome : "";
                    DoChanged("PuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del punto operativo di appartenenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePuntoOperativo
            {
                get
                {
                    return m_NomePuntoOperativo;
                }

                set
                {
                    string oldValue = m_NomePuntoOperativo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePuntoOperativo = value;
                    DoChanged("NomePuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    string oldValue = m_NomePersona;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'elenco dei flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new PersonFlags Flags
            {
                get
                {
                    return (PersonFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la riga è relativa ad una persona fisica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool isPersonaFisica
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, PersonFlags.PersonaFisica);
                }

                set
                {
                    if (isPersonaFisica == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, PersonFlags.PersonaFisica, value);
                    DoChanged("isPersonaFisica", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la persona è ricontattabile (non deceduto e non ok ricontatti)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool isRicontattabile
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, PersonFlags.Ricontattabile);
                }

                set
                {
                    if (isRicontattabile == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, PersonFlags.Ricontattabile, value);
                    DoChanged("isRicontattabile", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la persona è deceduta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool isDeceduto
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, PersonFlags.Deceduto);
                }

                set
                {
                    if (isDeceduto == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, PersonFlags.Deceduto, value);
                    DoChanged("isDeceduta", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica che il contatto rappresenta un cliente in fase di acquisizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool isClienteInAcquisizione
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, PersonFlags.ClienteInAquisizione);
                }

                set
                {
                    if (isClienteInAcquisizione == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, PersonFlags.ClienteInAquisizione, value);
                    DoChanged("isClienteInAcquisizione", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica che il cliente è acquisito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool isClienteAcquisito
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, PersonFlags.ClienteAcquisito);
                }

                set
                {
                    if (isClienteAcquisito == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, PersonFlags.ClienteAcquisito, value);
                    DoChanged("isClienteAcquisito", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona a cui fa riferimento questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona a cui fa riferimento questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_NomePersona = value.Nominativo;
                        m_IconURL = value.IconURL;
                    }

                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(Anagrafica.CPersona value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ultimo contatto avuto con la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CContattoUtente UltimoContattoNo
            {
                get
                {
                    if (m_UltimoContattoNo is null)
                        m_UltimoContattoNo = CRM.GetItemById(m_UltimoContattoNoID);
                    return m_UltimoContattoNo;
                }

                set
                {
                    var oldValue = m_UltimoContattoNo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_UltimoContattoNo = value;
                    m_UltimoContattoNoID = DBUtils.GetID(value, 0);
                    DoChanged("UltimoContattoNo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'ultimo contatto avuto con la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int UltimoContattoNoID
            {
                get
                {
                    return DBUtils.GetID(m_UltimoContattoNo, m_UltimoContattoNoID);
                }

                set
                {
                    int oldValue = UltimoContattoNoID;
                    if (oldValue == value)
                        return;
                    m_UltimoContattoNo = null;
                    m_UltimoContattoNoID = value;
                    DoChanged("UltimoContattoNoID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ultimo contatto con esito positivo avuto con la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CContattoUtente UltimoContattoOk
            {
                get
                {
                    if (m_UltimoContattoOk is null)
                        m_UltimoContattoOk = CRM.GetItemById(m_UltimoContattoOkID);
                    return m_UltimoContattoOk;
                }

                set
                {
                    var oldValue = m_UltimoContattoOk;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_UltimoContattoOk = value;
                    m_UltimoContattoOkID = DBUtils.GetID(value, 0);
                    DoChanged("UltimoContattoOk", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'ultimo contatto con esito positivo con la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int UltimoContattoOkID
            {
                get
                {
                    return DBUtils.GetID(m_UltimoContattoOk, m_UltimoContattoOkID);
                }

                set
                {
                    int oldValue = UltimoContattoOkID;
                    if (oldValue == value)
                        return;
                    m_UltimoContattoOk = null;
                    m_UltimoContattoOkID = value;
                    DoChanged("UltimoContattoOkID", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta la data dell'ultimo contatto con esito positivo avuto con la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataUltimoContattoOk
            {
                get
                {
                    return m_DataUltimoContattoOk;
                }
                // Set(value As Date?)
                // Dim oldValue As Date? = Me.m_DataUltimoContattoOk
                // If (Calendar.Compare(oldValue, value) = 0) Then Exit Property
                // Me.m_DataUltimoContattoOk = value
                // Me.DoChanged("DataUltimoContattoOk", value, oldValue)
                // End Set
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultimo contatto senza risposta
            /// </summary>
            public DateTime? DataUltimoContattoNo
            {
                get
                {
                    return m_DataUltimoContattoNo;
                }
                // Set(value As Date?)
                // Dim oldValue As Date? = Me.m_DataUltimoContattoNo
                // If (Calendar.Compare(oldValue, value) = 0) Then Exit Property
                // Me.m_DataUltimoContattoNo = value
                // Me.DoChanged("DataUltimoContattoNo", value, oldValue)
                // End Set
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultimo contatto avuto con la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataUltimoContatto
            {
                get
                {
                    return DMD.DateUtils.Max(DataUltimoContattoOk, DataUltimoContattoNo);
                }
            }


            /// <summary>
            /// Restituisce o imposta il conteggio dei contatti con esito positivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ConteggioRisp
            {
                get
                {
                    return m_ConteggioRisp;
                }

                set
                {
                    int oldValue = m_ConteggioRisp;
                    if (oldValue == value)
                        return;
                    m_ConteggioRisp = value;
                    DoChanged("ConteggioRisp", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il conteggio dei contatti con esito non positivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ConteggioNoRisp
            {
                get
                {
                    return m_ConteggioNoRispo;
                }

                set
                {
                    int oldValue = m_ConteggioNoRispo;
                    if (oldValue == value)
                        return;
                    m_ConteggioNoRispo = value;
                    DoChanged("ConteggioNoRisp", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle note aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.Statistiche.StatistichePersona;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_UltimaChiamata";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("CRMStats: ", this.m_NomePersona, " [", this.IDPersona, "]");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_DataAggiornamento);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CRMStatistichePersona ) && this.Equals((CRMStatistichePersona)obj);
            }


            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRMStatistichePersona obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.Strings.EQ(this.m_DettaglioEsito, obj.m_DettaglioEsito)
                    && DMD.Strings.EQ(this.m_DettaglioEsito1, obj.m_DettaglioEsito1)
                    && DMD.Integers.EQ(this.m_ConteggioVisiteRicevute, obj.m_ConteggioVisiteRicevute)
                    && DMD.Integers.EQ(this.m_ConteggioVisiteEffettuate, obj.m_ConteggioVisiteEffettuate)
                    && DMD.Integers.EQ(this.m_IDUltimaVisita, obj.m_IDUltimaVisita)
                    && DMD.DateUtils.EQ(this.m_DataUltimaVisita, obj.m_DataUltimaVisita)
                    && DMD.Integers.EQ(this.m_UltimoContattoNoID, obj.m_UltimoContattoNoID)
                    && DMD.DateUtils.EQ(this.m_DataUltimoContattoNo, obj.m_DataUltimoContattoNo)
                    && DMD.Integers.EQ(this.m_ConteggioNoRispo, obj.m_ConteggioNoRispo)
                    && DMD.Integers.EQ(this.m_UltimoContattoOkID, obj.m_UltimoContattoOkID)
                    && DMD.DateUtils.EQ(this.m_DataUltimoContattoOk, obj.m_DataUltimoContattoOk)
                    && DMD.Integers.EQ(this.m_ConteggioRisp, obj.m_ConteggioRisp)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.DateUtils.EQ(this.m_DataProssimoRicontatto, obj.m_DataProssimoRicontatto)
                    && DMD.Strings.EQ(this.m_MotivoProssimoRicontatto, obj.m_MotivoProssimoRicontatto)
                    && DMD.Integers.EQ(this.m_IDPuntoOperativo, obj.m_IDPuntoOperativo)
                    && DMD.Strings.EQ(this.m_NomePuntoOperativo, obj.m_NomePuntoOperativo)
                    && DMD.Strings.EQ(this.m_DescrizioneStato, obj.m_DescrizioneStato)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.DateUtils.EQ(this.m_DataAggiornamento, obj.m_DataAggiornamento)
                    && DMD.DateUtils.EQ(this.m_DataUltimaOperazione, obj.m_DataUltimaOperazione)
                    ; //private CCollection<CPersonWatchCond> m_CondizioniAttenzione;
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPersona", IDPersona);
                writer.Write("IDTelefonata", UltimoContattoNoID);
                writer.Write("IDTelefonataOk", UltimoContattoOkID);
                writer.Write("DataUltimaTelefonataOk", m_DataUltimoContattoOk);
                writer.Write("DataUltimaTelefonata", m_DataUltimoContattoNo);
                writer.Write("ConteggioRisp", m_ConteggioRisp);
                writer.Write("ConteggioNoRispo", m_ConteggioNoRispo);
                writer.Write("Note", m_Note);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("IDPuntoOperativo", IDPuntoOperativo);
                writer.Write("MotivoProssimoRicontatto", m_MotivoProssimoRicontatto);
                writer.Write("DataProssimoRicontatto", m_DataProssimoRicontatto);
                writer.Write("IconURL", m_IconURL);
                writer.Write("DataAggiornamento", m_DataAggiornamento);
                writer.Write("DataUltimaOperazione", m_DataUltimaOperazione);
                writer.Write("Gravita", Gravita);
                writer.Write("Condizioni", DMD.XML.Utils.Serializer.Serialize(CondizioniAttenzione));
                writer.Write("ContaCondizioni", CondizioniAttenzione.Count);
                writer.Write("IDUltimaVisita", IDUltimaVisita);
                writer.Write("DataUltimaVisita", m_DataUltimaVisita);
                //writer.Write("DataAggiornamento1", DBUtils.ToDBDateStr(m_DataAggiornamento));
                writer.Write("DettaglioEsito", m_DettaglioEsito);
                writer.Write("DettaglioEsito1", m_DettaglioEsito1);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPersona = reader.Read("IDPersona", m_IDPersona);
                m_UltimoContattoNoID = reader.Read("IDTelefonata", m_UltimoContattoNoID);
                m_UltimoContattoOkID = reader.Read("IDTelefonataOk", m_UltimoContattoOkID);
                m_DataUltimoContattoOk = reader.Read("DataUltimaTelefonataOk", m_DataUltimoContattoOk);
                m_DataUltimoContattoNo = reader.Read("DataUltimaTelefonata", m_DataUltimoContattoNo);
                m_ConteggioRisp = reader.Read("ConteggioRisp", m_ConteggioRisp);
                m_ConteggioNoRispo = reader.Read("ConteggioNoRispo", m_ConteggioNoRispo);
                m_Flags = reader.Read("Flags", m_Flags);
                m_Note = reader.Read("Note", m_Note);
                m_NomePersona = reader.Read("NomePersona", m_NomePersona);
                m_IDPuntoOperativo = reader.Read("IDPuntoOperativo", m_IDPuntoOperativo);
                m_NomePuntoOperativo = reader.Read("NomePuntoOperativo", m_NomePuntoOperativo);
                m_MotivoProssimoRicontatto = reader.Read("MotivoProssimoRicontatto", m_MotivoProssimoRicontatto);
                m_DataProssimoRicontatto = reader.Read("DataProssimoRicontatto", m_DataProssimoRicontatto);
                m_IconURL = reader.Read("IconURL", m_IconURL);
                m_DataAggiornamento = reader.Read("DataAggiornamento", m_DataAggiornamento);
                m_DataUltimaOperazione = reader.Read("DataUltimaOperazione", m_DataUltimaOperazione);
                m_DataUltimaVisita = reader.Read("DataUltimaVisita", m_DataUltimaVisita);
                m_IDUltimaVisita = reader.Read("IDUltimaVisita", m_IDUltimaVisita);
                m_DettaglioEsito = reader.Read("DettaglioEsito", m_DettaglioEsito);
                m_DettaglioEsito1 = reader.Read("DettaglioEsito1", m_DettaglioEsito1);
                m_CondizioniAttenzione = new CCollection<CPersonWatchCond>();
                string tmp = reader.Read("Condizioni", "");
                if (!string.IsNullOrEmpty(tmp))
                    m_CondizioniAttenzione.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("IDTelefonata", typeof(int), 1);
                c = table.Fields.Ensure("IDTelefonataOk", typeof(int), 1);
                c = table.Fields.Ensure("DataUltimaTelefonataOk", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataUltimaTelefonata", typeof(DateTime), 1);
                c = table.Fields.Ensure("ConteggioRisp", typeof(int), 1);
                c = table.Fields.Ensure("ConteggioNoRispo", typeof(int), 1);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("IDPuntoOperativo", typeof(int), 1);
                c = table.Fields.Ensure("MotivoProssimoRicontatto", typeof(string), 255);
                c = table.Fields.Ensure("DataProssimoRicontatto", typeof(DateTime), 1);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("DataAggiornamento", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataUltimaOperazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("Gravita", typeof(int), 1);
                c = table.Fields.Ensure("Condizioni", typeof(string), 0);
                c = table.Fields.Ensure("ContaCondizioni", typeof(int), 1);
                c = table.Fields.Ensure("IDUltimaVisita", typeof(int), 1);
                c = table.Fields.Ensure("DataUltimaVisita", typeof(DateTime), 1);
                c = table.Fields.Ensure("DettaglioEsito", typeof(string), 255);
                c = table.Fields.Ensure("DettaglioEsito1", typeof(string), 255);
                //writer.Write("DataAggiornamento1", DBUtils.ToDBDateStr(m_DataAggiornamento));
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxAggiornamento", new string[] { "DataAggiornamento", "DataUltimaOperazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPuntoOp", new string[] { "IDPuntoOperativo", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTelefonataOk", new string[] { "IDTelefonataOk", "DataUltimaTelefonataOk" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTelefonataNo", new string[] { "IDTelefonata", "DataUltimaTelefonata" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxVisite", new string[] { "IDUltimaVisita", "DataUltimaVisita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxConteggi", new string[] { "ConteggioRisp", "ConteggioNoRispo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxProssimoApp", new string[] { "DataProssimoRicontatto", "MotivoProssimoRicontatto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEsiti", new string[] { "DettaglioEsito", "DettaglioEsito1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "ConteggioRisp", "Note" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxGravita", new string[] { "Gravita", "ContaCondizioni" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
                //c = table.Fields.Ensure("Condizioni", typeof(string), 0);
                 
                //writer.Write("DataAggiornamento1", DBUtils.ToDBDateStr(m_DataAggiornamento));
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("IDContattoNo", UltimoContattoNoID);
                writer.WriteAttribute("IDContattoOk", UltimoContattoOkID);
                writer.WriteAttribute("DataContattoOk", m_DataUltimoContattoOk);
                writer.WriteAttribute("DataContattoNo", m_DataUltimoContattoNo);
                writer.WriteAttribute("ConteggioRisp", m_ConteggioRisp);
                writer.WriteAttribute("ConteggioNoRispo", m_ConteggioNoRispo);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("NomePuntoOperativo", m_NomePuntoOperativo);
                writer.WriteAttribute("MotivoProssimoRicontatto", m_MotivoProssimoRicontatto);
                writer.WriteAttribute("DataProssimoRicontatto", m_DataProssimoRicontatto);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("DataAggiornamento", m_DataAggiornamento);
                writer.WriteAttribute("DataUltimaOperazione", m_DataUltimaOperazione);
                writer.WriteAttribute("IDUltimaVisita", IDUltimaVisita);
                writer.WriteAttribute("DataUltimaVisita", m_DataUltimaVisita);
                writer.WriteAttribute("DettaglioEsito", m_DettaglioEsito);
                writer.WriteAttribute("DettaglioEsito1", m_DettaglioEsito1);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
                if (writer.GetSetting("SerializeContatti", false))
                {
                    writer.WriteTag("ContattoOk", UltimoContattoOk);
                    writer.WriteTag("ContattoNo", UltimoContattoNo);
                }

                writer.WriteTag("Condizioni", CondizioniAttenzione);
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
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDContattoNo":
                        {
                            m_UltimoContattoNoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDContattoOk":
                        {
                            m_UltimoContattoOkID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataContattoOk":
                        {
                            m_DataUltimoContattoOk = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataContattoNo":
                        {
                            m_DataUltimoContattoNo = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ConteggioRisp":
                        {
                            m_ConteggioRisp = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConteggioNoRispo":
                        {
                            m_ConteggioNoRispo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                 
                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativo":
                        {
                            m_IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePuntoOperativo":
                        {
                            m_NomePuntoOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoProssimoRicontatto":
                        {
                            m_MotivoProssimoRicontatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataProssimoRicontatto":
                        {
                            m_DataProssimoRicontatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAggiornamento":
                        {
                            m_DataAggiornamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ContattoOk":
                        {
                            m_UltimoContattoOk = (CContattoUtente)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "ContattoNo":
                        {
                            m_UltimoContattoNo = (CContattoUtente)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "DataUltimaOperazione":
                        {
                            m_DataUltimaOperazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDUltimaVisita":
                        {
                            m_IDUltimaVisita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataUltimaVisita":
                        {
                            m_DataUltimaVisita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Condizioni":
                        {
                            m_CondizioniAttenzione = (CCollection<CPersonWatchCond>)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            m_DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioEsito1":
                        {
                            m_DettaglioEsito1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Aggiorna le informazioni sulla base della persona
            /// </summary>
            /// <remarks></remarks>
            public void AggiornaPersona()
            {
                if (Persona is null)
                    throw new ArgumentNullException("persona");
                PuntoOperativo = Persona.PuntoOperativo;
                NomePersona = Persona.Nominativo;
                isDeceduto = Persona.Deceduto;
                isRicontattabile = !isDeceduto;
                isPersonaFisica = Persona.TipoPersona == Anagrafica.TipoPersona.PERSONA_FISICA;
                IconURL = Persona.IconURL;
                DettaglioEsito = Persona.DettaglioEsito;
                DettaglioEsito1 = Persona.DettaglioEsito1;
                if (isRicontattabile && Persona.GetFlag(Anagrafica.PFlags.CF_CONSENSOADV).HasValue)
                {
                    isRicontattabile = Persona.GetFlag(Anagrafica.PFlags.CF_CONSENSOADV).Value;
                }

                // If (Me.Persona.GetFlag(PFlags.Cliente).HasValue AndAlso Me.Persona.GetFlag(PFlags.Cliente).Value = True) Then
                // Me.isClienteInAcquisizione = False
                // Me.isClienteAcquisito = True
                // Else

                // End If


            }

            /// <summary>
            /// Aggiorna questo oggetto sulla base del prossimo appuntamento programmato
            /// </summary>
            public void AggiornaAppuntamenti()
            {
                if (Persona is null)
                    throw new ArgumentNullException("persona");
                var ric = this.GetProssimoRicontatto(this.Persona); // , ""
                if (ric is null)
                {
                    m_MotivoProssimoRicontatto = "";
                    m_DataProssimoRicontatto = default;
                }
                else
                {
                    m_MotivoProssimoRicontatto = ric.Note;
                    m_DataProssimoRicontatto = ric.DataPrevista;
                }

                Save(true);
            }

            /// <summary>
            /// Ricalcola le statistiche
            /// </summary>
            public void Ricalcola()
            {
                if (Persona is null) throw new ArgumentNullException("persona");
                ConteggioNoRisp = 0;
                ConteggioRisp = 0;
                m_DataUltimoContattoNo = default;
                m_DataUltimoContattoOk = default;
                UltimoContattoNo = null;
                UltimoContattoOk = null;
                m_DataProssimoRicontatto = default;
                m_MotivoProssimoRicontatto = "";
                AggiornaPersona();
                var ric = GetProssimoRicontatto(Persona);
                if (ric is null)
                {
                    m_MotivoProssimoRicontatto = "";
                    m_DataProssimoRicontatto = default;
                }
                else
                {
                    m_MotivoProssimoRicontatto = ric.Note;
                    m_DataProssimoRicontatto = ric.DataPrevista;
                }

                //TODO Ricalcola Statistiche Ultima Chiamata
                throw new NotImplementedException();

                //using (var dbRis = CRM.TelDB.ExecuteReader("SELECT Count(*) As [Cnt], Max([Data]) As [MaxData] FROM [tbl_Telefonate] WHERE [IDPersona]=" + DBUtils.DBNumber(IDPersona) + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [Esito]=" + DBUtils.DBNumber(EsitoChiamata.OK) + " AND ([ClassName]='CTelefonata')"))
                //{
                //    if (dbRis.Read())
                //    {
                //        ConteggioRisp = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                //        m_DataUltimoContattoOk = Sistema.Formats.ParseDate(dbRis["MaxData"]);
                //    }
                //}

                //using (var dbRis = CRM.TelDB.ExecuteReader("SELECT Count(*) As [Cnt], Max([Data]) As [MaxData] FROM [tbl_Telefonate] WHERE [IDPersona]=" + DBUtils.DBNumber(IDPersona) + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [Esito]<>" + DBUtils.DBNumber(EsitoChiamata.OK) + " AND ([ClassName]='CTelefonata')"))
                //{
                //    if (dbRis.Read())
                //    {
                //        ConteggioNoRisp = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                //        m_DataUltimoContattoNo = Sistema.Formats.ParseDate(dbRis["MaxData"]);
                //    }
                //}

                //using (var dbRis = CRM.TelDB.ExecuteReader("SELECT [ID] FROM [tbl_Telefonate] WHERE [IDPersona]=" + DBUtils.DBNumber(IDPersona) + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [Esito]=" + DBUtils.DBNumber(EsitoChiamata.OK) + " AND ([ClassName]='CTelefonata') ORDER BY [Data] DESC"))
                //{
                //    if (dbRis.Read())
                //    {
                //        UltimoContattoOkID = Sistema.Formats.ToInteger(dbRis["ID"]);
                //    }

                //}

                //using (var dbRis = CRM.TelDB.ExecuteReader("SELECT [ID] FROM [tbl_Telefonate] WHERE [IDPersona]=" + DBUtils.DBNumber(IDPersona) + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [Esito]<>" + DBUtils.DBNumber(EsitoChiamata.OK) + " AND ([ClassName]='CTelefonata') ORDER BY [Data] DESC"))
                //{
                //    if (dbRis.Read())
                //    {
                //        UltimoContattoNoID = Sistema.Formats.ToInteger(dbRis["ID"]);
                //    }

                //}

                //UltimaVisita = Visite.GetUltimaVisita(Persona);
                Save(true);
            }

            /// <summary>
            /// Aggiorna l'operazone
            /// </summary>
            /// <param name="source"></param>
            /// <param name="descrizione"></param>
            public void AggiornaOperazione(object source, string descrizione)
            {
                m_DataAggiornamento = DMD.DateUtils.Now();
                m_DataUltimaOperazione = DMD.DateUtils.Now();
                m_Note = descrizione;
                Save(true);
            }

            /// <summary>
            /// Aggiorna le statistiche sulla base del contatto
            /// </summary>
            /// <param name="contatto"></param>
            public void AggiornaContatto(CContattoUtente contatto)
            {
                m_DataAggiornamento = DMD.DateUtils.Now();
                if (contatto is CVisita)
                {
                    UltimaVisita = (CVisita)contatto;
                }
                else if (contatto is CTelefonata)
                {
                    switch (contatto.Esito)
                    {
                        case EsitoChiamata.OK:
                            {
                                m_UltimoContattoOk = contatto;
                                m_UltimoContattoOkID = DBUtils.GetID(contatto, 0);
                                m_DataUltimoContattoOk = contatto.Data;
                                ConteggioRisp += 1;
                                break;
                            }

                        default:
                            {
                                m_UltimoContattoNo = contatto;
                                m_UltimoContattoNoID = DBUtils.GetID(contatto, 0);
                                m_DataUltimoContattoNo = contatto.Data;
                                ConteggioNoRisp += 1;
                                break;
                            }
                    }
                }

                Save(true);
            }

            private Anagrafica.CRicontatto GetProssimoRicontatto(Anagrafica.CPersona p)
            {
                using (var cursor = new Anagrafica.CRicontattiCursor())
                { 
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoRicontatto.ValueIn(new Anagrafica.StatoRicontatto[] { Anagrafica.StatoRicontatto.PROGRAMMATO, StatoRicontatto.RIMANDATO });
                    cursor.IDPersona.Value = DBUtils.GetID(p, 0);
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC;
                    // cursor.NomeLista.Value = ""
                    // cursor.NomeLista.IncludeNulls = True
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }                 
            }
        }
    }
}