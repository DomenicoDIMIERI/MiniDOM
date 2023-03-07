using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom.repositories;
using static minidom.Sistema;


namespace minidom
{
    public partial class ADV
    {

        /// <summary>
        /// Valori che indicano lo stato di un messaggio inviato in una campagna pubblicitaria
        /// </summary>
        /// <remarks></remarks>
        public enum StatoMessaggioCampagna : int
        {
            /// <summary>
            /// Il messaggio non è ancora pronto per essere spedito
            /// </summary>
            /// <remarks></remarks>
            InPreparazione = 0,

            /// <summary>
            /// Il messaggio è pronto per essere spedito
            /// </summary>
            /// <remarks></remarks>
            ProntoPerLaSpedizione = 1,

            /// <summary>
            /// Il messaggio è stato inviato al destinatario
            /// </summary>
            /// <remarks></remarks>
            Inviato = 2,

            /// <summary>
            /// Il messaggio è stato rifiutato dal vettore di spedizione
            /// </summary>
            /// <remarks></remarks>
            RifiutatoDalVettore = 3,

            /// <summary>
            /// Il messaggio è stato rifiutato dal destinatario
            /// </summary>
            /// <remarks></remarks>
            RifiutatoDalDestinatario = 4,

            /// <summary>
            /// Il messaggio è stato confermato
            /// </summary>
            /// <remarks></remarks>
            Letto = 5
        }


