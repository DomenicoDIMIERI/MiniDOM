using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella dei documenti caricati per una pratica
    /// </summary>
    /// <remarks></remarks>
        public class CDocumentoPraticaCaricatoCursor : Databases.DBObjectCursor<CDocumentoPraticaCaricato>
        {
            private DBCursorField<int> m_IDDocumento; // [INT] ID del tipo di documento (CDocumentoPerPratica)
            private DBCursorField<int> m_IDPratica; // ID della pratica
            private DBCursorField<DateTime> m_DataCaricamento; // [Date] Data ed ora di caricamento
            private DBCursorField<int> m_IDOperatoreCaricamento; // [INT] ID dell'operatore che ha caricato il documento
            private DBCursorStringField m_NomeOperatoreCaricamento;
            private DBCursorField<DateTime> m_DataInizioSpedizione; // [Date] Data di inizio spedizione
            private DBCursorField<int> m_IDOperatoreSpedizione; // [INT] ID dell'operatore che ha preso in carico la spedizione
            private DBCursorStringField m_NomeOperatoreSpedizione;
            private DBCursorField<DateTime> m_DataConsegna; // [Date] Data di consegna
            private DBCursorField<int> m_IDOperatoreConsegna; // [INT] ID dell'operatore che consegnato il documento
            private DBCursorStringField m_NomeOperatoreConsegna;
            private DBCursorField<bool> m_Firmato; // [BOOL] Se vero indica che il documento è stato firmato
            private DBCursorField<int> m_StatoConsegna; // [INT] 0 in preaccettazione, 1 in gestione, 2 spedito, 3 fermo, 4 in consegna, 5 consegnato, 255 errore
            private DBCursorStringField m_Note; // [TEXT]
            private DBCursorField<int> m_Progressivo = new DBCursorField<int>("Progressivo");
            private DBCursorField<bool> m_Verificato = new DBCursorField<bool>("Verificato");

            public CDocumentoPraticaCaricatoCursor()
            {
                m_IDDocumento = new DBCursorField<int>("Documento");
                m_IDPratica = new DBCursorField<int>("Pratica");
                m_DataCaricamento = new DBCursorField<DateTime>("DataCaricamento");
                m_IDOperatoreCaricamento = new DBCursorField<int>("IDOpCaricamento");
                m_NomeOperatoreCaricamento = new DBCursorStringField("NmOpCaricamento");
                m_DataInizioSpedizione = new DBCursorField<DateTime>("DataInizioSpedizione");
                m_IDOperatoreSpedizione = new DBCursorField<int>("IDOpSpedizione");
                m_NomeOperatoreSpedizione = new DBCursorStringField("NmOpSpedizione");
                m_DataConsegna = new DBCursorField<DateTime>("DataConsegna");
                m_IDOperatoreConsegna = new DBCursorField<int>("IDOpConsegna");
                m_NomeOperatoreConsegna = new DBCursorStringField("NmOpConsegna");
                m_Firmato = new DBCursorField<bool>("Firmato");
                m_StatoConsegna = new DBCursorField<int>("StatoConsegna");
                m_Note = new DBCursorStringField("Notes");
            }

            public DBCursorField<bool> Verificato
            {
                get
                {
                    return m_Verificato;
                }
            }

            public DBCursorField<int> Progressivo
            {
                get
                {
                    return m_Progressivo;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public DBCursorField<int> IDDocumento
            {
                get
                {
                    return m_IDDocumento;
                }
            }

            public DBCursorField<int> IDPratica
            {
                get
                {
                    return m_IDPratica;
                }
            }

            public DBCursorField<DateTime> DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }
            }

            public DBCursorField<int> IDOperatoreCaricamento
            {
                get
                {
                    return m_IDOperatoreCaricamento;
                }
            }

            public DBCursorStringField NomeOperatoreCaricamento
            {
                get
                {
                    return m_NomeOperatoreCaricamento;
                }
            }

            public DBCursorField<DateTime> DataInizioSpedizione
            {
                get
                {
                    return m_DataInizioSpedizione;
                }
            }

            public DBCursorField<int> IDOperatoreSpedizione
            {
                get
                {
                    return m_IDOperatoreSpedizione;
                }
            }

            public DBCursorStringField NomeOperatoreSpedizione
            {
                get
                {
                    return m_NomeOperatoreSpedizione;
                }
            }

            public DBCursorField<DateTime> DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }
            }

            public DBCursorField<int> IDOperatoreConsegna
            {
                get
                {
                    return m_IDOperatoreConsegna;
                }
            }

            public DBCursorStringField NomeOperatoreConsegna
            {
                get
                {
                    return m_NomeOperatoreConsegna;
                }
            }

            public DBCursorField<bool> Firmato
            {
                get
                {
                    return m_Firmato;
                }
            }

            public DBCursorField<int> StatoConsegna
            {
                get
                {
                    return m_StatoConsegna;
                }
            }

            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CDocumentoPraticaCaricato();
            }

            public override string GetTableName()
            {
                return "tbl_DocXPrat";
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }
        }
    }
}