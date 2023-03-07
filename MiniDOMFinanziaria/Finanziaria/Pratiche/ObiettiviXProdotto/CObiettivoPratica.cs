using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum TipoObiettivo : int
        {
            Liquidato = 0,
            NumreoPratiche = 1,
            Spread = 2,
            UpFront = 4
        }

        public enum PeriodicitaObiettivo : int
        {
            Giornaliero = 0,
            Mensile = 1,
            Annuale = 2,
            Trimestrale = 3,
            Bimestrale = 4,
            Quadrimestrale = 5,
            Semestrale = 6,
            TraDate = 7
        }

        /// <summary>
    /// Rappresenta la specifica di un obiettivo per un ufficio
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CObiettivoPratica : Databases.DBObjectPO, IComparable
        {
            private const int FLAG_ATTIVO = 1;
            private string m_Nome;
            private TipoObiettivo m_TipoObiettivo;
            private PeriodicitaObiettivo m_PeriodicitaObiettivo;
            private decimal? m_MontanteLordoLiq;
            private int? m_NumeroPraticheLiq;
            private decimal? m_ValoreSpreadLiq;
            private float? m_SpreadLiq;
            private decimal? m_ValoreUpFrontLiq;
            private float? m_UpFrontLiq;
            private decimal? m_ValoreScontoLiq;
            private float? m_ScontoLiq;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private int m_Flags;
            private string m_Descrizione;
            private int m_Livello;
            private decimal? m_CostoStruttura;
            private CKeyCollection m_Attributi;

            public CObiettivoPratica()
            {
                m_Nome = "";
                m_TipoObiettivo = TipoObiettivo.Liquidato;
                m_PeriodicitaObiettivo = PeriodicitaObiettivo.Mensile;
                m_MontanteLordoLiq = default;
                m_NumeroPraticheLiq = default;
                m_ValoreSpreadLiq = default;
                m_SpreadLiq = default;
                m_ValoreUpFrontLiq = default;
                m_ScontoLiq = default;
                m_ValoreScontoLiq = default;
                m_UpFrontLiq = default;
                m_DataInizio = default;
                m_DataFine = default;
                m_Livello = 0;
                m_Flags = FLAG_ATTIVO;
                m_Descrizione = "";
                m_CostoStruttura = default;
                m_Attributi = null;
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
        /// Restituisce o imposta il costo struttura
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? CostoStruttura
            {
                get
                {
                    return m_CostoStruttura;
                }

                set
                {
                    var oldValue = m_CostoStruttura;
                    if (oldValue == value == true)
                        return;
                    m_CostoStruttura = value;
                    DoChanged("CostoStruttura", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero del livello
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Livello
            {
                get
                {
                    return m_Livello;
                }

                set
                {
                    int oldValue = m_Livello;
                    if (oldValue == value)
                        return;
                    m_Livello = value;
                    DoChanged("Livello", value, oldValue);
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

            // ''' <summary>
            // ''' Restituisce o imposta il valore dell'obiettivo
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property ValoreObiettivo As Double
            // Get
            // Return Me.m_ValoreObiettivo
            // End Get
            // Set(value As Double)
            // Dim oldValue As Double = Me.m_ValoreObiettivo
            // If (oldValue = value) Then Exit Property
            // Me.m_ValoreObiettivo = value
            // Me.DoChanged("ValoreObiettivo", value, oldValue)
            // End Set
            // End Property

            /// <summary>
        /// Restituisce o imposta il vincolo sul montante lordo liquidato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? MontanteLordoLiq
            {
                get
                {
                    return m_MontanteLordoLiq;
                }

                set
                {
                    var oldValue = m_MontanteLordoLiq;
                    if (oldValue == value == true)
                        return;
                    m_MontanteLordoLiq = value;
                    DoChanged("MontanteLordoLiq", value, oldValue);
                }
            }

            /// <summary>
        /// Restitusice o imposta il vincolo sul numero di pratiche liquidate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int? NumeroPraticheLiq
            {
                get
                {
                    return m_NumeroPraticheLiq;
                }

                set
                {
                    var oldValue = m_NumeroPraticheLiq;
                    if (oldValue == value == true)
                        return;
                    m_NumeroPraticheLiq = value;
                    DoChanged("NumeroPraticheLiq", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il vincolo sul valore dello spread
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreSpreadLiq
            {
                get
                {
                    return m_ValoreSpreadLiq;
                }

                set
                {
                    var oldValue = m_ValoreSpreadLiq;
                    if (oldValue == value == true)
                        return;
                    m_ValoreSpreadLiq = value;
                    DoChanged("ValoreSpreadLiq", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il vincolo sulla percentuale media dello spread liquidato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public float? SpreadLiq
            {
                get
                {
                    return m_SpreadLiq;
                }

                set
                {
                    var oldValue = m_SpreadLiq;
                    if (oldValue == value == true)
                        return;
                    m_SpreadLiq = value;
                    DoChanged("SpreadLiq", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il vincolo sul valore dell'upfront
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreUpFront
            {
                get
                {
                    return m_ValoreUpFrontLiq;
                }

                set
                {
                    var oldValue = m_ValoreUpFrontLiq;
                    if (oldValue == value == true)
                        return;
                    m_ValoreUpFrontLiq = value;
                    DoChanged("ValoreUpFront", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il vincolo sulla percentuale dell'upfront
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public float? UpFrontLiq
            {
                get
                {
                    return m_UpFrontLiq;
                }

                set
                {
                    var oldValue = m_UpFrontLiq;
                    if (oldValue == value == true)
                        return;
                    m_UpFrontLiq = value;
                    DoChanged("UpFrontLiq", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il vincolo sul valore dello sconto effettuato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal? ValoreScontoLiq
            {
                get
                {
                    return m_ValoreScontoLiq;
                }

                set
                {
                    var oldValue = m_ValoreScontoLiq;
                    if (oldValue == value == true)
                        return;
                    m_ValoreScontoLiq = value;
                    DoChanged("ValoreScontoLiq", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il vincolo sulla percentuale di sconto effettuato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public float? ScontoLiq
            {
                get
                {
                    return m_ScontoLiq;
                }

                set
                {
                    var oldValue = m_ScontoLiq;
                    if (oldValue == value == true)
                        return;
                    m_ScontoLiq = value;
                    DoChanged("ScontoLiq", value, oldValue);
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
                return Obiettivi.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDObiettivi";
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret)
                    Obiettivi.UpdateCached(this);
                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_TipoObiettivo = reader.Read("TipoObiettivo", m_TipoObiettivo);
                m_PeriodicitaObiettivo = reader.Read("PeriodicitaObiettivo", m_PeriodicitaObiettivo);
                // reader.Read("ValoreObiettivo", Me.m_ValoreObiettivo)
                m_MontanteLordoLiq = reader.Read("MontanteLordoLiq", m_MontanteLordoLiq);
                m_NumeroPraticheLiq = reader.Read("NumeroPraticheLiq", m_NumeroPraticheLiq);
                m_ValoreSpreadLiq = reader.Read("ValoreSpreadLiq", m_ValoreSpreadLiq);
                m_SpreadLiq = reader.Read("SpreadLiq", m_SpreadLiq);
                m_ValoreUpFrontLiq = reader.Read("ValoreUpFrontLiq", m_ValoreUpFrontLiq);
                m_UpFrontLiq = reader.Read("UpFrontLiq", m_UpFrontLiq);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                m_Flags = reader.Read("Flags", m_Flags);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_ValoreScontoLiq = reader.Read("ValoreScontoLiq", m_ValoreScontoLiq);
                m_ScontoLiq = reader.Read("ScontoLiq", m_ScontoLiq);
                m_Livello = reader.Read("Livello", m_Livello);
                m_CostoStruttura = reader.Read("CostoStruttura", m_CostoStruttura);
                try
                {
                    string argvalue = "";
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Attributi", argvalue));
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
                // writer.Write("ValoreObiettivo", Me.m_ValoreObiettivo)
                writer.Write("MontanteLordoLiq", m_MontanteLordoLiq);
                writer.Write("NumeroPraticheLiq", m_NumeroPraticheLiq);
                writer.Write("ValoreSpreadLiq", m_ValoreSpreadLiq);
                writer.Write("SpreadLiq", m_SpreadLiq);
                writer.Write("ValoreUpFrontLiq", m_ValoreUpFrontLiq);
                writer.Write("UpFrontLiq", m_UpFrontLiq);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Flags", m_Flags);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("ValoreScontoLiq", m_ValoreScontoLiq);
                writer.Write("ScontoLiq", m_ScontoLiq);
                writer.Write("Livello", m_Livello);
                writer.Write("CostoStruttura", m_CostoStruttura);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("TipoObiettivo", (int?)m_TipoObiettivo);
                writer.WriteAttribute("PeriodicitaObiettivo", (int?)m_PeriodicitaObiettivo);
                // writer.WriteAttribute("ValoreObiettivo", Me.m_ValoreObiettivo)
                writer.WriteAttribute("MontanteLordoLiq", m_MontanteLordoLiq);
                writer.WriteAttribute("NumeroPraticheLiq", m_NumeroPraticheLiq);
                writer.WriteAttribute("ValoreSpreadLiq", m_ValoreSpreadLiq);
                writer.WriteAttribute("SpreadLiq", m_SpreadLiq);
                writer.WriteAttribute("ValoreUpFrontLiq", m_ValoreUpFrontLiq);
                writer.WriteAttribute("UpFrontLiq", m_UpFrontLiq);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Flags", m_Flags);
                writer.WriteAttribute("ValoreScontoLiq", m_ValoreScontoLiq);
                writer.WriteAttribute("ScontoLiq", m_ScontoLiq);
                writer.WriteAttribute("Livello", m_Livello);
                writer.WriteAttribute("CostoStruttura", m_CostoStruttura);
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
                    // Case "ValoreObiettivo" : Me.m_ValoreObiettivo = DMD.XML.Utils.Serializer.DeserializeDouble(fieldValue)
                    case "MontanteLordoLiq":
                        {
                            m_MontanteLordoLiq = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NumeroPraticheLiq":
                        {
                            m_NumeroPraticheLiq = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValoreSpreadLiq":
                        {
                            m_ValoreSpreadLiq = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpreadLiq":
                        {
                            m_SpreadLiq = (float?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreUpFrontLiq":
                        {
                            m_ValoreUpFrontLiq = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "UpFrontLiq":
                        {
                            m_UpFrontLiq = (float?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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

                    case "ValoreScontoLiq":
                        {
                            m_ValoreScontoLiq = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ScontoLiq":
                        {
                            m_ScontoLiq = (float?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Livello":
                        {
                            m_Livello = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CostoStruttura":
                        {
                            m_CostoStruttura = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public int CompareTo(CObiettivoPratica obj)
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
                return CompareTo((CObiettivoPratica)obj);
            }

            public override string ToString()
            {
                return m_Nome;
            }
        }
    }
}