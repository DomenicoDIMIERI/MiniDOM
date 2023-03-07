
Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Office
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Web
Imports minidom.XML

Namespace Forms

    Public Class RichiesteECHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New EstrattiContributiviCursor
        End Function


    End Class


End Namespace
