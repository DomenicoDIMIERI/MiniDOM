Imports minidom
Imports minidom.Sistema
Imports minidom.Databases



Partial Public Class Anagrafica


    <Serializable>
    Public Class CRegisteredFindHandlerCursor
        Inherits DBObjectCursor(Of CRegisteredFindHandler)

        Private m_HandlerClass As New CCursorFieldObj(Of String)("HandlerClass")
        Private m_EditorClass As New CCursorFieldObj(Of String)("EditorClass")
        Private m_Context As New CCursorFieldObj(Of String)("Context")
        Private m_Priority As New CCursorField(Of Integer)("Priority")


        Public Sub New()
        End Sub

        Public ReadOnly Property EditorClass As CCursorFieldObj(Of String)
            Get
                Return Me.m_EditorClass
            End Get
        End Property

        Public ReadOnly Property HandlerClass As CCursorFieldObj(Of String)
            Get
                Return Me.m_HandlerClass
            End Get
        End Property

        Public ReadOnly Property Context As CCursorFieldObj(Of String)
            Get
                Return Me.m_Context
            End Get
        End Property

        Public ReadOnly Property Priority As CCursorField(Of Integer)
            Get
                Return Me.m_Priority
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CRegisteredFindHandler()
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMRegFindHandler"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Anagrafica.Persone.GetConnection()
        End Function


    End Class

End Class