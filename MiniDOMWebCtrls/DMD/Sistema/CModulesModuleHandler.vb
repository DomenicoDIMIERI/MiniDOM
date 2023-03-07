Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils


Imports minidom.Web
Imports minidom.XML

Namespace Forms

    '-------------------------------------------------------
    Public Class CModulesModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CModulesCursor
        End Function

    End Class


End Namespace