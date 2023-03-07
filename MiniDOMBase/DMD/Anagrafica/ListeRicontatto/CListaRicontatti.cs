using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Lista di ricontatti
        /// </summary>
        [Serializable]
        public class CListaRicontatti 
            : Databases.DBObjectPO
        {
            private string m_Name;
            private string m_Descrizione;
            private DateTime? m_DataInserimento;
            private int m_IDProprietario;
            [NonSerialized] private Sistema.CUser m_Proprietario;
            private string m_NomeProprietario;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CListaRicontatti()
            {
                m_Name = "";
                m_Descrizione = "";
                m_DataInserimento = default;
                m_IDProprietario = 0;
                m_Proprietario = null;
                m_NomeProprietario = "";
            }

             

            /// <summary>
            /// Restituisce o imposta il nome della lista
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione della lista
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di generazione della lista
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInserimento
            {
                get
                {
                    return m_DataInserimento;
                }

                set
                {
                    var oldValue = m_DataInserimento;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataInserimento = value;
                    DoChanged("DataInserimento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il proprietario della lista
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Proprietario
            {
                get
                {
                    if (m_Proprietario is null)
                        m_Proprietario = Sistema.Users.GetItemById(m_IDProprietario);
                    return m_Proprietario;
                }

                set
                {
                    var oldValue = Proprietario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Proprietario = value;
                    m_IDProprietario = DBUtils.GetID(value, this.m_IDProprietario);
                    if (value is object)
                        m_NomeProprietario = value.Nominativo;
                    DoChanged("Proprietario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del proprietario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDProprietario
            {
                get
                {
                    return DBUtils.GetID(m_Proprietario, m_IDProprietario);
                }

                set
                {
                    int oldValue = IDProprietario;
                    if (oldValue == value)
                        return;
                    m_Proprietario = null;
                    m_IDProprietario = value;
                    DoChanged("IDProprietariO", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del proprietario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeProprietario
            {
                get
                {
                    return m_NomeProprietario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeProprietario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProprietario = value;
                    DoChanged("NomeProprietario", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il proprietario
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetProprietario(Sistema.CUser value)
            {
                m_Proprietario = value;
                m_IDProprietario = DBUtils.GetID(value, m_IDProprietario);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Name);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CListaRicontatti) && this.Equals((CListaRicontatti)obj);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CListaRicontatti obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.DateUtils.EQ(this.m_DataInserimento, obj.m_DataInserimento)
                    && DMD.Integers.EQ(this.m_IDProprietario, obj.m_IDProprietario)
                    && DMD.Strings.EQ(this.m_NomeProprietario, obj.m_NomeProprietario)
                    ;
            //private CKeyCollection m_Parameters;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ListeRicontatto; //.Module;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return ListeRicontatto.Database;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ListeRicontatto";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name",  m_Name);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_DataInserimento = reader.Read("DataInserimento",  m_DataInserimento);
                m_IDProprietario = reader.Read("IDProprietario",  m_IDProprietario);
                m_NomeProprietario = reader.Read("NomeProprietario",  m_NomeProprietario);
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
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("DataInserimento", m_DataInserimento);
                writer.Write("IDProprietario", IDProprietario);
                writer.Write("NomeProprietario", m_NomeProprietario);
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
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("DataInserimento", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDProprietario", typeof(int), 1);
                c = table.Fields.Ensure("NomeProprietario", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxName", new string[] { "Name" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInserimento", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOwner", new string[] { "IDProprietario", "NomeProprietario" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Descrizione", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("DataInserimento", m_DataInserimento);
                writer.WriteAttribute("IDProprietario", IDProprietario);
                writer.WriteAttribute("NomeProprietario", m_NomeProprietario);
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
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInserimento":
                        {
                            m_DataInserimento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDProprietario":
                        {
                            m_IDProprietario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProprietario":
                        {
                            m_NomeProprietario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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