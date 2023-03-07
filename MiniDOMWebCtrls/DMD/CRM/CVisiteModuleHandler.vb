Imports minidom
Imports minidom.Sistema
Imports minidom.WebSite
Imports minidom.CustomerCalls
Imports minidom.Forms
Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Forms.Utils

Imports minidom.Office


Namespace Forms

 

    Public Class CVisiteModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CVisiteCursor
        End Function



    End Class


End Namespace