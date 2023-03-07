Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica


Partial Public Class Finanziaria



    ''' <summary>
    ''' Cursore sulla tabella degli sconti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RichiestaApprovazioneGroupCursor
        Inherits DBObjectCursorPO(Of RichiestaApprovazioneGroup)

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_IDRichiedente As New CCursorField(Of Integer)("IDRichiedente")
        Private m_NomeRichiedente As New CCursorFieldObj(Of String)("NomeRichiedente")
        Private m_IDMotivoRichiesta As New CCursorField(Of Integer)("IDMotivoRichiesta")
        Private m_Motivo As New CCursorFieldObj(Of String)("Motivo")
        Private m_DettaglioRichiesta As New CCursorFieldObj(Of String)("DettaglioRichiesta")
        Private m_IDSupervisore As New CCursorField(Of Integer)("IDSupervisore")
        Private m_DataEsito As New CCursorField(Of Date)("DataEsito")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property IDRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiedente
            End Get
        End Property

        Public ReadOnly Property NomeRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRichiedente
            End Get
        End Property

        Public ReadOnly Property IDMotivoRichiesta As CCursorField(Of Integer)
            Get
                Return Me.m_IDMotivoRichiesta
            End Get
        End Property

        Public ReadOnly Property Motivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Motivo
            End Get
        End Property

        Public ReadOnly Property DettaglioRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioRichiesta
            End Get
        End Property

        Public ReadOnly Property IDSupervisore As CCursorField(Of Integer)
            Get
                Return Me.m_IDSupervisore
            End Get
        End Property

        Public ReadOnly Property DataEsito As CCursorField(Of Date)
            Get
                Return Me.m_DataEsito
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Finanziaria.RichiesteApprovazioneGroups.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDGrpRichApp"
        End Function
    End Class




End Class
