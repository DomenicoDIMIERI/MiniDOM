using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Rappresenta la specifica di un obiettivo per un ufficio
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CObiettivoCategoriaProdotto : Databases.DBObjectPO, IComparable
        {
            private const int FLAG_ATTIVO = 1;
            private string m_Nome;
            private TipoObiettivo m_TipoObiettivo;
            private PeriodicitaObiettivo m_PeriodicitaObiettivo;
            private int m_IDCategoria;
            private CCategoriaProdotto m_Categoria;
            private string m_NomeGruppo;
            private double m_Percentuale;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private int m_Flags;
            private string m_Descrizione;
            private CKeyCollection m_Attributi;

            public CObiettivoCategoriaProdotto()
            {
                m_Nome = "";
                m_IDCategoria = 0;
                m_Categoria = null;
                m_TipoObiettivo = TipoObiettivo.Liquidato;
                m_PeriodicitaObiettivo = PeriodicitaObiettivo.Mensile;
                m_DataInizio = default;
                m_DataFine = default;
                m_Descrizione = "";
                m_Attributi = null;
                m_NomeGruppo = "";
                m_Percentuale = 0d;
                m_Flags = FLAG_ATTIVO;
            }

            /// <summary>
        /// Restituisce o imposta il nome del gruppo di categorie prodotto
        /// </summary>
        /// <returns></returns>
            public string NomeGruppo
            {
                get
                {
                    return m_NomeGruppo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeGruppo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeGruppo = value;
                    DoChanged("NomeGruppo", value, oldValue);
                }
            }

            public int IDCategoria
            {
                get
                {
                    return DBUtils.GetID(m_Categoria, m_IDCategoria);
                }

                set
                {
                    int oldValue = IDCategoria;
                    if (oldValue == value)
                        return;
                    m_IDCategoria = value;
                    m_Categoria = null;
                    DoChanged("IDCategoria", value, oldValue);
                }
            }

            public CCategoriaProdotto Categoria
            {
                get
                {
                    if (m_Categoria is null)
                        m_Categoria = CategorieProdotto.GetItemById(m_IDCategoria);
                    return m_Categoria;
                }

                set
                {
                    var oldValue = Categoria;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Categoria = value;
                    m_IDCategoria = DBUtils.GetID(value);
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale
        /// </summary>
        /// <returns></returns>
            public double Percentuale
            {
                get
                {
                    return m_Percentuale;
                }

                set
                {
                    double oldValue = m_Percentuale;
                    if (oldValue == value)
                        return;
                    m_Percentuale = value;
                    DoChanged("Percentuale", value, oldValue);
                }
            }

            public CKeyCollection Attributi
            {
                get
                {
                    if (m_Attributi is null)
                        m_Attributi = new CKeyCollection();
                    return m_Attributi;
                }
            }



            /// <summary>
        /// Restituisce o imposta il nome dell'obiettivo
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
        /// Restituisce o imposta la descrizione dell'obiettivo
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
        /// Restituisce o imposta il tipo di obiettivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoObiettivo TipoObiettivo
            {
                get
                {
                    return m_TipoObiettivo;
                }

                set
                {
                    var oldValue = m_TipoObiettivo;
                    if (oldValue == value)
                        return;
                    m_TipoObiettivo = value;
                    DoChanged("TipoObiettivo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la periodicità dell'obiettivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public PeriodicitaObiettivo PeriodicitaObiettivo
            {
                get
                {
                    return m_PeriodicitaObiettivo;
                }

                set
                {
                    var oldValue = m_PeriodicitaObiettivo;
                    if (oldValue == value)
                        return;
                    m_PeriodicitaObiettivo = value;
                    DoChanged("PeriodicitaObiettivo", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta la data di inizio validità dell'obiettivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Restituisce o imposta la data di fine validità dell'obiettivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Restituisce o imposta dei flags
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se l'obiettivo è attivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, FLAG_ATTIVO);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, FLAG_ATTIVO, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
        /// Restituisce vero se l'obiettivo è attivo e valido alla data di oggi
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
        /// Restituisce vero se l'obiettivo è attivo e valido alla data indicata
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsValid(DateTime d)
            {
                return Attivo && DMD.DateUtils.CheckBetween(d, m_DataInizio, m_DataFine);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return ObiettiviXCategoria.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDObiettiviXCat";
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret)
                    ObiettiviXCategoria.UpdateCached(this);
                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_TipoObiettivo = reader.Read("TipoObiettivo",  m_TipoObiettivo);
                m_PeriodicitaObiettivo = reader.Read("PeriodicitaObiettivo",  m_PeriodicitaObiettivo);
                m_DataInizio = reader.Read("DataInizio",  m_DataInizio);
                m_DataFine = reader.Read("DataFine",  m_DataFine);
                m_Flags = reader.Read("Flags",  m_Flags);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_IDCategoria = reader.Read("IDCategoria",  m_IDCategoria);
                m_Percentuale = reader.Read("Percentuale",  m_Percentuale);
                m_NomeGruppo = reader.Read("NomeGruppo",  m_NomeGruppo);
                try
                {
                    string argvalue = "";
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Attributi",  argvalue));
                }
                catch (Exception ex)
                {
                    m_Attributi = null;
                }

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("TipoObiettivo", m_TipoObiettivo);
                writer.Write("PeriodicitaObiettivo", m_PeriodicitaObiettivo);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Flags", m_Flags);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDCategoria", IDCategoria);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("Percentuale", m_Percentuale);
                writer.Write("NomeGruppo", m_NomeGruppo);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("TipoObiettivo", (int?)m_TipoObiettivo);
                writer.WriteAttribute("PeriodicitaObiettivo", (int?)m_PeriodicitaObiettivo);
                // writer.WriteAttribute("ValoreObiettivo", Me.m_ValoreObiettivo)
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("IDCategoria", IDCategoria);
                writer.WriteAttribute("Percentuale", m_Percentuale);
                writer.WriteAttribute("NomeGruppo", m_NomeGruppo);
                base.XMLSerialize(writer);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("Descrizione", m_Descrizione);
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

                    case "TipoObiettivo":
                        {
                            m_TipoObiettivo = (TipoObiettivo)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PeriodicitaObiettivo":
                        {
                            m_PeriodicitaObiettivo = (PeriodicitaObiettivo)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "IDCategoria":
                        {
                            m_IDCategoria = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Percentuale":
                        {
                            m_Percentuale = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeGruppo":
                        {
                            m_NomeGruppo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public int CompareTo(CObiettivoCategoriaProdotto obj)
            {
                int a1 = Sistema.IIF(Attivo, 0, 1);
                int a2 = Sistema.IIF(obj.Attivo, 0, 1);
                int ret = a1 - a1;
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataInizio, obj.m_DataInizio);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataFine, obj.m_DataFine);
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CObiettivoCategoriaProdotto)obj);
            }

            public override string ToString()
            {
                return m_Nome;
            }
        }
    }
}