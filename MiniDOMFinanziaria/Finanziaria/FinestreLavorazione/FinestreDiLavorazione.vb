Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals
Imports minidom.Anagrafica
Imports minidom.CustomerCalls

Namespace Internals

    <Serializable>
    Public Class FinestreDiLavorazioneClass
        Inherits CModulesClass(Of FinestraLavorazione)

        Public Sub New()
            MyBase.New("modCQSPDWinLavorazione", GetType(FinestraLavorazioneCursor), 0)
        End Sub

        ''' <summary>
        ''' Restituisce la prossima finestra di lavorazione per il cliente specificato
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetProssimaFinestra(ByVal persona As CPersonaFisica) As FinestraLavorazione
            Dim lw As FinestraLavorazione

            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            If (GetID(persona) = 0) Then Return Nothing

            lw = Me.GetFinestraCorrente(persona)
            If (lw IsNot Nothing) Then Return lw 'Non ci possono essere due finestra attive
            If (persona.Deceduto) Then Return Nothing 'Non si pu� pi� lavorare un deceduto
            If (lw Is Nothing) Then
                lw = Me.GetUltimaFinestraLavorata(persona)
            End If


            Dim w As New FinestraLavorazione
            w.Cliente = persona
            w.Stato = ObjectStatus.OBJECT_VALID
            w.DataInizioLavorabilita = w.DataProssimaFinestra
            w.SetFlag(FinestraLavorazioneFlags.Rinnovo, w.AltriPrestiti.Count > 0)
            w.DataUltimoAggiornamento = DateUtils.Now
            w.SetFlag(FinestraLavorazioneFlags.Disponibile_CQS, Me.HaCQS(persona, w.DataInizioLavorabilita))
            w.SetFlag(FinestraLavorazioneFlags.Disponibile_PD, Me.HaPD(persona, w.DataInizioLavorabilita))
            If (persona.ImpiegoPrincipale.StipendioNetto.HasValue) Then
                'If w.GetFlag(FinestraLavorazioneFlags.Disponibile_CQS) AndAlso w.GetFlag(FinestraLavorazioneFlags.Disponibile_PD) Then
                '    w.QuotaCedibile = 2 * persona.ImpiegoPrincipale.StipendioNetto.Value / 5
                'Else
                '    w.QuotaCedibile = persona.ImpiegoPrincipale.StipendioNetto.Value / 5
                'End If
            End If
            w.Save()

            Return w
        End Function

        Private Function GetMinDate(ByVal contatti As CCollection(Of CContattoUtente)) As DateTime?
            Dim ret As DateTime? = Nothing
            For Each c As CContattoUtente In contatti
                ret = DateUtils.Min(ret, c.Data)
            Next
            Return ret
        End Function

        Public Function Ricostruisci(ByVal p As CPersonaFisica) As CCollection(Of FinestraLavorazione)
            Dim finestre As CCollection(Of FinestraLavorazione) = Finanziaria.FinestreDiLavorazione.GetFinestreByPersona(p) : finestre.Sort()
            Dim pratiche As CCollection(Of CPraticaCQSPD) = Finanziaria.Pratiche.GetPraticheByPersona(p) : pratiche.Sort()
            Dim studif As CCollection(Of CQSPDConsulenza) = Finanziaria.Consulenze.GetConsulenzeByPersona(p) : studif.Sort()
            Dim richiestef As CCollection(Of CRichiestaFinanziamento) = Finanziaria.RichiesteFinanziamento.GetRichiesteByPersona(p) : richiestef.Sort()
            Dim contatti As CCollection(Of CContattoUtente) = CustomerCalls.CRM.GetContattiByPersona(p) : contatti.Sort()
            Dim prestiti As CCollection(Of CEstinzione) = Finanziaria.Estinzioni.GetEstinzioniByPersona(p)

            Dim w As FinestraLavorazione = Nothing
            If (finestre.Count = 0) Then
                w = New FinestraLavorazione
                w.Cliente = p
                w.Stato = ObjectStatus.OBJECT_VALID
                finestre.Add(w)
            End If
            Dim i As Integer = finestre.Count
            Dim data As Date? = Me.GetMinDate(contatti)
            If (data Is Nothing) Then data = p.CreatoIl
            data = DateUtils.GetDatePart(data)
            While (i > 0)
                i = 1
                w = finestre(i)
                ' w.AltriPrestiti

            End While

            Return finestre
        End Function

        Public Function GetProssimaFinestra(ByVal id As Integer) As FinestraLavorazione
            If (id = 0) Then Return Nothing
            Dim persona As CPersonaFisica = Anagrafica.Persone.GetItemById(id)
            Return Me.GetProssimaFinestra(persona)
        End Function

        Public Function GetFinestraCorrente(ByVal pid As Integer) As FinestraLavorazione
            Dim cursor As FinestraLavorazioneCursor = Nothing
            Dim ret As FinestraLavorazione = Nothing
#If Not DEBUG Then
            Try
#End If
            If (pid = 0) Then Return Nothing
            cursor = New FinestraLavorazioneCursor
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCliente.Value = pid
            cursor.DataInizioLavorazione.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            ret = cursor.Item
            cursor.Dispose() : cursor = Nothing
            If (ret Is Nothing) Then
                cursor = New FinestraLavorazioneCursor
                cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDCliente.Value = pid
                cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                ret = cursor.Item
            End If

#If Not DEBUG Then
 
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
            End Try
            
