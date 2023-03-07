
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore che consente di recuperare tutte le tabelle finanziarie associate ad un prodotto
    /// </summary>
    /// <remarks></remarks>
        public class CProdottoXTabellaAssCursor : Databases.DBObjectCursor<CProdottoXTabellaAss>
        {
            private DBCursorField<int> m_ProdottoID;
            private DBCursorStringField m_Descrizione;
            private DBCursorField<int> m_RischioVitaID;
            private DBCursorField<int> m_RischioImpiegoID;
            private DBCursorField<int> m_RischioCreditoID;

            public CProdottoXTabellaAssCursor()
            {
                m_ProdottoID = new DBCursorField<int>("Prodotto");
                m_Descrizione = new DBCursorStringField("Descrizione");
                m_RischioVitaID = new DBCursorField<int>("RischioVita");
                m_RischioImpiegoID = new DBCursorField<int>("RischioImpiego");
                m_RischioCreditoID = new DBCursorField<int>("RischioCredito");
            }

            public DBCursorField<int> ProdottoID
            {
                get
                {
                    return m_ProdottoID;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<int> RischioVitaID
            {
                get
                {
                    return m_RischioVitaID;
                }
            }

            public DBCursorField<int> RischioImpiegoID
            {
                get
                {
                    return m_RischioImpiegoID;
                }
            }

            public DBCursorField<int> RischioCreditoID
            {
                get
                {
                    return m_RischioCreditoID;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProdottoXTabellaAss();
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabAss";
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