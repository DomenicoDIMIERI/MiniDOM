Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms

    Public Class ImportFromCLHandler
        Inherits CBaseModuleHandler


        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SAnnotations)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCQSPDOfferteCursor
        End Function


    End Class


End Namespace