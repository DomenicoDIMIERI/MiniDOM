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
        /// Cursore di oggetti <see cref="CRichiestaConteggio"/>
        /// </summary>
        [Serializable]
        public class CRichiestaConteggioCursor 
            : Databases.DBObjectCursorPO<CRichiestaConteggio>
        {
            private DBCursorField<int> m_IDRichiestaDiFinanziamento = new DBCursorField<int>("IDRichiestaF");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<int> m_IDIstituto = new DBCursorField<int>("IDIstituto");
            private DBCursorStringField m_NomeIstituto = new DBCursorStringField("NomeIstituto");
            private DBCursorField<int> m_IDAgenziaRichiedente = new DBCursorField<int>("IDAgenziaR");
            private DBCursorStringField m_NomeAgenziaRichiedente = new DBCursorStringField("NomeAgenziaR");
            private DBCursorField<int> m_IDAgente = new DBCursorField<int>("IDAgente");
            private DBCursorStringField m_NomeAgente = new DBCursorStringField("NomeAgente");
            private DBCursorField<DateTime> m_DataEvasione = new DBCursorField<DateTime>("DataEvasione");
            private DBCursorField<int> m_IDPratica = new DBCursorField<int>("IDPratica");
            private DBCursorStringField m_NumeroPratica = new DBCursorStringField("NumeroPratica");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_IDAllegato = new DBCursorField<int>("IDAllegato");
            private DBCursorField<int> m_PresaInCaricoDaID = new DBCursorField<int>("PresaInCaricoDaID");
            private DBCursorField<int> m_PresaInCaricoDaNome = new DBCursorField<int>("PresaInCaricoDaNome");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<DateTime> m_DataSegnalazione = new DBCursorField<DateTime>("DataSegnalazione");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<decimal> m_ImportoCE = new DBCursorField<decimal>("ImportoCE");
            private DBCursorField<int> m_InviatoDaID = new DBCursorField<int>("InviatoDaID");
            private DBCursorStringField m_InviatoDaNome = new DBCursorStringField("InviatoDaNome");
            private DBCursorField<DateTime> m_InviatoIl = new DBCursorField<DateTime>("InviatoIl");
            private DBCursorField<int> m_RicevutoDaID = new DBCursorField<int>("RicevutoDaID");
            private DBCursorStringField m_RicevutoDaNome = new DBCursorStringField("RicevutoDaNome");
            private DBCursorField<DateTime> m_RicevutoIl = new DBCursorField<DateTime>("RicevutoIl");
            private DBCursorStringField m_MezzoDiInvio = new DBCursorStringField("MezzoDiInvio");
            private DBCursorField<int> m_IDFinestraLavorazione = new DBCursorField<int>("IDFinestraLavorazione");
            private DBCursorStringField m_Esito = new DBCursorStringField("Esito");
            private DBCursorField<int> m_IDCessionario = new DBCursorField<int>("IDCessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorField<int> m_DurataMesi = new DBCursorField<int>("DurataMesi");
            private DBCursorField<decimal> m_ImportoRata = new DBCursorField<decimal>("ImportoRata");
            private DBCursorField<double> m_TAN = new DBCursorField<double>("TAN");
            private DBCursorField<double> m_TAEG = new DBCursorField<double>("TAEG");
            private DBCursorField<DateTime> m_DataDecorrenzaPratica = new DBCursorField<DateTime>("DataDecorrenzaPratica");
            private DBCursorField<DateTime> m_UltimaScadenza = new DBCursorField<DateTime>("UltimaScadenza");
            private DBCursorStringField m_ATCPIVA = new DBCursorStringField("ATCPIVA");
            private DBCursorStringField m_ATCDescrizione = new DBCursorStringField("ATCDescrizione");
            private DBCursorField<int> m_IDEstinzione = new DBCursorField<int>("IDEstinzione");
            private DBCursorField<int> m_IDDOCConteggio = new DBCursorField<int>("IDDOCConteggio");
            private DBCursorField<StatoRichiestaConteggio> m_StatoRichiestaConteggio = new DBCursorField<StatoRichiestaConteggio>("StatoRichiestaConteggio");
            // Private m_DataRichiestaConteggio As New DBCursorField(Of DateTime)("DataRichiestaConteggio")
            // Private m_ConteggioRichiestoDaID As New DBCursorField(Of Integer)("ConteggioRichiestoDaID")

            // Private m_DataEsito As New DBCursorField(Of DateTime)("DataEsito")
            // Private m_EsitoUserID As New DBCursorField(Of Integer)("EsitoUserID")
            // Private m_IDDocumentoEsito As New DBCursorField(Of Integer)("IDDocumentoEsito")

            // Private m_NoteRichiestaConteggio As New CCursorFieldObj(Of String)("NoteRichiestaConteggio")
            // Private m_NoteEsito As New CCursorFieldObj(Of String)("NoteEsito")



            /// <summary>
            /// Costruttore
            /// </summary>
            public CRichiestaConteggioCursor()
            {
            }

            /// <summary>
            /// StatoRichiestaConteggio
            /// </summary>
            public DBCursorField<StatoRichiestaConteggio> StatoRichiestaConteggio
            {
                get
                {
                    return m_StatoRichiestaConteggio;
                }
            }

            // Public ReadOnly Property DataRichiestaConteggio As DBCursorField(Of DateTime)
            // Get
            // Return Me.m_DataRichiestaConteggio
            // End Get
            // End Property

            // Public ReadOnly Property ConteggioRichiestoDaID As DBCursorField(Of Integer)
            // Get
            // Return Me.m_ConteggioRichiestoDaID
            // End Get
            // End Property

            // Public ReadOnly Property DataEsito As DBCursorField(Of DateTime)
            // Get
            // Return Me.m_DataEsito
            // End Get
            // End Property

            // Public ReadOnly Property EsitoUserID As DBCursorField(Of Integer)
            // Get
            // Return Me.m_EsitoUserID
            // End Get
            // End Property

            // Public ReadOnly Property IDDocumentoEsito As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDDocumentoEsito
            // End Get
            // End Property

            // Public ReadOnly Property NoteRichiestaConteggio As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NoteRichiestaConteggio
            // End Get
            // End Property

            // Public ReadOnly Property NoteEsito As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NoteEsito
            // End Get
            // End Property

            /// <summary>
            /// IDDOCConteggio
            /// </summary>
            public DBCursorField<int> IDDOCConteggio
            {
                get
                {
                    return m_IDDOCConteggio;
                }
            }


            /// <summary>
            /// Esito
            /// </summary>
            public DBCursorStringField Esito
            {
                get
                {
                    return m_Esito;
                }
            }

            /// <summary>
            /// IDEstinzione
            /// </summary>
            public DBCursorField<int> IDEstinzione
            {
                get
                {
                    return m_IDEstinzione;
                }
            }

            /// <summary>
            /// IDFinestraLavorazione
            /// </summary>
            public DBCursorField<int> IDFinestraLavorazione
            {
                get
                {
                    return m_IDFinestraLavorazione;
                }
            }

            /// <summary>
            /// InviatoDaID
            /// </summary>
            public DBCursorField<int> InviatoDaID
            {
                get
                {
                    return m_InviatoDaID;
                }
            }

            /// <summary>
            /// InviatoDaNome
            /// </summary>
            public DBCursorStringField InviatoDaNome
            {
                get
                {
                    return m_InviatoDaNome;
                }
            }

            /// <summary>
            /// InviatoIl
            /// </summary>
            public DBCursorField<DateTime> InviatoIl
            {
                get
                {
                    return m_InviatoIl;
                }
            }

            /// <summary>
            /// RicevutoDaID
            /// </summary>
            public DBCursorField<int> RicevutoDaID
            {
                get
                {
                    return m_RicevutoDaID;
                }
            }

            /// <summary>
            /// RicevutoDaNome
            /// </summary>
            public DBCursorStringField RicevutoDaNome
            {
                get
                {
                    return m_RicevutoDaNome;
                }
            }

            /// <summary>
            /// RicevutoIl
            /// </summary>
            public DBCursorField<DateTime> RicevutoIl
            {
                get
                {
                    return m_RicevutoIl;
                }
            }

            /// <summary>
            /// MezzoDiInvio
            /// </summary>
            public DBCursorStringField MezzoDiInvio
            {
                get
                {
                    return m_MezzoDiInvio;
                }
            }

            /// <summary>
            /// ImportoCE
            /// </summary>
            public DBCursorField<decimal> ImportoCE
            {
                get
                {
                    return m_ImportoCE;
                }
            }

            /// <summary>
            /// IDPratica
            /// </summary>
            public DBCursorField<int> IDPratica
            {
                get
                {
                    return m_IDPratica;
                }
            }

            /// <summary>
            /// NumeroPratica
            /// </summary>
            public DBCursorStringField NumeroPratica
            {
                get
                {
                    return m_NumeroPratica;
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
            /// IDAllegato
            /// </summary>
            public DBCursorField<int> IDAllegato
            {
                get
                {
                    return m_IDAllegato;
                }
            }

            /// <summary>
            /// PresaInCaricoDaID
            /// </summary>
            public DBCursorField<int> PresaInCaricoDaID
            {
                get
                {
                    return m_PresaInCaricoDaID;
                }
            }

            /// <summary>
            /// PresaInCaricoDaNome
            /// </summary>
            public DBCursorField<int> PresaInCaricoDaNome
            {
                get
                {
                    return m_PresaInCaricoDaNome;
                }
            }

            /// <summary>
            /// DataPresaInCarico
            /// </summary>
            public DBCursorField<DateTime> DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
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
            /// DataRichiesta
            /// </summary>
            public DBCursorField<DateTime> DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }
            }

            /// <summary>
            /// DataSegnalazione
            /// </summary>
            public DBCursorField<DateTime> DataSegnalazione
            {
                get
                {
                    return m_DataSegnalazione;
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
            /// IDAgenziaRichiedente
            /// </summary>
            public DBCursorField<int> IDAgenziaRichiedente
            {
                get
                {
                    return m_IDAgenziaRichiedente;
                }
            }

            /// <summary>
            /// NomeAgenziaRichiedente
            /// </summary>
            public DBCursorStringField NomeAgenziaRichiedente
            {
                get
                {
                    return m_NomeAgenziaRichiedente;
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
            /// IDCessionario
            /// </summary>
            public DBCursorField<int> IDCessionario
            {
                get
                {
                    return m_IDCessionario;
                }
            }

            /// <summary>
            /// NomeCessionario
            /// </summary>
            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            /// <summary>
            /// DurataMesi
            /// </summary>
            public DBCursorField<int> DurataMesi
            {
                get
                {
                    return m_DurataMesi;
                }
            }

            /// <summary>
            /// ImportoRata
            /// </summary>
            public DBCursorField<decimal> ImportoRata
            {
                get
                {
                    return m_ImportoRata;
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
            /// DataDecorrenzaPratica
            /// </summary>
            public DBCursorField<DateTime> DataDecorrenzaPratica
            {
                get
                {
                    return m_DataDecorrenzaPratica;
                }
            }

            /// <summary>
            /// UltimaScadenza
            /// </summary>
            public DBCursorField<DateTime> UltimaScadenza
            {
                get
                {
                    return m_UltimaScadenza;
                }
            }

            /// <summary>
            /// ATCPIVA
            /// </summary>
            public DBCursorStringField ATCPIVA
            {
                get
                {
                    return m_ATCPIVA;
                }
            }

            /// <summary>
            /// ATCDescrizione
            /// </summary>
            public DBCursorStringField ATCDescrizione
            {
                get
                {
                    return m_ATCDescrizione;
                }
            }
             

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.RichiesteConteggi;
            }
             
        }
    }
}