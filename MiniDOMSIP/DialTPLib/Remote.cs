using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.WebSite;

namespace minidom.PBX
{
    public class Remote
    {
        public const string __FSEENTRYSVC = "/widgets";
        public int lResolve = 60 * 1000;
        public int lConnect = 60 * 1000;
        public int lSend = 120 * 1000;
        public int lReceive = 120 * 1000;

        public static event RequireUserLoginEventHandler RequireUserLogin;

        public delegate void RequireUserLoginEventHandler(object sender, EventArgs e);

        public static event UserLoggedInEventHandler UserLoggedIn;

        public delegate void UserLoggedInEventHandler(object sender, UserLoginEventArgs e);

        public static event UserLoggedOutEventHandler UserLoggedOut;

        public delegate void UserLoggedOutEventHandler(object sender, UserLogoutEventArgs e);

        public static event UploadProgressEventHandler UploadProgress;

        public delegate void UploadProgressEventHandler(object sender, UploadProgressChangedEventArgs e);

        public static event UploadCompletedEventHandler UploadCompleted;

        public delegate void UploadCompletedEventHandler(object sender, UploadFileCompletedEventArgs e);

        // Public Shared WithEvents client As New WebClient
        public static Sistema.CUser CurrentUser = null;
        private static CSessionInfo m_CurrentSession = null;
        private static Anagrafica.CAzienda m_AziendaPrincipale = null;
        private static string logToken = "";
        private static int uploadCount = 0;

        public static Sistema.CUser CheckUserLogged()
        {
            if (CurrentUser is null)
            {
                RequireUserLogin?.Invoke(null, new EventArgs());
            }

            if (CurrentUser is null)
                throw new InvalidOperationException("Utente non connesso");
            return CurrentUser;
        }

        public static Anagrafica.CAzienda AziendaPrincipale
        {
            get
            {
                CheckUserLogged();
                if (m_AziendaPrincipale is null)
                {
                    string url = "/widgets/websvcf/dialtp.aspx?_a=GetAziendaPrincipale";
                    string tmp = DMD.XML.Utils.Serializer.DeserializeString(Sistema.RPC.InvokeMethod(getServerName() + url));
                    if (!string.IsNullOrEmpty(tmp))
                        m_AziendaPrincipale = (Anagrafica.CAzienda)DMD.XML.Utils.Serializer.Deserialize(tmp);
                }

                return m_AziendaPrincipale;
            }

            set
            {
                m_AziendaPrincipale = value;
            }
        }


        // Public shared Function InvokeMethod(ByVal methodName As String, ByVal ParamArray params() As Object) As String
        // Dim buf As New System.Text.StringBuilder
        // buf.Append(getServerName)
        // buf.Append(methodName)

        // If (params IsNot Nothing AndAlso params.Length > 0) Then
        // If (InStr(methodName, "?") <= 0) Then
        // buf.Append("?")
        // Else
        // buf.Append("&")
        // End If
        // For i As Integer = 0 To UBound(params) Step 2
        // If (i > 0) Then buf.Append("&")
        // buf.Append(params(i))
        // buf.Append("=")
        // buf.Append(DMD.Strings.URLEncode(params(i + 1)))
        // Next
        // End If

        // Dim ret As String = client.DownloadString(buf.ToString)

        // If (Left(ret, 2) = "00") Then
        // ret = Mid(ret, 3)
        // Else
        // Throw New Exception("RPC: Error 0x" & Left(ret, 2) & vbCrLf & Mid(ret, 3))
        // End If
        // Return ret
        // End Function

        public static string GetLokToken()
        {
            if (string.IsNullOrEmpty(logToken))
            {
                string url = "/widgets/websvcf/dialtp.aspx?_a=GetSessionID";
                logToken = DMD.XML.Utils.Serializer.DeserializeString(Sistema.RPC.InvokeMethod(getServerName() + url));
            }

            return logToken;
        }

        public static void Login(string userName, string password)
        {
            string url = "/widgets/websvcf/dialtp.aspx?_a=UserLogin";
            Sistema.RPC.sessionID = GetLokToken();
            string tmp = Sistema.RPC.InvokeMethod(DMDSIPApp.CurrentConfig.ServerName + url, "u", Sistema.RPC.str2n(userName), "p", Sistema.RPC.str2n(password));
            CurrentUser = (Sistema.CUser)DMD.XML.Utils.Serializer.Deserialize(tmp);
            if (CurrentUser is object)
            {
                switch (DMD.Strings.LCase(CurrentUser.UserName) ?? "")
                {
                    case "system":
                    case "guest":
                        {
                            CurrentUser = null;
                            throw new InvalidOperationException("Utente non ammesso");
                            break;
                        }

                    default:
                        {
                            UserLoggedIn?.Invoke(null, new UserLoginEventArgs(CurrentUser));
                            break;
                        }
                }
            }
        }

