Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.CustomerCalls
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    ''' <summary>
    ''' Aggiunge le elaborazioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoElaborazioni
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Me.AggiungiEInternal(items, filter)
            Me.AggiungiIInternal(items, filter)
        End Sub

        Private Sub AggiungiEInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor As New CImportExportCursor


#If Not DEBUG Then
            Try
#End If
            cursor.Esportazione.Value = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If (filter.Dal.HasValue) Then
                If (filter.Al.HasValue) Then
                    cursor.DataEsportazione.Between(filter.Dal, filter.Al)
                Else
                    cursor.DataEsportazione.Value = filter.Dal
                    cursor.DataEsportazione.Operator = OP.OP_GE
                End If
            ElseIf (filter.Al.HasValue) Then
                cursor.DataEsportazione.Value = filter.Dal
                cursor.DataEsportazione.Operator = OP.OP_LE
            End If

            If filter.IDPersona <> 0 Then
                cursor.IDPersonaEsportata.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor.NomePersonaEsportata.Value = filter.Nominativo & "%"
                cursor.NomePersonaEsportata.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor.IDEsportataDa.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                'cursor1..Value = filter.Scopo & "%"
                'cursor1.Motivo.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor.StatoRemoto.Value = StatoEsportazione.NonEsportato
                    Case 1 'in corso
                        cursor.StatoRemoto.Value = StatoEsportazione.Esportato
                        cursor.StatoConferma.Value = StatoConfermaEsportazione.Inviato
                    Case Else
                        cursor.StatoConferma.ValueIn({StatoConfermaEsportazione.Confermato, StatoConfermaEsportazione.Revocato, StatoConfermaEsportazione.Rifiutata})
                End Select
            End If
            If filter.Numero <> "" Then
                'cursor.sou.Value = filter.Numero & "%"
                'cursor.MezzoDiInvio.Operator = OP.OP_LIKE
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor1.id.Value = filter.TipoContesto
                'cursor1.ContextID.Value = filter.IDContesto
            End If
            cursor.DataEsportazione.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights

            While Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                Me.AddEActivities(items, cursor.Item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            Catch Ex As Exception
                Throw
            Finally
#End If
            cursor.Dispose()
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Private Sub AggiungiIInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor As New CImportExportCursor


#If Not DEBUG Then
            Try
#End If
            cursor.Esportazione.Value = False
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If (filter.Dal.HasValue) Then
                If (filter.Al.HasValue) Then
                    cursor.DataEsportazione.Between(filter.Dal, filter.Al)
                Else
                    cursor.DataEsportazione.Value = filter.Dal
                    cursor.DataEsportazione.Operator = OP.OP_GE
                End If
            ElseIf (filter.Al.HasValue) Then
                cursor.DataEsportazione.Value = filter.Dal
                cursor.DataEsportazione.Operator = OP.OP_LE
            End If
            If filter.IDPersona <> 0 Then
                cursor.IDPersonaImportata.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor.NomePersonaImportata.Value = filter.Nominativo & "%"
                cursor.NomePersonaImportata.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor.IDOperatoreConferma.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                'cursor1..Value = filter.Scopo & "%"
                'cursor1.Motivo.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor.StatoRemoto.Value = StatoEsportazione.NonEsportato
                    Case 1 'in corso
                        cursor.StatoRemoto.Value = StatoEsportazione.Esportato
                        cursor.StatoConferma.Value = StatoConfermaEsportazione.Inviato
                    Case Else
                        cursor.StatoConferma.ValueIn({StatoConfermaEsportazione.Confermato, StatoConfermaEsportazione.Revocato, StatoConfermaEsportazione.Rifiutata})
                End Select
            End If
            If filter.Numero <> "" Then
                'cursor.sou.Value = filter.Numero & "%"
                'cursor.MezzoDiInvio.Operator = OP.OP_LIKE
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor1.id.Value = filter.TipoContesto
                'cursor1.ContextID.Value = filter.IDContesto
            End If
            cursor.DataEsportazione.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights

            While Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                Me.AddIActivities(items, cursor.Item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            Catch Ex As Exception
                Throw
            Finally
#End If
            cursor.Dispose()
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Private Sub AddEActivities(ByVal col As CCollection(Of StoricoAction), ByVal exp As CImportExport)
            Dim action As New StoricoAction

            action.Data = exp.DataEsportazione
            action.IDOperatore = exp.IDEsportataDa
            action.NomeOperatore = exp.NomeEsportataDa
            action.IDCliente = exp.IDPersonaEsportata
            action.NomeCliente = exp.NomePersonaEsportata
            action.Note = "Richiesta Confronto - Inviata<br/><br/>" & exp.MessaggioEsportazione & "<br/><hr/><br/>" & exp.MessaggioImportazione
            action.Scopo = "Richiesta Confronto"
            action.NumeroOIndirizzo = exp.Source.Name
            If (exp.StatoRemoto = StatoEsportazione.Errore) Then
                action.Esito = EsitoChiamata.RIPOSTA
            Else
                action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
            End If
            action.DettaglioEsito = exp.StatoRemoto
            action.Durata = 0
            action.Attesa = 0
            action.Tag = exp
            action.ActionSubID = 0
            action.Ricevuta = False
            action.StatoConversazione = IIf(exp.StatoConferma = StatoConfermaEsportazione.Inviato, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
            col.Add(action)

            If (exp.StatoConferma <> StatoConfermaEsportazione.Inviato) Then
                action = New StoricoAction
                action.Data = exp.DataEsportazioneOk
                action.IDOperatore = exp.IDOperatoreConferma
                action.NomeOperatore = exp.NomeOperatoreConferma
                action.IDCliente = exp.IDPersonaEsportata
                action.NomeCliente = exp.NomePersonaEsportata
                Select Case exp.StatoConferma
                    Case StatoConfermaEsportazione.Confermato
                        action.Note = "Invio Forzato da Locale<br/><br/>" & exp.MessaggioConferma
                        action.Scopo = "Invio Forzato"
                    Case StatoConfermaEsportazione.Revocato
                        action.Note = "Elaborazione Annullata da Locale<br/><br/>" & exp.MessaggioConferma
                        action.Scopo = "Elaborazione Annullata"
                    Case StatoConfermaEsportazione.Rifiutata
                        action.Note = "Elaborazione Rifiutata da Remoto<br/><br/>" & exp.MessaggioConferma
                        action.Scopo = "Elaborazione Rifiutata"

                End Select
                action.NumeroOIndirizzo = exp.Source.Name
                If (exp.StatoRemoto = StatoEsportazione.Errore) Then
                    action.Esito = EsitoChiamata.RIPOSTA
                Else
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                End If
                action.DettaglioEsito = exp.StatoRemoto
                action.Durata = 0
                action.Attesa = 0
                action.Tag = exp
                action.ActionSubID = 1
                action.Ricevuta = False
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            End If


        End Sub

        Private Sub AddIActivities(ByVal col As CCollection(Of StoricoAction), ByVal exp As CImportExport)
            Dim action As New StoricoAction

            action.Data = exp.DataEsportazione
            action.IDOperatore = exp.IDEsportataDa
            action.NomeOperatore = exp.NomeEsportataDa
            action.IDCliente = exp.IDPersonaImportata
            action.NomeCliente = exp.NomePersonaImportata
            action.Note = "Richiesta Confronto Ricevuta<br/><br/>" & exp.MessaggioEsportazione & "<br/><hr/><br/>" & exp.MessaggioImportazione
            action.Scopo = "Richiesta Confronto"
            action.NumeroOIndirizzo = exp.Source.Name
            If (exp.StatoRemoto = StatoEsportazione.Errore) Then
                action.Esito = EsitoChiamata.RIPOSTA
            Else
                action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
            End If
            action.DettaglioEsito = exp.StatoRemoto
            action.Durata = 0
            action.Attesa = 0
            action.Tag = exp
            action.ActionSubID = 0
            action.Ricevuta = True
            action.StatoConversazione = IIf(exp.StatoConferma = StatoConfermaEsportazione.Inviato, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
            col.Add(action)

            If (exp.StatoConferma <> StatoConfermaEsportazione.Inviato) Then
                action = New StoricoAction
                action.Data = exp.DataEsportazioneOk
                action.IDOperatore = exp.IDOperatoreConferma
                action.NomeOperatore = exp.NomeOperatoreConferma
                action.IDCliente = exp.IDPersonaEsportata
                action.NomeCliente = exp.NomePersonaEsportata

                Select Case exp.StatoConferma
                    Case StatoConfermaEsportazione.Confermato
                        action.Note = "Richiesta Confronto Confermata da Remoto<br/><br/>" & exp.MessaggioConferma
                        action.Scopo = "Richiesta Confronto"
                    Case StatoConfermaEsportazione.Revocato
                        action.Note = "Richiesta Confronto Annullata da Remoto<br/><br/>" & exp.MessaggioConferma
                        action.Scopo = "Richiesta Confronto"
                    Case StatoConfermaEsportazione.Rifiutata
                        action.Note = "Richiesta Confronto Rifiutata<br/><br/>" & exp.MessaggioConferma
                        action.Scopo = "Richiesta Confronto"
                End Select
                action.NumeroOIndirizzo = exp.Source.Name
                If (exp.StatoRemoto = StatoEsportazione.Errore) Then
                    action.Esito = EsitoChiamata.RIPOSTA
                Else
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                End If
                action.DettaglioEsito = exp.StatoRemoto
                action.Durata = 0
                action.Attesa = 0
                action.Tag = exp
                action.ActionSubID = 1
                action.Ricevuta = False
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            End If

        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CImportExport", "Importazioni/Esportazioni")
        End Sub

        Public Sub New()

        End Sub
    End Class


End Class