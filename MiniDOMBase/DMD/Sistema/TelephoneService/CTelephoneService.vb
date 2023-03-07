Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Sistema
Imports minidom.Internals

Namespace Internals


    Public NotInheritable Class CTelephoneServiceClass
        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione di questo oggetto
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConfigurationChanged(ByVal e As System.EventArgs)

        ''' <summary>
        ''' Evento generato quando un Telephone inviato cambia stato
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event TelephoneStatusChanged(ByVal sender As Object, ByVal e As TelephoneStatusEventArgs)


        ''' <summary>
        ''' Evento generato quando un Telephone inviato cambia stato
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event TelephoneDeliver(ByVal sender As Object, ByVal e As TelephoneDeliverEventArgs)

        ''' <summary>
        ''' Evento generato quando viene ricevuto un Telephone
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event TelephoneReceived(ByVal sender As Object, ByVal e As TelephoneReceivedEventArgs)

        ''' <summary>
        ''' Evento generato per segnalare un evento generico del driver
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event TelephoneEvent(ByVal sender As Object, ByVal e As TelephoneEventArgs)

        Private m_InstalledDrivers As New CKeyCollection(Of BasicTelephoneDriver)
        Private m_DefualtDriver As BasicTelephoneDriver
        Private m_Config As CTelephoneConfig


        Friend Sub New()
            DMDObject.IncreaseCounter(Me)
            Dim nd As New minidom.Drivers.NullTelephoneDriver
            Me.m_InstalledDrivers.Add(nd.GetUniqueID, nd)
        End Sub


        Public ReadOnly Property Config As CTelephoneConfig
            Get
                If m_Config Is Nothing Then
                    m_Config = New CTelephoneConfig
                    m_Config.Load()
                End If
                Return m_Config
            End Get
        End Property

        Protected Friend Sub SetConfig(ByVal value As CTelephoneConfig)
            Me.m_Config = value
            Me.m_DefualtDriver = Nothing
            Me.doConfigChanged(New System.EventArgs)
        End Sub


        Friend Sub doConfigChanged(ByVal e As System.EventArgs)
            RaiseEvent ConfigurationChanged(e)
        End Sub

        Public Function GetInstalledDrivers() As CKeyCollection(Of BasicTelephoneDriver)
            Return New CKeyCollection(Of BasicTelephoneDriver)(Me.m_InstalledDrivers)
        End Function


        Public Sub InstallDriver(ByVal driver As BasicTelephoneDriver)
            If (m_InstalledDrivers.ContainsKey(driver.GetUniqueID)) Then
                Throw New ArgumentException("Il driver " & driver.GetUniqueID & " è già installato")
            End If
            m_InstalledDrivers.Add(driver.GetUniqueID, driver)
        End Sub

        Public Sub RemoveDriver(ByVal driver As BasicTelephoneDriver)
            m_InstalledDrivers.RemoveByKey(driver.GetUniqueID)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il driver predefinito utilizzato per l'invio degli Telephone
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefaultDriver As BasicTelephoneDriver
            Get
                ''If (m_DefualtDriver Is Nothing) Then
                ''    Dim dName As String = Trim([Module].Settings.GetValueString("DefaultTelephoneDriverName", ""))
                ''    If (dName <> "") Then m_DefualtDriver = m_InstalledDrivers.GetItemByKey(dName)
                ''End If
                'If (m_DefualtDriver Is Nothing AndAlso m_InstalledDrivers.Count > 0) Then
                '    m_DefualtDriver = m_InstalledDrivers(0)
                'End If
                'Return m_DefualtDriver
                If (m_DefualtDriver Is Nothing AndAlso m_InstalledDrivers.Count > 0) Then
                    m_DefualtDriver = Me.m_InstalledDrivers.GetItemByKey(Me.Config.DefaultDriverName)
                    If (Me.m_DefualtDriver Is Nothing) Then Me.m_DefualtDriver = m_InstalledDrivers(0)
                End If
                Return Me.m_DefualtDriver
            End Get
            'Set(value As BasicTelephoneDriver)
            '    'If (value IsNot Nothing) Then
            '    '    [Module].Settings.SetValueString("DefaultTelephoneDriverName", value.GetUniqueID)
            '    'Else
            '    '    [Module].Settings.SetValueString("DefaultTelephoneDriverName", "")
            '    'End If
            '    m_DefualtDriver = value
            'End Set
        End Property

        ''' <summary>
        ''' Invia un Telephone utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal targetNumber As String, ByVal message As String) As String
            Return Send(DefaultDriver, targetNumber, message, DefaultDriver.GetDefaultOptions)
        End Function

        ''' <summary>
        ''' Invia un Telephone utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal targetNumber As String, ByVal message As String, ByVal options As TelephoneDriverOptions) As String
            Return Send(DefaultDriver, targetNumber, message, options)
        End Function

        ''' <summary>
        ''' Invia un Telephone utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal driver As BasicTelephoneDriver, ByVal targetNumber As String, ByVal message As String) As String
            If driver Is Nothing Then Throw New ArgumentNullException("driver")
            Return Send(driver, targetNumber, message, driver.GetDefaultOptions)
        End Function

        ''' <summary>
        ''' Invia un Telephone utilizzando il driver predefinito
        ''' </summary>
        ''' <param name="targetNumber"></param>
        ''' <param name="message"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal driver As BasicTelephoneDriver, ByVal targetNumber As String, ByVal message As String, ByVal options As TelephoneDriverOptions) As String
            If driver Is Nothing Then Throw New ArgumentNullException("driver")
            If options Is Nothing Then Throw New ArgumentNullException("options")
            Return driver.Dial(targetNumber, message, options)
        End Function

        Function GetDriver(ByVal driverId As String) As BasicTelephoneDriver
            SyncLock Me
                Return Me.m_InstalledDrivers.GetItemByKey(driverId)
            End SyncLock
        End Function



        Public Function GetStatus(ByVal messageID As String) As MessageStatus
            Return GetStatus(DefaultDriver, messageID)
        End Function

        Public Function GetStatus(ByVal driver As BasicTelephoneDriver, ByVal messageID As String) As MessageStatus
            If (driver Is Nothing) Then Throw New ArgumentNullException("driver")
            Return driver.GetStatus(messageID)
        End Function

        Public Function IsValidNumber(ByVal value As String) As Boolean
            Return IsValidNumber(DefaultDriver, value)
        End Function

        Public Function IsValidNumber(ByVal driver As BasicTelephoneDriver, ByVal value As String) As Boolean
            If (driver Is Nothing) Then Throw New ArgumentNullException("driver")
            Return driver.IsValidNumber(value)
        End Function

        Friend Sub doIncomingCall(ByVal e As TelephoneReceivedEventArgs)
            RaiseEvent TelephoneReceived(Nothing, e)
        End Sub

        Friend Sub doTelephoneStatusChanged(ByVal e As TelephoneStatusEventArgs)
            RaiseEvent TelephoneStatusChanged(Nothing, e)
        End Sub


        Friend Sub doTelephoneEvent(ByVal e As TelephoneEventArgs)
            RaiseEvent TelephoneEvent(Nothing, e)
        End Sub

        Friend Sub doNotifyCall(ByVal e As TelephoneDeliverEventArgs)
            RaiseEvent TelephoneDeliver(Nothing, e)
        End Sub

        Public Function GetDriverConfiguration(drvName As String) As TelephoneDriverOptions
            Dim driver As BasicTelephoneDriver = Me.GetDriver(drvName)
            Return driver.GetDefaultOptions
        End Function

        Public Sub SetDriverConfiguration(drvName As String, config As TelephoneDriverOptions)
            Dim driver As BasicTelephoneDriver = Me.GetDriver(drvName)
            driver.SetDefaultOptions(config)
        End Sub

        Protected Friend Sub UpdateDriver(ByVal drv As BasicTelephoneDriver)
            SyncLock Me
                Dim i As Integer = Me.m_InstalledDrivers.IndexOfKey(drv.GetUniqueID)
                If (i >= 0) Then
                    Dim isConnected As Boolean = Me.m_InstalledDrivers(i).IsConnected
                    If (isConnected) Then Me.m_InstalledDrivers(i).Disconnect()
                    Me.m_InstalledDrivers(i) = drv
                    If (isConnected) Then Me.m_InstalledDrivers(i).Connect()
                Else
                    Me.m_InstalledDrivers.Add(drv.GetUniqueID, drv)
                    drv.Connect()
                End If
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Inizializza le tabelle
        ''' </summary>
        Public Sub Initialize()
            Dim db As CDBConnection = APPConn
            If (Not db.TableExists("tbl_TelephoneConfig")) Then
                Dim dbSQL As String
                dbSQL = "CREATE TABLE [tbl_TelephoneConfig]" &
                       "(" &
                       "[ID] Counter Primary Key," &
                       "[DefaultDriverName] Text(255)," &
                       "[DefaultSenderName] Text(255)," &
                       "[DefaultSenderNumber] Text(255)," &
                       "[Flags] Int," &
                       "[CreatoDa] Int," &
                       "[CreatoIl] Date," &
                       "[ModificatoDa] Int," &
                       "[ModificatoIl] Date," &
                       "[Stato] Int" &
                       ")"
                DBUtils.CreateTable(db, dbSQL)
                DBUtils.CreateIndex(db, "idxDefaultDriverName", "tbl_TelephoneConfig", {"DefaultDriverName"})
            End If
        End Sub

    End Class

End Namespace

Partial Public Class Sistema


    Private Shared m_TelephoneService As CTelephoneServiceClass = Nothing

    Public Shared ReadOnly Property TelephoneService As CTelephoneServiceClass
        Get
            If (m_TelephoneService Is Nothing) Then
                m_TelephoneService = New CTelephoneServiceClass()
                m_TelephoneService.Initialize()
            End If
            Return m_TelephoneService
        End Get
    End Property


End Class