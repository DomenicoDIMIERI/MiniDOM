using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Finanziaria;
using DMD.Databases;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Cursore di oggetti <see cref="ClienteXCollaboratore"/>
        /// </summary>
        [Serializable]
        public class ClienteXCollaboratoreCursor 
            : minidom.Databases.DBObjectCursor<ClienteXCollaboratore>
        {
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Cognome = new DBCursorStringField("Cognome");
            private DBCursorStringField m_CodiceFiscale = new DBCursorStringField("CodiceFiscale");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private DBCursorField<DateTime> m_DataNascita = new DBCursorField<DateTime>("DataNascita");
            private DBCursorStringField m_Indirizzo_Provincia = new DBCursorStringField("Indirizzo_Provincia");
            private DBCursorStringField m_Indirizzo_Citta = new DBCursorStringField("Indirizzo_Citta");
            private DBCursorStringField m_Indirizzo_CAP = new DBCursorStringField("Indirizzo_CAP");
            private DBCursorStringField m_Indirizzo_Via = new DBCursorStringField("Indirizzo_Via");
            private DBCursorField<DateTime> m_DataAcquisizione = new DBCursorField<DateTime>("DataAcquisizione");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorStringField m_NomeFonte = new DBCursorStringField("NomeFonte");
            private DBCursorField<ClienteCollaboratoreFlags> m_Flags = new DBCursorField<ClienteCollaboratoreFlags>("Flags");
            private DBCursorField<StatoClienteCollaboratore> m_StatoLavorazione = new DBCursorField<StatoClienteCollaboratore>("StatoLavorazione");
            private DBCursorStringField m_DettaglioStatoLavorazione = new DBCursorStringField("DettaglioStatoLavorazione");
            private DBCursorStringField m_NomeAmministrazione = new DBCursorStringField("NomeAmministrazione");
            private DBCursorStringField m_TelefonoCasa = new DBCursorStringField("TelefonoCasa");
            private DBCursorStringField m_TelefonoUfficio = new DBCursorStringField("TelefonoUfficio");
            private DBCursorStringField m_TelefonoCellulare = new DBCursorStringField("TelefonoCellulare");
            private DBCursorStringField m_Fax = new DBCursorStringField("Fax");
            private DBCursorStringField m_AltroTelefono = new DBCursorStringField("AltroTelefono");
            private DBCursorStringField m_eMailPersonale = new DBCursorStringField("eMailPersonale");
            private DBCursorStringField m_eMailUfficio = new DBCursorStringField("eMailUfficio");
            private DBCursorStringField m_PEC = new DBCursorStringField("PEC");
            private DBCursorField<DateTime> m_DataRinnovoCQS = new DBCursorField<DateTime>("DataRinnovoCQS");
            private DBCursorStringField m_MotivoRicontatto = new DBCursorStringField("MotivoRicontatto");
            private DBCursorField<DateTime> m_DataRinnovoPD = new DBCursorField<DateTime>("DataRinnovoPD");
            private DBCursorStringField m_ImportoRichiesto = new DBCursorStringField("ImportoRichiesto");
            private DBCursorStringField m_MotivoRichiesta = new DBCursorStringField("MotivoRichiesta");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<int> m_IDConsulente = new DBCursorField<int>("IDConsulente");
            private DBCursorField<DateTime> m_DataAssegnazione = new DBCursorField<DateTime>("DataAssegnazione");
            private DBCursorStringField m_MotivoAssegnazione = new DBCursorStringField("MotivoAssegnazione");
            private DBCursorField<int> m_IDAssegnatoDa = new DBCursorField<int>("IDAssegnatoDa");
            private DBCursorField<DateTime> m_DataRimozione = new DBCursorField<DateTime>("DataRimozione");
            private DBCursorStringField m_MotivoRimozione = new DBCursorStringField("MotivoRimozione");
            private DBCursorField<int> m_IDRimossoDa = new DBCursorField<int>("IDRimossoDa");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ClienteXCollaboratoreCursor()
            {
            }

            /// <summary>
            /// DataRimozione
            /// </summary>
            public DBCursorField<DateTime> DataRimozione
            {
                get
                {
                    return m_DataRimozione;
                }
            }

            /// <summary>
            /// MotivoRimozione
            /// </summary>
            public DBCursorStringField MotivoRimozione
            {
                get
                {
                    return m_MotivoRimozione;
                }
            }

            /// <summary>
            /// IDRimossoDa
            /// </summary>
            public DBCursorField<int> IDRimossoDa
            {
                get
                {
                    return m_IDRimossoDa;
                }
            }

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
            /// DataAssegnazione
            /// </summary>
            public DBCursorField<DateTime> DataAssegnazione
            {
                get
                {
                    return m_DataAssegnazione;
                }
            }

            /// <summary>
            /// MotivoAssegnazione
            /// </summary>
            public DBCursorStringField MotivoAssegnazione
            {
                get
                {
                    return m_MotivoAssegnazione;
                }
            }

            /// <summary>
            /// IDAssegnatoDa
            /// </summary>
            public DBCursorField<int> IDAssegnatoDa
            {
                get
                {
                    return m_IDAssegnatoDa;
                }
            }

            /// <summary>
            /// MotivoRicontatto
            /// </summary>
            public DBCursorStringField MotivoRicontatto
            {
                get
                {
                    return m_MotivoRicontatto;
                }
            }

            /// <summary>
            /// ImportoRichiesto
            /// </summary>
            public DBCursorStringField ImportoRichiesto
            {
                get
                {
                    return m_ImportoRichiesto;
                }
            }

            /// <summary>
            /// MotivoRichiesta
            /// </summary>
            public DBCursorStringField MotivoRichiesta
            {
                get
                {
                    return m_MotivoRichiesta;
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
            /// DataRinnovoCQS
            /// </summary>
            public DBCursorField<DateTime> DataRinnovoCQS
            {
                get
                {
                    return m_DataRinnovoCQS;
                }
            }

            /// <summary>
            /// DataRinnovoPD
            /// </summary>
            public DBCursorField<DateTime> DataRinnovoPD
            {
                get
                {
                    return m_DataRinnovoPD;
                }
            }

            /// <summary>
            /// IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
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
            /// Cognome
            /// </summary>
            public DBCursorStringField Cognome
            {
                get
                {
                    return m_Cognome;
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
            /// Provincia di residen
            /// </summary>
            public DBCursorStringField Indirizzo_Provincia
            {
                get
                {
                    return m_Indirizzo_Provincia;
                }
            }

            /// <summary>
            /// Indirizzo_Citta
            /// </summary>
            public DBCursorStringField Indirizzo_Citta
            {
                get
                {
                    return m_Indirizzo_Citta;
                }
            }

            /// <summary>
            /// Indirizzo_CAP
            /// </summary>
            public DBCursorStringField Indirizzo_CAP
            {
                get
                {
                    return m_Indirizzo_CAP;
                }
            }

            /// <summary>
            /// Indirizzo_Via
            /// </summary>
            public DBCursorStringField Indirizzo_Via
            {
                get
                {
                    return m_Indirizzo_Via;
                }
            }

            /// <summary>
            /// DataAcquisizione
            /// </summary>
            public DBCursorField<DateTime> DataAcquisizione
            {
                get
                {
                    return m_DataAcquisizione;
                }
            }

            /// <summary>
            /// TipoFonte
            /// </summary>
            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonte;
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
            /// Flags
            /// </summary>
            public DBCursorField<ClienteCollaboratoreFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// StatoLavorazione
            /// </summary>
            public DBCursorField<StatoClienteCollaboratore> StatoLavorazione
            {
                get
                {
                    return m_StatoLavorazione;
                }
            }

            /// <summary>
            /// DettaglioStatoLavorazione
            /// </summary>
            public DBCursorStringField DettaglioStatoLavorazione
            {
                get
                {
                    return m_DettaglioStatoLavorazione;
                }
            }

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
            /// TelefonoCasa
            /// </summary>
            public DBCursorStringField TelefonoCasa
            {
                get
                {
                    return m_TelefonoCasa;
                }
            }

            /// <summary>
            /// TelefonoUfficio
            /// </summary>
            public DBCursorStringField TelefonoUfficio
            {
                get
                {
                    return m_TelefonoUfficio;
                }
            }

            /// <summary>
            /// TelefonoCellulare
            /// </summary>
            public DBCursorStringField TelefonoCellulare
            {
                get
                {
                    return m_TelefonoCellulare;
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
            /// AltroTelefono
            /// </summary>
            public DBCursorStringField AltroTelefono
            {
                get
                {
                    return m_AltroTelefono;
                }
            }

            /// <summary>
            /// eMailPersonale
            /// </summary>
            public DBCursorStringField eMailPersonale
            {
                get
                {
                    return m_eMailPersonale;
                }
            }

            /// <summary>
            /// eMailUfficio
            /// </summary>
            public DBCursorStringField eMailUfficio
            {
                get
                {
                    return m_eMailUfficio;
                }
            }

            /// <summary>
            /// PEC
            /// </summary>
            public DBCursorStringField PEC
            {
                get
                {
                    return m_PEC;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Collaboratori.ClientiXCollaboratori;
            }

             
        }
    }
}