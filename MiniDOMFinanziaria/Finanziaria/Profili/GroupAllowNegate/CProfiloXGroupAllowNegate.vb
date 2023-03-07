Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria

    Public Class CProfiloXGroupAllowNegate
        Inherits GroupAllowNegate(Of CProfilo)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Preventivatore"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PreventivatoriXGroup"
        End Function

        Protected Overrides Function GetGroupFieldName() As String
            Return "Gruppo"
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
