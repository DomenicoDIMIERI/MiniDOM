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
        /// Cursore sulla tabella delle utenze
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class UtenzeCursor 
            : minidom.Databases.DBObjectCursorPO<Utenza>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_IDFornitore = new DBCursorField<int>("IDFornitore");
            private DBCursorStringField m_NomeFornitore = new DBCursorStringField("NomeFornitore");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_NumeroContratto = new DBCursorStringField("NumeroContratto");
            private DBCursorStringField m_CodiceCliente = new DBCursorStringField("CodiceCliente");
            private DBCursorStringField m_CodiceUtenza = new DBCursorStringField("CodiceUtenza");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<PeriodicitaUtenza> m_TipoPeriodicita = new DBCursorField<PeriodicitaUtenza>("TipoPeriodicita");
            private DBCursorField<int> m_IntervalloPeriodicita = new DBCursorField<int>("IntervalloPeriodicita");
            private DBCursorField<DateTime> m_DataSottoscrizione = new DBCursorField<DateTime>("DataSottoscrizione");
            private DBCursorField<DateTime> m_DataInizioPeriodo = new DBCursorField<DateTime>("DataInizioPeriodo");
            private DBCursorField<DateTime> m_DataFinePeriodo = new DBCursorField<DateTime>("DataFinePeriodo");
            private DBCursorStringField m_UnitaMisura = new DBCursorStringField("UnitaMisura");
            private DBCursorField<decimal> m_CostoUnitario = new DBCursorField<decimal>("CostoUnitario");
            private DBCursorField<decimal> m_CostiFissi = new DBCursorField<decimal>("CostiFissi");
            private DBCursorStringField m_NomeValuta = new DBCursorStringField("NomeValuta");
            private DBCursorField<UtenzaFlags> m_Flags = new DBCursorField<UtenzaFlags>("Flags");
            private DBCursorStringField m_TipoMetodoDiPagamento = new DBCursorStringField("TipoMetodoDiPagamento");
            private DBCursorField<int> m_IDMetodoDiPagamento = new DBCursorField<int>("IDMetodotoDiPagamento");
            private DBCursorStringField m_NomeMetodoDiPagamento = new DBCursorStringField("NomeMetodoDiPagamento");
            private DBCursorStringField m_TipoUtenza = new DBCursorStringField("TipoUtenza");
            private DBCursorStringField m_StimatoreBolletta = new DBCursorStringField("StimatoreBolletta");

            /// <summary>
            /// Costruttore
            /// </summary>
            public UtenzeCursor()
            {
            }

            /// <summary>
            /// StimatoreBolletta
            /// </summary>
            public DBCursorStringField StimatoreBolletta
            {
                get
                {
                    return m_StimatoreBolletta;
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
            /// IDFornitore
            /// </summary>
            public DBCursorField<int> IDFornitore
            {
                get
                {
                    return m_IDFornitore;
                }
            }

            /// <summary>
            /// NomeFornitore
            /// </summary>
            public DBCursorStringField NomeFornitore
            {
                get
                {
                    return m_NomeFornitore;
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
            /// NumeroContratto
            /// </summary>
            public DBCursorStringField NumeroContratto
            {
                get
                {
                    return m_NumeroContratto;
                }
            }

            /// <summary>
            /// CodiceCliente
            /// </summary>
            public DBCursorStringField CodiceCliente
            {
                get
                {
                    return m_CodiceCliente;
                }
            }

            /// <summary>
            /// CodiceUtenza
            /// </summary>
            public DBCursorStringField CodiceUtenza
            {
                get
                {
                    return m_CodiceUtenza;
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
            /// TipoPeriodicita
            /// </summary>
            public DBCursorField<PeriodicitaUtenza> TipoPeriodicita
            {
                get
                {
                    return m_TipoPeriodicita;
                }
            }

            /// <summary>
            /// IntervalloPeriodicita
            /// </summary>
            public DBCursorField<int> IntervalloPeriodicita
            {
                get
                {
                    return m_IntervalloPeriodicita;
                }
            }

            /// <summary>
            /// DataSottoscrizione
            /// </summary>
            public DBCursorField<DateTime> DataSottoscrizione
            {
                get
                {
                    return m_DataSottoscrizione;
                }
            }

            /// <summary>
            /// DataInizioPeriodo
            /// </summary>
            public DBCursorField<DateTime> DataInizioPeriodo
            {
                get
                {
                    return m_DataInizioPeriodo;
                }
            }

            /// <summary>
            /// DataFinePeriodo
            /// </summary>
            public DBCursorField<DateTime> DataFinePeriodo
            {
                get
                {
                    return m_DataFinePeriodo;
                }
            }

            /// <summary>
            /// UnitaMisura
            /// </summary>
            public DBCursorStringField UnitaMisura
            {
                get
                {
                    return m_UnitaMisura;
                }
            }

            /// <summary>
            /// CostoUnitario
            /// </summary>
            public DBCursorField<decimal> CostoUnitario
            {
                get
                {
                    return m_CostoUnitario;
                }
            }

            /// <summary>
            /// CostiFissi
            /// </summary>
            public DBCursorField<decimal> CostiFissi
            {
                get
                {
                    return m_CostiFissi;
                }
            }

            /// <summary>
            /// NomeValuta
            /// </summary>
            public DBCursorStringField NomeValuta
            {
                get
                {
                    return m_NomeValuta;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<UtenzaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// TipoMetodoDiPagamento
            /// </summary>
            public DBCursorStringField TipoMetodoDiPagamento
            {
                get
                {
                    return m_TipoMetodoDiPagamento;
                }
            }

            /// <summary>
            /// IDMetodoDiPagamento
            /// </summary>
            public DBCursorField<int> IDMetodoDiPagamento
            {
                get
                {
                    return m_IDMetodoDiPagamento;
                }
            }

            /// <summary>
            /// NomeMetodoDiPagamento
            /// </summary>
            public DBCursorStringField NomeMetodoDiPagamento
            {
                get
                {
                    return m_NomeMetodoDiPagamento;
                }
            }

            /// <summary>
            /// TipoUtenza
            /// </summary>
            public DBCursorStringField TipoUtenza
            {
                get
                {
                    return m_TipoUtenza;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Utenze;
            }
        }
    }
}