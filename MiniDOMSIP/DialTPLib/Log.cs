using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using DMD.XML;
using DMD;
using minidom.Win32;
using DMD.Zip;

namespace minidom.PBX
{
    public class Log
    {
        private class MailInfo
        {
            public ArrayList files = new ArrayList();
            public System.Net.Mail.MailMessage m;

            public MailInfo()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~MailInfo()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        private static CCollection<MailInfo> m_SentMessages = new CCollection<MailInfo>();
        private static readonly object sessionsLock = new object();
        private static CLogSession m_CurrSession = null;
        private static System.Timers.Timer m_Timer = null;
        private static System.Timers.Timer m_TimerEvents = null;
        private static CCollection<CLogSession> pendinSessions = new CCollection<CLogSession>();

        static Log()
        {
            m_Timer = new System.Timers.Timer();
            m_Timer.Interval = 15 * 1000;
            m_Timer.Elapsed += handleTimer;
            m_Timer.Enabled = true;
            m_TimerEvents = new System.Timers.Timer();
            m_TimerEvents.Interval = 1 * 1000;
            m_TimerEvents.Elapsed += handleTimerEvents;
            m_TimerEvents.Enabled = true;
        }

        private static void handleTimerEvents(object sender, EventArgs e)
        {
            Keyboard.KeyboardEventArgs k = null;
            lock (keys)
            {
                if (keys.Count > 0)
                {
                    var s = EnsureSession();
                    while (keys.Count > 0)
                    {
                        k = keys[0];
                        s.LogKey(k);
                        keys.RemoveAt(0);
                    }
                }
            }

            if (m_TakeMouseShot)
            {
                var s = EnsureSession();
                try
                {
                    s.TakeMouseScreenShot();
                }
                catch (Exception ex)
                {
                    try
                    {
                        s.TakeFullScreenShot();
                        if (s.ScreenShots.Count > 5)
                        {
                            Reset();
                        }
                    }
                    catch (Exception ex1)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }
                }

                m_TakeMouseShot = false;
            }
        }

        private static void handleTimer(object sender, EventArgs e)
        {
            lock (sessionsLock)
            {
                if (pendinSessions.Count == 0)
                    return;
                var s = pendinSessions[0];
                pendinSessions.RemoveAt(0);
                s.LogMessage("Chiudo la sessione di log " + s.SessionID);
                s.EnsureFolder();
                string fileName = s.GetDefaultFileName();
                s.Save(fileName);
                s.Dispose();
            }
        }

        public static void LogNetworkConfiguration()
        {
            string strHostName = System.Net.Dns.GetHostName();
            LogMessage("HostName: " + strHostName);
            foreach (System.Net.IPAddress hostAdr in System.Net.Dns.GetHostEntry(strHostName).AddressList)
                LogMessage("---> " + hostAdr.ToString());
        }

        [Serializable]
        public class LogSession : IDisposable
        {
            public static object sessionsLock = new object();
            public static int SessionsCount = 0;
            public int SessionID;
            public DateTime StartTime = DMD.DateUtils.Now();
            public ArrayList images = new ArrayList();
            public System.Text.StringBuilder logBuffer = new System.Text.StringBuilder();
            public System.Text.StringBuilder textBuffer = new System.Text.StringBuilder();
            public System.Text.StringBuilder keysBuffer = new System.Text.StringBuilder();
            public System.Text.StringBuilder filesBuffer = new System.Text.StringBuilder();
            public string FileName = "";
            public string Description = "";

            public LogSession()
            {
                DMDObject.IncreaseCounter(this);
                lock (sessionsLock)
                {
                    SessionID = SessionsCount;
                    SessionsCount += 1;
                }
                // In case you end up with "0" (probably because you just created the PerformanceCounter and then directly used it) you need to add 2 lines as the PerformanceCounter need some time to work:


            }

            public void Append(string str)
            {
                lock (this)
                {
                    var d = DMD.DateUtils.Now();
                    str = Sistema.Formats.FormatUserDateTime(d) + DMD.Strings.vbTab + str + "<br/>";
                    logBuffer.Append(str);
                }
            }

            public void AppendKeyabord(string str)
            {
                lock (this)
                {
                    var d = DMD.DateUtils.Now();
                    str = Sistema.Formats.FormatUserDateTime(d) + DMD.Strings.vbTab + str + "<br/>";
                    keysBuffer.Append(str);
                }
            }

