Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria



    ''' <summary>
    ''' Rappresenta una collezione di estinzioni
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CEstinzioniXEstintoreCollection
        Inherits CCollection(Of EstinzioneXEstintore)

        <NonSerialized> Private m_Estintore As Object

        Public Sub New()
            Me.m_Estintore = Nothing
        End Sub

        Public Sub New(ByVal estintore As Object)
            Me.m_Estintore = estintore
            Me.Load()
        End Sub

        Public ReadOnly Property Estintore As IEstintore
            Get
                Return Me.m_Estintore
            End Get
        End Property

        Public Overloads Function Add(ByVal es As CEstinzione) As EstinzioneXEstintore
            Dim item As New EstinzioneXEstintore
            item.Estintore = Me.m_Estintore
            item.Stato = ObjectStatus.OBJECT_VALID
            item.Save()
            MyBase.Add(item)
            Return item
        End Function

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Estintore IsNot Nothing) Then DirectCast(newValue, EstinzioneXEstintore).SetEstintore(Me.m_Estintore)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Estintore IsNot Nothing) Then DirectCast(value, EstinzioneXEstintore).SetEstintore(Me.m_Estintore)
            MyBase.OnInsert(index, value)
        End Sub

        Public Sub Load()
            If (Me.m_Estintore Is Nothing) Then Throw New ArgumentNullException("Estintore")
            Me.Clear()
            If (GetID(Me.m_Estintore) = 0) Then Exit Sub

            Using cursor As New EstinzioneXEstintoreCursor()
                cursor.IDEstintore.Value = GetID(Me.m_Estintore)
                cursor.TipoEstintore.Value = TypeName(Me.m_Estintore)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End Using

        End Sub

        Protected Friend Overridable Sub SetEstintore(ByVal value As Object)
            Me.m_Estintore = value
        End Sub

        Public Sub PreparaEstinzini()
            Me.PreparaEstinzini(DirectCast(Me.m_Estintore, IEstintore).DataDecorrenza)
        End Sub

        Public Sub PreparaEstinzini(ByVal decorrenza As Date)
            Dim estintore As IEstintore = Me.m_Estintore
            Dim persona = estintore.Cliente()
            If (persona Is Nothing) Then Throw New ArgumentNullException("cliente")

            Dim altriPrestiti As CCollection(Of CEstinzione) = Finanziaria.Estinzioni.GetEstinzioniByPersona(persona)
            Dim items As CEstinzioniXEstintoreCollection = Me

            For i As Integer = 0 To altriPrestiti.Count() - 1
                Dim trovato As Boolean = False
                Dim est As CEstinzione = altriPrestiti(i)
                Dim item As EstinzioneXEstintore = Nothing

                If (est.Stato() = ObjectStatus.OBJECT_VALID AndAlso est.IsInCorso(decorrenza)) Then
                    For j As Integer = 0 To items.Count() - 1
                        item = items(j)
                        trovato = (item.IDEstinzione = GetID(est))
                        If (trovato) Then Exit For
                    Next
                End If
                If (Not trovato) Then
                    Dim resid As Integer = Formats.ToInteger(est.NumeroRateResidue())
                    If (est.Scadenza.HasValue = False AndAlso est.DataInizio.HasValue AndAlso Formats.ToInteger(est.Durata) > 0) Then
                        est.Scadenza = DateUtils.GetLastMonthDay(DateUtils.DateAdd("M", est.Durata.Value, est.DataInizio()))
                    End If
                    If (est.Scadenza.HasValue) Then resid = Math.Max(0, DateUtils.DateDiff("M", decorrenza, est.Scadenza.Value) + 1)
                    If (Formats.ToInteger(est.Durata) > 0) Then resid = Math.Min(resid, est.Durata)

                    item = New EstinzioneXEstintore()


                    item.Selezionata = False
                    item.Estinzione = est
                    item.Estintore = Me.m_Estintore
                    item.Parametro = Nothing
                    item.Correzione = 0
                    item.NumeroQuoteInsolute = 0
                    item.NumeroQuoteResidue = resid
                    item.Stato = ObjectStatus.OBJECT_VALID
                    item.DataEstinzione = decorrenza
                    'item.DataCaricamento = dataCaricamento
                    item.AggiornaValori()
                    items.Add(item)
                    item.Save(True)
                End If
            Next
        End Sub

        Public Sub Aggiorna()
            Dim estintore As IEstintore = Me.Estintore
            Dim nomeCess As String = ""
            ' Dim d As Date
            Dim o As COffertaCQS = Nothing
            Dim dcar As Date
            Dim dest As Date
            Dim ddec As Date

            ddec = dcar

            If (estintore IsNot Nothing) Then
                dcar = Me.Estintore.DataCaricamento
                If (estintore.DataDecorrenza.HasValue) Then ddec = Me.Estintore.DataDecorrenza.Value

                If (TypeOf (estintore) Is CPraticaCQSPD) Then
                    o = DirectCast(estintore, CPraticaCQSPD).OffertaCorrente
                ElseIf (TypeOf (estintore) Is CQSPDConsulenza) Then
                    Dim cons As CQSPDConsulenza = estintore
                    If (cons.OffertaCQS IsNot Nothing AndAlso cons.OffertaCQS.Stato = ObjectStatus.OBJECT_VALID) Then
                        o = cons.OffertaCQS
                    ElseIf (cons.OffertaPD IsNot Nothing AndAlso cons.OffertaPD.Stato = ObjectStatus.OBJECT_VALID) Then
                        o = cons.OffertaPD
                    End If
                End If

                If (o IsNot Nothing) Then
                    nomeCess = o.NomeCessionario
                    dcar = o.DataCaricamento
                    If (o.DataDecorrenza.HasValue) Then
                        ddec = o.DataDecorrenza
                    End If
                End If

            End If


            For Each item As EstinzioneXEstintore In Me
                Dim resid As Integer = item.NumeroQuoteResidue
                Dim IsInterno As Boolean = Strings.Compare(nomeCess, item.NomeCessionario, CompareMethod.Text) = 0

                If (Not item.DataFine.HasValue AndAlso item.DataDecorrenza.HasValue AndAlso Formats.ToInteger(item.Durata) > 0) Then
                    item.DataFine = DateUtils.GetLastMonthDay(DateUtils.DateAdd("M", item.Durata.Value, item.DataDecorrenza.Value))
                End If

                item.DataCaricamento = dcar
                item.DataEstinzione = dest
                item.AggiornaCalcolo()
                item.Save()
            Next
        End Sub

    End Class

End Class