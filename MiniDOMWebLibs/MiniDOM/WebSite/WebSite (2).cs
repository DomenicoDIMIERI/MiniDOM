using System;
using System.Globalization;
using DMD;

namespace minidom
{
    public sealed partial class WebSite
    {
        // Inherits CModulesClass(Of DBObjectBase)

        /// <summary>
    /// Evento generato quando viene modificata la configurazione base del sito
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
        public event ConfigurationChangedEventHandler ConfigurationChanged;

        public delegate void ConfigurationChangedEventHandler(object sender, EventArgs e);

        private const int MAXDEBUGITEMS = 1000;

        public enum ServeFileMode : int
        {
            @default = 0,
            inline = 1,
            attachment = 2
        }

        internal WebSite()
        {
            // MyBase.New("WebSite", Nothing)
        }

        // Private m_TimesIndex As Integer = 0
        public readonly object webSiteLock = new object();
        // Private m_Times As StatsItem() = Arrays.CreateInstance(Of StatsItem)(MAXDEBUGITEMS)


        private SiteConfig m_Config;
        private CAllowedIPs m_AllowedIPs;
        private Sistema.CModule m_Module = null;

        public Sistema.CModule Module
        {
            get
            {
                if (m_Module is null)
                    m_Module = Sistema.Modules.GetItemByName("modWebSite");
                return m_Module;
            }
        }

        public bool IsLocal()
        {
            if (ASP_Request is object)
                return ASP_Request.IsLocal;
            return false;
        }

        // Public Function BeginLog(ByVal page As String) As StatsItem
        // SyncLock Me.webSiteLock
        // Dim i As Integer = Me.m_TimesIndex
        // Me.m_TimesIndex += 1
        // If (Me.m_TimesIndex >= MAXDEBUGITEMS) Then Me.m_TimesIndex = 0

        // Dim item As New StatsItem(page)
        // item.LastRun = Now
        // Me.m_Times(i) = item
        // Return Me.m_Times(i)
        // End SyncLock
        // End Function

        // Public Sub EndLog(ByVal item As StatsItem)
        // SyncLock Me.webSiteLock
        // Dim span As TimeSpan = Now - item.LastRun
        // item.Count += 1
        // item.ExecTime = span.TotalMilliseconds / 1000
        // item.MaxExecTime = item.ExecTime
        // End SyncLock
        // End Sub

        // Public Function GetQueryTimes() As StatsItem() ' KeyValuePair(Of String, Double)()
        // Return Me.m_Times
        // End Function

        public CAllowedIPs AllowedIPs
        {
            get
            {
                if (m_AllowedIPs is null)
                {
                    m_AllowedIPs = new CAllowedIPs();
                    m_AllowedIPs.Load();
                }

                return m_AllowedIPs;
            }
        }

        public IPADDRESSinfo GetIPAllowInfo(string value)
        {
            return AllowedIPs.GetIPAllowInfo(value);
        }

        public bool IsIPAllowed(string value)
        {
            return AllowedIPs.IsIPAllowed(value);
        }

        public bool IsIPNegated(string value)
        {
            return AllowedIPs.IsIPNegated(value);
        }

        public Sistema.CUser get_CurrentUser(string sessionID)
        {
            return ((WebApplicationContext)Sistema.ApplicationContext).get_CurrentUser(sessionID);
        }

        public void set_CurrentUser(string sessionID, Sistema.CUser value)
        {
            ((WebApplicationContext)Sistema.ApplicationContext).set_CurrentUser(sessionID, value);
        }

        /// <summary>
    /// Restituisce vero se il sito è in manutenzione
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
        public bool IsMaintenance()
        {
            return Sistema.ApplicationContext.IsMaintenance();
        }

        /// <summary>
    /// Imposta il sito in manutenzione
    /// </summary>
    /// <remarks></remarks>
        public void SetMaintenance(bool value)
        {
            if (value)
            {
                Sistema.ApplicationContext.EnterMaintenance();
            }
            else
            {
                Sistema.ApplicationContext.QuitMaintenance();
            }
        }

        public CSiteSession CurrentSession
        {
            get
            {
                if (Sistema.ApplicationContext.CurrentSession is null && ASP_Session is object)
                    Sistema.ApplicationContext.CurrentSession = new CCurrentSiteSession();
                return (CSiteSession)Sistema.ApplicationContext.CurrentSession;
            }
        }

