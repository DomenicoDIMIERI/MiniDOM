Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils


Imports minidom.Anagrafica

Namespace Forms

 
    Public Class CSysEventsModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New Sistema.CEventsCursor
        End Function


    End Class
     
End Namespace