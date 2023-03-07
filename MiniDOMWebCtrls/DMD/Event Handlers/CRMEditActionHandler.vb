Imports Microsoft.VisualBasic
Imports minidom.Anagrafica
Imports minidom.CustomerCalls
Imports minidom.Sistema

Namespace Forms

    Public Class CRMEditActionHandler
        Inherits minidom.Sistema.CBaseEventHandler


        Public Overrides Sub NotifyEvent(ByVal e As EventDescription)
            Dim toAddress As String = CustomerCalls.CRM.Module.Settings.GetValueString("NotifyChangesTo", "")
            If (toAddress <> "") Then
                Dim item As CContattoUtente = e.Descrittore
                If (item.Persona.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                    Select Case item.Scopo
                        Case "Richiesta Conteggio Estintivo"
                            Dim text As String = "Il cliente " & item.NomePersona & " (CF: " & Formats.FormatCodiceFiscale(item.Persona.CodiceFiscale) & ") ha richiesto un conteggio estintivo all'operatore " & item.NomeOperatore & " dell'ufficio di " & item.NomePuntoOperativo
                            Dim m As System.Net.Mail.MailMessage = minidom.Sistema.EMailer.PrepareMessage("robot@minidom.net", toAddress, "", "", text, e.Descrizione, "", False)
                            EMailer.SendMessageAsync(m, True)
                    End Select
                End If
            End If
        End Sub

    End Class

End Namespace