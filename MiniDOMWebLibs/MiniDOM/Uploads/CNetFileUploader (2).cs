using System.IO;
using System.Web.UI.HtmlControls;

namespace minidom
{
    public partial class WebSite
    {
        public class CNetFileUploader : CFileUploader
        {
            private Stream m_OutputStream;
            private Stream m_InputStream;
            private HtmlInputFile m_Field;
            // Private ascb As New AsyncCallback(AddressOf HandleReceive)

            // Private ascb As New AsyncCallback(AddressOf HandleReceive)

            public CNetFileUploader()
            {
            }

            public override void Dispose()
            {
                base.Dispose();
                if (m_OutputStream is object)
                {
                    m_OutputStream.Dispose();
                    m_OutputStream = null;
                }

                if (m_InputStream is object)
                    m_InputStream = null;
                m_Field = null;
                // Me.ascb = Nothing
            }

            /// <summary>
        /// Restituisce il nome del campo del form da cui è stato caricato il file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual string FieldName
            {
                get
                {
                    return m_Field.ClientID;
                }
            }

            /// <summary>
        /// Restituisce il percorso del file sul PC remoto (da cui è stato caricato)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual string FieldValue
            {
                get
                {
                    return m_Field.Value;
                }
            }

            protected override void PrepareUpload()
            {
                // If Me.FileName = vbNullString Then
                // Do
                // Me.TargetFileName = ASPSecurity.GetRandomKey(8) & "." & FileSystem.GetExtensionName(Me.FieldValue)
                // Loop While FileSystem.FileExists(Me.TargetFileName)
                // End If

                m_InputStream = m_Field.PostedFile.InputStream;

                // minidom.Sistema.FileSystem.CreateRecursiveFolder(minidom.Sistema.FileSystem.GetFolderName (Me.BasePath))

                m_OutputStream = new FileStream(TargetFileName, FileMode.CreateNew);
            }

            protected override long GetTotalBytesToReceive()
            {
                return m_Field.PostedFile.ContentLength;
            }

            /// <summary>
        /// Avvia il caricamento
        /// </summary>
        /// <remarks></remarks>
            public void SetFields(HtmlInputFile filed, string destURL, string destFile, bool async)
            {
                m_Field = filed;
                DestinationUrl = destURL;
                TargetFileName = destFile;
                Async = async;
            }

            // Protected Overrides Sub InternalUploadAsync()
            // Me.Receive(ascb)
            // Dim ret As Boolean
            // If (WebSite.Instance.Configuration.UploadTimeOut > 0) Then
            // ret = Me.m_ManualReset.WaitOne(WebSite.Instance.Configuration.UploadTimeOut * 1000)
            // Else
            // ret = Me.m_ManualReset.WaitOne()
            // End If
            // If (ret = False) Then
            // Throw New TimeoutException
            // End If
            // End Sub

            // Private Sub Receive(ByVal cb As AsyncCallback)
            // 'Debug.Print("CFileUploade>Receive")
            // Me.m_InputStream.BeginRead(Me.buffer, 0, Me.BufferSize, cb, Nothing)
            // End Sub

            // ''' <summary>
            // ''' Gets the single line response callback.
            // ''' </summary>
            // ''' <param name="ar">The ar.</param>
            // Protected Sub HandleReceive(ByVal ar As IAsyncResult)
            // Try
            // If (Me.IsAborting) Then Throw New UploadCalcelledException("Annullato dall'utente")

            // Dim nRead As Integer = Me.m_InputStream.EndRead(ar)
            // Me.m_BytesReceived += nRead
            // Me.DoConsumer(Me.buffer, nRead)
            // Uploader.FireUploadProgress(New UploadEventArgs(Me))

            // If (Me.UploadedBytes >= Me.TotalBytes) Then
            // Me.FinalizeUpload()
            // Uploader.FireEndUpload(New UploadEventArgs(Me))
            // Me.m_ManualReset.Set()
            // Else
            // Me.LimitateSpeed(nRead)
            // Me.m_LastRead = Now
            // Me.Receive(ascb)
            // End If


            // Catch ex As Exception
            // Sistema.Events.NotifyUnhandledException(ex)
            // Me.FinalizeUpload()
            // Me.OnUploadError(New UploadErrorEventArgs(Me, ex))
            // Me.m_ManualReset.Set()
            // End Try

            // End Sub


            protected override void FinalizeUpload()
            {
                if (m_OutputStream is object)
                {
                    m_OutputStream.Dispose();
                    m_OutputStream = null;
                }
                // If (Me.m_InputStream IsNot Nothing) Then Me.m_InputStream.Dispose()

            }

            /// <summary>
        /// Legge i dati dalla sorgente nel buffer e restituisce il numero di bytes letti
        /// </summary>
        /// <remarks></remarks>
            protected override int DoReader(byte[] buffer)
            {
                return m_InputStream.Read(buffer, 0, BufferSize);
            }

            /// <summary>
        /// Scrive il numero di bytes indicati da bytes nella destinazione
        /// </summary>
        /// <remarks></remarks>
            protected override void DoConsumer(byte[] buffer, int nBytes)
            {
                m_OutputStream.Write(buffer, 0, nBytes);
                base.DoConsumer(buffer, nBytes);
            }
        }
    }
}