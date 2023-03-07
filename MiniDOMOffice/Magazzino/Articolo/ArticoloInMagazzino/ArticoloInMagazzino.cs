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
        /// Definisce la relazione tra un articolo ed il magazzino
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ArticoloInMagazzino 
            : Databases.DBObject, IComparable, IComparable<ArticoloInMagazzino>
        {
            private double m_Quantita;
            private int m_IDArticolo;
            [NonSerialized] private Articolo m_Articolo;
            private int m_IDMagazzino;
            [NonSerialized] private Magazzino m_Magazzino;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloInMagazzino()
            {
                m_Quantita = 0.0d;
                m_IDArticolo = 0;
                m_Articolo = null;
                m_IDMagazzino = 0;
                m_Magazzino = null;
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
            public Magazzino Magazzino
            {
                get
                {
                    lock (this)
                    {
                        if (m_Magazzino is null)
                            m_Magazzino = Magazzini.GetItemById(m_IDMagazzino);
                        return m_Magazzino;
                    }
                }

                set
                {
                    Magazzino oldValue;
                    lock (this)
                    {
                        oldValue = m_Magazzino;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Magazzino = value;
                        m_IDMagazzino = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Magazzino", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del magazzino
            /// </summary>
            public int IDMagazzino
            {
                get
                {
                    return DBUtils.GetID(m_Magazzino, m_IDMagazzino);
                }

                set
                {
                    int oldValue = IDMagazzino;
                    if (oldValue == value)
                        return;
                    m_IDMagazzino = value;
                    m_Magazzino = null;
                    DoChanged("IDMagazzino", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità dell'articolo in magazzino espressa in unità base
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Quantita
            {
                get
                {
                    return m_Quantita;
                }

                set
                {
                    double oldValue = m_Quantita;
                    if (oldValue == value)
                        return;
                    m_Quantita = value;
                    DoChanged("Quantita", value, oldValue);
                }
            }
             
            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDArticolo", IDArticolo);
                writer.WriteAttribute("IDMagazzino", IDMagazzino);
                writer.WriteAttribute("Quantita", m_Quantita);
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

                    case "IDMagazzino":
                        {
                            m_IDMagazzino = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
 
                    case "Quantita":
                        {
                            m_Quantita = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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
                return "tbl_OfficeArticoliMagazzino";
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.Articoli.QuantitaInMagazzino;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDArticolo = reader.Read("IDArticolo",  m_IDArticolo);
                m_IDMagazzino = reader.Read("IDMagazzino",  m_IDMagazzino);
                m_Quantita = reader.Read("Quantita",  m_Quantita);
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
                writer.Write("IDMagazzino", IDMagazzino);
                writer.Write("Quantita", m_Quantita);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDArticolo", typeof(int), 1);
                c = table.Fields.Ensure("IDMagazzino", typeof(int), 1);
                c = table.Fields.Ensure("Quantita", typeof(double), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxArticolo", new string[] { "IDArticolo", "IDMagazzino", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxQuantita", new string[] { "Quantita" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(ArticoloInMagazzino other)
            {
                return (int)(m_Quantita - other.m_Quantita);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((ArticoloInMagazzino)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", this.m_IDArticolo, " x ", this.m_IDMagazzino, " : ", this.m_Quantita, " }");
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
                return (obj is ArticoloInMagazzino) && this.Equals((ArticoloInMagazzino)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ArticoloInMagazzino obj)
            {
                return base.Equals(obj)
                    && DMD.Doubles.EQ(this.m_Quantita, obj.m_Quantita)
                    && DMD.Integers.EQ(this.m_IDArticolo, obj.m_IDArticolo)
                    && DMD.Integers.EQ(this.m_IDMagazzino, obj.m_IDMagazzino)
                    ;
            }
        }
    }
}