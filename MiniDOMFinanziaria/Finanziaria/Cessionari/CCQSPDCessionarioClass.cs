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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Flag validi per un cessionario
        /// </summary>
        [Flags]
        public enum CessionarioFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Se vero il cessionario viene mostrato tra quelli disponibili in credito V
            /// </summary>
            /// <remarks></remarks>
            UsabileInPratiche = 1,

            /// <summary>
            /// Se vero indica che le provvigioni agenzia sono ricavate come differenza tra la provvigione massima
            /// caricabile per il prodotto e lo sconto applicato al cliente
            /// </summary>
            /// <remarks></remarks>
            SistemaProvvigioniScontate = 2,

            /// <summary>
            /// Questo flag indica che il cessionario chiama la provvigione agenzia UpFront
            /// </summary>
            /// <remarks></remarks>
            UpFront = 4,

            /// <summary>
            /// Questo flag indica che il cessionario utilizza il campo Running
            /// </summary>
            /// <remarks></remarks>
            Running = 8
        }

        /// <summary>
        /// Rappresenta un istituto cessionario
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCQSPDCessionarioClass
            : Databases.DBObject, IComparable, IComparable<CCQSPDCessionarioClass>
        {
            private string m_Nome;
            private string m_ImagePath;    // Percorso dell'immagine
            private bool m_Visibile; // Se vero il cessionario viene mostrato negli elenchi 
            private string m_Preventivatore; // Percorso dello script di backdoor alla pagina di preventivazione
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CCQSPDCessionarioClass()
            {
                m_Nome = "";
                m_ImagePath = "";
                m_Visibile = true;
                m_Preventivatore = "";
                m_DataInizio = default;
                m_DataFine = default;
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
                return (obj is CCQSPDCessionarioClass) && this.Equals((CCQSPDCessionarioClass)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CCQSPDCessionarioClass obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_ImagePath, obj.m_ImagePath)
                     && DMD.Booleans.EQ(this.m_Visibile, obj.m_Visibile)
                     && DMD.Strings.EQ(this.m_Preventivatore, obj.m_Preventivatore)
                     && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                     && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    ;

            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Cessionari;
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se le provvigioni agenzia sono calcolate
            /// come differenza tra la provvigione massima caricabile per il prodotto e lo sconto applicato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool SistemaProvvigioniScontate
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, CessionarioFlags.SistemaProvvigioniScontate);
                }

                set
                {
                    if (SistemaProvvigioniScontate == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, CessionarioFlags.SistemaProvvigioniScontate, value);
                    DoChanged("SistemaProvvigioniScontate", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta i flags impostati per il cessionario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new CessionarioFlags Flags
            {
                get
                {
                    return (CessionarioFlags) base.Flags;
                }

                set
                {
                    var oldValue = (CessionarioFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int) value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il cessionario è utilizzabile nel modulo Pratiche
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool UsabileInPratiche
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, CessionarioFlags.UsabileInPratiche);
                }

                set
                {
                    if (UsabileInPratiche == value)
                        return;
                    m_Flags = (int) DMD.RunTime.SetFlag(this.Flags, CessionarioFlags.UsabileInPratiche, value);
                    DoChanged("UsabileInPratiche", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del cessionario (deve essere univoco)
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
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se il cessionario é elencabile
            /// </summary>
            public bool Visibile
            {
                get
                {
                    return m_Visibile;
                }

                set
                {
                    if (m_Visibile == value)
                        return;
                    m_Visibile = value;
                    DoChanged("Visibile", value, !value);
                }
            }

            /// <summary>
            /// Restituisce la url del webservice che implementa il preventivatore
            /// </summary>
            public string Preventivatore
            {
                get
                {
                    return m_Preventivatore;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Preventivatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Preventivatore = value;
                    DoChanged("Preventivatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la url di un'immagine
            /// </summary>
            public string ImagePath
            {
                get
                {
                    return m_ImagePath;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_ImagePath;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ImagePath = value;
                    DoChanged("ImagePath", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio rapporto
            /// </summary>
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
            /// Restituisce o imposta la data di fine rapporto
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
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce un array contenente tutti i prodotti validi 
            /// </summary>
            /// <returns></returns>
            public CCQSPDProdotto[] GetArrayProdottiValidi()
            {
                var ret = new List<CCQSPDProdotto>();

                foreach (var p in minidom.Finanziaria.Prodotti)
                {
                    if (
                         (p.Stato == ObjectStatus.OBJECT_VALID)
                        && p.CessionarioID == DBUtils.GetID(this, 0)
                        && p.IsValid()
                        )
                    {
                        ret.Add(p);
                    }
                }

                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce l'array dei gruppi prodotto validi
            /// </summary>
            /// <returns></returns>

            public CGruppoProdotti[] GetArrayGruppoProdottiValidi()
            {
                var ret = new List<CGruppoProdotti>();
                foreach(var g in minidom.Finanziaria.GruppiProdotto)
                { 
                    if (
                           (g.Stato == ObjectStatus.OBJECT_VALID)
                        && g.CessionarioID == DBUtils.GetID(this, 0)
                        && g.IsValid()
                        )
                    {
                        ret.Add(g);
                    }
                }

                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce true se l'oggetto è valido alla data corrente
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Restituisce true se l'oggetto è valido alla data specificata
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            public bool IsValid(DateTime? d)
            {
                return DMD.DateUtils.CheckBetween(d, m_DataInizio, m_DataFine);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public int CompareTo(CCQSPDCessionarioClass item)
            {
                int ret = DMD.Strings.Compare(Nome, item.Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CCQSPDCessionarioClass)obj);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Cessionari";
            }
 
            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_Visibile = reader.Read("Visibile", this.m_Visibile);
                m_Preventivatore = reader.Read("Preventivatore", this.m_Preventivatore);
                m_ImagePath = reader.Read("ImagePath", this.m_ImagePath);
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
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
                writer.Write("Visibile", m_Visibile);
                writer.Write("Preventivatore", m_Preventivatore);
                writer.Write("ImagePath", m_ImagePath);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
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
                c = table.Fields.Ensure("Visibile", typeof(bool), 1);
                c = table.Fields.Ensure("Preventivatore", typeof(string), 255);
                c = table.Fields.Ensure("ImagePath", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "Preventivatore", "ImagePath", "Visibile" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("ImagePath", m_ImagePath);
                writer.WriteAttribute("Visibile", m_Visibile);
                writer.WriteAttribute("Preventivatore", m_Preventivatore);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
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

                    case "ImagePath":
                        {
                            m_ImagePath = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Visibile":
                        {
                            m_Visibile = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Preventivatore":
                        {
                            m_Preventivatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            public CCQSPDCessionarioClass Clone()
            {
                return (CCQSPDCessionarioClass)base._Clone();
            }
        }
    }
}