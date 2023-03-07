Imports minidom
Imports minidom.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CMdbDBConnection
        Inherits COleDBConnection

        Public Sub New()
        End Sub

        Public Sub New(ByVal fileName As String)
            Me.Path = fileName
        End Sub

        Public Sub New(ByVal fileName As String, ByVal userName As String, ByVal password As String)
            Me.Path = fileName
            Me.SetCredentials(userName, password)
        End Sub

    End Class

End Class


