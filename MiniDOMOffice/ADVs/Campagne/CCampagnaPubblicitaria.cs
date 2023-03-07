using System;
using System.Collections.Generic;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class ADV
    {
        /// <summary>
        /// Tipi di campagne pubblicitarie
        /// </summary>
        public enum TipoCampagnaPubblicitaria : int
        {
            /// <summary>
            /// Non impostato
            /// </summary>
            NonImpostato = 0,

            /// <summary>
            /// Campagna basata sull'invio di email 
            /// </summary>
            eMail = 1,

            /// <summary>
            /// Campagna basata sulla chiamata a servizi web
            /// </summary>
            Web = 2,

            /// <summary>
            /// Campagna manuale basata sulla pubblicazione su sistemi cartacei
            /// </summary>
            Quotidiani = 3,

            /// <summary>
            /// Campagna basata sull'invio di fax
            /// </summary>
            Fax = 4,

            /// <summary>
            /// Campagna basata sull'invio di SMS
            /// </summary>
            SMS = 5
        }

        /// <summary>
        /// Stati di una campagna pubblicitaria
        /// </summary>
        public enum StatoCampagnaPubblicitaria : int
        {
            /// <summary>
            /// In attesa
            /// </summary>
            InAttesa = 0,

            /// <summary>
            /// Pronta per l'invio
            /// </summary>
            Programmata = 1,

            /// <summary>
            /// Eseguita
            /// </summary>
            Eseguita = 2
        }

        /// <summary>
        /// Campagna pubblicitaria
        /// </summary>
        [Serializable]
        public class CCampagnaPubblicitaria 
            : minidom.Databases.DBObject, ISchedulable
        {
            private const int UsaSoloIndirizziVerificatiFlag = 1;
            private const int UsaUnSoloContattoPerPersonaFlag = 2;
            private MultipleScheduleCollection m_Programmazione;   // Parametri di programmazione della campagna
            private string m_NomeCampagna;                    // Nome della campagna
            private string m_Titolo;                          // Titolo della campagna
            private string m_Testo;                           // Contenuto della campagna
                                                              // Private m_TipoTesto As String                       'Tipo del contenuto della campanga (immagine, testo html, ecc)
            private bool m_UsaListaDinamica;               // Se vero la lista di ricontatti viene aggiornata in base al criterio di ricerca
            private string m_ParametriLista;                  // Parametri di ricerca per la lista dinamica
            private string m_IndirizzoMittente;
            private string m_NomeMittente;
            // Private m_IDListaDestinatari As Integer
            // Private m_ListaDestinatari As CListaRicontatti           'Lista di ricontatti a cui inviare la campagna
            private bool m_Attiva;                         // Se vera indica che la campagna è abilitata
            private TipoCampagnaPubblicitaria m_TipoCampagna;
            private StatoCampagnaPubblicitaria m_StatoCampagna;            // Stato della campagna
            private bool m_RichiediConfermaDiLettura;
            private bool m_RichiediConfermaDiRecapito;
            private string m_FileDaUtilizzare;
            private string m_ListaCC;
            private string m_ListaCCN;
            private string m_ListaNO;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagnaPubblicitaria()
            {
                m_TipoCampagna = TipoCampagnaPubblicitaria.NonImpostato;
                m_Programmazione = null;
                m_NomeCampagna = "";
                m_Titolo = "";
                // Me.m_TipoTesto = "html"
                m_Testo = "";
                m_UsaListaDinamica = true;
                m_ParametriLista = "";
                m_IndirizzoMittente = "";
                m_NomeMittente = "";
                // Private m_IDListaDestinatari As Integer
                // Private m_ListaDestinatari As CListaRicontatti           'Lista di ricontatti a cui inviare la campagna
                m_Attiva = true;
                m_StatoCampagna = StatoCampagnaPubblicitaria.InAttesa;
                m_RichiediConfermaDiLettura = false;
                m_RichiediConfermaDiRecapito = false;
                m_Flags = 0;
                m_ListaCC = DMD.Strings.vbNullString;
                m_ListaCCN = DMD.Strings.vbNullString;
                m_ListaNO = "";
            }

            void ISchedulable.InvalidateProgrammazione() { this.InvalidateProgrammazione();  }
            void ISchedulable.NotifySchedule(CalendarSchedule s) { this.NotifySchedule(s); }

            /// <summary>
            /// Restituisce i lotti 
            /// </summary>
            /// <returns></returns>
            public IEnumerable<DateTime> GetLotti()
            {
                var dic = new Dictionary<DateTime, DateTime>();
                if (DBUtils.GetID(this, 0) != 0) return dic.Values;
                using (var cursor = new CRisultatoCampagnaCursor())
                {
                    cursor.IDCampagna.Value = DBUtils.GetID(this, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        if (cursor.Item.DataEsecuzione.HasValue)
                            dic.Add(cursor.Item.DataEsecuzione.Value, cursor.Item.DataEsecuzione.Value);
                    }                    
                }
                return dic.Values;
                 
            }

            /// <summary>
            /// Restituisce o imposta la lista degli indirizzi bloccati
            /// </summary>
            public string ListaNO
            {
                get
                {
                    return m_ListaNO;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ListaNO;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ListaNO = value;
                    DoChanged("ListaNO", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una lista di destinatari in Copia Carbone
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ListaCC
            {
                get
                {
                    return m_ListaCC;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ListaCC;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ListaCC = value;
                    DoChanged("ListaCC", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una lista di destinatari in Copia Carbone Nascosta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ListaCCN
            {
                get
                {
                    return m_ListaCCN;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ListaCCN;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ListaCCN = value;
                    DoChanged("ListaCCN", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta il file da inviare (se vuoto verrà utilizzato il campo Testo)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FileDaUtilizzare
            {
                get
                {
                    return m_FileDaUtilizzare;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_FileDaUtilizzare;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FileDaUtilizzare = value;
                    DoChanged("FileDaUtilizzare", value, oldValue);
                }
            }

            /// <summary>
            /// Se vero istruisce l'handler ad utilizzare solo gli indirizzi per cui è stato specificato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool UsaSoloIndirizziVerificati
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, UsaSoloIndirizziVerificatiFlag);
                }

                set
                {
                    if (UsaSoloIndirizziVerificati == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, UsaSoloIndirizziVerificatiFlag, value);
                    DoChanged("UsaSoloIndirizziVerificati", value, !value);
                }
            }

            /// <summary>
            /// Se true la campagna utilizzerà solo l'indirizzo predefinito per la persona.
            /// Se false la campagna utilizzerà tutti gli indirizzi validi per la persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool UsaUnSoloContattoPerPersona
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, UsaUnSoloContattoPerPersonaFlag);
                }

                set
                {
                    if (UsaUnSoloContattoPerPersona == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, UsaUnSoloContattoPerPersonaFlag, value);
                    DoChanged("UsaUnSoloContattoPerPersona", value, !value);
                }
            }

            /// <summary>
            /// Restituisce la programmazione dell'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.MultipleScheduleCollection Programmazione
            {
                get
                {
                    lock (this)
                    {
                        if (m_Programmazione is null)
                            m_Programmazione = new Sistema.MultipleScheduleCollection(this);
                        return m_Programmazione;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della campagna
            /// </summary>
            public string NomeCampagna
            {
                get
                {
                    return m_NomeCampagna;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCampagna;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCampagna = value;
                    DoChanged("NomeCampagna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il titolo della campagna
            /// </summary>
            public string Titolo
            {
                get
                {
                    return m_Titolo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Titolo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Titolo = value;
                    DoChanged("Titolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il testo (in formato HTML se supportato dall'handler) della campagna. Se il campo FileDaUtilizzare il campo Testo viene ignorato in favore del contenuto del file
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Testo
            {
                get
                {
                    return m_Testo;
                }

                set
                {
                    string oldValue = m_Testo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Testo = value;
                    DoChanged("Testo", value, oldValue);
                }
            }

            /// <summary>
            /// Se vero indica al sistema di utilizzare i parametri della lista per effettuare una ricerca all'interno del CRM (nel momento in cui viene eseguita la campagna).
            /// Se falso i parametri della lista verrano utilizzati come elenco di indirizzi separati dal carattere ;
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool UsaListaDinamica
            {
                get
                {
                    return m_UsaListaDinamica;
                }

                set
                {
                    if (m_UsaListaDinamica == value)
                        return;
                    m_UsaListaDinamica = value;
                    DoChanged("UsaListaDinamica", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che rappresenta i parametri della lista
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ParametriLista
            {
                get
                {
                    return m_ParametriLista;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_ParametriLista;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ParametriLista = value;
                    DoChanged("ParametriLista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo del mittente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IndirizzoMittente
            {
                get
                {
                    return m_IndirizzoMittente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IndirizzoMittente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IndirizzoMittente = value;
                    DoChanged("IndirizzoMittente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mittente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeMittente
            {
                get
                {
                    return m_NomeMittente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeMittente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeMittente = value;
                    DoChanged("NomeMittente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo della campagna
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public TipoCampagnaPubblicitaria TipoCampagna
            {
                get
                {
                    return m_TipoCampagna;
                }

                set
                {
                    var oldValue = m_TipoCampagna;
                    if (oldValue == value)
                        return;
                    m_TipoCampagna = value;
                    DoChanged("TipoCampagna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore boolean che indica se è richiesta la conferma di lettura
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool RichiediConfermaDiLettura
            {
                get
                {
                    return m_RichiediConfermaDiLettura;
                }

                set
                {
                    if (value == m_RichiediConfermaDiLettura)
                        return;
                    m_RichiediConfermaDiLettura = value;
                    DoChanged("RichiediConfermaDiLettura", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica se è richiesta la conferma di recapito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool RichiediConfermaDiRecapito
            {
                get
                {
                    return m_RichiediConfermaDiRecapito;
                }

                set
                {
                    if (m_RichiediConfermaDiRecapito == value)
                        return;
                    m_RichiediConfermaDiRecapito = value;
                    DoChanged("RichiediConfermaDiRecapito", value, !value);
                }
            }

            /// <summary>
            /// Restituisce la data dell'ultima esecuzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? UltimaEsecuzione
            {
                get
                {
                    DateTime? ret = default;
                    foreach (Sistema.CalendarSchedule s in Programmazione)
                    {
                        var p = s.UltimaEsecuzione;
                        if (p.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                if (ret.Value < p.Value)
                                    ret = p;
                            }
                            else
                            {
                                ret = p;
                            }
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce la data della prossima esecuzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? ProssimaEsecuzione
            {
                get
                {
                    DateTime? ret = default;
                    foreach (Sistema.CalendarSchedule s in Programmazione)
                    {
                        var p = s.CalcolaProssimaEsecuzione();
                        if (p.HasValue)
                        {
                            if (ret.HasValue)
                            {
                                if (p.Value < ret.Value)
                                    ret = p;
                            }
                            else
                            {
                                ret = p;
                            }
                        }
                    }

                    return ret;
                }
            }

            // Public Property IDListaDestinatari As Integer
            // Get
            // Return GetID(Me.m_ListaDestinatari, Me.m_IDListaDestinatari)
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.IDListaDestinatari
            // If (oldValue = value) Then Exit Property
            // Me.m_IDListaDestinatari = value
            // Me.m_ListaDestinatari = Nothing
            // Me.DoChanged("IDListaDestinatari", value, oldValue)
            // End Set
            // End Property

            // Public Property ListaDestinatari As CListaRicontatti           '
            // Get
            // If Me.m_ListaDestinatari Is Nothing Then Me.m_ListaDestinatari = Anagrafica.ListeRicontatto.GetItemById(Me.m_IDListaDestinatari)
            // Return Me.m_ListaDestinatari
            // End Get
            // Set(value As CListaRicontatti)
            // Me.m_ListaDestinatari = value
            // Me.m_IDListaDestinatari = GetID(value)
            // Me.DoChanged("ListaDestinatari", value)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce o imposta un valore boolean che indica se la campagna è attiva o meno
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attiva
            {
                get
                {
                    return m_Attiva;
                }

                set
                {
                    if (m_Attiva == value)
                        return;
                    m_Attiva = value;
                    DoChanged("Attiva", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato di programmazione della campagna
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoCampagnaPubblicitaria StatoCampagna
            {
                get
                {
                    return m_StatoCampagna;
                }

                internal set
                {
                    var oldValue = m_StatoCampagna;
                    if (oldValue == value)
                        return;
                    m_StatoCampagna = value;
                    DoChanged("StatoCampagna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_NomeCampagna;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeCampagna, this.m_TipoCampagna);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CCampagnaPubblicitaria) && this.Equals((CCampagnaPubblicitaria)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CCampagnaPubblicitaria obj)
            {
                return base.Equals(obj)
                        && DMD.Strings.EQ(this.m_NomeCampagna, obj.m_NomeCampagna)
                        && DMD.Strings.EQ(this.m_Titolo, obj.m_Titolo)
                        && DMD.Strings.EQ(this.m_Testo, obj.m_Testo)
                        && DMD.Booleans.EQ(this.m_UsaListaDinamica, obj.m_UsaListaDinamica)
                        && DMD.Strings.EQ(this.m_ParametriLista, obj.m_ParametriLista)
                        && DMD.Strings.EQ(this.m_IndirizzoMittente, obj.m_IndirizzoMittente)
                        && DMD.Strings.EQ(this.m_NomeMittente, obj.m_NomeMittente)
                        && DMD.Booleans.EQ(this.m_Attiva, obj.m_Attiva)
                        && DMD.Integers.EQ((int)this.m_TipoCampagna, (int)obj.m_TipoCampagna)
                        && DMD.Integers.EQ((int)this.m_StatoCampagna, (int)obj.m_StatoCampagna)
                        && DMD.Booleans.EQ(this.m_RichiediConfermaDiLettura, obj.m_RichiediConfermaDiLettura)
                        && DMD.Booleans.EQ(this.m_RichiediConfermaDiRecapito, obj.m_RichiediConfermaDiRecapito)
                        && DMD.Strings.EQ(this.m_FileDaUtilizzare, obj.m_FileDaUtilizzare)
                        && DMD.Strings.EQ(this.m_ListaCC, obj.m_ListaCC)
                        && DMD.Strings.EQ(this.m_ListaCCN, obj.m_ListaCCN)
                        && DMD.Strings.EQ(this.m_ListaNO, obj.m_ListaNO)
                        ;

            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.ADV.Campagne;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ADVs";
            }

            
            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_NomeCampagna = reader.Read("NomeCampagna", m_NomeCampagna);
                m_Titolo = reader.Read("Titolo", m_Titolo);
                m_Testo = reader.Read("Testo", m_Testo);
                // reader.Read("TipoTesto", Me.m_TipoTesto)
                m_UsaListaDinamica = reader.Read("UsaListaDinamica", m_UsaListaDinamica);
                m_ParametriLista = reader.Read("ParametriLista", m_ParametriLista);
                // reader.Read("IDListaDestinatari", Me.m_IDListaDestinatari)
                m_Attiva = reader.Read("Attiva",  m_Attiva);
                m_NomeMittente = reader.Read("NomeMittente",  m_NomeMittente);
                m_IndirizzoMittente = reader.Read("IndirizzoMittente",  m_IndirizzoMittente);
                m_StatoCampagna = reader.Read("StatoCampagna",  m_StatoCampagna);
                m_TipoCampagna = reader.Read("TipoCampagna",  m_TipoCampagna);
                m_RichiediConfermaDiLettura = reader.Read("ConfermaLettura",  m_RichiediConfermaDiLettura);
                m_RichiediConfermaDiRecapito = reader.Read("ConfermaRecapito",  m_RichiediConfermaDiRecapito);
                m_FileDaUtilizzare = reader.Read("FileDaUtilizzare",  m_FileDaUtilizzare);
                m_ListaCC = reader.Read("ListaCC",  m_ListaCC);
                m_ListaCCN = reader.Read("ListaCCN",  m_ListaCCN);
                m_ListaNO = reader.Read("ListaNO",  m_ListaNO);
                string tmp = reader.Read("Programmazione", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.m_Programmazione = DMD.XML.Utils.Deserialize<MultipleScheduleCollection>(tmp);
                    this.m_Programmazione.SetOwner(this);
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
                writer.Write("NomeCampagna", m_NomeCampagna);
                writer.Write("Titolo", m_Titolo);
                writer.Write("Testo", m_Testo);
                // writer.Write("TipoTesto", Me.m_TipoTesto)
                writer.Write("UsaListaDinamica", m_UsaListaDinamica);
                writer.Write("ParametriLista", m_ParametriLista);
                // writer.Write("IDListaDestinatari", Me.IDListaDestinatari)
                writer.Write("Attiva", m_Attiva);
                writer.Write("NomeMittente", m_NomeMittente);
                writer.Write("IndirizzoMittente", m_IndirizzoMittente);
                writer.Write("StatoCampagna", m_StatoCampagna);
                writer.Write("TipoCampagna", m_TipoCampagna);
                writer.Write("ConfermaLettura", m_RichiediConfermaDiLettura);
                writer.Write("ConfermaRecapito", m_RichiediConfermaDiRecapito);
                writer.Write("FileDaUtilizzare", m_FileDaUtilizzare);
                writer.Write("ListaCC", m_ListaCC);
                writer.Write("ListaCCN", m_ListaCCN);
                writer.Write("ListaNO", m_ListaNO);
                writer.Write("Programmazione", DMD.XML.Utils.Serialize(this.Programmazione));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("NomeCampagna", typeof(string), 255);
                c = table.Fields.Ensure("Titolo", typeof(string), 255);
                c = table.Fields.Ensure("Testo", typeof(string), 0);
                c = table.Fields.Ensure("UsaListaDinamica", typeof(bool), 1);
                c = table.Fields.Ensure("ParametriLista", typeof(string), 0);
                c = table.Fields.Ensure("Attiva", typeof(bool), 1);
                c = table.Fields.Ensure("NomeMittente", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoMittente", typeof(string), 255);
                c = table.Fields.Ensure("StatoCampagna", typeof(int), 1);
                c = table.Fields.Ensure("TipoCampagna", typeof(int), 1);
                c = table.Fields.Ensure("ConfermaLettura", typeof(bool), 1);
                c = table.Fields.Ensure("ConfermaRecapito", typeof(bool), 1);
                c = table.Fields.Ensure("FileDaUtilizzare", typeof(string), 255);
                c = table.Fields.Ensure("ListaCC", typeof(string), 0);
                c = table.Fields.Ensure("ListaCCN", typeof(string), 0);
                c = table.Fields.Ensure("ListaNO", typeof(string), 0);
                c = table.Fields.Ensure("Programmazione", typeof(string), 0);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "NomeCampagna", "StatoCampagna", "TipoCampagna" }, DBFieldConstraintFlags.None );
                c = table.Constraints.Ensure("idxTesto", new string[] { "Titolo", "Testo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFlags", new string[] { "Attiva", "UsaListaDinamica", "Testo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMittente", new string[] { "IndirizzoMittente", "NomeMittente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri", new string[] { "FileDaUtilizzare", "ConfermaLettura", "ConfermaRecapito" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxListe1", new string[] { "ParametriLista", "ListaCC" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxListe2", new string[] { "ListaNO", "ListaCCN" }, DBFieldConstraintFlags.None);
                 

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                // writer.WriteTag("Programmazione", Me.Programmazione)
                writer.WriteAttribute("NomeCampagna", m_NomeCampagna);
                writer.WriteAttribute("Titolo", m_Titolo);
                // writer.WriteTag("TipoTesto", Me.m_TipoTesto)
                writer.WriteAttribute("UsaListaDinamica", m_UsaListaDinamica);
                // writer.WriteTag("IDListaDestinatari", Me.IDListaDestinatari)
                writer.WriteAttribute("Attiva", m_Attiva);
                writer.WriteAttribute("NomeMittente", m_NomeMittente);
                writer.WriteAttribute("IndirizzoMittente", m_IndirizzoMittente);
                writer.WriteAttribute("StatoCampagna", (int?)m_StatoCampagna);
                writer.WriteAttribute("TipoCampagna", (int?)m_TipoCampagna);
                writer.WriteAttribute("ConfermaLettura", m_RichiediConfermaDiLettura);
                writer.WriteAttribute("ConfermaRecapito", m_RichiediConfermaDiRecapito);
                writer.WriteAttribute("FileDaUtilizzare", m_FileDaUtilizzare);
                base.XMLSerialize(writer);
                writer.WriteTag("Programmazione", Programmazione);
                writer.WriteTag("ListaCC", m_ListaCC);
                writer.WriteTag("ListaCCN", m_ListaCCN);
                writer.WriteTag("Testo", m_Testo);
                writer.WriteTag("ParametriLista", m_ParametriLista);
                writer.WriteTag("ListaNO", m_ListaNO);
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
                    // Case "Programmazione" : Me.m_Programmazione = fieldValue
                    case "NomeCampagna":
                        {
                            m_NomeCampagna = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Titolo":
                        {
                            m_Titolo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Testo":
                        {
                            m_Testo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // Case "TipoTesto" : Me.m_TipoTesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
                    case "UsaListaDinamica":
                        {
                            m_UsaListaDinamica = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ParametriLista":
                        {
                            m_ParametriLista = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // Case "IDListaDestinatari" : Me.m_IDListaDestinatari = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    case "Attiva":
                        {
                            m_Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "NomeMittente":
                        {
                            m_NomeMittente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IndirizzoMittente":
                        {
                            m_IndirizzoMittente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoCampagna":
                        {
                            m_StatoCampagna = (StatoCampagnaPubblicitaria)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCampagna":
                        {
                            m_TipoCampagna = (TipoCampagnaPubblicitaria)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConfermaLettura":
                        {
                            m_RichiediConfermaDiLettura = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ConfermaRecapito":
                        {
                            m_RichiediConfermaDiRecapito = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Programmazione":
                        {
                            m_Programmazione = (Sistema.MultipleScheduleCollection)fieldValue;
                            m_Programmazione.SetOwner(this);
                            break;
                        }

                    case "FileDaUtilizzare":
                        {
                            m_FileDaUtilizzare = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ListaCC":
                        {
                            m_ListaCC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ListaCCN":
                        {
                            m_ListaCCN = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ListaNO":
                        {
                            m_ListaNO = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce la collezione degli indirizzi supportati dall'handler 
            /// del tipo campagna sulla base dei filtri definiti dalla campagna stessa
            /// </summary>
            /// <returns></returns>
            public CCollection<CRisultatoCampagna> GetListaDiInvio()
            {
                var handler = Campagne.GetHandler(m_TipoCampagna);
                var ret = handler.GetListaInvio(this);
                return ret;
            }

            /// <summary>
            /// Invia i messaggi alla lista di invio
            /// </summary>
            /// <param name="lista"></param>
            public void Invia(CCollection<CRisultatoCampagna> lista)
            {
                var de = DMD.DateUtils.Now();
                foreach (CRisultatoCampagna item in lista)
                {
                    item.DataEsecuzione = de;
                    item.Invia();
                }
            }

            /// <summary>
            /// Restituisce la collezione dei messaggi della campagna
            /// </summary>
            /// <returns></returns>
            public CCollection<CRisultatoCampagna> GetMessaggiCampagna()
            {
                var risultatiCampagna = new CCollection<CRisultatoCampagna>();
                if (DBUtils.GetID(this, 0) != 0)
                {
                    using (var cursor = new CRisultatoCampagnaCursor())
                    {
                        cursor.IDCampagna.Value = DBUtils.GetID(this, 0);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.CreatoIl.SortOrder = SortEnum.SORT_ASC;
                        while (cursor.Read())
                        {
                            risultatiCampagna.Add(cursor.Item);
                        }

                    }
                }

                return risultatiCampagna;
            }

            /// <summary>
            /// Restituisce i messaggi nello stato specifico
            /// </summary>
            /// <param name="stato"></param>
            /// <returns></returns>
            public CCollection<CRisultatoCampagna> GetMessaggiCampagna(StatoMessaggioCampagna stato)
            {
                var risultatiCampagna = new CCollection<CRisultatoCampagna>();
                if (DBUtils.GetID(this, 0) != 0)
                {
                    using (var cursor = new CRisultatoCampagnaCursor())
                    {
                        cursor.IDCampagna.Value = DBUtils.GetID(this, 0);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.StatoMessaggio.Value = stato;
                        cursor.CreatoIl.SortOrder = SortEnum.SORT_ASC;
                        while (cursor.Read())
                        {
                            risultatiCampagna.Add(cursor.Item);
                        }

                    }
                }

                return risultatiCampagna;
            }

            //public override void InitializeFrom(object value)
            //{
            //    base.InitializeFrom(value);
            //    m_Programmazione = null;
            //}

            /// <summary>
            /// Invalida la programmazione
            /// </summary>
            protected internal void InvalidateProgrammazione()
            {
                lock (this)
                    m_Programmazione = null;
            }

            /// <summary>
            /// Notifica la modifica della programmazione
            /// </summary>
            /// <param name="s"></param>
            protected internal void NotifySchedule(Sistema.CalendarSchedule s)
            {
                lock (this)
                {
                    if (m_Programmazione is null)
                        return;
                    var o = m_Programmazione.GetItemById(DBUtils.GetID(s));
                    if (ReferenceEquals(o, s))
                    {
                        return;
                    }

                    if (o is object)
                    {
                        int i = m_Programmazione.IndexOf(o);
                        if (s.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            m_Programmazione[i] = s;
                        }
                        else
                        {
                            m_Programmazione.RemoveAt(i);
                        }
                    }
                    else if (s.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        m_Programmazione.Add(s);
                    }
                }
            }
        }
    }
}