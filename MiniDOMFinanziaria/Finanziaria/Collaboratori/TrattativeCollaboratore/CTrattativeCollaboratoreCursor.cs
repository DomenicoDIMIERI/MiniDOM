using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CTrattativeCollaboratoreCursor : Databases.DBObjectCursor<CTrattativaCollaboratore>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<bool> m_Richiesto = new DBCursorField<bool>("Richiesto");
            private DBCursorField<CQSPDTipoProvvigioneEnum> m_TipoCalcolo = new DBCursorField<CQSPDTipoProvvigioneEnum>("TipoCalcolo");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private DBCursorStringField m_NomeCollaboratore = new DBCursorStringField("NomeCollaboratore");
            private DBCursorField<int> m_IDCessionario = new DBCursorField<int>("IDCessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorField<int> m_IDProdotto = new DBCursorField<int>("IDProdotto");
            private DBCursorStringField m_NomeProdotto = new DBCursorStringField("NomeProdotto");
            private DBCursorField<StatoTrattativa> m_StatoTrattativa = new DBCursorField<StatoTrattativa>("StatoTrattativa");
            private DBCursorField<double> m_SpreadProposto = new DBCursorField<double>("SpreadProposto");
            private DBCursorField<double> m_SpreadRichiesto = new DBCursorField<double>("SpreadRichiesto");
            private DBCursorField<double> m_SpreadApprovato = new DBCursorField<double>("SpreadApprovato");
            private DBCursorField<double> m_SpreadFidelizzazione = new DBCursorField<double>("SpreadFidelizzazione");
            private DBCursorField<TrattativaCollaboratoreFlags> m_Flags = new DBCursorField<TrattativaCollaboratoreFlags>("Flags");
            private DBCursorField<double> m_ValoreBase = new DBCursorField<double>("ValoreBase");
            private DBCursorField<double> m_ValoreMax = new DBCursorField<double>("ValoreMax");
            private DBCursorStringField m_Formula = new DBCursorStringField("Formula");
            private DBCursorField<int> m_IDPropostoDa = new DBCursorField<int>("IDPropostoDa");
            private DBCursorField<DateTime> m_PropostoIl = new DBCursorField<DateTime>("PropostoIl");
            private DBCursorField<int> m_IDApprovatoDa = new DBCursorField<int>("IDApprovatoDa");
            private DBCursorField<DateTime> m_ApprovatoIl = new DBCursorField<DateTime>("ApprovatoIl");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");

            public CTrattativeCollaboratoreCursor()
            {
            }

            protected override Sistema.CModule GetModule()
            {
                return TrattativeCollaboratore.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDCollabBUI";
            }

            public DBCursorField<CQSPDTipoProvvigioneEnum> TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }
            }

            public DBCursorField<int> IDCollaboratore
            {
                get
                {
                    return m_IDCollaboratore;
                }
            }

            public DBCursorField<bool> Richiesto
            {
                get
                {
                    return m_Richiesto;
                }
            }

            public DBCursorStringField NomeCollaboratore
            {
                get
                {
                    return m_NomeCollaboratore;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorField<int> IDCessionario
            {
                get
                {
                    return m_IDCessionario;
                }
            }

            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_IDProdotto;
                }
            }

            public DBCursorStringField NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }
            }

            public DBCursorField<StatoTrattativa> StatoTrattativa
            {
                get
                {
                    return m_StatoTrattativa;
                }
            }

            public DBCursorField<double> SpreadProposto
            {
                get
                {
                    return m_SpreadProposto;
                }
            }

            public DBCursorField<double> SpreadRichiesto
            {
                get
                {
                    return m_SpreadRichiesto;
                }
            }

            public DBCursorField<double> SpreadApprovato
            {
                get
                {
                    return m_SpreadApprovato;
                }
            }

            public DBCursorField<double> SpreadFidelizzazione
            {
                get
                {
                    return m_SpreadFidelizzazione;
                }
            }

            public DBCursorField<TrattativaCollaboratoreFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<double> ValoreBase
            {
                get
                {
                    return m_ValoreBase;
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

            public DBCursorField<int> IDPropostoDa
            {
                get
                {
                    return m_IDPropostoDa;
                }
            }

            public DBCursorField<DateTime> PropostoIl
            {
                get
                {
                    return m_PropostoIl;
                }
            }

            public DBCursorField<int> IDApprovatoDa
            {
                get
                {
                    return m_IDApprovatoDa;
                }
            }

            public DBCursorField<DateTime> ApprovatoIl
            {
                get
                {
                    return m_ApprovatoIl;
                }
            }

            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}