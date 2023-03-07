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
        /// Setting
        /// </summary>
        [Serializable]
        public class CSettingRecord
            : Databases.DBObjectBase, ISetting
        {
            private int m_OwnerID; // ID della persona associata
            private string m_OwnerType;
            [NonSerialized] private object m_Owner;
            private string m_Nome;
            private object m_Valore;
            private TypeCode m_TipoValore;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSettingRecord()
            {
                m_OwnerID = 0;
                m_OwnerType = "";
                m_Owner = null;
                m_Nome = "";
                m_Valore = DBNull.Value;
                m_TipoValore = TypeCode.Empty;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public CSettingRecord(string key, object value)
            {
                m_Nome = key;
                m_Valore = value;
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CSettingRecord) && this.Equals((CSettingRecord)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CSettingRecord obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_OwnerID, obj.m_OwnerID)
                    && DMD.Strings.EQ(this.m_OwnerType, obj.m_OwnerType)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.RunTime.EQ(this.m_Valore, obj.m_Valore)
                    && DMD.Integers.EQ((int)this.m_TipoValore, (int)obj.m_TipoValore)
                    ;

            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Settings;
            }

            /// <summary>
            /// Restituisce l'id dell'oggetto a cui appartiene il setting
            /// </summary>
            public int OwnerID
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Owner, m_OwnerID);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto a cui appartiene il setting
            /// </summary>
            public object Owner
            {
                get
                {
                    if (m_Owner is null && !string.IsNullOrEmpty(m_OwnerType))
                    {
                        m_Owner = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_OwnerType, m_OwnerID);
                    }

                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta l'owner del setting
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetOwner(object value)
            {
                m_Owner = value;
                m_OwnerID = DBUtils.GetID(value, 0);
                m_OwnerType = DMD.RunTime.vbTypeName(value);
            }

            /// <summary>
            /// Restituisce o imposta il nome del setting
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }
             
            /// <summary>
            /// Restituisce o imposta il valore del setting
            /// </summary>
            public object Valore
            {
                get
                {
                    return m_Valore;
                }

                set
                {
                    var oldValue = m_Valore;
                    if (DMD.RunTime.EQ(oldValue, value))
                        return;
                    m_Valore = value;
                    m_TipoValore = DMD.RunTime.GetTypeCode(value);
                    DoChanged("Valore", value, oldValue);
                }
            }

            ISettingsOwner ISetting.Owner
            {
                get
                {
                    return ((ISettingsOwner)this.Owner);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Settings";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_TipoValore = reader.Read("TipoValore", this.m_TipoValore);
                string strValue = reader.Read("Valore", DMD.Strings.vbNullString);
                this.m_Valore = DMD.RunTime.CastTo(strValue, this.m_TipoValore);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("OwnerID", OwnerID);
                writer.Write("OwnerType", m_OwnerType);
                writer.Write("Nome", m_Nome);
                writer.Write("Valore", Formats.ToString(m_Valore));
                writer.Write("TipoValore", m_TipoValore);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("OwnerID", typeof(int), 1);
                c = table.Fields.Ensure("OwnerType", typeof(string), 255);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Valore", typeof(string), 0);
                c = table.Fields.Ensure("TipoValore", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxStato", new string[] { "OwnerID", "OwnerType", "Nome" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxValore", new string[] { "TipoValore", "Valore"  }, DBFieldConstraintFlags.None);


            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome; // & " = " & m_Valore
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OwnerID", OwnerID);
                writer.WriteAttribute("OwnerType", m_OwnerType);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("TipoValore", (int?)m_TipoValore);
                switch (m_TipoValore)
                {
                    case TypeCode.Boolean:
                        {
                            writer.WriteAttribute("Valore", Formats.ToBool(m_Valore));
                            break;
                        }

                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        {
                            writer.WriteAttribute("Valore", Formats.ToInteger(m_Valore));
                            break;
                        }

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        {
                            writer.WriteAttribute("Valore", Formats.ToLong(m_Valore));
                            break;
                        }

                    case TypeCode.DateTime:
                        {
                            writer.WriteAttribute("Valore", Formats.ToDate(m_Valore));
                            break;
                        }

                    case TypeCode.Decimal:
                        {
                            writer.WriteAttribute("Valore", Formats.ToValuta(m_Valore));
                            break;
                        }

                    case TypeCode.Single:
                    case TypeCode.Double:
                        {
                            writer.WriteAttribute("Valore", Formats.ToDouble(m_Valore));
                            break;
                        }

                    case TypeCode.Char:
                        {
                            writer.WriteAttribute("Valore", DMD.Strings.CStr(m_Valore));
                            break;
                        }

                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                        {
                            break;
                        }
                    // NULL
                    // TAG
                    case TypeCode.String:
                    case TypeCode.Object:
                        {
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                base.XMLSerialize(writer);
                switch (m_TipoValore)
                {
                    case TypeCode.String:
                        {
                            writer.WriteTag("Valore", DMD.Strings.CStr(m_Valore));
                            break;
                        }

                    case TypeCode.Object:
                        {
                            writer.WriteTag("Valore", DMD.Strings.CStr(m_Valore));
                            break;
                        }

                    default:
                        {
                            break;
                        }
                        // OTHERS
                }
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
                    case "OwnerID":
                        {
                            m_OwnerID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OwnerType":
                        {
                            m_OwnerType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            // If (Me.m_TipoValore = TypeCode.Empty) Then
                            // Me.m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                            // Else
                            // Me.m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                            // Me.m_Valore = Types.CastTo(Me.m_Valore, Me.m_TipoValore)
                            // End If
                            switch (m_TipoValore)
                            {
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Single:
                                    {
                                        m_Valore = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                                        break;
                                    }

                                case TypeCode.Boolean:
                                    {
                                        m_Valore = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue);
                                        break;
                                    }

                                case TypeCode.Byte:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.SByte:
                                case TypeCode.UInt16:
                                case TypeCode.UInt32:
                                case TypeCode.UInt64:
                                    {
                                        m_Valore = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                                        break;
                                    }

                                case TypeCode.Char:
                                case TypeCode.String:
                                    {
                                        m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "TipoValore":
                        {
                            m_TipoValore = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            if (m_Valore is object)
                            {
                                m_Valore = DMD.RunTime.CastTo(m_Valore, m_TipoValore);
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
            /// OnAfterDelete
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterDelete(DMDEventArgs e)
            {
                base.OnAfterDelete(e);
                if (this.Owner is object)
                    ((ISettingsOwner)this.Owner).NotifySettingsChanged(new SettingsChangedEventArgs(this, SettingChangedEnum.Removed));
            }

            /// <summary>
            /// OnAfterCreate
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterCreate(DMDEventArgs e)
            {
                base.OnAfterCreate(e);
                if (this.Owner is object)
                    ((ISettingsOwner)this.Owner).NotifySettingsChanged(new SettingsChangedEventArgs(this,  SettingChangedEnum.Added));
            }

            /// <summary>
            /// OnAfterChange
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterChange(DMDEventArgs e)
            {
                base.OnAfterChange(e);
                if (this.Owner is object)
                    ((ISettingsOwner)this.Owner).NotifySettingsChanged(new SettingsChangedEventArgs(this, SettingChangedEnum.Changed));

            }

            void ISetting.SetOwner(ISettingsOwner value)
            {
                this.SetOwner(value);
            }

            void ISetting.Delete()
            {
                this.Delete();
            }
        }
    }
}