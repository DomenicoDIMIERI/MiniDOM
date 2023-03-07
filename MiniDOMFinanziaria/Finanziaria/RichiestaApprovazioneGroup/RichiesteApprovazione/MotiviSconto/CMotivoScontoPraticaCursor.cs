
namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Cursore sulla tabella degli sconti
    /// </summary>
    /// <remarks></remarks>
        public class CMotivoScontoPraticaCursor : Databases.DBObjectCursor<CMotivoScontoPratica>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");

            public CMotivoScontoPraticaCursor()
            {
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

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return MotiviSconto.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDMotiviSconti";
            }
        }
    }
}