            public void AppendChar(string str)
            {
                lock (this)
                    textBuffer.Append(str);
            }

            public void AppendFile(string str)
            {
                lock (this)
                {
                    var d = DMD.DateUtils.Now();
                    str = Sistema.Formats.FormatUserDateTime(d) + DMD.Strings.vbTab + str + "<br/>";
                    filesBuffer.Append(str);
                }
            }




            // This code added by Visual Basic to correctly implement the disposable pattern.
            public void Dispose()
            {
                foreach (System.Drawing.Image img in images)
                    img.Dispose();
                images = null;
                filesBuffer = null;
                keysBuffer = null;
                logBuffer = null;
                textBuffer = null;
                FileName = DMD.Strings.vbNullString;
                Description = DMD.Strings.vbNullString;
            }



            // Public Sub Upload()
            // If Me.GetUploadServer <> "" Then
            // My.Computer.Network.UploadFile(Me.FileName, Me.GetUploadServer & "?kname=" & My.Computer.Name)
            // End If
            // End Sub

            private string DataPath()
            {
                var d = DMD.DateUtils.Now();
                return DMD.Strings.Right("0000" + d.Year, 4) + DMD.Strings.Right("00" + d.Month, 2) + DMD.Strings.Right("00" + d.Day, 2) + DMD.Strings.Right("00" + d.Hour, 2) + DMD.Strings.Right("00" + d.Minute, 2) + DMD.Strings.Right("00" + d.Second, 2);
            }

            public void Initialize()
            {
                // If (Me.FileName <> "") Then Return

                // SyncLock lockObject
                // Dim folder As String = GetLogFolder()
                // Me.FileName = folder & "\" & Me.DataPath & ".dtp"
                // Dim i As Integer = 1
                // While System.IO.File.Exists(Me.FileName)
                // Me.FileName = folder & "" & Me.DataPath & Right("0000" & i, 4) & ".dtp"
                // i += 1
                // End While
                // Dim stream As New System.IO.FileStream(Me.FileName, System.IO.FileMode.Create)
                // stream.Dispose()
                // End SyncLock
            }

            public void Save()
            {
                using (var stream = new System.IO.FileStream(FileName + ".tmp", System.IO.FileMode.Create))
                {
                    var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    serializer.Serialize(stream, this);
                }

                var @out = new System.IO.StringWriter();
                using (var zip = new ZipFile())
                {
                    zip.StatusMessageTextWriter = @out;
                    zip.ZipErrorAction = ZipErrorAction.Skip;
                    // zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
                    zip.AddFile(FileName + ".tmp", "");
                    zip.CompressionLevel = CompressionLevel.None; // Ionic.Zlib.CompressionLevel.BestCompression
                    zip.CompressionMethod = CompressionMethod.Deflate; // CompressionMethod.BZip2
                    zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                    using (var stream = new System.IO.FileStream(FileName, System.IO.FileMode.Create))
                    {
                        zip.Save(stream);
                    }
                }

                @out.Dispose();
                Sistema.FileSystem.DeleteFile(FileName + ".tmp");
            }

            // Private Shared Sub FixDialTP(ByVal stream As System.IO.Stream)
            // stream.Position = 0
            // Dim reader As New System.IO.StreamReader(stream)
            // Dim text As String = reader.ReadToEnd
            // text = Replace(text, "DIALTP, Version=1.0.0.0,", "DialTPLib, Version=1.0.0.0,")
            // Dim writer As New System.IO.StreamWriter(stream)
            // stream.Position = 0
            // writer.Write(text)
            // stream.SetLength(Len(text))
            // End Sub

            public static LogSession Load(string fileName)
            {
                // Dim fName As String
                //System.IO.Stream stream = null;
                //ZipFile zip = null;
                //ZipEntry e = null;
                //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = null;
                //LogSession ret = null;

                ///* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                //stream = new System.IO.MemoryStream();
                //zip = new ZipFile(fileName);
                //e = zip.Entries.ElementAtOrDefault(0);
                //e.Extract(stream);
                //// zip.ExtractAll(GetLogFolder, ExtractExistingFileAction.OverwriteSilently)
                //zip.Dispose();

                //// FixDialTP(stream)

                //stream.Position = 0L;
                //serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //ret = (LogSession)serializer.Deserialize(stream);
                //stream.Dispose();

                //// minidom.Sistema.FileSystem.DeleteFile(GetLogFolder1() & "\" & fName & ".tmp")
                ///* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                //if (zip is object)
                //    zip.Dispose();
                //if (stream is object)
                //    stream.Dispose();
                ///* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                //return ret;
                throw new NotImplementedException();
            }

