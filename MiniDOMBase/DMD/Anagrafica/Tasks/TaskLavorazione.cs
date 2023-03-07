using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom.repositories;
using static minidom.Anagrafica;
using static minidom.Sistema;


namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta uno stato di lavorazione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class TaskLavorazione
            : Databases.DBObjectPO
        {
            private int m_IDCliente;
            [NonSerialized] private CPersona m_Cliente;
            private string m_NomeCliente;
            private int m_IDStatoAttuale;
            [NonSerialized] private StatoTaskLavorazione m_StatoAttuale;
            private DateTime m_DataAssegnazione;
            private int m_IDAssegnatoDa;
            [NonSerialized] private Sistema.CUser m_AssegnatoDa;
            private DateTime? m_DataPrevista;
            private int m_IDAssegnatoA;
            [NonSerialized] private Sistema.CUser m_AssegnatoA;
            private string m_Note;
            private int m_IDAzioneEseguita;
            // Private m_AzioneEseguita As AzioneTaskLavorazione

            private string m_Categoria;       // Nome della categoria a cui appartiene il task
            private int m_Priorita;       // Priorità (crescente più importante)
            private int m_IDSorgente;     // ID dell'oggetto da cui è partito il task
            private string m_TipoSorgente;    // Tipo dell'oggetto da cui è partito il task
            private object m_Sorgente;        // Oggetto da cui è partito il task
            private int m_IDContesto;     // ID del contesto in cui è stato generato il task
            private string m_TipoContesto;    // Tipo del contesto in cui è stato generato il task
            private int m_IDRegolaEseguita; // ID della regola la cui esecuzione ha portato allo stato corrente
            [NonSerialized] private RegolaTaskLavorazione m_RegolaEseguita;   // Regola la cui esecuzione ha portato allo stato corrente
            private string m_ParametriAzione;
            private string m_RisultatoAzione;
            private DateTime? m_DataInizioEsecuzione;
            private DateTime? m_DataFineEsecuzione;
            private int m_IDTaskSorgente;
            [NonSerialized] private TaskLavorazione m_TaskSorgente;
            private int m_IDTaskDestinazione;
            [NonSerialized] private TaskLavorazione m_TaskDestinazione;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public TaskLavorazione()
            {
                m_IDCliente = 0;
                m_Cliente = null;
                m_NomeCliente = "";
                m_IDStatoAttuale = 0;
                m_StatoAttuale = null;
                m_DataAssegnazione = DMD.DateUtils.Now();
                m_IDAssegnatoDa = 0;
                m_AssegnatoDa = null;
                m_DataPrevista = default;
                m_IDAssegnatoA = 0;
                m_AssegnatoA = null;
                m_Note = "";
                m_IDAzioneEseguita = 0;
                // Me.m_AzioneEseguita = Nothing

                m_Categoria = "";
                m_Priorita = 0;
                m_IDSorgente = 0;
                m_TipoSorgente = "";
                m_Sorgente = null;
                m_IDContesto = 0;
                m_TipoContesto = "";
                m_IDRegolaEseguita = 0;
                m_RegolaEseguita = null;
                m_ParametriAzione = "";
                m_RisultatoAzione = "";
                m_DataInizioEsecuzione = default;
                m_DataFineEsecuzione = default;
                m_IDTaskDestinazione = 0;
                m_IDTaskSorgente = 0;
            }

            
            /// <summary>
            /// Restituisce o imposta i parametri passati all'azione per eseguire il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ParametriAzione
            {
                get
                {
                    return m_ParametriAzione;
                }

                set
                {
                    string oldValue = m_ParametriAzione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ParametriAzione = value;
                    DoChanged("ParametriAzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il risultato dell'esecuzione dell'azione sul task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string RisultatoAzione
            {
                get
                {
                    return m_RisultatoAzione;
                }

                set
                {
                    string oldValue = m_RisultatoAzione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RisultatoAzione = value;
                    DoChanged("RisultatoAzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di esecuzione dell'azione sul task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizioEsecuzione
            {
                get
                {
                    return m_DataInizioEsecuzione;
                }

                set
                {
                    var oldValue = m_DataInizioEsecuzione;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_DataInizioEsecuzione = value;
                    DoChanged("DataInizioEsecuzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine esecuzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataFineEsecuzione
            {
                get
                {
                    return m_DataFineEsecuzione;
                }

                set
                {
                    var oldValue = m_DataFineEsecuzione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFineEsecuzione = value;
                    DoChanged("DataFineEsecuzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del task di provenienza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDTaskSorgente
            {
                get
                {
                    return DBUtils.GetID(m_TaskSorgente, m_IDTaskSorgente);
                }

                set
                {
                    int oldValue = IDTaskSorgente;
                    if (oldValue == value)
                        return;
                    m_IDTaskSorgente = value;
                    m_TaskSorgente = null;
                    DoChanged("IDTaskSorgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il task di provenienza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TaskLavorazione TaskSorgente
            {
                get
                {
                    if (m_TaskSorgente is null)
                        m_TaskSorgente = TasksDiLavorazione.GetItemById(m_IDTaskSorgente);
                    return m_TaskSorgente;
                }

                set
                {
                    var oldValue = TaskSorgente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TaskSorgente = value;
                    m_IDTaskSorgente = DBUtils.GetID(value, 0);
                    DoChanged("TaskSorgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del task di destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDTaskDestinazione
            {
                get
                {
                    return DBUtils.GetID(m_TaskDestinazione, m_IDTaskDestinazione);
                }

                set
                {
                    int oldValue = IDTaskDestinazione;
                    if (oldValue == value)
                        return;
                    m_IDTaskDestinazione = value;
                    m_TaskDestinazione = null;
                    DoChanged("TaskDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il task di destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TaskLavorazione TaskDestinazione
            {
                get
                {
                    if (m_TaskDestinazione is null)
                        m_TaskDestinazione = TasksDiLavorazione.GetItemById(m_IDTaskSorgente);
                    return m_TaskDestinazione;
                }

                set
                {
                    var oldValue = TaskDestinazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TaskDestinazione = value;
                    m_IDTaskDestinazione = DBUtils.GetID(value, 0);
                    DoChanged("TaskDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della regola la cui esecuzione ha portato allo stato corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRegolaEseguita
            {
                get
                {
                    return DBUtils.GetID(m_RegolaEseguita, m_IDRegolaEseguita);
                }

                set
                {
                    int oldValue = IDRegolaEseguita;
                    if (oldValue == value)
                        return;
                    m_RegolaEseguita = null;
                    m_IDRegolaEseguita = value;
                    DoChanged("IDRegolaEseguita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la regola la cui esecuzione ha portato allo stato corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public RegolaTaskLavorazione RegolaEseguita
            {
                get
                {
                    if (m_RegolaEseguita is null)
                        m_RegolaEseguita = RegoleTasksLavorazione.GetItemById(m_IDRegolaEseguita);
                    return m_RegolaEseguita;
                }

                set
                {
                    var oldValue = RegolaEseguita;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RegolaEseguita = value;
                    m_IDRegolaEseguita = DBUtils.GetID(value, 0);
                    DoChanged("RegolaEseguita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria a cui appartiene il task
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
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un numero che indica la priorità crescente assegnata al task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Priorita
            {
                get
                {
                    return m_Priorita;
                }

                set
                {
                    int oldValue = m_Priorita;
                    if (oldValue == value)
                        return;
                    m_Priorita = value;
                    DoChanged("Priorita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'oggetto da cui è partito il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDSorgente
            {
                get
                {
                    return DBUtils.GetID(m_Sorgente, m_IDSorgente);
                }

                set
                {
                    int oldValue = IDSorgente;
                    if (oldValue == value)
                        return;
                    m_IDSorgente = value;
                    m_Sorgente = null;
                    DoChanged("IDSorgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo dell'oggetto che ha generato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoSorgente
            {
                get
                {
                    if (m_Sorgente is object)
                        return DMD.RunTime.vbTypeName(m_Sorgente);
                    return m_TipoSorgente;
                }

                set
                {
                    string oldValue = TipoSorgente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoSorgente = value;
                    DoChanged("TipoSorgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto che ha generato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object Sorgente
            {
                get
                {
                    if (m_Sorgente is null)
                        m_Sorgente = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_TipoSorgente, m_IDSorgente);
                    return m_Sorgente;
                }

                set
                {
                    var oldValue = Sorgente;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_Sorgente = value;
                    m_IDSorgente = DBUtils.GetID(value, 0);
                    if (value is null)
                    {
                        m_TipoSorgente = "";
                    }
                    else
                    {
                        m_TipoSorgente = DMD.RunTime.vbTypeName(value);
                    }

                    DoChanged("Sorgente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del contesto in cui è stato creato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContesto
            {
                get
                {
                    return m_IDContesto;
                }

                set
                {
                    int oldValue = IDContesto;
                    if (oldValue == value)
                        return;
                    m_IDContesto = value;
                    DoChanged("IDContesto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del contesto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }

                set
                {
                    string oldValue = m_TipoContesto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContesto = value;
                    DoChanged("TipoContesto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del cliente in lavorazione
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
            /// Restituisce o imposta il cliente in lavorazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = Cliente;
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
            /// Imposta il cliente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetCliente(CPersona value)
            {
                m_Cliente = value;
                m_IDCliente = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome del cliente
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
            /// Restituisce o imposta l'id dello stato attuale
            /// </summary>
            public int IDStatoAttuale
            {
                get
                {
                    return DBUtils.GetID(m_StatoAttuale, m_IDStatoAttuale);
                }

                set
                {
                    int oldValue = IDStatoAttuale;
                    if (oldValue == value)
                        return;
                    m_IDStatoAttuale = value;
                    m_StatoAttuale = null;
                    DoChanged("IDStatoAttuale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato attuale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoTaskLavorazione StatoAttuale
            {
                get
                {
                    if (m_StatoAttuale is null)
                        m_StatoAttuale = StatiTasksLavorazione.GetItemById(m_IDStatoAttuale);
                    return m_StatoAttuale;
                }

                set
                {
                    var oldValue = StatoAttuale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDStatoAttuale = DBUtils.GetID(value, 0);
                    m_StatoAttuale = value;
                    DoChanged("StatoAttuale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di assegnazione del task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime DataAssegnazione
            {
                get
                {
                    return m_DataAssegnazione;
                }

                set
                {
                    var oldValeu = m_DataAssegnazione;
                    if (DMD.DateUtils.Compare(value, oldValeu) == 0)
                        return;
                    m_DataAssegnazione = value;
                    DoChanged("DataAssegnazione", value, oldValeu);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha creato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAssegnatoDa
            {
                get
                {
                    return DBUtils.GetID(m_AssegnatoDa, m_IDAssegnatoDa);
                }

                set
                {
                    int oldValue = IDAssegnatoDa;
                    if (oldValue == value)
                        return;
                    m_IDAssegnatoDa = value;
                    m_AssegnatoDa = null;
                    DoChanged("IDAssegnatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha creato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser AssegnatoDa
            {
                get
                {
                    if (m_AssegnatoDa is null)
                        m_AssegnatoDa = Sistema.Users.GetItemById(m_IDAssegnatoDa);
                    return m_AssegnatoDa;
                }

                set
                {
                    var oldValue = AssegnatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AssegnatoDa = value;
                    m_IDAssegnatoDa = DBUtils.GetID(value, 0);
                    DoChanged("AssegnatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data prevista per il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }

                set
                {
                    var oldValue = m_DataPrevista;
                    if (oldValue == value == true)
                        return;
                    m_DataPrevista = value;
                    DoChanged("DataPrevista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è stato assegnato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta l'utente a cui è stato assegnato il task
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
                    var oldValue = AssegnatoA;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AssegnatoA = value;
                    m_IDAssegnatoA = DBUtils.GetID(value, 0);
                    DoChanged("AssegnatoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle note inserite in fase di assegnazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta l'ID dell'azione eseguita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAzioneEseguita
            {
                get
                {
                    return m_IDAzioneEseguita; // Return GetID(Me.m_AzioneEseguita, Me.m_IDAzioneEseguita)
                }

                set
                {
                    int oldValue = IDAzioneEseguita;
                    if (oldValue == value)
                        return;
                    m_IDAzioneEseguita = value;
                    // Me.m_AzioneEseguita = Nothing
                    DoChanged("IDAzioneEseguita", value, oldValue);
                }
            }

            // ''' <summary>
            // ''' Restituisce o imposta l'azione eseguita per il task di lavorazione
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property AzioneEseguita As AzioneTaskLavorazione
            // Get
            // If Me.m_AzioneEseguita Is Nothing Then Me.m_AzioneEseguita = Anagrafica.AzioniTasksLavorazione.GetItemById(Me.m_IDAzioneEseguita)
            // Return Me.m_AzioneEseguita
            // End Get
            // Set(value As AzioneTaskLavorazione)
            // Dim oldValue As AzioneTaskLavorazione = Me.AzioneEseguita
            // If (oldValue Is value) Then Exit Property
            // Me.m_AzioneEseguita = value
            // Me.m_IDAzioneEseguita = GetID(value)
            // Me.DoChanged("AzioneEseguita", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.TasksDiLavorazione; //.Module;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return TasksDiLavorazione.Database;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_TaskLavorazione";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente", this.m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", this.m_NomeCliente);
                m_IDStatoAttuale = reader.Read("IDStatoAttuale", this.m_IDStatoAttuale);
                m_DataAssegnazione = reader.Read("DataAssegnazione", this.m_DataAssegnazione);
                m_IDAssegnatoDa = reader.Read("IDAssegnatoDa", this.m_IDAssegnatoDa);
                m_DataPrevista = reader.Read("DataPrevista", this.m_DataPrevista);
                m_IDAssegnatoA = reader.Read("IDAssegnatoA", this.m_IDAssegnatoA);
                m_Note = reader.Read("Note", this.m_Note);
                m_IDAzioneEseguita = reader.Read("IDAzioneEseguita", this.m_IDAzioneEseguita);
                m_Categoria = reader.Read("Categoria", this.m_Categoria);
                m_Priorita = reader.Read("Priorita", this.m_Priorita);
                m_IDSorgente = reader.Read("IDSorgente", this.m_IDSorgente);
                m_TipoSorgente = reader.Read("TipoSorgente", this.m_TipoSorgente);
                m_IDContesto = reader.Read("IDContesto", this.m_IDContesto);
                m_TipoContesto = reader.Read("TipoContesto", this.m_TipoContesto);
                m_IDRegolaEseguita = reader.Read("IDRegolaEseguita", this.m_IDRegolaEseguita);
                m_ParametriAzione = reader.Read("ParametriAzione", this.m_ParametriAzione);
                m_RisultatoAzione = reader.Read("RisultatoAzione", this.m_RisultatoAzione);
                m_DataInizioEsecuzione = reader.Read("DataEsecuzione", this.m_DataInizioEsecuzione);
                m_DataFineEsecuzione = reader.Read("DataFineEsecuzione", this.m_DataFineEsecuzione);
                m_IDTaskDestinazione = reader.Read("IDTaskDestinazione", this.m_IDTaskDestinazione);
                m_IDTaskSorgente = reader.Read("IDTaskSorgente", this.m_IDTaskSorgente);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IDStatoAttuale", IDStatoAttuale);
                writer.Write("DataAssegnazione", m_DataAssegnazione);
                writer.Write("IDAssegnatoDa", IDAssegnatoDa);
                writer.Write("DataPrevista", m_DataPrevista);
                writer.Write("IDAssegnatoA", IDAssegnatoA);
                writer.Write("Note", m_Note);
                writer.Write("IDAzioneEseguita", IDAzioneEseguita);
                writer.Write("Categoria", m_Categoria);
                writer.Write("Priorita", m_Priorita);
                writer.Write("IDSorgente", IDSorgente);
                writer.Write("TipoSorgente", TipoSorgente);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("IDRegolaEseguita", IDRegolaEseguita);
                writer.Write("ParametriAzione", m_ParametriAzione);
                writer.Write("RisultatoAzione", m_RisultatoAzione);
                writer.Write("DataEsecuzione", m_DataInizioEsecuzione);
                writer.Write("DataFineEsecuzione", m_DataFineEsecuzione);
                writer.Write("IDTaskDestinazione", IDTaskDestinazione);
                writer.Write("IDTaskSorgente", IDTaskSorgente);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("IDStatoAttuale", typeof(int), 1);
                c = table.Fields.Ensure("DataAssegnazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDAssegnatoDa", typeof(int), 1);
                c = table.Fields.Ensure("DataPrevista", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDAssegnatoA", typeof(int), 1);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("IDAzioneEseguita", typeof(int), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
                c = table.Fields.Ensure("IDSorgente", typeof(int), 1);
                c = table.Fields.Ensure("TipoSorgente", typeof(string), 255);
                c = table.Fields.Ensure("IDContesto", typeof(int), 1);
                c = table.Fields.Ensure("TipoContesto", typeof(string), 255);
                c = table.Fields.Ensure("IDRegolaEseguita", typeof(int), 1);
                c = table.Fields.Ensure("ParametriAzione", typeof(string), 0);
                c = table.Fields.Ensure("RisultatoAzione", typeof(string), 0);
                c = table.Fields.Ensure("DataEsecuzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFineEsecuzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDTaskDestinazione", typeof(int), 1);
                c = table.Fields.Ensure("IDTaskSorgente", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAssegnato", new string[] { "IDAssegnatoA", "IDAssegnatoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataPrevista", "DataAssegnazione", "DataEsecuzione", "DataFineEsecuzione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoAtt", new string[] { "IDStatoAttuale", "IDTaskSorgente", "IDTaskDestinazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAzione", new string[] { "IDAzioneEseguita", "IDRegolaEseguita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "Priorita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSorgente", new string[] { "IDSorgente", "TipoSorgente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContesto", new string[] { "IDContesto", "TipoContesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "Note" }, DBFieldConstraintFlags.None);
                
               
                //c = table.Fields.Ensure("ParametriAzione", typeof(string), 0);
                //c = table.Fields.Ensure("RisultatoAzione", typeof(string), 0);                 

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDStatoAttuale", IDStatoAttuale);
                writer.WriteAttribute("DataAssegnazione", m_DataAssegnazione);
                writer.WriteAttribute("IDAssegnatoDa", IDAssegnatoDa);
                writer.WriteAttribute("DataPrevista", m_DataPrevista);
                writer.WriteAttribute("IDAssegnatoA", IDAssegnatoA);
                writer.WriteAttribute("Note", m_Note);
                writer.WriteAttribute("IDAzioneEseguita", IDAzioneEseguita);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("Priorita", m_Priorita);
                writer.WriteAttribute("IDSorgente", IDSorgente);
                writer.WriteAttribute("TipoSorgente", TipoSorgente);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("IDRegolaEseguita", IDRegolaEseguita);
                writer.WriteAttribute("ParametriAzione", m_ParametriAzione);
                writer.WriteAttribute("RisultatoAzione", m_RisultatoAzione);
                writer.WriteAttribute("DataEsecuzione", m_DataInizioEsecuzione);
                writer.WriteAttribute("DataFineEsecuzione", m_DataFineEsecuzione);
                writer.WriteAttribute("IDTaskDestinazione", IDTaskDestinazione);
                writer.WriteAttribute("IDTaskSorgente", IDTaskSorgente);
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

                    case "IDStatoAttuale":
                        {
                            m_IDStatoAttuale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataAssegnazione":
                        {
                            m_DataAssegnazione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDAssegnatoDa":
                        {
                            m_IDAssegnatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataPrevista":
                        {
                            m_DataPrevista = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDAssegnatoA":
                        {
                            m_IDAssegnatoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAzioneEseguita":
                        {
                            m_IDAzioneEseguita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Priorita":
                        {
                            m_Priorita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDSorgente":
                        {
                            m_IDSorgente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoSorgente":
                        {
                            m_TipoSorgente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoContesto":
                        {
                            m_TipoContesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRegolaEseguita":
                        {
                            m_IDRegolaEseguita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ParametriAzione":
                        {
                            m_ParametriAzione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RisultatoAzione":
                        {
                            m_RisultatoAzione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataEsecuzione":
                        {
                            m_DataInizioEsecuzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFineEsecuzione":
                        {
                            m_DataFineEsecuzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDTaskDestinazione":
                        {
                            m_IDTaskDestinazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTaskSorgente":
                        {
                            m_IDTaskSorgente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return DMD.Strings.ConcatArray(this.m_NomeCliente, this.m_IDStatoAttuale);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeCliente, this.m_IDStatoAttuale);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is TaskLavorazione) && this.Equals((TaskLavorazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(TaskLavorazione obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Integers.EQ(this.m_IDStatoAttuale, obj.m_IDStatoAttuale)
                    && DMD.DateUtils.EQ(this.m_DataAssegnazione, obj.m_DataAssegnazione)
                    && DMD.Integers.EQ(this.m_IDAssegnatoDa, obj.m_IDAssegnatoDa)
                    && DMD.DateUtils.EQ(this.m_DataPrevista, obj.m_DataPrevista)
                    && DMD.Integers.EQ(this.m_IDAssegnatoA, obj.m_IDAssegnatoA)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Integers.EQ(this.m_IDAzioneEseguita, obj.m_IDAzioneEseguita)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Integers.EQ(this.m_Priorita, obj.m_Priorita)
                    && DMD.Integers.EQ(this.m_IDSorgente, obj.m_IDSorgente)
                    && DMD.Strings.EQ(this.m_TipoSorgente, obj.m_TipoSorgente)
                    && DMD.Integers.EQ(this.m_IDContesto, obj.m_IDContesto)
                    && DMD.Strings.EQ(this.m_TipoContesto, obj.m_TipoContesto)
                    && DMD.Integers.EQ(this.m_IDRegolaEseguita, obj.m_IDRegolaEseguita)
                    && DMD.Strings.EQ(this.m_ParametriAzione, obj.m_ParametriAzione)
                    && DMD.Strings.EQ(this.m_RisultatoAzione, obj.m_RisultatoAzione)
                    && DMD.DateUtils.EQ(this.m_DataInizioEsecuzione, obj.m_DataInizioEsecuzione)
                    && DMD.DateUtils.EQ(this.m_DataFineEsecuzione, obj.m_DataFineEsecuzione)
                    && DMD.Integers.EQ(this.m_IDTaskSorgente, obj.m_IDTaskSorgente)
                    && DMD.Integers.EQ(this.m_IDTaskDestinazione, obj.m_IDTaskDestinazione)
                    ;
            }
        }
    }
}