using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Attributi
        /// </summary>
        [Serializable]
        public class CObjectAttribute 
            : minidom.Databases.DBObject
        {
            private int m_ObjectID;
            [NonSerialized] private object m_Object;
            private string m_AttributeName;
            private string m_AttributeValue;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CObjectAttribute()
            {
                m_ObjectID = 0;
                m_Object = null;
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Attributi; // Sistema.Module;
            }

            /// <summary>
            /// ObjectID
            /// </summary>
            public int ObjectID
            {
                get
                {
                    return DBUtils.GetID(this.m_Object, this.m_ObjectID);
                }

                set
                {
                    int oldValue = this.ObjectID;
                    if (oldValue == value)
                        return;
                    this.m_Object = null;
                    this.m_ObjectID = value;
                    DoChanged("ObjectID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto a cui appartengono gli attributi
            /// </summary>
            public object Object
            {
                get
                {
                    return this.m_Object;
                }

                set
                {
                    var oldValue = m_Object;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    this.m_Object = value;
                    this.m_ObjectID = DBUtils.GetID(value, this.m_ObjectID);
                    this.DoChanged("Object", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'attributo
            /// </summary>
            public string AttributeName
            {
                get
                {
                    return m_AttributeName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AttributeName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AttributeName = value;
                    DoChanged("AttributeName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore dell'attributo
            /// </summary>
            public string AttributeValue
            {
                get
                {
                    return m_AttributeValue;
                }

                set
                {
                    string oldValue = m_AttributeValue;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AttributeValue = value;
                    DoChanged("AttributeValue", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome del discriminatore
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ObjectAttributes";
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_ObjectID = reader.Read("Object", this.m_ObjectID);
                this.m_AttributeName = reader.Read("AttributeName", this.m_AttributeName);
                this.m_AttributeValue = reader.Read("AttributeValue", this.m_AttributeValue);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Object", ObjectID);
                writer.Write("AttributeName", m_AttributeName);
                writer.Write("AttributeValue", m_AttributeValue);
                writer.Write("AttributeType", DMD.RunTime.vbTypeName(m_AttributeValue));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Object", typeof(int), 1);
                c = table.Fields.Ensure("AttributeName", typeof(string), 255);
                c = table.Fields.Ensure("AttributeValue", typeof(string), 255);
                c = table.Fields.Ensure("AttributeType", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxNome", new string[] { "Object", "AttributeName", "Stato" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxValues", new string[] { "AttributeType", "AttributeValue" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Object", ObjectID);
                writer.WriteAttribute("AttributeName", m_AttributeName);
                writer.WriteAttribute("AttributeType", DMD.RunTime.vbTypeName(m_AttributeValue));
                base.XMLSerialize(writer);
                writer.WriteTag("AttributeValue", m_AttributeValue);
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
                    case "Object":
                        {
                            m_ObjectID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AttributeName":
                        {
                            m_AttributeName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AttributeType":
                        {
                            break;
                        }

                    case "AttributeValue":
                        {
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
                var ret = new System.Text.StringBuilder();
                ret.Append("[");
                ret.Append(this.AttributeName);
                ret.Append(" = ");
                ret.Append(this.m_AttributeValue);
                ret.Append("]");
                return ret.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_AttributeName, this.m_AttributeValue);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CObjectAttribute) && this.Equals((CObjectAttribute)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CObjectAttribute obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.ObjectID, obj.ObjectID)
                    && DMD.Strings.EQ(this.m_AttributeName, obj.m_AttributeName)
                    && DMD.Strings.EQ(this.m_AttributeValue, obj.m_AttributeValue)
                    ;
            }


        }
    }
}