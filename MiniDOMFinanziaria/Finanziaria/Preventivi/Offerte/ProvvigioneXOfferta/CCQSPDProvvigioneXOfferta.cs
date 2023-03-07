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
        [Flags]
        public enum ProvvigioneXOffertaFlags : int
        {
            None = 0,

            /// <summary>
        /// La provvigione é visibile solo agli utenti privileggiati
        /// </summary>
            Privileged = 1
        }

        [Serializable]
        public class CCQSPDProvvigioneXOfferta
            : Databases.DBObject, IComparable
        {
            private string m_Nome;
            private int m_IDOfferta;
            private COffertaCQS m_Offerta;
            private int m_IDTipoProvvigione;
            private CCQSPDTipoProvvigione m_TipoProvvigione;
            private int m_IDCedente;
            private Anagrafica.CPersona m_Cedente;
            private string m_NomeCedente;
            private int m_IDRicevente;
            private Anagrafica.CPersona m_Ricevente;
            private string m_NomeRicevente;
            private decimal? m_BaseDiCalcolo;
            private decimal? m_Valore;
            private decimal? m_ValorePagato;
            private DateTime? m_DataPagamento;
            private ProvvigioneXOffertaFlags m_Flags;
            private CKeyCollection m_Parameters;
            private CQSPDTipoProvvigioneEnum m_TipoCalcolo;
            private CQSPDTipoSoggetto m_PagataDa;
            private CQSPDTipoSoggetto m_PagataA;
            private double? m_Percentuale;
            private decimal? m_Fisso;
            private string m_Formula;
            private CCollection<CTableConstraint> m_Vincoli;
            private int m_IDTrattativaCollaboratore;
            private CTrattativaCollaboratore m_TrattativaCollaboratore;
            private int m_IDCollaboratore;
            private CCollaboratore m_Collaboratore;
            private string m_NomeCollaboratore;

            public CCQSPDProvvigioneXOfferta()
            {
                m_Nome = "";
                m_IDOfferta = 0;
                m_Offerta = null;
                m_IDTipoProvvigione = 0;
                m_TipoProvvigione = null;
                m_IDCedente = 0;
                m_Cedente = null;
                m_NomeCedente = "";
                m_IDRicevente = 0;
                m_Ricevente = null;
                m_NomeRicevente = "";
                m_BaseDiCalcolo = default;
                m_Valore = default;
                m_ValorePagato = default;
                m_DataPagamento = default;
                m_Flags = ProvvigioneXOffertaFlags.None;
                m_Parameters = null;
                m_TipoCalcolo = CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo;
                m_PagataDa = CQSPDTipoSoggetto.Cessionario;
                m_PagataA = CQSPDTipoSoggetto.Agenzia;
                m_Percentuale = default;
                m_Fisso = default;
                m_Formula = "";
                m_Vincoli = null;
                m_IDTrattativaCollaboratore = 0;
                m_TrattativaCollaboratore = null;
                m_IDCollaboratore = 0;
                m_Collaboratore = null;
                m_NomeCollaboratore = "";
            }

            public CCQSPDProvvigioneXOfferta Duplicate()
            {
                CCQSPDProvvigioneXOfferta ret = (CCQSPDProvvigioneXOfferta)MemberwiseClone();
                DBUtils.SetID(ret, 0);
                return ret;
            }

            /// <summary>
        /// Restituisce o imposta l'ID della trattativa con il collaboratore
        /// </summary>
        /// <returns></returns>
            public int IDTrattativaCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_TrattativaCollaboratore, m_IDTrattativaCollaboratore);
                }

                set
                {
                    int oldValue = IDTrattativaCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDTrattativaCollaboratore = value;
                    m_TrattativaCollaboratore = null;
                    DoChanged("IDTrattativaCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la trattativa con il collaboratore
        /// </summary>
        /// <returns></returns>
            public CTrattativaCollaboratore TrattativaCollaboratore
            {
                get
                {
                    if (m_TrattativaCollaboratore is null)
                        m_TrattativaCollaboratore = TrattativeCollaboratore.GetItemById(m_IDTrattativaCollaboratore);
                    return m_TrattativaCollaboratore;
                }

                set
                {
                    var oldValue = m_TrattativaCollaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TrattativaCollaboratore = value;
                    m_IDTrattativaCollaboratore = DBUtils.GetID(value);
                    DoChanged("TrattativaCollaboratore", value, oldValue);
                }
            }

            protected internal void SetTrattativaCollaboratore(CTrattativaCollaboratore value)
            {
                m_TrattativaCollaboratore = value;
                m_IDTrattativaCollaboratore = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta l'ID del collaboratore
        /// </summary>
        /// <returns></returns>
            public int IDCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_Collaboratore, m_IDCollaboratore);
                }

                set
                {
                    int oldValue = IDCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDCollaboratore = value;
                    m_Collaboratore = null;
                    DoChanged("IDCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il collaboratore
        /// </summary>
        /// <returns></returns>
            public CCollaboratore Collaboratore
            {
                get
                {
                    if (m_Collaboratore is null)
                        m_Collaboratore = Collaboratori.GetItemById(m_IDCollaboratore);
                    return m_Collaboratore;
                }

                set
                {
                    var oldValue = Collaboratore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Collaboratore = value;
                    m_IDCollaboratore = DBUtils.GetID(value);
                    m_NomeCollaboratore = "";
                    if (value is object)
                        m_NomeCollaboratore = value.NomePersona;
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            protected internal void SetCollaboratore(CCollaboratore value)
            {
                m_Collaboratore = value;
                m_IDCollaboratore = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il nome del collaboratore
        /// </summary>
        /// <returns></returns>
            public string NomeCollaboratore
            {
                get
                {
                    return m_NomeCollaboratore;
                }

                set
                {
                    string oldValue = m_NomeCollaboratore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCollaboratore = value;
                    DoChanged("NomeCollaboratore", value, oldValue);
                }
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
                    if (oldValue == value == true)
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
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            public int IDOfferta
            {
                get
                {
                    return DBUtils.GetID(m_Offerta, m_IDOfferta);
                }

                set
                {
                    int oldValue = IDOfferta;
                    if (oldValue == value)
                        return;
                    m_IDOfferta = value;
                    m_Offerta = null;
                    DoChanged("IDOfferta", value, oldValue);
                }
            }

            public COffertaCQS Offerta
            {
                get
                {
                    if (m_Offerta is null)
                        m_Offerta = Offerte.GetItemById(m_IDOfferta);
                    return m_Offerta;
                }

                set
                {
                    var oldValue = m_Offerta;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Offerta = value;
                    m_IDOfferta = DBUtils.GetID(value);
                    DoChanged("Offerta", value, oldValue);
                }
            }

            internal void SetOfferta(COffertaCQS value)
            {
                m_Offerta = value;
                m_IDOfferta = DBUtils.GetID(value);
            }

            public int IDTipoProvvigione
            {
                get
                {
                    return DBUtils.GetID(m_TipoProvvigione, m_IDTipoProvvigione);
                }

                set
                {
                    int oldValue = IDTipoProvvigione;
                    if (oldValue == value)
                        return;
                    m_IDTipoProvvigione = value;
                    m_TipoProvvigione = null;
                    DoChanged("IDTipoProvvigione", value, oldValue);
                }
            }

            public CCQSPDTipoProvvigione TipoProvvigione
            {
                get
                {
                    return m_TipoProvvigione;
                }

                set
                {
                    var oldValue = m_TipoProvvigione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TipoProvvigione = value;
                    m_IDTipoProvvigione = DBUtils.GetID(value);
                    DoChanged("TipoProvvigione", value, oldValue);
                }
            }

            public ProvvigioneXOffertaFlags Flags
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

            public int IDCedente
            {
                get
                {
                    return DBUtils.GetID(m_Cedente, m_IDCedente);
                }

                set
                {
                    int oldValue = IDCedente;
                    if (oldValue == value)
                        return;
                    m_IDCedente = value;
                    m_Cedente = null;
                    DoChanged("IDCedente", value, oldValue);
                }
            }

            public Anagrafica.CPersona Cedente
            {
                get
                {
                    if (m_Cedente is null)
                        m_Cedente = Anagrafica.Persone.GetItemById(m_IDCedente);
                    return m_Cedente;
                }

                set
                {
                    var oldValue = m_Cedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cedente = value;
                    m_IDCedente = DBUtils.GetID(value);
                    m_NomeCedente = "";
                    if (value is object)
                        m_NomeCedente = value.Nominativo;
                    DoChanged("Cedente", value, oldValue);
                }
            }

            public string NomeCedente
            {
                get
                {
                    return m_NomeCedente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCedente = value;
                    DoChanged("NomeCedente", value, oldValue);
                }
            }

            public int IDRicevente
            {
                get
                {
                    return DBUtils.GetID(m_Ricevente, m_IDRicevente);
                }

                set
                {
                    int oldValue = IDRicevente;
                    if (oldValue == value)
                        return;
                    m_IDRicevente = value;
                    m_Ricevente = null;
                    DoChanged("IDRicevente", value, oldValue);
                }
            }

            public Anagrafica.CPersona Ricevente
            {
                get
                {
                    if (m_Ricevente is null)
                        m_Ricevente = Anagrafica.Persone.GetItemById(m_IDRicevente);
                    return m_Ricevente;
                }

                set
                {
                    var oldValue = m_Ricevente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Ricevente = value;
                    m_IDRicevente = DBUtils.GetID(value);
                    m_NomeRicevente = "";
                    if (value is object)
                        m_NomeRicevente = value.Nominativo;
                    DoChanged("Ricevente", value, oldValue);
                }
            }

            public string NomeRicevente
            {
                get
                {
                    return m_NomeRicevente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRicevente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRicevente = value;
                    DoChanged("NomeRicevente", value, oldValue);
                }
            }

            public decimal? BaseDiCalcolo
            {
                get
                {
                    return m_BaseDiCalcolo;
                }

                set
                {
                    var oldValue = m_BaseDiCalcolo;
                    if (oldValue == value == true)
                        return;
                    m_BaseDiCalcolo = value;
                    DoChanged("BaseDiCalcolo", value, oldValue);
                }
            }

            public decimal? Valore
            {
                get
                {
                    return m_Valore;
                }

                set
                {
                    var oldValue = m_Valore;
                    if (oldValue == value == true)
                        return;
                    m_Valore = value;
                    DoChanged("Valore", value, oldValue);
                }
            }

            public decimal? ValorePagato
            {
                get
                {
                    return m_ValorePagato;
                }

                set
                {
                    var oldValue = m_ValorePagato;
                    if (oldValue == value == true)
                        return;
                    m_ValorePagato = value;
                    DoChanged("ValorePagato", value, oldValue);
                }
            }

            public DateTime? DataPagamento
            {
                get
                {
                    return m_DataPagamento;
                }

                set
                {
                    var oldValue = m_DataPagamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPagamento = value;
                    DoChanged("DataPagamento", value, oldValue);
                }
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDProvvXOfferta";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_IDOfferta = reader.Read("IDOfferta", this.m_IDOfferta);
                m_IDTipoProvvigione = reader.Read("IDTipoProvvigione", this.m_IDTipoProvvigione);
                m_IDCedente = reader.Read("IDCedente", this.m_IDCedente);
                m_NomeCedente = reader.Read("NomeCedente", this.m_NomeCedente);
                m_IDRicevente = reader.Read("IDRicevente", this.m_IDRicevente);
                m_NomeRicevente = reader.Read("NomeRicevente", this.m_NomeRicevente);
                m_BaseDiCalcolo = reader.Read("BaseDiCalcolo", this.m_BaseDiCalcolo);
                m_Valore = reader.Read("Valore", this.m_Valore);
                m_ValorePagato = reader.Read("ValorePagato", this.m_ValorePagato);
                m_DataPagamento = reader.Read("DataPagamento", this.m_DataPagamento);
                m_PagataDa = reader.Read("PagataDa", this.m_PagataDa);
                m_PagataA = reader.Read("PagataA", this.m_PagataA);
                m_TipoCalcolo = reader.Read("TipoCalcolo", this.m_TipoCalcolo);
                m_Percentuale = reader.Read("Percentuale", this.m_Percentuale);
                m_Fisso = reader.Read("Fisso", this.m_Fisso);
                m_Formula = reader.Read("Formula", this.m_Formula);
                m_Flags = reader.Read("Flags", this.m_Flags);
                string tmp = reader.Read("Params", "");
                if (!string.IsNullOrEmpty(tmp))
                    m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

                tmp = reader.Read("Vincoli", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Vincoli = new CCollection<CTableConstraint>();
                    m_Vincoli.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
                }

                m_IDTrattativaCollaboratore = reader.Read("IDTrattativaCollaboratore", this.m_IDTrattativaCollaboratore);
                m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                m_NomeCollaboratore = reader.Read("NomeCollaboratore", this.m_NomeCollaboratore);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("IDOfferta", IDOfferta);
                writer.Write("IDTipoProvvigione", IDTipoProvvigione);
                writer.Write("IDCedente", IDCedente);
                writer.Write("NomeCedente", m_NomeCedente);
                writer.Write("IDRicevente", IDRicevente);
                writer.Write("NomeRicevente", m_NomeRicevente);
                writer.Write("BaseDiCalcolo", m_BaseDiCalcolo);
                writer.Write("Valore", m_Valore);
                writer.Write("ValorePagato", m_ValorePagato);
                writer.Write("DataPagamento", m_DataPagamento);
                writer.Write("PagataDa", m_PagataDa);
                writer.Write("PagataA", m_PagataA);
                writer.Write("TipoCalcolo", m_TipoCalcolo);
                writer.Write("Percentuale", m_Percentuale);
                writer.Write("Fisso", m_Fisso);
                writer.Write("Formula", m_Formula);
                writer.Write("Flags", m_Flags);
                writer.Write("Params", DMD.XML.Utils.Serializer.Serialize(Parameters));
                writer.Write("Vincoli", DMD.XML.Utils.Serializer.Serialize(Vincoli));
                writer.Write("IDTrattativaCollaboratore", IDTrattativaCollaboratore);
                writer.Write("IDCollaboratore", IDCollaboratore);
                writer.Write("NomeCollaboratore", m_NomeCollaboratore);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDOfferta", IDOfferta);
                writer.WriteAttribute("IDTipoProvvigione", IDTipoProvvigione);
                writer.WriteAttribute("IDCedente", IDCedente);
                writer.WriteAttribute("NomeCedente", m_NomeCedente);
                writer.WriteAttribute("IDRicevente", IDRicevente);
                writer.WriteAttribute("NomeRicevente", m_NomeRicevente);
                writer.WriteAttribute("BaseDiCalcolo", m_BaseDiCalcolo);
                writer.WriteAttribute("Valore", m_Valore);
                writer.WriteAttribute("ValorePagato", m_ValorePagato);
                writer.WriteAttribute("DataPagamento", m_DataPagamento);
                writer.WriteAttribute("PagataDa", (int?)m_PagataDa);
                writer.WriteAttribute("PagataA", (int?)m_PagataA);
                writer.WriteAttribute("TipoCalcolo", (int?)m_TipoCalcolo);
                writer.WriteAttribute("Percentuale", m_Percentuale);
                writer.WriteAttribute("Fisso", m_Fisso);
                writer.WriteAttribute("Formula", m_Formula);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("IDTrattativaCollaboratore", IDTrattativaCollaboratore);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteAttribute("NomeCollaboratore", m_NomeCollaboratore);
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

                    case "IDOfferta":
                        {
                            m_IDOfferta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTipoProvvigione":
                        {
                            m_IDTipoProvvigione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCedente":
                        {
                            m_IDCedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCedente":
                        {
                            m_NomeCedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRicevente":
                        {
                            m_IDRicevente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRicevente":
                        {
                            m_NomeRicevente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            m_Valore = DMD.XML.Utils.Serializer.DeserializeValuta(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "BaseDiCalcolo":
                        {
                            m_BaseDiCalcolo = DMD.XML.Utils.Serializer.DeserializeValuta(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValorePagato":
                        {
                            m_ValorePagato = DMD.XML.Utils.Serializer.DeserializeValuta(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataPagamento":
                        {
                            m_DataPagamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
                            m_Fisso = DMD.XML.Utils.Serializer.DeserializeValuta(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Formula":
                        {
                            m_Formula = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (ProvvigioneXOffertaFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "IDTrattativaCollaboratore":
                        {
                            m_IDTrattativaCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCollaboratore":
                        {
                            m_NomeCollaboratore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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

            public int CompareTo(CCQSPDProvvigioneXOfferta obj)
            {
                return DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CCQSPDProvvigioneXOfferta)obj);
            }

            public void Aggiorna(COffertaCQS offerta, CCollection<EstinzioneXEstintore> estinzioni)
            {
                CCQSPDProdotto p;
                object o;
                p = offerta.Prodotto;
                if (IDTipoProvvigione != 0)
                {
                    CGruppoProdotti gp = null;
                    CCQSPDTipoProvvigione tp = null;
                    if (p is object)
                        gp = p.GruppoProdotti;
                    if (gp is null)
                        return;
                    var items = gp.Provvigioni;
                    // tp = items.GetItemById(this.getIDTipoProvvigione());
                    if (tp is null)
                        tp = items.GetItemByName(Nome);
                    if (tp is null)
                        return;
                    switch (TipoCalcolo)
                    {
                        case CQSPDTipoProvvigioneEnum.NessunaPercentuale:
                            {
                                m_Valore = tp.Fisso;
                                break;
                            }

                        case CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo:
                            {
                                m_Valore = tp.Fisso + (decimal)((double)offerta.MontanteLordo * (double)tp.Percentuale.Value / 100);
                                break;
                            }

                        case CQSPDTipoProvvigioneEnum.PercentualeSuDeltaMontante:
                            {
                                m_Valore = tp.Fisso + (decimal)((double)offerta.CalcolaBaseML(estinzioni) * (double)tp.Percentuale / 100);
                                break;
                            }

                        case CQSPDTipoProvvigioneEnum.Formula:
                            {
                                o = Sistema.Types.CreateInstance(tp.Formula);
                                if (o is minidom.Finanziaria.CCQSPDProvvigioneXOffertaEvaluator)
                                m_Valore = (decimal?)(o as minidom.Finanziaria.CCQSPDProvvigioneXOffertaEvaluator).Evaluate(this, offerta, estinzioni);
                                break;
                            }
                    }

                    if (tp.ValoreMax.HasValue && m_Valore.HasValue)
                    {
                        m_Valore = Maths.Min(m_Valore.Value, tp.ValoreMax);
                    }
                }
                else if (DMD.Booleans.ValueOf(IDTrattativaCollaboratore))
                {
                    var col = Collaboratore;
                    CTrattativaCollaboratore tp = null;
                    if (col is null)
                        return;
                    var items = col.Trattative;
                    // tp = items.GetItemById(this.getIDTrattativaCollaboratore());
                    if (tp is null)
                        tp = items.GetItemByNameAndProdotto(Nome, p, Sistema.TestFlag(offerta.Flags, OffertaFlags.DirettaCollaboratore));
                    if (tp is null)
                        return;
                    switch (TipoCalcolo)
                    {
                        case CQSPDTipoProvvigioneEnum.NessunaPercentuale:
                            {
                                if (tp.ValoreBase.HasValue)
                                    m_Valore = (decimal?)tp.ValoreBase;
                                break;
                            }

                        case CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo:
                            {
                                if (tp.ValoreBase.HasValue)
                                    m_Valore = (decimal?)tp.ValoreBase;
                                if (tp.SpreadApprovato.HasValue)
                                    m_Valore += (decimal?) ((double)offerta.MontanteLordo * (double)tp.SpreadApprovato / 100);
                                break;
                            }

                        case CQSPDTipoProvvigioneEnum.PercentualeSuDeltaMontante:
                            {
                                if (tp.ValoreBase.HasValue)
                                    m_Valore = (decimal?)tp.ValoreBase;
                                if (tp.SpreadApprovato.HasValue)
                                    m_Valore += (decimal?) ((double)offerta.CalcolaBaseML(estinzioni) * (double)tp.SpreadApprovato / 100);
                                break;
                            }

                        case CQSPDTipoProvvigioneEnum.Formula:
                            {
                                o = Sistema.Types.CreateInstance(tp.Formula);
                                if (o is CCQSPDProvvigioneXOffertaEvaluator)
                                    m_Valore = (decimal?)(o as CCQSPDProvvigioneXOffertaEvaluator).Evaluate(this, offerta, estinzioni);
                                break;
                            }
                    }

                    if (tp.ValoreMax.HasValue && m_Valore.HasValue)
                    {
                        m_Valore = Maths.Min(m_Valore.Value, (decimal)tp.ValoreMax.Value);
                    }
                }

                Save(true);
            }
        }
    }
}