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
        /// Flag per gli oggetti collegati ad un articolo
        /// </summary>
        public enum RelazioniOggettiCollegati : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            Sconosciuta = 0,

            /// <summary>
            /// L'articolo é utilizzato 
            /// </summary>
            UtilizzatoDa = 2
        }

        /// <summary>
        /// Relazione tra un oggetto generico ed un articolo
        /// </summary>
        [Serializable]
        public class OggettoCollegato 
            : minidom.Databases.DBObject
        {
            private DateTime? m_DataCollegamento;
            private DateTime? m_DataScollegamento;
            [NonSerialized] private RelazioniOggettiCollegati m_Relazione;
            private int m_IDOggetto1;
            [NonSerialized] private OggettoInventariato m_Oggetto1;
            private int m_IDOggetto2;
            [NonSerialized] private OggettoInventariato m_Oggetto2;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettoCollegato()
            {
                m_DataCollegamento = default;
                m_DataScollegamento = default;
                m_Relazione = RelazioniOggettiCollegati.Sconosciuta;
                m_IDOggetto1 = 0;
                m_Oggetto1 = null;
                m_IDOggetto2 = 0;
                m_Oggetto2 = null;
            }

            /// <summary>
            /// DataCollegamento
            /// </summary>
            public DateTime? DataCollegamento
            {
                get
                {
                    return m_DataCollegamento;
                }

                set
                {
                    var oldValue = m_DataCollegamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataCollegamento = value;
                    DoChanged("DataCollegamento", value, oldValue);
                }
            }

            /// <summary>
            /// DataScollegamento
            /// </summary>
            public DateTime? DataScollegamento
            {
                get
                {
                    return m_DataScollegamento;
                }

                set
                {
                    var oldValue = m_DataScollegamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataScollegamento = value;
                    DoChanged("DataScollegamento", value, oldValue);
                }
            }

            /// <summary>
            /// IDOggetto1
            /// </summary>
            public int IDOggetto1
            {
                get
                {
                    return DBUtils.GetID(m_Oggetto1, m_IDOggetto1);
                }

                set
                {
                    int oldValue = IDOggetto1;
                    if (oldValue == value)
                        return;
                    m_Oggetto1 = null;
                    m_IDOggetto1 = value;
                    DoChanged("IDOggetto1", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'oggetto 1
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetOggetto1(OggettoInventariato value)
            {
                m_IDOggetto1 = DBUtils.GetID(value, 0);
                m_Oggetto1 = value;
            }

            /// <summary>
            /// Oggetto 1
            /// </summary>
            public OggettoInventariato Oggetto1
            {
                get
                {
                    lock (this)
                    {
                        if (m_Oggetto1 is null)
                            m_Oggetto1 = OggettiInventariati.GetItemById(m_IDOggetto1);
                        return m_Oggetto1;
                    }
                }

                set
                {
                    OggettoInventariato oldValue;
                    lock (this)
                    {
                        oldValue = m_Oggetto1;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Oggetto1 = value;
                        m_IDOggetto1 = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Oggetto1", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'oggetto 2
            /// </summary>
            public int IDOggetto2
            {
                get
                {
                    return DBUtils.GetID(m_Oggetto2, m_IDOggetto2);
                }

                set
                {
                    int oldValue = IDOggetto2;
                    if (oldValue == value)
                        return;
                    m_Oggetto2 = null;
                    m_IDOggetto2 = value;
                    DoChanged("IDOggetto2", value, oldValue);
                }
            }

            /// <summary>
            /// Oggetto 2
            /// </summary>
            public OggettoInventariato Oggetto2
            {
                get
                {
                    lock (this)
                    {
                        if (m_Oggetto2 is null)
                            m_Oggetto2 = OggettiInventariati.GetItemById(m_IDOggetto2);
                        return m_Oggetto2;
                    }
                }

                set
                {
                    OggettoInventariato oldValue;
                    lock (this)
                    {
                        oldValue = m_Oggetto2;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Oggetto2 = value;
                        m_IDOggetto2 = DBUtils.GetID(value, 0);
                    }

                    DoChanged("Oggetto2", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'oggetto 2
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetOggetto2(OggettoInventariato value)
            {
                m_IDOggetto2 = DBUtils.GetID(value, 0);
                m_Oggetto2 = value;
            }

            /// <summary>
            /// Tipo di relazione
            /// </summary>
            public RelazioniOggettiCollegati Relazione
            {
                get
                {
                    return m_Relazione;
                }

                set
                {
                    var oldValue = m_Relazione;
                    if (oldValue == value)
                        return;
                    m_Relazione = value;
                    DoChanged("Relazione", value, oldValue);
                }
            }
             
            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataCollegamento", m_DataCollegamento);
                writer.WriteAttribute("DataScollegamento", m_DataScollegamento);
                writer.WriteAttribute("Relazione", (int?)m_Relazione);
                writer.WriteAttribute("IDOggetto1", IDOggetto1);
                writer.WriteAttribute("IDOggetto2", IDOggetto2);
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
                    case "DataCollegamento":
                        {
                            m_DataCollegamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataScollegamento":
                        {
                            m_DataScollegamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Relazione":
                        {
                            m_Relazione = (RelazioniOggettiCollegati)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOggetto1":
                        {
                            m_IDOggetto1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOggetto2":
                        {
                            m_IDOggetto2 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return "tbl_OfficeOggettiCollegati";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.OggettiInventariati.OggettiCollegati;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_DataCollegamento = reader.Read("DataCollegamento", m_DataCollegamento);
                m_DataScollegamento = reader.Read("DataScollegamento", m_DataScollegamento);
                m_Relazione = reader.Read("Relazione", m_Relazione);
                m_IDOggetto1 = reader.Read("IDOggetto1", m_IDOggetto1);
                m_IDOggetto2 = reader.Read("IDOggetto2", m_IDOggetto2);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataCollegamento", m_DataCollegamento);
                writer.Write("DataScollegamento", m_DataScollegamento);
                writer.Write("Relazione", m_Relazione);
                writer.Write("IDOggetto1", IDOggetto1);
                writer.Write("IDOggetto2", IDOggetto2);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataCollegamento", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataScollegamento", typeof(DateTime), 1);
                c = table.Fields.Ensure("Relazione", typeof(int), 1);
                c = table.Fields.Ensure("IDOggetto1", typeof(int), 1);
                c = table.Fields.Ensure("IDOggetto2", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDate", new string[] { "DataCollegamento", "DataScollegamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRelazione", new string[] { "Relazione", "IDOggetto1", "IDOggetto2" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Link (", this.IDOggetto1, ", ", this.IDOggetto2, ")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDOggetto1);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is OggettoCollegato) && this.Equals((OggettoCollegato)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(OggettoCollegato obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataCollegamento, obj.m_DataCollegamento)
                    && DMD.DateUtils.EQ(this.m_DataScollegamento, obj.m_DataScollegamento)
                    && DMD.Integers.EQ((int)this.m_Relazione, (int)obj.m_Relazione)
                    && DMD.Integers.EQ(this.m_IDOggetto1, obj.m_IDOggetto1)
                    && DMD.Integers.EQ(this.m_IDOggetto2, obj.m_IDOggetto2)
                    ;
            }
        }
    }
}