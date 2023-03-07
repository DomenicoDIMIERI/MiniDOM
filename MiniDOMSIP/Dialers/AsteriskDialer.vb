Imports System.IO
Imports minidom.CallManagers.Actions
Imports minidom.CallManagers.Events

Public Class AsteriskDialer
    Inherits DialerBaseClass

    'Private m_Thread As System.Threading.Thread
    Private m_Server As AsteriskServer

    Public Sub New()
        'Me.m_Thread = Nothing
    End Sub

    Public Sub New(ByVal server As AsteriskServer)
        Me.New
        Me.m_Server = server
    End Sub

    Function PrepareNumber(ByVal number As String) As String
        Return Trim(number)
    End Function

    Public Overrides Sub Dial(number As String)

        If (Len(number) <= 1) Then Return

        Me.HangUp()

        'Me.m_Thread = New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf thread))
        'Me.m_Thread.Start(number)
        'End Sub

        'Private Sub thread(ByVal o As Object)
        '    Dim Number As String = CStr(o)

        Dim e As New DialEventArgs(Number)
        Me.OnBegidDial(e)

        Dim a As New Originate

        'If Not Me.m_Server.IsConnected Then Me.m_Server.Connect()
        'Me.m_Server.Disconnect()
        'System.Threading.Thread.Sleep(100)
        'Me.m_Server.Connect()
        'System.Threading.Thread.Sleep(100)
        If Not Me.IsInstalled Then Return

        'Me.m_Server.Connect()
        a.CallerID = "A: " & Number 'Me.m_Server.CallerID
        a.Context = "from-internal"
        a.Channel = Me.m_Server.Channel
        a.Exten = number
        a.Priority = 1
        Dim r As minidom.CallManagers.Responses.OriginateResponse
        Try
            r = DirectCast(Me.m_Server.GetManager.Execute(a, 1000), minidom.CallManagers.Responses.OriginateResponse)
        Catch ex As Exception
            minidom.Sistema.ApplicationContext.Log("AsteriskDialerException - RETRY 1 - " & ex.Message & vbNewLine & ex.StackTrace)

            Try
                If (Me.m_Server.IsConnected) Then Me.m_Server.Disconnect()
                System.Threading.Thread.Sleep(500)
                Me.m_Server.Connect()
                System.Threading.Thread.Sleep(500)
                r = DirectCast(Me.m_Server.GetManager.Execute(a, 1000), minidom.CallManagers.Responses.OriginateResponse)
                'If r.IsSuccess Then
                '    Return
                'Else
                '    Return
                'End If
            Catch ex1 As Exception
                minidom.Sistema.ApplicationContext.Log("AsteriskDialerException - FAIL - " & ex1.Message & vbNewLine & ex1.StackTrace)
            End Try

        End Try



        Me.OnEndDial(e)
    End Sub




    Public Overrides Function IsInstalled() As Boolean
        Return (Me.m_Server IsNot Nothing) AndAlso (Me.m_Server.GetManager IsNot Nothing) AndAlso (Me.m_Server.GetManager.IsConnected)
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Asterisk: " & Me.m_Server.Channel
        End Get
    End Property

    Public Overrides Sub HangUp()
        'If (Me.m_Thread IsNot Nothing) Then
        '    Me.m_Thread.Abort()
        '    Me.m_Thread = Nothing
        'End If
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is C3CXDialer) Then Return False
        Return MyBase.Equals(obj) AndAlso DirectCast(obj, AsteriskDialer).m_Server.Equals(Me.m_Server)
    End Function

End Class
