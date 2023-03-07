Imports minidom
Imports minidom.Sistema
Imports minidom.Databases



Partial Public Class Anagrafica


    Public Class MotiviContattoCursor
        Inherits DBObjectCursor(Of MotivoContatto)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Flags As New CCursorField(Of MotivoContattoFlags)("Flags")
        Private m_TipoContatto As New CCursorFieldObj(Of String)("TipoContatto")

        Public Sub New()
        End Sub

        Public ReadOnly Property TipoContatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContatto
            End Get
        End Property


        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of MotivoContattoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMMotiviContatto"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.MotiviContatto.Module
        End Function


        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

    End Class


End Class