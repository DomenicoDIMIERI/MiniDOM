using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoPraticaEnum : int
        {
            /// <summary>
            /// Preventivo
            /// </summary>
            /// <remarks></remarks>
            STATO_PREVENTIVO = -10,

            /// <summary>
            /// Preventivo accettato
            /// </summary>
            /// <remarks></remarks>
            STATO_PREVENTIVO_ACCETTATO = 0,

            /// <summary>
            /// Contratto stampato
            /// </summary>
            /// <remarks></remarks>
            STATO_CONTRATTO_STAMPATO = 10,

            /// <summary>
            /// Contratto Firmato
            /// </summary>
            /// <remarks></remarks>
            STATO_CONTRATTO_FIRMATO = 20,

            /// <summary>
            /// Pratica caricata
            /// </summary>
            /// <remarks></remarks>
            STATO_PRATICA_CARICATA = 30,

            /// <summary>
            /// La pratica è in stato richiesta delibera
            /// </summary>
            /// <remarks></remarks>
            STATO_RICHIESTADELIBERA = 40,

            /// <summary>
            /// La pratica è stata deliberata
            /// </summary>
            /// <remarks></remarks>
            STATO_DELIBERATA = 50,

            /// <summary>
            /// La pratica è pronta per la liquidazione
            /// </summary>
            /// <remarks></remarks>
            STATO_PRONTALIQUIDAZIONE = 60,

            /// <summary>
            /// La pratica è stata liquidata
            /// </summary>
            /// <remarks></remarks>
            STATO_LIQUIDATA = 70,

            /// <summary>
        /// La pratica è stata archiviata
        /// </summary>
        /// <remarks></remarks>
            STATO_ARCHIVIATA = 80,

            /// <summary>
        /// La pratica è stata estinta anticipatamente
        /// </summary>
        /// <remarks></remarks>
            STATO_ESTINTAANTICIPATAMENTE = 90,

            /// <summary>
            /// La pratica è stata annullata
            /// </summary>
            /// <remarks></remarks>
            STATO_ANNULLATA = 1000
        }

        [Flags]
        public enum StatoPraticaFlags : int
        {
            None = 0,

            /// <summary>
        /// Se vero indica che gli utenti normali non possono modificare l'anagrafica di una pratica in questo stato
        /// </summary>
        /// <remarks></remarks>
            BLOCCA_ANAGRAFICA = 1,

            /// <summary>
        /// Se vero indica che gli utenti normali non possono modificare l'offerta di una pratica in questo stato
        /// </summary>
        /// <remarks></remarks>
            BLOCCA_OFFERTA = 2,


            /// <summary>
        /// Se vero indica che una pratica che viene messa in questo stato marca come estinti gli altri prstiti che
        /// sono flaggati come estinzioni
        /// </summary>
        /// <remarks></remarks>
            ACQUISISCI_ESTINZIONI = 4,

            /// <summary>
        /// Se vero indica che una pratica che viene messa in questo stato marca come non estinti gli altri prestiti
        /// che sono flaggati come estinzioni
        /// </summary>
        /// <remarks></remarks>
            RILASCIA_ESTINZIONI = 8
        }


        /// <summary>
    /// Rappresenta uno degli stati possibili per una pratica
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CStatoPratica : Databases.DBObject, ICloneable
        {
            private string m_Nome;              // [TEXT] Nome dello stato
            private StatoPraticaEnum? m_MacroStato; // [INT]  Valore intero di compatibilità con il vecchio sistema    
            private string m_Descrizione;             // [TEXT] Descrizione estesa 
                                                      // Private m_CanChangeOfferta As Boolean       '[BOOL] Se vero indica che lo stato consente di modificare i dati del prodotto
                                                      // Private m_CanChangeAnagrafica As Boolean    'Se vero indica che lo stato consente di modificare l'anagrafica del cliente
                                                      // Private m_Vincolante As Boolean             'Se vero indica che lo stato vincola eventuali richieste ed estinzioni alla pratica corrente
            private bool m_Attivo; // [BOOL] Se vero indica che lo stato è attivo
            private CStatoPratRulesCollection m_StatiSuccessivi; // [CStatoPratRules]  Collezione di oggetti CStatoPratRule che definiscono gli stati successivi possibili
            private int m_IDDefaultTarget; // [INT]  ID dello stato di lavorazione suggerito
            private CStatoPratica m_DefaultTarget;   // CStatopratica Oggetto che rappresenta lo stato di lavorazione successivo (opzione suggerita)
            private int? m_GiorniAvviso;  // Giorni trascorsi i quali il sistema deve mostrare un avviso
            private int? m_GiorniStallo; // Giorni trascorsi i quali il sistema considera la pratica in stallo
            private StatoPraticaFlags m_Flags;        // Flags
            private CKeyCollection m_Attributi;

            public CStatoPratica()
            {
                m_Nome = "";
                m_Descrizione = "";
                // Me.m_CanChangeOfferta = True
                // Me.m_CanChangeAnagrafica = True
                m_Attivo = true;
                m_StatiSuccessivi = null;
                m_MacroStato = StatoPraticaEnum.STATO_PREVENTIVO;
                m_IDDefaultTarget = 0;
                m_DefaultTarget = null;
                m_GiorniAvviso = default;
                m_GiorniStallo = default;
                // Me.m_Vincolante = False
                m_Flags = StatoPraticaFlags.None;
            }

            public override CModulesClass GetModule()
            {
                return StatiPratica.Module;
            }

            public CKeyCollection Attributi
            {
                get
                {
                    if (m_Attributi is null)
                        m_Attributi = new CKeyCollection();
                    return m_Attributi;
                }
            }

            // ''' <summary>
            // ''' Restituisce o imposta un valore booleano che indica se lo stato vincola eventuali estinzioni e richieste alla pratica
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property Vincolante As Boolean
            // Get
            // Return Me.m_Vincolante
            // End Get
            // Set(value As Boolean)
            // If (Me.m_Vincolante = value) Then Exit Property
            // Me.m_Vincolante = value
            // Me.DoChanged("Vincolante", value, Not value)
            // End Set
            // End Property

            public StatoPraticaFlags Flags
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
        /// Restituisce o imposta il numero di giorni trascorsi i quali il sistema emette un avviso
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int? GiorniAvviso
            {
                get
                {
                    return m_GiorniAvviso;
                }

                set
                {
                    var oldValue = m_GiorniAvviso;
                    if (oldValue == value == true)
                        return;
                    m_GiorniAvviso = value;
                    DoChanged("GiorniAvviso", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di giorni trascorsi i quali il sistema considera in stallo una pratica
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int? GiorniStallo
            {
                get
                {
                    return m_GiorniStallo;
                }

                set
                {
                    var oldValue = m_GiorniStallo;
                    if (oldValue == value == true)
                        return;
                    m_GiorniStallo = value;
                    DoChanged("GiorniStallo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome univoco dello stato
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
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una descrizione estesa per lo stato
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
        /// Restituisec o imposta un valore che indica se lo stato consente di modificare i dati del prodotto offerto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CanChangeOfferta
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, StatoPraticaFlags.BLOCCA_OFFERTA) == false; // Me.m_CanChangeOfferta
                }

                set
                {
                    // If (Me.m_CanChangeOfferta = value) Then Exit Property
                    // Me.m_CanChangeOfferta = value
                    // Me.DoChanged("CanChangeOfferta", value, Not value)
                    if (CanChangeOfferta == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, StatoPraticaFlags.BLOCCA_OFFERTA, !value);
                    DoChanged("CanChangeOfferta", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se lo stato consente di modificare i dati dell'anagrafica del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CanChangeAnagrafica
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, StatoPraticaFlags.BLOCCA_ANAGRAFICA) == false;  // Me.m_CanChangeAnagrafica
                }

                set
                {
                    // If (Me.m_CanChangeAnagrafica = value) Then Exit Property
                    // Me.m_CanChangeAnagrafica = value
                    // Me.DoChanged("CanChangeAnagrafica", value, Not value)
                    if (CanChangeAnagrafica == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, StatoPraticaFlags.BLOCCA_OFFERTA, !value);
                    DoChanged("CanChangeAnagrafica", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se una pratica nello stato corrente deve acquisire e dichiarare come estinti gli altri prestiti flaggati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool AcquisisciEstinzioni
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, StatoPraticaFlags.ACQUISISCI_ESTINZIONI); // Me.m_CanChangeAnagrafica
                }

                set
                {
                    if (AcquisisciEstinzioni == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, StatoPraticaFlags.ACQUISISCI_ESTINZIONI, value);
                    DoChanged("AcquisisciEstinzioni", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se una pratica nello stato corrente deve rilasciare e dichiarare come non estinti gli altri prestiti flaggati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool RilasciaEstinzioni
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, StatoPraticaFlags.RILASCIA_ESTINZIONI);
                }

                set
                {
                    if (RilasciaEstinzioni == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, StatoPraticaFlags.RILASCIA_ESTINZIONI, value);
                    DoChanged("RilasciaEstinzioni", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica se lo stato è attivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che rappresenta la codifica dello stato nel vecchio sistema
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoPraticaEnum? MacroStato
            {
                get
                {
                    return m_MacroStato;
                }

                set
                {
                    var oldValue = m_MacroStato;
                    if (oldValue == value == true)
                        return;
                    m_MacroStato = value;
                    DoChanged("OldStatus", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dello stato successivo (suggerito)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDDefaultTarget
            {
                get
                {
                    return DBUtils.GetID(m_DefaultTarget, m_IDDefaultTarget);
                }

                set
                {
                    int oldValue = IDDefaultTarget;
                    if (oldValue == value)
                        return;
                    m_IDDefaultTarget = value;
                    m_DefaultTarget = null;
                    DoChanged("IDDefaultTarget", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato successivo predefinito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoPratica DefaultTarget
            {
                get
                {
                    if (m_DefaultTarget is null)
                        m_DefaultTarget = StatiPratica.GetItemById(m_IDDefaultTarget);
                    return m_DefaultTarget;
                }

                set
                {
                    var oldValue = m_DefaultTarget;
                    if (oldValue == value)
                        return;
                    m_DefaultTarget = value;
                    m_IDDefaultTarget = DBUtils.GetID(value);
                    DoChanged("DefaultTarget", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce una collezione di regole che definiscono il passaggio agli stati successivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoPratRulesCollection StatiSuccessivi
            {
                get
                {
                    lock (this)
                    {
                        if (m_StatiSuccessivi is null)
                        {
                            m_StatiSuccessivi = new CStatoPratRulesCollection();
                            m_StatiSuccessivi.Load(this);
                        }

                        return m_StatiSuccessivi;
                    }
                }
            }

            public override string GetTableName()
            {
                return "tbl_PraticheSTS";
            }

            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (!ret && m_StatiSuccessivi is object)
                    ret = DBUtils.IsChanged(m_StatiSuccessivi);
                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret & m_StatiSuccessivi is object)
                    m_StatiSuccessivi.Save(force);
                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                this.m_MacroStato = reader.Read("OldStatus", this.m_MacroStato);
                this.m_IDDefaultTarget = reader.Read("IDDefaultTarget", this.m_IDDefaultTarget);
                // Me.m_CanChangeOfferta = reader.Read("CanChangeOfferta", Me.m_CanChangeOfferta)
                // Me.m_CanChangeAnagrafica = reader.Read("CanChangeAnagrafica", Me.m_CanChangeAnagrafica)
                this.m_Attivo = reader.Read("Attivo", this.m_Attivo);
                this.m_GiorniAvviso = reader.Read("GiorniAvviso", this.m_GiorniAvviso);
                this.m_GiorniStallo = reader.Read("GiorniStallo", this.m_GiorniStallo);
                // Me.m_Vincolante = reader.Read("Vincolante", Me.m_Vincolante)
                this.m_Flags = reader.Read("Flags", this.m_Flags);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                }
                 

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("OldStatus", m_MacroStato);
                writer.Write("IDDefaultTarget", IDDefaultTarget);
                // writer.Write("CanChangeOfferta", Me.m_CanChangeOfferta)
                // writer.Write("CanChangeAnagrafica", Me.m_CanChangeOfferta)
                writer.Write("Attivo", m_Attivo);
                writer.Write("GiorniAvviso", m_GiorniAvviso);
                writer.Write("GiorniStallo", m_GiorniStallo);
                // writer.Write("Vincolante", Me.m_Vincolante)
                writer.Write("Flags", m_Flags);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("MacroStato", m_MacroStato);
                writer.WriteAttribute("IDDefaultTarget", IDDefaultTarget);
                // writer.WriteAttribute("CanChangeOfferta", Me.m_CanChangeOfferta)
                // writer.WriteAttribute("CanChangeAnagrafica", Me.m_CanChangeAnagrafica)
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("GiorniAvviso", m_GiorniAvviso);
                writer.WriteAttribute("GiorniStallo", m_GiorniStallo);
                // writer.WriteAttribute("Vincolante", Me.m_Vincolante)
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                writer.WriteTag("StatiSuccessivi", StatiSuccessivi);
                writer.WriteTag("Attributi", Attributi);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MacroStato":
                        {
                            m_MacroStato = (StatoPraticaEnum?)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDDefaultTarget":
                        {
                            m_IDDefaultTarget = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Case "CanChangeOfferta" : Me.m_CanChangeOfferta = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                    // Case "CanChangeAnagrafica" : Me.m_CanChangeAnagrafica = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "GiorniAvviso":
                        {
                            m_GiorniAvviso = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "GiorniStallo":
                        {
                            m_GiorniStallo = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    // Case "Vincolante" : Me.m_Vincolante = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                    case "Flags":
                        {
                            m_Flags = (StatoPraticaFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatiSuccessivi":
                        {
                            m_StatiSuccessivi = (CStatoPratRulesCollection)fieldValue;
                            m_StatiSuccessivi.SetOwner(this);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public string Colore
            {
                get
                {
                    return DMD.Strings.CStr(Attributi.GetItemByKey("Colore"));
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = Colore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    Attributi.SetItemByKey("Colore", value);
                    DoChanged("Colore", value, oldValue);
                }
            }

            public override string ToString()
            {
                return m_Nome;
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                StatiPratica.UpdateCached(this);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}