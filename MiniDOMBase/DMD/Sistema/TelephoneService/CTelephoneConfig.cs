using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Parametri di configurazione di un driver per una linea telefonica
        /// </summary>
        [Serializable]
        public class CTelephoneConfig
            : DMD.XML.DMDBaseXMLObject
        {
            private string m_DefaultSenderName;
            private string m_DefaultSenderNumber;
            private string m_DefaultDriverName;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelephoneConfig()
            {
                m_DefaultDriverName = "";
                m_DefaultSenderName = "";
                m_DefaultSenderNumber = "";
            }

            /// <summary>
            /// Restituisce o imposta il nome del driver predefinito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DefaultDriverName
            {
                get
                {
                    return m_DefaultDriverName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DefaultDriverName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DefaultDriverName = value;
                    DoChanged("DefaultDriverName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mittente predefinito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DefaultSenderName
            {
                get
                {
                    return m_DefaultSenderName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DefaultSenderName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DefaultSenderName = value;
                    DoChanged("DefaultSenderName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero del mittente predefinito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DefaultSenderNumbmer
            {
                get
                {
                    return m_DefaultSenderNumber;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DefaultSenderNumber;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DefaultSenderNumber = value;
                    DoChanged("DefaultSenderNumber", value, oldValue);
                }
            }

            

            ///// <summary>
            ///// Carica dal db
            ///// </summary>
            ///// <param name="reader"></param>
            ///// <returns></returns>
            //protected override bool LoadFromRecordset(DBReader reader)
            //{
            //    this.m_DefaultDriverName = reader.Read("DefaultDriverName", this. m_DefaultDriverName);
            //    this.m_DefaultSenderName = reader.Read("DefaultSenderName", this.m_DefaultSenderName);
            //    this.m_DefaultSenderNumber = reader.Read("DefaultSenderNumber", this.m_DefaultSenderNumber);
            //    return base.LoadFromRecordset(reader);
            //}

            ///// <summary>
            ///// Salva nel db
            ///// </summary>
            ///// <param name="writer"></param>
            ///// <returns></returns>
            //protected override bool SaveToRecordset(DBWriter writer)
            //{
            //    writer.Write("DefaultDriverName", m_DefaultDriverName);
            //    writer.Write("DefaultSenderName", m_DefaultSenderName);
            //    writer.Write("DefaultSenderNumber", m_DefaultSenderNumber);
            //    return base.SaveToRecordset(writer);
            //}

            ///// <summary>
            ///// Prepara i campi
            ///// </summary>
            ///// <param name="table"></param>
            //protected override void PrepareDBSchemaFields(DBTable table)
            //{
            //    base.PrepareDBSchemaFields(table);

            //    var c = table.Fields.Ensure("DefaultDriverName", typeof(string), 255);
            //    c = table.Fields.Ensure("DefaultSenderName", typeof(string), 255);
            //    c = table.Fields.Ensure("DefaultSenderNumber", typeof(string), 255);
            //}

            ///// <summary>
            ///// Prepara i vincoli
            ///// </summary>
            ///// <param name="table"></param>
            //protected override void PrepareDBSchemaConstraints(DBTable table)
            //{
            //    base.PrepareDBSchemaConstraints(table);

            //    var c = table.Constraints.Ensure("idxParams", new string[] { "DefaultDriverName", "DefaultSenderName", "DefaultSenderNumber" }, DBFieldConstraintFlags.None);
            //}


            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "DefaultDriverName":
                        {
                            m_DefaultDriverName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DefaultSenderName":
                        {
                            m_DefaultSenderName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DefaultSenderNumber":
                        {
                            m_DefaultSenderNumber = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DefaultDriverName", m_DefaultDriverName);
                writer.WriteAttribute("DefaultSenderName", m_DefaultSenderName);
                writer.WriteAttribute("DefaultSenderNumber", m_DefaultSenderNumber);
                base.XMLSerialize(writer);
            }


            //public void Load()
            //{
            //    Databases.DBReader reader;
            //    string dbSQL = "SELECT * FROM [" + GetTableName() + "] ORDER BY [ID] ASC";
            //    reader = new Databases.DBReader(Databases.APPConn.Tables[GetTableName()], dbSQL);
            //    if (reader.Read())
            //    {
            //        Databases.APPConn.Load(this, reader);
            //    }

            //    reader.Dispose();
            //}

            // Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
            // MyBase.DoChanged(propName, newVal, oldVal)
            // FaxService.doConfigChanged(New System.EventArgs)
            // End Sub

            //protected override void OnAfterSave(SystemEvent e)
            //{
            //    base.OnAfterSave(e);
            //    TelephoneService.SetConfig(this);
            //}

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", this.m_DefaultDriverName, ", ", this.m_DefaultSenderName, " : ", this.m_DefaultSenderNumber, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DefaultDriverName);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return (obj is CTelephoneConfig) && this.Equals((CTelephoneConfig)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CTelephoneConfig obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_DefaultSenderName, obj.m_DefaultSenderName)
                    && DMD.Strings.EQ(this.m_DefaultSenderNumber, obj.m_DefaultSenderNumber)
                    && DMD.Strings.EQ(this.m_DefaultDriverName, obj.m_DefaultDriverName)
                    ;
            }
        }
    }
}