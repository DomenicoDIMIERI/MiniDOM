Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Sistema
Imports minidom.CustomerCalls

Partial Public Class Finanziaria

    Public Enum CQSPDStatoLavorazioneLevel As Integer
        NonIniziata = 0
        Contatto = 10
        Visita = 20
        Consulenza = 30
        Pratica = 40
    End Enum

    Public Enum CQSPDStatoLavorazioneEsito As Integer
        Nessuno = 0
        InLavorazione = 10
        Concluso = 20
        Annullato = 30
    End Enum

    <Serializable>
    Public Class StatisticaLavorazione
        Implements XML.IDMDXMLSerializable, IComparable

        Private m_IDOperatoreContatto As Integer
        Private m_OperatoreContatto As CUser

        Private m_IDConsulente As Integer
        Private m_Consulente As CConsulentePratica

        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore

        Private m_IDCliente As Integer
        Private m_Cliente As CPersonaFisica
        Private m_NomeCliente As String
        Private m_IconURL As String
        Private m_InizioLavorazione As Date?
        Private m_FineLavorazione As Date?
        Private m_DataContatto As Date?
        Private m_DataVisita As Date?
        Private m_DataConsulenza As Date?
        Private m_DataPratica As Date?

        Private m_Livello As CQSPDStatoLavorazioneLevel
        Private m_Esito As CQSPDStatoLavorazioneEsito
        Private m_DettaglioEsito As String

        Friend Contatti As New CCollection(Of CContattoUtente)
        Friend Visite As New CCollection(Of CVisita)
        Friend Consulenze As New CCollection(Of CQSPDConsulenza)
        Friend Pratiche As New CCollection(Of CPraticaCQSPD)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)

            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_IconURL = ""
            Me.m_InizioLavorazione = Nothing
            Me.m_FineLavorazione = Nothing
            Me.m_DataContatto = Nothing
            Me.m_DataVisita = Nothing
            Me.m_DataConsulenza = Nothing
            Me.m_DataPratica = Nothing

            Me.m_Livello = CQSPDStatoLavorazioneLevel.NonIniziata
            Me.m_Esito = CQSPDStatoLavorazioneEsito.Nessuno
            Me.m_DettaglioEsito = ""

            Me.m_IDOperatoreContatto = 0
            Me.m_OperatoreContatto = Nothing

            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing

            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing
        End Sub

        Public Property IDOperatoreContatto As Integer
            Get
                Return GetID(Me.m_OperatoreContatto, Me.m_IDOperatoreContatto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreContatto
                If (oldValue = value) Then Return
                Me.m_IDOperatoreContatto = value
                Me.m_OperatoreContatto = Nothing
                Me.DoChanged("IDOperatoreContatto", value, oldValue)
            End Set
        End Property

        Public Property OperatoreContatto As CUser
            Get
                If (Me.m_OperatoreContatto Is Nothing) Then Me.m_OperatoreContatto = Sistema.Users.GetItemById(Me.m_IDOperatoreContatto)
                Return Me.m_OperatoreContatto
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreContatto
                If (oldValue Is value) Then Return
                Me.m_OperatoreContatto = value
                Me.m_IDOperatoreContatto = GetID(value)
                Me.DoChanged("OperatoreContatto", value, oldValue)
            End Set
        End Property

        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulente
                If (oldValue = value) Then Return
                Me.m_IDConsulente = value
                Me.m_Consulente = Nothing
                Me.DoChanged("IDConsulente", value, oldValue)
            End Set
        End Property

        Public Property Consulente As CConsulentePratica
            Get
                If (Me.m_Consulente Is Nothing) Then Me.m_Consulente = Finanziaria.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Dim oldValue As CConsulentePratica = Me.Consulente
                If (oldValue Is value) Then Return
                Me.m_Consulente = value
                Me.m_IDConsulente = GetID(value)
                Me.DoChanged("Consulente", value, oldValue)
            End Set
        End Property

        Public Property IDCollaboratore As Integer
            Get
                Return GetID(Me.m_Collaboratore, Me.m_IDCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCollaboratore
                If (oldValue = value) Then Return
                Me.m_Collaboratore = Nothing
                Me.m_IDCollaboratore = value
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property

        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = Finanziaria.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue Is value) Then Return
                Me.m_Collaboratore = value
                Me.m_IDCollaboratore = GetID(value)
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        Private Sub DoChanged(ByVal propName As String, ByVal newValue As Object, ByVal oldValue As Object)

        End Sub

        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Return
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        Public Property Cliente As CPersonaFisica
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue Is value) Then Return
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                Me.m_NomeCliente = "" : Me.m_IconURL = ""
                If (value IsNot Nothing) Then
                    Me.m_NomeCliente = value.Nominativo
                    Me.m_IconURL = value.IconURL
                End If
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCliente(ByVal value As CPersonaFisica)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        Public Property InizioLavorazione As Date?
            Get
                Return Me.m_InizioLavorazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_InizioLavorazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_InizioLavorazione = value
                Me.DoChanged("InizioLavorazione", value, oldValue)
            End Set
        End Property

        Public Property FineLavorazione As Date?
            Get
                Return Me.m_FineLavorazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_FineLavorazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_FineLavorazione = value
                Me.DoChanged("FineLavorazione", value, oldValue)
            End Set
        End Property

        Public Property DataContatto As Date?
            Get
                Return Me.m_DataContatto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataContatto
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataContatto = value
                Me.DoChanged("DataContatto", value, oldValue)
            End Set
        End Property

        Public Property DataVisita As Date?
            Get
                Return Me.m_DataVisita
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataVisita
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataVisita = value
                Me.DoChanged("DataVisita", value, oldValue)
            End Set
        End Property

        Public Property DataConsulenza As Date?
            Get
                Return Me.m_DataConsulenza
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataConsulenza
                If (oldValue = value) Then Return
                Me.m_DataConsulenza = value
                Me.DoChanged("DataConsulenza", value, oldValue)
            End Set
        End Property

        Public Property DataPratica As Date
            Get
                Return Me.m_DataPratica
            End Get
            Set(value As Date)
                Dim oldValue As Date? = Me.m_DataPratica
                If (oldValue = value) Then Return
                Me.m_DataPratica = value
                Me.DoChanged("DataPratica", value, oldValue)
            End Set
        End Property

        Public Property Livello As CQSPDStatoLavorazioneLevel
            Get
                Return Me.m_Livello
            End Get
            Set(value As CQSPDStatoLavorazioneLevel)
                Dim oldValue As CQSPDStatoLavorazioneLevel = Me.m_Livello
                If (oldValue = value) Then Return
                Me.m_Livello = value
                Me.DoChanged("Livello", value, oldValue)
            End Set
        End Property

        Public Property Esito As CQSPDStatoLavorazioneEsito
            Get
                Return Me.m_Esito
            End Get
            Set(value As CQSPDStatoLavorazioneEsito)
                Dim oldValue As CQSPDStatoLavorazioneEsito = Me.m_Esito
                If (oldValue = value) Then Return
                Me.m_Esito = value
                Me.DoChanged("Esito", value, oldValue)
            End Set
        End Property

        Public Property DettaglioEsito As String
            Get
                Return Me.m_DettaglioEsito
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsito
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DettaglioEsito = value
                Me.DoChanged("DettaglioEsito", value, oldValue)
            End Set
        End Property

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "InizioLavorazione" : Me.m_InizioLavorazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FineLavorazione" : Me.m_FineLavorazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataContatto" : Me.m_DataContatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataVisita" : Me.m_DataVisita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataConsulenza" : Me.m_DataConsulenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataPratica" : Me.m_DataPratica = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Livello" : Me.m_Livello = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Esito" : Me.m_Esito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioEsito" : Me.m_DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOperatoreContatto" : Me.m_IDOperatoreContatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDConsulente" : Me.m_IDConsulente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("InizioLavorazione", Me.m_InizioLavorazione)
            writer.WriteAttribute("FineLavorazione", Me.m_FineLavorazione)
            writer.WriteAttribute("DataContatto", Me.m_DataContatto)
            writer.WriteAttribute("DataVisita", Me.m_DataVisita)
            writer.WriteAttribute("DataConsulenza", Me.m_DataConsulenza)
            writer.WriteAttribute("DataPratica", Me.m_DataPratica)
            writer.WriteAttribute("Livello", Me.m_Livello)
            writer.WriteAttribute("Esito", Me.m_Esito)
            writer.WriteAttribute("IDOperatoreContatto", Me.IDOperatoreContatto)
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteTag("DettaglioEsito", Me.m_DettaglioEsito)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property WorkingProgressPercentage As Integer
            Get
                'NonIniziata = 0
                'Contatto = 10
                'Visita = 20
                'Consulenza = 30
                'Pratica = 40
                Return CInt(Me.Livello) * 100 / 40
            End Get
        End Property

        Friend Function GetDataPrimaConsulenza() As Date?
            Dim ret As Date? = Nothing
            For Each c As CQSPDConsulenza In Me.Consulenze
                ret = DateUtils.Min(ret, c.DataConsulenza)
            Next
            Return ret
        End Function

        Public Sub Update()
            Me.DataConsulenza = Me.GetDataPrimaConsulenza
            If (Me.Pratiche.Count > 0) Then
                Me.Livello = CQSPDStatoLavorazioneLevel.Pratica
                Me.Esito = CQSPDStatoLavorazioneEsito.Concluso
                Me.DettaglioEsito = "Pratica"
            ElseIf (Me.Consulenze.Count > 0) Then
                Me.Livello = CQSPDStatoLavorazioneLevel.Consulenza
                Me.Esito = CQSPDStatoLavorazioneEsito.Concluso
                Me.DettaglioEsito = "Consulenza"
            ElseIf (Me.Visite.Count > 0) Then
                Me.Livello = CQSPDStatoLavorazioneLevel.Visita
                Me.Esito = CQSPDStatoLavorazioneEsito.Concluso
                Me.DettaglioEsito = "Visita"
            ElseIf (Me.Contatti.Count > 0) Then
                Me.Livello = CQSPDStatoLavorazioneLevel.Contatto
                Me.Esito = CQSPDStatoLavorazioneEsito.Concluso
                Me.DettaglioEsito = "Contatto"
            Else
                Me.Livello = CQSPDStatoLavorazioneLevel.NonIniziata
                Me.Esito = CQSPDStatoLavorazioneEsito.Nessuno
                Me.DettaglioEsito = ""
            End If
        End Sub

        Public Function CompareTo(ByVal obj As StatisticaLavorazione) As Integer
            Return obj.WorkingProgressPercentage - Me.WorkingProgressPercentage
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class

End Class