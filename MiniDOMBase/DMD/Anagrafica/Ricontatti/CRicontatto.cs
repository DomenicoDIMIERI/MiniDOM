using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{

    
    public partial class Anagrafica
    {
        /// <summary>
        /// Stato di un appuntamento
        /// </summary>
        public enum StatoRicontatto : int
        {
            /// <summary>
            /// Non programmato
            /// </summary>
            NONPROGRAMMATO = 0,

            /// <summary>
            /// Programmato
            /// </summary>
            PROGRAMMATO = 1,

            /// <summary>
            /// Effettuato
            /// </summary>
            EFFETTUATO = 2,

            /// <summary>
            /// Rimandato
            /// </summary>
            RIMANDATO = 3,

            /// <summary>
            /// Annullato
            /// </summary>
            ANNULLATO = 4
        }

        /// <summary>
        /// Flag di un appuntamento
        /// </summary>
        [Flags]
        public enum RicontattoFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            /// <remarks></remarks>
            None = 0,

            /// <summary>
            /// Flag che indica che il ricontatto è relativo all'intera giornata piuttosto che ad un orario specifico
            /// </summary>
            /// <remarks></remarks>
            GiornataIntera = 1,

            /// <summary>
            /// Il ricontatto è riservato per il sistema: può essere visualizzato dagli operatori ma non viene mostrato nei campi di default dedicati al ricontatto nell'interfaccia
            /// </summary>
            /// <remarks></remarks>
            Reserved = 2048
        }


        /// <summary>
        /// Appuntamento 
        /// </summary>
        [Serializable]
        public class CRicontatto 
            : Databases.DBObjectPO
        {
            private string m_TipoAppuntamento;
            private string m_NumeroOIndirizzo;
            private DateTime m_DataPrevista; // [Date] Data ed ora in cui ricontattare
            private int m_IDAssegnatoA;  // [Int]  ID dell'operatore che deve ricontattare
            [NonSerialized] private Sistema.CUser m_AssegnatoA; // [CUser]Utente che deve ricontttare
            private string m_NomeAssegnatoA; // [Text] Nome dell'operatore che deve ricontattare
            private string m_Note; // [Text] Promemoria per il ricontatto
            private StatoRicontatto m_StatoRicontatto; // [Int] Valore che indica lo stato del ricontatto
            private DateTime? m_DataRicontatto; // [Date] Data ed ora del ricontatto
            private int m_IDOperatore;      // [Int] ID dell'operatore che ha preso in carico il ricontatto
            [NonSerialized] private Sistema.CUser m_Operatore; // [CUser] Operatore che ha preso in carico il ricontatto
            private string m_NomeOperatore; // [Text] Nome dell'operatore che ha preso in carico il ricontatto
            private string m_TipoContatto; // [Text] Stringa che indica il tipo di contatto (telefonata, e-mail, ecc...)
            private int m_IDContatto;   // [Int] ID del contatto
            [NonSerialized] private CContatto m_Contatto; // [CContatto] Oggetto contatto
            private int m_IDPersona; // [ID] id della persona da contattare
            [NonSerialized] private CPersona m_Persona; // [CPersona] Persona da ricontattare
            private string m_NomePersona; // [Text] Nominativo della persona da ricontattare
            private string m_SourceName; // [Text] Nome del server che ha generato l'oggetto
            private string m_SourceParam; // [Text] Parametri per il server che ha generato l'oggetto
            private int m_Promemoria; // Promemoria in minuti
            private string m_Categoria;
            private string m_DettaglioStato;
            private string m_DettaglioStato1;
            private int m_IDRicontattoPrecedente;
            [NonSerialized] private CRicontatto m_RicontattoPrecedente;
            private int m_IDRicontattoSuccessivo;
            [NonSerialized] private CRicontatto m_RicontattoSuccessivo;
            private int m_Priorita;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRicontatto()
            {
                m_DataPrevista = default;
                m_IDAssegnatoA = 0;
                m_AssegnatoA = null;
                m_NomeAssegnatoA = "";
                m_Note = "";
                m_DataRicontatto = default;
                m_StatoRicontatto = StatoRicontatto.PROGRAMMATO;
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_TipoContatto = "";
                m_IDContatto = 0;
                m_Contatto = null;
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_SourceName = "";
                m_SourceParam = "";
                m_Promemoria = 0;
                m_Flags = (int)RicontattoFlags.None;
                m_Categoria = "Normale";
                m_DettaglioStato = "";
                m_DettaglioStato1 = "";
                m_TipoAppuntamento = "Telefonata";
                m_NumeroOIndirizzo = "";
                m_IDRicontattoPrecedente = 0;
                m_RicontattoPrecedente = null;
                m_IDRicontattoSuccessivo = 0;
                m_RicontattoSuccessivo = null;
                m_Priorita = 0;
            }

            

            /// <summary>
            /// Restituisce o imposta un valore di priorità
            /// </summary>
            public int Priorita
            {
                get
                {
                    return m_Priorita;
                }

                set
                {
                    int oldValue = m_Priorita;
                    if (oldValue == value)
                        return;
                    m_Priorita = value;
                    DoChanged("Priorita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del ricontatto precedente nella catena dei ricontatti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRicontattoPrecedente
            {
                get
                {
                    return DBUtils.GetID(m_RicontattoPrecedente, m_IDRicontattoPrecedente);
                }

                set
                {
                    int oldValue = IDRicontattoPrecedente;
                    if (oldValue == value)
                        return;
                    m_IDRicontattoPrecedente = value;
                    m_RicontattoPrecedente = null;
                    DoChanged("IDRicontattoPrecedente", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il ricontatto precedente nella catena dei ricontatti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRicontatto RicontattoPrecedente
            {
                get
                {
                    if (m_RicontattoPrecedente is null)
                        m_RicontattoPrecedente = Ricontatti.GetItemById(m_IDRicontattoPrecedente);
                    return m_RicontattoPrecedente;
                }

                set
                {
                    var oldValue = m_RicontattoPrecedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RicontattoPrecedente = value;
                    m_IDRicontattoPrecedente = DBUtils.GetID(value, this.m_IDRicontattoPrecedente);
                    DoChanged("RicontattoPrecedente", value, oldValue);
                }
            }



            /// <summary>
        /// Restituisce o imposta l'ID del ricontatto successivo nella catena dei ricontatti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDRicontattoSuccessivo
            {
                get
                {
                    return DBUtils.GetID(m_RicontattoSuccessivo, m_IDRicontattoSuccessivo);
                }

                set
                {
                    int oldValue = IDRicontattoSuccessivo;
                    if (oldValue == value)
                        return;
                    m_IDRicontattoSuccessivo = value;
                    m_RicontattoSuccessivo = null;
                    DoChanged("IDRicontattoSuccessivo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il ricontatto Successivo nella catena dei ricontatti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CRicontatto RicontattoSuccessivo
            {
                get
                {
                    if (m_RicontattoSuccessivo is null)
                        m_RicontattoSuccessivo = Ricontatti.GetItemById(m_IDRicontattoSuccessivo);
                    return m_RicontattoSuccessivo;
                }

                set
                {
                    var oldValue = m_RicontattoSuccessivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RicontattoSuccessivo = value;
                    m_IDRicontattoSuccessivo = DBUtils.GetID(value, this.m_IDRicontattoSuccessivo);
                    DoChanged("RicontattoSuccessivo", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il tipo di appuntamento desiderato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoAppuntamento
            {
                get
                {
                    return m_TipoAppuntamento;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoAppuntamento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoAppuntamento = value;
                    DoChanged("TipoAppuntamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero o l'indirizzo fissato per l'appuntamento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NumeroOIndirizzo
            {
                get
                {
                    return m_NumeroOIndirizzo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroOIndirizzo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroOIndirizzo = value;
                    DoChanged("NumeroOIndirizzo", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce o imposta la categoria del ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((m_Categoria ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle informazioni aggiuntive sull'esito del ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioStato;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato = value;
                    DoChanged("DettaglioStato", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta delle informazioni aggiuntive sull'esito del ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioStato1
            {
                get
                {
                    return m_DettaglioStato1;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioStato1;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato1 = value;
                    DoChanged("DettaglioStato1", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Ricontatti; //.Module;
            }



            /// <summary>
            /// Restituisce o imposta la data prevista per il ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }

                set
                {
                    var oldValue = m_DataPrevista;
                    if (oldValue == value)
                        return;
                    m_DataPrevista = value;
                    DoChanged("DataPrevista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il promemoria in minuti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Promemoria
            {
                get
                {
                    return m_Promemoria;
                }

                set
                {
                    int oldValue = m_Promemoria;
                    if (oldValue == value)
                        return;
                    m_Promemoria = value;
                    DoChanged("Promemoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il ricontatto deve essere effettuato ad un determinato orario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool GiornataIntera
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, (int)RicontattoFlags.GiornataIntera);
                }

                set
                {
                    if (GiornataIntera == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, (int)RicontattoFlags.GiornataIntera, value);
                    DoChanged("GiornataIntera", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags aggiuntivi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new RicontattoFlags Flags
            {
                get
                {
                    return (RicontattoFlags)m_Flags;
                }

                set
                {
                    var oldValue = (RicontattoFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che deve effettuare il ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAssegnatoA
            {
                get
                {
                    return DBUtils.GetID(m_AssegnatoA, m_IDAssegnatoA);
                }

                set
                {
                    int oldValue = IDAssegnatoA;
                    if (oldValue == value)
                        return;
                    m_IDAssegnatoA = value;
                    m_AssegnatoA = null;
                    DoChanged("IDAssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che deve effettuare il ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser AssegnatoA
            {
                get
                {
                    if (m_AssegnatoA is null)
                        m_AssegnatoA = Sistema.Users.GetItemById(m_IDAssegnatoA);
                    return m_AssegnatoA;
                }

                set
                {
                    var oldValue = m_AssegnatoA;
                    if (oldValue == value)
                        return;
                    m_AssegnatoA = value;
                    m_IDAssegnatoA = DBUtils.GetID(value, this.m_IDAssegnatoA);
                    if (value is object)
                        m_NomeAssegnatoA = value.Nominativo;
                    DoChanged("AssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che deve effettuare il ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAssegnatoA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssegnatoA = value;
                    DoChanged("NomeAssegnatoA", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta delle annotazioni
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
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui è stato effettuato il ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataRicontatto
            {
                get
                {
                    return m_DataRicontatto;
                }

                set
                {
                    var oldValue = m_DataRicontatto;
                    if (oldValue == value == true)
                        return;
                    m_DataRicontatto = value;
                    DoChanged("DataRicontatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato del ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoRicontatto StatoRicontatto
            {
                get
                {
                    return m_StatoRicontatto;
                }

                set
                {
                    var oldValue = m_StatoRicontatto;
                    if (oldValue == value)
                        return;
                    m_StatoRicontatto = value;
                    DoChanged("StatoRicontatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha effettuato il ricontatto
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
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetPersona(CPersona value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value, this.m_IDPersona);
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha effettuato il ricontatto
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
                    if (oldValue == value)
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, this.m_IDOperatore);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha effettuato il ricontatto
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del contatto generato a partire da questo ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoContatto
            {
                get
                {
                    return m_TipoContatto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContatto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContatto = value;
                    DoChanged("TipoContatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del contatto generato da questo ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContatto
            {
                get
                {
                    return DBUtils.GetID(m_Contatto, m_IDContatto);
                }

                set
                {
                    int oldValue = IDContatto;
                    if (oldValue == value)
                        return;
                    m_IDContatto = value;
                    m_Contatto = null;
                    DoChanged("IDContatto", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID della persona associata
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
                    m_Persona = null;
                    m_IDPersona = value;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, this.m_IDPersona);
                    this.m_NomePersona = (value is object)? value.Nominativo : string.Empty;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nominativo della persona associata
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta una stringa che indica la sorgente che ha creato questo ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceName
            {
                get
                {
                    return m_SourceName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceName = value;
                    DoChanged("SourceName", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta una stringa che identifica il contesto in cui la sorgente ha creato questo ricontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceParam
            {
                get
                {
                    return m_SourceParam;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceParam;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceParam = value;
                    DoChanged("SourceParam", value, oldValue);
                }
            }

            /// <summary>
            /// Discriminante nel repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Ricontatti";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_DataPrevista = reader.Read("DataPrevista", this.m_DataPrevista);
                m_IDAssegnatoA = reader.Read("IDAssegnatoA", this.m_IDAssegnatoA);
                m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", this.m_NomeAssegnatoA);
                m_Note = reader.Read("Note", this.m_Note);
                m_DataRicontatto = reader.Read("DataRicontatto", this.m_DataRicontatto);
                m_StatoRicontatto = reader.Read("StatoRicontatto", this.m_StatoRicontatto);
                m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                m_TipoContatto = reader.Read("TipoContatto", this.m_TipoContatto);
                m_IDContatto = reader.Read("IDContatto", this.m_IDContatto);
                m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                m_NomePersona = reader.Read("NomePersona", this.m_NomePersona);
                m_SourceName = reader.Read("SourceName", this.m_SourceName);
                m_SourceParam = reader.Read("SourceParam", this.m_SourceParam);
                m_Promemoria = reader.Read("Promemoria", this.m_Promemoria);
                m_Categoria = reader.Read("Categoria", this.m_Categoria);
                m_DettaglioStato = reader.Read("DettaglioStato", this.m_DettaglioStato);
                m_DettaglioStato1 = reader.Read("DettaglioStato1", this.m_DettaglioStato1);
                m_TipoAppuntamento = reader.Read("TipoAppuntamento", this.m_TipoAppuntamento);
                m_NumeroOIndirizzo = reader.Read("NumeroOIndirizzo", this.m_NumeroOIndirizzo);
                m_Priorita = reader.Read("Priorita", this.m_Priorita);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                switch (Categoria ?? "")
                {
                    case "Urgente":
                        {
                            Priorita = -10;
                            break;
                        }

                    case "Importante":
                        {
                            Priorita = -5;
                            break;
                        }

                    case "Normale":
                        {
                            Priorita = 0;
                            break;
                        }

                    case "Poco importante":
                        {
                            Priorita = 10;
                            break;
                        }
                }

                writer.Write("DataPrevista", m_DataPrevista);
                //writer.Write("DataPrevistaStr", DBUtils.ToDBDateStr(m_DataPrevista));
                writer.Write("IDAssegnatoA", IDAssegnatoA);
                writer.Write("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.Write("Note", m_Note);
                writer.Write("DataRicontatto", m_DataRicontatto);
                writer.Write("StatoRicontatto", m_StatoRicontatto);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("TipoContatto", m_TipoContatto);
                writer.Write("IDContatto", IDContatto);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("SourceName", m_SourceName);
                writer.Write("SourceParam", m_SourceParam);
                writer.Write("Promemoria", m_Promemoria);
                writer.Write("Categoria", m_Categoria);
                writer.Write("DettaglioStato", m_DettaglioStato);
                writer.Write("DettaglioStato1", m_DettaglioStato1);
                writer.Write("TipoAppuntamento", m_TipoAppuntamento);
                writer.Write("NumeroOIndirizzo", m_NumeroOIndirizzo);
                writer.Write("IDRicPrec", IDRicontattoPrecedente);
                writer.Write("IDRicSucc", IDRicontattoSuccessivo);
                writer.Write("Priorita", m_Priorita);
                writer.Write("Parameters", DMD.XML.Utils.Serializer.Serialize(this.Parameters));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataPrevista", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDAssegnatoA", typeof(int), 1);
                c = table.Fields.Ensure("NomeAssegnatoA", typeof(string), 255);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("DataRicontatto", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoRicontatto", typeof(int), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("TipoContatto", typeof(string), 255);
                c = table.Fields.Ensure("IDContatto", typeof(int), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("SourceName", typeof(string), 255);
                c = table.Fields.Ensure("SourceParam", typeof(string), 255);
                c = table.Fields.Ensure("Promemoria", typeof(int), 1);
                c = table.Fields.Ensure("NFlags", typeof(int), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("DettaglioStato", typeof(string), 255);
                c = table.Fields.Ensure("DettaglioStato1", typeof(string), 255);
                c = table.Fields.Ensure("TipoAppuntamento", typeof(string), 255);
                c = table.Fields.Ensure("NumeroOIndirizzo", typeof(string), 255);
                c = table.Fields.Ensure("IDRicPrec", typeof(int), 1);
                c = table.Fields.Ensure("IDRicSucc", typeof(int), 1);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
                c = table.Fields.Ensure("Parameters", typeof(string), 0);

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDate", new string[] { "DataPrevista" , "DataRicontatto" , "Promemoria" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAssegnato", new string[] { "IDAssegnatoA", "NomeAssegnatoA", "Priorita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoRic", new string[] { "StatoRicontatto", "TipoContatto", "IDContatto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceName", "SourceParam" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTipoApp", new string[] { "TipoAppuntamento", "Categoria", "NFlags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDettagli", new string[] { "DettaglioStato", "DettaglioStato1"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumero", new string[] { "NumeroOIndirizzo", "DettaglioStato1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSuccPrec", new string[] { "IDRicPrec", "IDRicSucc" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Note", typeof(string), 0);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);

            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_TipoAppuntamento, " ", this.m_NomePersona, " ", this.m_DataPrevista);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_TipoAppuntamento, this.m_NomePersona, this.m_DataPrevista);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CRicontatto) && this.Equals((CRicontatto)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRicontatto obj)
            {
                return base.Equals(obj)
                        && DMD.Strings.EQ(this.m_TipoAppuntamento, obj.m_TipoAppuntamento)
                        && DMD.Strings.EQ(this.m_NumeroOIndirizzo, obj.m_NumeroOIndirizzo)
                        && DMD.DateUtils.EQ(this.m_DataPrevista, obj.m_DataPrevista)
                        && DMD.Integers.EQ(this.IDAssegnatoA, obj.IDAssegnatoA)
                        && DMD.Strings.EQ(this.m_NomeAssegnatoA, obj.m_NomeAssegnatoA)
                        && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                        && DMD.Integers.EQ((int)this.m_StatoRicontatto, (int)obj.m_StatoRicontatto)
                        && DMD.DateUtils.EQ(this.m_DataRicontatto, obj.m_DataRicontatto)
                        && DMD.Integers.EQ(this.IDOperatore, obj.IDOperatore)
                        && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                        && DMD.Strings.EQ(this.m_TipoContatto, obj.m_TipoContatto)
                        && DMD.Integers.EQ(this.IDContatto, obj.IDContatto)
                        && DMD.Integers.EQ(this.IDPersona, obj.IDPersona)
                        && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                        && DMD.Strings.EQ(this.m_SourceName, obj.m_SourceName)
                        && DMD.Strings.EQ(this.m_SourceParam, obj.m_SourceParam)
                        && DMD.Integers.EQ(this.m_Promemoria, obj.m_Promemoria)
                        && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                        && DMD.Strings.EQ(this.m_DettaglioStato, obj.m_DettaglioStato)
                        && DMD.Strings.EQ(this.m_DettaglioStato1, obj.m_DettaglioStato1)
                        && DMD.Integers.EQ(this.IDRicontattoPrecedente, obj.IDRicontattoPrecedente)
                        && DMD.Integers.EQ(this.IDRicontattoSuccessivo, obj.IDRicontattoSuccessivo)
                        && DMD.Integers.EQ(this.m_Priorita, obj.m_Priorita)
                        ;
            }

            //protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            //{
            //    bool ret = base.SaveToDatabase(dbConn, force);
            //    MirrorMe(dbConn);
            //    return ret;
            //}

            //protected virtual void MirrorMe(Databases.CDBConnection dbConn)
            //{
            //    if (dbConn.IsRemote())
            //    {
            //    }
            //    else
            //    {
            //        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            //    }
            //}

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataPrevista", m_DataPrevista);
                writer.WriteAttribute("IDAssegnatoA", IDAssegnatoA);
                writer.WriteAttribute("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.WriteAttribute("DataRicontatto", m_DataRicontatto);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("TipoContatto", m_TipoContatto);
                writer.WriteAttribute("IDContatto", IDContatto);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("SourceName", m_SourceName);
                writer.WriteAttribute("SourceParam", m_SourceParam);
                writer.WriteAttribute("Promemoria", m_Promemoria);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("StatoRicontatto", (int?)m_StatoRicontatto);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
                writer.WriteAttribute("DettaglioStato1", m_DettaglioStato1);
                writer.WriteAttribute("TipoAppuntamento", m_TipoAppuntamento);
                writer.WriteAttribute("NumeroOIndirizzo", m_NumeroOIndirizzo);
                writer.WriteAttribute("IDRicPrec", IDRicontattoPrecedente);
                writer.WriteAttribute("IDRicSucc", IDRicontattoSuccessivo);
                writer.WriteAttribute("Priorita", m_Priorita);
                base.XMLSerialize(writer);
                writer.WriteTag("Parameters", this.Parameters);
                writer.WriteTag("Note", m_Note);
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
                    case "DataPrevista":
                        {
                            m_DataPrevista = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDAssegnatoA":
                        {
                            m_IDAssegnatoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Set m_AssegnatoA = Nothing
                    case "NomeAssegnatoA":
                        {
                            m_NomeAssegnatoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRicontatto":
                        {
                            m_DataRicontatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoRicontatto":
                        {
                            m_StatoRicontatto = (StatoRicontatto)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "TipoContatto":
                        {
                            m_TipoContatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContatto":
                        {
                            m_IDContatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Set m_Contatto = Nothing
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Set m_Persona = Nothing
                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceName":
                        {
                            m_SourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceParam":
                        {
                            m_SourceParam = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Promemoria":
                        {
                            m_Promemoria = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    
                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var @case when @case == "StatoRicontatto":
                        {
                            m_StatoRicontatto = (StatoRicontatto)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioStato1":
                        {
                            m_DettaglioStato1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoAppuntamento":
                        {
                            m_TipoAppuntamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroOIndirizzo":
                        {
                            m_NumeroOIndirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRicPrec":
                        {
                            m_IDRicontattoPrecedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRicSucc":
                        {
                            m_IDRicontattoSuccessivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Priorita":
                        {
                            m_Priorita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Ricontatti.Database;
            //}



            // Protected Overridable Sub ProgrammaNotifica()
            // If ( _
            // Me.Operatore IsNot Nothing AndAlso _
            // Me.Stato = ObjectStatus.OBJECT_VALID AndAlso _
            // (Me.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO OrElse Me.StatoRicontatto = Anagrafica.StatoRicontatto.RIMANDATO) AndAlso _
            // Me.Promemoria > 0 _
            // ) Then
            // Dim notifiche As CCollection(Of Notifica) = Sistema.Notifiche.GetAlertsBySource(Me, "")
            // Dim notifica As Notifica
            // Dim text As String = Me.DescrizioneTipoRicontatto & vbNewLine & "Ora prevista: " & Formats.FormatUserDateTime(Me.DataPrevista) & vbNewLine & "Persona: " & Me.NomePersona & vbNewLine & Me.Note
            // Dim data As Date = Calendar.DateAdd(DateTimeInterval.Minute, -Me.Promemoria, Me.DataPrevista)
            // If (notifiche.Count > 0) Then
            // notifica = notifiche(0)
            // If (notifica.StatoNotifica <= StatoNotifica.CONSEGNATA) Then
            // notifica.Descrizione = text
            // notifica.Data = data
            // notifica.Target = Me.Operatore
            // notifica.Save()
            // End If
            // Else
            // notifica = Sistema.Notifiche.ProgramAlert(Me.Operatore, text, data, Me, "Promemoria " & Me.TipoAppuntamento)
            // End If
            // Else
            // Sistema.Notifiche.CancelPendingAlertsBySource(Nothing, Me, "Promemoria " & Me.TipoAppuntamento)
            // End If
            // End Sub

            //protected override void OnCreate(SystemEvent e)
            //{
            //    base.OnCreate(e);
            //    NotifyCreated();
            //}

            //protected override void OnDelete(SystemEvent e)
            //{
            //    base.OnDelete(e);
            //    NotifyDeleted();
            //}

            //protected override void OnModified(SystemEvent e)
            //{
            //    base.OnModified(e);
            //    NotifyModified();
            //}

            //protected virtual void NotifyCreated()
            //{
            //    Ricontatti.doRicontattoCreated(new RicontattoEventArgs(this));
            //}

            //protected virtual void NotifyDeleted()
            //{
            //    Ricontatti.doRicontattoDeleted(new RicontattoEventArgs(this));
            //}

            //protected virtual void NotifyModified()
            //{
            //    Ricontatti.doRicontattoModified(new RicontattoEventArgs(this));
            //}

            //protected override void OnAfterSave(SystemEvent e)
            //{
            //    base.OnAfterSave(e);
            //    // Me.ProgrammaNotifica()
            //}


            /// <summary>
            /// Restituisce la descrizione dell'appuntamento
            /// </summary>
            public virtual string DescrizioneTipoRicontatto
            {
                get
                {
                    switch (DMD.Strings.LCase(TipoAppuntamento) ?? "")
                    {
                        case "telefonata":
                            {
                                return "Appuntamento Telefonico";
                            }

                        case "appuntamento":
                            {
                                return "Appuntamento";
                            }

                        default:
                            {
                                return "Altro tipo di appuntamento";
                            }
                    }
                }
            }
        }
    }
}