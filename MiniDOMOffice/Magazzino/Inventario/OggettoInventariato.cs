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
using static minidom.Office;

namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Stati di un oggetto inventariato
        /// </summary>
        public enum StatoOggettoInventariato : int
        {
            /// <summary>
            /// Sconosciuto
            /// </summary>
            Sconosciuto = 0,

            /// <summary>
            /// Oggetto funzionante
            /// </summary>
            Funzionante = 1,

            /// <summary>
            /// Oggetto funzionante ma con qualche difetto
            /// </summary>
            ConQualcheDifetto = 2,

            /// <summary>
            /// Oggetto da riparare
            /// </summary>
            DaRiparare = 3,

            /// <summary>
            /// Oggetto di cui valutare la riparazione
            /// </summary>
            ValutazioneRiparazione = 4,

            /// <summary>
            /// Oggetto non riparabile
            /// </summary>
            NonRiparabile = 5,

            /// <summary>
            /// Oggetto non più a disposizione
            /// </summary>
            Dismesso = 10
        }

        /// <summary>
        /// Stato di acquisto dell'oggetto
        /// </summary>
        public enum StatoAcquistoOggettoInventariato : int
        {
            /// <summary>
            /// Sconosciuto
            /// </summary>
            Sconosciuto = 0,

            /// <summary>
            /// Oggetto nuovo acuistato 
            /// </summary>
            Nuovo = 1,

            /// <summary>
            /// Oggetto acquistato usato
            /// </summary>
            Usato = 2,

            /// <summary>
            /// Oggetto acquistato ricondizionato
            /// </summary>
            Ricondizionato = 3,

            /// <summary>
            /// Oggetto acquistato in pessime condizioni ma funzionanti
            /// </summary>
            Usurato = 4,

            /// <summary>
            /// Oggetto acquistato guasto ma riparabile
            /// </summary>
            GuastoRiparabile = 5,

            /// <summary>
            /// Oggetto acquistato guasto e non riparabile
            /// </summary>
            GuastoNonRiparabile = 6
        }

        /// <summary>
        /// Flags di un oggetto inventariato
        /// </summary>
        [Flags]
        public enum FlagsOggettoInventariato : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Indica che l'oggetto è in uso
            /// </summary>
            /// <remarks></remarks>
            InUso = 1
        }


        /// <summary>
        /// Rappresenta un oggetto inventariato
        /// </summary>
        [Serializable]
        public class OggettoInventariato
            : minidom.Databases.DBObjectPO
        {
            private string m_Codice;          // Codice di inventario
            private int m_IDArticolo;     // ID dell'articolo associato
            [NonSerialized] private Articolo m_Articolo;      // Articolo associato
            private string m_NomeArticolo;            // Nome dell'articolo
            private string m_Nome;            // Nome che identifica questo oggetto
            private string m_Categoria;       // Categoria dell'articolo
            private string m_Marca;           // Marca dell'articolo
            private string m_Modello;         // Modello dell'articolo
            private string m_Seriale;         // Restituisce il numero seriale dell'articolo 
            private string m_Descrizione;     // Descrizione dell'articolo
            private string m_IconURL;         // URL dell'immagine predefinita associata all'articolo
            private StatoOggettoInventariato m_StatoAttuale;
            private string m_DettaglioStatoAttuale;
            private decimal? m_ValoreStimato;
            private DateTime? m_DataValutazione;
            private string m_TipoInUsoDa;
            private int m_IDInUsoDa;
            private object m_InUsoDa;
            private string m_NomeInUsoDa;
            private DateTime? m_DataProduzione;       // Data di produzione di questo oggetto
            private DateTime? m_DataAcquisto;     // Data di acquisto
            private string m_TipoDocumentoAcquisto;   // Tipo del documento che attesta l'acquisto
            private string m_NumeroDocumentoAcquisto; // Numero del documento che attesta l'acquisto
            private StatoAcquistoOggettoInventariato m_StatoAcquisto;
            private string m_DettaglioStatoAcquisto;
            private int m_AcquistatoDaID;
            [NonSerialized] private Sistema.CUser m_AcquistatoDa;
            private string m_NomeAcquistatoDa;
            private decimal? m_PrezzoAcquisto;
            private double? m_AliquotaIVA;
            private int m_IDUfficioOriginale;     // ID dell'ufficio per cui è stato originariamente acquistato l'articolo
            [NonSerialized] private Anagrafica.CUfficio m_UfficioOriginale;      // Ufficio per cui è stato originariamente acquistato l'articolo
            private string m_NomeUfficioOriginale;    // Nome dell'ufficio per cui è stato originariamente acquistato l'articolo
            private string m_CodiceScaffale;          // Codice dello scaffale
            private string m_CodiceReparto;           // Codice del reparto
            private DateTime? m_DataDismissione;          // Data e ora di dismissione
            private int m_DismessoDaID;           // Utente che ha dismesso l'articolo
            [NonSerialized] private Sistema.CUser m_DismessoDa;               // Utente che ha dismesso l'articolo
            private string m_NomeDismessoDa;          // Nome dell'utente che ha dismesso l'articolo
            private string m_MotivoDismissione;       // Motivo dismissione
            private string m_DettaglioDismissione;    // Dettaglio Dismissione
            private decimal? m_ValoreDismissione;
            private double? m_AliquotaIVADismissione;
            private AttributiOggettoCollection m_Attributi;
            private OggettiCollegatiCollection m_Relazioni;
            //private FlagsOggettoInventariato m_Flags;
            private int m_IDOrdineAcquisto;
            private DocumentoContabile m_OrdineAcquisto;
            private int m_IDDocumentoAcquisto;
            private DocumentoContabile m_DocumentoAcquisto;
            private int m_IDSpedizione;
            private Spedizione m_Spedizione;
            private string m_CodiceRFID;
            private GPSRecord m_PosizioneGPS;
            private bool m_IsPosizioneGPSRelativa;

            // Private m_CodiceInterno As String

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettoInventariato()
            {
                m_Codice = "";
                m_Nome = "";
                m_NomeArticolo = "";
                m_Categoria = "";
                m_Marca = "";
                m_Modello = "";
                m_Seriale = "";
                m_Descrizione = "";
                m_IconURL = "";
                m_StatoAttuale = StatoOggettoInventariato.Sconosciuto;
                m_DettaglioStatoAttuale = "";
                m_ValoreStimato = default;
                m_DataValutazione = default;
                m_TipoInUsoDa = "";
                m_IDInUsoDa = 0;
                m_InUsoDa = null;
                m_NomeInUsoDa = "";
                m_DataProduzione = default;
                m_DataAcquisto = default;
                m_TipoDocumentoAcquisto = "";
                m_NumeroDocumentoAcquisto = "";
                m_StatoAcquisto = StatoAcquistoOggettoInventariato.Sconosciuto;
                m_DettaglioStatoAcquisto = "";
                m_AcquistatoDaID = 0;
                m_AcquistatoDa = null;
                m_NomeAcquistatoDa = "";
                m_PrezzoAcquisto = default;
                m_AliquotaIVA = default;
                m_IDUfficioOriginale = 0;
                m_UfficioOriginale = null;
                m_NomeUfficioOriginale = "";
                m_CodiceReparto = "";
                m_CodiceScaffale = "";
                m_DataDismissione = default;
                m_DismessoDaID = 0;
                m_DismessoDa = null;
                m_NomeDismessoDa = "";
                m_MotivoDismissione = "";
                m_DettaglioDismissione = "";
                m_ValoreDismissione = default;
                m_AliquotaIVADismissione = default;
                m_Attributi = null;
                m_Relazioni = null;
                m_Flags = (int)FlagsOggettoInventariato.None;
                m_IDOrdineAcquisto = 0;
                m_OrdineAcquisto = null;
                m_IDDocumentoAcquisto = 0;
                m_DocumentoAcquisto = null;
                m_IDSpedizione = 0;
                m_Spedizione = null;
                m_CodiceRFID = "";
                m_PosizioneGPS = new GPSRecord();
                m_IsPosizioneGPSRelativa = true;
                // Me.m_CodiceInterno = ""
            }

            // ''' <summary>
            // ''' Restituisce o imposta il codice interno
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property CodiceInterno As String
            // Get
            // Return Me.m_CodiceInterno
            // End Get
            // Set(value As String)
            // Dim oldValue As String = Me.m_CodiceInterno
            // value = Strings.Trim(value)
            // If (oldValue = value) Then Exit Property
            // Me.m_CodiceInterno = value
            // Me.DoChanged("CodiceInterno", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce o imposta il codice RFID associato all'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceRFID
            {
                get
                {
                    return m_CodiceRFID;
                }

                set
                {
                    string oldValue = m_CodiceRFID;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceRFID = value;
                    DoChanged("CodiceRFID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta le coordinate GPS che determinano la posizione dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public GPSRecord PosizioneGPS
            {
                get
                {
                    return m_PosizioneGPS;
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la posizione GPS è relativa all'ufficio di appartenenza o se è assoluta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsPosizioneGPSRelativa
            {
                get
                {
                    return m_IsPosizioneGPSRelativa;
                }

                set
                {
                    if (m_IsPosizioneGPSRelativa == value)
                        return;
                    m_IsPosizioneGPSRelativa = value;
                    DoChanged("IsPosizioneGPSRelativa", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del documento di ordine di acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOrdineAcquisto
            {
                get
                {
                    return DBUtils.GetID(m_OrdineAcquisto, m_IDOrdineAcquisto);
                }

                set
                {
                    int oldValue = IDOrdineAcquisto;
                    if (oldValue == value)
                        return;
                    m_IDOrdineAcquisto = value;
                    m_OrdineAcquisto = null;
                    DoChanged("IDOrdineAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ordine di acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DocumentoContabile OrdineAcquisto
            {
                get
                {
                    if (m_OrdineAcquisto is null)
                        m_OrdineAcquisto = DocumentiContabili.GetItemById(m_IDOrdineAcquisto);
                    return m_OrdineAcquisto;
                }

                set
                {
                    var oldValue = m_OrdineAcquisto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DocumentoAcquisto = value;
                    m_IDDocumentoAcquisto = DBUtils.GetID(value, 0);
                    DoChanged("OrdineAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del documento che convalida l'acquisto dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDocumentoAcquisto
            {
                get
                {
                    return DBUtils.GetID(m_DocumentoAcquisto, m_IDDocumentoAcquisto);
                }

                set
                {
                    int oldValue = IDDocumentoAcquisto;
                    if (oldValue == value)
                        return;
                    m_IDDocumentoAcquisto = value;
                    m_DocumentoAcquisto = null;
                    DoChanged("IDDocumentoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il documento di acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DocumentoContabile DocumentoAcquisto
            {
                get
                {
                    if (m_DocumentoAcquisto is null)
                        m_DocumentoAcquisto = DocumentiContabili.GetItemById(m_IDDocumentoAcquisto);
                    return m_DocumentoAcquisto;
                }

                set
                {
                    var oldValue = m_DocumentoAcquisto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DocumentoAcquisto = value;
                    m_IDDocumentoAcquisto = DBUtils.GetID(value, 0);
                    DoChanged("DocumentoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della spedizione con cui è stato ricevuto l'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDSpedizione
            {
                get
                {
                    return DBUtils.GetID(m_Spedizione, m_IDSpedizione);
                }

                set
                {
                    int oldValue = IDSpedizione;
                    if (oldValue == value)
                        return;
                    m_IDSpedizione = value;
                    m_Spedizione = null;
                    DoChanged("IDSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la spedizione con cui è stato ricevuto l'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Spedizione Spedizione
            {
                get
                {
                    if (m_Spedizione is null)
                        m_Spedizione = minidom.Office.Spedizioni.GetItemById(m_IDSpedizione);
                    return m_Spedizione;
                }

                set
                {
                    var oldValue = m_Spedizione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Spedizione = value;
                    m_IDSpedizione = DBUtils.GetID(value, 0);
                    DoChanged("Spedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione delle relazione con gli altri oggetti inventariati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public OggettiCollegatiCollection Relazioni
            {
                get
                {
                    lock (this)
                    {
                        if (m_Relazioni is null)
                            m_Relazioni = new OggettiCollegatiCollection(this);
                        return m_Relazioni;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di produzione del lotto a cui appartiene questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataProduzione
            {
                get
                {
                    return m_DataProduzione;
                }

                set
                {
                    var oldValue = m_DataProduzione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataProduzione = value;
                    DoChanged("DataProduzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'articolo associato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDArticolo
            {
                get
                {
                    return DBUtils.GetID(m_Articolo, m_IDArticolo);
                }

                set
                {
                    int oldValue = IDArticolo;
                    if (oldValue == value)
                        return;
                    m_IDArticolo = value;
                    m_Articolo = null;
                    DoChanged("IDArticolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'articolo in magazzino corrispondente all'oggetto inventariato
            /// </summary>
            public Articolo Articolo
            {
                get
                {
                    if (m_Articolo is null)
                        m_Articolo = Articoli.GetItemById(m_IDArticolo);
                    return m_Articolo;
                }

                set
                {
                    var oldValue = m_Articolo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Articolo = value;
                    m_IDArticolo = DBUtils.GetID(value, 0);
                    if (value is null)
                    {
                        m_NomeArticolo = "";
                    }
                    else
                    {
                        m_NomeArticolo = value.Nome;
                    }

                    DoChanged("Articolo", value, oldValue);
                }
            }

            /// <summary>
            /// Nome Articolo associato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeArticolo
            {
                get
                {
                    return m_NomeArticolo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeArticolo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeArticolo = value;
                    DoChanged("NomeArticolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice di inventario dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Codice
            {
                get
                {
                    return m_Codice;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Codice;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Codice = value;
                    DoChanged("Codice", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la gategoria principale dell'articolo
            /// </summary>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la marca dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Marca
            {
                get
                {
                    return m_Marca;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Marca;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Marca = value;
                    DoChanged("Marca", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modello dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Modello
            {
                get
                {
                    return m_Modello;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Modello;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Modello = value;
                    DoChanged("Modello", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice una collezione di proprietà aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public AttributiOggettoCollection Attributi
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attributi is null)
                            m_Attributi = new AttributiOggettoCollection(this);
                        return m_Attributi;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive in dettaglio l'articolo
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
            /// Restituisce o imposta la url dell'immagine principale dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato attuale dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoOggettoInventariato StatoAttuale
            {
                get
                {
                    return m_StatoAttuale;
                }

                set
                {
                    var oldValue = m_StatoAttuale;
                    if (oldValue == value)
                        return;
                    m_StatoAttuale = value;
                    DoChanged("StatoAttuale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il dettaglio sullo stato attuale dell'oggetto inventariato
            /// </summary>
            public string DettaglioStatoAttuale
            {
                get
                {
                    return m_DettaglioStatoAttuale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioStatoAttuale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStatoAttuale = value;
                    DoChanged("DettaglioStatoAttuale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di oggetto che utilizza l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoInUsoDa
            {
                get
                {
                    return m_TipoInUsoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoInUsoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    if (value is null)
                    {
                        m_TipoInUsoDa = "";
                    }
                    else
                    {
                        m_TipoInUsoDa = DMD.RunTime.vbTypeName(value);
                    }

                    m_TipoInUsoDa = value;
                    m_InUsoDa = null;
                    DoChanged("TipoInUsoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta l'ID dell'oggetto che utilizza questo articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDInUsoDa
            {
                get
                {
                    return DBUtils.GetID(m_InUsoDa, m_IDInUsoDa);
                }

                set
                {
                    int oldValue = IDInUsoDa;
                    if (oldValue == value)
                        return;
                    m_IDInUsoDa = value;
                    m_InUsoDa = null;
                    DoChanged("IDInUsoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto che utilizza questo artcillo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object InUsoDa
            {
                get
                {
                    if (m_InUsoDa is null)
                        m_InUsoDa = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_TipoInUsoDa, m_IDInUsoDa);
                    return m_InUsoDa;
                }

                set
                {
                    var oldValue = InUsoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InUsoDa = value;
                    m_IDInUsoDa = DBUtils.GetID(value, 0);
                    DoChanged("InUsoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'oggetto che utilizza questo articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeInUsoDa
            {
                get
                {
                    return m_NomeInUsoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeInUsoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeInUsoDa = value;
                    DoChanged("NomeInUsoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore stimato dell'articolo alla data memorizzata in DataValutazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? ValoreStimato
            {
                get
                {
                    return m_ValoreStimato;
                }

                set
                {
                    var oldValue = m_ValoreStimato;
                    if (oldValue == value == true)
                        return;
                    m_ValoreStimato = value;
                    DoChanged("ValoreStimato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di valutazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataValutazione
            {
                get
                {
                    return m_DataValutazione;
                }

                set
                {
                    var oldValue = m_DataValutazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataValutazione = value;
                    DoChanged("DataValutazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }

                set
                {
                    var oldValue = m_DataAcquisto;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataAcquisto = value;
                    DoChanged("DataAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del documento di acquisto (Fattura, Scontrino, ...)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoDocumentoAcquisto
            {
                get
                {
                    return m_TipoDocumentoAcquisto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoDocumentoAcquisto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoDocumentoAcquisto = value;
                    DoChanged("TipoDocumentoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero del documento di acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NumeroDocumentoAcquisto
            {
                get
                {
                    return m_NumeroDocumentoAcquisto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroDocumentoAcquisto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroDocumentoAcquisto = value;
                    DoChanged("NumeroDocumentoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato dell'oggetto al momento dell'acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoAcquistoOggettoInventariato StatoAcquisto
            {
                get
                {
                    return m_StatoAcquisto;
                }

                set
                {
                    var oldValue = m_StatoAcquisto;
                    if (oldValue == value)
                        return;
                    m_StatoAcquisto = value;
                    DoChanged("StatoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il dettaglio sullo stato di acquisto
            /// </summary>
            public string DettaglioStatoAcquisto
            {
                get
                {
                    return m_DettaglioStatoAcquisto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DettaglioStatoAcquisto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStatoAcquisto = value;
                    DoChanged("DettaglioStatoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta l'ID dell'utente che ha acquistato l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int AcquistatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_AcquistatoDa, m_AcquistatoDaID);
                }

                set
                {
                    int oldValue = AcquistatoDaID;
                    if (oldValue == value)
                        return;
                    m_AcquistatoDaID = value;
                    m_AcquistatoDa = null;
                    DoChanged("AcquistatoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha acquistato l'oggetto
            /// </summary>
            public Sistema.CUser AcquistatoDa
            {
                get
                {
                    lock (this)
                    {
                        if (m_AcquistatoDa is null)
                            m_AcquistatoDa = Sistema.Users.GetItemById(m_AcquistatoDaID);
                        return m_AcquistatoDa;
                    }
                }

                set
                {
                    Sistema.CUser oldValue;
                    lock (this)
                    {
                        oldValue = AcquistatoDa;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_AcquistatoDa = value;
                        m_AcquistatoDaID = DBUtils.GetID(value, 0);
                        if (value is object)
                            m_NomeAcquistatoDa = value.Nominativo;
                    }

                    DoChanged("AcquistatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha acquistato l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAcquistatoDa
            {
                get
                {
                    return m_NomeAcquistatoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAcquistatoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAcquistatoDa = value;
                    DoChanged("NomeAcquistatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il prezzo (imponibile) di acquisto dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? PrezzoAcquisto
            {
                get
                {
                    return m_PrezzoAcquisto;
                }

                set
                {
                    var oldValue = m_PrezzoAcquisto;
                    if (oldValue == value == true)
                        return;
                    m_PrezzoAcquisto = value;
                    DoChanged("PrezzoAcquisto", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta l'aliquota iva espressa in % sul prezzo imponibile di acquisto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? AliquotaIVA
            {
                get
                {
                    return m_AliquotaIVA;
                }

                set
                {
                    var oldValue = m_AliquotaIVA;
                    if (oldValue == value == true)
                        return;
                    m_AliquotaIVA = value;
                    DoChanged("AliquotaIVA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del punto di operativo per cui è stato originariamente acquistato l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDUfficioOriginale
            {
                get
                {
                    return DBUtils.GetID(m_UfficioOriginale, m_IDUfficioOriginale);
                }

                set
                {
                    int oldValue = IDUfficioOriginale;
                    if (oldValue == value)
                        return;
                    m_IDUfficioOriginale = value;
                    m_UfficioOriginale = null;
                    DoChanged("IDUfficioOriginale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ufficio per cui è stato originariamente acquistato l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CUfficio UfficioOriginale
            {
                get
                {
                    if (m_UfficioOriginale is null)
                        m_UfficioOriginale = Anagrafica.Uffici.GetItemById(m_IDUfficioOriginale);
                    return m_UfficioOriginale;
                }

                set
                {
                    var oldValue = UfficioOriginale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_UfficioOriginale = value;
                    m_IDUfficioOriginale = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeUfficioOriginale = value.Nome;
                    DoChanged("UfficioOriginale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'ufficio originario a cui é stato destinato l'oggetto
            /// </summary>
            public string NomeUfficioOriginale
            {
                get
                {
                    return m_NomeUfficioOriginale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeUfficioOriginale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUfficioOriginale = value;
                    DoChanged("NomeUfficioOriginale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice del reparto in cui è stoccato l'articolo se non in uso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceReparto
            {
                get
                {
                    return m_CodiceReparto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceReparto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceReparto = value;
                    DoChanged("CodiceReparto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice dello scaffale in cui è stoccato l'articolo se non in uso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceScaffale
            {
                get
                {
                    return m_CodiceScaffale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceScaffale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceScaffale = value;
                    DoChanged("CodiceScaffale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di dismissione dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }

                set
                {
                    var oldValue = m_DataDismissione;
                    if (oldValue == value == true)
                        return;
                    m_DataDismissione = value;
                    DoChanged("DataDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha dismesso l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int DismessoDaID
            {
                get
                {
                    return DBUtils.GetID(m_DismessoDa, m_DismessoDaID);
                }

                set
                {
                    int oldValue = DismessoDaID;
                    if (oldValue == value)
                        return;
                    m_DismessoDa = null;
                    m_DismessoDaID = value;
                    DoChanged("DismessoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha dismesso l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser DismessoDa
            {
                get
                {
                    if (m_DismessoDa is null)
                        m_DismessoDa = Sistema.Users.GetItemById(m_DismessoDaID);
                    return m_DismessoDa;
                }

                set
                {
                    var oldValue = DismessoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DismessoDa = value;
                    m_DismessoDaID = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeDismessoDa = value.Nominativo;
                    DoChanged("DismessoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha dismesso l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeDismessoDa
            {
                get
                {
                    return m_NomeDismessoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeDismessoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDismessoDa = value;
                    DoChanged("NomeDismessoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta il motivo della dismissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string MotivoDismissione
            {
                get
                {
                    return m_MotivoDismissione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_MotivoDismissione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoDismissione = value;
                    DoChanged("MotivoDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo della dismissione
            /// </summary>
            public string DettaglioDismissione
            {
                get
                {
                    return m_DettaglioDismissione;
                }

                set
                {
                    string oldValue = m_DettaglioDismissione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioDismissione = value;
                    DoChanged("DettaglioDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stima del valore dell'oggetto nel momento della dismissione (utilizzabile ai fini contabili)
            /// </summary>
            public decimal? ValoreDismissione
            {
                get
                {
                    return m_ValoreDismissione;
                }

                set
                {
                    var oldValue = m_ValoreDismissione;
                    if (oldValue == value == true)
                        return;
                    m_ValoreDismissione = value;
                    DoChanged("ValoreDismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'aliquota iva applicabile al momento della dismissione
            /// </summary>
            public double? AliquotaIVADismissione
            {
                get
                {
                    return m_AliquotaIVADismissione;
                }

                set
                {
                    var oldValue = m_AliquotaIVADismissione;
                    if (oldValue == value == true)
                        return;
                    m_AliquotaIVADismissione = value;
                    DoChanged("AliquotaIVADismissione", value, oldValue);
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Codice", m_Codice);
                writer.WriteAttribute("IDArticolo", IDArticolo);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("NomeArticolo", m_NomeArticolo);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("Marca", m_Marca);
                writer.WriteAttribute("Modello", m_Modello);
                writer.WriteAttribute("Seriale", m_Seriale);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("StatoAttuale", (int?)m_StatoAttuale);
                writer.WriteAttribute("DettaglioStatoAttuale", m_DettaglioStatoAttuale);
                writer.WriteAttribute("ValoreStimato", m_ValoreStimato);
                writer.WriteAttribute("DataValutazione", m_DataValutazione);
                writer.WriteAttribute("TipoInUsoDa", TipoInUsoDa);
                writer.WriteAttribute("IDInUsoDa", IDInUsoDa);
                writer.WriteAttribute("NomeInUsoDa", NomeInUsoDa);
                writer.WriteAttribute("DataProduzione", m_DataProduzione);
                writer.WriteAttribute("DataAcquisto", m_DataAcquisto);
                writer.WriteAttribute("TipoDocumentoAcquisto", m_TipoDocumentoAcquisto);
                writer.WriteAttribute("NumeroDocumentoAcquisto", m_NumeroDocumentoAcquisto);
                writer.WriteAttribute("StatoAcquisto", (int?)m_StatoAcquisto);
                writer.WriteAttribute("DettaglioStatoAcquisto", m_DettaglioStatoAcquisto);
                writer.WriteAttribute("AcquistatoDaID", AcquistatoDaID);
                writer.WriteAttribute("NomeAcquistatoDa", m_NomeAcquistatoDa);
                writer.WriteAttribute("PrezzoAcquisto", m_PrezzoAcquisto);
                writer.WriteAttribute("AliquotaIVA", m_AliquotaIVA);
                writer.WriteAttribute("IDUfficioOriginale", IDUfficioOriginale);
                writer.WriteAttribute("NomeUfficioOriginale", m_NomeUfficioOriginale);
                writer.WriteAttribute("CodiceScaffale", m_CodiceScaffale);
                writer.WriteAttribute("CodiceReparto", m_CodiceReparto);
                writer.WriteAttribute("DataDismissione", m_DataDismissione);
                writer.WriteAttribute("DismessoDaID", DismessoDaID);
                writer.WriteAttribute("NomeDismessoDa", m_NomeDismessoDa);
                writer.WriteAttribute("MotivoDismissione", m_MotivoDismissione);
                writer.WriteAttribute("DettaglioDismissione", m_DettaglioDismissione);
                writer.WriteAttribute("ValoreDismissione", m_ValoreDismissione);
                writer.WriteAttribute("AliquotaIVADismissione", m_AliquotaIVADismissione);
                writer.WriteAttribute("IDOrdineAcquisto", IDOrdineAcquisto);
                writer.WriteAttribute("IDDocumentoAcquisto", IDDocumentoAcquisto);
                writer.WriteAttribute("IDSpedizione", IDSpedizione);
                writer.WriteAttribute("CodiceRFID", m_CodiceRFID);
                writer.WriteAttribute("GSP_REL", m_IsPosizioneGPSRelativa);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("Relazioni", Relazioni);
                writer.WriteTag("PosizioneGPS", m_PosizioneGPS);
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
                    case "Codice":
                        {
                            m_Codice = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeArticolo":
                        {
                            m_NomeArticolo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Marca":
                        {
                            m_Marca = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Modello":
                        {
                            m_Modello = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Seriale":
                        {
                            m_Seriale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoAttuale":
                        {
                            m_StatoAttuale = (StatoOggettoInventariato)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioStatoAttuale":
                        {
                            m_DettaglioStatoAttuale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreStimato":
                        {
                            m_ValoreStimato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataValutazione":
                        {
                            m_DataValutazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoInUsoDa":
                        {
                            m_TipoInUsoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDInUsoDa":
                        {
                            m_IDInUsoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeInUsoDa":
                        {
                            NomeInUsoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataProduzione":
                        {
                            m_DataProduzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataAcquisto":
                        {
                            m_DataAcquisto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoDocumentoAcquisto":
                        {
                            m_TipoDocumentoAcquisto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroDocumentoAcquisto":
                        {
                            m_NumeroDocumentoAcquisto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoAcquisto":
                        {
                            m_StatoAcquisto = (StatoAcquistoOggettoInventariato)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioStatoAcquisto":
                        {
                            m_DettaglioStatoAcquisto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AcquistatoDaID":
                        {
                            m_AcquistatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAcquistatoDa":
                        {
                            m_NomeAcquistatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PrezzoAcquisto":
                        {
                            m_PrezzoAcquisto = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "AliquotaIVA":
                        {
                            m_AliquotaIVA = (double?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDUfficioOriginale":
                        {
                            m_IDUfficioOriginale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUfficioOriginale":
                        {
                            m_NomeUfficioOriginale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceScaffale":
                        {
                            m_CodiceScaffale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceReparto":
                        {
                            m_CodiceReparto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataDismissione":
                        {
                            m_DataDismissione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DismessoDaID":
                        {
                            m_DismessoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeDismessoDa":
                        {
                            m_NomeDismessoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoDismissione":
                        {
                            m_MotivoDismissione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioDismissione":
                        {
                            m_DettaglioDismissione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreDismissione":
                        {
                            m_ValoreDismissione = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "AliquotaIVADismissione":
                        {
                            m_AliquotaIVADismissione = (double?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDArticolo":
                        {
                            m_IDArticolo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOrdineAcquisto":
                        {
                            m_IDOrdineAcquisto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDocumentoAcquisto":
                        {
                            m_IDDocumentoAcquisto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDSpedizione":
                        {
                            m_IDSpedizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CodiceRFID":
                        {
                            m_CodiceRFID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "GSP_REL":
                        {
                            m_IsPosizioneGPSRelativa = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "PosizioneGPS":
                        {
                            m_PosizioneGPS = (GPSRecord)fieldValue;
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (AttributiOggettoCollection)fieldValue;
                            m_Attributi.SetOggetto(this);
                            break;
                        }

                    case "Relazioni":
                        {
                            m_Relazioni = new OggettiCollegatiCollection();
                            m_Relazioni.SetOwner(this);
                            foreach (OggettoCollegato item in (IEnumerable)fieldValue)
                            {
                                m_Relazioni.Add(item);
                                if (item.IDOggetto1 == DBUtils.GetID(this, 0))
                                    item.SetOggetto1(this);
                                if (item.IDOggetto2 == DBUtils.GetID(this, 0))
                                    item.SetOggetto2(this);
                            }

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
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeOggettiInventariati";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.OggettiInventariati;
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return  base.IsChanged() 
                      || (m_Attributi is object && m_Attributi.IsChanged())
                      || m_PosizioneGPS.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (m_Attributi is object)
                    m_Attributi.Save(force);                     
                m_PosizioneGPS.SetChanged(false);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Codice = reader.Read("Codice",  m_Codice);
                m_Nome = reader.Read("Nome",  m_Nome);
                m_NomeArticolo = reader.Read("NomeArticolo",  m_NomeArticolo);
                m_IDArticolo = reader.Read("IDArticolo",  m_IDArticolo);
                m_Categoria = reader.Read("Categoria",  m_Categoria);
                m_Marca = reader.Read("Marca",  m_Marca);
                m_Modello = reader.Read("Modello",  m_Modello);
                m_Seriale = reader.Read("Seriale",  m_Seriale);
                m_IconURL = reader.Read("IconURL",  m_IconURL);
                m_StatoAttuale = reader.Read("StatoAttuale",  m_StatoAttuale);
                m_DettaglioStatoAttuale = reader.Read("DettaglioStatoAttuale",  m_DettaglioStatoAttuale);
                m_ValoreStimato = reader.Read("ValoreStimato",  m_ValoreStimato);
                m_DataValutazione = reader.Read("DataValutazione",  m_DataValutazione);
                m_TipoInUsoDa = reader.Read("TipoInUsoDa",  m_TipoInUsoDa);
                m_IDInUsoDa = reader.Read("IDInUsoDa",  m_IDInUsoDa);
                m_NomeInUsoDa = reader.Read("NomeInUsoDa",  m_NomeInUsoDa);
                m_DataProduzione = reader.Read("DataProduzione",  m_DataProduzione);
                m_DataAcquisto = reader.Read("DataAcquisto",  m_DataAcquisto);
                m_TipoDocumentoAcquisto = reader.Read("TipoDocumentoAcquisto",  m_TipoDocumentoAcquisto);
                m_NumeroDocumentoAcquisto = reader.Read("NumeroDocumentoAcquisto",  m_NumeroDocumentoAcquisto);
                m_StatoAcquisto = reader.Read("StatoAcquisto",  m_StatoAcquisto);
                m_DettaglioStatoAcquisto = reader.Read("DettaglioStatoAcquisto",  m_DettaglioStatoAcquisto);
                m_AcquistatoDaID = reader.Read("AcquistatoDaID",  m_AcquistatoDaID);
                m_NomeAcquistatoDa = reader.Read("NomeAcquistatoDa",  m_NomeAcquistatoDa);
                m_PrezzoAcquisto = reader.Read("PrezzoAcquisto",  m_PrezzoAcquisto);
                m_AliquotaIVA = reader.Read("AliquotaIVA",  m_AliquotaIVA);
                m_IDUfficioOriginale = reader.Read("IDUfficioOriginale",  m_IDUfficioOriginale);
                m_NomeUfficioOriginale = reader.Read("NomeUfficioOriginale",  m_NomeUfficioOriginale);
                m_CodiceScaffale = reader.Read("CodiceScaffale",  m_CodiceScaffale);
                m_CodiceReparto = reader.Read("CodiceReparto",  m_CodiceReparto);
                m_DataDismissione = reader.Read("DataDismissione",  m_DataDismissione);
                m_DismessoDaID = reader.Read("DismessoDaID",  m_DismessoDaID);
                m_NomeDismessoDa = reader.Read("NomeDismessoDa",  m_NomeDismessoDa);
                m_MotivoDismissione = reader.Read("MotivoDismissione",  m_MotivoDismissione);
                m_DettaglioDismissione = reader.Read("DettaglioDismissione",  m_DettaglioDismissione);
                m_ValoreDismissione = reader.Read("ValoreDismissione",  m_ValoreDismissione);
                m_AliquotaIVADismissione = reader.Read("AliquotaIVADismissione",  m_AliquotaIVADismissione);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_IDOrdineAcquisto = reader.Read("IDOrdineAcquisto",  m_IDOrdineAcquisto);
                m_IDDocumentoAcquisto = reader.Read("IDDocumentoAcquisto",  m_IDDocumentoAcquisto);
                m_IDSpedizione = reader.Read("IDSpedizione",  m_IDSpedizione);
                m_CodiceRFID = reader.Read("CodiceRFID",  m_CodiceRFID);
                {
                    var withBlock = m_PosizioneGPS;
                    withBlock.Longitudine = reader.Read("GPS_LON", withBlock.Longitudine);
                    withBlock.Latitudine = reader.Read("GPS_LAT", withBlock.Latitudine);
                    withBlock.Altitudine = reader.Read("GPS_ALT", withBlock.Altitudine);
                    withBlock.SetChanged(false);
                }

                this.m_IsPosizioneGPSRelativa = reader.Read("GSP_REL", this.m_IsPosizioneGPSRelativa);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp)) 
                { 
                    this.m_Attributi = (AttributiOggettoCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                    this.m_Attributi.SetOggetto(this);
                }
                 

                // Me.m_CodiceInterno = reader.Read("CodiceInterno", Me.m_CodiceInterno)
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel database
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Codice", m_Codice);
                writer.Write("Nome", m_Nome);
                writer.Write("NomeArticolo", m_NomeArticolo);
                writer.Write("IDArticolo", IDArticolo);
                writer.Write("Categoria", m_Categoria);
                writer.Write("Marca", m_Marca);
                writer.Write("Modello", m_Modello);
                writer.Write("Seriale", m_Seriale);
                writer.Write("IconURL", m_IconURL);
                writer.Write("StatoAttuale", m_StatoAttuale);
                writer.Write("DettaglioStatoAttuale", m_DettaglioStatoAttuale);
                writer.Write("ValoreStimato", m_ValoreStimato);
                writer.Write("DataValutazione", m_DataValutazione);
                writer.Write("TipoInUsoDa", TipoInUsoDa);
                writer.Write("IDInUsoDa", IDInUsoDa);
                writer.Write("NomeInUsoDa", NomeInUsoDa);
                writer.Write("DataProduzione", m_DataProduzione);
                writer.Write("DataAcquisto", m_DataAcquisto);
                writer.Write("TipoDocumentoAcquisto", m_TipoDocumentoAcquisto);
                writer.Write("NumeroDocumentoAcquisto", m_NumeroDocumentoAcquisto);
                writer.Write("StatoAcquisto", m_StatoAcquisto);
                writer.Write("DettaglioStatoAcquisto", m_DettaglioStatoAcquisto);
                writer.Write("AcquistatoDaID", AcquistatoDaID);
                writer.Write("NomeAcquistatoDa", m_NomeAcquistatoDa);
                writer.Write("PrezzoAcquisto", m_PrezzoAcquisto);
                writer.Write("AliquotaIVA", m_AliquotaIVA);
                writer.Write("IDUfficioOriginale", IDUfficioOriginale);
                writer.Write("NomeUfficioOriginale", m_NomeUfficioOriginale);
                writer.Write("CodiceScaffale", m_CodiceScaffale);
                writer.Write("CodiceReparto", m_CodiceReparto);
                writer.Write("DataDismissione", m_DataDismissione);
                writer.Write("DismessoDaID", DismessoDaID);
                writer.Write("NomeDismessoDa", m_NomeDismessoDa);
                writer.Write("MotivoDismissione", m_MotivoDismissione);
                writer.Write("DettaglioDismissione", m_DettaglioDismissione);
                writer.Write("ValoreDismissione", m_ValoreDismissione);
                writer.Write("AliquotaIVADismissione", m_AliquotaIVADismissione);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("IDOrdineAcquisto", IDOrdineAcquisto);
                writer.Write("IDDocumentoAcquisto", IDDocumentoAcquisto);
                writer.Write("IDSpedizione", IDSpedizione);
                writer.Write("CodiceRFID", m_CodiceRFID);
                {
                    var withBlock = m_PosizioneGPS;
                    writer.Write("GPS_LON", withBlock.Longitudine);
                    writer.Write("GPS_LAT", withBlock.Latitudine);
                    writer.Write("GPS_ALT", withBlock.Altitudine);
                }

                writer.Write("GSP_REL", m_IsPosizioneGPSRelativa);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Codice", typeof(string), 255);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("NomeArticolo", typeof(string), 255);
                c = table.Fields.Ensure("IDArticolo", typeof(int), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Marca", typeof(string), 255);
                c = table.Fields.Ensure("Modello", typeof(string), 255);
                c = table.Fields.Ensure("Seriale", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("StatoAttuale", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioStatoAttuale", typeof(string), 0);
                c = table.Fields.Ensure("ValoreStimato", typeof(decimal), 1);
                c = table.Fields.Ensure("DataValutazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("TipoInUsoDa", typeof(string), 255);
                c = table.Fields.Ensure("IDInUsoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeInUsoDa", typeof(string), 255);
                c = table.Fields.Ensure("DataProduzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataAcquisto", typeof(DateTime), 1);
                c = table.Fields.Ensure("TipoDocumentoAcquisto", typeof(string), 255);
                c = table.Fields.Ensure("NumeroDocumentoAcquisto", typeof(string), 255);
                c = table.Fields.Ensure("StatoAcquisto", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioStatoAcquisto", typeof(string), 0);
                c = table.Fields.Ensure("AcquistatoDaID", typeof(int), 1);
                c = table.Fields.Ensure("NomeAcquistatoDa", typeof(string), 255);
                c = table.Fields.Ensure("PrezzoAcquisto", typeof(decimal), 1);
                c = table.Fields.Ensure("AliquotaIVA", typeof(double), 1);
                c = table.Fields.Ensure("IDUfficioOriginale", typeof(int), 1);
                c = table.Fields.Ensure("NomeUfficioOriginale", typeof(string), 255);
                c = table.Fields.Ensure("CodiceScaffale", typeof(string), 255);
                c = table.Fields.Ensure("CodiceReparto", typeof(string), 255);
                c = table.Fields.Ensure("DataDismissione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DismessoDaID", typeof(int), 1);
                c = table.Fields.Ensure("NomeDismessoDa", typeof(string), 255);
                c = table.Fields.Ensure("MotivoDismissione", typeof(string), 255);
                c = table.Fields.Ensure("DettaglioDismissione", typeof(string), 0);
                c = table.Fields.Ensure("ValoreDismissione", typeof(decimal), 1);
                c = table.Fields.Ensure("AliquotaIVADismissione", typeof(double), 1);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("Attributi", typeof(string), 0);
                c = table.Fields.Ensure("IDOrdineAcquisto", typeof(int), 1);
                c = table.Fields.Ensure("IDDocumentoAcquisto", typeof(int), 1);
                c = table.Fields.Ensure("IDSpedizione", typeof(int), 1);
                c = table.Fields.Ensure("CodiceRFID", typeof(string), 255);
 
                {
                    c = table.Fields.Ensure("GPS_LON", typeof(double), 1);
                    c = table.Fields.Ensure("GPS_LAT", typeof(double), 1);
                    c = table.Fields.Ensure("GPS_ALT", typeof(double), 1);
                    c = table.Fields.Ensure("GSP_REL", typeof(bool), 1);
                }

 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCodice", new string[] { "Codice", "Seriale", "CodiceScaffale", "CodiceReparto", "CodiceRFID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "IconURL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxArticolo", new string[] { "IDArticolo", "NomeArticolo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "Marca", "Modello" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStato", new string[] { "StatoAttuale", "DettaglioStatoAttuale" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValore", new string[] { "DataValutazione", "ValoreStimato" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInUso", new string[] { "TipoInUsoDa", "IDInUsoDa", "NomeInUsoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAcquisto1", new string[] { "DataAcquisto", "TipoDocumentoAcquisto", "NumeroDocumentoAcquisto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAcquisto2", new string[] { "DataProduzione", "StatoAcquisto", "DettaglioStatoAcquisto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAcquisto3", new string[] { "AcquistatoDaID", "NomeAcquistatoDa", "PrezzoAcquisto", "AliquotaIVA" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUfficioOrig", new string[] { "IDUfficioOriginale", "NomeUfficioOriginale" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDismissione1", new string[] { "DataDismissione", "DismessoDaID", "NomeDismessoDa", "MotivoDismissione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDismissione2", new string[] { "DettaglioDismissione", "ValoreDismissione", "AliquotaIVADismissione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione", "IDOrdineAcquisto", "IDDocumentoAcquisto", "IDSpedizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxGPS", new string[] { "GPS_LON", "GPS_LAT", "GPS_ALT", "GSP_REL" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Attributi", typeof(string), 0);
                

            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is OggettoInventariato) && this.Equals((OggettoInventariato)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(OggettoInventariato obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Codice, obj.m_Codice)
                    && DMD.Integers.EQ(this.m_IDArticolo, obj.m_IDArticolo)
                    && DMD.Strings.EQ(this.m_NomeArticolo, obj.m_NomeArticolo)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Strings.EQ(this.m_Marca, obj.m_Marca)
                    && DMD.Strings.EQ(this.m_Modello, obj.m_Modello)
                    && DMD.Strings.EQ(this.m_Seriale, obj.m_Seriale)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.Integers.EQ((int)this.m_StatoAttuale, (int)obj.m_StatoAttuale)
                    && DMD.Strings.EQ(this.m_DettaglioStatoAttuale, obj.m_DettaglioStatoAttuale)
                    && DMD.Decimals.EQ(this.m_ValoreStimato, obj.m_ValoreStimato)
                    && DMD.DateUtils.EQ(this.m_DataValutazione, obj.m_DataValutazione)
                    && DMD.Strings.EQ(this.m_TipoInUsoDa, obj.m_TipoInUsoDa)
                    && DMD.Integers.EQ(this.m_IDInUsoDa, obj.m_IDInUsoDa)
                    && DMD.Strings.EQ(this.m_NomeInUsoDa, obj.m_NomeInUsoDa)
                    && DMD.DateUtils.EQ(this.m_DataProduzione, obj.m_DataProduzione)
                    && DMD.DateUtils.EQ(this.m_DataAcquisto, obj.m_DataAcquisto)
                    && DMD.Strings.EQ(this.m_TipoDocumentoAcquisto, obj.m_TipoDocumentoAcquisto)
                    && DMD.Strings.EQ(this.m_NumeroDocumentoAcquisto, obj.m_NumeroDocumentoAcquisto)
                    && DMD.Integers.EQ((int)this.m_StatoAcquisto, (int)obj.m_StatoAcquisto)
                    && DMD.Strings.EQ(this.m_DettaglioStatoAcquisto, obj.m_DettaglioStatoAcquisto)
                    && DMD.Integers.EQ(this.m_AcquistatoDaID, obj.m_AcquistatoDaID)
                    && DMD.Strings.EQ(this.m_NomeAcquistatoDa, obj.m_NomeAcquistatoDa)
                    && DMD.Decimals.EQ(this.m_PrezzoAcquisto, obj.m_PrezzoAcquisto)
                    && DMD.Doubles.EQ(this.m_AliquotaIVA, obj.m_AliquotaIVA)
                    && DMD.Integers.EQ(this.m_IDUfficioOriginale, obj.m_IDUfficioOriginale)
                    && DMD.Strings.EQ(this.m_NomeUfficioOriginale, obj.m_NomeUfficioOriginale)
                    && DMD.Strings.EQ(this.m_CodiceScaffale, obj.m_CodiceScaffale)
                    && DMD.Strings.EQ(this.m_CodiceReparto, obj.m_CodiceReparto)
                    && DMD.DateUtils.EQ(this.m_DataDismissione, obj.m_DataDismissione)
                    && DMD.Integers.EQ(this.m_DismessoDaID, obj.m_DismessoDaID)
                    && DMD.Strings.EQ(this.m_NomeDismessoDa, obj.m_NomeDismessoDa)
                    && DMD.Strings.EQ(this.m_MotivoDismissione, obj.m_MotivoDismissione)
                    && DMD.Strings.EQ(this.m_DettaglioDismissione, obj.m_DettaglioDismissione)
                    && DMD.Decimals.EQ(this.m_ValoreDismissione, obj.m_ValoreDismissione)
                    && DMD.Doubles.EQ(this.m_AliquotaIVADismissione, obj.m_AliquotaIVADismissione)
                    //private AttributiOggettoCollection m_Attributi;
                    //private OggettiCollegatiCollection m_Relazioni;
                    //private FlagsOggettoInventariato m_Flags;
                    && DMD.Integers.EQ(this.m_IDOrdineAcquisto, obj.m_IDOrdineAcquisto)
                    && DMD.Integers.EQ(this.m_IDDocumentoAcquisto, obj.m_IDDocumentoAcquisto)
                    && DMD.Integers.EQ(this.m_IDSpedizione, obj.m_IDSpedizione)
                    //Spedizione m_Spedizione;
                    && DMD.Strings.EQ(this.m_CodiceRFID, obj.m_CodiceRFID)
                    && this.m_PosizioneGPS.Equals(obj.m_PosizioneGPS)
                    && DMD.Booleans.EQ(this.m_IsPosizioneGPSRelativa, obj.m_IsPosizioneGPSRelativa)
                    ;
            }
        }
    }
}