        public static CCollection<Anagrafica.CPersonaInfo> FindPersone(string text)
        {
            CheckUserLogged();
            string url = getServerName() + "/widgets/websvcf/dialtp.aspx?_a=FindPersona";
            var filter = new Anagrafica.CRMFindParams();
            filter.Text = text;
            filter.nMax = 500;
            Sistema.RPC.sessionID = GetLokToken();
            string tmp = Sistema.RPC.InvokeMethod(url, "filter", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(filter)));
            var items = new CCollection<Anagrafica.CPersonaInfo>();
            if (!string.IsNullOrEmpty(tmp))
                items.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
            items.Sort();
            return items;
        }

        public static object FindInfoNumbero(string serverName, string text)
        {
            var ret = new CKeyCollection();
            serverName = DMD.Strings.Trim(serverName);
            if (!serverName.EndsWith("/"))
                serverName += "/";
            string url = serverName + "widgets/websvcf/dialtp.aspx?_a=FindInfoNumbero";
            var filter = new Anagrafica.CRMFindParams();
            filter.Text = text;
            filter.nMax = 500;
            // RPC.sessionID = GetLokToken()
            string tmp = Sistema.RPC.InvokeMethod(url, "filter", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(filter)));
            if (!string.IsNullOrEmpty(tmp))
                ret = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
            return ret;
        }

        public static Sistema.AsyncState FindInfoNumberoAsync(string serverName, string text, Sistema.IRPCCallHandler handler)
        {
            // If (Remote.CurrentUser Is Nothing) Then
            // Remote.Login(My.Settings.UserName, My.Settings.Password)
            // End If
            serverName = DMD.Strings.Trim(serverName);
            if (!serverName.EndsWith("/"))
                serverName += "/";
            string url = serverName + "widgets/websvcf/dialtp.aspx?_a=FindInfoNumbero";
            var filter = new Anagrafica.CRMFindParams();
            filter.Text = text;
            filter.nMax = 500;
            // RPC.sessionID = GetLokToken()
            return Sistema.RPC.InvokeMethodAsync(url, handler, "filter", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(filter)));
        }

        public static Sistema.AsyncState FindPersoneAsync(string serverName, string text, Sistema.IRPCCallHandler handler)
        {
            // If (Remote.CurrentUser Is Nothing) Then
            // Remote.Login(My.Settings.UserName, My.Settings.Password)
            // End If
            // getServerName()

            string url = serverName + "/widgets/websvcf/dialtp.aspx?_a=FindPersona";
            var filter = new Anagrafica.CRMFindParams();
            filter.Text = text;
            filter.nMax = 500;
            // RPC.sessionID = GetLokToken()
            return Sistema.RPC.InvokeMethodAsync(url, handler, "filter", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(filter)));
            // Dim items As New CCollection(Of CPersonaInfo)
            // items.Sort()
            // If (tmp <> "") Then items.AddRange(minidom.XML.Utils.Serializer.Deserialize(tmp))

            // Return items
        }

        public static bool IsLogged()
        {
            return DBUtils.GetID(CurrentUser) != 0 && DMD.Strings.LCase(CurrentUser.UserName) != "guest";
        }

        public static void Logout()
        {
            // Try
            // Throw New NotImplementedException
            // Catch ex As Exception
            // minidom.Sistema.Events.NotifyUnhandledException(ex)
            // MsgBox(ex, MsgBoxStyle.Critical)
            // End Try
            var u = CurrentUser;
            Sistema.RPC.sessionID = GetLokToken();
            Sistema.RPC.InvokeMethod(getServerName() + "/widgets/websvcf/dialtp.aspx?_a=CurrentUserLogOut");
            CurrentUser = null;
            UserLoggedOut?.Invoke(null, new UserLogoutEventArgs(u, Sistema.LogOutMethods.LOGOUT_LOGOUT));
        }

        public static Anagrafica.CPersona GetPersonaById(int id)
        {
            if (id == 0)
                return null;
            CheckUserLogged();
            string url = "/widgets/websvcf/dialtp.aspx?_a=GetPersonaById";
            Sistema.RPC.sessionID = GetLokToken();
            string tmp = Sistema.RPC.InvokeMethod(getServerName() + url, "id", Sistema.RPC.int2n(id));
            if (!string.IsNullOrEmpty(tmp))
            {
                return (Anagrafica.CPersona)DMD.XML.Utils.Serializer.Deserialize(tmp);
            }
            else
            {
                return null;
            }
        }

        public static CCollection<Anagrafica.CContatto> GetRecapitiPersonaById(Anagrafica.CPersona p)
        {
            CheckUserLogged();
            var ret = new CCollection<Anagrafica.CContatto>();
            string url = "/widgets/websvcf/dialtp.aspx?_a=LoadContattiPersona";
            Sistema.RPC.sessionID = GetLokToken();
            string tmp = Sistema.RPC.InvokeMethod(getServerName() + url, "pid", Sistema.RPC.int2n(DBUtils.GetID(p)));
            if (!string.IsNullOrEmpty(tmp))
            {
                ret.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(tmp));
            }

            return ret;
        }

        public static string Upload(string fileName)
        {
            uploadCount += 1;
            var info = new FileInfo(fileName);
            var data = info.CreationTime;
            string serverName = DMDSIPApp.CurrentConfig.UploadServer;
            string userName = DMDSIPApp.CurrentConfig.UserName;
            string tmpName = Path.GetTempFileName();
            File.Copy(fileName, tmpName, true);
            string url = serverName + "/widgets/websvc/dialtpuploader.aspx?p=" + System.Environment.MachineName + "&u=" + userName + "&f=" + fileName + "&d=" + Sistema.RPC.date2n(data);
            var nvc = new NameValueCollection();
            // nvc.Add("id", "TTR")
            nvc.Add("File0", "Upload");
            string ret = Sistema.RPC.HttpUploadFile(url, tmpName, "file", "image/jpeg", nvc);
            File.Delete(tmpName);
            return ret;
        }

        public static WebSite.CUploadedFile GetUploadedFileByKey(string key)
        {
            CheckUserLogged();
            string url = "/widgets/websvcf/dialtp.aspx?_a=GetUoloadByKey";
            Sistema.RPC.sessionID = GetLokToken();
            string tmp = Sistema.RPC.InvokeMethod(getServerName() + url, "rk", Sistema.RPC.str2n(Sistema.RPC.sessionID + "_" + DBUtils.GetID(CurrentUser) + "_" + key));
            if (!string.IsNullOrEmpty(tmp))
                return (WebSite.CUploadedFile)DMD.XML.Utils.Serializer.Deserialize(tmp);
            return null;
        }

        public static string getServerName()
        {
            return DMDSIPApp.CurrentConfig.ServerName;
        }

        public static WebSite.CSessionInfo CurrentSession
        {
            get
            {
                CheckUserLogged();
                if (m_CurrentSession is null)
                {
                    string url = "/widgets/websvcf/dialtp.aspx?_a=GetCurrentSession";
                    string tmp = DMD.XML.Utils.Serializer.DeserializeString(Sistema.RPC.InvokeMethod(getServerName() + url));
                    if (!string.IsNullOrEmpty(tmp))
                        m_CurrentSession = (WebSite.CSessionInfo)DMD.XML.Utils.Serializer.Deserialize(tmp);
                }

                return m_CurrentSession;
            }

            set
            {
                m_CurrentSession = value;
            }
        }

        public static object SaveObject(object o)
        {
            CheckUserLogged();
            string url = "/widgets/websvcf/dialtp.aspx?_a=SaveObject";
            string text = DMD.XML.Utils.Serializer.Serialize(o);
            string tmp = Sistema.RPC.InvokeMethod(getServerName() + url, "text", Sistema.RPC.str2n(text));
            return DMD.XML.Utils.Serializer.Deserialize(tmp);
        }

        public static CustomerCalls.FaxDocument SendFax(CustomerCalls.FaxDocument doc)
        {
            CheckUserLogged();
            string url = __FSEENTRYSVC + "/websvcf/dialtp.aspx?_a=SendFax";
            string text = DMD.XML.Utils.Serializer.Serialize(doc);
            string tmp = Sistema.RPC.InvokeMethod(getServerName() + url, "text", Sistema.RPC.str2n(text));
            return (CustomerCalls.FaxDocument)DMD.XML.Utils.Serializer.Deserialize(tmp);
        }
    }
}