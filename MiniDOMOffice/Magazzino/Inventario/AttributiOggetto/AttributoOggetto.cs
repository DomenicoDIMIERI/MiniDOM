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
using static minidom.Store;


namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Specifica il valore di un attributo dell'Oggetto per l'oggetto inventariato
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class AttributoOggetto 
            : minidom.Databases.DBObject, IComparable, IComparable<AttributoOggetto>
        {
            private int m_IDOggetto;
            [NonSerialized] private OggettoInventariato m_Oggetto;
            private string m_NomeAttributo;
            private string m_ValoreAttributo;
            private string m_UnitaDiMisura;
            private TypeCode m_TipoAttributo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributoOggetto()
            {
                m_IDOggetto = 0;
                m_Oggetto = null;
                m_NomeAttributo = "";
                m_ValoreAttributo = "";
                m_UnitaDiMisura = "";
                m_TipoAttributo = TypeCode.String;
            }

            /// <summary>
            /// Restituisce o imposta l'Oggetto a cui è associato il codice
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public OggettoInventariato Oggetto
            {
                get
                {
                    lock (this)
                    {
                        if (m_Oggetto is null)
                            m_Oggetto = OggettiInventariati.GetItemById(m_IDOggetto);
                        return m_Oggetto;
                    }
                }

                set
                {
                    OggettoInventariato oldValue;
                    lock (this)
                    {
                        oldValue = m_Oggetto;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Oggetto = value;
                        m_IDOggetto = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Oggetto", value, oldValue);
                }
            }

            /// <summary>
            /// ID oggetto inventariato
            /// </summary>
            public int IDOggetto
            {
                get
                {
                    return DBUtils.GetID(m_Oggetto, m_IDOggetto);
                }

                set
                {
                    int oldValue = IDOggetto;
                    if (oldValue == value)
                        return;
                    m_IDOggetto = 0;
                    m_Oggetto = null;
                    DoChanged("IDOggetto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'oggetto inventariato
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetOggetto(OggettoInventariato value)
            {
                m_Oggetto = value;
                m_IDOggetto = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'attributo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAttributo
            {
                get
                {
                    return m_NomeAttributo;
                }

                set
                {
                    string oldValue = m_NomeAttributo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAttributo = value;
                    DoChanged("NomeAttributo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore formattato dell'attributo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ValoreAttributoFormattato
            {
                get
                {
                    return m_ValoreAttributo;
                }

                set
                {
                    string oldValue = m_ValoreAttributo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ValoreAttributo = value;
                    DoChanged("ValoreAttributoFormattato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore dell'attributo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object ValoreAttributo
            {
                get
                {
                    switch (m_TipoAttributo)
                    {
                        case TypeCode.Boolean:
                            {
                                return Sistema.Formats.ParseBool(m_ValoreAttributo);
                            }

                        case TypeCode.Byte:
                            {
                                return Sistema.Formats.ParseByte(m_ValoreAttributo);
                            }

                        case TypeCode.Char:
                            {
                                return Sistema.Formats.ParseChar(m_ValoreAttributo);
                            }

                        case TypeCode.DateTime:
                            {
                                return DMD.DateUtils.ParseUSADate(m_ValoreAttributo);
                            }

                        case TypeCode.DBNull:
                            {
                                return DBNull.Value;
                            }

                        case TypeCode.Decimal:
                            {
                                return Sistema.Formats.ParseDecimal(m_ValoreAttributo);
                            }

                        case TypeCode.Double:
                            {
                                return Sistema.Formats.ParseDouble(m_ValoreAttributo);
                            }

                        case TypeCode.Empty:
                            {
                                return null;
                            }

                        case TypeCode.Int16:
                            {
                                return Sistema.Formats.ParseShort(m_ValoreAttributo);
                            }

                        case TypeCode.Int32:
                            {
                                return Sistema.Formats.ParseInteger(m_ValoreAttributo);
                            }

                        case TypeCode.Int64:
                            {
                                return Sistema.Formats.ParseLong(m_ValoreAttributo);
                            }

                        case TypeCode.Object:
                            {
                                return m_ValoreAttributo;
                            }

                        case TypeCode.SByte:
                            {
                                return Sistema.Formats.ParseSByte(m_ValoreAttributo);
                            }

                        case TypeCode.Single:
                            {
                                return Sistema.Formats.ParseSingle(m_ValoreAttributo);
                            }

                        case TypeCode.String:
                            {
                                return m_ValoreAttributo;
                            }

                        case TypeCode.UInt16:
                            {
                                return Sistema.Formats.ParseUShort(m_ValoreAttributo);
                            }

                        case TypeCode.UInt32:
                            {
                                return Sistema.Formats.ParseUInteger(m_ValoreAttributo);
                            }

                        case TypeCode.UInt64:
                            {
                                return Sistema.Formats.ParseULong(m_ValoreAttributo);
                            }

                        default:
                            {
                                return m_ValoreAttributo;
                            }
                    }
                }

                set
                {
                    var oldValue = ValoreAttributo;
                    if (DMD.RunTime.EQ(value, oldValue))
                        return;
                    if (DMD.RunTime.IsNull(value))
                    {
                        m_ValoreAttributo = "";
                    }
                    else
                    {
                        switch (m_TipoAttributo)
                        {
                            case TypeCode.Boolean:
                                {
                                    m_ValoreAttributo = (DMD.Booleans.ValueOf(value)? "T" : "F";
                                    break;
                                }

                            case TypeCode.Byte:
                                {
                                    m_ValoreAttributo = Sistema.Formats.FormatInteger(value);
                                    break;
                                }

                            case TypeCode.Char:
                                {
                                    m_ValoreAttributo = DMD.Strings.CStr(value);
                                    break;
                                }

                            case TypeCode.DateTime:
                                {
                                    m_ValoreAttributo = DMD.DateUtils.FormatUSADate(value);
                                    break;
                                }

                            case TypeCode.DBNull:
                                {
                                    m_ValoreAttributo = "";
                                    break;
                                }

                            case TypeCode.Decimal:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.Double:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.Empty:
                                {
                                    m_ValoreAttributo = "";
                                    break;
                                }

                            case TypeCode.Int16:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.Int32:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.Int64:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.Object:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.SByte:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.Single:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.String:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.UInt16:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.UInt32:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            case TypeCode.UInt64:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }

                            default:
                                {
                                    m_ValoreAttributo = value.ToString();
                                    break;
                                }
                        }
                    }

                    DoChanged("ValoreAttributo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce un valore che indica il tipo dell'attributo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TypeCode TipoAttributo
            {
                get
                {
                    return m_TipoAttributo;
                }

                set
                {
                    var oldValue = m_TipoAttributo;
                    if (oldValue == value)
                        return;
                    m_TipoAttributo = value;
                    DoChanged("TipoAttributo ", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'unità di misura dell'attributo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UnitaDiMisura
            {
                get
                {
                    return m_UnitaDiMisura;
                }

                set
                {
                    string oldValue = m_UnitaDiMisura;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UnitaDiMisura = value;
                    DoChanged("UnitaDiMisura", value, oldValue);
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOggetto", IDOggetto);
                writer.WriteAttribute("NomeAttributo", m_NomeAttributo);
                writer.WriteAttribute("ValoreAttributo", m_ValoreAttributo);
                writer.WriteAttribute("TipoAttributo", (int?)m_TipoAttributo);
                writer.WriteAttribute("UnitaDiMisura", m_UnitaDiMisura);
                base.XMLSerialize(writer);
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
                    case "IDOggetto":
                        {
                            m_IDOggetto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAttributo":
                        {
                            m_NomeAttributo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreAttributo":
                        {
                            m_ValoreAttributo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoAttributo":
                        {
                            m_TipoAttributo = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UnitaDiMisura":
                        {
                            m_UnitaDiMisura = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeOggettiInventariatiAttr";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.OggettiInventariati.Attributi;
            }

            /// <summary>
            /// Carica dal database
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDOggetto = reader.Read("IDOggetto", m_IDOggetto);
                m_NomeAttributo = reader.Read("NomeAttributo", m_NomeAttributo);
                m_ValoreAttributo = reader.Read("ValoreAttributo", m_ValoreAttributo);
                m_TipoAttributo = reader.Read("TipoAttributo", m_TipoAttributo);
                m_UnitaDiMisura = reader.Read("UnitaDiMisura", m_UnitaDiMisura);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel database
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDOggetto", IDOggetto);
                writer.Write("NomeAttributo", m_NomeAttributo);
                writer.Write("ValoreAttributo", m_ValoreAttributo);
                writer.Write("TipoAttributo", m_TipoAttributo);
                writer.Write("UnitaDiMisura", m_UnitaDiMisura);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDOggetto", typeof(int), 1);
                c = table.Fields.Ensure("NomeAttributo", typeof(string), 255);
                c = table.Fields.Ensure("ValoreAttributo", typeof(string), 0);
                c = table.Fields.Ensure("TipoAttributo", typeof(int), 1);
                c = table.Fields.Ensure("UnitaDiMisura", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOggetto", new string[] { "IDOggetto", "NomeAttributo", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxValore", new string[] { "TipoAttributo", "UnitaDiMisura", "ValoreAttributo" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(AttributoOggetto other)
            {
                int ret = DMD.Strings.Compare(m_NomeAttributo, other.m_NomeAttributo, true);
                if (ret == 0) ret = DMD.RunTime.Compare(ValoreAttributo, other.ValoreAttributo);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((AttributoOggetto)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", m_NomeAttributo , " = " , ValoreAttributoFormattato, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeAttributo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is AttributoOggetto) && this.Equals((AttributoOggetto)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(AttributoOggetto obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.m_IDOggetto, obj.m_IDOggetto)
                     && DMD.Strings.EQ(this.m_NomeAttributo, obj.m_NomeAttributo)
                     && DMD.Strings.EQ(this.m_ValoreAttributo, obj.m_ValoreAttributo)
                     && DMD.Strings.EQ(this.m_UnitaDiMisura, obj.m_UnitaDiMisura)
                     && DMD.Integers.EQ((int)this.m_TipoAttributo, (int)obj.m_TipoAttributo)
                    ;
            }

            
        }
    }
}