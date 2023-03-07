Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports minidom.diallib
Imports LumiSoft.Media.Wave
Imports LumiSoft.Net.Codec

Public Class InterfonoConnection
    '  Implements IDisposable

    Public Const DEFBUFFSIZE As Integer = 400 * 5
    Public Const BITSPERSMAPLE As Integer = 16
    Public Const SAMPLESPERSECOND As Integer = 8000
    Public Const CHANNELS As Integer = 1

    Public client As TcpClient
    Public stream As Stream
    Public Peer As Interfono
    Public InDevName As String
    Public OutDevName As String
    Public InDev As WaveIn
    Public OutDev As WaveOut
    Public params As InterfonoParams

    Public IAR As IAsyncResult
    Public buffer As Byte()
    Public buffIndex As Integer
    Public nBytesSent As Long = 0
    Public nBytesReceived As Long = 0

    Public pOutID As Integer = 0
    Public pInID As Integer = -1

    Public fStream As System.IO.Stream

    Public Sub New()
        Me.client = Nothing
        Me.fStream = Nothing
    End Sub

    Public Sub New(ByVal client As TcpClient)
        Me.client = client
        Me.stream = client.GetStream
    End Sub

    Friend Function BeginHandshake() As Boolean
        Me.params = CType(minidom.Sistema.BinaryDeserialize(Me.stream), InterfonoParams)
        For Each i As Interfono In InterfonoService.Interfoni
            If i.UniqueID = Me.params.srcID Then
                Me.Peer = i
            End If
        Next

        If (Me.Peer Is Nothing) Then
            Me.params.Result = "DMDERR|Peer not found"
            minidom.Sistema.BinarySerialize(Me.params, Me.stream)
            Return False
        End If


        Try
            Me.StartDevices()
        Catch ex As Exception
            Me.params.Result = "DMDWARNING|StartDevices|" & ex.Message
            minidom.Sistema.BinarySerialize(Me.params, Me.stream)
            Return False
        End Try

        Me.params.Result = "DMDOK"
        minidom.Sistema.BinarySerialize(Me.params, Me.stream)



        Return True
    End Function

    Friend m_StreamThread As System.Threading.Thread = Nothing

    Private Function GetFileName() As String
        Dim ret As String = Settings.WaveFolder & "\" & minidom.Sistema.Formats.GetTimeStamp & ".raw"
        Return ret
    End Function

    Friend Sub StartListening()
        If Settings.WaveFolder <> "" Then
            Dim fName As String = Me.GetFileName
            Me.fStream = New System.IO.FileStream(fName, FileMode.Create)
        End If

        Me.m_Quitting = False
        Me.m_StreamThread = New System.Threading.Thread(AddressOf Listener)
        Me.m_StreamThread.Start()
    End Sub

    Private m_Buffer As Byte()
    Private m_nRead As Integer
    Private m_Quitting As Boolean

    Private ReadOnly lock As New Object

    Friend Sub Listener()
#If Not DEBUG Then
        Try
#End If
        Log.InternalLog("Interfono.Remote Connection From " & Me.Peer.UserName & " (" & Me.Peer.Address & ")")
        Do
            Dim pl As InterfonoPayLoad
            pl = CType(minidom.Sistema.BinaryDeserialize(Me.stream), InterfonoPayLoad)
            Me.pInID += 1
            If (Me.pInID <> pl.id) Then Throw New Exception("Ordine del pacchetto non corretto")
            If (pl.buffer.Length <> pl.bufferSize) Then Throw New Exception("buffer incompleto")
            Dim crc As Integer = pl.id Xor pl.codec Xor pl.bufferSize
            If (crc <> pl.crc) Then Throw New Exception("bad crc")
            Select Case pl.type
                Case InterfonoPayLoadType.AudioData
                    Dim decodedData As Byte() = Nothing
                    Select Case pl.codec
                        Case 0 : decodedData = G711.Decode_aLaw(pl.buffer, 0, pl.buffer.Length)
                        Case 1 : decodedData = G711.Decode_uLaw(pl.buffer, 0, pl.buffer.Length)
                    End Select
                    Me.AppendBuffer(decodedData)
                Case InterfonoPayLoadType.Disconnect
                    Me.Peer.InternalDisconnect()
                    Me.m_Quitting = True
            End Select
        Loop While (Not Me.m_Quitting)
