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
        /// Cursore su oggetti di tipo <see cref="CPausaCRM"/>
        /// </summary>
        [Serializable]
        public class CPausaCRMCursor 
            : minidom.Databases.DBObjectCursorPO<CPausaCRM>
        {
            private DBCursorField<int> m_IDSessioneCRM = new DBCursorField<int>("IDSessioneCRM");
            private DBCursorField<int> m_IDUtente = new DBCursorField<int>("IDUtente");
            private DBCursorStringField m_NomeUtente = new DBCursorStringField("NomeUtente");
            private DBCursorField<DateTime> m_OraRichiesta = new DBCursorField<DateTime>("OraRichiesta");
            private DBCursorField<DateTime> m_OraInizioValutazione = new DBCursorField<DateTime>("OraInizioValutazione");
            private DBCursorField<DateTime> m_OraFineValutazione = new DBCursorField<DateTime>("OraFineValutazione");
            private DBCursorField<DateTime> m_OraPrevista = new DBCursorField<DateTime>("OraPrevista");
            private DBCursorField<DateTime> m_Inizio = new DBCursorField<DateTime>("Inizio");
            private DBCursorField<DateTime> m_Fine = new DBCursorField<DateTime>("Fine");
            private DBCursorStringField m_Motivo = new DBCursorStringField("Motivo");
            private DBCursorField<int> m_DurataPrevista = new DBCursorField<int>("DurataPrevista");
            private DBCursorStringField m_DettaglioMotivo = new DBCursorStringField("DettaglioMotivo");
            private DBCursorField<int> m_IDSupervisore = new DBCursorField<int>("IDSupervisore");
            private DBCursorStringField m_NomeSupervisore = new DBCursorStringField("NomeSupervisore");
            private DBCursorStringField m_EsitoSupervisione = new DBCursorStringField("EsitoSupervisione");
            private DBCursorStringField m_NoteAmministrative = new DBCursorStringField("NoteAmministrative");
            private DBCursorField<StatoPausaCRM> m_StatoRichiesta = new DBCursorField<StatoPausaCRM>("StatoRichiesta");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPausaCRMCursor()
            {
            }

            /// <summary>
            /// IDSessioneCRM
            /// </summary>
            public DBCursorField<int> IDSessioneCRM
            {
                get
                {
                    return m_IDSessioneCRM;
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
            /// OraRichiesta
            /// </summary>
            public DBCursorField<DateTime> OraRichiesta
            {
                get
                {
                    return m_OraRichiesta;
                }
            }

            /// <summary>
            /// OraInizioValutazione
            /// </summary>
            public DBCursorField<DateTime> OraInizioValutazione
            {
                get
                {
                    return m_OraInizioValutazione;
                }
            }

            /// <summary>
            /// OraFineValutazione
            /// </summary>
            public DBCursorField<DateTime> OraFineValutazione
            {
                get
                {
                    return m_OraFineValutazione;
                }
            }

            /// <summary>
            /// OraPrevista
            /// </summary>
            public DBCursorField<DateTime> OraPrevista
            {
                get
                {
                    return m_OraPrevista;
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
            /// Motivo
            /// </summary>
            public DBCursorStringField Motivo
            {
                get
                {
                    return m_Motivo;
                }
            }

            /// <summary>
            /// DurataPrevista
            /// </summary>
            public DBCursorField<int> DurataPrevista
            {
                get
                {
                    return m_DurataPrevista;
                }
            }

            /// <summary>
            /// DettaglioMotivo
            /// </summary>
            public DBCursorStringField DettaglioMotivo
            {
                get
                {
                    return m_DettaglioMotivo;
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
            /// EsitoSupervisione
            /// </summary>
            public DBCursorStringField EsitoSupervisione
            {
                get
                {
                    return m_EsitoSupervisione;
                }
            }

            /// <summary>
            /// NoteAmministrative
            /// </summary>
            public DBCursorStringField NoteAmministrative
            {
                get
                {
                    return m_NoteAmministrative;
                }
            }

            /// <summary>
            /// StatoRichiesta
            /// </summary>
            public DBCursorField<StatoPausaCRM> StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
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
                return minidom.CustomerCalls.PauseCRM;
            }

             
        }
    }
}