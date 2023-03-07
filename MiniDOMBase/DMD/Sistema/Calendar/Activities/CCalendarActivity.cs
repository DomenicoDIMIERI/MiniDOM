using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {



        /// <summary>
        /// Oggetto che rappresenta un attività o un appuntamento del calendario
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCalendarActivity 
            : Databases.DBObject, ICalendarActivity
        {
            private string m_Categoria;
            private bool m_GiornataIntera;
            private DateTime m_DataInizio;
            private DateTime? m_DataFine;
            private string m_Descrizione;
            private string m_Luogo;
            private string m_Note;
            private StatoAttivita m_StatoAttivita;
            private int m_OperatorID;
            [NonSerialized] private CUser m_Operator;
            private string m_OperatorName;
            private int m_IDAssegnatoA;
            [NonSerialized] private CUser m_AssegnatoA;
            private string m_NomeAssegnatoA;
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_NomePersona;
            private int m_Promemoria;
            private int m_Ripetizione;
            [NonSerialized] private object m_Tag;
            private string m_IconURL;
            private string m_ProviderName;
            [NonSerialized] private ICalendarProvider m_Provider;
            private int m_Priorita;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCalendarActivity()
            {
                m_GiornataIntera = false;
                m_DataInizio = default;
                m_DataFine = default;
                m_Descrizione = "";
                m_Luogo = "";
                m_Note = "";
                m_StatoAttivita = 0;
                m_OperatorID = 0;
                m_Operator = null;
                m_OperatorName = "";
                m_Flags = (int)CalendarActivityFlags.None;
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
                m_Promemoria = 300;
                m_Ripetizione = 0;
                m_Categoria = "";
                m_Tag = null;
                m_IconURL = "";
                m_IDAssegnatoA = 0;
                m_AssegnatoA = null;
                m_NomeAssegnatoA = "";
                m_ProviderName = "DEFCALPROV";
                m_Provider = null;
                m_Priorita = 0;
            }
             
            /// <summary>
            /// Restituisce o imposta la priorità (crescente) dell'evento.
            /// Gli eventi sono ordinati prima per priorità e poi per data
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Calendar;
            }

            /// <summary>
            /// Restituisce il nome del provider che gestisce l'attività
            /// </summary>
            public string ProviderName
            {
                get
                {
                    return m_ProviderName;
                }
            }

            /// <summary>
            /// Restituisce il provider che gestisce l'attività
            /// </summary>
            public ICalendarProvider Provider
            {
                get
                {
                    if (m_Provider is null)
                        m_Provider = minidom.Sistema.Calendar.GetProviderByName(m_ProviderName);
                    return m_Provider;
                }
            }

            /// <summary>
            /// Imposta il provider che gestisce l'attività
            /// </summary>
            /// <param name="p"></param>
            protected internal virtual void SetProvider(ICalendarProvider p)
            {
                m_Provider = p;
                m_ProviderName = p.UniqueName;
            }

            /// <summary>
            /// Restituisce o imposta i flag
            /// </summary>
            public new CalendarActivityFlags Flags
            {
                get
                {
                    return (CalendarActivityFlags)m_Flags;
                }

                set
                {
                    var oldValue = (CalendarActivityFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria dell'attività
            /// </summary>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    string oldValue = m_Categoria;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'icona associata all'attività
            /// </summary>
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
            /// Restituisce o imposta lo stato dell'attività
            /// </summary>
            public StatoAttivita StatoAttivita
            {
                get
                {
                    return m_StatoAttivita;
                }

                set
                {
                    var oldValue = m_StatoAttivita;
                    if (oldValue == value)
                        return;
                    m_StatoAttivita = value;
                    DoChanged("StatoAttivita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio dell'attività
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il luogo dell'attività
            /// </summary>
            public string Luogo
            {
                get
                {
                    return m_Luogo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Luogo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Luogo = value;
                    DoChanged("Luogo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una nota per l'attività
            /// </summary>
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
            /// Restituisce o imposta un valore booleano che indica se l'attività é prevista per la giornata intera
            /// </summary>
            public bool GiornataIntera
            {
                get
                {
                    return m_GiornataIntera;
                }

                set
                {
                    if (m_GiornataIntera == value)
                        return;
                    m_GiornataIntera = value;
                    DoChanged("GiornataIntera", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di minuti del promemoria
            /// </summary>
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
            /// Restituisce o imposta il numero di ripetizioni
            /// </summary>
            public int Ripetizione
            {
                get
                {
                    return m_Ripetizione;
                }

                set
                {
                    int oldValue = m_Ripetizione;
                    if (oldValue == value)
                        return;
                    m_Ripetizione = value;
                    DoChanged("Ripetizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data inizio
            /// </summary>
            public DateTime DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data fine
            /// </summary>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'operatore che ha svolto l'attività
            /// </summary>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operator, m_OperatorID);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_Operator = null;
                    m_OperatorID = value;
                    DoChanged("OperatorID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore a cui è assegnata l'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser Operatore
            {
                get
                {
                    if (m_Operator is null)
                        m_Operator = Users.GetItemById(m_OperatorID);
                    return m_Operator;
                }

                set
                {
                    var oldValue = Operatore;
                    if (oldValue == value)
                        return;
                    m_Operator = value;
                    m_OperatorID = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_OperatorName = value.Nominativo;
                    DoChanged("Operator", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore a cui è assegnata l'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_OperatorName;
                }

                set
                {
                    string oldValue = m_OperatorName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_OperatorName = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'operatore a cui è assegnata l'attività
            /// </summary>
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
            /// Restituisce o imposta l'operatore a cui è assegnata l'attività
            /// </summary>
            public CUser AssegnatoA
            {
                get
                {
                    lock (this)
                    {
                        if (m_AssegnatoA is null)
                            m_AssegnatoA = Users.GetItemById(m_IDAssegnatoA);
                        return m_AssegnatoA;
                    }
                }

                set
                {
                    CUser oldValue;
                    lock (this)
                    {
                        oldValue = AssegnatoA;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_AssegnatoA = value;
                        m_IDAssegnatoA = DBUtils.GetID(value, 0);
                        if (value is object)
                            m_NomeAssegnatoA = value.Nominativo;
                    }

                    DoChanged("AssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore a cui è assegnata l'attività
            /// </summary>
            public string NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
                }

                set
                {
                    string oldValue = m_NomeAssegnatoA;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssegnatoA = value;
                    DoChanged("NomeAssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un tag
            /// </summary>
            public object Tag
            {
                get
                {
                    return m_Tag;
                }

                set
                {
                    m_Tag = value;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della persona associata all'attività
            /// </summary>
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
                    DoChanged("IDpersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona associata all'attività
            /// </summary>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    lock (this)
                    {
                        if (m_Persona is null)
                            m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                        return m_Persona;
                    }
                }

                set
                {
                    Anagrafica.CPersona oldValue;
                    lock (this)
                    {
                        oldValue = m_Persona;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Persona = value;
                        m_IDPersona = DBUtils.GetID(value, 0);
                        if (value is object)
                            m_NomePersona = value.Nominativo;
                    }

                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona associata all'attività
            /// </summary>
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
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CalendarActivities";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Categoria = reader.Read("Categoria", this.m_Categoria);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                m_StatoAttivita = reader.Read("StatoAttivita", this.m_StatoAttivita);
                m_OperatorID = reader.Read("Operatore", this.m_OperatorID);
                m_OperatorName = reader.Read("NomeOperatore", this.m_OperatorName);
                m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                m_NomePersona = reader.Read("NomePersona", this.m_NomePersona);
                m_Note = reader.Read("Note", this.m_Note);
                m_Luogo = reader.Read("Luogo", this.m_Luogo);
                m_Promemoria = reader.Read("Promemoria", this.m_Promemoria);
                m_Ripetizione = reader.Read("Ripetizione", this.m_Ripetizione);
                m_GiornataIntera = reader.Read("GiornataIntera", this.m_GiornataIntera);
                m_IconURL = reader.Read("IconURL", this.m_IconURL);
                m_IDAssegnatoA = reader.Read("IDAssegnatoA", this.m_IDAssegnatoA);
                m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", this.m_NomeAssegnatoA);
                m_ProviderName = reader.Read("ProviderName", this.m_ProviderName);
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
                writer.Write("Categoria", m_Categoria);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("StatoAttivita", m_StatoAttivita);
                writer.Write("Operatore", IDOperatore);
                writer.Write("NomeOperatore", m_OperatorName);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("Note", m_Note);
                writer.Write("Luogo", m_Luogo);
                writer.Write("Promemoria", m_Promemoria);
                writer.Write("Ripetizione", m_Ripetizione);
                writer.Write("GiornataIntera", m_GiornataIntera);
                writer.Write("IconURL", m_IconURL);
                writer.Write("IDAssegnatoA", IDAssegnatoA);
                writer.Write("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.Write("ProviderName", m_ProviderName);
                writer.Write("Priorita", m_Priorita);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoAttivita", typeof(int), 1);
                c = table.Fields.Ensure("Operatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("Flags", typeof(int), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("Luogo", typeof(string), 0);
                c = table.Fields.Ensure("Promemoria", typeof(int), 1);
                c = table.Fields.Ensure("Ripetizione", typeof(int), 1);
                c = table.Fields.Ensure("GiornataIntera", typeof(bool), 1);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("IDAssegnatoA", typeof(int), 1);
                c = table.Fields.Ensure("NomeAssegnatoA", typeof(string), 255);
                c = table.Fields.Ensure("ProviderName", typeof(string), 255);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "ProviderName", "Priorita", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione", "Luogo", "Note" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoAtt", new string[] { "StatoAttivita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "Operatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAssegnatoA", new string[] { "IDAssegnatoA", "NomeAssegnatoA", "GiornataIntera" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPromemoria", new string[] { "Promemoria", "Ripetizione", "IconURL" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Descrizione;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Descrizione, this.m_ProviderName);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("GiornataIntera", m_GiornataIntera);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("StatoAttivita", (int?)m_StatoAttivita);
                writer.WriteAttribute("OperatorID", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_OperatorName);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("Promemoria", m_Promemoria);
                writer.WriteAttribute("Ripetizione", m_Ripetizione);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("IDAssegnatoA", IDAssegnatoA);
                writer.WriteAttribute("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.WriteAttribute("ProviderName", m_ProviderName);
                writer.WriteAttribute("Priorita", m_Priorita);
                if (m_Tag is Databases.IDBObjectBase)
                {
                    writer.WriteAttribute("Tag", DMD.RunTime.vbTypeName(m_Tag) + ":" + DBUtils.GetID((Databases.IDBObjectBase)m_Tag));
                }
                else
                {
                    writer.WriteAttribute("Tag", DMD.Strings.CStr(m_Tag));
                }

                base.XMLSerialize(writer);
                writer.WriteTag("Luogo", m_Luogo);
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
                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "GiornataIntera":
                        {
                            m_GiornataIntera = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoAttivita":
                        {
                            m_StatoAttivita = (StatoAttivita)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OperatorID":
                        {
                            m_OperatorID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_OperatorName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAssegnatoA":
                        {
                            m_IDAssegnatoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAssegnatoA":
                        {
                            m_NomeAssegnatoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Promemoria":
                        {
                            m_Promemoria = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Ripetizione":
                        {
                            m_Ripetizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Luogo":
                        {
                            m_Luogo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    case "ProviderName":
                        {
                            m_ProviderName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Priorita":
                        {
                            m_Priorita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Tag":
                        {
                            if (DMD.Strings.InStr(DMD.Strings.CStr(fieldValue), ":") > 0)
                            {
                                var n = DMD.Strings.Split(DMD.Strings.CStr(fieldValue), ":");
                                m_Tag = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(n[0], DMD.Integers.ValueOf(n[1]));
                            }
                            else
                            {
                                m_Tag = fieldValue;
                            }

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
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual int CompareTo(CCalendarActivity obj)
            {
                int ret;
                ret = DMD.Strings.Compare(m_ProviderName, obj.m_ProviderName, true);
                if (ret == 0)
                    ret = DMD.Arrays.Compare(m_Priorita, obj.m_Priorita);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataInizio, obj.m_DataInizio);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataFine, obj.m_DataFine);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CCalendarActivity)obj);
            }

            /// <summary>
            /// Elimina l'attività
            /// </summary>
            /// <param name="force"></param>
            public override void Delete(bool force = false)
            {
                Provider.DeleteActivity(this, force);
            }

            /// <summary>
            /// Salva l'attività
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                Provider.SaveActivity(this, force);
            }

            /// <summary>
            /// Salvataggio nel db
            /// </summary>
            /// <param name="force"></param>
            protected internal void OldSave(bool force)
            {
                base.Save(force);
            }

            /// <summary>
            /// Eliminazione dal db
            /// </summary>
            /// <param name="force"></param>
            protected internal void OldDelete(bool force)
            {
                base.Delete(force);
            }

            /// <summary>
            /// Imposta il provider
            /// </summary>
            /// <param name="p"></param>
            void ICalendarActivity.SetProvider(ICalendarProvider p)
            {
                this.SetProvider(p);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CCalendarActivity) && this.Equals((CCalendarActivity)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CCalendarActivity obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Booleans.EQ(this.m_GiornataIntera, obj.m_GiornataIntera)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_Luogo, obj.m_Luogo)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Integers.EQ((int)this.m_StatoAttivita, (int)obj.m_StatoAttivita)
                    && DMD.Integers.EQ(this.m_OperatorID, obj.m_OperatorID)
                    && DMD.Strings.EQ(this.m_OperatorName, obj.m_OperatorName)
                    && DMD.Integers.EQ(this.m_IDAssegnatoA, obj.m_IDAssegnatoA)
                    && DMD.Strings.EQ(this.m_NomeAssegnatoA, obj.m_NomeAssegnatoA)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                    && DMD.Integers.EQ(this.m_Promemoria, obj.m_Promemoria)
                    && DMD.Integers.EQ(this.m_Ripetizione, obj.m_Ripetizione)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.Strings.EQ(this.m_ProviderName, obj.m_ProviderName)
                    && DMD.Integers.EQ(this.m_Priorita, obj.m_Priorita)
                    ;
            //[NonSerialized] private ICalendarProvider m_Provider;
            }
        }
    }
}