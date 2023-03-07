Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria


    ''' <summary>
    ''' Cursore che consente di recuperare tutte le tabelle finanziarie associate ad un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CProdottoXTabellaFinCursor
        Inherits DBObjectCursor(Of CProdottoXTabellaFin)

        Private m_ProdottoID As CCursorField(Of Integer)
        Private m_TabellaFinanziariaID As CCursorField(Of Integer)

        Public Sub New()
            Me.m_ProdottoID = New CCursorField(Of Integer)("Prodotto")
            Me.m_TabellaFinanziariaID = New CCursorField(Of Integer)("Tabella")
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return minidom.Finanziaria.TabelleFinanziarie.Module 'modCQSPDTblFinanz
        End Function

        Public ReadOnly Property ProdottoID As CCursorField(Of Integer)
            Get
                Return Me.m_ProdottoID
            End Get
        End Property

        Public ReadOnly Property TabellaFinanziariaID As CCursorField(Of Integer)
            Get
                Return Me.m_TabellaFinanziariaID
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabFin"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProdottoXTabellaFin
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Finanziaria.Database
        End Function
    End Class

End Class