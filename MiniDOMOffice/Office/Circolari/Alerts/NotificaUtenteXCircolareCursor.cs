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
using static minidom.Store;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Cursore di <see cref="NotificaUtenteXCircolare"/>
        /// </summary>
        [Serializable]
        public class NotificaUtenteXCircolareCursor
            : Databases.DBObjectCursor<NotificaUtenteXCircolare>
        {
            private DBCursorField<int> m_IDComunicazione = new DBCursorField<int>("IDComunicazione");
            private DBCursorField<int> m_IDUser = new DBCursorField<int>("IDUser");
            private DBCursorStringField m_Via = new DBCursorStringField("Via");
            private DBCursorStringField m_Param = new DBCursorStringField("Param");
            private DBCursorField<StatoNotificaUtenteXCircolare> m_StatoComunicazione = new DBCursorField<StatoNotificaUtenteXCircolare>("StatoComunicazione");
            private DBCursorField<DateTime> m_DataConsegna = new DBCursorField<DateTime>("DataConsegna");
            private DBCursorField<DateTime> m_DataLettura = new DBCursorField<DateTime>("DataLettura");

            /// <summary>
            /// Costruttore
            /// </summary>
            public NotificaUtenteXCircolareCursor()
            {
            }

            /// <summary>
            /// IDComunicazione
            /// </summary>
            public DBCursorField<int> IDComunicazione
            {
                get
                {
                    return m_IDComunicazione;
                }
            }

            /// <summary>
            /// IDUser
            /// </summary>
            public DBCursorField<int> IDUser
            {
                get
                {
                    return m_IDUser;
                }
            }

            /// <summary>
            /// Via
            /// </summary>
            public DBCursorStringField Via
            {
                get
                {
                    return m_Via;
                }
            }

            /// <summary>
            /// Param
            /// </summary>
            public DBCursorStringField Param
            {
                get
                {
                    return m_Param;
                }
            }

            /// <summary>
            /// StatoComunicazione
            /// </summary>
            public DBCursorField<StatoNotificaUtenteXCircolare> StatoComunicazione
            {
                get
                {
                    return m_StatoComunicazione;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Circolari.Results;
            }

           
        }
    }
}