using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom.repositories;
using static minidom.Sistema;


namespace minidom
{
    public partial class ADV
    {


        /// <summary>
        /// Cursore sulla tabella dei <see cref="CRisultatoCampagna"/>
        /// </summary>
        [Serializable]
        public class CRisultatoCampagnaCursor 
            : minidom.Databases.DBObjectCursor<CRisultatoCampagna>
        {
            private DBCursorField<int> m_IDCampagna = new DBCursorField<int>("IDCampagna");
            private DBCursorStringField m_NomeCampagna = new DBCursorStringField("NomeCampagna");
            private DBCursorField<int> m_IDDestinatario = new DBCursorField<int>("IDDestinatario");
            private DBCursorStringField m_NomeDestinatario = new DBCursorStringField("NomeDestinatario");
            private DBCursorField<StatoMessaggioCampagna> m_StatoMessaggio = new DBCursorField<StatoMessaggioCampagna>("StatoMessaggio");
            private DBCursorField<DateTime> m_DataSpedizione = new DBCursorField<DateTime>("DataSpedizione");
            private DBCursorStringField m_NomeMezzoSpedizione = new DBCursorStringField("NomeMezzoSpedizione");
            private DBCursorStringField m_StatoSpedizione = new DBCursorStringField("StatoSpedizione");
            private DBCursorField<DateTime> m_DataConsegna = new DBCursorField<DateTime>("DataConsegna");
            private DBCursorField<DateTime> m_DataLettura = new DBCursorField<DateTime>("DataLettura");
            private DBCursorField<TipoCampagnaPubblicitaria> m_TipoCampagna = new DBCursorField<TipoCampagnaPubblicitaria>("TipoCampagna");
            private DBCursorField<DateTime> m_DataEsecuzione = new DBCursorField<DateTime>("DataEsecuzione");
            private DBCursorStringField m_MessageID = new DBCursorStringField("MessageID");
            private DBCursorStringField m_IndirizzoDestinatario = new DBCursorStringField("IndirizzoDestinatario");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRisultatoCampagnaCursor()
            {
            }

            //protected override CDBConnection GetConnection()
            //{
            //    return CustomerCalls.CRM.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.ADV.RisultatiCampagna; //.Module;
            }

            /// <summary>
            /// IndirizzoDestinatario
            /// </summary>
            public DBCursorStringField IndirizzoDestinatario
            {
                get
                {
                    return m_IndirizzoDestinatario;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_ADVResults";
            //}

            /// <summary>
            /// MessageID
            /// </summary>
            public DBCursorStringField MessageID
            {
                get
                {
                    return m_MessageID;
                }
            }

            /// <summary>
            /// IDCampagna
            /// </summary>
            public DBCursorField<int> IDCampagna
            {
                get
                {
                    return m_IDCampagna;
                }
            }

            /// <summary>
            /// NomeCampagna
            /// </summary>
            public DBCursorStringField NomeCampagna
            {
                get
                {
                    return m_NomeCampagna;
                }
            }

            /// <summary>
            /// IDDestinatario
            /// </summary>
            public DBCursorField<int> IDDestinatario
            {
                get
                {
                    return m_IDDestinatario;
                }
            }

            /// <summary>
            /// NomeDestinatario
            /// </summary>
            public DBCursorStringField NomeDestinatario
            {
                get
                {
                    return m_NomeDestinatario;
                }
            }

            /// <summary>
            /// StatoMessaggio
            /// </summary>
            public DBCursorField<StatoMessaggioCampagna> StatoMessaggio
            {
                get
                {
                    return m_StatoMessaggio;
                }
            }

            /// <summary>
            /// DataSpedizione
            /// </summary>
            public DBCursorField<DateTime> DataSpedizione
            {
                get
                {
                    return m_DataSpedizione;
                }
            }

            /// <summary>
            /// NomeMezzoSpedizione
            /// </summary>
            public DBCursorStringField NomeMezzoSpedizione
            {
                get
                {
                    return m_NomeMezzoSpedizione;
                }
            }

            /// <summary>
            /// StatoSpedizione
            /// </summary>
            public DBCursorStringField StatoSpedizione
            {
                get
                {
                    return m_StatoSpedizione;
                }
            }

            /// <summary>
            /// DataConsegna
            /// </summary>
            public DBCursorField<DateTime> DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }
            }

            /// <summary>
            /// DataLettura
            /// </summary>
            public DBCursorField<DateTime> DataLettura
            {
                get
                {
                    return m_DataLettura;
                }
            }

            /// <summary>
            /// TipoCampagna
            /// </summary>
            public DBCursorField<TipoCampagnaPubblicitaria> TipoCampagna
            {
                get
                {
                    return m_TipoCampagna;
                }
            }

            /// <summary>
            /// DataEsecuzione
            /// </summary>
            public DBCursorField<DateTime> DataEsecuzione
            {
                get
                {
                    return m_DataEsecuzione;
                }
            }
        }
    }
}