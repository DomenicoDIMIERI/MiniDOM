/* TODO ERROR: Skipped DefineDirectiveTrivia *//* TODO ERROR: Skipped DefineDirectiveTrivia */
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.UI;
using DMD.Licensing;
using DMD;
using DMD.XML;
using minidom.License;

namespace minidom
{
    public partial class WebSite
    {
        public class WebPageEx : Page
        {
            static WebPageEx()
            {
                timer = InitTimer();
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public static int DELAYTONOTIFY = 5000;
            private static ArrayList pendingRequests = new ArrayList();
            private static System.Timers.Timer _timer;

            private static System.Timers.Timer timer
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return _timer;
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (_timer != null)
                    {
                        _timer.Elapsed -= timer_Elapsed;
                    }

                    _timer = value;
                    if (_timer != null)
                    {
                        _timer.Elapsed += timer_Elapsed;
                    }
                }
            }

            private static int requestCount = 0;

            public sealed class reqInfo
            {
                // Implements IDisposable

                public string UserName;
                public string Name;
                public WebPageEx page;
                public string pageUrl;
                public int Timeout;
                public DateTime StartTime;
                public DateTime? EndTime;
                public Thread requetThread;

                public reqInfo(WebPageEx page, int timeout)
                {
                    UserName = Sistema.Users.CurrentUser.Nominativo;
                    Name = page.GetType().FullName;
                    this.page = page;
                    Timeout = timeout;
                    StartTime = DMD.DateUtils.Now();
                    EndTime = default;
                    requetThread = Thread.CurrentThread;
                    pageUrl = this.page.Request.Url.ToString();
                }

                public override string ToString()
                {
                    return pageUrl;
                }

                public void NotifyEnd()
                {
                    EndTime = DMD.DateUtils.Now();
                    requetThread = null;
                    page = null;
                }

                public bool IsRunning()
                {
                    return EndTime.HasValue == false;
                }

                public int ExecTime
                {
                    get
                    {
                        if (IsRunning())
                        {
                            return (int)Math.Floor((DMD.DateUtils.Now() - StartTime).TotalMilliseconds);
                        }
                        else
                        {
                            return (int)Math.Floor((EndTime.Value - StartTime).TotalMilliseconds);
                        }
                    }
                }
            }

            private static System.Timers.Timer InitTimer()
            {
                var ret = new System.Timers.Timer();
                ret.Interval = 500d;
                ret.Enabled = true;
                return ret;
            }

            public static reqInfo[] GetPendingRequests()
            {
                lock (pendingRequests)
                    return (reqInfo[])pendingRequests.ToArray(typeof(reqInfo));
            }

