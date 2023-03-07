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
        /// Rappresenta la fonte di un contatto, di una pratica o di una persona
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CFonte 
            : Databases.DBObject, IFonte, ICloneable
        {
            private string m_Nome;
            private string m_Tipo;
            private string m_IDCampagna;
            private string m_IDAnnuncio;
            private string m_IDKeyWord;
            private string m_IconURL;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private bool m_Attiva;
            private string m_Siti;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFonte()
            {
                m_Nome = "";
                m_Tipo = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_Attiva = true;
                m_IconURL = "";
                m_IDCampagna = "";
                m_IDAnnuncio = "";
                m_IDKeyWord = "";
                m_Siti = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="nome"></param>
            public CFonte(string tipo, string nome) : this()
            {
                tipo = DMD.Strings.Trim(tipo);
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(tipo))
                    throw new ArgumentNullException("tipo");
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException("nome");
                m_Nome = nome;
                m_Tipo = tipo;
            }


            /// <summary>
            /// Restituisce o imposta il nome della fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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

            string IFonte.Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso dell'immagine utilizzata come icona per la fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            string IFonte.IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della fonte (Radio, TV, Cartaceo, ecc)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio validità della fonte
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
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine validità della fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la fonte è attiva
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attiva
            {
                get
                {
                    return m_Attiva;
                }

                set
                {
                    if (m_Attiva == value)
                        return;
                    m_Attiva = value;
                    DoChanged("Attiva", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che identifica la campagna pubblicitaria a cui appartiene la fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IDCampagna
            {
                get
                {
                    return m_IDCampagna;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IDCampagna;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDCampagna = value;
                    DoChanged("IDCampagna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che identifica univocamente questa fonte in un database esterno
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IDAnnuncio
            {
                get
                {
                    return m_IDAnnuncio;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IDAnnuncio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDAnnuncio = value;
                    DoChanged("IDAnnuncio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la parola associata alla fonte (per campagne tipo google)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IDKeyWord
            {
                get
                {
                    return m_IDKeyWord;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IDKeyWord;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDKeyWord = value;
                    DoChanged("IDKeyWord", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa descrittiva
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Siti
            {
                get
                {
                    return m_Siti;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Siti;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Siti = value;
                    DoChanged("Siti", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se l'oggeto è valido alla data corrente
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return Attiva & DMD.DateUtils.CheckBetween(DMD.DateUtils.Now(), m_DataInizio, m_DataFine);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_Tipo , " : ", this.m_Nome);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Tipo, this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CFonte) && this.Equals((CFonte)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CFonte obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_IDCampagna, obj.m_IDCampagna)
                    && DMD.Strings.EQ(this.m_IDAnnuncio, obj.m_IDAnnuncio)
                    && DMD.Strings.EQ(this.m_IDKeyWord, obj.m_IDKeyWord)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Booleans.EQ(this.m_Attiva, obj.m_Attiva)
                    && DMD.Strings.EQ(this.m_Siti, obj.m_Siti)
                    ;
            //private CKeyCollection m_Parameters;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Fonti; //.Module;
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_FontiContatto";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_Tipo = reader.Read("Tipo",  m_Tipo);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataFine = reader.Read("DataFine",  m_DataFine);
                m_Attiva = reader.Read("Attiva",  m_Attiva);
                m_IconURL = reader.Read("IconURL",  m_IconURL);
                m_IDCampagna = reader.Read("IDCampagna",  m_IDCampagna);
                m_IDAnnuncio = reader.Read("IDAnnuncio",  m_IDAnnuncio);
                m_IDKeyWord = reader.Read("IDKeyWord",  m_IDKeyWord);
                m_Siti = reader.Read("Siti",  m_Siti);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ClassName", GetType().FullName);
                writer.Write("Nome", m_Nome);
                writer.Write("Tipo", m_Tipo);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Attiva", m_Attiva);
                writer.Write("IconURL", m_IconURL);
                writer.Write("IDCampagna", m_IDCampagna);
                writer.Write("IDAnnuncio", m_IDAnnuncio);
                writer.Write("IDKeyWord", m_IDKeyWord);
                writer.Write("Siti", m_Siti);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("ClassName", typeof(string), 255);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("Attiva", typeof(bool), 1);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("IDCampagna", typeof(string), 255);
                c = table.Fields.Ensure("IDAnnuncio", typeof(string), 255);
                c = table.Fields.Ensure("IDKeyWord", typeof(string), 255);
                c = table.Fields.Ensure("Siti", typeof(string), 0);
                c = table.Fields.Ensure("Flags", typeof(int), 1);
                c = table.Fields.Ensure("Parameters", typeof(string), 0);

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "ClassName", "Tipo", "Nome" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine", "Attiva" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCodici", new string[] { "IDCampagna", "IDAnnuncio", "IDKeyWord" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
                //c = table.Fields.Ensure("Siti", typeof(string), 0);
                //c = table.Fields.Ensure("Flags", typeof(int), 1);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Attiva", m_Attiva);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("IDCampagna", m_IDCampagna);
                writer.WriteAttribute("IDAnnuncio", m_IDAnnuncio);
                writer.WriteAttribute("IDKeyWord", m_IDKeyWord);
                base.XMLSerialize(writer);
                writer.WriteTag("Siti", m_Siti);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

                    case "Attiva":
                        {
                            m_Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCampagna":
                        {
                            m_IDCampagna = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAnnuncio":
                        {
                            m_IDAnnuncio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDKeyWord":
                        {
                            m_IDKeyWord = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Siti":
                        {
                            m_Siti = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CFonte Clone()
            {
                return (CFonte)this.MemberwiseClone();
            }
            

            object ICloneable.Clone()
            {
                return this.Clone();
            }
        }
    }
}