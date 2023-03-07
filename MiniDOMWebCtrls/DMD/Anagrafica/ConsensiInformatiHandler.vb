Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.WebSite
Imports minidom.Anagrafica
Imports minidom.Forms.Utils




Namespace Forms

 
    Public Class ConsensiInformatiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ConsensoInformatoCursor
        End Function



    End Class


End Namespace