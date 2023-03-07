using System;
using DMD;
using DMD.Databases;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella dei conti online
        /// </summary>
        [Serializable]
        public class ContoOnlineCursor 
            : minidom.Databases.DBObjectCursor<ContoOnline>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorField<int> m_IDContoCorrente = new DBCursorField<int>("IDContoCorrente");
            private DBCursorStringField m_NomeConto = new DBCursorStringField("NomeConto");
            private DBCursorField<int> m_DataInizio = new DBCursorField<int>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Sito = new DBCursorStringField("Sito");
            private DBCursorStringField m_Account = new DBCursorStringField("Account");
            private DBCursorStringField m_Password = new DBCursorStringField("Password");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ContoOnlineCursor()
            {
            }

            /// <summary>
            /// Campo Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// Campo IDContoCorrente
            /// </summary>
            public DBCursorField<int> IDContoCorrente
            {
                get
                {
                    return m_IDContoCorrente;
                }
            }

            /// <summary>
            /// Campo NomeConto
            /// </summary>
            public DBCursorStringField NomeConto
            {
                get
                {
                    return m_NomeConto;
                }
            }

            /// <summary>
            /// Campo DataInizio
            /// </summary>
            public DBCursorField<int> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            /// <summary>
            /// Campo DataFine
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Campo Sito
            /// </summary>
            public DBCursorStringField Sito
            {
                get
                {
                    return m_Sito;
                }
            }

            /// <summary>
            /// Campo Account
            /// </summary>
            public DBCursorStringField Account
            {
                get
                {
                    return m_Account;
                }
            }

            /// <summary>
            /// Campo Password
            /// </summary>
            public DBCursorStringField Password
            {
                get
                {
                    return m_Password;
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
                return minidom.Anagrafica.ContiOnline; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_ContiOnline";
            //}
        }
    }
}