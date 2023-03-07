using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public abstract class CTableConstraintCursor : Databases.DBObjectCursor<CTableConstraint>
        {
            private DBCursorStringField m_Espressione = new DBCursorStringField("Espressione");
            private DBCursorField<TypeCode> m_TipoValore = new DBCursorField<TypeCode>("TipoValore");
            private DBCursorField<TableContraints> m_TipoVincolo = new DBCursorField<TableContraints>("TipoVincolo");

            public CTableConstraintCursor()
            {
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public abstract override string GetTableName();

            public DBCursorStringField Espressione
            {
                get
                {
                    return m_Espressione;
                }
            }

            public DBCursorField<TypeCode> TipoValore
            {
                get
                {
                    return m_TipoValore;
                }
            }

            public DBCursorField<TableContraints> TipoVincolo
            {
                get
                {
                    return m_TipoVincolo;
                }
            }
        }
    }
}