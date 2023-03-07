using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace minidom.PBX
{
    public sealed class InterfonoService
    {
        private static readonly object synchRoot = new object();
        private static TcpListener _listener;
        private static Thread _listenerThread;
        private static bool m_Quit;
        private static CCollection<Interfono> m_Interfoni = null;

        public static void StartServer(string bindAddress, int bindPort)
        {
            lock (synchRoot)
            {
                m_Quit = false;
                if (_listener is object)
                    throw new InvalidOperationException("Già in ascolto");
                _listener = new TcpListener(IPAddress.Parse(bindAddress), bindPort);
                _listener.Start();
                _listenerThread = new Thread(new ParameterizedThreadStart(Listen));
                _listenerThread.Priority = ThreadPriority.BelowNormal;
                _listenerThread.Start(_listener);
            }
        }

        public static void StopService()
        {
            lock (synchRoot)
            {
                m_Quit = true;
                _listener.Stop();
                _listenerThread.Join(2000);
                _listenerThread.Abort();
                _listener = null;
                _listenerThread = null;
                if (m_Interfoni is object)
                {
                    var col = new CCollection<Interfono>(m_Interfoni);
                    foreach (Interfono i in m_Interfoni)
                    {
                        try
                        {
                            if (i.IsConnected())
                                i.Disconnect();
                        }
                        catch (Exception ex)
                        {
                            Log.LogException(ex);
                        }
                    }

                    m_Interfoni = null;
                }
            }
        }

        public static CCollection<Interfono> Interfoni
        {
            get
            {
                if (m_Interfoni is null)
                    m_Interfoni = GetInterfoni();
                return new CCollection<Interfono>(m_Interfoni);
            }
        }

        public static void Invalidate()
        {
            if (m_Interfoni is null)
                return;
            var oldItems = Interfoni;
            m_Interfoni = GetInterfoni();
            foreach (Interfono i in m_Interfoni)
            {
                foreach (Interfono i1 in oldItems)
                {
                    if (i.UniqueID == i1.UniqueID)
                    {
                        i.Con = i1.Con;
                        break;
                    }
                }
            }
        }

        private static CCollection<Interfono> GetInterfoni()
        {
            var ret = new CCollection<Interfono>();
            string str = Sistema.RPC.InvokeMethod(Remote.getServerName() + Remote.__FSEENTRYSVC + "/websvcf/dialtp.aspx?_a=GetDevices", "po", 0);
            var arr = new System.Text.StringBuilder();
            CCollection tmp;
            Interfono interfono;
            if (!string.IsNullOrEmpty(str))
            {
                tmp = (CCollection)DMD.XML.Utils.Serializer.Deserialize(str);
                foreach (Office.Dispositivo dev in tmp)
                {
                    if (arr.Length > 0)
                        arr.Append(",");
                    arr.Append(DBUtils.GetID(dev));
                    interfono = new Interfono();
                    interfono.Dev = dev;
                    // interfono.Address = dev.
                    ret.Add(interfono);
                }
            }

            if (arr.Length > 0)
            {
                str = Sistema.RPC.InvokeMethod(Remote.getServerName() + Remote.__FSEENTRYSVC + "/websvcf/dialtp.aspx?_a=GetDevicesLastLog", "ids", Sistema.RPC.str2n(arr.ToString()));
                tmp = (CCollection)DMD.XML.Utils.Serializer.Deserialize(str);
                foreach (var currentInterfono in ret)
                {
                    interfono = currentInterfono;
                    foreach (Office.DeviceLog log in tmp)
                    {
                        if (log.IDDevice == DBUtils.GetID(interfono.Dev))
                        {
                            interfono.Log = log;
                            interfono.UserName = log.NomeUtente;
                            interfono.Address = log.IPAddress;
                        }
                    }
                }
            }

            return ret;
        }

        private static void Listen(object obj)
        {
            TcpListener listener = (TcpListener)obj;
            while (!m_Quit)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    bool ret;
                    var wc = new WaitCallback(ProcessClient);
                    client.NoDelay = true;
                    do
                    {
                        ret = ThreadPool.QueueUserWorkItem(wc, client);
                        Thread.Sleep(100);
                    }
                    while (!ret && !m_Quit);
                }
                catch (ThreadAbortException e)
                {
                }
                catch (SocketException e)
                {
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        // Private Shared m_Connessioni As New CCollection(Of InterfonoConnection)

        private static void ProcessClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            try
            {
                var con = new InterfonoConnection(client);
                if (con.BeginHandshake())
                {
                    // m_Connessioni.Add(con)
                    con.m_StreamThread = Thread.CurrentThread;
                    try
                    {
                        con.Listener();
                    }
                    catch (Exception ex)
                    {
                        Log.LogException(ex);
                    }
                    // m_Connessioni.Remove(con)
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }
    }
}