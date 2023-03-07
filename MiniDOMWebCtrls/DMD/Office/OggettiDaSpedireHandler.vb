Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.Office

Namespace Forms


    Public Class OggettiDaSpedireHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New OggettiDaSpedireCursor
        End Function



    End Class

End Namespace