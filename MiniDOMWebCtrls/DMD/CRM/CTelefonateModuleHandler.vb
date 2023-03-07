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

 

    Public Class CTelefonateModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTelefonateCursor
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CustomerCalls.CRM.GetItemById(id)
        End Function

    End Class


End Namespace