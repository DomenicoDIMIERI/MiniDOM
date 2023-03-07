
namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Cursore che consente di recuperare tutte le tabelle finanziarie associate ad un prodotto
    /// </summary>
    /// <remarks></remarks>
        public class CRigheTEGMaxCursor : Databases.DBObjectCursor<CRigaTEGMax>
        {
            private DBCursorField<int> m_TabellaID;
            private DBCursorField<double> m_ValoreSoglia;

            public CRigheTEGMaxCursor()
            {
                m_TabellaID = new DBCursorField<int>("Tabella");
                m_ValoreSoglia = new DBCursorField<double>("ValoreSoglia");
            }

            protected override Sistema.CModule GetModule()
            {
                return TabelleTEGMax.Module; // modCQSPDTblTEGMax
            }

            public DBCursorField<int> TabellaID
            {
                get
                {
                    return m_TabellaID;
                }
            }

            public DBCursorField<double> ValoreSoglia
            {
                get
                {
                    return m_ValoreSoglia;
                }
            }

            public override string GetTableName()
            {
                return "tbl_FIN_TEGMaxI";
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CRigaTEGMax();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}