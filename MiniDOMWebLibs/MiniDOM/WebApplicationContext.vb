﻿Imports Microsoft.VisualBasic
Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica
Imports System.Web
Imports System.Web.SessionState
Imports System.Net
Imports System.Threading
Imports System.Timers
Imports System.IO
Imports System.Text
Imports System.Security
Imports System.Runtime.InteropServices

Partial Class WebSite

    Public MustInherit Class WebApplicationContext
        Implements minidom.Sistema.IApplicationContext

        ''' <summary>
        ''' Evento generato il certificato SSL utilizzato da un sito non è valido
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event CertificateValidationFail(ByVal sender As Object, ByVal e As MailCertificateEventArgs)


        Private m_Settings As CApplicationSettings
        Private m_BaseName As String = "frmdmd"
        Private m_Info As New CKeyCollection(Of CSessionInfo)

        Public ReadOnly applicationLock As New Object

        Private oldCheckMails As Boolean = False
        Private m_Maintenance As Boolean = False
        Private m_mainenanceDBCollection As CCollection(Of CDBConnection)
        Private m_LogStream As System.IO.Stream = Nothing
        Private m_LogWriter As System.IO.StreamWriter = Nothing
        Private m_LogFileName As String = ""
        Private m_SettingsFileName As String = "/app_data/minidom/settings.xml"
        Private m_PublicUrl As String = "/public/minidom/"
        Private m_DBURL As String = "/mdb-database/minidom/"
        Private m_StartupFolder As String = ""



        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal baseName As String, ByVal baseFolder As String, ByVal publicUrl As String, ByVal baseDB As String)
            Me.New
            Me.SetStartupFloder(baseFolder)
            Me.m_BaseName = baseName
            Me.m_PublicUrl = publicUrl
            Me.m_DBURL = baseDB
        End Sub

        Public ReadOnly Property PublicURL As String
            Get
                Return Me.m_PublicUrl
            End Get
        End Property

        Public ReadOnly Property DBURL As String
            Get
                Return Me.m_DBURL
            End Get
        End Property


        Protected Overridable Function GetLogFileName() As String
            SyncLock Me.applicationLock
                If (Me.m_LogFileName = "") Then
                    Const validChars As String = "abcdefghijklmnopqrstuvwxyz1234567890"
                    Dim baseName As String = ""
                    For Each ch As Char In Me.BaseName
                        If (validChars.IndexOf(LCase(ch)) >= 0) Then baseName &= ch
                    Next

                    Me.m_LogFileName = System.IO.Path.Combine(Me.SystemDataFolder, "Log\" & Formats.GetTimeStamp() & baseName & ".log")
                    While (System.IO.File.Exists(Me.m_LogFileName))
                        Me.m_LogFileName = System.IO.Path.Combine(Me.SystemDataFolder, "Log\" & Formats.GetTimeStamp() & baseName & ".log")
                    End While
                End If
                Return Me.m_LogFileName
            End SyncLock
        End Function

        Protected Overridable Sub RegisterTypeProviders()
            Me.Log("Inizializzo i type providers")
            Sistema.Types.RegisteredTypeProviders.Add("CAzienda", AddressOf Anagrafica.Aziende.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CPersonaFisica", AddressOf Anagrafica.Persone.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CUser", AddressOf Sistema.Users.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CGroup", AddressOf Sistema.Groups.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CModule", AddressOf Sistema.Modules.GetItemById)
            Sistema.Types.RegisteredTypeProviders.Add("CProcedura", AddressOf Sistema.Procedure.GetItemById)
        End Sub

        Protected Overridable Sub UnregisterTypeProviders()
            Me.Log("Termino i type providers")
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CAzienda")
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CPersonaFisica")
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CUser")
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CGroup")
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CModule")
            Sistema.Types.RegisteredTypeProviders.RemoveByKey("CProcedura")
        End Sub

        Private Sub CheckLogStream()
            If (Me.m_LogStream Is Nothing) Then
                Me.m_LogStream = New System.IO.FileStream(Me.GetLogFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.Read)
                Me.m_LogWriter = New System.IO.StreamWriter(Me.m_LogStream)
            End If
        End Sub

        Private Sub CloseLogStream()
            If (Me.m_LogStream IsNot Nothing) Then
                Me.m_LogWriter.Dispose()
                Me.m_LogStream.Close()
                Me.m_LogStream.Dispose()
                Me.m_LogWriter = Nothing
                Me.m_LogStream = Nothing
            End If
            Me.m_LogFileName = ""
        End Sub

        Public Overridable Sub BeginLog()
            Dim buffer As New System.Text.StringBuilder
            buffer.Append(minidom.Sistema.Strings.NChars(80, "-"))
            buffer.Append(Formats.FormatUserDateTime(Now))
            buffer.Append(vbNewLine)
            Me.Log(buffer.ToString)
        End Sub

        Public Overridable Sub EndLog()
            Dim buffer As New System.Text.StringBuilder
            buffer.Append(vbNewLine)
            buffer.Append(vbNewLine)
            buffer.Append(vbNewLine)
            buffer.Append(minidom.Sistema.Strings.NChars(80, "*"))
            Me.Log(buffer.ToString)


        End Sub

        Public Overridable Function RotateLog() As String
            SyncLock Me.applicationLock
                Dim targetFile As String = ""

                If (Me.m_LogStream IsNot Nothing) Then
                    Me.m_LogWriter.Flush()
                    Me.m_LogStream.Flush()
                    Me.m_LogStream.Close()
                    Me.m_LogStream.Dispose()

                    Me.m_LogStream = Nothing

                    'Me.m_LogWriter.Dispose()
                    Me.m_LogWriter = Nothing

                End If

                Dim logfile As String = Me.GetLogFileName
                Dim logfolder As String = Sistema.FileSystem.GetFolderName(logfile)

                If (Sistema.FileSystem.FileExists(logfile)) Then
                    'Iviamo il file via mail

                    Dim i As Integer = 0
                    Dim ext As String = Sistema.FileSystem.GetExtensionName(logfile)
                    Dim fn As String = Sistema.FileSystem.GetBaseName(logfile)

                    targetFile = logfolder & "\" & fn & CStr(i) & "." & ext
                    While Sistema.FileSystem.FileExists(targetFile)
                        i += 1
                        targetFile = logfolder & "\" & fn & CStr(i) & "." & ext
                    End While

                    Sistema.FileSystem.MoveFile(logfile, targetFile)
                    Sistema.FileSystem.DeleteFile(logfile, True)
                End If


                Return targetFile
            End SyncLock
        End Function

        Public Overridable Sub Log(ByVal message As String) Implements IApplicationContext.Log
            SyncLock Me.applicationLock
                Me.CheckLogStream()
                Me.m_LogWriter.WriteLine(message)
                Me.m_LogWriter.Flush()
            End SyncLock
        End Sub


        Public Function GetAllSessions() As CCollection(Of CSessionInfo)
            SyncLock Me.applicationLock
                Return New CCollection(Of CSessionInfo)(Me.m_Info)
            End SyncLock
        End Function

        Private m_MaxSessionsTimeout As Integer = 60

        ''' <summary>
        ''' Restituisce o imposta il limite massimo (in minuti) oltre il quale una sessione inattiva viene terminata
        ''' </summary>
        ''' <returns></returns>
        Public Property MaxSessionsTimeout As Integer
            Get
                Return Me.m_MaxSessionsTimeout
            End Get
            Set(value As Integer)
                If (value < 1) Then Throw New ArgumentNullException("Il limite non puà essere < 1")
                Me.m_MaxSessionsTimeout = value
            End Set
        End Property



        ''' <summary>
        ''' Questa procedura controlla l'effettivo utilizzo delle sessioni e termina quelle non più attive
        ''' </summary>
        Public Sub SessionsMaintenance()
            SyncLock Me.applicationLock
                Me.Log("WebSite Session Maintenance: Started")
                Dim i As Integer = 0
                Dim info As CSessionInfo
                Dim cnt As Integer = 0
                While (i < Me.m_Info.Count)
                    info = Me.m_Info(i)
                    If (DateUtils.DateDiff(DateInterval.Minute, info.LastUpdated, DateUtils.Now) > Me.MaxSessionsTimeout) Then
                        Me.Log("WebSite Session Maintenance: Rimuovo la sessione " & info.SessionID)
                        Try
                            info.Dispose()
                        Catch ex As Exception
                            Me.Log("WebSite Session Maintenance: Errore nel terminare la sessione " & info.SessionID)
                            Me.Log(ex.StackTrace)
                        End Try

                        Me.m_Info.Remove(info)
                        cnt += 1
                    Else
                        i += 1
                    End If
                End While
                Me.Log("WebSite Session Maintenance: Completed (" & cnt & " sessions removed")
            End SyncLock

        End Sub

        Public Function GetUserSessions(ByVal user As CUser) As CCollection(Of CSessionInfo)
            SyncLock Me.applicationLock
                If (user Is Nothing) Then Throw New ArgumentNullException("user")
                Dim ret As New CCollection(Of CSessionInfo)
                For Each s As CSessionInfo In Me.m_Info
                    If (s.OriginalUser Is user) OrElse (s.OriginalUser Is Nothing AndAlso s.CurrentUser = user) Then
                        ret.Add(s)
                    End If
                Next
                Return ret
            End SyncLock
        End Function

        ''' <summary>
        ''' Metodo richiamato da una pagina per informare che la sessione corrente é ancora attiva
        ''' </summary>
        Public Sub UpdateCurrentSession()
            SyncLock Me.applicationLock
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                info.LastUpdated = Now
            End SyncLock
        End Sub

        ''' <summary>
        ''' Restituisce il descrittore di sessione corrispondente alla sessione corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCurrentSessionInfo() As CSessionInfo
            If ASP_Session Is Nothing Then Return Nothing
            Return Me.GetSessionInfo(ASP_Session.SessionID)
        End Function


        Public Overridable ReadOnly Property BaseName() As String
            Get
                Return Me.m_BaseName
            End Get
        End Property



        Public Overridable ReadOnly Property Description As String Implements IApplicationContext.Description
            Get
                Return "Applicazione Web" ' WebSite.Request.ServerVariables("HTTP_USER_AGENT")
            End Get
        End Property


        Public Overridable ReadOnly Property InstanceID As String Implements IApplicationContext.InstanceID
            Get
                If (WebSite.ASP_Session Is Nothing) Then Return ""
                Return WebSite.ASP_Session.SessionID
            End Get
        End Property

        Public Overridable ReadOnly Property RemoteIP As String Implements IApplicationContext.RemoteIP
            Get
                If (WebSite.ASP_Request Is Nothing) Then Return ""
                Return WebSite.ASP_Request.ServerVariables("REMOTE_ADDR")
            End Get
        End Property

        Public Overridable ReadOnly Property RemotePort As Integer Implements IApplicationContext.RemotePort
            Get
                If (WebSite.ASP_Request Is Nothing) Then Return 0
                Return WebSite.ASP_Request.ServerVariables("REMOTE_PORT")
            End Get
        End Property


        Public ReadOnly Property StartupFloder As String Implements IApplicationContext.StartupFloder
            Get
                If (Me.m_StartupFolder = "") Then
                    Return Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL)
                Else
                    Return Me.m_StartupFolder
                End If
            End Get
        End Property

        Public Sub SetStartupFloder(ByVal value As String)
            Me.m_StartupFolder = value
        End Sub

        Private m_TmporaryFolder As String = ""

        Public Overridable ReadOnly Property TmporaryFolder As String Implements IApplicationContext.TmporaryFolder
            Get
                If (Me.m_TmporaryFolder = "") Then
                    Return System.IO.Path.Combine(Me.StartupFloder, "Temp")
                Else
                    Return Me.m_TmporaryFolder
                End If
            End Get
        End Property

        Protected m_SystemDataFolder As String = ""

        Public Overridable ReadOnly Property SystemDataFolder As String Implements IApplicationContext.SystemDataFolder
            Get
                If (Me.m_SystemDataFolder = "") Then
                    Return System.IO.Path.Combine(Me.StartupFloder, "System\Data")
                Else
                    Return Me.m_SystemDataFolder
                End If
            End Get
        End Property

        Public Sub SetSystemDataFolder(ByVal value As String)
            Me.m_SystemDataFolder = value
        End Sub

        Protected m_UserDataFolder As String = ""

        Public Overridable ReadOnly Property UserDataFolder As String Implements IApplicationContext.UserDataFolder
            Get
                If (Me.m_UserDataFolder = "") Then
                    Return System.IO.Path.Combine(Me.StartupFloder, "Users\" & GetID(Me.CurrentUser) & "\Data")
                Else
                    Return Me.m_UserDataFolder
                End If
            End Get
        End Property

        Public Sub SetUserDataFolder(ByVal value As String)
            Me.m_UserDataFolder = value
        End Sub


        Public Overridable Function IsUserLogged(ByVal user As CUser) As Boolean Implements IApplicationContext.IsUserLogged
            Dim userID As Integer = GetID(user)
            If (userID = 0) Then Return False
            Select Case LCase(Trim(user.UserName))
                Case "system", "guest" : Return False
                Case Else
                    For i As Integer = 0 To Me.m_Info.Count - 1
                        Dim info As CSessionInfo = Me.m_Info(i)
                        If (GetID(info.CurrentUser) = userID) Then Return True
                    Next
                    Return False
            End Select
        End Function

        Public Property CurrentLogin As CLoginHistory Implements IApplicationContext.CurrentLogin
            Get
                If ASP_Session Is Nothing Then Return Nothing
                Return Me.CurrentLogin(ASP_Session.SessionID)
            End Get
            Set(value As CLoginHistory)
                If ASP_Session Is Nothing Then Exit Property
                Me.CurrentLogin(ASP_Session.SessionID) = value
            End Set
        End Property



        Public Overridable Property CurrentUser(ByVal sessionID As String) As CUser
            Get
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                If (info.CurrentUser Is Nothing) Then Return Sistema.Users.KnownUsers.GuestUser
                If info.ForceAbadon Then
                    info.ForceAbadon = False
                    ASP_Session.Abandon()
                    Return Sistema.Users.KnownUsers.GuestUser
                Else
                    Return info.CurrentUser
                End If

            End Get
            Set(value As CUser)
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                If (value Is Nothing) Then
                    info.CurrentUser = Sistema.Users.KnownUsers.GuestUser
                Else
                    info.CurrentUser = value
                End If
            End Set
        End Property

        Public Overridable Property CurrentLogin(ByVal sessionID As String) As CLoginHistory
            Get
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                Return info.CurrentLogin
            End Get
            Set(value As CLoginHistory)
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                info.CurrentLogin = value
            End Set
        End Property

        Public Property CurrentUfficio(ByVal sessionID As String) As CUfficio
            Get
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                Return info.CurrentUfficio
            End Get
            Set(value As CUfficio)
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                info.CurrentUfficio = value
            End Set
        End Property

        Public Overridable Property CurrentSession(ByVal sessionID As String) As CSiteSession
            Get
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                Return info.CurrentSession
            End Get
            Set(value As CSiteSession)
                Dim info As CSessionInfo = Me.GetSessionInfo(sessionID)
                info.CurrentSession = value
            End Set
        End Property

        Public Function GetSessionInfo(ByVal sessionID As String) As CSessionInfo
            SyncLock Me.applicationLock
                If (sessionID = vbNullString) Then Return Nothing
                Dim info As CSessionInfo = Me.m_Info.GetItemByKey(sessionID)
                'Dim info As CSessionInfo = Nothing  
                'For Each o As CSessionInfo In Me.m_Info
                '    If (o.SessionID = sessionID) Then
                '        info = o
                '        Exit For
                '    End If
                'Next

                If (info Is Nothing) Then
                    info = New CSessionInfo(sessionID)
                    info.ServerTime = DateUtils.Now
                    info.CurrentUser = Sistema.Users.KnownUsers.GuestUser
                    info.CurrentUfficio = Nothing
                    info.Descrizione = Me.IdentifyBrowser
                    info.CurrentSession = New minidom.WebSite.CCurrentSiteSession
                    Me.m_Info.Add(info.SessionID, info)
                    Me.m_Info.Add(sessionID, info)

                    info.RemoteIP = info.CurrentSession.RemoteIP
                    info.RemotePort = info.CurrentSession.RemotePort

                End If
                Return info
            End SyncLock
        End Function



        Public Overridable Property CurrentSession As Object Implements IApplicationContext.CurrentSession
            Get
                If ASP_Session Is Nothing Then Return Nothing
                Return Me.CurrentSession(ASP_Session.SessionID)
            End Get
            Set(value As Object)
                If ASP_Session Is Nothing Then Exit Property
                Me.CurrentSession(ASP_Session.SessionID) = value
            End Set
        End Property

        Public Overridable Property CurrentUser As Sistema.CUser Implements minidom.Sistema.IApplicationContext.CurrentUser
            Get
                If ASP_Session Is Nothing Then Return minidom.Sistema.Users.KnownUsers.GuestUser
                Return Me.CurrentUser(ASP_Session.SessionID)
            End Get
            Set(value As Sistema.CUser)
                If ASP_Session Is Nothing Then Exit Property
                Me.CurrentUser(ASP_Session.SessionID) = value
            End Set
        End Property

        Public Property CurrentUfficio As CUfficio Implements IApplicationContext.CurrentUfficio
            Get
                If ASP_Session Is Nothing Then Return Nothing
                Return Me.CurrentUfficio(ASP_Session.SessionID)
            End Get
            Set(value As CUfficio)
                If ASP_Session Is Nothing Then Exit Property
                Me.CurrentUfficio(ASP_Session.SessionID) = value
            End Set
        End Property


        Public MustOverride Function GetEntryAssembly() As Reflection.Assembly Implements IApplicationContext.GetEntryAssembly

#Region "Settings"

        Public Class CApplicationSettings
            Inherits CSettings

            Public Shadows Sub SetOwner(ByVal app As WebApplicationContext)
                MyBase.SetOwner(app)
            End Sub


        End Class


        Public Overridable ReadOnly Property Settings As CSettings Implements ISettingsOwner.Settings
            Get
                If (Me.m_Settings Is Nothing) Then
                    Dim _as As New CApplicationSettings()
                    _as.SetOwner(Me)
                    Dim fileName As String = Me.MapPath(Me.m_SettingsFileName)
                    If (System.IO.File.Exists(fileName)) Then
                        _as.LoadFromFile(fileName)
                    End If
                    Me.m_Settings = _as
                End If
                Return m_Settings
            End Get
        End Property

        Private Sub NotifySettingsChanged(ByVal e As CSettingsChangedEventArgs) Implements ISettingsOwner.NotifySettingsChanged
            Me.Settings.SaveToFile(Me.MapPath(Me.m_SettingsFileName))
        End Sub

#End Region



        Private Function HasKey(ByVal c As Specialized.NameValueCollection, ByVal key As String) As Integer
            key = Trim(key)
            For i As Integer = 0 To c.Keys.Count - 1
                Dim k As String = c.Keys(i)
                If Sistema.Strings.Compare(k, key, CompareMethod.Text) = 0 Then Return True
            Next
            Return False
        End Function

        Public Overridable Function GetParameter(paramName As String, Optional ByVal defValue As String = vbNullString) As String Implements IApplicationContext.GetParameter
            If HasKey(WebSite.ASP_Request.Form, paramName) Then
                Return WebSite.ASP_Request.Form(paramName)
            ElseIf HasKey(WebSite.ASP_Request.QueryString, paramName) Then
                Return WebSite.ASP_Request.QueryString(paramName)
            Else
                Return defValue
            End If
        End Function

        Public Overridable Function IsFormSet(ByVal key As String) As Boolean
            Dim keys() As String = WebSite.ASP_Request.Form.AllKeys
            For i As Integer = 0 To UBound(keys)
                If keys(i) = key Then Return True
            Next
            Return False
        End Function

        Public Overridable Function IsQueryStringSet(ByVal key As String) As Boolean
            Dim keys() As String = WebSite.ASP_Request.QueryString.AllKeys
            For i As Integer = 0 To UBound(keys)
                If keys(i) = key Then Return True
            Next
            Return False
        End Function

        Public Overridable Function GetParameter(Of T)(paramName As String, Optional defValue As Object = Nothing) As T Implements IApplicationContext.GetParameter
            Dim value As String
            If IsFormSet(paramName) Then
                value = WebSite.ASP_Request.Form(paramName)
            ElseIf IsQueryStringSet(paramName) Then
                value = WebSite.ASP_Request.QueryString(paramName)
            Else
                value = defValue
            End If
            Return Types.CastTo(value, Type.GetTypeCode(GetType(T)))
        End Function

        Public Overridable Function MapPath(path As String) As String Implements IApplicationContext.MapPath
            If (WebSite.ASP_Server IsNot Nothing) Then
                Return WebSite.ASP_Server.MapPath(path)
            Else
                Dim bp As String = Me.BasePath
                path = Trim(path)
                If (Right(bp, 1) <> "\") Then bp &= "\"
                If (Left(path, 1) = "/" Or Left(path, 1) = "~") Then
                    Return bp & Replace(Mid(path, 2), "/", "\")
                ElseIf (Left(path, 2) = "..") Then
                    Return bp & Replace(Mid(path, 3), "/", "\")
                End If
                Return path
            End If
        End Function

        Public Overridable Function UnMapPath(path As String) As String Implements IApplicationContext.UnMapPath
            Dim basePath As String = LCase(Sistema.FileSystem.NormalizePath(Me.MapPath("/")))
            If (LCase(Left(path, Len(basePath))) = basePath) Then
                path = "/" & Replace(Mid(path, Len(basePath) + 1), "\", "/")
                Return path
            Else
                Throw New ArgumentException("Il percorso non è invertibile")
            End If
        End Function

        Public Overridable Function IsDebug() As Boolean Implements IApplicationContext.IsDebug
            'Return minidom.Sistema.FileSystem.FileExists(Me.MapPath("/debug.dbg"))
#If DEBUG Then
            Return True
#Else
            return False
#End If
        End Function

        'Protected m_AziendaPrincipale As CAzienda = Nothing

        ''' <summary>
        ''' Restituisce o imposta l'azienda utilizzata come azienda principale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property IDAziendaPrincipale As Integer Implements IApplicationContext.IDAziendaPrincipale
            Get
                Return Anagrafica.Aziende.Module.Settings.GetValueInt("AziendaPrincipale", 0)
            End Get
            Set(value As Integer)
                Anagrafica.Aziende.Module.Settings.SetValueInt("AziendaPrincipale", value)
            End Set
        End Property

        Public MustOverride ReadOnly Property SupportEMail As String Implements IApplicationContext.SupportEMail


        Public MustOverride ReadOnly Property BasePath As String


        Public MustOverride ReadOnly Property BaseURL As String Implements IApplicationContext.BaseURL
        '    Get
        '        Dim tmp As String = Me.m_BaseURL
        '        If (tmp = "" AndAlso HttpContext.Current IsNot Nothing) Then tmp = HttpContext.Current.Request.Url.AbsoluteUri
        '        Dim i As Integer = InStr(tmp, "://")
        '        If (i <= 0) Then Return tmp
        '        i = InStr(i + 3, tmp, "/")
        '        If (i > 0) Then
        '            Return Left(tmp, i - 1)
        '        Else
        '            Return tmp
        '        End If
        '        Return Me.BasePath
        '    End Get
        'End Property

        Public MustOverride ReadOnly Property Title As String Implements IApplicationContext.Title


        Private Function GetCurrentURL() As String
            If (Me.IsDebug) Then
                Return "http://localhost:50528"
            Else
                Return WebSite.Instance.Configuration.SiteURL
            End If
        End Function

        Protected Overridable Sub InitializeLoggingSystem()
            Dim logFile As String = Me.GetLogFileName()
            Dim logfolder As String = Sistema.FileSystem.GetFolderName(logFile)
            Sistema.FileSystem.CreateRecursiveFolder(logfolder)
            'Sistema.FileSystem.CreateRecursiveFolder(Sistema.FileSystem.GetFolderName(Me.GetLogFileName))

        End Sub

        ''' <summary>
        ''' Inizializza le connessioni al db
        ''' </summary>
        Protected Overridable Sub InitializeConnections()

        End Sub

        Protected Overridable Sub InitializeModules()
            Anagrafica.Intialize()
            Anagrafica.Persone.Initialize()
            'For Each m As CModule In Sistema.Modules.LoadAll()
            '    m.InitializeFrom()

            'Next
        End Sub

        Public Sub Start() Implements IApplicationContext.Start
            'Inizializza il sistema dei log
            Me.InitializeLoggingSystem()

            'Inizializza gli handlers
            Me.RegisterHandlers()
            'Inizializza i tipi
            Me.RegisterTypes()
            Me.RegisterTypeProviders()

            'Avvia le connessioni
            Me.InitializeConnections()

            'Avvia i moduli
            Me.InitializeModules()

            'Avvio interno
            Me.InternalStart()

            If (LOGConn.IsOpen) Then
                LOGConn.ExecuteCommand("UPDATE [tbl_LoginHistory] SET [LogoutMethod]=" & LogOutMethods.LOGOUT_REMOTEDISCONNECT & ", [LogOutTime]=" & DBUtils.DBDate(Now) & " WHERE [LogoutMethod]=" & LogOutMethods.LOGOUT_UNKNOWN)
            End If
            If (APPConn.IsOpen) Then
                'Sistema.FileSystem.DeleteFile (Sistema.FileSystem . "*.*

                Sistema.Backups.Initialize()

                minidom.Sistema.Module.DispatchEvent(New EventDescription("APP_Start", "Applicazione avviata", ""))
            End If
            RPC.BaseURL = Me.GetCurrentURL ' 

            'Eliminiamo i log 
            If WebSite.Instance.Configuration.DeleteLogFilesAfterNDays > 0 Then
                Dim logPath As String = Sistema.FileSystem.GetFolderName(Me.GetLogFileName)
                minidom.Sistema.FileSystem.CreateRecursiveFolder(logPath)
                Dim dinfo As New System.IO.DirectoryInfo(logPath)
                Dim files As System.IO.FileInfo() = dinfo.GetFiles("*.log")
                For i As Integer = 0 To Arrays.Len(files) - 1
                    If DateUtils.DateDiff(DateInterval.Day, files(i).CreationTime, Today) > WebSite.Instance.Configuration.DeleteLogFilesAfterNDays Then
                        Try
                            files(i).Delete()
                        Catch ex As Exception
                            Me.Log("Errore nell'eliminazione del file di log: " & files(i).FullName & " : " & ex.Message)
                        End Try
                    End If
                Next
            End If

            Sistema.Procedure.StartBackgroundWorker()
        End Sub

        Protected Overridable Sub RegisterHandlers()
            'AddHandler DBUtils.CursorOpened, AddressOf handleCursorOpen
            'AddHandler DBUtils.CursorClosed, AddressOf handleCursorClosed
            'AddHandler DBUtils.ConnectionOpened, AddressOf handleConnectionOpen
            'AddHandler DBUtils.ConnectionClosed, AddressOf handleConnectionClosed
        End Sub

        ''' <summary>
        ''' Inizializza il sottosistema di gestione dei tipi
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub RegisterTypes()
            Sistema.Types.Imports.Add("minidom")
            Sistema.Types.Imports.Add("minidom.Sistema")
            Sistema.Types.Imports.Add("minidom.FileSystem")
            Sistema.Types.Imports.Add("minidom.Databases")
            Sistema.Types.Imports.Add("minidom.Calendar")
            Sistema.Types.Imports.Add("minidom.Collegamenti")
            Sistema.Types.Imports.Add("minidom.Comunicazioni")
            Sistema.Types.Imports.Add("minidom.Appuntamenti")
            Sistema.Types.Imports.Add("minidom.GDE")
            Sistema.Types.Imports.Add("Anagrafica.Luoghi")
            Sistema.Types.Imports.Add("minidom.Anagrafica")
            Sistema.Types.Imports.Add("minidom.CustomerCalls")
            Sistema.Types.Imports.Add("minidom.Finanziaria")
            Sistema.Types.Imports.Add("minidom.Forms")
            Sistema.Types.Imports.Add("minidom.Messenger")
            Sistema.Types.Imports.Add("minidom.Web")
            Sistema.Types.Imports.Add("minidom.WebSite")
            Sistema.Types.Imports.Add("minidom.Visite")
            Sistema.Types.Imports.Add("minidom.ADV")
            Sistema.Types.Imports.Add("minidom.Anagrafica+Fonti")
            Sistema.Types.Imports.Add("minidom.Office")
            Sistema.Types.Imports.Add("minidom.Mail")
            Sistema.Types.Imports.Add("minidom.Tickets")
            Sistema.Types.Imports.Add("minidom.GPS")
            Sistema.Types.Imports.Add("minidom.Drivers")
            Sistema.Types.Imports.Add("minidom.Internals")
        End Sub


        Protected Overridable Sub InternalStart()

            ServicePointManager.ServerCertificateValidationCallback = AddressOf handleremotecertificatevalidationcallback


        End Sub

        'public delegate function remotecertificatevalidationcallback(sender as object, certificate as system.security.cryptography.x509certificates.x509certificate, chain as system.security.cryptography.x509certificates.x509chain, sslpolicyerrors as system.net.security.sslpolicyerrors) as boolean
        Private Function handleremotecertificatevalidationcallback(sender As Object, certificate As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslpolicyerrors As System.Net.Security.SslPolicyErrors) As Boolean
            If (sslpolicyerrors = System.Net.Security.SslPolicyErrors.None) Then
                Return True
            Else
                Dim e As New MailCertificateEventArgs(certificate, chain, sslpolicyerrors)
                e.Allow = False
                Me.OnCertificateValidationFail(sender, e)
                Return e.Allow
            End If
        End Function

        Protected Overridable Sub OnCertificateValidationFail(ByVal sender As Object, ByVal e As MailCertificateEventArgs)
            RaiseEvent CertificateValidationFail(sender, e)
        End Sub


        Public Sub [Stop]() Implements IApplicationContext.Stop
            Sistema.Procedure.StopBackgroundWorker()

            If (LOGConn.IsOpen) Then
                LOGConn.ExecuteCommand("UPDATE [tbl_LoginHistory] SET [LogoutMethod]=" & LogOutMethods.LOGOUT_REMOTEDISCONNECT & ", [LogOutTime]=" & DBUtils.DBDate(Now) & " WHERE [LogoutMethod]=" & LogOutMethods.LOGOUT_UNKNOWN)
            End If
            If (APPConn.IsOpen) Then
                minidom.Sistema.Module.DispatchEvent(New EventDescription("APP_End", "Applicazione terminata", ""))
            End If

            Me.InternalStop()

            Me.UnregisterTypeProviders()
            Me.UnregisterTypes()

            Me.UnregisterHandlers()
        End Sub

        Protected Overridable Sub UnregisterTypes()
            Sistema.Types.Imports.Remove("minidom")
            Sistema.Types.Imports.Remove("minidom.Sistema")
            Sistema.Types.Imports.Remove("minidom.FileSystem")
            Sistema.Types.Imports.Remove("minidom.Databases")
            Sistema.Types.Imports.Remove("minidom.Calendar")
            Sistema.Types.Imports.Remove("minidom.Collegamenti")
            Sistema.Types.Imports.Remove("minidom.Comunicazioni")
            Sistema.Types.Imports.Remove("minidom.Appuntamenti")
            Sistema.Types.Imports.Remove("minidom.GDE")
            Sistema.Types.Imports.Remove("Anagrafica.Luoghi")
            Sistema.Types.Imports.Remove("minidom.Anagrafica")
            Sistema.Types.Imports.Remove("minidom.CustomerCalls")
            Sistema.Types.Imports.Remove("minidom.Finanziaria")
            Sistema.Types.Imports.Remove("minidom.Forms")
            Sistema.Types.Imports.Remove("minidom.Messenger")
            Sistema.Types.Imports.Remove("minidom.Web")
            Sistema.Types.Imports.Remove("minidom.WebSite")
            Sistema.Types.Imports.Remove("minidom.Visite")
            Sistema.Types.Imports.Remove("minidom.ADV")
            Sistema.Types.Imports.Remove("minidom.Anagrafica+Fonti")
            Sistema.Types.Imports.Remove("minidom.Office")
            Sistema.Types.Imports.Remove("minidom.Mail")
            Sistema.Types.Imports.Remove("minidom.Tickets")
            Sistema.Types.Imports.Remove("minidom.GPS")
            Sistema.Types.Imports.Remove("minidom.Drivers")
        End Sub

        Protected Overridable Sub UnregisterHandlers()
            'RemoveHandler DBUtils.CursorOpened, AddressOf handleCursorOpen
            'RemoveHandler DBUtils.CursorClosed, AddressOf handleCursorClosed
            'RemoveHandler DBUtils.ConnectionOpened, AddressOf handleConnectionOpen
            'RemoveHandler DBUtils.ConnectionClosed, AddressOf handleConnectionClosed
        End Sub

        Protected Overridable Sub InternalStop()
            Me.ResettaSessioni()
            Me.ResettaConnessioni()
        End Sub

        'Private Sub handleCursorOpen(ByVal sender As Object, ByVal e As DBCursorEventArgs)
        '    SyncLock Me.GlobalOpenedCursors
        '        Me.GlobalOpenedCursors.Add(e.Cursor)
        '    End SyncLock
        'End Sub

        'Private Sub handleCursorClosed(ByVal sender As Object, ByVal e As DBCursorEventArgs)
        '    SyncLock Me.GlobalOpenedCursors
        '        Me.GlobalOpenedCursors.Remove(e.Cursor)
        '    End SyncLock
        'End Sub

        'Private Sub handleConnectionOpen(ByVal sender As Object, ByVal e As DBEventArgs)
        '    SyncLock Me.applicationLock
        '        Me.OpenedConnections.Add(e.Connection)
        '    End SyncLock
        'End Sub

        'Private Sub handleConnectionClosed(ByVal sender As Object, ByVal e As DBEventArgs)
        '    SyncLock Me.applicationLock
        '        Me.OpenedConnections.Remove(e.Connection)
        '    End SyncLock
        'End Sub

        'Public Sub ResettaCursori()
        '    SyncLock Me.applicationLock
        '        While Me.GlobalOpenedCursors.Count > 0
        '            Me.GlobalOpenedCursors(0).Dispose()
        '        End While
        '    End SyncLock
        'End Sub

        Public Sub ResettaConnessioni()
            Dim col As CCollection(Of CDBConnection) = DBUtils.GetAllOpenedConnections
            For Each con As CDBConnection In col
                Try
                    con.CloseDB()
                Catch ex As Exception

                End Try
            Next
            Threading.Thread.Sleep(1000)
        End Sub

        Public Sub ResettaSessioni()
            Dim col As CCollection(Of CSessionInfo) = Me.GetAllSessions
            For Each info As CSessionInfo In col
                info.Reset()
            Next
        End Sub


        Public Overridable Function GetProperty(name As String) As Object Implements IApplicationContext.GetProperty
            Dim session As CSiteSession = Me.CurrentSession

            Select Case LCase(Trim(name))
                Case "remoteip"
                    If (session Is Nothing) Then Return ""
                    Return session.RemoteIP
                Case "remoteport"
                    If (session Is Nothing) Then Return ""
                    Return session.RemotePort
                Case "sessionid"
                    If (session Is Nothing) Then Return ""
                    Return session.SessionID
                Case "useragent"
                    If (session Is Nothing) Then Return ""
                    Return session.UserAgent
                Case Else
                    Return Nothing
            End Select
        End Function


        ''' <summary>
        ''' Mette l'applicazione in stato manutenzione
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EnterMaintenance() Implements IApplicationContext.EnterMaintenance
            If (Me.m_Maintenance) Then Exit Sub
            Me.m_Maintenance = True
            Me.Log(Formats.FormatUserDateTime(DateUtils.Now) & " - " & CurrentUser.Nominativo & " - Enter Maintenance Mode")
            Sistema.Module.DispatchEvent(New EventDescription("enter_maintenance", "Entro in modalità manutenzione", Me))
            Me.EnterMaintenanceInternal()
        End Sub


        ''' <summary>
        ''' Metodo richiamato internamente da EnterMaintenance.
        ''' Questa funzione chiude tute le connessioni e le memorizza in una variabile interna per poterle poi riaprire all'uscita dall
        ''' modalità manutenzione
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub EnterMaintenanceInternal()
            SyncLock Me.applicationLock
                Me.m_mainenanceDBCollection = DBUtils.GetAllOpenedConnections
                oldCheckMails = Sistema.EMailer.AutoSynchronize
                Sistema.EMailer.AutoSynchronize = False
                Me.ResettaSessioni()
                Me.ResettaConnessioni()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Restituisce vero se l'applicazione è in stato manutenzione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsMaintenance() As Boolean Implements IApplicationContext.IsMaintenance
            Return Me.m_Maintenance
        End Function

        ''' <summary>
        ''' Esce dallo stato manutenzione
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub QuitMaintenance() Implements IApplicationContext.QuitMaintenance
            If (Me.m_Maintenance = False) Then Exit Sub
            Me.QuitMaintenanceInternal()
            System.Threading.Thread.Sleep(2000)
            Me.Log(Formats.FormatUserDateTime(DateUtils.Now) & " - " & CurrentUser.Nominativo & " - Quit Maintenance Mode")
            Sistema.Module.DispatchEvent(New EventDescription("quit_maintenance", "Esco dalla modalità manutenzione", Me))
            Me.m_Maintenance = False
        End Sub

        Protected Overridable Sub QuitMaintenanceInternal()
            SyncLock Me.applicationLock
                For Each db As CDBConnection In Me.m_mainenanceDBCollection
                    db.OpenDB()
                Next
                Sistema.EMailer.AutoSynchronize = oldCheckMails
                Me.m_mainenanceDBCollection = Nothing
            End SyncLock
        End Sub

        Public Sub OpenCursor(ByVal cursor As DBObjectCursorBase)
            SyncLock Me.CurrentSession
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                cursor.Token = ASPSecurity.GetRandomKey(25)
                While info.RemoteOpenedCursors.ContainsKey(cursor.Token)
                    cursor.Token = ASPSecurity.GetRandomKey(25)
                End While
                info.RemoteOpenedCursors.Add(cursor.Token, cursor)
            End SyncLock
            If (cursor.CursorStatus <> DBCursorStatus.NORMAL) Then cursor.Open()
        End Sub

        Public Function GetCursor(ByVal token As String) As DBObjectCursorBase
            SyncLock Me.CurrentSession
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                Return info.RemoteOpenedCursors.GetItemByKey(token)
            End SyncLock
        End Function

        Public Sub DestroyCursor(ByVal token As String)
            SyncLock Me.CurrentSession
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                Dim cursor As DBObjectCursorBase = info.RemoteOpenedCursors.GetItemByKey(token) ' DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
                If (cursor IsNot Nothing) Then
                    'If (TypeName(cursor) <> tn) Then Throw New ArgumentException("Il token non identifica il cursore specificato")
                    cursor.Dispose()
                    info.RemoteOpenedCursors.RemoveByKey(token)
                End If
            End SyncLock
        End Sub

        Public Sub ResetCursor(ByVal token As String)
            Dim cursor As DBObjectCursorBase
            SyncLock Me.CurrentSession
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                cursor = info.RemoteOpenedCursors(token) ' DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
            End SyncLock
            cursor.Reset1()
        End Sub

        Public Function RestoreCursor(ByVal token As String) As DBObjectCursorBase
            SyncLock Me.CurrentSession
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                Return info.RemoteOpenedCursors.GetItemByKey(token) ' DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
            End SyncLock
        End Function

        Public Function StartSession(ByVal session As HttpSessionState) As CCurrentSiteSession
            SyncLock Me.applicationLock
                Me.Log("Sessione Web Iniziata: " & session.SessionID)

                Dim info As CSessionInfo = Me.m_Info.GetItemByKey(session.SessionID)
                'Dim info As CSessionInfo = Nothing
                'For Each o As CSessionInfo In Me.m_Info
                '    If o.SessionID = session.SessionID Then
                '        info = o
                '        Exit For
                '    End If
                'Next

                If (info IsNot Nothing AndAlso info.Descrizione <> Me.IdentifyBrowser) Then
                    Dim str As String = Formats.GetTimeStamp & " - " & info.SessionID & " - Riavvio della sessione" & vbNewLine
                    str &= "Browser: " & info.Descrizione & " - " & Me.IdentifyBrowser & vbNewLine
                    str &= "CurrentUser: " & info.CurrentUserName & " - " & vbNewLine
                    If (info.CurrentUfficio IsNot Nothing) Then str &= "Ufficio: " & info.CurrentUfficio.Nome & vbNewLine
                    str &= "Remote IP: " & info.RemoteIP & " - " & WebSite.ASP_Request.ServerVariables("REMOTE_ADDR") & vbNewLine
                    Me.Log(str)
                    Throw New InvalidOperationException("La sessione Web " & session.SessionID & " già attiva")
                End If
                info = New CSessionInfo(session.SessionID)
                info.Descrizione = Me.IdentifyBrowser
                info.ServerTime = DateUtils.Now
                info.CurrentUser = Sistema.Users.KnownUsers.GuestUser
                info.CurrentUfficio = Nothing
                info.CurrentSession = New minidom.WebSite.CCurrentSiteSession
                Me.m_Info.Add(info.SessionID, info)
                Dim parameters As New CKeyCollection
                parameters.SetItemByKey("SessionID", session.SessionID)
                parameters.SetItemByKey("RemoteIP", WebSite.ASP_Request.ServerVariables("REMOTE_ADDR"))
                parameters.SetItemByKey("RemotePort", WebSite.ASP_Request.ServerVariables("REMOTE_PORT"))
                parameters.SetItemByKey("RemoteUserAgent", WebSite.ASP_Request.ServerVariables("HTTP_USER_AGENT"))
                parameters.SetItemByKey("RemoteLocalIP", WebSite.GetRemoteMachineIP())
                Dim initialReferrer As String = CStr(ASP_Session("InitialReferrer"))
                If (initialReferrer = "") Then
                    initialReferrer = WebSite.ASP_Request.ServerVariables("HTTP_REFERER")
                    ASP_Session("InitialReferrer") = initialReferrer
                End If
                parameters.SetItemByKey("InitialReferrer", initialReferrer)
                Dim siteCookie As String = Cookies.GetCookie("SiteCookie")
                If (siteCookie = "") Then
                    siteCookie = ASPSecurity.GetRandomKey(64)
                    Cookies.SetCookie("SiteCookie", siteCookie)
                End If
                parameters.SetItemByKey("SiteCookie", siteCookie)
                DirectCast(info.CurrentSession, minidom.WebSite.CCurrentSiteSession).NotifyStart(parameters)
                info.RemoteIP = info.CurrentSession.RemoteIP
                info.RemotePort = info.CurrentSession.RemotePort

                Return info.CurrentSession
            End SyncLock
        End Function

        Private Function IdentifyBrowser() As String
            If ASP_Request.Browser Is Nothing Then Return ""
            Dim bc As HttpBrowserCapabilities = ASP_Request.Browser

            Return bc.Platform & ", " & bc.Browser & " " & bc.Version

            '            HttpBrowserCapabilities bc = Request.Browser;
            'Response.Write("<p>Browser Capabilities:</p>");
            'Response.Write("Type = " + bc.Type + "<br>");
            'Response.Write("Name = " + bc.Browser + "<br>");
            'Response.Write("Version = " + bc.Version + "<br>");
            'Response.Write("Major Version = " + bc.MajorVersion + "<br>");
            'Response.Write("Minor Version = " + bc.MinorVersion + "<br>");
            'Response.Write("Platform = " + bc.Platform + "<br>");
            'Response.Write("Is Beta = " + bc.Beta + "<br>");
            'Response.Write("Is Crawler = " + bc.Crawler + "<br>");
            'Response.Write("Is AOL = " + bc.AOL + "<br>");
            'Response.Write("Is Win16 = " + bc.Win16 + "<br>");
            'Response.Write("Is Win32 = " + bc.Win32 + "<br>");
            'Response.Write("Supports Frames = " + bc.Frames + "<br>");
            'Response.Write("Supports Tables = " + bc.Tables + "<br>");
            'Response.Write("Supports Cookies = " + bc.Cookies + "<br>");
            'Response.Write("Supports VB Script = " + bc.VBScript + "<br>");
            'Response.Write("Supports JavaScript = " + bc.JavaScript + "<br>");
            'Response.Write("Supports Java Applets = " + bc.JavaApplets + "<br>");
            'Response.Write("Supports ActiveX Controls = " + bc.ActiveXControls + "<br>");
            'Response.Write("CDF = " + bc.CDF + "<br>");
        End Function

        Public Sub DisposeSession(ByVal session As HttpSessionState)
            SyncLock Me.applicationLock
                Me.Log("Sessione Web Terminata: " & session.SessionID)

                If (session Is Nothing) Then Throw New ArgumentNullException("session")
                Dim key As String = session.SessionID

                Dim info As CSessionInfo = Me.m_Info.GetItemByKey(key)
                'Dim info As CSessionInfo = Nothing
                'For Each o As CSessionInfo In Me.m_Info
                '    If (o.SessionID = session.SessionID) Then
                '        info = o
                '        Exit For
                '    End If
                'Next

                If (info Is Nothing) Then Return ' Throw New InvalidOperationException("La sessione Web " & session.SessionID & " non è attiva")

                Dim cu As minidom.Sistema.CUser = info.CurrentUser
                Dim uName As String = "Guest"

                If (cu IsNot Nothing AndAlso cu.IsLogged) Then
                    cu.LogOut(minidom.Sistema.LogOutMethods.LOGOUT_TIMEOUT)
                End If

                If (info.CurrentSession IsNot Nothing) Then
                    DirectCast(info.CurrentSession, minidom.WebSite.CCurrentSiteSession).NotifyEnd()
                    info.Dispose()
                End If


                Me.m_Info.Remove(info)

            End SyncLock



        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Sub Personifica(ByVal newUser As CUser)
            SyncLock Me.applicationLock
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                If info Is Nothing Then Throw New ArgumentNullException("currentsessioninfo")
                If (newUser Is Nothing) Then Throw New ArgumentNullException("newuser")
                Me.Log("Impersonificazione -> Inizio da " & info.CurrentUser.Nominativo & " a " & newUser.Nominativo)
                info.OriginalUser = info.CurrentUser
                info.CurrentUser = newUser

            End SyncLock

        End Sub

        Public Sub EsciPersonifica(ByVal newUser As CUser)
            SyncLock Me.applicationLock
                Dim info As CSessionInfo = Me.GetCurrentSessionInfo
                If info Is Nothing Then Throw New ArgumentNullException("currentsessioninfo")
                If (newUser IsNot info.CurrentUser) Then Throw New ArgumentNullException("newuser")
                If (info.OriginalUser Is Nothing) Then Throw New ArgumentNullException("originaluser")
                Me.Log("Impersonificazione -> Fine da " & info.CurrentUser.Nominativo & " a " & info.OriginalUser.Nominativo)
                info.CurrentUser = info.OriginalUser
                info.OriginalUser = Nothing

            End SyncLock

        End Sub

        Private Shared Function ConvertToBytes(ByVal val As SecureString) As Byte()
            If (val Is Nothing) Then Throw New ArgumentNullException("val")
            'Dim unmanagedString As IntPtr = IntPtr.Zero
            'Try
            '    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(val)
            '    Return System.Text.Encoding.ASCII.GetBytes(Marshal.PtrToStringUni(unmanagedString))
            'Finally
            '    'Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString)
            'End Try
            Dim c As New NetworkCredential("", val)
            Return System.Text.Encoding.ASCII.GetBytes(c.Password)
        End Function

        ''' <summary>
        ''' Apre il database specificato e se non lo trova 
        ''' </summary>
        ''' <param name="url"></param>
        ''' <returns></returns>
        Public Function OpenIfExistsDB(ByVal url As String) As CDBConnection
            url = Sistema.Strings.Trim(url)
            If (url.StartsWith("/") AndAlso url.Length > 1) Then url = url.Substring(1)

            Dim path As String = Me.DBURL
            If (Not path.EndsWith("/")) Then path = path & "/"

            path = path & url

            path = System.Web.HttpContext.Current.Server.MapPath(path)
            If (System.IO.File.Exists(path)) Then
                Me.Log("Apro il database [" & url & "] -> " & path)
                Dim ret As CMdbDBConnection
                Try
                    ret = New CMdbDBConnection(path)
                    Dim s As SecureString = __ENCKEY()

                    Dim enc As Byte() = ConvertToBytes(s)
                    ret.EncryptionKey = enc
                    ret.OpenDB()
                Catch ex As Exception
                    Me.Log("Can't open  database" & url & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
                    Me.sendCriticalEmail(ex)
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw ex
                End Try
                Return ret
            Else
                Me.Log("Il database [" & url & "] -> " & path & " non esiste")
                Return APPConn
            End If
        End Function

        <SecuritySafeCritical>
        <DllImport("dll\DMDCoreC.dll", EntryPoint:="#4", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Private Shared Function ___IOGEN(ByVal buff As IntPtr, ByVal len As Integer) As Integer

        End Function

        <SecuritySafeCritical>
        <DllImport("dll\DMDCoreC.dll", EntryPoint:="#4", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Private Shared Function ___IOGEN(ByVal buff As System.Text.StringBuilder, ByVal len As Integer) As Integer

        End Function

        <SecuritySafeCritical>
        Private Function __ENCKEY() As SecureString

            Dim s As New SecureString()
            s.AppendChar("W"c)
            s.AppendChar("P"c)
            s.AppendChar("A"c)
            s.AppendChar("Z"c)
            s.AppendChar("W"c)
            s.AppendChar("O"c)
            s.AppendChar("Q"c)
            s.AppendChar("A"c)
            s.AppendChar("G"c)
            s.AppendChar("E"c)
            s.AppendChar("M"c)
            s.AppendChar("M"c)
            s.AppendChar("E"c)
            s.AppendChar("Z"c)
            s.AppendChar("O"c)
            s.AppendChar("W"c)

            Return s

            'Dim buffLen As Integer = ___IOGEN(IntPtr.Zero, 0)
            'Dim buff As New System.Text.StringBuilder(buffLen)
            'Dim len As Integer = ___IOGEN(buff, buffLen)
            'Dim ret As New SecureString()
            'For i As Integer = 0 To len - 1
            '    ret.AppendChar(buff(i))
            'Next
            'Return ret
        End Function

        Protected Overridable Sub sendCriticalEmail(ByVal ex As System.Exception)
            Dim subject As String = "ERRORE CRITICO IN " & Me.BaseName
            Dim body As String = ""
            Dim FROMADDRESS As String = Me.SupportEMail
            Dim TOADDRESS As String = "domenico.dimieri@hotmail.it"
            Dim CCNADDRESS As String = ""

            body &= "ERRORE CRITICO IN <b>" & Me.BaseName & "</b><br/><br/>"
            body &= "<b>" & ex.Message & "</b><br/><br/>"
            body &= ex.StackTrace

            Try
                minidom.Sistema.EMailer.SendEMail1(FROMADDRESS, TOADDRESS, "", CCNADDRESS, minidom.Sistema.Strings.TrimTo(subject, 78), body)
            Catch ex1 As System.Exception

            End Try
        End Sub
        Public Delegate Sub LogFunction(ByVal message As String)

        Public Overridable Sub CompactAllDB(ByVal logfun As LogFunction)
            SyncLock (Me.applicationLock)
                Dim col As CCollection = DBUtils.GetAllOpenedConnections()

                Dim Engine As Object = ASP_Server.CreateObject("JRO.JetEngine")


                For Each db As CDBConnection In col
                    logfun("Chiudo il database<br/>")
                    Try
                        db.CloseDB()
                    Catch ex As Exception
                        logfun("<span style=""color:red;"">Errore nella chiusura del db " & ex.Message & "</span><br/>")
                        Continue For
                    End Try

                    Try
                        Dim tmpPath As String = System.IO.Path.GetTempFileName() ' Me.MapPath("/public/temp/minidom/tmpDB.tmp")
                        Dim dbPath As String = db.Path
                        '
                        logfun("Inizio la compattazione di """ & dbPath & """ in """ & tmpPath & """<br/>")

                        'DBUtils.CompactDB(dbConn.Path)
                        logfun("Pulisco """ & tmpPath & """<br/>")
                        Sistema.FileSystem.DeleteFile(tmpPath, True)
                        'My.Computer.Network.DownloadFile(ApplicationContext.BaseURL & "admin/compact.asp?source=" & dbPath & "&target=" & tmp, FileSystem.GetTempFileName, "", "", False, 60 * 10 * 1000, True)
                        'DBUtils.CompactDB(dbConn.Path)
                        logfun("Compatto """ & dbPath & """ in """ & tmpPath & """<br/>")

                        Dim password As String = System.Text.Encoding.ASCII.GetString(db.EncryptionKey())
                        '          )
                        Engine.CompactDatabase(
                                   "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbPath & ";Jet OLEDB:Database Password=""" & password & """ ",
                                   "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & tmpPath & ";Jet OLEDB: Engine Type=5;Jet OLEDB:Database Password=""" & password & """ "
                                   )

                        logfun("Elimino di """ & dbPath & """<br/>")

                        Sistema.FileSystem.DeleteFile(dbPath, True)

                        'Me.Log("Copio """ & tmpPath & """ in """ & dbPath & """<br/>")
                        Sistema.FileSystem.CopyFile(tmpPath, dbPath)
                        Sistema.FileSystem.DeleteFile(tmpPath, True)

                        logfun("Compattazione terminata<br>")
                    Catch ex As Exception
                        logfun("<span style=""color:red;"">Errore nella compattazione del db " & ex.Message & "</span><br/>")
                    End Try

                    logfun("Riapro il database<br/>")
                    db.OpenDB()

                    logfun("Rilasco l'applicazione<br/>")
                Next

                Engine = Nothing
            End SyncLock
        End Sub

        Public Overridable Sub CompactDB(ByVal db As CDBConnection, ByVal logfun As LogFunction)
            SyncLock (Me.applicationLock)
                Dim Engine As Object = ASP_Server.CreateObject("JRO.JetEngine")


                logfun("Chiudo il database<br/>")
                Try
                    db.CloseDB()
                Catch ex As Exception
                    logfun("<span style=""color:red;"">Errore nella chiusura del db " & ex.Message & "</span><br/>")
                    Exit Sub
                End Try

                Try
                    Dim tmpPath As String = System.IO.Path.GetTempFileName() ' Me.MapPath("/public/temp/minidom/tmpDB.tmp")
                    Dim dbPath As String = db.Path
                    '
                    logfun("Inizio la compattazione di """ & dbPath & """ in """ & tmpPath & """<br/>")

                    'DBUtils.CompactDB(dbConn.Path)
                    logfun("Pulisco """ & tmpPath & """<br/>")
                    Sistema.FileSystem.DeleteFile(tmpPath, True)
                    'My.Computer.Network.DownloadFile(ApplicationContext.BaseURL & "admin/compact.asp?source=" & dbPath & "&target=" & tmp, FileSystem.GetTempFileName, "", "", False, 60 * 10 * 1000, True)
                    'DBUtils.CompactDB(dbConn.Path)
                    logfun("Compatto """ & dbPath & """ in """ & tmpPath & """<br/>")

                    Dim password As String = System.Text.Encoding.ASCII.GetString(db.EncryptionKey())
                    '          )
                    Engine.CompactDatabase(
                                   "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbPath & ";Jet OLEDB:Database Password=""" & password & """ ",
                                   "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & tmpPath & ";Jet OLEDB: Engine Type=5;Jet OLEDB:Database Password=""" & password & """ "
                                   )

                    logfun("Elimino di """ & dbPath & """<br/>")

                    Sistema.FileSystem.DeleteFile(dbPath, True)

                    'Me.Log("Copio """ & tmpPath & """ in """ & dbPath & """<br/>")
                    Sistema.FileSystem.CopyFile(tmpPath, dbPath)
                    Sistema.FileSystem.DeleteFile(tmpPath, True)

                    logfun("Compattazione terminata<br>")
                Catch ex As Exception
                    logfun("<span style=""color:red;"">Errore nella compattazione del db " & ex.Message & "</span><br/>")
                End Try

                logfun("Riapro il database<br/>")
                db.OpenDB()

                logfun("Rilasco l'applicazione<br/>")

                Engine = Nothing
            End SyncLock

        End Sub
    End Class

End Class
