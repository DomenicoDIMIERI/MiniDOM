using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.Net;
using DMD.XML;
using static DMD.Logging;

namespace minidom
{
  public partial class WebSite
  {

    /// <summary>
    /// Applicazione Web Standard
    /// </summary>
    public abstract class WebApplicationContext
        : IWebApplicationContext
    {
      /// <summary>
      /// Evento generato quando viene modificata la configurazione del sistema
      /// </summary>
      /// <returns></returns>
      public event EventHandler ConfigChanged;

      /// <summary>
      /// Lock globale dell'applicazione
      /// </summary>
      public readonly object applicationLock = new object();


      /// <summary>
      /// Evento generato il certificato SSL utilizzato da un sito non è valido
      /// </summary>
      /// <remarks></remarks>
      public event CertificateValidationFailEventHandler CertificateValidationFail;

      /// <summary>
      /// Evento generato il certificato SSL utilizzato da un sito non è valido
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <remarks></remarks>
      public delegate void CertificateValidationFailEventHandler(object sender, MailCertificateEventArgs e);

      private CApplicationSettings m_Settings;
      private string m_BaseName = "frmdmd";
      private CKeyCollection<CSessionInfo> m_Info = new CKeyCollection<CSessionInfo>();
      private bool oldCheckMails = false;+
            private bool m_Maintenance = false;
      private CCollection<DBConnection> m_mainenanceDBCollection;
      private AbstractLogger m_LogWriter = null;
      private string m_LogFileName = "";
      private string m_SettingsFileName = "/app_data/minidom/settings.xml";
      private string m_PublicUrl = "/public/minidom/";
      private string m_DBURL = "/mdb-database/minidom/";
      private string m_StartupFolder = "";

      /// <summary>
      /// Costruttore
      /// </summary>
      public WebApplicationContext()
      {

      }

      /// <summary>
      /// Costruttore
      /// </summary>
      /// <param name="baseName"></param>
      /// <param name="baseFolder"></param>
      /// <param name="publicUrl"></param>
      /// <param name="baseDB"></param>
      public WebApplicationContext(string baseName, string baseFolder, string publicUrl, string baseDB) : this()
      {
        SetStartupFloder(baseFolder);
        m_BaseName = baseName;
        m_PublicUrl = publicUrl;
        m_DBURL = baseDB;
      }

      /// <summary>
      /// URL pubblica del sito (esterno)
      /// </summary>
      public string PublicURL
      {
        get
        {
          return m_PublicUrl;
        }
      }

      /// <summary>
      /// URL del DB
      /// </summary>
      public string DBURL
      {
        get
        {
          return m_DBURL;
        }
      }

      /// <summary>
      /// Restituisce il nome del file di log predefinito
      /// </summary>
      /// <returns></returns>
      protected virtual string GetLogFileName()
      {
        lock (applicationLock)
        {
          if (string.IsNullOrEmpty(m_LogFileName))
          {
            const string validChars = "abcdefghijklmnopqrstuvwxyz1234567890";
            string baseName = "";
            foreach (char ch in BaseName)
            {
              if (validChars.IndexOf(DMD.Chars.LCase(ch)) >= 0)
                baseName += DMD.Strings.CStr(ch);
            }

            m_LogFileName = Path.Combine(SystemDataFolder, @"Log\" + Sistema.Formats.GetTimeStamp() + baseName + ".log");
            while (File.Exists(m_LogFileName))
              m_LogFileName = Path.Combine(SystemDataFolder, @"Log\" + Sistema.Formats.GetTimeStamp() + baseName + ".log");
          }

          return m_LogFileName;
        }
      }

      /// <summary>
      /// Registra i tipi gestiti dai moduli
      /// </summary>
      protected virtual void RegisterTypeProviders()
      {
        Log("Inizializzo i type providers");
        Sistema.Types.RegisteredTypeProviders.Add("CAzienda", Anagrafica.Aziende.GetItemById);
        Sistema.Types.RegisteredTypeProviders.Add("CPersonaFisica", Anagrafica.Persone.GetItemById);
        Sistema.Types.RegisteredTypeProviders.Add("CUser", Sistema.Users.GetItemById);
        Sistema.Types.RegisteredTypeProviders.Add("CGroup", Sistema.Groups.GetItemById);
        Sistema.Types.RegisteredTypeProviders.Add("CModule", Sistema.Modules.GetItemById);
        Sistema.Types.RegisteredTypeProviders.Add("CProcedura", Sistema.Procedure.GetItemById);
      }

      /// <summary>
      /// Deregistra i tipi gestiti dai moduli
      /// </summary>
      protected virtual void UnregisterTypeProviders()
      {
        Log("Termino i type providers");
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CAzienda");
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CPersonaFisica");
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CUser");
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CGroup");
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CModule");
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("CProcedura");
      }




      /// <summary>
      /// Inizializza il 
      /// </summary>
      public virtual void BeginLog()
      {
        var buffer = new StringBuilder();
        buffer.Append(DMD.Strings.NChars(80, "-"));
        buffer.Append(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()));
        buffer.Append(DMD.Strings.vbNewLine);
        Log(buffer.ToString());
      }

      public virtual void EndLog()
      {
        var buffer = new StringBuilder();
        buffer.Append(DMD.Strings.vbNewLine);
        buffer.Append(DMD.Strings.vbNewLine);
        buffer.Append(DMD.Strings.vbNewLine);
        buffer.Append(DMD.Strings.NChars(80, "*"));
        Log(buffer.ToString());
      }

      /// <summary>
      /// Ruota il log in un file archivio e restituisce il nome del file archivio
      /// </summary>
      /// <returns></returns>
      public virtual string RotateLog()
      {
        string logFileName = this.GetLogFileName();
        string rotateToFile = DMD.IOUtils.AppendToFileName(logFileName, DMD.DateUtils.GetTimeStamp());
        this.m_LogWriter.Rotate(rotateToFile);
        return rotateToFile;
      }

      /// <summary>
      /// Logga un messaggio
      /// </summary>
      /// <param name="message"></param>
      /// <param name="severity"></param>
      public virtual void Log(string message, WarningSeverity severity = WarningSeverity.MESSAGE)
      {
        m_LogWriter.Notify(message, severity);
      }

      public CCollection<CSessionInfo> GetAllSessions()
      {
        lock (applicationLock)
          return new CCollection<CSessionInfo>(m_Info);
      }

      private int m_MaxSessionsTimeout = 60;

      /// <summary>
      /// Restituisce o imposta il limite massimo (in minuti) oltre il quale una sessione inattiva viene terminata
      /// </summary>
      /// <returns></returns>
      public int MaxSessionsTimeout
      {
        get
        {
          return m_MaxSessionsTimeout;
        }

        set
        {
          if (value < 1)
            throw new ArgumentNullException("Il limite non puà essere < 1");
          m_MaxSessionsTimeout = value;
        }
      }



      /// <summary>
      /// Questa procedura controlla l'effettivo utilizzo delle sessioni e termina quelle non più attive
      /// </summary>
      public void SessionsMaintenance()
      {
        lock (applicationLock)
        {
          Log("WebSite Session Maintenance: Started");
          int i = 0;
          CSessionInfo info;
          int cnt = 0;
          while (i < m_Info.Count)
          {
            info = m_Info[i];
            if (DMD.DateUtils.DateDiff(DateTimeInterval.Minute, info.LastUpdated, DMD.DateUtils.Now()) > MaxSessionsTimeout)
            {
              Log("WebSite Session Maintenance: Rimuovo la sessione " + info.SessionID);
#if (!DEBUG)
                            try {
#endif
              info.Dispose();
#if (!DEBUG)
                            } catch (Exception ex) {
                                Log("WebSite Session Maintenance: Errore nel terminare la sessione " + info.SessionID);
                                Log(ex.StackTrace);
                            }
#endif
              m_Info.Remove(info);
              cnt += 1;
            }
            else
            {
              i += 1;
            }
          }

          Log("WebSite Session Maintenance: Completed (" + cnt + " sessions removed");
        }
      }

      public CCollection<CSessionInfo> GetUserSessions(Sistema.CUser user)
      {
        lock (applicationLock)
        {
          if (user is null)
            throw new ArgumentNullException("user");
          var ret = new CCollection<CSessionInfo>();
          foreach (CSessionInfo s in m_Info)
          {
            if (ReferenceEquals(s.OriginalUser, user) || s.OriginalUser is null && s.CurrentUser == user)
            {
              ret.Add(s);
            }
          }

          return ret;
        }
      }

      /// <summary>
      /// Metodo richiamato da una pagina per informare che la sessione corrente é ancora attiva
      /// </summary>
      public void UpdateCurrentSession()
      {
        lock (applicationLock)
        {
          var info = GetCurrentSessionInfo();
          info.LastUpdated = DMD.DateUtils.Now();
        }
      }

      /// <summary>
      /// Restituisce il descrittore di sessione corrispondente alla sessione corrente
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
      public CSessionInfo GetCurrentSessionInfo()
      {
        if (ASP_Session is null)
          return null;
        return GetSessionInfo(ASP_Session.SessionID);
      }

      public virtual string BaseName
      {
        get
        {
          return m_BaseName;
        }
      }

      public virtual string Description
      {
        get
        {
          return "Applicazione Web"; // WebSite.Request.ServerVariables("HTTP_USER_AGENT")
        }
      }

      public virtual string InstanceID
      {
        get
        {
          if (ASP_Session is null)
            return "";
          return ASP_Session.SessionID;
        }
      }

      public virtual string RemoteIP
      {
        get
        {
          if (ASP_Request is null)
            return "";
          return ASP_Request.ServerVariables["REMOTE_ADDR"];
        }
      }

      public virtual int RemotePort
      {
        get
        {
          if (ASP_Request is null)
            return 0;
          return DMD.Integers.ValueOf(ASP_Request.ServerVariables["REMOTE_PORT"]);
        }
      }

      public string StartupFloder
      {
        get
        {
          if (string.IsNullOrEmpty(m_StartupFolder))
          {
            return Sistema.ApplicationContext.MapPath(Instance.Configuration.PublicURL);
          }
          else
          {
            return m_StartupFolder;
          }
        }
      }

      public void SetStartupFloder(string value)
      {
        m_StartupFolder = value;
      }

      private string m_TmporaryFolder = "";

      public virtual string TmporaryFolder
      {
        get
        {
          if (string.IsNullOrEmpty(m_TmporaryFolder))
          {
            return Path.Combine(StartupFloder, "Temp");
          }
          else
          {
            return m_TmporaryFolder;
          }
        }
      }

      protected string m_SystemDataFolder = "";

      public virtual string SystemDataFolder
      {
        get
        {
          if (string.IsNullOrEmpty(m_SystemDataFolder))
          {
            return Path.Combine(StartupFloder, @"System\Data");
          }
          else
          {
            return m_SystemDataFolder;
          }
        }
      }

      public void SetSystemDataFolder(string value)
      {
        m_SystemDataFolder = value;
      }

      protected string m_UserDataFolder = "";

      public virtual string UserDataFolder
      {
        get
        {
          if (string.IsNullOrEmpty(m_UserDataFolder))
          {
            return Path.Combine(StartupFloder, @"Users\" + Databases.GetID(CurrentUser) + @"\Data");
          }
          else
          {
            return m_UserDataFolder;
          }
        }
      }

      public void SetUserDataFolder(string value)
      {
        m_UserDataFolder = value;
      }

      public virtual bool IsUserLogged(Sistema.CUser user) => Sistema.Users.IsUserLogged(user);

      public Sistema.CLoginHistory CurrentLogin
      {
        get
        {
          if (ASP_Session is null)
            return null;
          return get_CurrentLogin(ASP_Session.SessionID);
        }

        set
        {
          if (ASP_Session is null)
            return;
          set_CurrentLogin(ASP_Session.SessionID, value);
        }
      }

      public virtual Sistema.CUser get_CurrentUser(string sessionID)
      {
        var info = GetSessionInfo(sessionID);
        if (info.CurrentUser is null)
          return Sistema.Users.KnownUsers.GuestUser;
        if (info.ForceAbadon)
        {
          info.ForceAbadon = false;
          ASP_Session.Abandon();
          return Sistema.Users.KnownUsers.GuestUser;
        }
        else
        {
          return info.CurrentUser;
        }
      }

      public virtual void set_CurrentUser(string sessionID, Sistema.CUser value)
      {
        var info = GetSessionInfo(sessionID);
        if (value is null)
        {
          info.CurrentUser = Sistema.Users.KnownUsers.GuestUser;
        }
        else
        {
          info.CurrentUser = value;
        }
      }

      public virtual Sistema.CLoginHistory get_CurrentLogin(string sessionID)
      {
        var info = GetSessionInfo(sessionID);
        return info.CurrentLogin;
      }

      public virtual void set_CurrentLogin(string sessionID, Sistema.CLoginHistory value)
      {
        var info = GetSessionInfo(sessionID);
        info.CurrentLogin = value;
      }

      public Anagrafica.CUfficio get_CurrentUfficio(string sessionID)
      {
        var info = GetSessionInfo(sessionID);
        return info.CurrentUfficio;
      }

      public void set_CurrentUfficio(string sessionID, Anagrafica.CUfficio value)
      {
        var info = GetSessionInfo(sessionID);
        info.CurrentUfficio = value;
      }

      public virtual CSiteSession get_CurrentSession(string sessionID)
      {
        var info = GetSessionInfo(sessionID);
        return info.CurrentSession;
      }

      public virtual void set_CurrentSession(string sessionID, CSiteSession value)
      {
        var info = GetSessionInfo(sessionID);
        info.CurrentSession = value;
      }

      public CSessionInfo GetSessionInfo(string sessionID)
      {
        lock (applicationLock)
        {
          if (string.IsNullOrEmpty(sessionID))
            return null;
          var info = m_Info.GetItemByKey(sessionID);
          // Dim info As CSessionInfo = Nothing  
          // For Each o As CSessionInfo In Me.m_Info
          // If (o.SessionID = sessionID) Then
          // info = o
          // Exit For
          // End If
          // Next

          if (info is null)
          {
            info = new CSessionInfo(sessionID);
            info.ServerTime = DMD.DateUtils.Now();
            info.CurrentUser = Sistema.Users.KnownUsers.GuestUser;
            info.CurrentUfficio = null;
            info.Descrizione = IdentifyBrowser();
            info.CurrentSession = new CCurrentSiteSession();
            m_Info.Add(info.SessionID, info);
            m_Info.Add(sessionID, info);
            info.RemoteIP = info.CurrentSession.RemoteIP;
            info.RemotePort = info.CurrentSession.RemotePort.ToString();
          }

          return info;
        }
      }

      public virtual object CurrentSession
      {
        get
        {
          if (ASP_Session is null)
            return null;
          return get_CurrentSession(ASP_Session.SessionID);
        }

        set
        {
          if (ASP_Session is null)
            return;
          set_CurrentSession(ASP_Session.SessionID, (CSiteSession)value);
        }
      }

      public virtual Sistema.CUser CurrentUser
      {
        get
        {
          if (ASP_Session is null)
            return Sistema.Users.KnownUsers.GuestUser;
          return get_CurrentUser(ASP_Session.SessionID);
        }

        set
        {
          if (ASP_Session is null)
            return;
          set_CurrentUser(ASP_Session.SessionID, value);
        }
      }

      public Anagrafica.CUfficio CurrentUfficio
      {
        get
        {
          if (ASP_Session is null)
            return null;
          return get_CurrentUfficio(ASP_Session.SessionID);
        }

        set
        {
          if (ASP_Session is null)
            return;
          set_CurrentUfficio(ASP_Session.SessionID, value);
        }
      }

      public abstract System.Reflection.Assembly GetEntryAssembly();

      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      public class CApplicationSettings : Sistema.CSettings
      {
        public new void SetOwner(WebApplicationContext app)
        {
          base.SetOwner(app);
        }
      }

      /// <summary>
      /// Impostazioni dell'applicazione
      /// </summary>
      public virtual CSettings Settings
      {
        get
        {
          if (m_Settings is null)
          {
            var _as = new CApplicationSettings();
            _as.SetOwner(this);
            string fileName = MapPath(m_SettingsFileName);
            if (File.Exists(fileName))
            {
              _as.LoadFromFile(fileName);
            }

            m_Settings = _as;
          }

          return m_Settings;
        }
      }

      void minidom.Sistema.ISettingsOwner.NotifySettingsChanged(CSettingsChangedEventArgs e)
      {
        Settings.SaveToFile(MapPath(m_SettingsFileName));
      }

      /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


      private bool HasKey(System.Collections.Specialized.NameValueCollection c, string key)
      {
        key = Strings.Trim(key);
        for (int i = 0, loopTo = c.Keys.Count - 1; i <= loopTo; i++)
        {
          string k = c.Keys[i];
          if (DMD.Strings.Compare(k, key, true) == 0)
            return true;
        }

        return false;
      }

      public virtual string GetParameter(string paramName, string defValue = DMD.Strings.vbNullString)
      {
        if (this.HasKey(ASP_Request.Form, paramName))
        {
          return ASP_Request.Form[paramName];
        }
        else if (this.HasKey(ASP_Request.QueryString, paramName))
        {
          return ASP_Request.QueryString[paramName];
        }
        else
        {
          return defValue;
        }
      }

      public virtual bool IsFormSet(string key)
      {
        var keys = ASP_Request.Form.AllKeys;
        for (int i = 0, loopTo = DMD.Arrays.UBound(keys); i <= loopTo; i++)
        {
          if (CultureInfo.CurrentCulture.CompareInfo.Compare(keys[i] ?? "", key ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            return true;
        }

        return false;
      }

      public virtual bool IsQueryStringSet(string key)
      {
        var keys = ASP_Request.QueryString.AllKeys;
        for (int i = 0, loopTo = DMD.Arrays.UBound(keys); i <= loopTo; i++)
        {
          if (CultureInfo.CurrentCulture.CompareInfo.Compare(keys[i] ?? "", key ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            return true;
        }

        return false;
      }

      public virtual T GetParameter<T>(string paramName, object defValue = null)
      {
        string value;
        if (IsFormSet(paramName))
        {
          value = ASP_Request.Form[paramName];
        }
        else if (IsQueryStringSet(paramName))
        {
          value = ASP_Request.QueryString[paramName];
        }
        else
        {
          value = DMD.Strings.CStr(defValue);
        }

        return (T)Sistema.Types.CastTo(value, Type.GetTypeCode(typeof(T)));
      }

      public virtual string MapPath(string path)
      {
        if (ASP_Server is object)
        {
          return ASP_Server.MapPath(path);
        }
        else
        {
          string bp = this.BasePath;
          string localPath = Strings.Replace(path, "/", "\\");
          return IOUtils.GetAbsolutePath(path, bp);
        }
      }

      public virtual string UnMapPath(string path)
      {
        string basePath = Strings.LCase(Sistema.FileSystem.NormalizePath(MapPath("/")));
        if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.LCase(Strings.Left(path, Strings.Len(basePath))) ?? "", basePath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
        {
          path = "/" + Strings.Replace(Strings.Mid(path, Strings.Len(basePath) + 1), @"\", "/");
          return path;
        }
        else
        {
          throw new ArgumentException("Il percorso non è invertibile");
        }
      }

      public virtual bool IsDebug()
      {
        // Return minidom.Sistema.FileSystem.FileExists(Me.MapPath("/debug.dbg"))
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        return true;
        /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
      }

      // Protected m_AziendaPrincipale As CAzienda = Nothing

      /// <summary>
      /// Restituisce o imposta l'azienda utilizzata come azienda principale
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
      public virtual int IDAziendaPrincipale
      {
        get
        {
          return Anagrafica.Aziende.Module.Settings.GetValueInt("AziendaPrincipale", 0);
        }

        set
        {
          Anagrafica.Aziende.Module.Settings.SetValueInt("AziendaPrincipale", value);
        }
      }

      public abstract string SupportEMail { get; }
      public abstract string BasePath { get; }
      public abstract string BaseURL { get; }
      // Get
      // Dim tmp As String = Me.m_BaseURL
      // If (tmp = "" AndAlso HttpContext.Current IsNot Nothing) Then tmp = HttpContext.Current.Request.Url.AbsoluteUri
      // Dim i As Integer = InStr(tmp, "://")
      // If (i <= 0) Then Return tmp
      // i = InStr(i + 3, tmp, "/")
      // If (i > 0) Then
      // Return Left(tmp, i - 1)
      // Else
      // Return tmp
      // End If
      // Return Me.BasePath
      // End Get
      // End Property

      public abstract string Title { get; }

      private string GetCurrentURL()
      {
        if (IsDebug())
        {
          return "http://localhost:50528";
        }
        else
        {
          return Instance.Configuration.SiteURL;
        }
      }

      /// <summary>
      /// Inizializza il sistema di log
      /// </summary>
      protected virtual void InitializeLoggingSystem()
      {
        string logFile = GetLogFileName();
        string logfolder = Sistema.FileSystem.GetFolderName(logFile);

        lock (applicationLock)
        {
          Sistema.FileSystem.CreateRecursiveFolder(logfolder);
          m_LogWriter = new FileLogger(GetLogFileName());
        }
      }

      /// <summary>
      /// Inizializza le connessioni al db
      /// </summary>
      protected virtual void InitializeConnections()
      {
      }

      /// <summary>
      /// Inizializza i moduli
      /// </summary>
      protected virtual void InitializeModules()
      {
        //Anagrafica.Intialize();
        //Anagrafica.Persone.Initialize();
        //// For Each m As CModule In Sistema.Modules.LoadAll()
        // m.InitializeFrom()

        // Next
      }



      /// <summary>
      /// Avvia l'applicazione
      /// </summary>
      public void Start()
      {
#if (!DEBUG)
                AppDomain.CurrentDomain.UnhandledException += this.handleUnhandledExceptions;
#endif
        this.InstallTypeResolvers();

        // Inizializza il sistema dei log
        InitializeLoggingSystem();

        // Inizializza gli handlers
        RegisterHandlers();
        // Inizializza i tipi
        RegisterTypes();
        RegisterTypeProviders();

        // Avvia le connessioni
        InitializeConnections();

        // Avvia i moduli
        InitializeModules();

        // Avvio interno
        InternalStart();
        if (Databases.LOGConn.IsOpen())
        {
          var dbSQL = new System.Text.StringBuilder();
          dbSQL.Append("UPDATE [tbl_LoginHistory] SET [LogoutMethod]=");
          dbSQL.Append((int)Sistema.LogOutMethods.LOGOUT_REMOTEDISCONNECT);
          dbSQL.Append(", [LogOutTime]=");
          dbSQL.Append(Databases.DBUtils.DBDate(DMD.DateUtils.Now()));
          dbSQL.Append(" WHERE [LogoutMethod]=");
          dbSQL.Append((int)Sistema.LogOutMethods.LOGOUT_UNKNOWN);
          Databases.LOGConn.ExecuteCommand(dbSQL.ToString());
        }

        if (Databases.APPConn.IsOpen())
        {
          // Sistema.FileSystem.DeleteFile (Sistema.FileSystem . "*.*

          Sistema.Backups.Initialize();
          Sistema.Module.DispatchEvent(new Sistema.EventDescription("APP_Start", "Applicazione avviata", ""));
        }

        Sistema.RPC.BaseURL = GetCurrentURL(); // 

        // Eliminiamo i log 
        if (Instance.Configuration.DeleteLogFilesAfterNDays > 0)
        {
          string logPath = Sistema.FileSystem.GetFolderName(GetLogFileName());
          Sistema.FileSystem.CreateRecursiveFolder(logPath);
          var dinfo = new DirectoryInfo(logPath);
          var files = dinfo.GetFiles("*.log");
          for (int i = 0, loopTo = DMD.Arrays.Len(files) - 1; i <= loopTo; i++)
          {
            if (DMD.DateUtils.DateDiff(DateTimeInterval.Day, files[i].CreationTime, DMD.DateUtils.ToDay()) > Instance.Configuration.DeleteLogFilesAfterNDays)
            {
#if (!DEBUG)
                            try {
#endif
              files[i].Delete();
#if (!DEBUG)
                            } catch (Exception ex) {
                                Log("Errore nell'eliminazione del file di log: " + files[i].FullName + " : " + ex.Message);
                            }
#endif
            }
          }
        }

        Sistema.Procedure.StartBackgroundWorker();
      }

      /// <summary>
      /// Cattura gli errori non gestiti
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected virtual void handleUnhandledExceptions(object sender, UnhandledExceptionEventArgs e)
      {


      }

      private DMD.RunTime.ITypeResolver typeResolver = null;

      protected virtual void InstallTypeResolvers()
      {
        if (this.typeResolver is null)
        {
          this.typeResolver = new MiniDOMTypeResolver();
          DMD.RunTime.InstallTypeResolver(this.typeResolver);
        }
      }

      protected virtual void UninstallTypeResolvers()
      {
        if (this.typeResolver is object)
        {
          DMD.RunTime.UnInstallTypeResolver(this.typeResolver);
          this.typeResolver = null;
        }
      }

      private class MiniDOMTypeResolver
          : DMD.RunTime.ITypeResolver
      {
        public Type Resolve(string typeName)
        {
          return minidom.Sistema.Types.GetType(typeName);
        }
      }

      protected virtual void RegisterHandlers()
      {
        // AddHandler DBUtils.CursorOpened, AddressOf handleCursorOpen
        // AddHandler DBUtils.CursorClosed, AddressOf handleCursorClosed
        // AddHandler DBUtils.ConnectionOpened, AddressOf handleConnectionOpen
        // AddHandler DBUtils.ConnectionClosed, AddressOf handleConnectionClosed
      }

      /// <summary>
      /// Inizializza il sottosistema di gestione dei tipi
      /// </summary>
      /// <remarks></remarks>
      protected virtual void RegisterTypes()
      {
        Sistema.Types.Imports.Add("minidom");
        Sistema.Types.Imports.Add("minidom.Sistema");
        Sistema.Types.Imports.Add("minidom.FileSystem");
        Sistema.Types.Imports.Add("minidom.Databases");
        Sistema.Types.Imports.Add("minidom.Calendar");
        Sistema.Types.Imports.Add("minidom.Collegamenti");
        Sistema.Types.Imports.Add("minidom.Comunicazioni");
        Sistema.Types.Imports.Add("minidom.Appuntamenti");
        Sistema.Types.Imports.Add("minidom.GDE");
        Sistema.Types.Imports.Add("Anagrafica.Luoghi");
        Sistema.Types.Imports.Add("minidom.Anagrafica");
        Sistema.Types.Imports.Add("minidom.CustomerCalls");
        Sistema.Types.Imports.Add("minidom.Finanziaria");
        Sistema.Types.Imports.Add("minidom.Forms");
        Sistema.Types.Imports.Add("minidom.Messenger");
        Sistema.Types.Imports.Add("minidom.Web");
        Sistema.Types.Imports.Add("minidom.WebSite");
        Sistema.Types.Imports.Add("minidom.Visite");
        Sistema.Types.Imports.Add("minidom.ADV");
        Sistema.Types.Imports.Add("minidom.Anagrafica+Fonti");
        Sistema.Types.Imports.Add("minidom.Office");
        Sistema.Types.Imports.Add("minidom.Mail");
        Sistema.Types.Imports.Add("minidom.Tickets");
        Sistema.Types.Imports.Add("minidom.GPS");
        Sistema.Types.Imports.Add("minidom.Drivers");
        Sistema.Types.Imports.Add("minidom.internals");
      }

      protected virtual void InternalStart()
      {
        ServicePointManager.ServerCertificateValidationCallback = handleremotecertificatevalidationcallback;
      }

      // public delegate function remotecertificatevalidationcallback(sender as object, certificate as system.security.cryptography.x509certificates.x509certificate, chain as system.security.cryptography.x509certificates.x509chain, sslpolicyerrors as system.net.security.sslpolicyerrors) as boolean
      private bool handleremotecertificatevalidationcallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslpolicyerrors)
      {
        if (sslpolicyerrors == System.Net.Security.SslPolicyErrors.None)
        {
          return true;
        }
        else
        {
          var e = new Sistema.MailCertificateEventArgs(certificate, chain, sslpolicyerrors);
          e.Allow = false;
          OnCertificateValidationFail(sender, e);
          return e.Allow;
        }
      }

      protected virtual void OnCertificateValidationFail(object sender, Sistema.MailCertificateEventArgs e)
      {
        CertificateValidationFail?.Invoke(sender, e);
      }

      /// <summary>
      /// Ferma l'applicazione
      /// </summary>
      public void Stop()
      {
        Sistema.Procedure.StopBackgroundWorker();
        if (Databases.LOGConn.IsOpen())
        {
          Databases.LOGConn.ExecuteCommand("UPDATE [tbl_LoginHistory] SET [LogoutMethod]=" + ((int)Sistema.LogOutMethods.LOGOUT_REMOTEDISCONNECT).ToString() + ", [LogOutTime]=" + Databases.DBUtils.DBDate(DMD.DateUtils.Now()) + " WHERE [LogoutMethod]=" + ((int)Sistema.LogOutMethods.LOGOUT_UNKNOWN).ToString());
        }

        if (Databases.APPConn.IsOpen())
        {
          Sistema.Module.DispatchEvent(new Sistema.EventDescription("APP_End", "Applicazione terminata", ""));
        }

        InternalStop();
        UnregisterTypeProviders();
        UnregisterTypes();
        UnregisterHandlers();

        this.UninstallTypeResolvers();

        AppDomain.CurrentDomain.UnhandledException -= this.handleUnhandledExceptions;
      }

      /// <summary>
      /// Deregistra i tipi
      /// </summary>
      protected virtual void UnregisterTypes()
      {
        Sistema.Types.Imports.Remove("minidom");
        Sistema.Types.Imports.Remove("minidom.Sistema");
        Sistema.Types.Imports.Remove("minidom.FileSystem");
        Sistema.Types.Imports.Remove("minidom.Databases");
        Sistema.Types.Imports.Remove("minidom.Calendar");
        Sistema.Types.Imports.Remove("minidom.Collegamenti");
        Sistema.Types.Imports.Remove("minidom.Comunicazioni");
        Sistema.Types.Imports.Remove("minidom.Appuntamenti");
        Sistema.Types.Imports.Remove("minidom.GDE");
        Sistema.Types.Imports.Remove("Anagrafica.Luoghi");
        Sistema.Types.Imports.Remove("minidom.Anagrafica");
        Sistema.Types.Imports.Remove("minidom.CustomerCalls");
        Sistema.Types.Imports.Remove("minidom.Finanziaria");
        Sistema.Types.Imports.Remove("minidom.Forms");
        Sistema.Types.Imports.Remove("minidom.Messenger");
        Sistema.Types.Imports.Remove("minidom.Web");
        Sistema.Types.Imports.Remove("minidom.WebSite");
        Sistema.Types.Imports.Remove("minidom.Visite");
        Sistema.Types.Imports.Remove("minidom.ADV");
        Sistema.Types.Imports.Remove("minidom.Anagrafica+Fonti");
        Sistema.Types.Imports.Remove("minidom.Office");
        Sistema.Types.Imports.Remove("minidom.Mail");
        Sistema.Types.Imports.Remove("minidom.Tickets");
        Sistema.Types.Imports.Remove("minidom.GPS");
        Sistema.Types.Imports.Remove("minidom.Drivers");
      }

      protected virtual void UnregisterHandlers()
      {
        // RemoveHandler DBUtils.CursorOpened, AddressOf handleCursorOpen
        // RemoveHandler DBUtils.CursorClosed, AddressOf handleCursorClosed
        // RemoveHandler DBUtils.ConnectionOpened, AddressOf handleConnectionOpen
        // RemoveHandler DBUtils.ConnectionClosed, AddressOf handleConnectionClosed
      }

      protected virtual void InternalStop()
      {
        ResettaSessioni();
        ResettaConnessioni();
      }

      // Private Sub handleCursorOpen(ByVal sender As Object, ByVal e As DBCursorEventArgs)
      // SyncLock Me.GlobalOpenedCursors
      // Me.GlobalOpenedCursors.Add(e.Cursor)
      // End SyncLock
      // End Sub

      // Private Sub handleCursorClosed(ByVal sender As Object, ByVal e As DBCursorEventArgs)
      // SyncLock Me.GlobalOpenedCursors
      // Me.GlobalOpenedCursors.Remove(e.Cursor)
      // End SyncLock
      // End Sub

      // Private Sub handleConnectionOpen(ByVal sender As Object, ByVal e As DBEventArgs)
      // SyncLock Me.applicationLock
      // Me.OpenedConnections.Add(e.Connection)
      // End SyncLock
      // End Sub

      // Private Sub handleConnectionClosed(ByVal sender As Object, ByVal e As DBEventArgs)
      // SyncLock Me.applicationLock
      // Me.OpenedConnections.Remove(e.Connection)
      // End SyncLock
      // End Sub

      // Public Sub ResettaCursori()
      // SyncLock Me.applicationLock
      // While Me.GlobalOpenedCursors.Count > 0
      // Me.GlobalOpenedCursors(0).Dispose()
      // End While
      // End SyncLock
      // End Sub

      public void ResettaConnessioni()
      {
        var col = Databases.DBUtils.GetAllOpenedConnections();
        foreach (Databases.CDBConnection con in col)
        {
#if (!DEBUG)
                    try {
#endif
          con.CloseDB();
#if (!DEBUG)
                    } catch (Exception ex)
                    {
                    }
#endif
        }

        Thread.Sleep(1000);
      }

      public void ResettaSessioni()
      {
        var col = GetAllSessions();
        foreach (CSessionInfo info in col)
          info.Reset();
      }

      public virtual object GetProperty(string name)
      {
        CSiteSession session = (CSiteSession)CurrentSession;
        switch (Strings.LCase(Strings.Trim(name)) ?? "")
        {
          case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "remoteip", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
            {
              if (session is null)
                return "";
              return session.RemoteIP;
            }

          case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "remoteport", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
            {
              if (session is null)
                return "";
              return session.RemotePort;
            }

          case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "sessionid", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
            {
              if (session is null)
                return "";
              return session.SessionID;
            }

          case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "useragent", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
            {
              if (session is null)
                return "";
              return session.UserAgent;
            }

          default:
            {
              return null;
            }
        }
      }


      /// <summary>
      /// Mette l'applicazione in stato manutenzione
      /// </summary>
      /// <remarks></remarks>
      public void EnterMaintenance()
      {
        if (m_Maintenance)
          return;
        m_Maintenance = true;
        Log(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - " + CurrentUser.Nominativo + " - Enter Maintenance Mode");
        Sistema.Module.DispatchEvent(new Sistema.EventDescription("enter_maintenance", "Entro in modalità manutenzione", this));
        EnterMaintenanceInternal();
      }


      /// <summary>
      /// Metodo richiamato internamente da EnterMaintenance.
      /// Questa funzione chiude tute le connessioni e le memorizza in una variabile interna per poterle poi riaprire all'uscita dall
      /// modalità manutenzione
      /// </summary>
      /// <remarks></remarks>
      protected virtual void EnterMaintenanceInternal()
      {
        lock (applicationLock)
        {
          m_mainenanceDBCollection = Databases.DBUtils.GetAllOpenedConnections();
          oldCheckMails = Sistema.EMailer.AutoSynchronize;
          Sistema.EMailer.AutoSynchronize = false;
          ResettaSessioni();
          ResettaConnessioni();
        }
      }

      /// <summary>
      /// Restituisce vero se l'applicazione è in stato manutenzione
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
      public bool IsMaintenance()
      {
        return m_Maintenance;
      }

      /// <summary>
      /// Esce dallo stato manutenzione
      /// </summary>
      /// <remarks></remarks>
      public void QuitMaintenance()
      {
        if (m_Maintenance == false)
          return;
        QuitMaintenanceInternal();
        Thread.Sleep(2000);
        Log(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - " + CurrentUser.Nominativo + " - Quit Maintenance Mode");
        Sistema.Module.DispatchEvent(new Sistema.EventDescription("quit_maintenance", "Esco dalla modalità manutenzione", this));
        m_Maintenance = false;
      }

      protected virtual void QuitMaintenanceInternal()
      {
        lock (applicationLock)
        {
          foreach (var db in m_mainenanceDBCollection)
            db.Open();
          Sistema.EMailer.AutoSynchronize = oldCheckMails;
          m_mainenanceDBCollection = null;
        }
      }

      /// <summary>
      /// Apre il cursore e lo aggiunge all'elenco dei cursori aperti
      /// </summary>
      /// <param name="cursor"></param>
      public void OpenCursor(Databases.DBObjectCursorBase cursor)
      {
        lock (CurrentSession)
        {
          var info = GetCurrentSessionInfo();
          //while (info.RemoteOpenedCursors.ContainsKey(cursor.Token))
          //    cursor.Token = Sistema.ASPSecurity.GetRandomKey(25);
          info.RemoteOpenedCursors.Add(cursor.GetToken(), cursor);
        }

        if (cursor.CursorStatus != DBCursorStatus.NORMAL)
          cursor.Open();
      }

      /// <summary>
      /// Restituisce il cursore associato al token
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      public Databases.DBObjectCursorBase GetCursor(string token)
      {
        lock (CurrentSession)
        {
          var info = GetCurrentSessionInfo();
          return info.RemoteOpenedCursors.GetItemByKey(token);
        }
      }

      /// <summary>
      /// Distrugge il cursore associato al token
      /// </summary>
      /// <param name="token"></param>
      public void DestroyCursor(string token)
      {
        lock (CurrentSession)
        {
          var info = GetCurrentSessionInfo();
          var cursor = info.RemoteOpenedCursors.GetItemByKey(token); // DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
          if (cursor is object)
          {
            // If (TypeName(cursor) <> tn) Then Throw New ArgumentException("Il token non identifica il cursore specificato")
            cursor.Dispose();
            info.RemoteOpenedCursors.RemoveByKey(token);
          }
        }
      }

      /// <summary>
      /// Resetta il cursore associato al token
      /// </summary>
      /// <param name="token"></param>
      public void ResetCursor(string token)
      {
        Databases.DBObjectCursorBase cursor;
        lock (CurrentSession)
        {
          var info = GetCurrentSessionInfo();
          cursor = info.RemoteOpenedCursors[token]; // DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
        }

        cursor.Reset1();
      }

      /// <summary>
      /// Ripristina il cursore associato al token
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      public Databases.DBObjectCursorBase RestoreCursor(string token)
      {
        lock (CurrentSession)
        {
          var info = GetCurrentSessionInfo();
          return info.RemoteOpenedCursors.GetItemByKey(token); // DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
        }
      }

      /// <summary>
      /// Avvia la sessione
      /// </summary>
      /// <param name="session"></param>
      /// <returns></returns>
      public CCurrentSiteSession StartSession(HttpSessionState session)
      {
        lock (applicationLock)
        {
          Log("Sessione Web Iniziata: " + session.SessionID);
          var info = m_Info.GetItemByKey(session.SessionID);
          // Dim info As CSessionInfo = Nothing
          // For Each o As CSessionInfo In Me.m_Info
          // If o.SessionID = session.SessionID Then
          // info = o
          // Exit For
          // End If
          // Next

          if (info is object && CultureInfo.CurrentCulture.CompareInfo.Compare(info.Descrizione ?? "", IdentifyBrowser() ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
          {
            string str = Sistema.Formats.GetTimeStamp() + " - " + info.SessionID + " - Riavvio della sessione" + DMD.Strings.vbNewLine;
            str += "Browser: " + info.Descrizione + " - " + IdentifyBrowser() + DMD.Strings.vbNewLine;
            str += "CurrentUser: " + info.CurrentUserName + " - " + DMD.Strings.vbNewLine;
            if (info.CurrentUfficio is object)
              str += "Ufficio: " + info.CurrentUfficio.Nome + DMD.Strings.vbNewLine;
            str += "Remote IP: " + info.RemoteIP + " - " + ASP_Request.ServerVariables["REMOTE_ADDR"] + DMD.Strings.vbNewLine;
            Log(str);
            throw new InvalidOperationException("La sessione Web " + session.SessionID + " già attiva");
          }

          info = new CSessionInfo(session.SessionID);
          info.Descrizione = IdentifyBrowser();
          info.ServerTime = DMD.DateUtils.Now();
          info.CurrentUser = Sistema.Users.KnownUsers.GuestUser;
          info.CurrentUfficio = null;
          info.CurrentSession = new CCurrentSiteSession();
          m_Info.Add(info.SessionID, info);
          var parameters = new CKeyCollection();
          parameters.SetItemByKey("SessionID", session.SessionID);
          parameters.SetItemByKey("RemoteIP", ASP_Request.ServerVariables["REMOTE_ADDR"]);
          parameters.SetItemByKey("RemotePort", ASP_Request.ServerVariables["REMOTE_PORT"]);
          parameters.SetItemByKey("RemoteUserAgent", ASP_Request.ServerVariables["HTTP_USER_AGENT"]);
          parameters.SetItemByKey("RemoteLocalIP", GetRemoteMachineIP());
          string initialReferrer = DMD.Strings.CStr(ASP_Session["InitialReferrer"]);
          if (string.IsNullOrEmpty(initialReferrer))
          {
            initialReferrer = ASP_Request.ServerVariables["HTTP_REFERER"];
            ASP_Session["InitialReferrer"] = initialReferrer;
          }

          parameters.SetItemByKey("InitialReferrer", initialReferrer);
          string siteCookie = Cookies.GetCookie("SiteCookie");
          if (string.IsNullOrEmpty(siteCookie))
          {
            siteCookie = DMD.Strings.GetRandomString(64);
            Cookies.SetCookie("SiteCookie", siteCookie);
          }

          parameters.SetItemByKey("SiteCookie", siteCookie);
          ((CCurrentSiteSession)info.CurrentSession).NotifyStart(parameters);
          info.RemoteIP = info.CurrentSession.RemoteIP;
          info.RemotePort = info.CurrentSession.RemotePort.ToString();
          return (CCurrentSiteSession)info.CurrentSession;
        }
      }

      private string IdentifyBrowser()
      {
        if (ASP_Request.Browser is null)
          return "";
        var bc = ASP_Request.Browser;
        return bc.Platform + ", " + bc.Browser + " " + bc.Version;

        // HttpBrowserCapabilities bc = Request.Browser;
        // Response.Write("<p>Browser Capabilities:</p>");
        // Response.Write("Type = " + bc.Type + "<br>");
        // Response.Write("Name = " + bc.Browser + "<br>");
        // Response.Write("Version = " + bc.Version + "<br>");
        // Response.Write("Major Version = " + bc.MajorVersion + "<br>");
        // Response.Write("Minor Version = " + bc.MinorVersion + "<br>");
        // Response.Write("Platform = " + bc.Platform + "<br>");
        // Response.Write("Is Beta = " + bc.Beta + "<br>");
        // Response.Write("Is Crawler = " + bc.Crawler + "<br>");
        // Response.Write("Is AOL = " + bc.AOL + "<br>");
        // Response.Write("Is Win16 = " + bc.Win16 + "<br>");
        // Response.Write("Is Win32 = " + bc.Win32 + "<br>");
        // Response.Write("Supports Frames = " + bc.Frames + "<br>");
        // Response.Write("Supports Tables = " + bc.Tables + "<br>");
        // Response.Write("Supports Cookies = " + bc.Cookies + "<br>");
        // Response.Write("Supports VB Script = " + bc.VBScript + "<br>");
        // Response.Write("Supports JavaScript = " + bc.JavaScript + "<br>");
        // Response.Write("Supports Java Applets = " + bc.JavaApplets + "<br>");
        // Response.Write("Supports ActiveX Controls = " + bc.ActiveXControls + "<br>");
        // Response.Write("CDF = " + bc.CDF + "<br>");
      }

      public void DisposeSession(HttpSessionState session)
      {
        lock (applicationLock)
        {
          if (session is null)
            return;

          Log("Sessione Web Terminata: " + session.SessionID);

          string key = session.SessionID;
          var info = m_Info.GetItemByKey(key);
          // Dim info As CSessionInfo = Nothing
          // For Each o As CSessionInfo In Me.m_Info
          // If (o.SessionID = session.SessionID) Then
          // info = o
          // Exit For
          // End If
          // Next

          if (info is null)
            return; // Throw New InvalidOperationException("La sessione Web " & session.SessionID & " non è attiva")
          var cu = info.CurrentUser;
          string uName = "Guest";
          if (cu is object && cu.IsLogged())
          {
            cu.LogOut(Sistema.LogOutMethods.LOGOUT_TIMEOUT);
          }

          if (info.CurrentSession is object)
          {
            ((CCurrentSiteSession)info.CurrentSession).NotifyEnd();
            info.Dispose();
          }

          m_Info.Remove(info);
        }
      }

      ~WebApplicationContext()
      {
        DMDObject.DecreaseCounter(this);
      }

      public void Personifica(Sistema.CUser newUser)
      {
        lock (applicationLock)
        {
          var info = GetCurrentSessionInfo();
          if (info is null)
            throw new ArgumentNullException("currentsessioninfo");
          if (newUser is null)
            throw new ArgumentNullException("newuser");
          Log("Impersonificazione -> Inizio da " + info.CurrentUser.Nominativo + " a " + newUser.Nominativo);
          info.OriginalUser = info.CurrentUser;
          info.CurrentUser = newUser;
        }
      }

      public void EsciPersonifica(Sistema.CUser newUser)
      {
        lock (applicationLock)
        {
          var info = GetCurrentSessionInfo();
          if (info is null)
            throw new ArgumentNullException("currentsessioninfo");
          if (!ReferenceEquals(newUser, info.CurrentUser))
            throw new ArgumentNullException("newuser");
          if (info.OriginalUser is null)
            throw new ArgumentNullException("originaluser");
          Log("Impersonificazione -> Fine da " + info.CurrentUser.Nominativo + " a " + info.OriginalUser.Nominativo);
          info.CurrentUser = info.OriginalUser;
          info.OriginalUser = null;
        }
      }

      private static byte[] ConvertToBytes(SecureString val)
      {
        if (val is null)
          throw new ArgumentNullException("val");
        // Dim unmanagedString As IntPtr = IntPtr.Zero
        // Try
        // unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(val)
        // Return System.Text.Encoding.ASCII.GetBytes(Marshal.PtrToStringUni(unmanagedString))
        // Finally
        // 'Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString)
        // End Try
        var c = new NetworkCredential("", val);
        return System.Text.Encoding.ASCII.GetBytes(c.Password);
      }

      /// <summary>
      /// Apre il database specificato e se non lo trova
      /// </summary>
      /// <param name="url"></param>
      /// <returns></returns>
      public Databases.CDBConnection OpenIfExistsDB(string url)
      {
        url = DMD.Strings.Trim(url);
        if (url.StartsWith("/") && url.Length > 1)
          url = url.Substring(1);
        string path = DBURL;
        if (!path.EndsWith("/"))
          path = path + "/";
        path = path + url;
        path = HttpContext.Current.Server.MapPath(path);
        if (File.Exists(path))
        {
          Log("Apro il database [" + url + "] -> " + path);
          Databases.CMdbDBConnection ret;
#if (!DEBUG)
                    try {
#endif
          ret = new Databases.CMdbDBConnection(path);
          var s = __ENCKEY();
          var enc = ConvertToBytes(s);
          ret.EncryptionKey = enc;
          ret.OpenDB();
#if (!DEBUG)
                    } catch (Exception ex) {
                        Log("Can't open  database" + url + DMD.Strings.vbNewLine + ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
                        sendCriticalEmail(ex);
                        Sistema.Events.NotifyUnhandledException(ex);
                        throw;
                    }
#endif
          return ret;
        }
        else
        {
          Log("Il database [" + url + "] -> " + path + " non esiste");
          return Databases.APPConn;
        }
      }

      //[SecuritySafeCritical]
      //[DllImport(@"dll\DMDCoreC.dll", EntryPoint = "#4", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
      //private static extern int ___IOGEN(IntPtr buff, int len);
      //[SecuritySafeCritical]
      //[DllImport(@"dll\DMDCoreC.dll", EntryPoint = "#4", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
      //private static extern int ___IOGEN(StringBuilder buff, int len);

      [SecuritySafeCritical]
      private SecureString __ENCKEY()
      {
        var s = new SecureString();
        s.AppendChar('W');
        s.AppendChar('P');
        s.AppendChar('A');
        s.AppendChar('Z');
        s.AppendChar('W');
        s.AppendChar('O');
        s.AppendChar('Q');
        s.AppendChar('A');
        s.AppendChar('G');
        s.AppendChar('E');
        s.AppendChar('M');
        s.AppendChar('M');
        s.AppendChar('E');
        s.AppendChar('Z');
        s.AppendChar('O');
        s.AppendChar('W');
        return s;

        // Dim buffLen As Integer = ___IOGEN(IntPtr.Zero, 0)
        // Dim buff As New System.Text.StringBuilder(buffLen)
        // Dim len As Integer = ___IOGEN(buff, buffLen)
        // Dim ret As New SecureString()
        // For i As Integer = 0 To len - 1
        // ret.AppendChar(buff(i))
        // Next
        // Return ret
      }

      /// <summary>
      /// Invia l'email di errore critico
      /// </summary>
      /// <param name="ex"></param>
      protected virtual void sendCriticalEmail(Exception ex)
      {
        string subject = "ERRORE CRITICO IN " + BaseName;
        string body = "";
        string FROMADDRESS = SupportEMail;
        string TOADDRESS = "domenico.dimieri@hotmail.it";
        string CCNADDRESS = "";
        body += "ERRORE CRITICO IN <b>" + BaseName + "</b><br/><br/>";
        body += "<b>" + ex.Message + "</b><br/><br/>";
        body += ex.StackTrace;
#if (!DEBUG)
                try {
#endif
        Sistema.EMailer.SendEMail1(
                    FROMADDRESS,
                    TOADDRESS,
                    "",
                    CCNADDRESS,
                    DMD.Strings.TrimTo(subject, 78),
                    body
                    );
#if (!DEBUG)
                } catch (Exception ex1) {
                }
#endif
      }

      /// <summary>
      /// Delegato usato per i log della funzione di compattamento
      /// </summary>
      /// <param name="message"></param>
      public delegate void LogFunction(string message);

      /// <summary>
      /// Compatta tutte i database usati dall'app
      /// </summary>
      /// <param name="logfun"></param>
      public virtual void CompactAllDB(LogFunction logfun)
      {
        lock (applicationLock)
        {
          logfun("Blocco l'applicazione<br/>");
          CCollection col = DBUtils.GetAllOpenedConnections();
          foreach (var db in col)
          {
            string dbPath = db.Path;
            logfun("Inizio la compattazione di \"" + dbPath + "\"<br/>");
#if (!DEBUG)
                        try {
#endif
            db.Compact();
#if (!DEBUG)
                        catch (System.Exception ex )
                        {
                            logfun("<span class=\"red\">Errore: " + ex.Message + "\r\n" + ex.StackTrace + "</span>");
                        }
#endif
            logfun("Compattazione terminata<br>");

          }
          logfun("Rilasco l'applicazione<br/>");
        }
      }

      /// <summary>
      /// Restituisce il nome del file di configurazione predefinito
      /// </summary>
      /// <returns></returns>
      protected virtual string GetConfigFileName()
      {
        return this.MapPath("/App_Data/minidom/minidom.conf");
      }


      private SiteConfig _config = null;

      /// <summary>
      /// Accede alla configurazione dell'app
      /// </summary>
      public SiteConfig WebSiteConfig
      {
        get
        {
          lock (this.applicationLock)
          {
            if (this._config is null)
            {
              string fName = this.GetConfigFileName();
              if (System.IO.File.Exists(fName))
              {
                this._config = SiteConfig.Load(fName);
              }
              else
              {
                this._config = new SiteConfig();
                IOUtils.CreateFolderRecursivelly(System.IO.Path.GetDirectoryName(fName));
                SiteConfig.Save(this._config, fName);
              }
              this.ApplyConfiguration(this._config);
            }
            return this._config;
          }
        }
        set
        {
          lock (this.applicationLock)
          {
            this._config = value;
            this.ApplyConfiguration(this._config);
            string fName = this.GetConfigFileName();
            if (!string.IsNullOrEmpty(fName))
            {
              IOUtils.CreateFolderRecursivelly(System.IO.Path.GetDirectoryName(fName));
              SiteConfig.Save(this._config, fName);
            }
          }
          this.doConfigChanged(new EventArgs());
        }
      }

      /// <summary>
      /// Applica la configurazione
      /// </summary>
      /// <param name="value"></param>
      protected virtual void ApplyConfiguration(SiteConfig value)
      {
        Sistema.IndexingService.MaxCacheSize = (value.CRMMaxCacheSize > 1) ? value.CRMMaxCacheSize : 1;
        Sistema.IndexingService.UnloadFactor = Math.Max(value.CRMUnloadFactor, 0.25f);
        DBUtils.StopStatistics = !value.LogDBCommands;
      }



      /// <summary>
      /// Genera l'evento ConfigChanged
      /// </summary>
      /// <param name="e"></param>
      protected virtual void doConfigChanged(System.EventArgs e)
      {
        if (this.ConfigChanged is object)
          this.ConfigChanged.Invoke(this, e);
      }
      /// <summary>
      /// Compatta il database
      /// </summary>
      /// <param name="db"></param>
      /// <param name="logfun"></param>
      public virtual void CompactDB(DBConnection db, LogFunction logfun)
      {
        lock (applicationLock)
        {
          string tmpPath = Path.GetTempFileName(); // Me.MapPath("/public/temp/minidom/tmpDB.tmp")
          string dbPath = db.DriverName;
          // 
          logfun("Inizio la compattazione di \"" + dbPath + "\" in \"" + tmpPath + "\"<br/>");
          db.CompactDatabase();
          logfun("Compattazione terminata<br>");
        }
      }

      void IWebApplicationContext.SaveConfiguration(SiteConfig value)
      {
        SiteConfig.Save(value, this.GetConfigFileName());
      }

      SiteConfig IWebApplicationContext.LoadConfiguration()
      {
        return SiteConfig.Load(this.GetConfigFileName());
      }
    }
  }
}