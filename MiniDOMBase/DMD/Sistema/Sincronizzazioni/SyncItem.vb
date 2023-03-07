Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Rappresenta una elemento di sincronizzazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public NotInheritable Class SyncItem
        Inherits DBObject

        Private m_RemoteSite As String
        Private m_ItemType As String

        Private m_LocalID As Integer
        Private m_RemoteID As Integer
        Private m_SyncDate As Date?

        Public Sub New()
            Me.m_RemoteSite = ""
            Me.m_ItemType = ""
            Me.m_LocalID = 0
            Me.m_RemoteID = 0
            Me.m_SyncDate = Nothing
        End Sub

        Public Property RemoteSite As String
            Get
                Return Me.m_RemoteSite
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RemoteSite
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_RemoteSite = value
                Me.DoChanged("RemoteSite", value, oldValue)
            End Set
        End Property

        Public Property ItemType As String
            Get
                Return Me.m_ItemType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ItemType
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ItemType = value
                Me.DoChanged("ItemType", value, oldValue)
            End Set
        End Property

        Public Property LocalID As Integer
            Get
                Return Me.m_LocalID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.LocalID
                If (oldValue = value) Then Return
                Me.m_LocalID = value
                Me.DoChanged("LocalID", value, oldValue)
            End Set
        End Property

        Public Property RemoteID As Integer
            Get
                Return Me.m_RemoteID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.RemoteID
                If (oldValue = value) Then Return
                Me.m_RemoteID = value
                Me.DoChanged("RemoteID", value, oldValue)
            End Set
        End Property

        Public Property SyncDate As Date?
            Get
                Return Me.m_SyncDate
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_SyncDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_SyncDate = value
                Me.DoChanged("SyncDate", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_Syncs"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_RemoteSite = reader.Read("RemoteSite", Me.m_RemoteSite)
            Me.m_ItemType = reader.Read("ItemType", Me.m_ItemType)
            Me.m_LocalID = reader.Read("LocalID", Me.m_LocalID)
            Me.m_RemoteID = reader.Read("RemoteID", Me.m_RemoteID)
            Me.m_SyncDate = reader.Read("SyncDate", Me.m_SyncDate)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("RemoteSite", Me.m_RemoteSite)
            writer.Write("ItemType", Me.m_ItemType)
            writer.Write("LocalID", Me.LocalID)
            writer.Write("RemoteID", Me.RemoteID)
            writer.Write("SyncDate", Me.m_SyncDate)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As minidom.XML.XMLWriter)
            writer.WriteAttribute("RemoteSite", Me.m_RemoteSite)
            writer.WriteAttribute("ItemType", Me.m_ItemType)
            writer.WriteAttribute("LocalID", Me.LocalID)
            writer.WriteAttribute("RemoteID", Me.RemoteID)
            writer.WriteAttribute("SyncDate", Me.m_SyncDate)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "RemoteSite" : Me.m_RemoteSite = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ItemType" : Me.m_ItemType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LocalID" : Me.m_LocalID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RemoteID" : Me.m_RemoteID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SyncDate" : Me.m_SyncDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class


End Class