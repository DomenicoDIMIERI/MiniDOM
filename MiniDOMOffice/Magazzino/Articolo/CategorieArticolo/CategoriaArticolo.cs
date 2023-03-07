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
        /// Definisce la categoria di un articolo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CategoriaArticolo
            : minidom.Databases.DBObject
        {
            private int m_IDParent;
            [NonSerialized] private CategoriaArticolo m_Parent;
            private string m_Nome;            // Nome dell'articolo
            private string m_Descrizione;
            private string m_IconURL;
            private AttributiCategoriaCollection m_Attributi;
            private SottocategorieCollection m_Sottocategorie;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CategoriaArticolo()
            {
                m_Parent = null;
                m_IDParent = 0;
                m_Nome = "";
                m_Descrizione = "";
                m_Flags = 0;
                m_Attributi = null;
                m_IconURL = "";
                m_Sottocategorie = null;
            }

            /// <summary>
            /// Sottocategorie
            /// </summary>
            public SottocategorieCollection Sottocategorie
            {
                get
                {
                    lock (this)
                    {
                        if (m_Sottocategorie is null)
                            m_Sottocategorie = new SottocategorieCollection(this);
                        return m_Sottocategorie;
                    }
                }
            }

            /// <summary>
            /// Categoria genitore
            /// </summary>
            public CategoriaArticolo Parent
            {
                get
                {
                    lock (this)
                    {
                        if (m_Parent is null)
                            m_Parent = CategorieArticoli.GetItemById(m_IDParent);
                        return m_Parent;
                    }
                }

                set
                {
                    CategoriaArticolo oldValue;
                    lock (this)
                    {
                        oldValue = Parent;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Parent = value;
                        m_IDParent = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Parent", value, oldValue);
                }
            }

            /// <summary>
            /// ID della categoria genitore
            /// </summary>
            public int IDParent
            {
                get
                {
                    return DBUtils.GetID(m_Parent, m_IDParent);
                }

                set
                {
                    int oldValue = IDParent;
                    if (oldValue == value)
                        return;
                    m_IDParent = value;
                    m_Parent = null;
                    DoChanged("IDParent", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la categoria genitore
            /// </summary>
            /// <param name="parent"></param>
            protected internal virtual void SetParent(CategoriaArticolo parent)
            {
                m_Parent = parent;
                m_IDParent = DBUtils.GetID(parent, 0);
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
            /// Restitusice una collezione di proprietà aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public AttributiCategoriaCollection Attributi
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attributi is null)
                            m_Attributi = new AttributiCategoriaCollection(this);
                        return m_Attributi;
                    }
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
                writer.WriteAttribute("IDParent", IDParent);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                writer.WriteTag("Attributi", Attributi);
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

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (AttributiCategoriaCollection)fieldValue;
                            m_Attributi.SetCategoria(this);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDParent":
                        {
                            m_IDParent = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return "tbl_OfficeCategorieArticoli";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.CategorieArticoli;
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (m_Attributi is object)
                    m_Attributi.Save(force);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_IconURL = reader.Read("IconURL", m_IconURL);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_IDParent = reader.Read("IDParent", m_IDParent);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Attributi = (AttributiCategoriaCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                    m_Attributi.SetCategoria(this);
                }
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
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("IDParent", IDParent);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("Attributi", typeof(string), 0);
                c = table.Fields.Ensure("IDParent", typeof(int), 1);

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxParent", new string[] { "IDParent", "IconURL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
            }


            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged()
                      || (m_Attributi is object && DBUtils.IsChanged(this.m_Attributi));
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
                return (obj is CategoriaArticolo) && this.Equals((CategoriaArticolo)obj);
            }


            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CategoriaArticolo obj)
            {
                return base.Equals(obj)
                        && DMD.Integers.EQ(this.m_IDParent, obj.m_IDParent)
                        && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                        && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                        && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                        ;
                //private AttributiCategoriaCollection m_Attributi;
                //private SottocategorieCollection m_Sottocategorie;

            }

        }
    }
}