Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.Office

Namespace Forms


    Public Class SpedizioniHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New SpedizioniCursor
        End Function


    End Class

End Namespace