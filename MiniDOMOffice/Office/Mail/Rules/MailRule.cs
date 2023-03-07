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
        /// Flag per gli oggetti <see cref="MailRule"/>
        /// </summary>
        public enum MailRuleDisposition : int
        {
            /// <summary>
            /// Nessuna azione richiesta
            /// </summary>
            /// <remarks></remarks>
            None = 0,

            /// <summary>
            /// Sposta il messaggio nel cestino
            /// </summary>
            /// <remarks></remarks>
            Delete = 1,

            /// <summary>
            /// Copia il messaggio nella cartella specificata
            /// </summary>
            /// <remarks></remarks>
            CopyTo = 2,

            /// <summary>
            /// Sposta il messaggio nella cartella specificata
            /// </summary>
            /// <remarks></remarks>
            MoveTo = 3,

            /// <summary>
            /// Invia una copia del messaggio al destinatario specificato
            /// </summary>
            /// <remarks></remarks>
            SendTo = 4
        }

        /// <summary>
        /// Regola per i messaggi mail
        /// </summary>
        [Serializable]
        public class MailRuleAction 
            : DMD.XML.DMDBaseXMLObject
        {

            /// <summary>
            /// Disposizione
            /// </summary>
            public MailRuleDisposition Disposition;

            /// <summary>
            /// Parametro 1 dell'azione
            /// </summary>
            public string Parameter1;

            /// <summary>
            /// Parametro 2 dell'azione
            /// </summary>
            public string Parameter2;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailRuleAction()
            {
                Disposition = MailRuleDisposition.None;
                Parameter1 = "";
                Parameter2 = "";
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
                    case "Disposition":
                        {
                            Disposition = (MailRuleDisposition)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Parameter1":
                        {
                            Parameter1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Parameter2":
                        {
                            Parameter2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Disposition", (int?)Disposition);
                writer.WriteAttribute("Parameter1", Parameter1);
                writer.WriteAttribute("Parameter2", Parameter2);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Esegue la regola
            /// </summary>
            /// <param name="m"></param>
            protected internal virtual void Execute(MailMessage m)
            {
                switch (Disposition)
                {
                    case MailRuleDisposition.None:
                        break;

                    case MailRuleDisposition.Delete:
                        
                    // m.MoveTo(m.Application.Folders.TrashBin)
                    case MailRuleDisposition.CopyTo:
                         
                    // m.CopyTo(m.Application.Folders.GetItemByName(Me.Parameter1))
                    case MailRuleDisposition.MoveTo:
                         
                    // m.MoveTo(m.Application.Folders.GetItemByName(Me.Parameter1))
                    case MailRuleDisposition.SendTo:
                         
                            // m.MoveTo(m.Application.Folders.GetItemByName(Me.Parameter1))
                            throw new NotImplementedException();
                            
                         
                }
            }
             
        }

        /// <summary>
        /// Regola per i messaggi mail <see cref="MailMessage"/> in una <see cref="MailApplication"/>
        /// </summary>
        [Serializable]
        public class MailRule
            : MailRuleBase
        {

             

            /// <summary>
            /// Restituisce o imposta un elenco di indirizzi e-mail separati da "," che vengono verificati nel campo "From"
            /// Il test è definito vero l'intersezione tra i due elenchi non è vuota
            /// </summary>
            /// <remarks></remarks>
            public string FromAddress;

            /// <summary>
            /// Restituisce o imposta un elenco di indirizzi e-mail separati da "," che vengono verificati nei campi "To, CC, CCN"
            /// Il test è definito vero l'intersezione tra i due elenchi non è vuota
            /// </summary>
            /// <remarks></remarks>
            public string ToAddress;

            /// <summary>
            /// Restituisce o imposta un elenco di indirizzi e-mail separati da "," che vengono verificati nel campo "To"
            /// Il test è definito vero l'intersezione tra i due elenchi non è vuota
            /// </summary>
            /// <remarks></remarks>
            public string JustToAddress;

            /// <summary>
            /// Restituisce o imposta un elenco di domini separati da "," che vengono testati nel campo "From"
            /// </summary>
            /// <remarks></remarks>
            public string FromDomain;

            /// <summary>
            /// Restituisce o imposta un elenco di domini separati da "," che vengono testati nel campo "To, CC e CCN"
            /// </summary>
            /// <remarks></remarks>
            public string ToDomain;

            /// <summary>
            /// Restituisce o imposta un intervallo per la data di invio
            /// </summary>
            /// <remarks></remarks>
            public string SendDateInterval;

            /// <summary>
            /// Restituisce o imposta l'estremo inferiore per la data di invio
            /// </summary>
            /// <remarks></remarks>
            public DateTime? SendDateFrom;

            /// <summary>
            /// Restituisce o imposta l'estremo superiore della data di invio
            /// </summary>
            /// <remarks></remarks>
            public DateTime? SendDateTo;

            /// <summary>
            /// Restituisce o imposta un intervallo per la data di ricezione
            /// </summary>
            /// <remarks></remarks>
            public string ReceiveDateInterval;

            /// <summary>
            /// Restituisce o imposta l'estremo inferiore per la data di ricezione
            /// </summary>
            /// <remarks></remarks>
            public DateTime? ReceiveDateFrom;

            /// <summary>
            /// Restituisce o imposta l'estremo superiore della data di ricezione
            /// </summary>
            /// <remarks></remarks>
            public DateTime? ReceiveDateTo;

            /// <summary>
            /// Restituisce o imposta il nome dell'account utilizzato per inviare/ricevere il messaggio
            /// </summary>
            /// <remarks></remarks>
            public string AccountName;

            /// <summary>
            /// Elenco di parole o frasi (separate dal CR+LF) contenute nel corpo del messaggio
            /// </summary>
            /// <remarks></remarks>
            public string BodyContains;

            /// <summary>
            /// Frase contenuto nel soggetto
            /// </summary>
            /// <remarks></remarks>
            public string SubjectContains;

            [NonSerialized] private MailApplication m_Application;

            /// <summary>
            /// Elenco di azioni da eseguire in sequenza dalla prima all'ultima
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<MailRuleAction> Actions { get; set; } = new CCollection<MailRuleAction>();

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailRule()
            {
                m_Application = null;
            }

            /// <summary>
            /// Restituisce un riferimento all'app
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    return m_Application;
                }
            }

            /// <summary>
            /// Imposta il riferimento all'app
            /// </summary>
            /// <param name="app"></param>
            protected virtual internal void SetApplication(MailApplication app)
            {
                m_Application = app;
            }

            /// <summary>
            /// Controlla gli indirizzi del messaggio
            /// </summary>
            /// <param name="testList"></param>
            /// <param name="adressies"></param>
            /// <returns></returns>
            private bool CheckAddresses(string testList, CCollection<MailAddress> adressies)
            {
                testList = Strings.Trim(testList);
                if (string.IsNullOrEmpty(testList))
                    return true;
                var testItems = Strings.Split(testList, ",");
                foreach (var testItem in testItems)
                {
                    foreach (MailAddress address in adressies)
                    {
                        if (DMD.Booleans.ValueOf(DMD.Strings.Compare(address.Address, testItem, true)))
                            return true;
                    }
                }

                return false;
            }

            private bool CheckAddress(string testList, MailAddress address)
            {
                testList = Strings.Trim(testList);
                if (string.IsNullOrEmpty(testList))
                    return true;
                var testItems = Strings.Split(testList, ",");
                foreach (var testItem in testItems)
                {
                    if (DMD.Booleans.ValueOf(DMD.Strings.Compare(address.Address, testItem, true)))
                        return true;
                }

                return false;
            }

            private bool CheckDomains(string testList, MailAddress address)
            {
                testList = Strings.Trim(testList);
                if (string.IsNullOrEmpty(testList))
                    return true;
                var testItems = Strings.Split(testList, ",");
                foreach (var testItem in testItems)
                {
                    int p = Strings.InStr(testItem, "@");
                    if (p > 0)
                    {
                        string domain = Strings.Mid(testItem, p + 1);
                        if (DMD.Booleans.ValueOf(DMD.Strings.Compare(address.Host, domain, true)))
                            return true;
                    }
                }

                return false;
            }

            private bool CheckDomains(string testList, CCollection<MailAddress> adressies)
            {
                testList = Strings.Trim(testList);
                if (string.IsNullOrEmpty(testList))
                    return true;
                var testItems = Strings.Split(testList, ",");
                foreach (var testItem in testItems)
                {
                    int p = Strings.InStr(testItem, "@");
                    if (p > 0)
                    {
                        string domain = Strings.Mid(testItem, p + 1);
                        foreach (MailAddress address in adressies)
                        {
                            if (DMD.Booleans.ValueOf(DMD.Strings.Compare(address.Host, domain, true)))
                                return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// Controlla il messaggio e restituisce true se la regola si applica al messaggio
            /// </summary>
            /// <param name="m"></param>
            /// <returns></returns>
            public override bool Check(MailMessage m)
            {
                bool ret = true;
                var toItems = new CCollection<MailAddress>();
                toItems.AddRange(m.To);
                ret = ret && CheckAddress(FromAddress, m.From);
                ret = ret && CheckAddresses(JustToAddress, toItems);
                toItems.AddRange(m.Cc);
                toItems.AddRange(m.Bcc);
                ret = ret && CheckAddresses(ToAddress, toItems);
                ret = ret && CheckDomains(FromDomain, m.From);
                ret = ret && CheckDomains(ToDomain, toItems);
                ret = ret && CheckAccount(AccountName, m.AccountName);
                ret = ret && CheckSubject(SubjectContains, m.Subject);
                ret = ret && CheckBody(SubjectContains, m.Body);
                return ret;
            }

            /// <summary>
            /// Esegue la regola
            /// </summary>
            /// <param name="m"></param>
            public override void Execute(MailMessage m)
            {
                foreach (MailRuleAction a in Actions)
                    a.Execute(m);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("FromAddress", FromAddress);
                writer.WriteAttribute("ToAddress", ToAddress);
                writer.WriteAttribute("JustToAddress", JustToAddress);
                writer.WriteAttribute("FromDomain", FromDomain);
                writer.WriteAttribute("ToDomain", ToDomain);
                writer.WriteAttribute("SendDateInterval", SendDateInterval);
                writer.WriteAttribute("SendDateFrom", SendDateFrom);
                writer.WriteAttribute("SendDateTo", SendDateTo);
                writer.WriteAttribute("ReceiveDateInterval", ReceiveDateInterval);
                writer.WriteAttribute("ReceiveDateFrom", ReceiveDateFrom);
                writer.WriteAttribute("ReceiveDateTo", ReceiveDateTo);
                writer.WriteAttribute("AccountName", AccountName);
                writer.WriteAttribute("SubjectContains", SubjectContains);
                base.XMLSerialize(writer);
                writer.WriteTag("Actions", Actions);
                writer.WriteTag("BodyContains", BodyContains);
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
                    case "FromAddress":
                        {
                            FromAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ToAddress":
                        {
                            ToAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "JustToAddress":
                        {
                            JustToAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FromDomain":
                        {
                            FromDomain = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ToDomain":
                        {
                            ToDomain = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SendDateInterval":
                        {
                            SendDateInterval = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SendDateFrom":
                        {
                            SendDateFrom = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "SendDateTo":
                        {
                            SendDateTo = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ReceiveDateInterval":
                        {
                            ReceiveDateInterval = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ReceiveDateFrom":
                        {
                            ReceiveDateFrom = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ReceiveDateTo":
                        {
                            ReceiveDateTo = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "AccountName":
                        {
                            AccountName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SubjectContains":
                        {
                            SubjectContains = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Actions":
                        {
                            Actions = (CCollection<MailRuleAction>)fieldValue;
                            break;
                        }

                    case "BodyContains":
                        {
                            BodyContains = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            private bool CheckAccount(string testAccount, string messageAccount)
            {
                testAccount = Strings.Trim(testAccount);
                messageAccount = Strings.Trim(messageAccount);
                return string.IsNullOrEmpty(testAccount) || DMD.Strings.Compare(testAccount, messageAccount, true) == 0;
            }

            private bool CheckSubject(string testSubject, string messageSubject)
            {
                testSubject = Strings.Trim(testSubject);
                messageSubject = Strings.Trim(messageSubject);
                return string.IsNullOrEmpty(testSubject) || Strings.InStr(messageSubject, testSubject) > 0;
            }

            private bool CheckBody(string testBody, string messageBody)
            {
                testBody = Strings.Trim(testBody);
                if (string.IsNullOrEmpty(testBody))
                    return true;
                var rows = Strings.Split(testBody, DMD.Strings.vbCrLf);
                foreach (string row in rows)
                {
                    if (!string.IsNullOrEmpty(row) && Strings.InStr(messageBody, row) > 0)
                        return true;
                }

                return false;
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed  override bool Equals(MailRuleBase obj)
            {
                return (obj is MailRule) && this.Equals((MailRule)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MailRule obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.FromAddress, obj.FromAddress)
                    && DMD.Strings.EQ(this.ToAddress, obj.ToAddress)
                    && DMD.Strings.EQ(this.JustToAddress, obj.JustToAddress)
                    && DMD.Strings.EQ(this.FromDomain, obj.FromDomain)
                    && DMD.Strings.EQ(this.ToDomain, obj.ToDomain)
                    && DMD.Strings.EQ(this.SendDateInterval, obj.SendDateInterval)
                    && DMD.DateUtils.EQ(this.SendDateFrom, obj.SendDateFrom)
                    && DMD.DateUtils.EQ(this.SendDateTo, obj.SendDateTo)
                    && DMD.Strings.EQ(this.ReceiveDateInterval, obj.ReceiveDateInterval)
                    && DMD.DateUtils.EQ(this.ReceiveDateFrom, obj.ReceiveDateFrom)
                    && DMD.DateUtils.EQ(this.ReceiveDateTo, obj.ReceiveDateTo)
                    && DMD.Strings.EQ(this.AccountName, obj.AccountName)
                    && DMD.Strings.EQ(this.BodyContains, obj.BodyContains)
                    && DMD.Strings.EQ(this.SubjectContains, obj.SubjectContains)
                    ;
            }
        }
    }
}