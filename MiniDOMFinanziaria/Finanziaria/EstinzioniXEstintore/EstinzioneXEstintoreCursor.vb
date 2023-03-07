﻿Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria

    ''' <summary>
    ''' Cursore sulla tabella delle estinzioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EstinzioneXEstintoreCursor
        Inherits DBObjectCursor(Of EstinzioneXEstintore)

        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Selezionata As New CCursorField(Of Boolean)("Selezionata")
        Private m_IDEstinzione As New CCursorField(Of Integer)("IDEstinzione")
        Private m_IDEstintore As New CCursorField(Of Integer)("IDEstintore")
        Private m_TipoEstintore As New CCursorFieldObj(Of String)("TipoEstintore")
        Private m_DataDecorrenza As New CCursorField(Of Date)("DataDecorrenza")
        Private m_NumeroQuoteInsolute As New CCursorField(Of Integer)("NQI")
        Private m_NumeroQuoteScadute As New CCursorField(Of Integer)("NQS")
        Private m_NumeroQuoteResidue As New CCursorField(Of Integer)("NQR")
        Private m_Parametro As New CCursorFieldObj(Of String)("Parametro")
        Private m_Correzione As New CCursorField(Of Decimal)("Correzione")
        Private m_PenaleEstinzione As New CCursorField(Of Double)("PenaleEstinzione")
        Private m_DataEstinzione As New CCursorField(Of Date)("DataEstinzione")
        Private m_Rata As New CCursorField(Of Decimal)("Rata")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Durata As New CCursorField(Of Integer)("Durata")
        Private m_TAN As New CCursorField(Of Double)("TAN")
        Private m_TAEG As New CCursorField(Of Double)("TAEG")
        Private m_TotaleDaEstinguere As New CCursorField(Of Double)("TotaleDaEstinguere")
        Private m_IDCessionario As New CCursorField(Of Integer)("IDCessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_NomeAgenzia As New CCursorFieldObj(Of String)("NomeAgenzia")
        Private m_Tipo As New CCursorField(Of TipoEstinzione)("Tipo")
        Private m_MontanteResiduo As New CCursorField(Of Decimal)("MontanteResiduo")
        Private m_DataCaricamento As New CCursorField(Of Date)("DataCaricamento")
        Private m_MontanteResiduoForzato As New CCursorField(Of Decimal)("MontanteResiduoForzato")

        Public Sub New()
        End Sub

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property MontanteResiduoForzato As CCursorField(Of Decimal)
            Get
                Return Me.m_MontanteResiduoForzato
            End Get
        End Property

        Public ReadOnly Property DataCaricamento As CCursorField(Of Date)
            Get
                Return Me.m_DataCaricamento
            End Get
        End Property

        Public ReadOnly Property PenaleEstinzione As CCursorField(Of Double)
            Get
                Return Me.m_PenaleEstinzione
            End Get
        End Property

        Public ReadOnly Property MontanteResiduo As CCursorField(Of Decimal)
            Get
                Return Me.m_MontanteResiduo
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorField(Of TipoEstinzione)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_IDCessionario
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property NomeAgenzia As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgenzia
            End Get
        End Property


        Public ReadOnly Property DataEstinzione As CCursorField(Of Date)
            Get
                Return Me.m_DataEstinzione
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Integer)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
            End Get
        End Property

        Public ReadOnly Property TAEG As CCursorField(Of Double)
            Get
                Return Me.m_TAEG
            End Get
        End Property

        Public ReadOnly Property TotaleDaEstinguere As CCursorField(Of Double)
            Get
                Return Me.m_TotaleDaEstinguere
            End Get
        End Property

        Public ReadOnly Property Selezionata As CCursorField(Of Boolean)
            Get
                Return Me.m_Selezionata
            End Get
        End Property

        Public ReadOnly Property Parametro As CCursorFieldObj(Of String)
            Get
                Return Me.m_Parametro
            End Get
        End Property

        Public ReadOnly Property IDEstinzione As CCursorField(Of Integer)
            Get
                Return Me.m_IDEstinzione
            End Get
        End Property

        Public ReadOnly Property IDEstintore As CCursorField(Of Integer)
            Get
                Return Me.m_IDEstintore
            End Get
        End Property

        Public ReadOnly Property TipoEstintore As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoEstintore
            End Get
        End Property

        Public ReadOnly Property DataDecorrenza As CCursorField(Of Date)
            Get
                Return Me.m_DataDecorrenza
            End Get
        End Property

        Public ReadOnly Property NumeroQuoteInsolute As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroQuoteInsolute
            End Get
        End Property

        Public ReadOnly Property NumeroQuoteScadute As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroQuoteScadute
            End Get
        End Property

        Public ReadOnly Property NumeroQuoteResidue As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroQuoteResidue
            End Get
        End Property

        Public ReadOnly Property Correzione As CCursorField(Of Decimal)
            Get
                Return Me.m_Correzione
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New EstinzioneXEstintore
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EstinzioniXEstintore"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function



    End Class


End Class
