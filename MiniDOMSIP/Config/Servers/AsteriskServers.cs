using minidom.CallManagers;

namespace minidom.PBX
{
    public sealed class AsteriskServers
    {
        public static event ConnectedEventHandler Connected;

        public delegate void ConnectedEventHandler(object sender, AsteriskEventArgs e);

        public static event DisconnectedEventHandler Disconnected;

        public delegate void DisconnectedEventHandler(object sender, AsteriskEventArgs e);

        public static event ManagerEventEventHandler ManagerEvent;

        public delegate void ManagerEventEventHandler(object sender, AsteriskEvent e);

        private static CCollection<AsteriskServer> m_Servers = new CCollection<AsteriskServer>();

        private AsteriskServers()
        {
            DMDObject.IncreaseCounter(this);
        }

        static AsteriskServers()
        {
        }

        public static void StartListening(CCollection<AsteriskServer> servers)
        {
            var unchanged = new CCollection<AsteriskServer>();

            // Rimuoviamo i servers presenti in entrambi le configurazioni
            int i = 0;
            while (i < m_Servers.Count)
            {
                var curr = m_Servers[i];
                bool found = false;
                int j = 0;
                while (j < servers.Count)
                {
                    var other = servers[j];
                    if (curr.Equals(other))
                    {
                        unchanged.Add(curr);
                        servers.RemoveAt(j);
                        other.SetManager(curr.GetManager());
                        found = true;
                        break;
                    }
                    else
                    {
                        j += 1;
                    }
                }

                if (found)
                {
                    m_Servers.RemoveAt(i);
                }
                else
                {
                    i += 1;
                }
            }

            // Disconnettiamo le configurazioni non valide
            foreach (AsteriskServer server in m_Servers)
            {
                if (server.IsConnected())
                    server.Disconnect();
                server.Connected -= handleAsteriskConnect;
                server.Disconnected -= handleAsteriskDisconnect;
                server.ManagerEvent -= handleAsteriskEvent;
            }

            // Connettiamo le nuove configurazioni
            foreach (AsteriskServer server in servers)
            {
                server.Connected += handleAsteriskConnect;
                server.Disconnected += handleAsteriskDisconnect;
                server.ManagerEvent += handleAsteriskEvent;
                server.Connect();
            }

            m_Servers = new CCollection<AsteriskServer>();
            m_Servers.AddRange(unchanged);
            m_Servers.AddRange(servers);
        }

        public static void StopListening()
        {
            foreach (AsteriskServer server in m_Servers)
            {
                server.Disconnect();
                server.Connected -= handleAsteriskConnect;
                server.Disconnected -= handleAsteriskDisconnect;
                server.ManagerEvent -= handleAsteriskEvent;
            }

            m_Servers.Clear();
        }

        private static void handleAsteriskEvent(object sender, AsteriskEvent e)
        {
            ManagerEvent?.Invoke(sender, e);
        }

        private static void handleAsteriskDisconnect(object sender, AsteriskEventArgs e)
        {
            Disconnected?.Invoke(sender, e);
        }

        private static void handleAsteriskConnect(object sender, AsteriskEventArgs e)
        {
            Connected?.Invoke(sender, e);
        }

        ~AsteriskServers()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}