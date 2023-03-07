
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore che consente di recuperare tutte le tabelle dei TEG massimi
    /// </summary>
    /// <remarks></remarks>
        public class CTabelleTEGMaxCursor : Databases.DBObjectCursor<CTabellaTEGMax>
        {
            private DBCursorField<int> m_ProdottoID;
            private DBCursorField<int> m_CessionarioID;
            private DBCursorStringField m_Nome;
            private DBCursorStringField m_NomeCessionario;
            private DBCursorStringField m_Descrizione;
            private DBCursorField<bool> m_Visible = new DBCursorField<bool>("Visible");

            public CTabelleTEGMaxCursor()
            {
                m_ProdottoID = new DBCursorField<int>("Prodotto");
                m_CessionarioID = new DBCursorField<int>("Cessionario");
                m_Nome = new DBCursorStringField("Nome");
                m_NomeCessionario = new DBCursorStringField("NomeCessionario");
                m_Descrizione = new DBCursorStringField("Descrizione");
            }

            public override string GetTableName()
            {
                return "tbl_FIN_TEGMax";
            }

            protected override Sistema.CModule GetModule()
            {
                return TabelleTEGMax.Module;
            }

            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_ProdottoID;
                }
            }

            public DBCursorField<bool> Visible
            {
                get
                {
                    return m_Visible;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
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

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CTabellaTEGMax();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}