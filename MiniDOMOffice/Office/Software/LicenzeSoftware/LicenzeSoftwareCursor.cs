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
        /// Cursore di <see cref="LicenzaSoftware"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class LicenzeSoftwareCursor
            : minidom.Databases.DBObjectCursorPO<LicenzaSoftware>
        {
            private DBCursorField<int> m_IDSoftware = new DBCursorField<int>("IDSoftware");
            private DBCursorStringField m_NomeSoftware = new DBCursorStringField("NomeSoftware");
            private DBCursorField<int> m_IDDispositivo = new DBCursorField<int>("IDDispositivo");
            private DBCursorStringField m_NomeDispositivo = new DBCursorStringField("NomeDispositivo");
            private DBCursorStringField m_CodiceLicenza = new DBCursorStringField("CodiceLicenza");
            private DBCursorField<DateTime> m_DataAcquisto = new DBCursorField<DateTime>("DataAcquisto");
            private DBCursorField<DateTime> m_DataInstallazione = new DBCursorField<DateTime>("DataInstallazione");
            private DBCursorField<DateTime> m_DataDismissione = new DBCursorField<DateTime>("DataDismissione");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");
            private DBCursorStringField m_ScaricatoDa = new DBCursorStringField("ScaricatoDa");
            private DBCursorField<StatoLicenzaSoftware> m_StatoUtilizzo = new DBCursorField<StatoLicenzaSoftware>("StatoUtilizzo");
            private DBCursorField<FlagsLicenzaSoftware> m_Flags = new DBCursorField<FlagsLicenzaSoftware>("Flags");
            private DBCursorField<int> m_IDProprietario = new DBCursorField<int>("IDProprietario");
            private DBCursorStringField m_NomeProprietario = new DBCursorStringField("NomeProprietario");
            private DBCursorField<int> m_IDDocumentoAcquisto = new DBCursorField<int>("IDDocumentoAcquisto");
            private DBCursorStringField m_NumeroDocumentoAcquisto = new DBCursorStringField("NumeroDocumentoAcquisto");

            /// <summary>
            /// Costruttore
            /// </summary>
            public LicenzeSoftwareCursor()
            {
            }

            /// <summary>
            /// IDSoftware
            /// </summary>
            public DBCursorField<int> IDSoftware
            {
                get
                {
                    return m_IDSoftware;
                }
            }

            /// <summary>
            /// NomeSoftware
            /// </summary>
            public DBCursorStringField NomeSoftware
            {
                get
                {
                    return m_NomeSoftware;
                }
            }

            /// <summary>
            /// IDDispositivo
            /// </summary>
            public DBCursorField<int> IDDispositivo
            {
                get
                {
                    return m_IDDispositivo;
                }
            }

            /// <summary>
            /// NomeDispositivo
            /// </summary>
            public DBCursorStringField NomeDispositivo
            {
                get
                {
                    return m_NomeDispositivo;
                }
            }

            /// <summary>
            /// CodiceLicenza
            /// </summary>
            public DBCursorStringField CodiceLicenza
            {
                get
                {
                    return m_CodiceLicenza;
                }
            }

            /// <summary>
            /// DataAcquisto
            /// </summary>
            public DBCursorField<DateTime> DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }
            }

            /// <summary>
            /// DataInstallazione
            /// </summary>
            public DBCursorField<DateTime> DataInstallazione
            {
                get
                {
                    return m_DataInstallazione;
                }
            }

            /// <summary>
            /// DataDismissione
            /// </summary>
            public DBCursorField<DateTime> DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }
            }

            /// <summary>
            /// DettaglioStato
            /// </summary>
            public DBCursorStringField DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }
            }

            /// <summary>
            /// ScaricatoDa
            /// </summary>
            public DBCursorStringField ScaricatoDa
            {
                get
                {
                    return m_ScaricatoDa;
                }
            }

            /// <summary>
            /// StatoUtilizzo
            /// </summary>
            public DBCursorField<StatoLicenzaSoftware> StatoUtilizzo
            {
                get
                {
                    return m_StatoUtilizzo;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<FlagsLicenzaSoftware> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// IDProprietario
            /// </summary>
            public DBCursorField<int> IDProprietario
            {
                get
                {
                    return m_IDProprietario;
                }
            }

            /// <summary>
            /// NomeProprietario
            /// </summary>
            public DBCursorStringField NomeProprietario
            {
                get
                {
                    return m_NomeProprietario;
                }
            }

            /// <summary>
            /// IDDocumentoAcquisto
            /// </summary>
            public DBCursorField<int> IDDocumentoAcquisto
            {
                get
                {
                    return m_IDDocumentoAcquisto;
                }
            }

            /// <summary>
            /// NumeroDocumentoAcquisto
            /// </summary>
            public DBCursorStringField NumeroDocumentoAcquisto
            {
                get
                {
                    return m_NumeroDocumentoAcquisto;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.LicenzeSoftware;
            }
 
        }
    }
}