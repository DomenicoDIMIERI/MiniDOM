using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Flag di un contatto
        /// </summary>
        [Flags]
        public enum ContattoFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            NONE = 0,

            /// <summary>
            /// Il cliente consente la pubblicità su questo numero
            /// </summary>
            CONSENTE_PUBBLICITA = 1,

            /// <summary>
            /// Il cliente ha negato esplicitamente la pubblicità su questo numero
            /// </summary>
            NEGA_PUBBLICITA = 2,

            /// <summary>
            /// Contatto predefinito
            /// </summary>
            PREDEFINITO = 4,

            /// <summary>
            /// 
            /// </summary>
            RICHIEDE_IDENTIFICATIVO = 64,

            /// <summary>
            /// Il cliente non risponde su questo numero
            /// </summary>
            NONRISPONDE = 128,

            /// <summary>
            /// Il contatto è bloccato
            /// </summary>
            BLOCCATO = 256
        }

        /// <summary>
        /// Stato del contatto
        /// </summary>
        public enum StatoRecapito : int
        {
            /// <summary>
            /// Sconosciuto
            /// </summary>
            Sconosciuto = 0,

            /// <summary>
            /// Contatto attivo
            /// </summary>
            Attivo = 1,

            /// <summary>
            /// Contatto intattivo
            /// </summary>
            Inattivo = 3,

            /// <summary>
            /// Il contatto non appartiene alla persona 
            /// </summary>
            AltraPersona = 4,

            /// <summary>
            /// Non risponde a questo numero
            /// </summary>
            NonRisponde = 5
        }


        /// <summary>
        /// Contatto relativo ad una persona
        /// </summary>
        [Serializable]
        public class CContatto 
            : Databases.DBObject, IComparable, ICloneable
        {
            private int m_PersonaID;     // ID della persona associata
            [NonSerialized] private CPersona m_Persona; // Oggetto CPersona associato

            private string m_Tipo; // Tipo del contatto ("e-mail", "telefono", "fax", ecc...)
            private string m_Nome; // Nome dell'indirizzo
            private string m_Valore; // Valore 
            private bool? m_Validated;
            private int m_ValidatoDaID;
            [NonSerialized] private Sistema.CUser m_ValidatoDa;
            private DateTime? m_ValidatoIl;
            private int m_Ordine;
            private string m_Commento;
            private CCollection<CIntervalloData> m_Intervalli;
            [NonSerialized] private object m_BlackList;
            private string m_TipoFonte;
            private int m_IDFonte;
            private IFonte m_Fonte;
            private StatoRecapito m_StatoRecapito;
            private int m_IDUfficio;
            [NonSerialized] private CUfficio m_Ufficio;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CContatto()
            {
                this.m_PersonaID = 0;
                this.m_Persona = null;
                this.m_Tipo = "";
                this.m_Nome = "";
                this.m_Valore = "";
                this.m_Validated = default;
                this.m_ValidatoDaID = 0;
                this.m_ValidatoDa = null;
                this.m_ValidatoIl = default;
                this.m_Ordine = 0;
                this.m_Flags = (int)ContattoFlags.NONE;
                this.m_Commento = "";
                this.m_Intervalli = new CCollection<CIntervalloData>();
                this.m_StatoRecapito = StatoRecapito.Sconosciuto;
                this.m_IDUfficio = 0;
                this.m_Ufficio = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="valore"></param>
            public CContatto(string tipo, string valore) : this()
            {
                m_Tipo = DMD.Strings.Trim(tipo);
                m_Valore = ParseValore(valore);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="tipo"></param>
            /// <param name="nome"></param>
            public CContatto(CPersona persona, string tipo, string nome) : this()
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                SetPersona(persona);
                m_Tipo = DMD.Strings.Trim(tipo);
                m_Nome = DMD.Strings.Trim(nome);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'ufficio
            /// </summary>
            public int IDUfficio
            {
                get
                {
                    return DBUtils.GetID(m_Ufficio, m_IDUfficio);
                }

                set
                {
                    int oldValue = IDUfficio;
                    if (oldValue == value)
                        return;
                    m_IDUfficio = value;
                    DoChanged("IDUfficio", value, oldValue);
                }
            }

          

            /// <summary>
            /// Restituisce o impostal 'ufficio
            /// </summary>
            public CUfficio Ufficio
            {
                get
                {
                    return m_Ufficio;
                }

                set
                {
                    var oldValue = m_Ufficio;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDUfficio = DBUtils.GetID(value, m_IDUfficio);
                    m_Ufficio = value;
                    DoChanged("Ufficio", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'ufficio
            /// </summary>
            /// <param name="value"></param>
            internal void SetUfficio(CUfficio value)
            {
                m_Ufficio = value;
                m_IDUfficio = DBUtils.GetID(value, m_IDUfficio);
            }

            /// <summary>
            /// Restituisce o imposta lo stato del recapito
            /// </summary>
            public StatoRecapito StatoRecapito
            {
                get
                {
                    return m_StatoRecapito;
                }

                set
                {
                    var oldValue = m_StatoRecapito;
                    if (oldValue == value)
                        return;
                    m_StatoRecapito = value;
                    DoChanged("StatoRecapito", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il contatto é il predefinito per il tipo
            /// </summary>
            public bool Predefinito
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, (int)ContattoFlags.PREDEFINITO);
                }

                set
                {
                    if (value == Predefinito)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, (int)ContattoFlags.PREDEFINITO, value);
                    DoChanged("Predefinito", value, !value);
                }
            }

            /// <summary>
            /// Restituisce il repository dei contatti
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Contatti;
            }

            /// <summary>
            /// Restituisce l'intervallo degli orari in cui il cliente è disponibile su questo recapito
            /// </summary>
            public CCollection<CIntervalloData> Intervalli
            {
                get
                {
                    return m_Intervalli;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha validato il contatto
            /// </summary>
            public Sistema.CUser ValidatoDa
            {
                get
                {
                    if (m_ValidatoDa is null)
                        m_ValidatoDa = Sistema.Users.GetItemById(m_ValidatoDaID);
                    return m_ValidatoDa;
                }

                set
                {
                    var oldValue = m_ValidatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ValidatoDa = value;
                    m_ValidatoDaID = DBUtils.GetID(value, m_ValidatoDaID);
                    DoChanged("ValidatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha validato il contatto
            /// </summary>
            public int ValidatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_ValidatoDa, m_ValidatoDaID);
                }

                set
                {
                    int oldValue = ValidatoDaID;
                    if (oldValue == value)
                        return;
                    m_ValidatoDa = null;
                    m_ValidatoDaID = value;
                    DoChanged("ValidatoDaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui é stato validato il contatto
            /// </summary>
            public DateTime? ValidatoIl
            {
                get
                {
                    return m_ValidatoIl;
                }

                set
                {
                    var oldValue = m_ValidatoIl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_ValidatoIl = value;
                    DoChanged("ValidatoIl", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta i flags del contatto
            /// </summary>
            public new ContattoFlags Flags
            {
                get
                {
                    return (ContattoFlags)m_Flags;
                }

                set
                {
                    var oldValue = (ContattoFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Unisce le informazioni del contatto corrente con quelle contenuto nell'argomento
            /// </summary>
            /// <param name="contatto"></param>
            public void MergeWith(CContatto contatto)
            {
                if (contatto is null)
                    throw new ArgumentNullException("contatto");

                if (PersonaID == 0)
                {
                    m_PersonaID = contatto.m_PersonaID;
                    m_Persona = contatto.m_Persona;
                }

                if (string.IsNullOrEmpty(m_Tipo))
                    m_Tipo = contatto.m_Tipo;
                if (string.IsNullOrEmpty(m_Nome))
                    m_Nome = contatto.m_Nome;
                if (string.IsNullOrEmpty(m_Valore))
                    m_Valore = contatto.m_Valore;
                m_Commento = DMD.Strings.Combine(m_Commento, contatto.m_Commento, " ");
                if (m_Validated.HasValue == false)
                {
                    m_Validated = contatto.m_Validated;
                    m_ValidatoDa = contatto.m_ValidatoDa;
                    m_ValidatoDaID = contatto.m_ValidatoDaID;
                    m_ValidatoIl = contatto.m_ValidatoIl;
                }

                m_Flags = m_Flags | contatto.m_Flags;
                m_Ordine = Maths.Min(m_Ordine, contatto.m_Ordine);
                if (Fonte is null)
                {
                    m_TipoFonte = contatto.m_TipoFonte;
                    m_IDFonte = contatto.m_IDFonte;
                    m_Fonte = contatto.m_Fonte;
                }

                if (m_StatoRecapito == StatoRecapito.Sconosciuto)
                    m_StatoRecapito = contatto.m_StatoRecapito;
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona a cui appartiene il contatto
            /// </summary>
            public int PersonaID
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_PersonaID);
                }

                set
                {
                    int oldValue = PersonaID;
                    if (oldValue == value)
                        return;
                    m_Persona = null;
                    m_PersonaID = value;
                    DoChanged("PersonaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto Persona a cui è associato l'indirizzo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Persone.GetItemById(m_PersonaID);
                    return m_Persona;
                }
                // Set(value As CPersona)
                // Dim oldValue As CPersona = Me.m_Persona
                // If (oldValue Is value) Then Exit Property
                // Me.SetPersona(value)
                // Me.DoChanged("Persona", value, oldValue)
                // End Set
            }

            /// <summary>
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(CPersona value)
            {
                m_Persona = value;
                m_PersonaID = DBUtils.GetID(value, this.m_PersonaID);
            }

            /// <summary>
            /// Restituisce o imposta il nome del contatto
            /// </summary>
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
            /// Restituisce o imposta il tipo del contatto
            /// </summary>
            public string Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Tipo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del contatto
            /// </summary>
            public string Valore
            {
                get
                {
                    return m_Valore;
                }

                set
                {
                    value = ParseValore(value);
                    string oldValue = m_Valore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Valore = value;
                    DoChanged("Valore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una nota sul contatto
            /// </summary>
            public string Commento
            {
                get
                {
                    return m_Commento;
                }

                set
                {
                    string oldValue = m_Commento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Commento = value;
                    DoChanged("Commento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ordine di visualizzazione crescente del contatto
            /// </summary>
            public int Ordine
            {
                get
                {
                    return m_Ordine;
                }

                set
                {
                    int oldValue = m_Ordine;
                    if (oldValue == value)
                        return;
                    m_Ordine = value;
                    DoChanged("Ordine", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'ordine
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetOrdine(int value)
            {
                m_Ordine = value;
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il contatto è stato validato
            /// </summary>
            public bool? Validated
            {
                get
                {
                    return m_Validated;
                }

                set
                {
                    if (m_Validated == value == true)
                        return;
                    m_Validated = value;
                    DoChanged("Validated", value, !value);
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
                    m_Fonte = null;
                    DoChanged("TipoFonte", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della fonte da cui proviene il contatto
            /// </summary>
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
            /// Restituisce la fonte da cui proviene il contatto
            /// </summary>
            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null)
                        m_Fonte = Fonti.GetItemById(m_TipoFonte, m_Tipo, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    var oldValue = Fonte;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID(value, this.m_IDFonte);
                    // Me.m_TipoFonte = value.
                    DoChanged("Fonte", value, oldValue);
                }
            }

            /// <summary>
            /// Sanifica il valore del contatto
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            protected virtual string ParseValore(string value)
            {
                switch (DMD.Strings.LCase(DMD.Strings.Trim(m_Tipo)) ?? "")
                {
                    case "cellulare":
                    case "telefono":
                    case "fax":
                        {
                            return Sistema.Formats.ParsePhoneNumber(value);
                        }

                    case "e-mail":
                    case "pec":
                        {
                            return Sistema.Formats.ParseEMailAddress(value);
                        }

                    case "website":
                        {
                            return Sistema.Formats.ParseWebAddress(value);
                        }

                    default:
                        {
                            return value;
                        }
                }
            }

            /// <summary>
            /// Restituisce una stringa che contiene gli intervalli data
            /// </summary>
            /// <returns></returns>
            private string GetIntervalliAsStr()
            {
                var ret = new System.Text.StringBuilder();
                foreach (CIntervalloData @int in m_Intervalli)
                {
                    if (@int.Inizio.HasValue || @int.Fine.HasValue)
                    {
                        if (ret.Length > 0)
                            ret.Append(",");
                        ret.Append(DMD.XML.Utils.Serializer.SerializeDate(@int.Inizio));
                        ret.Append("|");
                        ret.Append(DMD.XML.Utils.Serializer.SerializeDate(@int.Fine));
                    }
                }

                return ret.ToString();
            }

            /// <summary>
            /// Interpreta la stringa degli intervalli data
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            private CCollection<CIntervalloData> SplitIntervalliAsStr(string str)
            {
                var ret = new CCollection<CIntervalloData>();
                str = DMD.Strings.Trim(str);
                var items = DMD.Strings.Split(str, ",");
                for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                {
                    string item = DMD.Strings.Trim(items[i]);
                    if (!string.IsNullOrEmpty(item))
                    {
                        var @int = new CIntervalloData();
                        var n = DMD.Strings.Split(item, "|");
                        @int.Inizio = DMD.XML.Utils.Serializer.DeserializeDate(n[0]);
                        @int.Fine = DMD.XML.Utils.Serializer.DeserializeDate(n[1]);
                        if (@int.Inizio.HasValue || @int.Fine.HasValue)
                            ret.Add(@int);
                    }
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Persona", PersonaID);
                writer.WriteAttribute("Tipo", m_Tipo);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Valore", m_Valore);
                writer.WriteAttribute("Validated", m_Validated);
                writer.WriteAttribute("ValidatoDa", ValidatoDaID);
                writer.WriteAttribute("ValidatoIl", m_ValidatoIl);
                writer.WriteAttribute("Ordine", m_Ordine);
                writer.WriteAttribute("Intervalli", GetIntervalliAsStr());
                writer.WriteAttribute("TipoFonte", m_TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("StatoRecapito", (int?)m_StatoRecapito);
                writer.WriteAttribute("IDUfficio", IDUfficio);
                base.XMLSerialize(writer);
                writer.WriteTag("Commento", m_Commento);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Persona":
                        {
                            m_PersonaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            m_Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Validated":
                        {
                            m_Validated = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue);
                            break;
                        }

                    case "ValidatoDa":
                        {
                            m_ValidatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValidatoIl":
                        {
                            m_ValidatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Ordine":
                        {
                            m_Ordine = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    
                    case "Commento":
                        {
                            m_Commento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Intervalli":
                        {
                            m_Intervalli = SplitIntervalliAsStr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
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

                    case "StatoRecapito":
                        {
                            m_StatoRecapito = (StatoRecapito)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUfficio":
                        {
                            m_IDUfficio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Restituisce il nome della tabella di serializzazione
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Contatti";
            }

            /// <summary>
            /// Carica dal database
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_PersonaID = reader.Read("Persona", m_PersonaID);
                this.m_Tipo = reader.Read("Tipo", m_Tipo);
                this.m_Nome = reader.Read("Nome",  m_Nome);
                this.m_Valore = reader.Read("Valore",  m_Valore);
                this.m_Validated = reader.Read("Validated",  m_Validated);
                this.m_ValidatoDaID = reader.Read("ValidatoDa",  m_ValidatoDaID);
                this.m_ValidatoIl = reader.Read("ValidatoIl",  m_ValidatoIl);
                this.m_Ordine = reader.Read("Ordine",  m_Ordine);
                this.m_Commento = reader.Read("Commento",  m_Commento);
                this.m_TipoFonte = reader.Read("TipoFonte",  m_TipoFonte);
                this.m_IDFonte = reader.Read("IDFonte",  m_IDFonte);
                this.m_StatoRecapito = reader.Read("StatoRecapito",  m_StatoRecapito);
                this.m_IDUfficio = reader.Read("IDUfficio",  m_IDUfficio);
                string str = reader.Read("Intervalli", "");
                this.m_Intervalli = SplitIntervalliAsStr(str);
                
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel database
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Persona", PersonaID);
                writer.Write("Tipo", m_Tipo);
                writer.Write("Nome", m_Nome);
                writer.Write("Valore", m_Valore);
                writer.Write("Validated", m_Validated);
                writer.Write("ValidatoDa", ValidatoDaID);
                writer.Write("ValidatoIl", m_ValidatoIl);
                writer.Write("Ordine", m_Ordine);
                writer.Write("Commento", m_Commento);
                writer.Write("Intervalli", GetIntervalliAsStr());
                writer.Write("TipoFonte", m_TipoFonte);
                writer.Write("IDFonte", IDFonte);
                writer.Write("StatoRecapito", m_StatoRecapito);
                writer.Write("IDUfficio", IDUfficio);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Persona", typeof(int), 1);
                c = table.Fields.Ensure("Tipo", typeof(string), 255);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Valore", typeof(string), 255);
                c = table.Fields.Ensure("Validated", typeof(bool), 1);
                c = table.Fields.Ensure("ValidatoDa", typeof(int), 1);
                c = table.Fields.Ensure("ValidatoIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("Ordine", typeof(int), 1);
                c = table.Fields.Ensure("Commento", typeof(string), 0);
                c = table.Fields.Ensure("Intervalli", typeof(string), 0);
                c = table.Fields.Ensure("TipoFonte", typeof(string), 255);
                c = table.Fields.Ensure("IDFonte", typeof(int), 1);
                c = table.Fields.Ensure("StatoRecapito", typeof(int), 1);
                c = table.Fields.Ensure("IDUfficio", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPersona", new string[] { "Persona", "IDUfficio", "Ordine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContatto", new string[] { "Tipo", "Valore", "StatoRecapito" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValidato", new string[] { "ValidatoDa", "ValidatoIl", "Validated" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFonte", new string[] { "TipoFonte", "IDFonte" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Commento", typeof(string), 0);
                //c = table.Fields.Ensure("Intervalli", typeof(string), 0);
            }

            ///// <summary>
            ///// Restituisce la connessione
            ///// </summary>
            ///// <returns></returns>
            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Restituisce una stringa che rappresenta il contatto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray("{ ", this.m_Nome, " = ", this.m_Valore, " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome, this.m_Valore, this.m_Tipo, this.m_Validated);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CContatto) && this.Equals((CContatto)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CContatto obj)
            {
                return Integers.EQ(this.PersonaID, obj.PersonaID)
                        && Strings.EQ(this.m_Tipo, obj.m_Tipo)
                        && Strings.EQ(this.m_Nome, obj.m_Nome)
                        && Strings.EQ(this.m_Valore, obj.m_Valore)
                        && Booleans.EQ(this.m_Validated, obj.m_Validated)
                        && Integers.EQ(this.ValidatoDaID, obj.ValidatoDaID)
                        && DateUtils.EQ(this.m_ValidatoIl, obj.m_ValidatoIl)
                        && Integers.EQ(this.m_Ordine, obj.m_Ordine)
                        && Strings.EQ(this.m_Commento, obj.m_Commento)
                        && Strings.EQ(this.m_TipoFonte, obj.m_TipoFonte)
                        && Integers.EQ(this.IDFonte, obj.IDFonte)
                        && Integers.EQ((int)this.m_StatoRecapito, (int)obj.m_StatoRecapito)
                        && Integers.EQ(this.IDUfficio, obj.IDUfficio)
                        ;
            //private CCollection<CIntervalloData> m_Intervalli;
            //private object m_BlackList;
            }


            /// <summary>
            /// Compara i due contatti 
            /// </summary>
            /// <param name="a"></param>
            /// <returns></returns>
            public int CompareTo(CContatto a)
            {
                int a1 = 1;
                int a2 = 1;
                int ret;
                a1 = (m_StatoRecapito == StatoRecapito.Sconosciuto)? 1000 : (int)m_StatoRecapito;
                a2 = (a.m_StatoRecapito == StatoRecapito.Sconosciuto)? 1000 : (int)a.m_StatoRecapito;
                ret = DMD.Arrays.Compare(a1, a2);
                if (ret == 0)
                    ret = m_Ordine.CompareTo(a.m_Ordine);
                if (Validated.HasValue && Validated.Value)
                    a1 = 0;
                if (a.Validated.HasValue && a.Validated.Value)
                    a2 = 0;
                if (ret == 0)
                    ret = a1.CompareTo(a2);
                if (ret == 0)
                {
                    a1 = (!string.IsNullOrEmpty(m_Valore))? 0 : 1;
                    a2 = (!string.IsNullOrEmpty(a.m_Valore))? 0 : 1;
                    ret = a1.CompareTo(a2);
                }

                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Nome, a.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CContatto)obj);
            }

            /// <summary>
            /// Restituisce il valore formattato del contatto
            /// </summary>
            /// <returns></returns>
            public string GetValoreFormattato()
            {
                switch (DMD.Strings.LCase(Tipo) ?? "")
                {
                    case "telefono":
                    case "cellulare":
                    case "fax":
                        {
                            return Sistema.Formats.FormatPhoneNumber(m_Valore);
                        }

                    case "e-mail":
                    case "pec":
                        {
                            return Sistema.Formats.FormatEMailAddress(m_Valore);
                        }

                    case "website":
                        {
                            return Sistema.Formats.FormatWebAddress(m_Valore);
                        }

                    default:
                        {
                            return m_Valore;
                        }
                }
            }

            /// <summary>
            /// Clona il contatto
            /// </summary>
            /// <returns></returns>
            public CContatto Clone()
            {
                return (CContatto) this.MemberwiseClone();
            }

            object ICloneable.Clone() { return this.Clone();  }


        }
    }
}