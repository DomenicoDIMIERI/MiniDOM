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
    public partial class Office
    {

        /// <summary>
        /// Cursore di <see cref="PeriodoLavorato"/>
        /// </summary>
        [Serializable]
        public class PeriodoLavoratoCursor 
            : Databases.DBObjectCursorPO<PeriodoLavorato>
        {
            private DBCursorField<DateTime> m_Periodo = new DBCursorField<DateTime>("Periodo");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<int> m_IDTurno = new DBCursorField<int>("IDTurno");
            private DBCursorStringField m_NomeTurno = new DBCursorStringField("NomeTurno");
            private DBCursorField<double> m_DeltaIngresso = new DBCursorField<double>("DeltaIngresso");
            private DBCursorField<double> m_DeltaUscita = new DBCursorField<double>("DeltaUscita");
            private DBCursorField<double> m_OreLavorateTurno = new DBCursorField<double>("OreLavorateTurno");
            private DBCursorField<double> m_OreLavorateEffettive = new DBCursorField<double>("OreLavorateEffettive");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<decimal> m_RetribuzioneCalcolata = new DBCursorField<decimal>("RetribuzioneCalcolata");
            private DBCursorField<decimal> m_RetribuzioneErogabile = new DBCursorField<decimal>("RetribuzioneErogabile");
            private DBCursorField<decimal> m_RetribuzioneErogata = new DBCursorField<decimal>("RetribuzioneErogata");
            private DBCursorField<DateTime> m_DataVerifica = new DBCursorField<DateTime>("DataVerifica");
            private DBCursorField<int> m_IDVerificatoDa = new DBCursorField<int>("IDVerificatoDa");
            private DBCursorStringField m_NomeVerificatoDa = new DBCursorStringField("NomeVerificatoDa");
            private DBCursorStringField m_NoteVerifica = new DBCursorStringField("NoteVerifica");

            /// <summary>
            /// Costruttore
            /// </summary>
            public PeriodoLavoratoCursor()
            {
            }

            /// <summary>
            /// Periodo
            /// </summary>
            public DBCursorField<DateTime> Periodo
            {
                get
                {
                    return m_Periodo;
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
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            /// <summary>
            /// IDTurno
            /// </summary>
            public DBCursorField<int> IDTurno
            {
                get
                {
                    return m_IDTurno;
                }
            }

            /// <summary>
            /// NomeTurno
            /// </summary>
            public DBCursorStringField NomeTurno
            {
                get
                {
                    return m_NomeTurno;
                }
            }

            /// <summary>
            /// DeltaIngresso
            /// </summary>
            public DBCursorField<double> DeltaIngresso
            {
                get
                {
                    return m_DeltaIngresso;
                }
            }

            /// <summary>
            /// DeltaUscita
            /// </summary>
            public DBCursorField<double> DeltaUscita
            {
                get
                {
                    return m_DeltaUscita;
                }
            }

            /// <summary>
            /// OreLavorateTurno
            /// </summary>
            public DBCursorField<double> OreLavorateTurno
            {
                get
                {
                    return m_OreLavorateTurno;
                }
            }

            /// <summary>
            /// OreLavorateEffettive
            /// </summary>
            public DBCursorField<double> OreLavorateEffettive
            {
                get
                {
                    return m_OreLavorateEffettive;
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
            /// RetribuzioneCalcolata
            /// </summary>
            public DBCursorField<decimal> RetribuzioneCalcolata
            {
                get
                {
                    return m_RetribuzioneCalcolata;
                }
            }

            /// <summary>
            /// RetribuzioneErogabile
            /// </summary>
            public DBCursorField<decimal> RetribuzioneErogabile
            {
                get
                {
                    return m_RetribuzioneErogabile;
                }
            }

            /// <summary>
            /// RetribuzioneErogata
            /// </summary>
            public DBCursorField<decimal> RetribuzioneErogata
            {
                get
                {
                    return m_RetribuzioneErogata;
                }
            }

            /// <summary>
            /// DataVerifica
            /// </summary>
            public DBCursorField<DateTime> DataVerifica
            {
                get
                {
                    return m_DataVerifica;
                }
            }

            /// <summary>
            /// IDVerificatoDa
            /// </summary>
            public DBCursorField<int> IDVerificatoDa
            {
                get
                {
                    return m_IDVerificatoDa;
                }
            }

            /// <summary>
            /// NomeVerificatoDa
            /// </summary>
            public DBCursorStringField NomeVerificatoDa
            {
                get
                {
                    return m_NomeVerificatoDa;
                }
            }

            /// <summary>
            /// NoteVerifica
            /// </summary>
            public DBCursorStringField NoteVerifica
            {
                get
                {
                    return m_NoteVerifica;
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.PeriodiLavorati;
            }
        }
    }
}