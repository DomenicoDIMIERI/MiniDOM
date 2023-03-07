using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Rappresenta una elemento di sincronizzazione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class SyncItem 
            : Databases.DBObject, IComparable, IComparable<SyncItem>
        {
            private string m_RemoteSite;
            private string m_ItemType;
            private int m_LocalID;
            private int m_RemoteID;
            private DateTime? m_SyncDate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public SyncItem()
            {
                m_RemoteSite = "";
                m_ItemType = "";
                m_LocalID = 0;
                m_RemoteID = 0;
                m_SyncDate = default;
            }

            /// <summary>
            /// ID del sistema remoto
            /// </summary>
            public string RemoteSite
            {
                get
                {
                    return m_RemoteSite;
                }

                set
                {
                    string oldValue = m_RemoteSite;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RemoteSite = value;
                    DoChanged("RemoteSite", value, oldValue);
                }
            }

            /// <summary>
            /// Tipo dell'oggtto collegato
            /// </summary>
            public string ItemType
            {
                get
                {
                    return m_ItemType;
                }

                set
                {
                    string oldValue = m_ItemType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ItemType = value;
                    DoChanged("ItemType", value, oldValue);
                }
            }

            /// <summary>
            /// ID locale dell'oggetto collegato
            /// </summary>
            public int LocalID
            {
                get
                {
                    return m_LocalID;
                }

                set
                {
                    int oldValue = LocalID;
                    if (oldValue == value)
                        return;
                    m_LocalID = value;
                    DoChanged("LocalID", value, oldValue);
                }
            }

            /// <summary>
            /// ID remoto dell'oggetto collegato
            /// </summary>
            public int RemoteID
            {
                get
                {
                    return m_RemoteID;
                }

                set
                {
                    int oldValue = RemoteID;
                    if (oldValue == value)
                        return;
                    m_RemoteID = value;
                    DoChanged("RemoteID", value, oldValue);
                }
            }

            /// <summary>
            /// Data di sincronizzazione
            /// </summary>
            public DateTime? SyncDate
            {
                get
                {
                    return m_SyncDate;
                }

                set
                {
                    var oldValue = m_SyncDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_SyncDate = value;
                    DoChanged("SyncDate", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Sincronizzazioni;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Syncs";
            }

            /// <summary>
            /// Carica del db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_RemoteSite = reader.Read("RemoteSite", this. m_RemoteSite);
                this.m_ItemType = reader.Read("ItemType", this.m_ItemType);
                this.m_LocalID = reader.Read("LocalID", this.m_LocalID);
                this.m_RemoteID = reader.Read("RemoteID", this.m_RemoteID);
                this.m_SyncDate = reader.Read("SyncDate", this.m_SyncDate);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("RemoteSite", m_RemoteSite);
                writer.Write("ItemType", m_ItemType);
                writer.Write("LocalID", LocalID);
                writer.Write("RemoteID", RemoteID);
                writer.Write("SyncDate", m_SyncDate);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("RemoteSite", typeof(string), 255);
                c = table.Fields.Ensure("ItemType", typeof(string), 255);
                c = table.Fields.Ensure("LocalID", typeof(int), 1);
                c = table.Fields.Ensure("RemoteID", typeof(int), 1);
                c = table.Fields.Ensure("SyncDate", typeof(DateTime), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxRemoteSite", new string[] { "RemoteSite", "RemoteID" }, DBFieldConstraintFlags.None );
                c = table.Constraints.Ensure("idxLocalItem", new string[] { "ItemType", "LocalID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSyncDate", new string[] { "SyncDate" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("RemoteSite", m_RemoteSite);
                writer.WriteAttribute("ItemType", m_ItemType);
                writer.WriteAttribute("LocalID", LocalID);
                writer.WriteAttribute("RemoteID", RemoteID);
                writer.WriteAttribute("SyncDate", m_SyncDate);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deseriaizzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "RemoteSite":
                        {
                            m_RemoteSite = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ItemType":
                        {
                            m_ItemType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LocalID":
                        {
                            m_LocalID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RemoteID":
                        {
                            m_RemoteID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SyncDate":
                        {
                            m_SyncDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
                return DMD.Strings.ConcatArray(this.m_SyncDate, ": ", this.m_ItemType, "[", this.m_LocalID , "] - ", this.m_RemoteSite);
            }

            /// <summary>
            /// Calcola il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_SyncDate, this.m_ItemType , this.m_LocalID);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is SyncItem) && this.Equals ((SyncItem)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(SyncItem obj)
            {
                return base.Equals(obj)
                     &&  DMD.Strings.EQ(this.m_RemoteSite, obj.m_RemoteSite)
                     && DMD.Strings.EQ(this.m_ItemType, obj.m_ItemType)
                     && DMD.Integers.EQ(this.m_LocalID, obj.m_LocalID)
                     && DMD.DateUtils.EQ(this.m_SyncDate, obj.m_SyncDate)
                        ;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(SyncItem obj)
            {
                int ret = DMD.DateUtils.Compare(this.m_SyncDate, obj.m_SyncDate);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_RemoteSite, obj.m_RemoteSite, false);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_ItemType, obj.m_ItemType, false);
                if (ret == 0) ret = DMD.Integers.Compare(this.m_LocalID, obj.m_LocalID);
                if (ret == 0) ret = DMD.Integers.Compare(this.m_RemoteID, obj.m_RemoteID);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((SyncItem)obj); }
        }
    }
}