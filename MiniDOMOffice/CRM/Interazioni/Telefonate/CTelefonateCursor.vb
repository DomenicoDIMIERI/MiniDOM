Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

Imports minidom.Anagrafica

Partial Public Class CustomerCalls


    Public Class CTelefonateCursor
        Inherits CCustomerCallsCursor

        Public Sub New()
            MyBase.ClassName.Value = "CTelefonata"
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTelefonata
        End Function

        Public Shadows Property Item As CTelefonata
            Get
                Return MyBase.Item
            End Get
            Set(value As CTelefonata)
                MyBase.Item = value
            End Set
        End Property
    End Class



End Class