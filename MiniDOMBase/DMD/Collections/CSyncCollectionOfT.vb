Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports System.Net
Imports minidom.Sistema

<Serializable>
Public Class CSyncCollection(Of T)
    Inherits CSyncCollection

    Public Sub New()
    End Sub

    Default Public Shadows Property Item(ByVal index As Integer) As T
        Get
            Return MyBase.Item(index)
        End Get
        Set(value As T)
            MyBase.Item(index) = value
        End Set
    End Property

End Class
