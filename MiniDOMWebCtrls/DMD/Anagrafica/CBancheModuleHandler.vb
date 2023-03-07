Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils


Imports minidom.Web

Namespace Forms

    Public Class CBancheModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CBancheCursor
        End Function


    End Class

End Namespace