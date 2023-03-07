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
        /// Cursore sulla tabella <see cref="CCampagnaCRM"/>
        /// </summary>
        [Serializable]
        public class CCampagnaCRMCursor 
            : Databases.DBObjectCursorPO<CCampagnaCRM>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<DateTime> m_Inizio = new DBCursorField<DateTime>("Inizio");
            private DBCursorField<DateTime> m_Fine = new DBCursorField<DateTime>("Fine");
            private DBCursorField<TipoCampagnaCRM> m_TipoAssegnazione = new DBCursorField<TipoCampagnaCRM>("TipoAssegnazione");
            private DBCursorStringField m_TipoCampagna = new DBCursorStringField("TipoCampagna");
            private DBCursorField<CampagnaCRMFlag> m_Flags = new DBCursorField<CampagnaCRMFlag>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagnaCRMCursor()
            {
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
            /// TipoAssegnazione
            /// </summary>
            public DBCursorField<TipoCampagnaCRM> TipoAssegnazione
            {
                get
                {
                    return m_TipoAssegnazione;
                }
            }

            /// <summary>
            /// TipoCampagna
            /// </summary>
            public DBCursorStringField TipoCampagna
            {
                get
                {
                    return m_TipoCampagna;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<CampagnaCRMFlag> Flags
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
                return minidom.CustomerCalls.CampagneCRM;
            }
             
        }
    }
}