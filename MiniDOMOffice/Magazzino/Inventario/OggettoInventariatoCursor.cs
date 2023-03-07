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
using static minidom.Store;

namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Cursore di <see cref="OggettoInventariato"/>
        /// </summary>

        [Serializable]
        public class OggettoInventariatoCursor
            : minidom.Databases.DBObjectCursorPO<OggettoInventariato>
        {
            private DBCursorField<int> m_IDArticolo = new DBCursorField<int>("IDArticolo");
            private DBCursorStringField m_Codice = new DBCursorStringField("Codice");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_NomeArticolo = new DBCursorStringField("NomeArticolo");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_Marca = new DBCursorStringField("Marca");
            private DBCursorStringField m_Modello = new DBCursorStringField("Modello");
            private DBCursorStringField m_Seriale = new DBCursorStringField("Seriale");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorField<StatoOggettoInventariato> m_StatoAttuale = new DBCursorField<StatoOggettoInventariato>("StatoAttuale");
            private DBCursorStringField m_DettaglioStatoAttuale = new DBCursorStringField("DettaglioStatoAttuale");
            private DBCursorField<decimal> m_ValoreStimato = new DBCursorField<decimal>("ValoreStimato");
            private DBCursorField<DateTime> m_DataValutazione = new DBCursorField<DateTime>("DataValutazione");
            private DBCursorStringField m_TipoInUsoDa = new DBCursorStringField("TipoInUsoDa");
            private DBCursorField<int> m_IDInUsoDa = new DBCursorField<int>("InUsoDaID");
            private DBCursorStringField m_NomeInUsoDa = new DBCursorStringField("NomeInUsoDa");
            private DBCursorField<DateTime> m_DataProduzione = new DBCursorField<DateTime>("DataProduzione");
            private DBCursorField<DateTime> m_DataAcquisto = new DBCursorField<DateTime>("DataAscquisto");
            private DBCursorStringField m_TipoDocumentoAcquisto = new DBCursorStringField("TipoDocumentoAcquisto");
            private DBCursorStringField m_NumeroDocumentoAcquisto = new DBCursorStringField("NumeroDocumentoAcquisto");
            private DBCursorField<StatoAcquistoOggettoInventariato> m_StatoAcquisto = new DBCursorField<StatoAcquistoOggettoInventariato>("StatoAcquisto");
            private DBCursorStringField m_DettaglioStatoAcquisto = new DBCursorStringField("DettaglioStatoAcquisto");
            private DBCursorField<int> m_AcquistatoDaID = new DBCursorField<int>("AcquistatoDaID");
            private DBCursorStringField m_NomeAcquistatoDa = new DBCursorStringField("NomeAcquistatoDa");
            private DBCursorField<decimal> m_PrezzoAcquisto = new DBCursorField<decimal>("PrezzoAcquisto");
            private DBCursorField<double> m_AliquotaIVA = new DBCursorField<double>("AliquotaIVA");
            private DBCursorField<int> m_IDUfficioOriginale = new DBCursorField<int>("IDUfficioOriginale");
            private DBCursorStringField m_NomeUfficioOriginale = new DBCursorStringField("NomeUfficioOriginale");
            private DBCursorStringField m_CodiceScaffale = new DBCursorStringField("CodiceScaffale");
            private DBCursorStringField m_CodiceReparto = new DBCursorStringField("CodiceReparto");
            private DBCursorField<DateTime> m_DataDismissione = new DBCursorField<DateTime>("DataDismissione");
            private DBCursorField<int> m_DismessoDaID = new DBCursorField<int>("DismessoDaID");
            private DBCursorStringField m_NomeDismessoDa = new DBCursorStringField("NomeDismessoDa");
            private DBCursorStringField m_MotivoDismissione = new DBCursorStringField("MotivoDismissione");
            private DBCursorStringField m_DettaglioDismissione = new DBCursorStringField("DettaglioDismissione");
            private DBCursorField<decimal> m_ValoreDismissione = new DBCursorField<decimal>("ValoreDismissione");
            private DBCursorField<double> m_AliquotaIVADismissione = new DBCursorField<double>("AliquotaIVADismissione");
            private DBCursorField<FlagsOggettoInventariato> m_Flags = new DBCursorField<FlagsOggettoInventariato>("Flags");
            private DBCursorField<int> m_IDOrdineAcquisto = new DBCursorField<int>("IDOrdineAcquisto");
            private DBCursorField<int> m_IDDocumentoAcquisto = new DBCursorField<int>("IDDocumentoAcquisto");
            private DBCursorField<int> m_IDSpedizione = new DBCursorField<int>("IDSpedizione");
            private DBCursorStringField m_CodiceRFID = new DBCursorStringField("CodiceRFID");

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettoInventariatoCursor()
            {
            }

            /// <summary>
            /// CodiceRFID
            /// </summary>
            public DBCursorStringField CodiceRFID
            {
                get
                {
                    return m_CodiceRFID;
                }
            }

            /// <summary>
            /// IDOrdineAcquisto
            /// </summary>
            public DBCursorField<int> IDOrdineAcquisto
            {
                get
                {
                    return m_IDOrdineAcquisto;
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
            /// IDSpedizione
            /// </summary>
            public DBCursorField<int> IDSpedizione
            {
                get
                {
                    return m_IDSpedizione;
                }
            }

            /// <summary>
            /// IDArticolo
            /// </summary>
            public DBCursorField<int> IDArticolo
            {
                get
                {
                    return m_IDArticolo;
                }
            }

            /// <summary>
            /// Codice
            /// </summary>
            public DBCursorStringField Codice
            {
                get
                {
                    return m_Codice;
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
            /// NomeArticolo
            /// </summary>
            public DBCursorStringField NomeArticolo
            {
                get
                {
                    return m_NomeArticolo;
                }
            }

            /// <summary>
            /// Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// Marca
            /// </summary>
            public DBCursorStringField Marca
            {
                get
                {
                    return m_Marca;
                }
            }

            /// <summary>
            /// Modello
            /// </summary>
            public DBCursorStringField Modello
            {
                get
                {
                    return m_Modello;
                }
            }

            /// <summary>
            /// Seriale
            /// </summary>
            public DBCursorStringField Seriale
            {
                get
                {
                    return m_Seriale;
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
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// StatoAttuale
            /// </summary>
            public DBCursorField<StatoOggettoInventariato> StatoAttuale
            {
                get
                {
                    return m_StatoAttuale;
                }
            }

            /// <summary>
            /// DettaglioStatoAttuale
            /// </summary>
            public DBCursorStringField DettaglioStatoAttuale
            {
                get
                {
                    return m_DettaglioStatoAttuale;
                }
            }

            /// <summary>
            /// ValoreStimato
            /// </summary>
            public DBCursorField<decimal> ValoreStimato
            {
                get
                {
                    return m_ValoreStimato;
                }
            }

            /// <summary>
            /// DataValutazione
            /// </summary>
            public DBCursorField<DateTime> DataValutazione
            {
                get
                {
                    return m_DataValutazione;
                }
            }

            /// <summary>
            /// TipoInUsoDa
            /// </summary>
            public DBCursorStringField TipoInUsoDa
            {
                get
                {
                    return m_TipoInUsoDa;
                }
            }

            /// <summary>
            /// IDInUsoDa
            /// </summary>
            public DBCursorField<int> IDInUsoDa
            {
                get
                {
                    return m_IDInUsoDa;
                }
            }

            /// <summary>
            /// NomeInUsoDa
            /// </summary>
            public DBCursorStringField NomeInUsoDa
            {
                get
                {
                    return m_NomeInUsoDa;
                }
            }

            /// <summary>
            /// DataProduzione
            /// </summary>
            public DBCursorField<DateTime> DataProduzione
            {
                get
                {
                    return m_DataProduzione;
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
            /// TipoDocumentoAcquisto
            /// </summary>
            public DBCursorStringField TipoDocumentoAcquisto
            {
                get
                {
                    return m_TipoDocumentoAcquisto;
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
            /// StatoAcquisto
            /// </summary>
            public DBCursorField<StatoAcquistoOggettoInventariato> StatoAcquisto
            {
                get
                {
                    return m_StatoAcquisto;
                }
            }

            /// <summary>
            /// DettaglioStatoAcquisto
            /// </summary>
            public DBCursorStringField DettaglioStatoAcquisto
            {
                get
                {
                    return m_DettaglioStatoAcquisto;
                }
            }

            /// <summary>
            /// AcquistatoDaID
            /// </summary>
            public DBCursorField<int> AcquistatoDaID
            {
                get
                {
                    return m_AcquistatoDaID;
                }
            }

            /// <summary>
            /// NomeAcquistatoDa
            /// </summary>
            public DBCursorStringField NomeAcquistatoDa
            {
                get
                {
                    return m_NomeAcquistatoDa;
                }
            }

            /// <summary>
            /// PrezzoAcquisto
            /// </summary>
            public DBCursorField<decimal> PrezzoAcquisto
            {
                get
                {
                    return m_PrezzoAcquisto;
                }
            }

            /// <summary>
            /// AliquotaIVA
            /// </summary>
            public DBCursorField<double> AliquotaIVA
            {
                get
                {
                    return m_AliquotaIVA;
                }
            }

            /// <summary>
            /// IDUfficioOriginale
            /// </summary>
            public DBCursorField<int> IDUfficioOriginale
            {
                get
                {
                    return m_IDUfficioOriginale;
                }
            }

            /// <summary>
            /// NomeUfficioOriginale
            /// </summary>
            public DBCursorStringField NomeUfficioOriginale
            {
                get
                {
                    return m_NomeUfficioOriginale;
                }
            }

            /// <summary>
            /// CodiceScaffale
            /// </summary>
            public DBCursorStringField CodiceScaffale
            {
                get
                {
                    return m_CodiceScaffale;
                }
            }

            /// <summary>
            /// CodiceReparto
            /// </summary>
            public DBCursorStringField CodiceReparto
            {
                get
                {
                    return m_CodiceReparto;
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
            /// DismessoDaID
            /// </summary>
            public DBCursorField<int> DismessoDaID
            {
                get
                {
                    return m_DismessoDaID;
                }
            }

            /// <summary>
            /// NomeDismessoDa
            /// </summary>
            public DBCursorStringField NomeDismessoDa
            {
                get
                {
                    return m_NomeDismessoDa;
                }
            }

            /// <summary>
            /// MotivoDismissione
            /// </summary>
            public DBCursorStringField MotivoDismissione
            {
                get
                {
                    return m_MotivoDismissione;
                }
            }

            /// <summary>
            /// DettaglioDismissione
            /// </summary>
            public DBCursorStringField DettaglioDismissione
            {
                get
                {
                    return m_DettaglioDismissione;
                }
            }

            /// <summary>
            /// ValoreDismissione
            /// </summary>
            public DBCursorField<decimal> ValoreDismissione
            {
                get
                {
                    return m_ValoreDismissione;
                }
            }

            /// <summary>
            /// AliquotaIVADismissione
            /// </summary>
            public DBCursorField<double> AliquotaIVADismissione
            {
                get
                {
                    return m_AliquotaIVADismissione;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<FlagsOggettoInventariato> Flags
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
                return minidom.Store.OggettiInventariati;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_OfficeOggettiInventariati";
            //}
        }
    }
}