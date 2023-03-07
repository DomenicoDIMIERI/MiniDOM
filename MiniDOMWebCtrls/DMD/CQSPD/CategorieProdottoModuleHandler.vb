Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms



    Public Class CategorieProdottoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CCategorieProdottoCursor
            Return cursor
        End Function


    End Class




End Namespace