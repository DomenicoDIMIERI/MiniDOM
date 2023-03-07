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
        /// Descrive un attributo di un Categoria
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class AttributoCategoria
            : minidom.Databases.DBObject, IComparable, IComparable<AttributoCategoria>
        {
            private int m_IDCategoria;
            [NonSerialized] private CategoriaArticolo m_Categoria;
            private string m_NomeAttributo;
            private string m_ValoreAttributo;
            private string m_UnitaDiMisura;
            private TypeCode m_TipoAttributo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributoCategoria()
            {
                m_IDCategoria = 0;
                m_Categoria = null;
                m_NomeAttributo = "";
                m_ValoreAttributo = "";
                m_UnitaDiMisura = "";
                m_TipoAttributo = TypeCode.String;
            }

            /// <summary>
            /// Restituisce o imposta l'Categoria a cui è associato il codice
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CategoriaArticolo Categoria
            {
                get
                {
                    lock (this)
                    {
                        if (m_Categoria is null)
                            m_Categoria = CategorieArticoli.GetItemById(m_IDCategoria);
                        return m_Categoria;
                    }
                }

                set
                {
                    CategoriaArticolo oldValue;
                    lock (this)
                    {
                        oldValue = Categoria;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Categoria = value;
                        m_IDCategoria = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della categoria a cui appartiene l'attributo
            /// </summary>
            public int IDCategoria
            {
                get
                {
                    return DBUtils.GetID(m_Categoria, m_IDCategoria);
                }

                set
                {
                    int oldValue = IDCategoria;
                    if (oldValue == value)
                        return;
                    m_IDCategoria = 0;
                    m_Categoria = null;
                    DoChanged("IDCategoria", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la categoria
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetCategoria(CategoriaArticolo value)
            {
                m_Categoria = value;
                m_IDCategoria = DBUtils.GetID(value, 0);
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
                                    m_ValoreAttributo = (DMD.Booleans.ValueOf(value))? "T" : "F";
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
                writer.WriteAttribute("IDCategoria", IDCategoria);
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
                    case "IDCategoria":
                        {
                            m_IDCategoria = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return "tbl_OfficeCategorieAttributi";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.CategorieArticoli.Attributi;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCategoria = reader.Read("IDCategoria", m_IDCategoria);
                m_NomeAttributo = reader.Read("NomeAttributo", m_NomeAttributo);
                m_ValoreAttributo = reader.Read("ValoreAttributo", m_ValoreAttributo);
                m_TipoAttributo = reader.Read("TipoAttributo", m_TipoAttributo);
                m_UnitaDiMisura = reader.Read("UnitaDiMisura",  m_UnitaDiMisura);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCategoria", IDCategoria);
                writer.Write("NomeAttributo", m_NomeAttributo);
                writer.Write("ValoreAttributo", m_ValoreAttributo);
                writer.Write("TipoAttributo", m_TipoAttributo);
                writer.Write("UnitaDiMisura", m_UnitaDiMisura);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("IDCategoria", typeof(int), 1);
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
                var c = table.Constraints.Ensure("idxCategoria", new string[] { "IDCategoria", "NomeAttributo", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxTipo", new string[] { "TipoAttributo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValoreAttributo", new string[] { "ValoreAttributo", "UnitaDiMisura" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(AttributoCategoria other)
            {
                int ret = DMD.Strings.Compare(m_NomeAttributo, other.m_NomeAttributo, true);
                if (ret == 0) ret = DMD.Arrays.Compare(ValoreAttributo, other.ValoreAttributo);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((AttributoCategoria)obj);
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
                return (obj is AttributoCategoria) && this.Equals((AttributoCategoria)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(AttributoCategoria obj)
            {
                return base.Equals(obj)
                    &&  DMD.Integers.EQ(this.m_IDCategoria, obj.m_IDCategoria)
                    && DMD.Strings.EQ(this.m_NomeAttributo, obj.m_NomeAttributo)
                    && DMD.Strings.EQ(this.m_ValoreAttributo, obj.m_ValoreAttributo)
                    && DMD.Strings.EQ(this.m_UnitaDiMisura, obj.m_UnitaDiMisura)
                    && DMD.Integers.EQ((int)this.m_TipoAttributo, (int)obj.m_TipoAttributo)
                    ;
            }

        }
    }
}