Imports System.IO
Imports System.Xml.Serialization
Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Drivers
Imports minidom.Internals
Imports minidom.diallib
Imports System.Deployment.Application
Imports minidom.Win32

Public NotInheritable Class DMDSIPApp

    Public Shared Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)


    Private Shared m_Database As CDBConnection
    Private Shared m_Config As DMDSIPConfig
    Private Shared m_Configs As DialTPConfigClass
    Public Shared ReadOnly PendingCommands As New CCollection(Of DMDSIPCommand)

    Shared Sub New()
        m_Database = Nothing
        m_Config = New DMDSIPConfig
        m_Configs = Nothing
    End Sub

    Public Shared Property Database As CDBConnection
        Get
            If (m_Database Is Nothing) Then Return APPConn
            Return m_Database
        End Get
        Set(value As CDBConnection)
            m_Database = value
        End Set
    End Property


    Public Shared ReadOnly Property Configs As DialTPConfigClass
        Get
            If (m_Configs Is Nothing) Then
                m_Configs = New DialTPConfigClass
                m_Configs.Initialize()
            End If
            Return m_Configs
        End Get
    End Property

    Public Shared ReadOnly Property CurrentConfig As DMDSIPConfig
        Get
            Return m_Config
        End Get
    End Property

    Public Shared IDUltimaTelefonata As Integer = 0


    Shared Sub SetConfiguration(config As DMDSIPConfig)
        If (config Is Nothing) Then Throw New ArgumentNullException
        m_Config = config
#If Not DEBUG Then
        Try
#End If
        Dim tmp As String = Strings.Trim(CStr(config.Attributi.GetItemByKey("IconURL")))
#If Not DEBUG Then
        Catch ex As Exception

        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetAsteriskServers()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetWatchFolders()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If


#If Not DEBUG Then
        Try
#End If
        ResetUSBHandler()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If



        RaiseEvent ConfigurationChanged(Nothing, New System.EventArgs)
    End Sub

    Public Shared Sub ResetUSBHandler()
        If CStr(DMDSIPApp.CurrentConfig.Attributi.GetItemByKey("USBHandler")) = "true" Then
            If (Not USBDriveHandler.IsRunning) Then
                USBDriveHandler.Start()
            End If
        Else
            If (USBDriveHandler.IsRunning) Then
                USBDriveHandler.Stop()
            End If
        End If
    End Sub

    Private Shared Function UnescapeFolderName(ByVal folderName As String) As String
        folderName = Strings.Replace(folderName, "%%DESKTOP%%", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory))
        folderName = Strings.Replace(folderName, "%%COMMONDESKTOP%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory))
        folderName = Strings.Replace(folderName, "%%DOCUMENTS%%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        folderName = Strings.Replace(folderName, "%%COMMONDOCUMENTS%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))
        folderName = Strings.Replace(folderName, "%%APPDATA%%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        folderName = Strings.Replace(folderName, "%%COMMONAPPDATA%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))
        folderName = Strings.Replace(folderName, "%%SYSDIR%%", Environment.GetFolderPath(Environment.SpecialFolder.System))
        folderName = Strings.Replace(folderName, "%%SYSDIR86%%", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))
        folderName = Strings.Replace(folderName, "%%WINDOWSDIR%%", Environment.GetFolderPath(Environment.SpecialFolder.Windows))
        folderName = Strings.Replace(folderName, "%%PICTURES%%", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
        folderName = Strings.Replace(folderName, "%%COMMONPICTURES%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures))
        folderName = Strings.Replace(folderName, "%%MUSIC%%", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))
        folderName = Strings.Replace(folderName, "%%COMMONMUSIC%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic))
        folderName = Strings.Replace(folderName, "%%VIDEO%%", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos))
        folderName = Strings.Replace(folderName, "%%COMMONVIDEO%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos))
        Return Trim(folderName)
    End Function

    Private Shared Sub ResetWatchFolders()
        FolderWatch.StopWatching()
        For Each folderName As String In CurrentConfig.CartelleMonitorate
            folderName = UnescapeFolderName(folderName)
            If (folderName <> "") Then FolderWatch.AddFolder(folderName)
        Next
        For Each folderName As String In CurrentConfig.CartelleEscluse
            folderName = UnescapeFolderName(folderName)
            If (folderName <> "") Then FolderWatch.ExcludeFolder(folderName)
        Next
        FolderWatch.StartWatching()
    End Sub

    Private Shared Sub ResetAsteriskServers()

        ' diallib.AsteriskServers.StopListening()
        AsteriskServers.StartListening(New CCollection(Of AsteriskServer)(DMDSIPApp.CurrentConfig.AsteriskServers))

    End Sub

    Public Shared Function DequeueCommand(ByVal machine As String, ByVal user As String) As DMDSIPCommand
        SyncLock DMDSIPApp.PendingCommands
            Dim i As Integer = 0
            While (i < PendingCommands.Count)
                Dim tmp As DMDSIPCommand = PendingCommands(i)
                If (Strings.Compare(machine, tmp.IDPostazione) = 0 AndAlso Strings.Compare(user, tmp.IDUtente) = 0) Then
                    PendingCommands.RemoveAt(i)
                    Return tmp
                End If
                i += 1
            End While
            Return Nothing
        End SyncLock
    End Function


    Public Shared Function CheckForUpdates() As Boolean
        Dim info As UpdateCheckInfo = Nothing
        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            info = AD.CheckForDetailedUpdate()
            Return info.UpdateAvailable
        Else
            Return False
        End If
    End Function

    Public Shared Function CheckForRequiredUpdates() As Boolean
        Dim info As UpdateCheckInfo = Nothing
        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            info = AD.CheckForDetailedUpdate()
            Return info.UpdateAvailable AndAlso info.IsUpdateRequired
        Else
            Return False
        End If
    End Function

    Public Shared Sub InstallUpdateSyncWithInfo(ByVal forceUpdate As Boolean)
        Dim info As UpdateCheckInfo = Nothing

        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            info = AD.CheckForDetailedUpdate()
            If (info.UpdateAvailable) Then
                AD.Update()
                System.Windows.Forms.Application.Restart()
            End If
        End If
    End Sub

End Class