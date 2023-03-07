using System.Collections;
using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Template utilizzabile per generare il documento di output
        /// </summary>
        [Serializable]
        public class DocumentTemplate 
            : Databases.DBObjectPO, IComparable, IComparable<DocumentTemplate>
        {
            private string m_Name;
            private string m_SourceFile;
            private string m_Description;
            private string m_PageFormatName;
            private CSize m_PageFormat;
            private TemplateItemsCollection m_TemplateItems;
            private string m_ContextType;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentTemplate()
            {
                m_Name = "";
                m_SourceFile = "";
                m_Description = "";
                m_PageFormatName = "";
                m_PageFormat = new CSize(0d, 0d);
                m_TemplateItems = null;
                m_ContextType = "";
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is DocumentTemplate) && this.Equals((DocumentTemplate)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(DocumentTemplate obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                    && DMD.Strings.EQ(this.m_SourceFile, obj.m_SourceFile)
                    && DMD.Strings.EQ(this.m_Description, obj.m_Description)
                    && DMD.Strings.EQ(this.m_PageFormatName, obj.m_PageFormatName)
                    && DMD.Strings.EQ(this.m_ContextType, obj.m_ContextType)
                    && m_PageFormat.Equals(obj.m_PageFormat)
                    && CollectionUtils.EQ(this.m_TemplateItems, obj.m_TemplateItems)
                    ;
            }

            /// <summary>
            /// Compare due oggetti template per nome
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(DocumentTemplate other)
            {
                return Strings.Compare(this.m_Name, other.m_Name, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((DocumentTemplate)obj);
            }

            /// <summary>
            /// Tipo del contesto in cui viene generato l'oggetto
            /// </summary>
            public string ContextType
            {
                get
                {
                    return m_ContextType;
                }

                set
                {
                    string oldValue = m_ContextType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContextType = value;
                    DoChanged("ContextType", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del template
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del file contenente il template
            /// </summary>
            public string SourceFile
            {
                get
                {
                    return m_SourceFile;
                }

                set
                {
                    string oldValue = m_SourceFile;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceFile = value;
                    DoChanged("SourceValue", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione del template
            /// </summary>
            public string Description
            {
                get
                {
                    return m_Description;
                }

                set
                {
                    string oldValue = m_Description;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Description = value;
                    DoChanged("Description", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del formato pagina del documento generato
            /// </summary>
            public string PageFormatName
            {
                get
                {
                    return m_PageFormatName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_PageFormatName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PageFormatName = value;
                    DoChanged("PageFormatName", value, oldValue);
                }
            }

            /// <summary>
            /// Dimesione della pagina generata dal documento
            /// </summary>
            public CSize PageFormat
            {
                get
                {
                    return m_PageFormat;
                }

                set
                {
                    var oldValue = m_PageFormat;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PageFormat = value;
                    DoChanged("PageFormat", value, oldValue);
                }
            }

            /// <summary>
            /// Elementi da generare
            /// </summary>
            public TemplateItemsCollection Items
            {
                get
                {
                    if (m_TemplateItems is null)
                        m_TemplateItems = new TemplateItemsCollection(this);
                    return m_TemplateItems;
                }
            }

            /// <summary>
            /// Restituisce il nome dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate( this.m_Name);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Templates;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_DocumentiTemplates";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name",  m_Name);
                m_ContextType = reader.Read("ContextType",  m_ContextType);
                m_Description = reader.Read("Description",  m_Description);
                m_PageFormatName = reader.Read("PageFormatName",  m_PageFormatName);
                m_SourceFile = reader.Read("SourceFile",  m_SourceFile);
                float w = reader.Read("PageFormatWidth", 0f);
                float h = reader.Read("PageFormatHeight", 0f);
                m_PageFormat = new CSize(w, h);
                string txt = reader.Read("TemplateItems", "");
                if (!string.IsNullOrEmpty(txt))
                {
                    m_TemplateItems = (TemplateItemsCollection)DMD.XML.Utils.Serializer.Deserialize(txt);
                    m_TemplateItems.SetDocument(this);
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
                writer.Write("Name", m_Name);
                writer.Write("Description", m_Description);
                writer.Write("ContextType", m_ContextType);
                writer.Write("PageFormatName", m_PageFormatName);
                writer.Write("PageFormatWidth", m_PageFormat.Width);
                writer.Write("PageFormatHeight", m_PageFormat.Height);
                writer.Write("SourceFile", m_SourceFile);
                writer.Write("TemplateItems", DMD.XML.Utils.Serializer.Serialize(Items));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Name", typeof(string), 255);
                c = table.Fields.Ensure("Description", typeof(string), 0);
                c = table.Fields.Ensure("ContextType", typeof(string), 255);
                c = table.Fields.Ensure("PageFormatName", typeof(string), 255);
                c = table.Fields.Ensure("PageFormatWidth", typeof(double), 255);
                c = table.Fields.Ensure("PageFormatHeight", typeof(double), 255);
                c = table.Fields.Ensure("SourceFile", typeof(string), 255);
                c = table.Fields.Ensure("TemplateItems", typeof(string), 0); 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Name", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxContext", new string[] { "ContextType", "Description" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPage", new string[] { "PageFormatName", "PageFormatWidth", "PageFormatHeight" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFile", new string[] { "SourceFile"  }, DBFieldConstraintFlags.None);
                //TemplateItems
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("Description", m_Description);
                writer.WriteAttribute("ContextType", m_ContextType);
                writer.WriteAttribute("PageFormatName", m_PageFormatName);
                writer.WriteAttribute("SourceFile", m_SourceFile);
                base.XMLSerialize(writer);
                writer.WriteTag("PageFormat", PageFormat);
                writer.WriteTag("TemplateItems", Items);
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
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceFile":
                        {
                            m_SourceFile = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Description":
                        {
                            m_Description = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContextType":
                        {
                            m_ContextType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PageFormatName":
                        {
                            m_PageFormatName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PageFormat":
                        {
                            m_PageFormat = (CSize)fieldValue;
                            break;
                        }

                    case "TemplateItems":
                        {
                            m_TemplateItems = new TemplateItemsCollection(this);
                            foreach (TemplateItem item in (IEnumerable)fieldValue)
                                m_TemplateItems.Add(item);
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
            /// Genera un PDF
            /// </summary>
            /// <param name="context"></param>
            /// <param name="fileName"></param>
            public void RenderToPDFFile(object context, string fileName)
            {
                //TODO RenderToPDFFile
                throw new NotImplementedException();
            }

        }
    }
}