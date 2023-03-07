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
        public class CCQSPDTipoProvvigioneCursor : Databases.DBObjectCursor<CCQSPDTipoProvvigione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_IDGruppoProdotti = new DBCursorField<int>("IDGruppoProdotti");
            private DBCursorField<CQSPDTipoSoggetto> m_PagataDa = new DBCursorField<CQSPDTipoSoggetto>("PagataDa");
            private DBCursorField<CQSPDTipoSoggetto> m_PagataA = new DBCursorField<CQSPDTipoSoggetto>("PagataA");
            private DBCursorField<CQSPDTipoProvvigioneEnum> m_TipoCalcolo = new DBCursorField<CQSPDTipoProvvigioneEnum>("TipoCalcolo");
            private DBCursorField<double> m_Percentuale = new DBCursorField<double>("Percentuale");
            private DBCursorField<double> m_Fisso = new DBCursorField<double>("Fisso");
            private DBCursorField<double> m_ValoreMax = new DBCursorField<double>("ValoreMax");
            private DBCursorStringField m_Formula = new DBCursorStringField("Formula");
            private DBCursorField<CQSPDTipoProvvigioneFlags> m_Flags = new DBCursorField<CQSPDTipoProvvigioneFlags>("Flags");

            public CCQSPDTipoProvvigioneCursor()
            {
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<int> IDGruppoProdotti
            {
                get
                {
                    return m_IDGruppoProdotti;
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

            public DBCursorField<double> ValoreMax
            {
                get
                {
                    return m_ValoreMax;
                }
            }

            public DBCursorStringField Formula
            {
                get
                {
                    return m_Formula;
                }
            }

            public DBCursorField<CQSPDTipoProvvigioneFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CCQSPDTipoProvvigione();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDTipiProvvigione";
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