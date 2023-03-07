using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore sulla tabella <see cref="CSessioneCRM"/>
        /// </summary>
        [Serializable]
        public class CSessioneCRMCursor 
            : Databases.DBObjectCursorPO<CSessioneCRM>
        {
            private DBCursorField<int> m_IDCampagnaCRM = new DBCursorField<int>("IDCampagnaCRM");
            private DBCursorField<int> m_IDUtente = new DBCursorField<int>("IDUtente");
            private DBCursorStringField m_NomeUtente = new DBCursorStringField("NomeUtente");
            private DBCursorField<DateTime> m_Inizio = new DBCursorField<DateTime>("Inizio");
            private DBCursorField<DateTime> m_Fine = new DBCursorField<DateTime>("Fine");
            private DBCursorField<DateTime> m_LastUpdated = new DBCursorField<DateTime>("LastUpdated");
            private DBCursorField<int> m_NumeroTelefonateRisposte = new DBCursorField<int>("NumeroTelefonateRisposte");
            private DBCursorField<int> m_NumeroTelefonateNonRisposte = new DBCursorField<int>("NumeroTelefonateNonRisposte");
            private DBCursorField<int> m_MinutiConversazione = new DBCursorField<int>("MinutiConversazione");
            private DBCursorField<int> m_MinutiAttesa = new DBCursorField<int>("MinutiAttesa");
            private DBCursorField<int> m_NumeroAppuntamentiFissati = new DBCursorField<int>("NumeroAppuntamentiFissati");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_DMDPage = new DBCursorStringField("dmdpage");
            private DBCursorField<int> m_IDSupervisore = new DBCursorField<int>("IDSupervisore");
            private DBCursorStringField m_NomeSupervisore = new DBCursorStringField("NomeSupervisore");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSessioneCRMCursor()
            {
            }

            /// <summary>
            /// LastUpdated
            /// </summary>
            public DBCursorField<DateTime> LastUpdated
            {
                get
                {
                    return m_LastUpdated;
                }
            }

            /// <summary>
            /// IDSupervisore
            /// </summary>
            public DBCursorField<int> IDSupervisore
            {
                get
                {
                    return m_IDSupervisore;
                }
            }

            /// <summary>
            /// NomeSupervisore
            /// </summary>
            public DBCursorStringField NomeSupervisore
            {
                get
                {
                    return m_NomeSupervisore;
                }
            }

            /// <summary>
            /// dmdpage
            /// </summary>
            public DBCursorStringField dmdpage
            {
                get
                {
                    return m_DMDPage;
                }
            }

            /// <summary>
            /// IDCampagnaCRM
            /// </summary>
            public DBCursorField<int> IDCampagnaCRM
            {
                get
                {
                    return m_IDCampagnaCRM;
                }
            }

            /// <summary>
            /// IDUtente
            /// </summary>
            public DBCursorField<int> IDUtente
            {
                get
                {
                    return m_IDUtente;
                }
            }

            /// <summary>
            /// NomeUtente
            /// </summary>
            public DBCursorStringField NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }
            }

            /// <summary>
            /// Inizio
            /// </summary>
            public DBCursorField<DateTime> Inizio
            {
                get
                {
                    return m_Inizio;
                }
            }

            /// <summary>
            /// Fine
            /// </summary>
            public DBCursorField<DateTime> Fine
            {
                get
                {
                    return m_Fine;
                }
            }

            /// <summary>
            /// NumeroTelefonateRisposte
            /// </summary>
            public DBCursorField<int> NumeroTelefonateRisposte
            {
                get
                {
                    return m_NumeroTelefonateRisposte;
                }
            }

            /// <summary>
            /// NumeroTelefonateNonRisposte
            /// </summary>
            public DBCursorField<int> NumeroTelefonateNonRisposte
            {
                get
                {
                    return m_NumeroTelefonateNonRisposte;
                }
            }

            /// <summary>
            /// MinutiConversazione
            /// </summary>
            public DBCursorField<int> MinutiConversazione
            {
                get
                {
                    return m_MinutiConversazione;
                }
            }

            /// <summary>
            /// MinutiAttesa
            /// </summary>
            public DBCursorField<int> MinutiAttesa
            {
                get
                {
                    return m_MinutiAttesa;
                }
            }

            /// <summary>
            /// NumeroAppuntamentiFissati
            /// </summary>
            public DBCursorField<int> NumeroAppuntamentiFissati
            {
                get
                {
                    return m_NumeroAppuntamentiFissati;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.SessioniCRM;
            }
             
        }
    }
}