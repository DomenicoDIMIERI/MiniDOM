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
using static minidom.Contabilita;

namespace minidom
{
    public partial class Contabilita
    {

        /// <summary>
        /// Cursore di oggetti <see cref="DocumentoContabile"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class DocumentoContabileCursor
            : minidom.Databases.DBObjectCursorPO<DocumentoContabile>
        {
            private DBCursorStringField m_TipoDocumento = new DBCursorStringField("TipoDocumento");
            private DBCursorStringField m_NumeroDocumento = new DBCursorStringField("NumeroDocumento");
            private DBCursorField<DateTime> m_DataRegistrazione = new DBCursorField<DateTime>("DataRegistrazione");
            private DBCursorField<DateTime> m_DataEmissione = new DBCursorField<DateTime>("DataEmissione");
            private DBCursorField<DateTime> m_DataEvasione = new DBCursorField<DateTime>("DataEvasione");
            private DBCursorField<StatoDocumentoContabile> m_StatoDocumento = new DBCursorField<StatoDocumentoContabile>("StatoDocumento");
            private DBCursorField<decimal> m_TotaleImponibile = new DBCursorField<decimal>("TotaleImponibile");
            private DBCursorField<decimal> m_TotaleIvato = new DBCursorField<decimal>("TotaleIvato");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_IndirizzoCliente_Nome = new DBCursorStringField("INDCLT_LABEL");
            private DBCursorStringField m_IndirizzoCliente_ToponimoViaECivico = new DBCursorStringField("INDCLT_VIA");
            private DBCursorStringField m_IndirizzoCliente_CAP = new DBCursorStringField("INDCLT_CAP");
            private DBCursorStringField m_IndirizzoCliente_Citta = new DBCursorStringField("INDCLT_CITTA");
            private DBCursorStringField m_IndirizzoCliente_Provincia = new DBCursorStringField("INDCLT_PROV");
            private DBCursorStringField m_CodiceFiscaleCliente = new DBCursorStringField("CodiceFiscaleCliente");
            private DBCursorStringField m_PartitaIVACliente = new DBCursorStringField("PartitaIVACliente");
            private DBCursorField<int> m_IDFornitore = new DBCursorField<int>("IDFornitore");
            private DBCursorStringField m_NomeFornitore = new DBCursorStringField("NomeFornitore");
            private DBCursorStringField m_IndirizzoFornitore_Nome = new DBCursorStringField("INDFNT_LABEL");
            private DBCursorStringField m_IndirizzoFornitore_ToponimoViaECivico = new DBCursorStringField("INDFNT_VIA");
            private DBCursorStringField m_IndirizzoFornitore_CAP = new DBCursorStringField("INDFNT_CAP");
            private DBCursorStringField m_IndirizzoFornitore_Citta = new DBCursorStringField("INDFNT_CITTA");
            private DBCursorStringField m_IndirizzoFornitore_Provincia = new DBCursorStringField("INDFNT_PROV");
            private DBCursorStringField m_CodiceFiscaleFornitore = new DBCursorStringField("CodiceFiscaleFornitore");
            private DBCursorStringField m_PartitaIVAFornitore = new DBCursorStringField("PartitaIVAFornitore");
            private DBCursorStringField m_IndirizzoSpedizione_Nome = new DBCursorStringField("INDSPD_LABEL");
            private DBCursorStringField m_IndirizzoSpedizione_ToponimoViaECivico = new DBCursorStringField("INDSPD_VIA");
            private DBCursorStringField m_IndirizzoSpedizione_CAP = new DBCursorStringField("INDSPD_CAP");
            private DBCursorStringField m_IndirizzoSpedizione_Citta = new DBCursorStringField("INDSPD_CITTA");
            private DBCursorStringField m_IndirizzoSpedizione_Provincia = new DBCursorStringField("INDSPD_PROV");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_SourceType = new DBCursorStringField("SourceType");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorStringField m_SourceParams = new DBCursorStringField("SourceParams");

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentoContabileCursor()
            {
            }

            /// <summary>
            /// TipoDocumento
            /// </summary>
            public DBCursorStringField TipoDocumento
            {
                get
                {
                    return m_TipoDocumento;
                }
            }

            /// <summary>
            /// NumeroDocumento
            /// </summary>
            public DBCursorStringField NumeroDocumento
            {
                get
                {
                    return m_NumeroDocumento;
                }
            }

            /// <summary>
            /// DataRegistrazione
            /// </summary>
            public DBCursorField<DateTime> DataRegistrazione
            {
                get
                {
                    return m_DataRegistrazione;
                }
            }

            /// <summary>
            /// DataEmissione
            /// </summary>
            public DBCursorField<DateTime> DataEmissione
            {
                get
                {
                    return m_DataEmissione;
                }
            }

            /// <summary>
            /// DataEvasione
            /// </summary>
            public DBCursorField<DateTime> DataEvasione
            {
                get
                {
                    return m_DataEvasione;
                }
            }

            /// <summary>
            /// StatoDocumento
            /// </summary>
            public DBCursorField<StatoDocumentoContabile> StatoDocumento
            {
                get
                {
                    return m_StatoDocumento;
                }
            }

            /// <summary>
            /// TotaleImponibile
            /// </summary>
            public DBCursorField<decimal> TotaleImponibile
            {
                get
                {
                    return m_TotaleImponibile;
                }
            }

            /// <summary>
            /// TotaleIvato
            /// </summary>
            public DBCursorField<decimal> TotaleIvato
            {
                get
                {
                    return m_TotaleIvato;
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
            /// IndirizzoCliente_Nome
            /// </summary>
            public DBCursorStringField IndirizzoCliente_Nome
            {
                get
                {
                    return m_IndirizzoCliente_Nome;
                }
            }

            /// <summary>
            /// IndirizzoCliente_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoCliente_ToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoCliente_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// IndirizzoCliente_CAP
            /// </summary>
            public DBCursorStringField IndirizzoCliente_CAP
            {
                get
                {
                    return m_IndirizzoCliente_CAP;
                }
            }

            /// <summary>
            /// IndirizzoCliente_Citta
            /// </summary>
            public DBCursorStringField IndirizzoCliente_Citta
            {
                get
                {
                    return m_IndirizzoCliente_Citta;
                }
            }

            /// <summary>
            /// IndirizzoCliente_Provincia
            /// </summary>
            public DBCursorStringField IndirizzoCliente_Provincia
            {
                get
                {
                    return m_IndirizzoCliente_Provincia;
                }
            }

            /// <summary>
            /// CodiceFiscaleCliente
            /// </summary>
            public DBCursorStringField CodiceFiscaleCliente
            {
                get
                {
                    return m_CodiceFiscaleCliente;
                }
            }

            /// <summary>
            /// PartitaIVACliente
            /// </summary>
            public DBCursorStringField PartitaIVACliente
            {
                get
                {
                    return m_PartitaIVACliente;
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
            /// IndirizzoFornitore_Nome
            /// </summary>
            public DBCursorStringField IndirizzoFornitore_Nome
            {
                get
                {
                    return m_IndirizzoFornitore_Nome;
                }
            }

            /// <summary>
            /// IndirizzoFornitore_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoFornitore_ToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoFornitore_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// IndirizzoFornitore_CAP
            /// </summary>
            public DBCursorStringField IndirizzoFornitore_CAP
            {
                get
                {
                    return m_IndirizzoFornitore_CAP;
                }
            }

            /// <summary>
            /// IndirizzoFornitore_Citta
            /// </summary>
            public DBCursorStringField IndirizzoFornitore_Citta
            {
                get
                {
                    return m_IndirizzoFornitore_Citta;
                }
            }

            /// <summary>
            /// IndirizzoFornitore_Provincia
            /// </summary>
            public DBCursorStringField IndirizzoFornitore_Provincia
            {
                get
                {
                    return m_IndirizzoFornitore_Provincia;
                }
            }

            /// <summary>
            /// CodiceFiscaleFornitore
            /// </summary>
            public DBCursorStringField CodiceFiscaleFornitore
            {
                get
                {
                    return m_CodiceFiscaleFornitore;
                }
            }

            /// <summary>
            /// PartitaIVAFornitore
            /// </summary>
            public DBCursorStringField PartitaIVAFornitore
            {
                get
                {
                    return m_PartitaIVAFornitore;
                }
            }

            /// <summary>
            /// IndirizzoSpedizione_Nome
            /// </summary>
            public DBCursorStringField IndirizzoSpedizione_Nome
            {
                get
                {
                    return m_IndirizzoSpedizione_Nome;
                }
            }

            /// <summary>
            /// IndirizzoSpedizione_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoSpedizione_ToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoSpedizione_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// IndirizzoSpedizione_CAP
            /// </summary>
            public DBCursorStringField IndirizzoSpedizione_CAP
            {
                get
                {
                    return m_IndirizzoSpedizione_CAP;
                }
            }

            /// <summary>
            /// IndirizzoSpedizione_Citta
            /// </summary>
            public DBCursorStringField IndirizzoSpedizione_Citta
            {
                get
                {
                    return m_IndirizzoSpedizione_Citta;
                }
            }

            /// <summary>
            /// IndirizzoSpedizione_Provincia
            /// </summary>
            public DBCursorStringField IndirizzoSpedizione_Provincia
            {
                get
                {
                    return m_IndirizzoSpedizione_Provincia;
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
                return minidom.Contabilita.DocumentiContabili;
            }
        }
    }
}