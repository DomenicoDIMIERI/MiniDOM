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

 

    Public Class TemplatesModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()

        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CustomerCalls.CTemplatesCursor
        End Function

         
        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CustomerCalls.Templates.GetItemById(id)
        End Function

        'Public Overrides Function GetItemByName() As String
        '    Return CustomerCalls.Templates.GetItemById(id)
        'End Function
         
    End Class


End Namespace