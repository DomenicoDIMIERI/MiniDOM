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
        /// Cursore di oggetti di tipo <see cref="CAltroPreventivo"/>
        /// </summary>
        [Serializable]
        public class CAltriPreventiviCursor 
            : minidom.Databases.DBObjectCursor<CAltroPreventivo>
        {
            private DBCursorField<int> m_IDRichiestaDiFinanziamento = new DBCursorField<int>("IDRichiestaDiFinanziamento");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<DateTime> m_DataAccettazione = new DBCursorField<DateTime>("DataAccettazione");
            private DBCursorField<int> m_IDIstituto = new DBCursorField<int>("IDIstituto");
            private DBCursorStringField m_NomeIstituto = new DBCursorStringField("NomeIstituto");
            private DBCursorField<int> m_IDAgenzia = new DBCursorField<int>("IDAgenzia");
            private DBCursorStringField m_NomeAgenzia = new DBCursorStringField("NomeAgenzia");
            private DBCursorField<int> m_IDAgente = new DBCursorField<int>("IDAgente");
            private DBCursorStringField m_NomeAgente = new DBCursorStringField("NomeAgente");
            private DBCursorField<decimal> m_Rata = new DBCursorField<decimal>("Rata");
            private DBCursorField<int> m_Durata = new DBCursorField<int>("Durata");
            private DBCursorField<decimal> m_MontanteLordo = new DBCursorField<decimal>("MontanteLordo");
            private DBCursorField<decimal> m_NettoRicavo = new DBCursorField<decimal>("NettoRicavo");
            private DBCursorField<double> m_TAN = new DBCursorField<double>("TAN");
            private DBCursorField<double> m_TAEG = new DBCursorField<double>("TAEG");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<decimal> m_NettoAllaMano = new DBCursorField<decimal>("NettoAllaMano");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAltriPreventiviCursor()
            {
            }

            /// <summary>
            /// IDRichiestaDiFinanziamento
            /// </summary>
            public DBCursorField<int> IDRichiestaDiFinanziamento
            {
                get
                {
                    return m_IDRichiestaDiFinanziamento;
                }
            }

            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            /// <summary>
            /// DataAccettazione
            /// </summary>
            public DBCursorField<DateTime> DataAccettazione
            {
                get
                {
                    return m_DataAccettazione;
                }
            }

            /// <summary>
            /// IDIstituto
            /// </summary>
            public DBCursorField<int> IDIstituto
            {
                get
                {
                    return m_IDIstituto;
                }
            }

            /// <summary>
            /// NomeIstituto
            /// </summary>
            public DBCursorStringField NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }
            }

            /// <summary>
            /// IDAgenzia
            /// </summary>
            public DBCursorField<int> IDAgenzia
            {
                get
                {
                    return m_IDAgenzia;
                }
            }

            /// <summary>
            /// NomeAgenzia
            /// </summary>
            public DBCursorStringField NomeAgenzia
            {
                get
                {
                    return m_NomeAgenzia;
                }
            }

            /// <summary>
            /// IDAgente
            /// </summary>
            public DBCursorField<int> IDAgente
            {
                get
                {
                    return m_IDAgente;
                }
            }

            /// <summary>
            /// NomeAgente
            /// </summary>
            public DBCursorStringField NomeAgente
            {
                get
                {
                    return m_NomeAgente;
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
            /// MontanteLordo
            /// </summary>
            public DBCursorField<decimal> MontanteLordo
            {
                get
                {
                    return m_MontanteLordo;
                }
            }

            /// <summary>
            /// NettoRicavo
            /// </summary>
            public DBCursorField<decimal> NettoRicavo
            {
                get
                {
                    return m_NettoRicavo;
                }
            }

            /// <summary>
            /// TAN
            /// </summary>
            public DBCursorField<double> TAN
            {
                get
                {
                    return m_TAN;
                }
            }

            /// <summary>
            /// TAEG
            /// </summary>
            public DBCursorField<double> TAEG
            {
                get
                {
                    return m_TAEG;
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
            /// NettoAllaMano
            /// </summary>
            public DBCursorField<decimal> NettoAllaMano
            {
                get
                {
                    return m_NettoAllaMano;
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
                return minidom.Finanziaria.AltriPreventivi;
            }

            
        }
    }
}