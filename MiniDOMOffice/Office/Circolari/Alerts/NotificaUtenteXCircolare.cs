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
using static minidom.Store;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Stato di una pubblicazione / circolare
        /// </summary>
        public enum StatoNotificaUtenteXCircolare : int
        {
            /// <summary>
            /// Non consegnato
            /// </summary>
            NONCONSEGNATO = 0,

            /// <summary>
            /// Consegnato
            /// </summary>
            CONSEGNATO = 1,

            /// <summary>
            /// Letto
            /// </summary>
            LETTO = 2
        }

        /// <summary>
        /// Descrive lo stato di consegna di una comunicazione / circolare
        /// </summary>
        [Serializable]
        public class NotificaUtenteXCircolare
            : minidom.Databases.DBObject
        {
            private int m_IDComunicazione;
            [NonSerialized] private Circolare m_Comunicazione;
            private int m_TargetUserID;
            [NonSerialized] private Sistema.CUser m_TargetUser;
            private string m_Via;
            private string m_Param;
            private StatoNotificaUtenteXCircolare m_StatoComunicazione;
            private DateTime? m_DataConsegna;
            private DateTime? m_DataLettura;
            private string m_DettaglioStato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public NotificaUtenteXCircolare()
            {
                m_IDComunicazione = 0;
                m_Comunicazione = null;
                m_TargetUserID = 0;
                m_TargetUser = null;
                m_Via = "";
                m_StatoComunicazione = StatoNotificaUtenteXCircolare.NONCONSEGNATO;
                m_DataConsegna = default;
                m_DataLettura = default;
                m_DettaglioStato = "";
            }

            /// <summary>
            /// ID della circolare
            /// </summary>
            public int IDComunicazione
            {
                get
                {
                    return DBUtils.GetID(m_Comunicazione, m_IDComunicazione);
                }

                set
                {
                    int oldValue = IDComunicazione;
                    if (oldValue == value)
                        return;
                    m_IDComunicazione = value;
                    m_Comunicazione = null;
                    DoChanged("IDComunicazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o impsta l'oggetto comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Circolare Comunicazione
            {
                get
                {
                    if (m_Comunicazione is null)
                        m_Comunicazione = minidom.Office.Circolari.GetItemById(m_IDComunicazione);
                    return m_Comunicazione;
                }

                set
                {
                    var oldValue = Comunicazione;
                    if (oldValue == value)
                        return;
                    m_Comunicazione = value;
                    m_IDComunicazione = DBUtils.GetID(value, 0);
                    DoChanged("Comunicazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente destinatario della comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser TargetUser
            {
                get
                {
                    if (m_TargetUser is null)
                        m_TargetUser = Sistema.Users.GetItemById(m_TargetUserID);
                    return m_TargetUser;
                }

                set
                {
                    var oldValue = TargetUser;
                    if (oldValue == value)
                        return;
                    m_TargetUser = value;
                    m_TargetUserID = DBUtils.GetID(value, 0);
                    DoChanged("TargetUser", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente destinazione
            /// </summary>
            public int TargetUserID
            {
                get
                {
                    return DBUtils.GetID(m_TargetUser, m_TargetUserID);
                }

                set
                {
                    int oldValue = TargetUserID;
                    if (oldValue == value)
                        return;
                    m_TargetUser = null;
                    m_TargetUserID = value;
                    DoChanged("TargetUserID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituosce o imposta una stringa che descrive lo stato della comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Via;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato = value;
                    DoChanged("DettaglioStato ", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mezzo attraverso cui è stata inviata la comunicazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Via
            {
                get
                {
                    return m_Via;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Via;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Via = value;
                    DoChanged("Via", value, oldValue);
                }
            }

            /// <summary>
            /// Parametri
            /// </summary>
            public string Param
            {
                get
                {
                    return m_Param;
                }

                set
                {
                    string oldValue = m_Param;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Param = value;
                    DoChanged("Param", value, oldValue);
                }
            }

            /// <summary>
            /// Stato di consegna del messaggio
            /// </summary>
            public StatoNotificaUtenteXCircolare StatoComunicazione
            {
                get
                {
                    return m_StatoComunicazione;
                }

                set
                {
                    var oldValue = m_StatoComunicazione;
                    if (oldValue == value)
                        return;
                    m_StatoComunicazione = value;
                    DoChanged("StatoComunicazione", value, oldValue);
                }
            }

            /// <summary>
            /// Data di consegna
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
            /// Data di lettura
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
            /// Interpreta il template
            /// </summary>
            /// <param name="body"></param>
            /// <returns></returns>
            private string ParseTemplate(string body)
            {
                body = Strings.Replace(body, "%%TARGERTUSERNAME%%", TargetUser.Nominativo);
                body = Strings.Replace(body, "%%SOURCEUSERNAME%%", Sistema.Users.CurrentUser.Nominativo);
                body = Strings.Replace(body, "%%ID%%", IDComunicazione.ToString());
                body = Strings.Replace(body, "%%DOCUMENTNAME%%", Comunicazione.ToString());
                body = Strings.Replace(body, "%%DOCUMENTLINK%%", Comunicazione.URL);
                body = Strings.Replace(body, "%%BASEURL%%", Sistema.ApplicationContext.BaseURL);
                body = Strings.Replace(body, "%%NUMEROCOMUNICAZIONE%%", Comunicazione.Progressivo.ToString());
                body = Strings.Replace(body, "%%CATEGORIACOMUNICAZIONE%%", Comunicazione.Categoria);
                body = Strings.Replace(body, "%%DESCRIZIONECOMUNICAZIONE%%", Comunicazione.Titolo);
                body = Strings.Replace(body, "%%NOTECOMUNICAZIONE%%", Comunicazione.Testo);
                return body;
            }

            /// <summary>
            /// Notifica all'utente la comunicazione
            /// </summary>
            public void Notify()
            {
                Save();
                switch (Strings.LCase(Via) ?? "")
                {
                    case "e-mail":
                        {
                            if (!string.IsNullOrEmpty(Param))
                            {
                                string sender = minidom.Sistema.ApplicationContext.Settings.GetValueString("Office.Comunicazioni.Templates.Sender", "");
                                string template = minidom.Sistema.ApplicationContext.Settings.GetValueString("Office.Comunicazioni.Templates.Template", "");
                                string subject = minidom.Sistema.ApplicationContext.Settings.GetValueString("Office.Comunicazioni.Templates.NotifySubject", "");
                                if (string.IsNullOrEmpty(subject))
                                    subject = "E' richiesta la presa visione della circolare N° %%NUMEROCOMUNICAZIONE%% - %%DOCUMENTNAME%%"; // "Notifica da parte di %%SOURCEUSERNAME%%: Il documento %%DOCUMENTNAME%% è stato aggiornato"
                                subject = ParseTemplate(subject);
                                string body = ParseTemplate(Sistema.FileSystem.GetTextFileContents(template));
                                var m = Sistema.EMailer.PrepareMessage(sender, Param, "", "", subject, body, "COMUNICAZIONI", true);
                                Sistema.EMailer.SendMessageAsync(m, true);
                            }

                            break;
                        }

                    default:
                        {
                            string text = "E' richiesta la presa visione della circolare N° <b>%%NUMEROCOMUNICAZIONE%%</b><br/><span style='color:blue;'>%%DOCUMENTNAME%%</span>";
                            text = ParseTemplate(text);
                            Sistema.Notifiche.ProgramAlert(TargetUser, text, DMD.DateUtils.Now(), this, "Comunicazioni/Circolari");
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
                return DMD.Strings.ConcatArray(this.DataConsegna, ": ", this.IDComunicazione);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.DataConsegna);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Circolari.Results;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ComunicazioniAlert";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDComunicazione = reader.Read("IDComunicazione", m_IDComunicazione);
                m_TargetUserID = reader.Read("IDUser", m_TargetUserID);
                m_Via = reader.Read("Via", m_Via);
                m_Param = reader.Read("Param", m_Param);
                m_StatoComunicazione = reader.Read("StatoComunicazione", m_StatoComunicazione);
                m_DataConsegna = reader.Read("DataConsegna", m_DataConsegna);
                m_DataLettura = reader.Read("DataLettura", m_DataLettura);
                m_DettaglioStato = reader.Read("DettaglioStato", m_DettaglioStato);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel database
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDComunicazione", IDComunicazione);
                writer.Write("IDUser", TargetUserID);
                writer.Write("Via", m_Via);
                writer.Write("Param", m_Param);
                writer.Write("StatoComunicazione", m_StatoComunicazione);
                writer.Write("DataConsegna", m_DataConsegna);
                writer.Write("DataLettura", m_DataLettura);
                writer.Write("DettaglioStato", m_DettaglioStato);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDComunicazione", typeof(int), 1);
                c = table.Fields.Ensure("IDUser", typeof(int), 1);
                c = table.Fields.Ensure("Via", typeof(string), 255);
                c = table.Fields.Ensure("Param", typeof(string), 255);
                c = table.Fields.Ensure("StatoComunicazione", typeof(int), 1);
                c = table.Fields.Ensure("DataConsegna", typeof(int), 1);
                c = table.Fields.Ensure("DataLettura", typeof(int), 1);
                c = table.Fields.Ensure("DettaglioStato", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxComunicazione", new string[] { "IDComunicazione", "IDUser" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxVia", new string[] { "Via", "Param" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStato", new string[] { "StatoComunicazione", "DataConsegna", "DataLettura" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDettaglio", new string[] { "DettaglioStato" }, DBFieldConstraintFlags.None);
            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDComunicazione", IDComunicazione);
                writer.WriteAttribute("IDUser", TargetUserID);
                writer.WriteAttribute("Via", m_Via);
                writer.WriteAttribute("Param", m_Param);
                writer.WriteAttribute("StatoComunicazione", (int?)m_StatoComunicazione);
                writer.WriteAttribute("DataConsegna", m_DataConsegna);
                writer.WriteAttribute("DataLettura", m_DataLettura);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
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
                    case "IDComunicazione":
                        {
                            m_IDComunicazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDUser":
                        {
                            m_TargetUserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Via":
                        {
                            m_Via = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Param":
                        {
                            m_Param = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoComunicazione":
                        {
                            m_StatoComunicazione = (StatoNotificaUtenteXCircolare)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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

                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is NotificaUtenteXCircolare) && this.Equals((NotificaUtenteXCircolare)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(NotificaUtenteXCircolare obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDComunicazione, obj.m_IDComunicazione)
                    && DMD.Integers.EQ(this.m_TargetUserID, obj.m_TargetUserID)
                    && DMD.Strings.EQ(this.m_Via, obj.m_Via)
                    && DMD.Strings.EQ(this.m_Param, obj.m_Param)
                    && DMD.Integers.EQ((int)this.m_StatoComunicazione, (int)obj.m_StatoComunicazione)
                    && DMD.DateUtils.EQ(this.m_DataLettura, obj.m_DataLettura)
                    && DMD.DateUtils.EQ(this.m_DataConsegna, obj.m_DataConsegna)
                    && DMD.Strings.EQ(this.m_DettaglioStato, obj.m_DettaglioStato)
                    ;
            }
        }
    }
}