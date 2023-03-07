Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils



Namespace Forms

   
 
    Public Class CAnnotazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SExport Or ModuleSupportFlags.SImport)
            
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAnnotazioniCursor
        End Function

    End Class
 
    
End Namespace