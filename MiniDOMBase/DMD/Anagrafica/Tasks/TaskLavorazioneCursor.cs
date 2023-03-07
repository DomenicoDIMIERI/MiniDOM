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
        /// Cursore sulla tabella dei <see cref="TaskLavorazione"/>
        /// </summary>
        [Serializable]
        public class TaskLavorazioneCursor
            : minidom.Databases.DBObjectCursorPO<TaskLavorazione>
        {
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<int> m_IDStatoAttuale = new DBCursorField<int>("IDStatoAttuale");
            private DBCursorField<DateTime> m_DataAssegnazione = new DBCursorField<DateTime>("DataAssegnazione");
            private DBCursorField<int> m_IDAssegnatoDa = new DBCursorField<int>("IDAssegnatoDa");
            private DBCursorField<DateTime> m_DataPrevista = new DBCursorField<DateTime>("DataPrevista");
            private DBCursorField<int> m_IDAssegnatoA = new DBCursorField<int>("IDAssegnatoA");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<int> m_IDAzioneEseguita = new DBCursorField<int>("IDAzioneEseguita");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorField<int> m_Priorita = new DBCursorField<int>("Priorita");
            private DBCursorField<int> m_IDSorgente = new DBCursorField<int>("IDSorgente");
            private DBCursorStringField m_TipoSorgente = new DBCursorStringField("TipoSorgente");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorField<int> m_IDRegolaEseguita = new DBCursorField<int>("IDRegolaEseguita");
            private DBCursorStringField m_ParametriAzione = new DBCursorStringField("ParametriAzione");
            private DBCursorStringField m_RisultatoAzione = new DBCursorStringField("RisultatoAzione");
            private DBCursorField<DateTime> m_DataInizioEsecuzione = new DBCursorField<DateTime>("DataEsecuzione");
            private DBCursorField<DateTime> m_DataFineEsecuzione = new DBCursorField<DateTime>("DataFineEsecuzione");
            private DBCursorField<int> m_IDTaskDestinazione = new DBCursorField<int>("IDTaskDestinazione");
            private DBCursorField<int> m_IDTaskSorgente = new DBCursorField<int>("IDTaskSorgente");

            /// <summary>
            /// Costruttore
            /// </summary>
            public TaskLavorazioneCursor()
            {
            }

            /// <summary>
            /// IDRegolaEseguita
            /// </summary>
            public DBCursorField<int> IDRegolaEseguita
            {
                get
                {
                    return m_IDRegolaEseguita;
                }
            }

            /// <summary>
            /// ParametriAzione
            /// </summary>
            public DBCursorStringField ParametriAzione
            {
                get
                {
                    return m_ParametriAzione;
                }
            }

            /// <summary>
            /// RisultatoAzione
            /// </summary>
            public DBCursorStringField RisultatoAzione
            {
                get
                {
                    return m_RisultatoAzione;
                }
            }

            /// <summary>
            /// DataInizioEsecuzione
            /// </summary>
            public DBCursorField<DateTime> DataInizioEsecuzione
            {
                get
                {
                    return m_DataInizioEsecuzione;
                }
            }

            /// <summary>
            /// DataFineEsecuzione
            /// </summary>
            public DBCursorField<DateTime> DataFineEsecuzione
            {
                get
                {
                    return m_DataFineEsecuzione;
                }
            }

            /// <summary>
            /// IDTaskDestinazione
            /// </summary>
            public DBCursorField<int> IDTaskDestinazione
            {
                get
                {
                    return m_IDTaskDestinazione;
                }
            }

            /// <summary>
            /// IDTaskSorgente
            /// </summary>
            public DBCursorField<int> IDTaskSorgente
            {
                get
                {
                    return m_IDTaskSorgente;
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
            /// Priorita
            /// </summary>
            public DBCursorField<int> Priorita
            {
                get
                {
                    return m_Priorita;
                }
            }

            /// <summary>
            /// IDSorgente
            /// </summary>
            public DBCursorField<int> IDSorgente
            {
                get
                {
                    return m_IDSorgente;
                }
            }

            /// <summary>
            /// TipoSorgente
            /// </summary>
            public DBCursorStringField TipoSorgente
            {
                get
                {
                    return m_TipoSorgente;
                }
            }

            /// <summary>
            /// IDContesto
            /// </summary>
            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            /// <summary>
            /// TipoContesto
            /// </summary>
            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            /// <summary>
            /// IDCliente
            /// </summary>
            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            /// <summary>
            /// NomeCliente
            /// </summary>
            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            /// <summary>
            /// IDStatoAttuale
            /// </summary>
            public DBCursorField<int> IDStatoAttuale
            {
                get
                {
                    return m_IDStatoAttuale;
                }
            }

            /// <summary>
            /// DataAssegnazione
            /// </summary>
            public DBCursorField<DateTime> DataAssegnazione
            {
                get
                {
                    return m_DataAssegnazione;
                }
            }

            /// <summary>
            /// IDAssegnatoDa
            /// </summary>
            public DBCursorField<int> IDAssegnatoDa
            {
                get
                {
                    return m_IDAssegnatoDa;
                }
            }

            /// <summary>
            /// DataPrevista
            /// </summary>
            public DBCursorField<DateTime> DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }
            }

            /// <summary>
            /// IDAssegnatoA
            /// </summary>
            public DBCursorField<int> IDAssegnatoA
            {
                get
                {
                    return m_IDAssegnatoA;
                }
            }

            /// <summary>
            /// Note
            /// </summary>
            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            /// <summary>
            /// IDAzioneEseguita
            /// </summary>
            public DBCursorField<int> IDAzioneEseguita
            {
                get
                {
                    return m_IDAzioneEseguita;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_TaskLavorazione";
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
                return minidom.Anagrafica.TasksDiLavorazione; //.Module;
            }
        }
    }
}