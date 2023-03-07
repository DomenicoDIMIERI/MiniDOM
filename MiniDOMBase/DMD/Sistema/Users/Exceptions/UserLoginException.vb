Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Sistema


Public Class UserLoginException
    Inherits System.Exception

    Private m_UserName As String

    Public Sub New()
        MyBase.New("Errore generico di accesso")
        DMDObject.IncreaseCounter(Me)
    End Sub

    Public Sub New(ByVal userName As String)
        Me.New(userName, "Errore generico di accesso")
    End Sub

    Public Sub New(ByVal userName As String, ByVal message As String)
        MyBase.New(message)
        Me.m_UserName = userName
        DMDObject.IncreaseCounter(Me)
    End Sub

    Public ReadOnly Property UserName As String
        Get
            Return Me.m_UserName
        End Get
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMDObject.DecreaseCounter(Me)
    End Sub
End Class
