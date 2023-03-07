using System;
using System.Net;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.PBX
{
    public class DataEventArgs : EventArgs
    {
        public string Data;

        public DataEventArgs()
        {
            Data = "";
        }

        public DataEventArgs(string data)
        {
            Data = data;
        }
    }

    public class WebPageAnalizer : IDisposable, Sistema.IRPCCallHandler
    {
        public event DataReadyEventHandler DataReady;

        public delegate void DataReadyEventHandler(object sender, DataEventArgs e);

        private bool m_Stopping;
        private Sistema.AsyncState m_AR;
        // Private m_WC As  System.Windows.Forms.WebBrowser

        public WebPageAnalizer()
        {
            // Me.m_WC = New System.Windows.Forms.WebBrowser
            // Me.m_WC.ScriptErrorsSuppressed = True
            // AddHandler Me.m_WC.Navigating, AddressOf handleNavigating
            // AddHandler Me.m_WC.DocumentCompleted, AddressOf OnDataReady
            m_Stopping = false;
        }

        public void Analize(string url)
        {
            Cancel();
            string file = Sistema.FileSystem.GetTempFileName();
            using (var w = new WebClient())
            {
                w.DownloadFile(url, file);
            }
            string text = System.IO.File.ReadAllText(file);
            System.IO.File.Delete(file);
            var e = new DataEventArgs();
            e.Data = text;
            OnDataReady(e);
        }

        // Public ReadOnly Property WebControl As System.Windows.Forms.WebBrowser
        // Get
        // Return Me.m_WC
        // End Get
        // End Property


        public void Dispose()
        {
            if (m_AR is object)
            {
                m_AR.Cancel();
                m_AR = null;
            }
        }

        protected virtual void OnDataReady(DataEventArgs e)
        {
            // Dim webBrowserForPrinting As WebBrowser = CType(sender, WebBrowser)

            // ' Print the document now that it is fully loaded.
            // webBrowserForPrinting.Print()
            // MessageBox.Show("print")

            // Dispose the WebBrowser now that the task is complete. 
            // webBrowserForPrinting.Dispose()
            DataReady?.Invoke(this, e);
        }

        public virtual void Cancel()
        {
            // If (Me.m_Thread IsNot Nothing) Then
            m_Stopping = true;
            if (m_AR is object)
            {
                m_AR.Cancel();
                m_AR = null;
            }

            m_Stopping = false;
            // End If
        }

        // Private Sub handleNavigating(ByVal sender As Object, ByVal e As WebBrowserNavigatingEventArgs)
        // e.Cancel = Me.m_Stopping
        // End Sub

        void IRPCCallHandler.OnAsyncComplete(Sistema.AsyncResult res)
        {
            m_AR = null;
            string html = res.getResponse();
            // Dim doc As New System.Xml.XmlDocument
            // doc.LoadXml(html)
            OnDataReady(new DataEventArgs(html));
        }

        void IRPCCallHandler.OnAsyncError(Sistema.AsyncResult res)
        {
            m_AR = null;
        }
    }
}