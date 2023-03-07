using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CSogliePremiCursor : Databases.DBObjectCursor<CSogliaPremio>
        {
            private DBCursorField<int> m_SetPremiID = new DBCursorField<int>("SetPremi");
            private DBCursorField<decimal> m_Soglia = new DBCursorField<decimal>("Soglia");
            private DBCursorField<decimal> m_Fisso = new DBCursorField<decimal>("Fisso");
            private DBCursorField<double> m_PercSuML = new DBCursorField<double>("PercSuML");
            private DBCursorField<double> m_PercSuProvvAtt = new DBCursorField<double>("PercSuProvvAtt");
            private DBCursorField<double> m_PercSuNetto = new DBCursorField<double>("PercSuNetto");

            public CSogliePremiCursor()
            {
            }

            public DBCursorField<int> SetPremiID
            {
                get
                {
                    return m_SetPremiID;
                }
            }

            public DBCursorField<decimal> Soglia
            {
                get
                {
                    return m_Soglia;
                }
            }

            public DBCursorField<decimal> Fisso
            {
                get
                {
                    return m_Fisso;
                }
            }

            public DBCursorField<double> PercSuML
            {
                get
                {
                    return m_PercSuML;
                }
            }

            public DBCursorField<double> PercSuProvvAtt
            {
                get
                {
                    return m_PercSuProvvAtt;
                }
            }

            public DBCursorField<double> PercSuNetto
            {
                get
                {
                    return m_PercSuNetto;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_TeamManagers_SogliePremi";
            }
        }
    }
}