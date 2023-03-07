Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils

Namespace Forms

    Public Class CQSPDRichiesteConteggiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New Finanziaria.CRichiestaConteggioCursor
        End Function

        Public Function Segnala(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ric As CRichiestaConteggio = Me.GetInternalItemById(id)
            If Not Me.Module.UserCanDoAction("segnalare") Then Throw New PermissionDeniedException(Me.Module, "segnalare")
            ric.Segnala()
            Return ""
        End Function

        Public Function PrendiInCarico(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim ric As CRichiestaConteggio = Me.GetInternalItemById(id)
            If Not Me.Module.UserCanDoAction("prendereincarico") Then Throw New PermissionDeniedException(Me.Module, "prendereincarico")
            ric.PrendiInCarico()
            Return ""
        End Function


    End Class

    


End Namespace