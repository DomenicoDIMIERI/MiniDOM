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
        /// Definisce una marca
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class MarcaArticolo 
            : minidom.Databases.DBObject
        {
            private string m_Nome;            // Nome dell'articolo
            private string m_Descrizione;
            private string m_IconURL;
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public MarcaArticolo()
            {
                m_Nome = "";
                m_Descrizione = "";
                m_IconURL = "";
            }


            /// <summary>
            /// Restituisce o imposta il nome della categoria
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
            /// Restituisce o imposta una stringa che descrive in dettaglio l'articolo
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
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la url dell'immagine principale dell'articolo
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

          
            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IconURL", m_IconURL);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 
                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return "tbl_OfficeMarcheArticoli";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.MarcheArticoli;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_IconURL = reader.Read("IconURL",  m_IconURL);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IconURL", m_IconURL);
                writer.Write("Descrizione", m_Descrizione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Restituisce il nome della marca
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
                return (obj is MarcaArticolo) && this.Equals((MarcaArticolo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MarcaArticolo obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    ;
            }

            
        }
    }
}