
namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Cursore sulla tabella delle regole sugli stati pratica
    /// </summary>
    /// <remarks></remarks>
        public class CStatoPratRuleCursor : Databases.DBObjectCursor<CStatoPratRule>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_IDSource = new DBCursorField<int>("IDSource");
            private DBCursorField<int> m_IDTarget = new DBCursorField<int>("IDTarget");
            private DBCursorField<int> m_Order = new DBCursorField<int>("Order");
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");
            private DBCursorField<FlagsRegolaStatoPratica> m_Flags = new DBCursorField<FlagsRegolaStatoPratica>("Attivo");

            public CStatoPratRuleCursor()
            {
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CStatoPratRule();
            }

            public DBCursorField<FlagsRegolaStatoPratica> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public override string GetTableName()
            {
                return "tbl_PraticheSTR";
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<int> IDSource
            {
                get
                {
                    return m_IDSource;
                }
            }

            public DBCursorField<int> IDTarget
            {
                get
                {
                    return m_IDTarget;
                }
            }

            public DBCursorField<int> Order
            {
                get
                {
                    return m_Order;
                }
            }

            public DBCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return StatiPratRules.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}