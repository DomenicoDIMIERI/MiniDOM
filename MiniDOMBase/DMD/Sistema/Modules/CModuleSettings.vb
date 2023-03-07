Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    <Serializable> _
    Public Class CModuleSettings
        Inherits CSettings

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As CModule)
            MyBase.New(owner)
        End Sub

        Public Overloads Sub Update()
            Me.Initialize(Me.Owner)
        End Sub

    End Class

End Class