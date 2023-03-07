Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria

    <Serializable>
    Public Class CCollaboratoriCursor
        Inherits DBObjectCursorPO(Of CCollaboratore)

        Private m_PersonaID As CCursorField(Of Integer)
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_UserID As CCursorField(Of Integer)
        Private m_NomeUtente As New CCursorFieldObj(Of String)("NomeUtente")
        Private m_AttivatoDaID As CCursorField(Of Integer)
        Private m_DataAttivazione As CCursorField(Of Date)
        Private m_ReferenteID As CCursorField(Of Integer)
        Private m_Indirizzo As CCursorFieldObj(Of String)
        Private m_NumeroIscrizioneUIF As CCursorFieldObj(Of String)
        Private m_NumeroIscrizioneRUI As CCursorFieldObj(Of String)
        Private m_NumeroIscrizioneISVAP As CCursorFieldObj(Of String)
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_PersonaID = New CCursorField(Of Integer)("Persona")
            Me.m_UserID = New CCursorField(Of Integer)("Utente")
            Me.m_AttivatoDaID = New CCursorField(Of Integer)("AttivatoDa")
            Me.m_DataAttivazione = New CCursorField(Of Date)("DataAttivazione")
            Me.m_ReferenteID = New CCursorField(Of Integer)("Referente")
            Me.m_Indirizzo = New CCursorFieldObj(Of String)("Indirizzo")
            Me.m_NumeroIscrizioneUIF = New CCursorFieldObj(Of String)("NumeroIscrizioneUIF")
            Me.m_NumeroIscrizioneRUI = New CCursorFieldObj(Of String)("NumeroIscrizioneRUI")
            Me.m_NumeroIscrizioneISVAP = New CCursorFieldObj(Of String)("NumeroIscrizioneISVAP")
            Me.m_OnlyValid = False
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property NomeUtente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUtente
            End Get
        End Property

        Public ReadOnly Property PersonaID As CCursorField(Of Integer)
            Get
                Return Me.m_PersonaID
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property


        Public ReadOnly Property AttivatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_AttivatoDaID
            End Get
        End Property

        Public ReadOnly Property DataAttivazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAttivazione
            End Get
        End Property

        Public ReadOnly Property ReferenteID As CCursorField(Of Integer)
            Get
                Return Me.m_ReferenteID
            End Get
        End Property

        Public ReadOnly Property Indirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Public ReadOnly Property NumeroIscrizioneUIF As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroIscrizioneUIF
            End Get
        End Property

        Public ReadOnly Property NumeroIscrizioneRUI As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroIscrizioneRUI
            End Get
        End Property

        Public ReadOnly Property NumeroIscrizioneISVAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroIscrizioneISVAP
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Collaboratori"
        End Function

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Finanziaria.Collaboratori.Module
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCollaboratore
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("OnlyValid", Me.m_OnlyValid)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizioRapporto] Is Null) Or ([DataInizioRapporto]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFineRapporto] Is Null) Or ([DataFineRapporto]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class

End Class