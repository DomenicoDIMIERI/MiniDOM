
namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Cursore sulla tabella degli stati pratica
    /// </summary>
    /// <remarks></remarks>
        public class CStatoPraticaCursor : Databases.DBObjectCursor<CStatoPratica>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            // Private m_CanChangeOfferta As New DBCursorField(Of Boolean)("CanChangeOfferta")
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");
            private DBCursorField<StatoPraticaEnum> m_MacroStato = new DBCursorField<StatoPraticaEnum>("OldStatus");
            private DBCursorField<int> m_IDDefaultTarget = new DBCursorField<int>("IDDefaultTarget");
            private DBCursorField<int> m_GiorniAvviso = new DBCursorField<int>("GiorniAvviso");
            private DBCursorField<int> m_GiorniStallo = new DBCursorField<int>("GiorniStallo");
            // Private m_Vincolante As New DBCursorField(Of Boolean)("Vincolante")
            private DBCursorField<StatoPraticaFlags> m_Flags = new DBCursorField<StatoPraticaFlags>("Flags");

            public CStatoPraticaCursor()
            {
            }

            public override string GetTableName()
            {
                return "tbl_PraticheSTS";
            }

            public DBCursorField<StatoPraticaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<int> GiorniAvviso
            {
                get
                {
                    return m_GiorniAvviso;
                }
            }

            public DBCursorField<int> GiorniStallo
            {
                get
                {
                    return m_GiorniStallo;
                }
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

            public DBCursorField<StatoPraticaEnum> MacroStato
            {
                get
                {
                    return m_MacroStato;
                }
            }

            // Public ReadOnly Property CanChangeOfferta As DBCursorField(Of Boolean)
            // Get
            // Return Me.m_CanChangeOfferta
            // End Get
            // End Property

            public DBCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
                }
            }

            public DBCursorField<int> IDDefaultTarget
            {
                get
                {
                    return m_IDDefaultTarget;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return StatiPratica.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}