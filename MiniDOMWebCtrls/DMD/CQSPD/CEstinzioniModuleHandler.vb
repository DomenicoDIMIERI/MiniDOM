Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.XML

Namespace Forms

    Public Class CEstinzioniModuleHandler
        Inherits CBaseModuleHandler





        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CEstinzioniCursor
        End Function


    End Class


End Namespace