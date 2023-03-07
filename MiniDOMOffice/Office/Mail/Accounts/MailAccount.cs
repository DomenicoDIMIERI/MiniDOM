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
        /// Flag per un account mail
        /// </summary>
        public enum SMTPTipoCrittografica : int
        {
            /// <summary>
            /// Nesusno
            /// </summary>
            Nessuna = 0,

            /// <summary>
            /// Connessione ssl
            /// </summary>
            SSL = 1,

            /// <summary>
            /// Connessione tsl
            /// </summary>
            TLS = 2,

            /// <summary>
            /// Crittografia automatica
            /// </summary>
            Automatica = 3
        }

        /// <summary>
        /// Rappresenta un account di posta elettronica configurato sul sistema
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class MailAccount
            : minidom.Databases.DBObjectPO
        {
            private const int DELAFTERRECYCLECLEAN = 1;
            private const int DELAFTERRECYCLE = 2;
            private const int DELAFTERNDAYS = 3;

            // Parametri per la ricezione
            private string m_AccountName;
            private int m_DefaultFolderID;
            [NonSerialized] private MailFolder m_DefaultFolder;
            private string m_UserName;
            private string m_Password;
            private string m_ServerName;
            private int m_ServerPort;                 // Porta per la ricezione
            private string m_eMailAddress;                // Indirizzo email del mittente
            private string m_Protocol;                    // Nome del protocollo (POP3 o IMPAP)
            private bool m_UseSSL;                     // Se vero la connessione avviene su Secure Socket Layer
            private string m_SMTPServerName;              // Nome del server smtp (per l'invio)
            private int m_SMTPPort;                   // Porta del server smpt (per la ricezione)
            private string m_ReplayTo;                    // Indirizzo usato per le risposte
            private string m_DisplayName;                 // Nome del mittente visualizzato dai destinatari
            private string m_SMTPUserName;                // UserName utilizzato per l'invio
            private string m_SMTPPassword;                // Password utilizzata per l'invio
            private bool m_PopBeforeSMPT;              // Se vero il programma effettua prima l'accesso al server POP3
            private int m_DelServerAfterDays;         // Rimuove le email dal server dopo N giorni da quando sono stato scaricate
            private int m_TimeOut;
            private SMTPTipoCrittografica m_SMTPCrittografia;
            private DateTime? m_LastSync;
            private string m_FirmaPerNuoviMessaggi;
            private string m_FirmaPerRisposte;
            private int m_ApplicationID;
            [NonSerialized] private MailApplication m_Application;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAccount()
            {
                m_AccountName = "";
                m_DefaultFolder = null;
                m_DefaultFolderID = 0;
                m_UserName = "";
                m_Password = "";
                m_ServerName = "";
                m_ServerPort = 110;
                m_eMailAddress = "";
                m_Protocol = "POP3";
                m_UseSSL = false;
                m_SMTPServerName = "127.0.0.1";
                m_SMTPPort = 25;
                m_ReplayTo = "";
                m_DisplayName = "";
                m_SMTPUserName = "";
                m_SMTPPassword = "";
                m_PopBeforeSMPT = false;
                m_Flags = 0;
                m_DelServerAfterDays = 0;
                m_TimeOut = 120;
                m_SMTPCrittografia = SMTPTipoCrittografica.Nessuna;
                m_LastSync = default;
                m_FirmaPerNuoviMessaggi = "";
                m_FirmaPerRisposte = "";
                m_ApplicationID = 0;
                m_Application = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="host"></param>
            /// <param name="userName"></param>
            /// <param name="password"></param>
            public MailAccount(string host, string userName, string password) 
                : this()
            {
                m_ServerName = host;
                m_UserName = userName;
                m_Password = password;
            }

            /// <summary>
            /// Cartella predefinita in cui le email ricevute vengono salvate
            /// </summary>
            public MailFolder DefaultFolder
            {
                get
                {
                    if (m_DefaultFolder is null)
                    {
                        if (Application is object)
                        {
                            m_DefaultFolder = Application.GetFolderById(m_DefaultFolderID);
                            if (m_DefaultFolder is null)
                                m_DefaultFolder = Application.Root.Inbox;
                        }
                    }

                    return m_DefaultFolder;
                }

                set
                {
                    var oldValue = DefaultFolder;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_DefaultFolder = value;
                    m_DefaultFolderID = DBUtils.GetID(value, 0);
                    DoChanged("DefaultFolder", value, oldValue);
                }
            }

            /// <summary>
            /// ID della cartella predefinita
            /// </summary>
            public int DefaultFolderID
            {
                get
                {
                    return DBUtils.GetID(m_DefaultFolder, m_DefaultFolderID);
                }

                set
                {
                    int oldValue = DefaultFolderID;
                    if (oldValue == value)
                        return;
                    m_DefaultFolderID = value;
                    m_DefaultFolder = null;
                    DoChanged("DefaultFolderID", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'applicazione 
            /// </summary>
            public int ApplicationID
            {
                get
                {
                    return DBUtils.GetID(m_Application, m_ApplicationID);
                }

                set
                {
                    int oldValue = ApplicationID;
                    if (oldValue == value)
                        return;
                    m_ApplicationID = value;
                    m_Application = null;
                    DoChanged("ApplicationID", value, oldValue);
                }
            }

            /// <summary>
            /// Applicazione
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    if (m_Application is null)
                        m_Application = Mails.Applications.GetItemById(m_ApplicationID);
                    return m_Application;
                }

                set
                {
                    var oldValue = Application;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Application = value;
                    m_ApplicationID = DBUtils.GetID(value, 0);
                    DoChanged("Application", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'applicazione
            /// </summary>
            /// <param name="app"></param>
            protected internal void SetApplication(MailApplication app)
            {
                m_Application = app;
                m_ApplicationID = DBUtils.GetID(app, 0);
            }

            /// <summary>
            /// Restituisce o imposta la firma da inserire in coda ai nuovi messaggi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FirmaPerNuoviMessaggi
            {
                get
                {
                    return m_FirmaPerNuoviMessaggi;
                }

                set
                {
                    string oldValue = m_FirmaPerNuoviMessaggi;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FirmaPerNuoviMessaggi = value;
                    DoChanged("FirmaPerNuoviMessaggi", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la firma da inserire in coda ai messaggi inviati come risposta o inoltrati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FirmaPerRisposte
            {
                get
                {
                    return m_FirmaPerRisposte;
                }

                set
                {
                    string oldValue = m_FirmaPerRisposte;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FirmaPerRisposte = value;
                    DoChanged("FirmaPerRisposte", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta la data dell'ultima sincronizzazione completa avvenuta con il server
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? LastStync
            {
                get
                {
                    return m_LastSync;
                }

                set
                {
                    var oldValue = m_LastSync;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_LastSync = value;
                    DoChanged("LastSync", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di crittografia utilizzato dal server della posta in uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public SMTPTipoCrittografica SMTPCrittografia
            {
                get
                {
                    return m_SMTPCrittografia;
                }

                set
                {
                    var oldValue = m_SMTPCrittografia;
                    if (oldValue == value)
                        return;
                    m_SMTPCrittografia = value;
                    DoChanged("SMTPCrittografia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tempo massimo di attesa per scaricare i messaggi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int TimeOut
            {
                get
                {
                    return m_TimeOut;
                }

                set
                {
                    int oldValue = m_TimeOut;
                    if (oldValue == value)
                        return;
                    m_TimeOut = value;
                    DoChanged("TimeOut", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AccountName
            {
                get
                {
                    return m_AccountName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_AccountName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AccountName = value;
                    DoChanged("AccountName", value, oldValue);
                }
            }


            /// <summary>
            /// Restitusice o imposta la username utilizzata per accedere al server della posta in arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UserName
            {
                get
                {
                    return m_UserName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_UserName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UserName = value;
                    DoChanged("UserName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la password utilizzata per accedere al server della posta in arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Password
            {
                get
                {
                    return m_Password;
                }

                set
                {
                    string oldValue = m_Password;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Password = value;
                    DoChanged("Password", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del server della posta in arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ServerName
            {
                get
                {
                    return m_ServerName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_ServerName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ServerName = value;
                    DoChanged("ServerName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero della porta per la connessione al server della posta in arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ServerPort
            {
                get
                {
                    return m_ServerPort;
                }

                set
                {
                    int oldValue = m_ServerPort;
                    if (oldValue == value)
                        return;
                    m_ServerPort = value;
                    DoChanged("ServerPort", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo di posta elettronica gestito da questo account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string eMailAddress
            {
                get
                {
                    return m_eMailAddress;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_eMailAddress;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_eMailAddress = value;
                    DoChanged("eMailAddress", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del protocollo utilizzate per ricevere le email (POP3 o IMAP)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Protocol
            {
                get
                {
                    return m_Protocol;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Protocol;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Protocol = value;
                    DoChanged("Protocol", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la connessione al server della posta in arrivo deve avvenire su SSL
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool UseSSL
            {
                get
                {
                    return m_UseSSL;
                }

                set
                {
                    if (m_UseSSL == value)
                        return;
                    m_UseSSL = value;
                    DoChanged("UseSSL", value, !value);
                }
            }


            /// <summary>
            /// Restituisce o imposta il nome visualizzato per le email inviate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DisplayName
            {
                get
                {
                    return m_DisplayName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_DisplayName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DisplayName = value;
                    DoChanged("DisplayName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del server per la posta in uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SMTPServerName
            {
                get
                {
                    return m_SMTPServerName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SMTPServerName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SMTPServerName = value;
                    DoChanged("SMTPServerName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la porta per la connessione al server della posta in uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int SMTPPort
            {
                get
                {
                    return m_SMTPPort;
                }

                set
                {
                    int oldValue = m_SMTPPort;
                    if (oldValue == value)
                        return;
                    m_SMTPPort = value;
                    DoChanged("SMTPPort", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo per le risposte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ReplayTo
            {
                get
                {
                    return m_ReplayTo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_ReplayTo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ReplayTo = value;
                    DoChanged("ReplayTo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome utente per il server della posta in uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SMTPUserName
            {
                get
                {
                    return m_SMTPUserName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SMTPUserName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SMTPUserName = value;
                    DoChanged("SMTPUserName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la password per l'accesso al server della posta in uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SMTPPassword
            {
                get
                {
                    return m_SMTPPassword;
                }

                set
                {
                    string oldValue = m_SMTPPassword;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SMTPPassword = value;
                    DoChanged("STMPPassword", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se accedere al server SMTP solo dopo aver effettuato il login su POP
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool POPBeforeSMTP
            {
                get
                {
                    return m_PopBeforeSMPT;
                }

                set
                {
                    if (m_PopBeforeSMPT == value)
                        return;
                    m_PopBeforeSMPT = value;
                    DoChanged("POPBeforeSMTP", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica se le email devono essere eliminate dal server dopo aver svuotato il cestino
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool DelServerAfterRecycleClean
            {
                get
                {
                    return (m_Flags & DELAFTERRECYCLECLEAN) == DELAFTERRECYCLECLEAN;
                }

                set
                {
                    if (DelServerAfterRecycleClean == value)
                        return;
                    if (value)
                    {
                        m_Flags = m_Flags | DELAFTERRECYCLECLEAN;
                    }
                    else
                    {
                        m_Flags = m_Flags & ~DELAFTERRECYCLECLEAN;
                    }

                    DoChanged("DelServerAfterRecycleClean", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica se le email devono essere eliminate dal server dopo averle cestinate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool DelServerAfterRecycle
            {
                get
                {
                    return (m_Flags & DELAFTERRECYCLE) == DELAFTERRECYCLE;
                }

                set
                {
                    if (DelServerAfterRecycle == value)
                        return;
                    if (value)
                    {
                        m_Flags = m_Flags | DELAFTERRECYCLE;
                    }
                    else
                    {
                        m_Flags = m_Flags & ~DELAFTERRECYCLE;
                    }

                    DoChanged("DelServerAfterRecycle", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica se le email devono essere eliminate dopo N giorni
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool DelServerAfterNDays
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, DELAFTERNDAYS);
                }

                set
                {
                    if (DelServerAfterNDays == value)
                        return;
                    if (value)
                    {
                        m_Flags = m_Flags | DELAFTERNDAYS;
                    }
                    else
                    {
                        m_Flags = m_Flags & ~DELAFTERNDAYS;
                    }

                    DoChanged("DelServerAfterNDays", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di giorni dalla data di scaricamento per eliminare le email dal server.
            /// 0 indica che le email non verranno eliminate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int DelServerAfterDays
            {
                get
                {
                    return m_DelServerAfterDays;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("value");
                    int oldValue = m_DelServerAfterDays;
                    if (oldValue == value)
                        return;
                    m_DelServerAfterDays = value;
                    DoChanged("DelServerAfterDays", value, oldValue);
                }
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                var app = this.Application;
                if (app is object)
                {
                    var old = app.Accounts.GetItemById(this.ID);
                    if (this.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        if (old is null)
                        {
                            app.Accounts.Add(this);
                        }
                        else if (!object.ReferenceEquals(this, old))
                        {
                            app.Accounts.Remove(old);
                            app.Accounts.Add(this);
                        }
                    }
                    else
                    {
                        if (old is object)
                        {
                            app.Accounts.Remove(old);
                        }
                    }
                }

                     
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_EmailAccounts";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_AccountName = reader.Read("AccountName", m_AccountName);
                m_DefaultFolderID = reader.Read("DefaultFolderID", m_DefaultFolderID);
                m_UserName = reader.Read("UserName", m_UserName);
                m_Password = reader.Read("Password", m_Password);
                m_ServerName = reader.Read("ServerName", m_ServerName);
                m_ServerPort = reader.Read("ServerPort", m_ServerPort);
                m_eMailAddress = reader.Read("eMailAddress", m_eMailAddress);
                m_Protocol = reader.Read("Protocol", m_Protocol);
                m_UseSSL = reader.Read("UseSSL", m_UseSSL);
                m_SMTPServerName = reader.Read("SMTPServerName", m_SMTPServerName);
                m_SMTPPort = reader.Read("SMTPPort", m_SMTPPort);
                m_ReplayTo = reader.Read("ReplayTo", m_ReplayTo);
                m_DisplayName = reader.Read("DisplayName", m_DisplayName);
                m_SMTPUserName = reader.Read("SMTPUserName", m_SMTPUserName);
                m_SMTPPassword = reader.Read("SMTPPassword", m_SMTPPassword);
                m_PopBeforeSMPT = reader.Read("PopBeforeSMTP", m_PopBeforeSMPT);
                m_DelServerAfterDays = reader.Read("DelAfterDays", m_DelServerAfterDays);
                m_TimeOut = reader.Read("TimeOut", m_TimeOut);
                m_LastSync = reader.Read("LastSync", m_LastSync);
                m_FirmaPerNuoviMessaggi = reader.Read("FirmaPerNuoviMessaggi", m_FirmaPerNuoviMessaggi);
                m_FirmaPerRisposte = reader.Read("FirmaPerRisposte", m_FirmaPerRisposte);
                m_ApplicationID = reader.Read("ApplicationID", m_ApplicationID);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("AccountName", m_AccountName);
                writer.Write("DefaultFolderID", DefaultFolderID);
                writer.Write("UserName", m_UserName);
                writer.Write("Password", m_Password);
                writer.Write("ServerName", m_ServerName);
                writer.Write("ServerPort", m_ServerPort);
                writer.Write("eMailAddress", m_eMailAddress);
                writer.Write("Protocol", m_Protocol);
                writer.Write("UseSSL", m_UseSSL);
                writer.Write("SMTPServerName", m_SMTPServerName);
                writer.Write("SMTPPort", m_SMTPPort);
                writer.Write("ReplayTo", m_ReplayTo);
                writer.Write("DisplayName", m_DisplayName);
                writer.Write("SMTPUserName", m_SMTPUserName);
                writer.Write("SMTPPassword", m_SMTPPassword);
                writer.Write("PopBeforeSMTP", m_PopBeforeSMPT);
                writer.Write("Flags", m_Flags);
                writer.Write("DelAfterDays", m_DelServerAfterDays);
                writer.Write("TimeOut", m_TimeOut);
                writer.Write("LastSync", m_LastSync);
                writer.Write("FirmaPerNuoviMessaggi", m_FirmaPerNuoviMessaggi);
                writer.Write("FirmaPerRisposte", m_FirmaPerRisposte);
                writer.Write("ApplicationID", ApplicationID);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Preapra lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("AccountName", typeof(string), 255);
                c = table.Fields.Ensure("DefaultFolderID", typeof(int), 1);
                c = table.Fields.Ensure("UserName", typeof(string), 255);
                c = table.Fields.Ensure("Password", typeof(string), 255);
                c = table.Fields.Ensure("ServerName", typeof(string), 255);
                c = table.Fields.Ensure("ServerPort", typeof(int), 1);
                c = table.Fields.Ensure("eMailAddress", typeof(string), 255);
                c = table.Fields.Ensure("Protocol", typeof(string), 255);
                c = table.Fields.Ensure("UseSSL", typeof(bool), 1);
                c = table.Fields.Ensure("SMTPServerName", typeof(string), 255);
                c = table.Fields.Ensure("SMTPPort", typeof(int), 1);
                c = table.Fields.Ensure("ReplayTo", typeof(string), 255);
                c = table.Fields.Ensure("DisplayName", typeof(string), 255);
                c = table.Fields.Ensure("SMTPUserName", typeof(string), 255);
                c = table.Fields.Ensure("SMTPPassword", typeof(string), 255);
                c = table.Fields.Ensure("PopBeforeSMTP", typeof(bool), 1);
                c = table.Fields.Ensure("DelAfterDays", typeof(int), 1);
                c = table.Fields.Ensure("TimeOut", typeof(int), 1);
                c = table.Fields.Ensure("LastSync", typeof(DateTime), 1);
                c = table.Fields.Ensure("FirmaPerNuoviMessaggi", typeof(string), 0);
                c = table.Fields.Ensure("FirmaPerRisposte", typeof(string), 0);
                c = table.Fields.Ensure("ApplicationID", typeof(int), 1);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxAccount", new string[] { "ApplicationID", "AccountName", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxFolder", new string[] { "DefaultFolderID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPOP", new string[] { "UserName", "ServerName", "ServerPort", "Protocol", "UseSSL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSMTP", new string[] { "SMTPServerName", "SMTPPort", "SMTPUserName", "PopBeforeSMTP" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxEMAIL", new string[] { "eMailAddress", "ReplayTo", "DisplayName"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParams", new string[] { "DelAfterDays", "TimeOut", "LastSync" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFirme", new string[] { "FirmaPerNuoviMessaggi", "FirmaPerRisposte" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Password", typeof(string), 255);
                //c = table.Fields.Ensure("SMTPPassword", typeof(string), 255);
                  

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("AccountName", m_AccountName);
                writer.WriteAttribute("DefaultFolderID", DefaultFolderID);
                writer.WriteAttribute("UserName", m_UserName);
                writer.WriteAttribute("Password", m_Password);
                writer.WriteAttribute("ServerName", m_ServerName);
                writer.WriteAttribute("ServerPort", m_ServerPort);
                writer.WriteAttribute("eMailAddress", m_eMailAddress);
                writer.WriteAttribute("Protocol", m_Protocol);
                writer.WriteAttribute("UseSSL", m_UseSSL);
                writer.WriteAttribute("SMTPServerName", m_SMTPServerName);
                writer.WriteAttribute("SMTPPort", m_SMTPPort);
                writer.WriteAttribute("ReplayTo", m_ReplayTo);
                writer.WriteAttribute("DisplayName", m_DisplayName);
                writer.WriteAttribute("SMTPUserName", m_SMTPUserName);
                writer.WriteAttribute("SMTPPassword", m_SMTPPassword);
                writer.WriteAttribute("PopBeforeSMTP", m_PopBeforeSMPT);
                writer.WriteAttribute("DelAfterDays", m_DelServerAfterDays);
                writer.WriteAttribute("TimeOut", m_TimeOut);
                writer.WriteAttribute("LastSync", m_LastSync);
                writer.WriteAttribute("ApplicationID", ApplicationID);
                base.XMLSerialize(writer);
                writer.WriteTag("FirmaPerNuoviMessaggi", m_FirmaPerNuoviMessaggi);
                writer.WriteTag("FirmaPerRisposte", m_FirmaPerRisposte);
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
                    case "AccountName":
                        {
                            m_AccountName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DefaultFolderID":
                        {
                            m_DefaultFolderID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Password":
                        {
                            m_Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ServerName":
                        {
                            m_ServerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ServerPort":
                        {
                            m_ServerPort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "eMailAddress":
                        {
                            m_eMailAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Protocol":
                        {
                            m_Protocol = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UseSSL":
                        {
                            m_UseSSL = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "SMTPServerName":
                        {
                            m_SMTPServerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SMTPPort":
                        {
                            m_SMTPPort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ReplayTo":
                        {
                            m_ReplayTo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DisplayName":
                        {
                            m_DisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SMTPUserName":
                        {
                            m_SMTPUserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SMTPPassword":
                        {
                            m_SMTPPassword = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PopBeforeSMTP":
                        {
                            m_PopBeforeSMPT = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }
 

                    case "DelAfterDays":
                        {
                            m_DelServerAfterDays = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TimeOut":
                        {
                            m_TimeOut = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SMTPCrittografia":
                        {
                            m_SMTPCrittografia = (SMTPTipoCrittografica)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "LastSync":
                        {
                            m_LastSync = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "FirmaPerNuoviMessaggi":
                        {
                            m_FirmaPerNuoviMessaggi = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FirmaPerRisposte":
                        {
                            m_FirmaPerRisposte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ApplicationID":
                        {
                            m_ApplicationID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Accounts;
            }


            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_AccountName;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_AccountName);
            }

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is MailAccount) && this.Equals((MailAccount)obj);
            }

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MailAccount obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_AccountName, obj.m_AccountName)
                    && DMD.Integers.EQ(this.m_DefaultFolderID, obj.m_DefaultFolderID)
                    && DMD.Strings.EQ(this.m_UserName, obj.m_UserName)
                    && DMD.Strings.EQ(this.m_Password, obj.m_Password)
                    && DMD.Strings.EQ(this.m_ServerName, obj.m_ServerName)
                    && DMD.Integers.EQ(this.m_ServerPort, obj.m_ServerPort)
                    && DMD.Strings.EQ(this.m_eMailAddress, obj.m_eMailAddress)
                    && DMD.Strings.EQ(this.m_Protocol, obj.m_Protocol)
                    && DMD.Booleans.EQ(this.m_UseSSL, obj.m_UseSSL)
                    && DMD.Strings.EQ(this.m_SMTPServerName, obj.m_SMTPServerName)
                    && DMD.Integers.EQ(this.m_SMTPPort, obj.m_SMTPPort)
                    && DMD.Strings.EQ(this.m_ReplayTo, obj.m_ReplayTo)
                    && DMD.Strings.EQ(this.m_DisplayName, obj.m_DisplayName)
                    && DMD.Strings.EQ(this.m_SMTPUserName, obj.m_SMTPUserName)
                    && DMD.Strings.EQ(this.m_SMTPPassword, obj.m_SMTPPassword)
                    && DMD.Booleans.EQ(this.m_PopBeforeSMPT, obj.m_PopBeforeSMPT)
                    && DMD.Integers.EQ(this.m_DelServerAfterDays, obj.m_DelServerAfterDays)
                    && DMD.Integers.EQ(this.m_TimeOut, obj.m_TimeOut)
                    && DMD.RunTime.EQ(this.m_SMTPCrittografia, obj.m_SMTPCrittografia)
                    && DMD.DateUtils.EQ(this.m_LastSync, obj.m_LastSync)
                    && DMD.Strings.EQ(this.m_FirmaPerNuoviMessaggi, obj.m_FirmaPerNuoviMessaggi)
                    && DMD.Strings.EQ(this.m_FirmaPerRisposte, obj.m_FirmaPerRisposte)
                    && DMD.Integers.EQ(this.m_ApplicationID, obj.m_ApplicationID)
                    ;
            }   
        }
    }
}