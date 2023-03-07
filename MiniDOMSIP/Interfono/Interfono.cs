/* TODO ERROR: Skipped DefineDirectiveTrivia */
using System;
using System.Net.Sockets;
using DMD.XML;
using minidom.Win32;

namespace minidom.PBX
{
    [Serializable]
    public class Interfono : IDMDXMLSerializable, IComparable
    {
        private string m_UserName;
        private string m_Address;
        internal Office.Dispositivo Dev;
        internal Office.DeviceLog Log;
        internal InterfonoConnection Con;

        public Interfono()
        {
            m_UserName = "";
            m_Address = "";
        }

        public int UniqueID
        {
            get
            {
                return DBUtils.GetID(Dev);
            }
        }

        private void DoChanged(string pName, object newValue, object oldValue)
        {
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("UserName", m_UserName);
            writer.WriteAttribute("Address", m_Address);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "UserName":
                    {
                        m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Address":
                    {
                        m_Address = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        public string UserName
        {
            get
            {
                return m_UserName;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_UserName;
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_UserName = value;
                DoChanged("UserName", value, oldValue);
            }
        }

        public string Address
        {
            get
            {
                return m_Address;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_Address;
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_Address = value;
                DoChanged("Address", value, oldValue);
            }
        }

        public bool DisableMic = false;

        public override string ToString()
        {
            var ret = new System.Text.StringBuilder();
            ret.Append(UserName);
            ret.Append("   (");
            if (Dev is object)
            {
                ret.Append(Dev.Nome);
                ret.Append(" - ");
            }

            ret.Append(Address);
            ret.Append(")");
            return ret.ToString();
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((Interfono)obj);
        }

        public int CompareTo(Interfono obj)
        {
            return string.Compare(ToString(), obj.ToString(), true);
        }

        public bool IsConnected()
        {
            return Con is object && Con.client.Connected && Con.@params.Result == "DMDOK";
        }

        public InterfonoConnection Connect()
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect");
            PBX.Log.InternalLog("Interfono.Connect " + UserName + " (" + Address + ")");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Con = new InterfonoConnection();
            Con.@params = new InterfonoParams();
            Con.@params.codec = Settings.WaveCodec;
            Con.@params.srcID = UniqueID;
            Con.@params.srcIP = Machine.GetIPAddress();
            Con.@params.srcPort = 10446;
            Con.@params.targetIP = Address;
            Con.@params.targetPort = 10446;

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect -> TcpClient.BeginConnect");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Con.client = new TcpClient();
            IAsyncResult result;
            result = Con.client.BeginConnect(Address, 10445, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (Con.client.Connected)
            {
                Con.client.EndConnect(result);
            }
            else
            {
                // NOTE, MUST CLOSE THE SOCKET
                Con.client.Close();
                Con = null;
                throw new ApplicationException("Failed to connect server.");
            }

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect -> TcpClient.GetStream");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Con.stream = Con.client.GetStream();
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect -> TcpClient.WriteParams");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Sistema.BinarySerialize(Con.@params, Con.stream);
            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect -> TcpClient.ReadParams");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            System.Threading.Thread.Sleep(1000);
            Con.@params = (InterfonoParams)Sistema.BinaryDeserialize(Con.stream);
            var res = DMD.Strings.Split(Con.@params.Result, "|");
            string errorcode = res[0];
            switch (errorcode ?? "")
            {
                case "DMDOK":
                    {
                        PBX.Log.InternalLog("Interfono.Connect (ok)");
                        break;
                    }

                case "DMDWARNING":
                    {
                        PBX.Log.LogMessage("Interfono.Connect -> Warning: " + res[1]);
                        // Toast.Show("Errore nell'handshake: " & res(1), 5000)
                        PBX.Log.InternalLog("Interfono.Connect (warning) " + res[1]);
                        break;
                    }

                default:
                    {

                        /* TODO ERROR: Skipped IfDirectiveTrivia */
                        PBX.Log.LogMessage("Interfono.Connect -> Error: " + res[1]);
                        PBX.Log.InternalLog("Interfono.Connect (error) " + res[1]);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                        Con.stream.Close();
                        Con.client.Close();
                        Con = null;
                        throw new Exception("Errore nell'handshake: " + res[1]);
                        break;
                    }
            }


            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect -> StartDevices");
            PBX.Log.InternalLog("Interfono.Connect (startdevices)");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Con.StartDevices();

            /* TODO ERROR: Skipped IfDirectiveTrivia */
            PBX.Log.LogMessage("Interfono.Connect -> StartListening");
            PBX.Log.InternalLog("Interfono.Connect (startlistening)");
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Con.Peer = this;
            Con.StartListening();
            return Con;
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        public InterfonoConnection Disconnect()
        {
            PBX.Log.InternalLog("Interfono.Disconnect " + UserName + " (" + Address + ")");
            var con = Con;
            con.StopDevices();
            con.SendDisconnectCommand();
            InternalDisconnect();
            Con = null;
            return con;
        }

        public bool IsAvailable()
        {
            if (!string.IsNullOrEmpty(Address) && Log is object)
            {
                var d = Log.EndDate;
                if (d.HasValue == false)
                    d = Log.StartDate;
                if (d.HasValue)
                {
                    if (DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Minute, d.Value, DMD.DateUtils.Now()) <= 5L)
                        return true;
                }
            }

            return false;
        }

        internal void InternalDisconnect()
        {
            Con.StopListening();
            System.Threading.Thread.Sleep(100);
            Con.m_StreamThread.Abort();
            Con.m_StreamThread = null;
            if (Con.stream is object)
            {
                Con.stream.Dispose();
                Con.stream = null;
            }

            if (Con.client is object)
            {
                Con.client.Close();
                Con.client = null;
            }
        }
    }
}