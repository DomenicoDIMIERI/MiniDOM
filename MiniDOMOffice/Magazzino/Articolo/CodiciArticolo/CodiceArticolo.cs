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
        /// Definisce un codice articolo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CodiceArticolo 
            : minidom.Databases.DBObject, IComparable, IComparable<CodiceArticolo>
        {
            private int m_IDArticolo;
            [NonSerialized] private Articolo m_Articolo;
            private string m_Tipo;
            private string m_Nome;            // Nome dell'articolo
            private string m_Valore;
            private int m_Ordine;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CodiceArticolo()
            {
                m_IDArticolo = 0;
                m_Articolo = null;
                m_Nome = "";
                m_Tipo = "";
                m_Valore = "";
                m_Flags = 0;
                m_Ordine = 0;
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
            /// Restituisce o imposta il nome del codice articolo
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
            /// Restituisce o imposta il tipo del codice articolo
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
            /// Restituisce o imposta il valore del codice articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Valore
            {
                get
                {
                    return m_Valore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Valore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Valore = value;
                    DoChanged("Valore", value, oldValue);
                }
            }


             
            /// <summary>
            /// Restituisce o imposta l'ordine inteso anche come priorità nella ricerca degli articoli per codice in caso di codici duplicati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Ordine
            {
                get
                {
                    return m_Ordine;
                }

                set
                {
                    int oldValue = m_Ordine;
                    if (oldValue == value)
                        return;
                    m_Ordine = value;
                    DoChanged("Ordine", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'ordine di visualizzazione del codice articolo
            /// </summary>
            /// <param name="value"></param>
            internal virtual void SetOrdine(int value)
            {
                m_Ordine = value;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDArticolo", IDArticolo);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Valore", m_Valore);
                writer.WriteAttribute("Ordine", m_Ordine);
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

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                   
                    case "Ordine":
                        {
                            m_Ordine = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDArticolo":
                        {
                            m_IDArticolo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return "tbl_OfficeCodiciArticolo";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.Articoli.Codici;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_Tipo = reader.Read("Tipo",  m_Tipo);
                m_Valore = reader.Read("Valore",  m_Valore);
                m_Ordine = reader.Read("Ordine",  m_Ordine);
                m_IDArticolo = reader.Read("IDArticolo",  m_IDArticolo);
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
                writer.Write("Tipo", m_Tipo);
                writer.Write("Valore", m_Valore);
                writer.Write("Ordine", m_Ordine);
                writer.Write("IDArticolo", IDArticolo);
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
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Valore", typeof(string), 255);
                c = table.Fields.Ensure("Ordine", typeof(int), 1);
                c = table.Fields.Ensure("IDArticolo", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Tipo", "Valore", "IDArticolo", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxOrdine", new string[] { "Ordine" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ ", this.m_Tipo , " : " , m_Valore , " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Valore);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CodiceArticolo other)
            {
                int ret = DMD.Integers.Compare(this.m_Ordine , other.m_Ordine);
                //if (ret == 0) ret = DMD.Strings.Compare(m_Nome, other.m_Nome, true);
                if (ret == 0) ret = DMD.Strings.Compare(m_Nome, other.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CodiceArticolo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CodiceArticolo) && this.Equals((CodiceArticolo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CodiceArticolo obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDArticolo, obj.m_IDArticolo)
                    && DMD.Strings.EQ(this.m_Tipo, obj.m_Tipo)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Valore, obj.m_Valore)
                    && DMD.Integers.EQ(this.m_Ordine, obj.m_Ordine)
                    ;
            }
        }
    }
}