using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CRichiestaAssegniCursor : Databases.DBObjectCursor<CRichiestaAssegni>
        {
            private DBCursorStringField m_Banca;
            private DBCursorStringField m_NomeRichiedente;
            private DBCursorStringField m_CognomeRichiedente;
            private DBCursorStringField m_IndirizzoRichiedente;
            private DBCursorStringField m_Dipendenza;
            private DBCursorField<DateTime> m_Data;
            private DBCursorField<DateTime> m_PerCassa;
            private DBCursorField<DateTime> m_ConAddebitoSuCC;
            private DBCursorStringField m_NumeroContoCorrente;
            private DBCursorStringField m_IntestazioneContoCorrente;

            public CRichiestaAssegniCursor()
            {
                m_Banca = new DBCursorStringField("Banca");
                m_NomeRichiedente = new DBCursorStringField("NomeRichiedente");
                m_CognomeRichiedente = new DBCursorStringField("CognomeRichiedente");
                m_IndirizzoRichiedente = new DBCursorStringField("IndirizzoRichiedente");
                m_Dipendenza = new DBCursorStringField("Dipendenza");
                m_Data = new DBCursorField<DateTime>("Data");
                m_PerCassa = new DBCursorField<DateTime>("PerCassa");
                m_ConAddebitoSuCC = new DBCursorField<DateTime>("ConAddebitoSuCC");
                m_NumeroContoCorrente = new DBCursorStringField("NumeroCCBancario");
                m_IntestazioneContoCorrente = new DBCursorStringField("IntestazioneCC");
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return RichiesteAssegni.Module;
            }

            public override string GetTableName()
            {
                return "tbl_RichiesteAssegniCircolari";
            }
        }
    }
}