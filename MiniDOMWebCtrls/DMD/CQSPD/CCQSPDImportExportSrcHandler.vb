Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.CustomerCalls

Namespace Forms


    Public Class CCQSPDImportExportSrcHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SExport)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CImportExportSourceCursor()
        End Function



    End Class

End Namespace