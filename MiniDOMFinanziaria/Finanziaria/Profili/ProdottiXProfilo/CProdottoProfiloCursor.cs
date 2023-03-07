
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle relazioni tra profili e prodotti
    /// </summary>
    /// <remarks></remarks>
        public class CProdottoProfiloCursor : Databases.DBObjectCursor<CProdottoProfilo>
        {
            private DBCursorField<int> m_IDProfilo;
            private DBCursorField<int> m_IDProdotto;
            private DBCursorField<IncludeModes> m_Azione;
            private DBCursorField<double> m_Spread;

            public CProdottoProfiloCursor()
            {
                m_IDProdotto = new DBCursorField<int>("Prodotto");
                m_IDProfilo = new DBCursorField<int>("Preventivatore");
                m_Azione = new DBCursorField<IncludeModes>("Azione");
                m_Spread = new DBCursorField<double>("Spread");
            }

            /// <summary>
        /// ID del profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DBCursorField<int> IDProfilo
            {
                get
                {
                    return m_IDProfilo;
                }
            }

            /// <summary>
        /// ID del prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_IDProdotto;
                }
            }

            /// <summary>
        /// Relazione tra profilo e prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DBCursorField<IncludeModes> Azione
            {
                get
                {
                    return m_Azione;
                }
            }

            /// <summary>
        /// Spread rispetto al genitore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DBCursorField<double> Spread
            {
                get
                {
                    return m_Spread;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProdottoProfilo();
            }

            public override string GetTableName()
            {
                return "tbl_PreventivatoriXProdotto";
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