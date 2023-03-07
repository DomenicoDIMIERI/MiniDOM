using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle Categorie Prodotto
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CCategorieProdottoCursor : Databases.DBObjectCursor<CCategoriaProdotto>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_NomeGruppo = new DBCursorStringField("NomeGruppo");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");

            public CCategorieProdottoCursor()
            {
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField NomeGruppo
            {
                get
                {
                    return m_NomeGruppo;
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
                return new CCategoriaProdotto();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDCatProd";
            }

            protected override Sistema.CModule GetModule()
            {
                return CategorieProdotto.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}