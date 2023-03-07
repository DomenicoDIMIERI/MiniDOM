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
        /// Cursore di <see cref="Spedizione"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class SpedizioniCursor 
            : minidom.Databases.DBObjectCursorPO<Spedizione>
        {
            private DBCursorStringField m_AspettoBeni = new DBCursorStringField("AspettoBeni");
            private DBCursorField<int> m_IDMittente = new DBCursorField<int>("IDMittente");
            private DBCursorStringField m_NomeMittente = new DBCursorStringField("NomeMittente");
            private DBCursorStringField m_IndirizzoMittente_Nome = new DBCursorStringField("IndirizzoMittente_Nome");
            private DBCursorStringField m_IndirizzoMittente_ToponimoViaECivico = new DBCursorStringField("IndirizzoMittente_Via");
            private DBCursorStringField m_IndirizzoMittente_CAP = new DBCursorStringField("IndirizzoMittente_CAP");
            private DBCursorStringField m_IndirizzoMittente_Citta = new DBCursorStringField("IndirizzoMittente_Citta");
            private DBCursorStringField m_IndirizzoMittente_Provincia = new DBCursorStringField("IndirizzoMittente_Provincia");
            private DBCursorField<int> m_IDDestinatario = new DBCursorField<int>("IDDestinatario");
            private DBCursorStringField m_NomeDestinatario = new DBCursorStringField("NomeDestinatario");
            private DBCursorStringField m_IndirizzoDestinatario_Nome = new DBCursorStringField("IndirizzoDest_Nome");
            private DBCursorStringField m_IndirizzoDestinatario_ToponimoViaECivico = new DBCursorStringField("IndirizzoDest_Via");
            private DBCursorStringField m_IndirizzoDestinatario_CAP = new DBCursorStringField("IndirizzoDest_CAP");
            private DBCursorStringField m_IndirizzoDestinatario_Citta = new DBCursorStringField("IndirizzoDest_Citta");
            private DBCursorStringField m_IndirizzoDestinatario_Provincia = new DBCursorStringField("IndirizzoDest_Provincia");
            private DBCursorField<int> m_NumeroColli = new DBCursorField<int>("NumeroColli");
            private DBCursorField<double> m_Peso = new DBCursorField<double>("Peso");
            private DBCursorField<double> m_Altezza = new DBCursorField<double>("Altezza");
            private DBCursorField<double> m_Larghezza = new DBCursorField<double>("Larghezza");
            private DBCursorField<double> m_Profondita = new DBCursorField<double>("Profondita");
            private DBCursorField<int> m_IDSpeditoDa = new DBCursorField<int>("IDSpeditoDa");
            private DBCursorStringField m_NomeSpeditoDa = new DBCursorStringField("NomeSpeditoDa");
            private DBCursorField<int> m_IDRicevutoDa = new DBCursorField<int>("IDRicevutoDa");
            private DBCursorStringField m_NomeRicevutoDa = new DBCursorStringField("NomeRicevutoDa");
            private DBCursorField<DateTime> m_DataInizioSpedizione = new DBCursorField<DateTime>("DataInizioSpedizione");
            private DBCursorStringField m_NotePerIlCorriere = new DBCursorStringField("NotePerIlCorriere");
            private DBCursorStringField m_NotePerIlDestinatario = new DBCursorStringField("NotePerIlDestinatario");
            private DBCursorField<StatoSpedizione> m_StatoSpedizione = new DBCursorField<StatoSpedizione>("StatoSpedizione");
            private DBCursorField<StatoConsegna> m_StatoConsegna = new DBCursorField<StatoConsegna>("StatoConsegna");
            private DBCursorField<DateTime> m_DataConsegna = new DBCursorField<DateTime>("DataConsegna");
            private DBCursorField<SpedizioneFlags> m_Flags = new DBCursorField<SpedizioneFlags>("Flags");
            private DBCursorStringField m_NomeCorriere = new DBCursorStringField("NomeCorriere");
            private DBCursorField<int> m_IDCorriere = new DBCursorField<int>("IDCorriere");
            private DBCursorStringField m_NumeroSpedizione = new DBCursorStringField("NumeroSpedizione");

            /// <summary>
            /// Costruttore
            /// </summary>
            public SpedizioniCursor()
            {
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
            /// IDMittente
            /// </summary>
            public DBCursorField<int> IDMittente
            {
                get
                {
                    return m_IDMittente;
                }
            }

            /// <summary>
            /// NomeMittente
            /// </summary>
            public DBCursorStringField NomeMittente
            {
                get
                {
                    return m_NomeMittente;
                }
            }

            /// <summary>
            /// IndirizzoMittente_Nome
            /// </summary>
            public DBCursorStringField IndirizzoMittente_Nome
            {
                get
                {
                    return m_IndirizzoDestinatario_Nome;
                }
            }

            /// <summary>
            /// IndirizzoMittente_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoMittente_ToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoDestinatario_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// IndirizzoMittente_CAP
            /// </summary>
            public DBCursorStringField IndirizzoMittente_CAP
            {
                get
                {
                    return m_IndirizzoMittente_CAP;
                }
            }

            /// <summary>
            /// IndirizzoMittente_Citta
            /// </summary>
            public DBCursorStringField IndirizzoMittente_Citta
            {
                get
                {
                    return m_IndirizzoMittente_Citta;
                }
            }

            /// <summary>
            /// IndirizzoMittente_Provincia
            /// </summary>
            public DBCursorStringField IndirizzoMittente_Provincia
            {
                get
                {
                    return m_IndirizzoMittente_Provincia;
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
            /// IndirizzoDestinatario_Nome
            /// </summary>
            public DBCursorStringField IndirizzoDestinatario_Nome
            {
                get
                {
                    return m_IndirizzoDestinatario_Nome;
                }
            }

            /// <summary>
            /// IndirizzoDestinatario_ToponimoViaECivico
            /// </summary>
            public DBCursorStringField IndirizzoDestinatario_ToponimoViaECivico
            {
                get
                {
                    return m_IndirizzoDestinatario_ToponimoViaECivico;
                }
            }

            /// <summary>
            /// IndirizzoDestinatario_CAP
            /// </summary>
            public DBCursorStringField IndirizzoDestinatario_CAP
            {
                get
                {
                    return m_IndirizzoDestinatario_CAP;
                }
            }

            /// <summary>
            /// IndirizzoDestinatario_Citta
            /// </summary>
            public DBCursorStringField IndirizzoDestinatario_Citta
            {
                get
                {
                    return m_IndirizzoDestinatario_Citta;
                }
            }

            /// <summary>
            /// IndirizzoDestinatario_Provincia
            /// </summary>
            public DBCursorStringField IndirizzoDestinatario_Provincia
            {
                get
                {
                    return m_IndirizzoDestinatario_Provincia;
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
            /// IDSpeditoDa
            /// </summary>
            public DBCursorField<int> IDSpeditoDa
            {
                get
                {
                    return m_IDSpeditoDa;
                }
            }

            /// <summary>
            /// NomeSpeditoDa
            /// </summary>
            public DBCursorStringField NomeSpeditoDa
            {
                get
                {
                    return m_NomeSpeditoDa;
                }
            }

            /// <summary>
            /// IDRicevutoDa
            /// </summary>
            public DBCursorField<int> IDRicevutoDa
            {
                get
                {
                    return m_IDRicevutoDa;
                }
            }

            /// <summary>
            /// NomeRicevutoDa
            /// </summary>
            public DBCursorStringField NomeRicevutoDa
            {
                get
                {
                    return m_NomeRicevutoDa;
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
            /// StatoSpedizione
            /// </summary>
            public DBCursorField<StatoSpedizione> StatoSpedizione
            {
                get
                {
                    return m_StatoSpedizione;
                }
            }

            /// <summary>
            /// StatoConsegna
            /// </summary>
            public DBCursorField<StatoConsegna> StatoConsegna
            {
                get
                {
                    return m_StatoConsegna;
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
            /// Flags
            /// </summary>
            public DBCursorField<SpedizioneFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// NomeCorriere
            /// </summary>
            public DBCursorStringField NomeCorriere
            {
                get
                {
                    return m_NomeCorriere;
                }
            }

            /// <summary>
            /// IDCorriere
            /// </summary>
            public DBCursorField<int> IDCorriere
            {
                get
                {
                    return m_IDCorriere;
                }
            }

            /// <summary>
            /// NumeroSpedizione
            /// </summary>
            public DBCursorStringField NumeroSpedizione
            {
                get
                {
                    return m_NumeroSpedizione;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Spedizioni;
            }
             
        }
    }
}