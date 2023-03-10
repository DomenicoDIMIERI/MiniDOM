using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle relazioni prodotto x convenzione
    /// </summary>
    /// <remarks></remarks>
        public class CProdottoXConvenzioneCursor : Databases.DBObjectCursor<CProdottoXConvenzione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_IDProdotto = new DBCursorField<int>("IDProdotto");
            private DBCursorField<int> m_IDConvenzione = new DBCursorField<int>("IDConvenzione");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");

            public CProdottoXConvenzioneCursor()
            {
            }

            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_IDProdotto;
                }
            }

            public DBCursorField<int> IDConvenzione
            {
                get
                {
                    return m_IDConvenzione;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
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

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProdottoXConvenzione();
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXConvenzioni";
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}