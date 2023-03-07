Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Office
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Web

Namespace Forms

    Public Class LuoghiVisitatiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New LuoghiVisitatiCursor
        End Function

        Public Function GetLuoghiVisitati(ByVal renderer As Object) As String
            Dim lid As Integer = RPC.n2int(GetParameter(renderer, "lid", "0"))
            Dim uscita As Uscita = Office.Uscite.GetItemById(lid)
            Return XML.Utils.Serializer.Serialize(uscita.LuoghiVisitati)
        End Function



    End Class


End Namespace