#End If
            Return ret
        End Function

        Public Function GetFinestraCorrente(ByVal persona As CPersonaFisica) As FinestraLavorazione
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            If (GetID(persona) = 0) Then Return Nothing
            Dim ret As FinestraLavorazione = Me.GetFinestraCorrente(GetID(persona))
            If (ret IsNot Nothing) Then ret.SetCliente(persona)
            Return ret
        End Function

        Public Sub AggiornaFinestraLavorazione(ByVal persona As CPersonaFisica, ByVal w As FinestraLavorazione)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            If (w Is Nothing) Then Throw New ArgumentNullException("w")
            w.Cliente = persona
            w.NomeCliente = persona.Nominativo
            w.IconaCliente = persona.IconURL
            w.Stato = ObjectStatus.OBJECT_VALID
            w.DataInizioLavorabilita = Me.CalcolaDataLavorabilita(persona)
            w.DataUltimoAggiornamento = DateUtils.Now
            w.SetFlag(FinestraLavorazioneFlags.Disponibile_CQS, Me.HaCQS(persona, w.DataInizioLavorabilita))
            w.SetFlag(FinestraLavorazioneFlags.Disponibile_PD, Me.HaPD(persona, w.DataInizioLavorabilita))
            w.Save()

        End Sub

        ''' <summary>
        ''' Restituisce la data di inizio lavorabilit� per la persona
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalcolaDataLavorabilita(ByVal persona As CPersonaFisica) As Date?
            Dim ret As Date? = Nothing
            Dim ultima As FinestraLavorazione = Me.GetUltimaFinestraLavorata(persona)
            If Not (ultima Is Nothing) Then ret = ultima.DataFineLavorazione

            Dim prestiti As CCollection(Of CEstinzione) = Finanziaria.Estinzioni.GetPrestitiAttivi(persona)
            Dim minPrestito As Date? = Nothing
            For Each p As CEstinzione In prestiti
                minPrestito = DateUtils.Min(minPrestito, p.DataRinnovo)
            Next

            ret = DateUtils.Max(minPrestito, ret)
            If (ret.HasValue = False) Then ret = DateUtils.ToDay

            Return ret
        End Function

        Public Function HaCQS(ByVal persona As CPersonaFisica, ByVal al As Date?) As Boolean
            Dim tr As String = persona.ImpiegoPrincipale.TipoRapporto
            Return True
            'If (tr = "") Then Return True
            'If (al.HasValue = False) Then al = Calendar.Now
            'For Each p As CCQSPDProdotto In Finanziaria.Prodotti.LoadAll
            '    If p.Stato = ObjectStatus.OBJECT_VALID AndAlso p.IdTipoRapporto = tr AndAlso p.IdTipoContratto = "C" AndAlso p.IsValid(al) Then
            '        Return True
            '    End If
            'Next
            'Return False
        End Function

        Public Function HaPD(ByVal persona As CPersonaFisica, ByVal al As Date?) As Boolean
            Dim tr As String = persona.ImpiegoPrincipale.TipoRapporto
            Return (tr = "") OrElse (tr <> "H")
        End Function

        Function GetFinestreByPersona(ByVal persona As CPersona) As CCollection(Of FinestraLavorazione)
            Dim cursor As FinestraLavorazioneCursor = Nothing

            Try
                If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
                Dim ret As New CCollection(Of FinestraLavorazione)
                If (GetID(persona) = 0) Then Return ret

                cursor = New FinestraLavorazioneCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_ASC
                cursor.IDCliente.Value = GetID(persona)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Dim fl As FinestraLavorazione = cursor.Item
                    fl.SetCliente(persona)
                    ret.Add(fl)
                    cursor.MoveNext()
                End While


                ret.Sort()

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Restituisce la finestra di lavorazione attiva alla data specificata. 
        ''' La funzione non � utilizzabile per le date future (cerca solo tra le finestre gi� salvate)
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <param name="allaData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFinestra(ByVal persona As CPersona, ByVal allaData As Date) As FinestraLavorazione
            Dim cursor As FinestraLavorazioneCursor = Nothing

            Try
                If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
                If (GetID(persona) = 0) Then Return Nothing

                cursor = New FinestraLavorazioneCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.DataInizioLavorabilita.SortOrder = SortEnum.SORT_ASC
                cursor.IDCliente.Value = GetID(persona)
                cursor.IgnoreRights = True
                cursor.WhereClauses.Add("([DataInizioLavorabilita]<=" & DBUtils.DBDate(allaData) & ") AND ([DataFineLavorazione]>=" & DBUtils.DBDate(allaData) & ")")
                Dim fl As FinestraLavorazione = cursor.Item
                If (fl IsNot Nothing) Then fl.SetCliente(persona)

                Return fl
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Restituisce l'ultima finestra in stato chiuso
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUltimaFinestraLavorata(ByVal persona As CPersona) As FinestraLavorazione
            Dim cursor As FinestraLavorazioneCursor = Nothing

            Try
                If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
                If (GetID(persona) = 0) Then Return Nothing

                cursor = New FinestraLavorazioneCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDCliente.Value = GetID(persona)
                cursor.StatoFinestra.Value = StatoFinestraLavorazione.Chiusa
                cursor.IgnoreRights = True
                cursor.DataFineLavorazione.SortOrder = SortEnum.SORT_DESC
                Dim fl As FinestraLavorazione = cursor.Item
                If (fl IsNot Nothing) Then fl.SetCliente(persona)

                Return fl
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub


    End Class

End Namespace

Partial Public Class Finanziaria

    Private Shared m_FinestreDiLavorazione As FinestreDiLavorazioneClass = Nothing

    Public Shared ReadOnly Property FinestreDiLavorazione As FinestreDiLavorazioneClass
        Get
            If m_FinestreDiLavorazione Is Nothing Then m_FinestreDiLavorazione = New FinestreDiLavorazioneClass
            Return m_FinestreDiLavorazione
        End Get
    End Property


End Class