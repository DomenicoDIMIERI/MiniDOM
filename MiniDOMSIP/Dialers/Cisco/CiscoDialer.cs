using System;
using DMD.XML;
using DMD;

namespace minidom.PBX
{

    /// <summary>
    /// Dialer tramite telefono VoIP cisco
    /// </summary>
    public class CiscoDialer 
        : DialerBaseClass
    {
        private System.Threading.Thread m_Thread;
        private string m_DeviceIP = "";
        private string m_DeviceName = "";

        /// <summary>
        /// Costruttore
        /// </summary>
        public CiscoDialer()
        {
            m_Thread = null;
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="deviceIP"></param>
        /// <param name="deviceName"></param>
        public CiscoDialer(string deviceIP, string deviceName) : this()
        {
            deviceIP = DMD.Strings.Trim(deviceIP);
            if (string.IsNullOrEmpty(deviceIP))
                throw new ArgumentNullException("deviceIP");
            m_DeviceIP = deviceIP;
            m_DeviceName = deviceName;
        }

        /// <summary>
        /// Restituisce o imposta l'IP del dispositivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DeviceIP
        {
            get
            {
                return m_DeviceIP;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("DeviceIP");
                m_DeviceIP = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta il nome del dispositivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DeviceName
        {
            get
            {
                return m_DeviceName;
            }

            set
            {
                m_DeviceName = value;
            }
        }

        public string PrepareNumber(string number)
        {
            return DMD.Strings.Trim(number);
        }

        public override void Dial(string number)
        {
            if (!IsInstalled())
                return;
            HangUp();
            if (DMD.Strings.Len(number) <= 1)
                return;
            m_Thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(thread));
            m_Thread.Start(number);
        }

        /// <summary>
        /// Calcola il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.m_DeviceIP, this.m_DeviceName);
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DMD.Strings.JoinW(this.m_DeviceName , " (", this.m_DeviceIP , ")");
        }

        
        /// <summary>
        /// Metodo usato per comporre tramite un thread diverso
        /// </summary>
        /// <param name="o"></param>
        private void thread(object o)
        {
            string Number = DMD.Strings.CStr(o);
            var e = new DialEventArgs(Number);
            OnBegidDial(e);
            const int sleeptime = 500;
            var telnet = new Net.Telnet.TelnetClient();
            telnet.Host = m_DeviceIP;
            telnet.Port = 23;
            telnet.Connect();
            string password = "cisco"; // cisco phone password
            int mute = 0; // mute on a dial 0/1
            string prompt = ">";

            // telnet = new Net::Telnet ( Timeout=>3, Errmode=>'die');
            // connecting
            telnet.WaitFor("Password :");
            telnet.Println(password);
            telnet.WaitFor(prompt);

            // telnet.Print("test close")

            telnet.Println("test open");
            telnet.WaitFor(prompt);
            System.Threading.Thread.Sleep(sleeptime);
            telnet.Println("test key spkr");
            telnet.WaitFor(prompt);
            System.Threading.Thread.Sleep(sleeptime);
            if (mute != 0)
            {
                telnet.Println("test key mute");
                telnet.WaitFor(prompt);
                System.Threading.Thread.Sleep(sleeptime);
            }

            telnet.Println("test key " + Number);
            telnet.WaitFor(prompt);
            System.Threading.Thread.Sleep(Number.Length * sleeptime);
            telnet.Println("test close");
            telnet.WaitFor(prompt);
            System.Threading.Thread.Sleep(sleeptime);
            telnet.Close();
            OnEndDial(e);
        }

        /// <summary>
        /// Restituisce true se l'indirizzo IP del telefono esiste
        /// </summary>
        /// <returns></returns>
        public override bool IsInstalled()
        {
            return !string.IsNullOrEmpty(this.m_DeviceIP);
        }

        /// <summary>
        /// Restituisce il nome del telefono
        /// </summary>
        public override string Name
        {
            get
            {
                return DMD.Strings.JoinW("Cisco " , this.m_DeviceName);
            }
        }

        /// <summary>
        /// Aggancia
        /// </summary>
        public override void HangUp()
        {
            if (m_Thread is object)
            {
                m_Thread.Abort();
                m_Thread = null;
            }
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CiscoDialer))
                return false;
            var o = obj as CiscoDialer;
            return   DMD.Strings.EQ(this.m_DeviceIP, o.m_DeviceIP)
                  && DMD.Strings.EQ(this.m_DeviceName, o.m_DeviceName, true);
        }
    }
}