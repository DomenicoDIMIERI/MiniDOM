Imports minidom
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.WebSite

Imports minidom.Databases

Imports minidom.ADV
Imports minidom.Web

Namespace Forms

    Public Class ADVResModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRisultatoCampagnaCursor
        End Function




    End Class


End Namespace