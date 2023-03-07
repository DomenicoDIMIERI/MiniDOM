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
        /// Cursore sulla tabella degli intestatari dei conti corrente
        /// </summary>
        [Serializable]
        public class IntestatarioContoCorrenteCursor 
            : minidom.Databases.DBObjectCursor<IntestatarioContoCorrente>
        {
            private DBCursorField<int> m_IDContoCorrente = new DBCursorField<int>("IDContoCorrente");
            private DBCursorStringField m_NomeConto = new DBCursorStringField("NomeConto");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public IntestatarioContoCorrenteCursor()
            {
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
            /// Campo IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// Campo NomePersona
            /// </summary>
            public DBCursorStringField NomePersona
            {
                get
                {
                    return m_NomePersona;
                }
            }

            /// <summary>
            /// Campo DataInizio
            /// </summary>
            public DBCursorField<DateTime> DataInizio
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

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            //protected override Sistema.CModule GetModule()
            //{
            //    return null;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ContiCorrente.Intestatari;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_ContiCorrentiInt";
            //}
        }
    }
}