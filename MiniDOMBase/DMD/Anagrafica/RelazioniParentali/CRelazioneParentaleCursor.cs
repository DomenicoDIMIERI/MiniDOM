using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle relazioni parentali
        /// </summary>
        [Serializable]
        public class CRelazioneParentaleCursor 
            : minidom.Databases.DBObjectCursor<CRelazioneParentale>
        {
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorStringField m_NomeRelazione = new DBCursorStringField("NomeRelazione");
            private DBCursorField<int> m_IDPersona1 = new DBCursorField<int>("IDPersona1");
            private DBCursorStringField m_NomePersona1 = new DBCursorStringField("NomePersona1");
            private DBCursorField<int> m_IDPersona2 = new DBCursorField<int>("IDPersona2");
            private DBCursorStringField m_NomePersona2 = new DBCursorStringField("NomePersona2");
            private DBCursorField<int> m_Ordine1 = new DBCursorField<int>("Ordine1");
            private DBCursorStringField m_Descrizione1 = new DBCursorStringField("Descrizione1");
            private DBCursorField<int> m_Ordine2 = new DBCursorField<int>("Ordine2");
            private DBCursorStringField m_Descrizione2 = new DBCursorStringField("Descrizione2");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRelazioneParentaleCursor()
            {
            }

            /// <summary>
            /// Ordine1
            /// </summary>
            public DBCursorField<int> Ordine1
            {
                get
                {
                    return m_Ordine1;
                }
            }

            /// <summary>
            /// Descrizione1
            /// </summary>
            public DBCursorStringField Descrizione1
            {
                get
                {
                    return m_Descrizione1;
                }
            }

            /// <summary>
            /// Ordine2
            /// </summary>
            public DBCursorField<int> Ordine2
            {
                get
                {
                    return m_Ordine2;
                }
            }

            /// <summary>
            /// Descrizione2
            /// </summary>
            public DBCursorStringField Descrizione2
            {
                get
                {
                    return m_Descrizione2;
                }
            }

            /// <summary>
            /// DataInizio
            /// </summary>
            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            /// <summary>
            /// DataFine
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// NomeRelazione
            /// </summary>
            public DBCursorStringField NomeRelazione
            {
                get
                {
                    return m_NomeRelazione;
                }
            }

            /// <summary>
            /// IDPersona1
            /// </summary>
            public DBCursorField<int> IDPersona1
            {
                get
                {
                    return m_IDPersona1;
                }
            }

            /// <summary>
            /// NomePersona1
            /// </summary>
            public DBCursorStringField NomePersona1
            {
                get
                {
                    return m_NomePersona1;
                }
            }

            /// <summary>
            /// IDPersona2
            /// </summary>
            public DBCursorField<int> IDPersona2
            {
                get
                {
                    return m_IDPersona2;
                }
            }

            /// <summary>
            /// NomePersona2
            /// </summary>
            public DBCursorStringField NomePersona2
            {
                get
                {
                    return m_NomePersona2;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_PersoneRelazioni";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.RelazioniParentali; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}