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
        /// Definisce il tipo prezzo
        /// </summary>
        /// <remarks></remarks>
        public enum TipoPrezzoListino : int
        {
            /// <summary>
            /// Flag validi per le relazioni Articoli x Listino
            /// </summary>
            FISSO = 0
        }

        /// <summary>
        /// Definisce la relazione tra un articolo ed un listino prezzi
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ArticoloXListino 
            : minidom.Databases.DBObject
        {
            private TipoPrezzoListino m_TipoPrezzo;
            private int m_IDArticolo;
            [NonSerialized] private Articolo m_Articolo;
            private int m_IDListino;
            [NonSerialized] private Listino m_Listino;
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloXListino()
            {
                m_IDArticolo = 0;
                m_Articolo = null;
                m_IDListino = 0;
                m_Listino = null;
                m_TipoPrezzo = TipoPrezzoListino.FISSO;
            }

            /// <summary>
            /// Restituisce o imposta il tipo prezzo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TipoPrezzoListino TipoPrezzo
            {
                get
                {
                    return m_TipoPrezzo;
                }

                set
                {
                    var oldValue = m_TipoPrezzo;
                    if (oldValue == value)
                        return;
                    m_TipoPrezzo = value;
                    DoChanged("TipoPrezzo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'articolo a cui è associato il codice
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Articolo Articolo
            {
                get
                {
                    lock (this)
                    {
                        if (m_Articolo is null)
                            m_Articolo = Articoli.GetItemById(m_IDArticolo);
                        return m_Articolo;
                    }
                }

                set
                {
                    Articolo oldValue;
                    lock (this)
                    {
                        oldValue = m_Articolo;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Articolo = value;
                        m_IDArticolo = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Articolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'articolo
            /// </summary>
            public int IDArticolo
            {
                get
                {
                    return DBUtils.GetID(m_Articolo, m_IDArticolo);
                }

                set
                {
                    int oldValue = IDArticolo;
                    if (oldValue == value)
                        return;
                    m_IDArticolo = 0;
                    m_Articolo = null;
                    DoChanged("IDArticolo", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'articolo
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetArticolo(Articolo value)
            {
                m_Articolo = value;
                m_IDArticolo = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il magazzino
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Listino Listino
            {
                get
                {
                    lock (this)
                    {
                        if (m_Listino is null)
                            m_Listino = Listini.GetItemById(m_IDListino);
                        return m_Listino;
                    }
                }

                set
                {
                    Listino oldValue;
                    lock (this)
                    {
                        oldValue = m_Listino;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Listino = value;
                        m_IDListino = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Listino", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'articolo
            /// </summary>
            public int IDListino
            {
                get
                {
                    return DBUtils.GetID(m_Listino, m_IDListino);
                }

                set
                {
                    int oldValue = IDListino;
                    if (oldValue == value)
                        return;
                    m_IDListino = value;
                    m_Listino = null;
                    DoChanged("IDListino", value, oldValue);
                }
            }

             
            /// <summary>
            /// Calcola il prezzo di vendita
            /// </summary>
            /// <param name="docuemnto"></param>
            /// <returns></returns>
            public decimal CalcolaPrezzoVendita(object docuemnto)
            {
                //TODO CalcolaPrezzoVendita
                return 0m;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDArticolo", IDArticolo);
                writer.WriteAttribute("IDListino", IDListino);
                writer.WriteAttribute("TipoPrezzo", (int?)m_TipoPrezzo);
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
                    case "IDArticolo":
                        {
                            m_IDArticolo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDListino":
                        {
                            m_IDListino = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
 
                    case "TipoPrezzo":
                        {
                            m_TipoPrezzo = (TipoPrezzoListino)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return "tbl_OfficeArticoliListino";
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.Articoli.Listini;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDArticolo = reader.Read("IDArticolo",  m_IDArticolo);
                m_IDListino = reader.Read("IDListino",  m_IDListino);
                m_Flags = reader.Read("Flags",  m_Flags);
                m_TipoPrezzo = reader.Read("TipoPrezzo",  m_TipoPrezzo);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDArticolo", IDArticolo);
                writer.Write("IDListino", IDListino);
                writer.Write("TipoPrezzo", m_TipoPrezzo);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDArticolo", typeof(int), 1);
                c = table.Fields.Ensure("IDListino", typeof(int), 1);
                c = table.Fields.Ensure("TipoPrezzo", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxArticolo", new string[] { "IDArticolo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTipo", new string[] { "IDListino", "TipoPrezzo" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_IDListino, " x ", this.m_IDArticolo);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDArticolo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is ArticoloXListino) && this.Equals((ArticoloXListino)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ArticoloXListino obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ((int)this.m_TipoPrezzo, (int)obj.m_TipoPrezzo)
                    && DMD.Integers.EQ(this.m_IDArticolo, obj.m_IDArticolo)
                    && DMD.Integers.EQ(this.m_IDListino, obj.m_IDListino)
                    ;
            }


        }
    }
}