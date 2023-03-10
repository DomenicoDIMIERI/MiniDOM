Imports System.IO

Public Class XLiteDialer
    Inherits DialerBaseClass

    Private m_Path As String

    Public Sub New()
        Me.m_Path = Get3CXPath()
    End Sub

    Public Overrides Sub Dial(number As String)
        If Not Me.IsInstalled Then Return
        Shell(Me.m_Path & " -call?to=" & number)
    End Sub

    Public Overrides Function IsInstalled() As Boolean
        Return (Me.m_Path <> vbNullString)
    End Function

    Private Shared Function Get3CXPath() As String
        Dim p As String = Path.Combine(GetProgramFilesFolder, "CounterPath\X-Lite\X-Lite.exe")
        If (File.Exists(p)) Then Return p
        p = Path.Combine(GetProgramFilesFolder() & " (x86)", "CounterPath\X-Lite\X-Lite.exe")
        If (File.Exists(p)) Then Return p
        Return vbNullString
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "CounterPath X-Lite"
        End Get
    End Property

    Public Overrides Sub HangUp()

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is C3CXDialer) Then Return False
        Return MyBase.Equals(obj) AndAlso DirectCast(obj, XLiteDialer).m_Path = Me.m_Path
    End Function

End Class
