using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum CQSPDTipoProvvigioneEnum : int
        {
            /// <summary>
        /// Provvigione fissa espressa nella valuta corrente (Fisso)
        /// </summary>
            NessunaPercentuale = 0,

            /// <summary>
        /// Fisso + Percentuale sul montante lordo
        /// </summary>
            PercentualeSuMontanteLordo = 1,

            /// <summary>
        /// Fisso + Percentuale sul delta montante
        /// </summary>
            PercentualeSuDeltaMontante = 2,

            /// <summary>
        /// Fisso + Valutazione della formula
        /// </summary>
            Formula = 3
        }

        [Flags]
        public enum CQSPDTipoSoggetto : int
        {
            Cessionario = 10,
            Agenzia = 20,
            Collaboratore = 30,
            Cliente = 40
        }

        [Flags]
        public enum CQSPDTipoProvvigioneFlags : int
        {
            None = 0,
            Nascosta = 1,
            IncludiNelTAN = 2,
            IncludiNelTAEG = 4,
            IncludiNelTEG = 8
        }

        [Serializable]
        public class CCQSPDTipoProvvigione 
            : Databases.DBObject, IComparable, ICloneable
        {
            private string m_Nome;
            private int m_IDGruppoProdotti;
            private CGruppoProdotti m_GruppoProdotti;
            private CQSPDTipoSoggetto m_PagataDa;
            private CQSPDTipoSoggetto m_PagataA;
            private CQSPDTipoProvvigioneEnum m_TipoCalcolo;
            private double? m_Percentuale;
            private decimal? m_Fisso;
            private decimal? m_ValoreMax;
            private string m_Formula;
            private CQSPDTipoProvvigioneFlags m_Flags;
            private CCollection<CTableConstraint> m_Vincoli;
            private CKeyCollection m_Parameters;

            public CCQSPDTipoProvvigione()
            {
                m_Nome = "";
                m_IDGruppoProdotti = 0;
                m_GruppoProdotti = null;
                m_PagataDa = CQSPDTipoSoggetto.Cessionario;
                m_PagataA = CQSPDTipoSoggetto.Agenzia;
                m_TipoCalcolo = CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo;
                m_Percentuale = default;
                m_Fisso = default;
                m_ValoreMax = default;
                m_Formula = "";
                m_Flags = CQSPDTipoProvvigioneFlags.None;
                m_Parameters = null;
                m_Vincoli = null;
            }

            /// <summary>
        /// Restituisce una collezione di vincoli che determinano l'applicabilità o meno di questo tipo di provvigione all'offerta:
        /// Affinché questo tipo di provvigione sia valido tutti i vincoli devono essere rispettati
        /// </summary>
        /// <returns></returns>
            public CCollection<CTableConstraint> Vincoli
            {
                get
                {
                    if (m_Vincoli is null)
                        m_Vincoli = new CCollection<CTableConstraint>();
                    return m_Vincoli;
                }
            }

            public bool RispettaVincoli(COffertaCQS offerta)
            {
                foreach (CTableConstraint v in Vincoli)
                {
                    if (!v.Check(offerta))
                        return false;
                }

                return true;
            }

            public decimal? ValoreMax
            {
                get
                {
                    return m_ValoreMax;
                }

                set
                {
                    var oldValue = m_ValoreMax;
                    if (value == oldValue )
                        return;
                    m_ValoreMax = value;
                    DoChanged("ValoreMax", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della provvigione
        /// </summary>
        /// <returns></returns>
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
        /// Restituisce o imposta l'ID del gruppo prodotti per cui è definita la provvigione
        /// </summary>
        /// <returns></returns>
            public int IDGruppoProdotti
            {
                get
                {
                    return DBUtils.GetID(m_GruppoProdotti, m_IDGruppoProdotti);
                }

                set
                {
                    int oldValue = IDGruppoProdotti;
                    if (oldValue == value)
                        return;
                    m_IDGruppoProdotti = value;
                    m_GruppoProdotti = null;
                    DoChanged("IDGruppoProdotti", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il gruppo prodotti per cui é definita la provvigione
        /// </summary>
        /// <returns></returns>
            public CGruppoProdotti GruppoProdotti
            {
                get
                {
                    if (m_GruppoProdotti is null)
                        m_GruppoProdotti = GruppiProdotto.GetItemById(m_IDGruppoProdotti);
                    return m_GruppoProdotti;
                }

                set
                {
                    var oldValue = GruppoProdotti;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_GruppoProdotti = value;
                    m_IDGruppoProdotti = DBUtils.GetID(value);
                    DoChanged("GruppoProdotti", value, oldValue);
                }
            }

            internal void SetGruppoProdotti(CGruppoProdotti value)
            {
                m_GruppoProdotti = value;
                m_IDGruppoProdotti = DBUtils.GetID(value);
            }

            /// <summary>
        /// Definisce il soggetto che eroga la provvigione
        /// </summary>
        /// <returns></returns>
            public CQSPDTipoSoggetto PagataDa
            {
                get
                {
                    return m_PagataDa;
                }

                set
                {
                    var oldValue = m_PagataDa;
                    if (oldValue == value)
                        return;
                    m_PagataDa = value;
                    DoChanged("PagataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Definisce il soggetto che riceve la provvigione
        /// </summary>
        /// <returns></returns>
            public CQSPDTipoSoggetto PagataA
            {
                get
                {
                    return m_PagataA;
                }

                set
                {
                    var oldValue = m_PagataA;
                    if (oldValue == value)
                        return;
                    m_PagataA = value;
                    DoChanged("PagataA", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo di calcolo usato per calcolare la provvigione
        /// </summary>
        /// <returns></returns>
            public CQSPDTipoProvvigioneEnum TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }

                set
                {
                    var oldValue = m_TipoCalcolo;
                    if (oldValue == value)
                        return;
                    m_TipoCalcolo = value;
                    DoChanged("TipoCalcolo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale calcolata secondo quanto definito in TipoCalcolo
        /// </summary>
        /// <returns></returns>
            public double? Percentuale
            {
                get
                {
                    return m_Percentuale;
                }

                set
                {
                    var oldValue = m_Percentuale;
                    if (oldValue == value == true)
                        return;
                    m_Percentuale = value;
                    DoChanged("Percentuale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore fisso aggiunto alla provvigione
        /// </summary>
        /// <returns></returns>
            public decimal? Fisso
            {
                get
                {
                    return m_Fisso;
                }

                set
                {
                    var oldValue = m_Fisso;
                    
                    if (oldValue == value)
                        return;

                    m_Fisso = value;
                    DoChanged("Fisso", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la formula usata nel caso TipoCalcolo sia impostato a Formula
        /// </summary>
        /// <returns></returns>
            public string Formula
            {
                get
                {
                    return m_Formula;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Formula;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Formula = value;
                    DoChanged("Formula", value, oldValue);
                }
            }

            public CQSPDTipoProvvigioneFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            public CKeyCollection Parameters
            {
                get
                {
                    if (m_Parameters is null)
                        m_Parameters = new CKeyCollection();
                    return m_Parameters;
                }
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDTipiProvvigione";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_IDGruppoProdotti = reader.Read("IDGruppoProdotti", this.m_IDGruppoProdotti);
                m_PagataDa = reader.Read("PagataDa", this.m_PagataDa);
                m_PagataA = reader.Read("PagataA", this.m_PagataA);
                m_TipoCalcolo = reader.Read("TipoCalcolo", this.m_TipoCalcolo);
                m_Percentuale = reader.Read("Percentuale", this.m_Percentuale);
                m_Fisso = reader.Read("Fisso", this.m_Fisso);
                m_Formula = reader.Read("Formula", this.m_Formula);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_ValoreMax = reader.Read("ValoreMax", this.m_ValoreMax);
                string tmp = reader.Read("Params", "");
                if (!string.IsNullOrEmpty(tmp))
                    m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

                tmp = reader.Read("Vincoli", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Vincoli = new CCollection<CTableConstraint>();
                    m_Vincoli.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                }

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IDGruppoProdotti", IDGruppoProdotti);
                writer.Write("PagataDa", m_PagataDa);
                writer.Write("PagataA", m_PagataA);
                writer.Write("TipoCalcolo", m_TipoCalcolo);
                writer.Write("Percentuale", m_Percentuale);
                writer.Write("Fisso", m_Fisso);
                writer.Write("Formula", m_Formula);
                writer.Write("Flags", m_Flags);
                writer.Write("Params", DMD.XML.Utils.Serializer.Serialize(Parameters));
                writer.Write("Vincoli", DMD.XML.Utils.Serializer.Serialize(Vincoli));
                writer.Write("ValoreMax", m_ValoreMax);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDGruppoProdotti", IDGruppoProdotti);
                writer.WriteAttribute("PagataDa", (int?)m_PagataDa);
                writer.WriteAttribute("PagataA", (int?)m_PagataA);
                writer.WriteAttribute("TipoCalcolo", (int?)m_TipoCalcolo);
                writer.WriteAttribute("Percentuale", m_Percentuale);
                writer.WriteAttribute("Fisso", m_Fisso);
                writer.WriteAttribute("Formula", m_Formula);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("ValoreMax", m_ValoreMax);
                base.XMLSerialize(writer);
                writer.WriteTag("Params", Parameters);
                writer.WriteTag("Vincoli", Vincoli);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDGruppoProdotti":
                        {
                            m_IDGruppoProdotti = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PagataDa":
                        {
                            m_PagataDa = (CQSPDTipoSoggetto)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PagataA":
                        {
                            m_PagataA = (CQSPDTipoSoggetto)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCalcolo":
                        {
                            m_TipoCalcolo = (CQSPDTipoProvvigioneEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Percentuale":
                        {
                            m_Percentuale = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Fisso":
                        {
                            m_Fisso = DMD.XML.Utils.Serializer.DeserializeValuta(fieldValue);
                            break;
                        }

                    case "Formula":
                        {
                            m_Formula = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (CQSPDTipoProvvigioneFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValoreMax":
                        {
                            m_ValoreMax = DMD.XML.Utils.Serializer.DeserializeValuta(fieldValue);
                            break;
                        }

                    case "Params":
                        {
                            m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "Vincoli":
                        {
                            m_Vincoli = new CCollection<CTableConstraint>();
                            m_Vincoli.AddRange((IEnumerable)DMD.XML.Utils.Serializer.ToObject(fieldValue));
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                return m_Nome;
            }

            public override void Save(bool force = false)
            {
                base.Save(force);
                GruppiProdotto.InvalidateTipiProvvigione();
            }

            public int CompareTo(CCQSPDTipoProvvigione obj)
            {
                return DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CCQSPDTipoProvvigione)obj);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}