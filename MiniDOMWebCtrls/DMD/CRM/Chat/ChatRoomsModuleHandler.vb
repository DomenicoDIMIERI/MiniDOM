Imports minidom
Imports minidom.WebSite
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.Messenger
Imports minidom.XML

Namespace Forms
    Public Class ChatRoomsModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New CChatRoomCursor
        End Function
    End Class


End Namespace
