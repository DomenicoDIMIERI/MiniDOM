using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Azioni
        /// </summary>
        public enum AzioneManutenzione : int
        {
            /// <summary>
            /// Nessuna
            /// </summary>
            Unknown = 0,
            None = 1,
            Installed = 2,
            Removed = 3,
            Repaired = 4,
            Upgraded = 5
        }

        /// <summary>
        /// Voci di manutenzione
        /// </summary>
        [Serializable]
        public class VoceManutenzione 
            : Databases.DBObjectPO
        {
            private int m_IDManutenzione;
            [NonSerialized] private CManutenzione m_Manutezione;
            private string m_Categoria1;
            private string m_Descrizione;
            private int m_IDOggettoRimosso;
            [NonSerialized] private object m_OggettoRimosso;
            private string m_NomeOggettoRimosso;
            private int m_IDOggetto;
            [NonSerialized] private object m_Oggetto;
            private string m_NomeOggetto;
            private decimal? m_ValoreImponibile;
            private decimal? m_ValoreIvato;
            private AzioneManutenzione m_Azione;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public VoceManutenzione()
            {
                m_IDManutenzione = 0;
                m_Manutezione = null;
                m_Categoria1 = "";
                m_Descrizione = "";
                m_IDOggettoRimosso = 0;
                m_OggettoRimosso = null;
                m_NomeOggettoRimosso = "";
                m_IDOggetto = 0;
                m_Oggetto = null;
                m_NomeOggetto = "";
                m_ValoreImponibile = default;
                m_ValoreIvato = default;
                m_Azione = AzioneManutenzione.Unknown;
                
                 
            }

            

            /// <summary>
            /// Azione
            /// </summary>
            public AzioneManutenzione Azione
            {
                get
                {
                    return m_Azione;
                }

                set
                {
                    var oldValue = m_Azione;
                    if (oldValue == value)
                        return;
                    m_Azione = value;
                    DoChanged("Azione", value, oldValue);
                }
            }
             

            /// <summary>
            /// IDManutenzione
            /// </summary>
            public int IDManutenzione
            {
                get
                {
                    return DBUtils.GetID(m_Manutezione, m_IDManutenzione);
                }

                set
                {
                    int oldValue = IDManutenzione;
                    if (oldValue == value)
                        return;
                    m_IDManutenzione = value;
                    m_Manutezione = null;
                    DoChanged("IDManutenzione", value, oldValue);
                }
            }

            /// <summary>
            /// Manutezione
            /// </summary>
            public CManutenzione Manutezione
            {
                get
                {
                    if (m_Manutezione is null)
                        m_Manutezione = Manutenzioni.GetItemById(m_IDManutenzione);
                    return m_Manutezione;
                }

                set
                {
                    var oldValue = m_Manutezione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Manutezione = value;
                    m_IDManutenzione = DBUtils.GetID(value, 0);
                    DoChanged("Manutezione", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la manutenzione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetManutenzione(CManutenzione value)
            {
                m_Manutezione = value;
                m_IDManutenzione = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Categoria1
            /// </summary>
            public string Categoria1
            {
                get
                {
                    return m_Categoria1;
                }

                set
                {
                    string oldValue = m_Categoria1;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria1 = value;
                    DoChanged("Categoria1", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
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
            /// IDOggettoRimosso
            /// </summary>
            public int IDOggettoRimosso
            {
                get
                {
                    return DBUtils.GetID(this.m_Oggetto, m_IDOggetto);
                }

                set
                {
                    int oldValue = IDOggettoRimosso;
                    if (oldValue == value)
                        return;
                    m_IDOggettoRimosso = value;
                    m_OggettoRimosso = null;
                    DoChanged("IDOggettoRimosso", value, oldValue);
                }
            }

            /// <summary>
            /// OggettoRimosso
            /// </summary>
            public object OggettoRimosso
            {
                get
                {
                    return m_OggettoRimosso;
                }

                set
                {
                    var oldValue = m_OggettoRimosso;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDOggettoRimosso = DBUtils.GetID(value, 0);
                    m_OggettoRimosso = value;
                    DoChanged("OggettoRimosso", value, oldValue);
                }
            }

            /// <summary>
            /// NomeOggettoRimosso
            /// </summary>
            public string NomeOggettoRimosso
            {
                get
                {
                    return m_NomeOggettoRimosso;
                }

                set
                {
                    string oldValue = m_NomeOggettoRimosso;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOggettoRimosso = value;
                    DoChanged("NomeOggettoRimosso", value, oldValue);
                }
            }

            /// <summary>
            /// IDOggetto
            /// </summary>
            public int IDOggetto
            {
                get
                {
                    return DBUtils.GetID(m_Oggetto, m_IDOggetto);
                }

                set
                {
                    int oldValue = IDOggetto;
                    if (oldValue == value)
                        return;
                    m_IDOggetto = value;
                    m_Oggetto = null;
                    DoChanged("IDOggetto", value, oldValue);
                }
            }

            /// <summary>
            /// Oggetto
            /// </summary>
            public object Oggetto
            {
                get
                {
                    return m_Oggetto;
                }

                set
                {
                    var oldValue = m_Oggetto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Oggetto = value;
                    m_IDOggetto = DBUtils.GetID(value, 0);
                    DoChanged("Oggetto", value, oldValue);
                }
            }

            /// <summary>
            /// NomeOggetto
            /// </summary>
            public string NomeOggetto
            {
                get
                {
                    return m_NomeOggetto;
                }

                set
                {
                    string oldValue = m_NomeOggetto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOggetto = value;
                    DoChanged("NomeOggetto", value, oldValue);
                }
            }

            /// <summary>
            /// ValoreImponibile
            /// </summary>
            public decimal? ValoreImponibile
            {
                get
                {
                    return m_ValoreImponibile;
                }

                set
                {
                    var oldValue = m_ValoreImponibile;
                    if (oldValue == value == true)
                        return;
                    m_ValoreImponibile = value;
                    DoChanged("ValoreImponibile", value, oldValue);
                }
            }

            /// <summary>
            /// ValoreIvato
            /// </summary>
            public decimal? ValoreIvato
            {
                get
                {
                    return m_ValoreIvato;
                }

                set
                {
                    var oldValue = m_ValoreIvato;
                    if (oldValue == value == true)
                        return;
                    m_ValoreIvato = value;
                    DoChanged("ValoreIvato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Descrizione;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Descrizione);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is VoceManutenzione) && this.Equals((VoceManutenzione)obj);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(VoceManutenzione obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.IDManutenzione, obj.IDManutenzione)
                    && DMD.Strings.EQ(this.m_Categoria1, obj.m_Categoria1)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Integers.EQ(this.IDOggettoRimosso, obj.IDOggettoRimosso)
                    && DMD.Strings.EQ(this.m_NomeOggettoRimosso, obj.m_NomeOggettoRimosso)
                    && DMD.Integers.EQ(this.IDOggetto, obj.IDOggetto)
                    && DMD.Strings.EQ(this.m_NomeOggetto, obj.m_NomeOggetto)
                    && DMD.Decimals.EQ(this.m_ValoreImponibile, obj.m_ValoreImponibile)
                    && DMD.Decimals.EQ(this.m_ValoreIvato, obj.m_ValoreIvato)
                    && DMD.Integers.EQ(this.m_Flags, obj.m_Flags)
                    && DMD.Integers.EQ((int)this.m_Azione, (int)obj.m_Azione)
                    ;
            //[NonSerialized] private AzioneManutenzione m_Azione;
            }

            /// <summary>
            /// Respository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Manutenzioni.Voci; //.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ManutenzioniVoci";
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
                m_IDManutenzione = reader.Read("IDManutenzione",  m_IDManutenzione);
                m_Categoria1 = reader.Read("Categoria1",  m_Categoria1);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_IDOggettoRimosso = reader.Read("IDOggettoRimosso",  m_IDOggettoRimosso);
                m_NomeOggettoRimosso = reader.Read("NomeOggettoRimosso",  m_NomeOggettoRimosso);
                m_IDOggetto = reader.Read("IDOggetto",  m_IDOggetto);
                m_NomeOggetto = reader.Read("NomeOggetto",  m_NomeOggetto);
                m_ValoreImponibile = reader.Read("ValoreImponibile",  m_ValoreImponibile);
                m_ValoreIvato = reader.Read("ValoreIvato",  m_ValoreIvato);
                m_Azione = reader.Read("Azione", m_Azione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDManutenzione", IDManutenzione);
                writer.Write("Categoria1", m_Categoria1);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDOggettoRimosso", IDOggettoRimosso);
                writer.Write("NomeOggettoRimosso", m_NomeOggettoRimosso);
                writer.Write("IDOggetto", IDOggetto);
                writer.Write("NomeOggetto", m_NomeOggetto);
                writer.Write("ValoreImponibile", m_ValoreImponibile);
                writer.Write("ValoreIvato", m_ValoreIvato);
                writer.Write("Azione", m_Azione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDManutenzione", typeof(int), 1);
                c = table.Fields.Ensure("Categoria1", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("IDOggettoRimosso", typeof(int), 1);
                c = table.Fields.Ensure("NomeOggettoRimosso", typeof(string), 255);
                c = table.Fields.Ensure("IDOggetto", typeof(int), 1);
                c = table.Fields.Ensure("NomeOggetto", typeof(string), 255);
                c = table.Fields.Ensure("ValoreImponibile", typeof(decimal), 1);
                c = table.Fields.Ensure("ValoreIvato", typeof(decimal), 1);
                c = table.Fields.Ensure("Azione", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxManutenzione", new string[] { "IDManutenzione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOggettoRimosso", new string[] { "IDOggettoRimosso", "NomeOggettoRimosso" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOggetto", new string[] { "IDOggetto", "NomeOggetto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValore", new string[] { "ValoreImponibile", "ValoreIvato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAzione", new string[] { "Azione", "Flags" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Descrizione", typeof(string), 0);
            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDManutenzione", IDManutenzione);
                writer.WriteAttribute("Categoria1", m_Categoria1);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("IDOggettoRimosso", IDOggettoRimosso);
                writer.WriteAttribute("NomeOggettoRimosso", m_NomeOggettoRimosso);
                writer.WriteAttribute("IDOggetto", IDOggetto);
                writer.WriteAttribute("NomeOggetto", m_NomeOggetto);
                writer.WriteAttribute("ValoreImponibile", m_ValoreImponibile);
                writer.WriteAttribute("ValoreIvato", m_ValoreIvato);
                writer.WriteAttribute("Azione", (int?)m_Azione);
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
                    case "IDManutenzione":
                        {
                            m_IDManutenzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Categoria1":
                        {
                            m_Categoria1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOggettoRimosso":
                        {
                            m_IDOggettoRimosso = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOggettoRimosso":
                        {
                            m_NomeOggettoRimosso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDOggetto":
                        {
                            m_IDOggetto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOggetto":
                        {
                            m_NomeOggetto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreImponibile":
                        {
                            m_ValoreImponibile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreIvato":
                        {
                            m_ValoreIvato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Azione":
                        {
                            m_Azione = (AzioneManutenzione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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