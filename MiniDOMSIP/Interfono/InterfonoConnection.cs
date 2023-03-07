using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using LumiSoft.Media.Wave;
using LumiSoft.Net.Codec;
using DMD.XML;

namespace minidom.PBX
{
    public class InterfonoConnection
    {
        // Implements IDisposable

        public const int DEFBUFFSIZE = 400 * 5;
        public const int BITSPERSMAPLE = 16;
        public const int SAMPLESPERSECOND = 8000;
        public const int CHANNELS = 1;
        public TcpClient client;
        public Stream stream;
        public Interfono Peer;
        public string InDevName;
        public string OutDevName;
        public WaveIn InDev;
        public WaveOut OutDev;
        public InterfonoParams @params;
        public IAsyncResult IAR;
        public byte[] buffer;
        public int buffIndex;
        public long nBytesSent = 0L;
        public long nBytesReceived = 0L;
        public int pOutID = 0;
        public int pInID = -1;
        public Stream fStream;

        public InterfonoConnection()
        {
            client = null;
            fStream = null;
        }

        public InterfonoConnection(TcpClient client)
        {
            this.client = client;
            stream = client.GetStream();
        }

        internal bool BeginHandshake()
        {
            @params = (InterfonoParams)Sistema.BinaryDeserialize(stream);
            foreach (Interfono i in InterfonoService.Interfoni)
            {
                if (i.UniqueID == @params.srcID)
                {
                    Peer = i;
                }
            }

            if (Peer is null)
            {
                @params.Result = "DMDERR|Peer not found";
                Sistema.BinarySerialize(@params, stream);
                return false;
            }

            try
            {
                StartDevices();
            }
            catch (Exception ex)
            {
                @params.Result = "DMDWARNING|StartDevices|" + ex.Message;
                Sistema.BinarySerialize(@params, stream);
                return false;
            }

            @params.Result = "DMDOK";
            Sistema.BinarySerialize(@params, stream);
            return true;
        }

        internal Thread m_StreamThread = null;

        private string GetFileName()
        {
            string ret = Settings.WaveFolder + @"\" + Sistema.Formats.GetTimeStamp() + ".raw";
            return ret;
        }

        internal void StartListening()
        {
            if (!string.IsNullOrEmpty(Settings.WaveFolder))
            {
                string fName = GetFileName();
                fStream = new FileStream(fName, FileMode.Create);
            }

            m_Quitting = false;
            m_StreamThread = new Thread(Listener);
            m_StreamThread.Start();
        }

        private byte[] m_Buffer;
        private int m_nRead;
        private bool m_Quitting;
        private readonly object @lock = new object();

        internal void Listener()
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            Log.InternalLog("Interfono.Remote Connection From " + Peer.UserName + " (" + Peer.Address + ")");
            do
            {
                InterfonoPayLoad pl;
                pl = (InterfonoPayLoad)Sistema.BinaryDeserialize(stream);
                pInID += 1;
                if (pInID != pl.id)
                    throw new Exception("Ordine del pacchetto non corretto");
                if (pl.buffer.Length != pl.bufferSize)
                    throw new Exception("buffer incompleto");
                int crc = pl.id ^ pl.codec ^ pl.bufferSize;
                if (crc != pl.crc)
                    throw new Exception("bad crc");
                switch (pl.type)
                {
                    case InterfonoPayLoadType.AudioData:
                        {
                            byte[] decodedData = null;
                            switch (pl.codec)
                            {
                                case 0:
                                    {
                                        decodedData = G711.Decode_aLaw(pl.buffer, 0, pl.buffer.Length);
                                        break;
                                    }

                                case 1:
                                    {
                                        decodedData = G711.Decode_uLaw(pl.buffer, 0, pl.buffer.Length);
                                        break;
                                    }
                            }

                            AppendBuffer(decodedData);
                            break;
                        }

                    case InterfonoPayLoadType.Disconnect:
                        {
                            Peer.InternalDisconnect();
                            m_Quitting = true;
                            break;
                        }
                }
            }
            while (!m_Quitting);
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        private MemoryStream m_JitterBuffer = new MemoryStream();

        private void AppendBuffer(byte[] buffer)
        {
            if (OutDev is null || OutDev.IsDisposed)
                return;
            if (fStream is object)
                fStream.Write(buffer, 0, buffer.Length);
            m_JitterBuffer.Write(buffer, 0, buffer.Length);
            if (m_JitterBuffer.Length >= 2 * DEFBUFFSIZE)
            {
                var arr = m_JitterBuffer.ToArray();
                m_JitterBuffer.SetLength(0L);
                m_JitterBuffer.Write(arr, DEFBUFFSIZE, arr.Length - DEFBUFFSIZE);
                OutDev.Play(arr, 0, DEFBUFFSIZE);
            }
        }

        internal void StopListening()
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            m_Quitting = true;
            if (fStream is object)
            {
                fStream.Dispose();
                fStream = null;
            }
        }

