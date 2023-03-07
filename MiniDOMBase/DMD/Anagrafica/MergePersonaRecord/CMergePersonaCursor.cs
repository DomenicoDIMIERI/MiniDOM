using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle unioni
        /// </summary>
        [Serializable]
        public class CMergePersonaCursor 
            : minidom.Databases. DBObjectCursor<CMergePersona>
        {
            private DBCursorField<int> m_IDPersona1 = new DBCursorField<int>("IDPersona1");
            private DBCursorStringField m_NomePersona1 = new DBCursorStringField("NomePersona1");
            private DBCursorField<int> m_IDPersona2 = new DBCursorField<int>("IDPersona2");
            private DBCursorStringField m_NomePersona2 = new DBCursorStringField("NomePersona2");
            private DBCursorField<DateTime> m_DataOperazione = new DBCursorField<DateTime>("DataOperazione");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMergePersonaCursor()
            {
            }

            /// <summary>
            /// IDPersona1
            /// </summary>
            public DBCursorField<int> IDPersona1
            {
                get
                {
                    return m_IDPersona1;
                }
            }

            /// <summary>
            /// NomePersona1
            /// </summary>
            public DBCursorStringField NomePersona1
            {
                get
                {
                    return m_NomePersona1;
                }
            }

            /// <summary>
            /// IDPersona2
            /// </summary>
            public DBCursorField<int> IDPersona2
            {
                get
                {
                    return m_IDPersona2;
                }
            }

            /// <summary>
            /// NomePersona2
            /// </summary>
            public DBCursorStringField NomePersona2
            {
                get
                {
                    return m_NomePersona2;
                }
            }

            /// <summary>
            /// DataOperazione
            /// </summary>
            public DBCursorField<DateTime> DataOperazione
            {
                get
                {
                    return m_DataOperazione;
                }
            }

            /// <summary>
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.MergePersone; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_MergePersone";
            //}
        }
    }
}