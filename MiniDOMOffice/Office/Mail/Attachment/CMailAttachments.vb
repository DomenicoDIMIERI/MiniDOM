Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Office
Imports minidom.Internals


Namespace Internals

    Public Class CMailAttachments
        Inherits CModulesClass(Of MailAttachment)

        Public Sub New()
            MyBase.New("modOfficeEMailsAtt", GetType(MailAttachmentCursor), 0)
        End Sub

    End Class

End Namespace

