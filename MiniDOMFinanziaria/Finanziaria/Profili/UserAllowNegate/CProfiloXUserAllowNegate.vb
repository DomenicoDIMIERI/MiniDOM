Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

Partial Public Class Finanziaria

    <Serializable> _
    Public Class CProfiloXUserAllowNegate
        Inherits UserAllowNegate(Of CProfilo)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Preventivatore"
        End Function

        Protected Overrides Function GetUserFieldName() As String
            Return "Utente"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PreventivatoriXUser"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Shadows Sub SetItem(ByVal item As CProfilo)
            MyBase.SetItem(item)
        End Sub

    End Class

End Class

