using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using DMD.XML;
using DMD.Zip;
using minidom.Win32;

namespace minidom.PBX
{
    [Serializable]
    public class CLogSession 
        : IDisposable
    {
        public static object sessionsLock = new object();
        public static int SessionsCount = 0;
        public int SessionID;
        public DateTime StartTime;
        public ArrayList ScreenShots;
        public System.Text.StringBuilder logBuffer;
        public System.Text.StringBuilder textBuffer;
        public ArrayList keysBuffer;
        public ArrayList filesBuffer;
        public string Description;
        private DateTime? m_LastScreenShot = default;
        private IntPtr m_LastWin;
        private bool m_Saved = false;

        public CLogSession()
        {
            DMDObject.IncreaseCounter(this);
            lock (sessionsLock)
            {
                ScreenShots = new ArrayList();
                logBuffer = new System.Text.StringBuilder();
                textBuffer = new System.Text.StringBuilder();
                keysBuffer = new ArrayList();
                filesBuffer = new ArrayList();
                Description = "";
                SessionID = SessionsCount;
                SessionsCount += 1;
                StartTime = DMD.DateUtils.Now();
            }
        }

        public void LogMessage(string str)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            var d = DMD.DateUtils.Now();
            str = Sistema.Formats.FormatUserDateTime(d) + DMD.Strings.vbTab + str + "<br/>";
            logBuffer.Append(str);
        }