#If Not DEBUG Then
        Catch ex As System.Exception
            Me.OnError(ex)
            Try
                Me.StopListening()
            Catch ex1 As Exception
                Me.OnError(ex)
            End Try
        End Try
#End If

    End Sub

    Private m_JitterBuffer As New System.IO.MemoryStream

    Private Sub AppendBuffer(ByVal buffer As Byte())
        If (Me.OutDev Is Nothing OrElse Me.OutDev.IsDisposed) Then Return
        If (Me.fStream IsNot Nothing) Then Me.fStream.Write(buffer, 0, buffer.Length)
        m_JitterBuffer.Write(buffer, 0, buffer.Length)
        If (m_JitterBuffer.Length >= 2 * DEFBUFFSIZE) Then
            Dim arr As Byte() = m_JitterBuffer.ToArray
            m_JitterBuffer.SetLength(0)
            m_JitterBuffer.Write(arr, DEFBUFFSIZE, arr.Length - DEFBUFFSIZE)
            Me.OutDev.Play(arr, 0, DEFBUFFSIZE)
        End If
    End Sub

    Friend Sub StopListening()
#If VERBOSE > 0 Then
        diallib.Log.LogMessage("InterfonoConnection -> StopListening")
#End If
        Me.m_Quitting = True
        If (Me.fStream IsNot Nothing) Then
            Me.fStream.Dispose()
            Me.fStream = Nothing
        End If
    End Sub


    Public Sub StartDevices()
        Me.pOutID = 0
        Me.pInID = -1

        Me.InDevName = diallib.Settings.WaveInDevName
        Me.OutDev = Nothing

        Dim o As WavInDevice = Nothing
        Dim arrIn As WavInDevice() = WaveIn.Devices
        For Each dev As WavInDevice In arrIn
            If (dev.Name = Me.InDevName) Then
                o = dev
                Exit For
            End If
        Next
        If (o Is Nothing AndAlso arrIn.Length > 0) Then o = arrIn(0)
        If (o Is Nothing) Then Throw New Exception("Nessuna periferica di input disponibile")
        Me.InDev = New WaveIn(o, SAMPLESPERSECOND, BITSPERSMAPLE, CHANNELS, DEFBUFFSIZE)
        AddHandler Me.InDev.BufferFull, AddressOf InDev_BufferFull
        Me.InDev.Start()

        Me.OutDevName = Settings.WaveOutDevName
        Dim arrOut As WavOutDevice() = WaveOut.Devices
        Dim o1 As WavOutDevice = Nothing
        For Each dev As WavOutDevice In arrOut
            If (dev.Name = Me.InDevName) Then
                o1 = dev
                Exit For
            End If
        Next
        If (o1 Is Nothing AndAlso arrOut.Length > 0) Then o1 = arrOut(0)
        If (o1 Is Nothing) Then Throw New Exception("Nessuna periferica di output disponibile")
        Me.OutDev = New WaveOut(o1, SAMPLESPERSECOND, BITSPERSMAPLE, CHANNELS)


    End Sub

    Public Sub StopDevices()
        If (Me.InDev IsNot Nothing) Then
            Me.InDev.Stop()
            Me.InDev.Dispose()
            Me.InDev = Nothing
        End If
        If (Me.OutDev IsNot Nothing) Then
            Me.OutDev.Dispose()
            Me.OutDev = Nothing
        End If
    End Sub

    Friend Sub SendDisconnectCommand()
        Dim pl As New InterfonoPayLoad
        pl.id = Me.pOutID : Me.pOutID += 1
        pl.time = Now
        pl.type = InterfonoPayLoadType.Disconnect
        pl.codec = Me.params.codec
        pl.bufferSize = 1
        pl.buffer = New Byte() {1}
        pl.crc = pl.id Xor pl.codec Xor pl.bufferSize


        'Me.stream.Write(buffer, 0, buffer.Length)
        'Me.stream.Flush()
        minidom.Sistema.BinarySerialize(pl, Me.stream)
    End Sub

    Private Sub InDev_BufferFull(ByVal buffer() As Byte)
        Try
