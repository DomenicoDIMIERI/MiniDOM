Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria


    Public Class CAreaManagerCursor
        Inherits CTeamManagersCursor

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Finanziaria.AreaManagers.Module 'modAreaManager
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_AreaManagers"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAreaManager
        End Function

    End Class

End Class
