using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {
        /// <summary>
        /// Tipo di vincolo usato per il blocco
        /// </summary>
        public enum BlackListType : int
        {
            /// <summary>
            /// Blocca solo gli indirizzi uguali al parametro
            /// </summary>
            UGUALEA = 0,

            /// <summary>
            /// Blocca tutti gli indirizzi che iniziano con il parametro
            /// </summary>
            COMINCIACON = 1,

            /// <summary>
            /// Blocca tutti gli indirizzi che terminano con il parametro
            /// </summary>
            FINISCECON = 2,

            /// <summary>
            /// Blocca tutti gli indirizzi che contengono il parametro
            /// </summary>
            CONTIENE = 3,

            /// <summary>
            /// Blocca tutti gli indirizzi diversi dal parametro
            /// </summary>
            DIVERSODA = 4,

            /// <summary>
            /// Blocca tutti gli indirizzi che non iniziano con il parametro
            /// </summary>
            NONCOMINCIACON = 5,

            /// <summary>
            /// Blocca tutti gli indirizzi che non finiscono con il parametro
            /// </summary>
            NONFINISCECON = 6
        }


        /// <summary>
        /// Indirizzo bloccato
        /// </summary>
        [Serializable()]
        public class BlackListAddress
            : Databases.DBObjectBase, IComparable, IComparable<BlackListAddress>
        {
            private BlackListType m_TipoRegola;
            private string m_TipoContatto;
            private string m_ValoreContatto;
            private DateTime m_DataBlocco;
            private int m_IDBloccatoDa;
            [NonSerialized] private Sistema.CUser m_BloccatoDa;
            private string m_NomeBloccatoDa;
            private string m_MotivoBlocco;

            /// <summary>
            /// Costruttore
            /// </summary>
            public BlackListAddress()
            {
                this.m_TipoRegola = BlackListType.UGUALEA;
                this.m_TipoContatto = "";
                this.m_ValoreContatto = "";
                this.m_DataBlocco = default;
                this.m_IDBloccatoDa = 0;
                this.m_BloccatoDa = null;
                this.m_NomeBloccatoDa = "";
                this.m_MotivoBlocco = "";
            }

            /// <summary>
            /// Tipo di vincolo
            /// </summary>
            public BlackListType TipoRegola
            {
                get
                {
                    return m_TipoRegola;
                }

                set
                {
                    var oldValue = m_TipoRegola;
                    if (oldValue == value)
                        return;
                    m_TipoRegola = value;
                    DoChanged("TipoRegola", value, oldValue);
                }
            }

            /// <summary>
            /// Tipo contatto
            /// </summary>
            public string TipoContatto
            {
                get
                {
                    return m_TipoContatto;
                }

                set
                {
                    string oldValue = m_TipoContatto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContatto = value;
                    DoChanged("TipoContatto", value, oldValue);
                }
            }

            /// <summary>
            /// Valore del vincolo
            /// </summary>
            public string ValoreContatto
            {
                get
                {
                    return m_ValoreContatto;
                }

                set
                {
                    string oldValue = m_ValoreContatto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ValoreContatto = value;
                    DoChanged("ValoreContatto", value, oldValue);
                }
            }

            /// <summary>
            /// Data del blocco
            /// </summary>
            public DateTime DataBlocco
            {
                get
                {
                    return m_DataBlocco;
                }

                set
                {
                    var oldValue = m_DataBlocco;
                    if (oldValue == value)
                        return;
                    m_DataBlocco = value;
                    DoChanged("DataBlocco", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente che ha bloccato l'indirizzo
            /// </summary>
            public int IDBloccatoDa
            {
                get
                {
                    return DBUtils.GetID(m_BloccatoDa, m_IDBloccatoDa);
                }

                set
                {
                    int oldValue = IDBloccatoDa;
                    if (oldValue == value)
                        return;
                    m_IDBloccatoDa = value;
                    m_BloccatoDa = null;
                    DoChanged("IDBloccatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Utente che ha bloccato l'indirizzo
            /// </summary>
            public Sistema.CUser BloccatoDa
            {
                get
                {
                    if (m_BloccatoDa is null)
                        m_BloccatoDa = Sistema.Users.GetItemById(m_IDBloccatoDa);
                    return m_BloccatoDa;
                }

                set
                {
                    var oldValue = m_BloccatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_BloccatoDa = value;
                    m_IDBloccatoDa = DBUtils.GetID(value, 0);
                    m_NomeBloccatoDa = (value is object)? value.Nominativo : string.Empty;
                    DoChanged("BloccatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'utente che ha bloccato l'indirizzo
            /// </summary>
            public string NomeBloccatoDa
            {
                get
                {
                    return m_NomeBloccatoDa;
                }

                set
                {
                    string oldValue = m_NomeBloccatoDa;
                    value = DMD.Strings.Trim(value);
                    if (DMD.Strings.Compare(value, oldValue, true) == 0)
                        return;
                    m_NomeBloccatoDa = value;
                    DoChanged("NomeBloccatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Stringa che descrive il motivo del blocco
            /// </summary>
            public string MotivoBlocco
            {
                get
                {
                    return m_MotivoBlocco;
                }

                set
                {
                    string oldValue = m_MotivoBlocco;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_MotivoBlocco = value;
                    DoChanged("MotivoBlocco", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se l'indirizzo é bloccato
            /// </summary>
            /// <param name="indirizzo"></param>
            /// <returns></returns>
            public bool IsNegated(string indirizzo)
            {
                indirizzo = DMD.Strings.Trim(indirizzo);
                switch (TipoRegola)
                {
                    case BlackListType.COMINCIACON:
                        {
                            return DMD.Strings.Compare(Strings.Left(indirizzo, Strings.Len(m_ValoreContatto)), m_ValoreContatto, true) == 0;
                        }

                    case BlackListType.CONTIENE:
                        {
                            return Strings.InStr(indirizzo, m_ValoreContatto, true) > 0;
                        }

                    case BlackListType.DIVERSODA:
                        {
                            return DMD.Strings.Compare(indirizzo, m_ValoreContatto, true) != 0;
                        }

                    case BlackListType.FINISCECON:
                        {
                            return DMD.Strings.Compare(Strings.Right(indirizzo, Strings.Len(m_ValoreContatto)), m_ValoreContatto, true) == 0;
                        }

                    case BlackListType.NONCOMINCIACON:
                        {
                            return DMD.Strings.Compare(Strings.Left(indirizzo, Strings.Len(m_ValoreContatto)), m_ValoreContatto, true) != 0;
                        }

                    case BlackListType.NONFINISCECON:
                        {
                            return DMD.Strings.Compare(
                                    Strings.Right(indirizzo, Strings.Len(m_ValoreContatto)), 
                                    m_ValoreContatto, true) != 0;
                        }

                    case BlackListType.UGUALEA:
                        {
                            return DMD.Strings.Compare(indirizzo, m_ValoreContatto, true) == 0;
                        }

                    default:
                        {
                            // oops
                            throw new NotImplementedException();
                        }
                }
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((BlackListAddress)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(BlackListAddress obj)
            {
                int ret = DMD.Strings.Compare(m_TipoContatto, obj.m_TipoContatto, true);
                if (ret == 0) ret = DMD.Strings.Compare(m_ValoreContatto, obj.m_ValoreContatto, true);
                return ret;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("TipoRegola", (int?)m_TipoRegola);
                writer.WriteAttribute("TipoContatto", m_TipoContatto);
                writer.WriteAttribute("ValoreContatto", m_ValoreContatto);
                writer.WriteAttribute("DataBlocco", m_DataBlocco);
                writer.WriteAttribute("IDBloccatoDa", IDBloccatoDa);
                writer.WriteAttribute("NomeBloccatoDa", m_NomeBloccatoDa);
                base.XMLSerialize(writer);
                writer.WriteTag("MotivoBlocco", m_MotivoBlocco);
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
                    case "TipoRegola":
                        {
                            m_TipoRegola = (BlackListType)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TipoContatto":
                        {
                            m_TipoContatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreContatto":
                        {
                            m_ValoreContatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataBlocco":
                        {
                            m_DataBlocco = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDBloccatoDa":
                        {
                            m_IDBloccatoDa = (int)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeBloccatoDa":
                        {
                            m_NomeBloccatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoBlocco":
                        {
                            m_MotivoBlocco = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return "tbl_BlackListAddress";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_TipoRegola = reader.Read("Tipo", this.m_TipoRegola);
                this.m_TipoContatto = reader.Read("TipoContatto", this.m_TipoContatto);
                this.m_ValoreContatto = reader.Read("Valore", this.m_ValoreContatto);
                this.m_DataBlocco = reader.Read("DataBlocco", this.m_DataBlocco);
                this.m_IDBloccatoDa = reader.Read("IDBloccatoDa", this.m_IDBloccatoDa);
                this.m_NomeBloccatoDa = reader.Read("NomeBloccatoDa", this.m_NomeBloccatoDa);
                this.m_MotivoBlocco = reader.Read("MotivoBlocco", this.m_MotivoBlocco);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Tipo", m_TipoRegola);
                writer.Write("TipoContatto", m_TipoContatto);
                writer.Write("Valore", m_ValoreContatto);
                writer.Write("DataBlocco", m_DataBlocco);
                writer.Write("IDBloccatoDa", IDBloccatoDa);
                writer.Write("NomeBloccatoDa", m_NomeBloccatoDa);
                writer.Write("MotivoBlocco", m_MotivoBlocco);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Tipo", typeof(int), 1);
                c = table.Fields.Ensure("TipoContatto", typeof(string), 255);
                c = table.Fields.Ensure("Valore", typeof(string), 255);
                c = table.Fields.Ensure("DataBlocco", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDBloccatoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeBloccatoDa", typeof(string), 255);
                c = table.Fields.Ensure("MotivoBlocco", typeof(string), 255);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxTipo", new string[] { "Tipo", "TipoContatto", "Valore" },  DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxBloccatoDa", new string[] { "DataBlocco", "IDBloccatoDa", "NomeBloccatoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMotivo", new string[] { "MotivoBlocco" }, DBFieldConstraintFlags.None);

            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_TipoContatto);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is BlackListAddress) && this.Equals((BlackListAddress)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(BlackListAddress obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ( (int)this.m_TipoRegola , (int)obj.m_TipoRegola)
                    && DMD.Strings.EQ(this.m_TipoContatto, obj.m_TipoContatto)
                    && DMD.Strings.EQ(this.m_ValoreContatto, obj.m_ValoreContatto)
                    && DMD.DateUtils.EQ(this.m_DataBlocco, obj.m_DataBlocco)
                    && DMD.Integers.EQ(this.m_IDBloccatoDa, obj.m_IDBloccatoDa)
                    && DMD.Strings.EQ(this.m_NomeBloccatoDa, obj.m_NomeBloccatoDa)
                    && DMD.Strings.EQ(this.m_MotivoBlocco, obj.m_MotivoBlocco)
                    ;
            }


            /// <summary>
            /// Restituisce una sringa che rapresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                switch (TipoRegola)
                {
                    case BlackListType.COMINCIACON:
                        {
                            return "Blocca se comincia con \"" + m_ValoreContatto + "\" ";
                        }

                    case BlackListType.CONTIENE:
                        {
                            return "Blocca se contiene \"" + m_ValoreContatto + "\" ";
                        }

                    case BlackListType.DIVERSODA:
                        {
                            return "Blocca se è diverso da \"" + m_ValoreContatto + "\" ";
                        }

                    case BlackListType.FINISCECON:
                        {
                            return "Blocca se finisce con \"" + m_ValoreContatto + "\" ";
                        }

                    case BlackListType.NONCOMINCIACON:
                        {
                            return "Blocca se non comincia con \"" + m_ValoreContatto + "\" ";
                        }

                    case BlackListType.NONFINISCECON:
                        {
                            return "Blocca se non finisce con \"" + m_ValoreContatto + "\" ";
                        }

                    case BlackListType.UGUALEA:
                        {
                            return "Blocca se è uguale a \"" + m_ValoreContatto + "\" ";
                        }

                    default:
                        {
                            // oops
                            return "OOpps";
                        }
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.BlackListAdresses;
            }

          
        }
    }
}