            private static void timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                lock (pendingRequests)
                {
                    int i = 0;
                    while (i < pendingRequests.Count)
                    {
                        reqInfo req = (reqInfo)pendingRequests[i];
                        // req.ExecTime += Me.timer.Interval
                        if (req.ExecTime > req.Timeout)
                        {
                            pendingRequests.RemoveAt(i);
                            if (req.IsRunning())
                            {
                                Sistema.ApplicationContext.Log(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - " + req.UserName + " - Timeout dello script: " + req.pageUrl + " (" + req.Timeout + ")");
                                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                            }


                            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */                        // req.Dispose()
                            req = null;
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                }
            }

            private static bool _licensed = false;
            private static MiniDOMLicence _lic = null;
            private static string _msg = string.Empty;
            private static LicenseStatus _status = LicenseStatus.UNDEFINED;
            private static DateTime _startTime = DateTime.Now;

            public static bool VerifyLicenseFile(string licFile, ref LicenseStatus _status, ref string _msg)
            {
                // Read public key from assembly
                var _assembly = typeof(WebPageEx).Assembly;
                byte[] _certPubicKeyData;
                using (var _mem = new MemoryStream())
                {
                    _assembly.GetManifestResourceStream("minidom.MiniDOM.cer").CopyTo(_mem);
                    _certPubicKeyData = _mem.ToArray();
                }

                // var linfo = New DMD.Licensing.Helpers.LicenseInfo(;

                // Check if the XML license file exists
                if (File.Exists(licFile))
                {
                    _lic = (MiniDOMLicence)LicenseHandler.ParseLicenseFromBASE64String(typeof(MiniDOMLicence), File.ReadAllText(licFile), _certPubicKeyData, out _status, out _msg);
                }
                else
                {
                    _status = LicenseStatus.INVALID;
                    _msg = "Your copy of this application is not activated";
                }

                switch (_status)
                {
                    case LicenseStatus.VALID:
                        {

                            // //TODO If License Is valid Then, you can Do extra checking here
                            // //TODO: E.g., check license expiry date if you have added expiry date property to your license entity
                            // //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                            // //Here for demo, just show the license information And RETURN without additional checking       
                            // licInfo.ShowLicenseInfo(_lic);
                            string str1 = DMD.Strings.Trim(_lic.SiteURL).ToUpper().TrimEnd('/');
                            string str2 = DMD.Strings.Trim(ASP_Server.MapPath("/")).ToUpper().TrimEnd('/');
                            if (CultureInfo.CurrentCulture.CompareInfo.Compare(str1 ?? "", str2 ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            {
                                _status = _lic.DoExtraValidation(out _msg);
                                _licensed = _status == LicenseStatus.VALID;
                            }
                            else
                            {
                                _status = LicenseStatus.CRACKED;
                                _msg = "License is Expired " + DMD.Strings.vbNewLine + _lic.SiteURL + DMD.Strings.vbNewLine + ASP_Server.MapPath("/");
                                _licensed = false;
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                        // for the other status of license file, show the warning message
                        // And also popup the activation form for user to activate your application
                        // MessageBox.Show(_msg, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Using (frmActivation frm = New frmActivation())
                        // {
                        // frm.CertificatePublicKeyData = _certPubicKeyData;
                        // frm.ShowDialog();

                        // //Exit the application after activation to reload the license file 
                        // //Actually it Is Not nessessary, you may just call the API to reload the license file
                        // //Here just simplied the demo process

                        // Application.Exit();
                        // }
                        // break;
                }

                return _licensed;
            }

            public static bool ValidateLicense(string licFile, string licenseString, ref LicenseStatus _status, ref string _msg)
            {
                // Read public key from assembly
                var _assembly = typeof(WebPageEx).Assembly;
                byte[] _certPubicKeyData;
                using (var _mem = new MemoryStream())
                {
                    _assembly.GetManifestResourceStream("minidom.MiniDOM.cer").CopyTo(_mem);
                    _certPubicKeyData = _mem.ToArray();
                }

                bool ret = LicenseHandler.ValidateLicense<MiniDOMLicence>(licenseString, _certPubicKeyData, out _status, out _msg);
                if (ret)
                {
                    // If license if valid, save the license string into a local file
                    File.WriteAllText(licFile, licenseString);
                }

                return ret;
            }

            public bool IsLicenced()
            {
                // Initialize variables with default values
                if (_lic is object)
                {
                    return _licensed;
                }

                // Read public key from assembly

                // var linfo = New DMD.Licensing.Helpers.LicenseInfo(;

                // Check if the XML license file exists
                string licFile = AC.MapPath("/App_Data/MiniDOM/license.lic");
                _licensed = VerifyLicenseFile(licFile, ref _status, ref _msg);
                return _licensed;
            }

            public DateTime? GetInstallationDate()
            {
                // Initialize variables with default values
                if (!IsLicenced())
                    return default;
                return _lic.CreateDateTime;
            }




            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            public reqInfo req;

            // Protected WebLock As New Object

            private CCurrentPage m_CurrentPage;
            private Sistema.CModule m_CurrentModule;
            private string m_a;

            public WebPageEx()
            {
                CheckLicense();
                DMDObject.IncreaseCounter(this);
                CheckWebSite();
            }

            private void CheckLicense()
            {
                if (IsLicenced())
                    return;
                var duration = DateTime.Now - _startTime;
                if (duration.TotalSeconds > 120d)
                    throw new Exception("Licenza Scaduta");
            }

            /// <summary>
        /// Restituisce true se l'applicazione è compilata in modalità debug
        /// </summary>
        /// <returns></returns>
            public bool IsDebug()
            {
                return AC.IsDebug();
            }


            /// <summary>
        /// Restituisce il contesto dell'applicazione "Sistema.ApplicationContext" convertito nel tipo WebApplicationContext
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public WebApplicationContext ApplicationContext
            {
                get
                {
                    return (WebApplicationContext)Sistema.ApplicationContext;
                }
            }



            /// <summary>
        /// Effettua i controlli sul sito
        /// </summary>
        /// <remarks></remarks>
            protected virtual void CheckWebSite()
            {
            }


            /// <summary>
        /// Restituisce vero se l'accesso alla pagina genera un log
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool IsLogEnabled()
            {
                return Instance.Configuration.LogPages;
            }

            /// <summary>
        /// Restituisce vero se l'accesso alla pagina richiede che l'utente effettui il login
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool RequiresLogin()
            {
                return false;
            }




            /// <summary>
        /// Restituisce o imposta il modulo corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual Sistema.CModule CurrentModule
            {
                get
                {
                    if (m_CurrentModule is null)
                    {
                        string m = GetParameter("_m", "");
                        if (DMD.Strings.IsNumeric(m))
                        {
                            m_CurrentModule = Sistema.Modules.GetItemById(DMD.Integers.CInt(m));
                        }
                        else
                        {
                            m_CurrentModule = Sistema.Modules.GetItemByName(m);
                        }
                    }

                    return m_CurrentModule;
                }
            }

            // Public ReadOnly Property ModuleContext As String
            // Get
            // Return Me
            // End Get
            // End Property

            private IModuleHandler _currentModuleHandler = null;

            public virtual IModuleHandler CurrentModuleHandler
            {
                get
                {
                    if (_currentModuleHandler is null && CurrentModule is object)
                    {
                        _currentModuleHandler = (IModuleHandler)CurrentModule.CreateHandler(this);
                    }

                    return _currentModuleHandler;
                }
            }

            public virtual string RequestedAction
            {
                get
                {
                    if (string.IsNullOrEmpty(m_a))
                        m_a = DMD.Strings.Trim(GetParameter("_a", ""));
                    return m_a;
                }
            }

            public bool IsUserLogged()
            {
                string sessionID = CurrentSession.SessionID;
                if (string.IsNullOrEmpty(sessionID))
                    return false;
                var user = ((WebApplicationContext)Sistema.ApplicationContext).get_CurrentUser(sessionID);
                switch (Strings.LCase(user.UserName) ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "system", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "guest", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            return false;
                        }

                    default:
                        {
                            return true;
                        }
                }
            }

            public virtual bool IsAllowedIP(string value)
            {
                bool ret = false;
                string lastUserIP = Strings.Trim(DMD.Strings.CStr(Session["LastUserIP"]));
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(lastUserIP ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                {
                    // Prima convalida dell'IP
                    var info = Instance.GetIPAllowInfo(value);
                    if (info is null)
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = info.Allow && !info.Negate;
                    }

                    Session["LastUserIP_Res"] = ret;
                    Session["LastUserIP"] = value;
                }
                else if (CultureInfo.CurrentCulture.CompareInfo.Compare(lastUserIP ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    // L'IP è già stato convalidato in questa sessione
                    ret = DMD.Booleans.CBool(Session["LastUserIP_Res"]);
                }

                return ret;
            }

            /// <summary>
        /// Questa funzione viene richiamanta per indicare un tentativo di accesso anomalo al sistema
        /// da un IP che è cambiato durante la sessione
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            protected virtual bool SegnalaIPAnomalo(string value, bool consentito)
            {
                return false;
            }

            public virtual bool IsNegateIP(string value)
            {
                return Instance.IsIPNegated(value);
            }

            public virtual bool IsAllowedCertificate(HttpClientCertificate value)
            {
                if (value.IsPresent == false)
                    return false;
                if (value.ValidFrom > DMD.DateUtils.Now() || value.ValidUntil < DMD.DateUtils.Now())
                    return false;
                return true;
            }



            // Restituisce vero se l'orario di sistema indica 
            public bool IsWorkTime()
            {
                // Dim days, intervals, i
                // Dim wd, hh, hh1, hh2, t

                // days = Split(WORKDAYS, ", ")
                // wd = WeekDay(Now, 2)
                // hh = CDbl(Hour(Now)) + CDbl(Minute(Now)) / 60

                // t = False
                // i = 0
                // While (i <= UBound(days)) And Not t
                // t = (wd = CInt(days(i)))
                // i = i + 1
                // End While
                // If t Then
                // intervals = Split(WORKHOURS, ", ")
                // t = False
                // i = 0
                // While (i <= UBound(intervals)) And Not t
                // Dim times, nibbles
                // times = Split(intervals(i), ": ")
                // nibbles = Split(times(0), ".")
                // hh1 = CDbl(nibbles(0)) + CDbl(nibbles(1)) / 60
                // nibbles = Split(times(1), ".")
                // hh2 = CDbl(nibbles(0)) + CDbl(nibbles(1)) / 60
                // t = (hh >= hh1) And (hh <= hh2)
                // i = i + 1
                // End While
                // End If

                // IsWorkTime = t
                return true;
            }

            /// <summary>
        /// Verifica il protocollo di connessione (tipo HTTP o HTTPs)
        /// </summary>
            protected virtual void SecurityCheckProtocol()
            {
            }

            /// <summary>
        /// Effettua i controlli per l'esecuzione della pagina
        /// </summary>
            protected virtual void PerformSecurityChecks()
            {
                SecurityCheckProtocol();
                SecurityCheckMaintenance();
                SecurityCheckTimeRestrictions();
                SecurityCheckRemoteIP();
                SecurityCheckValidUser();
            }

            /// <summary>
        /// Verifica che l'utente che ha effettuato la richiesta remota sia valido (SystemUser non può effettuare richieste remote)
        /// </summary>
            protected virtual void SecurityCheckValidUser()
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(Sistema.Users.CurrentUser.UserName ?? "", Sistema.Users.KnownUsers.SystemUser.UserName ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    NotifyUnhautorized("Metodo di accesso non valido");
                }
            }

            protected virtual void SecurityCheckMaintenance()
            {
                if (Instance.IsMaintenance() || Sistema.FileSystem.FileExists(Server.MapPath("/maintenance.html")))
                {
                    CurrentPage.TransferredTo("/maintenance.html");
                    Server.Transfer("/maintenance.html");
                    Response.End();
                }
            }

            protected virtual void SecurityCheckTimeRestrictions()
            {
                if (Instance.Configuration.VerifyTimeRestrictions && !IsWorkTime())
                {
                    NotifyUnhautorized("Accesso non consentito fuori gli orari prestabiliti");
                }
            }

            protected virtual void SecurityCheckRemoteIP()
            {
                if (AC.IsDebug())
                    return;
                string remoteIP = Request.ServerVariables["REMOTE_ADDR"];
                // If (Len(remoteIP) < 5) Then remoteIP = "0.0.0.0"

                if (Instance.Configuration.VerifyRemoteIP && !IsAllowedIP(remoteIP))
                {
                    NotifyUnhautorized("Tentativo di accesso da un IP non consentito: " + remoteIP);
                }
            }

            protected virtual void SecurityCheckClientCertificate()
            {
                if (Instance.Configuration.VerifyClientCertificate && !IsAllowedCertificate(Request.ClientCertificate) && !IsNegateIP(Request.ServerVariables["REMOTE_ADDR"]))
                {
                    NotifyUnhautorized("Certificato client non valido o IP bloccato esplicitamente");
                }
            }

            public virtual void NotifyUnhautorized(string msg)
            {
                // HttpContext.Current.Response.Headers.Add("WWW-Authenticate", "Basic")

                // Me.CurrentPage.EndExecution("200.403", msg)
                CurrentPage.NotifyUnhautorized(msg);

                // Dim last_msg_unhauth As String = Session("last_msg_unhauth")
                // If (last_msg_unhauth = msg) Then
                // Response.End()
                // Return
                // End If
                // Session("last_msg_unhauth") = msg
                Response.Write("FF ");
                Response.Write(msg);
                Response.End();
            }

            protected virtual void ReadCookies()
            {
                // If Me.GetParameter("interface") <> "0" Then
                // End If
            }

            protected virtual void WriteCookies()
            {
                Cookies.SetCookie("_SVRTIME", DMD.XML.Utils.Serializer.SerializeDate(DMD.DateUtils.Now()));
            }

            protected override void OnInit(EventArgs e)
            {
                ReadCookies();
                base.OnInit(e);
            }

            protected override void OnSaveStateComplete(EventArgs e)
            {
                base.OnSaveStateComplete(e);
            }

            /// <summary>
        /// Restituisce il timeout predefinito per la pagina (in secondi)
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            protected virtual int GetDefaultTimeOut()
            {
                int ret = Instance.Configuration.LongTimeOut;
                if (ret <= 1)
                    ret = 30;
                return ret;
            }

            public WebApplicationContext AC
            {
                get
                {
                    return (WebApplicationContext)Sistema.ApplicationContext;
                }
            }

            protected override void OnPreInit(EventArgs e)
            {
                lock (pendingRequests)
                {
                    req = new reqInfo(this, GetDefaultTimeOut() * 1000);
                    pendingRequests.Add(req);
                }

                VisitedPage page = CurrentPage;
                if (CheckGZIP())
                    GZipEncodePage();
                StartExecution();
                if (IsLogEnabled())
                    SavePageLogInfo();
                base.OnPreInit(e);
                PerformSecurityChecks();
                WriteCookies();
            }

            protected override void OnLoad(EventArgs e)
            {
                AC.UpdateCurrentSession();
                base.OnLoad(e);
            }

            /// <summary>
        /// Funziona richiata dal metodo OnLoad (se IsLogEnabled restituisce true) allo scopo di salvare le informazioni sulla pagina
        /// </summary>
        /// <remarks></remarks>
            protected virtual void SavePageLogInfo()
            {
                CurrentPage.SaveLog();
            }

            // Protected Overridable Sub EndStatsInfo()
            // 'Try
            // Me.AC.GetCurrentSessionInfo.EndPage(Me.m_GCInfo)
            // 'Catch ex As Exception
            // 'Throw
            // 'End Try
            // End Sub

            protected override void OnUnload(EventArgs e)
            {
                try
                {
                    if (m_CurrentPage is object)
                    {
                        if (IsLogEnabled())
                            SavePageLogInfo();
                        if (string.IsNullOrEmpty(CurrentPage.StatusCode))
                        {
                            EndExecution("200", "");
                        }
                        else
                        {
                            EndExecution(CurrentPage.StatusCode, CurrentPage.StatusDescription);
                        }
                    }
                    else
                    {
                        AC.Log(ClientQueryString.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                }

                lock (pendingRequests)
                {
                    if (req is object)
                    {
                        try
                        {
                            req.NotifyEnd();
                            pendingRequests.Remove(req);
                            req = null;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                base.OnUnload(e);
            }

            protected override void OnError(EventArgs e)
            {
                try
                {
                    lock (pendingRequests)
                    {
                        if (req is object)
                        {
                            try
                            {
                                req.NotifyEnd();
                                pendingRequests.Remove(req);
                                req = null;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                base.OnError(e);
            }


            /// <summary>
        /// Restituisce vero se la pagina
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            protected virtual bool CheckGZIP()
            {
                return Instance.Configuration.CompressResponse;
            }

            protected override void Render(HtmlTextWriter writer)
            {
                var startTime = DMD.DateUtils.Now();
                if (AC.IsDebug())
                {
                    InternalRender(writer);
                }
                else
                {
                    try
                    {
                        InternalRender(writer);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                    }
                }
            }

            protected virtual void InternalRender(HtmlTextWriter writer)
            {
                base.Render(writer);
            }


            // Public Function GetParameter1(ByVal paramName As String) As String
            // Return Me.GetParameter(paramName)
            // End Function

            public T? GetParameter<T>(string paramName, object defValue = null) where T : struct
            {
                if (!IsParameterSet(paramName))
                    return (T?)defValue;
                var tp = typeof(T);
                string param = GetParameter(paramName, "");
                object ret = null;
                if (ReferenceEquals(tp, typeof(string)))
                    ret = Sistema.Formats.ToString(param);
                if (ReferenceEquals(tp, typeof(bool)))
                    ret = Sistema.Formats.ParseBool(param);
                if (ReferenceEquals(tp, typeof(int)))
                    ret = Sistema.Formats.ParseInteger(param);
                if (ReferenceEquals(tp, typeof(double)))
                    ret = Sistema.Formats.ParseDouble(param);
                if (ReferenceEquals(tp, typeof(decimal)))
                    ret = Sistema.Formats.ParseValuta(param);
                if (ReferenceEquals(tp, typeof(DateTime)))
                    ret = Sistema.Formats.ParseDate(param);
                return (T?)ret;
            }

            public string GetParameter(string paramName, string defValue)
            {
                System.Collections.Specialized.NameObjectCollectionBase.KeysCollection keys = null;
                if (Request.QueryString.HasKeys())
                {
                    keys = Request.QueryString.Keys;
                    foreach (string key in keys)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(key ?? "", paramName ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            return Request.QueryString[paramName];
                    }
                }

                if (Request.Form.HasKeys())
                {
                    keys = Request.Form.Keys;
                    foreach (string key in keys)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(key ?? "", paramName ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            return Request.Form[paramName];
                    }
                }

                return defValue;
            }

            public bool IsParameterSet(string paramName)
            {
                System.Collections.Specialized.NameObjectCollectionBase.KeysCollection keys = null;
                if (Request.QueryString.HasKeys())
                {
                    keys = Request.QueryString.Keys;
                    foreach (string key in keys)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(key ?? "", paramName ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            return true;
                    }
                }

                if (Request.Form.HasKeys())
                {
                    keys = Request.Form.Keys;
                    foreach (string key in keys)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(key ?? "", paramName ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            return true;
                    }
                }

                return false;
            }



            /// <summary>
        /// Restituisce vero se la richiesta proviene da un dispositivo mobile
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsMobileDevice()
            {
                if (IsIPad() | IsIPhone())
                    return true;
                string u = Strings.Trim(Request.ServerVariables["HTTP_USER_AGENT"]);
                var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|iphone|ipdad|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase);
                var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase);
                return b.IsMatch(u) | v.IsMatch(Strings.Left(u, 4));
            }

            /// <summary>
        /// Restituisce vero se il dispositivo remoto viene riconosciuto come un iPad
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsIPad()
            {
                string str = Strings.Trim(Request.ServerVariables["HTTP_USER_AGENT"]);
                var uA = UserAgents.GetItemByString(str);
                if (uA is null)
                {
                    return Strings.InStr(str, "ipad", true) > 0;
                }
                else
                {
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.LCase(uA.Device), "ipad", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;
                }
            }

            /// <summary>
        /// Restituisce vero se il dispositivo remoto viene riconosciuto come un iPhone
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsIPhone()
            {
                string str = Strings.Trim(Request.ServerVariables["HTTP_USER_AGENT"]);
                var uA = UserAgents.GetItemByString(str);
                if (uA is null)
                {
                    return Strings.InStr(str, "iphone", true) > 0;
                }
                else
                {
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.LCase(uA.Device), "iphone", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;
                }
            }

            /// <summary>
        /// Determines if GZip is supported
        /// </summary>
        /// <returns></returns>
            public bool IsGZipSupported()
            {
                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
                return !string.IsNullOrEmpty(AcceptEncoding) && AcceptEncoding.Contains("gzip") | AcceptEncoding.Contains("deflate");
            }

            /// <summary>
        /// Sets up the current page or handler to use GZip through a Response.Filter
        /// IMPORTANT:
        /// You have to call this method before any output is generated!
        /// </summary>
            public void GZipEncodePage()
            {
                if (IsGZipSupported())
                {
                    var Response = HttpContext.Current.Response;
                    string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
                    if (AcceptEncoding.Contains("gzip"))
                    {
                        Response.Filter = new System.IO.Compression.GZipStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
                        Response.AppendHeader("Content-Encoding", "gzip");
                    }
                    else
                    {
                        Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
                        Response.AppendHeader("Content-Encoding", "deflate");
                    }
                }
            }


            /// <summary>
        /// Restituisce un oggetto che descrive la pagina corrente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCurrentPage CurrentPage
            {
                get
                {
                    if (m_CurrentPage is null)
                    {
                        m_CurrentPage = new CCurrentPage();
                        m_CurrentPage.Initialize(this);
                    }

                    return m_CurrentPage;
                }
            }

            public CCurrentSiteSession CurrentSession
            {
                get
                {
                    return (CCurrentSiteSession)Instance.CurrentSession;
                }
            }

            // Private m_TimeStart As Date
            // Private m_GCStart As Long

            public virtual void StartExecution()
            {
                // Me.m_TimeStart = Now
                // Me.m_GCStart = GC.GetTotalMemory(False)
                CurrentPage.StartExecution();
            }

            public virtual void EndExecution(string status, string message)
            {
                CurrentPage.EndExecution(status, message);
                m_CurrentPage = null;
                // Me.m_CurrentModule = Nothing


            }

            /// <summary>
        /// Questo metodo viene richiamato prima di eseguire un'azione e può essere usato per log o per convalidare
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="context"></param>
        /// <remarks></remarks>
            protected virtual void ValidateActionBeforeRun(string actionName, object context)
            {
            }

            public override void Dispose()
            {
                m_a = null;
                m_CurrentModule = null;
                m_CurrentPage = null;
                base.Dispose();
            }

            ~WebPageEx()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}