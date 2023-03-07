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
    public partial class Finanziaria
    {


        /// <summary>
        /// Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CConteggioEstintivo 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<CConteggioEstintivo>
        {

            /// <summary>
            /// Evento generato quando la richiesta di conteggio estintivo viene segnalato
            /// </summary>
            public event ItemEventHandler<CConteggioEstintivo> Segnalata;

            /// <summary>
            /// Evento generato quando la richiesta di conteggio estintivo viene presa in carico
            /// </summary>
            public event ItemEventHandler<CConteggioEstintivo> PresaInCarico;
             

            [NonSerialized] private CPersona m_Cliente;           // Cliente per cui è stato richiesto il conteggio
            private int m_IDCliente;                          // ID del cliente per cui è stato chiesto il conteggio
            private string m_NomeCliente;                         // Nome del cliente per cui é stato chiesto il cliente
            private int m_IDEstinzione;   // ID del prestito per cui è stato richiesto il conteggio
            [NonSerialized] private CEstinzione m_Estinzione; // Prestito per cui è stato richiesto il conteggio
            private int m_IDRichiestaDiFinanziamento;
            [NonSerialized] private CRichiestaFinanziamento m_RichiestaDiFinanziamento;
            private int m_NumeroRichiesta; // Numero sequenziale della richiesta
            private int m_IDIstituto;                 // ID del cessionario
            [NonSerialized] private CAzienda m_Istituto;  // Cessionario
            private string m_NomeIstituto;                // Nome del cessionario
            private DateTime? m_DataRichiesta;    // Data della richiesta 
            private int m_IDAgenziaRichiedente;
            [NonSerialized] private CAzienda m_AgenziaRichiedente;
            private string m_NomeAgenziaRichiedente;
            private int m_IDAgente;
            [NonSerialized] private CPersonaFisica m_Agente;
            private string m_NomeAgente;
            private DateTime? m_DataEvasione;
            private int m_IDPratica;
            [NonSerialized] private CPraticaCQSPD m_Pratica;
            private string m_NumeroPratica;
            private string m_Descrizione;
            private int m_IDAllegato;                     // ID del modulo di richiesta
            [NonSerialized] private CAttachment m_Allegato;   // Modulo di richiesta
            private int m_IDDOCConteggio;                     // ID del documento di risposta
            [NonSerialized] private CAttachment m_DOCConteggio;                   // Documento di risposta
            private int m_PresaInCaricoDaID;
            private string m_PresaInCaricoDaNome;
            [NonSerialized] private CUser m_PresaInCaricoDa;
            private DateTime? m_DataPresaInCarico;
            private decimal? m_ImportoCE;
            private DateTime? m_DataSegnalazione;
            [NonSerialized] private CPersona m_InviatoDa;
            private int m_InviatoDaID;
            private string m_InviatoDaNome;
            private DateTime? m_InviatoIl;
            [NonSerialized] private CUser m_RicevutoDa;
            private int m_RicevutoDaID;
            private string m_RicevutoDaNome;
            private DateTime? m_RicevutoIl;
            private string m_MezzoDiInvio;
            private int m_IDFinestraLavorazione;
            [NonSerialized] private FinestraLavorazione m_FinestraLavorazione;
            private string m_Esito;
            private int m_IDCessionario;
            [NonSerialized] private CCQSPDCessionarioClass m_Cessionario;
            private string m_NomeCessionario;
            private int? m_DurataMesi;
            private decimal? m_ImportoRata;
            private double? m_TAN;
            private double? m_TAEG;
            private DateTime? m_DataDecorrenzaPratica;
            private DateTime? m_UltimaScadenza;
            private string m_ATCPIVA;
            private string m_ATCDescrizione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CConteggioEstintivo()
            {
                m_IDRichiestaDiFinanziamento = 0;
                m_RichiestaDiFinanziamento = null;
                m_DataRichiesta = default;
                m_IDIstituto = 0;
                m_Istituto = null;
                m_NomeIstituto = "";
                m_IDAgenziaRichiedente = 0;
                m_AgenziaRichiedente = null;
                m_NomeAgenziaRichiedente = "";
                m_IDAgente = 0;
                m_Agente = null;
                m_NomeAgente = "";
                m_DataEvasione = default;
                m_IDPratica = 0;
                m_Pratica = null;
                m_NumeroPratica = "";
                m_Descrizione = "";
                m_IDAllegato = 0;
                m_Allegato = null;
                m_IDDOCConteggio = 0;
                m_DOCConteggio = null;
                m_PresaInCaricoDa = null;
                m_PresaInCaricoDaID = 0;
                m_PresaInCaricoDaNome = "";
                m_DataPresaInCarico = default;
                m_Cliente = null;
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_ImportoCE = default;
                m_DataSegnalazione = default;
                m_InviatoDa = null;
                m_InviatoDaID = 0;
                m_InviatoDaNome = "";
                m_InviatoIl = default;
                m_RicevutoDa = null;
                m_RicevutoDaID = 0;
                m_RicevutoDaNome = "";
                m_RicevutoIl = default;
                m_MezzoDiInvio = "";
                m_IDFinestraLavorazione = 0;
                m_FinestraLavorazione = null;
                m_Esito = "";
                m_IDCessionario = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_DurataMesi = default;
                m_ImportoRata = default;
                m_TAN = default;
                m_TAEG = default;
                m_DataDecorrenzaPratica = default;
                m_UltimaScadenza = default;
                m_ATCPIVA = "";
                m_ATCDescrizione = "";
                m_IDEstinzione = 0;
                m_Estinzione = null;
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'oggetto da estinguere
            /// </summary>
            /// <returns></returns>
            public int IDEstinzione
            {
                get
                {
                    return DBUtils.GetID(m_Estinzione, m_IDEstinzione);
                }

                set
                {
                    int oldValue = IDEstinzione;
                    if (oldValue == value)
                        return;
                    m_IDEstinzione = value;
                    m_Estinzione = null;
                    DoChanged("IDEstinzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il prestito da estinguere
            /// </summary>
            /// <returns></returns>
            public CEstinzione Estinzione
            {
                get
                {
                    if (m_Estinzione is null)
                        m_Estinzione = Estinzioni.GetItemById(m_IDEstinzione);
                    return m_Estinzione;
                }

                set
                {
                    var oldValue = m_Estinzione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDEstinzione = DBUtils.GetID(value);
                    m_Estinzione = value;
                    DoChanged("Estinzione", value, oldValue);
                }
            }

            /// <summary>
            /// ID del cessionario
            /// </summary>
            /// <returns></returns>
            public int IDCessionario
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_IDCessionario);
                }

                set
                {
                    int oldValue = IDCessionario;
                    if (oldValue == value)
                        return;
                    m_IDCessionario = value;
                    m_Cessionario = null;
                    DoChanged("IDCessionario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il cessionario
            /// </summary>
            /// <returns></returns>
            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_IDCessionario);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cessionario = value;
                    m_IDCessionario = DBUtils.GetID(value);
                    m_NomeCessionario = "";
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del cessionario
            /// </summary>
            public string NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            /// <summary>
            /// Durata in mesi
            /// </summary>
            public int? DurataMesi
            {
                get
                {
                    return m_DurataMesi;
                }

                set
                {
                    var oldValue = m_DurataMesi;
                    if (oldValue == value == true)
                        return;
                    m_DurataMesi = value;
                    DoChanged("DurataMesi", value, oldValue);
                }
            }

            /// <summary>
            /// Importo della rata
            /// </summary>
            public decimal? ImportoRata
            {
                get
                {
                    return m_ImportoRata;
                }

                set
                {
                    var oldValue = m_ImportoRata;
                    if (oldValue == value == true)
                        return;
                    m_ImportoRata = value;
                    DoChanged("ImportoRata", value, oldValue);
                }
            }

            /// <summary>
            /// TAN
            /// </summary>
            public double? TAN
            {
                get
                {
                    return m_TAN;
                }

                set
                {
                    var oldValue = m_TAN;
                    if (oldValue == value == true)
                        return;
                    m_TAN = value;
                    DoChanged("TAN", value, oldValue);
                }
            }

            /// <summary>
            /// TAEG
            /// </summary>
            public double? TAEG
            {
                get
                {
                    return m_TAEG;
                }

                set
                {
                    var oldValue = m_TAEG;
                    if (oldValue == value == true)
                        return;
                    m_TAEG = value;
                    DoChanged("TAEG", value, oldValue);
                }
            }

            /// <summary>
            /// Data di decorrenza della pratica
            /// </summary>
            public DateTime? DataDecorrenzaPratica
            {
                get
                {
                    return m_DataDecorrenzaPratica;
                }

                set
                {
                    var oldValue = m_DataDecorrenzaPratica;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataDecorrenzaPratica = value;
                    DoChanged("DataDecorrenzaPratica", value, oldValue);
                }
            }

            /// <summary>
            /// Scadenza dell'ultima rata da pagare
            /// </summary>
            public DateTime? UltimaScadenza
            {
                get
                {
                    return m_UltimaScadenza;
                }

                set
                {
                    var oldValue = m_UltimaScadenza;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_UltimaScadenza = value;
                    DoChanged("UltimaScadenza", value, oldValue);
                }
            }

            /// <summary>
            /// Prtita IVA dell'ATC
            /// </summary>
            public string ATCPIVA
            {
                get
                {
                    return m_ATCPIVA;
                }

                set
                {
                    string oldValue = m_ATCPIVA;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_ATCPIVA = value;
                    DoChanged("ATCPIVA", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione dell'ATC
            /// </summary>
            public string ATCDescrizione
            {
                get
                {
                    return m_ATCDescrizione;
                }

                set
                {
                    string oldValue = m_ATCDescrizione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ATCDescrizione = value;
                    DoChanged("ATCDescrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive l'esito del conteggio
            /// </summary>
            /// <returns></returns>
            public string Esito
            {
                get
                {
                    return m_Esito;
                }

                set
                {
                    string oldValue = m_Esito;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Esito = value;
                    DoChanged("Esito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della finestra di lavorazione in cui è avvenuta la richiesta
            /// </summary>
            /// <returns></returns>
            public int IDFinestraLavorazione
            {
                get
                {
                    return DBUtils.GetID(m_FinestraLavorazione, m_IDFinestraLavorazione);
                }

                set
                {
                    int oldValue = IDFinestraLavorazione;
                    if (oldValue == value)
                        return;
                    m_IDFinestraLavorazione = value;
                    m_FinestraLavorazione = null;
                    DoChanged("IDFinestraLavorazione", value, oldValue);
                }
            }

            /// <summary>
            /// Finestra di lavorazione
            /// </summary>
            public FinestraLavorazione FinestraLavorazione
            {
                get
                {
                    if (m_FinestraLavorazione is null)
                        m_FinestraLavorazione = FinestreDiLavorazione.GetItemById(m_IDFinestraLavorazione);
                    return m_FinestraLavorazione;
                }

                set
                {
                    var oldValue = m_FinestraLavorazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_FinestraLavorazione = value;
                    m_IDFinestraLavorazione = DBUtils.GetID(value);
                    DoChanged("FinestraLavorazione", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la finestra di lavorazione
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetFinestraLavorazione(FinestraLavorazione value)
            {
                m_FinestraLavorazione = value;
                m_IDFinestraLavorazione = DBUtils.GetID(value);
            }


            /// <summary>
            /// Restituisce o imposta la persona che ha inviato il modulo di richiesta all'agenzia
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona InviatoDa
            {
                get
                {
                    if (m_InviatoDa is null)
                        m_InviatoDa = Anagrafica.Persone.GetItemById(m_InviatoDaID);
                    return m_InviatoDa;
                }

                set
                {
                    var oldValue = m_InviatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InviatoDa = value;
                    m_InviatoDaID = DBUtils.GetID(value);
                    if (value is object)
                        m_InviatoDaNome = value.Nominativo;
                    DoChanged("InviatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona che ha inviato il modulo all'agenzia
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int InviatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_InviatoDa, m_InviatoDaID);
                }

                set
                {
                    int oldValue = InviatoDaID;
                    if (oldValue == value)
                        return;
                    m_InviatoDa = null;
                    m_InviatoDaID = value;
                    DoChanged("InviatoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persoan che ha inviato il modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string InviatoDaNome
            {
                get
                {
                    return m_InviatoDaNome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_InviatoDaNome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_InviatoDaNome = value;
                    DoChanged("InviatoDaNome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di invio del modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? InviatoIl
            {
                get
                {
                    return m_InviatoIl;
                }

                set
                {
                    var oldValue = m_InviatoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_InviatoIl = value;
                    DoChanged("InviatoIl", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha registrato il modulo nel sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser RicevutoDa
            {
                get
                {
                    if (m_RicevutoDa is null)
                        m_RicevutoDa = Sistema.Users.GetItemById(m_RicevutoDaID);
                    return m_RicevutoDa;
                }

                set
                {
                    var oldValue = RicevutoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RicevutoDa = value;
                    m_RicevutoDaID = DBUtils.GetID(value);
                    if (value is object)
                        m_RicevutoDaNome = value.Nominativo;
                    DoChanged("RicevutoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha registrato il modulo nel sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int RicevutoDaID
            {
                get
                {
                    return DBUtils.GetID(m_RicevutoDa, m_RicevutoDaID);
                }

                set
                {
                    int oldValue = RicevutoDaID;
                    if (oldValue == value)
                        return;
                    m_RicevutoDa = null;
                    m_RicevutoDaID = value;
                    DoChanged("RicevutoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha registrato il modulo nel sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string RicevutoDaNome
            {
                get
                {
                    return m_RicevutoDaNome;
                }

                set
                {
                    string oldValue = m_RicevutoDaNome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RicevutoDaNome = value;
                    DoChanged("RicevutoDaNome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di registrazione del modulo nel sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? RicevutoIl
            {
                get
                {
                    return m_RicevutoIl;
                }

                set
                {
                    var oldValue = m_RicevutoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_RicevutoIl = value;
                    DoChanged("RicevutoIl", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mezzo utilizzato per inviare il modulo (da Prestitalia all'agenzia, es. e-mail)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string MezzoDiInvio
            {
                get
                {
                    return m_MezzoDiInvio;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MezzoDiInvio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MezzoDiInvio = value;
                    DoChanged("MezzoDiInvio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore da estinguere alla data di rilascio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? ImportoCE
            {
                get
                {
                    return m_ImportoCE;
                }

                set
                {
                    var oldValue = m_ImportoCE;
                    if (oldValue == value == true)
                        return;
                    m_ImportoCE = value;
                    DoChanged("ImportoCE", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona per cui viene fatta la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Cliente
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
                    m_IDCliente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il cliente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetCliente(Anagrafica.CPersona value)
            {
                m_Cliente = value;
                m_IDCliente = DBUtils.GetID(value);
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona per cui viene fatta la richiesta
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
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del cliente che ha effettuato la richiesta
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
                    string oldValue = m_NomeCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della pratica (interna) per cui è stato richiesto il conteggio estintivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPratica
            {
                get
                {
                    return DBUtils.GetID(m_Pratica, m_IDPratica);
                }

                set
                {
                    int oldValue = IDPratica;
                    if (oldValue == value)
                        return;
                    m_IDPratica = value;
                    m_Pratica = null;
                    DoChanged("IDPratica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la pratica per cui è stato chiesto il conteggio estintivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPraticaCQSPD Pratica
            {
                get
                {
                    if (m_Pratica is null)
                        m_Pratica = Pratiche.GetItemById(m_IDPratica);
                    return m_Pratica;
                }

                set
                {
                    var oldValue = m_Pratica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                    DoChanged("Pratica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero esterno della pratica per cui è stato richiesto il conteggio estintivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NumeroPratica
            {
                get
                {
                    return m_NumeroPratica;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroPratica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroPratica = value;
                    DoChanged("NumeroPratica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'allegato contenente il modulo di richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAllegato
            {
                get
                {
                    return DBUtils.GetID(m_Allegato, m_IDAllegato);
                }

                set
                {
                    int oldValue = IDAllegato;
                    if (oldValue == value)
                        return;
                    m_IDAllegato = value;
                    m_Allegato = null;
                    DoChanged("IDAllegato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'allegato contenente il modulo di richiesta del conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CAttachment Allegato
            {
                get
                {
                    if (m_Allegato is null)
                        m_Allegato = Sistema.Attachments.GetItemById(m_IDAllegato);
                    return m_Allegato;
                }

                set
                {
                    var oldValue = m_Allegato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Allegato = value;
                    m_IDAllegato = DBUtils.GetID(value);
                    DoChanged("Allegato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'documento contenente il conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDOCConteggio
            {
                get
                {
                    return DBUtils.GetID(m_DOCConteggio, m_IDDOCConteggio);
                }

                set
                {
                    int oldValue = IDDOCConteggio;
                    if (oldValue == value)
                        return;
                    m_IDDOCConteggio = value;
                    m_DOCConteggio = null;
                    DoChanged("IDDOCConteggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il documento contenente il conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CAttachment DOCConteggio
            {
                get
                {
                    if (m_DOCConteggio is null)
                        m_DOCConteggio = Sistema.Attachments.GetItemById(m_IDDOCConteggio);
                    return m_DOCConteggio;
                }

                set
                {
                    var oldValue = m_DOCConteggio;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DOCConteggio = value;
                    m_IDDOCConteggio = DBUtils.GetID(value);
                    DoChanged("DOCConteggio", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'utente che ha preso in carico la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser PresaInCaricoDa
            {
                get
                {
                    if (m_PresaInCaricoDa is null)
                        m_PresaInCaricoDa = Sistema.Users.GetItemById(m_PresaInCaricoDaID);
                    return m_PresaInCaricoDa;
                }

                set
                {
                    var oldValue = PresaInCaricoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PresaInCaricoDa = value;
                    m_PresaInCaricoDaID = DBUtils.GetID(value);
                    if (value is object)
                        m_PresaInCaricoDaNome = value.Nominativo;
                    DoChanged("PresaInCaricoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta L'ID dell'operatore che ha preso in carico la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int PresaInCaricoDaID
            {
                get
                {
                    return DBUtils.GetID(m_PresaInCaricoDa, m_PresaInCaricoDaID);
                }

                set
                {
                    int oldValue = PresaInCaricoDaID;
                    if (oldValue == value)
                        return;
                    m_PresaInCaricoDaID = value;
                    m_PresaInCaricoDa = null;
                    DoChanged("PresaInCaricoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha preso in carico la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string PresaInCaricoDaNome
            {
                get
                {
                    return m_PresaInCaricoDaNome;
                }

                set
                {
                    string oldValue = m_PresaInCaricoDaNome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PresaInCaricoDaNome = value;
                    DoChanged("PresaInCaricoDaNome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui la richiesta è stata presa in carico
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
            /// Restituisdce o imposta l'ID della richiesta di finanziamento in cui è stato registrato questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRichiestaDiFinanziamento
            {
                get
                {
                    return DBUtils.GetID(m_RichiestaDiFinanziamento, m_IDRichiestaDiFinanziamento);
                }

                set
                {
                    int oldValue = IDRichiestaDiFinanziamento;
                    if (oldValue == value)
                        return;
                    m_IDRichiestaDiFinanziamento = value;
                    m_RichiestaDiFinanziamento = null;
                    DoChanged("IDRichiestaDiFinanziamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la richiesta di finanziamento in cui è stato registrato questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CRichiestaFinanziamento RichiestaDiFinanziamento
            {
                get
                {
                    if (m_RichiestaDiFinanziamento is null)
                        m_RichiestaDiFinanziamento = RichiesteFinanziamento.GetItemById(m_IDRichiestaDiFinanziamento);
                    return m_RichiestaDiFinanziamento;
                }

                set
                {
                    var oldValue = m_RichiestaDiFinanziamento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RichiestaDiFinanziamento = value;
                    m_IDRichiestaDiFinanziamento = DBUtils.GetID(value);
                    DoChanged("RichiestaDiFinanziamento", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la richiesta di finanziamento
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetRichiestaDiFinanziamento(CRichiestaFinanziamento value)
            {
                m_RichiestaDiFinanziamento = value;
                m_IDRichiestaDiFinanziamento = DBUtils.GetID(value);
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora della richiesta
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
                    if (oldValue == value == true)
                        return;
                    m_DataRichiesta = value;
                    DoChanged("DataRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta L'ID dell'istituto presso cui è stata effettuata la richiesta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDIstituto
            {
                get
                {
                    return DBUtils.GetID(m_Istituto, m_IDIstituto);
                }

                set
                {
                    int oldValue = IDIstituto;
                    if (oldValue == value)
                        return;
                    m_IDIstituto = value;
                    m_Istituto = null;
                    DoChanged("IDIstituto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'istituto presso cui è stato richiesto questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda Istituto
            {
                get
                {
                    if (m_Istituto is null)
                        m_Istituto = (Anagrafica.CAzienda)Anagrafica.Persone.GetItemById(m_IDIstituto);
                    return m_Istituto;
                }

                set
                {
                    var oldValue = m_Istituto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Istituto = value;
                    m_IDIstituto = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeIstituto = value.Nominativo;
                    DoChanged("Istituto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'istituto presso cui è stato richiesto questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeIstituto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIstituto = value;
                    DoChanged("NomeIstituto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'agenzia che ha richiesto il conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAgenziaRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_AgenziaRichiedente, m_IDAgenziaRichiedente);
                }

                set
                {
                    int oldValue = IDAgenziaRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDAgenziaRichiedente = value;
                    m_AgenziaRichiedente = null;
                    DoChanged("IDAgenziaRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusce o imposta l'agenzia che ha richiesto questo conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAzienda AgenziaRichiedente
            {
                get
                {
                    if (m_AgenziaRichiedente is null)
                        m_AgenziaRichiedente = Anagrafica.Aziende.GetItemById(m_IDAgenziaRichiedente);
                    return m_AgenziaRichiedente;
                }

                set
                {
                    var oldValue = m_AgenziaRichiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AgenziaRichiedente = value;
                    m_IDAgenziaRichiedente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAgenziaRichiedente = value.Nominativo;
                    DoChanged("AgenziaRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'agenzia richiedente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAgenziaRichiedente
            {
                get
                {
                    return m_NomeAgenziaRichiedente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgenziaRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgenziaRichiedente = value;
                    DoChanged("NomeAgenziaRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'agente che ha richiesto il conteggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAgente
            {
                get
                {
                    return DBUtils.GetID(m_Agente, m_IDAgente);
                }

                set
                {
                    int oldValue = IDAgente;
                    if (oldValue == value)
                        return;
                    m_IDAgente = value;
                    m_Agente = null;
                    DoChanged("IDAgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersonaFisica Agente
            {
                get
                {
                    if (m_Agente is null)
                        m_Agente = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(m_IDAgente);
                    return m_Agente;
                }

                set
                {
                    var oldValue = m_Agente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Agente = value;
                    m_IDAgente = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeAgente = value.Nominativo;
                    DoChanged("Agente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'agente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAgente
            {
                get
                {
                    return m_NomeAgente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAgente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAgente = value;
                    DoChanged("NomeAgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di evasione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataEvasione
            {
                get
                {
                    return m_DataEvasione;
                }

                set
                {
                    var oldValue = m_DataEvasione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataEvasione = value;
                    DoChanged("DataEvasione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui questo oggetto è stato segnalato agli operatori
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataSegnalazione
            {
                get
                {
                    return m_DataSegnalazione;
                }

                set
                {
                    var oldValue = m_DataSegnalazione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataSegnalazione = value;
                    DoChanged("DataSegnalazione", value, oldValue);
                }
            }

            /// <summary>
            /// Segnala la ricezione della richiesta di conteggio estintivo
            /// </summary>
            public void Segnala()
            {
                m_DataSegnalazione = DMD.DateUtils.Now();
                Save(true);
                var e = new ItemEventArgs<CRichiestaConteggio>(this);
                this.OnSegnalata(e);
            }

            /// <summary>
            /// Genera l'evento Segnalata
            /// </summary>
            /// <param name="e"></param>             
            protected virtual void OnSegnalata(ItemEventArgs<CRichiestaConteggio> e)
            {
                Segnalata?.Invoke(this, e);
                ConteggiEstintivi.doOnSegnalata(e);
            }

            /// <summary>
            /// Prende in carico la richiesta
            /// </summary>
            public void PrendiInCarico()
            {
                PresaInCaricoDa = Sistema.Users.CurrentUser;
                DataPresaInCarico = DMD.DateUtils.Now();
                Save(true);
                var e = new ItemEventArgs<CRichiestaConteggio>(this);
                this.OnPresaInCarico(e);
            }
             
            /// <summary>
            /// Genera l'evento PresaInCarico
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnPresaInCarico(ItemEventArgs<CRichiestaConteggio> e)
            {
                PresaInCarico?.Invoke(this, e);
                ConteggiEstintivi.doOnPresaInCarico(e);
            }

          
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.ConteggiEstintivi;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ConteggiEstintivi";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaF",  m_IDRichiestaDiFinanziamento);
                m_DataRichiesta = reader.Read("DataRichiesta",  m_DataRichiesta);
                m_IDIstituto = reader.Read("IDIstituto",  m_IDIstituto);
                m_NomeIstituto = reader.Read("NomeIstituto",  m_NomeIstituto);
                m_IDAgenziaRichiedente = reader.Read("IDAgenziaR",  m_IDAgenziaRichiedente);
                m_NomeAgenziaRichiedente = reader.Read("NomeAgenziaR",  m_NomeAgenziaRichiedente);
                m_IDAgente = reader.Read("IDAgente",  m_IDAgente);
                m_NomeAgente = reader.Read("NomeAgente",  m_NomeAgente);
                m_DataEvasione = reader.Read("DataEvasione",  m_DataEvasione);
                m_ImportoCE = reader.Read("ImportoCE",  m_ImportoCE);
                m_IDPratica = reader.Read("IDPratica",  m_IDPratica);
                m_NumeroPratica = reader.Read("NumeroPratica",  m_NumeroPratica);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_IDAllegato = reader.Read("IDAllegato",  m_IDAllegato);
                m_IDDOCConteggio = reader.Read("IDDOCConteggio",  m_IDDOCConteggio);
                m_PresaInCaricoDaID = reader.Read("PresaInCaricoDaID",  m_PresaInCaricoDaID);
                m_PresaInCaricoDaNome = reader.Read("PresaInCaricoDaNome",  m_PresaInCaricoDaNome);
                m_DataPresaInCarico = reader.Read("DataPresaInCarico",  m_DataPresaInCarico);
                m_IDCliente = reader.Read("IDCliente",  m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente",  m_NomeCliente);
                m_DataSegnalazione = reader.Read("DataSegnalazione",  m_DataSegnalazione);
                m_InviatoDaID = reader.Read("InviatoDaID",  m_InviatoDaID);
                m_InviatoDaNome = reader.Read("InviatoDaNome",  m_InviatoDaNome);
                m_InviatoIl = reader.Read("InviatoIl",  m_InviatoIl);
                m_RicevutoDaID = reader.Read("RicevutoDaID",  m_RicevutoDaID);
                m_RicevutoDaNome = reader.Read("RicevutoDaNome",  m_RicevutoDaNome);
                m_RicevutoIl = reader.Read("RicevutoIl",  m_RicevutoIl);
                m_MezzoDiInvio = reader.Read("MezzoDiInvio",  m_MezzoDiInvio);
                m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione",  m_IDFinestraLavorazione);
                m_Esito = reader.Read("Esito",  m_Esito);
                m_IDCessionario = reader.Read("IDCessionario",  m_IDCessionario);
                m_NomeCessionario = reader.Read("NomeCessionario",  m_NomeCessionario);
                m_DurataMesi = reader.Read("DurataMesi",  m_DurataMesi);
                m_ImportoRata = reader.Read("ImportoRata",  m_ImportoRata);
                m_TAN = reader.Read("TAN",  m_TAN);
                m_TAEG = reader.Read("TAEG",  m_TAEG);
                m_DataDecorrenzaPratica = reader.Read("DataDecorrenzaPratica",  m_DataDecorrenzaPratica);
                m_UltimaScadenza = reader.Read("UltimaScadenza",  m_UltimaScadenza);
                m_ATCPIVA = reader.Read("ATCPIVA",  m_ATCPIVA);
                m_ATCDescrizione = reader.Read("ATCDescrizione",  m_ATCDescrizione);
                m_IDEstinzione = reader.Read("IDEstinzione",  m_IDEstinzione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDRichiestaF", typeof(int), 1);
                c = table.Fields.Ensure("DataRichiesta", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDIstituto", typeof(int), 1);
                c = table.Fields.Ensure("NomeIstituto", typeof(string), 255);
                c = table.Fields.Ensure("IDAgenziaR", typeof(int), 1);
                c = table.Fields.Ensure("NomeAgenziaR", typeof(string), 255);
                c = table.Fields.Ensure("IDAgente", typeof(int), 1);
                c = table.Fields.Ensure("NomeAgente", typeof(string), 255);
                c = table.Fields.Ensure("DataEvasione", typeof(DateTime), 1);
                c = table.Fields.Ensure("ImportoCE", typeof(decimal), 1);
                c = table.Fields.Ensure("IDPratica", typeof(int), 1);
                c = table.Fields.Ensure("NumeroPratica", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("IDAllegato", typeof(int), 1);
                c = table.Fields.Ensure("IDDOCConteggio", typeof(int), 1);
                c = table.Fields.Ensure("PresaInCaricoDaID", typeof(int), 1);
                c = table.Fields.Ensure("PresaInCaricoDaNome", typeof(string), 255);
                c = table.Fields.Ensure("DataPresaInCarico", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("DataSegnalazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("InviatoDaID", typeof(int), 1);
                c = table.Fields.Ensure("InviatoDaNome", typeof(string), 255);
                c = table.Fields.Ensure("InviatoIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("RicevutoDaID", typeof(int), 1);
                c = table.Fields.Ensure("RicevutoDaNome", typeof(string), 255);
                c = table.Fields.Ensure("RicevutoIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("MezzoDiInvio", typeof(string), 255);
                c = table.Fields.Ensure("IDFinestraLavorazione", typeof(int), 1);
                c = table.Fields.Ensure("Esito", typeof(string), 255);
                c = table.Fields.Ensure("IDCessionario", typeof(int), 1);
                c = table.Fields.Ensure("NomeCessionario", typeof(string), 255);
                c = table.Fields.Ensure("DurataMesi", typeof(int), 1);
                c = table.Fields.Ensure("ImportoRata", typeof(decimal), 1);
                c = table.Fields.Ensure("TAN", typeof(double), 1);
                c = table.Fields.Ensure("TAEG", typeof(double), 1);
                c = table.Fields.Ensure("DataDecorrenzaPratica", typeof(DateTime), 1);
                c = table.Fields.Ensure("UltimaScadenza", typeof(DateTime), 1);
                c = table.Fields.Ensure("ATCPIVA", typeof(string), 255);
                c = table.Fields.Ensure("ATCDescrizione", typeof(string), 0);
                c = table.Fields.Ensure("IDEstinzione", typeof(int), 1);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxRichiesta", new string[] { "IDRichiestaF", "DataRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIstituto", new string[] { "IDIstituto", "NomeIstituto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAgenzia", new string[] { "IDAgenziaR", "NomeAgenziaR" , "IDAgente", "NomeAgente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPratica", new string[] { "IDPratica", "NumeroPratica", "ImportoCE" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataEvasione", "DataSegnalazione"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Esito", "Descrizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAllegato", new string[] { "IDAllegato" , "IDDOCConteggio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPresaInCarico", new string[] { "PresaInCaricoDaID", "PresaInCaricoDaNome", "DataPresaInCarico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInviato", new string[] { "InviatoDaID", "InviatoDaNome", "InviatoIl" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRicevuto", new string[] { "RicevutoDaID", "RicevutoDaNome", "RicevutoIl" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMezzo", new string[] { "MezzoDiInvio"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFinestra", new string[] { "IDFinestraLavorazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCessionario", new string[] { "IDCessionario", "NomeCessionario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri1", new string[] { "DurataMesi", "ImportoRata", "TAN", "TAEG" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri2", new string[] { "DataDecorrenzaPratica", "UltimaScadenza"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEstinzione", new string[] { "IDEstinzione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxATC", new string[] { "ATCPIVA", "ATCDescrizione" }, DBFieldConstraintFlags.None);
                


            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDRichiestaF", IDRichiestaDiFinanziamento);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("IDIstituto", IDIstituto);
                writer.Write("NomeIstituto", m_NomeIstituto);
                writer.Write("IDAgenziaR", IDAgenziaRichiedente);
                writer.Write("NomeAgenziaR", m_NomeAgenziaRichiedente);
                writer.Write("IDAgente", IDAgente);
                writer.Write("NomeAgente", m_NomeAgente);
                writer.Write("DataEvasione", m_DataEvasione);
                writer.Write("ImportoCE", m_ImportoCE);
                writer.Write("IDPratica", IDPratica);
                writer.Write("NumeroPratica", m_NumeroPratica);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDAllegato", IDAllegato);
                writer.Write("IDDOCConteggio", IDDOCConteggio);
                writer.Write("PresaInCaricoDaID", PresaInCaricoDaID);
                writer.Write("PresaInCaricoDaNome", m_PresaInCaricoDaNome);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("DataSegnalazione", m_DataSegnalazione);
                writer.Write("InviatoDaID", InviatoDaID);
                writer.Write("InviatoDaNome", m_InviatoDaNome);
                writer.Write("InviatoIl", m_InviatoIl);
                writer.Write("RicevutoDaID", RicevutoDaID);
                writer.Write("RicevutoDaNome", m_RicevutoDaNome);
                writer.Write("RicevutoIl", m_RicevutoIl);
                writer.Write("MezzoDiInvio", m_MezzoDiInvio);
                writer.Write("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.Write("Esito", m_Esito);
                writer.Write("IDCessionario", IDCessionario);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("DurataMesi", m_DurataMesi);
                writer.Write("ImportoRata", m_ImportoRata);
                writer.Write("TAN", m_TAN);
                writer.Write("TAEG", m_TAEG);
                writer.Write("DataDecorrenzaPratica", m_DataDecorrenzaPratica);
                writer.Write("UltimaScadenza", m_UltimaScadenza);
                writer.Write("ATCPIVA", m_ATCPIVA);
                writer.Write("ATCDescrizione", m_ATCDescrizione);
                writer.Write("IDEstinzione", IDEstinzione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDRichiestaF", IDRichiestaDiFinanziamento);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("IDIstituto", IDIstituto);
                writer.WriteAttribute("NomeIstituto", m_NomeIstituto);
                writer.WriteAttribute("IDAgenziaR", IDAgenziaRichiedente);
                writer.WriteAttribute("NomeAgenziaR", m_NomeAgenziaRichiedente);
                writer.WriteAttribute("IDAgente", IDAgente);
                writer.WriteAttribute("NomeAgente", m_NomeAgente);
                writer.WriteAttribute("DataEvasione", m_DataEvasione);
                writer.WriteAttribute("ImportoCE", m_ImportoCE);
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("NumeroPratica", m_NumeroPratica);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("IDAllegato", IDAllegato);
                writer.WriteAttribute("PresaInCaricoDaID", PresaInCaricoDaID);
                writer.WriteAttribute("PresaInCaricoDaNome", m_PresaInCaricoDaNome);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("DataSegnalazione", m_DataSegnalazione);
                writer.WriteAttribute("InviatoDaID", InviatoDaID);
                writer.WriteAttribute("InviatoDaNome", m_InviatoDaNome);
                writer.WriteAttribute("InviatoIl", m_InviatoIl);
                writer.WriteAttribute("RicevutoDaID", RicevutoDaID);
                writer.WriteAttribute("RicevutoDaNome", m_RicevutoDaNome);
                writer.WriteAttribute("RicevutoIl", m_RicevutoIl);
                writer.WriteAttribute("MezzoDiInvio", m_MezzoDiInvio);
                writer.WriteAttribute("IDFinestraLavorazione", IDFinestraLavorazione);
                writer.WriteAttribute("Esito", m_Esito);
                writer.WriteAttribute("IDCessionario", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("DurataMesi", m_DurataMesi);
                writer.WriteAttribute("ImportoRata", m_ImportoRata);
                writer.WriteAttribute("TAN", m_TAN);
                writer.WriteAttribute("TAEG", m_TAEG);
                writer.WriteAttribute("DataDecorrenzaPratica", m_DataDecorrenzaPratica);
                writer.WriteAttribute("UltimaScadenza", m_UltimaScadenza);
                writer.WriteAttribute("ATCPIVA", m_ATCPIVA);
                writer.WriteAttribute("ATCDescrizione", m_ATCDescrizione);
                writer.WriteAttribute("IDEstinzione", IDEstinzione);
                writer.WriteAttribute("IDDOCConteggio", IDDOCConteggio);
                base.XMLSerialize(writer);
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
                    case "IDRichiestaF":
                        {
                            m_IDRichiestaDiFinanziamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDIstituto":
                        {
                            m_IDIstituto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeIstituto":
                        {
                            m_NomeIstituto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgenziaR":
                        {
                            m_IDAgenziaRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgenziaR":
                        {
                            m_NomeAgenziaRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAgente":
                        {
                            m_IDAgente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAgente":
                        {
                            m_NomeAgente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataEvasione":
                        {
                            m_DataEvasione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroPratica":
                        {
                            m_NumeroPratica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAllegato":
                        {
                            m_IDAllegato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PresaInCaricoDaID":
                        {
                            m_PresaInCaricoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PresaInCaricoDaNome":
                        {
                            m_PresaInCaricoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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

                    case "ImportoCE":
                        {
                            m_ImportoCE = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataSegnalazione":
                        {
                            m_DataSegnalazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "InviatoDaID":
                        {
                            m_InviatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "InviatoDaNome":
                        {
                            m_InviatoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "InviatoIl":
                        {
                            m_InviatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RicevutoDaID":
                        {
                            m_RicevutoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RicevutoDaNome":
                        {
                            m_RicevutoDaNome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RicevutoIl":
                        {
                            m_RicevutoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MezzoDiInvio":
                        {
                            m_MezzoDiInvio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFinestraLavorazione":
                        {
                            m_IDFinestraLavorazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            m_Esito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCessionario":
                        {
                            m_IDCessionario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DurataMesi":
                        {
                            m_DurataMesi = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ImportoRata":
                        {
                            m_ImportoRata = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAN":
                        {
                            m_TAN = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            m_TAEG = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataDecorrenzaPratica":
                        {
                            m_DataDecorrenzaPratica = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "UltimaScadenza":
                        {
                            m_UltimaScadenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ATCPIVA":
                        {
                            m_ATCPIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ATCDescrizione":
                        {
                            m_ATCDescrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDEstinzione":
                        {
                            m_IDEstinzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDOCConteggio":
                        {
                            m_IDDOCConteggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Conteggi Estintivi: " , m_NomeCliente , ", Pratica: " , m_NumeroPratica);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DataRichiesta);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return  (obj is CConteggioEstintivo) && this.Equals((CConteggioEstintivo) obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CConteggioEstintivo obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Integers.EQ(this.m_IDEstinzione, obj.m_IDEstinzione)
                    && DMD.Integers.EQ(this.m_IDRichiestaDiFinanziamento, obj.m_IDRichiestaDiFinanziamento)
                    && DMD.Integers.EQ(this.m_NumeroRichiesta, obj.m_NumeroRichiesta)
                    && DMD.Integers.EQ(this.m_IDIstituto, obj.m_IDIstituto)
                    && DMD.Strings.EQ(this.m_NomeIstituto, obj.m_NomeIstituto)
                    && DMD.DateUtils.EQ(this.m_DataRichiesta, obj.m_DataRichiesta)
                    && DMD.Integers.EQ(this.m_IDAgenziaRichiedente, obj.m_IDAgenziaRichiedente)
                    && DMD.Strings.EQ(this.m_NomeAgenziaRichiedente, obj.m_NomeAgenziaRichiedente)
                    && DMD.Integers.EQ(this.m_IDAgente, obj.m_IDAgente)
                    && DMD.Strings.EQ(this.m_NomeAgente, obj.m_NomeAgente)
                    && DMD.DateUtils.EQ(this.m_DataEvasione, obj.m_DataEvasione)
                    && DMD.Integers.EQ(this.m_IDPratica, obj.m_IDPratica)
                    && DMD.Strings.EQ(this.m_NumeroPratica, obj.m_NumeroPratica)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Integers.EQ(this.m_IDAllegato, obj.m_IDAllegato)
                    && DMD.Integers.EQ(this.m_IDDOCConteggio, obj.m_IDDOCConteggio)
                    && DMD.Integers.EQ(this.m_PresaInCaricoDaID, obj.m_PresaInCaricoDaID)
                    && DMD.Strings.EQ(this.m_PresaInCaricoDaNome, obj.m_PresaInCaricoDaNome)
                    && DMD.DateUtils.EQ(this.m_DataPresaInCarico, obj.m_DataPresaInCarico)
                    && DMD.Decimals.EQ(this.m_ImportoCE, obj.m_ImportoCE)
                    && DMD.DateUtils.EQ(this.m_DataSegnalazione, obj.m_DataSegnalazione)
                    && DMD.Integers.EQ(this.m_InviatoDaID, obj.m_InviatoDaID)
                    && DMD.Strings.EQ(this.m_InviatoDaNome, obj.m_InviatoDaNome)
                    && DMD.DateUtils.EQ(this.m_InviatoIl, obj.m_InviatoIl)
                    && DMD.Integers.EQ(this.m_RicevutoDaID, obj.m_RicevutoDaID)
                    && DMD.Strings.EQ(this.m_RicevutoDaNome, obj.m_RicevutoDaNome)
                    && DMD.DateUtils.EQ(this.m_RicevutoIl, obj.m_RicevutoIl)
                    && DMD.Strings.EQ(this.m_MezzoDiInvio, obj.m_MezzoDiInvio)
                    && DMD.Integers.EQ(this.m_IDFinestraLavorazione, obj.m_IDFinestraLavorazione)
                    && DMD.Strings.EQ(this.m_Esito, obj.m_Esito)
                    && DMD.Integers.EQ(this.m_IDCessionario, obj.m_IDCessionario)
                    && DMD.Strings.EQ(this.m_NomeCessionario, obj.m_NomeCessionario)
                    && DMD.Integers.EQ(this.m_DurataMesi, obj.m_DurataMesi)
                    && DMD.Decimals.EQ(this.m_ImportoRata, obj.m_ImportoRata)
                    && DMD.Doubles.EQ(this.m_TAN, obj.m_TAN)
                    && DMD.Doubles.EQ(this.m_TAEG, obj.m_TAEG)
                    && DMD.DateUtils.EQ(this.m_DataDecorrenzaPratica, obj.m_DataDecorrenzaPratica)
                    && DMD.DateUtils.EQ(this.m_UltimaScadenza, obj.m_UltimaScadenza)
                    && DMD.Strings.EQ(this.m_ATCPIVA, obj.m_ATCPIVA)
                    && DMD.Strings.EQ(this.m_ATCDescrizione, obj.m_ATCDescrizione)
                    ;
            }
        }
    }
}