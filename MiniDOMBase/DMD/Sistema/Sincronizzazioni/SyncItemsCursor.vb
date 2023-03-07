Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulla tabella degli elementi sincronizzati
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SyncItemsCursor
        Inherits DBObjectCursor(Of SyncItem)

        Private m_RemoteSite As New CCursorFieldObj(Of String)("RemoteSite")
        Private m_ItemType As New CCursorFieldObj(Of String)("ItemType")
        Private m_LocalID As New CCursorField(Of Integer)("LocalID")
        Private m_RemoteID As New CCursorField(Of Integer)("RemoteID")
        Private m_SyncDate As New CCursorField(Of Date)("SyncDate")

        Public Sub New()
        End Sub


        Public ReadOnly Property RemoteSite As CCursorFieldObj(Of String)
            Get
                Return Me.m_RemoteSite
            End Get
        End Property

        Public ReadOnly Property ItemType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ItemType
            End Get
        End Property

        Public ReadOnly Property LocalID As CCursorField(Of Integer)
            Get
                Return Me.m_LocalID
            End Get
        End Property

        Public ReadOnly Property RemoteID As CCursorField(Of Integer)
            Get
                Return Me.m_RemoteID
            End Get
        End Property

        Public ReadOnly Property SyncDate As CCursorField(Of Date)
            Get
                Return Me.m_SyncDate
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Syncs"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class


End Class