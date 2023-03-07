Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

Partial Public Class Finanziaria

    Public Class CProfiloXGroupAllowNegateCursor
        Inherits GroupAllowNegateCursor(Of CProfilo)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Preventivatore"
        End Function

        Protected Overrides Function GetGroupFieldName() As String
            Return "Gruppo"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PreventivatoriXGroup"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProfiloXGroupAllowNegate
        End Function
    End Class

End Class

