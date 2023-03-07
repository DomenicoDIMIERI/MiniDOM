using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Rappresenta dei dati aggiuntivi per la pratica (relazione 1 a 1)
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CInfoPratica : Databases.DBObjectBase, ICloneable
        {
            private int m_IDPratica;
            [NonSerialized] private CPraticaCQSPD m_Pratica;
            private int m_IDCommerciale;
            [NonSerialized] private CTeamManager m_Commerciale;
            // Private m_NomeCommerciale As String


            private int m_IDDistributore;
            [NonSerialized] private Anagrafica.CDistributore m_Distributore;

            // Private m_IDPraticaOrigine      '[INT] ID della pratica originaria
            private string m_TrasferitoDaURL; // [TEXT] 
            private DateTime? m_DataTrasferimento; // [DATE] Data e ora del trasferimento della pratica
            private string m_TrasferitoA;        // [TEXT] Stringa che identifica univocamente l'azienda esterna
            private int m_IDTrasferitoDa; // [INT]  ID dell'utente che ha effettuato il trasferimento
            [NonSerialized] private Sistema.CUser m_TrasferitoDa; // [CUser]Oggetto utente che ha effettuato il trasferimento
            private int m_IDPraticaTrasferita;  // [INT]  ID della pratica memorizzata sul sistema dell'azienda esterna
            private DateTime? m_DataAggiornamentoPT; // [DATE] Data e ora dell'ultimo aggiornamento sullo stato della pratica richiesto al sistema esterno
            private int m_EsitoAggiornamentoPT; // [INT]  Codice che identifica il risultato dell'ultimo aggiornamento (0 = ok, ... da definire gli errori) 
            private decimal? m_Costo;
            private int m_IDPraticaDiRiferimento;
            [NonSerialized] private CPraticaCQSPD m_PraticaDiRiferimento;
            private int m_IDCorrettaDa;
            [NonSerialized] private Sistema.CUser m_CorrettaDa;
            private DateTime? m_DataCorrezione;
            private int m_IDCorrezione;
            [NonSerialized] private COffertaCQS m_Correzione;
            private string m_MotivoSconto;
            private string m_MotivoScontoDettaglio;
            private int m_IDScontoAutorizzatoDa;
            [NonSerialized] private Sistema.CUser m_ScontoAutorizzatoDa;
            private DateTime? m_ScontoAutorizzatoIl;
            private string m_ScontoAutorizzatoNote;
            private string m_NoteAmministrative;
            private decimal? m_ValoreUpFront;
            private decimal? m_ValoreProvvTAN;
            private decimal? m_ValoreProvvAGG;
            private decimal? m_ValoreProvvTOT;
            private int m_Flags;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CInfoPratica()
            {
                m_IDPratica = 0;
                m_Pratica = null;
                m_IDPraticaTrasferita = 0;
                m_DataAggiornamentoPT = default;
                m_EsitoAggiornamentoPT = 0;
                m_IDTrasferitoDa = 0;
                m_TrasferitoDa = null;
                m_Costo = default;
                m_IDPraticaDiRiferimento = 0;
                m_PraticaDiRiferimento = null;
                m_IDCommerciale = 0;
                m_Commerciale = null;
                m_IDCorrettaDa = 0;
                m_CorrettaDa = null;
                m_DataCorrezione = default;
                m_IDCorrezione = 0;
                m_Correzione = null;
                m_MotivoSconto = DMD.Strings.vbNullString;
                m_MotivoScontoDettaglio = DMD.Strings.vbNullString;
                m_IDScontoAutorizzatoDa = 0;
                m_ScontoAutorizzatoDa = null;
                m_ScontoAutorizzatoIl = default;
                m_ScontoAutorizzatoNote = DMD.Strings.vbNullString;
                m_NoteAmministrative = "";
                m_ValoreUpFront = default;
                m_ValoreProvvTAN = default;
                m_ValoreProvvAGG = default;
                m_ValoreProvvTOT = default;
                m_Flags = 0;
            }

            /// <summary>
        /// Restituisce o imposta il valore dell'upfront
        /// </summary>
        /// <returns></returns>
            public decimal? ValoreUpFront
            {
                get
                {
                    return m_ValoreUpFront;
                }

                set
                {
                    var oldValue = m_ValoreUpFront;
                    if (oldValue == value == true)
                        return;
                    m_ValoreUpFront = value;
                    DoChanged("ValoreUpFront", value, oldValue);
                }
            }

            public decimal? ValoreProvvTAN
            {
                get
                {
                    return m_ValoreProvvTAN;
                }

                set
                {
                    var oldValue = m_ValoreProvvTAN;
                    if (oldValue == value == true)
                        return;
                    m_ValoreProvvTAN = value;
                    DoChanged("ValoreProvvTAN", value, oldValue);
                }
            }

            public decimal? ValoreProvvAGG
            {
                get
                {
                    return m_ValoreProvvAGG;
                }

                set
                {
                    var oldValue = m_ValoreProvvAGG;
                    if (oldValue == value == true)
                        return;
                    m_ValoreProvvAGG = value;
                    DoChanged("ValoreProvvAGG", value, oldValue);
                }
            }

            public decimal? ValoreProvvTOT
            {
                get
                {
                    return m_ValoreProvvTOT;
                }

                set
                {
                    var oldValue = m_ValoreProvvTOT;
                    if (oldValue == value == true)
                        return;
                    m_ValoreProvvTOT = value;
                    DoChanged("ValoreProvvTOT", value, oldValue);
                }
            }

            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            public string NoteAmministrative
            {
                get
                {
                    return m_NoteAmministrative;
                }

                set
                {
                    string oldValue = m_NoteAmministrative;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NoteAmministrative = value;
                    DoChanged("NoteAmministrative", value, oldValue);
                }
            }

            public int IDCorrettaDa
            {
                get
                {
                    return DBUtils.GetID(m_CorrettaDa, m_IDCorrettaDa);
                }

                set
                {
                    int oldValue = IDCorrettaDa;
                    if (oldValue == value)
                        return;
                    m_IDCorrettaDa = value;
                    m_CorrettaDa = null;
                    DoChanged("IDCorrettaDa", value, oldValue);
                }
            }

            public Sistema.CUser CorrettaDa
            {
                get
                {
                    if (m_CorrettaDa is null)
                        m_CorrettaDa = Sistema.Users.GetItemById(m_IDCorrettaDa);
                    return m_CorrettaDa;
                }

                set
                {
                    var oldValue = m_CorrettaDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CorrettaDa = value;
                    m_IDCorrettaDa = DBUtils.GetID(value);
                    DoChanged("CorrettaDa", value, oldValue);
                }
            }

            public DateTime? DataCorrezione
            {
                get
                {
                    return m_DataCorrezione;
                }

                set
                {
                    var oldValue = m_DataCorrezione;
                    if (oldValue == value == true)
                        return;
                    m_DataCorrezione = value;
                    DoChanged("DataCorrezione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica a cui fa riferimento questo oggetto
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
        /// Restituisce o imposta la pratica a cui fa riferimento questo oggetto
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

            protected internal void SetPratica(CPraticaCQSPD p)
            {
                m_Pratica = p;
                m_IDPratica = DBUtils.GetID(p);
            }


            /// <summary>
        /// Restituisce o imposta l'ID dell'offerta che corrisponde ad un'eventuale correzione amministrativa (contabile)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCorrezione
            {
                get
                {
                    return DBUtils.GetID(m_Correzione, m_IDCorrezione);
                }

                set
                {
                    int oldValue = IDCorrezione;
                    if (oldValue == value)
                        return;
                    m_IDCorrezione = value;
                    m_Correzione = null;
                    DoChanged("IDCorrezione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'offerta che corrisponde ad una eventuale correzione amministrativa della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public COffertaCQS Correzione
            {
                get
                {
                    if (m_Correzione is null)
                        m_Correzione = Offerte.GetItemById(m_IDCorrezione);
                    return m_Correzione;
                }

                set
                {
                    var oldValue = m_Correzione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Correzione = value;
                    m_IDCorrezione = DBUtils.GetID(value);
                    DoChanged("Correzione", value, oldValue);
                }
            }

            public override CModulesClass GetModule()
            {
                return null;
            }



            /// <summary>
        /// Restituisce o imposta il motivo dello sconto (eventuale) rispetto alla provvigione massima applicabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MotivoSconto
            {
                get
                {
                    return m_MotivoSconto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_MotivoSconto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoSconto = value;
                    DoChanged("MotivoSconto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta delle informazioni aggiuntive sul motivo dello sconto alla provvigione massima applicabile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string MotivoScontoDettaglio
            {
                get
                {
                    return m_MotivoScontoDettaglio;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_MotivoScontoDettaglio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MotivoScontoDettaglio = value;
                    DoChanged("MotivoScontoDettaglio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha autorizzato lo sconto rispetto alla provvigione massima
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDScontoAutorizzatoDa
            {
                get
                {
                    return DBUtils.GetID(m_ScontoAutorizzatoDa, m_IDScontoAutorizzatoDa);
                }

                set
                {
                    int oldValue = IDScontoAutorizzatoDa;
                    if (oldValue == value)
                        return;
                    m_IDScontoAutorizzatoDa = value;
                    m_ScontoAutorizzatoDa = null;
                    DoChanged("IDScontoAutorizzatoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha autorizzato lo sconto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser ScontoAutorizzatoDa
            {
                get
                {
                    if (m_ScontoAutorizzatoDa is null)
                        m_ScontoAutorizzatoDa = Sistema.Users.GetItemById(m_IDScontoAutorizzatoDa);
                    return m_ScontoAutorizzatoDa;
                }

                set
                {
                    var oldValue = m_ScontoAutorizzatoDa;
                    if (oldValue == value)
                        return;
                    m_ScontoAutorizzatoDa = value;
                    m_IDScontoAutorizzatoDa = DBUtils.GetID(value);
                    DoChanged("ScontoAutorizzatoDa", value, oldValue);
                }
            }

            public DateTime? ScontoAutorizzatoIl
            {
                get
                {
                    return m_ScontoAutorizzatoIl;
                }

                set
                {
                    var oldValue = m_ScontoAutorizzatoIl;
                    if (oldValue == value == true)
                        return;
                    m_ScontoAutorizzatoIl = value;
                    DoChanged("ScontoAutorizzatoIl", value, oldValue);
                }
            }

            public string ScontoAutorizzatoNote
            {
                get
                {
                    return m_ScontoAutorizzatoNote;
                }

                set
                {
                    string oldValue = m_ScontoAutorizzatoNote;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ScontoAutorizzatoNote = value;
                    DoChanged("ScontoAutorizzatoNote", value, oldValue);
                }
            }

            public CPraticaCQSPD PraticaDiRiferimento
            {
                get
                {
                    if (m_PraticaDiRiferimento is null)
                        m_PraticaDiRiferimento = Pratiche.GetItemById(m_IDPraticaDiRiferimento);
                    return m_PraticaDiRiferimento;
                }

                set
                {
                    var oldValue = m_PraticaDiRiferimento;
                    if (oldValue == value)
                        return;
                    m_PraticaDiRiferimento = value;
                    m_IDPraticaDiRiferimento = DBUtils.GetID(value);
                    DoChanged("PraticaDiRiferimento", value, oldValue);
                }
            }

            public int IDPraticaDiRiferimento
            {
                get
                {
                    return DBUtils.GetID(m_PraticaDiRiferimento, m_IDPraticaDiRiferimento);
                }

                set
                {
                    int oldValue = IDPraticaDiRiferimento;
                    if (oldValue == value)
                        return;
                    m_PraticaDiRiferimento = null;
                    m_IDPraticaDiRiferimento = value;
                    DoChanged("IDPraticaDiRiferimento", value, oldValue);
                }
            }

            public CTeamManager Commerciale
            {
                get
                {
                    if (m_Commerciale is null)
                        m_Commerciale = TeamManagers.GetItemById(m_IDCommerciale);
                    return m_Commerciale;
                }

                set
                {
                    var oldValue = m_Commerciale;
                    if (oldValue == value)
                        return;
                    m_Commerciale = value;
                    m_IDCommerciale = DBUtils.GetID(value);
                    // If (value IsNot Nothing) Then Me.m_NomeCommerciale = value.Nominativo
                    DoChanged("Commerciale", value, oldValue);
                }
            }

            public int IDCommerciale
            {
                get
                {
                    return DBUtils.GetID(m_Commerciale, m_IDCommerciale);
                }

                set
                {
                    int oldValue = IDCommerciale;
                    if (oldValue == value)
                        return;
                    m_IDCommerciale = value;
                    m_Commerciale = null;
                    DoChanged("IDCommerciale", value, oldValue);
                }
            }

            public int IDDistributore
            {
                get
                {
                    return DBUtils.GetID(m_Distributore, m_IDDistributore);
                }

                set
                {
                    int oldValue = IDDistributore;
                    if (oldValue == value)
                        return;
                    m_IDDistributore = value;
                    m_Distributore = null;
                    DoChanged("IDDistributore", value, oldValue);
                }
            }

            public Anagrafica.CDistributore Distributore
            {
                get
                {
                    if (m_Distributore is null)
                        m_Distributore = Anagrafica.Distributori.GetItemById(m_IDDistributore);
                    return m_Distributore;
                }

                set
                {
                    var oldValue = m_Distributore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Distributore = value;
                    m_IDDistributore = DBUtils.GetID(value);
                    DoChanged("Distributore", value, oldValue);
                }
            }

            // Public Property NomeCommerciale As String
            // Get
            // Return Me.m_NomeCommerciale
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_NomeCommerciale
            // If (oldValue = value) Then Exit Property
            // Me.m_NomeCommerciale = value
            // Me.DoChanged("NomeCommerciale", value, oldValue)
            // End Set
            // End Property




            // Public Property NomeConsulente As String
            // Get
            // Return Me.m_NomeConsulente
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_NomeConsulente
            // If (oldValue = value) Then Exit Property
            // Me.m_NomeConsulente = value
            // Me.DoChanged("NomeConsulente", value, oldValue)
            // End Set
            // End Property

            public decimal? Costo
            {
                get
                {
                    return m_Costo;
                }

                set
                {
                    var oldValue = m_Costo;
                    if (oldValue == value == true)
                        return;
                    m_Costo = value;
                    DoChanged("Costo", value, oldValue);
                }
            }

            public DateTime? DataTrasferimento
            {
                get
                {
                    return m_DataTrasferimento;
                }

                set
                {
                    var oldValue = m_DataTrasferimento;
                    if (oldValue == value == true)
                        return;
                    m_DataTrasferimento = value;
                    DoChanged("DataTrasferimento", value, oldValue);
                }
            }

            public string TrasferitoA
            {
                get
                {
                    return m_TrasferitoA;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TrasferitoA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TrasferitoA = value;
                    DoChanged("TrasferitoA", value, oldValue);
                }
            }

            public int IDTrasferitoDa
            {
                get
                {
                    return DBUtils.GetID(m_TrasferitoDa, m_IDTrasferitoDa);
                }

                set
                {
                    int oldValue = IDTrasferitoDa;
                    if (oldValue == value)
                        return;
                    m_TrasferitoDa = null;
                    m_IDTrasferitoDa = value;
                    DoChanged("TrasferitoDaID", value, oldValue);
                }
            }

            public Sistema.CUser TrasferitoDa
            {
                get
                {
                    if (m_TrasferitoDa is null)
                        m_TrasferitoDa = Sistema.Users.GetItemById(m_IDTrasferitoDa);
                    return m_TrasferitoDa;
                }

                set
                {
                    var oldValue = m_TrasferitoDa;
                    if (oldValue == value)
                        return;
                    m_TrasferitoDa = value;
                    m_IDTrasferitoDa = DBUtils.GetID(value);
                    DoChanged("TrasferitoDa", value, oldValue);
                }
            }

            public string TrasferitoDaURL
            {
                get
                {
                    return m_TrasferitoDaURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TrasferitoDaURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TrasferitoDaURL = value;
                    DoChanged("TrasferitoDaURL", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della pratica memorizzata sul sistema esterno presso cui è stata inviata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPraticaTrasferita
            {
                get
                {
                    return m_IDPraticaTrasferita;
                }

                set
                {
                    int oldValue = m_IDPraticaTrasferita;
                    if (oldValue == value)
                        return;
                    m_IDPraticaTrasferita = value;
                    DoChanged("IDPraticaTrasferita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultimo controllo fatto sul sistema esterno per verificare lo stato della pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataAggiornamentoPT
            {
                get
                {
                    return m_DataAggiornamentoPT;
                }

                set
                {
                    var oldValue = m_DataAggiornamentoPT;
                    if (oldValue == value == true)
                        return;
                    m_DataAggiornamentoPT = value;
                    DoChanged("DataAggiornamentoPT", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il codice di errore relativo all'ultimo controllo fatto sul sistema esterno per verificare lo stato della pratica.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int EsitoAggiornamentoPT
            {
                get
                {
                    return m_EsitoAggiornamentoPT;
                }

                set
                {
                    int oldValue = m_EsitoAggiornamentoPT;
                    if (oldValue == value)
                        return;
                    m_EsitoAggiornamentoPT = value;
                    DoChanged("EsitoAggiornamentoPT", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_PraticheInfo";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPratica = reader.Read("IDPratica",  m_IDPratica);
                m_IDCommerciale = reader.Read("IDCommerciale",  m_IDCommerciale);
                m_IDDistributore = reader.Read("IDDistributore",  m_IDDistributore);
                m_TrasferitoDaURL = reader.Read("TrasferitaDaURL",  m_TrasferitoDaURL);
                m_DataTrasferimento = reader.Read("DataTrasferimento",  m_DataTrasferimento);
                m_TrasferitoA = reader.Read("TrasferitoA",  m_TrasferitoA);
                m_IDTrasferitoDa = reader.Read("IDTrasferitoDa",  m_IDTrasferitoDa);
                m_IDPraticaTrasferita = reader.Read("IDPraticaTrasferita",  m_IDPraticaTrasferita);
                m_DataAggiornamentoPT = reader.Read("DataAggiornamentoPT",  m_DataAggiornamentoPT);
                m_EsitoAggiornamentoPT = reader.Read("EsitoAggiornamentoPT",  m_EsitoAggiornamentoPT); // Me.m_EsitoAggiornamentoPT = reader.GetValue(Of Integer)("EsitoAggiornamentoPT", 0)
                m_Costo = reader.Read("Costo",  m_Costo);
                m_IDPraticaDiRiferimento = reader.Read("IDPraticaDiRiferimento",  m_IDPraticaDiRiferimento);
                m_IDCorrezione = reader.Read("IDCorrezione",  m_IDCorrezione);
                m_MotivoSconto = reader.Read("MotivoSconto",  m_MotivoSconto);
                m_MotivoScontoDettaglio = reader.Read("MotivoScontoDettaglio",  m_MotivoScontoDettaglio);
                m_IDScontoAutorizzatoDa = reader.Read("IDScontoAutorizzatoDa",  m_IDScontoAutorizzatoDa);
                m_ScontoAutorizzatoIl = reader.Read("ScontoAutorizzatoIl",  m_ScontoAutorizzatoIl);
                m_ScontoAutorizzatoNote = reader.Read("ScontoAutorizzatoNote",  m_ScontoAutorizzatoNote);
                m_IDCorrettaDa = reader.Read("IDCorrettaDa",  m_IDCorrettaDa);
                m_DataCorrezione = reader.Read("DataCorrezione",  m_DataCorrezione);
                m_NoteAmministrative = reader.Read("NoteAmministrative",  m_NoteAmministrative);
                m_ValoreUpFront = reader.Read("ValoreUpFront",  m_ValoreUpFront);
                m_ValoreProvvTAN = reader.Read("ValoreProvvTAN",  m_ValoreProvvTAN);
                m_ValoreProvvAGG = reader.Read("ValoreProvvAGG",  m_ValoreProvvAGG);
                m_ValoreProvvTOT = reader.Read("ValoreProvvTOT",  m_ValoreProvvTOT);
                m_Flags = reader.Read("Flags",  m_Flags);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPratica", IDPratica);
                writer.Write("MotivoSconto", m_MotivoSconto);
                writer.Write("MotivoScontoDettaglio", m_MotivoScontoDettaglio);
                writer.Write("IDScontoAutorizzatoDa", IDScontoAutorizzatoDa);
                writer.Write("ScontoAutorizzatoIl", m_ScontoAutorizzatoIl);
                writer.Write("ScontoAutorizzatoNote", m_ScontoAutorizzatoNote);
                writer.Write("IDCorrezione", IDCorrezione);
                writer.Write("IDCommerciale", IDCommerciale);
                writer.Write("IDPraticaDiRiferimento", IDPraticaDiRiferimento);
                writer.Write("TrasferitaDaURL", m_TrasferitoDaURL);
                writer.Write("DataTrasferimento", m_DataTrasferimento);
                writer.Write("TrasferitoA", m_TrasferitoA);
                writer.Write("IDTrasferitoDa", IDTrasferitoDa);
                writer.Write("DataAggiornamentoPT", m_DataAggiornamentoPT);
                writer.Write("EsitoAggiornamentoPT", m_EsitoAggiornamentoPT); // Me.m_EsitoAggiornamentoPT = reader.GetValue(Of Integer)("EsitoAggiornamentoPT", 0)
                writer.Write("Costo", m_Costo);
                writer.Write("IDPraticaTrasferita", m_IDPraticaTrasferita);
                writer.Write("IDDistributore", IDDistributore);
                writer.Write("IDCorrettaDa", IDCorrettaDa);
                writer.Write("DataCorrezione", m_DataCorrezione);
                writer.Write("NoteAmministrative", m_NoteAmministrative);
                writer.Write("ValoreUpFront", m_ValoreUpFront);
                writer.Write("ValoreProvvTAN", m_ValoreProvvTAN);
                writer.Write("ValoreProvvAGG", m_ValoreProvvAGG);
                writer.Write("ValoreProvvTOT", m_ValoreProvvTOT);
                writer.Write("Flags", m_Flags);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("IDCommerciale", IDCommerciale);
                writer.WriteAttribute("TrasferitoDaURL", m_TrasferitoDaURL);
                writer.WriteAttribute("DataTrasferimento", m_DataTrasferimento);
                writer.WriteAttribute("TrasferitoA", m_TrasferitoA);
                writer.WriteAttribute("TrasferitaDaURL", m_TrasferitoDaURL);
                writer.WriteAttribute("TrasferitoDaID", IDTrasferitoDa);
                writer.WriteAttribute("IDPraticaTrasferita", IDPraticaTrasferita);
                writer.WriteAttribute("DataAggiornamentoPT", m_DataAggiornamentoPT);
                writer.WriteAttribute("EsitoAggiornamentoPT", m_EsitoAggiornamentoPT);
                writer.WriteAttribute("Costo", m_Costo);
                writer.WriteAttribute("PraticaDiRiferimentoID", IDPraticaDiRiferimento);
                writer.WriteAttribute("MotivoSconto", m_MotivoSconto);
                writer.WriteAttribute("IDScontoAutorizzatoDa", IDScontoAutorizzatoDa);
                writer.WriteAttribute("ScontoAutorizzatoIl", m_ScontoAutorizzatoIl);
                writer.WriteAttribute("ScontoAutorizzatoNote", m_ScontoAutorizzatoNote);
                writer.WriteAttribute("IDCorrezione", IDCorrezione);
                writer.WriteAttribute("IDDistributore", IDDistributore);
                writer.WriteAttribute("IDCorrettaDa", IDCorrettaDa);
                writer.WriteAttribute("DataCorrezione", m_DataCorrezione);
                writer.WriteAttribute("ValoreUpFront", m_ValoreUpFront);
                writer.WriteAttribute("ValoreProvvTAN", m_ValoreProvvTAN);
                writer.WriteAttribute("ValoreProvvAGG", m_ValoreProvvAGG);
                writer.WriteAttribute("ValoreProvvTOT", m_ValoreProvvTOT);
                writer.WriteAttribute("Flags", m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("MotivoScontoDettaglio", m_MotivoScontoDettaglio);
                writer.WriteTag("NoteAmministrative", m_NoteAmministrative);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCommerciale":
                        {
                            m_IDCommerciale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TrasferitoDaURL":
                        {
                            m_TrasferitoDaURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataTrasferimento":
                        {
                            m_DataTrasferimento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TrasferitoA":
                        {
                            m_TrasferitoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TrasferitaDaURL":
                        {
                            m_TrasferitoDaURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TrasferitoDaID":
                        {
                            m_IDTrasferitoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPraticaTrasferita":
                        {
                            m_IDPraticaTrasferita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataAggiornamentoPT":
                        {
                            m_DataAggiornamentoPT = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "EsitoAggiornamentoPT":
                        {
                            m_EsitoAggiornamentoPT = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Costo":
                        {
                            m_Costo = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PraticaDiRiferimentoID":
                        {
                            m_IDPraticaDiRiferimento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MotivoSconto":
                        {
                            m_MotivoSconto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MotivoScontoDettaglio":
                        {
                            m_MotivoScontoDettaglio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDScontoAutorizzatoDa":
                        {
                            m_IDScontoAutorizzatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Case "NomeScontoAutorizzatoDa" : Me.m_NomeScontoAutorizzatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                    case "ScontoAutorizzatoIl":
                        {
                            m_ScontoAutorizzatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ScontoAutorizzatoNote":
                        {
                            m_ScontoAutorizzatoNote = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCorrezione":
                        {
                            m_IDCorrezione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDistributore":
                        {
                            m_IDDistributore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCorrettaDa":
                        {
                            m_IDCorrettaDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataCorrezione":
                        {
                            m_DataCorrezione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NoteAmministrative":
                        {
                            m_NoteAmministrative = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ValoreUpFront":
                        {
                            m_ValoreUpFront = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreProvvTAN":
                        {
                            m_ValoreProvvTAN = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreProvvAGG":
                        {
                            m_ValoreProvvAGG = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreProvvTOT":
                        {
                            m_ValoreProvvTOT = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}