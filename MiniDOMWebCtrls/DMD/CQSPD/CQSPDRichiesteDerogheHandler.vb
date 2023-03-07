Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils

Namespace Forms

    Public Class CQSPDRichiesteDerogheHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Finanziaria.RichiesteDeroghe.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New Finanziaria.CRichiestaDerogaCursor
        End Function

        Public Function Invia(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("inviare") Then Throw New PermissionDeniedException(Me.Module, "inviare")
            Dim ric As CRichiestaDeroga = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            ric.Invia()
            Return XML.Utils.Serializer.Serialize(ric)
        End Function

        Public Function Ricevi(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("ricevere") Then Throw New PermissionDeniedException(Me.Module, "ricevere")
            Dim ric As CRichiestaDeroga = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            ric.Ricevi()
            Return XML.Utils.Serializer.Serialize(ric)
        End Function


    End Class




End Namespace