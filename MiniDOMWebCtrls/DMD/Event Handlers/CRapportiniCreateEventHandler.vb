Imports Microsoft.VisualBasic
Imports minidom.Sistema

Namespace Forms

    Public Class CRapportiniCreateEventHandler
        Inherits Sistema.CBaseEventHandler

        Public Overrides Sub NotifyEvent(ByVal e As EventDescription)
            Dim msg As String = Users.CurrentUser.UserName & " (" & Users.CurrentUser.Nominativo & ") - " & e.Descrizione
            Dim m As System.Net.Mail.MailMessage = minidom.Sistema.EMailer.PrepareMessage(minidom.Finanziaria.Configuration.FromAddress, minidom.Finanziaria.Configuration.NotifyChangesTo, "", "", msg, msg, "", True)
            minidom.Sistema.EMailer.SendMessageAsync(m, True)
        End Sub
    End Class

End Namespace