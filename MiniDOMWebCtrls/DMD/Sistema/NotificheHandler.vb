Imports minidom
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Forms.Utils



Namespace Forms


    '--------------------------------------------------------
    Public Class NotificheHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub


        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New NotificaCursor
        End Function

    End Class

    


End Namespace