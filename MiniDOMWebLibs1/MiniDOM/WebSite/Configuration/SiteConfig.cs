using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {
        [Flags]
        public enum SiteFlags : int
        {
            NONE = 0,
            COMPRESS_RESPONSE = 1,
            LOG_SESSIONS = 2,
            LOG_PAGES = 4,
            LOG_QUERYSTRINGS = 8,
            LOG_POSTDATA = 16,
            NOTIFY_UNHAUTORIZED = 32,
            VERIFY_REMOTEIP = 64,
            VERIFY_TIMERESTRICTIONS = 256,
            VERIFY_CLIENTCERTIFICATE = 512,
            LOG_DBQUERIES = 1024
        }

        [Serializable]
        public class SiteConfig : Databases.DBObjectBase
        {
            private string m_SiteName;
            private string m_SiteDescription;
            private string m_SiteURL;
            private string m_InfoEMail;
            private string m_PartitaIVA;
            private string m_CodiceFiscale;
            private string m_Telefono;
            private string m_LogoURL;
            private string m_Note;
            private string m_SupportEMail;
            private string m_PublicURL;
            private string m_SimboloValuta;
            private int m_DecimaliPerValuta;
            private int m_DecimaliPerPercentuale;
            private SiteFlags m_Flags;
            private int m_NumberOfUploadsLimit;
            private int m_UploadSpeedLimit;
            private int m_UploadTimeOut;
            private int m_ShortTimeOut;
            private int m_LongTimeOut;
            private string m_KeyWords;
            private int m_UploadBufferSize;
            private int m_CRMMaxCacheSize;
            private float m_CRMUnloadFactor;
            private int m_DeleteLogFilesAfterNDays;

            public SiteConfig()
            {
                m_SiteName = "";
                m_SiteDescription = "";
                m_SiteURL = "";
                m_InfoEMail = "";
                m_PartitaIVA = "";
                m_CodiceFiscale = "";
                m_Telefono = "";
                m_LogoURL = "";
                m_Note = "";
                m_SupportEMail = "";
                m_PublicURL = "";
                m_SimboloValuta = "";
                m_DecimaliPerValuta = 2;
                m_DecimaliPerPercentuale = 2;
                m_Flags = SiteFlags.NONE;
                m_NumberOfUploadsLimit = -1;
                m_UploadSpeedLimit = 0;
                m_UploadTimeOut = 10 * 60;
                m_ShortTimeOut = 15;
                m_LongTimeOut = 120;
                m_KeyWords = "";
                m_UploadBufferSize = 1024 * 10;
                m_CRMMaxCacheSize = 100;
                m_CRMUnloadFactor = 0.25f;
                m_DeleteLogFilesAfterNDays = 30;
            }

            public string SiteName
            {
                get
                {
                    return m_SiteName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SiteName;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SiteName = value;
                    DoChanged("SiteName", value, oldValue);
                }
            }

            public string SiteDescription
            {
                get
                {
                    return m_SiteDescription;
                }

                set
                {
                    string oldValue = m_SiteDescription;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SiteDescription = value;
                    DoChanged("SiteDescription", value, oldValue);
                }
            }

            public string SiteURL
            {
                get
                {
                    return m_SiteURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SiteURL;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SiteURL = value;
                    DoChanged("SiteURL", value, oldValue);
                }
            }

            public string InfoEMail
            {
                get
                {
                    return m_InfoEMail;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_InfoEMail;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_InfoEMail = value;
                    DoChanged("InfoEMail", value, oldValue);
                }
            }

            public string PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_PartitaIVA;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_PartitaIVA = value;
                    DoChanged("PartitaIVA", value, oldValue);
                }
            }

            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceFiscale;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(value ?? "", oldValue ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            public string Telefono
            {
                get
                {
                    return m_Telefono;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Telefono;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Telefono = value;
                    DoChanged("Telefono", value, oldValue);
                }
            }

            public string LogoURL
            {
                get
                {
                    return m_LogoURL;
                }

                set
                {
                    string oldValue = m_LogoURL;
                    value = DMD.Strings.Trim(value);
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_LogoURL = value;
                    DoChanged("LogoURL", value, oldValue);
                }
            }

            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            public string SupportEMail
            {
                get
                {
                    return m_SupportEMail;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SupportEMail;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    DoChanged("SupportEMail", value, oldValue);
                }
            }

            public SiteFlags Flags
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
        /// Restituisce o imposta il numero di giorni per cui i files di log vengono mantenuti.
        /// Se viene impostato un valore negativo i files di log non vengono eliminati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int DeleteLogFilesAfterNDays
            {
                get
                {
                    return m_DeleteLogFilesAfterNDays;
                }

                set
                {
                    int oldValue = m_DeleteLogFilesAfterNDays;
                    if (oldValue == value)
                        return;
                    m_DeleteLogFilesAfterNDays = value;
                    DoChanged("DeleteLogFilesAfterNDays", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la dimensione massima dell'indice delle anagrafiche delle persone (mantenuto in memoria)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int CRMMaxCacheSize
            {
                get
                {
                    return m_CRMMaxCacheSize;
                }

                set
                {
                    int oldValue = m_CRMMaxCacheSize;
                    if (oldValue == value)
                        return;
                    m_CRMMaxCacheSize = value;
                    DoChanged("CRMMaxCacheSize", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la percentuale della dell'indice delle anagrafiche (in memoria) che viene scaricata quando si raggiunge la dimensione massima
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public float CRMUnloadFactor
            {
                get
                {
                    return m_CRMUnloadFactor;
                }

                set
                {
                    float oldValue = m_CRMUnloadFactor;
                    if (value == oldValue)
                        return;
                    m_CRMUnloadFactor = value;
                    DoChanged("CRMUnloadFactor", value, oldValue);
                }
            }

            /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int UploadBufferSize
            {
                get
                {
                    return m_UploadBufferSize;
                }

                set
                {
                    int oldValue = m_UploadBufferSize;
                    if (oldValue == value)
                        return;
                    m_UploadBufferSize = value;
                    DoChanged("UploadBufferSize", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisc o imposta il timeout in secondi per le richieste "veloci"
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ShortTimeOut
            {
                get
                {
                    return m_ShortTimeOut;
                }

                set
                {
                    int oldValue = m_ShortTimeOut;
                    if (oldValue == value)
                        return;
                    m_ShortTimeOut = value;
                    DoChanged("ShortTimeOut", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il timeout in secondi per le richieste "lente"
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int LongTimeOut
            {
                get
                {
                    return m_LongTimeOut;
                }

                set
                {
                    int oldValue = m_LongTimeOut;
                    if (oldValue == value)
                        return;
                    m_LongTimeOut = value;
                    DoChanged("LongTimeOut", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o impostai il tempo, in scondi, entro cui considerare fallito un upload
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int UploadTimeOut
            {
                get
                {
                    return m_UploadTimeOut;
                }

                set
                {
                    int oldValue = m_UploadTimeOut;
                    if (oldValue == value)
                        return;
                    m_UploadTimeOut = value;
                    DoChanged("UploadTimeOut ", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o impostai l numero massimo di upload contemporanei concessi per il server
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumberOfUploadsLimit
            {
                get
                {
                    return m_NumberOfUploadsLimit;
                }

                set
                {
                    int oldValue = m_NumberOfUploadsLimit;
                    if (oldValue == value)
                        return;
                    m_NumberOfUploadsLimit = value;
                    DoChanged("NumberOfUploadsLimit", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il limite massimo di bytes al secondo inviabili per un singolo upload
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int UploadSpeedLimit
            {
                get
                {
                    return m_UploadSpeedLimit;
                }

                set
                {
                    int oldValue = m_UploadSpeedLimit;
                    if (oldValue == value)
                        return;
                    m_UploadSpeedLimit = value;
                    DoChanged("UploadSpeedLimit", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'elenco delle parole chiave inserite nella pagina
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string KeyWords
            {
                get
                {
                    return m_KeyWords;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_KeyWords;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_KeyWords = value;
                    DoChanged("KeyWords", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il simbolo usato per la valuta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string SimboloValuta
            {
                get
                {
                    return m_SimboloValuta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SimboloValuta;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SimboloValuta = value;
                    DoChanged("SimboloValuta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di decimali usati per la valuta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int DecimaliPerValuta
            {
                get
                {
                    return m_DecimaliPerValuta;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("DecimaliPerValuta non può essere un valore negativo");
                    int oldValue = m_DecimaliPerValuta;
                    m_DecimaliPerValuta = value;
                    DoChanged("DecimaliPerValuta", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di decimali usati per formattare i valori decimali
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int DecimaliPerPercentuale
            {
                get
                {
                    return m_DecimaliPerPercentuale;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("DecimaliPerPercentuale non può essere un valore negativo");
                    int oldValue = m_DecimaliPerPercentuale;
                    if (oldValue == value)
                        return;
                    m_DecimaliPerPercentuale = value;
                    DoChanged("DecimaliPerPercentuale", value, oldValue);
                }
            }

            public override Sistema.CModule GetModule()
            {
                return Instance.Module;
            }

            /// <summary>
        /// Restituisce la URL del percorso temporaneo predefinito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TempFolder
            {
                get
                {
                    return PublicURL + "Temp/";
                }
            }

            /// <summary>
        /// Restituisce o imposta la url del percorso utilizzabile come cartella pubblica sul sito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PublicURL
            {
                get
                {
                    return m_PublicURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.Right(value, 1), "/", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                        value += "/";
                    string oldValue = m_PublicURL;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_PublicURL = value;
                    DoChanged("PublicURL", value, oldValue);
                }
            }


            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se comprimere i dati inviati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CompressResponse
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.COMPRESS_RESPONSE);
                }

                set
                {
                    bool oldValue = CompressResponse;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.COMPRESS_RESPONSE, value);
                    DoChanged("CompressResponse", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve effettuare il salvataggio di ogni sessione nel db di log
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool LogSessions
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.LOG_SESSIONS);
                }

                set
                {
                    bool oldValue = LogSessions;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.LOG_SESSIONS, value);
                    DoChanged("LogSessions", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se tutti i comandi inviati ai vari database devono essere registrati nel file di log
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool LogDBCommands
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.LOG_DBQUERIES);
                }

                set
                {
                    bool oldValue = LogDBCommands;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.LOG_DBQUERIES, value);
                    DoChanged("LogDBCommands", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve effettuare il salvataggio di ogni pagina nel db di log
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool LogPages
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.LOG_PAGES);
                }

                set
                {
                    bool oldValue = LogPages;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.LOG_PAGES, value);
                    DoChanged("LogPages", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve memorizzare la URL comprensiva dai dati GET quando effettua il log delle pagine
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool LogQueryStrings
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.LOG_QUERYSTRINGS);
                }

                set
                {
                    bool oldValue = LogQueryStrings;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.LOG_QUERYSTRINGS, value);
                    DoChanged("LogQueryStrings", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve memorizzare i dati di tipo POST quando effettua il log delle pagine
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool LogPostData
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.LOG_POSTDATA);
                }

                set
                {
                    bool oldValue = LogPostData;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.LOG_POSTDATA, value);
                    DoChanged("LogPostData", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve memorizzare i tentativi di accesso non validi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool NotifyUnhautorized
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.NOTIFY_UNHAUTORIZED);
                }

                set
                {
                    bool oldValue = NotifyUnhautorized;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.NOTIFY_UNHAUTORIZED, value);
                    DoChanged("NotifyUnhautorized", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve verificare gli accessi in base all'IP del client
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool VerifyRemoteIP
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.VERIFY_REMOTEIP);
                }

                set
                {
                    bool oldValue = VerifyRemoteIP;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.VERIFY_REMOTEIP, value);
                    DoChanged("VerifyRemoteIP", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve verificare gli accessi in base agli orari prestabiliti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool VerifyTimeRestrictions
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.VERIFY_TIMERESTRICTIONS);
                }

                set
                {
                    bool oldValue = VerifyTimeRestrictions;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.VERIFY_TIMERESTRICTIONS, value);
                    DoChanged("VerifyTimeRestrictions", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il sistema deve verificare il certificato client
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool VerifyClientCertificate
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, SiteFlags.VERIFY_CLIENTCERTIFICATE);
                }

                set
                {
                    bool oldValue = VerifyClientCertificate;
                    if (oldValue == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, SiteFlags.VERIFY_CLIENTCERTIFICATE, value);
                    DoChanged("VerifyClientCertificate", value, oldValue);
                }
            }

            public override string GetTableName()
            {
                return "tbl_SiteConfiguration";
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                m_SiteName = reader.Read("SiteName", ref m_SiteName);
                m_SiteDescription = reader.Read("SiteDescription", ref m_SiteDescription);
                m_SiteURL = reader.Read("SiteURL", ref m_SiteURL);
                m_InfoEMail = reader.Read("InfoEMail", ref m_InfoEMail);
                m_PartitaIVA = reader.Read("PartitaIVA", ref m_PartitaIVA);
                m_CodiceFiscale = reader.Read("CodiceFiscale", ref m_CodiceFiscale);
                m_Telefono = reader.Read("Telefono", ref m_Telefono);
                m_LogoURL = reader.Read("LogoURL", ref m_LogoURL);
                m_Note = reader.Read("Note", ref m_Note);
                m_PublicURL = reader.Read("PublicFolder", ref m_PublicURL);
                // m_ExternalDB = d bRis("ExternalDB") 
                m_SupportEMail = reader.Read("SupportEMail", ref m_SupportEMail);
                m_SimboloValuta = reader.Read("SimboloValuta", ref m_SimboloValuta);
                m_DecimaliPerValuta = reader.Read("DecimaliPerValuta", ref m_DecimaliPerValuta);
                m_DecimaliPerPercentuale = reader.Read("DecimaliPerPercentuale", ref m_DecimaliPerPercentuale);
                m_Flags = reader.Read("Flags", ref m_Flags);
                m_UploadSpeedLimit = reader.Read("UploadSpeedLimit", ref m_UploadSpeedLimit);
                m_NumberOfUploadsLimit = reader.Read("NumberOfUploadsLimit", ref m_NumberOfUploadsLimit);
                m_UploadTimeOut = reader.Read("UploadTimeOut", ref m_UploadTimeOut);
                m_ShortTimeOut = reader.Read("ShortTimeOut", ref m_ShortTimeOut);
                m_LongTimeOut = reader.Read("LongTimeOut", ref m_LongTimeOut);
                m_KeyWords = reader.Read("KeyWords", ref m_KeyWords);
                m_UploadBufferSize = reader.Read("UploadBufferSize", ref m_UploadBufferSize);
                m_CRMMaxCacheSize = reader.Read("CRMMaxCacheSize", ref m_CRMMaxCacheSize);
                m_CRMUnloadFactor = reader.Read("CRMUnloadFactor", ref m_CRMUnloadFactor);
                m_DeleteLogFilesAfterNDays = reader.Read("DelLogNDays", ref m_DeleteLogFilesAfterNDays);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("SiteName", m_SiteName);
                writer.Write("SiteDescription", m_SiteDescription);
                writer.Write("SiteURL", m_SiteURL);
                writer.Write("InfoEMail", m_InfoEMail);
                writer.Write("PartitaIVA", m_PartitaIVA);
                writer.Write("CodiceFiscale", m_CodiceFiscale);
                writer.Write("Telefono", m_Telefono);
                writer.Write("LogoURL", m_LogoURL);
                writer.Write("Note", m_Note);
                // dbRis("ExternalDB") = m_ExternalDB
                writer.Write("SupportEMail", m_SupportEMail);
                writer.Write("PublicFolder", m_PublicURL);
                writer.Write("SimboloValuta", m_SimboloValuta);
                writer.Write("DecimaliPerValuta", m_DecimaliPerValuta);
                writer.Write("DecimaliPerPercentuale", m_DecimaliPerPercentuale);
                writer.Write("Flags", m_Flags);
                writer.Write("UploadSpeedLimit", m_UploadSpeedLimit);
                writer.Write("NumberOfUploadsLimit", m_NumberOfUploadsLimit);
                writer.Write("UploadTimeOut", m_UploadTimeOut);
                writer.Write("ShortTimeOut", m_ShortTimeOut);
                writer.Write("LongTimeOut", m_LongTimeOut);
                writer.Write("KeyWords", m_KeyWords);
                writer.Write("UploadBufferSize", m_UploadBufferSize);
                writer.Write("CRMMaxCacheSize", m_CRMMaxCacheSize);
                writer.Write("CRMUnloadFactor", m_CRMUnloadFactor);
                writer.Write("DelLogNDays", m_DeleteLogFilesAfterNDays);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XML.XMLWriter writer)
            {
                writer.WriteAttribute("SiteURL", m_SiteURL);
                writer.WriteAttribute("InfoEMail", m_InfoEMail);
                writer.WriteAttribute("PartitaIVA", m_PartitaIVA);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                writer.WriteAttribute("Telefono", m_Telefono);
                writer.WriteAttribute("LogoURL", m_LogoURL);
                writer.WriteAttribute("SupportEMail", m_SupportEMail);
                writer.WriteAttribute("PublicFolder", m_PublicURL);
                writer.WriteAttribute("SimboloValuta", m_SimboloValuta);
                writer.WriteAttribute("DecimaliPerValuta", m_DecimaliPerValuta);
                writer.WriteAttribute("DecimaliPerPercentuale", m_DecimaliPerPercentuale);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("SiteName", m_SiteName);
                writer.WriteAttribute("UploadSpeedLimit", m_UploadSpeedLimit);
                writer.WriteAttribute("NumberOfUploadsLimit", m_NumberOfUploadsLimit);
                writer.WriteAttribute("UploadTimeOut", m_UploadTimeOut);
                writer.WriteAttribute("ShortTimeOut", m_ShortTimeOut);
                writer.WriteAttribute("LongTimeOut", m_LongTimeOut);
                writer.WriteAttribute("UploadBufferSize", m_UploadBufferSize);
                writer.WriteAttribute("CRMMaxCacheSize", m_CRMMaxCacheSize);
                writer.WriteAttribute("CRMUnloadFactor", m_CRMUnloadFactor);
                writer.WriteAttribute("DelLogNDays", m_DeleteLogFilesAfterNDays);
                base.XMLSerialize(writer);
                writer.WriteTag("SiteDescription", m_SiteDescription);
                writer.WriteTag("Note", m_Note);
                writer.WriteTag("KeyWords", m_KeyWords);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "SiteName", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SiteName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "SiteDescription", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SiteDescription = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "SiteURL", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SiteURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "InfoEMail", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_InfoEMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "PartitaIVA", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_PartitaIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "CodiceFiscale", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "Telefono", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Telefono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case7 when CultureInfo.CurrentCulture.CompareInfo.Compare(case7, "LogoURL", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_LogoURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case8 when CultureInfo.CurrentCulture.CompareInfo.Compare(case8, "Note", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // dbRis("ExternalDB") = m_ExternalDB
                    case var case9 when CultureInfo.CurrentCulture.CompareInfo.Compare(case9, "SupportEMail", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SupportEMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case10 when CultureInfo.CurrentCulture.CompareInfo.Compare(case10, "PublicFolder", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_PublicURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case11 when CultureInfo.CurrentCulture.CompareInfo.Compare(case11, "SimboloValuta", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SimboloValuta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case12 when CultureInfo.CurrentCulture.CompareInfo.Compare(case12, "DecimaliPerValuta", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_DecimaliPerValuta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case13 when CultureInfo.CurrentCulture.CompareInfo.Compare(case13, "DecimaliPerPercentuale", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_DecimaliPerPercentuale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case14 when CultureInfo.CurrentCulture.CompareInfo.Compare(case14, "Flags", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Flags = (SiteFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case15 when CultureInfo.CurrentCulture.CompareInfo.Compare(case15, "UploadSpeedLimit", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UploadSpeedLimit = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case16 when CultureInfo.CurrentCulture.CompareInfo.Compare(case16, "NumberOfUploadsLimit", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_NumberOfUploadsLimit = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case17 when CultureInfo.CurrentCulture.CompareInfo.Compare(case17, "UploadTimeOut", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UploadTimeOut = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case18 when CultureInfo.CurrentCulture.CompareInfo.Compare(case18, "ShortTimeOut", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_ShortTimeOut = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case19 when CultureInfo.CurrentCulture.CompareInfo.Compare(case19, "LongTimeOut", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_LongTimeOut = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case20 when CultureInfo.CurrentCulture.CompareInfo.Compare(case20, "KeyWords", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_KeyWords = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case21 when CultureInfo.CurrentCulture.CompareInfo.Compare(case21, "UploadBufferSize", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UploadBufferSize = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case22 when CultureInfo.CurrentCulture.CompareInfo.Compare(case22, "CRMMaxCacheSize", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_CRMMaxCacheSize = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case23 when CultureInfo.CurrentCulture.CompareInfo.Compare(case23, "CRMUnloadFactor", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_CRMUnloadFactor = (float)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case var case24 when CultureInfo.CurrentCulture.CompareInfo.Compare(case24, "DelLogNDays", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_DeleteLogFilesAfterNDays = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public bool Load()
            {
                if (Databases.APPConn is null || !Databases.APPConn.IsOpen())
                    return false;
                string dbSQL = "SELECT * FROM [tbl_SiteConfiguration] ORDER BY [ID] ASC";
                var reader = new Databases.DBReader(Databases.APPConn.Tables["tbl_SiteConfiguration"], dbSQL);
                if (reader.Read())
                    Databases.APPConn.Load(this, reader);
                reader.Dispose();
                return true;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret)
                {
                    Instance.SetConfiguration(this);
                }

                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            // Function LoadFromCollection(col, baseName)
            // m_ID = Formats.ToInteger("&H" & col(baseName & "ID"))
            // m_SiteName = "" & col(baseName & "SiteName")
            // m_SiteDescription = "" & col(baseName & "SiteDescription")
            // m_SiteURL = "" & col(baseName & "SiteURL")
            // m_InfoEMail = "" & col(baseName & "InfoEMail")
            // m_PartitaIVA = "" & col(baseName & "PartitaIVA")
            // m_CodiceFiscale = "" & col(baseName & "CodiceFiscale")
            // m_Telefono = "" & col(baseName & "Telefono")
            // m_LogoURL = "" & col(baseName & "LogoURL")
            // m_Note = "" & col(baseName & "Note")
            // m_ExternalDB = "" & col(baseName & "ExternalDB")
            // m_SupportEMail = "" & col(baseName & "SupportEMail")
            // LoadFromCollection = True
            // End Function

            // Function SaveToCollection(col, baseName)
            // col(baseName & "ID") = Hex(Formats.ToInteger(m_ID))
            // col(baseName & "SiteName") = "" & m_SiteName
            // col(baseName & "SiteDescription") = "" & m_SiteDescription
            // col(baseName & "SiteURL") = "" & m_SiteURL
            // col(baseName & "InfoEMail") = "" & m_InfoEMail
            // col(baseName & "PartitaIVA") = "" & m_PartitaIVA
            // col(baseName & "CodiceFiscale") = "" & m_CodiceFiscale
            // col(baseName & "Telefono") = "" & m_Telefono
            // col(baseName & "LogoURL") = "" & m_LogoURL
            // col(baseName & "Note") = "" & m_Note
            // col(baseName & "ExternalDB") = "" & m_ExternalDB
            // col(baseName & "SupportEMail") = "" & m_SupportEMail
            // SaveToCollection = True
            // End Function

            protected override void DoChanged(string propName, object newVal = null, object oldVal = null)
            {
                base.DoChanged(propName, newVal, oldVal);
                Instance.doConfigChanged(new EventArgs());
            }
        }
    }
}