        public void StartDevices()
        {
            pOutID = 0;
            pInID = -1;
            InDevName = Settings.WaveInDevName;
            OutDev = null;
            WavInDevice o = null;
            var arrIn = WaveIn.Devices;
            foreach (WavInDevice dev in arrIn)
            {
                if ((dev.Name ?? "") == (InDevName ?? ""))
                {
                    o = dev;
                    break;
                }
            }

            if (o is null && arrIn.Length > 0)
                o = arrIn[0];
            if (o is null)
                throw new Exception("Nessuna periferica di input disponibile");
            InDev = new WaveIn(o, SAMPLESPERSECOND, BITSPERSMAPLE, CHANNELS, DEFBUFFSIZE);
            InDev.BufferFull += InDev_BufferFull;
            InDev.Start();
            OutDevName = Settings.WaveOutDevName;
            var arrOut = WaveOut.Devices;
            WavOutDevice o1 = null;
            foreach (WavOutDevice dev in arrOut)
            {
                if ((dev.Name ?? "") == (InDevName ?? ""))
                {
                    o1 = dev;
                    break;
                }
            }

            if (o1 is null && arrOut.Length > 0)
                o1 = arrOut[0];
            if (o1 is null)
                throw new Exception("Nessuna periferica di output disponibile");
            OutDev = new WaveOut(o1, SAMPLESPERSECOND, BITSPERSMAPLE, CHANNELS);
        }

        public void StopDevices()
        {
            if (InDev is object)
            {
                InDev.Stop();
                InDev.Dispose();
                InDev = null;
            }

            if (OutDev is object)
            {
                OutDev.Dispose();
                OutDev = null;
            }
        }

        internal void SendDisconnectCommand()
        {
            var pl = new InterfonoPayLoad();
            pl.id = pOutID;
            pOutID += 1;
            pl.time = DMD.DateUtils.Now();
            pl.type = InterfonoPayLoadType.Disconnect;
            pl.codec = @params.codec;
            pl.bufferSize = 1;
            pl.buffer = new byte[] { 1 };
            pl.crc = pl.id ^ pl.codec ^ pl.bufferSize;


            // Me.stream.Write(buffer, 0, buffer.Length)
            // Me.stream.Flush()
            Sistema.BinarySerialize(pl, stream);
        }

        private void InDev_BufferFull(byte[] buffer)
        {
            try
            {
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (Settings.WaveInDisabled)
                {
                    for (int i = 0, loopTo = buffer.Length - 1; i <= loopTo; i++)
                        buffer[i] = 0;
                }

                // Compress data. 
                byte[] encodedData = null;
                switch (@params.codec)
                {
                    case 0:
                        {
                            encodedData = G711.Encode_aLaw(buffer, 0, buffer.Length);
                            break;
                        }

                    case 1:
                        {
                            encodedData = G711.Encode_uLaw(buffer, 0, buffer.Length);
                            break;
                        }

                    default:
                        {
                            throw new Exception("Codice non supportato");
                            break;
                        }
                }
                // We just sent buffer to target end point.
                // m_pUdpServer.SendPacket(encodedData, 0, encodedData.Length, m_pTargetEP)

                nBytesSent += encodedData.Length;
                var pl = new InterfonoPayLoad();
                pl.id = pOutID;
                pOutID += 1;
                pl.time = DMD.DateUtils.Now();
                pl.type = InterfonoPayLoadType.AudioData;
                pl.codec = @params.codec;
                pl.bufferSize = encodedData.Length;
                pl.buffer = encodedData;
                pl.crc = pl.id ^ pl.codec ^ pl.bufferSize;


                // Me.stream.Write(buffer, 0, buffer.Length)
                // Me.stream.Flush()
                Sistema.BinarySerialize(pl, stream);
            }

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected void OnError(Exception ex)
        {
            // minidom.Sistema.Events.NotifyUnhandledException(CType(e.ExceptionObject, System.Exception))
            Log.LogException(ex);
        }

        // #Region "IDisposable Support"
        // Private disposedValue As Boolean ' Per rilevare chiamate ridondanti

        // ' IDisposable
        // Protected Overridable Sub Dispose(disposing As Boolean)
        // If Not disposedValue Then
        // If disposing Then
        // ' TODO: eliminare lo stato gestito (oggetti gestiti).
        // End If

        // ' TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di Finalize().
        // ' TODO: impostare campi di grandi dimensioni su Null.
        // End If

        // 'If (Me.client IsNot Nothing )Then Me.client.Close : me.client = nothing 
        // If (Me.stream IsNot Nothing) Then Me.stream.Dispose() : Me.stream = Nothing

        // If (Me.fStream IsNot Nothing) Then Me.fStream.Dispose() : Me.fStream = Nothing

        // disposedValue = True
        // End Sub

        // ' TODO: eseguire l'override di Finalize() solo se Dispose(disposing As Boolean) include il codice per liberare risorse non gestite.
        // 'Protected Overrides Sub Finalize()
        // '    ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
        // '    Dispose(False)
        // '    MyBase.Finalize()
        // 'End Sub

        // ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        // Public Sub Dispose() Implements IDisposable.Dispose
        // ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
        // Dispose(True)
        // ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
        // ' GC.SuppressFinalize(Me)
        // End Sub
        // #End Region

    }
}