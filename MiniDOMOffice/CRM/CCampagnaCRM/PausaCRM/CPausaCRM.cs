using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {
        /// <summary>
        /// Tipo di pausa
        /// TODO ristrutturare il sistema dei flag
        /// </summary>
        public enum StatoPausaCRM : int
        {
            /// <summary>
            /// Non chiesta
            /// </summary>
            NonChiesta = 0,

            /// <summary>
            /// Pausa chiesta ed in attesa di essere presa in carico
            /// </summary>
            Chiesta = 1,

            /// <summary>
            /// Pausa in attesa di autorizzazione
            /// </summary>
            InValutazione = 2,

            /// <summary>
            /// Pausa approvata
            /// </summary>
            Approvata = 3,

            /// <summary>
            /// Pausa negata
            /// </summary>
            Negata = 4,

            /// <summary>
            /// Pausa iniziata
            /// </summary>
            Iniziata = 5,

            /// <summary>
            /// Pausa terminata
            /// </summary>
            Terminata = 6,

            /// <summary>
            /// Pausa annullata prima della richiesta di valutazione
            /// </summary>
            AnnullataPrimaValutazione = 7,

            /// <summary>
            /// Pausa annullata dopo la richiesta
            /// </summary>
            AnnullataDopoValutazione = 8
        }


        /// <summary>
        /// Descrive una pausa di un operatore di crm
        /// </summary>
        [Serializable]
        public class CPausaCRM 
            : minidom.Databases.DBObjectPO
        {
            private int m_IDSessioneCRM;
            [NonSerialized] private CSessioneCRM m_SessioneCRM;
            private int m_IDUtente;
            [NonSerialized] private Sistema.CUser m_Utente;
            private string m_NomeUtente;
            private DateTime? m_OraRichiesta;
            private DateTime? m_OraInizioValutazione;
            private DateTime? m_OraFineValutazione;
            private DateTime? m_OraPrevista;
            private DateTime? m_Inizio;
            private DateTime? m_Fine;
            private string m_Motivo;
            private int? m_DurataPrevista;
            private string m_DettaglioMotivo;
            private int m_IDSupervisore;
            [NonSerialized] private Sistema.CUser m_Supervisore;
            private string m_NomeSupervisore;
            private string m_EsitoSupervisione;
            private string m_NoteAmministrative;
            private StatoPausaCRM m_StatoRichiesta;
            
            /// <summary>
            /// Costrutttore
            /// </summary>
            public CPausaCRM()
            {
                m_IDSessioneCRM = 0;
                m_SessioneCRM = null;
                m_IDUtente = 0;
                m_Utente = null;
                m_NomeUtente = "";
                m_Inizio = default;
                m_Fine = default;
                m_Motivo = "";
                m_DurataPrevista = default;
                m_DettaglioMotivo = "";
                m_IDSupervisore = 0;
                m_Supervisore = null;
                m_NomeSupervisore = "";
                m_EsitoSupervisione = "";
                m_NoteAmministrative = "";
                m_StatoRichiesta = StatoPausaCRM.NonChiesta;
                m_Flags = 0;
                m_OraRichiesta = default;
                m_OraInizioValutazione = default;
                m_OraFineValutazione = default;
                m_OraPrevista = default;
            }

            /// <summary>
            /// Restituisce o imposta l'id della sessione crm
            /// </summary>
            public int IDSessioneCRM
            {
                get
                {
                    return DBUtils.GetID(m_SessioneCRM, m_IDSessioneCRM);
                }

                set
                {
                    int oldValue = IDSessioneCRM;
                    if (oldValue == value)
                        return;
                    m_IDSessioneCRM = value;
                    m_SessioneCRM = null;
                    DoChanged("IDSessioneCRM", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sessione crm
            /// </summary>
            public CSessioneCRM SessioneCRM
            {
                get
                {
                    if (m_SessioneCRM is null)
                        m_SessioneCRM = minidom.CustomerCalls.SessioniCRM.GetItemById(m_IDSessioneCRM);
                    return m_SessioneCRM;
                }

                set
                {
                    var oldValue = SessioneCRM;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_SessioneCRM = value;
                    m_IDSessioneCRM = DBUtils.GetID(value, 0);
                    DoChanged("SessioneCRM", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la sessione crm
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetSessioneCRM(CSessioneCRM value)
            {
                m_SessioneCRM = value;
                m_IDSessioneCRM = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'utente
            /// </summary>
            public int IDUtente
            {
                get
                {
                    return DBUtils.GetID(m_Utente, m_IDUtente);
                }

                set
                {
                    int oldValue = IDUtente;
                    if (oldValue == value)
                        return;
                    m_IDUtente = value;
                    m_Utente = null;
                    DoChanged("IDUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente 
            /// </summary>
            public Sistema.CUser Utente
            {
                get
                {
                    if (m_Utente is null)
                        m_Utente = Sistema.Users.GetItemById(m_IDUtente);
                    return m_Utente;
                }

                set
                {
                    var oldValue = Utente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Utente = value;
                    m_IDUtente = DBUtils.GetID(value, 0);
                    m_NomeUtente = (value is object)? value.Nominativo : string.Empty;
                    DoChanged("Utente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome utente
            /// </summary>
            public string NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }

                set
                {
                    string oldValue = m_NomeUtente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUtente = value;
                    DoChanged("NomeUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il nome utente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUtente(Sistema.CUser value)
            {
                m_Utente = value;
                m_IDUtente = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio della pausa
            /// </summary>
            public DateTime? Inizio
            {
                get
                {
                    return m_Inizio;
                }

                set
                {
                    var oldValue = m_Inizio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Inizio = value;
                    DoChanged("Inizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine della pausa
            /// </summary>
            public DateTime? Fine
            {
                get
                {
                    return m_Fine;
                }

                set
                {
                    var oldValue = m_Fine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Fine = value;
                    DoChanged("Fine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo della richiesta della pausa
            /// </summary>
            public string Motivo
            {
                get
                {
                    return m_Motivo;
                }

                set
                {
                    string oldValue = m_Motivo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Motivo = value;
                    DoChanged("Motivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la durata prevista in secondi
            /// </summary>
            public int? DurataPrevista
            {
                get
                {
                    return m_DurataPrevista;
                }

                set
                {
                    var oldValue = m_DurataPrevista;
                    if (oldValue == value == true)
                        return;
                    m_DurataPrevista = value;
                    DoChanged("DurataPrevista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il motivo dettagliato per la richiesta della pausa
            /// </summary>
            public string DettaglioMotivo
            {
                get
                {
                    return m_DettaglioMotivo;
                }

                set
                {
                    string oldValue = m_DettaglioMotivo;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_DettaglioMotivo = value;
                    DoChanged("DettaglioMotivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del supervisore
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
            /// Restituisce o imposta il supervisore
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
                    var oldValue = Supervisore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Supervisore = value;
                    m_IDSupervisore = DBUtils.GetID(value, 0);
                    m_NomeSupervisore = "";
                    if (value is object)
                        m_NomeSupervisore = value.Nominativo;
                    DoChanged("Supervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del supervisore
            /// </summary>
            public string NomeSupervisore
            {
                get
                {
                    return m_NomeSupervisore;
                }

                set
                {
                    string oldValue = m_NomeSupervisore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSupervisore = value;
                    DoChanged("NomeSupervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la stringa che descrive l'esito della supervizione
            /// </summary>
            public string EsitoSupervisione
            {
                get
                {
                    return m_EsitoSupervisione;
                }

                set
                {
                    string oldValue = m_EsitoSupervisione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EsitoSupervisione = value;
                    DoChanged("EsitoSupervisione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la note inserite dal supervisore e visibili solo ai supervisori
            /// </summary>
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

            /// <summary>
            /// Restituisce o imposta la data della richiesta
            /// </summary>
            public StatoPausaCRM StatoRichiesta
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
            /// Restituisce o imposta l'ora della richiesta
            /// </summary>
            public DateTime? OraRichiesta
            {
                get
                {
                    return m_OraRichiesta;
                }

                set
                {
                    var oldValue = m_OraRichiesta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraRichiesta = value;
                    DoChanged("OraRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ora di presa in carico da parte del supervisore
            /// </summary>
            public DateTime? OraInizioValutazione
            {
                get
                {
                    return m_OraInizioValutazione;
                }

                set
                {
                    var oldValue = m_OraInizioValutazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraInizioValutazione = value;
                    DoChanged("OraInizioValutazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ora di emissione dell'esito da parte del supervisore
            /// </summary>
            public DateTime? OraFineValutazione
            {
                get
                {
                    return m_OraFineValutazione;
                }

                set
                {
                    var oldValue = m_OraFineValutazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraFineValutazione = value;
                    DoChanged("OraFineValutazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ora di rientro prevista
            /// </summary>
            public DateTime? OraPrevista
            {
                get
                {
                    return m_OraPrevista;
                }

                set
                {
                    var oldValue = m_OraPrevista;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraPrevista = value;
                    DoChanged("OraPrevista", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.PauseCRM;
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PausaCRM";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDSessioneCRM = reader.Read("IDSessioneCRM", m_IDSessioneCRM);
                m_IDUtente = reader.Read("IDUtente", m_IDUtente);
                m_NomeUtente = reader.Read("NomeUtente", m_NomeUtente);
                m_Inizio = reader.Read("Inizio", m_Inizio);
                m_Fine = reader.Read("Fine", m_Fine);
                m_Motivo = reader.Read("Motivo", m_Motivo);
                m_DurataPrevista = reader.Read("DurataPrevista", m_DurataPrevista);
                m_DettaglioMotivo = reader.Read("DettaglioMotivo", m_DettaglioMotivo);
                m_IDSupervisore = reader.Read("IDSupervisore", m_IDSupervisore);
                m_NomeSupervisore = reader.Read("NomeSupervisore", m_NomeSupervisore);
                m_EsitoSupervisione = reader.Read("EsitoSupervisione", m_EsitoSupervisione);
                m_NoteAmministrative = reader.Read("NoteAmministrative", m_NoteAmministrative);
                m_StatoRichiesta = reader.Read("StatoRichiesta", m_StatoRichiesta);
                m_OraRichiesta = reader.Read("OraRichiesta", m_OraRichiesta);
                m_OraInizioValutazione = reader.Read("OraInizioValutazione", m_OraInizioValutazione);
                m_OraFineValutazione = reader.Read("OraFineValutazione", m_OraFineValutazione);
                m_OraPrevista = reader.Read("OraPrevista", m_OraPrevista);
                 
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDSessioneCRM", IDSessioneCRM);
                writer.Write("IDUtente", IDUtente);
                writer.Write("NomeUtente", m_NomeUtente);
                writer.Write("Inizio", m_Inizio);
                writer.Write("Fine", m_Fine);
                writer.Write("Motivo", m_Motivo);
                writer.Write("DurataPrevista", m_DurataPrevista);
                writer.Write("DettaglioMotivo", m_DettaglioMotivo);
                writer.Write("IDSupervisore", IDSupervisore);
                writer.Write("NomeSupervisore", m_NomeSupervisore);
                writer.Write("EsitoSupervisione", m_EsitoSupervisione);
                writer.Write("NoteAmministrative", m_NoteAmministrative);
                writer.Write("StatoRichiesta", m_StatoRichiesta);
                writer.Write("OraRichiesta", m_OraRichiesta);
                writer.Write("OraInizioValutazione", m_OraInizioValutazione);
                writer.Write("OraFineValutazione", m_OraFineValutazione);
                writer.Write("OraPrevista", m_OraPrevista);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDSessioneCRM", typeof(int), 1);
                c = table.Fields.Ensure("IDUtente", typeof(int), 1);
                c = table.Fields.Ensure("NomeUtente", typeof(string), 255);
                c = table.Fields.Ensure("Inizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("Fine", typeof(DateTime), 1);
                c = table.Fields.Ensure("Motivo", typeof(string), 255);
                c = table.Fields.Ensure("DurataPrevista", typeof(DateTime), 1);
                c = table.Fields.Ensure("DettaglioMotivo", typeof(string), 0);
                c = table.Fields.Ensure("IDSupervisore", typeof(int), 1);
                c = table.Fields.Ensure("NomeSupervisore", typeof(string), 255);
                c = table.Fields.Ensure("EsitoSupervisione", typeof(string), 255);
                c = table.Fields.Ensure("NoteAmministrative", typeof(string), 255);
                c = table.Fields.Ensure("StatoRichiesta", typeof(int), 1);
                c = table.Fields.Ensure("OraRichiesta", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraInizioValutazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraFineValutazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraPrevista", typeof(DateTime), 1);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxSessione", new string[] { "IDSessioneCRM" , "NomeUtente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUtente", new string[] { "IDUtente", "Inizio" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxUtente", new string[] { "Fine", "DurataPrevista", "OraPrevista" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMotivo", new string[] { "Motivo", "DettaglioMotivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSupervisore", new string[] { "IDSupervisore", "NomeSupervisore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEsito", new string[] { "EsitoSupervisione", "NoteAmministrative" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoRich", new string[] { "StatoRichiesta", "OraRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStatoVali", new string[] { "OraInizioValutazione", "OraFineValutazione" }, DBFieldConstraintFlags.None);

                 

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDSessioneCRM", IDSessioneCRM);
                writer.WriteAttribute("IDUtente", IDUtente);
                writer.WriteAttribute("NomeUtente", m_NomeUtente);
                writer.WriteAttribute("Inizio", m_Inizio);
                writer.WriteAttribute("Fine", m_Fine);
                writer.WriteAttribute("Motivo", m_Motivo);
                writer.WriteAttribute("DurataPrevista", m_DurataPrevista);
                writer.WriteAttribute("DettaglioMotivo", m_DettaglioMotivo);
                writer.WriteAttribute("IDSupervisore", IDSupervisore);
                writer.WriteAttribute("NomeSupervisore", m_NomeSupervisore);
                writer.WriteAttribute("EsitoSupervisione", m_EsitoSupervisione);
                writer.WriteAttribute("NoteAmministrative", m_NoteAmministrative);
                writer.WriteAttribute("StatoRichiesta", (int?)m_StatoRichiesta);
                writer.WriteAttribute("OraRichiesta", m_OraRichiesta);
                writer.WriteAttribute("OraInizioValutazione", m_OraInizioValutazione);
                writer.WriteAttribute("OraFineValutazione", m_OraFineValutazione);
                writer.WriteAttribute("OraPrevista", m_OraPrevista);
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
                    case "IDSessioneCRM":
                        {
                            m_IDSessioneCRM = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUtente":
                        {
                            m_IDUtente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUtente":
                        {
                            m_NomeUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Inizio":
                        {
                            m_Inizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Fine":
                        {
                            m_Fine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Motivo":
                        {
                            m_Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DurataPrevista":
                        {
                            m_DurataPrevista = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioMotivo":
                        {
                            m_DettaglioMotivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDSupervisore":
                        {
                            IDSupervisore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeSupervisore":
                        {
                            m_NomeSupervisore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EsitoSupervisione":
                        {
                            m_EsitoSupervisione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NoteAmministrative":
                        {
                            m_NoteAmministrative = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoRichiesta":
                        {
                            m_StatoRichiesta = (StatoPausaCRM)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
 

                    case "OraRichiesta":
                        {
                            m_OraRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraInizioValutazione":
                        {
                            m_OraInizioValutazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraFineValutazione":
                        {
                            m_OraFineValutazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraPrevista":
                        {
                            m_OraPrevista = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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
                return Strings.ConcatArray(this.m_NomeUtente, 
                                " [", Sistema.Formats.FormatUserDateTime(m_Inizio),
                                " - ", Sistema.Formats.FormatUserDateTime(m_Fine),
                                "]");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Inizio);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CPausaCRM) && this.Equals((CPausaCRM)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CPausaCRM obj)
            {
                return base.Equals(obj)
                        && DMD.Integers.EQ(this.m_IDSessioneCRM, obj.m_IDSessioneCRM)
                        && DMD.Integers.EQ(this.m_IDUtente, obj.m_IDUtente)
                        && DMD.Strings.EQ(this.m_NomeUtente, obj.m_NomeUtente)
                        && DMD.DateUtils.EQ(this.m_OraRichiesta, obj.m_OraRichiesta)
                        && DMD.DateUtils.EQ(this.m_OraInizioValutazione, obj.m_OraInizioValutazione)
                        && DMD.DateUtils.EQ(this.m_OraFineValutazione, obj.m_OraFineValutazione)
                        && DMD.DateUtils.EQ(this.m_OraPrevista, obj.m_OraPrevista)
                        && DMD.DateUtils.EQ(this.m_Inizio, obj.m_Inizio)
                        && DMD.DateUtils.EQ(this.m_Fine, obj.m_Fine)
                        && DMD.Strings.EQ(this.m_Motivo, obj.m_Motivo)
                        && DMD.Integers.EQ(this.m_DurataPrevista, obj.m_DurataPrevista)
                        && DMD.Strings.EQ(this.m_DettaglioMotivo, obj.m_DettaglioMotivo)
                        && DMD.Integers.EQ(this.m_IDSupervisore, obj.m_IDSupervisore)
                        && DMD.Strings.EQ(this.m_NomeSupervisore, obj.m_NomeSupervisore)
                        && DMD.Strings.EQ(this.m_EsitoSupervisione, obj.m_EsitoSupervisione)
                        && DMD.Strings.EQ(this.m_NoteAmministrative, obj.m_NoteAmministrative)
                        && DMD.Integers.EQ((int)this.m_StatoRichiesta, (int)obj.m_StatoRichiesta)
                        ;
            }

        }
    }
}