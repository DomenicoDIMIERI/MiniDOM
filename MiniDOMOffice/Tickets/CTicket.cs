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
        /// Valori enumerativi che indicano lo stato di una segnalazione
        /// </summary>
        /// <remarks></remarks>
        public enum TicketStatus : int
        {
            /// <summary>
            /// Ticket inserito ma non ancora aperto
            /// </summary>
            /// <remarks></remarks>
            INSERITO = 0,

            /// <summary>
            /// Aperto
            /// </summary>
            /// <remarks></remarks>
            APERTO = 1,

            /// <summary>
            /// Preso in carico
            /// </summary>
            /// <remarks></remarks>
            INLAVORAZIONE = 2,

            /// <summary>
            /// Risolto
            /// </summary>
            /// <remarks></remarks>
            RISOLTO = 3,

            /// <summary>
            /// L'utente ha chiuso il ticket
            /// </summary>
            /// <remarks></remarks>
            NONRISOLVIBILE = 4,

            /// <summary>
            /// Sospeso
            /// </summary>
            /// <remarks></remarks>
            SOSPESO = 5,

            /// <summary>
            /// L'utente ha riaperto il ticket
            /// </summary>
            /// <remarks></remarks>
            RIAPERTO = 6
        }

        /// <summary>
        /// Rappresenta una richiesta fatta da un utente
        /// TODO correggere la gestione degli eventi
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTicket 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<CTicket>
        {
            private int m_IDApertoDa;
            [NonSerialized] private Sistema.CUser m_ApertoDa;
            private DateTime? m_DataRichiesta;
            private string m_NomeApertoDa;
            [NonSerialized] private CUser m_InCaricoA;                    // Utente cha ha preso in carico la segnalazione
            private int m_IDInCaricoA;                // ID dell'utente che ha preso in carico la segnalazione
            private DateTime? m_DataPresaInCarico;
            private DateTime? m_DataChiusura;
            private string m_NomeInCaricoA;               // Nome dell'utente che ha preso in carico la segnalazione
            private string m_Categoria;                   // Categoria del problema
            private string m_Sottocategoria;              // Sottocategoria
            private string m_Messaggio;
            private TicketStatus m_StatoSegnalazione;     // Stato di risoluzione del problema
            private PriorityEnum m_Priorita;
            private int m_IDSupervisore;
            [NonSerialized] private CUser m_Supervisore;
            private string m_NomeSupervisore;
            private string m_Canale;
            [NonSerialized] private CCollection<CAttachment> m_Attachments;
            private int m_IDCliente;
            [NonSerialized] private CPersona m_Cliente;
            private string m_NomeCliente;
            private CTicketAnswaresCollection m_Messages;
            private string m_TipoContesto;
            private int m_IDContesto;
            private int m_IDPostazione;
            [NonSerialized] private CPostazione m_Postazione;
            private string m_NomePostazione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicket()
            {
                m_InCaricoA = null;
                m_IDInCaricoA = 0;
                m_NomeInCaricoA = "";
                m_Messaggio = DMD.Strings.vbNullString;
                m_Categoria = DMD.Strings.vbNullString;
                m_StatoSegnalazione = TicketStatus.INSERITO;
                m_Priorita = PriorityEnum.PRIORITY_NORMAL;
                m_IDSupervisore = 0;
                m_Supervisore = null;
                m_NomeSupervisore = DMD.Strings.vbNullString;
                m_ApertoDa = null;
                m_IDApertoDa = 0;
                m_NomeApertoDa = DMD.Strings.vbNullString;
                m_DataRichiesta = default;
                m_DataPresaInCarico = default;
                m_DataChiusura = default;
                m_Canale = DMD.Strings.vbNullString;
                m_Attachments = null;
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_Messages = null;
                m_TipoContesto = "";
                m_IDContesto = 0;
                m_IDPostazione = 0;
                m_Postazione = null;
                m_NomePostazione = "";
            }

            /// <summary>
            /// Compara due ticket in base al numero 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CTicket other)
            {
                return Strings.Compare(this.NumberEx, other.NumberEx, true);
            }


            int IComparable.CompareTo(object obj) { return this.CompareTo((CTicket)obj);  }

            /// <summary>
            /// Restituisce o imposta l'id della postazione interna da cui é stato aperto il ticket
            /// </summary>
            public int IDPostazione
            {
                get
                {
                    return DBUtils.GetID(m_Postazione, m_IDPostazione);
                }

                set
                {
                    int oldvalue = IDPostazione;
                    if (oldvalue == value)
                        return;
                    m_IDPostazione = value;
                    m_Postazione = null;
                    DoChanged("idpostazione", value, oldvalue);
                }
            }

            /// <summary>
            /// Postazione per cui é stato aperto il ticket
            /// </summary>
            public CPostazione Postazione
            {
                get
                {
                    if (m_Postazione is null)
                        m_Postazione = Anagrafica.Postazioni.GetItemById(m_IDPostazione);
                    return m_Postazione;
                }

                set
                {
                    var oldvalue = Postazione;
                    if (ReferenceEquals(oldvalue, value))
                        return;
                    m_Postazione = value;
                    m_IDPostazione = DBUtils.GetID(value, 0);
                    m_NomePostazione =  (value is object)? value.Nome : "";
                    DoChanged("postazione", value, oldvalue);
                }
            }

            /// <summary>
            /// Nome della postazione
            /// </summary>
            public string NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }

                set
                {
                    string oldvalue = m_NomePostazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldvalue ?? "") == (value ?? ""))
                        return;
                    m_NomePostazione = value;
                    DoChanged("nomepostazione", value, oldvalue);
                }
            }

            /// <summary>
            /// Data dell'utilmo aggiornamento del ticket
            /// </summary>
            /// <returns></returns>
            public DateTime GetDataUltimoAggiornamento()
            {
                var ret = DataRichiesta;
                ret = DMD.DateUtils.Max(ret, DataPresaInCarico);
                ret = DMD.DateUtils.Max(ret, DataChiusura);
                foreach (var m in this.Messages)
                    ret = DMD.DateUtils.Max(ret, m.Data);
                if (ret.HasValue)
                {
                    return (DateTime)ret;
                }
                else
                {
                    return (DateTime)DMD.DateUtils.Max(CreatoIl, ModificatoIl);
                }
            }

            /// <summary>
            /// Tipo del contesto in cui é stato aperto il ticket
            /// </summary>
            public string TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContesto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContesto = value;
                    DoChanged("TipoContesto", value, oldValue);
                }
            }

            /// <summary>
            /// ID del contesto in cui é stato aperto il ticket
            /// </summary>
            public int IDContesto
            {
                get
                {
                    return m_IDContesto;
                }

                set
                {
                    int oldValue = m_IDContesto;
                    if (oldValue == value)
                        return;
                    m_IDContesto = value;
                    DoChanged("IDContesto", value, oldValue);
                }
            }

            /// <summary>
            /// ID del cliente a cui fa riferimento il ticket
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
            /// Collezione dei messaggi scambiati con il cliente
            /// </summary>
            public CTicketAnswaresCollection Messages
            {
                get
                {
                    lock (this)
                    {
                        if (m_Messages is null)
                            m_Messages = new CTicketAnswaresCollection(this);
                        return m_Messages;
                    }
                }
            }

            /// <summary>
            /// Cliente
            /// </summary>
            public Anagrafica.CPersona Cliente
            {
                get
                {
                    lock (this)
                    {
                        if (m_Cliente is null)
                            m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                        return m_Cliente;
                    }
                }

                set
                {
                    Anagrafica.CPersona oldValue;
                    lock (this)
                    {
                        oldValue = m_Cliente;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Cliente = value;
                        m_IDCliente = DBUtils.GetID(value, 0);
                        m_NomeCliente = (value is object)? value.Nominativo : "";
                    }

                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del cliente
            /// </summary>
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
            /// Allegati inseriti dal cliente all'apertura del ticket
            /// </summary>
            public CCollection<Sistema.CAttachment> Attachments
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attachments is null)
                            m_Attachments = new CCollection<Sistema.CAttachment>(new Sistema.CAttachmentsCollection(this));
                        return m_Attachments;
                    }
                }
            }

            /// <summary>
            /// Numero del ticket
            /// </summary>
            public string NumberEx
            {
                get
                {
                    return Sistema.RPC.FormatID(ID);
                }
            }

            /// <summary>
            /// Canale 
            /// </summary>
            public string Canale
            {
                get
                {
                    return m_Canale;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Canale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Canale = value;
                    DoChanged("Canale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui è stata effettuata la richiesta di assistenza
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
            /// Data di presa in carico
            /// </summary>
            public DateTime? DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }

                set
                {
                    var oldValue = m_DataPresaInCarico;
                    if (oldValue == value == true)
                        return;
                    m_DataPresaInCarico = value;
                    DoChanged("DataPresaInCarico", value, oldValue);
                }
            }

            /// <summary>
            /// Data di chiusura
            /// </summary>
            public DateTime? DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }

                set
                {
                    var oldValue = m_DataChiusura;
                    if (oldValue == value == true)
                        return;
                    m_DataChiusura = value;
                    DoChanged("DataChiusura", value, oldValue);
                }
            }

            /// <summary>
            /// Priorità
            /// </summary>
            public PriorityEnum Priorita
            {
                get
                {
                    return m_Priorita;
                }

                set
                {
                    var oldValue = m_Priorita;
                    if (oldValue == value)
                        return;
                    m_Priorita = value;
                    DoChanged("Priorita", value, oldValue);
                }
            }

            /// <summary>
            /// ID del supervisore
            /// </summary>
            public int IDSupervisore
            {
                get
                {
                    return DBUtils.GetID(m_Supervisore, m_IDSupervisore);
                }

                set
                {
                    int oldValue = IDSupervisore;
                    if (oldValue == value)
                        return;
                    m_IDSupervisore = value;
                    m_Supervisore = null;
                    DoChanged("IDSupervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Supervisore
            /// </summary>
            public Sistema.CUser Supervisore
            {
                get
                {
                    if (m_Supervisore is null)
                        m_Supervisore = Sistema.Users.GetItemById(m_IDSupervisore);
                    return m_Supervisore;
                }

                set
                {
                    var oldValue = m_Supervisore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Supervisore = null;
                    m_IDSupervisore = DBUtils.GetID(value, 0);
                    m_NomeSupervisore = (value is object)? value.Nominativo : "";
                    DoChanged("Supervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del supervisore
            /// </summary>
            public string NomeSupervisore
            {
                get
                {
                    return m_NomeSupervisore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeSupervisore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSupervisore = value;
                    DoChanged("NomeSupervisore", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente che ha aperto il ticket
            /// </summary>
            public int IDApertoDa
            {
                get
                {
                    return DBUtils.GetID(m_ApertoDa, m_IDApertoDa);
                }

                set
                {
                    int oldValue = IDApertoDa;
                    if (oldValue == value)
                        return;
                    m_IDApertoDa = value;
                    m_ApertoDa = null;
                    DoChanged("ApertoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Utente che ha aperto il ticket
            /// </summary>
            public Sistema.CUser ApertoDa
            {
                get
                {
                    if (m_ApertoDa is null)
                        m_ApertoDa = Sistema.Users.GetItemById(m_IDApertoDa);
                    return m_ApertoDa;
                }

                set
                {
                    var oldValue = m_ApertoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ApertoDa = value;
                    m_IDApertoDa = DBUtils.GetID(value, 0);
                    m_NomeApertoDa = (value is object)? value.Nominativo : "";
                    DoChanged("ApertoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'utente che ha aperto il ticket
            /// </summary>
            public string NomeApertoDa
            {
                get
                {
                    return m_NomeApertoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeApertoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeApertoDa = value;
                    DoChanged("NomeApertoDa", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha preso in carico la segnalazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDInCaricoA
            {
                get
                {
                    return DBUtils.GetID(m_InCaricoA, m_IDInCaricoA);
                }

                set
                {
                    int oldValue = IDInCaricoA;
                    if (oldValue == value)
                        return;
                    m_InCaricoA = null;
                    m_IDInCaricoA = value;
                    DoChanged("InCaricoAID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisec o imposta l'Utente che ha preso in carico la segnalazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser InCaricoA
            {
                get
                {
                    if (m_InCaricoA is null)
                        m_InCaricoA = Sistema.Users.GetItemById(m_IDInCaricoA);
                    return m_InCaricoA;
                }

                set
                {
                    // Dim oldValue As CUser = Me.InCaricoA
                    m_InCaricoA = value;
                    m_IDInCaricoA = DBUtils.GetID(value, 0);
                    m_NomeInCaricoA = (value is object)? value.Nominativo : "";
                    DoChanged("InCaricoA", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha preso in carico la segnalazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeInCaricoA
            {
                get
                {
                    return m_NomeInCaricoA;
                }

                set
                {
                    string oldValue = m_NomeInCaricoA;
                    value = Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeInCaricoA = value;
                    DoChanged("InCaricoANome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria del problema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    string oldValue = m_Categoria;
                    value = Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sotto-categoria del problema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Sottocategoria
            {
                get
                {
                    return m_Sottocategoria;
                }

                set
                {
                    string oldValue = m_Sottocategoria;
                    value = Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sottocategoria = value;
                    DoChanged("Sottocategoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Messaggio
            {
                get
                {
                    return m_Messaggio;
                }

                set
                {
                    string oldValue = m_Messaggio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Messaggio = value;
                    DoChanged("Messaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della segnalazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TicketStatus StatoSegnalazione
            {
                get
                {
                    return m_StatoSegnalazione;
                }

                set
                {
                    var oldValue = m_StatoSegnalazione;
                    if (oldValue == value)
                        return;
                    m_StatoSegnalazione = value;
                    DoChanged("StatoSegnalazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray(this.NumberEx, " - ", this.DataRichiesta, " - ", this.NomeApertoDa , " : ", this.Messaggio);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.NumberEx.GetHashCode();
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CTicket) && this.Equals((CTicket)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CTicket obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDApertoDa, obj.m_IDApertoDa)
                    && DMD.DateUtils.EQ(this.m_DataRichiesta, obj.m_DataRichiesta)
                    && DMD.Strings.EQ(this.m_NomeApertoDa, obj.m_NomeApertoDa)
                    && DMD.Integers.EQ(this.m_IDInCaricoA, obj.m_IDInCaricoA)
                    && DMD.DateUtils.EQ(this.m_DataPresaInCarico, obj.m_DataPresaInCarico)
                    && DMD.DateUtils.EQ(this.m_DataChiusura, obj.m_DataChiusura)
                    && DMD.Strings.EQ(this.m_NomeInCaricoA, obj.m_NomeInCaricoA)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Strings.EQ(this.m_Sottocategoria, obj.m_Sottocategoria)
                    && DMD.Strings.EQ(this.m_Messaggio, obj.m_Messaggio)
                    && DMD.Integers.EQ((int)this.m_StatoSegnalazione, (int) obj.m_StatoSegnalazione)
                    && DMD.Integers.EQ((int)this.m_Priorita, (int)obj.m_Priorita)
                    && DMD.Integers.EQ(this.m_IDSupervisore, obj.m_IDSupervisore)
                    && DMD.Strings.EQ(this.m_NomeSupervisore, obj.m_NomeSupervisore)
                    && DMD.Strings.EQ(this.m_Canale, obj.m_Canale)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Strings.EQ(this.m_TipoContesto, obj.m_TipoContesto)
                    && DMD.Integers.EQ(this.m_IDContesto, obj.m_IDContesto)
                    && DMD.Integers.EQ(this.m_IDPostazione, obj.m_IDPostazione)
                    && DMD.Strings.EQ(this.m_NomePostazione, obj.m_NomePostazione)
                    ;
            }

         
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Tickets;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SupportTickets";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                
                m_IDInCaricoA = reader.Read("InCaricoA", m_IDInCaricoA);
                m_NomeInCaricoA = reader.Read("InCaricoANome", m_NomeInCaricoA);
                m_Categoria = reader.Read("Categoria", m_Categoria);
                m_Sottocategoria = reader.Read("Sottocategoria", m_Sottocategoria);
                m_Messaggio = reader.Read("Messaggio", m_Messaggio);
                m_StatoSegnalazione = reader.Read("StatoSegnalazione", m_StatoSegnalazione);
                m_IDApertoDa = reader.Read("ApertoDa", m_IDApertoDa);
                m_NomeApertoDa = reader.Read("NomeApertoDa", m_NomeApertoDa);
                m_Priorita = reader.Read("Priorita", m_Priorita);
                m_IDSupervisore = reader.Read("IDSupervisore", m_IDSupervisore);
                m_NomeSupervisore = reader.Read("NomeSupervisore", m_NomeSupervisore);
                m_DataRichiesta = reader.Read("DataRichiesta", m_DataRichiesta);
                m_DataPresaInCarico = reader.Read("DataPresaInCarico", m_DataPresaInCarico);
                m_DataChiusura = reader.Read("DataChiusura", m_DataChiusura);
                m_Canale = reader.Read("Canale", m_Canale);
                m_IDCliente = reader.Read("IDCliente", m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                m_TipoContesto = reader.Read("TipoContesto", m_TipoContesto);
                m_IDContesto = reader.Read("IDContesto", m_IDContesto);
                m_IDPostazione = reader.Read("IDPostazione", m_IDPostazione);
                m_NomePostazione = reader.Read("NomePostazione", m_NomePostazione);

                // Try
                // str = reader.Read("Messages", "")
                // If (str <> "") Then
                // tmp = DMD.XML.Utils.Serializer.Deserialize(str)
                // Me.m_Messages = New CCollection(Of CTicketAnsware)
                // Me.m_Messages.AddRange(tmp)
                // End If
                // Catch ex As Exception

                // End Try

                var str = reader.Read("Attachments", "");
                if (!string.IsNullOrEmpty(str))
                {
                    m_Attachments = new CCollection<Sistema.CAttachment>();
                    m_Attachments.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(str));
                }

                return base.LoadFromRecordset(reader);
            }

         
            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("InCaricoA", IDInCaricoA);
                writer.Write("InCaricoANome", m_NomeInCaricoA);
                writer.Write("Categoria", m_Categoria);
                writer.Write("Sottocategoria", m_Sottocategoria);
                writer.Write("Messaggio", m_Messaggio);
                writer.Write("StatoSegnalazione", m_StatoSegnalazione);
                writer.Write("ApertoDa", IDApertoDa);
                writer.Write("NomeApertoDa", m_NomeApertoDa);
                writer.Write("Priorita", m_Priorita);
                writer.Write("IDSupervisore", IDSupervisore);
                writer.Write("NomeSupervisore", m_NomeSupervisore);
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("DataChiusura", m_DataChiusura);
                writer.Write("Canale", m_Canale);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                // writer.Write("Messages", DMD.XML.Utils.Serializer.Serialize(Me.Messages))
                writer.Write("Attachments", DMD.XML.Utils.Serializer.Serialize(Attachments));
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("IDPostazione", IDPostazione);
                writer.Write("NomePostazione", m_NomePostazione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("InCaricoA", typeof(int), 1);
                c = table.Fields.Ensure("InCaricoANome", typeof(string), 255);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Sottocategoria", typeof(string), 255);
                c = table.Fields.Ensure("Messaggio", typeof(string), 0);
                c = table.Fields.Ensure("StatoSegnalazione", typeof(int), 1);
                c = table.Fields.Ensure("ApertoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeApertoDa", typeof(string), 255);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
                c = table.Fields.Ensure("IDSupervisore", typeof(int), 1);
                c = table.Fields.Ensure("NomeSupervisore", typeof(string), 255);
                c = table.Fields.Ensure("DataRichiesta", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataPresaInCarico", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataChiusura", typeof(DateTime), 1);
                c = table.Fields.Ensure("Canale", typeof(string), 255);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("TipoContesto", typeof(string), 255);
                c = table.Fields.Ensure("IDContesto", typeof(int), 1);
                c = table.Fields.Ensure("IDPostazione", typeof(int), 1);
                c = table.Fields.Ensure("NomePostazione", typeof(string), 255);
                c = table.Fields.Ensure("Attachments", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxInCarico", new string[] { "InCaricoA", "InCaricoANome" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "Sottocategoria" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMessaggio", new string[] { "Messaggio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoSegnalazione", new string[] { "StatoSegnalazione", "Priorita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxApertoDa", new string[] { "ApertoDa", "NomeApertoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSupervisore", new string[] { "IDSupervisore", "NomeSupervisore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataRichiesta", "DataPresaInCarico", "DataChiusura" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContesto", new string[] { "TipoContesto", "IDContesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPostazione", new string[] { "IDPostazione", "NomePostazione", "Canale" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Attachments", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("InCaricoA", IDInCaricoA);
                writer.WriteAttribute("InCaricoANome", m_NomeInCaricoA);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("Sottocategoria", m_Sottocategoria);
                writer.WriteAttribute("StatoSegnalazione", (int?)m_StatoSegnalazione);
                writer.WriteAttribute("ApertoDa", IDApertoDa);
                writer.WriteAttribute("NomeApertoDa", m_NomeApertoDa);
                writer.WriteAttribute("IDSupervisore", IDSupervisore);
                writer.WriteAttribute("NomeSupervisore", m_NomeSupervisore);
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("DataChiusura", m_DataChiusura);
                writer.WriteAttribute("Canale", m_Canale);
                writer.WriteAttribute("Priorita", (int?)m_Priorita);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("IDPostazione", IDPostazione);
                writer.WriteAttribute("NomePostazione", m_NomePostazione);
                base.XMLSerialize(writer);
                writer.WriteTag("Messaggio", m_Messaggio);
                writer.WriteTag("Attachments", Attachments);
                // writer.WriteTag("Messages", Me.Messages)
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
                    case "InCaricoA":
                        {
                            m_IDInCaricoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "InCaricoANome":
                        {
                            m_NomeInCaricoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Sottocategoria":
                        {
                            m_Sottocategoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Messaggio":
                        {
                            m_Messaggio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoSegnalazione":
                        {
                            m_StatoSegnalazione = (TicketStatus)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ApertoDa":
                        {
                            m_IDApertoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeApertoDa":
                        {
                            m_NomeApertoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Priorita":
                        {
                            m_Priorita = (PriorityEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDSupervisore":
                        {
                            m_IDSupervisore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeSupervisore":
                        {
                            m_NomeSupervisore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataChiusura":
                        {
                            m_DataChiusura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Canale":
                        {
                            m_Canale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attachments":
                        {
                            m_Attachments = new CCollection<Sistema.CAttachment>();
                            m_Attachments.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                    // Case "Messages" : Me.m_Messages = New CCollection(Of CTicketAnsware) : Me.m_Messages.AddRange(fieldValue)
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

                    case "TipoContesto":
                        {
                            m_TipoContesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPostazione":
                        {
                            m_IDPostazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePostazione":
                        {
                            m_NomePostazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Apre il ticket
            /// </summary>
            /// <remarks></remarks>
            public void Apri()
            {
                if (m_StatoSegnalazione != TicketStatus.INSERITO)
                    throw new InvalidOperationException("Stato non valido");
                StatoSegnalazione = TicketStatus.APERTO;
                DataRichiesta = DMD.DateUtils.Now();
                ApertoDa = Sistema.Users.CurrentUser;
                Save();
                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_open", "Apertura del ticket #" + NumberEx, this));
            }

            /// <summary>
            /// Prende in carico
            /// </summary>
            /// <remarks></remarks>
            public void PrendiInCarico()
            {
                if (m_StatoSegnalazione != TicketStatus.APERTO && m_StatoSegnalazione != TicketStatus.RIAPERTO)
                    throw new InvalidOperationException("Stato non valido");
                StatoSegnalazione = TicketStatus.INLAVORAZIONE;
                DataPresaInCarico = DMD.DateUtils.Now();
                InCaricoA = Sistema.Users.CurrentUser;
                Save();
                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_work", "Presa in carico del ticket #" + NumberEx, this));
            }

            /// <summary>
            /// Chiude il ticket come risolto
            /// </summary>
            /// <remarks></remarks>
            public void Risolto()
            {
                if (m_StatoSegnalazione != TicketStatus.INLAVORAZIONE && m_StatoSegnalazione != TicketStatus.SOSPESO)
                    throw new InvalidOperationException("Stato non valido");
                StatoSegnalazione = TicketStatus.RISOLTO;
                DataChiusura = DMD.DateUtils.Now();
                InCaricoA = Sistema.Users.CurrentUser;
                Save();
                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_closed", "Chiusura del ticket #" + NumberEx, this));
            }

            /// <summary>
            /// Chiude il ticket come non risolvibile
            /// </summary>
            /// <remarks></remarks>
            public void Errore()
            {
                if (m_StatoSegnalazione != TicketStatus.INLAVORAZIONE && m_StatoSegnalazione != TicketStatus.SOSPESO)
                    throw new InvalidOperationException("Stato non valido");
                StatoSegnalazione = TicketStatus.NONRISOLVIBILE;
                DataChiusura = DMD.DateUtils.Now();
                InCaricoA = Sistema.Users.CurrentUser;
                Save();
                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_error", "Chiusura con errore del ticket #" + NumberEx, this));
            }

            /// <summary>
            /// Riapre il ticket
            /// </summary>
            /// <remarks></remarks>
            public void Riapri()
            {
                if (m_StatoSegnalazione != TicketStatus.RISOLTO && m_StatoSegnalazione != TicketStatus.NONRISOLVIBILE)
                    throw new InvalidOperationException("Stato non valido");
                StatoSegnalazione = TicketStatus.APERTO;
                Save();
                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_resume", "Riapertura del ticket #" + NumberEx, this));
            }

            /// <summary>
            /// Sospende il ticket
            /// </summary>
            /// <remarks></remarks>
            public void Sospendi()
            {
                if (m_StatoSegnalazione != TicketStatus.INLAVORAZIONE)
                    throw new InvalidOperationException("Stato non valido");
                StatoSegnalazione = TicketStatus.SOSPESO;
                Save();
                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_suspended", "Sospensione del ticket #" + NumberEx, this));
            }

            /// <summary>
            /// Aggiunge un messaggio
            /// </summary>
            /// <param name="msg"></param>
            public void AddMessage(CTicketAnsware msg)
            {
                var oldStato = StatoSegnalazione;
                this.Messages.Add(msg);
                this.Messages.Sort();
                this.StatoSegnalazione = msg.StatoTicket;
                Save(true);
                msg.Save();
                if (oldStato != StatoSegnalazione)
                {
                    switch (StatoSegnalazione)
                    {
                        case TicketStatus.APERTO:
                        case TicketStatus.RIAPERTO:
                        case TicketStatus.INSERITO:
                            {
                                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_resume", "Riapertura del ticket #" + NumberEx, this));
                                break;
                            }

                        case TicketStatus.INLAVORAZIONE:
                            {
                                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_work", "Presa in carico del ticket #" + NumberEx, this));
                                break;
                            }

                        case TicketStatus.NONRISOLVIBILE:
                            {
                                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_error", "Chiusura con errore del ticket #" + NumberEx, this));
                                break;
                            }

                        case TicketStatus.RISOLTO:
                            {
                                GetModule().DispatchEvent(new Sistema.EventDescription("ticket_closed", "Chiusura del ticket #" + NumberEx, this));
                                break;
                            }
                    }
                }
                else
                {
                    GetModule().DispatchEvent(new Sistema.EventDescription("ticket_message", "Aggiornamento del ticket #" + NumberEx, this));
                }
            }

              
        }
    }
}