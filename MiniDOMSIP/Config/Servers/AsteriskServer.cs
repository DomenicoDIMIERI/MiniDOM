using DMD;
using DMD.XML;
using minidom.CallManagers;
using System;
using System.Runtime.CompilerServices;

namespace minidom.PBX
{

    /// <summary>
    /// Rappresenta un server asterisk
    /// </summary>
    [Serializable]
    public class AsteriskServer 
        : IDMDXMLSerializable
    {

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

        public event ConnectedEventHandler Connected;

        public delegate void ConnectedEventHandler(object sender, AsteriskEventArgs e);

        public event DisconnectedEventHandler Disconnected;

        public delegate void DisconnectedEventHandler(object sender, AsteriskEventArgs e);

        public event ManagerEventEventHandler ManagerEvent;

        public delegate void ManagerEventEventHandler(object sender, AsteriskEvent e);

        private System.Timers.Timer _m_Timer;

        private System.Timers.Timer m_Timer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _m_Timer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_m_Timer != null)
                {
                    _m_Timer.Elapsed -= m_Timer_Elapsed;
                }

                _m_Timer = value;
                if (_m_Timer != null)
                {
                    _m_Timer.Elapsed += m_Timer_Elapsed;
                }
            }
        }

        public string Nome;
        public string ServerName;
        public int ServerPort = 5038;
        public string UserName = "";
        public string Password = "";
        public string Channel = "";
        public string CallerID = "";
        [NonSerialized] private AsteriskCallManager _m_Manager;

        private AsteriskCallManager m_Manager
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _m_Manager;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_m_Manager != null)
                {
                    _m_Manager.ManagerEvent -= handleAsteriskEvent;
                    _m_Manager.Connected -= handleAsteriskConnect;
                    _m_Manager.Disconnected -= handleAsteriskDisconnect;
                }

                _m_Manager = value;
                if (_m_Manager != null)
                {
                    _m_Manager.ManagerEvent += handleAsteriskEvent;
                    _m_Manager.Connected += handleAsteriskConnect;
                    _m_Manager.Disconnected += handleAsteriskDisconnect;
                }
            }
        }

        public AsteriskServer()
        {
            DMDObject.IncreaseCounter(this);
            m_Timer = new System.Timers.Timer();
            m_Timer.Interval = 60 * 5 * 1000;
            m_Timer.Enabled = false;
            m_Manager = null;
        }

        public AsteriskServer(string nome, string serverName, string channel, string userName, string password) : this()
        {
            Nome = nome;
            ServerName = serverName;
            Channel = channel;
            UserName = userName;
            Password = password;
        }

        public override string ToString()
        {
            return DMD.Strings.JoinW( Nome , " (" , ServerName , ":" , DMD.Strings.CStr(ServerPort), ")");
        }

        public AsteriskCallManager GetManager()
        {
            return m_Manager;
        }

        public void SetManager(AsteriskCallManager value)
        {
            m_Manager = value;
        }

        public bool IsConnected()
        {
            return m_Manager is object && GetManager().IsConnected();
        }

        public void Connect()
        {
            if (IsConnected())
                return;
            m_Manager = new AsteriskCallManager(UserName, Password, ServerName, ServerPort);
            m_Manager.Start();
            m_Manager.Login();
            m_Timer.Enabled = true;
        }

        public void Disconnect()
        {
            if (!IsConnected())
                return;
            m_Timer.Enabled = false;
            m_Manager.Stop();
            m_Manager = null;
        }

        private void handleAsteriskEvent(object sender, AsteriskEvent e)
        {
            ManagerEvent?.Invoke(sender, e);
        }

        private void handleAsteriskConnect(object sender, AsteriskEventArgs e)
        {
            Connected?.Invoke(sender, e);
        }

        private void handleAsteriskDisconnect(object sender, AsteriskEventArgs e)
        {
            Disconnected?.Invoke(sender, e);
        }

        private void m_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // If Me.IsConnected Then
            // Try

            // SyncLock Me
            // Me.Disconnect()
            // Me.Connect()
            // End SyncLock
            // Catch ex As Exception
            // minidom.Sistema.Events.NotifyUnhandledException(ex)
            // End Try
            // End If

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
                return true;
            if (!(obj is AsteriskServer))
                return false;
            {
                var withBlock = (AsteriskServer)obj;
                return (Nome ?? "") == (withBlock.Nome ?? "") && (ServerName ?? "") == (withBlock.ServerName ?? "") && ServerPort == withBlock.ServerPort && (UserName ?? "") == (withBlock.UserName ?? "") && (Password ?? "") == (withBlock.Password ?? "") && (Channel ?? "") == (withBlock.Channel ?? "") && (CallerID ?? "") == (withBlock.CallerID ?? "");
            }
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "nome":
                    {
                        Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "servername":
                    {
                        ServerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "serverport":
                    {
                        ServerPort = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "username":
                    {
                        UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "password":
                    {
                        Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "channel":
                    {
                        Channel = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "callerid":
                    {
                        CallerID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("nome", Nome);
            writer.WriteAttribute("servername", ServerName);
            writer.WriteAttribute("serverport", ServerPort);
            writer.WriteAttribute("username", UserName);
            writer.WriteAttribute("password", Password);
            writer.WriteAttribute("channel", Channel);
            writer.WriteAttribute("callerid", CallerID);
        }

        ~AsteriskServer()
        {
            DMDObject.DecreaseCounter(this);
        }

        // Private Sub ReadXml(reader As Xml.XmlReader) Implements Xml.Serialization.IXmlSerializable.ReadXml
        // Me.Nome = reader.GetAttribute("nome")
        // Me.ServerName = reader.GetAttribute("servername")
        // Me.ServerPort = CInt(reader.GetAttribute("serverport"))
        // Me.UserName = reader.GetAttribute("username")
        // Me.Password = reader.GetAttribute("password")
        // Me.Channel = reader.GetAttribute("channel")
        // Me.CallerID = reader.GetAttribute("callerid")
        // reader.Read()
        // End Sub

        // Public Sub WriteXml(writer As Xml.XmlWriter) Implements Xml.Serialization.IXmlSerializable.WriteXml
        // writer.WriteAttributeString("nome", Me.Nome)
        // writer.WriteAttributeString("servername", Me.ServerName)
        // writer.WriteAttributeString("serverport", CStr(Me.ServerPort))
        // writer.WriteAttributeString("username", Me.UserName)
        // writer.WriteAttributeString("password", Me.Password)
        // writer.WriteAttributeString("channel", Me.Channel)
        // writer.WriteAttributeString("callerid", Me.CallerID)
        // End Sub
    }


}