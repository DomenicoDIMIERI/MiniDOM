Imports minidom
Imports minidom.Sistema
Imports minidom.WebSite

Imports minidom.Forms
Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Forms.Utils

Namespace Forms
 
  
    Public Class CIndirizziModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CIndirizziCursor
        End Function




    End Class

 
End Namespace