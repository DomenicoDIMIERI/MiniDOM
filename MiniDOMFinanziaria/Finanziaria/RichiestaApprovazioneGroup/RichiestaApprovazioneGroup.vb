Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica


Partial Public Class Finanziaria

    ''' <summary>
    ''' Oggetto per richiesta approvazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class RichiestaApprovazioneGroup
        Inherits DBObjectPO
        Implements ICloneable

        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String
        Private m_DataRichiesta As Date
        Private m_IDRichiedente As Integer
        Private m_Richiedente As CUser
        Private m_NomeRichiedente As String
        Private m_IDMotivoRichiesta As Integer
        Private m_MotivoRichiesta As CMotivoScontoPratica
        Private m_Motivo As String
        Private m_DettaglioRichiesta As String
        Private m_IDSupervisore As Integer
        Private m_Supervisore As CUser
        Private m_DataEsito As Date?
        Private m_Richieste As RichiesteApprovazioneCollection

        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_DataRichiesta = Now
            Me.m_IDRichiedente = 0
            Me.m_Richiedente = Nothing
            Me.m_NomeRichiedente = ""
            Me.m_IDMotivoRichiesta = 0
            Me.m_MotivoRichiesta = Nothing
            Me.m_Motivo = ""
            Me.m_DettaglioRichiesta = ""
            Me.m_IDSupervisore = 0
            Me.m_Supervisore = Nothing
            Me.m_DataEsito = Nothing
            Me.m_Richieste = Nothing
        End Sub

        Public ReadOnly Property Richieste As RichiesteApprovazioneCollection
            Get
                If (Me.m_Richieste Is Nothing) Then Me.m_Richieste = New RichiesteApprovazioneCollection(Me)
                Return Me.m_Richieste
            End Get
        End Property

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

        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Return
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                Me.m_NomeCliente = "" : If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCliente(ByVal p As CPersona)
            Me.m_Cliente = p
            Me.m_IDCliente = GetID(p)
        End Sub

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Return
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property DataRichiesta As Date
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataRichiesta
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        Public Property IDRichiedente As Integer
            Get
                Return GetID(Me.m_Richiedente, Me.m_IDRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiedente
                If (oldValue = value) Then Return
                Me.m_IDRichiedente = value
                Me.m_Richiedente = Nothing
                Me.DoChanged("IDRichiedente", value, oldValue)
            End Set
        End Property

        Public Property Richiedente As CUser
            Get
                If (Me.m_Richiedente Is Nothing) Then Me.m_Richiedente = Sistema.Users.GetItemById(Me.m_IDRichiedente)
                Return Me.m_Richiedente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Richiedente
                If (oldValue Is value) Then Return
                Me.m_Richiedente = value
                Me.m_IDRichiedente = GetID(value)
                Me.m_NomeRichiedente = "" : If (value IsNot Nothing) Then Me.m_NomeRichiedente = value.Nominativo
                Me.DoChanged("Richiedente", value, oldValue)
            End Set
        End Property

        Public Property NomeRichiedente As String
            Get
                Return Me.m_NomeRichiedente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRichiedente
                If (oldValue = value) Then Return
                Me.m_NomeRichiedente = value
                Me.DoChanged("NomeRichiedente", value, oldValue)
            End Set
        End Property

        Public Property IDMotivoRichiesta As Integer
            Get
                Return GetID(Me.m_MotivoRichiesta, Me.m_IDMotivoRichiesta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMotivoRichiesta
                If (oldValue = value) Then Return
                Me.m_IDMotivoRichiesta = value
                Me.m_MotivoRichiesta = Nothing
                Me.DoChanged("IDMotivoRichiesta", value, oldValue)
            End Set
        End Property

        Public Property MotivoRichiesta As CMotivoScontoPratica
            Get
                If (Me.m_MotivoRichiesta Is Nothing) Then Me.m_MotivoRichiesta = Finanziaria.MotiviSconto.GetItemById(Me.m_IDMotivoRichiesta)
                Return Me.m_MotivoRichiesta
            End Get
            Set(value As CMotivoScontoPratica)
                Dim oldValue As CMotivoScontoPratica = Me.MotivoRichiesta
                If (oldValue Is value) Then Return
                Me.m_MotivoRichiesta = value
                Me.m_IDMotivoRichiesta = GetID(value)
                Me.m_Motivo = "" : If (value IsNot Nothing) Then Me.m_Motivo = value.Nome
                Me.DoChanged("MotivoRichiesta", value, oldValue)
            End Set
        End Property

        Public Property Motivo As String
            Get
                Return Me.m_Motivo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Motivo
                If (oldValue = value) Then Return
                Me.m_Motivo = value
                Me.DoChanged("Motivo", value, oldValue)
            End Set
        End Property

        Public Property DettaglioRichiesta As String
            Get
                Return Me.m_DettaglioRichiesta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioRichiesta
                If (oldValue = value) Then Return
                Me.m_DettaglioRichiesta = value
                Me.DoChanged("DettaglioRichiesta", value, oldValue)
            End Set
        End Property

        Public Property IDSupervisore As Integer
            Get
                Return GetID(Me.m_Supervisore, Me.m_IDSupervisore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSupervisore
                If (oldValue = value) Then Return
                Me.m_IDSupervisore = value
                Me.m_Supervisore = Nothing
                Me.DoChanged("IDSupervisore", value, oldValue)
            End Set
        End Property
        Public Property Supervisore As CUser
            Get
                If (Me.m_Supervisore Is Nothing) Then Me.m_Supervisore = Sistema.Users.GetItemById(Me.m_IDSupervisore)
                Return Me.m_Supervisore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Supervisore
                If (oldValue Is value) Then Return
                Me.m_Supervisore = value
                Me.m_IDSupervisore = GetID(value)
                Me.DoChanged("Supervisore", value, oldValue)
            End Set
        End Property
        Public Property DataEsito As Date?
            Get
                Return Me.m_DataEsito
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEsito
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataEsito = value
                Me.DoChanged("DataEsito", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Finanziaria.RichiesteApprovazioneGroups.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDGrpRichApp"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_IDRichiedente = reader.Read("IDRichiedente", Me.m_IDRichiedente)
            Me.m_NomeRichiedente = reader.Read("NomeRichiedente", Me.m_NomeRichiedente)
            Me.m_IDMotivoRichiesta = reader.Read("IDMotivoRichiesta", Me.m_IDMotivoRichiesta)
            Me.m_Motivo = reader.Read("Motivo", Me.m_Motivo)
            Me.m_DettaglioRichiesta = reader.Read("DettaglioRichiesta", Me.m_DettaglioRichiesta)
            Me.m_IDSupervisore = reader.Read("IDSupervisore", Me.m_IDSupervisore)
            Me.m_DataEsito = reader.Read("DataEsito", Me.m_DataEsito)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("IDRichiedente", Me.IDRichiedente)
            writer.Write("NomeRichiedente", Me.m_NomeRichiedente)
            writer.Write("IDMotivoRichiesta", Me.IDMotivoRichiesta)
            writer.Write("Motivo", Me.m_Motivo)
            writer.Write("DettaglioRichiesta", Me.m_DettaglioRichiesta)
            writer.Write("IDSupervisore", Me.IDSupervisore)
            writer.Write("DataEsito", Me.m_DataEsito)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("IDRichiedente", Me.IDRichiedente)
            writer.WriteAttribute("NomeRichiedente", Me.m_NomeRichiedente)
            writer.WriteAttribute("IDMotivoRichiesta", Me.IDMotivoRichiesta)
            writer.WriteAttribute("Motivo", Me.m_Motivo)
            writer.WriteAttribute("IDSupervisore", Me.IDSupervisore)
            writer.WriteAttribute("DataEsito", Me.m_DataEsito)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("DettaglioRichiesta", Me.m_DettaglioRichiesta)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDRichiedente" : Me.m_IDRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRichiedente" : Me.m_NomeRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDMotivoRichiesta" : Me.m_IDMotivoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Motivo" : Me.m_Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioRichiesta" : Me.m_DettaglioRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDSupervisore" : Me.m_IDSupervisore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataEsito" : Me.m_DataEsito = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


        Public Sub Richiedi()
            For Each ra As CRichiestaApprovazione In Me.Richieste
                If ra.Stato = ObjectStatus.OBJECT_VALID AndAlso ra.StatoRichiesta = StatoRichiestaApprovazione.NONCHIESTA Then
                    ra.StatoRichiesta = StatoRichiestaApprovazione.ATTESA
                    ra.Save()
                End If
            Next
            Finanziaria.RichiesteApprovazioneGroups.doOnRichiedi(New ItemEventArgs(Of RichiestaApprovazioneGroup)(Me))
        End Sub

        Public Sub Approva()
            Finanziaria.RichiesteApprovazioneGroups.doOnApprova(New ItemEventArgs(Of RichiestaApprovazioneGroup)(Me))
        End Sub

        Public Sub Rifiuta()
            Finanziaria.RichiesteApprovazioneGroups.doOnRifiuta(New ItemEventArgs(Of RichiestaApprovazioneGroup)(Me))
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function
    End Class




End Class
