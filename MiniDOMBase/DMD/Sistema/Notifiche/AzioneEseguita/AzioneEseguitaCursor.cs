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

namespace minidom
{
    public partial class Sistema
    {



        /// <summary>
        /// Cursore sulla tabella delle azione eseguite
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class AzioneEseguitaCursor 
            : minidom.Databases.DBObjectCursor
        {
            private DBCursorField<int> m_IDNotifica = new DBCursorField<int>("Notifica");
            private DBCursorStringField m_AzioneType = new DBCursorStringField("Azione");
            private DBCursorField<DateTime> m_DataEsecuzione = new DBCursorField<DateTime>("DataEsecuzione");
            private DBCursorStringField m_ActionParameters = new DBCursorStringField("ActionParameters");
            private DBCursorStringField m_ActionResults = new DBCursorStringField("ActionResults");

            /// <summary>
            /// Costruttore
            /// </summary>
            public AzioneEseguitaCursor()
            {
            }

            /// <summary>
            /// IDNotifica
            /// </summary>
            public DBCursorField<int> IDNotifica
            {
                get
                {
                    return m_IDNotifica;
                }
            }

            /// <summary>
            /// AzioneType
            /// </summary>
            public DBCursorStringField AzioneType
            {
                get
                {
                    return m_AzioneType;
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

            /// <summary>
            /// Parameters
            /// </summary>
            public DBCursorStringField ActionParameters
            {
                get
                {
                    return m_ActionParameters;
                }
            }

            /// <summary>
            /// ActionResults
            /// </summary>
            public DBCursorStringField ActionResults
            {
                get
                {
                    return m_ActionResults;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Notifiche.Database;
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_SYSNotifyRes";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Notifiche.AzioniEseguiteRepository;
            }

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new AzioneEseguita();
            //}
        }
    }
}