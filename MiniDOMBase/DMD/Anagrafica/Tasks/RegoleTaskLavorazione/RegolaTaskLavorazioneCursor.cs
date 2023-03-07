using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle regole di lavorazione
        /// </summary>
        [Serializable]
        public class RegolaTaskLavorazioneCursor
            : minidom.Databases.DBObjectCursor<RegolaTaskLavorazione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_IDStatoSorgente = new DBCursorField<int>("IDStatoSorgente");
            private DBCursorField<int> m_IDStatoDestinazione = new DBCursorField<int>("IDStatoDestinazione");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_NomeHandler = new DBCursorStringField("NomeHandler");
            private DBCursorStringField m_ContextType = new DBCursorStringField("ContextType");
            private DBCursorField<int> m_Ordine = new DBCursorField<int>("Ordine");

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegolaTaskLavorazioneCursor()
            {
            }

            /// <summary>
            /// ContextType
            /// </summary>
            public DBCursorStringField ContextType
            {
                get
                {
                    return m_ContextType;
                }
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// NomeHandler
            /// </summary>
            public DBCursorStringField NomeHandler
            {
                get
                {
                    return m_NomeHandler;
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// IDStatoSorgente
            /// </summary>
            public DBCursorField<int> IDStatoSorgente
            {
                get
                {
                    return m_IDStatoSorgente;
                }
            }

            /// <summary>
            /// IDStatoDestinazione
            /// </summary>
            public DBCursorField<int> IDStatoDestinazione
            {
                get
                {
                    return m_IDStatoDestinazione;
                }
            }

            /// <summary>
            /// Ordine
            /// </summary>
            public DBCursorField<int> Ordine
            {
                get
                {
                    return m_Ordine;
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

            //public override string GetTableName()
            //{
            //    return "tbl_TaskLavorazioneRegole";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return TasksDiLavorazione.Database;
            //}


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.RegoleTasksLavorazione; //.Module
            }
        }
    }
}