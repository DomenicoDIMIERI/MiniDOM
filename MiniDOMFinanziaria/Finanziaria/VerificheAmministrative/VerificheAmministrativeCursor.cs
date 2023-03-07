using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class VerificheAmministrativeCursor : Databases.DBObjectCursor<VerificaAmministrativa>
        {
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<StatoVerificaAmministrativa> m_StatoVerifica = new DBCursorField<StatoVerificaAmministrativa>("StatoVerifica");
            private DBCursorField<EsitoVerificaAmministrativa> m_EsitoVerifica = new DBCursorField<EsitoVerificaAmministrativa>("EsitoVerifica");
            private DBCursorStringField m_DettaglioEsitoVerifica = new DBCursorStringField("DettaglioEsitoVerifica");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDOggettoVerificato = new DBCursorField<int>("IDOggettoVerificato");
            private DBCursorStringField m_TipoOggettoVerificato = new DBCursorStringField("TipoOggettoVerificato");

            public VerificheAmministrativeCursor()
            {
            }

            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            public DBCursorField<StatoVerificaAmministrativa> StatoVerifica
            {
                get
                {
                    return m_StatoVerifica;
                }
            }

            public DBCursorField<EsitoVerificaAmministrativa> EsitoVerifica
            {
                get
                {
                    return m_EsitoVerifica;
                }
            }

            public DBCursorStringField DettaglioEsitoVerifica
            {
                get
                {
                    return m_DettaglioEsitoVerifica;
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

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<int> IDOggettoVerificato
            {
                get
                {
                    return m_IDOggettoVerificato;
                }
            }

            public DBCursorStringField TipoOggettoVerificato
            {
                get
                {
                    return m_TipoOggettoVerificato;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return VerificheAmministrative.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDVerificheAmministrative";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}