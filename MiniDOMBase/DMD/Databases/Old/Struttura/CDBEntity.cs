using System;
using System.Data;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Databases
    {
        public abstract class CDBEntity : CDBObject
        {
            private string m_Catalog;
            private string m_Schema;
            private string m_Type;
            private string m_Guid;
            private string m_PropID;
            private string m_Description;
            private DateTime? m_DateCreated;
            private DateTime? m_DateModified;
            private CDBEntityFields m_Fields;
            private CDBConstraintsCollection m_Constraints;
            private bool m_IsHidden;
            private bool m_TrackChanges;

            public CDBEntity()
            {
                m_Catalog = DMD.Strings.vbNullString;
                m_Schema = DMD.Strings.vbNullString;
                m_Type = DMD.Strings.vbNullString;
                m_Guid = DMD.Strings.vbNullString;
                m_PropID = DMD.Strings.vbNullString;
                m_Description = DMD.Strings.vbNullString;
                m_DateCreated = default;
                m_DateModified = default;
                m_Fields = null;
                m_Constraints = null;
                m_IsHidden = false;
                m_TrackChanges = false;
            }

            public CDBEntity(string name) : this()
            {
                SetName(name);
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che impone al sistema di tenere traccia di tutte le modifiche fatte ai campi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool TrackChanges
            {
                get
                {
                    return m_TrackChanges;
                }

                set
                {
                    m_TrackChanges = value;
                }
            }

            public CDBEntityFields Fields
            {
                get
                {
                    lock (this)
                    {
                        if (m_Fields is null)
                            m_Fields = new CDBEntityFields(this);
                        return m_Fields;
                    }
                }
            }

            public IDataAdapter CreateAdapter()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append("SELECT * FROM [");
                ret.Append(Name);
                ret.Append("]");
                return Connection.CreateAdapter(ret.ToString());
            }

            public string Description
            {
                get
                {
                    return m_Description;
                }

                set
                {
                    m_Description = value;
                }
            }

            public string Schema
            {
                get
                {
                    return m_Schema;
                }

                set
                {
                    m_Schema = DMD.Strings.Trim(value);
                }
            }

            public string Guid
            {
                get
                {
                    return m_Guid;
                }

                set
                {
                    m_Guid = DMD.Strings.Trim(value);
                }
            }

            public string PropID
            {
                get
                {
                    return m_PropID;
                }

                set
                {
                    m_PropID = DMD.Strings.Trim(value);
                }
            }

            public string Type
            {
                get
                {
                    return m_Type;
                }

                set
                {
                    m_Type = DMD.Strings.Trim(value);
                }
            }

            public string Catalog
            {
                get
                {
                    return m_Catalog;
                }

                set
                {
                    m_Catalog = DMD.Strings.Trim(value);
                }
            }

            public DateTime? DateCreated
            {
                get
                {
                    return m_DateCreated;
                }

                set
                {
                    m_DateCreated = value;
                }
            }

            public DateTime? DateModified
            {
                get
                {
                    return m_DateModified;
                }

                set
                {
                    m_DateModified = value;
                }
            }

            public bool IsHidden
            {
                get
                {
                    return m_IsHidden;
                }

                set
                {
                    m_IsHidden = value;
                }
            }

            public CDBConstraintsCollection Constraints
            {
                get
                {
                    if (m_Constraints is null)
                        m_Constraints = new CDBConstraintsCollection(this);
                    return m_Constraints;
                }
            }

            protected override void OnConnectionClosed(object sender, EventArgs e)
            {
                // Me.Dispose()
            }

            public override bool IsChanged()
            {
                if (base.IsChanged())
                    return true;
                if (m_Fields is null)
                    return false;
                foreach (CDBEntityField field in m_Fields)
                {
                    if (field.IsChanged())
                        return true;
                }

                return false;
            }

            public string InternalName
            {
                get
                {
                    if (Connection is object)
                        return Connection.GetInternalTableName(this);
                    return Name;
                }
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Catalog", m_Catalog);
                writer.WriteAttribute("Schema", m_Schema);
                writer.WriteAttribute("Type", m_Type);
                writer.WriteAttribute("Guid", m_Guid);
                writer.WriteAttribute("PropID", m_PropID);
                writer.WriteAttribute("Description", m_Description);
                writer.WriteAttribute("DateCreated", m_DateCreated);
                writer.WriteAttribute("DateModified", m_DateModified);
                writer.WriteAttribute("IsHidden", m_IsHidden);
                writer.WriteAttribute("TrackChanged", m_TrackChanges);
                base.XMLSerialize(writer);
                writer.WriteTag("Fields", Fields);
                writer.WriteTag("Constraints", Constraints);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Catalog":
                        {
                            m_Catalog = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Schema":
                        {
                            m_Schema = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Type":
                        {
                            m_Type = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Guid":
                        {
                            m_Guid = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PropID":
                        {
                            m_PropID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Description":
                        {
                            m_Description = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DateCreated":
                        {
                            m_DateCreated = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DateModified":
                        {
                            m_DateModified = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IsHidden":
                        {
                            m_IsHidden = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "TrackChanged":
                        {
                            m_TrackChanges = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Fields":
                        {
                            m_Fields = (CDBEntityFields)fieldValue;
                            m_Fields.SetOwner(this);
                            break;
                        }

                    case "Constraints":
                        {
                            m_Constraints = (CDBConstraintsCollection)fieldValue;
                            m_Constraints.SetOwner(this);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}