using System;
using System.Globalization;
using System.Threading;
using DMD;

namespace minidom
{
    public partial class WebSite
    {
        public class CFileUploader : Databases.DBObjectBase, IDisposable
        {
            public event UploadBeginEventHandler UploadBegin;

            public delegate void UploadBeginEventHandler(object sender, UploadEventArgs e);

            public event UploadProgressEventHandler UploadProgress;

            public delegate void UploadProgressEventHandler(object sender, UploadEventArgs e);

            public event UploadCompleteEventHandler UploadComplete;

            public delegate void UploadCompleteEventHandler(object sender, UploadEventArgs e);

            public event UploadErrorEventHandler UploadError;

            public delegate void UploadErrorEventHandler(object sender, UploadErrorEventArgs e);

            private string m_Key;
            private string m_BasePath;
            private string m_FileName;
            private DateTime m_StartTime;
            private DateTime m_EndTime;
            private bool m_Async;
            private string m_DestinationFile;
            private string m_DestURL;

            /// <summary>
        /// Data e ora dell'ultima lettura completa (reader)
        /// </summary>
        /// <remarks></remarks>
            protected DateTime m_LastRead;

            /// <summary>
        /// Numero totale di bytes da ricevere
        /// </summary>
        /// <remarks></remarks>
            protected long m_BytesToReceive;

            /// <summary>
        /// Conteggio dei bytes ricevuti
        /// </summary>
        /// <remarks></remarks>
            protected long m_BytesReceived;

            /// <summary>
        /// Eventuale eccezione durante l'uload
        /// </summary>
        /// <remarks></remarks>
            private Exception m_Exception;

            /// <summary>
        /// Se vero indica che l'utente ha richiesto l'annullamento del download .Abort()
        /// </summary>
        /// <remarks></remarks>
            private bool m_Cancel;



            /// <summary>
        /// Semaforo per le operazioni asincrone
        /// </summary>
        /// <remarks></remarks>
            protected ManualResetEvent m_ManualReset;

            /// <summary>
        /// Dimensione del buffer
        /// </summary>
        /// <remarks></remarks>
            private int m_BufferSize;

            /// <summary>
        /// Buffer utilizzato per lo scambio tra il reader ed il consumer
        /// </summary>
        /// <remarks></remarks>
            protected byte[] buffer;

            private delegate void UploadDelegate();

            private UploadDelegate m_AsyncCall;
            private IAsyncResult m_AsyncRes;

            public CFileUploader()
            {
                m_Key = "";
                m_BasePath = Instance.Configuration.PublicURL;
                m_Async = false;
                // ReDim m_Buffer(BUFFERSIZE - 1)
                m_ManualReset = new ManualResetEvent(false);
                m_Cancel = false;
                m_DestinationFile = "";
                m_BufferSize = Math.Max(512, Instance.Configuration.UploadBufferSize);
                buffer = new byte[m_BufferSize];
                m_AsyncCall = InternalUploadSync;
                m_AsyncRes = null;
            }

            /// <summary>
        /// Restituisce o imposta la dimensione del buffer utilizzato per l'upload
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int BufferSize
            {
                get
                {
                    return m_BufferSize;
                }

                set
                {
                    m_BufferSize = Math.Max(512, value);
                    buffer = new byte[m_BufferSize];
                }
            }



            /// <summary>
        /// Restituisce vero se è stato richiesto alla classe di abortire un upoload
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsAborting()
            {
                return m_Cancel;
            }

