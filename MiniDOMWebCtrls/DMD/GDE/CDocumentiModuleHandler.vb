Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.Office
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.XML

Namespace Forms

    Public Class CGDEModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CDocumentiCursor
        End Function



    End Class





End Namespace