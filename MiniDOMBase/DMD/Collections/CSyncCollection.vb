Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports System.Net
Imports minidom.Sistema

<Serializable>
Public Class CSyncCollection
    Inherits CCollection

    <NonSerialized> Private ReadOnly lockObject As New Object

    Public Sub New()
    End Sub

    Public Overrides ReadOnly Property IsSynchronized As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property SyncRoot As Object
        Get
            Return Me.lockObject
        End Get
    End Property
 
End Class