            /// <summary>
        /// Restituisce vero se la procedura è terminata
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool IsCompleted()
            {
                return Exception is null && UploadedBytes >= TotalBytes;
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se utilizzare il codice asincrono
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Async
            {
                get
                {
                    return m_Async;
                }

                set
                {
                    m_Async = value;
                }
            }

            public Exception Exception
            {
                get
                {
                    return m_Exception;
                }
            }

            public override Sistema.CModule GetModule()
            {
                return null;
            }

            /// <summary>
        /// Annulla il download
        /// </summary>
        /// <remarks></remarks>
            public void Abort()
            {
                if (IsCompleted() == false)
                {
                    m_Cancel = true;
                    FinalizeUpload();
                }
            }

            /// <summary>
        /// Restituisce la chiave univoca associata quando viene creato il download
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Key
            {
                get
                {
                    return m_Key;
                }
            }

            /// <summary>
        /// Imposta la chiave univoca che identifica il download
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
            internal void SetKey(string value)
            {
                m_Key = value;
            }

            /// <summary>
        /// Restituisce o imposta il nome ed il percorso del file da salvare sul server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Se questo valore viene lasciato vuoto verrà generato un nome automaticamente in base al contenuto caricato</remarks>
            public string FileName
            {
                get
                {
                    return m_FileName;
                }

                set
                {
                    m_FileName = Strings.Trim(value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL del percorso base di caricamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string BasePath
            {
                get
                {
                    return m_BasePath;
                }

                set
                {
                    m_BasePath = Strings.Trim(value);
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.Right(m_BasePath, 1), "/", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        m_BasePath = Strings.Left(m_BasePath, Strings.Len(m_BasePath) - 1);
                }
            }



            /// <summary>
        /// Restituisce il percorso fisico in cui è stato salvato il file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TargetFileName
            {
                get
                {
                    return m_DestinationFile;
                }

                set
                {
                    m_DestinationFile = value;
                }
            }

            /// <summary>
        /// Restituisce la URL del file salvato sul server
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DestinationUrl
            {
                get
                {
                    return m_DestURL;
                }

                set
                {
                    m_DestURL = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
        /// Restituisce la percentuale di caricamento del file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Percentage
            {
                get
                {
                    return 100d * m_BytesReceived / TotalBytes;
                }
            }

            /// <summary>
        /// Restituisce i bytes finora ricevuti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public long UploadedBytes
            {
                get
                {
                    return m_BytesReceived;
                }

                set
                {
                    m_BytesReceived = value;
                }
            }

            /// <summary>
        /// Restituisce il numero totale di bytes inviati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public long TotalBytes
            {
                get
                {
                    return m_BytesToReceive;
                }

                set
                {
                    m_BytesReceived = value;
                }
            }

            /// <summary>
        /// Restituisce la data e ora di inizio del caricamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime StartTime
            {
                get
                {
                    return m_StartTime;
                }
            }

            /// <summary>
        /// Restituisce la data e l'ora di fine caricamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime EndTime
            {
                get
                {
                    return m_EndTime;
                }
            }

            public override string GetTableName()
            {
                return "tbl_FileUploads";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.LOGConn;
            }

            protected virtual void OnBeforeBeginUpload(UploadEventArgs e)
            {
                UploadBegin?.Invoke(this, e);
                Uploader.FireBeginUpload(e);
            }

            protected virtual void PrepareUpload()
            {
            }

            protected virtual long GetTotalBytesToReceive()
            {
                return 0L;
            }

            public void Upload()
            {
                try
                {
                    m_StartTime = DMD.DateUtils.Now();
                    m_BytesReceived = 0L;
                    m_BytesToReceive = GetTotalBytesToReceive();
                    m_Cancel = false;
                    PrepareUpload();
                    var e = new UploadEventArgs(this);
                    OnBeforeBeginUpload(e);
                    if (Async)
                    {
                        m_AsyncRes = m_AsyncCall.BeginInvoke(doEndAsync, this);
                        m_AsyncCall.EndInvoke(m_AsyncRes);
                    }
                    else
                    {
                        InternalUploadSync();
                    }

                    FinalizeUpload();
                    m_EndTime = DMD.DateUtils.Now();
                    OnAfterUpload(e);
                }
                // End If

                catch (Exception ex)
                {
                    FinalizeUpload();
                    OnUploadError(new UploadErrorEventArgs(this, ex));
                }
            }

            private void doEndAsync(IAsyncResult o)
            {
                // Try

                // Catch ex As Exception
                // Sistema.Events.NotifyUnhandledException(ex)
                // '
                // End Try
            }

            // Protected Overridable Sub InternalUploadAsync()
            // Me.InternalUploadSync()
            // End Sub

            protected virtual void InternalUploadSync()
            {
                m_LastRead = DMD.DateUtils.Now();
                while (!IsCompleted() && !m_Cancel)
                {
                    if (m_Cancel)
                    {
                        throw new UploadCalcelledException();
                    }
                    else
                    {
                        // Leggiamo
                        int nRead;
                        nRead = DoReader(buffer);
                        if (nRead <= 0)
                        {
                            Instance.AppContext.Log(Sistema.Formats.FormatUserDate(DMD.DateUtils.Now()) + " - Lettura di dimensione 0 caricando il file " + m_DestURL + " (" + m_BytesReceived + " / " + m_BytesToReceive + ")");
                            break;
                        }

                        m_BytesReceived += nRead;

                        // Scriviamo
                        DoConsumer(buffer, nRead);
                        // Me.m_BytesReceived += Me.m_BytesRead

                        // Se sono impostati dei limiti di velocità applichiamoli
                        LimitateSpeed(nRead);
                        m_LastRead = DMD.DateUtils.Now();
                        var e = new UploadEventArgs(this);
                        OnUploadProgress(e);
                    }
                }
            }

            protected virtual void OnUploadProgress(UploadEventArgs e)
            {
                UploadProgress?.Invoke(this, e);
                Uploader.FireUploadProgress(e);
            }

            /// <summary>
        /// Questo metodo viene richiamato per rilasciare le risorse allocate per l'upload
        /// </summary>
        /// <remarks></remarks>
            protected virtual void FinalizeUpload()
            {
            }

            protected virtual void OnAfterUpload(UploadEventArgs e)
            {
                UploadComplete?.Invoke(this, e);
                Uploader.FireEndUpload(e);
            }

            protected virtual void OnUploadError(UploadErrorEventArgs e)
            {
                m_Exception = e.Cause;
                UploadError?.Invoke(this, e);
                Uploader.FireUploadError(e);
            }


            /// <summary>
        /// Legge i dati dalla sorgente nel buffer e restituisce il numero di bytes letti
        /// </summary>
        /// <param name="buffer">[out] buffer (di dimensione BufferSize) in cui vengono caricati i dati letti</param>
        /// <returns>Restituisce il numero di byres letti e caricati nel buffer</returns>
        /// <remarks></remarks>
            protected virtual int DoReader(byte[] buffer)
            {
                return 0;
            }

            /// <summary>
        /// Scrive il numero di bytes indicati da bytes nella destinazione
        /// </summary>
        /// <param name="buffer">[in] Buffer di dimensione BufferSize in cui si trovano i dati</param>
        /// <param name="nBytes">[in] Numero di bytes validi all'interno del buffer</param>
        /// <remarks></remarks>
            protected virtual void DoConsumer(byte[] buffer, int nBytes)
            {
            }

            protected virtual void LimitateSpeed(int nRead)
            {
                if (!IsCompleted() && Instance.Configuration.UploadSpeedLimit > 0)
                {
                    var t = DMD.DateUtils.Now() - m_LastRead;
                    double speed = 0d;
                    int delay;
                    if (t.TotalMilliseconds > 0d)
                    {
                        speed = nRead * 1000d / t.TotalMilliseconds;
                        delay = (int)(speed / Instance.Configuration.UploadSpeedLimit);
                    }
                    else
                    {
                        delay = (int)Math.Floor(BufferSize * 1000 / (double)Instance.Configuration.UploadSpeedLimit);
                    }

                    if (delay > 0)
                        Thread.Sleep(Math.Min(delay, 2000));
                }
            }



            // This code added by Visual Basic to correctly implement the disposable pattern.
            public virtual void Dispose()
            {
                FinalizeUpload();
                if (m_ManualReset is object)
                {
                    m_ManualReset.Dispose();
                    m_ManualReset = null;
                }

                m_Key = DMD.Strings.vbNullString;
                m_BasePath = DMD.Strings.vbNullString;
                m_FileName = DMD.Strings.vbNullString;
                m_DestinationFile = DMD.Strings.vbNullString;
                m_DestURL = DMD.Strings.vbNullString;
                m_Exception = null;
                if (buffer is object)
                {
                    buffer = null;
                    buffer = null;
                }
            }

            public override string ToString()
            {
                return "{" + Key + ", " + FileName + "}";
            }
        }
    }
}