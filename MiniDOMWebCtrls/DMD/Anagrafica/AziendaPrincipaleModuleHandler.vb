Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.XML

Namespace Forms

    Public Class AziendaPrincipaleModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function


    End Class


End Namespace