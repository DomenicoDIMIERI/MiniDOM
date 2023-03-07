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
        /// Stati di una commissione
        /// </summary>
        /// <remarks></remarks>
        public enum StatoEstrattoContributivo : int
        {
            /// <summary>
            /// Da richiedere
            /// </summary>
            DaRichiedere = 0,

            /// <summary>
            /// Richiesto
            /// </summary>
            Richiesto = 1,

            /// <summary>
            /// Assegnato
            /// </summary>
            Assegnato = 2,

            /// <summary>
            /// Evaso
            /// </summary>
            Evaso = 3,

            /// <summary>
            /// Sospeso
            /// </summary>
            Sospeso = -1,

            /// <summary>
            /// Errore
            /// </summary>
            Errore = 255
        }

        /// <summary>
        /// Rappresenta una richiesta di estratto contributivo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class EstrattoContributivo
            : Databases.DBObjectPO
        {

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene richiesto
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> Richiesto;

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene preso in carico
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> PresoInCarico;

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene evaso
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> Evaso;

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene segnato come annullato 
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> Errato;


            /// <summary>
            /// Evento generato quando l'estratto contributivo viene segnato come sospeso
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> Sospeso;


            [NonSerialized] private CUser m_Richiedente;                  // Operatore che ha richiesto l'estratto
            private int m_IDRichiedente;              // ID dell'operatore che ha richiesto l'estratto
            private string m_NomeRichiedente;             // Nome dell'operatore che ha richiesto l'estratto
            private DateTime? m_DataRichiesta;    // Data ed ora della richiesta
            private DateTime? m_DataAssegnazione; // Data ed ora di presa in carico della richiesta
            [NonSerialized] private CUser m_AssegnatoA;                   // Utente che ha preso in carico la richiesta
            private int m_IDAssegnatoA;               // ID dell'utente che ha preso in carico la richiesta
            private string m_NomeAssegnatoA;              // Nome dell'utente che ha preso in carico la richiesta
            private StatoEstrattoContributivo m_StatoRichiesta; // Stato di lavorazione
            private DateTime? m_DataCompletamento; // Data ed ora di completamento della richiesta (o di annullamento)
            private int m_IDCliente;                   // ID del cliente per cui è stata fatta la richiesta
            [NonSerialized] private CPersonaFisica m_Cliente;             // Cliente per cui è stata fatta la richiesta
            private string m_NomeCliente;                 // Nome del cliente per cui è stata fatta la richiesta
            private int m_IDAmministrazione;          // ID dell'amministrazione a cui è stata fatta la richiesta
            [NonSerialized] private CAzienda m_Amministrazione;           // Amministrazione a cui è stata fatta la richiesta
            private string m_NomeAmministrazione;         // Nome dell'amministrazione a cui è stata fatta la richiesta
            private int m_IDDelega;                   // ID dell'allegato relativo alla delega del cliente
            [NonSerialized] private CAttachment m_Delega;
            private int m_IDDocumentoRiconoscimento;  // ID dell'allegato contenente il documento di riconoscimento
            [NonSerialized] private CAttachment m_DocumentoRiconoscimento;
            private int m_IDCodiceFiscale;            // ID dell'allegato relativo al codice fiscale del cliente
            [NonSerialized] private CAttachment m_CodiceFiscale;

            // Private m_IDAllegato As Integer                 'ID dell'allegato prodotto come risposta
            // Private m_Allegato As CAttachment               'Allegato prodotto come risposta
            [NonSerialized] private CAnnotazioni m_Messages;
            [NonSerialized] private CAttachmentsCollection m_Allegati;
            private string m_Note;
            [NonSerialized] private object m_Source;          // Oggetto che ha generato la richiesta (verrà informato in caso delle modifiche)
            private string m_SourceType;      // Tipo dell'oggetto che ha generato la richiesta
            private int m_SourceID;       // ID dell'oggetto che ha generato la richiesta

            /// <summary>
            /// Costruttore
            /// </summary>
            public EstrattoContributivo()
            {
                m_Richiedente = null;
                m_IDRichiedente = 0;
                m_NomeRichiedente = DMD.Strings.vbNullString;
                m_DataRichiesta = default;
                m_AssegnatoA = null;
                m_IDAssegnatoA = 0;
                m_NomeAssegnatoA = DMD.Strings.vbNullString;
                m_DataAssegnazione = default;
                m_StatoRichiesta = StatoEstrattoContributivo.DaRichiedere;
                m_DataCompletamento = default;
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = DMD.Strings.vbNullString;
                m_IDAmministrazione = 0;
                m_Amministrazione = null;
                m_NomeAmministrazione = DMD.Strings.vbNullString;
                m_IDDelega = 0;
                m_Delega = null;
                m_IDDocumentoRiconoscimento = 0;
                m_DocumentoRiconoscimento = null;
                m_IDCodiceFiscale = 0;
                m_CodiceFiscale = null;
                // Me.m_IDAllegato = 0
                // Me.m_Allegato = Nothing
                m_Messages = null;
                m_Allegati = null;
                m_Note = DMD.Strings.vbNullString;
                m_Source = null;
                m_SourceID = 0;
                m_SourceType = "";
            }

            /// <summary>
            /// Restituisce l'oggetto che ha creato l'estratto contributivo
            /// </summary>
            public object Source
            {
                get
                {
                    if (m_Source is null && !string.IsNullOrEmpty(m_SourceType) && m_SourceID != 0)
                        m_Source = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_SourceType, m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_SourceType = DMD.RunTime.vbTypeName(value);
                    m_SourceID = DBUtils.GetID(value, 0);
                    DoChanged("Source", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome del tipo dell'oggetto che ha generato l'estratto contributivo
            /// </summary>
            public string SourceType
            {
                get
                {
                    if (m_Source is object)
                        return DMD.RunTime.vbTypeName(m_Source);
                    return m_SourceType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = SourceType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceType = value;
                    m_Source = null;
                    DoChanged("SourceType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'id dell'oggetto che ha generato l'estratto contributivo
            /// </summary>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID(m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del campo note
            /// </summary>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'allegato che corrisponde alla delega firmata dal cliente
            /// </summary>
            public Sistema.CAttachment Delega
            {
                get
                {
                    if (m_Delega is null)
                        m_Delega = Sistema.Attachments.GetItemById(m_IDDelega);
                    return m_Delega;
                }

                set
                {
                    var oldValue = m_Delega;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Delega = value;
                    m_IDDelega = DBUtils.GetID(value, 0);
                    DoChanged("Delega", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'allegato che contiene la delega firmata dal cliente
            /// </summary>
            public int IDDelega
            {
                get
                {
                    return DBUtils.GetID(m_Delega, m_IDDelega);
                }

                set
                {
                    int oldValue = IDDelega;
                    if (oldValue == value)
                        return;
                    m_IDDelega = value;
                    m_Delega = null;
                    DoChanged("IDDelega", value, oldValue);
                }
            }

            /// <summary>
            /// Allegato che contiene il documento di riconoscimento del cliente
            /// </summary>
            public Sistema.CAttachment DocumentoRiconoscimento
            {
                get
                {
                    if (m_DocumentoRiconoscimento is null)
                        m_DocumentoRiconoscimento = Sistema.Attachments.GetItemById(m_IDDocumentoRiconoscimento);
                    return m_DocumentoRiconoscimento;
                }

                set
                {
                    var oldValue = m_DocumentoRiconoscimento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DocumentoRiconoscimento = value;
                    m_IDDocumentoRiconoscimento = DBUtils.GetID(value, 0);
                    DoChanged("DocumentoRiconoscimento", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'allegato che contiene il documento di riconoscimento del cliente
            /// </summary>
            public int IDDocumentoRiconoscimento
            {
                get
                {
                    return DBUtils.GetID(m_DocumentoRiconoscimento, m_IDDocumentoRiconoscimento);
                }

                set
                {
                    int oldValue = IDDocumentoRiconoscimento;
                    if (oldValue == value)
                        return;
                    m_IDDocumentoRiconoscimento = value;
                    m_DocumentoRiconoscimento = null;
                    DoChanged("IDDocumentoRiconoscimento", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'allegato che contiene il codice fiscale del cliente
            /// </summary>
            public int IDCodiceFiscale
            {
                get
                {
                    return DBUtils.GetID(m_CodiceFiscale, m_IDCodiceFiscale);
                }

                set
                {
                    int oldValue = IDCodiceFiscale;
                    if (oldValue == value)
                        return;
                    m_IDCodiceFiscale = value;
                    m_CodiceFiscale = null;
                    DoChanged("IDCodiceFiscale", value, oldValue);
                }
            }

            /// <summary>
            /// Allegato che contiene il codice fiscale del cliente
            /// </summary>
            public Sistema.CAttachment CodiceFiscale
            {
                get
                {
                    if (m_CodiceFiscale is null)
                        m_CodiceFiscale = Sistema.Attachments.GetItemById(m_IDCodiceFiscale);
                    return m_CodiceFiscale;
                }

                set
                {
                    var oldValue = m_CodiceFiscale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CodiceFiscale = value;
                    m_IDCodiceFiscale = DBUtils.GetID(value, 0);
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            // Public Property Allegato As CAttachment
            // Get
            // If (Me.m_Allegato Is Nothing) Then Me.m_Allegato = Attachments.GetItemById(Me.m_IDAllegato)
            // Return Me.m_Allegato
            // End Get
            // Set(value As CAttachment)
            // Dim oldValue As CAttachment = Me.m_Allegato
            // If (oldValue Is value) Then Exit Property
            // Me.m_Allegato = value
            // Me.m_IDAllegato = GetID(value)
            // Me.DoChanged("Allegato", value, oldValue)
            // End Set
            // End Property

            // Public Property IDAllegato As Integer
            // Get
            // Return GetID(Me.m_Allegato, Me.m_IDAllegato)
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.IDAllegato
            // If (oldValue = value) Then Exit Property
            // Me.m_IDAllegato = value
            // Me.m_Allegato = Nothing
            // Me.DoChanged("IDAllegato", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce la collezione degli allegati caricati per questo oggetto
            /// </summary>
            public Sistema.CAttachmentsCollection Allegati
            {
                get
                {
                    if (m_Allegati is null)
                        m_Allegati = new Sistema.CAttachmentsCollection(this);
                    return m_Allegati;
                }
            }

            /// <summary>
            /// Restituisce la collezione delle note inserite per questo oggetto
            /// </summary>
            public Sistema.CAnnotazioni Messaggi
            {
                get
                {
                    if (m_Messages is null)
                        m_Messages = new Sistema.CAnnotazioni(this);
                    return m_Messages;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha effettuat la richiesta dell'estratto contributivo
            /// </summary>
            public Sistema.CUser Richiedente
            {
                get
                {
                    if (m_Richiedente is null)
                        m_Richiedente = Sistema.Users.GetItemById(m_IDRichiedente);
                    return m_Richiedente;
                }

                set
                {
                    var oldValue = m_Richiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiedente = value;
                    m_IDRichiedente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeRichiedente = value.Nominativo;
                    DoChanged("Richiedente", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente che ha effettuato la richiesta di estratto contributivo
            /// </summary>
            public int IDRichiedente
            {
                get
                {
                    return DBUtils.GetID(m_Richiedente, m_IDRichiedente);
                }

                set
                {
                    int oldValue = IDRichiedente;
                    if (oldValue == value)
                        return;
                    m_IDRichiedente = value;
                    m_Richiedente = null;
                    DoChanged("IDRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'utente che ha effettuato la richiesta di estratto contributivo
            /// </summary>
            public string NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiedente = value;
                    DoChanged("NomeRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Data della richiesta
            /// </summary>
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
            /// Utente a cui é assegnata la richiesta di estratto contributivo
            /// </summary>
            public Sistema.CUser AssegnatoA
            {
                get
                {
                    if (m_AssegnatoA is null)
                        m_AssegnatoA = Sistema.Users.GetItemById(m_IDAssegnatoA);
                    return m_AssegnatoA;
                }

                set
                {
                    var oldValue = m_AssegnatoA;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AssegnatoA = value;
                    if (value is object)
                        m_NomeAssegnatoA = value.Nominativo;
                    DoChanged("AssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente a cui è assegnata la richiesta
            /// </summary>
            public int IDAssegnatoA
            {
                get
                {
                    return DBUtils.GetID(m_AssegnatoA, m_IDAssegnatoA);
                }

                set
                {
                    int oldValue = IDAssegnatoA;
                    if (oldValue == value)
                        return;
                    m_IDAssegnatoA = value;
                    m_AssegnatoA = null;
                    DoChanged("IDAssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'utente a cui é assegnata la richiesta
            /// </summary>
            public string NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAssegnatoA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAssegnatoA = value;
                    DoChanged("NomeAssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Data di assegnazione della richiesta
            /// </summary>
            public DateTime? DataAssegnazione
            {
                get
                {
                    return m_DataAssegnazione;
                }

                set
                {
                    var oldValue = m_DataAssegnazione;
                    if (oldValue == value == true)
                        return;
                    m_DataAssegnazione = value;
                    DoChanged("DataAssegnazione", value, oldValue);
                }
            }

            /// <summary>
            /// Stato della richiesta
            /// </summary>
            public StatoEstrattoContributivo StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }

                set
                {
                    var oldValue = m_StatoRichiesta;
                    if (oldValue == value)
                        return;
                    m_StatoRichiesta = value;
                    DoChanged("StatoRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Data di esito della richiesta
            /// </summary>
            public DateTime? DataCompletamento
            {
                get
                {
                    return m_DataCompletamento;
                }

                set
                {
                    var oldValue = m_DataCompletamento;
                    if (oldValue == value == true)
                        return;
                    m_DataCompletamento = value;
                    DoChanged("DataCompletamento", value, oldValue);
                }
            }

            /// <summary>
            /// ID del cliente per cui é stata fatta la richiesta
            /// </summary>
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
            /// Cliente per cui é stata fatta la richiesta
            /// </summary>
            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.PersoneFisiche.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_NomeCliente = (value is object)? value.Nominativo : "";
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Nome della persona per cui é stata fatta la richiesta
            /// </summary>
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
            /// ID dell'amministrazione
            /// </summary>
            public int IDAmministrazione
            {
                get
                {
                    return DBUtils.GetID(m_Amministrazione, m_IDAmministrazione);
                }

                set
                {
                    int oldValue = IDAmministrazione;
                    if (oldValue == value)
                        return;
                    m_IDAmministrazione = value;
                    m_Amministrazione = null;
                    DoChanged("IDAmministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Amministrazione
            /// </summary>
            public Anagrafica.CAzienda Amministrazione
            {
                get
                {
                    if (m_Amministrazione is null)
                        m_Amministrazione = Anagrafica.Aziende.GetItemById(m_IDAmministrazione);
                    return m_Amministrazione;
                }

                set
                {
                    var oldValue = m_Amministrazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Amministrazione = value;
                    m_NomeAmministrazione = (value is object)? value.Nominativo : "";
                    m_IDAmministrazione = DBUtils.GetID(value, 0);
                    DoChanged("Amministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'amministrazione
            /// </summary>
            public string NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAmministrazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAmministrazione = value;
                    DoChanged("NomeAmministrazione", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.EstrattiContributivi;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeEstrattiC";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDRichiedente = reader.Read("IDRichiedente",  m_IDRichiedente);
                m_NomeRichiedente = reader.Read("NomeRichiedente",  m_NomeRichiedente);
                m_DataRichiesta = reader.Read("DataRichiesta",  m_DataRichiesta);
                m_IDAssegnatoA = reader.Read("IDAssegnatoA",  m_IDAssegnatoA);
                m_NomeAssegnatoA = reader.Read("NomeAssegnatoA",  m_NomeAssegnatoA);
                m_DataAssegnazione = reader.Read("DataAssegnazione",  m_DataAssegnazione);
                m_StatoRichiesta = reader.Read("StatoRichiesta",  m_StatoRichiesta);
                m_DataCompletamento = reader.Read("DataCompletamento",  m_DataCompletamento);
                m_IDCliente = reader.Read("IDCliente",  m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente",  m_NomeCliente);
                m_IDAmministrazione = reader.Read("IDAmministrazione",  m_IDAmministrazione);
                m_NomeAmministrazione = reader.Read("NomeAmministrazione",  m_NomeAmministrazione);
                m_IDDelega = reader.Read("IDDelega",  m_IDDelega);
                m_IDDocumentoRiconoscimento = reader.Read("IDDocRic",  m_IDDocumentoRiconoscimento);
                m_IDCodiceFiscale = reader.Read("IDCF",  m_IDCodiceFiscale);
                m_Note = reader.Read("Note",  m_Note);
                m_SourceType = reader.Read("SourceType",  m_SourceType);
                m_SourceID = reader.Read("SourceID",  m_SourceID);
                // Me.m_IDAllegato = reader.Read("IDAllegato", Me.m_IDAllegato)
                string tmp = reader.Read("Messaggi", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_Messages = (Sistema.CAnnotazioni)DMD.XML.Utils.Serializer.Deserialize(tmp);

                tmp = reader.Read("Allegati", "");
                if (!string.IsNullOrEmpty(tmp))
                    m_Allegati = (Sistema.CAttachmentsCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDRichiedente", IDRichiedente);
                writer.Write("NomeRichiedente", m_NomeRichiedente);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("IDAssegnatoA", IDAssegnatoA);
                writer.Write("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.Write("DataAssegnazione", m_DataAssegnazione);
                writer.Write("StatoRichiesta", m_StatoRichiesta);
                writer.Write("DataCompletamento", m_DataCompletamento);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IDAmministrazione", IDAmministrazione);
                writer.Write("NomeAmministrazione", m_NomeAmministrazione);
                writer.Write("IDDelega", IDDelega);
                writer.Write("IDDocRic", IDDocumentoRiconoscimento);
                writer.Write("IDCF", IDCodiceFiscale);
                // writer.Write("IDAllegato", Me.IDAllegato)
                writer.Write("Note", m_Note);
                writer.Write("SourceType", SourceType);
                writer.Write("SourceID", SourceID);
                writer.Write("Messaggi", DMD.XML.Utils.Serializer.Serialize(Messaggi));
                writer.Write("Allegati", DMD.XML.Utils.Serializer.Serialize(Allegati));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c  = table.Fields.Ensure("IDRichiedente", typeof(int), 1);
                c = table.Fields.Ensure("NomeRichiedente", typeof(string), 255);
                c = table.Fields.Ensure("DataRichiesta", typeof(DateUtils), 1);
                c = table.Fields.Ensure("IDAssegnatoA", typeof(int), 1);
                c = table.Fields.Ensure("NomeAssegnatoA", typeof(string), 255);
                c = table.Fields.Ensure("DataAssegnazione", typeof(DateUtils), 1);
                c = table.Fields.Ensure("StatoRichiesta", typeof(int), 1);
                c = table.Fields.Ensure("DataCompletamento", typeof(DateUtils), 1);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("IDAmministrazione", typeof(int), 1);
                c = table.Fields.Ensure("NomeAmministrazione", typeof(string), 255);
                c = table.Fields.Ensure("IDDelega", typeof(int), 1);
                c = table.Fields.Ensure("IDDocRic", typeof(int), 1);
                c = table.Fields.Ensure("IDCF", typeof(int), 1);
                c = table.Fields.Ensure("Note", typeof(string), 255);
                c = table.Fields.Ensure("SourceType", typeof(string), 255);
                c = table.Fields.Ensure("SourceID", typeof(int), 1);
                c = table.Fields.Ensure("Messaggi", typeof(string), 0);
                c = table.Fields.Ensure("Allegati", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxRichiedente", new string[] { "IDRichiedente", "NomeRichiedente", "DataRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAssegnatoA", new string[] { "IDAssegnatoA", "NomeAssegnatoA", "DataAssegnazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoRichiesta", new string[] { "StatoRichiesta", "DataCompletamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAzienda", new string[] {  "IDAmministrazione", "NomeAmministrazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDelega", new string[] { "IDDelega", "IDDocRic", "IDCF" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "Note" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceID", "SourceType" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Messaggi", typeof(string), 0);
                //c = table.Fields.Ensure("Allegati", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDRichiedente", IDRichiedente);
                writer.WriteAttribute("NomeRichiedente", m_NomeRichiedente);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("IDAssegnatoA", IDAssegnatoA);
                writer.WriteAttribute("NomeAssegnatoA", m_NomeAssegnatoA);
                writer.WriteAttribute("DataAssegnazione", m_DataAssegnazione);
                writer.WriteAttribute("StatoRichiesta", (int?)m_StatoRichiesta);
                writer.WriteAttribute("DataCompletamento", m_DataCompletamento);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDAmministrazione", IDAmministrazione);
                writer.WriteAttribute("NomeAmministrazione", m_NomeAmministrazione);
                writer.WriteAttribute("IDDelega", IDDelega);
                writer.WriteAttribute("IDDocRic", IDDocumentoRiconoscimento);
                writer.WriteAttribute("IDCF", IDCodiceFiscale);
                writer.WriteAttribute("SourceType", SourceType);
                writer.WriteAttribute("SourceID", SourceID);
                base.XMLSerialize(writer);
                writer.WriteTag("Allegati", Allegati);
                writer.WriteTag("Messaggi", Messaggi);
                writer.WriteTag("Note", m_Note);
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
                    case "IDRichiedente":
                        {
                            m_IDRichiedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRichiedente":
                        {
                            m_NomeRichiedente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDAssegnatoA":
                        {
                            m_IDAssegnatoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAssegnatoA":
                        {
                            m_NomeAssegnatoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAssegnazione":
                        {
                            m_DataAssegnazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoRichiesta":
                        {
                            m_StatoRichiesta = (StatoEstrattoContributivo)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataCompletamento":
                        {
                            m_DataCompletamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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

                    case "IDAmministrazione":
                        {
                            m_IDAmministrazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAmministrazione":
                        {
                            m_NomeAmministrazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDelega":
                        {
                            m_IDDelega = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDocRic":
                        {
                            m_IDDocumentoRiconoscimento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCF":
                        {
                            m_IDCodiceFiscale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Allegati":
                        {
                            m_Allegati = (Sistema.CAttachmentsCollection)fieldValue;
                            m_Allegati.SetOwner(this);
                            break;
                        }

                    case "Messaggi":
                        {
                            m_Messages = (Sistema.CAnnotazioni)fieldValue;
                            m_Messages.SetOwner(this);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceType":
                        {
                            m_SourceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Repository
            /// </summary>
            public CEstrattiContributiviClass Repository
            {
                get
                {
                    return (CEstrattiContributiviClass) this.GetModule();
                }
            }

            /// <summary>
            /// Salva e richiede
            /// </summary>
            /// <remarks></remarks>
            public void Richiedi()
            {
                if (StatoRichiesta != StatoEstrattoContributivo.DaRichiedere)
                    throw new InvalidOperationException("Impossibile effettuare la richiesta in questo stato");
                Richiedente = Sistema.Users.CurrentUser;
                DataRichiesta = DMD.DateUtils.Now();
                StatoRichiesta = StatoEstrattoContributivo.Richiesto;
                Save();
                OnRichiesta(new ItemEventArgs<EstrattoContributivo>(this));
            }

            /// <summary>
            /// Genera l'evento Richiesto
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnRichiesta(ItemEventArgs<EstrattoContributivo> e)
            {
                this.Richiesto?.Invoke(this, e);
                this.Repository.OnRichiesto(e);
                //GetModule().DispatchEvent(new Sistema.EventDescription("richiesta", "Richiesta dell'estratto contributivo ID= " + ID, this));
            }

            /// <summary>
            /// Salva e prende in carico la richiesta
            /// </summary>
            /// <remarks></remarks>
            public void PrendiInCarico()
            {
                if (StatoRichiesta != StatoEstrattoContributivo.Richiesto)
                    throw new InvalidOperationException("Impossibile prendere in carico la richiesta in questo stato");
                AssegnatoA = Sistema.Users.CurrentUser;
                DataAssegnazione = DMD.DateUtils.Now();
                StatoRichiesta = StatoEstrattoContributivo.Assegnato;
                Save();
                OnPresaInCarico(new ItemEventArgs<EstrattoContributivo>(this));
            }

            /// <summary>
            /// Genera l'evento PresoInCarico
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnPresaInCarico(ItemEventArgs<EstrattoContributivo> e)
            {
                this.PresoInCarico?.Invoke(this, e);
                this.Repository.OnPresoInCarico(e);
                //// RaiseEvent PresaInCarico(Me, e)
                //GetModule().DispatchEvent(new Sistema.EventDescription("presaincarico", "Presa in carico dell'estratto contributivo ID= " + ID, this));
            }

            /// <summary>
            /// Salva ed evade
            /// </summary>
            /// <remarks></remarks>
            public void Evadi()
            {
                if (StatoRichiesta != StatoEstrattoContributivo.Assegnato && StatoRichiesta != StatoEstrattoContributivo.Sospeso)
                    throw new InvalidOperationException("Impossibile evadere la richiesta in questo stato");
                // If (Me.Allegati.Count = 0) Then Throw New InvalidOperationException("Impossibile evadere la richiesta senza allegare la documentazione")
                DataCompletamento = DMD.DateUtils.Now();
                StatoRichiesta = StatoEstrattoContributivo.Evaso;
                Save();
                OnEvaso(new ItemEventArgs<EstrattoContributivo>(this));
            }

            /// <summary>
            /// Genera l'evento Evaso
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnEvaso(ItemEventArgs<EstrattoContributivo> e)
            {
                this.Evaso?.Invoke(this, e);
                this.Repository.OnEvaso(e);
                //// RaiseEvent PresaInCarico(Me, e)
                //GetModule().DispatchEvent(new Sistema.EventDescription("evaso", "Evaso l'estratto contributivo ID= " + ID, this));
            }

            /// <summary>
            /// Salva ed errore
            /// </summary>
            /// <remarks></remarks>
            public void Errore()
            {
                if (StatoRichiesta != StatoEstrattoContributivo.Assegnato && StatoRichiesta != StatoEstrattoContributivo.Sospeso)
                    throw new InvalidOperationException("Impossibile evadere con errore la richiesta in questo stato");
                DataCompletamento = DMD.DateUtils.Now();
                StatoRichiesta = StatoEstrattoContributivo.Errore;
                Save();
                OnErrore(new ItemEventArgs<EstrattoContributivo>(this));
            }

            /// <summary>
            /// Genera l'evento Errore
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnErrore(ItemEventArgs<EstrattoContributivo> e)
            {
                this.Errato?.Invoke(this, e);
                this.Repository.OnErrato(e);

                //// RaiseEvent PresaInCarico(Me, e)
                //GetModule().DispatchEvent(new Sistema.EventDescription("errore", "Evaso con errore l'estratto contributivo ID= " + ID, this));
            }

            /// <summary>
            /// Aggiunge un nuovo messaggio
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public CAnnotazione AddMessage(string message)
            {
                var note = Messaggi.Add(message);
                note.ForceUser(Sistema.Users.CurrentUser, DMD.DateUtils.Now());
                Save(true);
                //GetModule().DispatchEvent(new Sistema.EventDescription("message", "Comunicazione relativa alla richiesta di estratto ID= " + ID, this));
                return note;
            }

            /// <summary>
            /// Aggiunge un nuovo allegato
            /// </summary>
            /// <param name="att"></param>
            /// <returns></returns>
            public CAttachment AddAllegato(CAttachment att)
            {
                Allegati.Add(att);
                att.ForceUser(Sistema.Users.CurrentUser, DMD.DateUtils.Now());
                Save(true);
                return att;
            }

            
            /// <summary>
            /// Sospende la richiesta
            /// </summary>
            /// <remarks></remarks>
            public void Sospendi()
            {
                if (StatoRichiesta != StatoEstrattoContributivo.Assegnato)
                    throw new InvalidOperationException("Impossibile sospendere la richiesta in questo stato");
                DataCompletamento = DMD.DateUtils.Now();
                StatoRichiesta = StatoEstrattoContributivo.Sospeso;
                Save();
                OnSospeso(new ItemEventArgs<EstrattoContributivo>(this));
            }

            /// <summary>
            /// Sospeso
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnSospeso(ItemEventArgs<EstrattoContributivo> e)
            {
                this.Sospeso?.Invoke(this, e);
                this.Repository.OnSospeso(e);

                //// RaiseEvent PresaInCarico(Me, e)
                //GetModule().DispatchEvent(new Sistema.EventDescription("sospeso", "Sospeso l'estratto contributivo ID= " + ID, this));
            }

            /// <summary>
            /// Restitusice una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Estratto contributivo per ", this.m_NomeCliente, " del ", this.m_DataRichiesta);
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
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is EstrattoContributivo) && this.Equals((EstrattoContributivo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(EstrattoContributivo obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDRichiedente, obj.m_IDRichiedente)
                    && DMD.Strings.EQ(this.m_NomeRichiedente, obj.m_NomeRichiedente)
                    && DMD.DateUtils.EQ(this.m_DataRichiesta, obj.m_DataRichiesta)
                    && DMD.DateUtils.EQ(this.m_DataAssegnazione, obj.m_DataAssegnazione)
                    && DMD.Integers.EQ(this.m_IDAssegnatoA, obj.m_IDAssegnatoA)
                    && DMD.Strings.EQ(this.m_NomeAssegnatoA, obj.m_NomeAssegnatoA)
                    && DMD.Integers.EQ((int)this.m_StatoRichiesta, (int)obj.m_StatoRichiesta)
                    && DMD.DateUtils.EQ(this.m_DataCompletamento, obj.m_DataCompletamento)
                    && DMD.Integers.EQ((int)this.m_IDCliente, (int)obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Integers.EQ(this.m_IDAmministrazione, obj.m_IDAmministrazione)
                    && DMD.Strings.EQ(this.m_NomeAmministrazione, obj.m_NomeAmministrazione)
                    && DMD.Integers.EQ(this.m_IDDelega, obj.m_IDDelega)
                    && DMD.Integers.EQ(this.m_IDDocumentoRiconoscimento, obj.m_IDDocumentoRiconoscimento)
                    && DMD.Integers.EQ(this.m_IDCodiceFiscale, obj.m_IDCodiceFiscale)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Strings.EQ(this.m_SourceType, obj.m_SourceType)
                    && DMD.Integers.EQ(this.m_SourceID, obj.m_SourceID)
                    ;
    
            }
        }
    }
}