            public void Delete()
            {
                Sistema.FileSystem.DeleteFile(FileName);
            }

            public void LogNetworkConfiguration()
            {
                string strHostName = System.Net.Dns.GetHostName();
                Append("HostName: " + strHostName);
                foreach (System.Net.IPAddress hostAdr in System.Net.Dns.GetHostEntry(strHostName).AddressList)
                    Append("---> " + hostAdr.ToString());
            }

            ~LogSession()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        // Private Shared lock As New Object


        public static void Reset()
        {
            lock (Sistema.@lock)
            {
                try
                {
                    var s = GetCurrentSession();
                    if (s is object)
                        pendinSessions.Add(s);
                    m_CurrSession = null;
                    m_CurrSession = EnsureSession();
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
                }
            }
        }

        public static double GetCPUUsage()
        {
            PerformanceCounter cpu = null;
            try
            {
                cpu = new PerformanceCounter();
                {
                    var withBlock = cpu;
                    withBlock.CategoryName = "Processor";
                    withBlock.CounterName = "% Processor Time";
                    withBlock.InstanceName = "_Total";
                }

                cpu.NextValue();
                return Maths.Ceiling(cpu.NextValue());
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (cpu is object)
                {
                    cpu.Dispose();
                    cpu = null;
                }
            }
        }

