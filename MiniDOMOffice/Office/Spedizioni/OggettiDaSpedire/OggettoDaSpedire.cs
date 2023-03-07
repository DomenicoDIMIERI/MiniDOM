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
        /// Stato di un oggetto <see cref="OggettoDaSpedire"/>
        /// </summary>
        public enum StatoOggettoDaSpedire : int
        {
            /// <summary>
            /// Oggetto in preparazione (ancora non deve essere spedito)
            /// </summary>
            /// <remarks></remarks>
            InPreparazione = 0,

            /// <summary>
            /// Oggetto pronto (deve essere spedito)
            /// </summary>
            /// <remarks></remarks>
            ProntoPerLaSpedizione = 10,

            /// <summary>
            /// L'oggetto è stato Spedito
            /// </summary>
            /// <remarks></remarks>
            Spedito = 20,

            /// <summary>
            /// La spedizione è stata annullata dall'operatore
            /// </summary>
            /// <remarks></remarks>
            SpedizioneAnnullata = 30,

            /// <summary>
            /// La spedizione è stata rifiutata dal corriere
            /// </summary>
            /// <remarks></remarks>
            SpedizioneRifiutata = 31,

            /// <summary>
            /// La spedizione è stata fermata dall'agenzia
            /// </summary>
            /// <remarks></remarks>
            SpedizioneBocciata = 32,

            /// <summary>
            /// L'oggetto è stato consegnato correttamente
            /// </summary>
            /// <remarks></remarks>
            Consegnato = 40,


            /// <summary>
            /// Non è stato possibile consegnare l'oggetto
            /// </summary>
            /// <remarks></remarks>
            ConsegnaFallita = 50,

            /// <summary>
            /// Consegna fallita perché l'indirizzo è errato
            /// </summary>
            /// <remarks></remarks>
            ConsegnaFallitaIndirizzoErrato = 51,

            /// <summary>
            /// Consegna fallita perché il destinatario ha rifiutato l'oggetto
            /// </summary>
            /// <remarks></remarks>
            ConsegnaFallitaRifiutoDestinatario = 52,

            /// <summary>
            /// Consegna fallita perché il destinatario non era all'indirizzo indicato
            /// </summary>
            /// <remarks></remarks>
            ConsegnaFallitaNonTrovato = 53
        }

        /// <summary>
        /// Flag per gli oggetti <see cref="OggettoDaSpedire"/>
        /// </summary>
        [Flags]
        public enum OggettoDaSpedireFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }

        /// <summary>
        /// Rappresenta un oggetto da spedire
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class OggettoDaSpedire
            : Databases.DBObjectPO, IComparable, IComparable<OggettoDaSpedire>
        {
            private string m_AspettoBeni;                 // Indica il tipo di oggetto spedito (busta, pacco, pallet)
            private int m_IDCliente;                  // Persona per cui si spedisce l'oggetto
            [NonSerialized] private CPersona m_Cliente;
            private string m_NomeCliente;
            private int m_IDDestinatario;             // Persona a cui è destinato l'oggetto
            [NonSerialized] private CPersona m_Destinatario;
            private string m_NomeDestinatario;
            private CIndirizzo m_IndirizzoMittente;   // Indirizzo a cui inviare l'oggetto
            private CIndirizzo m_IndirizzoDestinatario;   // Indirizzo a cui inviare l'oggetto
            private int? m_NumeroColli;                // Numero di colli spediti
            private double? m_Peso;                       // Peso degli oggetti spediti
            private double? m_Altezza;                    // Altezza
            private double? m_Larghezza;                  // Larghezza
            private double? m_Profondita;                // Profondita
            [NonSerialized] private CUser m_RichiestaDa;                  // Operatore che ha richiesto la spedizione
            private int m_IDRichiestaDa;
            private string m_NomeRichiestaDa;
            private DateTime? m_DataRichiesta;
            [NonSerialized] private CUser m_PresaInCaricoDa;              // Utente che ha preso in carico l'oggetto ed ha predisposto la spedizione
            private int m_IDPresaInCaricoDa;
            private string m_NomePresaInCaricoDa;
            private DateTime? m_DataPresaInCarico;
            [NonSerialized] private CUser m_ConfermatoDa;                 // Utente che ha registrato l'esito finale della spedizione
            private int m_IDConfermatoDa;
            private string m_NomeConfermatoDa;
            private DateTime? m_DataConferma;
            private string m_DescrizioneSpedizione;
            private string m_NotePerIlCorriere;
            private string m_NotePerIlDestinatario;
            private StatoOggettoDaSpedire m_StatoOggetto;
            private string m_DettaglioStato;
            private DateTime? m_DataInizioSpedizione;
            private DateTime? m_DataConsegna;
            private string m_CategoriaContenuto;
            private string m_DescrizioneContenuto;
            private int m_IDSpedizione;
            [NonSerialized] private Spedizione m_Spedizione;
            private string m_NumeroSpedizione;
            private int m_IDCorriere;
            private string m_NomeCorriere;

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettoDaSpedire()
            {
                m_AspettoBeni = "";
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_IDDestinatario = 0;
                m_Destinatario = null;
                m_NomeDestinatario = "";
                m_IndirizzoMittente = new Anagrafica.CIndirizzo();
                m_IndirizzoDestinatario = new Anagrafica.CIndirizzo();
                m_NumeroColli = default;
                m_Peso = default;
                m_Altezza = default;
                m_Larghezza = default;
                m_Profondita = default;
                m_RichiestaDa = null;
                m_IDRichiestaDa = 0;
                m_NomeRichiestaDa = "";
                m_DataRichiesta = default;
                m_PresaInCaricoDa = null;
                m_IDPresaInCaricoDa = 0;
                m_NomePresaInCaricoDa = "";
                m_DataPresaInCarico = default;
                m_ConfermatoDa = null;
                m_IDConfermatoDa = 0;
                m_NomeConfermatoDa = "";
                m_DataConferma = default;
                m_DescrizioneSpedizione = "";
                m_NotePerIlCorriere = "";
                m_NotePerIlDestinatario = "";
                m_StatoOggetto = StatoOggettoDaSpedire.InPreparazione;
                m_Flags = (int) OggettoDaSpedireFlags.None;
                m_DettaglioStato = "";
                m_DataInizioSpedizione = default;
                m_DataConsegna = default;
                m_CategoriaContenuto = "";
                m_DescrizioneContenuto = "";
                m_IDSpedizione = 0;
                m_Spedizione = null;
                m_NumeroSpedizione = "";
                m_IDCorriere = 0;
                m_NomeCorriere = "";
            }

            /// <summary>
            /// Restituisce o imposta una stringa che identifica la categoria del contenuto (es. Documenti, ...)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CategoriaContenuto
            {
                get
                {
                    return m_CategoriaContenuto;
                }

                set
                {
                    string oldValue = m_CategoriaContenuto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CategoriaContenuto = value;
                    DoChanged("CategoriaContenuto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la descrizione per esteso del contenuto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DescrizioneContenuto
            {
                get
                {
                    return m_DescrizioneContenuto;
                }

                set
                {
                    string oldValue = m_DescrizioneContenuto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneContenuto = value;
                    DoChanged("DescrizioneContenuto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive l'aspetto degli oggetti spediti (Buste, Pacchi, Pallet, )
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AspettoBeni
            {
                get
                {
                    return m_AspettoBeni;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AspettoBeni;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AspettoBeni = value;
                    DoChanged("AspettoBeni", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona che ha inviato l'oggetto (mittente)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    string oldValue = IDCliente.ToString();
                    if (DMD.Doubles.CDbl(oldValue) == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDMittente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona che ha inviato l'oggetto (mittente)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mittente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCliente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto che descrive l'indirizzo del mittente (da cui parte la spedizione)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoMittente
            {
                get
                {
                    return m_IndirizzoMittente;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona a cui è destinata la spedizione (Destinatario)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDestinatario
            {
                get
                {
                    return DBUtils.GetID(m_Destinatario, m_IDDestinatario);
                }

                set
                {
                    int oldValue = IDDestinatario;
                    if (oldValue == value)
                        return;
                    m_Destinatario = null;
                    m_IDDestinatario = value;
                    DoChanged("IDDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona a cui è destinata la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Destinatario
            {
                get
                {
                    if (m_Destinatario is null)
                        m_Destinatario = Anagrafica.Persone.GetItemById(m_IDDestinatario);
                    return m_Destinatario;
                }

                set
                {
                    var oldValue = m_Destinatario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Destinatario = value;
                    m_IDDestinatario = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeDestinatario = value.Nominativo;
                    DoChanged("Destinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persoan a cui è destinata la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeDestinatario
            {
                get
                {
                    return m_NomeDestinatario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeDestinatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDestinatario = value;
                    DoChanged("NomeDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto che descrive l'indirizzo presso cui consegnare la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoDestinatario
            {
                get
                {
                    return m_IndirizzoDestinatario;
                }
            }

            /// <summary>
            /// Restituisce o imosta il numero di colli della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int? NumeroColli
            {
                get
                {
                    return m_NumeroColli;
                }

                set
                {
                    var oldValue = m_NumeroColli;
                    if (oldValue == value == true)
                        return;
                    m_NumeroColli = value;
                    DoChanged("NumeroColli", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il peso totale degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Peso
            {
                get
                {
                    return m_Peso;
                }

                set
                {
                    double oldValue = (double)m_Peso;
                    if (oldValue == value == true)
                        return;
                    m_Peso = value;
                    DoChanged("Peso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'altezza degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Altezza
            {
                get
                {
                    return m_Altezza;
                }

                set
                {
                    var oldValue = m_Altezza;
                    if (oldValue == value == true)
                        return;
                    m_Altezza = value;
                    DoChanged("Altezza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la larghezza degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Larghezza
            {
                get
                {
                    return m_Larghezza;
                }

                set
                {
                    var oldValue = m_Larghezza;
                    if (oldValue == value == true)
                        return;
                    m_Larghezza = value;
                    DoChanged("Larghezza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la profondità degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Profondita
            {
                get
                {
                    return m_Profondita;
                }

                set
                {
                    var oldValue = m_Profondita;
                    if (oldValue == value == true)
                        return;
                    m_Profondita = value;
                    DoChanged("Profondita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che richiesto la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser RichiestaDa
            {
                get
                {
                    if (m_RichiestaDa is null)
                        m_RichiestaDa = Sistema.Users.GetItemById(m_IDRichiestaDa);
                    return m_RichiestaDa;
                }

                set
                {
                    var oldValue = RichiestaDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaDa = value;
                    m_IDRichiestaDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeRichiestaDa = value.Nominativo;
                    DoChanged("RichiestaDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha richiesto la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRichiestaDa
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaDa, m_IDRichiestaDa);
                }

                set
                {
                    int oldValue = IDRichiestaDa;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaDa = value;
                    m_RichiestaDa = null;
                    DoChanged("IDRichiestaDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha richiesto la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeRichiestaDa
            {
                get
                {
                    return m_NomeRichiestaDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRichiestaDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiestaDa = value;
                    DoChanged("NomeRichiestaDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora in cui l'oggetto è stato marcato come pronto per la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }

                set
                {
                    var oldValue = m_DataRichiesta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiesta = value;
                    DoChanged("DataRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha preso in carico la spedizione dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser PresaInCaricoDa
            {
                get
                {
                    if (m_PresaInCaricoDa is null)
                        m_PresaInCaricoDa = Sistema.Users.GetItemById(m_IDPresaInCaricoDa);
                    return m_PresaInCaricoDa;
                }

                set
                {
                    var oldValue = PresaInCaricoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PresaInCaricoDa = value;
                    m_IDPresaInCaricoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePresaInCaricoDa = value.Nominativo;
                    DoChanged("PresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha preso in carico l'oggetto per la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPresaInCaricoDa
            {
                get
                {
                    return DBUtils.GetID(m_PresaInCaricoDa, m_IDPresaInCaricoDa);
                }

                set
                {
                    int oldValue = IDPresaInCaricoDa;
                    if (oldValue == value)
                        return;
                    m_IDPresaInCaricoDa = value;
                    m_PresaInCaricoDa = null;
                    DoChanged("IDPresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha preso in carico l'oggetto per la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePresaInCaricoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePresaInCaricoDa = value;
                    DoChanged("NomePresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui l'utente ha preso in carico l'oggetto per la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }

                set
                {
                    var oldValue = m_DataPresaInCarico;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPresaInCarico = value;
                    DoChanged("DataPresaInCarico", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha registrato la conclusione della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser ConfermatoDa                 // Utente che ha registrato l'esito finale della spedizione
            {
                get
                {
                    if (m_ConfermatoDa is null)
                        m_ConfermatoDa = Sistema.Users.GetItemById(m_IDConfermatoDa);
                    return m_ConfermatoDa;
                }

                set
                {
                    var oldValue = ConfermatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ConfermatoDa = value;
                    m_IDConfermatoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeConfermatoDa = value.Nominativo;
                    DoChanged("ConfermatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha confermato l'esito della registrazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDConfermatoDa
            {
                get
                {
                    return DBUtils.GetID(m_ConfermatoDa, m_IDConfermatoDa);
                }

                set
                {
                    int oldValue = IDConfermatoDa;
                    if (oldValue == value)
                        return;
                    m_IDConfermatoDa = value;
                    m_ConfermatoDa = null;
                    DoChanged("IDConfermatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha confermato l'esito della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeConfermatoDa
            {
                get
                {
                    return m_NomeConfermatoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeConfermatoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeConfermatoDa = value;
                    DoChanged("NomeConfermatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di registrazione dell'esito della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataConferma
            {
                get
                {
                    return m_DataConferma;
                }

                set
                {
                    var oldValue = m_DataConferma;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataConferma = value;
                    DoChanged("DataConferma", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta la data di inizio della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizioSpedizione
            {
                get
                {
                    return m_DataInizioSpedizione;
                }

                set
                {
                    var oldValue = m_DataInizioSpedizione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioSpedizione = value;
                    DoChanged("DataInizioSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DescrizioneSpedizione
            {
                get
                {
                    return m_DescrizioneSpedizione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DescrizioneSpedizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneSpedizione = value;
                    DoChanged("DescrizioneSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa contenente degli avvertimenti diretti al corriere
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NotePerIlCorriere
            {
                get
                {
                    return m_NotePerIlCorriere;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NotePerIlCorriere;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NotePerIlCorriere = value;
                    DoChanged("NotePerIlCorriere", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle annotazioni dirette al destinatario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NotePerIlDestinatario
            {
                get
                {
                    return m_NotePerIlDestinatario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NotePerIlDestinatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NotePerIlDestinatario = value;
                    DoChanged("NotePerIlDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoOggettoDaSpedire StatoOggetto
            {
                get
                {
                    return m_StatoOggetto;
                }

                set
                {
                    var oldValue = m_StatoOggetto;
                    if (oldValue == value)
                        return;
                    m_StatoOggetto = value;
                    DoChanged("StatoOggetto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags aggiuntivi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public OggettoDaSpedireFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di consegna
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }

                set
                {
                    var oldValue = m_DataConsegna;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataConsegna = value;
                    DoChanged("DataConsegna", value, oldValue);
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.OggettiDaSpedire;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeOggettiDaSpedire";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return    base.IsChanged() 
                       || m_IndirizzoDestinatario.IsChanged() 
                       || m_IndirizzoMittente.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_IndirizzoDestinatario.SetChanged(false);
                m_IndirizzoMittente.SetChanged(false);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_AspettoBeni = reader.Read("AspettoBeni", m_AspettoBeni);
                m_IDCliente = reader.Read("IDCliente", m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                m_IDDestinatario = reader.Read("IDDestinatario", m_IDDestinatario);
                m_NomeDestinatario = reader.Read("NomeDestinatario", m_NomeDestinatario);
                {
                    var withBlock = m_IndirizzoMittente;
                    withBlock.Nome = reader.Read("INDMITT_LABEL", withBlock.Nome);
                    withBlock.CAP = reader.Read("INDMITT_CAP", withBlock.CAP);
                    withBlock.Citta = reader.Read("INDMITT_CITTA", withBlock.Citta);
                    withBlock.Provincia = reader.Read("INDMITT_PROV", withBlock.Provincia);
                    withBlock.ToponimoViaECivico = reader.Read("INDMITT_VIA", withBlock.ToponimoViaECivico);
                    withBlock.SetChanged(false);
                }

                {
                    var withBlock1 = m_IndirizzoDestinatario;
                    withBlock1.Nome = reader.Read("INDDEST_LABEL", withBlock1.Nome);
                    withBlock1.CAP = reader.Read("INDDEST_CAP", withBlock1.CAP);
                    withBlock1.Citta = reader.Read("INDDEST_CITTA", withBlock1.Citta);
                    withBlock1.Provincia = reader.Read("INDDEST_PROV", withBlock1.Provincia);
                    withBlock1.ToponimoViaECivico = reader.Read("INDDEST_VIA", withBlock1.ToponimoViaECivico);
                    withBlock1.SetChanged(false);
                }

                m_NumeroColli = reader.Read("NumeroColli", m_NumeroColli);
                m_Peso = reader.Read("Peso", m_Peso);
                m_Altezza = reader.Read("Altezza", m_Altezza);
                m_Larghezza = reader.Read("Larghezza",  m_Larghezza);
                m_Profondita = reader.Read("Profondita",  m_Profondita);
                m_IDRichiestaDa = reader.Read("IDRichiestaDa",  m_IDRichiestaDa);
                m_NomeRichiestaDa = reader.Read("NomeRichiestaDa",  m_NomeRichiestaDa);
                m_DataRichiesta = reader.Read("DataRichiesta",  m_DataRichiesta);
                m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa",  m_IDPresaInCaricoDa);
                m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa",  m_NomePresaInCaricoDa);
                m_DataPresaInCarico = reader.Read("DataPresaInCarico",  m_DataPresaInCarico);
                m_IDConfermatoDa = reader.Read("IDConfermatoDa",  m_IDConfermatoDa);
                m_NomeConfermatoDa = reader.Read("NomeConfermatoDa",  m_NomeConfermatoDa);
                m_DataConferma = reader.Read("DataConferma",  m_DataConferma);
                m_DescrizioneSpedizione = reader.Read("DescrizioneSpedizione",  m_DescrizioneSpedizione);
                m_NotePerIlCorriere = reader.Read("NotePerIlCorriere",  m_NotePerIlCorriere);
                m_NotePerIlDestinatario = reader.Read("NotePerIlDestinatario",  m_NotePerIlDestinatario);
                m_StatoOggetto = reader.Read("StatoOggetto",  m_StatoOggetto);
                m_DettaglioStato = reader.Read("DettaglioStato",  m_DettaglioStato);
                m_DataInizioSpedizione = reader.Read("DataInizioSpedizione",  m_DataInizioSpedizione);
                m_DataConsegna = reader.Read("DataConsegna",  m_DataConsegna);
                m_CategoriaContenuto = reader.Read("CategoriaContenuto",  m_CategoriaContenuto);
                m_DescrizioneContenuto = reader.Read("DescrizioneContenuto",  m_DescrizioneContenuto);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("AspettoBeni", m_AspettoBeni);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IDDestinatario", IDDestinatario);
                writer.Write("NomeDestinatario", m_NomeDestinatario);
                {
                    var withBlock = m_IndirizzoMittente;
                    writer.Write("INDMITT_LABEL", withBlock.Nome);
                    writer.Write("INDMITT_CAP", withBlock.CAP);
                    writer.Write("INDMITT_CITTA", withBlock.Citta);
                    writer.Write("INDMITT_PROV", withBlock.Provincia);
                    writer.Write("INDMITT_VIA", withBlock.ToponimoViaECivico);
                }

                {
                    var withBlock1 = m_IndirizzoDestinatario;
                    writer.Write("INDDEST_LABEL", withBlock1.Nome);
                    writer.Write("INDDEST_CAP", withBlock1.CAP);
                    writer.Write("INDDEST_CITTA", withBlock1.Citta);
                    writer.Write("INDDEST_PROV", withBlock1.Provincia);
                    writer.Write("INDDEST_VIA", withBlock1.ToponimoViaECivico);
                }

                writer.Write("NumeroColli", m_NumeroColli);
                writer.Write("Peso", m_Peso);
                writer.Write("Altezza", m_Altezza);
                writer.Write("Larghezza", m_Larghezza);
                writer.Write("Profondita", m_Profondita);
                writer.Write("IDRichiestaDa", IDRichiestaDa);
                writer.Write("NomeRichiestaDa", m_NomeRichiestaDa);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("DescrizioneSpedizione", m_DescrizioneSpedizione);
                writer.Write("NotePerIlCorriere", m_NotePerIlCorriere);
                writer.Write("NotePerIlDestinatario", m_NotePerIlDestinatario);
                writer.Write("StatoOggetto", m_StatoOggetto);
                writer.Write("DettaglioStato", m_DettaglioStato);
                writer.Write("DataInizioSpedizione", m_DataInizioSpedizione);
                writer.Write("DataConsegna", m_DataConsegna);
                writer.Write("CategoriaContenuto", m_CategoriaContenuto);
                writer.Write("DescrizioneContenuto", m_DescrizioneContenuto);
                writer.Write("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.Write("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("IDConfermatoDa", IDConfermatoDa);
                writer.Write("NomeConfermatoDa", m_NomeConfermatoDa);
                writer.Write("DataConferma", m_DataConferma);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo sche,a
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("AspettoBeni", typeof(string), 255);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("IDDestinatario", typeof(int), 1);
                c = table.Fields.Ensure("NomeDestinatario", typeof(string), 255);
                c = table.Fields.Ensure("INDMITT_LABEL", typeof(string), 255);
                c = table.Fields.Ensure("INDMITT_CAP", typeof(string), 255);
                c = table.Fields.Ensure("INDMITT_CITTA", typeof(string), 255);
                c = table.Fields.Ensure("INDMITT_PROV", typeof(string), 255);
                c = table.Fields.Ensure("INDMITT_VIA", typeof(string), 255);

                c = table.Fields.Ensure("INDDEST_LABEL", typeof(string), 255);
                c = table.Fields.Ensure("INDDEST_CAP", typeof(string), 255);
                c = table.Fields.Ensure("INDDEST_CITTA", typeof(string), 255);
                c = table.Fields.Ensure("INDDEST_PROV", typeof(string), 255);
                c = table.Fields.Ensure("INDDEST_VIA", typeof(string), 255);
                c = table.Fields.Ensure("NumeroColli", typeof(int), 1);
                c = table.Fields.Ensure("Peso", typeof(double), 1);
                c = table.Fields.Ensure("Altezza", typeof(double), 1);
                c = table.Fields.Ensure("Larghezza", typeof(double), 1);
                c = table.Fields.Ensure("Profondita", typeof(double), 1);
                c = table.Fields.Ensure("IDRichiestaDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeRichiestaDa", typeof(string), 255);
                c = table.Fields.Ensure("DataRichiesta", typeof(DateTime), 1);
                c = table.Fields.Ensure("DescrizioneSpedizione", typeof(string), 255);
                c = table.Fields.Ensure("NotePerIlCorriere", typeof(string), 0);
                c = table.Fields.Ensure("NotePerIlDestinatario", typeof(string), 0);
                c = table.Fields.Ensure("StatoOggetto", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioStato", typeof(string), 0);
                c = table.Fields.Ensure("DataInizioSpedizione", typeof(string), 255);
                c = table.Fields.Ensure("DataConsegna", typeof(DateTime), 1);
                c = table.Fields.Ensure("CategoriaContenuto", typeof(string), 255);
                c = table.Fields.Ensure("DescrizioneContenuto", typeof(string), 0);
                c = table.Fields.Ensure("IDPresaInCaricoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomePresaInCaricoDa", typeof(string), 255);
                c = table.Fields.Ensure("DataPresaInCarico", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDConfermatoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeConfermatoDa", typeof(string), 255);
                c = table.Fields.Ensure("DataConferma", typeof(DateTime), 1);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxAspetto", new string[] { "AspettoBeni" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDestinatario", new string[] { "IDDestinatario", "NomeDestinatario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndMitt", new string[] { "INDMITT_LABEL", "INDMITT_CAP", "INDMITT_CITTA", "INDMITT_PROV", "INDMITT_VIA" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndDest", new string[] { "INDDEST_LABEL", "INDDEST_CAP", "INDDEST_CITTA", "INDDEST_PROV", "INDDEST_VIA" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStats", new string[] { "NumeroColli", "Peso", "Altezza", "Larghezza", "Profondita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRichiestoDa", new string[] { "IDRichiestaDa", "NomeRichiestaDa", "DataRichiesta"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrSped", new string[] { "DescrizioneSpedizione"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "NotePerIlCorriere", "NotePerIlDestinatario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoSped", new string[] { "StatoOggetto", "DettaglioStato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizioSpedizione", "DataConsegna" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "CategoriaContenuto", "DescrizioneContenuto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPresaInCarico", new string[] { "IDPresaInCaricoDa", "NomePresaInCaricoDa", "DataPresaInCarico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxConfermatoDa", new string[] { "IDConfermatoDa", "NomeConfermatoDa", "DataConferma" }, DBFieldConstraintFlags.None);
                 

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("AspettoBeni", m_AspettoBeni);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDDestinatario", IDDestinatario);
                writer.WriteAttribute("NomeDestinatario", m_NomeDestinatario);
                writer.WriteAttribute("NumeroColli", m_NumeroColli);
                writer.WriteAttribute("Peso", m_Peso);
                writer.WriteAttribute("Altezza", m_Altezza);
                writer.WriteAttribute("Larghezza", m_Larghezza);
                writer.WriteAttribute("Profondita", m_Profondita);
                writer.WriteAttribute("IDRichiestaDa", IDRichiestaDa);
                writer.WriteAttribute("NomeRichiestaDa", m_NomeRichiestaDa);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("StatoOggetto", (int?)m_StatoOggetto);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
                writer.WriteAttribute("DataInizioSpedizione", m_DataInizioSpedizione);
                writer.WriteAttribute("DataConsegna", m_DataConsegna);
                writer.WriteAttribute("CategoriaContenuto", m_CategoriaContenuto);
                writer.WriteAttribute("IDPresaInCaricoDa", IDPresaInCaricoDa);
                writer.WriteAttribute("NomePresaInCaricoDa", m_NomePresaInCaricoDa);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("IDConfermatoDa", IDConfermatoDa);
                writer.WriteAttribute("NomeConfermatoDa", m_NomeConfermatoDa);
                writer.WriteAttribute("DataConferma", m_DataConferma);
                base.XMLSerialize(writer);
                writer.WriteTag("INDMITT", m_IndirizzoMittente);
                writer.WriteTag("INDDEST", m_IndirizzoDestinatario);
                writer.WriteTag("DescrizioneContenuto", m_DescrizioneContenuto);
                writer.WriteTag("DescrizioneSpedizione", m_DescrizioneSpedizione);
                writer.WriteTag("NotePerIlCorriere", m_NotePerIlCorriere);
                writer.WriteTag("NotePerIlDestinatario", m_NotePerIlDestinatario);
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
                    case "AspettoBeni":
                        {
                            m_AspettoBeni = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDestinatario":
                        {
                            IDDestinatario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeDestinatario":
                        {
                            m_NomeDestinatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroColli":
                        {
                            m_NumeroColli = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Peso":
                        {
                            m_Peso = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Altezza":
                        {
                            m_Altezza = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Larghezza":
                        {
                            m_Larghezza = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Profondita":
                        {
                            m_Profondita = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDRichiestaDa":
                        {
                            m_IDRichiestaDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRichiestaDa":
                        {
                            m_NomeRichiestaDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoOggetto":
                        {
                            m_StatoOggetto = (StatoOggettoDaSpedire)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                 
                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizioSpedizione":
                        {
                            m_DataInizioSpedizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataConsegna":
                        {
                            m_DataConsegna = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "INDMITT":
                        {
                            m_IndirizzoMittente = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "INDDEST":
                        {
                            m_IndirizzoDestinatario = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "DescrizioneSpedizione":
                        {
                            m_DescrizioneSpedizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NotePerIlCorriere":
                        {
                            m_NotePerIlCorriere = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NotePerIlDestinatario":
                        {
                            m_NotePerIlDestinatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CategoriaContenuto":
                        {
                            m_CategoriaContenuto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DescrizioneContenuto":
                        {
                            m_DescrizioneContenuto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPresaInCaricoDa":
                        {
                            m_IDPresaInCaricoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePresaInCaricoDa":
                        {
                            m_NomePresaInCaricoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDConfermatoDa":
                        {
                            m_IDConfermatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConfermatoDa":
                        {
                            m_NomeConfermatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataConferma":
                        {
                            m_DataConferma = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public int CompareTo(OggettoDaSpedire b)
            {
                return DMD.DateUtils.Compare(m_DataInizioSpedizione, b.m_DataInizioSpedizione);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((OggettoDaSpedire)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_DescrizioneSpedizione;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DescrizioneSpedizione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is OggettoDaSpedire) && this.Equals((OggettoDaSpedire)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(OggettoDaSpedire obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_AspettoBeni, obj.m_AspettoBeni)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Integers.EQ(this.m_IDDestinatario, obj.m_IDDestinatario)
                    && DMD.Strings.EQ(this.m_NomeDestinatario, obj.m_NomeDestinatario)
                    && this.m_IndirizzoMittente.Equals(obj.m_IndirizzoMittente)
                    && this.m_IndirizzoDestinatario.Equals(obj.m_IndirizzoDestinatario)
                    && DMD.Integers.EQ(this.m_NumeroColli, obj.m_NumeroColli)
                    && DMD.Doubles.EQ(this.m_Peso, obj.m_Peso)
                    && DMD.Doubles.EQ(this.m_Altezza, obj.m_Altezza)
                    && DMD.Doubles.EQ(this.m_Larghezza, obj.m_Larghezza)
                    && DMD.Doubles.EQ(this.m_Profondita, obj.m_Profondita)
                    && DMD.Integers.EQ(this.m_IDRichiestaDa, obj.m_IDRichiestaDa)
                    && DMD.Strings.EQ(this.m_NomeRichiestaDa, obj.m_NomeRichiestaDa)
                    && DMD.DateUtils.EQ(this.m_DataRichiesta, obj.m_DataRichiesta)
                    && DMD.Integers.EQ(this.m_IDPresaInCaricoDa, obj.m_IDPresaInCaricoDa)
                    && DMD.Strings.EQ(this.m_NomePresaInCaricoDa, obj.m_NomePresaInCaricoDa)
                    && DMD.DateUtils.EQ(this.m_DataPresaInCarico, obj.m_DataPresaInCarico)
                    && DMD.Integers.EQ(this.m_IDConfermatoDa, obj.m_IDConfermatoDa)
                    && DMD.Strings.EQ(this.m_NomeConfermatoDa, obj.m_NomeConfermatoDa)
                    && DMD.DateUtils.EQ(this.m_DataConferma, obj.m_DataConferma)
                    && DMD.Strings.EQ(this.m_DescrizioneSpedizione, obj.m_DescrizioneSpedizione)
                    && DMD.Strings.EQ(this.m_NotePerIlCorriere, obj.m_NotePerIlCorriere)
                    && DMD.Strings.EQ(this.m_NotePerIlDestinatario, obj.m_NotePerIlDestinatario)
                    && DMD.RunTime.EQ(this.m_StatoOggetto, obj.m_StatoOggetto)
                    && DMD.Strings.EQ(this.m_DettaglioStato, obj.m_DettaglioStato)
                    && DMD.DateUtils.EQ(this.m_DataInizioSpedizione, obj.m_DataInizioSpedizione)
                    && DMD.DateUtils.EQ(this.m_DataConsegna, obj.m_DataConsegna)
                    && DMD.Strings.EQ(this.m_CategoriaContenuto, obj.m_CategoriaContenuto)
                    && DMD.Strings.EQ(this.m_DescrizioneContenuto, obj.m_DescrizioneContenuto)
                    && DMD.Integers.EQ(this.m_IDSpedizione, obj.m_IDSpedizione)
                    && DMD.Strings.EQ(this.m_NumeroSpedizione, obj.m_NumeroSpedizione)
                    && DMD.Integers.EQ(this.m_IDCorriere, obj.m_IDCorriere)
                    && DMD.Strings.EQ(this.m_NomeCorriere, obj.m_NomeCorriere)
                    ;
            }


        }
    }
}