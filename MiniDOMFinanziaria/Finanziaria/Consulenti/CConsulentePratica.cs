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
        public class CConsulentePratica : Databases.DBObjectPO, IComparable, ICloneable
        {
            private string m_Nome;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private int m_IDUser;             // ID dell'utente associato al consulente
            private Sistema.CUser m_User;

            public CConsulentePratica()
            {
                m_Nome = "";
                m_DataInizio = default;
                m_DataFine = default;
                m_IDUser = 0;
                m_User = null;
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
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
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

            public bool IsValid(DateTime atDate)
            {
                return DMD.DateUtils.CheckBetween(atDate, m_DataInizio, m_DataFine);
            }

            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            public int IDUser
            {
                get
                {
                    return DBUtils.GetID(m_User, m_IDUser);
                }

                set
                {
                    int oldValue = IDUser;
                    if (oldValue == value)
                        return;
                    m_IDUser = value;
                    m_User = null;
                    DoChanged("IDUsers", value, oldValue);
                }
            }

            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_IDUser);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (oldValue == value)
                        return;
                    m_User = value;
                    m_IDUser = DBUtils.GetID(value);
                    DoChanged("User", value, oldValue);
                }
            }

            public override string ToString()
            {
                return m_Nome;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDConsulenti";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override void Save(bool force = false)
            {
                base.Save(force);
                base.Save(force);
                var grp = GruppoConsulenti;
                if (User is object && !grp.Members.Contains(User))
                {
                    grp.Members.Add(User);
                }
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                this.m_IDUser = reader.Read("IDUser", this.m_IDUser);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", this.m_Nome);
                writer.Write("DataInizio", this.m_DataInizio);
                writer.Write("DataFine", this.m_DataFine);
                writer.Write("IDUser", this.IDUser);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", this.m_Nome);
                writer.WriteAttribute("DataInizio", this.m_DataInizio);
                writer.WriteAttribute("DataFine", this.m_DataFine);
                writer.WriteAttribute("IDUser", this.IDUser);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            this.m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            this.m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            this.m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDUser":
                        {
                            this.m_IDUser = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override CModulesClass GetModule()
            {
                return Consulenti.Module;
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                Consulenti.UpdateCached(this);
            }

            public int CompareTo(object obj)
            {
                CConsulentePratica tmp = (CConsulentePratica)obj;
                return DMD.Strings.Compare(Nome, tmp.Nome, true);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}