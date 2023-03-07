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
        /// Cursore di <see cref="ChiamataRegistrata"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ChiamataRegistrataCursor 
            : minidom.Databases.DBObjectCursorPO<ChiamataRegistrata>
        {
            private DBCursorStringField m_IDChiamata = new DBCursorStringField("IDChiamata");
            private DBCursorField<StatoChiamataRegistrata> m_StatoChiamata = new DBCursorField<StatoChiamataRegistrata>("StatoChiamata");
            private DBCursorStringField m_EsitoChiamataEx = new DBCursorStringField("EsitoChiamataEx");
            private DBCursorField<EsitoChiamataRegistrata> m_EsitoChiamata = new DBCursorField<EsitoChiamataRegistrata>("EsitoChiamata");
            private DBCursorStringField m_StatoChiamataEx = new DBCursorStringField("StatoChiamataEx");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataRisposta = new DBCursorField<DateTime>("DataRisposta");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_IDPBX = new DBCursorField<int>("IDPBX");
            private DBCursorStringField m_NomePBX = new DBCursorStringField("NomePBX");
            private DBCursorField<int> m_IDChiamante = new DBCursorField<int>("IDChiamante");
            private DBCursorStringField m_NomeChiamante = new DBCursorStringField("NomeChiamante");
            private DBCursorField<int> m_IDChiamato = new DBCursorField<int>("IDChiamato");
            private DBCursorStringField m_NomeChiamato = new DBCursorStringField("NomeChiamato");
            private DBCursorStringField m_DaNumero = new DBCursorStringField("DaNumero");
            private DBCursorStringField m_ANumero = new DBCursorStringField("ANumero");
            private DBCursorStringField m_NomeCanale = new DBCursorStringField("NomeCanale");
            private DBCursorField<QualitaChiamata> m_Qualita = new DBCursorField<QualitaChiamata>("Qualita");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ChiamataRegistrataCursor()
            {
            }

            /// <summary>
            /// IDChiamata
            /// </summary>
            public DBCursorStringField IDChiamata
            {
                get
                {
                    return m_IDChiamata;
                }
            }

            /// <summary>
            /// StatoChiamata
            /// </summary>
            public DBCursorField<StatoChiamataRegistrata> StatoChiamata
            {
                get
                {
                    return m_StatoChiamata;
                }
            }

            /// <summary>
            /// EsitoChiamataEx
            /// </summary>
            public DBCursorStringField EsitoChiamataEx
            {
                get
                {
                    return m_EsitoChiamataEx;
                }
            }

            /// <summary>
            /// EsitoChiamata
            /// </summary>
            public DBCursorField<EsitoChiamataRegistrata> EsitoChiamata
            {
                get
                {
                    return m_EsitoChiamata;
                }
            }

            /// <summary>
            /// StatoChiamataEx
            /// </summary>
            public DBCursorStringField StatoChiamataEx
            {
                get
                {
                    return m_StatoChiamataEx;
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
            /// DataRisposta
            /// </summary>
            public DBCursorField<DateTime> DataRisposta
            {
                get
                {
                    return m_DataRisposta;
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
            /// IDPBX
            /// </summary>
            public DBCursorField<int> IDPBX
            {
                get
                {
                    return m_IDPBX;
                }
            }

            /// <summary>
            /// NomePBX
            /// </summary>
            public DBCursorStringField NomePBX
            {
                get
                {
                    return m_NomePBX;
                }
            }

            /// <summary>
            /// IDChiamante
            /// </summary>
            public DBCursorField<int> IDChiamante
            {
                get
                {
                    return m_IDChiamante;
                }
            }

            /// <summary>
            /// NomeChiamante
            /// </summary>
            public DBCursorStringField NomeChiamante
            {
                get
                {
                    return m_NomeChiamante;
                }
            }

            /// <summary>
            /// IDChiamato
            /// </summary>
            public DBCursorField<int> IDChiamato
            {
                get
                {
                    return m_IDChiamato;
                }
            }

            /// <summary>
            /// NomeChiamato
            /// </summary>
            public DBCursorStringField NomeChiamato
            {
                get
                {
                    return m_NomeChiamato;
                }
            }

            /// <summary>
            /// DaNumero
            /// </summary>
            public DBCursorStringField DaNumero
            {
                get
                {
                    return m_DaNumero;
                }
            }

            /// <summary>
            /// ANumero
            /// </summary>
            public DBCursorStringField ANumero
            {
                get
                {
                    return m_ANumero;
                }
            }

            /// <summary>
            /// NomeCanale
            /// </summary>
            public DBCursorStringField NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }
            }

            /// <summary>
            /// Qualita
            /// </summary>
            public DBCursorField<QualitaChiamata> Qualita
            {
                get
                {
                    return m_Qualita;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.ChiamateRegistrate;
            }
             
        }
    }
}