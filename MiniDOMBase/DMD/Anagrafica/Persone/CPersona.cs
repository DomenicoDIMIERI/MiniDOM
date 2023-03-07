using System;
using System.Collections;
using System.Data;
using static minidom.Sistema;
using DMD;
using DMD.Databases;
using DMD.XML;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Tipologie di persone
        /// </summary>
        public enum TipoPersona : int
        {
            /// <summary>
            /// Persona fisica
            /// </summary>
            PERSONA_FISICA = 0,

            /// <summary>
            /// Persona giuridica
            /// </summary>
            PERSONA_GIURIDICA = 1
        }

        /// <summary>
        /// Flag validi per una persona
        /// </summary>
        [Flags]
        public enum PFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            NOTSET = 0,

            /// <summary>
            /// Flag impostato se il codice fiscale è stato verificato
            /// </summary>
            /// <remarks></remarks>
            CF_VERIFICATO = 1,

            /// <summary>
            /// L'utente ha espresso il consenso ad essere contattato per offerte pubblicitarie
            /// </summary>
            /// <remarks></remarks>
            CF_CONSENSOADV = 2,

            /// <summary>
            /// L'utente ha espresso il consenso al trattamento dei dati personali
            /// </summary>
            /// <remarks></remarks>
            CF_CONSENSOLAV = 4,


            Moroso = 16,

            /// <summary>
            /// Indica che si tratta di un cliente attenzionato perché in lavorazione
            /// </summary>
            /// <remarks></remarks>
            InLavorazione = 32,

            /// <summary>
            /// Indica che si tratta di un cliente
            /// </summary>
            /// <remarks></remarks>
            Cliente = 64,


            /// <summary>
            /// Indica che si tratta di un fornitore
            /// </summary>
            /// <remarks></remarks>
            Fornitore = 128
        }

        /// <summary>
        /// Evento generato quando questo oggetto viene unito con un altra anagrafica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void MergedEventHandler(object sender, MergePersonaEventArgs e);

        /// <summary>
        /// Evento generato quando questo oggetto viene annullata l'unione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void UnMergedEventHandler(object sender, MergePersonaEventArgs e);

        /// <summary>
        /// Evento generato quando questo oggetto viene trasferito ad un altro punto operativo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void TransferredEventHandler(object sender, TransferPersonaEventArgs e);

        /// <summary>
        /// Rappresenta una persona fisica o giuridica
        /// </summary>
        [Serializable]
        public abstract class CPersona 
            : Databases.DBObjectPO, IComparable, IComparable<CPersona>, Sistema.IIndexable, ISupportInitializeFrom
        {

            /// <summary>
            /// Evento generato quando questo oggetto viene unito con un altra anagrafica
            /// </summary>
            /// <remarks></remarks>
            public event MergedEventHandler Merged;

            /// <summary>
            /// Evento generato quando questo oggetto viene annullata l'unione
            /// </summary>
            /// <remarks></remarks>
            public event UnMergedEventHandler UnMerged;

            

            /// <summary>
            /// Evento generato quando questo oggetto viene trasferito ad un altro punto operativo
            /// </summary>
            /// <remarks></remarks>
            public event TransferredEventHandler Transferred;

            private string m_Alias1;
            private string m_Alias2;
            private string m_Professione; // Professione
            private string m_Titolo; // Titolo
            private string m_Sesso; // M per maschio, F per femmina
            private DateTime? m_DataNascita; // Data di nascita/apertura
            private DateTime? m_DataMorte; // Data di morte/chiusura
            private string m_Cittadinanza;
            private CIndirizzo m_NatoA;
            private CIndirizzo m_MortoA;
            private CIndirizzo m_ResidenteA;
            private CIndirizzo m_DomiciliatoA;
            private string m_CodiceFiscale;
            private string m_PartitaIVA;
            private string m_IconURL;
            private bool m_Deceduto;
            private string m_TipoFonte;
            private int m_IDFonte;
            [NonSerialized] private IFonte m_Fonte;
            private string m_NomeFonte;
            private PFlags m_NFlags;
            [NonSerialized] private CCanale m_Canale;
            private int m_IDCanale;
            private string m_NomeCanale;
            private string m_Descrizione;
            private string m_DettaglioEsito;
            private string m_DettaglioEsito1;
            private int m_IDReferente1;
            [NonSerialized] private Sistema.CUser m_Referente1;
            private int m_IDReferente2;
            [NonSerialized] private Sistema.CUser m_Referente2;
            [NonSerialized] private Sistema.CAnnotazioni m_Annotazioni;
            [NonSerialized] private Sistema.CAttachmentsCollection m_Attachments;
            [NonSerialized] private CContattiPerPersonaCollection m_Recapiti;
            private int m_IDStatoAttuale;
            [NonSerialized] private StatoTaskLavorazione m_StatoAttuale;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersona()
            {
                m_IDFonte = 0;
                m_Fonte = null;
                m_NomeFonte = "";
                m_Alias1 = "";
                m_Alias2 = "";
                m_Professione = "";
                m_Titolo = "";
                m_Sesso = "";
                m_Cittadinanza = "";
                m_DataNascita = default;
                m_DataMorte = default;
                m_NatoA = new CIndirizzo(this, "Nato a", "");
                m_MortoA = new CIndirizzo(this, "Morto a", "");
                m_ResidenteA = new CIndirizzo(this, "Residente a", "");
                m_DomiciliatoA = new CIndirizzo(this, "Domiciliato a", "");
                m_CodiceFiscale = "";
                m_PartitaIVA = "";
                m_IconURL = "";
                m_Deceduto = false;
                m_Flags = (int) PFlags.NOTSET;
                m_NFlags = PFlags.NOTSET;
                m_Canale = null;
                m_IDCanale = 0;
                m_NomeCanale = DMD.Strings.vbNullString;
                m_Recapiti = null;
                m_Descrizione = "";
                m_DettaglioEsito = "";
                m_DettaglioEsito1 = "";
                m_IDReferente1 = 0;
                m_Referente1 = null;
                m_IDReferente2 = 0;
                m_Referente2 = null;
                m_IDStatoAttuale = 0;
                m_StatoAttuale = null;
            }

             
            /// <summary>
            /// Restituisce o imposta l'ID dello stato attuale
            /// </summary>
            /// <returns></returns>
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
            public StatoTaskLavorazione StatoAttuale
            {
                get
                {
                    if (m_StatoAttuale is null)
                        m_StatoAttuale = StatiTasksLavorazione.GetItemById(m_IDStatoAttuale);
                    if (m_StatoAttuale is null)
                    {
                        var col = TasksDiLavorazione.GetTasksInCorso(this);
                        TaskLavorazione stl = null;
                        if (col.Count > 0)
                            stl = col[0];
                        if (stl is null)
                            stl = TasksDiLavorazione.Inizializza(this, null);
                        m_StatoAttuale = stl.StatoAttuale;
                        DoChanged("StatoAttuale", m_StatoAttuale, null);
                    }

                    return m_StatoAttuale;
                }

                set
                {
                    var oldValue = StatoAttuale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoAttuale = value;
                    m_IDStatoAttuale = DBUtils.GetID(value, 0);
                    DoChanged("StatoAttuale", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID del referente 1
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDReferente1
            {
                get
                {
                    return DBUtils.GetID(m_Referente1, m_IDReferente1);
                }

                set
                {
                    int oldValue = IDReferente1;
                    if (oldValue == value)
                        return;
                    m_IDReferente1 = value;
                    m_Referente1 = null;
                    DoChanged("IDReferente1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il referente 1
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Referente1
            {
                get
                {
                    if (m_Referente1 is null)
                        m_Referente1 = Sistema.Users.GetItemById(m_IDReferente1);
                    return m_Referente1;
                }

                set
                {
                    var oldValue = Referente1;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Referente1 = value;
                    m_IDReferente1 = DBUtils.GetID(value, 0);
                    DoChanged("Referente1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del referente 2
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDReferente2
            {
                get
                {
                    return DBUtils.GetID(m_Referente2, m_IDReferente2);
                }

                set
                {
                    int oldValue = IDReferente2;
                    if (oldValue == value)
                        return;
                    m_IDReferente2 = value;
                    m_Referente2 = null;
                    DoChanged("IDReferente2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il referente 2
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Referente2
            {
                get
                {
                    if (m_Referente2 is null)
                        m_Referente2 = Sistema.Users.GetItemById(m_IDReferente2);
                    return m_Referente2;
                }

                set
                {
                    var oldValue = Referente2;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Referente2 = value;
                    m_IDReferente2 = DBUtils.GetID(value, 0);
                    DoChanged("Referente2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che indica lo stato di lavorazione della persona nell'ambito dell'applicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioEsito
            {
                get
                {
                    return m_DettaglioEsito;
                }

                set
                {
                    string oldValue = m_DettaglioEsito;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsito = value;
                    DoChanged("DettaglioEsito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisec o imposta una stringa che indica un sottostato dello stato di lavorazione (DettaglioEsito)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioEsito1
            {
                get
                {
                    return m_DettaglioEsito1;
                }

                set
                {
                    string oldValue = m_DettaglioEsito1;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsito1 = value;
                    DoChanged("DettaglioEsito1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione
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
            /// Restituisce la collezione dei recapiti e-mail, telefonici, ecc.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CContattiPerPersonaCollection Recapiti
            {
                get
                {
                    lock (this)
                    {
                        if (m_Recapiti is null)
                            m_Recapiti = new CContattiPerPersonaCollection(this);
                        return m_Recapiti;
                    }
                }
            }

            /// <summary>
            /// Trasferisce la persona ad un altro punto operativo e genera l'evento Transferred
            /// </summary>
            /// <param name="ufficio"></param>
            /// <param name="messaggio"></param>
            /// <remarks></remarks>
            public void TransferTo(CUfficio ufficio, string messaggio)
            {
                PuntoOperativo = ufficio;
                Save();
                var e = new TransferPersonaEventArgs(this, ufficio, messaggio);
                OnTransferred(e);
            }

            /// <summary>
            /// Genera l'evento Transferred
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnTransferred(TransferPersonaEventArgs e)
            {
                Transferred?.Invoke(this, e);
                Anagrafica.OnPersonaTransferred(e);
                Anagrafica.Module.DispatchEvent(new Sistema.EventDescription("Transferred", "Trasferito l'anagrafica di: " + Nominativo + DMD.Strings.vbCrLf + "Nato a: " + NatoA.NomeComune + DMD.Strings.vbCrLf + "Nato il: " + Sistema.Formats.FormatUserDate(DataNascita) + DMD.Strings.vbCrLf + "C.F.:" + Sistema.Formats.FormatCodiceFiscale(CodiceFiscale) + " all'ufficio di " + e.Ufficio.Nome, e));
                
            }

            /// <summary>
            /// Restituisce o imposta una combinazione di falgs che definiscono alcune proprietà della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new PFlags Flags
            {
                get
                {
                    return (PFlags)this.m_Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int) value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una combinazione di flags che indicano le proprietà negate della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PFlags NFlags
            {
                get
                {
                    return m_NFlags;
                }

                set
                {
                    var oldValue = m_NFlags;
                    if (oldValue == value)
                        return;
                    m_NFlags = value;
                    DoChanged("NFlags", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il percorso dell'immagine associata
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
            /// Annulla l'ultima unione effettuata per la persona corrente
            /// </summary>
            /// <remarks></remarks>
            public CMergePersona UndoMerge()
            {
                var mi = MergePersone.GetLastMerge(this);
                if (mi is null)
                    throw new InvalidOperationException("Nessuna unione precedente");
                var persona1 = this;
                var persona2 = mi.Persona2;
                persona2.Stato = ObjectStatus.OBJECT_VALID;
                persona2.Save();
                OnUnMerged(new MergePersonaEventArgs(mi));
                mi.Delete();
                return mi;
            }

            /// <summary>
            /// Unisce le due persone.
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="autoDelete"></param>
            public virtual void MergeWith(CPersona persona, bool autoDelete = true)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                // If (Me Is persona) Then Return
                string oldName = Nominativo;
                CMergePersona mi = null;
                if (DBUtils.GetID(persona, 0) != 0)
                {
                    mi = new CMergePersona();
                    mi.Persona1 = this;
                    mi.Persona2 = persona;
                    mi.DataOperazione = DMD.DateUtils.Now();
                    mi.Operatore = Sistema.Users.CurrentUser;
                    mi.Stato = ObjectStatus.OBJECT_VALID;
                    mi.Save();
                }

                if (DBUtils.GetID(this) != DBUtils.GetID(persona))
                {
                    if (string.IsNullOrEmpty(m_NomeFonte))
                        m_NomeFonte = persona.m_NomeFonte;
                    if (Fonte is null)
                        Fonte = persona.Fonte;
                    if (string.IsNullOrEmpty(m_Alias1))
                        m_Alias1 = persona.Alias1;
                    if (string.IsNullOrEmpty(m_Alias2))
                        m_Alias2 = persona.Alias2;
                    if (string.IsNullOrEmpty(m_Professione))
                        m_Professione = persona.Professione;
                    if (string.IsNullOrEmpty(m_Titolo))
                        m_Titolo = persona.Titolo;
                    if (string.IsNullOrEmpty(m_Sesso))
                        m_Sesso = persona.Sesso;
                    if (string.IsNullOrEmpty(m_Cittadinanza))
                        m_Cittadinanza = persona.Cittadinanza;
                    if (string.IsNullOrEmpty(m_DettaglioEsito))
                        m_DettaglioEsito = persona.DettaglioEsito;
                    if (string.IsNullOrEmpty(m_DettaglioEsito1))
                        m_DettaglioEsito1 = persona.DettaglioEsito1;
                    if (DMD.RunTime.IsNull(m_DataNascita))
                        m_DataNascita = persona.DataNascita;
                    if (DMD.RunTime.IsNull(m_DataMorte))
                        m_DataMorte = persona.DataMorte;
                    NatoA.MergeWith(persona.NatoA);
                    MortoA.MergeWith(persona.MortoA);
                    ResidenteA.MergeWith(persona.ResidenteA);
                    DomiciliatoA.MergeWith(persona.DomiciliatoA);
                    if ( string.IsNullOrEmpty(m_CodiceFiscale) 
                        || DMD.RunTime.TestFlag(this.Flags, PFlags.CF_VERIFICATO) == false 
                        && DMD.RunTime.TestFlag(persona.Flags, PFlags.CF_VERIFICATO) == true)
                    {
                        m_CodiceFiscale = persona.m_CodiceFiscale;
                    }

                    if (string.IsNullOrEmpty(m_PartitaIVA))
                        m_PartitaIVA = persona.PartitaIVA;
                    if (PuntoOperativo is null)
                        PuntoOperativo = persona.PuntoOperativo;
                    if (string.IsNullOrEmpty(m_IconURL))
                        m_IconURL = persona.m_IconURL;
                    m_Deceduto = m_Deceduto | persona.m_Deceduto;

                    // For Each c As CContatto In persona.Contatti
                    // Me.AddPhoneNumber(c.Nome, c.Valore, c.Validated)
                    // Next
                    // For Each c As CContatto In persona.ContattiWeb
                    // Me.AddWebAddress(c.Nome, c.Valore, c.Validated)
                    // Next
                    foreach (CContatto c in persona.Recapiti)
                        Recapiti.Add(c);
                    persona.Recapiti.Clear();
                    PFlags[] items = (PFlags[])Enum.GetValues(typeof(PFlags));
                    foreach (PFlags f in items)
                    {
                        var v1 = GetFlag(f);
                        var v2 = persona.GetFlag(f);
                        if (!v1.HasValue && v2.HasValue)
                        {
                            SetFlag(f, v2.Value);
                        }
                    }

                    m_Descrizione = DMD.Strings.Combine(m_Descrizione, persona.Descrizione, DMD.Strings.vbNewLine);
                    if (Referente1 is null)
                    {
                        m_Referente1 = persona.Referente1;
                        m_IDReferente1 = DBUtils.GetID(m_Referente1, 0);
                    }

                    if (Referente2 is null)
                    {
                        m_Referente2 = persona.Referente2;
                        m_IDReferente2 = DBUtils.GetID(m_Referente2, 0);
                    }

                    foreach (string key in persona.Parameters.Keys)
                    {
                        if (!this.Parameters.ContainsKey(key))
                        {
                            this.Parameters.Add(key, persona.Parameters[key]);
                        }
                    }

                    Save(true);
                }
                else
                {
                    Save(true);
                }

                if (DBUtils.GetID(persona, 0) != 0)
                {
                    OnMerged(new MergePersonaEventArgs(mi));
                    mi.Save(true);
                }

                if (autoDelete && DBUtils.GetID(persona, 0) != 0 && DBUtils.GetID(persona, 0) != DBUtils.GetID(this, 0))
                    persona.Delete();
            }

            /// <summary>
            /// Restituisce vero se il flag è consentito, false se il flag è negato, nothing se il valore non è stato specificato
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool? GetFlag(PFlags f)
            {
                bool v1 = DMD.RunTime.TestFlag(this.Flags, f);
                bool v2 = DMD.RunTime.TestFlag(this.NFlags, f);
                if (v1 == v2)
                {
                    return default;
                }
                else
                {
                    return v1;
                }
            }

            /// <summary>
            /// Imposta il flag vero se consentito, false se negato, nothing se il valore non è stato specificato
            /// </summary>
            /// <param name="f"></param>
            /// <param name="value"></param>
            /// <remarks></remarks>
            public void SetFlag(PFlags f, bool? value)
            {
                var oldF = m_Flags;
                var oldN = m_NFlags;
                if (value.HasValue)
                {
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, (int)f, value.Value);
                    m_NFlags = DMD.RunTime.SetFlag(m_NFlags, f, !value.Value);
                }
                else
                {
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, (int)f, false);
                    m_NFlags = DMD.RunTime.SetFlag(m_NFlags, f, false);
                }

                if (oldF != m_Flags || oldN != m_NFlags)
                    DoChanged("Flags", null, null);
            }

            /// <summary>
            /// Restituisce o imposta l'ID della fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDFonte
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Fonte, m_IDFonte);
                }

                set
                {
                    int oldValue = IDFonte;
                    if (oldValue == value)
                        return;
                    m_IDFonte = value;
                    m_Fonte = null;
                    DoChanged("IDFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoFonte = value;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la fonte dell'anagrafica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null)
                        m_Fonte = Fonti.GetItemById(m_TipoFonte, m_TipoFonte, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    Databases.DBObjectBase oldValue = (Databases.DBObjectBase)m_Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID(value, 0);
                    m_NomeFonte = (value is object)? value.Nome : string.Empty;
                    DoChanged("Fonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della fonte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeFonte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeFonte = value;
                    DoChanged("NomeFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCanale Canale
            {
                get
                {
                    if (m_Canale is null)
                        m_Canale = Canali.GetItemById(m_IDCanale);
                    return m_Canale;
                }

                set
                {
                    var oldValue = m_Canale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Canale = value;
                    m_IDCanale = DBUtils.GetID(value, 0);
                    m_NomeCanale = (value is object)? value.Nome : string.Empty;
                    DoChanged("Canale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del canale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCanale
            {
                get
                {
                    return DBUtils.GetID(m_Canale, m_IDCanale);
                }

                set
                {
                    int oldValue = IDCanale;
                    if (oldValue == value)
                        return;
                    m_IDCanale = value;
                    m_Canale = null;
                    DoChanged("IDCanale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del canale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCanale;
                    if (Strings.EQ(oldValue , value)) return;
                    m_NomeCanale = value;
                    DoChanged("NomeCanale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della persona (0 per persona fisica)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract TipoPersona TipoPersona { get; }

            /// <summary>
            /// Restituisce o imposta la professione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Professione
            {
                get
                {
                    return m_Professione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Professione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Professione = value;
                    DoChanged("Professione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il titolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Titolo
            {
                get
                {
                    return m_Titolo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Titolo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Titolo = value;
                    DoChanged("Titolo", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce o imposta la cittadinanza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Cittadinanza
            {
                get
                {
                    return m_Cittadinanza;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Cittadinanza;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Cittadinanza = value;
                    DoChanged("Cittadinanza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce un alias per la persona
            /// </summary>
            public string Alias1
            {
                get
                {
                    return m_Alias1;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Alias1;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Alias1 = value;
                    DoChanged("Alias1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce un secondo alias per la persona
            /// </summary>
            public string Alias2
            {
                get
                {
                    return m_Alias2;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Alias2;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Alias2 = value;
                    DoChanged("Alias2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il sesso della persona (M = Maschio, F = Femmina)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Sesso
            {
                get
                {
                    return m_Sesso;
                }

                set
                {
                    value = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(value), 1));
                    string oldValue = m_Sesso;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sesso = value;
                    DoChanged("Sesso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che identifica la persona (es. Nome e Cognome, Ragione sociale, ecc..)
            /// </summary>
            public abstract string Nominativo { get; }

            /// <summary>
            /// Restituisce o imposta la data di nascita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataNascita
            {
                get
                {
                    return m_DataNascita;
                }

                set
                {
                    var oldValue = m_DataNascita;
                    if (oldValue == value == true)
                        return;
                    m_DataNascita = value;
                    DoChanged("DataNascita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'età della persona in anni e frazioni di anni.
            /// Il calcolo è fatto alla data odierna
            /// </summary>
            public double? Eta
            {
                get
                {
                    return get_Eta(DMD.DateUtils.Now());
                }
            }

            /// <summary>
            /// Restituisce l'età della persona alla data specifica
            /// </summary>
            /// <param name="al"></param>
            /// <returns></returns>
            public double? get_Eta(DateTime al)
            {
                if (this.m_DataNascita.HasValue == false)
                    return null;

                double ret;
                var t1 = DMD.DateUtils.GetDatePart(this.m_DataNascita.Value);
                var t2 = DMD.DateUtils.GetDatePart(al);
                ret = DMD.DateUtils.Year(t2) - DMD.DateUtils.Year(t1);
                if (ret > 0d)
                {
                    if (DMD.DateUtils.Month(t2) < DMD.DateUtils.Month(t1))
                    {
                        ret = ret - 1d;
                    }
                    else if (DMD.DateUtils.Month(t2) == DMD.DateUtils.Month(t1))
                    {
                        if (DMD.DateUtils.Day(t2) < DMD.DateUtils.Day(t1))
                        {
                            ret = ret - 1d;
                        }
                    }
                }
                else ret = 0;

                return ret;
            }

            /// <summary>
            /// Restituisce o imposta la data di morte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataMorte
            {
                get
                {
                    return m_DataMorte;
                }

                set
                {
                    var oldValue = m_DataMorte;
                    if (oldValue == value == true)
                        return;
                    m_DataMorte = value;
                    DoChanged("DataMorte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica che la persona è deceduta o l'azienda è stata chiusa
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Deceduto
            {
                get
                {
                    return m_Deceduto;
                }

                set
                {
                    if (m_Deceduto == value)
                        return;
                    m_Deceduto = value;
                    DoChanged("Deceduto", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice fiscale del contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    string oldValue = m_CodiceFiscale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il codice fiscale è stato verificato (con i documenti)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool CodiceFiscaleVerificato
            {
                get
                {
                    var v = GetFlag(PFlags.CF_VERIFICATO);
                    return v.HasValue && v.Value;
                }

                set
                {
                    if (CodiceFiscaleVerificato == value)
                        return;
                    SetFlag(PFlags.CF_VERIFICATO, value);
                    DoChanged("CodiceFiscaleVerificato", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la partita iva del contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }

                set
                {
                    value = Sistema.Formats.ParsePartitaIVA(value);
                    string oldValue = m_PartitaIVA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PartitaIVA = value;
                    DoChanged("PartitaIVA", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di nascita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIndirizzo NatoA
            {
                get
                {
                    return m_NatoA;
                }
            }

            /// <summary>
            /// Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di morte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIndirizzo MortoA
            {
                get
                {
                    return m_MortoA;
                }
            }

            /// <summary>
            /// Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di residenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIndirizzo ResidenteA
            {
                get
                {
                    return m_ResidenteA;
                }
            }

            /// <summary>
            /// Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di domicilio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIndirizzo DomiciliatoA
            {
                get
                {
                    return m_DomiciliatoA;
                }
            }


            /// <summary>
            /// Restituisce il nome del discriminante nel repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Persone";
            }

            /// <summary>
            /// Restituisce una collezione di annotazioni
            /// </summary>
            public virtual Sistema.CAnnotazioni Annotazioni
            {
                get
                {
                    lock (this)
                    {
                        if (m_Annotazioni is null)
                            m_Annotazioni = new Sistema.CAnnotazioni(this);
                        return m_Annotazioni;
                    }
                }
            }

            /// <summary>
            /// Restituisce i documenti caricati per la persona
            /// </summary>
            public virtual Sistema.CAttachmentsCollection Attachments
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attachments is null)
                            m_Attachments = new Sistema.CAttachmentsCollection(this);
                        return m_Attachments;
                    }
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                ret = ret || m_NatoA.IsChanged() || m_MortoA.IsChanged() || m_ResidenteA.IsChanged() || m_DomiciliatoA.IsChanged();
                // ret = ret OrElse DBUtils.IsChanged(Me.m_Contatti) OrElse DBUtils.IsChanged(Me.m_ContattiWeb)
                ret = ret || m_Recapiti is object && DBUtils.IsChanged(m_Recapiti);
                ret = ret || m_Annotazioni is object && DBUtils.IsChanged(m_Annotazioni);
                ret = ret || m_Attachments is object && DBUtils.IsChanged(m_Attachments);
                //ret = ret || m_Attributi is object && DBUtils.IsChanged(m_Attributi)
                ;
                return ret;
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_NatoA.SetChanged(false);
                m_MortoA.SetChanged(false);
                if (m_Annotazioni is object) m_Annotazioni.Save(this.GetConnection(), force);
                if (m_Attachments is object) m_Attachments.Save(this.GetConnection(), force);
                if (m_Recapiti is object) m_Recapiti.Save(this.GetConnection(), force);
                if (this.m_ResidenteA.ID == 0 || this.m_DomiciliatoA.ID == 0)
                {
                    this.m_ResidenteA.Save();
                    this.m_DomiciliatoA.Save();
                    this.Save(true);
                }
                //using(var cursor = new CIndirizziCursor ())
                //{
                    //cursor.PersonaID.Value = DBUtils.GetID(this, 0);
                    //cursor.IgnoreRights = true;
                    //while (cursor.Read())
                    //{
                    //    var indirizzo = cursor.Item;
                    //    if (Strings.EQ(indirizzo.Nome, this.ResidenteA.Nome , true))
                    //    {

                    //    }
                    //}
                //}
            }

            //protected override bool SaveToDatabase(DBConnection dbConn, bool force)
            //{
            //    bool ret;
            //    ret = base.SaveToDatabase(dbConn, force);
            //    if (ret)
            //    {
            //        if (m_Annotazioni is object)
            //            m_Annotazioni.Save(force);
            //        if (m_Attachments is object)
            //            m_Attachments.Save(force);
            //        if (m_Recapiti is object)
            //            m_Recapiti.Save(force);
            //        m_NatoA.SetChanged(false);
            //        m_MortoA.SetChanged(false);
            //    }

            //    if (!dbConn.IsRemote())
            //    {
            //        // Carichiamo gli ID degli indirizzi
            //        if (DBUtils.GetID(m_ResidenteA) == 0)
            //        {
            //            int id;
            //            var dbSQL = new System.Text.StringBuilder();
            //            dbSQL.Append("SELECT [ID], [TipoIndirizzo] FROM [tbl_Indirizzi] WHERE [Persona]=");
            //            dbSQL.Append(DBUtils.GetID(this));
            //            dbSQL.Append(" AND ([TipoIndirizzo]='Residenza' Or [TipoIndirizzo]='Domicilio')");
                         
            //            using (var dbRis = dbConn.ExecuteReader(dbSQL.ToString()))
            //            { 
            //                while (dbRis.Read())
            //                {
            //                    id = DMD.Integers.CInt(dbRis["ID"]);
            //                    switch (Sistema.Formats.ToString(dbRis["TipoIndirizzo"]) ?? "")
            //                    {
            //                        case "Residenza":
            //                            {
            //                                DBUtils.SetID(m_ResidenteA, id);
            //                                break;
            //                            }

            //                        case "Domicilio":
            //                            {
            //                                DBUtils.SetID(m_DomiciliatoA, id);
            //                                break;
            //                            }

            //                        default:
            //                            {
            //                                break;
            //                            }
            //                    }
            //                }
            //            }
                         
            //        }

            //        // Salviamo gli indirizzi per le ricerche
            //        int oldID;
            //        {
            //            var withBlock = m_ResidenteA;
            //            oldID = DBUtils.GetID(m_ResidenteA);
            //            withBlock.SetPersona(this);
            //            withBlock.Stato = Stato;
            //            // If (.ID = 0) Then
            //            // dbSQL = "INSERT INTO [tbl_Indirizzi] ([Persona], [TipoIndirizzo], [Nome], [Citta], [Provincia], [CAP], [Toponimo], [Via], [Civico], [Note]) VALUES (" & DBUtils.DBNumber(GetID(Me)) & ", 'Residenza', " & DBUtils.DBString(.Nome) & ", " & DBUtils.DBString(.Citta) & ", " & DBUtils.DBString(.Provincia) & ", " & DBUtils.DBString(.CAP) & ", " & DBUtils.DBString(.Toponimo) & "," & DBUtils.DBString(.Via) & ", " & DBUtils.DBString(.Civico) & ", " & DBUtils.DBString(.Note) & ")"
            //            // Else
            //            // dbSQL = "UPDATE [tbl_Indirizzi] Set [Nome]=" & DBUtils.DBString(.Note) & ", [Citta]=" & DBUtils.DBString(.Citta) & ", [Provincia]=" & DBUtils.DBString(.Provincia) & ", [CAP]=" & DBUtils.DBString(.CAP) & ", [Via]=" & DBUtils.DBString(.Via) & ", [Toponimo]=" & DBUtils.DBString(.Toponimo) & ", [Civico]=" & DBUtils.DBString(.Civico) & ", [Note]=" & DBUtils.DBString(.Note) & " WHERE [ID]=" & .ID
            //            // End If
            //            // dbConn.ExecuteCommand(dbSQL)
            //            withBlock.Save();
            //            if (oldID == 0)
            //            {
            //                dbConn.ExecuteCommand("UPDATE [tbl_Persone] SET [ResidenteA_ID]=" + DBUtils.GetID(m_ResidenteA) + " WHERE [ID]=" + DBUtils.GetID(this));
            //            }
            //        }

            //        {
            //            var withBlock1 = m_DomiciliatoA;
            //            oldID = DBUtils.GetID(m_DomiciliatoA);
            //            withBlock1.SetPersona(this);
            //            withBlock1.Stato = Stato;
            //            withBlock1.Save();
            //            // If (.ID = 0) Then
            //            // dbSQL = "INSERT INTO [tbl_Indirizzi] ([Persona], [TipoIndirizzo], [Nome], [Citta], [Provincia], [CAP], [Toponimo], [Via], [Civico], [Note]) VALUES (" & DBUtils.DBNumber(GetID(Me)) & ", 'Domicilio', " & DBUtils.DBString(.Nome) & ", " & DBUtils.DBString(.Citta) & ", " & DBUtils.DBString(.Provincia) & ", " & DBUtils.DBString(.CAP) & ", " & DBUtils.DBString(.Toponimo) & "," & DBUtils.DBString(.Via) & ", " & DBUtils.DBString(.Civico) & ", " & DBUtils.DBString(.Note) & ")"
            //            // Else
            //            // dbSQL = "UPDATE [tbl_Indirizzi] Set [Nome]=" & DBUtils.DBString(.Note) & ", [Citta]=" & DBUtils.DBString(.Citta) & ", [Provincia]=" & DBUtils.DBString(.Provincia) & ", [CAP]=" & DBUtils.DBString(.CAP) & ", [Via]=" & DBUtils.DBString(.Via) & ", [Toponimo]=" & DBUtils.DBString(.Toponimo) & ", [Civico]=" & DBUtils.DBString(.Civico) & ", [Note]=" & DBUtils.DBString(.Note) & " WHERE [ID]=" & .ID
            //            // End If
            //            // dbConn.ExecuteCommand(dbSQL)
            //            if (oldID == 0)
            //            {
            //                dbConn.ExecuteCommand("UPDATE [tbl_Persone] SET [DomiciliatoA_ID]=" + DBUtils.GetID(m_DomiciliatoA) + " WHERE [ID]=" + DBUtils.GetID(this));
            //            }
            //        }
            //    }


            //    // Salviamo gli indici
            //    if (Sistema.IndexingService.Database is object && !Sistema.IndexingService.Database.IsRemote())
            //    {
            //        if (Stato == ObjectStatus.OBJECT_VALID)
            //        {
            //            Sistema.IndexingService.Index(this);
            //        }
            //        else
            //        {
            //            Sistema.IndexingService.Unindex(this);
            //        }
            //    }

            //    return ret;
            //}

            /// <summary>
            /// Resetta l'oggetto
            /// </summary>
            protected override void ResetID()
            {
                base.ResetID();
                DBUtils.ResetID(this.ResidenteA);
                DBUtils.ResetID(this.DomiciliatoA);
            }

            /// <summary>
            /// Restituisce le parole chiave per la persona
            /// </summary>
            /// <returns></returns>
            protected virtual string[] GetKeyWords()
            {
                var ret = new ArrayList();
                ret.Add(Nominativo);
                string str = DMD.Strings.Replace(Alias1, DMD.Strings.vbCrLf, " ");
                str = DMD.Strings.Replace(str, DMD.Strings.vbCr, " ");
                str = DMD.Strings.Replace(str, DMD.Strings.vbLf, " ");
                str = DMD.Strings.Replace(str, "  ", " ");
                var a = DMD.Strings.Split(str, " ");
                if (DMD.Arrays.Len(a) > 0)
                    ret.AddRange(a);
                str = DMD.Strings.Replace(Alias2, DMD.Strings.vbCrLf, " ");
                str = DMD.Strings.Replace(str, DMD.Strings.vbCr, " ");
                str = DMD.Strings.Replace(str, DMD.Strings.vbLf, " ");
                str = DMD.Strings.Replace(str, "  ", " ");
                a = DMD.Strings.Split(str, " ");
                if (DMD.Arrays.Len(a) > 0)
                    ret.AddRange(a);
                return (string[])ret.ToArray(typeof(string));
            }

            /// <summary>
            /// Restituisce le parole indicizzate per la persona
            /// </summary>
            /// <returns></returns>
            protected virtual string[] GetIndexedWords()
            {
                var ret = new ArrayList();
                string str = DMD.Strings.Replace(Nominativo, DMD.Strings.vbCrLf, " ");
                str = DMD.Strings.Replace(str, DMD.Strings.vbCr, " ");
                str = DMD.Strings.Replace(str, DMD.Strings.vbLf, " ");
                str = DMD.Strings.Replace(str, "  ", " ");
                var a = DMD.Strings.Split(str, " ");
                if (DMD.Arrays.Len(a) > 0)
                    ret.AddRange(a);
                return (string[])ret.ToArray(typeof(string));
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Alias1 = reader.Read("Alias1", this.m_Alias1);
                m_Alias2 = reader.Read("Alias2", this.m_Alias2);
                m_Sesso = reader.Read("Sesso", this.m_Sesso);
                m_Cittadinanza = reader.Read("Cittadinanza", this.m_Cittadinanza);
                m_DataNascita = reader.Read("DataNascita", this.m_DataNascita);
                m_DataMorte = reader.Read("DataMorte", this.m_DataMorte);
                m_CodiceFiscale = reader.Read("CodiceFiscale", this.m_CodiceFiscale);
                m_PartitaIVA = reader.Read("PartitaIVA", this.m_PartitaIVA);
                m_Professione = reader.Read("Professione", this.m_Professione);
                m_Titolo = reader.Read("Titolo", this.m_Titolo);
                {
                    var withBlock = m_ResidenteA;
                    int i = reader.Read("ResidenteA_ID", 0);
                    DBUtils.SetID(m_ResidenteA, i);
                    withBlock.Nome = reader.Read("ResidenteA_Nome", withBlock.Nome);
                    withBlock.Citta = reader.Read("ResidenteA_Citta", withBlock.Citta);
                    withBlock.Provincia = reader.Read("ResidenteA_Provincia", withBlock.Provincia);
                    withBlock.CAP = reader.Read("ResidenteA_CAP", withBlock.CAP);
                    withBlock.ToponimoEVia = reader.Read("ResidenteA_Via", withBlock.ToponimoEVia);
                    withBlock.Civico = reader.Read("ResidenteA_Civico", withBlock.Civico);
                    withBlock.SetChanged(false);
                }

                {
                    var withBlock1 = m_DomiciliatoA;
                    int i = reader.Read("DomiciliatoA_ID", 0);
                    DBUtils.SetID(m_ResidenteA, i);
                    withBlock1.Nome = reader.Read("DomiciliatoA_Nome", withBlock1.Nome);
                    withBlock1.Citta = reader.Read("DomiciliatoA_Citta", withBlock1.Citta);
                    withBlock1.Provincia = reader.Read("DomiciliatoA_Provincia", withBlock1.Provincia);
                    withBlock1.CAP = reader.Read("DomiciliatoA_CAP", withBlock1.CAP);
                    withBlock1.ToponimoEVia = reader.Read("DomiciliatoA_Via", withBlock1.ToponimoEVia);
                    withBlock1.Civico = reader.Read("DomiciliatoA_Civico", withBlock1.Civico);
                    withBlock1.SetChanged(false);
                }

                {
                    var withBlock2 = m_NatoA;
                    withBlock2.Citta = reader.Read("NatoA_Citta", withBlock2.Citta);
                    withBlock2.Provincia = reader.Read("NatoA_Provincia", withBlock2.Provincia);
                    withBlock2.SetChanged(false);
                }

                {
                    var withBlock3 = m_MortoA;
                    withBlock3.Citta = reader.Read("MortoA_Citta", withBlock3.Citta);
                    withBlock3.Provincia = reader.Read("MortoA_Provincia", withBlock3.Provincia);
                    withBlock3.SetChanged(false);
                }

                m_IconURL = reader.Read("IconURL", this. m_IconURL);
                m_Deceduto = reader.Read("Deceduto", this.m_Deceduto);
                m_TipoFonte = reader.Read("TipoFonte", this.m_TipoFonte);
                m_IDFonte = reader.Read("IDFonte", this.m_IDFonte);
                m_NomeFonte = reader.Read("NomeFonte", this.m_NomeFonte);
                m_Flags = reader.Read("PFlags", this.m_Flags);
                m_NFlags = reader.Read("NFlags", this.m_NFlags);
                m_IDCanale = reader.Read("IDCanale", this.m_IDCanale);
                m_NomeCanale = reader.Read("NomeCanale", this.m_NomeCanale);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_DettaglioEsito = reader.Read("DettaglioEsito", this.m_DettaglioEsito);
                m_DettaglioEsito1 = reader.Read("DettagllioEsito", this.m_DettaglioEsito1);
                m_IDReferente1 = reader.Read("IDReferente1", this.m_IDReferente1);
                m_IDReferente2 = reader.Read("IDReferente2", this.m_IDReferente2);
                m_IDStatoAttuale = reader.Read("IDStatoAttuale", this.m_IDStatoAttuale);
                //string tmp = reader.Read("Attributi", "");
                //if (!string.IsNullOrEmpty(tmp))
                //    this.m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                // Dim i As Integer
                writer.Write("TipoPersona", TipoPersona);
                writer.Write("Sesso", m_Sesso);
                writer.Write("Cittadinanza", m_Cittadinanza);
                writer.Write("Alias1", m_Alias1);
                writer.Write("Alias2", m_Alias2);
                writer.Write("DataNascita", m_DataNascita);
                writer.Write("DataMorte", m_DataMorte);
                writer.Write("CodiceFiscale", m_CodiceFiscale);
                writer.Write("PartitaIVA", m_PartitaIVA);
                writer.Write("Professione", m_Professione);
                writer.Write("Titolo", this.m_Titolo);
                {
                    var withBlock = m_ResidenteA;
                    writer.Write("ResidenteA_ID", withBlock.ID);
                    writer.Write("ResidenteA_Nome", withBlock.Nome);
                    writer.Write("ResidenteA_Citta", withBlock.Citta);
                    writer.Write("ResidenteA_Provincia", withBlock.Provincia);
                    writer.Write("ResidenteA_CAP", withBlock.CAP);
                    writer.Write("ResidenteA_Via", withBlock.ToponimoEVia);
                    writer.Write("ResidenteA_Civico", withBlock.Civico);
                }

                {
                    var withBlock1 = m_DomiciliatoA;
                    writer.Write("DomiciliatoA_ID", withBlock1.ID);
                    writer.Write("DomiciliatoA_Nome", withBlock1.Nome);
                    writer.Write("DomiciliatoA_Citta", withBlock1.Citta);
                    writer.Write("DomiciliatoA_Provincia", withBlock1.Provincia);
                    writer.Write("DomiciliatoA_CAP", withBlock1.CAP);
                    writer.Write("DomiciliatoA_Via", withBlock1.ToponimoEVia);
                    writer.Write("DomiciliatoA_Civico", withBlock1.Civico);
                }

                {
                    var withBlock2 = m_NatoA;
                    writer.Write("NatoA_Citta", withBlock2.Citta);
                    writer.Write("NatoA_Provincia", withBlock2.Provincia);
                }

                {
                    var withBlock3 = m_MortoA;
                    writer.Write("MortoA_Citta", withBlock3.Citta);
                    writer.Write("MortoA_Provincia", withBlock3.Provincia);
                }

                writer.Write("IconURL", m_IconURL);
                writer.Write("Deceduto", m_Deceduto);
                writer.Write("NomeFonte", m_NomeFonte);
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("PFlags", m_Flags);
                writer.Write("NFlags", m_NFlags);
                writer.Write("IDCanale", IDCanale);
                writer.Write("NomeCanale", m_NomeCanale);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("DettaglioEsito", m_DettaglioEsito);
                writer.Write("DettagllioEsito", m_DettaglioEsito1);
                writer.Write("IDReferente1", IDReferente1);
                writer.Write("IDReferente2", IDReferente2);
                writer.Write("IDStatoAttuale", IDStatoAttuale);
                //writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(this.Attributi));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("TipoPersona", typeof(int), 1);
                c = table.Fields.Ensure("Sesso", typeof(string), 32);
                c = table.Fields.Ensure("Cittadinanza", typeof(string), 255);
                c = table.Fields.Ensure("Alias1", typeof(string), 255);
                c = table.Fields.Ensure("Alias2", typeof(string), 255);
                c = table.Fields.Ensure("DataNascita", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataMorte", typeof(DateTime), 1);
                c = table.Fields.Ensure("CodiceFiscale", typeof(string), 255);
                c = table.Fields.Ensure("PartitaIVA", typeof(string), 255);
                c = table.Fields.Ensure("Professione", typeof(string), 255);
                c = table.Fields.Ensure("Titolo", typeof(string), 255);
                
                {
                    c = table.Fields.Ensure("ResidenteA_ID", typeof(int), 1);
                    c = table.Fields.Ensure("ResidenteA_Nome", typeof(string), 255);
                    c = table.Fields.Ensure("ResidenteA_Citta", typeof(string), 255);
                    c = table.Fields.Ensure("ResidenteA_Provincia", typeof(string), 255);
                    c = table.Fields.Ensure("ResidenteA_CAP", typeof(string), 32);
                    c = table.Fields.Ensure("ResidenteA_Via", typeof(string), 255);
                    c = table.Fields.Ensure("ResidenteA_Civico", typeof(string), 255);
                }

                {
                    c = table.Fields.Ensure("DomiciliatoA_ID", typeof(int), 1);
                    c = table.Fields.Ensure("DomiciliatoA_Nome", typeof(string), 255);
                    c = table.Fields.Ensure("DomiciliatoA_Citta", typeof(string), 255);
                    c = table.Fields.Ensure("DomiciliatoA_Provincia", typeof(string), 255);
                    c = table.Fields.Ensure("DomiciliatoA_CAP", typeof(string), 32);
                    c = table.Fields.Ensure("DomiciliatoA_Via", typeof(string), 255);
                    c = table.Fields.Ensure("DomiciliatoA_Civico", typeof(string), 255);
                }

                {
                    c = table.Fields.Ensure("NatoA_Citta", typeof(string), 255);
                    c = table.Fields.Ensure("NatoA_Provincia", typeof(string), 255);
                }

                {
                    c = table.Fields.Ensure("MortoA_Citta", typeof(string), 255);
                    c = table.Fields.Ensure("MortoA_Provincia", typeof(string), 255);
                }

                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("Deceduto", typeof(bool), 1);
                c = table.Fields.Ensure("NomeFonte", typeof(string), 255);
                c = table.Fields.Ensure("TipoFonte", typeof(string), 255);
                c = table.Fields.Ensure("IDFonte", typeof(int), 1);
                c = table.Fields.Ensure("PFlags", typeof(int), 1);
                c = table.Fields.Ensure("NFlags", typeof(int), 1);
                c = table.Fields.Ensure("IDCanale", typeof(int), 1);
                c = table.Fields.Ensure("NomeCanale", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("DettaglioEsito", typeof(string), 255);
                c = table.Fields.Ensure("DettagllioEsito", typeof(string), 255);
                c = table.Fields.Ensure("IDReferente1", typeof(int), 1);
                c = table.Fields.Ensure("IDReferente2", typeof(int), 1);
                c = table.Fields.Ensure("IDStatoAttuale", typeof(int), 1);
                //c = table.Fields.Ensure("Attributi", typeof(string), 0);
            }


            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxTipoPersona", new string[] { "TipoPersona", "PFlags", "NFlags" }, DBFieldConstraintFlags.None);

                c = table.Constraints.Ensure("idxAlias", new string[] { "Alias1", "Alias2", "Professione", "Titolo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAnagrafica", new string[] { "CodiceFiscale", "PartitaIVA", "Sesso", "Cittadinanza" }, DBFieldConstraintFlags.None);
                 

                {
                    c = table.Constraints.Ensure("idxResidenza", new string[] { "ResidenteA_ID", "ResidenteA_CAP", "ResidenteA_Provincia", "ResidenteA_Citta", "ResidenteA_Via" }, DBFieldConstraintFlags.None);
                    //c = table.Fields.Ensure("ResidenteA_Nome", typeof(string), 255);
                    //c = table.Fields.Ensure("ResidenteA_Civico", typeof(string), 255);
                }

                {
                    c = table.Constraints.Ensure("idxDomicilio", new string[] { "DomiciliatoA_ID", "DomiciliatoA_CAP", "DomiciliatoA_Provincia", "DomiciliatoA_Citta", "DomiciliatoA_Via" }, DBFieldConstraintFlags.None);
                }

                {
                    c = table.Constraints.Ensure("idxNato", new string[] { "DataNascita", "NatoA_Provincia", "NatoA_Citta" }, DBFieldConstraintFlags.None);
                }

                {
                    c = table.Constraints.Ensure("idxMorto", new string[] { "DataMorte", "Morto_Provincia", "Morto_Citta" }, DBFieldConstraintFlags.None);
                }

                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
                //c = table.Fields.Ensure("Deceduto", typeof(bool), 1);
                c = table.Constraints.Ensure("idxFonte", new string[] { "IDFonte", "TipoFonte", "NomeFonte" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCanale", new string[] { "IDCanale", "NomeCanale" }, DBFieldConstraintFlags.None);

                c = table.Constraints.Ensure("idxDettagli", new string[] { "DettaglioEsito", "DettagllioEsito", "Descrizione" }, DBFieldConstraintFlags.None);

                c = table.Constraints.Ensure("idxReferenti", new string[] { "IDReferente1", "IDReferente2", "Descrizione" }, DBFieldConstraintFlags.None);

                c = table.Constraints.Ensure("idxStato", new string[] { "IDStatoAttuale" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Attributi", typeof(string), 0);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.Nominativo;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("NomeFonte", m_NomeFonte);
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("TipoPersona", (int?)TipoPersona);
                writer.WriteAttribute("Alias1", m_Alias1);
                writer.WriteAttribute("Alias2", m_Alias2);
                writer.WriteAttribute("Professione", m_Professione);
                writer.WriteAttribute("Titolo", m_Titolo);
                writer.WriteAttribute("Sesso", m_Sesso);
                writer.WriteAttribute("DataNascita", m_DataNascita);
                writer.WriteAttribute("DataMorte", m_DataMorte);
                writer.WriteAttribute("Cittadinanza", m_Cittadinanza);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                writer.WriteAttribute("PartitaIVA", m_PartitaIVA);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("Deceduto", m_Deceduto);
                writer.WriteAttribute("PFlags", (int?)m_Flags);
                writer.WriteAttribute("NFlags", (int?)m_NFlags);
                writer.WriteAttribute("IDCanale", IDCanale);
                writer.WriteAttribute("NomeCanale", m_NomeCanale);
                writer.WriteAttribute("DettaglioEsito", m_DettaglioEsito);
                writer.WriteAttribute("DettagllioEsito", m_DettaglioEsito1);
                writer.WriteAttribute("IDReferente1", IDReferente1);
                writer.WriteAttribute("IDReferente2", IDReferente2);
                writer.WriteAttribute("IDStatoAttuale", IDStatoAttuale);
                base.XMLSerialize(writer);
                writer.WriteTag("NatoA", m_NatoA);
                writer.WriteTag("MortoA", m_MortoA);
                writer.WriteTag("ResidenteA", m_ResidenteA);
                writer.WriteTag("DomiciliatoA", m_DomiciliatoA);
                writer.WriteTag("Descrizione", m_Descrizione);
                //writer.WriteTag("Attributi", Attributi);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                // Dim i As Integer
                switch (fieldName ?? "")
                {
                    case "NomeFonte":
                        {
                            m_NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            m_TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            m_IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoPersona": // Me.m_tipoPersona);
                        {
                            break;
                        }

                    case "Alias1":
                        {
                            m_Alias1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Alias2":
                        {
                            m_Alias2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Professione":
                        {
                            m_Professione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Titolo":
                        {
                            m_Titolo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Sesso":
                        {
                            m_Sesso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataNascita":
                        {
                            m_DataNascita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataMorte":
                        {
                            m_DataMorte = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Cittadinanza":
                        {
                            m_Cittadinanza = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NatoA":
                        {
                            m_NatoA = (CIndirizzo)fieldValue; //.InitializeFrom((CIndirizzo)fieldValue);
                            m_NatoA.SetPersona(this);
                            break;
                        }

                    case "MortoA":
                        {
                            m_MortoA = (CIndirizzo)fieldValue; //.InitializeFrom((CIndirizzo)fieldValue);
                            m_MortoA.SetPersona(this);
                            break;
                        }

                    case "ResidenteA":
                        {
                            m_ResidenteA = (CIndirizzo)fieldValue; //.InitializeFrom(fieldValue);
                            m_ResidenteA.SetPersona(this);
                            break;
                        }

                    case "DomiciliatoA":
                        {
                            m_DomiciliatoA = (CIndirizzo)fieldValue; // .InitializeFrom(fieldValue);
                            m_DomiciliatoA.SetPersona(this);
                            break;
                        }

                    case "CodiceFiscale":
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PartitaIVA":
                        {
                            m_PartitaIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Deceduto":
                        {
                            m_Deceduto = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "PFlags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NFlags":
                        {
                            m_NFlags = (PFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDCanale":
                        {
                            m_IDCanale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCanale":
                        {
                            m_NomeCanale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            m_DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettagllioEsito":
                        {
                            m_DettaglioEsito1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDReferente1":
                        {
                            m_IDReferente1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDReferente2":
                        {
                            m_IDReferente2 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDStatoAttuale":
                        {
                            m_IDStatoAttuale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    //case "Attributi":
                    //    {
                    //        m_Attributi.Clear();
                    //        CKeyCollection col = (CKeyCollection)fieldValue;
                    //        foreach (string k in col.Keys)
                    //            m_Attributi.Add(k, DMD.Strings.CStr(col[k]));
                    //        break;
                    //    }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Genera l'evento Created
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterCreate(DMDEventArgs e)
            {
                base.OnAfterCreate(e);
                var e1 = new ItemEventArgs<CPersona>(this);
                minidom.Anagrafica.OnPersonaCreated(e1);
                //minidom.Anagrafica.Module.DispatchEvent(new Sistema.EventDescription("Create", "Creata l'anagrafica di: " + Nominativo + " (ID:" + DBUtils.GetID(this) + ")", e1));
            }

            /// <summary>
            /// Genera l'evento Deleted
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterDelete(DMDEventArgs e)
            {
                base.OnAfterDelete(e);
                var e1 = new ItemEventArgs<CPersona>(this);
                minidom.Anagrafica.OnPersonaDeleted(e1);
                //minidom.Anagrafica.Module.DispatchEvent(new Sistema.EventDescription("Delete", "Eliminata l'anagrafica di: " + Nominativo + " (ID:" + DBUtils.GetID(this) + ")", e1));
            }

            /// <summary>
            /// Genera l'evento changed
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterChange(DMDEventArgs e)
            {
                base.OnAfterChange(e);
                var e1 = new ItemEventArgs<CPersona>(this);
                minidom.Anagrafica.OnPersonaModified(e1);
                //minidom.Anagrafica.Module.DispatchEvent(new Sistema.EventDescription("Edit", "Modificata l'anagrafica di: " + Nominativo + " (ID:" + DBUtils.GetID(this) + ")", e1));
            }

            /// <summary>
            /// Genera l'evento Merged
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnMerged(MergePersonaEventArgs e)
            {
                this.Merged?.Invoke(this, e);
                minidom.Anagrafica.OnPersonaMerged(e);
                //minidom.Anagrafica.Module.DispatchEvent(new Sistema.EventDescription("Merged", "Unito l'anagrafica di: " + Nominativo + " (ID:" + DBUtils.GetID(this) + ") con " + e.MI.NomePersona2 + " (ID:" + e.MI.IDPersona2 + ")", e));
            }

            /// <summary>
            /// Genera l'evento Unmerged
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnUnMerged(MergePersonaEventArgs e)
            {
                UnMerged?.Invoke(this, e);
                minidom.Anagrafica.OnPersonaUnMerged(e);
                //minidom.Anagrafica.Module.DispatchEvent(new Sistema.EventDescription("UnMerged", "Anullo l'unione dell'anagrafica di: " + Nominativo + " (ID:" + DBUtils.GetID(this) + ") con " + e.MI.NomePersona2 + " (ID:" + e.MI.IDPersona2 + ")", e));
            }

            /// <summary>
            /// Compara due persone in base al nominativo
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CPersona other)
            {
                return DMD.Strings.Compare(this.Nominativo, other.Nominativo, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CPersona)obj);
            }

            /// <summary>
            /// Cerca tra i recapiti il telefono principale o il primo numero di telefono valido (non cellulare)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Telefono
            {
                get
                {
                    var c = Recapiti.GetContattoPredefinito("Telefono");
                    if (c is object)
                        return c.Valore;
                    return "";
                }

                set
                {
                    Recapiti.SetContattoPredefinito("Telefono", value);
                }
            }

            /// <summary>
            /// Cerca tra i recapiti il Cellulare principale o il primo numero di telefono valido
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Cellulare
            {
                get
                {
                    var c = Recapiti.GetContattoPredefinito("Cellulare");
                    if (c is object)
                        return c.Valore;
                    return "";
                }

                set
                {
                    Recapiti.SetContattoPredefinito("Cellulare", value);
                }
            }

            /// <summary>
            /// Cerca tra i recapiti il Cellulare principale o il primo numero di telefono valido
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string eMail
            {
                get
                {
                    var c = Recapiti.GetContattoPredefinito("e-Mail");
                    if (c is object)
                        return c.Valore;
                    return "";
                }

                set
                {
                    Recapiti.SetContattoPredefinito("e-Mail", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo PEC predefinito
            /// </summary>
            public string PEC
            {
                get
                {
                    var c = Recapiti.GetContattoPredefinito("PEC");
                    if (c is object)
                        return c.Valore;
                    return "";
                }

                set
                {
                    Recapiti.SetContattoPredefinito("PEC", value);
                }
            }

            /// <summary>
            /// Cerca tra i recapiti il Cellulare principale o il primo numero di telefono valido
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Fax
            {
                get
                {
                    var c = Recapiti.GetContattoPredefinito("Fax");
                    if (c is object)
                        return c.Valore;
                    return "";
                }

                set
                {
                    Recapiti.SetContattoPredefinito("Fax", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo web predefinito
            /// </summary>
            public string WebSite
            {
                get
                {
                    var c = Recapiti.GetContattoPredefinito("WebSite");
                    if (c is object)
                        return c.Valore;
                    return "";
                }

                set
                {
                    Recapiti.SetContattoPredefinito("WebSite", value);
                }
            }

            /// <summary>
            /// Clona l'oggetto corrente
            /// </summary>
            /// <returns></returns>
            protected override Databases.DBObjectBase _Clone()
            {
                var ret = (CPersona) this.MemberwiseClone(); //(CPersona)Activator.CreateInstance(GetType());
                //ret.InitializeFrom(this);
                ret.m_Annotazioni = null;
                ret.m_Attachments = null;
                ret.m_Recapiti = null;
                ret.m_NatoA.SetPersona(ret);
                ret.m_MortoA.SetPersona(ret);
                ret.m_ResidenteA.SetPersona(ret);
                ret.m_DomiciliatoA.SetPersona(ret);
                return ret;
            }
             

            /// <summary>
            /// Effettua un passaggio di stato
            /// </summary>
            /// <param name="fromStato"></param>
            /// <param name="rule"></param>
            /// <param name="note"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public TaskLavorazione ChangeStatus(
                            TaskLavorazione fromStato, 
                            RegolaTaskLavorazione rule, 
                            string note, 
                            object context
                            )
            {
                var ret = new TaskLavorazione();
                ret.RegolaEseguita = rule;
                ret.DataInizioEsecuzione = DMD.DateUtils.Now();
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.StatoAttuale = rule.StatoDestinazione;
                ret.Cliente = this;
                ret.PuntoOperativo = PuntoOperativo;
                ret.AssegnatoA = fromStato.AssegnatoA;
                ret.AssegnatoDa = Sistema.Users.CurrentUser;
                ret.Categoria = fromStato.Categoria;
                ret.DataAssegnazione = DMD.DateUtils.Now();
                ret.DataPrevista = DMD.DateUtils.Now();
                // ret.Sorgente = Me
                ret.TaskSorgente = fromStato;
                ret.IDContesto = DBUtils.GetID(context, 0);
                ret.TipoContesto = DMD.RunTime.vbTypeName(context);
                ret.Save();
                fromStato.TaskDestinazione = ret;
                fromStato.DataFineEsecuzione = ret.DataInizioEsecuzione;
                fromStato.Save();
                StatoAttuale = ret.StatoAttuale;
                if (StatoAttuale is object)
                {
                    DettaglioEsito = StatoAttuale.Descrizione;
                    DettaglioEsito1 = StatoAttuale.Descrizione2;
                }

                Save();
                return ret;
            }

            string[] Sistema.IIndexable.GetIndexedWords()
            {
                return this.GetIndexedWords();
            }

            string[] Sistema.IIndexable.GetKeyWords()
            {
                return this.GetKeyWords();
            }

            // Public Function ChangeStatus(ByVal fromStato As TaskLavorazione, ByVal toStato As StatoTaskLavorazione, ByVal note As String, ByVal context As Object) As TaskLavorazione
            // Dim ret As New TaskLavorazione
            // ret.RegolaEseguita = Nothing
            // ret.DataInizioEsecuzione = Calendar.Now
            // ret.Stato = ObjectStatus.OBJECT_VALID
            // ret.StatoAttuale = toStato
            // ret.Cliente = Me
            // ret.PuntoOperativo = Me.PuntoOperativo
            // ret.AssegnatoA = fromStato.AssegnatoA
            // ret.AssegnatoDa = Sistema.Users.CurrentUser
            // ret.Categoria = fromStato.Categoria
            // ret.DataAssegnazione = Calendar.Now
            // ret.DataPrevista = Calendar.Now
            // 'ret.Sorgente = Me
            // ret.TaskSorgente = fromStato
            // ret.IDContesto = GetID(context)
            // ret.TipoContesto = vbTypeName(context)
            // ret.Save()

            // fromStato.TaskDestinazione = ret
            // fromStato.DataFineEsecuzione = ret.DataInizioEsecuzione
            // fromStato.Save()

            // Me.StatoAttuale = ret.StatoAttuale
            // If (Me.StatoAttuale IsNot Nothing) Then
            // Me.DettaglioEsito = Me.StatoAttuale.Descrizione
            // Me.DettaglioEsito1 = Me.StatoAttuale.Descrizione2
            // End If
            // Me.Save()

            // Return ret
            // End Function

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CPersona) && this.Equals((CPersona)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CPersona obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Alias1, obj.m_Alias1)
                     && DMD.Strings.EQ(this.m_Alias2, obj.m_Alias2)
                    && DMD.Strings.EQ(this.m_Professione, obj.m_Professione)
                    && DMD.Strings.EQ(this.m_Titolo, obj.m_Titolo)
                    && DMD.Strings.EQ(this.m_Sesso, obj.m_Sesso)
                    && DMD.DateUtils.EQ(this.m_DataNascita, obj.m_DataNascita)
                    && DMD.DateUtils.EQ(this.m_DataMorte, obj.m_DataMorte)
                    && DMD.Strings.EQ(this.m_Cittadinanza, obj.m_Cittadinanza)
                    && this.m_NatoA.Equals(obj.m_NatoA)
                    && this.m_MortoA.Equals(obj.m_MortoA)
                    && this.m_ResidenteA.Equals(obj.m_ResidenteA)
                    && this.m_DomiciliatoA.Equals(obj.m_DomiciliatoA)
                    && DMD.Strings.EQ(this.m_CodiceFiscale, obj.m_CodiceFiscale)
                    && DMD.Strings.EQ(this.m_PartitaIVA, obj.m_PartitaIVA)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.Booleans.EQ(this.m_Deceduto, obj.m_Deceduto)
                    && DMD.Strings.EQ(this.m_TipoFonte, obj.m_TipoFonte)
                    && DMD.Integers.EQ(this.IDFonte, obj.IDFonte)
                    && DMD.Strings.EQ(this.m_NomeFonte, obj.m_NomeFonte)
                    && DMD.Integers.EQ((int)this.m_Flags, (int)obj.m_Flags)
                    && DMD.Integers.EQ((int)this.m_NFlags, (int)obj.m_NFlags)
                    && DMD.Integers.EQ(this.IDCanale, obj.IDCanale)
                    && DMD.Strings.EQ(this.m_NomeCanale, obj.m_NomeCanale)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_DettaglioEsito, obj.m_DettaglioEsito)
                    && DMD.Strings.EQ(this.m_DettaglioEsito1, obj.m_DettaglioEsito1)
                    && DMD.Integers.EQ(this.IDReferente1, obj.IDReferente1)
                    && DMD.Integers.EQ(this.IDReferente2, obj.IDReferente2)
                    && DMD.Integers.EQ(this.IDStatoAttuale, obj.IDStatoAttuale)
                    ;
            //[NonSerialized] private StatoTaskLavorazione m_StatoAttuale;
            //private CKeyCollection m_Attributi;
            }

            /// <summary>
            /// Restitusice il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.TipoPersona, this.Nominativo, this.DataNascita, this.NatoA, this.ResidenteA);
            }

            /// <summary>
            /// Inizializza i campi dalla persona
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            protected virtual CPersona InitializeFrom(CPersona value)
            {
                this.m_Alias1 = value.m_Alias1;
                this.m_Alias2 = value.m_Alias2;
                this.m_Professione = value.m_Professione;
                this.m_Titolo = value.m_Titolo;
                this.m_Sesso = value.m_Sesso;
                this.m_DataNascita = value.m_DataNascita;
                this.m_DataMorte = value.m_DataMorte;
                this.m_Cittadinanza = value.m_Cittadinanza;
                this.m_NatoA.InitializeFrom(value.m_NatoA);
                this.m_MortoA.InitializeFrom(value.m_MortoA);
                this.m_ResidenteA.InitializeFrom(value.m_ResidenteA);
                this.m_DomiciliatoA.InitializeFrom(value.m_DomiciliatoA);
                this.m_CodiceFiscale = value.m_CodiceFiscale;
                this.m_PartitaIVA = value.m_PartitaIVA;
                this.m_IconURL = value.m_IconURL;
                this.m_Deceduto = value.m_Deceduto;
                this.m_TipoFonte = value.m_TipoFonte;
                this.m_IDFonte = value.IDFonte;
                this.m_Fonte = value.m_Fonte;
                this.m_NomeFonte = value.m_NomeFonte;
                this.m_Flags = value.m_Flags;
                this.m_NFlags = value.m_NFlags;
                this.m_Canale = value.m_Canale;
                this.m_IDCanale = value.m_IDCanale;
                this.m_NomeCanale = value.m_NomeCanale;
                this.m_Descrizione = value.m_Descrizione;
                this.m_DettaglioEsito = value.m_DettaglioEsito;
                this.m_DettaglioEsito1 = value.m_DettaglioEsito1;
                this.m_IDReferente1 = value.m_IDReferente1;
                this.m_Referente1 = value.m_Referente1;
                this.m_IDReferente2 = value.m_IDReferente2;
                this.m_Referente2 = value.m_Referente2;
            //    Serialized] private Sistema.CAnnotazioni m_Annotazioni;
            //[NonSerialized] private Sistema.CAttachmentsCollection m_Attachments;
            //[NonSerialized] private CContattiPerPersonaCollection m_Recapiti;
            //private int m_IDStatoAttuale;
            //[NonSerialized] private StatoTaskLavorazione m_StatoAttuale;
            //private CKeyCollection m_Attributi;

                return this;
            }

            void ISupportInitializeFrom.InitializeFrom(object value) { this.InitializeFrom((CPersona)value); }
        }
    }
}