Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils
Imports minidom.Office



Namespace Forms


    Public Class RilevatoriPresenzeHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New RilevatoriPresenzeCursor
        End Function


    End Class


End Namespace