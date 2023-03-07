using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta un conto corrente online
        /// </summary>
        [Serializable]
        public class ContoOnline 
            : minidom.Databases.DBObject, IMetodoDiPagamento
        {
            private string m_Name;
            private int m_IDContoCorrente;
            [NonSerialized] private ContoCorrente m_ContoCorrente;
            private string m_NomeConto;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private string m_Account;
            private string m_Password;
            private string m_Sito;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ContoOnline()
            {
                m_Name = "";
                m_IDContoCorrente = 0;
                m_ContoCorrente = null;
                m_NomeConto = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_Account = "";
                m_Password = "";
                m_Sito = "";
                m_Flags = 0;
            }

            /// <summary>
            /// Restituisce o imposta il nome della carta
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
            /// Restituisce o imposta l'ID del conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContoCorrente
            {
                get
                {
                    return DBUtils.GetID(m_ContoCorrente, m_IDContoCorrente);
                }

                set
                {
                    int oldValue = IDContoCorrente;
                    if (oldValue == value)
                        return;
                    m_IDContoCorrente = value;
                    m_ContoCorrente = null;
                    DoChanged("IDContoCorrente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public ContoCorrente ContoCorrente
            {
                get
                {
                    if (m_ContoCorrente is null)
                        m_ContoCorrente = ContiCorrente.GetItemById(m_IDContoCorrente);
                    return m_ContoCorrente;
                }

                set
                {
                    var oldValue = m_ContoCorrente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ContoCorrente = value;
                    m_IDContoCorrente = DBUtils.GetID(value, this.m_IDContoCorrente);
                    m_NomeConto = "";
                    if (value is object)
                        m_NomeConto = value.Nome;
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del conto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeConto
            {
                get
                {
                    return m_NomeConto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeConto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeConto = value;
                    DoChanged("NomeConto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il conto corrente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetContoCorrente(ContoCorrente value)
            {
                m_ContoCorrente = value;
                m_IDContoCorrente = DBUtils.GetID(value, this.m_IDContoCorrente);
            }

            /// <summary>
            /// Restitusice o imposta il nome dell'account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Account
            {
                get
                {
                    return m_Account;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Account;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Account = value;
                    DoChanged("Account", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il sito su cui è aperto il conto (tipo PayPal)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Sito
            {
                get
                {
                    return m_Sito;
                }

                set
                {
                    string oldValue = m_Sito;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sito = value;
                    DoChanged("Sito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la password di accesso al sito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Password
            {
                get
                {
                    return m_Password;
                }

                set
                {
                    string oldValue = m_Password;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Password = value;
                    DoChanged("Password", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInzio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di scadenza del conto
            /// </summary>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            
          
            /// <summary>
            /// Restituisce il modulo
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ContiOnline; //.Module;
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ContiOnline";
            }

            /// <summary>
            /// Restitusice una stringa che rappresenta loggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray("{ ", m_Sito, " : ", Account, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Sito, this.m_Account);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is ContoOnline) && this.Equals((ContoOnline)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ContoOnline obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                    && DMD.Integers.EQ(this.IDContoCorrente, obj.IDContoCorrente)
                    && DMD.Strings.EQ(this.m_NomeConto, obj.m_NomeConto)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Strings.EQ(this.m_Account, obj.m_Account)
                    && DMD.Strings.EQ(this.m_Password, obj.m_Password)
                    && DMD.Strings.EQ(this.m_Sito, obj.m_Sito)
                    ;
                    //private CKeyCollection m_Parametri;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name",  m_Name);
                m_IDContoCorrente = reader.Read("IDContoCorrente",  m_IDContoCorrente);
                m_NomeConto = reader.Read("NomeConto",  m_NomeConto);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataFine = reader.Read("DataFine",  m_DataFine);
                m_Sito = reader.Read("Sito",  m_Sito);
                m_Account = reader.Read("Account",  m_Account);
                m_Password = reader.Read("Password",  m_Password);
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
                writer.Write("IDContoCorrente", IDContoCorrente);
                writer.Write("NomeConto", m_NomeConto);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Sito", m_Sito);
                writer.Write("Account", m_Account);
                writer.Write("Password", m_Password);
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
                c = table.Fields.Ensure("IDContoCorrente", typeof(int), 1);
                c = table.Fields.Ensure("NomeConto", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("Flags", typeof(int), 1);
                c = table.Fields.Ensure("Sito", typeof(string), 255);
                c = table.Fields.Ensure("Account", typeof(string), 255);
                c = table.Fields.Ensure("Password", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Name" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxConto", new string[] { "IDContoCorrente", "NomeConto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSito", new string[] { "Sito", "Account" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("IDContoCorrente", IDContoCorrente);
                writer.WriteAttribute("NomeConto", m_NomeConto);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Sito", m_Sito);
                writer.WriteAttribute("Account", m_Account);
                writer.WriteAttribute("Password", m_Password);
                base.XMLSerialize(writer);                
            }

            /// <summary>
            /// Serializzazione XML
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

                    case "IDContoCorrente":
                        {
                            m_IDContoCorrente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConto":
                        {
                            m_NomeConto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Sito":
                        {
                            m_Sito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Account":
                        {
                            m_Account = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Password":
                        {
                            m_Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                     
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            string IMetodoDiPagamento.NomeMetodo
            {
                get
                {
                    return m_Name;
                }
            }
        }
    }
}