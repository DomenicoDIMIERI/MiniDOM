Imports minidom
Imports minidom.WebSite
Imports minidom.Sistema
Imports minidom.Forms
Imports minidom.Messenger

Namespace Forms

    Public Class ChatStats
        Implements XML.IDMDXMLSerializable

        Public OnlineUsers As CCollection(Of CChatUser)
        Public Messages As CCollection(Of CMessage)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "OnlineUsers" : Me.OnlineUsers.AddRange(fieldValue)
                Case "Messages" : Me.Messages.AddRange(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("OnlineUsers", Me.OnlineUsers.ToArray)
            writer.WriteTag("Messages", Me.Messages.ToArray)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Namespace