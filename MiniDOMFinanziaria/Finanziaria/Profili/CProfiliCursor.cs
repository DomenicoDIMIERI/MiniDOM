using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CProfiliCursor : Databases.DBObjectCursor<CProfilo>
        {
            private DBCursorField<int> m_CessionarioID = new DBCursorField<int>("Cessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Profilo = new DBCursorStringField("Profilo");
            private DBCursorStringField m_ProfiloVisibile = new DBCursorStringField("ProfiloVisibile");
            private DBCursorStringField m_UserName = new DBCursorStringField("UserName");
            private DBCursorField<bool> m_Visibile = new DBCursorField<bool>("Visibile");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_ParentID = new DBCursorField<int>("Padre");
            private bool m_ShowInterni;
            private bool m_ShowEsterni;
            private bool m_OnlyValid;
            private bool m_OnlyAssigned;

            public CProfiliCursor()
            {
                m_ShowInterni = true;
                m_ShowEsterni = true;
                m_OnlyAssigned = false;
                m_OnlyValid = false;
            }

            public bool OnlyAssigned
            {
                get
                {
                    return m_OnlyAssigned;
                }

                set
                {
                    m_OnlyAssigned = value;
                }
            }

            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    m_OnlyValid = value;
                }
            }

            public bool ShowInterni
            {
                get
                {
                    return m_ShowInterni;
                }

                set
                {
                    m_ShowInterni = value;
                }
            }

            public bool ShowEsterni
            {
                get
                {
                    return m_ShowEsterni;
                }

                set
                {
                    m_ShowEsterni = value;
                }
            }

            public DBCursorField<int> IDCessionario
            {
                get
                {
                    return m_CessionarioID;
                }
            }

            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            public DBCursorField<bool> Visibile
            {
                get
                {
                    return m_Visibile;
                }
            }

            public DBCursorField<int> ParentID
            {
                get
                {
                    return m_ParentID;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField Profilo
            {
                get
                {
                    return m_Profilo;
                }
            }

            public DBCursorStringField ProfiloVisibile
            {
                get
                {
                    return m_ProfiloVisibile;
                }
            }

            public DBCursorStringField UserName
            {
                get
                {
                    return m_UserName;
                }
            }

            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return Profili.Module;
            }

            public override string GetTableName()
            {
                return "tbl_Preventivatori";
            }

            private string GetTxtGruppi()
            {
                var ret = new System.Text.StringBuilder();
                lock (Sistema.Users.CurrentUser)
                {
                    foreach (Sistema.CGroup grp in Sistema.Users.CurrentUser.Groups)
                    {
                        if (ret.Length > 0)
                            ret.Append(",");
                        ret.Append(DBUtils.GetID(grp));
                    }
                }

                return ret.ToString();
            }

            public override string GetWherePart()
            {
                string wherePart;
                wherePart = base.GetWherePart();
                if (m_ShowInterni == false)
                    wherePart = DMD.Strings.Combine(wherePart, "(([path]<>'') And Not ([path] Is Null))", " AND ");
                if (m_ShowEsterni == false)
                    wherePart = DMD.Strings.Combine(wherePart, "(([path]='') Or ([path] Is Null))", " AND ");
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                }

                return wherePart;
            }

            public override string GetWherePartLimit()
            {
                string wherePart = base.GetWherePartLimit();
                if (m_OnlyAssigned && Module.UserCanDoAction("list_assigned"))
                {
                    int userID = DBUtils.GetID(Sistema.Users.CurrentUser);
                    string tmpSQL;
                    string txtGruppi = GetTxtGruppi();
                    if (string.IsNullOrEmpty(txtGruppi))
                    {
                        tmpSQL = "";
                        tmpSQL += "([ID] In (SELECT [Preventivatore] FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" + userID + "))";
                    }
                    else
                    {
                        tmpSQL = "";
                        tmpSQL += "(";
                        tmpSQL += "[ID] In (";
                        tmpSQL += "SELECT [Preventivatore] FROM (";
                        tmpSQL += "SELECT [Preventivatore] FROM [tbl_PreventivatoriXGroup] WHERE [Gruppo] In (" + txtGruppi + ") ";
                        tmpSQL += "UNION ";
                        tmpSQL += "SELECT [Preventivatore] FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" + userID + " ";
                        tmpSQL += ") GROUP BY [Preventivatore]";
                        tmpSQL += ")";
                        tmpSQL += ")";
                    }

                    wherePart = DMD.Strings.Combine(wherePart, tmpSQL, " AND ");
                }

                return wherePart;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProfilo();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_ShowInterni", m_ShowInterni);
                writer.WriteAttribute("m_ShowEsterni", m_ShowEsterni);
                writer.WriteAttribute("m_OnlyAssigned", m_OnlyAssigned);
                writer.WriteAttribute("m_OnlyValid", m_OnlyValid);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_ShowInterni":
                        {
                            m_ShowInterni = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_ShowEsterni":
                        {
                            m_ShowEsterni = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_OnlyAssigned":
                        {
                            m_OnlyAssigned = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_OnlyValid":
                        {
                            m_OnlyValid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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