Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema


    Public Class CUserSettings
        Inherits CSettings

        Public Sub New()
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New()
            Me.Initialize(user)
        End Sub

    End Class

End Class
