using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle relazioni Azienda x convenzione
    /// </summary>
    /// <remarks></remarks>
        public class AziendaXConvenzioneCursor : Databases.DBObjectCursor<AziendaXConvenzione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorField<int> m_IDConvenzione = new DBCursorField<int>("IDConvenzione");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");

            public AziendaXConvenzioneCursor()
            {
            }

            public DBCursorField<int> IDAzienda
            {
                get
                {
                    return m_IDAzienda;
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
                return new AziendaXConvenzione();
            }

            public override string GetTableName()
            {
                return "tbl_FIN_AzieXConvenzioni";
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