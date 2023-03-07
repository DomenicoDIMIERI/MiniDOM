Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace Forms
 
 
   

    Public Class AltriPrestitiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAltriPrestitiCursor
        End Function

        'Public Overrides Function GetEditor() As Object
        '    Return New AltriPrestitiEditor
        'End Function
          
    End Class


End Namespace