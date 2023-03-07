
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella dei documenti caricabili per prodotto
    /// </summary>
    /// <remarks></remarks>
        public class CDocumentiXGruppoProdottiCursor : Databases.DBObjectCursor<CDocumentoXGruppoProdotti>
        {
            private DBCursorField<int> m_IDGruppoProdotti = new DBCursorField<int>("GruppoProdotti");
            private DBCursorField<int> m_IDDocumento = new DBCursorField<int>("Documento");
            private DBCursorField<DocumentoXProdottoDisposition> m_Disposizione = new DBCursorField<DocumentoXProdottoDisposition>("Disposizione");
            private DBCursorField<bool> m_Richiesto = new DBCursorField<bool>("Richiesto");
            private DBCursorField<int> m_IDStatoPratica = new DBCursorField<int>("IDStatoPratica");
            private DBCursorField<int> m_Progressivo = new DBCursorField<int>("Progressivo");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Espressione = new DBCursorStringField("Espressione");
            private DBCursorField<VincoliProdottoFlags> m_Flags = new DBCursorField<VincoliProdottoFlags>("Flags");

            public CDocumentiXGruppoProdottiCursor()
            {
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField Espressione
            {
                get
                {
                    return m_Espressione;
                }
            }

            public DBCursorField<VincoliProdottoFlags> Flags
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

            public DBCursorField<int> Progressivo
            {
                get
                {
                    return m_Progressivo;
                }
            }

            public DBCursorField<int> IDGruppoProdotti
            {
                get
                {
                    return m_IDGruppoProdotti;
                }
            }

            public DBCursorField<int> IDDocumento
            {
                get
                {
                    return m_IDDocumento;
                }
            }

            public DBCursorField<DocumentoXProdottoDisposition> Disposizione
            {
                get
                {
                    return m_Disposizione;
                }
            }

            public DBCursorField<bool> Richiesto
            {
                get
                {
                    return m_Richiesto;
                }
            }

            public DBCursorField<int> IDStatoPratica
            {
                get
                {
                    return m_IDStatoPratica;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_DocumentiXGruppoProdotti";
            }
        }
    }
}