Imports minidom
Imports minidom.Sistema


Public Class UserExpiredException
    Inherits UserLoginException

    Private m_UserStatus As UserStatus

    Public Sub New()
        Me.New(vbNullString, "L'account utente � scaduto")
    End Sub

    Public Sub New(ByVal userName As String)
        Me.New(userName, "[" & userName & "] L'account utente � scaduto")
    End Sub

    Public Sub New(ByVal userName As String, ByVal message As String)
        MyBase.New(userName, message)
    End Sub

    Public Sub New(ByVal userName As String, ByVal stato As UserStatus)
        MyBase.New(userName, "[" & userName & "] L'account utente � scaduto: " & [Enum].GetName(GetType(UserStatus), stato))
        Me.m_UserStatus = stato
    End Sub

    Public ReadOnly Property UserStatus As UserStatus
        Get
            Return Me.m_UserStatus
        End Get
    End Property

End Class

