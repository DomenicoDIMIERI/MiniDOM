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
        /// Tipo richiesta
        /// </summary>
        public enum TipoRichiestaPermessoFerie : int
        {
            /// <summary>
            /// Ore non retribuite
            /// </summary>
            OreNonRetribuite = 0,

            /// <summary>
            /// Ferie
            /// </summary>
            Ferie = 1,

            /// <summary>
            /// Malattia
            /// </summary>
            Malattia = 2
        }

        /// <summary>
        /// Esito della richiesta
        /// </summary>
        public enum EsitoRichiestaPermessoFerie : int
        {
            /// <summary>
            /// Da valutare
            /// </summary>
            DaValutare = 0,

            /// <summary>
            /// In valutazione
            /// </summary>
            InValutazione = 1,

            /// <summary>
            /// Richiesta di permesso accettata
            /// </summary>
            Accettato = 2,

            /// <summary>
            /// Richiesta di permesso rifiutata
            /// </summary>
            Rifiutato = 3,

            /// <summary>
            /// Richiesta di permesso annullata dal richiedente
            /// </summary>
            Annullato = 4
        }

        /// <summary>
        /// Rappresenta una richiesta di permesso
        /// </summary>
        [Serializable]
        public class RichiestaPermessoFerie
            : minidom.Databases.DBObjectPO, IComparable
        {
            private DateTime? m_DataRichiesta;                 // Data della richiesta
            private DateTime? m_DataInizio;                    // Data di inizio del permesso 
            private DateTime? m_DataFine;                      // Data di fine del permesso
            private string m_MotivoRichiesta;              // Motivo della richiesta   
            private string m_NoteRichiesta;
            private TipoRichiestaPermessoFerie m_TipoRichiesta;
            private int m_IDRichiedente;                // ID del richiedente
            private Sistema.CUser m_Richiedente;                    // Richiedente
            private string m_NomeRichiedente;               // Nome del richiedente
            private DateTime? m_DataPresaInCarico;
            private int m_IDInCaricoA;
            [NonSerialized] private Sistema.CUser m_InCaricoA;
            private string m_NomeInCaricoA;
            private EsitoRichiestaPermessoFerie m_EsitoRichiesta;
            private string m_DettaglioEsitoRichiesta;
            private string m_NotaPrvSupervisore;
            private CCollection<CAttachment> m_Attachments;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RichiestaPermessoFerie()
            {
                m_DataRichiesta = default;
                m_DataInizio = default;
                m_DataFine = default;
                m_MotivoRichiesta = "";
                m_NoteRichiesta = "";
                m_TipoRichiesta = TipoRichiestaPermessoFerie.OreNonRetribuite;
                m_IDRichiedente = 0;
                m_Richiedente = null;
                m_NomeRichiedente = "";
                m_DataPresaInCarico = default;
                m_IDInCaricoA = 0;
                m_InCaricoA = null;
                m_NomeInCaricoA = "";
                m_EsitoRichiesta = EsitoRichiestaPermessoFerie.DaValutare;
                m_DettaglioEsitoRichiesta = "";
                m_NotaPrvSupervisore = "";
                m_Attachments = null;
                
            }

            /// <summary>
            /// Restituisce o imposta la data in cui è stata inviata la richiesta di permesso
            /// </summary>
            /// <returns></returns>
            public DateTime? DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }

                set
                {
                    var oldValue = m_DataRichiesta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRichiesta = value;
                    DoChanged("DataRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di inizio del permesso richiesto
            /// </summary>
            /// <returns></returns>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di fine del permesso richiesto
            /// </summary>
            /// <returns></returns>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa "breve" che indica il motivo della richiesta
            /// </summary>
            /// <returns></returns>
            public string MotivoRichiesta
            {
                get
                {
                    return m_MotivoRichiesta;
                }

                set
                {
                    string oldValue = m_MotivoRichiesta;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_MotivoRichiesta = value;
                    DoChanged("MotivoRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle note aggiuntive per descrivere la richiesta
            /// </summary>
            /// <returns></returns>
            public string NoteRichiesta
            {
                get
                {
                    return m_NoteRichiesta;
                }

                set
                {
                    string oldValue = m_NoteRichiesta;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_NoteRichiesta = value;
                    DoChanged("NoteRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o importa la tipologia della richiesta di permesso
            /// </summary>
            /// <returns></returns>
            public TipoRichiestaPermessoFerie TipoRichiesta
            {
                get
                {
                    return m_TipoRichiesta;
                }

                set
                {
                    var oldValue = m_TipoRichiesta;
                    if (oldValue == value)
                        return;
                    m_TipoRichiesta = value;
                    DoChanged("TipoRichiesta", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID del richiedente il permesso
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta il richiedente il permesso
            /// </summary>
            /// <returns></returns>
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
                    var oldValue = Richiedente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Richiedente = value;
                    m_IDRichiedente = DBUtils.GetID(value, 0);
                    m_NomeRichiedente = "";
                    if (value is object)
                        m_NomeRichiedente = value.Nominativo;
                    DoChanged("Richiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del richiedente il permesso
            /// </summary>
            /// <returns></returns>
            public string NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }

                set
                {
                    string oldValue = m_NomeRichiedente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiedente = value;
                    DoChanged("NomeRichiedente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di presa in carico della richiesta di permesso
            /// </summary>
            /// <returns></returns>
            public DateTime? DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }

                set
                {
                    var oldValue = m_DataPresaInCarico;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataPresaInCarico = value;
                    DoChanged("DataPresaInCarico", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha preso in carico la richiesta
            /// </summary>
            /// <returns></returns>
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
                    m_IDInCaricoA = value;
                    m_InCaricoA = null;
                    DoChanged("IDInCaricoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha preso in carico la richiesta
            /// </summary>
            /// <returns></returns>
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
                    var oldValue = InCaricoA;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InCaricoA = value;
                    m_IDInCaricoA = DBUtils.GetID(value, 0);
                    m_NomeInCaricoA = "";
                    if (value is object)
                        m_NomeInCaricoA = value.Nominativo;
                    DoChanged("InCaricoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha preso in carico la richiesta
            /// </summary>
            /// <returns></returns>
            public string NomeInCaricoA
            {
                get
                {
                    return m_NomeInCaricoA;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeInCaricoA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeInCaricoA = value;
                    DoChanged("NomeInCaricoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica l'esito della richiesta
            /// </summary>
            /// <returns></returns>
            public EsitoRichiestaPermessoFerie EsitoRichiesta
            {
                get
                {
                    return m_EsitoRichiesta;
                }

                set
                {
                    var oldValue = m_EsitoRichiesta;
                    if (oldValue == value)
                        return;
                    m_EsitoRichiesta = value;
                    DoChanged("EsitoRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una nota lunga che descrive il motivo per l'esito della richiesta
            /// </summary>
            /// <returns></returns>
            public string DettaglioEsitoRichiesta
            {
                get
                {
                    return m_DettaglioEsitoRichiesta;
                }

                set
                {
                    string oldValue = m_DettaglioEsitoRichiesta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioEsitoRichiesta = value;
                    DoChanged("DettaglioEsitoRichiesta", value, oldValue);
                }
            }

            /// <summary>
            /// Nota privata per il supervisore
            /// </summary>
            /// <returns></returns>
            public string NotaPrvSupervisore
            {
                get
                {
                    return m_NotaPrvSupervisore;
                }

                set
                {
                    string oldValue = m_NotaPrvSupervisore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NotaPrvSupervisore = value;
                    DoChanged("NotaPrvSupervisore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione dei documenti allegati alla richiesta
            /// </summary>
            /// <returns></returns>
            public CCollection<Sistema.CAttachment> Attachments
            {
                get
                {
                    if (m_Attachments is null)
                        m_Attachments = new CCollection<Sistema.CAttachment>();
                    return m_Attachments;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.RichiestePermessiFerie;
            }

            /// <summary>
        /// Restituisce o imposta dei parametri aggiuntivi
        /// </summary>
        /// <returns></returns>
            public CKeyCollection Params
            {
                get
                {
                    if (m_Params is null)
                        m_Params = new CKeyCollection();
                    return m_Params;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override Sistema.CModule GetModule()
            {
                return RichiestePermessiFerie.Module;
            }

            public override string GetTableName()
            {
                return "tbl_OfficeRichiestePermF";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_DataRichiesta = reader.Read("DataRichiesta", this.m_DataRichiesta);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                this.m_MotivoRichiesta = reader.Read("MotivoRichiesta", this.m_MotivoRichiesta);
                this.m_NoteRichiesta = reader.Read("NoteRichiesta", this.m_NoteRichiesta);
                this.m_TipoRichiesta = reader.Read("TipoRichiesta", this.m_TipoRichiesta);
                this.m_IDRichiedente = reader.Read("IDRichiedente", this.m_IDRichiedente);
                this.m_NomeRichiedente = reader.Read("NomeRichiedente", this.m_NomeRichiedente);
                this.m_DataPresaInCarico = reader.Read("DataPresaInCarico", this.m_DataPresaInCarico);
                this.m_IDInCaricoA = reader.Read("IDInCaricoA", this.m_IDInCaricoA);
                this.m_NomeInCaricoA = reader.Read("NomeInCaricoA", this.m_NomeInCaricoA);
                this.m_EsitoRichiesta = reader.Read("EsitoRichiesta", this.m_EsitoRichiesta);
                this.m_DettaglioEsitoRichiesta = reader.Read("DettaglioEsitoRichiesta", this.m_DettaglioEsitoRichiesta);
                this.m_NotaPrvSupervisore = reader.Read("NotaPrvSupervisore", this.m_NotaPrvSupervisore);
                 
                string tmp = reader.Read("Attachments", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Attachments = new CCollection<Sistema.CAttachment>();
                    m_Attachments.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp));
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
                writer.Write("DataRichiesta", m_DataRichiesta);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("MotivoRichiesta", m_MotivoRichiesta);
                writer.Write("NoteRichiesta", m_NoteRichiesta);
                writer.Write("TipoRichiesta", m_TipoRichiesta);
                writer.Write("IDRichiedente", IDRichiedente);
                writer.Write("NomeRichiedente", m_NomeRichiedente);
                writer.Write("DataPresaInCarico", m_DataPresaInCarico);
                writer.Write("IDInCaricoA", IDInCaricoA);
                writer.Write("NomeInCaricoA", m_NomeInCaricoA);
                writer.Write("EsitoRichiesta", m_EsitoRichiesta);
                writer.Write("DettaglioEsitoRichiesta", m_DettaglioEsitoRichiesta);
                writer.Write("NotaPrvSupervisore", m_NotaPrvSupervisore);
                writer.Write("Attachments", DMD.XML.Utils.Serializer.Serialize(Attachments));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataRichiesta", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("MotivoRichiesta", typeof(string), 255);
                c = table.Fields.Ensure("NoteRichiesta", typeof(string), 0);
                c = table.Fields.Ensure("TipoRichiesta", typeof(int), 1);
                c = table.Fields.Ensure("IDRichiedente", typeof(int), 1);
                c = table.Fields.Ensure("NomeRichiedente", typeof(string), 255);
                c = table.Fields.Ensure("DataPresaInCarico", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDInCaricoA", typeof(int), 1);
                c = table.Fields.Ensure("NomeInCaricoA", typeof(string), 255);
                c = table.Fields.Ensure("EsitoRichiesta", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioEsitoRichiesta", typeof(string), 255);
                c = table.Fields.Ensure("NotaPrvSupervisore", typeof(string), 0);
                c = table.Fields.Ensure("Attachments", typeof(string), 0);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxRichiedente", new string[] { "IDRichiedente", "DataRichiesta", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxInCarico", new string[] { "IDInCaricoA", "NomeInCaricoA", "DataPresaInCarico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine" , "DataPresaInCarico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMotivo", new string[] { "MotivoRichiesta", "NoteRichiesta", "TipoRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParametri", new string[] { "NomeRichiedente", "EsitoRichiesta", "DettaglioEsitoRichiesta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNote", new string[] { "NotaPrvSupervisore" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Attachments", typeof(string), 0);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataRichiesta", m_DataRichiesta);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("MotivoRichiesta", m_MotivoRichiesta);
                writer.WriteAttribute("TipoRichiesta", (int?)m_TipoRichiesta);
                writer.WriteAttribute("IDRichiedente", IDRichiedente);
                writer.WriteAttribute("NomeRichiedente", m_NomeRichiedente);
                writer.WriteAttribute("DataPresaInCarico", m_DataPresaInCarico);
                writer.WriteAttribute("IDInCaricoA", IDInCaricoA);
                writer.WriteAttribute("NomeInCaricoA", m_NomeInCaricoA);
                writer.WriteAttribute("EsitoRichiesta", (int?)m_EsitoRichiesta);
                base.XMLSerialize(writer);
                writer.WriteTag("Attachments", Attachments);
                writer.WriteTag("NoteRichiesta", m_NoteRichiesta);
                writer.WriteTag("NotaPrvSupervisore", m_NotaPrvSupervisore);
                writer.WriteTag("DettaglioEsitoRichiesta", m_DettaglioEsitoRichiesta);
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
                    case "DataRichiesta":
                        {
                            m_DataRichiesta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MotivoRichiesta":
                        {
                            m_MotivoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoRichiesta":
                        {
                            m_TipoRichiesta = (TipoRichiestaPermessoFerie)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

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

                    case "DataPresaInCarico":
                        {
                            m_DataPresaInCarico = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDInCaricoA":
                        {
                            m_IDInCaricoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeInCaricoA":
                        {
                            m_NomeInCaricoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EsitoRichiesta":
                        {
                            m_EsitoRichiesta = (EsitoRichiestaPermessoFerie)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                   
                    case "NoteRichiesta":
                        {
                            m_NoteRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioEsitoRichiesta":
                        {
                            m_DettaglioEsitoRichiesta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NotaPrvSupervisore":
                        {
                            m_NotaPrvSupervisore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 
                    case "Attachments":
                        {
                            m_Attachments = new CCollection<Sistema.CAttachment>();
                            m_Attachments.AddRange((CCollection)fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(RichiestaPermessoFerie obj)
            {
                int ret = DMD.DateUtils.Compare(m_DataRichiesta, obj.m_DataRichiesta);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataInizio, obj.m_DataInizio);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataFine, obj.m_DataFine);
                if (ret == 0)
                    ret = DMD.Strings.Compare(NomeRichiedente, obj.NomeRichiedente, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((RichiestaPermessoFerie)obj);
            }

            /// <summary>
            /// Richiede
            /// </summary>
            public void Richiedi()
            {
                if (EsitoRichiesta != EsitoRichiestaPermessoFerie.DaValutare)
                    throw new InvalidOperationException("Richiesta già inviata");
                EsitoRichiesta = EsitoRichiestaPermessoFerie.InValutazione;
                DataRichiesta = DMD.DateUtils.Now();
                Save();
            }

            /// <summary>
            /// Annulla
            /// </summary>
            /// <param name="dettaglioEsito"></param>
            public void Annulla(string dettaglioEsito)
            {
                switch (EsitoRichiesta)
                {
                    case EsitoRichiestaPermessoFerie.Rifiutato:
                    case EsitoRichiestaPermessoFerie.Annullato:
                        {
                            throw new InvalidOperationException("Richiesta non più annullabile");
                             
                        }

                    case EsitoRichiestaPermessoFerie.Accettato:
                        {
                            if (DBUtils.GetID(Sistema.Users.CurrentUser, 0) == IDRichiedente)
                            {
                                if (DMD.DateUtils.Compare(DMD.DateUtils.Now(), DataFine) >= 0)
                                {
                                    throw new InvalidOperationException("Richiesta non più annullabile");
                                }
                            }
                            else if (GetModule().UserCanDoAction("lavorare"))
                            {
                            }
                            else
                            {
                                throw new InvalidOperationException("Richiesta non più annullabile");
                            }

                            break;
                        }
                }

                EsitoRichiesta = EsitoRichiestaPermessoFerie.Annullato;
                DettaglioEsitoRichiesta = dettaglioEsito;
                DataPresaInCarico = DMD.DateUtils.Now();
                InCaricoA = Sistema.Users.CurrentUser;
                Save();
            }

            /// <summary>
            /// Accetta
            /// </summary>
            /// <param name="dettaglioEsito"></param>
            public void Accetta(string dettaglioEsito)
            {
                switch (EsitoRichiesta)
                {
                    case EsitoRichiestaPermessoFerie.Accettato:
                    case EsitoRichiestaPermessoFerie.Rifiutato:
                    case EsitoRichiestaPermessoFerie.Annullato:
                        {
                            throw new InvalidOperationException("Richiesta non più lavorabile");
                             
                        }
                }

                EsitoRichiesta = EsitoRichiestaPermessoFerie.Accettato;
                DettaglioEsitoRichiesta = dettaglioEsito;
                InCaricoA = Sistema.Users.CurrentUser;
                DataPresaInCarico = DMD.DateUtils.Now();
                Save();
            }

            /// <summary>
            /// Rifiuta
            /// </summary>
            /// <param name="dettaglioEsito"></param>
            public void Rifiuta(string dettaglioEsito)
            {
                switch (EsitoRichiesta)
                {
                    case EsitoRichiestaPermessoFerie.Accettato:
                    case EsitoRichiestaPermessoFerie.Rifiutato:
                    case EsitoRichiestaPermessoFerie.Annullato:
                        {
                            throw new InvalidOperationException("Richiesta non più lavorabile");
                            
                        }
                }

                this.EsitoRichiesta = EsitoRichiestaPermessoFerie.Rifiutato;
                this.DettaglioEsitoRichiesta = dettaglioEsito;
                this.InCaricoA = Sistema.Users.CurrentUser;
                this.DataPresaInCarico = DMD.DateUtils.Now();
                this.Save();
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_DataRichiesta , " - ", this.NomeRichiedente);
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
                return (obj is RichiestaPermessoFerie) && this.Equals((RichiestaPermessoFerie)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RichiestaPermessoFerie obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataRichiesta, obj.m_DataRichiesta)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Strings.EQ(this.m_MotivoRichiesta, obj.m_MotivoRichiesta)
                    && DMD.Strings.EQ(this.m_NoteRichiesta, obj.m_NoteRichiesta)
                    && DMD.Integers.EQ((int)this.m_TipoRichiesta, (int)obj.m_TipoRichiesta)
                    && DMD.Integers.EQ(this.m_IDRichiedente, obj.m_IDRichiedente)
                    && DMD.Strings.EQ(this.m_NomeRichiedente, obj.m_NomeRichiedente)
                    && DMD.DateUtils.EQ(this.m_DataPresaInCarico, obj.m_DataPresaInCarico)
                    && DMD.Integers.EQ(this.m_IDInCaricoA, obj.m_IDInCaricoA)
                    && DMD.Strings.EQ(this.m_NomeInCaricoA, obj.m_NomeInCaricoA)
                    && DMD.Integers.EQ((int)this.m_EsitoRichiesta, (int)obj.m_EsitoRichiesta)
                    && DMD.Strings.EQ(this.m_DettaglioEsitoRichiesta, obj.m_DettaglioEsitoRichiesta)
                    && DMD.Strings.EQ(this.m_NotaPrvSupervisore, obj.m_NotaPrvSupervisore)
                    ;

            }
        }
    }
}