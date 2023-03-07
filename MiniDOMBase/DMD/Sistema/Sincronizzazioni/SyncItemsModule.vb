Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Internals

Namespace Internals

    ''' <summary>
    ''' Collezione delle autorizzazioni utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public NotInheritable Class SyncItemsModule
        Inherits CModulesClass(Of SyncItem)


        Public Sub New()
            MyBase.New("modSyncItems", GetType(SyncItemsCursor), 0)
        End Sub

        Public Function GetMatch(ByVal remoteSite As String, ByVal itemType As String, ByVal remoteID As Integer) As SyncItem
            Dim cursor As New SyncItemsCursor
            Dim ret As SyncItem = Nothing
            cursor.IgnoreRights = True
            cursor.RemoteSite.Value = remoteSite
            cursor.RemoteID.Value = remoteID
            cursor.ItemType.Value = itemType
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            ret = cursor.Item
            cursor.Dispose()
            cursor = Nothing
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce l'ID dell'oggetto locale corrispondente
        ''' </summary>
        ''' <param name="itemType"></param>
        ''' <param name="remoteID"></param>
        ''' <returns></returns>
        Public Function MatchLocalItemID(ByVal remoteSite As String, ByVal itemType As String, ByVal remoteID As Integer) As Integer
            Dim item As SyncItem = Me.GetMatch(remoteSite, itemType, remoteID)
            If (item Is Nothing) Then Return 0
            Return item.LocalID
        End Function

        Public Function MatchLocalObject(ByVal remoteSite As String, ByVal itemType As String, ByVal remoteID As Integer) As Object
            Dim localID As Integer = Me.MatchLocalItemID(remoteSite, itemType, remoteID)
            If (localID = 0) Then Return Nothing
            Return Sistema.Types.GetItemByTypeAndId(itemType, localID)
        End Function

        Public Function SetMatch(ByVal remoteSite As String, ByVal itemType As String, ByVal remoteID As Integer, ByVal localID As Integer) As SyncItem
            Dim item As SyncItem = Me.GetMatch(remoteSite, itemType, remoteID)
            If (item Is Nothing) Then
                item = New SyncItem
                item.RemoteSite = remoteSite
                item.ItemType = itemType
                item.RemoteID = remoteID
            End If
            item.LocalID = localID
            item.SyncDate = DateUtils.Now
            item.Stato = ObjectStatus.OBJECT_VALID
            item.Save()
            Return item
        End Function



    End Class


End Namespace

Partial Class Sistema

    Private Shared m_SyncItems As SyncItemsModule = Nothing

    Public Shared ReadOnly Property SyncItems() As SyncItemsModule
        Get
            If (m_SyncItems Is Nothing) Then m_SyncItems = New SyncItemsModule
            Return m_SyncItems
        End Get
    End Property

End Class

