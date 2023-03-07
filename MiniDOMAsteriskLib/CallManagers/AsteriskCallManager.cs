using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DMD;
using minidom.CallManagers.Actions;
using minidom.CallManagers.Events;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers
{
    public class AsteriskCallManager : IDisposable
    {
        private class Worker : BackgroundWorker
        {
            public AsteriskCallManager o;

            public Worker(AsteriskCallManager o)
            {
                DMDObject.IncreaseCounter(this);
                this.o = o;
            }

            protected override void OnDoWork(DoWorkEventArgs e)
            {
                Listen();
            }

            public void Listen()
            {
                int n = 0;
                do
                {
                    if (CancellationPending)
                    {
                        Debug.Print("Worker Cencelled");
                        return;
                    }

                    var buffer = new byte[1024];
                    n = o.m_Socket.Receive(buffer);
                    if (n > 0)
                    {
                        string s = System.Text.Encoding.ASCII.GetString(buffer, 0, n);
                        lock (o.lockObject)
                            o.m_Buffer += s;
                    }

                    lock (o.lockObject)
                    {
                        int p = Strings.InStr(o.m_Buffer, DMD.Strings.vbCrLf + DMD.Strings.vbCrLf);
                        while (p > 0)
                        {
                            string data = Strings.Left(o.m_Buffer, p - 1);
                            o.m_Buffer = Strings.Mid(o.m_Buffer, p + 4);
                            o.ParseData(data);
                            p = Strings.InStr(o.m_Buffer, DMD.Strings.vbCrLf + DMD.Strings.vbCrLf);
                        }
                    }
                }
                while (true);
            }

            ~Worker()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        public event ConnectedEventHandler Connected;

        public delegate void ConnectedEventHandler(object sender, AsteriskEventArgs e);

        public event DisconnectedEventHandler Disconnected;

        public delegate void DisconnectedEventHandler(object sender, AsteriskEventArgs e);


        /// <summary>
        /// Evento generato quando il manager ci notifica un evento remoto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event ManagerEventEventHandler ManagerEvent;

        public delegate void ManagerEventEventHandler(object sender, AsteriskEvent e);

        /// <summary>
        /// Evento generato quando questo oggetto si autentica correttamente sul manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event LoggedInEventHandler LoggedIn;

        public delegate void LoggedInEventHandler(object sender, ManagerLoginEventArgs e);

        /// <summary>
        /// Evento generato quando questo oggetto chiude la comunicazione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event LoggedOutEventHandler LoggedOut;

        public delegate void LoggedOutEventHandler(object sender, ManagerLogoutEventArgs e);


        /// <summary>
        /// Evento generato quando il sistema riceve una chiamata in ingresso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event DialEventHandler Dial;

        public delegate void DialEventHandler(object sender, DialEvent e);

        protected object lockObject = new object();
        private string m_PeerVersion;
        private string m_Asterisk;
        private int m_Port;
        private Socket m_Socket;
        private IPEndPoint m_ServerEndPoint;
        private string m_UserName;
        private string m_Password;
        private string m_Buffer;
        private string m_Response;
        private ManualResetEvent m_DataLock = new ManualResetEvent(false);
        private ManualResetEvent m_ResponseLock = new ManualResetEvent(false);

        private delegate void ListenDelegate();

        private ListenDelegate m_ListenDelegate;
        private Hashtable m_SupportedEvents = null;
        private Action m_LastAction;
        private bool m_Authenticated;
        private Worker m_Worker1;
        private Channels m_Channels;
        private ActionResponseQueues m_ActionResponseQueues;
        private Peers m_Peers;

        public AsteriskCallManager(string userName, string password, string asteriskServer, int port = 5038)
        {
            DMDObject.IncreaseCounter(this);
            m_UserName = userName;
            m_Password = password;
            m_Asterisk = asteriskServer;
            m_Port = port;
            m_Worker1 = null;
            m_Channels = null;
        }

        public string UserName
        {
            get
            {
                return m_UserName;
            }
        }

        public string AsteriskServer
        {
            get
            {
                return m_Asterisk;
            }
        }

        public int Port
        {
            get
            {
                return m_Port;
            }
        }

        /// <summary>
        /// Restituisce vero se questa istanza è connessa ed autenticata su un server Asterisk
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsConnected()
        {
            return m_Socket is object;
        }

        public void Start() // Implements minidom.CustomerCalls.ICallManager.Start
        {
            if (IsConnected())
                throw new InvalidOperationException("Server Asterisk già connesso");
            lock (lockObject)
            {
                IAsyncResult result;
                m_ServerEndPoint = new IPEndPoint(IPAddress.Parse(m_Asterisk), m_Port);
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                result = m_Socket.BeginConnect(m_ServerEndPoint, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                if (m_Socket.Connected)
                {
                    m_Socket.EndConnect(result);
                }
                else
                {
                    // NOTE, MUST CLOSE THE SOCKET
                    m_Socket.Close();
                    throw new ApplicationException("Failed to connect server.");
                }

                m_Buffer = "";
                var buffer = new byte[1024];
                int n = 0;
                do
                {
                    n = m_Socket.Receive(buffer);
                    if (n > 0)
                    {
                        m_Buffer += System.Text.Encoding.ASCII.GetString(buffer, 0, n);
                        int p = Strings.InStr(m_Buffer, DMD.Strings.vbCrLf);
                        if (p > 0)
                        {
                            m_PeerVersion = Strings.Left(m_Buffer, p - 1);
                            m_Buffer = Strings.Mid(m_Buffer, p + 2);
                            break;
                        }
                    }
                }
                while (n > 0);
                // Me.m_ListenDelegate = AddressOf Me.Listen
                // Me.m_ListenDelegate.BeginInvoke(Nothing, Nothing)
                m_Worker1 = new Worker(this);

                // If (Me.m_Worker.IsBusy = False) Then
                m_Worker1.RunWorkerAsync();
            }
            // End If

        }

        public bool IsAuthenticated()
        {
            return m_Authenticated;
        }

        public void Stop() // Implements minidom.CustomerCalls.ICallManager.Stop
        {
            if (!IsConnected())
                throw new InvalidOperationException("Server Asterisk non connesso");
            lock (lockObject)
            {
                var a = new Logoff();
                LogoffResponse res = (LogoffResponse)Execute(a, 2000);
                Debug.Print(res.ToString());
                m_Authenticated = false;
                m_Worker1.CancelAsync();
                m_Worker1 = null;
                m_Socket.Disconnect(true);
                m_Socket = null;
            }

            Disconnected?.Invoke(this, new AsteriskEventArgs(this));
        }

        public void Login()
        {
            // Me.Send("Action: login" & vbCrLf & "Username: " & Me.m_UserName & vbCrLf & "Secret: " & Me.m_Password & vbCrLf)
            if (!IsConnected())
                throw new InvalidOperationException("Server Asterisk non connesso");
            var a = new Login(m_UserName, m_Password);
            LoginResponse res = (LoginResponse)Execute(a);
            m_Authenticated = res.IsSuccess();
            if (!m_Authenticated)
                throw new Exception("Authentication failed: " + res.Message);
            LoggedIn?.Invoke(this, new ManagerLoginEventArgs(m_UserName, res.Message));
        }

        public void Logout()
        {
            if (!IsConnected())
                throw new InvalidOperationException("Server Asterisk non connesso");
            var a = new Logoff();
            LogoffResponse res = (LogoffResponse)Execute(a);
            m_Authenticated = !res.IsSuccess();
            LoggedOut?.Invoke(this, new ManagerLogoutEventArgs());
        }

        public virtual ActionResponse Execute(Action action)
        {
            if (action.RequiresAuthentication() && IsConnected() == false)
                throw new InvalidOperationException("Il server Asterisk non è connesso");
            m_ResponseLock.Reset();
            m_LastAction = action;
            if (action is AsyncAction)
            {
                ((AsyncAction)action).SetOwner(this);
                ActionResponseQueues.Add(new ActionResponseQueue((AsyncAction)action));
            }

            Debug.Print(action.ToString());
            action.Send(this);
            m_ResponseLock.WaitOne();
            return m_LastAction.Response;
        }

        /// <summary>
        /// Esegue l'azione specificata ed attende la risposta per un massimo di timeOutMs millisecondi
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timeOutMs"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual ActionResponse Execute(Action action, int timeOutMs)
        {
            if (action.RequiresAuthentication() && IsConnected() == false)
                throw new InvalidOperationException("Il server Asterisk non è connesso");
            m_ResponseLock.Reset();
            m_LastAction = action;
            if (action is AsyncAction)
            {
                ((AsyncAction)action).SetOwner(this);
                ActionResponseQueues.Add(new ActionResponseQueue((AsyncAction)action));
            }

            Debug.Print(action.ToString());
            action.Send(this);
            m_ResponseLock.WaitOne(timeOutMs);
            return m_LastAction.Response;
        }

        /// <summary>
        /// Invia un comando e restituisce la risposta in maniera sincrona
        /// </summary>
        /// <param name="command"></param>
        /// <remarks></remarks>
        protected internal virtual void Send(string command)
        {
            var buffer = System.Text.Encoding.ASCII.GetBytes(command + DMD.Strings.vbCrLf);
            m_Socket.Send(buffer);
        }

        private string GetResponse()
        {
            m_Buffer = "";
            m_DataLock.WaitOne();
            return m_Buffer;
        }

        private void ParseData(string data)
        {
            var rows = Strings.Split(data, DMD.Strings.vbCrLf);
            var firstRow = new RowEntry(rows[0]);
            switch (Strings.LCase(firstRow.Command) ?? "")
            {
                case "response":
                    {
                        if (m_LastAction is object)
                        {
                            m_LastAction.ParseResponse(rows);
                            m_ResponseLock.Set();
                        }

                        break;
                    }

                case "event":
                    {
                        parseEvent(firstRow, rows);
                        break;
                    }

                default:
                    {
                        // Debug.Print("Unsupported message: " & firstRow.Command)
                        if (string.IsNullOrEmpty(firstRow.Command) && m_LastAction is object)
                        {
                            m_LastAction.ParseResponse(rows);
                            m_ResponseLock.Set();
                        }

                        break;
                    }
            }
        }

        private void parseEvent(RowEntry r, string[] rows)
        {
            AsteriskEvent e = null;
            Type t = null;
            if (SupportedEvents.ContainsKey(Strings.UCase(r.Params)))
            {
                t = (Type)SupportedEvents[Strings.UCase(r.Params)];
            }

            if (t is null)
            {
                e = new AsteriskEvent();
                e.Parse(rows);
                Debug.Print("EVENTO NON SUPPORTATO!");
                Debug.Print(e.ToString());
            }
            else
            {
                e = (AsteriskEvent)Activator.CreateInstance(t);
                e.Parse(rows);
                Debug.Print(e.ToString());
            }

            dispatchEvent(e);
        }

        protected virtual void dispatchEvent(AsteriskEvent e)
        {
            switch (Strings.UCase(e.EventName) ?? "")
            {
                case "NEWCHANNEL":
                    {
                        Channels.SetUpChannel((Newchannel)e);
                        break;
                    }

                case "HANGUP":
                    {
                        e = new HangupEvent(this, e);
                        Channels.HangUpChannel((HangupEvent)e);
                        break;
                    }

                case "PEERSTATUS":
                    {
                        Peers.UpdatePeerStatus((PeerStatusType)e);
                        break;
                    }

                case "LINK":
                case "BRIDGE":
                case "UNLINK":
                    {
                        Links.Update(e);
                        break;
                    }

                case "DIAL":
                    {
                        e = new DialEvent(this, e);
                        Dial?.Invoke(this, (DialEvent)e);
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(e.ActionID))
            {
                var q = ActionResponseQueues.GetItemByKey(e.ActionID);
                if (q is object)
                {
                    q.Items.Add(e);
                    q.Action.NotifyEvent(e);
                }
            }

            ManagerEvent?.Invoke(this, e);
        }

        private string WaitResponse()
        {
            m_DataLock.WaitOne();
            return m_Response;
        }

        public Hashtable SupportedEvents
        {
            get
            {
                lock (lockObject)
                {
                    if (m_SupportedEvents is null)
                    {
                        m_SupportedEvents = new Hashtable();
                        var a = GetType().Assembly;
                        var list = a.GetTypes();
                        foreach (Type t in list)
                        {
                            if (t.IsSubclassOf(typeof(AsteriskEvent)))
                            {
                                AsteriskEvent e = (AsteriskEvent)a.CreateInstance(t.FullName);
                                m_SupportedEvents.Add(Strings.UCase(e.EventName), t);
                            }
                        }
                    }

                    return m_SupportedEvents;
                }
            }
        }



        // This code added by Visual Basic to correctly implement the disposable pattern.
        public virtual void Dispose()
        {
            Stop();
            m_PeerVersion = DMD.Strings.vbNullString;
            m_Asterisk = DMD.Strings.vbNullString;
            if (m_Socket is object)
            {
                m_Socket.Close();
                m_Socket = null;
            }

            m_ServerEndPoint = null;
            m_UserName = DMD.Strings.vbNullString;
            m_Password = DMD.Strings.vbNullString;
            m_Buffer = DMD.Strings.vbNullString;
            m_Response = DMD.Strings.vbNullString;
            if (m_DataLock is object)
            {
                m_DataLock.Dispose();
                m_DataLock = null;
            }

            if (m_ResponseLock is object)
            {
                m_ResponseLock.Dispose();
                m_ResponseLock = null;
            }

            m_ListenDelegate = null;
            m_SupportedEvents = null;
            m_LastAction = null;
            if (m_Worker1 is object)
            {
                m_Worker1.Dispose();
                m_Worker1 = null;
            }

            m_Channels = null;
            m_ActionResponseQueues = null;
            m_Peers = null;
        }

        ~AsteriskCallManager()
        {
            DMDObject.DecreaseCounter(this);
        }

        public Channels Channels
        {
            get
            {
                if (m_Channels is null)
                    m_Channels = new Channels(this);
                return m_Channels;
            }
        }

        public ActionResponseQueues ActionResponseQueues
        {
            get
            {
                if (m_ActionResponseQueues is null)
                    m_ActionResponseQueues = new ActionResponseQueues(this);
                return m_ActionResponseQueues;
            }
        }

        public Peers Peers
        {
            get
            {
                if (m_Peers is null)
                    m_Peers = new Peers(this);
                return m_Peers;
            }
        }

        private Links m_Links = null;

        public Links Links
        {
            get
            {
                if (m_Links is null)
                    m_Links = new Links(this);
                return m_Links;
            }
        }
    }
}