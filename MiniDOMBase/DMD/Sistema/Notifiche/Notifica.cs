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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Stato delle notifiche di sistema
        /// </summary>
        /// <remarks></remarks>
        public enum StatoNotifica : int
        {
            /// <summary>
            /// La notifica non è stata consegnata
            /// </summary>
            /// <remarks></remarks>
            NON_CONSEGNATA = 0,

            /// <summary>
            /// L'utente ha ricevuto la notifica ma non l'ha ancora letta
            /// </summary>
            /// <remarks></remarks>
            CONSEGNATA = 1,

            /// <summary>
            /// La notifica è stata letta
            /// </summary>
            /// <remarks></remarks>
            LETTA = 2,

            /// <summary>
            /// La notifica è stata annullata
            /// </summary>
            /// <remarks></remarks>
            ANNULLATA = 3
        }

        /// <summary>
        /// Rappresenta una notifica del sistema
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class Notifica 
            : Databases.DBObjectPO
        {
            private DateTime m_Data;                          // Data a partire dalla quale deve essere visualizzata la nofica
            private string m_Context;                     // Contesto in cui è stata generata la nofica
            private string m_SourceName;                  // Nome dell'oggetto a cui è associata la notifica
            private int m_SourceID;                   // ID dell'oggetto a cui è associata la notifica
            private int m_TargetID;                   // ID dell'utente a cui è destinata la notifica
            [NonSerialized] private CUser m_Target;                       // Utente a cui è destinata la notifica
            private string m_TargetName;                  // Nome dell'utente a cui è destinata la notifica
            private string m_Descrizione;                 // Descrizione della notifica
            private DateTime? m_DataConsegna;      // Data di prima visualizzazione da parte dell'utente
            private DateTime? m_DataLettura;      // Data di lettura della notifica
            private StatoNotifica m_StatoNotifica;                // Stato della notifica
            private string m_Categoria;                   // Specifica una categoria

            /// <summary>
            /// Costruttore
            /// </summary>
            public Notifica()
            {
                m_Data = default;
                m_Context = DMD.Strings.vbNullString;
                m_SourceName = DMD.Strings.vbNullString;
                m_SourceID = 0;
                m_TargetID = 0;
                m_Target = null;
                m_TargetName = DMD.Strings.vbNullString;
                m_Descrizione = DMD.Strings.vbNullString;
                m_DataConsegna = default;
                m_DataLettura = default;
                m_StatoNotifica = StatoNotifica.NON_CONSEGNATA;
                m_Categoria = "";
            }

            /// <summary>
            /// Restituisce o imposta la categoria della notifica
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
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data a partire dalla quale deve essere visualizzata la nofifica
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
            /// Restituisce o imposta una stringa che descrive il contesto di validità dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Context
            {
                get
                {
                    return m_Context;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Context;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Context = value;
                    DoChanged("Context", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'oggetto che ha generato il promemoria
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
                    string oldValeu = m_SourceName;
                    if ((oldValeu ?? "") == (value ?? ""))
                        return;
                    m_SourceName = value;
                    DoChanged("SourceName", value, oldValeu);
                }
            }

            /// <summary>
            /// Retituisce o imposta l'ID dell'oggetto che ha generato questa notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return m_SourceID;
                }

                set
                {
                    int oldValue = m_SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è destinata la notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int TargetID
            {
                get
                {
                    return DBUtils.GetID(m_Target, m_TargetID);
                }

                set
                {
                    int oldValue = TargetID;
                    if (oldValue == value)
                        return;
                    m_TargetID = value;
                    m_Target = null;
                    DoChanged("TargetID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente a cui è destinata la notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser Target
            {
                get
                {
                    if (m_Target is null)
                        m_Target = Users.GetItemById(m_TargetID);
                    return m_Target;
                }

                set
                {
                    var oldValue = Target;
                    if (oldValue == value)
                        return;
                    m_Target = value;
                    m_TargetID = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_TargetName = value.Nominativo;
                    DoChanged("Target", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il destinatario della notifica
            /// </summary>
            /// <param name="value"></param>
            internal void SetTarget(CUser value)
            {
                m_Target = value;
                m_TargetID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente a cui è destinata la notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TargetName
            {
                get
                {
                    return m_TargetName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TargetName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TargetName = value;
                    DoChanged("TargetName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione della notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta la data e l'ora in cui la notifica è stata visualizzata sul PC dell'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }

                set
                {
                    var oldValue = m_DataConsegna;
                    if (oldValue == value == true)
                        return;
                    m_DataConsegna = value;
                    DoChanged("DataConsegna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di lettura della notifica cioè la data in cui l'utente ha eseguito un'azioen sulla notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataLettura
            {
                get
                {
                    return m_DataLettura;
                }

                set
                {
                    var oldValue = m_DataLettura;
                    if (oldValue == value == true)
                        return;
                    m_DataLettura = value;
                    DoChanged("DataLettura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della notifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoNotifica StatoNotifica
            {
                get
                {
                    return m_StatoNotifica;
                }

                set
                {
                    var oldValue = m_StatoNotifica;
                    if (oldValue == value)
                        return;
                    m_StatoNotifica = value;
                    DoChanged("StatoNofica", value, oldValue);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Data = reader.Read("Data", this. m_Data);
                this.m_Context = reader.Read("Context", this.m_Context);
                this.m_SourceName = reader.Read("SourceName", this.m_SourceName);
                this.m_SourceID = reader.Read("SourceID", this.m_SourceID);
                this.m_TargetID = reader.Read("TargetID", this.m_TargetID);
                this.m_TargetName = reader.Read("TargetName", this.m_TargetName);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                this.m_DataConsegna = reader.Read("DataConsegna", this.m_DataConsegna);
                this.m_DataLettura = reader.Read("DataLettura", this.m_DataLettura);
                this.m_StatoNotifica = reader.Read("StatoNotifica", this.m_StatoNotifica);
                this.m_Categoria = reader.Read("Categoria", this.m_Categoria);
                return base.LoadFromRecordset(reader);
            }


            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Data", m_Data);
                //writer.Write("DataStr", Databases.DBUtils.ToDBDateStr(m_Data));
                writer.Write("Context", m_Context);
                writer.Write("SourceName", m_SourceName);
                writer.Write("SourceID", SourceID);
                writer.Write("TargetID", TargetID);
                writer.Write("TargetName", m_TargetName);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("DataConsegna", m_DataConsegna);
                writer.Write("DataLettura", m_DataLettura);
                writer.Write("StatoNotifica", m_StatoNotifica);
                writer.Write("Categoria", m_Categoria);
                return base.SaveToRecordset(writer);
            }


            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                //writer.Write("DataStr", Databases.DBUtils.ToDBDateStr(m_Data));
                c = table.Fields.Ensure("Context", typeof(string), 255);
                c = table.Fields.Ensure("SourceName", typeof(string), 255);
                c = table.Fields.Ensure("SourceID", typeof(int), 1);
                c = table.Fields.Ensure("TargetName", typeof(string), 255);
                c = table.Fields.Ensure("TargetID", typeof(int), 1);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("DataConsegna", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataLettura", typeof(DateTime), 1);
                c = table.Fields.Ensure("StatoNotifica", typeof(int), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxContext", new string[] { "Context", "Categoria" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceName", "SourceID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTarget", new string[] { "TargetName", "TargetID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoConsegna", new string[] { "StatoNotifica" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataConsegna" , "DataLettura" }, DBFieldConstraintFlags.None);
            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("Context", m_Context);
                writer.WriteAttribute("SourceName", m_SourceName);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("TargetID", TargetID);
                writer.WriteAttribute("TargetName", m_TargetName);
                writer.WriteAttribute("DataConsegna", m_DataConsegna);
                writer.WriteAttribute("DataLettura", m_DataLettura);
                writer.WriteAttribute("StatoNotifica", (int?)m_StatoNotifica);
                writer.WriteAttribute("Categoria", m_Categoria);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
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

                    case "Context":
                        {
                            m_Context = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceName":
                        {
                            m_SourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TargetID":
                        {
                            m_TargetID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TargetName":
                        {
                            m_TargetName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataConsegna":
                        {
                            m_DataConsegna = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataLettura":
                        {
                            m_DataLettura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoNotifica":
                        {
                            m_StatoNotifica = (StatoNotifica)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            //    return minidom.Sistema.Notifiche.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Notifiche;
            }

            /// <summary>
            /// TableName
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SYSNotify";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_Data, "/", this.m_TargetName, ": ", this.m_Descrizione);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Data, this.m_SourceID, this.m_TargetID, this.m_Descrizione);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Notifica) && this.Equals((Notifica)obj);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(Notifica obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.Strings.EQ(this.m_Context, obj.m_Context)
                    && DMD.Strings.EQ(this.m_SourceName, obj.m_SourceName)
                    && DMD.Integers.EQ(this.m_SourceID, obj.m_SourceID)
                    && DMD.Integers.EQ(this.m_TargetID, obj.m_TargetID)
                    && DMD.Strings.EQ(this.m_TargetName, obj.m_TargetName)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.DateUtils.EQ(this.m_DataConsegna, obj.m_DataConsegna)
                    && DMD.DateUtils.EQ(this.m_DataLettura, obj.m_DataLettura)
                    && DMD.Integers.EQ((int)this.m_StatoNotifica, (int)obj.m_StatoNotifica)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    ;
            }
        }
    }
}