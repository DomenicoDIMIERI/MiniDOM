using System;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Cursore sulla tabella degli sconti
    /// </summary>
    /// <remarks></remarks>
        public class RichiestaApprovazioneGroupCursor : Databases.DBObjectCursorPO<RichiestaApprovazioneGroup>
        {
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<int> m_IDRichiedente = new DBCursorField<int>("IDRichiedente");
            private DBCursorStringField m_NomeRichiedente = new DBCursorStringField("NomeRichiedente");
            private DBCursorField<int> m_IDMotivoRichiesta = new DBCursorField<int>("IDMotivoRichiesta");
            private DBCursorStringField m_Motivo = new DBCursorStringField("Motivo");
            private DBCursorStringField m_DettaglioRichiesta = new DBCursorStringField("DettaglioRichiesta");
            private DBCursorField<int> m_IDSupervisore = new DBCursorField<int>("IDSupervisore");
            private DBCursorField<DateTime> m_DataEsito = new DBCursorField<DateTime>("DataEsito");

            public RichiestaApprovazioneGroupCursor()
            {
            }

            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            public DBCursorField<DateTime> DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }
            }

            public DBCursorField<int> IDRichiedente
            {
                get
                {
                    return m_IDRichiedente;
                }
            }

            public DBCursorStringField NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }
            }

            public DBCursorField<int> IDMotivoRichiesta
            {
                get
                {
                    return m_IDMotivoRichiesta;
                }
            }

            public DBCursorStringField Motivo
            {
                get
                {
                    return m_Motivo;
                }
            }

            public DBCursorStringField DettaglioRichiesta
            {
                get
                {
                    return m_DettaglioRichiesta;
                }
            }

            public DBCursorField<int> IDSupervisore
            {
                get
                {
                    return m_IDSupervisore;
                }
            }

            public DBCursorField<DateTime> DataEsito
            {
                get
                {
                    return m_DataEsito;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return RichiesteApprovazioneGroups.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDGrpRichApp";
            }
        }
    }
}