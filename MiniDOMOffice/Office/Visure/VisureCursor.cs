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
        /// Cursore di <see cref="Visura"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class VisureCursor 
            : minidom.Databases.DBObjectCursorPO<Visura>
        {
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<int> m_IDRichiedente = new DBCursorField<int>("IDRichiedente");
            private DBCursorStringField m_NomeRichiedente = new DBCursorStringField("NomeRichiedente");
            private DBCursorField<int> m_IDPresaInCaricoDa = new DBCursorField<int>("IDPresaInCaricoDa");
            private DBCursorStringField m_NomePresaInCaricoDa = new DBCursorStringField("NomePresaInCaricoDa");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<DateTime> m_DataCompletamento = new DBCursorField<DateTime>("DataCompletamento");
            private DBCursorField<StatoVisura> m_StatoVisura = new DBCursorField<StatoVisura>("StatoVisura");
            private DBCursorField<bool> m_ValutazioneAmministrazione = new DBCursorField<bool>("VALAMM");
            private DBCursorField<bool> m_CensimentoDatoreDiLavoro = new DBCursorField<bool>("CENSDATLAV");
            private DBCursorField<bool> m_CensimentoSedeOperativa = new DBCursorField<bool>("CENSSEDOP");
            private DBCursorField<bool> m_VariazioneDenominazione = new DBCursorField<bool>("VARIAZDENOM");
            private DBCursorField<bool> m_Sblocco = new DBCursorField<bool>("SBLOCCO");
            private DBCursorField<int> m_IDAmministrazione = new DBCursorField<int>("IDAmministrazione");
            private DBCursorStringField m_RagioneSociale = new DBCursorStringField("RagioneSociale");
            private DBCursorStringField m_OggettoSociale = new DBCursorStringField("OggettoSociale");
            private DBCursorStringField m_CodiceFiscale = new DBCursorStringField("CodiceFiscale");
            private DBCursorStringField m_PartitaIVA = new DBCursorStringField("PartitaIVA");
            private DBCursorStringField m_ResponsabileDaContattare = new DBCursorStringField("NomeResponsabile");
            private DBCursorStringField m_Qualifica = new DBCursorStringField("QualificaResponsabile");
            private DBCursorStringField m_IndirizzoProvincia = new DBCursorStringField("Indirizzo_Provincia");
            private DBCursorStringField m_IndirizzoCitta = new DBCursorStringField("Indirizzo_Citta");
            private DBCursorStringField m_IndirizzoCAP = new DBCursorStringField("Indirizzo_CAP");
            private DBCursorStringField m_IndirizzoToponimoViaECivico = new DBCursorStringField("Indirizzo_Via");
            private DBCursorStringField m_Telefono = new DBCursorStringField("Telefono");
            private DBCursorStringField m_Fax = new DBCursorStringField("Fax");
            private DBCursorStringField m_IndirizzoeMail = new DBCursorStringField("eMail");
            private DBCursorStringField m_IndirizzoDiNotificaProvincia = new DBCursorStringField("IndirizzoN_Provincia");
            private DBCursorStringField m_IndirizzoDiNotificaCitta = new DBCursorStringField("IndirizzoN_Citta");
            private DBCursorStringField m_IndirizzoDiNotificaCAP = new DBCursorStringField("IndirizzoN_CAP");
            private DBCursorStringField m_IndirizzoDiNotificaToponimoViaECivico = new DBCursorStringField("IndirizzoN_Via");
            private DBCursorStringField m_TelefonoDiNotifica = new DBCursorStringField("TelefonoN");
            private DBCursorStringField m_FaxDiNotifica = new DBCursorStringField("FaxN");
            private DBCursorField<bool> m_ConvenzionePresente = new DBCursorField<bool>("CONVSINO");
            private DBCursorStringField m_CodiceODescrizioneConvenzione = new DBCursorStringField("CODCONV");
            private DBCursorField<int> m_NumeroDipendenti = new DBCursorField<int>("NumeroDipendenti");
            private DBCursorField<bool> m_AmministrazioneSottoscriveMODPREST_008 = new DBCursorField<bool>("AMMMODPRST008");
            private DBCursorStringField m_NoteOInfoSullaSocieta = new DBCursorStringField("NoteSocieta");
            private DBCursorField<int> m_IDBustaPaga = new DBCursorField<int>("IDBustaPaga");
            private DBCursorField<int> m_IDMotivoRichiestaSblocco = new DBCursorField<int>("IDMotivoSblocco");
            private DBCursorStringField m_CodiceAmministrazioneCL = new DBCursorStringField("CODAMMCL");
            private DBCursorStringField m_StatoAmministrazioneCL = new DBCursorStringField("STATOAMMCL");

            /// <summary>
            /// Costruttore
            /// </summary>
            public VisureCursor()
            {
            }

            /// <summary>
            /// CodiceAmministrazioneCL
            /// </summary>
            public DBCursorStringField CodiceAmministrazioneCL
            {
                get
                {
                    return m_CodiceAmministrazioneCL;
                }
            }

            /// <summary>
            /// StatoAmministrazioneCL
            /// </summary>
            public DBCursorStringField StatoAmministrazioneCL
            {
                get
                {
                    return m_StatoAmministrazioneCL;
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
            /// IDRichiedente
            /// </summary>
            public DBCursorField<int> IDRichiedente
            {
                get
                {
                    return m_IDRichiedente;
                }
            }

            /// <summary>
            /// NomeRichiedente
            /// </summary>
            public DBCursorStringField NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }
            }

            /// <summary>
            /// IDPresaInCaricoDa
            /// </summary>
            public DBCursorField<int> IDPresaInCaricoDa
            {
                get
                {
                    return m_IDPresaInCaricoDa;
                }
            }

            /// <summary>
            /// NomePresaInCaricoDa
            /// </summary>
            public DBCursorStringField NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
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
            /// DataCompletamento
            /// </summary>
            public DBCursorField<DateTime> DataCompletamento
            {
                get
                {
                    return m_DataCompletamento;
                }
            }

            /// <summary>
            /// StatoVisura
            /// </summary>
            public DBCursorField<StatoVisura> StatoVisura
            {
                get
                {
                    return m_StatoVisura;
                }
            }

            /// <summary>
            /// ValutazioneAmministrazione
            /// </summary>
            public DBCursorField<bool> ValutazioneAmministrazione
            {
                get
                {
                    return m_ValutazioneAmministrazione;
                }
            }

            /// <summary>
            /// CensimentoDatoreDiLavoro
            /// </summary>
            public DBCursorField<bool> CensimentoDatoreDiLavoro
            {
                get
                {
                    return m_CensimentoDatoreDiLavoro;
                }
            }

            /// <summary>
            /// CensimentoSedeOperativa
            /// </summary>
            public DBCursorField<bool> CensimentoSedeOperativa
            {
                get
                {
                    return m_CensimentoSedeOperativa;
                }
            }

            /// <summary>
            /// VariazioneDenominazione
            /// </summary>
            public DBCursorField<bool> VariazioneDenominazione
            {
                get
                {
                    return m_VariazioneDenominazione;
                }
            }

            /// <summary>
            /// Sblocco
            /// </summary>
            public DBCursorField<bool> Sblocco
            {
                get
                {
                    return m_Sblocco;
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

            /// <summary>
            /// RagioneSociale
            /// </summary>
            public DBCursorStringField RagioneSociale
            {
                get
                {
                    return m_RagioneSociale;
                }
            }

            /// <summary>
            /// OggettoSociale
            /// </summary>
            public DBCursorStringField OggettoSociale
            {
                get
                {
                    return m_OggettoSociale;
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

            /// <summary>
            /// ResponsabileDaContattare
            /// </summary>
            public DBCursorStringField ResponsabileDaContattare
            {
                get
                {
                    return m_ResponsabileDaContattare;
                }
            }

            /// <summary>
            /// Qualifica
            /// </summary>
            public DBCursorStringField Qualifica
            {
                get
                {
                    return m_Qualifica;
                }
            }

            /// <summary>
            /// IndirizzoProvincia
            /// </summary>
            public DBCursorStringField IndirizzoProvincia
            {
                get
                {
                    return m_IndirizzoProvincia;
                }
            }

            /// <summary>
            /// IndirizzoCitta
            /// </summary>
            public DBCursorStringField IndirizzoCitta
            {
                get
                {
                    return m_IndirizzoCitta;
                }
            }

            /// <summary>
            /// IndirizzoCAP
            /// </summary>
            public DBCursorStringField IndirizzoCAP
            {
                get
                {
                    return m_IndirizzoCAP;
                }
            }

            /// <summary>
            /// IndirizzoToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoToponimoViaECivico;
                }
            }

            /// <summary>
            /// Telefono
            /// </summary>
            public DBCursorStringField Telefono
            {
                get
                {
                    return m_Telefono;
                }
            }

            /// <summary>
            /// Fax
            /// </summary>
            public DBCursorStringField Fax
            {
                get
                {
                    return m_Fax;
                }
            }

            /// <summary>
            /// IndirizzoeMail
            /// </summary>
            public DBCursorStringField IndirizzoeMail
            {
                get
                {
                    return m_IndirizzoeMail;
                }
            }

            /// <summary>
            /// IndirizzoDiNotificaProvincia
            /// </summary>
            public DBCursorStringField IndirizzoDiNotificaProvincia
            {
                get
                {
                    return m_IndirizzoDiNotificaProvincia;
                }
            }

            /// <summary>
            /// IndirizzoDiNotificaCitta
            /// </summary>
            public DBCursorStringField IndirizzoDiNotificaCitta
            {
                get
                {
                    return m_IndirizzoDiNotificaCitta;
                }
            }

            /// <summary>
            /// IndirizzoDiNotificaCAP
            /// </summary>
            public DBCursorStringField IndirizzoDiNotificaCAP
            {
                get
                {
                    return m_IndirizzoDiNotificaCAP;
                }
            }

            /// <summary>
            /// IndirizzoDiNotificaToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoDiNotificaToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoDiNotificaToponimoViaECivico;
                }
            }

            /// <summary>
            /// TelefonoDiNotifica
            /// </summary>
            public DBCursorStringField TelefonoDiNotifica
            {
                get
                {
                    return m_TelefonoDiNotifica;
                }
            }

            /// <summary>
            /// FaxDiNotifica
            /// </summary>
            public DBCursorStringField FaxDiNotifica
            {
                get
                {
                    return m_FaxDiNotifica;
                }
            }

            /// <summary>
            /// ConvenzionePresente
            /// </summary>
            public DBCursorField<bool> ConvenzionePresente
            {
                get
                {
                    return m_ConvenzionePresente;
                }
            }

            /// <summary>
            /// CodiceODescrizioneConvenzione
            /// </summary>
            public DBCursorStringField CodiceODescrizioneConvenzione
            {
                get
                {
                    return m_CodiceODescrizioneConvenzione;
                }
            }

            /// <summary>
            /// NumeroDipendenti
            /// </summary>
            public DBCursorField<int> NumeroDipendenti
            {
                get
                {
                    return m_NumeroDipendenti;
                }
            }

            /// <summary>
            /// AmministrazioneSottoscriveMODPREST_008
            /// </summary>
            public DBCursorField<bool> AmministrazioneSottoscriveMODPREST_008
            {
                get
                {
                    return m_AmministrazioneSottoscriveMODPREST_008;
                }
            }

            /// <summary>
            /// NoteOInfoSullaSocieta
            /// </summary>
            public DBCursorStringField NoteOInfoSullaSocieta
            {
                get
                {
                    return m_NoteOInfoSullaSocieta;
                }
            }

            /// <summary>
            /// IDBustaPaga
            /// </summary>
            public DBCursorField<int> IDBustaPaga
            {
                get
                {
                    return m_IDBustaPaga;
                }
            }

            /// <summary>
            /// IDMotivoRichiestaSblocco
            /// </summary>
            public DBCursorField<int> IDMotivoRichiestaSblocco
            {
                get
                {
                    return m_IDMotivoRichiestaSblocco;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Visure;
            }

            /// <summary>
            /// Inizializza i parametri del nuovo elemento
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(Visura item)
            {
                base.OnInitialize(item);

                item.Data = DMD.DateUtils.Now();
                item.Richiedente = Sistema.Users.CurrentUser;
            }
             
        }
    }
}