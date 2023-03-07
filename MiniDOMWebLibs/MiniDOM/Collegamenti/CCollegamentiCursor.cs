using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {


        /// <summary>
        /// Cursore sulla tabella dei collegamenti
        /// </summary>
        [Serializable]
        public class CCollegamentiCursor 
            : Databases.DBObjectCursor<CCollegamento>
        {

            private bool m_OnlyAssigned;

            private Databases.CCursorFieldObj<string> m_Nome = new Databases.CCursorFieldObj<string>("Nome");
            private Databases.CCursorFieldObj<string> m_Descrizione = new Databases.CCursorFieldObj<string>("Descrizione");
            private Databases.CCursorFieldObj<string> m_Link = new Databases.CCursorFieldObj<string>("Link");
            private Databases.CCursorFieldObj<string> m_Gruppo = new Databases.CCursorFieldObj<string>("Gruppo");
            private Databases.CCursorFieldObj<string> m_Target = new Databases.CCursorFieldObj<string>("Target");
            private Databases.CCursorField<int> m_IDParent = new Databases.CCursorField<int>("IDParent");
            private Databases.CCursorFieldObj<string> m_CallModule = new Databases.CCursorFieldObj<string>("CallModule");
            private Databases.CCursorFieldObj<string> m_CallAction = new Databases.CCursorFieldObj<string>("CallAction");
            private Databases.CCursorField<bool> m_Attivo = new Databases.CCursorField<bool>("Attivo");
            private Databases.CCursorField<int> m_Posizione = new Databases.CCursorField<int>("Posizione");
            private Databases.CCursorField<CollegamentoFlags> m_Flags = new Databases.CCursorField<CollegamentoFlags>("Flags");
            private Databases.CCursorFieldObj<string> m_PostedData = new Databases.CCursorFieldObj<string>("PostedData");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCollegamentiCursor()
            {
                m_OnlyAssigned = false;
            }

            public Databases.CCursorFieldObj<string> PostedData
            {
                get
                {
                    return m_PostedData;
                }
            }

            public Databases.CCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
                }
            }

            public Databases.CCursorField<int> Posizione
            {
                get
                {
                    return m_Posizione;
                }
            }

            public Databases.CCursorField<CollegamentoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public Databases.CCursorFieldObj<string> Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public Databases.CCursorFieldObj<string> CallModule
            {
                get
                {
                    return m_CallModule;
                }
            }

            public Databases.CCursorFieldObj<string> CallAction
            {
                get
                {
                    return m_CallAction;
                }
            }

            public Databases.CCursorFieldObj<string> Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public Databases.CCursorFieldObj<string> Link
            {
                get
                {
                    return m_Link;
                }
            }

            public Databases.CCursorField<int> IDParent
            {
                get
                {
                    return m_IDParent;
                }
            }

            public Databases.CCursorFieldObj<string> Gruppo
            {
                get
                {
                    return m_Gruppo;
                }
            }

            public Databases.CCursorFieldObj<string> Target
            {
                get
                {
                    return m_Target;
                }
            }

            public override object InstantiateNew(Databases.DBReader dbRis)
            {
                return new CCollegamento();
            }

            public override string GetTableName()
            {
                return "tbl_Collegamenti";
            }

            protected override Sistema.CModule GetModule()
            {
                return Collegamenti.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica che il cursore deve restituire solo i link assegnati all'utente o al gruppo di utenti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool OnlyAssigned
            {
                get
                {
                    return m_OnlyAssigned;
                }

                set
                {
                    if (m_OnlyAssigned == value)
                        return;
                    m_OnlyAssigned = value;
                    Reset1();
                }
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OnlyAssigned", m_OnlyAssigned);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OnlyAssigned":
                        {
                            m_OnlyAssigned = (bool)DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string GetWherePart()
            {
                string ret = base.GetWherePart();
                if (m_OnlyAssigned && Module.UserCanDoAction("list_assigned"))
                {
                    var u = Sistema.Users.CurrentUser;
                    var tmpSQL = new System.Text.StringBuilder();
                    tmpSQL.Append("( [ID] In ");
                    tmpSQL.Append("( SELECT [Collegamento] FROM ");
                    tmpSQL.Append("(SELECT [Collegamento] FROM [tbl_CollegamentiXGruppo] WHERE [Gruppo] In (0,NULL");
                    // SELECT DISTINCT [Group] FROM [tbl_UsersXGroup] WHERE [User]=" & userID 
                    lock (u)
                    {
                        foreach (Anagrafica.CUfficio uff in u.Uffici)
                        {
                            tmpSQL.Append(",");
                            tmpSQL.Append(Databases.GetID(uff));
                        }
                    }

                    tmpSQL.Append(") ");
                    tmpSQL.Append("UNION ");
                    tmpSQL.Append("SELECT [Collegamento] FROM [tbl_CollegamentiXUtente] WHERE [Utente]=");
                    tmpSQL.Append(Databases.GetID(u));
                    tmpSQL.Append(") GROUP BY [Collegamento]");
                    tmpSQL.Append(")");
                    tmpSQL.Append(")");
                    ret = DMD.Strings.Combine(ret, tmpSQL.ToString(), " AND ");
                }

                return ret;
            }
        }
    }
}