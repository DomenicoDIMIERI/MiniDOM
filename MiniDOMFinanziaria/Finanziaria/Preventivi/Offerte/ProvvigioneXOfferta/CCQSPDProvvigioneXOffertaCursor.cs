using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle offerte
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CCQSPDProvvigioneXOffertaCursor : Databases.DBObjectCursor<CCQSPDProvvigioneXOfferta>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_IDOfferta = new DBCursorField<int>("IDOfferta");
            private DBCursorField<int> m_IDTipoProvvigione = new DBCursorField<int>("IDTipoProvvigione");
            private DBCursorField<CQSPDTipoSoggetto> m_PagataDa = new DBCursorField<CQSPDTipoSoggetto>("PagataDa");
            private DBCursorField<CQSPDTipoSoggetto> m_PagataA = new DBCursorField<CQSPDTipoSoggetto>("PagataA");
            private DBCursorField<CQSPDTipoProvvigioneEnum> m_TipoCalcolo = new DBCursorField<CQSPDTipoProvvigioneEnum>("TipoCalcolo");
            private DBCursorField<double> m_Percentuale = new DBCursorField<double>("Percentuale");
            private DBCursorField<double> m_Fisso = new DBCursorField<double>("Fisso");
            private DBCursorStringField m_Formula = new DBCursorStringField("Formula");
            private DBCursorField<ProvvigioneXOffertaFlags> m_Flags = new DBCursorField<ProvvigioneXOffertaFlags>("Flags");
            private DBCursorField<int> m_IDCedente = new DBCursorField<int>("IDCedente");
            private DBCursorStringField m_NomeCedente = new DBCursorStringField("NomeCedente");
            private DBCursorField<int> m_IDRicevente = new DBCursorField<int>("IDRicevente");
            private DBCursorStringField m_NomeRicevente = new DBCursorStringField("NomeRicevente");
            private DBCursorField<double> m_BaseDiCalcolo = new DBCursorField<double>("BaseDiCalcolo");
            private DBCursorField<double> m_Valore = new DBCursorField<double>("Valore");
            private DBCursorField<double> m_ValorePagato = new DBCursorField<double>("ValorePagato");
            private DBCursorField<DateTime> m_DataPagamento = new DBCursorField<DateTime>("DataPagamento");

            public CCQSPDProvvigioneXOffertaCursor()
            {
            }

            public DBCursorField<int> IDCedente
            {
                get
                {
                    return m_IDCedente;
                }
            }

            public DBCursorStringField NomeCedente
            {
                get
                {
                    return m_NomeCedente;
                }
            }

            public DBCursorField<int> IDRicevente
            {
                get
                {
                    return m_IDRicevente;
                }
            }

            public DBCursorStringField NomeRicevente
            {
                get
                {
                    return m_NomeRicevente;
                }
            }

            public DBCursorField<double> BaseDiCalcolo
            {
                get
                {
                    return m_BaseDiCalcolo;
                }
            }

            public DBCursorField<double> Valore
            {
                get
                {
                    return m_Valore;
                }
            }

            public DBCursorField<double> ValorePagato
            {
                get
                {
                    return m_ValorePagato;
                }
            }

            public DBCursorField<DateTime> DataPagamento
            {
                get
                {
                    return m_DataPagamento;
                }
            }

            public DBCursorField<int> IDTipoProvvigione
            {
                get
                {
                    return m_IDTipoProvvigione;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<int> IDOfferta
            {
                get
                {
                    return m_IDOfferta;
                }
            }

            public DBCursorField<CQSPDTipoSoggetto> PagataDa
            {
                get
                {
                    return m_PagataDa;
                }
            }

            public DBCursorField<CQSPDTipoSoggetto> PagataA
            {
                get
                {
                    return m_PagataA;
                }
            }

            public DBCursorField<CQSPDTipoProvvigioneEnum> TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }
            }

            public DBCursorField<double> Percentuale
            {
                get
                {
                    return m_Percentuale;
                }
            }

            public DBCursorField<double> Fisso
            {
                get
                {
                    return m_Fisso;
                }
            }

            public DBCursorStringField Formula
            {
                get
                {
                    return m_Formula;
                }
            }

            public DBCursorField<ProvvigioneXOffertaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CCQSPDProvvigioneXOfferta();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDProvvXOfferta";
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