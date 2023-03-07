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
        /// Cursore sugli oggetti di tipo <see cref="COffertaCQS"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCQSPDOfferteCursor 
            : minidom.Databases.DBObjectCursorPO<COffertaCQS>
        {
            private DBCursorField<bool> m_OffertaLibera = new DBCursorField<bool>("OffertaLibera");
            private DBCursorField<int> m_IDPratica = new DBCursorField<int>("IDPratica");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<StatoOfferta> m_StatoOfferta = new DBCursorField<StatoOfferta>("StatoOfferta");
            private DBCursorField<int> m_PreventivoID = new DBCursorField<int>("Preventivo");
            private DBCursorField<int> m_IDCessionario = new DBCursorField<int>("IDCessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorField<int> m_IDProfilo = new DBCursorField<int>("IDProfilo");
            private DBCursorStringField m_NomeProfilo = new DBCursorStringField("NomeProfilo");
            private DBCursorField<int> m_ProdottoID = new DBCursorField<int>("Prodotto");
            private DBCursorStringField m_NomeProdotto = new DBCursorStringField("NomeProdotto");
            private DBCursorStringField m_CategoriaProdotto = new DBCursorStringField("CategoriaProdotto");
            private DBCursorField<bool> m_Calcolato = new DBCursorField<bool>("Calcolato");
            private DBCursorField<int> m_Durata = new DBCursorField<int>("Durata");
            private DBCursorField<decimal> m_Rata = new DBCursorField<decimal>("Rata");
            private DBCursorField<double> m_Eta = new DBCursorField<double>("Eta");
            private DBCursorField<double> m_Anzianita = new DBCursorField<double>("Anzianita");
            private DBCursorField<double> m_Rappel = new DBCursorField<double>("Rappel");
            private DBCursorField<int> m_TabellaAssicurativaRelID = new DBCursorField<int>("TabellaAssicurativaRel");
            private DBCursorStringField m_NomeTabellaAssicurativa = new DBCursorStringField("NomeTabellaAssicurativa");
            private DBCursorField<int> m_TabellaFinanziariaRelID = new DBCursorField<int>("TabellaFinanziariaRel");
            private DBCursorStringField m_NomeTabellaFinanziaria = new DBCursorStringField("NomeTabellaFinanziaria");
            private DBCursorField<int> m_TabellaSpeseID = new DBCursorField<int>("TabellaSpese");
            private DBCursorField<double> m_ProvvigioneMassima = new DBCursorField<double>("MaxProvv");
            private DBCursorField<double> m_SpreadBase = new DBCursorField<double>("SpreadBase");
            private DBCursorField<double> m_Spread = new DBCursorField<double>("Spread");
            private DBCursorField<double> m_Provvigioni = new DBCursorField<double>("Provvigioni");
            private DBCursorField<double> m_UpFront = new DBCursorField<double>("UpFront");
            private DBCursorField<double> m_Running = new DBCursorField<double>("Running");
            private DBCursorField<decimal> m_PremioVita = new DBCursorField<decimal>("PremioVita");
            private DBCursorField<decimal> m_PremioImpiego = new DBCursorField<decimal>("PremioImpiego");
            private DBCursorField<decimal> m_PremioCredito = new DBCursorField<decimal>("PremioCredito");
            private DBCursorField<DateTime> m_DataNascita = new DBCursorField<DateTime>("DataNascita");
            private DBCursorField<DateTime> m_DataAssunzione = new DBCursorField<DateTime>("DataAssunzione");
            private DBCursorField<decimal> m_ImpostaSostitutiva = new DBCursorField<decimal>("ImpostaSostitutiva");
            private DBCursorField<decimal> m_OneriErariali = new DBCursorField<decimal>("OneriErariali");
            private DBCursorField<decimal> m_NettoRicavo = new DBCursorField<decimal>("NettoRicavo");
            private DBCursorField<decimal> m_CommissioniBancarie = new DBCursorField<decimal>("CommissioniBancarie");
            private DBCursorField<decimal> m_Interessi = new DBCursorField<decimal>("Interessi");
            private DBCursorField<decimal> m_Imposte = new DBCursorField<decimal>("Imposte");
            private DBCursorField<decimal> m_SpeseConvenzioni = new DBCursorField<decimal>("SpeseConvenzioni");
            private DBCursorField<decimal> m_AltreSpese = new DBCursorField<decimal>("AltreSpese");
            private DBCursorField<decimal> m_Rivalsa = new DBCursorField<decimal>("Rivalsa");
            private DBCursorField<double> m_TEG = new DBCursorField<double>("TEG");
            private DBCursorField<double> m_TEG_Max = new DBCursorField<double>("TEG_Max");
            private DBCursorField<double> m_TAEG = new DBCursorField<double>("TAEG");
            private DBCursorField<double> m_TAEG_Max = new DBCursorField<double>("TAEG_Max");
            private DBCursorField<double> m_TAN = new DBCursorField<double>("TAN");
            private DBCursorField<DateTime> m_DataDecorrenza = new DBCursorField<DateTime>("DataDecorrenza");
            private DBCursorStringField m_Sesso = new DBCursorStringField("Sesso");
            private DBCursorField<bool> m_CaricaAlMassimo = new DBCursorField<bool>("CaricaAlMassimo");
            private DBCursorField<TEGCalcFlag> m_TipoCalcoloTEG = new DBCursorField<TEGCalcFlag>("TipoCalcoloTEG");
            private DBCursorField<TEGCalcFlag> m_TipoCalcoloTAEG = new DBCursorField<TEGCalcFlag>("TipoCalcoloTAEG");
            private DBCursorField<ErrorCodes> m_ErrorCode = new DBCursorField<ErrorCodes>("ErrorCode");
            private DBCursorStringField m_Messages = new DBCursorStringField("Messages");
            private DBCursorField<OffertaFlags> m_Flags = new DBCursorField<OffertaFlags>("Flags");
            private DBCursorField<decimal> m_ValoreRiduzioneProvvigionale = new DBCursorField<decimal>("ValoreRiduzioneProvv");
            private DBCursorField<decimal> m_CapitaleFinanziato = new DBCursorField<decimal>("CapitaleFinanziato");
            private DBCursorField<decimal> m_ValoreProvvigioneCollaboratore = new DBCursorField<decimal>("ProvvCollab");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private DBCursorField<int> m_IDClienteXCollaboratore = new DBCursorField<int>("IDClienteXCollaboratore");
            private DBCursorField<DateTime> m_DataCaricamento = new DBCursorField<DateTime>("DataCaricamento");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCQSPDOfferteCursor()
            {
            }

            /// <summary>
            /// DataCaricamento
            /// </summary>
            public DBCursorField<DateTime> DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }
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
            /// IDClienteXCollaboratore
            /// </summary>
            public DBCursorField<int> IDClienteXCollaboratore
            {
                get
                {
                    return m_IDClienteXCollaboratore;
                }
            }

            /// <summary>
            /// ValoreProvvigioneCollaboratore
            /// </summary>
            public DBCursorField<decimal> ValoreProvvigioneCollaboratore
            {
                get
                {
                    return (DBCursorField<decimal>)m_ValoreRiduzioneProvvigionale;
                }
            }

            /// <summary>
            /// CapitaleFinanziato
            /// </summary>
            public DBCursorField<decimal> CapitaleFinanziato
            {
                get
                {
                    return m_CapitaleFinanziato;
                }
            }

            /// <summary>
            /// ValoreRiduzioneProvvigionale
            /// </summary>
            public DBCursorField<decimal> ValoreRiduzioneProvvigionale
            {
                get
                {
                    return (DBCursorField<decimal>)m_ValoreRiduzioneProvvigionale;
                }
            }


            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<OffertaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// OffertaLibera
            /// </summary>
            public DBCursorField<bool> OffertaLibera
            {
                get
                {
                    return m_OffertaLibera;
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
            /// StatoOfferta
            /// </summary>
            public DBCursorField<StatoOfferta> StatoOfferta
            {
                get
                {
                    return m_StatoOfferta;
                }
            }

            /// <summary>
            /// PreventivoID
            /// </summary>
            public DBCursorField<int> PreventivoID
            {
                get
                {
                    return m_PreventivoID;
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
            /// ProdottoID
            /// </summary>
            public DBCursorField<int> ProdottoID
            {
                get
                {
                    return m_ProdottoID;
                }
            }

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
            /// Calcolato
            /// </summary>
            public DBCursorField<bool> Calcolato
            {
                get
                {
                    return m_Calcolato;
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
            /// Eta
            /// </summary>
            public DBCursorField<double> Eta
            {
                get
                {
                    return m_Eta;
                }
            }

            /// <summary>
            /// Anzianita
            /// </summary>
            public DBCursorField<double> Anzianita
            {
                get
                {
                    return m_Anzianita;
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
            /// TabellaAssicurativaRelID
            /// </summary>
            public DBCursorField<int> TabellaAssicurativaRelID
            {
                get
                {
                    return m_TabellaAssicurativaRelID;
                }
            }

            /// <summary>
            /// NomeTabellaAssicurativa
            /// </summary>
            public DBCursorStringField NomeTabellaAssicurativa
            {
                get
                {
                    return m_NomeTabellaAssicurativa;
                }
            }

            /// <summary>
            /// TabellaFinanziariaRelID
            /// </summary>
            public DBCursorField<int> TabellaFinanziariaRelID
            {
                get
                {
                    return m_TabellaFinanziariaRelID;
                }
            }

            /// <summary>
            /// NomeTabellaFinanziaria
            /// </summary>
            public DBCursorStringField NomeTabellaFinanziaria
            {
                get
                {
                    return m_NomeTabellaFinanziaria;
                }
            }

            /// <summary>
            /// TabellaSpeseID
            /// </summary>
            public DBCursorField<int> TabellaSpeseID
            {
                get
                {
                    return m_TabellaSpeseID;
                }
            }

            /// <summary>
            /// ProvvigioneMassima
            /// </summary>
            public DBCursorField<double> ProvvigioneMassima
            {
                get
                {
                    return m_ProvvigioneMassima;
                }
            }

            /// <summary>
            /// SpreadBase
            /// </summary>
            public DBCursorField<double> SpreadBase
            {
                get
                {
                    return m_SpreadBase;
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

            /// <summary>
            /// Provvigioni
            /// </summary>
            public DBCursorField<double> Provvigioni
            {
                get
                {
                    return m_Provvigioni;
                }
            }

            /// <summary>
            /// UpFront
            /// </summary>
            public DBCursorField<double> UpFront
            {
                get
                {
                    return m_UpFront;
                }
            }

            /// <summary>
            /// Running
            /// </summary>
            public DBCursorField<double> Running
            {
                get
                {
                    return m_Running;
                }
            }

            /// <summary>
            /// PremioVita
            /// </summary>
            public DBCursorField<decimal> PremioVita
            {
                get
                {
                    return m_PremioVita;
                }
            }

            /// <summary>
            /// PremioImpiego
            /// </summary>
            public DBCursorField<decimal> PremioImpiego
            {
                get
                {
                    return m_PremioImpiego;
                }
            }

            /// <summary>
            /// PremioCredito
            /// </summary>
            public DBCursorField<decimal> PremioCredito
            {
                get
                {
                    return m_PremioCredito;
                }
            }

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
            /// ImpostaSostitutiva
            /// </summary>
            public DBCursorField<decimal> ImpostaSostitutiva
            {
                get
                {
                    return m_ImpostaSostitutiva;
                }
            }

            /// <summary>
            /// OneriErariali
            /// </summary>
            public DBCursorField<decimal> OneriErariali
            {
                get
                {
                    return m_OneriErariali;
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
            /// CommissioniBancarie
            /// </summary>
            public DBCursorField<decimal> CommissioniBancarie
            {
                get
                {
                    return m_CommissioniBancarie;
                }
            }

            /// <summary>
            /// Interessi
            /// </summary>
            public DBCursorField<decimal> Interessi
            {
                get
                {
                    return m_Interessi;
                }
            }

            /// <summary>
            /// Imposte
            /// </summary>
            public DBCursorField<decimal> Imposte
            {
                get
                {
                    return m_Imposte;
                }
            }

            /// <summary>
            /// SpeseConvenzioni
            /// </summary>
            public DBCursorField<decimal> SpeseConvenzioni
            {
                get
                {
                    return m_SpeseConvenzioni;
                }
            }

            /// <summary>
            /// AltreSpese
            /// </summary>
            public DBCursorField<decimal> AltreSpese
            {
                get
                {
                    return m_AltreSpese;
                }
            }

            /// <summary>
            /// Rivalsa
            /// </summary>
            public DBCursorField<decimal> Rivalsa
            {
                get
                {
                    return m_Rivalsa;
                }
            }

            /// <summary>
            /// TEG
            /// </summary>
            public DBCursorField<double> TEG
            {
                get
                {
                    return m_TEG;
                }
            }

            /// <summary>
            /// TEG_Max
            /// </summary>
            public DBCursorField<double> TEG_Max
            {
                get
                {
                    return m_TEG_Max;
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
            /// TAEG_Max
            /// </summary>
            public DBCursorField<double> TAEG_Max
            {
                get
                {
                    return m_TAEG_Max;
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
            /// TipoCalcoloTEG
            /// </summary>
            public DBCursorField<TEGCalcFlag> TipoCalcoloTEG
            {
                get
                {
                    return m_TipoCalcoloTEG;
                }
            }

            /// <summary>
            /// TipoCalcoloTAEG
            /// </summary>
            public DBCursorField<TEGCalcFlag> TipoCalcoloTAEG
            {
                get
                {
                    return m_TipoCalcoloTAEG;
                }
            }

            /// <summary>
            /// ErrorCode
            /// </summary>
            public DBCursorField<ErrorCodes> ErrorCode
            {
                get
                {
                    return m_ErrorCode;
                }
            }

            /// <summary>
            /// Messages
            /// </summary>
            public DBCursorStringField Messages
            {
                get
                {
                    return m_Messages;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override COffertaCQS InstantiateNewT(DBReader dbRis)
            {
                return new COffertaCQS();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Preventivi_Offerte";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Offerte;
            }

            protected override CKeyCollection<DBCursorField> GetWhereFields()
            {
                var ret = new CKeyCollection<DBCursorField>(base.GetWhereFields());
                ret.Remove(m_CategoriaProdotto);
                return ret;
            }

            protected override CKeyCollection<DBCursorField> GetSortFields()
            {
                var ret = new CKeyCollection<DBCursorField>(base.GetWhereFields());
                ret.Remove(m_CategoriaProdotto);
                return ret;
            }

            /// <summary>
            /// Clausola where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var ret = base.GetWherePart();

                if (m_CategoriaProdotto.IsSet())
                {
                    var arr = DMD.Arrays.Empty<int>();
                    foreach (var p in Prodotti.LoadAll())
                    {
                        if (p.Stato == ObjectStatus.OBJECT_VALID && (p.Categoria ?? "") == (m_CategoriaProdotto.Value ?? ""))
                        {
                            int argitem = DBUtils.GetID(p);
                            arr = DMD.Arrays.Append<int>(arr, argitem);
                        }
                    }


                    ret *= Field("Prodotto").IsNull() + Field("Prodotto").In(ret.ToString());
                }

                return ret;
            }
        }
    }
}