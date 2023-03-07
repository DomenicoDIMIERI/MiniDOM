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
        /// Cursore sulla tabella degli oggetti da spedire
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class OggettiDaSpedireCursor
            : minidom.Databases.DBObjectCursorPO<OggettoDaSpedire>
        {
            private DBCursorStringField m_AspettoBeni = new DBCursorStringField("AspettoBeni");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<int> m_IDDestinatario = new DBCursorField<int>("IDDestinatario");
            private DBCursorStringField m_NomeDestinatario = new DBCursorStringField("NomeDestinatario");
            private DBCursorStringField m_INDMITT_CAP = new DBCursorStringField("INDMITT_CAP");
            private DBCursorStringField m_INDMITT_Citta = new DBCursorStringField("INDMITT_CITTA");
            private DBCursorStringField m_INDMITT_Provincia = new DBCursorStringField("INDMITT_PROV");
            private DBCursorStringField m_INDMITT_ToponimoViaECivico = new DBCursorStringField("INDMITT_VIA");
            private DBCursorStringField m_INDDEST_CAP = new DBCursorStringField("INDDEST_CAP");
            private DBCursorStringField m_INDDEST_Citta = new DBCursorStringField("INDDEST_CITTA");
            private DBCursorStringField m_INDDEST_Provincia = new DBCursorStringField("INDDEST_PROV");
            private DBCursorStringField m_INDDEST_ToponimoViaECivico = new DBCursorStringField("INDDEST_VIA");
            private DBCursorField<int> m_NumeroColli = new DBCursorField<int>("NumeroColli");
            private DBCursorField<double> m_Peso = new DBCursorField<double>("Peso");
            private DBCursorField<double> m_Altezza = new DBCursorField<double>("Altezza");
            private DBCursorField<double> m_Larghezza = new DBCursorField<double>("Larghezza");
            private DBCursorField<double> m_Profondita = new DBCursorField<double>("Profondita");
            private DBCursorField<int> m_IDRichiestaDa = new DBCursorField<int>("IDRichiestaDa");
            private DBCursorStringField m_NomeRichiestaDa = new DBCursorStringField("NomeRichiestaDa");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<int> m_IDPresaInCaricoDa = new DBCursorField<int>("IDPresaInCaricoDa");
            private DBCursorStringField m_NomePresaInCaricoDa = new DBCursorStringField("NomePresaInCaricoDa");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<int> m_IDConfermatoDa = new DBCursorField<int>("IDConfermatoDa");
            private DBCursorStringField m_NomeConfermatoDa = new DBCursorStringField("NomeConfermatoDa");
            private DBCursorField<DateTime> m_DataConferma = new DBCursorField<DateTime>("DataConferma");
            private DBCursorStringField m_DescrizioneSpedizione = new DBCursorStringField("DescrizioneSpedizione");
            private DBCursorStringField m_NotePerIlCorriere = new DBCursorStringField("NotePerIlCorriere");
            private DBCursorStringField m_NotePerIlDestinatario = new DBCursorStringField("NotePerIlDestinatario");
            private DBCursorField<StatoOggettoDaSpedire> m_StatoOggetto = new DBCursorField<StatoOggettoDaSpedire>("StatoOggetto");
            private DBCursorField<OggettoDaSpedireFlags> m_Flags = new DBCursorField<OggettoDaSpedireFlags>("Flags");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");
            private DBCursorField<DateTime> m_DataInizioSpedizione = new DBCursorField<DateTime>("DataInizioSpedizione");
            private DBCursorField<DateTime> m_DataConsegna = new DBCursorField<DateTime>("DataConsegna");
            private DBCursorStringField m_CategoriaContenuto = new DBCursorStringField("CategoriaContenuto");
            private DBCursorStringField m_DescrizioneContenuto = new DBCursorStringField("DescrizioneContenuto");

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettiDaSpedireCursor()
            {
            }

            /// <summary>
            /// DescrizioneContenuto
            /// </summary>
            public DBCursorStringField DescrizioneContenuto
            {
                get
                {
                    return m_DescrizioneContenuto;
                }
            }

            /// <summary>
            /// CategoriaContenuto
            /// </summary>
            public DBCursorStringField CategoriaContenuto
            {
                get
                {
                    return m_CategoriaContenuto;
                }
            }

            /// <summary>
            /// AspettoBeni
            /// </summary>
            public DBCursorStringField AspettoBeni
            {
                get
                {
                    return m_AspettoBeni;
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
            /// IDDestinatario
            /// </summary>
            public DBCursorField<int> IDDestinatario
            {
                get
                {
                    return m_IDDestinatario;
                }
            }

            /// <summary>
            /// NomeDestinatario
            /// </summary>
            public DBCursorStringField NomeDestinatario
            {
                get
                {
                    return m_NomeDestinatario;
                }
            }

            /// <summary>
            /// INDMITT_CAP
            /// </summary>
            public DBCursorStringField INDMITT_CAP
            {
                get
                {
                    return m_INDMITT_CAP;
                }
            }

            /// <summary>
            /// INDMITT_Citta
            /// </summary>
            public DBCursorStringField INDMITT_Citta
            {
                get
                {
                    return m_INDMITT_Citta;
                }
            }

            /// <summary>
            /// INDMITT_Provincia
            /// </summary>
            public DBCursorStringField INDMITT_Provincia
            {
                get
                {
                    return m_INDMITT_Provincia;
                }
            }

            /// <summary>
            /// INDMITT_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField INDMITT_ToponimoViaECivico
            {
                get
                {
                    return m_INDMITT_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// INDDEST_CAP
            /// </summary>
            public DBCursorStringField INDDEST_CAP
            {
                get
                {
                    return m_INDDEST_CAP;
                }
            }

            /// <summary>
            /// INDDEST_Citta
            /// </summary>
            public DBCursorStringField INDDEST_Citta
            {
                get
                {
                    return m_INDDEST_Citta;
                }
            }

            /// <summary>
            /// INDDEST_Provincia
            /// </summary>
            public DBCursorStringField INDDEST_Provincia
            {
                get
                {
                    return m_INDDEST_Provincia;
                }
            }

            /// <summary>
            /// INDDEST_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField INDDEST_ToponimoViaECivico
            {
                get
                {
                    return m_INDDEST_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// NumeroColli
            /// </summary>
            public DBCursorField<int> NumeroColli
            {
                get
                {
                    return m_NumeroColli;
                }
            }

            /// <summary>
            /// Peso
            /// </summary>
            public DBCursorField<double> Peso
            {
                get
                {
                    return m_Peso;
                }
            }

            /// <summary>
            /// Altezza
            /// </summary>
            public DBCursorField<double> Altezza
            {
                get
                {
                    return m_Altezza;
                }
            }

            /// <summary>
            /// Larghezza
            /// </summary>
            public DBCursorField<double> Larghezza
            {
                get
                {
                    return m_Larghezza;
                }
            }

            /// <summary>
            /// Profondita
            /// </summary>
            public DBCursorField<double> Profondita
            {
                get
                {
                    return m_Profondita;
                }
            }

            /// <summary>
            /// IDRichiestaDa
            /// </summary>
            public DBCursorField<int> IDRichiestaDa
            {
                get
                {
                    return m_IDRichiestaDa;
                }
            }

            /// <summary>
            /// NomeRichiestaDa
            /// </summary>
            public DBCursorStringField NomeRichiestaDa
            {
                get
                {
                    return m_NomeRichiestaDa;
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
            /// IDConfermatoDa
            /// </summary>
            public DBCursorField<int> IDConfermatoDa
            {
                get
                {
                    return m_IDConfermatoDa;
                }
            }

            /// <summary>
            /// NomeConfermatoDa
            /// </summary>
            public DBCursorStringField NomeConfermatoDa
            {
                get
                {
                    return m_NomeConfermatoDa;
                }
            }

            /// <summary>
            /// DataConferma
            /// </summary>
            public DBCursorField<DateTime> DataConferma
            {
                get
                {
                    return m_DataConferma;
                }
            }

            /// <summary>
            /// DescrizioneSpedizione
            /// </summary>
            public DBCursorStringField DescrizioneSpedizione
            {
                get
                {
                    return m_DescrizioneSpedizione;
                }
            }

            /// <summary>
            /// NotePerIlCorriere
            /// </summary>
            public DBCursorStringField NotePerIlCorriere
            {
                get
                {
                    return m_NotePerIlCorriere;
                }
            }

            /// <summary>
            /// NotePerIlDestinatario
            /// </summary>
            public DBCursorStringField NotePerIlDestinatario
            {
                get
                {
                    return m_NotePerIlDestinatario;
                }
            }

            /// <summary>
            /// StatoOggetto
            /// </summary>
            public DBCursorField<StatoOggettoDaSpedire> StatoOggetto
            {
                get
                {
                    return m_StatoOggetto;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<OggettoDaSpedireFlags> Flags
            {
                get
                {
                    return m_Flags;
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
            /// DataInizioSpedizione
            /// </summary>
            public DBCursorField<DateTime> DataInizioSpedizione
            {
                get
                {
                    return m_DataInizioSpedizione;
                }
            }

            /// <summary>
            /// DataConsegna
            /// </summary>
            public DBCursorField<DateTime> DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.OggettiDaSpedire;
            }
             
        }
    }
}