        public WebApplicationContext AppContext
        {
            get
            {
                return (WebApplicationContext)Sistema.ApplicationContext;
            }
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static System.Web.HttpServerUtility ASP_Server
        {
            get
            {
                if (System.Web.HttpContext.Current is null)
                    return null;
                return System.Web.HttpContext.Current.Server;
            }
        }

        /// <summary>
    /// Restituisce l'indirizzo IP della macchina che ha effettuato la richiesta remota (bypassando l'eventuale netmask)
    /// </summary>
    /// <returns></returns>
        public static string GetRemoteMachineIP()
        {
            string VisitorsIPAddr = string.Empty;
            if (System.Web.HttpContext.Current is object)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] is object)
                {
                    VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); // .ToString();
                }
                else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length > 0)
                {
                    VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
            }

            return VisitorsIPAddr;
        }

        public static System.Web.HttpRequest ASP_Request
        {
            get
            {
                if (System.Web.HttpContext.Current is null)
                    return null;
                return System.Web.HttpContext.Current.Request;
            }
        }

        public static System.Web.HttpResponse ASP_Response
        {
            get
            {
                if (System.Web.HttpContext.Current is null)
                    return null;
                return System.Web.HttpContext.Current.Response;
            }
        }

        public static System.Web.SessionState.HttpSessionState ASP_Session
        {
            get
            {
                if (System.Web.HttpContext.Current is null)
                    return null;
                return System.Web.HttpContext.Current.Session;
            }
        }

        public static System.Web.HttpApplicationState Application
        {
            get
            {
                if (System.Web.HttpContext.Current is null)
                    return null;
                return System.Web.HttpContext.Current.Application;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public SiteConfig Configuration
        {
            get
            {
                lock (webSiteLock)
                {
                    if (m_Config is null)
                    {
                        m_Config = new SiteConfig();
                        m_Config.Load();
                    }

                    return m_Config;
                }
            }
        }

        protected internal void SetConfiguration(SiteConfig value)
        {
            lock (webSiteLock)
            {
                m_Config = value;
                Sistema.IndexingService.MaxCacheSize = Sistema.IIF(value.CRMMaxCacheSize > 1, value.CRMMaxCacheSize, 1);
                Sistema.IndexingService.UnloadFactor = (float)Sistema.IIF(value.CRMUnloadFactor > 0f, value.CRMUnloadFactor, 0.25d);
                Databases.DBUtils.StopStatistics = !value.LogDBCommands;
            }

            doConfigChanged(new EventArgs());
        }

        protected internal void doConfigChanged(EventArgs e)
        {
            OnConfigurationChanged(new EventArgs());
        }

        protected void OnConfigurationChanged(EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


        public void ServeFile(string fileName)
        {
            string fileType = Sistema.FileSystem.GetExtensionName(fileName);
            string displayName = Sistema.FileSystem.GetFileName(fileName);
            ServeFile(fileName, fileType, displayName, ServeFileMode.@default, 1024);
        }

        public void ServeFile(string fileName, string fileType)
        {
            string displayName = Sistema.FileSystem.GetFileName(fileName);
            ServeFile(fileName, fileType, displayName, ServeFileMode.@default, 1024);
        }

        public void ServeFile(string fileName, string fileType, string displayName)
        {
            ServeFile(fileName, fileType, displayName, ServeFileMode.@default, 1024);
        }

        public void ServeFile(string fileName, string fileType, string displayName, ServeFileMode mode)
        {
            ServeFile(fileName, fileType, displayName, mode, 1024);
        }

        public void ServeFile(string fileName, string fileType, string displayName, ServeFileMode mode, int bufferSize)
        {
            long fileSize;
            string mimeType;
            string contentDisposition;
            fileSize = Sistema.FileSystem.GetFileSize(fileName);
            mimeType = Mime.GetMimeTypeFromExtension(fileType);
            switch (mode)
            {
                case ServeFileMode.attachment:
                    {
                        contentDisposition = "attachment";
                        break;
                    }

                case ServeFileMode.@default:
                    {
                        contentDisposition = Mime.GetDefaultContentDispositionExtension(fileType);
                        break;
                    }

                case ServeFileMode.inline:
                    {
                        contentDisposition = "inline";
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                        break;
                    }
            }
            // If displayName = vbNullString Then displayName = "download" & ASPSecurity.GetRandomKey(6)

            ASP_Response.ClearContent();
            ASP_Response.AddHeader("content-disposition", contentDisposition + "; filename= \"" + displayName + "\"");
            ASP_Response.AddHeader("content-length", fileSize.ToString());
            ASP_Response.ContentType = mimeType;
            ASP_Response.Charset = "utf-8"; // "windows-1252"
            ASP_Response.TransmitFile(fileName);
            ASP_Response.Flush();
            ASP_Response.End();
            // objStream.Close()
        }

        public string GetAbsolutePath(string path)
        {
            string ret = Sistema.ApplicationContext.BaseURL;
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.Right(ret, 1), "/", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                ret += "/";
            path = Strings.Trim(path);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.Left(path, 1), "/", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                path = Strings.Mid(path, 2);
            return ret + path;
        }

        public CCurrentPage CurrentPage
        {
            get
            {
                return new CCurrentPage();
            }
        }

        private static WebSite m_Instance = null;

        public static WebSite Instance
        {
            get
            {
                if (m_Instance is null)
                    m_Instance = new WebSite();
                return m_Instance;
            }
        }
    }
}