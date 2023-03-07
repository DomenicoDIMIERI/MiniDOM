Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Sistema


Public Class UserEventArgs
    Inherits System.EventArgs

    Private m_User As CUser

    Public Sub New()
        DMDObject.IncreaseCounter(Me)
    End Sub

    Public Sub New(ByVal user As CUser)
        Me.New()
        Me.m_User = user
    End Sub

    Public ReadOnly Property User As CUser
        Get
            Return Me.m_User
        End Get
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMDObject.DecreaseCounter(Me)
    End Sub
End Class
