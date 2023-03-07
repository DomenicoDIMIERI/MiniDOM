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
using static minidom.Finanziaria;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="CPreventivo"/>
        /// </summary>
        [Serializable]
        public class CPreventivoCursor 
            : minidom.Finanziaria.DBObjectCursorPO<CPreventivo>
        {
            private DBCursorField<int> m_ProfiloID = new DBCursorField<int>("Profilo");
            private DBCursorStringField m_NomeProfilo = new DBCursorStringField("NomeProfilo");
            private DBCursorField<DateTime> m_DataNascita = new DBCursorField<DateTime>("DataNascita");
            private DBCursorField<DateTime> m_DataAssunzione = new DBCursorField<DateTime>("DataAssunzione");
            private DBCursorField<DateTime> m_DataDecorrenza = new DBCursorField<DateTime>("DataDecorrenza");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Cognome = new DBCursorStringField("Cognome");
            private DBCursorStringField m_Nominativo = new DBCursorStringField("(Trim([Nome] & ' ' & [Cognome]))");
            private DBCursorStringField m_TipoContratto = new DBCursorStringField("TipoContratto");
            private DBCursorStringField m_TipoRapporto = new DBCursorStringField("TipoRapporto");
            private DBCursorStringField m_Sesso = new DBCursorStringField("Sesso");
            private DBCursorField<int> m_Durata = new DBCursorField<int>("Durata");
            private DBCursorField<decimal> m_Rata = new DBCursorField<decimal>("Rata");
            // Private m_NumeroOfferta As CCursorFieldObj(Of String)
            private DBCursorField<double> m_Provvigionale = new DBCursorField<double>("Provvigionale");
            private DBCursorField<bool> m_CaricaAlMassimo = new DBCursorField<bool>("CaricaAlMassimo");
            private DBCursorField<SortPreventivoBy> m_SortBy = new DBCursorField<SortPreventivoBy>("SortBy");
            private DBCursorField<decimal> m_StipendioLordo = new DBCursorField<decimal>("StipendioLordo");
            private DBCursorField<decimal> m_StipendioNetto = new DBCursorField<decimal>("StipendioNetto");
            private DBCursorField<decimal> m_TFR = new DBCursorField<decimal>("TFR");
            private DBCursorField<int> m_NumeroMensilita = new DBCursorField<int>("NumeroMensilita");
            private DBCursorField<int> m_Amministrazione = new DBCursorField<int>("Amministrazione");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPreventivoCursor()
            {
            }

            /// <summary>
            /// TipoContratto
            /// </summary>
            public DBCursorStringField TipoContratto
            {
                get
                {
                    return m_TipoContratto;
                }
            }

            /// <summary>
            /// TipoRapporto
            /// </summary>
            public DBCursorStringField TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }
            }

            /// <summary>
            /// Durata
            /// </summary>
            public DBCursorField<int> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            /// <summary>
            /// Rata
            /// </summary>
            public DBCursorField<decimal> Rata
            {
                get
                {
                    return m_Rata;
                }
            }

            /// <summary>
            /// Provvigionale
            /// </summary>
            public DBCursorField<double> Provvigionale
            {
                get
                {
                    return m_Provvigionale;
                }
            }

            /// <summary>
            /// CaricaAlMassimo
            /// </summary>
            public DBCursorField<bool> CaricaAlMassimo
            {
                get
                {
                    return m_CaricaAlMassimo;
                }
            }

            /// <summary>
            /// SortBy
            /// </summary>
            public DBCursorField<SortPreventivoBy> SortBy
            {
                get
                {
                    return m_SortBy;
                }
            }

            /// <summary>
            /// StipendioLordo
            /// </summary>
            public DBCursorField<decimal> StipendioLordo
            {
                get
                {
                    return m_StipendioLordo;
                }
            }

            /// <summary>
            /// StipendioNetto
            /// </summary>
            public DBCursorField<decimal> StipendioNetto
            {
                get
                {
                    return m_StipendioNetto;
                }
            }

            /// <summary>
            /// TFR
            /// </summary>
            public DBCursorField<decimal> TFR
            {
                get
                {
                    return m_TFR;
                }
            }

            /// <summary>
            /// NumeroMensilita
            /// </summary>
            public DBCursorField<int> NumeroMensilita
            {
                get
                {
                    return m_NumeroMensilita;
                }
            }

            /// <summary>
            /// Amministrazione
            /// </summary>
            public DBCursorField<int> Amministrazione
            {
                get
                {
                    return m_Amministrazione;
                }
            }

            /// <summary>
            /// Nominativo
            /// </summary>
            public DBCursorStringField Nominativo
            {
                get
                {
                    return m_Nominativo;
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
            /// Cognome
            /// </summary>
            public DBCursorStringField Cognome
            {
                get
                {
                    return m_Cognome;
                }
            }

            /// <summary>
            /// ProfiloID
            /// </summary>
            public DBCursorField<int> ProfiloID
            {
                get
                {
                    return m_ProfiloID;
                }
            }

            /// <summary>
            /// NomeProfilo
            /// </summary>
            public DBCursorStringField NomeProfilo
            {
                get
                {
                    return m_NomeProfilo;
                }
            }

            // Public ReadOnly Property NumeroOfferta As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NumeroOfferta
            // End Get
            // End Property

            /// <summary>
            /// DataNascita
            /// </summary>
            public DBCursorField<DateTime> DataNascita
            {
                get
                {
                    return m_DataNascita;
                }
            }

            /// <summary>
            /// DataAssunzione
            /// </summary>
            public DBCursorField<DateTime> DataAssunzione
            {
                get
                {
                    return m_DataAssunzione;
                }
            }

            /// <summary>
            /// DataDecorrenza
            /// </summary>
            public DBCursorField<DateTime> DataDecorrenza
            {
                get
                {
                    return m_DataDecorrenza;
                }
            }

            /// <summary>
            /// Sesso
            /// </summary>
            public DBCursorStringField Sesso
            {
                get
                {
                    return m_Sesso;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Preventivi;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Preventivi";
            //}

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CPreventivo();
            //}
        }
    }
}