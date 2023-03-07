
namespace minidom
{
    public partial class Finanziaria
    {
        public class CTipoContrattoCursor : Databases.DBObjectCursorBase<CTipoContratto>
        {
            private DBCursorStringField m_IdTipoContratto = new DBCursorStringField("IdTipoContratto");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("descrizione");

            public CTipoContrattoCursor()
            {
            }

            public DBCursorStringField IdTipoContratto
            {
                get
                {
                    return m_IdTipoContratto;
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
                return TipiContratto.Module;
            }

            public override string GetTableName()
            {
                return "Tipocontratto";
            }
        }
    }
}