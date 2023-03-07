using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CTabellaAssicurativa 
            : Databases.DBObject, ICloneable
        {
            private string m_Nome;
            private string m_Descrizione;
            private int m_Dividendo;
            private int m_IDAssicurazione;
            [NonSerialized] private CAssicurazione m_Assicurazione;
            private string m_NomeAssicurazione;
            private int m_MeseScatto;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private CCoefficientiAssicurativiCollection m_Coefficienti;

            public CTabellaAssicurativa()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_Descrizione = DMD.Strings.vbNullString;
                m_Dividendo = 1000;
                m_DataInizio = default;
                m_DataFine = default;
                m_IDAssicurazione = 0;
                m_Assicurazione = null;
                m_NomeAssicurazione = "";
                m_MeseScatto = -1;
                m_Coefficienti = null;
            }

            public override CModulesClass GetModule()
            {
                return TabelleAssicurative.Module;
            }

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

            public int Dividendo
            {
                get
                {
                    return m_Dividendo;
                }

                set
                {
                    if (value <= 0)
                        throw new ArgumentOutOfRangeException("Dividendo deve essere un valore positivo");
                    int oldValue = m_Dividendo;
                    if (oldValue == value)
                        return;
                    m_Dividendo = value;
                    DoChanged("Dividendo", value, oldValue);
                }
            }

            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            public bool IsValid(DateTime at)
            {
                return DMD.DateUtils.CheckBetween(at, m_DataInizio, m_DataFine);
            }

            public int IDAssicurazione
            {
                get
                {
                    return DBUtils.GetID(m_Assicurazione, m_IDAssicurazione);
                }

                set
                {
                    int oldValue = IDAssicurazione;
                    if (oldValue == value)
                        return;
                    m_IDAssicurazione = value;
                    m_Assicurazione = null;
                    DoChanged("IDAssicurazione", value, oldValue);
                }
            }

            public CAssicurazione Assicurazione
            {
                get
                {
                    if (m_Assicurazione is null)
                        m_Assicurazione = Assicurazioni.GetItemById(m_IDAssicurazione);
                    return m_Assicurazione;
                }

                set
                {
                    var oldValue = Assicurazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Assicurazione = value;
                    m_IDAssicurazione = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAssicurazione = value.Nome;
                    DoChanged("Assicurazione", value, oldValue);
                }
            }

            public string NomeAssicurazione
            {
                get
                {
                    return m_NomeAssicurazione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeAssicurazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssicurazione = value;
                    DoChanged("NomeAssicurazione", value, oldValue);
                }
            }

            public int MeseScatto
            {
                get
                {
                    return m_MeseScatto;
                }

                set
                {
                    if (value < -1 | value > 12)
                        throw new ArgumentOutOfRangeException("MeseScatto");
                    int oldValue = m_MeseScatto;
                    m_MeseScatto = value;
                    DoChanged("MeseScatto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce un oggetto CCoefficientiAssicurativiCollection relativo ai coefficienti di questa tabella
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCoefficientiAssicurativiCollection Coefficienti
            {
                get
                {
                    if (m_Coefficienti is null)
                        m_Coefficienti = new CCoefficientiAssicurativiCollection(this);
                    return m_Coefficienti;
                }
            }

            public double? GetCoefficiente(string Sesso, int Anni, int Durata)
            {
                double? p;
                if (m_Coefficienti is object)
                {
                    p = m_Coefficienti.GetCoefficiente(Sesso, Anni, Durata);
                }
                else
                {
                    p = (double?)GetConnection().ExecuteScalar("SELECT [C" + Durata + "] FROM [tbl_CoefficientiAssicurativi] WHERE ([Tabella]=" + DBUtils.GetID(this) + ") AND ([Sesso]=" + DBUtils.DBString(Sesso) + " Or [Sesso]='U') AND ([anni]=" + Anni + ")");
                }

                if (p.HasValue)
                    p = p.Value / m_Dividendo;
                return p;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (m_Coefficienti is object)
                    m_Coefficienti.Save(force);
                return ret;
            }

            public override string GetTableName()
            {
                return "tbl_TabelleAssicurative";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_Dividendo = reader.Read("Dividendo", this.m_Dividendo);
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                m_IDAssicurazione = reader.Read("IDAssicurazione", this.m_IDAssicurazione);
                m_NomeAssicurazione = reader.Read("NomeAssicurazione", this.m_NomeAssicurazione);
                m_MeseScatto = reader.Read("MeseScatto", this.m_MeseScatto);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Dividendo", m_Dividendo);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("IDAssicurazione", IDAssicurazione);
                writer.Write("NomeAssicurazione", m_NomeAssicurazione);
                writer.Write("MeseScatto", m_MeseScatto);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Dividendo", m_Dividendo);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("IDAssicurazione", IDAssicurazione);
                writer.WriteAttribute("NomeAssicurazione", m_NomeAssicurazione);
                writer.WriteAttribute("MeseScatto", m_MeseScatto);
                base.XMLSerialize(writer);
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

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Dividendo":
                        {
                            m_Dividendo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "IDAssicurazione":
                        {
                            m_IDAssicurazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAssicurazione":
                        {
                            m_NomeAssicurazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MeseScatto":
                        {
                            m_MeseScatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                TabelleAssicurative.UpdateCached(this);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}