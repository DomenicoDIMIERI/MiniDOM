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
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Stato di una chiamata
        /// </summary>
        public enum StatoChiamataRegistrata : int
        {
            /// <summary>
            /// Stato sconosciuto
            /// </summary>
            Sconosciuto = 0,

            /// <summary>
            /// La chiamata è in fase di composizione
            /// </summary>
            Composizione = 1,

            /// <summary>
            /// La chiamata è in corso (la controparte ha risposto)
            /// </summary>
            InCorso = 2,

            /// <summary>
            /// La chiamata è terminata senza che la controparte abbia risposto
            /// </summary>
            /// <remarks></remarks>
            NonRisposto = 3,

            /// <summary>
            /// Il chiamante ha agganciato
            /// </summary>
            /// <remarks></remarks>
            AgganciatoChiamante = 4,

            /// <summary>
            /// Il chiamato ha agganciato
            /// </summary>
            AgganciatoChiamato = 5,

            /// <summary>
            /// Il chiamato ha rifiutato la chiamata
            /// </summary>
            Rifiutata = 6,


            /// <summary>
            /// La chiamata non è stata effettuata a causa di un errore
            /// </summary>
            /// <remarks></remarks>
            Errore = 255
        }

        /// <summary>
        /// Esito di una chiamata
        /// </summary>
        public enum EsitoChiamataRegistrata : int
        {
            /// <summary>
            /// Non risposto
            /// </summary>
            NonRisposto = 0,

            /// <summary>
            /// Risposto
            /// </summary>
            Risposto = 1
        }

        /// <summary>
        /// Qualità della chiamata
        /// </summary>
        public enum QualitaChiamata : int
        {
            /// <summary>
            /// Ottima
            /// </summary>
            Ottima = -100,

            /// <summary>
            /// Buona
            /// </summary>
            Buona = -50,

            /// <summary>
            /// Normale
            /// </summary>
            Normale = 0,

            /// <summary>
            /// Scarsa
            /// </summary>
            Scarsa = 50,

            /// <summary>
            /// Pessima
            /// </summary>
            Pessima = 100
        }

        /// <summary>
        /// Rappresenta una chiamata registrata
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ChiamataRegistrata 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<ChiamataRegistrata>
        {
            private string m_IDChiamata;          // ID della chiamata (identifica la chiamata nel contesto del centralino)
            private StatoChiamataRegistrata m_StatoChiamata; // Stato della chiamata
            private string m_EsitoChiamataEx;
            private EsitoChiamataRegistrata m_EsitoChiamata; // Esito della chiamata
            private string m_StatoChiamataEx;
            private DateTime? m_DataInizio;           // Data e ora di inizio della chiamata (composizione)
            private DateTime? m_DataRisposta;         // In caso di risposta data e ora della risposta della controparte (o di un centralino/voce automatica)
            private DateTime? m_DataFine;             // Data e ora di fine della chiamata
            private int m_IDPBX;              // ID del centralino che ha gestito la chiamata
            [NonSerialized] private PBX m_PBX;                 // Centralino che ha gestito la chiamata
            private string m_NomePBX;             // Nome del centralino che ha gestito la chiamata
            private int m_IDChiamante;
            [NonSerialized] private CPersona m_Chiamante;
            private string m_NomeChiamante;
            private int m_IDChiamato;
            [NonSerialized] private CPersona m_Chiamato;
            private string m_NomeChiamato;
            private string m_DaNumero;
            private string m_ANumero;
            private string m_NomeCanale;
            private QualitaChiamata m_Qualita;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ChiamataRegistrata()
            {
                m_IDChiamata = "";
                m_StatoChiamata = StatoChiamataRegistrata.Sconosciuto;
                m_EsitoChiamataEx = "";
                m_EsitoChiamata = EsitoChiamataRegistrata.NonRisposto;
                m_StatoChiamataEx = "";
                m_DataInizio = default;
                m_DataRisposta = default;
                m_DataFine = default;
                m_IDPBX = 0;
                m_PBX = null;
                m_NomePBX = "";
                m_IDChiamante = 0;
                m_Chiamante = null;
                m_NomeChiamante = "";
                m_IDChiamato = 0;
                m_Chiamato = null;
                m_NomeChiamato = "";
                m_DaNumero = "";
                m_ANumero = "";
                m_NomeCanale = "";
                m_Qualita = QualitaChiamata.Normale;
            }

            /// <summary>
            /// ID della chiamata nel CRM
            /// </summary>
            public string IDChiamata
            {
                get
                {
                    return m_IDChiamata;
                }

                set
                {
                    string oldValue = m_IDChiamata;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IDChiamata = value;
                    DoChanged("IDChiamata", value, oldValue);
                }
            }

            /// <summary>
            /// Stato della chiamata
            /// </summary>
            public StatoChiamataRegistrata StatoChiamata
            {
                get
                {
                    return m_StatoChiamata;
                }

                set
                {
                    var oldValue = m_StatoChiamata;
                    if (oldValue == value)
                        return;
                    m_StatoChiamata = value;
                    DoChanged("StatoChiamata", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione estesa dello stato della chiamata
            /// </summary>
            public string StatoChiamataEx
            {
                get
                {
                    return m_StatoChiamataEx;
                }

                set
                {
                    string oldValue = m_StatoChiamataEx;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StatoChiamataEx = value;
                    DoChanged("StatoChiamataEx", value, oldValue);
                }
            }

            /// <summary>
            /// Esito della chiamata
            /// </summary>
            public EsitoChiamataRegistrata EsitoChiamata // Esito della chiamata
            {
                get
                {
                    return m_EsitoChiamata;
                }

                set
                {
                    var oldValue = m_EsitoChiamata;
                    if (oldValue == value)
                        return;
                    m_EsitoChiamata = value;
                    DoChanged("EsitoChiamata", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione dell'esito della chiamata
            /// </summary>
            public string EsitoChiamataEx
            {
                get
                {
                    return m_EsitoChiamataEx;
                }

                set
                {
                    string oldValue = m_EsitoChiamataEx;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EsitoChiamataEx = value;
                    DoChanged("EsitoChiamataEx", value, oldValue);
                }
            }

            /// <summary>
            /// Data di composizione
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Data della risposta
            /// </summary>
            public DateTime? DataRisposta
            {
                get
                {
                    return m_DataRisposta;
                }

                set
                {
                    var oldValue = m_DataRisposta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRisposta = value;
                    DoChanged("DataRisposta", value, oldValue);
                }
            }

            /// <summary>
            /// Data di fine (chiamata terminata)
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
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// ID del centralino usato
            /// </summary>
            public int IDPBX
            {
                get
                {
                    return DBUtils.GetID(m_PBX, m_IDPBX);
                }

                set
                {
                    int oldValue = IDPBX;
                    if (oldValue == value)
                        return;
                    m_IDPBX = value;
                    m_PBX = null;
                    DoChanged("IDPBX", value, oldValue);
                }
            }

            /// <summary>
            /// Centralino utilizzato
            /// </summary>
            public PBX PBX                 // Centralino che ha gestito la chiamata
            {
                get
                {
                    if (m_PBX is null)
                        m_PBX = PBXs.GetItemById(m_IDPBX);
                    return m_PBX;
                }

                set
                {
                    var oldValue = m_PBX;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PBX = value;
                    m_IDPBX = DBUtils.GetID(value, 0);
                    DoChanged("PBX", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del centralino utilizzato
            /// </summary>
            public string NomePBX
            {
                get
                {
                    return m_NomePBX;
                }

                set
                {
                    string oldValue = m_NomePBX;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePBX = value;
                    DoChanged("NomePBX", value, oldValue);
                }
            }

            /// <summary>
            /// ID del chiamante
            /// </summary>
            public int IDChiamante
            {
                get
                {
                    return DBUtils.GetID(m_Chiamante, m_IDChiamante);
                }

                set
                {
                    int oldValue = IDChiamante;
                    if (oldValue == value)
                        return;
                    m_IDChiamante = value;
                    m_Chiamante = null;
                    DoChanged("IDChiamante", value, oldValue);
                }
            }

            /// <summary>
            /// Chiamante
            /// </summary>
            public CPersona Chiamante
            {
                get
                {
                    if (m_Chiamante is null)
                        m_Chiamante = Anagrafica.Persone.GetItemById(m_IDChiamante);
                    return m_Chiamante;
                }

                set
                {
                    var oldValue = Chiamante;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Chiamante = value;
                    m_IDChiamante = DBUtils.GetID(value, 0);
                    m_NomeChiamante = "";
                    if (value is object)
                        m_NomeChiamante = value.Nominativo;
                    DoChanged("Chiamante", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del chiamante
            /// </summary>
            public string NomeChiamante
            {
                get
                {
                    return m_NomeChiamante;
                }

                set
                {
                    string oldValue = m_NomeChiamante;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeChiamante = value;
                    DoChanged("NomeChiamante", value, oldValue);
                }
            }

            /// <summary>
            /// ID del chiamanto
            /// </summary>
            public int IDChiamato
            {
                get
                {
                    return DBUtils.GetID(m_Chiamato, m_IDChiamato);
                }

                set
                {
                    int oldValue = IDChiamato;
                    if (oldValue == value)
                        return;
                    m_IDChiamato = value;
                    m_Chiamato = null;
                    DoChanged("IDChiamanto", value, oldValue);
                }
            }

            /// <summary>
            /// Chiamato
            /// </summary>
            public CPersona Chiamato
            {
                get
                {
                    if (m_Chiamato is null)
                        m_Chiamato = Anagrafica.Persone.GetItemById(m_IDChiamato);
                    return m_Chiamato;
                }

                set
                {
                    var oldValue = Chiamato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Chiamato = value;
                    m_IDChiamato = DBUtils.GetID(value, 0);
                    m_NomeChiamato = "";
                    if (value is object)
                        m_NomeChiamato = value.Nominativo;
                    DoChanged("Chiamato", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del chiamato
            /// </summary>
            public string NomeChiamato
            {
                get
                {
                    return m_NomeChiamato;
                }

                set
                {
                    string oldValue = m_NomeChiamato;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeChiamato = value;
                    DoChanged("NomeChiamato", value, oldValue);
                }
            }

            /// <summary>
            /// Numero del chiamante
            /// </summary>
            public string DaNumero
            {
                get
                {
                    return m_DaNumero;
                }

                set
                {
                    string oldValue = m_DaNumero;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DaNumero = value;
                    DoChanged("DaNumero", value, oldValue);
                }
            }

            /// <summary>
            /// Numero del chiamato
            /// </summary>
            public string ANumero
            {
                get
                {
                    return m_ANumero;
                }

                set
                {
                    string oldValue = m_ANumero;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ANumero = value;
                    DoChanged("ANumero", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del canale utilizzato
            /// </summary>
            public string NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }

                set
                {
                    string oldValue = m_NomeCanale;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCanale = value;
                    DoChanged("NomeCanale", value, oldValue);
                }
            }

            /// <summary>
            /// Qualità della chiamata
            /// </summary>
            public QualitaChiamata Qualita
            {
                get
                {
                    return m_Qualita;
                }

                set
                {
                    var oldValue = m_Qualita;
                    if (oldValue == value)
                        return;
                    m_Qualita = value;
                    DoChanged("Qualita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_NomePBX , ", " , Sistema.Formats.FormatUserDateTime(m_DataInizio) , ", "  , m_DaNumero , " -> " , m_ANumero , ", " , Enum.GetName(typeof(StatoChiamataRegistrata), m_StatoChiamata));
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DataInizio);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is ChiamataRegistrata) && this.Equals((ChiamataRegistrata)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ChiamataRegistrata obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_IDChiamata, obj.m_IDChiamata)
                    && DMD.RunTime.EQ(this.m_StatoChiamata, obj.m_StatoChiamata)
                    && DMD.Strings.EQ(this.m_EsitoChiamataEx, obj.m_EsitoChiamataEx)
                    && DMD.RunTime.EQ(this.m_EsitoChiamata, obj.m_EsitoChiamata)
                    && DMD.Strings.EQ(this.m_StatoChiamataEx, obj.m_StatoChiamataEx)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataRisposta, obj.m_DataRisposta)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Integers.EQ(this.m_IDPBX, obj.m_IDPBX)
                    && DMD.Strings.EQ(this.m_NomePBX, obj.m_NomePBX)
                    && DMD.Integers.EQ(this.m_IDChiamante, obj.m_IDChiamante)
                    && DMD.Strings.EQ(this.m_NomeChiamante, obj.m_NomeChiamante)
                    && DMD.Integers.EQ(this.m_IDChiamato, obj.m_IDChiamato)
                    && DMD.Strings.EQ(this.m_NomeChiamato, obj.m_NomeChiamato)
                    && DMD.Strings.EQ(this.m_DaNumero, obj.m_DaNumero)
                    && DMD.Strings.EQ(this.m_ANumero, obj.m_ANumero)
                    && DMD.Strings.EQ(this.m_NomeCanale, obj.m_NomeCanale)
                    && DMD.RunTime.EQ(this.m_Qualita, obj.m_Qualita)
                    ;
            }

            /// <summary>
            /// Compara due oggetti per data in ordine crescente
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(ChiamataRegistrata other)
            {
                int ret = DMD.DateUtils.Compare(this.m_DataInizio, other.m_DataInizio);
                if (ret == 0) ret = DMD.DateUtils.Compare(this.m_DataRisposta, other.m_DataRisposta);
                if (ret == 0) ret = DMD.DateUtils.Compare(this.m_DataFine, other.m_DataFine);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((ChiamataRegistrata)obj); }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.ChiamateRegistrate;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeRegCall";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDChiamata = reader.Read("IDChiamata",  m_IDChiamata);
                m_StatoChiamata = reader.Read("StatoChiamata",  m_StatoChiamata);
                m_StatoChiamataEx = reader.Read("StatoChiamataEx",  m_StatoChiamataEx);
                m_EsitoChiamata = reader.Read("EsitoChiamata",  m_EsitoChiamata);
                m_EsitoChiamataEx = reader.Read("EsitoChiamataEx",  m_EsitoChiamataEx);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataRisposta = reader.Read("DataRisposta",  m_DataRisposta);
                m_DataFine = reader.Read("DataFine",  m_DataFine);
                m_IDPBX = reader.Read("IDPBX",  m_IDPBX);
                m_NomePBX = reader.Read("NomePBX",  m_NomePBX);
                m_IDChiamante = reader.Read("IDChiamante",  m_IDChiamante);
                m_NomeChiamante = reader.Read("NomeChiamante",  m_NomeChiamante);
                m_IDChiamato = reader.Read("IDChiamato",  m_IDChiamato);
                m_NomeChiamato = reader.Read("NomeChiamato",  m_NomeChiamato);
                m_DaNumero = reader.Read("DaNumero",  m_DaNumero);
                m_ANumero = reader.Read("ANumero",  m_ANumero);
                m_NomeCanale = reader.Read("NomeCanale",  m_NomeCanale);
                m_Qualita = reader.Read("Qualita",  m_Qualita);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDChiamata", m_IDChiamata);
                writer.Write("StatoChiamata", m_StatoChiamata);
                writer.Write("StatoChiamataEx", m_StatoChiamataEx);
                writer.Write("EsitoChiamata", m_EsitoChiamata);
                writer.Write("EsitoChiamataEx", m_EsitoChiamataEx);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataRisposta", m_DataRisposta);
                writer.Write("DataFine", m_DataFine);
                writer.Write("IDPBX", IDPBX);
                writer.Write("NomePBX", m_NomePBX);
                writer.Write("IDChiamante", IDChiamante);
                writer.Write("NomeChiamante", m_NomeChiamante);
                writer.Write("IDChiamato", IDChiamato);
                writer.Write("NomeChiamato", m_NomeChiamato);
                writer.Write("DaNumero", m_DaNumero);
                writer.Write("ANumero", m_ANumero);
                writer.Write("NomeCanale", m_NomeCanale);
                writer.Write("Qualita", m_Qualita);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDChiamata", typeof(string), 255);
                c = table.Fields.Ensure("StatoChiamata", typeof(int), 1);
                c = table.Fields.Ensure("StatoChiamataEx", typeof(string), 255);
                c = table.Fields.Ensure("EsitoChiamata", typeof(int), 1);
                c = table.Fields.Ensure("EsitoChiamataEx", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataRisposta", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDPBX", typeof(int), 1);
                c = table.Fields.Ensure("NomePBX", typeof(string), 255);
                c = table.Fields.Ensure("IDChiamante", typeof(int), 1);
                c = table.Fields.Ensure("NomeChiamante", typeof(string), 255);
                c = table.Fields.Ensure("IDChiamato", typeof(int), 1);
                c = table.Fields.Ensure("NomeChiamato", typeof(string), 255);
                c = table.Fields.Ensure("DaNumero", typeof(string), 255);
                c = table.Fields.Ensure("ANumero", typeof(string), 255);
                c = table.Fields.Ensure("NomeCanale", typeof(string), 255);
                c = table.Fields.Ensure("Qualita", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxChiamanta", new string[] { "IDChiamata" , "IDPBX", "NomePBX", "NomeCanale"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoChiamata", new string[] {"StatoChiamata", "EsitoChiamata", "StatoChiamataEx", "EsitoChiamataEx" , "Qualita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataRisposta", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxChiamante", new string[] { "IDChiamante", "NomeChiamante", "DaNumero" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxChiamato", new string[] { "IDChiamato", "NomeChiamato", "ANumero" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDChiamata", m_IDChiamata);
                writer.WriteAttribute("StatoChiamata", (int?)m_StatoChiamata);
                writer.WriteAttribute("StatoChiamataEx", m_StatoChiamataEx);
                writer.WriteAttribute("EsitoChiamata", (int?)m_EsitoChiamata);
                writer.WriteAttribute("EsitoChiamataEx", m_EsitoChiamataEx);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataRisposta", m_DataRisposta);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("IDPBX", IDPBX);
                writer.WriteAttribute("NomePBX", m_NomePBX);
                writer.WriteAttribute("IDChiamante", IDChiamante);
                writer.WriteAttribute("NomeChiamante", m_NomeChiamante);
                writer.WriteAttribute("IDChiamato", IDChiamato);
                writer.WriteAttribute("NomeChiamato", m_NomeChiamato);
                writer.WriteAttribute("DaNumero", m_DaNumero);
                writer.WriteAttribute("ANumero", m_ANumero);
                writer.WriteAttribute("NomeCanale", m_NomeCanale);
                writer.WriteAttribute("Qualita", (int?)m_Qualita);
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
                    case "IDChiamata":
                        {
                            m_IDChiamata = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoChiamata":
                        {
                            m_StatoChiamata = (StatoChiamataRegistrata)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoChiamataEx":
                        {
                            m_StatoChiamataEx = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EsitoChiamata":
                        {
                            m_EsitoChiamata = (EsitoChiamataRegistrata)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "EsitoChiamataEx":
                        {
                            m_EsitoChiamataEx = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataRisposta":
                        {
                            m_DataRisposta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPBX":
                        {
                            m_IDPBX = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePBX":
                        {
                            m_NomePBX = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDChiamante":
                        {
                            m_IDChiamante = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeChiamante":
                        {
                            m_NomeChiamante = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDChiamato":
                        {
                            m_IDChiamato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeChiamato":
                        {
                            m_NomeChiamato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DaNumero":
                        {
                            m_DaNumero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ANumero":
                        {
                            m_ANumero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeCanale":
                        {
                            m_NomeCanale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Qualita":
                        {
                            m_Qualita = (QualitaChiamata)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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