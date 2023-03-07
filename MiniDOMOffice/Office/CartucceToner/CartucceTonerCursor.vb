Imports minidom.Databases
Imports minidom.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei scansioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CartucceTonerCursor
        Inherits DBObjectCursorPO(Of CartucciaToner)

        Private m_IDArticolo As New CCursorField(Of Integer)("IDArticolo")
        Private m_NomeArticolo As New CCursorFieldObj(Of String)("NomeArticolo")
        Private m_CodiceArticolo As New CCursorFieldObj(Of String)("CodiceArticolo")
        Private m_Modello As New CCursorFieldObj(Of String)("Modello")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")

        Private m_IDPostazione As New CCursorField(Of Integer)("IDPostazione")
        Private m_NomePostazione As New CCursorFieldObj(Of String)("NomePostazione")

        Private m_DataAcquisto As New CCursorField(Of Date)("DataAcquisto")
        Private m_DataInstallazione As New CCursorField(Of Date)("DataInstallazione")
        Private m_DataEsaurimento As New CCursorField(Of Date)("DataEsaurimento")
        Private m_DataRimozione As New CCursorField(Of Date)("DataRimozione")
        Private m_StampeDisponibili As New CCursorField(Of Integer)("StampeDisponibili")
        Private m_StampeEffettuate As New CCursorField(Of Integer)("StampeEffettuate")

        Private m_Flags As New CCursorField(Of CartucciaTonerFlags)("Flags")




        Public Sub New()
        End Sub

        Public ReadOnly Property IDArticolo As CCursorField(Of Integer)
            Get
                Return Me.m_IDArticolo
            End Get
        End Property

        Public ReadOnly Property NomeArticolo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeArticolo
            End Get
        End Property

        Public ReadOnly Property CodiceArticolo As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceArticolo
            End Get
        End Property

        Public ReadOnly Property Modello As CCursorFieldObj(Of String)
            Get
                Return Me.m_Modello
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IDPostazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDPostazione
            End Get
        End Property

        Public ReadOnly Property NomePostazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePostazione
            End Get
        End Property

        Public ReadOnly Property DataAcquisto As CCursorField(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
        End Property

        Public ReadOnly Property DataInstallazione As CCursorField(Of Date)
            Get
                Return Me.m_DataInstallazione
            End Get
        End Property

        Public ReadOnly Property DataEsaurimento As CCursorField(Of Date)
            Get
                Return Me.m_DataEsaurimento
            End Get
        End Property

        Public ReadOnly Property DataRimozione As CCursorField(Of Date)
            Get
                Return Me.m_DataRimozione
            End Get
        End Property

        Public ReadOnly Property StampeDisponibili As CCursorField(Of Integer)
            Get
                Return Me.m_StampeDisponibili
            End Get
        End Property

        Public ReadOnly Property StampeEffettuate As CCursorField(Of Integer)
            Get
                Return Me.m_StampeEffettuate
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of CartucciaTonerFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.CartucceToners.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCartucceToner"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class