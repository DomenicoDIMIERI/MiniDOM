Imports minidom
Imports minidom.Sistema
Imports minidom.WebSite
Imports minidom.Forms
Imports minidom.Databases

Namespace Forms

  
  
    Public Class CCollegamentiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCollegamentiCursor
        End Function


    End Class


End Namespace