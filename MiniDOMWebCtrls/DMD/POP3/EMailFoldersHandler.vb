Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.Office
Imports minidom.WebSite
Imports minidom.Anagrafica

Imports minidom.Databases
Imports minidom.Net

Namespace Forms



    Public Class EMailFoldersHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New minidom.Office.MailFolderCursor
        End Function


    End Class



End Namespace