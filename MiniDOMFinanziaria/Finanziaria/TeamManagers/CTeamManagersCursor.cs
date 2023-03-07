using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CTeamManagersCursor : Databases.DBObjectCursorPO<CTeamManager>
        {
            private DBCursorStringField m_Nominativo = new DBCursorStringField("Nominativo");
            private DBCursorField<int> m_IDListinoPredefinito = new DBCursorField<int>("ListinoPredefinito");
            private DBCursorField<int> m_IDReferente = new DBCursorField<int>("Referente");
            private DBCursorField<int> m_IDUtente = new DBCursorField<int>("Utente");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("Persona");
            private DBCursorStringField m_Rapporto = new DBCursorStringField("Rapporto");
            private DBCursorField<DateTime> m_DataInizioRapporto = new DBCursorField<DateTime>("DataInizioRapporto");
            private DBCursorField<DateTime> m_DataFineRapporto = new DBCursorField<DateTime>("DataFineRapporto");
            private DBCursorField<bool> m_SetPremiPersonalizzato = new DBCursorField<bool>("SetPremiPersonalizzato");
            private DBCursorField<int> m_IDSetPremiSpecificato = new DBCursorField<int>("SetPremi");
            private DBCursorField<StatoTeamManager> m_StatoTeamManager = new DBCursorField<StatoTeamManager>("StatoTeamManager");
            private bool m_OnlyValid;

            public CTeamManagersCursor()
            {
                m_OnlyValid = false;
            }

            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    m_OnlyValid = value;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public DBCursorStringField Nominativo
            {
                get
                {
                    return m_Nominativo;
                }
            }

            public DBCursorField<int> IDListinoPredefinito
            {
                get
                {
                    return m_IDListinoPredefinito;
                }
            }

            public DBCursorField<int> IDReferente
            {
                get
                {
                    return m_IDReferente;
                }
            }

            public DBCursorField<int> IDUtente
            {
                get
                {
                    return m_IDUtente;
                }
            }

            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            public DBCursorStringField Rapporto
            {
                get
                {
                    return m_Rapporto;
                }
            }

            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizioRapporto;
                }
            }

            public DBCursorField<StatoTeamManager> StatoTeamManager
            {
                get
                {
                    return m_StatoTeamManager;
                }
            }

            public DBCursorField<bool> SetPremiPersonalizzato
            {
                get
                {
                    return m_SetPremiPersonalizzato;
                }
            }

            public DBCursorField<int> IDSetPremiSpecificato
            {
                get
                {
                    return m_IDSetPremiSpecificato;
                }
            }

            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFineRapporto;
                }
            }

            public override string GetTableName()
            {
                return "tbl_TeamManagers";
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CTeamManager();
            }

            protected override Sistema.CModule GetModule()
            {
                return TeamManagers.Module;
            }

            public override string GetWherePart()
            {
                string wherePart = base.GetWherePart();
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "([StatoTeamManager] = " + ((int)Finanziaria.StatoTeamManager.STATO_ATTIVO).ToString() + ")", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizioRapporto] Is Null) Or ([DataInizioRapporto]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFineRapporto] Is Null) Or ([DataFineRapporto]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                }

                return wherePart;
            }
        }
    }
}