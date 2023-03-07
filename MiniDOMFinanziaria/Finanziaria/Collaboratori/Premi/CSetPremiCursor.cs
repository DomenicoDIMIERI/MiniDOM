using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CSetPremiCursor : Databases.DBObjectCursor
        {
            private DBCursorField<bool> m_AScaglioni = new DBCursorField<bool>("AScaglioni");
            private DBCursorField<TipoIntervalloSetPremi> m_TipoIntervallo = new DBCursorField<TipoIntervalloSetPremi>("");
            private DBCursorField<TipoCalcoloSetPremi> m_TipoCalcolo = new DBCursorField<TipoCalcoloSetPremi>("");

            public CSetPremiCursor()
            {
            }

            public DBCursorField<TipoIntervalloSetPremi> TipoIntervallo
            {
                get
                {
                    return m_TipoIntervallo;
                }
            }

            public DBCursorField<TipoCalcoloSetPremi> TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }
            }

            public DBCursorField<bool> AScaglioni
            {
                get
                {
                    return m_AScaglioni;
                }
            }

            public override string GetTableName()
            {
                return "tbl_TeamManagers_SetPremi";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CSetPremi();
            }
        }
    }
}