        /// <summary>
        /// Risultato di una campagna
        /// </summary>
        [Serializable]
        public class CRisultatoCampagna 
            : minidom.Databases.DBObject
        {
            private int m_IDCampagna;
            [NonSerialized] private CCampagnaPubblicitaria m_Campagna;
            private string m_NomeCampagna;
            [NonSerialized] private Anagrafica.CPersona m_Destinatario;
            private int m_IDDestinatario;
            private string m_NomeDestinatario;
            private StatoMessaggioCampagna m_StatoMessaggio;
            private DateTime? m_DataSpedizione;
            private string m_NomeMezzoSpedizione;
            private string m_StatoSpedizione;
            private DateTime? m_DataConsegna;
            private DateTime? m_DataLettura;
            private TipoCampagnaPubblicitaria m_TipoCampagna;
            private string m_IndirizzoDestinatario;
            private DateTime? m_DataEsecuzuine;
            private string m_MessageID;                   // ID del messaggio (usato dal sistema esterno)
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRisultatoCampagna()
            {
                m_IDCampagna = 0;
                m_Campagna = null;
                m_NomeCampagna = "";
                m_Destinatario = null;
                m_IDDestinatario = 0;
                m_NomeDestinatario = "";
                m_StatoMessaggio = StatoMessaggioCampagna.InPreparazione;
                m_DataSpedizione = default;
                m_NomeMezzoSpedizione = "";
                m_StatoSpedizione = "";
                m_DataConsegna = default;
                m_DataLettura = default;
                m_TipoCampagna = TipoCampagnaPubblicitaria.NonImpostato;
                m_IndirizzoDestinatario = "";
                m_DataEsecuzuine = default;
                m_MessageID = DMD.Strings.vbNullString;
                 
            }
 

            /// <summary>
            /// Restituisce o imposta una stringa utilizzabile dal sistema di invio/ricezione per identificare univocamente il messaggio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string MessageID
            {
                get
                {
                    return m_MessageID;
                }

                set
                {
                    string oldValue = m_MessageID;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MessageID = value;
                    DoChanged("MessageID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della campagna
            /// </summary>
            public int IDCampagna
            {
                get
                {
                    return DBUtils.GetID(m_Campagna, m_IDCampagna);
                }

                set
                {
                    int oldValue = IDCampagna;
                    if (oldValue == value)
                        return;
                    m_IDCampagna = value;
                    m_Campagna = null;
                    DoChanged("IDCampagna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un riferimento alla campagna
            /// </summary>
            public CCampagnaPubblicitaria Campagna
            {
                get
                {
                    if (m_Campagna is null)
                        m_Campagna = minidom.ADV.Campagne.GetItemById(m_IDCampagna);
                    return m_Campagna;
                }

                set
                {
                    m_Campagna = value;
                    m_IDCampagna = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeCampagna = value.NomeCampagna;
                    DoChanged("Campagna", value);
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
            /// Restituisce o imposta il destinatario
            /// </summary>
            public Anagrafica.CPersona Destinatario
            {
                get
                {
                    if (m_Destinatario is null)
                        m_Destinatario = Anagrafica.Persone.GetItemById(m_IDDestinatario);
                    return m_Destinatario;
                }

                set
                {
                    m_Destinatario = value;
                    m_IDDestinatario = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeDestinatario = value.Nominativo;
                    DoChanged("Destinatario", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del destinatario
            /// </summary>
            public int IDDestinatario
            {
                get
                {
                    return DBUtils.GetID(m_Destinatario, m_IDDestinatario);
                }

                set
                {
                    int oldValue = IDDestinatario;
                    if (oldValue == value)
                        return;
                    m_IDDestinatario = value;
                    m_Destinatario = null;
                    DoChanged("IDDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del destinatario
            /// </summary>
            public string NomeDestinatario
            {
                get
                {
                    return m_NomeDestinatario;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeDestinatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDestinatario = value;
                    DoChanged("NomeDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'esito della campagna per il destinatario
            /// </summary>
            public StatoMessaggioCampagna StatoMessaggio
            {
                get
                {
                    return m_StatoMessaggio;
                }

                set
                {
                    var oldValue = m_StatoMessaggio;
                    if (oldValue == value)
                        return;
                    m_StatoMessaggio = value;
                    DoChanged("StatoMessaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di spedizione
            /// </summary>
            public DateTime? DataSpedizione
            {
                get
                {
                    return m_DataSpedizione;
                }

                set
                {
                    var oldValue = m_DataSpedizione;
                    if (oldValue == value == true)
                        return;
                    m_DataSpedizione = value;
                    DoChanged("DataSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mezzo usato per la spedizione
            /// </summary>
            public string NomeMezzoSpedizione
            {
                get
                {
                    return m_NomeMezzoSpedizione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeMezzoSpedizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeMezzoSpedizione = value;
                    DoChanged("NomeMezzoSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive l'esito della spedizione
            /// </summary>
            public string StatoSpedizioneEx
            {
                get
                {
                    return m_StatoSpedizione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_StatoSpedizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StatoSpedizione = value;
                    DoChanged("StatoSpedizioneEx", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di consegna
            /// </summary>
            public DateTime? DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }

                set
                {
                    var oldValue = m_DataConsegna;
                    if (oldValue == value == true)
                        return;
                    m_DataConsegna = value;
                    DoChanged("DataConsegna", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta la data di lettura
            /// </summary>
            public DateTime? DataLettura
            {
                get
                {
                    return m_DataLettura;
                }

                set
                {
                    var oldValue = m_DataLettura;
                    if (oldValue == value == true)
                        return;
                    m_DataLettura = value;
                    DoChanged("DataLettura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la tipologia della campagna
            /// </summary>
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
            /// Restituisce o imposta una stringa che rappresenta l'indirizzo usato per raggiungere il destinatario
            /// </summary>
            public string IndirizzoDestinatario
            {
                get
                {
                    return m_IndirizzoDestinatario;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IndirizzoDestinatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IndirizzoDestinatario = value;
                    DoChanged("IndirizzoDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di esecuzione della campagna
            /// </summary>
            public DateTime? DataEsecuzione
            {
                get
                {
                    return m_DataEsecuzuine;
                }

                set
                {
                    var oldValue = m_DataEsecuzuine;
                    if (oldValue == value == true)
                        return;
                    m_DataEsecuzuine = value;
                    DoChanged("DataEsecuzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il testo generato dal modello
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ParseTemplate(string value)
            {
                string ret = value;
                if (Destinatario is object)
                {
                    if (Destinatario is Anagrafica.CPersonaFisica)
                    {
                        {
                            var withBlock = (Anagrafica.CPersonaFisica)Destinatario;
                            ret = Strings.Replace(ret, "%%NOME%%", withBlock.Nome);
                            ret = Strings.Replace(ret, "%%COGNOME%%", withBlock.Cognome);
                            if (withBlock.ImpiegoPrincipale is object)
                            {
                                ret = Strings.Replace(ret, "%%NOMEAZIENDA%%", withBlock.ImpiegoPrincipale.NomeAzienda);
                            }
                            else
                            {
                                ret = Strings.Replace(ret, "%%NOMEAZIENDA%%", "");
                            }
                        }
                    }
                    else
                    {
                        ret = Strings.Replace(ret, "%%NOME%%", "");
                        ret = Strings.Replace(ret, "%%COGNOME%%", "");
                        ret = Strings.Replace(ret, "%%NOMEAZIENDA%%", "");
                    }

                    ret = Strings.Replace(ret, "%%NOMINATIVO%%", Destinatario.Nominativo);
                    ret = Strings.Replace(ret, "%%DATANASCITA%%", Sistema.Formats.FormatUserDate(Destinatario.DataNascita));
                    ret = Strings.Replace(ret, "%%DATAMORTE%%", Sistema.Formats.FormatUserDate(Destinatario.DataMorte));
                    ret = Strings.Replace(ret, "%%TITOLO%%", Destinatario.Titolo);
                    ret = Strings.Replace(ret, "%%PROFESSIONE%%", Destinatario.Professione);
                    ret = Strings.Replace(ret, "%%CITTADINANZA%%", Destinatario.Cittadinanza);
                    ret = Strings.Replace(ret, "%%NATOA%%", Destinatario.NatoA.ToString());
                    ret = Strings.Replace(ret, "%%MORTOA%%", Destinatario.MortoA.ToString());
                    ret = Strings.Replace(ret, "%%RESIDENTEA%%", Destinatario.ResidenteA.ToString());
                    ret = Strings.Replace(ret, "%%DOMICILIATOA%%", Destinatario.DomiciliatoA.ToString());
                }
                else
                {
                    ret = Strings.Replace(ret, "%%NOME%%", "");
                    ret = Strings.Replace(ret, "%%COGNOME%%", "");
                    ret = Strings.Replace(ret, "%%NOMINATIVO%%", NomeDestinatario);
                    ret = Strings.Replace(ret, "%%DATANASCITA%%", "");
                    ret = Strings.Replace(ret, "%%DATAMORTE%%", "");
                    ret = Strings.Replace(ret, "%%TITOLO%%", "");
                    ret = Strings.Replace(ret, "%%PROFESSIONE%%", "");
                    ret = Strings.Replace(ret, "%%CITTADINANZA%%", "");
                    ret = Strings.Replace(ret, "%%NATOA%%", "");
                    ret = Strings.Replace(ret, "%%MORTOA%%", "");
                    ret = Strings.Replace(ret, "%%RESIDENTEA%%", "");
                    ret = Strings.Replace(ret, "%%DOMICILIATOA%%", "");
                }

                ret = Strings.Replace(ret, "%%DATAINVIO%%", Sistema.Formats.FormatUserDateTimeOggi(DataSpedizione));
                ret = Strings.Replace(ret, "%%DATAESECUZIONE%%", Sistema.Formats.FormatUserDateTimeOggi(DataEsecuzione));
                ret = Strings.Replace(ret, "%%DATACONSEGNA%%", Sistema.Formats.FormatUserDateTimeOggi(DataConsegna));
                ret = Strings.Replace(ret, "%%DATALETTURA%%", Sistema.Formats.FormatUserDateTimeOggi(DataLettura));
                ret = Strings.Replace(ret, "%%INDIRIZZOSPEDIZIONE%%", IndirizzoDestinatario);
                ret = Strings.Replace(ret, "%%NOMECAMPAGNA%%", NomeCampagna);
                ret = Strings.Replace(ret, "%%NOMEDESTINATARIO%%", NomeDestinatario);
                ret = Strings.Replace(ret, "%%MEZZOSPEDIZIONE%%", NomeMezzoSpedizione);
                ret = Strings.Replace(ret, "%%IDCAMPAGNA%%", IDCampagna.ToString());
                Anagrafica.CUfficio po = null;
                string nomePO = Anagrafica.Aziende.AziendaPrincipale.Nominativo;
                string indirizzoPO = Anagrafica.Aziende.AziendaPrincipale.ResidenteA.ToString();
                string telefonoPO = Sistema.Formats.FormatPhoneNumber(Anagrafica.Aziende.AziendaPrincipale.Telefono);
                string faxPO = Sistema.Formats.FormatPhoneNumber(Anagrafica.Aziende.AziendaPrincipale.Fax);
                string emailPO = Anagrafica.Aziende.AziendaPrincipale.eMail;
                if (Destinatario is object)
                    po = Destinatario.PuntoOperativo;
                if (po is object)
                {
                    nomePO = po.Nome;
                    indirizzoPO = po.Indirizzo.ToString();
                    telefonoPO = Sistema.Formats.FormatPhoneNumber(po.Telefono);
                    faxPO = Sistema.Formats.FormatPhoneNumber(po.Fax);
                    emailPO = po.eMail;
                }

                ret = Strings.Replace(ret, "%%PUNTOOPERATIVO_NOME%%", nomePO);
                ret = Strings.Replace(ret, "%%PUNTOOPERATIVO_INDIRIZZO%%", indirizzoPO);
                ret = Strings.Replace(ret, "%%PUNTOOPERATIVO_TELEFONO%%", telefonoPO);
                ret = Strings.Replace(ret, "%%PUNTOOPERATIVO_FAX%%", faxPO);
                ret = Strings.Replace(ret, "%%PUNTOOPERATIVO_EMAIL%%", emailPO);
                return ret;
            }

            /// <summary>
            /// Invia l'oggetto
            /// </summary>
            public void Invia()
            {
                switch (m_StatoMessaggio)
                {
                    case StatoMessaggioCampagna.ProntoPerLaSpedizione:
                    case StatoMessaggioCampagna.RifiutatoDalDestinatario:
                    case StatoMessaggioCampagna.RifiutatoDalVettore:
                        {
                            var handler = minidom.ADV.Campagne.GetHandler(TipoCampagna);
                            // If Me.Campagna.RichiediConfermaDiLettura AndAlso handler.SupportaConfermaLettura Then handler.RichiediConfermaLettura = True
                            // If Me.Campagna.RichiediConfermaDiRecapito AndAlso handler.SupportaConfermaRecapito Then handler.RichiediConfermaRecapito = True
                            m_DataSpedizione = DMD.DateUtils.Now();
                            m_StatoSpedizione = "Sto inviando";
                            m_NomeMezzoSpedizione = handler.GetNomeMezzoSpedizione();
                            m_StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore;
#if (!DEBUG)
                            try {
#endif
                                if (handler.IsBanned(this))
                                    throw new ArgumentException("Indirizzo bloccato nell'elenco principale");
                                if (handler.IsBlocked(this))
                                    throw new ArgumentException("Indirizzo bloccato nel CRM");
                                handler.Send(this);
#if (!DEBUG)
                        }  catch (Exception ex) {
                                m_StatoSpedizione = ex.Message;
                                Sistema.Events.NotifyUnhandledException(ex);
                            }
#endif
                            Save(true);
                            break;
                        }

                    default:
                        {
                            throw new InvalidOperationException("Stato di esecuzione non valido (" + ((int)m_StatoMessaggio).ToString() + ")");
                        }
                }
            }

            //protected override Databases.CDBConnection GetConnection()
            //{
            //    return CustomerCalls.CRM.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.ADV.RisultatiCampagna; // RisultatiCampagna.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ADVResults";
            }


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCampagna = reader.Read("IDCampagna",  m_IDCampagna);
                m_NomeCampagna = reader.Read("NomeCampagna",  m_NomeCampagna);
                m_IDDestinatario = reader.Read("IDDestinatario",  m_IDDestinatario);
                m_NomeDestinatario = reader.Read("NomeDestinatario",  m_NomeDestinatario);
                m_IndirizzoDestinatario = reader.Read("IndirizzoDestinatario",  m_IndirizzoDestinatario);
                m_StatoMessaggio = reader.Read("StatoMessaggio",  m_StatoMessaggio);
                m_DataSpedizione = reader.Read("DataSpedizione",  m_DataSpedizione);
                m_NomeMezzoSpedizione = reader.Read("NomeMezzoSpedizione",  m_NomeMezzoSpedizione);
                m_StatoSpedizione = reader.Read("StatoSpedizione",  m_StatoSpedizione);
                m_DataConsegna = reader.Read("DataConsegna",  m_DataConsegna);
                m_DataLettura = reader.Read("DataLettura",  m_DataLettura);
                m_TipoCampagna = reader.Read("TipoCampagna",  m_TipoCampagna);
                m_DataEsecuzuine = reader.Read("DataEsecuzione",  m_DataEsecuzuine);
                m_MessageID = reader.Read("MessageID",  m_MessageID);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCampagna", IDCampagna);
                writer.Write("NomeCampagna", m_NomeCampagna);
                writer.Write("IDDestinatario", IDDestinatario);
                writer.Write("NomeDestinatario", m_NomeDestinatario);
                writer.Write("IndirizzoDestinatario", m_IndirizzoDestinatario);
                writer.Write("StatoMessaggio", m_StatoMessaggio);
                writer.Write("DataSpedizione", m_DataSpedizione);
                writer.Write("NomeMezzoSpedizione", m_NomeMezzoSpedizione);
                writer.Write("StatoSpedizione", m_StatoSpedizione);
                writer.Write("DataConsegna", m_DataConsegna);
                writer.Write("DataLettura", m_DataLettura);
                writer.Write("TipoCampagna", m_TipoCampagna);
                writer.Write("DataEsecuzione", m_DataEsecuzuine);
                writer.Write("MessageID", m_MessageID);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDCampagna", typeof(int), 1);
                c = table.Fields.Ensure("NomeCampagna", typeof(string), 255);
                c = table.Fields.Ensure("IDDestinatario", typeof(int), 1);
                c = table.Fields.Ensure("NomeDestinatario", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoDestinatario", typeof(string), 0);
                c = table.Fields.Ensure("StatoMessaggio", typeof(int), 1);
                c = table.Fields.Ensure("DataSpedizione", typeof(DateTime), 1);
                c = table.Fields.Ensure("NomeMezzoSpedizione", typeof(string), 255);
                c = table.Fields.Ensure("StatoSpedizione", typeof(int), 1);
                c = table.Fields.Ensure("DataConsegna", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataLettura", typeof(DateTime), 1);
                c = table.Fields.Ensure("TipoCampagna", typeof(int), 1);
                c = table.Fields.Ensure("DataEsecuzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("MessageID", typeof(string), 255);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCampagna", new string[] { "TipoCampagna", "IDCampagna", "NomeCampagna" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDestinatario", new string[] { "IDDestinatario", "NomeDestinatario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo", new string[] { "IndirizzoDestinatario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEsito", new string[] { "StatoMessaggio", "MessageID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSpedizione", new string[] { "NomeMezzoSpedizione", "StatoSpedizione", "DataSpedizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxConsegna", new string[] { "DataConsegna", "DataLettura", "DataEsecuzione" }, DBFieldConstraintFlags.None);
                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCampagna", IDCampagna);
                writer.WriteAttribute("NomeCampagna", m_NomeCampagna);
                writer.WriteAttribute("IDDestinatario", IDDestinatario);
                writer.WriteAttribute("NomeDestinatario", m_NomeDestinatario);
                writer.WriteAttribute("IndirizzoDestinatario", m_IndirizzoDestinatario);
                writer.WriteAttribute("StatoMessaggio", (int?)m_StatoMessaggio);
                writer.WriteAttribute("DataSpedizione", m_DataSpedizione);
                writer.WriteAttribute("NomeMezzoSpedizione", m_NomeMezzoSpedizione);
                writer.WriteAttribute("StatoSpedizione", m_StatoSpedizione);
                writer.WriteAttribute("DataConsegna", m_DataConsegna);
                writer.WriteAttribute("DataLettura", m_DataLettura);
                writer.WriteAttribute("TipoCampagna", (int?)m_TipoCampagna);
                writer.WriteAttribute("DataEsecuzione", m_DataEsecuzuine);
                writer.WriteAttribute("MessageID", m_MessageID);
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
                    case "IDCampagna":
                        {
                            m_IDCampagna = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCampagna":
                        {
                            m_NomeCampagna = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDestinatario":
                        {
                            IDDestinatario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeDestinatario":
                        {
                            m_NomeDestinatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IndirizzoDestinatario":
                        {
                            m_IndirizzoDestinatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoMessaggio":
                        {
                            m_StatoMessaggio = (StatoMessaggioCampagna)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataSpedizione":
                        {
                            m_DataSpedizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NomeMezzoSpedizione":
                        {
                            m_NomeMezzoSpedizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoSpedizione":
                        {
                            m_StatoSpedizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataConsegna":
                        {
                            m_DataConsegna = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataLettura":
                        {
                            m_DataLettura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoCampagna":
                        {
                            m_TipoCampagna = (TipoCampagnaPubblicitaria)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataEsecuzione":
                        {
                            m_DataEsecuzuine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MessageID":
                        {
                            m_MessageID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Aggiorna lo stato del messaggio
            /// </summary>
            /// <remarks></remarks>
            public void Update()
            {
                if (string.IsNullOrEmpty(m_MessageID))
                    return;
                var handler = Campagne.GetHandler(TipoCampagna);
                handler.UpdateStatus(this);
                Save();
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.NomeCampagna, " ", this.DataEsecuzione, " ", this.NomeDestinatario, " ", this.IndirizzoDestinatario);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.NomeCampagna, this.DataEsecuzione, this.NomeDestinatario, this.IndirizzoDestinatario);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CRisultatoCampagna) && this.Equals((CRisultatoCampagna)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CRisultatoCampagna obj)
            {
                return base.Equals(obj)
                        && DMD.Integers.EQ(this.IDCampagna, obj.IDCampagna)
                        && DMD.Strings.EQ(this.m_NomeCampagna, obj.m_NomeCampagna)
                        && DMD.Integers.EQ(this.IDDestinatario, obj.IDDestinatario)
                        && DMD.Strings.EQ(this.m_NomeDestinatario, obj.m_NomeDestinatario)
                        && DMD.Integers.EQ((int)this.m_StatoMessaggio, (int)obj.m_StatoMessaggio)
                        && DMD.DateUtils.EQ(this.m_DataSpedizione, obj.m_DataSpedizione)
                        && DMD.Strings.EQ(this.m_NomeMezzoSpedizione, obj.m_NomeMezzoSpedizione)
                        && DMD.Strings.EQ(this.m_StatoSpedizione, obj.m_StatoSpedizione)
                        && DMD.DateUtils.EQ(this.m_DataConsegna, obj.m_DataConsegna)
                        && DMD.DateUtils.EQ(this.m_DataLettura, obj.m_DataLettura)
                        && DMD.Integers.EQ((int)this.m_TipoCampagna, (int)obj.m_TipoCampagna)
                        && DMD.Strings.EQ(this.m_IndirizzoDestinatario, obj.m_IndirizzoDestinatario)
                        && DMD.DateUtils.EQ(this.m_DataEsecuzuine, obj.m_DataEsecuzuine)
                        && DMD.Strings.EQ(this.m_MessageID, obj.m_MessageID)
                        ;                   // ID del messaggio (usato dal sistema esterno)
            //private CKeyCollection m_Parameters;

            }



        }
    }
}