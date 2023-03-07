Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.XML

Namespace Forms



    Public Class CQSPDConsulentiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CConsulentiPraticaCursor
        End Function



        Public Function GetItemByUser(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim item As CConsulentePratica = Finanziaria.Consulenti.GetItemByUser(id)
            If (item Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function CreateElencoConsulenti(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Dim ov As Boolean = RPC.n2bool(GetParameter(renderer, "ov", "T"))
            Return Utils.CQSPDUtils.CreateElencoConsulenti(id, ov)
        End Function

    End Class


End Namespace