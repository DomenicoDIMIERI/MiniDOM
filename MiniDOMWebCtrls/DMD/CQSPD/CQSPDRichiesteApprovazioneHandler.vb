Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils

Namespace Forms

    Public Class CQSPDRichiesteApprovazioneHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New Finanziaria.CRichiestaApprovazioneCursor
        End Function


    End Class

    


End Namespace