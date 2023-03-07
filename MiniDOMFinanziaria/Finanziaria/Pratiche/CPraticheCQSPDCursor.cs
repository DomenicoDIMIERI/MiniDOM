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
        /// Cursore sulla tabella delle pratiche
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CPraticheCQSPDCursor
            : Databases.DBObjectCursorPO<CPraticaCQSPD>
        {
            private string m_RinnovaDa = DMD.Strings.vbNullString;
            private DBCursorStringField m_Nominativo = new DBCursorStringField("Nominativo");
            private DBCursorField<int> m_IDConsulente = new DBCursorField<int>("IDConsulente");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorField<int> m_IDOperatoreTrasferita = new DBCursorField<int>("TrasferitoDa");
            private DBCursorField<bool> m_DaVedere = new DBCursorField<bool>("(([tbl_Pratiche].[Flags] AND " + ((int)PraticaFlags.DAVEDERE).ToString() + ")=" + ((int)PraticaFlags.DAVEDERE).ToString() + ")");
            private DBCursorField<bool> m_Trasferita = new DBCursorField<bool>("(([tbl_Pratiche].[Flags] AND " + ((int)PraticaFlags.TRASFERITA).ToString() + ")=" + ((int)PraticaFlags.TRASFERITA).ToString() + ")");
            // Private m_MotivoScontoDettaglio As New CCursorFieldObj(Of String)("MotivoScontoDettaglio")
            private DBCursorField<DateTime> m_ScontoAutorizzatoIl = new DBCursorField<DateTime>("ScontoAutorizzatoIl");
            private DBCursorStringField m_ScontoNomeMotivo = new DBCursorStringField("ScontoNomeMotivo");
            private DBCursorStringField m_NomeAmministrazione = new DBCursorStringField("Ente");
            private DBCursorField<int> m_IDAmministrazione = new DBCursorField<int>("IDAmministrazione");
            private DBCursorField<int> m_IDEntePagante = new DBCursorField<int>("IDEntePagante");
            private DBCursorField<int> m_IDScontoAutorizzatoDa = new DBCursorField<int>("IDScontoAutorizzatoDa");
            private DBCursorField<double> m_Spread = new DBCursorField<double>("(IIF([MontanteLordo]>0, 100*(([UpFront] + [Running])/[MontanteLordo]), 0))");
            private DBCursorField<double> m_Rappel = new DBCursorField<double>("(IIF([MontanteLordo]>0, 100*([Rappel]/[MontanteLordo]), 0))");
            private DBCursorField<double> m_PremioDaCessionario = new DBCursorField<double>("PremioDaCessionario");
            private DBCursorField<double> m_ValoreRappel = new DBCursorField<double>("Rappel");
            private DBCursorField<decimal> m_MontanteLordo = new DBCursorField<decimal>("MontanteLordo");
            private DBCursorField<DateTime> m_DataDecorrenza = new DBCursorField<DateTime>("DataDecorrenza");
            private DBCursorField<PraticaFlags> m_Flags = new DBCursorField<PraticaFlags>("[tbl_Pratiche].Flags");
            private DBCursorField<int> m_IDCorrezione = new DBCursorField<int>("IDCorrezione");
            private DBCursorField<int> m_IDStatoAttuale = new DBCursorField<int>("IDStatoAttuale");
            private DBCursorField<int> m_IDConsulenza = new DBCursorField<int>("IDConsulenza");
            private DBCursorField<int> m_IDRichiestaDiFinanziamento = new DBCursorField<int>("IDRichiestaFinanziamento");
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_CognomeCliente = new DBCursorStringField("CognomeCliente");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("Cliente");
            private DBCursorStringField m_NatoAComune = new DBCursorStringField("NatoAComune");
            private DBCursorStringField m_NatoAProvincia = new DBCursorStringField("NatoAProvincia");
            private DBCursorField<DateTime> m_NatoIl = new DBCursorField<DateTime>("NatoIl");
            private DBCursorStringField m_ResidenteAComune = new DBCursorStringField("ResidenteAComune");
            private DBCursorStringField m_ResidenteAProvincia = new DBCursorStringField("ResidenteAProvincia");
            private DBCursorStringField m_ResidenteACAP = new DBCursorStringField("ResidenteACAP");
            private DBCursorStringField m_ResidenteAVia = new DBCursorStringField("ResidenteAVia");
            private DBCursorStringField m_CodiceFiscale = new DBCursorStringField("CodiceFiscale");
            private DBCursorStringField m_PartitaIVA = new DBCursorStringField("PartitaIVA");
            private DBCursorField<int> m_CommercialeID = new DBCursorField<int>("Commerciale");
            private DBCursorStringField m_NomeProdotto = new DBCursorStringField("CQS_PD");
            private DBCursorField<int> m_ProdottoID = new DBCursorField<int>("Prodotto");
            private DBCursorStringField m_TipoFonteContatto = new DBCursorStringField("TipoFonteContatto");
            private DBCursorStringField m_NomeFonte = new DBCursorStringField("FonteContatto");
            private DBCursorStringField m_NomeProfilo = new DBCursorStringField("NomeProfilo");
            private DBCursorField<int> m_IDProfilo = new DBCursorField<int>("Profilo");
            private DBCursorField<decimal> m_NettoRicavo = new DBCursorField<decimal>("NettoRicavo");
            private DBCursorField<int> m_IDCessionario = new DBCursorField<int>("Cessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorField<StatoPraticaEnum> m_StatoPratica = new DBCursorField<StatoPraticaEnum>("StatoPratica");
            private DBCursorField<int> m_NumeroRate = new DBCursorField<int>("NumeroRate");
            private DBCursorField<int> m_IDCanale = new DBCursorField<int>("IDCanale");
            private DBCursorStringField m_NomeCanale = new DBCursorStringField("NomeCanale");
            private DBCursorField<int> m_IDCanale1 = new DBCursorField<int>("IDCanale1");
            private DBCursorStringField m_NomeCanale1 = new DBCursorStringField("NomeCanale1");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorField<int> m_IDRichiestaApprovazione = new DBCursorField<int>("IDRichiestaApprovazione");
            private DBCursorField<decimal> m_Rata = new DBCursorField<decimal>("[MontanteLordo]/IIF([NumeroRate]>0,[NumeroRate],1)");
            private DBCursorField<StatoRichiestaApprovazione> m_StatoRichiestaApprovazione = new DBCursorField<StatoRichiestaApprovazione>("StatoRichiestaApprovazione");
            private DBCursorStringField m_NumeroEsterno = new DBCursorStringField("StatRichD_Params");
            private DBCursorField<int> m_IDScontoRichiestoDa = new DBCursorField<int>("IDScontoRichiestoDa");
            private DBCursorStringField m_TipoFonteCliente = new DBCursorStringField("TipoFonteCliente");
            private DBCursorField<int> m_IDFonteCliente = new DBCursorField<int>("IDFonteCliente");
            private DBCursorField<int> m_IDFinestraLavorazione = new DBCursorField<int>("IDFinestraLavorazione");
            private DBCursorField<int> m_IDTabellaFinanziaria = new DBCursorField<int>("IDTabellaFinanziaria");
            private DBCursorField<int> m_IDTabellaVita = new DBCursorField<int>("IDTabellaVita");
            private DBCursorField<int> m_IDTabellaImpiego = new DBCursorField<int>("IDTabellaImpiego");
            private DBCursorField<int> m_IDTabellaCredito = new DBCursorField<int>("IDTabellaCredito");
            private DBCursorField<int> m_IDUltimaVerifica = new DBCursorField<int>("IDUltimaVerifica");
            private DBCursorField<DateTime> m_DataValuta = new DBCursorField<DateTime>("DataValuta");
            private DBCursorField<DateTime> m_DataStampaSecci = new DBCursorField<DateTime>("DataStampaSecci");
            private DBCursorField<decimal> m_CapitaleFinanziato = new DBCursorField<decimal>("CapitaleFinanziato");
            private DBCursorStringField m_CategoriaProdotto = new DBCursorStringField("CategoriaProdotto");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private CCollection<CInfoStato> m_PassaggiDiStato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPraticheCQSPDCursor()
            {
            }

            /// <summary>
            /// IDCollaboratore
            /// </summary>
            public DBCursorField<int> IDCollaboratore
            {
                get
                {
                    return m_IDCollaboratore;
                }
            }

            /// <summary>
            /// CategoriaProdotto
            /// </summary>
            public DBCursorStringField CategoriaProdotto
            {
                get
                {
                    return m_CategoriaProdotto;
                }
            }

            /// <summary>
            /// PremioDaCessionario
            /// </summary>
            public DBCursorField<double> PremioDaCessionario
            {
                get
                {
                    return m_PremioDaCessionario;
                }
            }

            /// <summary>
            /// DataValuta
            /// </summary>
            public DBCursorField<DateTime> DataValuta
            {
                get
                {
                    return m_DataValuta;
                }
            }

            /// <summary>
            /// DataStampaSecci
            /// </summary>
            public DBCursorField<DateTime> DataStampaSecci
            {
                get
                {
                    return m_DataStampaSecci;
                }
            }

            /// <summary>
            /// IDUltimaVerifica
            /// </summary>
            public DBCursorField<int> IDUltimaVerifica
            {
                get
                {
                    return m_IDUltimaVerifica;
                }
            }

            /// <summary>
            /// IDTabellaFinanziaria
            /// </summary>
            public DBCursorField<int> IDTabellaFinanziaria
            {
                get
                {
                    return m_IDTabellaFinanziaria;
                }
            }

            /// <summary>
            /// IDTabellaVita
            /// </summary>
            public DBCursorField<int> IDTabellaVita
            {
                get
                {
                    return m_IDTabellaVita;
                }
            }

            /// <summary>
            /// IDTabellaImpiego
            /// </summary>
            public DBCursorField<int> IDTabellaImpiego
            {
                get
                {
                    return m_IDTabellaImpiego;
                }
            }

            /// <summary>
            /// IDTabellaCredito
            /// </summary>
            public DBCursorField<int> IDTabellaCredito
            {
                get
                {
                    return m_IDTabellaCredito;
                }
            }

            /// <summary>
            /// DaVedere
            /// </summary>
            public DBCursorField<bool> DaVedere
            {
                get
                {
                    return m_DaVedere;
                }
            }

            /// <summary>
            /// Trasferita
            /// </summary>
            public DBCursorField<bool> Trasferita
            {
                get
                {
                    return m_Trasferita;
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
            /// Rappel
            /// </summary>
            public DBCursorField<double> Rappel
            {
                get
                {
                    return m_Rappel;
                }
            }

            /// <summary>
            /// ValoreRappel
            /// </summary>
            public DBCursorField<double> ValoreRappel
            {
                get
                {
                    return m_ValoreRappel;
                }
            }

            /// <summary>
            /// TipoFonteCliente
            /// </summary>
            public DBCursorStringField TipoFonteCliente
            {
                get
                {
                    return m_TipoFonteCliente;
                }
            }

            /// <summary>
            /// IDFonteCliente
            /// </summary>
            public DBCursorField<int> IDFonteCliente
            {
                get
                {
                    return m_IDFonteCliente;
                }
            }

            /// <summary>
            /// IDScontoRichiestoDa
            /// </summary>
            public DBCursorField<int> IDScontoRichiestoDa
            {
                get
                {
                    return m_IDScontoRichiestoDa;
                }
            }

            /// <summary>
            /// NumeroEsterno
            /// </summary>
            public DBCursorStringField NumeroEsterno
            {
                get
                {
                    return m_NumeroEsterno;
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
            /// IDRichiestaApprovazione
            /// </summary>
            public DBCursorField<int> IDRichiestaApprovazione
            {
                get
                {
                    return m_IDRichiestaApprovazione;
                }
            }

            /// <summary>
            /// IDContesto
            /// </summary>
            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            /// <summary>
            /// TipoContesto
            /// </summary>
            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            /// <summary>
            /// IDCanale
            /// </summary>
            public DBCursorField<int> IDCanale
            {
                get
                {
                    return m_IDCanale;
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
            /// IDCanale1
            /// </summary>
            public DBCursorField<int> IDCanale1
            {
                get
                {
                    return m_IDCanale1;
                }
            }

            /// <summary>
            /// NomeCanale1
            /// </summary>
            public DBCursorStringField NomeCanale1
            {
                get
                {
                    return m_NomeCanale1;
                }
            }

            /// <summary>
            /// IDProfilo
            /// </summary>
            public DBCursorField<int> IDProfilo
            {
                get
                {
                    return m_IDProfilo;
                }
            }

            /// <summary>
            /// IDConsulenza
            /// </summary>
            public DBCursorField<int> IDConsulenza
            {
                get
                {
                    return m_IDConsulenza;
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
            /// IDCorrezione
            /// </summary>
            public DBCursorField<int> IDCorrezione
            {
                get
                {
                    return m_IDCorrezione;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<PraticaFlags> Flags
            {
                get
                {
                    return m_Flags;
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
            /// RinnovaDa
            /// </summary>
            public string RinnovaDa
            {
                get
                {
                    return m_RinnovaDa;
                }

                set
                {
                    value = Strings.Trim(value);
                    if (Strings.EQ(this.m_RinnovaDa, value))
                        return;
                    this.m_RinnovaDa = value;
                    this.Reset1();
                }
            }

            /// <summary>
            /// Spread
            /// </summary>
            public DBCursorField<double> Spread
            {
                get
                {
                    return m_Spread;
                }
            }

            // Public ReadOnly Property IDOperatoreContatto As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreContatto
            // End Get
            // End Property

            // 'Private m_NomeOperatoreContatto As New CCursorFieldObj(Of String)("NomeOperatoreContatto")
            // Public ReadOnly Property IDOperatoreRichiestaDelibera As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreRichiestaDelibera
            // End Get
            // End Property

            // Public ReadOnly Property IDOperatoreDeliberata As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreDeliberata
            // End Get
            // End Property

            // Public ReadOnly Property IDOperatoreProntaPerLiquidazione As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreProntaPerLiquidazione
            // End Get
            // End Property

            // Public ReadOnly Property IDOperatoreLiquidata As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreLiquidata
            // End Get
            // End Property

            // Public ReadOnly Property IDOperatoreArchiviata As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreArchiviata
            // End Get
            // End Property

            /// <summary>
            /// IDOperatoreTrasferita
            /// </summary>
            public DBCursorField<int> IDOperatoreTrasferita
            {
                get
                {
                    return m_IDOperatoreTrasferita;
                }
            }

            // Public ReadOnly Property IDOperatoreAnnullata As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDOperatoreAnnullata
            // End Get
            // End Property

            /// <summary>
            /// NomeAmministrazione
            /// </summary>
            public DBCursorStringField NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }
            }

            /// <summary>
            /// IDAmministrazione
            /// </summary>
            public DBCursorField<int> IDAmministrazione
            {
                get
                {
                    return m_IDAmministrazione;
                }
            }

            // Public ReadOnly Property NomeEntePagante As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NomeEntePagante
            // End Get
            // End Property

            /// <summary>
            /// IDEntePagante
            /// </summary>
            public DBCursorField<int> IDEntePagante
            {
                get
                {
                    return m_IDEntePagante;
                }
            }

            /// <summary>
            /// NumeroRate
            /// </summary>
            public DBCursorField<int> NumeroRate
            {
                get
                {
                    return m_NumeroRate;
                }
            }

            // Public ReadOnly Property Running As DBCursorField(Of Double)
            // Get
            // Return Me.m_Running
            // End Get
            // End Property

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
            /// CognomeCliente
            /// </summary>
            public DBCursorStringField CognomeCliente
            {
                get
                {
                    return m_CognomeCliente;
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
            /// NatoAComune
            /// </summary>
            public DBCursorStringField NatoAComune
            {
                get
                {
                    return m_NatoAComune;
                }
            }

            /// <summary>
            /// NatoAProvincia
            /// </summary>
            public DBCursorStringField NatoAProvincia
            {
                get
                {
                    return m_NatoAProvincia;
                }
            }

            /// <summary>
            /// NatoIl
            /// </summary>
            public DBCursorField<DateTime> NatoIl
            {
                get
                {
                    return m_NatoIl;
                }
            }

            /// <summary>
            /// ResidenteAComune
            /// </summary>
            public DBCursorStringField ResidenteAComune
            {
                get
                {
                    return m_ResidenteAComune;
                }
            }

            /// <summary>
            /// ResidenteAProvincia
            /// </summary>
            public DBCursorStringField ResidenteAProvincia
            {
                get
                {
                    return m_ResidenteAProvincia;
                }
            }

            /// <summary>
            /// ResidenteACAP
            /// </summary>
            public DBCursorStringField ResidenteACAP
            {
                get
                {
                    return m_ResidenteACAP;
                }
            }

            /// <summary>
            /// ResidenteAVia
            /// </summary>
            public DBCursorStringField ResidenteAVia
            {
                get
                {
                    return m_ResidenteAVia;
                }
            }

            /// <summary>
            /// CodiceFiscale
            /// </summary>
            public DBCursorStringField CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }
            }

            /// <summary>
            /// PartitaIVA
            /// </summary>
            public DBCursorStringField PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }
            }

            // Public ReadOnly Property NomeConsulente As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NomeConsulente
            // End Get
            // End Property

            /// <summary>
            /// IDConsulente
            /// </summary>
            public DBCursorField<int> IDConsulente
            {
                get
                {
                    return m_IDConsulente;
                }
            }

            /// <summary>
            /// TipoFonte
            /// </summary>
            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonteContatto;
                }
            }

            /// <summary>
            /// NomeFonte
            /// </summary>
            public DBCursorStringField NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }
            }

            /// <summary>
            /// IDFonte
            /// </summary>
            public DBCursorField<int> IDFonte
            {
                get
                {
                    return m_IDFonte;
                }
            }

            // Public ReadOnly Property ProvvMax As DBCursorField(Of Double)
            // Get
            // Return Me.m_ProvvMax
            // End Get
            // End Property

            // Public ReadOnly Property MotivoSconto As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_MotivoSconto
            // End Get
            // End Property

            // Public ReadOnly Property MotivoScontoDettaglio As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_MotivoScontoDettaglio
            // End Get
            // End Property

            /// <summary>
            /// IDScontoAutorizzatoDa
            /// </summary>
            public DBCursorField<int> IDScontoAutorizzatoDa
            {
                get
                {
                    return m_IDScontoAutorizzatoDa;
                }
            }

            /// <summary>
            /// ScontoAutorizzatoIl
            /// </summary>
            public DBCursorField<DateTime> ScontoAutorizzatoIl
            {
                get
                {
                    return m_ScontoAutorizzatoIl;
                }
            }

            /// <summary>
            /// ScontoAutorizzatoNote
            /// </summary>
            public DBCursorStringField ScontoAutorizzatoNote
            {
                get
                {
                    return m_ScontoNomeMotivo;
                }
            }


            // ------------------------------------------------------
            // PRODOTTO
            // ------------------------------------------------------	

            /// <summary>
            /// NomeProdotto
            /// </summary>
            public DBCursorStringField NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }
            }

            /// <summary>
            /// IDProdotto
            /// </summary>
            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_ProdottoID;
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

            // Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NomeOperatore
            // End Get
            // End Property

            /// <summary>
            /// StatoPratica
            /// </summary>
            public DBCursorField<StatoPraticaEnum> StatoPratica
            {
                get
                {
                    return m_StatoPratica;
                }
            }

            /// <summary>
            /// IDStatoAttuale
            /// </summary>
            public DBCursorField<int> IDStatoAttuale
            {
                get
                {
                    return m_IDStatoAttuale;
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

            /// <summary>
            /// IDCommerciale
            /// </summary>
            public DBCursorField<int> IDCommerciale
            {
                get
                {
                    return m_CommercialeID;
                }
            }

            // Public ReadOnly Property NomeCommerciale As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NomeCommerciale
            // End Get
            // End Property

            // Public ReadOnly Property NomeProduttore As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_NomeProduttore
            // End Get
            // End Property

            /// <summary>
            /// IDAzienda
            /// </summary>
            public DBCursorField<int> IDAzienda
            {
                get
                {
                    return m_IDAzienda;
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
            /// StatoRichiestaApprovazione
            /// </summary>
            public DBCursorField<StatoRichiestaApprovazione> StatoRichiestaApprovazione
            {
                get
                {
                    return m_StatoRichiestaApprovazione;
                }
            }


            /// <summary>
            /// PassaggiDiStato
            /// </summary>
            public CCollection<CInfoStato> PassaggiDiStato
            {
                get
                {
                    if (m_PassaggiDiStato is null)
                    {
                        m_PassaggiDiStato = new CCollection<CInfoStato>();
                        var stati = new[] { StatoPraticaEnum.STATO_PREVENTIVO, StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO, StatoPraticaEnum.STATO_CONTRATTO_STAMPATO, StatoPraticaEnum.STATO_CONTRATTO_FIRMATO, StatoPraticaEnum.STATO_PRATICA_CARICATA, StatoPraticaEnum.STATO_RICHIESTADELIBERA, StatoPraticaEnum.STATO_DELIBERATA, StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE, StatoPraticaEnum.STATO_LIQUIDATA, StatoPraticaEnum.STATO_ARCHIVIATA, StatoPraticaEnum.STATO_ESTINTAANTICIPATAMENTE, StatoPraticaEnum.STATO_ANNULLATA };
                        foreach (StatoPraticaEnum stato in stati)
                        {
                            var info = new CInfoStato(stato);
                            m_PassaggiDiStato.Add(info);
                        }

                        m_PassaggiDiStato.Add(new CInfoStato());
                    }

                    return m_PassaggiDiStato;
                }
            }

            /// <summary>
            /// Restituisce l'oggetto specificato
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            protected CInfoStato GetStato(StatoPraticaEnum? value)
            {
                foreach (CInfoStato o in PassaggiDiStato)
                {
                    if (value.HasValue)
                    {
                        if (o.MacroStato.HasValue && o.MacroStato.Value == value.Value)
                            return o;
                    }
                    else if (o.MacroStato.HasValue == false)
                        return o;
                }

                return null;
            }

            /// <summary>
            /// StatoPreventivo
            /// </summary>
            public CInfoStato StatoPreventivo
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_PREVENTIVO);
                }
            }

            /// <summary>
            /// StatoPreventivoAccettato
            /// </summary>
            public CInfoStato StatoPreventivoAccettato
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO);
                }
            }

            /// <summary>
            /// StatoContrattoStampato
            /// </summary>
            public CInfoStato StatoContrattoStampato
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_CONTRATTO_STAMPATO);
                }
            }

            /// <summary>
            /// StatoContrattoFirmato
            /// </summary>
            public CInfoStato StatoContrattoFirmato
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_CONTRATTO_FIRMATO);
                }
            }

            /// <summary>
            /// StatoPraticaCaricata
            /// </summary>
            public CInfoStato StatoPraticaCaricata
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_PRATICA_CARICATA);
                }
            }

            /// <summary>
            /// StatoRichiestaDelibera
            /// </summary>
            public CInfoStato StatoRichiestaDelibera
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_RICHIESTADELIBERA);
                }
            }

            /// <summary>
            /// StatoDeliberata
            /// </summary>
            public CInfoStato StatoDeliberata
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_DELIBERATA);
                }
            }

            /// <summary>
            /// StatoProntaPerLiquidazione
            /// </summary>
            public CInfoStato StatoProntaPerLiquidazione
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE);
                }
            }

            /// <summary>
            /// StatoLiquidata
            /// </summary>
            public CInfoStato StatoLiquidata
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_LIQUIDATA);
                }
            }

            /// <summary>
            /// StatoArchiviata
            /// </summary>
            public CInfoStato StatoArchiviata
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_ARCHIVIATA);
                }
            }

            /// <summary>
            /// StatoEstintaAnticipatamente
            /// </summary>
            public CInfoStato StatoEstintaAnticipatamente
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_ESTINTAANTICIPATAMENTE);
                }
            }

            /// <summary>
            /// StatoAnnullata
            /// </summary>
            public CInfoStato StatoAnnullata
            {
                get
                {
                    return GetStato(StatoPraticaEnum.STATO_ANNULLATA);
                }
            }

            /// <summary>
            /// StatoGenerico
            /// </summary>
            public CInfoStato StatoGenerico
            {
                get
                {
                    return GetStato(default);
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Pratiche;
            }

            /// <summary>
            /// Query
            /// </summary>
            /// <returns></returns>
            public override DBSelect GetSQL()
            {
                DBSelect ret;
                var wherePart = GetWherePart();
                var items = PassaggiDiStato;
                bool isSet = false;
                foreach (CInfoStato s in items)
                {
                    if (s.IsSet())
                    {
                        isSet = true;
                        break;
                    }
                }

                if (isSet)
                {
                    ret = "";
                    int cnt = 0;
                    for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                    {
                        var item = items[i];
                        var tmp = "";
                        if (item.IsSet())
                        {
                            tmp += "(SELECT [IDPratica] FROM [tbl_PraticheSTL] WHERE [tbl_PraticheSTL].[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND " + item.GetSQL() + " GROUP BY [IDPratica]) AS [T" + i + "] ";
                            if (cnt == 0)
                            {
                                ret = "SELECT [tbl_Pratiche].* FROM [tbl_Pratiche] INNER JOIN " + tmp;
                                ret += " ON [tbl_Pratiche].[ID]=[T" + i + "].[IDPratica]";
                            }
                            else
                            {
                                ret = "SELECT [A" + i + "].* FROM (" + ret + ") As [A" + i + "] INNER JOIN " + tmp;
                                ret += " ON [A" + i + "].[ID]=[T" + i + "].[IDPratica]";
                            }

                            cnt += 1;
                        }
                    }

                    // If (Me.m_IDScontoAutorizzatoDa.IsSet) Then
                    // ret = "SELECT * FROM (SELECT [B].*, [tbl_PraticheInfo].[IDScontoAutorizzatoDa] FROM (" & ret & ") AS [B] INNER JOIN [tbl_PraticheInfo] ON [B].[ID] = [tbl_PraticheInfo].[IDPratica])"
                    // wherePart = Strings.Combine(wherePart, "[IDScontoAutorizzatoDa]=" & DBUtils.DBNumber(Me.m_IDScontoAutorizzatoDa.Value), " AND ")
                    // End If
                    if (!string.IsNullOrEmpty(wherePart))
                        ret = ret + " WHERE " + wherePart;
                }
                else
                {
                    // If (Me.m_IDScontoAutorizzatoDa.IsSet) Then
                    // ret = "SELECT * FROM (SELECT [tbl_Pratiche].*, [tbl_PraticheInfo].[IDScontoAutorizzatoDa] FROM [tbl_Pratiche] INNER JOIN [tbl_PraticheInfo] ON [tbl_Pratiche].[ID] = [tbl_PraticheInfo].[IDPratica]) WHERE "
                    // wherePart = Strings.Combine(wherePart, "[IDScontoAutorizzatoDa]=" & DBUtils.DBNumber(Me.m_IDScontoAutorizzatoDa.Value), " AND ")
                    // ret &= wherePart
                    // Else
                    ret = base.GetSQL();
                    // End If
                }

                if (m_StatoRichiestaApprovazione.IsSet() || m_IDScontoRichiestoDa.IsSet() || m_IDScontoAutorizzatoDa.IsSet() || m_ScontoAutorizzatoIl.IsSet() || m_ScontoNomeMotivo.IsSet())
                {
                    string where1 = "";
                    where1 = " [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [TipoOggettoApprovabile]='CPraticaCQSPD' ";
                    if (m_StatoRichiestaApprovazione.IsSet())
                        where1 = DMD.Strings.Combine(where1, m_StatoRichiestaApprovazione.GetSQL("StatoRichiesta"), " AND ");
                    if (m_IDScontoAutorizzatoDa.IsSet())
                        where1 = DMD.Strings.Combine(where1, m_IDScontoAutorizzatoDa.GetSQL("IDConfermataDa"), " AND ");
                    if (m_ScontoAutorizzatoIl.IsSet())
                        where1 = DMD.Strings.Combine(where1, m_ScontoAutorizzatoIl.GetSQL("DataConferma"), " AND ");
                    if (m_ScontoNomeMotivo.IsSet())
                        where1 = DMD.Strings.Combine(where1, m_ScontoNomeMotivo.GetSQL("NomeMotivoRichiesta"), " AND ");
                    if (m_IDScontoRichiestoDa.IsSet())
                        where1 = DMD.Strings.Combine(where1, m_IDScontoRichiestoDa.GetSQL("IDUtenteRichiestaApprovazione"), " AND ");
                    ret = "SELECT [TRA1].* FROM (" + ret + ") AS [TRA1] INNER JOIN (SELECT * FROM [tbl_CQSPDRichiesteApprovazione] WHERE ";
                    ret += where1;
                    ret += ") AS [TRA2] ON [TRA1].[ID]=[TRA2].[IDOggettoApprovabile]";
                }

                return ret;
            }

            /// <summary>
            /// Parte where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var wherePart = base.GetWherePart();

                if (!IgnoreRights)
                {
                    if (!Module.UserCanDoAction("seeliqui"))
                    {
                        wherePart *= (Field("StatoPratica").IsNull() + Field("StatoPratica").LT((int)StatoPraticaEnum.STATO_LIQUIDATA));
                    }

                    if (!Module.UserCanDoAction("seearch"))
                    {
                        wherePart *= (Field("StatoPratica").IsNull() + Field("StatoPratica").LT((int)StatoPraticaEnum.STATO_ARCHIVIATA));
                    }
                }

                if (!string.IsNullOrEmpty(this.m_RinnovaDa))
                {
                    var estinzioni = Finanziaria.Estinzioni.DISTINCT(DBProjection.Field("IDPratica"))
                                            .WHERE(DBCursorField.Field("NomeIstituto").EQ(m_RinnovaDa) * DBCursorField.Field("Stato").EQ((int)ObjectStatus.OBJECT_VALID));
                    wherePart *= Field("ID").In(estinzioni);
                }

                if (m_Nominativo.IsSet())
                {
                    m_Nominativo.Value = DMD.Strings.Replace(m_Nominativo.Value, "  ", " ");
                    wherePart *= (Field("CognomeCliente").Concat(" ").Concat(Field("NomeCliente"))).IsLike(m_Nominativo.Value) +
                                 (Field("NomeCliente").Concat(" ").Concat(Field("CognomeCliente"))).IsLike(m_Nominativo.Value);
                }

                if (m_CategoriaProdotto.IsSet())
                {
                    using (var cursor = new CProdottiCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        cursor.Categoria.InitFrom(m_CategoriaProdotto);
                        var list = new List<int>(2048);
                        while (cursor.Read())
                        {
                            list.Add(DBUtils.GetID(cursor.Item, 0));
                        }

                        wherePart *= Field("Prodotto").In(list.ToArray());
                    }
                         
                }

                return wherePart;
            }

            /// <summary>
            /// Sicurezza
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                DBCursorFieldBase tmpSQL = DBCursorField.False;
                if (!Module.UserCanDoAction("list"))
                {
                    if (Module.UserCanDoAction("list_office"))
                    {
                        var uffici = Sistema.Users.CurrentUser.Uffici;
                        var lst = new List<int?>(uffici.Count + 2);
                        lst.Add(0);
                        lst.Add(default);
                        foreach(var u in uffici) 
                        {
                            lst.Add(DBUtils.GetID(u, 0));
                        }
                        tmpSQL = Field("IDPuntoOperativo").In(lst.ToArray());
                    }

                    if (Module.UserCanDoAction("list_own"))
                    {
                        tmpSQL += Field("CreatoDa").EQ(DBUtils.GetID(Sistema.Users.CurrentUser, 0));
                    }

                    if (Module.UserCanDoAction("list_assigned"))
                    {
                        var col = Finanziaria.Collaboratori.GetItemByUser(Sistema.Users.CurrentUser);
                        if (col is object)
                        {
                            tmpSQL += Field("IDCollaboratore").EQ(DBUtils.GetID(col, 0));
                        }
                    }

                     
                }

                return tmpSQL;
            }
            
                 

            //public override void InitFrom(Databases.DBObjectCursorBase cursor)
            //{
            //    {
            //        var withBlock = (CPraticheCQSPDCursor)cursor;
            //        foreach (var tgt in PassaggiDiStato)
            //        {
            //            var src = withBlock.GetStato(tgt.MacroStato);
            //            tgt.InitializeFrom(src);
            //        }

            //        m_RinnovaDa = withBlock.m_RinnovaDa;
            //    }

            //    base.InitFrom(cursor);
            //}

            /// <summary>
            /// Sincronizzazione deli elementi
            /// </summary>
            public void SyncInfo()
            {
                Array arr = (Array)GetItemsArray();
                var buffer = new ArrayList();
                for (int i = 0, loopTo = DMD.Arrays.Len(arr) - 1; i <= loopTo; i++)
                {
                    CPraticaCQSPD r = (CPraticaCQSPD)arr.GetValue(i);
                    if (r is object)
                    {
                        buffer.Add(DBUtils.GetID(r));
                    }
                }

                if (buffer.Count > 0)
                {
                    // Dim cursor As New CInfoPraticaCursor
                    // Dim arrp() As Integer = buffer.ToArray(GetType(Integer))
                    // cursor.IDPratica.ValueIn(arrp)
                    // cursor.IgnoreRights = True
                    // While Not cursor.EOF
                    // Dim info As CInfoPratica = cursor.Item
                    // For i As Integer = 0 To Arrays.Len(arr) - 1
                    // Dim r As CPraticaCQSPD = arr.GetValue(i)
                    // If (GetID(r) = info.IDPratica) Then
                    // info.SetPratica(r)
                    // r.SetInfo(info)
                    // End If
                    // Next
                    // cursor.MoveNext()
                    // End While
                    // cursor.Dispose()
                }
            }

            /// <summary>
            /// Sincronizza gli stati di lavorazione
            /// </summary>
            public void SyncStatiLav()
            {
                Array arr = (Array)GetItemsArray();
                var buffer = new ArrayList();
                for (int i = 0, loopTo = DMD.Arrays.Len(arr) - 1; i <= loopTo; i++)
                {
                    CPraticaCQSPD r = (CPraticaCQSPD)arr.GetValue(i);
                    if (r is object)
                    {
                        buffer.Add(DBUtils.GetID(r));
                    }
                }

                if (buffer.Count > 0)
                {
                    var col = new CCollection<CStatoLavorazionePratica>();
                    using (var cursor = new CStatiLavorazionePraticaCursor())
                    {
                        int[] arrp = (int[])buffer.ToArray(typeof(int));
                        cursor.IDPratica.ValueIn(arrp);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            var stl = cursor.Item;
                            col.Add(stl);
                        }

                    }

                    for (int i = 0, loopTo1 = DMD.Arrays.Len(arr) - 1; i <= loopTo1; i++)
                    {
                        CPraticaCQSPD r = (CPraticaCQSPD)arr.GetValue(i);
                        if (r is object)
                        {
                            var statilav = new CStatiLavorazionePraticaCollection();
                            statilav.SetPratica(r);
                            foreach (var stl in col)
                            {
                                if (stl.IDPratica == DBUtils.GetID(r, 0))
                                {
                                    statilav.Add(stl);
                                }
                            }

                            statilav.Sort();
                            r.SetStatiDiLavorazione(statilav);
                            var sta = statilav.GetItemById(r.IDStatoDiLavorazioneAttuale);
                            if (sta is object)
                                r.SetStatoDiLavorazioneAttuale(sta);
                        }
                    }
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("RinnovaDa", m_RinnovaDa);
                base.XMLSerialize(writer);
                writer.WriteTag("PassaggiDiStato", PassaggiDiStato);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "PassaggiDiStato":
                        {
                            m_PassaggiDiStato = new CCollection<CInfoStato>();
                            m_PassaggiDiStato.AddRange((CCollection)fieldValue);
                            break;
                        }

                    case "RinnovaDa":
                        {
                            m_RinnovaDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce i campi del cursore
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldsCollection GetCursorFields()
            {
                var ret = base.GetCursorFields();
             
                ret.Remove(m_IDScontoAutorizzatoDa);
                ret.Remove(m_Nominativo);
                ret.Remove(m_StatoRichiestaApprovazione);
                ret.Remove(m_ScontoAutorizzatoIl);
                ret.Remove(m_ScontoNomeMotivo);
                ret.Remove(m_IDScontoRichiestoDa);
                ret.Remove(m_CategoriaProdotto);
                // ret.RemoveByKey(Me.m_TipoFonteCliente)
                // ret.RemoveByKey(Me.m_IDFonteCliente)
                return ret;
            }

            protected override string GetSortPart(DBCursorField field)
            {
                switch (field.FieldName ?? "")
                {
                    case "Nominativo":
                        {
                            switch (field.SortOrder)
                            {
                                case SortEnum.SORT_ASC:
                                    {
                                        return "[CognomeCliente] & ' ' & [NomeCliente] ASC";
                                    }

                                case SortEnum.SORT_DESC:
                                    {
                                        return "[CognomeCliente] & ' ' & [NomeCliente] DESC";
                                    }
                            }

                            break;
                        }

                    case "StatoPratica":
                        {
                            switch (field.SortOrder)
                            {
                                case SortEnum.SORT_ASC:
                                    {
                                        return "[Ordine] ASC";
                                    }

                                case SortEnum.SORT_DESC:
                                    {
                                        return "[Ordine] DESC";
                                    }
                            }

                            break;
                        }

                    case var @case when @case == (m_StatoRichiestaApprovazione.FieldName ?? ""):
                    case var case1 when case1 == (m_ScontoAutorizzatoIl.FieldName ?? ""):
                    case var case2 when case2 == (m_ScontoNomeMotivo.FieldName ?? ""):
                    case var case3 when case3 == (m_IDScontoRichiestoDa.FieldName ?? ""):
                    case var case4 when case4 == (m_IDScontoAutorizzatoDa.FieldName ?? ""): // , _
                        {
                            // Me.m_TipoFonteCliente.FieldName, _
                            // Me.m_IDFonteCliente.FieldName

                            return "";
                        }

                    default:
                        {
                            break;
                        }
                }

                return base.GetSortPart(field);
            }

            /// <summary>
            /// Sincronizza la pagina corrente
            /// </summary>
            protected override void SyncPage()
            {
                base.SyncPage();
                SyncInfo();
                SyncStatiLav();
            }
        }
    }
}