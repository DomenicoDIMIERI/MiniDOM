
using Microsoft.Win32;

namespace minidom.PBX
{
    public class Settings
    {

        /// <summary>
    /// Restituisce o imposta il server da cui viene scaricata la configurazione online
    /// </summary>
    /// <returns></returns>
        public static string ConfigServer
        {
            get
            {
                return GetRegValue("ConfigServer", "");
            }

            set
            {
                SetRegValue("ConfigServer", value);
            }
        }

        public static bool AutoStart
        {
            get
            {
                return My.Settings.Default.autostart;
            }

            set
            {
                My.Settings.Default.autostart = value;
                My.Settings.Default.Save();
            }
        }

        public static bool RegisterDialtp
        {
            get
            {
                return My.Settings.Default.registerprotocol;
            }

            set
            {
                My.Settings.Default.registerprotocol = value;
                My.Settings.Default.Save();
            }
        }

        public static int SMTPLimitOutSent
        {
            get
            {
                return My.Settings.Default.SMTPLimitOutSent;
            }

            set
            {
                My.Settings.Default.SMTPLimitOutSent = value;
                My.Settings.Default.Save();
            }
        }

        public static string SMTPPassword
        {
            get
            {
                return GetRegValue("SMTPPassword", My.Settings.Default.SMTPPassword);
            }

            set
            {
                SetRegValue("SMTPPassword", value);
            }
        }

        public static string SMTPUserName
        {
            get
            {
                return GetRegValue("SMTPUserName", My.Settings.Default.SMTPUserName);
            }

            set
            {
                SetRegValue("SMTPUserName", value);
            }
        }

        public static string SMTPServer
        {
            get
            {
                return GetRegValue("SMTPServer", My.Settings.Default.SMTPServer);
            }

            set
            {
                SetRegValue("SMTPServer", value);
            }
        }

        public static int SMTPPort
        {
            get
            {
                return GetRegValue("SMTPPort", My.Settings.Default.SMTPPort);
            }

            set
            {
                SetRegValue("SMTPPort", value);
            }
        }

        public static string SMTPDisplayName
        {
            get
            {
                return GetRegValue("SMTPDisplayName", My.Settings.Default.SMTPDisplayName);
            }

            set
            {
                SetRegValue("SMTPDisplayName", value);
            }
        }

        public static string SMTPSubject
        {
            get
            {
                return GetRegValue("SMTPSubject", My.Settings.Default.SMTPSubject);
            }

            set
            {
                SetRegValue("SMTPSubject", value);
            }
        }

        public static bool SMTPSSL
        {
            get
            {
                return GetRegValue("SMTPServer", My.Settings.Default.SMTPSSL);
            }

            set
            {
                SetRegValue("SMTPServer", value);
            }
        }

        public static string SMTPNotifyTo
        {
            get
            {
                return GetRegValue("SMTPNotifyTo", My.Settings.Default.SMTPNotifyTo);
            }

            set
            {
                SetRegValue("SMTPNotifyTo", value);
            }
        }

        public static int LogEvery
        {
            get
            {
                return GetRegValue("LogEvery", My.Settings.Default.LogEvery);
            }

            set
            {
                SetRegValue("LogEvery", value);
            }
        }

        public static string LastPrefix
        {
            get
            {
                return My.Settings.Default.LastPrefix;
            }

            set
            {
                My.Settings.Default.LastPrefix = value;
                My.Settings.Default.Save();
            }
        }

        public static string LastFaxPrefix
        {
            get
            {
                return My.Settings.Default.LastFaxPrefix;
            }

            set
            {
                My.Settings.Default.LastFaxPrefix = value;
                My.Settings.Default.Save();
            }
        }

        public static string LastDialerName
        {
            get
            {
                return My.Settings.Default.LastDialerName;
            }

            set
            {
                My.Settings.Default.LastDialerName = value;
                My.Settings.Default.Save();
            }
        }

        public static int Flags
        {
            get
            {
                return GetRegValue("Flags", My.Settings.Default.Flags);
            }

            set
            {
                SetRegValue("Flags", value);
            }
        }



        // Shared Property FoldersToExclude As String
        // Get
        // Return GetRegValue("FoldersToExclude", PBX.My.Settings.FoldersToExclude)
        // End Get
        // Set(value As String)
        // SetRegValue("FoldersToExclude", value)
        // End Set
        // End Property

        // Shared Property FoldersToWatch As String
        // Get
        // Return GetRegValue("FoldersToWatch", PBX.My.Settings.FoldersToWatch)
        // End Get
        // Set(value As String)
        // SetRegValue("FoldersToWatch", value)
        // End Set
        // End Property

        // Shared Sub Save()
        // My.Settings.Save()
        // End Sub

        public static string ServersList
        {
            get
            {
                return GetRegValue("ServersList", My.Settings.Default.ServersList);
            }

            set
            {
                SetRegValue("ServersList", value);
            }
        }

        public static string AsteriskServers
        {
            get
            {
                return My.Settings.Default.AsteriskServers;
            }

            set
            {
                My.Settings.Default.AsteriskServers = value;
                My.Settings.Default.Save();
            }
        }

        public static string APPPassword
        {
            get
            {
                return GetRegValue("APPPassword", "");
            }

            set
            {
                SetRegValue("APPPassword", value);
            }
        }

        private static T GetRegValue<T>(string name, T dVal = default)
        {
            return (T)Registry.CurrentUser.GetValue(@"Software\DIALTP\" + name, dVal);
        }

        private static void SetRegValue(string name, object dVal)
        {
            Registry.CurrentUser.SetValue(@"Software\DIALTP\" + name, dVal);
        }

        public static string WaveInDevName
        {
            get
            {
                return GetRegValue("WaveInDevName", "");
            }

            set
            {
                SetRegValue("WaveInDevName", value);
            }
        }

        public static bool AutoSaveConversations
        {
            get
            {
                return GetRegValue("AutoSaveConversations", false);
            }

            set
            {
                SetRegValue("AutoSaveConversations", value);
            }
        }

        public static bool WaveInDisabled
        {
            get
            {
                return GetRegValue("WaveInDisabled", false);
            }

            set
            {
                SetRegValue("WaveInDisabled", value);
            }
        }

        public static string WaveFolder
        {
            get
            {
                return GetRegValue("WaveFolder", "");
            }

            set
            {
                SetRegValue("WaveFolder", value);
            }
        }

        public static string WaveOutDevName
        {
            get
            {
                return GetRegValue("WaveOutDevName", "");
            }

            set
            {
                SetRegValue("WaveOutDevName", value);
            }
        }

        public static int WaveCodec
        {
            get
            {
                return GetRegValue("WaveCodec", 0);
            }

            set
            {
                SetRegValue("WaveCodec", value);
            }
        }
    }
}