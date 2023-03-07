Imports minidom
Imports minidom.Sistema
Imports minidom.WebSite

Imports minidom.Forms
Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Forms.Utils

Namespace Forms
 

    Public Class CProvinceModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CProvinceCursor
            ret.Nome.SortOrder = SortEnum.SORT_ASC
            Return ret
        End Function

    End Class

   

  
End Namespace