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
        /// Rappresenta una richiesta fatta da un utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTicketAnsware 
            : minidom.Databases.DBObject, IComparable, IComparable<CTicketAnsware>
        {
            private int m_IDTicket;
            [NonSerialized] private CTicket m_Ticket;
            private int m_IDOperatore;
            [NonSerialized] private CUser m_Operatore;
            private string m_NomeOperatore;
            private DateTime m_Data;
            private TicketStatus m_StatoTicket;
            private string m_Messaggio;
            private CCollection<CAttachment> m_Attachments;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketAnsware()
            {
                m_IDTicket = 0;
                m_Ticket = null;
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_Data = DMD.DateUtils.Now();
                m_Messaggio = "";
                m_Attachments = null;
            }

            /// <summary>
            /// Restituisce o imposta l'ID del ticket a cui appartiene il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDTicket
            {
                get
                {
                    return DBUtils.GetID(m_Ticket, m_IDTicket);
                }

                set
                {
                    int oldValue = IDTicket;
                    if (oldValue == value)
                        return;
                    m_IDTicket = value;
                    m_Ticket = null;
                    DoChanged("Ticket", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il ticket a cui appartiene il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CTicket Ticket
            {
                get
                {
                    lock (this)
                    {
                        if (m_Ticket is null)
                            m_Ticket = Tickets.GetItemById(m_IDTicket);
                        return m_Ticket;
                    }
                }

                set
                {
                    CTicket oldValue;
                    lock (this)
                    {
                        oldValue = m_Ticket;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Ticket = value;
                        m_IDTicket = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Ticket", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il ticket
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetTicket(CTicket value)
            {
                m_Ticket = value;
                m_IDTicket = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha inserito il messaggio
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
            /// Restituisce o imposta l'operatore che ha inserito il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser Operatore
            {
                get
                {
                    lock (this)
                    {
                        if (m_Operatore is null)
                            m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                        return m_Operatore;
                    }
                }

                set
                {
                    Sistema.CUser oldValue;
                    lock (this)
                    {
                        oldValue = Operatore;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Operatore = value;
                        m_IDOperatore = DBUtils.GetID(value, 0);
                        m_NomeOperatore = (value is object)? value.Nominativo : "";
                    }

                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha inserito il messaggio
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
                    string oldValue = m_NomeOperatore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui è stato inserito il messaggio
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Messaggio
            {
                get
                {
                    return m_Messaggio;
                }

                set
                {
                    string oldValue = m_Messaggio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Messaggio = value;
                    DoChanged("Messaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato in cui viene portato il ticket in seguito a questa risposta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TicketStatus StatoTicket
            {
                get
                {
                    return m_StatoTicket;
                }

                set
                {
                    var oldValue = m_StatoTicket;
                    if (oldValue == value)
                        return;
                    m_StatoTicket = value;
                    DoChanged("StatoTicket", value, oldValue);
                }
            }

            /// <summary>
            /// Allegati
            /// </summary>
            public CCollection<CAttachment> Attachments
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attachments is null)
                            m_Attachments = new CCollection<CAttachment>();
                        return m_Attachments;
                    }
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Messaggio;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.m_Data.GetHashCode();
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CTicketAnsware) && this.Equals((CTicketAnsware)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CTicketAnsware obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDTicket, obj.m_IDTicket)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.Integers.EQ((int)this.m_StatoTicket, (int)obj.m_StatoTicket)
                    && DMD.Strings.EQ(this.m_Messaggio, obj.m_Messaggio)
                    ;
                    //private CCollection<CAttachment> m_Attachments;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Tickets.Messages;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SupportTicketsAnswares";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDTicket = reader.Read("IDTicket", this.m_IDTicket);
                this.m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                this.m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                this.m_Data = reader.Read("Data", this.m_Data);
                this.m_Messaggio = reader.Read("Messaggio", this.m_Messaggio);
                this.m_StatoTicket = reader.Read("StatoTicket", this.m_StatoTicket);
                string tmp = reader.Read("Attachments", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    var col = (CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                    m_Attachments = new CCollection<CAttachment>();
                    m_Attachments.AddRange(col);
                }

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDTicket", this.IDTicket);
                writer.Write("IDOperatore", this.IDOperatore);
                writer.Write("NomeOperatore", this.m_NomeOperatore);
                writer.Write("Data", this.m_Data);
                writer.Write("Messaggio", this.m_Messaggio);
                writer.Write("StatoTicket", this.m_StatoTicket);
                writer.Write("Attachments", DMD.XML.Utils.Serializer.Serialize(this.Attachments));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDTicket", typeof(int), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("Messaggio", typeof(string), 0);
                c = table.Fields.Ensure("StatoTicket", typeof(int), 1);
                c = table.Fields.Ensure("Attachments", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxTicket", new string[] { "IDTicket" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxData", new string[] { "Data" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMessaggio", new string[] { "Messaggio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idStatoTicket", new string[] { "StatoTicket" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Attachments", typeof(string), 0);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDTicket", this.IDTicket);
                writer.WriteAttribute("IDOperatore", this.IDOperatore);
                writer.WriteAttribute("NomeOperatore", this.m_NomeOperatore);
                writer.WriteAttribute("Data", this.m_Data);
                writer.WriteAttribute("StatoTicket", (int?)this.m_StatoTicket);
                base.XMLSerialize(writer);
                writer.WriteTag("Attachments", this.Attachments);
                writer.WriteTag("Messaggio", this.m_Messaggio);
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
                    case "IDTicket":
                        {
                            m_IDTicket = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoTicket":
                        {
                            m_StatoTicket = (TicketStatus)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Messaggio":
                        {
                            m_Messaggio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attachments":
                        {
                            m_Attachments = new CCollection<Sistema.CAttachment>();
                            m_Attachments.AddRange((IEnumerable)fieldValue);
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
            /// Compara due messaggi per data
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CTicketAnsware other)
            {
                return DMD.DateUtils.Compare(m_Data, other.m_Data);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CTicketAnsware)obj);
            }
        }
    }
}