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
        /// Stati di una commissione
        /// </summary>
        /// <remarks></remarks>
        public enum StatoCommissione : int
        {
            /// <summary>
            /// Non iniziata
            /// </summary>
            /// <remarks></remarks>
            NonIniziata = 0,

            /// <summary>
            /// Iniziata
            /// </summary>
            /// <remarks></remarks>
            Iniziata = 1,

            /// <summary>
            /// Rimandata
            /// </summary>
            /// <remarks></remarks>
            Rimandata = 2,

            /// <summary>
            /// Completata
            /// </summary>
            /// <remarks></remarks>
            Completata = 3,

            /// <summary>
            /// Annullata
            /// </summary>
            /// <remarks></remarks>
            Annullata = 4
        }

        /// <summary>
        /// Flag sulle commissioni
        /// </summary>
        [Flags]
        public enum CommissioneFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Commissione programmata per l'intera giornata
            /// </summary>
            GiornataIntera = 1
        }

        /// <summary>
        /// Rappresenta una commissione fatta da un operatore
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Commissione 
            : minidom.Databases.DBObjectPO, ICloneable, IComparable, IComparable<Commissione>
        {
            [NonSerialized] private Sistema.CUser m_Operatore;                    // Operatore che ha svolto la commissione
            private int m_IDOperatore;                // ID dell'operatore che ha svolto la commissione
            private string m_NomeOperatore;               // Nome dell'operatore che ha svolto la commissione
            private DateTime? m_DataPrevista;     // Data prevista
            private DateTime? m_OraUscita;        // Data ed ora di uscita (per svolgere la commissione)
            private DateTime? m_OraRientro;       // Data ed ora di rientro
            private string m_Motivo;                      // Motivo della commissione
            private string m_Presso;                      // Nome del luogo presso cui svolgere la commissione (domicilio, residenza, sede lavoro)...
            [NonSerialized] private CAzienda m_Azienda;                   // Azienda presso cui si è recato l'operatore
            private int m_IDAzienda;                  // ID dell'azienda presso cui si è recato l'operatore
            private string m_NomeAzienda;                 // Nome dell'azienda presso cui si è recato l'operatore
            [NonSerialized] private CPersonaFisica m_PersonaIncontrata;   // Persona fisica incontrata
            private int m_IDPersonaIncontrata;        // ID della persona fisica incontrata
            private string m_NomePersonaIncontrata; // Nome della persona fisica incontrata
            private string m_Esito;                       // Descrizione estesa della commissione
            private DateTime? m_Scadenzario;      // Data per un eventuale promemoria
            private string m_NoteScadenzario;             // Note per un eventuale promemoria
            private StatoCommissione m_StatoCommissione;  // Stato della commissione
            private int m_IDRichiesta;                // ID della richiesta che ha generato la commissione
            private RichiestaCERQ m_Richiesta;            // Richiesta che ha generato la commissione
            private double? m_DistanzaPercorsa;   // Distanza percorsa
            private UscitePerCommissioneCollection m_Uscite;
            private int m_ContextID;
            private string m_ContextType;
            private StatoCommissione m_OldStatoCommissione;
            private int m_IDAssegnataDa;
            [NonSerialized] private CUser m_AssegnataDa;
            private string m_NomeAssegnataDa;
            private DateTime? m_AssegnataIl;
            private int m_IDAssegnataA;
            [NonSerialized] private CUser m_AssegnataA;
            private string m_NomeAssegnataA;
            [NonSerialized] private object m_Source;
            private string m_SourceType;
            private int m_SourceID;
            private CCollection<LuogoDaVisitare> m_Luoghi;
            private string m_ParametriCommissione;

            /// <summary>
            /// Costrutore
            /// </summary>
            public Commissione()
            {
                m_Operatore = null;
                m_IDOperatore = 0;
                m_NomeOperatore = DMD.Strings.vbNullString;
                m_DataPrevista = default;
                m_OraUscita = default;
                m_OraRientro = default;
                m_Motivo = DMD.Strings.vbNullString;
                // Me.m_Luogo = vbNullString
                m_Azienda = null;
                m_IDAzienda = 0;
                m_NomeAzienda = DMD.Strings.vbNullString;
                m_PersonaIncontrata = null;
                m_IDPersonaIncontrata = 0;
                m_NomePersonaIncontrata = DMD.Strings.vbNullString;
                m_Esito = DMD.Strings.vbNullString;
                m_Scadenzario = default;
                m_NoteScadenzario = DMD.Strings.vbNullString;
                m_StatoCommissione = StatoCommissione.NonIniziata;
                m_IDRichiesta = 0;
                m_Richiesta = null;
                m_DistanzaPercorsa = default;
                m_Uscite = null;
                m_IDAssegnataDa = 0;
                m_AssegnataDa = null;
                m_NomeAssegnataDa = DMD.Strings.vbNullString;
                m_AssegnataIl = default;
                m_ContextID = 0;
                m_ContextType = DMD.Strings.vbNullString;
                m_OldStatoCommissione = m_StatoCommissione;
                m_IDAssegnataA = 0;
                m_AssegnataA = null;
                m_NomeAssegnataA = "";
                m_Flags = (int)CommissioneFlags.None;
                m_Source = null;
                m_SourceID = 0;
                m_SourceType = "";
                m_Luoghi = new CCollection<LuogoDaVisitare>();
                m_ParametriCommissione = "";
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public int CompareTo(Commissione c)
            {
                int ret = DMD.DateUtils.Compare(this.m_DataPrevista, c.m_DataPrevista);
                return ret;
            }

            int IComparable.CompareTo(object c) { return this.CompareTo((Commissione)c); }

            /// <summary>
            /// Parametri stringa
            /// </summary>
            public string ParametriCommissione
            {
                get
                {
                    return m_ParametriCommissione;
                }

                set
                {
                    string oldValue = m_ParametriCommissione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ParametriCommissione = value;
                    DoChanged("ParametriCommissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o importa l'oggetto che ha creato la commissione (la commissione è associata in relazione 1 a molti a questo oggetto)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object Source
            {
                get
                {
                    if (m_Source is null && !string.IsNullOrEmpty(m_SourceType) && m_SourceID != 0)
                        m_Source = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_SourceType, m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_SourceType = DMD.RunTime.vbTypeName(value);
                    m_SourceID = DBUtils.GetID(value, 0);
                    DoChanged("Source", value, oldValue);
                }
            }

            /// <summary>
            /// Tipo dell'oggetto che ha generato la commissione
            /// </summary>
            public string SourceType
            {
                get
                {
                    if (m_Source is object)
                        return DMD.RunTime.vbTypeName(m_Source);
                    return m_SourceType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = SourceType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceType = value;
                    m_Source = null;
                    DoChanged("SourceType", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'oggetto che ha generato la commissione
            /// </summary>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta i flags per la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CommissioneFlags Flags
            {
                get
                {
                    return (CommissioneFlags) base.Flags;
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
            /// Restituisce o imposta un valore booleano che indica se la commissione non specifica un orario per essere eseguita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool GiornataIntera
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, CommissioneFlags.GiornataIntera);
                }

                set
                {
                    if (GiornataIntera == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, CommissioneFlags.GiornataIntera, value);
                    DoChanged("GiornataIntera", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del contesto in cui è stata creata la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ContextID
            {
                get
                {
                    return m_ContextID;
                }

                set
                {
                    int oldValue = m_ContextID;
                    if (oldValue == value)
                        return;
                    m_ContextID = value;
                    DoChanged("ContextID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del contesto in cui è stata creata la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ContextType
            {
                get
                {
                    return m_ContextType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ContextType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContextType = value;
                    DoChanged("ContextType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'elenco delle uscite in cui è stata "iniziata" questa commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public UscitePerCommissioneCollection Uscite
            {
                get
                {
                    if (m_Uscite is null)
                        m_Uscite = new UscitePerCommissioneCollection(this);
                    return m_Uscite;
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoCommissione StatoCommissione
            {
                get
                {
                    return m_StatoCommissione;
                }

                set
                {
                    var oldValue = m_StatoCommissione;
                    if (oldValue == value)
                        return;
                    m_StatoCommissione = value;
                    DoChanged("StatoCommissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha effettuato la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = m_Operatore;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha effettuato la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha effettuate la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data prevista per la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }

                set
                {
                    var oldValue = m_DataPrevista;
                    if (oldValue == value == true)
                        return;
                    m_DataPrevista = value;
                    DoChanged("DataPrevista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di uscita per la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? OraUscita
            {
                get
                {
                    return m_OraUscita;
                }

                set
                {
                    var oldValue = m_OraUscita;
                    if (oldValue == value == true)
                        return;
                    m_OraUscita = value;
                    DoChanged("OraUscita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la durata in secondi (differenza tra ora ingresso ed ora uscita)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int? Durata
            {
                get
                {
                    if (m_OraUscita.HasValue && m_OraRientro.HasValue)
                    {
                        return (int?)Maths.Abs(DMD.DateUtils.DateDiff("s", m_OraRientro.Value, m_OraUscita.Value));
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            /// <summary>
            /// Restituisce o imposta la data di rientro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? OraRientro
            {
                get
                {
                    return m_OraRientro;
                }

                set
                {
                    var oldValue = m_OraRientro;
                    if (oldValue == value == true)
                        return;
                    m_OraRientro = value;
                    DoChanged("OraRientro", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo della commissione (descrizione breve)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Motivo
            {
                get
                {
                    return m_Motivo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Motivo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Motivo = value;
                    DoChanged("Motivo", value, oldValue);
                }
            }

            // ''' <summary>
            // ''' Restituisce o imposta il luogo di destinazione dell'operatore
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property Luogo As String
            // Get
            // Return Me.m_Luogo
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_Luogo
            // If (oldValue = value) Then Exit Property
            // Me.m_Luogo = value
            // Me.DoChanged("Luogo", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce la collezione dei luoghi da visitare
            /// </summary>
            public CCollection<LuogoDaVisitare> Luoghi
            {
                get
                {
                    return m_Luoghi;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azienda presso cui si è recato l'operatore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda Azienda
            {
                get
                {
                    if (m_Azienda is null)
                        m_Azienda = Anagrafica.Aziende.GetItemById(m_IDAzienda);
                    return m_Azienda;
                }

                set
                {
                    var oldValue = Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Azienda = value;
                    m_IDAzienda = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAzienda = value.Nominativo;
                    DoChanged("Azienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azienda presso cui si è recato l'operatore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID(m_Azienda, m_IDAzienda);
                }

                set
                {
                    int oldValue = IDAzienda;
                    if (oldValue == value)
                        return;
                    m_IDAzienda = value;
                    m_Azienda = null;
                    DoChanged("IDAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda presso cui si è recato l'operatore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeAzienda;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAzienda = value;
                    DoChanged("NomeAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona fisica che si è incontrato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersonaFisica PersonaIncontrata
            {
                get
                {
                    if (m_PersonaIncontrata is null)
                        m_PersonaIncontrata = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDPersonaIncontrata);
                    return m_PersonaIncontrata;
                }

                set
                {
                    var oldValue = PersonaIncontrata;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PersonaIncontrata = value;
                    m_IDPersonaIncontrata = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePersonaIncontrata = value.Nominativo;
                    DoChanged("PersonaIncontrata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona fisica incontrata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersonaIncontrata
            {
                get
                {
                    return DBUtils.GetID(m_PersonaIncontrata, m_IDPersonaIncontrata);
                }

                set
                {
                    int oldValue = IDPersonaIncontrata;
                    if (oldValue == value)
                        return;
                    m_IDPersonaIncontrata = value;
                    m_PersonaIncontrata = null;
                    DoChanged("IDPersonaIncontrata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona per cui é programmata la commissione
            /// </summary>
            public string NomePersonaIncontrata
            {
                get
                {
                    return m_NomePersonaIncontrata;
                }

                set
                {
                    string oldValue = m_NomePersonaIncontrata;
                    value = Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersonaIncontrata = value;
                    DoChanged("NomePersonaIncontrata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'esito dell'incontro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Esito
            {
                get
                {
                    return m_Esito;
                }

                set
                {
                    string oldValue = m_Esito;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Esito = value;
                    DoChanged("Esito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un eventuale data per il promemoria
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? Scadenzario
            {
                get
                {
                    return m_Scadenzario;
                }

                set
                {
                    var oldValue = m_Scadenzario;
                    if (oldValue == value == true)
                        return;
                    m_Scadenzario = value;
                    DoChanged("Scadenzario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle note per l'eventuale promemoria
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NoteScadenzario
            {
                get
                {
                    return m_NoteScadenzario;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NoteScadenzario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NoteScadenzario = value;
                    DoChanged("NoteScadenzario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della richiesta associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRichiesta
            {
                get
                {
                    return DBUtils.GetID(m_Richiesta, m_IDRichiesta);
                }

                set
                {
                    int oldValue = m_IDRichiesta;
                    if (oldValue == value)
                        return;
                    m_IDRichiesta = value;
                    DoChanged("IDRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la richiesta associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public RichiestaCERQ Richiesta
            {
                get
                {
                    if (m_Richiesta is null)
                        m_Richiesta = minidom.Office.RichiesteCERQ.GetItemById(m_IDRichiesta);
                    return m_Richiesta;
                }

                set
                {
                    var oldValue = Richiesta;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiesta = value;
                    m_IDRichiesta = DBUtils.GetID(value, 0);
                    DoChanged("Richiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la distanza percorsa
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? DistanzaPercorsa
            {
                get
                {
                    return m_DistanzaPercorsa;
                }

                set
                {
                    var oldValue = m_DistanzaPercorsa;
                    if (oldValue == value == true)
                        return;
                    m_DistanzaPercorsa = value;
                    DoChanged("DistanzaPercorsa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha programmato la commissione
            /// </summary>
            public int IDAssegnataDa
            {
                get
                {
                    return DBUtils.GetID(m_AssegnataDa, m_IDAssegnataDa);
                }

                set
                {
                    int oldValue = IDAssegnataDa;
                    if (oldValue == value)
                        return;
                    m_IDAssegnataDa = value;
                    m_AssegnataDa = null;
                    DoChanged("IDAssegnataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha programmato la commissione
            /// </summary>
            public Sistema.CUser AssegnataDa
            {
                get
                {
                    if (m_AssegnataDa is null)
                        m_AssegnataDa = Sistema.Users.GetItemById(m_IDAssegnataDa);
                    return m_AssegnataDa;
                }

                set
                {
                    var oldValue = m_AssegnataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AssegnataDa = value;
                    m_IDAssegnataDa = DBUtils.GetID(value, 0);
                    m_NomeAssegnataDa = (value is object)? value.Nominativo : "";
                    DoChanged("AssegnataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che programmato la commissione
            /// </summary>
            public string NomeAssegnataDa
            {
                get
                {
                    return m_NomeAssegnataDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAssegnataDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssegnataDa = value;
                    DoChanged("NomeAssegnataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta la data in cui è stata programmata la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? AssegnataIl
            {
                get
                {
                    return m_AssegnataIl;
                }

                set
                {
                    var oldValue = m_AssegnataIl;
                    if (oldValue == value == true)
                        return;
                    m_AssegnataIl = value;
                    DoChanged("AssegnataIl", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è stata assegnata la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAssegnataA
            {
                get
                {
                    return DBUtils.GetID(m_AssegnataA, m_IDAssegnataA);
                }

                set
                {
                    int oldValue = IDAssegnataA;
                    if (oldValue == value)
                        return;
                    m_IDAssegnataA = value;
                    m_AssegnataA = null;
                    DoChanged("IDAssegnataA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente a cui è stata assegnata la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser AssegnataA
            {
                get
                {
                    if (m_AssegnataA is null)
                        m_AssegnataA = Sistema.Users.GetItemById(m_IDAssegnataA);
                    return m_AssegnataA;
                }

                set
                {
                    var oldValue = m_AssegnataA;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AssegnataA = value;
                    m_IDAssegnataA = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeAssegnataA = value.Nominativo;
                    DoChanged("AssegnataA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente a cui è stata assegnata la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAssegnataA
            {
                get
                {
                    return m_NomeAssegnataA;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAssegnataA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssegnataA = value;
                    DoChanged("NomeAssegnataA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del luogo presso cui svolgere la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Presso
            {
                get
                {
                    return m_Presso;
                }

                set
                {
                    string oldValue = m_Presso;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Presso = value;
                    DoChanged("Presso", value, oldValue);
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Commissioni;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeCommissioni";
            }

            /// <summary>
            /// Gestisce l'evento Commissioni.StatoCommissioneChanged
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                base.OnAfterSave(e);
                if (m_OldStatoCommissione != m_StatoCommissione)
                {
                    minidom.Office.Commissioni.OnStatoCommissioneChanged(new ItemEventArgs<Commissione>(this));
                }
                m_OldStatoCommissione = m_StatoCommissione;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_StatoCommissione = reader.Read("StatoCommissione", this.m_StatoCommissione);
                this.m_OldStatoCommissione = this.m_StatoCommissione;
                this.m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                this.m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                this.m_DataPrevista = reader.Read("DataPrevista", this.m_DataPrevista);
                this.m_OraUscita = reader.Read("OraUscita", this.m_OraUscita);
                this.m_OraRientro = reader.Read("OraRientro", this.m_OraRientro);
                this.m_Motivo = reader.Read("Motivo", this.m_Motivo);
                string luogo = reader.Read("Luogo", "");
                if (!string.IsNullOrEmpty(luogo))
                {
                    this.m_Luoghi = new CCollection<LuogoDaVisitare>();
                    this.m_Luoghi.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(luogo));
                }
                m_IDAzienda = reader.Read("IDAzienda", this.m_IDAzienda);
                m_NomeAzienda = reader.Read("NomeAzienda", this.m_NomeAzienda);
                m_IDPersonaIncontrata = reader.Read("IDPersona", this.m_IDPersonaIncontrata);
                m_NomePersonaIncontrata = reader.Read("NomePersona", this.m_NomePersonaIncontrata);
                m_Esito = reader.Read("Esito", this.m_Esito);
                m_Scadenzario = reader.Read("Scadenzario", this.m_Scadenzario);
                m_NoteScadenzario = reader.Read("NoteScadenzario", this.m_NoteScadenzario);
                m_IDRichiesta = reader.Read("IDRichiesta", this.m_IDRichiesta);
                m_DistanzaPercorsa = reader.Read("DistanzaPercorsa", this.m_DistanzaPercorsa);
                m_IDAssegnataDa = reader.Read("IDAssegnataDa", this.m_IDAssegnataDa);
                m_NomeAssegnataDa = reader.Read("NomeAssegnataDa", this.m_NomeAssegnataDa);
                m_AssegnataIl = reader.Read("AssegnataIl", this.m_AssegnataIl);
                m_IDAssegnataA = reader.Read("IDAssegnataA", this.m_IDAssegnataA);
                m_NomeAssegnataA = reader.Read("NomeAssegnataA", this.m_NomeAssegnataA);
                m_ContextID = reader.Read("ContextID", this.m_ContextID);
                m_ContextType = reader.Read("ContextType", this.m_ContextType);
                m_SourceType = reader.Read("SourceType", this.m_SourceType);
                m_SourceID = reader.Read("SourceID", this.m_SourceID);
                m_Presso = reader.Read("Presso", this.m_Presso);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("StatoCommissione", m_StatoCommissione);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("DataPrevista", m_DataPrevista);
                writer.Write("OraUscita", m_OraUscita);
                writer.Write("OraRientro", m_OraRientro);
                writer.Write("Motivo", m_Motivo);
                writer.Write("Luogo", DMD.XML.Utils.Serializer.Serialize(Luoghi));
                writer.Write("IDAzienda", IDAzienda);
                writer.Write("NomeAzienda", m_NomeAzienda);
                writer.Write("IDPersona", IDPersonaIncontrata);
                writer.Write("NomePersona", m_NomePersonaIncontrata);
                writer.Write("Esito", m_Esito);
                writer.Write("Scadenzario", m_Scadenzario);
                writer.Write("NoteScadenzario", m_NoteScadenzario);
                writer.Write("IDRichiesta", IDRichiesta);
                writer.Write("DistanzaPercorsa", m_DistanzaPercorsa);
                writer.Write("IDAssegnataDa", IDAssegnataDa);
                writer.Write("NomeAssegnataDa", m_NomeAssegnataDa);
                writer.Write("AssegnataIl", m_AssegnataIl);
                writer.Write("IDAssegnataA", IDAssegnataA);
                writer.Write("NomeAssegnataA", m_NomeAssegnataA);
                writer.Write("ContextID", m_ContextID);
                writer.Write("ContextType", m_ContextType);
                writer.Write("SourceType", SourceType);
                writer.Write("SourceID", SourceID);
                writer.Write("Presso", m_Presso);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("StatoCommissione", typeof(int), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("DataPrevista", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraUscita", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraRientro", typeof(DateTime), 1);
                c = table.Fields.Ensure("Motivo", typeof(string), 255);
                c = table.Fields.Ensure("Luogo", typeof(string), 0);
                c = table.Fields.Ensure("IDAzienda", typeof(int), 1);
                c = table.Fields.Ensure("NomeAzienda", typeof(string), 255);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("Esito", typeof(string), 255);
                c = table.Fields.Ensure("Scadenzario", typeof(DateTime), 1);
                c = table.Fields.Ensure("NoteScadenzario", typeof(string), 0);
                c = table.Fields.Ensure("IDRichiesta", typeof(int), 1);
                c = table.Fields.Ensure("DistanzaPercorsa", typeof(double), 1);
                c = table.Fields.Ensure("IDAssegnataDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeAssegnataDa", typeof(string), 255);
                c = table.Fields.Ensure("AssegnataIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDAssegnataA", typeof(int), 1);
                c = table.Fields.Ensure("NomeAssegnataA", typeof(string), 255);
                c = table.Fields.Ensure("ContextID", typeof(int), 1);
                c = table.Fields.Ensure("ContextType", typeof(string), 255);
                c = table.Fields.Ensure("SourceType", typeof(string), 255);
                c = table.Fields.Ensure("SourceID", typeof(int), 1);
                c = table.Fields.Ensure("Presso", typeof(string), 255);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxStato", new string[] { "StatoCommissione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUscita", new string[] { "OraUscita", "OraRientro" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxMotivo", new string[] { "Motivo" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxAzienda", new string[] { "IDAzienda" , "NomeAzienda" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxEsito", new string[] { "Esito", "Presso" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxScadenziario", new string[] { "Scadenzario", "NoteScadenzario" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxRichiesta", new string[] { "IDRichiesta", "DistanzaPercorsa" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxAssegnatoDa", new string[] { "IDAssegnataDa", "NomeAssegnataDa", "AssegnataIl" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxAssegnatoA", new string[] { "IDAssegnataA", "NomeAssegnataA", "DataPrevista" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxContesto", new string[] { "ContextID", "ContextType" }, DBFieldConstraintFlags.None); ;
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceType", "SourceID" }, DBFieldConstraintFlags.None); ;

                //c = table.Fields.Ensure("Luogo", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("StatoCommissione", (int?)m_StatoCommissione);
                writer.WriteAttribute("OldStatoCommissione", (int?)m_OldStatoCommissione);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("DataPrevista", m_DataPrevista);
                writer.WriteAttribute("OraUscita", m_OraUscita);
                writer.WriteAttribute("OraRientro", m_OraRientro);
                writer.WriteAttribute("Motivo", m_Motivo);
                writer.WriteAttribute("IDAzienda", IDAzienda);
                writer.WriteAttribute("NomeAzienda", m_NomeAzienda);
                writer.WriteAttribute("IDPersona", IDPersonaIncontrata);
                writer.WriteAttribute("NomePersona", m_NomePersonaIncontrata);
                writer.WriteAttribute("Esito", m_Esito);
                writer.WriteAttribute("Scadenzario", m_Scadenzario);
                writer.WriteAttribute("NoteScadenzario", m_NoteScadenzario);
                writer.WriteAttribute("IDRichiesta", IDRichiesta);
                writer.WriteAttribute("DistanzaPercorsa", m_DistanzaPercorsa);
                writer.WriteAttribute("IDAssegnataDa", IDAssegnataDa);
                writer.WriteAttribute("NomeAssegnataDa", m_NomeAssegnataDa);
                writer.WriteAttribute("AssegnataIl", m_AssegnataIl);
                writer.WriteAttribute("IDAssegnataA", IDAssegnataA);
                writer.WriteAttribute("NomeAssegnataA", m_NomeAssegnataA);
                writer.WriteAttribute("ContextID", m_ContextID);
                writer.WriteAttribute("ContextType", m_ContextType);
                writer.WriteAttribute("SourceType", SourceType);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("Presso", m_Presso);
                base.XMLSerialize(writer);
                writer.SetSetting("commissioneserialization", true);
                writer.WriteTag("Luoghi", Luoghi);
                writer.WriteTag("Uscite", Uscite);
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
                    case "StatoCommissione":
                        {
                            m_StatoCommissione = (StatoCommissione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OldStatoCommissione":
                        {
                            m_OldStatoCommissione = (StatoCommissione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataPrevista":
                        {
                            m_DataPrevista = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraUscita":
                        {
                            m_OraUscita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraRientro":
                        {
                            m_OraRientro = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Motivo":
                        {
                            m_Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Luoghi":
                        {
                            m_Luoghi.Clear();
                            m_Luoghi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAzienda":
                        {
                            m_NomeAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            m_IDPersonaIncontrata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersonaIncontrata = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            m_Esito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Scadenzario":
                        {
                            m_Scadenzario = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NoteScadenzario":
                        {
                            m_NoteScadenzario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRichiesta":
                        {
                            m_IDRichiesta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DistanzaPercorsa":
                        {
                            m_DistanzaPercorsa = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeAssegnataDa":
                        {
                            m_NomeAssegnataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAssegnataDa":
                        {
                            m_IDAssegnataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAssegnataA":
                        {
                            m_NomeAssegnataA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAssegnataA":
                        {
                            m_IDAssegnataA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AssegnataIl":
                        {
                            m_AssegnataIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ContextID":
                        {
                            m_ContextID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ContextType":
                        {
                            m_ContextType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Uscite":
                        {
                            m_Uscite = (UscitePerCommissioneCollection)fieldValue;
                            m_Uscite.SetCommissione(this);
                            break;
                        }

                    case "SourceType":
                        {
                            m_SourceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Presso":
                        {
                            m_Presso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder(256);
                ret.Append(m_Motivo);
                if (!string.IsNullOrEmpty(m_NomePersonaIncontrata))
                    ret.Append(" per " + m_NomePersonaIncontrata);
                if (!string.IsNullOrEmpty(m_NomeAzienda))
                    ret.Append(" presso la " + m_NomeAzienda);
                // If (Me.m_Luogo <> "") Then ret.Append(" in " & Me.m_Luogo)
                return ret.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DataPrevista);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Commissione) && this.Equals((Commissione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Commissione obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.DateUtils.EQ(this.m_DataPrevista, obj.m_DataPrevista)
                    && DMD.DateUtils.EQ(this.m_OraUscita, obj.m_OraUscita)
                    && DMD.DateUtils.EQ(this.m_OraRientro, obj.m_OraRientro)
                    && DMD.Strings.EQ(this.m_Motivo, obj.m_Motivo)
                    && DMD.Strings.EQ(this.m_Presso, obj.m_Presso)
                    && DMD.Integers.EQ(this.m_IDAzienda, obj.m_IDAzienda)
                    && DMD.Strings.EQ(this.m_NomeAzienda, obj.m_NomeAzienda)
                    && DMD.Integers.EQ(this.m_IDPersonaIncontrata, obj.m_IDPersonaIncontrata)
                    && DMD.Strings.EQ(this.m_NomePersonaIncontrata, obj.m_NomePersonaIncontrata)
                    && DMD.Strings.EQ(this.m_Esito, obj.m_Esito)
                    && DMD.DateUtils.EQ(this.m_Scadenzario, obj.m_Scadenzario)
                    && DMD.Strings.EQ(this.m_NoteScadenzario, obj.m_NoteScadenzario)
                    && DMD.Integers.EQ((int)this.m_StatoCommissione, (int)obj.m_StatoCommissione)
                    && DMD.Integers.EQ(this.m_IDRichiesta, obj.m_IDRichiesta)
                    && DMD.Doubles.EQ(this.m_DistanzaPercorsa, obj.m_DistanzaPercorsa)
                    && DMD.Integers.EQ(this.m_ContextID, obj.m_ContextID)
                    && DMD.Strings.EQ(this.m_ContextType, obj.m_ContextType)
                    && DMD.Integers.EQ((int)this.m_OldStatoCommissione, (int)obj.m_OldStatoCommissione)
                    && DMD.Integers.EQ(this.m_IDAssegnataDa, obj.m_IDAssegnataDa)
                    && DMD.Strings.EQ(this.m_NomeAssegnataDa, obj.m_NomeAssegnataDa)
                    && DMD.DateUtils.EQ(this.m_AssegnataIl, obj.m_AssegnataIl)
                    && DMD.Integers.EQ(this.m_IDAssegnataA, obj.m_IDAssegnataA)
                    && DMD.Strings.EQ(this.m_NomeAssegnataA, obj.m_NomeAssegnataA)
                    && DMD.Strings.EQ(this.m_SourceType, obj.m_SourceType)
                    && DMD.Integers.EQ(this.m_SourceID, obj.m_SourceID)
                    && DMD.Strings.EQ(this.m_ParametriCommissione, obj.m_ParametriCommissione)
                    ;

        }

        /// <summary>
        /// Clona l'oggetto
        /// </summary>
        /// <returns></returns>
        protected override Databases.DBObjectBase _Clone()
            {
                var ret = (Commissione)base._Clone();
                ret.m_Uscite = null;
                ret.m_Luoghi = new CCollection<LuogoDaVisitare>();
                foreach(var l in this.Luoghi)
                {
                    this.m_Luoghi.Add(DMD.RunTime.Clone(l));
                }
                return ret;
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public Commissione Clone()
            {
                return (Commissione) this._Clone();
            }
        }
    }
}