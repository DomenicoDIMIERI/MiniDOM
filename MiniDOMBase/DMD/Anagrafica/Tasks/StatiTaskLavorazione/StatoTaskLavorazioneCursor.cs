using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Cursore sulla tabella dei task di lavorazione
        /// </summary>
        [Serializable]
        public class StatoTaskLavorazioneCursor 
            : minidom.Databases.DBObjectCursor<StatoTaskLavorazione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_Descrizione2 = new DBCursorStringField("Descrizione2");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDStatoSuccessivoPredefinito = new DBCursorField<int>("IDStatoSuccessivo");
            private DBCursorField<MacroStatoLavorazione> m_MacroStato = new DBCursorField<MacroStatoLavorazione>("MacroStato");
            private DBCursorField<TipoPersona> m_SiApplicaA = new DBCursorField<TipoPersona>("SiApplicaA");
            private DBCursorStringField m_NomeHandler = new DBCursorStringField("NomeHandler");

            /// <summary>
            /// Costruttore
            /// </summary>
            public StatoTaskLavorazioneCursor()
            {
            }

            /// <summary>
            /// SiApplicaA
            /// </summary>
            public DBCursorField<TipoPersona> SiApplicaA
            {
                get
                {
                    return m_SiApplicaA;
                }
            }

            /// <summary>
            /// MacroStato
            /// </summary>
            public DBCursorField<MacroStatoLavorazione> MacroStato
            {
                get
                {
                    return m_MacroStato;
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
            /// Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Nome;
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
            /// IDStatoSuccessivoPredefinito
            /// </summary>
            public DBCursorField<int> IDStatoSuccessivoPredefinito
            {
                get
                {
                    return m_IDStatoSuccessivoPredefinito;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_TaskLavorazioneStati";
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
                return minidom.Anagrafica.StatiTasksLavorazione; //.Module;
            }
        }
    }
}