        public void LogKey(Keyboard.KeyboardEventArgs e)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            keysBuffer.Add(e);
            if (e.IsKeyUp && e.IsPrintable())
                textBuffer.Append(e.Char);
        }

        public void LogFile(System.IO.FileSystemEventArgs e)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    {
                        filesBuffer.Add(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - File Creato: \"" + e.FullPath + "\" ");
                        break;
                    }

                case System.IO.WatcherChangeTypes.Deleted:
                    {
                        filesBuffer.Add(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - File Eliminato: \"" + e.FullPath + "\" ");
                        break;
                    }

                case System.IO.WatcherChangeTypes.Changed:
                    {
                        filesBuffer.Add(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - File Modificato: \"" + e.FullPath + "\" ");
                        break;
                    }

                case System.IO.WatcherChangeTypes.Renamed:
                    {
                        filesBuffer.Add(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - File Rinominato da \"" + ((System.IO.RenamedEventArgs)e).OldFullPath + "\" a \"" + ((System.IO.RenamedEventArgs)e).FullPath + "\" ");
                        break;
                    }
            }
        }

        // Public ReadOnly Property ScreenShots As System.Collections.ArrayList
        // Get
        // SyncLock sessionsLock
        // If (Me.m_ScreenShots Is Nothing) Then Me.m_ScreenShots = New System.Collections.ArrayList
        // Return Me.m_ScreenShots
        // End SyncLock
        // End Get
        // End Property

        public ScreenShot AddScreenShot(string nome, bool isFullScreen, int x, int y, int width, int height, Bitmap content)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            var ret = new ScreenShot(nome, isFullScreen, x, y, width, height, content);
            ScreenShots.Add(ret);
            return ret;
        }




        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            if (m_Saved == false)
                throw new Exception("non salvato");
            ScreenShots = null;
            filesBuffer = null;
            keysBuffer = null;
            logBuffer = null;
            textBuffer = null;
            Description = DMD.Strings.vbNullString;
            if (ScreenShots is object)
            {
                foreach (ScreenShot sc in ScreenShots)
                    sc.Dispose();
            }

            ScreenShots = null;
        }




        // Public Sub Upload()
        // If Me.GetUploadServer <> "" Then
        // My.Computer.Network.UploadFile(Me.FileName, Me.GetUploadServer & "?kname=" & My.Computer.Name)
        // End If
        // End Sub

        private string DataPath()
        {
            return DMD.Strings.Right("0000" + StartTime.Year, 4) + DMD.Strings.Right("00" + StartTime.Month, 2) + DMD.Strings.Right("00" + StartTime.Day, 2) + DMD.Strings.Right("00" + StartTime.Hour, 2) + DMD.Strings.Right("00" + StartTime.Minute, 2) + DMD.Strings.Right("00" + StartTime.Second, 2);
        }

        private string m_FileName = "";

        public string GetDefaultFileName()
        {
            if (string.IsNullOrEmpty(m_FileName))
            {
                m_FileName = Sistema.ApplicationContext.SystemDataFolder;
                m_FileName = System.IO.Path.Combine(m_FileName, "TDTP" + DMD.DateUtils.FormatISODate(DMD.DateUtils.ToDay()));
                m_FileName = System.IO.Path.Combine(m_FileName, "SES" + DataPath() + DMD.Strings.Right("0000" + DMD.Integers.Hex(SessionID), 4) + ".dtp");
            }

            return m_FileName;
        }

        public void EnsureFolder()
        {
            string fileName = GetDefaultFileName();
            string dirName = Sistema.FileSystem.GetFolderName(fileName);
            Sistema.FileSystem.CreateRecursiveFolder(dirName);
            System.IO.File.SetAttributes(dirName, System.IO.FileAttributes.Hidden);
        }

        public void Save(string fileName)
        {
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            
            string tmpName = System.IO.Path.GetTempFileName();
            using (var stream = new System.IO.FileStream(tmpName, System.IO.FileMode.Create)) {
                serializer.Serialize(stream, this);
            }

            using (var @out = new System.IO.StringWriter())
            {
                using (var zip = new ZipFile())
                {
                    zip.StatusMessageTextWriter = @out;
                    zip.ZipErrorAction = ZipErrorAction.Skip;
                    // zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
                    zip.AddFile(tmpName, "");
                    zip.CompressionLevel = CompressionLevel.BestCompression;   // Ionic.Zlib.CompressionLevel.BestCompression
                    zip.CompressionMethod = CompressionMethod.Deflate; // CompressionMethod.BZip2
                    zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                    using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                    {
                        zip.Save(stream);
                    }
                }
            }
              

            try
            {
                if (System.IO.File.Exists(tmpName))
                {
                    System.IO.File.Delete(tmpName);
                }
            }
            catch (Exception ex)
            {
                // MsgBox("error")
                Sistema.Events.NotifyUnhandledException(ex);
                Debug.Print(ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
            }

            m_Saved = true;
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

        public static CLogSession Load(string fileName)
        {
            //string tmpName = System.IO.Path.GetTempFileName();
            //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = null;

            //using (var stream = new System.IO.FileStream(tmpName, System.IO.FileMode.Create))
            //{
            //    using (var zip = new ZipFile(fileName))
            //    {
            //        var e = zip.Entries.ElementAtOrDefault(0);
            //        e.Extract(stream);
            //        // zip.ExtractAll(GetLogFolder, ExtractExistingFileAction.OverwriteSilently)
            //    }

            //    // FixDialTP(stream)
            //    stream.Position = 0L;
            //    serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //    CLogSession ret = (CLogSession)serializer.Deserialize(stream);
            //    stream.Dispose();
            //    try
            //    {
            //        System.IO.File.Delete(tmpName);
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Diagnostics.Debug.Print(ex.Message);
            //    }
            //}

            //return ret;
            throw new NotImplementedException();
        }

        public void LogException(Exception ex)
        {
            LogMessage(DMD.RunTime.vbTypeName(ex) + " -> " + ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
        }

        ~CLogSession()
        {
            DMDObject.DecreaseCounter(this);
        }

        // Sub LogFileEvent(e As IO.FileSystemEventArgs)
        // Me.filesBuffer.Add(e)
        // If (TypeOf (e) Is System.IO.RenamedEventArgs) Then
        // With DirectCast(e, System.IO.RenamedEventArgs)
        // Me.LogMessage("File rinominato da [" & .OldFullPath & "] a [" & .FullPath & "]")
        // End With
        // Else
        // Select Case e.ChangeType
        // Case IO.WatcherChangeTypes.Created
        // Me.LogMessage("File creato [" & e.FullPath & "]")
        // Case IO.WatcherChangeTypes.Deleted
        // Me.LogMessage("File eliminato [" & e.FullPath & "]")
        // Case IO.WatcherChangeTypes.Changed
        // Me.LogMessage("File modificato [" & e.FullPath & "]")
        // End Select
        // End If
        // End Sub

        public bool CanTakeScreenShot()
        {
            if (m_LastScreenShot.HasValue)
            {
                return Maths.Abs(DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Second, m_LastScreenShot.Value, DMD.DateUtils.Now())) > 1d;
            }
            else
            {
                return true;
            }
        }

        public void TakeFullScreenShot()
        {
            var hWnd = IntPtr.Zero;
            var hDCWin = IntPtr.Zero;
            Graphics gSrc = null;
            var hDCImg = IntPtr.Zero;
            Bitmap img = null;
            Graphics g = null;
            Pen pen = null;

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            if (!CanTakeScreenShot())
                return;

            // If DialTPApp.CurrentConfig.WindowShot = False Then Return
            if (Sistema.ApplicationContext.IsDebug())
            {
                Sistema.ApplicationContext.Log("TakeFullScreenShot");
            }

            if (hWnd.ToInt32() == 0)
                hWnd = Window.GetDesktopWindow(); // Window.GetForegroundWindow '
            if (hDCWin.ToInt32() == 0)
                hDCWin = Window.GetWindowDC(hWnd);
            Window.RECTAPI rect = default;
            Window.GetWindowRect(hWnd, ref rect);
            gSrc = Graphics.FromHdc(hDCWin);
            int w = (int)(Maths.Abs(rect.right - rect.left) + 1d);
            int h = (int)(Maths.Abs(rect.bottom - rect.top) + 1d);
            img = new Bitmap(w, h, gSrc);
            g = Graphics.FromImage(img);
            hDCImg = g.GetHdc();
            Window.StretchBlt(hDCImg, 0, 0, img.Width, img.Height, hDCWin, 0, 0, img.Width, img.Height, Window.TernaryRasterOperations.SRCCOPY);
            if (!hDCImg.Equals(IntPtr.Zero))
            {
                g.ReleaseHdc(hDCImg);
                hDCImg = IntPtr.Zero;
            }

            var color = Color.FromArgb(150, 255, 0, 0);
            pen = new Pen(color, 2f);
            {
                var withBlock = Mouse.Position;
                g.DrawLine(pen, 0, withBlock.Y - rect.top, img.Width, withBlock.Y - rect.top);
                g.DrawLine(pen, withBlock.X - rect.left, 0, withBlock.X - rect.left, img.Height);
            }

            AddScreenShot("FULL SCREENSHOT " + Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()), true, 0, 0, img.Width, img.Height, img);
            m_LastScreenShot = DMD.DateUtils.Now();
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            if (pen is object)
            {
                pen.Dispose();
                pen = null;
            }

            if (g is object)
            {
                if (!hDCImg.Equals(IntPtr.Zero))
                {
                    g.ReleaseHdc(hDCImg);
                    hDCImg = IntPtr.Zero;
                }

                g.Dispose();
                g = null;
            }

            if (!hWnd.Equals(IntPtr.Zero))
            {
                if (!hDCWin.Equals(IntPtr.Zero))
                {
                    Window.ReleaseDC(hWnd, hDCWin);
                    hDCWin = IntPtr.Zero;
                }

                hWnd = IntPtr.Zero;
            }

            if (gSrc is object)
            {
                gSrc.Dispose();
                gSrc = null;
            }
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

        }

        public void TakeMouseScreenShot()
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            if (!CanTakeScreenShot())
                return;
            var currWin = Window.GetForegroundWindow();
            if (!m_LastWin.Equals(currWin))
            {
                TakeFullScreenShot();
            }
            else
            {
                var p = Mouse.Position;
                var img = Window.GetDesktopPortion(p.X - 100, p.Y - 100, 200, 200);
                AddScreenShot("PARZIALE", false, p.X - 100, p.Y - 100, 200, 200, img);
                m_LastScreenShot = DMD.DateUtils.Now();
            }

            m_LastWin = currWin;
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        public void CheckWindow()
        {
            var currWin = Window.GetForegroundWindow();
            if (!m_LastWin.Equals(currWin))
                TakeFullScreenShot();
            m_LastWin = currWin;
        }
    }
}