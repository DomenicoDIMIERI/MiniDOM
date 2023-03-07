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
    public partial class Finanziaria
    {

        /// <summary>
        /// Cursore sulla tabella degli obiettivi
        /// </summary>
        /// <remarks></remarks>
        public class CObiettivoPraticaCursor 
            : Databases.DBObjectCursorPO<CObiettivoPratica>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<TipoObiettivo> m_TipoObiettivo = new DBCursorField<TipoObiettivo>("TipoObiettivo");
            private DBCursorField<PeriodicitaObiettivo> m_PeriodicitaObiettivo = new DBCursorField<PeriodicitaObiettivo>("PeriodicitaObiettivo");
            // Private m_ValoreObiettivo As New DBCursorField(Of Double)("ValoreObiettivo")
            private DBCursorField<decimal> m_MontanteLordoLiq = new DBCursorField<decimal>("MontanteLordoLiq");
            private DBCursorField<int> m_NumeroPraticheLiq = new DBCursorField<int>("NumeroPraticheLiq");
            private DBCursorField<decimal> m_ValoreSpreadLiq = new DBCursorField<decimal>("ValoreSpreadLiq");
            private DBCursorField<float> m_SpreadLiq = new DBCursorField<float>("SpreadLiq");
            private DBCursorField<decimal> m_ValoreUpFrontLiq = new DBCursorField<decimal>("ValoreUpFrontLiq");
            private DBCursorField<float> m_UpFrontLiq = new DBCursorField<float>("UpFrontLiq");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<decimal> m_ValoreScontoLiq = new DBCursorField<decimal>("ValoreScontoLiq");
            private DBCursorField<float> m_ScontoLiq = new DBCursorField<float>("ScontoLiq");
            private DBCursorField<int> m_Livello = new DBCursorField<int>("Livello");
            private DBCursorField<decimal> m_CostoStruttura = new DBCursorField<decimal>("CostoStruttura");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CObiettivoPraticaCursor()
            {
            }

            /// <summary>
            /// CostoStruttura
            /// </summary>
            public DBCursorField<decimal> CostoStruttura
            {
                get
                {
                    return m_CostoStruttura;
                }
            }

            /// <summary>
            /// Livello
            /// </summary>
            public DBCursorField<int> Livello
            {
                get
                {
                    return m_Livello;
                }
            }

            /// <summary>
            /// ValoreScontoLiq
            /// </summary>
            public DBCursorField<decimal> ValoreScontoLiq
            {
                get
                {
                    return m_ValoreScontoLiq;
                }
            }

            /// <summary>
            /// ScontoLiq
            /// </summary>
            public DBCursorField<float> ScontoLiq
            {
                get
                {
                    return m_ScontoLiq;
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
            /// TipoObiettivo
            /// </summary>
            public DBCursorField<TipoObiettivo> TipoObiettivo
            {
                get
                {
                    return m_TipoObiettivo;
                }
            }

            /// <summary>
            /// PeriodicitaObiettivo
            /// </summary>
            public DBCursorField<PeriodicitaObiettivo> PeriodicitaObiettivo
            {
                get
                {
                    return m_PeriodicitaObiettivo;
                }
            }

            // Public ReadOnly Property ValoreObiettivo As DBCursorField(Of Double)
            // Get
            // Return Me.m_ValoreObiettivo
            // End Get
            // End Property

            /// <summary>
            /// MontanteLordoLiq
            /// </summary>
            public DBCursorField<decimal> MontanteLordoLiq
            {
                get
                {
                    return m_MontanteLordoLiq;
                }
            }

            /// <summary>
            /// NumeroPraticheLiq
            /// </summary>
            public DBCursorField<int> NumeroPraticheLiq
            {
                get
                {
                    return m_NumeroPraticheLiq;
                }
            }

            /// <summary>
            /// ValoreSpreadLiq
            /// </summary>
            public DBCursorField<decimal> ValoreSpreadLiq
            {
                get
                {
                    return m_ValoreSpreadLiq;
                }
            }

            /// <summary>
            /// SpreadLiq
            /// </summary>
            public DBCursorField<float> SpreadLiq
            {
                get
                {
                    return m_SpreadLiq;
                }
            }

            /// <summary>
            /// ValoreUpFrontLiq
            /// </summary>
            public DBCursorField<decimal> ValoreUpFrontLiq
            {
                get
                {
                    return m_ValoreUpFrontLiq;
                }
            }

            /// <summary>
            /// UpFrontLiq
            /// </summary>
            public DBCursorField<float> UpFrontLiq
            {
                get
                {
                    return m_UpFrontLiq;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Obiettivi;
            }
             
        }
    }
}