Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica


Partial Public Class Finanziaria



    ''' <summary>
    ''' Cursore sulla tabella degli obiettivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CObiettivoCategoriaProdottoCursor
        Inherits DBObjectCursorPO(Of CObiettivoCategoriaProdotto)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_TipoObiettivo As New CCursorField(Of TipoObiettivo)("TipoObiettivo")
        Private m_PeriodicitaObiettivo As New CCursorField(Of PeriodicitaObiettivo)("PeriodicitaObiettivo")
        Private m_IDCategoria As New CCursorField(Of Integer)("IDCategoria")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Percentuale As New CCursorField(Of Double)("Percentuale")
        Private m_NomeGruppo As New CCursorFieldObj(Of String)("NomeGruppo")

        Public Sub New()
        End Sub

        Public ReadOnly Property NomeGruppo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeGruppo
            End Get
        End Property

        Public ReadOnly Property Percentuale As CCursorField(Of Double)
            Get
                Return Me.m_Percentuale
            End Get
        End Property

        Public ReadOnly Property IDCategoria As CCursorField(Of Integer)
            Get
                Return Me.m_IDCategoria
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property TipoObiettivo As CCursorField(Of TipoObiettivo)
            Get
                Return Me.m_TipoObiettivo
            End Get
        End Property

        Public ReadOnly Property PeriodicitaObiettivo As CCursorField(Of PeriodicitaObiettivo)
            Get
                Return Me.m_PeriodicitaObiettivo
            End Get
        End Property



        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Finanziaria.ObiettiviXCategoria.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDObiettiviXCat"
        End Function
    End Class




End Class