        public static void NotifyPCStatus(string eventName)
        {
            string text = "";
            double cpuUsage = GetCPUUsage();
            float sysTemp = GetSystemTemperature();
            var mem = Memory.GetMemoryStatus();
            long DiskTotal = -1;
            long DiskAvail = -1;
            long RAMTotal = (long)mem.ullTotalPhys;
            long RAMAvail = (long)mem.ullAvailPhys;
            long RAMPageFileTotal = (long)mem.ullTotalPageFile;
            long RAMPageFileAval = (long)mem.ullAvailPageFile;
            long RAMVirtualTotal = (long)mem.ullTotalVirtual;
            long RAMVirtualAval = (long)mem.ullAvailVirtual;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int screenColors = Screen.PrimaryScreen.BitsPerPixel;
            var drives = System.IO.DriveInfo.GetDrives();
            string userName = DMDSIPApp.CurrentConfig.UserName;
            string unita;
            text += "<table border=\"1\" cellpadding=\"3px\">";
            text += "<tr><td>Stato:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + HtmlEncode(eventName) + " </td></tr>";
            text += "<tr><td>Sistema Operativo:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + HtmlEncode(Machine.GetOperatoratingSystemVersion()) + " </td></tr>";
            text += "<tr><td>Seriale CPU:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + HtmlEncode(Machine.GetCPUSerialNumber()) + " </td></tr>";
            text += "<tr><td>Utente:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + HtmlEncode(userName) + " %</td></tr>";
            text += "<tr><td>CPU:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatPercentage(cpuUsage) + " %</td></tr>";
            text += "<tr><td>Temperatura:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(sysTemp) + " &deg;</td></tr>";
            text += "<tr><td>Disco Totale:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(DiskTotal) + " Bytes</td></tr>";
            text += "<tr><td>Disco Disponibile:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(DiskAvail) + " Bytes</td></tr>";
            text += "<tr><td>Schermo:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + screenWidth + " x " + screenHeight + " (" + screenColors + " bits)</td></tr>";
            text += "<tr><td>RAM Totale:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(RAMTotal) + " Bytes</td></tr>";
            text += "<tr><td>RAM Disponibile:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(RAMAvail) + " Bytes</td></tr>";
            text += "<tr><td>Paginazione Totale:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(RAMPageFileTotal) + " Bytes</td></tr>";
            text += "<tr><td>Paginazione Disponibile:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(RAMPageFileAval) + " Bytes</td></tr>";
            text += "<tr><td>Virtuale Totale:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(RAMVirtualTotal) + " Bytes</td></tr>";
            text += "<tr><td>Virtuale Disponibile:" + DMD.Strings.vbTab + "</td><td style=\"text-align:right;\">" + Sistema.Formats.FormatInteger(RAMVirtualAval) + " Bytes</td></tr>";
            text += "<tr><td>Unità logiche:" + DMD.Strings.vbTab + "</td>";
            text += "<td style=\"text-align:right;\">";
            text += "<table>";
            text += "<tr>";
            text += "<th>Nome</th>";
            text += "<th>Etichetta</th>";
            text += "<th>Root</th>";
            text += "<th>Dimensione</th>";
            text += "<th>Libera</th>";
            text += "<th>Disponibile</th>";
            text += "</tr>";
            unita = "";
            foreach (System.IO.DriveInfo drive in drives)
            {
                string nomeUnita = drive.Name;
                if ((nomeUnita.Substring(0, 2).ToLower() ?? "") == (Environment.SystemDirectory.Substring(0, 2).ToLower() ?? ""))
                {
                    try
                    {
                        DiskTotal = drive.TotalSize;
                        DiskAvail = drive.AvailableFreeSpace;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                try
                {
                    nomeUnita = drive.Name + " (" + drive.TotalSize + ", " + drive.TotalFreeSpace + ", " + drive.AvailableFreeSpace + ")";
                    switch (drive.DriveType)
                    {
                        // Case IO.DriveType.CDRom
                        // Case IO.DriveType.Fixed
                        // Case IO.DriveType.Network
                        case System.IO.DriveType.Removable:
                        case System.IO.DriveType.CDRom:
                            {
                                text += "<tr>";
                                text += "<td>" + drive.Name + " (?)</td>";
                                text += "<td></td>";
                                text += "<td></td>";
                                text += "<td style=\"text-align:right;\"></td>";
                                text += "<td style=\"text-align:right;\"></td>";
                                text += "<td style=\"text-align:right;\"></td>";
                                text += "</tr>";
                                break;
                            }

                        default:
                            {
                                text += "<tr>";
                                text += "<td>" + drive.Name + " (" + drive.IsReady + ")</td>";
                                text += "<td>" + drive.VolumeLabel + "</td>";
                                text += "<td>" + drive.RootDirectory.FullName + "</td>";
                                text += "<td style=\"text-align:right;\">" + Sistema.Formats.FormatBytes(drive.TotalSize) + " Bytes</td>";
                                text += "<td style=\"text-align:right;\">" + Sistema.Formats.FormatBytes(drive.TotalFreeSpace) + " Bytes</td>";
                                text += "<td style=\"text-align:right;\">" + Sistema.Formats.FormatBytes(drive.AvailableFreeSpace) + " Bytes</td>";
                                text += "</tr>";
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                }

                unita = DMD.Strings.Combine(unita, nomeUnita, "; ");
            }

            text += "</table>";
            text += "</td></tr>";
            text += "</table>";


            // text &= Processi.CheckProcesses1()

            LogMessage(text);

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            string serverName = Settings.ConfigServer;
            if (!string.IsNullOrEmpty(serverName))
            {
                if (!serverName.EndsWith("/"))
                    serverName = serverName + "/";
                var col = new CKeyCollection();
                col.Add("EventName", eventName);
                col.Add("OSVersion", Machine.GetOperatoratingSystemVersion());
                col.Add("CPUSerialNumber", Machine.GetCPUSerialNumber());
                col.Add("SerialNumber", Machine.GetSerialNumber());
                col.Add("IDAppdress", Machine.GetIPAddress());
                col.Add("MACAppdress", Machine.GetMACAddress());
                col.Add("CPUUsage", cpuUsage);
                col.Add("SYSTemperature", sysTemp);
                col.Add("DiskTotal", DiskTotal);
                col.Add("DiskAvail", DiskAvail);
                col.Add("RAMTotal", RAMTotal);
                col.Add("RAMAvail", RAMAvail);
                col.Add("RAMPageFileTotal", RAMPageFileTotal);
                col.Add("RAMPageFileAval", RAMPageFileAval);
                col.Add("RAMVirtualTotal", RAMVirtualTotal);
                col.Add("RAMVirtualAval", RAMVirtualAval);
                col.Add("ScreenWidth", screenWidth);
                col.Add("ScreenHeight", screenHeight);
                col.Add("ScreenColors", screenColors);
                col.Add("currentUser", userName);
                col.Add("dtpVersion", Application.ProductVersion.ToString());
                col.Add("BootTime", Machine.BootTime);
                col.Add("Units", unita);
                col.Add("AudioInDevices", DMD.Strings.Join(Machine.GetAudioInputDeviceNames(), DMD.Strings.vbLf));
                string tmp = Sistema.RPC.InvokeMethod(serverName + "widgets/websvcf/dialtp.aspx?_a=NotifyPCStatus&oname=" + DMDSIPApp.CurrentConfig.IDPostazione + "&uname=" + userName, "col", DMD.XML.Utils.Serializer.Serialize(col));
                CKeyCollection ret = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                DMDSIPCommand cmd = (DMDSIPCommand)ret.GetItemByKey("DMD_Action");
                if (cmd is object)
                {
                    LogMessage("Esecuzione comando remoto: " + cmd.Name);
                    cmd.Execute();
                }
            }

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        /// <summary>
        /// Fake HTML encode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string HtmlEncode(string value)
        {
            return value;
        }

        public static void CheckWindow()
        {
            var s = GetCurrentSession();
            if (s is null)
                return;
            s.CheckWindow();
        }

        // Shared Sub DeleteTemporaryFiles()
        // Try
        // minidom.Sistema.FileSystem.DeleteFile(GetLogFolder1() & "\*.*", True)
        // Catch ex As Exception

        // End Try
        // End Sub

        private static bool CanTakeScreenShot()
        {
            lock (sessionsLock)
            {
                var s = GetCurrentSession();
                if (s is null)
                    return false;
                return s.CanTakeScreenShot();
            }
        }

        public static void TakeFullScreenShot()
        {
            // SyncLock sessionsLock
            try
            {
                var s = EnsureSession();
                s.TakeFullScreenShot();
                if (s.ScreenShots.Count > 5)
                {
                    Reset();
                }
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
            }
            // End SyncLock
        }

        private static bool m_TakeMouseShot = false;

        public static void TakeMouseScreenShot()
        {
            if (!(DMD.Strings.CStr(DMDSIPApp.CurrentConfig.Attributi.GetItemByKey("ALLOWSCREENSHOTS")) == "true"))
                return;
            m_TakeMouseShot = true;
        }

        public static float GetSystemTemperature()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                var enumerator = searcher.Get().GetEnumerator();
                if (enumerator is object)
                {
                    while (enumerator.MoveNext())
                    {
                        var tempObject = enumerator.Current;
                        // Console.WriteLine(tempObject("CurrentTemperature").ToString())

                        return (float)(DMD.Floats.CSng(tempObject["CurrentTemperature"]) / 10f - 273.15d);
                    }
                }

                return -1;
            }
            catch (Exception ex)
            {
                return -2;
            }
        }

        public static void LogMessage(string message)
        {
            EnsureSession().LogMessage(message);
        }

        public static void LogException(Exception ex)
        {
            EnsureSession().LogException(ex);
        }

        private static CCollection<Keyboard.KeyboardEventArgs> keys = new CCollection<Keyboard.KeyboardEventArgs>();

        public static void LogKey(Keyboard.KeyboardEventArgs e)
        {
            lock (keys)
                keys.Add(e);
        }

        public static void Terminate()
        {
            lock (sessionsLock)
            {
                m_TakeMouseShot = false;
                var s = EnsureSession();
                s.TakeFullScreenShot();
                while (keys.Count > 0)
                {
                    s.LogKey(keys[0]);
                    keys.RemoveAt(0);
                }

                LogMessage("Termino il log delle attività");
                LogMessage("Chiudo la sessione di log " + s.SessionID);
                s.EnsureFolder();
                string fileName = s.GetDefaultFileName();
                Thread.Sleep(1000);
                s.Save(fileName);
                s.Dispose();
                s = null;
                m_CurrSession = null;
            }
        }

        public static void StartLogging()
        {
            EnsureSession();
        }

        public static string GetSessionName()
        {
            lock (sessionsLock)
            {
                var s = GetCurrentSession();
                if (s is null)
                    return "";
                return s.GetDefaultFileName();
            }
        }

        public static CLogSession EnsureSession()
        {
            lock (sessionsLock)
            {
                if (m_CurrSession is null)
                {
                    m_CurrSession = new CLogSession();
                    LogMessage("Apro la sessione di log " + m_CurrSession.SessionID);
                }

                return m_CurrSession;
            }
        }

        public static CLogSession GetCurrentSession()
        {
            EnsureSession();
            return m_CurrSession;
        }

        public static string GetMachineName()
        {
            return System.Environment.MachineName; // My.MyProject.Computer.Name;
        }

        public static string GetCurrentUserName()
        {
            return Environment.UserName;
        }

        public static string GetUserDataPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public static string GetNetworkConfig()
        {
            return DMD.Strings.JoinW("Network Present: " , (DMD.Net.Utils.IsNetworkAvailable())? "Sì" : "No");
        }

        // Private Class DeleteWorker
        // Inherits System.ComponentModel.BackgroundWorker

        // Private d As Date

        // Public Sub New(ByVal d As Date)

        // Me.d = d
        // End Sub




        // Protected Overrides Sub OnDoWork(e As DoWorkEventArgs)
        // Dim path As String = minidom.Sistema.ApplicationContext.UserDataFolder & "\data\"
        // Dim finfo As New System.IO.DirectoryInfo(path)
        // If (finfo.Exists = False) Then Return
        // 'Dim files() As CodeProject.FileData = CodeProject.FastDirectoryEnumerator.GetFiles(path, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)

        // Dim files As IEnumerable(Of CodeProject.FileData) = CodeProject.FastDirectoryEnumerator.EnumerateFiles(path, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)
        // For Each file As CodeProject.FileData In files
        // If Calendar.Compare(file.CreationTime, d) < 0 Then
        // Try
        // file.Delete()
        // Catch ex As Exception
        // ' Sistema.Events.NotifyUnhandledException(ex)
        // End Try
        // 'System.Threading.Thread.Sleep(100)
        // End If
        // Next

        // MyBase.OnDoWork(e)
        // End Sub
        // End Class

        private static object timerLock = new object();
        private static System.Timers.Timer timerDf = null;
        private static DateTime timerdfd;

        public static void DeleteFilesBefore(DateTime d)
        {
            lock (timerLock)
            {
                if (timerDf is null)
                {
                    timerdfd = d;
                    timerDf = new System.Timers.Timer();
                    timerDf.Elapsed += handletimerdf;
                    timerDf.Interval = 1000 * 60 * 5;
                    timerDf.Enabled = true;
                }
            }
        }

        private static void handletimerdf(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (timerLock)
            {
                try
                {
                    // Dim worker As New DeleteWorker(timerdfd)

                    // worker.RunWorkerAsync()
                    timerDf.Enabled = false;
                    timerDf.Dispose();
                    timerDf = null;
                    var p = new Thread(threaddeletefiles);
                    p.Priority = ThreadPriority.Lowest;
                    p.Start();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }
        }

        private static void threaddeletefiles()
        {
            var tmpFolder = new System.IO.DirectoryInfo(Sistema.ApplicationContext.TmporaryFolder);
            if (tmpFolder.Exists == false)
                return;
            var folders = tmpFolder.GetDirectories("TDTP*");
            foreach (var folder in folders)
            {
                // Dim files As IEnumerable(Of CodeProject.FileData) = CodeProject.FastDirectoryEnumerator.EnumerateFiles(path, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)
                // For Each file As CodeProject.FileData In files
                // If Calendar.Compare(file.CreationTime, timerdfd) < 0 Then
                // Try
                // file.Delete()
                // Catch ex As Exception
                // ' Sistema.Events.NotifyUnhandledException(ex)
                // End Try
                // 'System.Threading.Thread.Sleep(100)
                // End If
                // Next
                try
                {
                    if (DMD.DateUtils.Compare(folder.CreationTime, timerdfd) < 0)
                    {
                        folder.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static void InternalLog(Exception ex)
        {
            InternalLog(ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
        }

        private static object fLock = new object();

        public static void InternalLog(string message)
        {
            lock (fLock)
            {
                try
                {
                    string path = Sistema.ApplicationContext.UserDataFolder; // System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    string fName = Application.ProductName + ".log";
                    string logFName = System.IO.Path.Combine(path, fName);
                    System.IO.File.AppendAllText(logFName, Sistema.Formats.GetTimeStamp() + " - " + message + DMD.Strings.vbNewLine);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }
        }
    }
}