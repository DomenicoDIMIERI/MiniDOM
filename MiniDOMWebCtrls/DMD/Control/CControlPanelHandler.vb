Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils

Namespace Forms


    Public Class CControlPanelHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub


        Public Function Load(ByVal renderer As Object) As String
            Dim ret As New ControlPanelInfo
            Dim fromDate As Date? = RPC.n2date(GetParameter(renderer, "fd", ""))
            If (fromDate.HasValue = False) Then fromDate = DateUtils.ToDay
            ret.Load(fromDate.Value)
            Dim tmp As String = XML.Utils.Serializer.Serialize(ret)
            Return tmp
        End Function

        Public Function Update(ByVal renderer As Object) As String
            Dim ret As New ControlPanelInfo
            Dim fromDate As Date? = RPC.n2date(GetParameter(renderer, "fd", ""))
            If (fromDate.HasValue = False) Then fromDate = DateUtils.ToDay
            ret.Update(fromDate.Value)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class

End Namespace