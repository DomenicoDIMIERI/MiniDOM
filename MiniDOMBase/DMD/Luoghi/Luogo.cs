using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta un luogo geografico, un indirizzo, un comune o una nazione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public abstract class Luogo 
            : Databases.DBObjectPO, IComparable, IComparable<Luogo>
        {
            private string m_Nome;
            private string m_IconURL;
            private int? m_NumeroAbitanti;
            private string m_NomeAbitanti;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public Luogo()
            {
                this.m_Nome = "";
                this.m_IconURL = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="nome"></param>
            public Luogo(string nome)
            {
                m_Nome = DMD.Strings.Trim(nome);
            }

            /// <summary>
            /// Restituisce o imposta il numero di abitanti
            /// </summary>
            public int? NumeroAbitanti
            {
                get
                {
                    return m_NumeroAbitanti;
                }

                set
                {
                    if (value.HasValue && value.Value < 0)
                        throw new ArgumentOutOfRangeException("NumeroAbitanti non valido");
                    var oldValue = m_NumeroAbitanti;
                    if (oldValue == value)
                        return;
                    m_NumeroAbitanti = value;
                    DoChanged("NumeroAbitanti", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome degli abitanti
            /// </summary>
            public string NomeAbitanti
            {
                get
                {
                    return m_NomeAbitanti;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAbitanti;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAbitanti = value;
                    DoChanged("NomeAbitanti", value, oldValue);
                }
            }

             

            /// <summary>
            /// Restituisce o imposta l'icona
            /// </summary>
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

            /// <summary>
            /// Restituisce o imposta il nome del luogo
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

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_NumeroAbitanti = reader.Read("NumeroAbitanti", this.m_NumeroAbitanti);
                this.m_NomeAbitanti = reader.Read("NomeAbitanti", this.m_NomeAbitanti);
                this.m_IconURL = reader.Read("IconURL", this.m_IconURL);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("NumeroAbitanti", this.m_NumeroAbitanti);
                writer.Write("NomeAbitanti", this.m_NomeAbitanti);
                writer.Write("Nome", this.m_Nome);
                writer.Write("IconURL", this.m_IconURL);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Preapra lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("NumeroAbitanti", typeof(int), 1);
                c = table.Fields.Ensure("NomeAbitanti", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAbitanti", new string[] { "NumeroAbitanti", "NomeAbitanti" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", this.m_Nome);
                writer.WriteAttribute("IconURL", this.m_IconURL);
                writer.WriteAttribute("NomeAbitanti", this.m_NomeAbitanti);
                writer.WriteAttribute("NumeroAbitanti", this.m_NumeroAbitanti);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                     
                    case "NomeAbitanti":
                        {
                            m_NomeAbitanti = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    case "NumeroAbitanti":
                        {
                            m_NumeroAbitanti = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Luogo) && this.Equals((Luogo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Luogo obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                     && DMD.Integers.EQ(this.m_NumeroAbitanti, obj.m_NumeroAbitanti)
                     && DMD.Strings.EQ(this.m_NomeAbitanti, obj.m_NomeAbitanti)
                     ;
                    //m_Parameters;
            }


            int IComparable.CompareTo(object obj)
            {
                return CompareTo((Luogo)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public virtual int CompareTo(Luogo c)
            {
                return DMD.Strings.Compare(Nome, c.Nome, true);
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}
        }
    }
}