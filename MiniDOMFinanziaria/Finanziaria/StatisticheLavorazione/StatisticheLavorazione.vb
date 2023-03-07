Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Sistema
Imports minidom.Internals
Imports minidom.Finanziaria
Imports minidom.CustomerCalls

Namespace Internals


    <Serializable>
    Public Class CStatisticheLavorazioneClass

        Public Function GetStatistiche(ByVal p As CPersonaFisica, ByVal fromDate As Date?, ByVal toDate As Date?) As StatisticaLavorazione
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Return Me.GetStatistiche({GetID(p)}, fromDate, toDate)(0)
        End Function

        Public Function GetStatistiche(ByVal idPersone As Integer(), ByVal fromDate As Date?, ByVal toDate As Date?) As StatisticaLavorazione()
            Dim ret As New CKeyCollection(Of StatisticaLavorazione)
            Dim item As StatisticaLavorazione = Nothing

            For Each pid As Integer In idPersone
                item = New StatisticaLavorazione
                item.IDCliente = pid
                ret.Add("K" & pid, item)
            Next

            Me.SyncPersone(ret)
            Me.SyncContatti(ret, fromDate, toDate)
            Me.SyncConsulenze(ret, fromDate, toDate)
            Me.SyncPratiche(ret, fromDate, toDate)

            For Each item In ret
                item.Update()
            Next

            'Correzione
            Dim retArr As StatisticaLavorazione() = ret.ToArray()
            If (retArr Is Nothing) Then retArr = New StatisticaLavorazione() {}

            Return retArr
        End Function

        Private Sub SyncPersone(ByVal items As CKeyCollection(Of StatisticaLavorazione))
            Dim arrID As Integer() = {}

            For Each item As StatisticaLavorazione In items
                Dim i As Integer = Arrays.BinarySearch(arrID, item.IDCliente)
                If (i < 0) Then arrID = Arrays.InsertSorted(arrID, item.IDCliente)
            Next

            If (arrID.Length = 0) Then Return
            Dim cursor As New CPersonaCursor
            cursor.ID.ValueIn(arrID)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
            cursor.PageSize = 1000

            While Not cursor.EOF
                Dim p As CPersonaFisica = cursor.Item
                Dim item As StatisticaLavorazione = items.GetItemByKey("K" & GetID(p))
                item.SetCliente(p)
                item.IconURL = p.IconURL
                item.NomeCliente = p.Nominativo
                cursor.MoveNext()
            End While
            cursor.Dispose()


        End Sub

        Private Sub SyncContatti(ByVal items As CKeyCollection(Of StatisticaLavorazione), ByVal fromDate As Date?, ByVal toDate As Date?)
            Dim arrID As Integer() = {}

            For Each item As StatisticaLavorazione In items
                Dim i As Integer = Arrays.BinarySearch(arrID, item.IDCliente)
                If (i < 0) Then arrID = Arrays.InsertSorted(arrID, item.IDCliente)
            Next

            If (arrID.Length = 0) Then Return
            Dim cursor As New CCustomerCallsCursor
            cursor.IDPersona.ValueIn(arrID)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.Data.Between(fromDate, toDate)
            cursor.PageSize = 1000

            While Not cursor.EOF
                Dim c As CContattoUtente = cursor.Item
                Dim item As StatisticaLavorazione = items.GetItemByKey("K" & c.IDPersona)
                If (TypeOf (c) Is CVisita) Then
                    item.Visite.Add(c)
                Else
                    item.Contatti.Add(c)
                End If
                cursor.MoveNext()
            End While
            cursor.Dispose()

        End Sub

        Private Sub SyncConsulenze(ByVal items As CKeyCollection(Of StatisticaLavorazione), ByVal fromDate As Date?, ByVal toDate As Date?)
            Dim arrID As Integer() = {}

            For Each item As StatisticaLavorazione In items
                Dim i As Integer = Arrays.BinarySearch(arrID, item.IDCliente)
                If (i < 0) Then arrID = Arrays.InsertSorted(arrID, item.IDCliente)
            Next

            If (arrID.Length = 0) Then Return
            Dim cursor As New CQSPDConsulenzaCursor
            cursor.IDCliente.ValueIn(arrID)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.DataConsulenza.Between(fromDate, toDate)
            cursor.PageSize = 1000

            While Not cursor.EOF
                Dim c As CQSPDConsulenza = cursor.Item
                Dim item As StatisticaLavorazione = items.GetItemByKey("K" & c.IDCliente)
                item.Consulenze.Add(c)
                cursor.MoveNext()
            End While
            cursor.Dispose()

        End Sub


        Private Sub SyncPratiche(ByVal items As CKeyCollection(Of StatisticaLavorazione), ByVal fromDate As Date?, ByVal toDate As Date?)
            Dim arrID As Integer() = {}

            For Each item As StatisticaLavorazione In items
                Dim i As Integer = Arrays.BinarySearch(arrID, item.IDCliente)
                If (i < 0) Then arrID = Arrays.InsertSorted(arrID, item.IDCliente)
            Next

            If (arrID.Length = 0) Then Return
            Dim cursor As New CPraticheCQSPDCursor
            cursor.IDCliente.ValueIn(arrID)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.CreatoIl.Between(fromDate, toDate)
            cursor.PageSize = 1000

            While Not cursor.EOF
                Dim c As CPraticaCQSPD = cursor.Item
                Dim item As StatisticaLavorazione = items.GetItemByKey("K" & c.IDCliente)
                item.DataConsulenza = DateUtils.GetDatePart(item.GetDataPrimaConsulenza())
                Dim dCar As Date? = c.DataCaricamento
                If (DateUtils.Compare(item.DataConsulenza, dCar) >= 0) Then item.Pratiche.Add(c)
                cursor.MoveNext()
            End While
            cursor.Dispose()

        End Sub
    End Class


End Namespace

Partial Public Class Finanziaria

    Private Shared m_StatisticheLavorazione As CStatisticheLavorazioneClass = Nothing

    Public Shared ReadOnly Property StatisticheLavorazione As CStatisticheLavorazioneClass
        Get
            If (m_StatisticheLavorazione Is Nothing) Then m_StatisticheLavorazione = New CStatisticheLavorazioneClass
            Return m_StatisticheLavorazione
        End Get
    End Property




End Class