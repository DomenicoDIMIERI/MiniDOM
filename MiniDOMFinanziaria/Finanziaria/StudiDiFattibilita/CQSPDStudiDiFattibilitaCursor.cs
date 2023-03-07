using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle consulenze
    /// </summary>
    /// <remarks></remarks>
        public class CQSPDStudiDiFattibilitaCursor : Databases.DBObjectCursorPO<CQSPDStudioDiFattibilita>
        {
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<DateTime> m_OraInizio = new DBCursorField<DateTime>("OraInizio");
            private DBCursorField<DateTime> m_OraFine = new DBCursorField<DateTime>("OraFine");
            private DBCursorField<int> m_IDRichiesta = new DBCursorField<int>("IDRichiesta");
            private DBCursorField<int> m_IDConsulente = new DBCursorField<int>("IDConsulente");
            private DBCursorStringField m_NomeConsulente = new DBCursorStringField("NomeConsulente");
            private DBCursorField<DateTime> m_DecorrenzaPratica = new DBCursorField<DateTime>("DecorrenzaPratica");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");

            public CQSPDStudiDiFattibilitaCursor()
            {
            }

            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            public DBCursorField<DateTime> DecorrenzaPratica
            {
                get
                {
                    return m_DecorrenzaPratica;
                }
            }

            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            public DBCursorField<int> IDRichiesta
            {
                get
                {
                    return m_IDRichiesta;
                }
            }

            public DBCursorField<int> IDConsulente
            {
                get
                {
                    return m_IDConsulente;
                }
            }

            public DBCursorStringField NomeConsulente
            {
                get
                {
                    return m_NomeConsulente;
                }
            }

            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            public DBCursorField<DateTime> OraInizio
            {
                get
                {
                    return m_OraInizio;
                }
            }

            public DBCursorField<DateTime> OraFine
            {
                get
                {
                    return m_OraFine;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CQSPDStudioDiFattibilita();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDGrpConsulenze";
            }

            protected override Sistema.CModule GetModule()
            {
                return Consulenze.Module;
            }
        }
    }
}