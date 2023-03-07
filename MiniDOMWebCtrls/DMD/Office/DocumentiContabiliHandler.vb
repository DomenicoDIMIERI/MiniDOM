Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.Office

Namespace Forms


    Public Class DocumentiContabiliHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.DocumentiContabili.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DocumentoContabileCursor
        End Function

    End Class

End Namespace