#If VERBOSE > 0 Then
        diallib.Log.LogMessage("InterfonoConnection -> InDev_BufferFull")
#End If
            If (Settings.WaveInDisabled) Then
                For i As Integer = 0 To buffer.Length - 1
                    buffer(i) = 0
                Next
            End If

            ' Compress data. 
            Dim encodedData As Byte() = Nothing
            Select Case (Me.params.codec)
                Case 0
                    encodedData = G711.Encode_aLaw(buffer, 0, buffer.Length)
                Case 1
                    encodedData = G711.Encode_uLaw(buffer, 0, buffer.Length)
                Case Else
                    Throw New Exception("Codice non supportato")
            End Select
            ' We just sent buffer to target end point.
            ' m_pUdpServer.SendPacket(encodedData, 0, encodedData.Length, m_pTargetEP)

            Me.nBytesSent += encodedData.Length

            Dim pl As New InterfonoPayLoad
            pl.id = Me.pOutID : Me.pOutID += 1
            pl.time = Now
            pl.type = InterfonoPayLoadType.AudioData
            pl.codec = Me.params.codec
            pl.bufferSize = encodedData.Length
            pl.buffer = encodedData
            pl.crc = pl.id Xor pl.codec Xor pl.bufferSize


            'Me.stream.Write(buffer, 0, buffer.Length)
            'Me.stream.Flush()
            minidom.Sistema.BinarySerialize(pl, Me.stream)

#If VERBOSE > 0 Then
        diallib.Log.LogMessage("InterfonoConnection -> InDev_BufferFull -> WriteBuffer " & encodedData.Length)
#End If
        Catch ex As System.Exception
            Me.OnError(ex)
        End Try

    End Sub

    Protected Sub OnError(ByVal ex As System.Exception)
        'minidom.Sistema.Events.NotifyUnhandledException(CType(e.ExceptionObject, System.Exception))
        Log.LogException(ex)
    End Sub

    '#Region "IDisposable Support"
    '    Private disposedValue As Boolean ' Per rilevare chiamate ridondanti

    '    ' IDisposable
    '    Protected Overridable Sub Dispose(disposing As Boolean)
    '        If Not disposedValue Then
    '            If disposing Then
    '                ' TODO: eliminare lo stato gestito (oggetti gestiti).
    '            End If

    '            ' TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di Finalize().
    '            ' TODO: impostare campi di grandi dimensioni su Null.
    '        End If

    '        'If (Me.client IsNot Nothing )Then Me.client.Close : me.client = nothing 
    '        If (Me.stream IsNot Nothing) Then Me.stream.Dispose() : Me.stream = Nothing

    '        If (Me.fStream IsNot Nothing) Then Me.fStream.Dispose() : Me.fStream = Nothing

    '        disposedValue = True
    '    End Sub

    '    ' TODO: eseguire l'override di Finalize() solo se Dispose(disposing As Boolean) include il codice per liberare risorse non gestite.
    '    'Protected Overrides Sub Finalize()
    '    '    ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
    '    '    Dispose(False)
    '    '    MyBase.Finalize()
    '    'End Sub

    '    ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
    '    Public Sub Dispose() Implements IDisposable.Dispose
    '        ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
    '        Dispose(True)
    '        ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
    '        ' GC.SuppressFinalize(Me)
    '    End Sub
    '#End Region

End Class
