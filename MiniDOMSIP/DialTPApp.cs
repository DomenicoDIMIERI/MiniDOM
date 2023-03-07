using System;
using System.Deployment.Application;
using DMD.XML;
using DMD;
using minidom.Win32;

namespace minidom.PBX
{
    public sealed class DMDSIPApp
    {
        public static event ConfigurationChangedEventHandler ConfigurationChanged;

        public delegate void ConfigurationChangedEventHandler(object sender, EventArgs e);

        private static Databases.CDBConnection m_Database;
        private static DMDSIPConfig m_Config;
        private static DMDSIPConfigRepository m_Configs;
        public static readonly CCollection<DMDSIPCommand> PendingCommands = new CCollection<DMDSIPCommand>();

        static DMDSIPApp()
        {
            m_Database = null;
            m_Config = new DMDSIPConfig();
            m_Configs = null;
        }

        public static Databases.CDBConnection Database
        {
            get
            {
                if (m_Database is null)
                    return Databases.APPConn;
                return m_Database;
            }

            set
            {
                m_Database = value;
            }
        }

        public static DMDSIPConfigRepository Configs
        {
            get
            {
                if (m_Configs is null)
                {
                    m_Configs = new DMDSIPConfigRepository();
                    m_Configs.Initialize();
                }

                return m_Configs;
            }
        }

        public static DMDSIPConfig CurrentConfig
        {
            get
            {
                return m_Config;
            }
        }

        public static int IDUltimaTelefonata = 0;

        public static void SetConfiguration(DMDSIPConfig config)
        {
            if (config is null)
                throw new ArgumentNullException();
            m_Config = config;
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            string tmp = DMD.Strings.Trim(DMD.Strings.CStr(config.Attributi.GetItemByKey("IconURL")));
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            ResetAsteriskServers();
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            ResetWatchFolders();
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            ResetUSBHandler();
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */


            ConfigurationChanged?.Invoke(null, new EventArgs());
        }

        public static void ResetUSBHandler()
        {
            if (DMD.Strings.CStr(CurrentConfig.Attributi.GetItemByKey("USBHandler")) == "true")
            {
                if (!USBDriveHandler.IsRunning())
                {
                    USBDriveHandler.Start();
                }
            }
            else if (USBDriveHandler.IsRunning())
            {
                USBDriveHandler.Stop();
            }
        }

        private static string UnescapeFolderName(string folderName)
        {
            folderName = DMD.Strings.Replace(folderName, "%%DESKTOP%%", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            folderName = DMD.Strings.Replace(folderName, "%%COMMONDESKTOP%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
            folderName = DMD.Strings.Replace(folderName, "%%DOCUMENTS%%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            folderName = DMD.Strings.Replace(folderName, "%%COMMONDOCUMENTS%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            folderName = DMD.Strings.Replace(folderName, "%%APPDATA%%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            folderName = DMD.Strings.Replace(folderName, "%%COMMONAPPDATA%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            folderName = DMD.Strings.Replace(folderName, "%%SYSDIR%%", Environment.GetFolderPath(Environment.SpecialFolder.System));
            folderName = DMD.Strings.Replace(folderName, "%%SYSDIR86%%", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
            folderName = DMD.Strings.Replace(folderName, "%%WINDOWSDIR%%", Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            folderName = DMD.Strings.Replace(folderName, "%%PICTURES%%", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            folderName = DMD.Strings.Replace(folderName, "%%COMMONPICTURES%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
            folderName = DMD.Strings.Replace(folderName, "%%MUSIC%%", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            folderName = DMD.Strings.Replace(folderName, "%%COMMONMUSIC%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
            folderName = DMD.Strings.Replace(folderName, "%%VIDEO%%", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            folderName = DMD.Strings.Replace(folderName, "%%COMMONVIDEO%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
            return DMD.Strings.Trim(folderName);
        }

        private static void ResetWatchFolders()
        {
            FolderWatch.StopWatching();
            foreach (string folderName in CurrentConfig.CartelleMonitorate)
            {
                string folderName1 = UnescapeFolderName(folderName);
                if (!string.IsNullOrEmpty(folderName1))
                    FolderWatch.AddFolder(folderName1);
            }

            foreach (string folderName in CurrentConfig.CartelleEscluse)
            {
                string folderName1 = UnescapeFolderName(folderName);
                if (!string.IsNullOrEmpty(folderName1))
                    FolderWatch.ExcludeFolder(folderName1);
            }

            FolderWatch.StartWatching();
        }

        private static void ResetAsteriskServers()
        {

            // PBX.AsteriskServers.StopListening()
            AsteriskServers.StartListening(new CCollection<AsteriskServer>(CurrentConfig.AsteriskServers));
        }

        public static DMDSIPCommand DequeueCommand(string machine, string user)
        {
            lock (PendingCommands)
            {
                int i = 0;
                while (i < PendingCommands.Count)
                {
                    var tmp = PendingCommands[i];
                    if (
                        DMD.Strings.Compare(machine, tmp.IDPostazione, true) == 0 
                        && 
                        DMD.Strings.Compare(user, tmp.IDUtente, true) == 0
                        )
                    {
                        PendingCommands.RemoveAt(i);
                        return tmp;
                    }

                    i += 1;
                }

                return null;
            }
        }

        public static bool CheckForUpdates()
        {
            UpdateCheckInfo info = null;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var AD = ApplicationDeployment.CurrentDeployment;
                info = AD.CheckForDetailedUpdate();
                return info.UpdateAvailable;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckForRequiredUpdates()
        {
            UpdateCheckInfo info = null;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var AD = ApplicationDeployment.CurrentDeployment;
                info = AD.CheckForDetailedUpdate();
                return info.UpdateAvailable && info.IsUpdateRequired;
            }
            else
            {
                return false;
            }
        }

        public static void InstallUpdateSyncWithInfo(bool forceUpdate)
        {
            UpdateCheckInfo info = null;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var AD = ApplicationDeployment.CurrentDeployment;
                info = AD.CheckForDetailedUpdate();
                if (info.UpdateAvailable)
                {
                    AD.Update();
                    System.Windows.Forms.Application.Restart();
                }
            }
        }
    }
}