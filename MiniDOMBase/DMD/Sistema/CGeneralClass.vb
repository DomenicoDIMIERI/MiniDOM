Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases

#Const UseGetNonCachedItemById = False

Partial Public Class Sistema

    Public MustInherit Class CModulesClass(Of T As DBObjectBase)


        ''' <summary>
        ''' Notifica al sistema l'inserimento di un nuovo oggetto
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ItemCreated(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Notifica al sistema l'eliminazione di un oggetto
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ItemDeleted(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Notifica al sistema che un oggetto è stato modificato
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ItemModified(ByVal sender As Object, ByVal e As ItemEventArgs)



        Private m_ModuleName As String
        Private m_CursorType As System.Type
        Private m_Module As CModule
        Private m_CachedItems As CCollection(Of CacheItem)
        Private m_CacheSize As Integer
        Private m_AllItems As CCollection(Of T)
        Private m_isInit As Boolean
        Private m_Conn As CDBConnection = Nothing

        ''' <summary>
        ''' Oggetto utilizzabile per la sincronizzazione
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly cacheLock As New Object

        Protected Class CacheItem
            Public Item As T
            Public Live As Integer

            Public Sub New()
                DMDObject.IncreaseCounter(Me)
            End Sub

            Public Sub New(ByVal item As T)
                DMDObject.IncreaseCounter(Me)
                Me.Item = item
                Me.Live = 1
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMDObject.DecreaseCounter(Me)
            End Sub
        End Class


        ''' <summary>
        ''' Costruttore
        ''' </summary>
        Public Sub New()
            DMDObject.IncreaseCounter(Me)
            Me.m_ModuleName = ""
            Me.m_CursorType = Nothing
            Me.m_Module = Nothing
            Me.m_CacheSize = 0
            Me.m_CachedItems = New CCollection(Of CacheItem)
            Me.m_AllItems = Nothing
            Me.m_isInit = False
        End Sub

        ''' <summary>
        ''' Costruttore
        ''' </summary>
        ''' <param name="moduleName"></param>
        Public Sub New(ByVal moduleName As String)
            Me.New()
            moduleName = Trim(moduleName)
            If (Left(moduleName, 3) <> "mod") Then Throw New ArgumentNullException("moduleName")
            If (moduleName = "") Then Throw New ArgumentNullException("moduleName")
            Me.m_ModuleName = moduleName
        End Sub

        ''' <summary>
        ''' Costruttore
        ''' </summary>
        ''' <param name="moduleName"></param>
        ''' <param name="cursorType"></param>
        Public Sub New(ByVal moduleName As String, ByVal cursorType As System.Type)
            Me.New()
            moduleName = Trim(moduleName)
            If (Left(moduleName, 3) <> "mod") Then Throw New ArgumentNullException("moduleName")
            If (moduleName = "") Then Throw New ArgumentNullException("moduleName")
            If (cursorType Is Nothing) Then Throw New ArgumentNullException("cursorType")
            Me.m_ModuleName = moduleName
            Me.m_CursorType = cursorType
        End Sub

        ''' <summary>
        ''' Costruttore
        ''' </summary>
        ''' <param name="moduleName"></param>
        ''' <param name="cursorType"></param>
        ''' <param name="cacheSize"></param>
        Public Sub New(ByVal moduleName As String, ByVal cursorType As System.Type, ByVal cacheSize As Integer)
            Me.New()
            moduleName = Strings.Trim(moduleName)
            If (Left(moduleName, 3) <> "mod") Then Throw New ArgumentNullException("moduleName")
            If (moduleName = "") Then Throw New ArgumentNullException("moduleName")
            If (cursorType Is Nothing) Then Throw New ArgumentNullException("cursorType")
            Me.m_ModuleName = moduleName
            Me.m_CursorType = cursorType
            Me.m_Module = Nothing
            Me.m_CacheSize = cacheSize
        End Sub

        ''' <summary>
        ''' Restituisce un riferimento al modulo
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property [Module] As CModule
            Get
                SyncLock Me
                    If (Me.m_Module Is Nothing) Then Me.m_Module = Sistema.Modules.GetItemByName(Me.m_ModuleName)
                    If (Me.m_Module Is Nothing) Then Me.m_Module = Me.CreateModuleInfo()
                    Return Me.m_Module
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Inizializza le risorse necessarie al modulo
        ''' </summary>
        Public Overridable Sub Initialize()
            If (Me.m_isInit) Then Return
            Dim m As CModule = Me.Module
            Me.m_isInit = True
        End Sub

        ''' <summary>
        ''' Rilascia le risorse necessarie al modulo
        ''' </summary>
        Public Overridable Sub Terminate()
            Me.m_isInit = False
        End Sub

        ''' <summary>
        ''' Crea il modulo e salva le informazioni nel DB
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function CreateModuleInfo() As CModule
            Dim ret As New CModule(Me.m_ModuleName)
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Description = Me.m_ModuleName
            ret.DisplayName = Me.m_ModuleName
            ret.ClassHandler = "CBaseModuleHandler"
            ret.Save()

            ret.InitializeStandardActions()
            Return ret
        End Function

        ''' <summary>
        ''' Crea un'istanza del cursore
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function CreateCursor() As DBObjectCursorBase
            Return Types.CreateInstance(Me.m_CursorType)
        End Function

        ''' <summary>
        ''' Restituisce l'elemento in base all'ID
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Overridable Function GetItemById(ByVal id As Integer) As T
            SyncLock Me.cacheLock
                If (id = 0) Then Return Nothing
                Dim ret As T = Me.GetCachedItemById(id)
                If (ret Is Nothing) Then
                    ret = Me.GetNonCachedItemById(id)
                    If (ret IsNot Nothing) Then Me.AddToCache(ret)
                End If
                Return ret
            End SyncLock
        End Function

        Protected Overridable Function GetCachedItemById(ByVal id As Integer) As T
            If (id = 0) Then Return Nothing
            For i As Integer = 0 To Me.m_CachedItems.Count - 1
                Dim o As CacheItem = Me.m_CachedItems(i)
                If (GetID(o.Item) = id) Then Return o.Item
            Next
            Return Nothing
        End Function


        Public Overridable Function GetConnection() As CDBConnection
            SyncLock Me
                If (Me.m_Conn Is Nothing) Then
                    Dim cursor As DBObjectCursorBase = Me.CreateCursor
                    If (cursor IsNot Nothing) Then
                        Me.m_Conn = cursor.GetConnection
                        cursor.Dispose()
                    End If
                End If
                Return Me.m_Conn
            End SyncLock
        End Function


        Protected Overridable Function GetNonCachedItemById(ByVal id As Integer) As T
#If UseGetNonCachedItemById Then
            Dim conn As CDBConnection = Me.GetConnection
            Return conn.GetItemById(Me.Module, id)
#Else
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                If (id = 0) Then Return Nothing

                Dim item As T = Sistema.Types.CreateInstance(GetType(T))

                Dim conn As CDBConnection = DBUtils.GetConnection(item)

                Dim dbSQL As New System.Text.StringBuilder
                dbSQL.Append("SELECT * FROM [")
                dbSQL.Append(DBUtils.GetTableName(item))
                dbSQL.Append("] WHERE [ID]=")
                dbSQL.Append(DBUtils.DBNumber(id))

                dbRis = conn.ExecuteReader(dbSQL.ToString)

                If (dbRis.Read) Then
                    conn.Load(item, dbRis)
                Else
                    If (TypeOf (item) Is IDisposable) Then DirectCast(item, IDisposable).Dispose()
                    item = Nothing
                End If

                Return item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
#End If
        End Function

        Protected Overridable Sub AddToCache(ByVal item As T)
            If (Me.m_CacheSize = 0) Then Exit Sub
            Me.m_CachedItems.Add(New CacheItem(item))
            If (Me.m_CacheSize > 0) Then
                While (Me.m_CachedItems.Count > Me.m_CacheSize)
                    Me.m_CachedItems.RemoveAt(0)
                End While
            End If
            Me.m_AllItems = Nothing
        End Sub

        Protected Overridable Sub AddToCache1(ByVal item As T)
            If (Me.m_CacheSize = 0) Then Exit Sub
            Me.m_CachedItems.Add(New CacheItem(item))
            If (Me.m_CacheSize > 0) Then
                While (Me.m_CachedItems.Count > Me.m_CacheSize)
                    Me.m_CachedItems.RemoveAt(0)
                End While
            End If
        End Sub

        ''' <summary>
        ''' Aggiorna la cache sulla base dello stato dell'oggetto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Public Overridable Sub UpdateCached(ByVal item As T)
            SyncLock Me.cacheLock
                If (Me.m_CacheSize = 0) Then Exit Sub

                For i As Integer = 0 To Me.m_CachedItems.Count - 1
                    Dim o As CacheItem = Me.m_CachedItems(i)
                    If (GetID(o.Item) = GetID(item)) Then
                        Dim oldItem As T = o.Item
                        oldItem.InitializeFrom(item)
                        item = oldItem
                        Me.m_CachedItems.RemoveAt(i)
                        Exit For
                    End If
                Next


                If (TypeOf (item) Is IDBObject) Then
                    If (DirectCast(item, IDBObject).Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_CachedItems.Add(New CacheItem(item))
                    End If
                ElseIf (GetID(item) <> 0) Then
                    Me.m_CachedItems.Add(New CacheItem(item))
                End If

                If (Me.m_CacheSize > 0) Then
                    While (Me.m_CachedItems.Count > Me.m_CacheSize)
                        Me.m_CachedItems.RemoveAt(0)
                    End While
                End If

                'If (Me.m_AllItems IsNot Nothing) Then
                '    For i As Integer = 0 To Me.m_AllItems.Count - 1
                '        If (GetID(Me.m_AllItems(i)) = GetID(item)) Then
                '            Dim oldItem As T = Me.m_AllItems(i)
                '            oldItem.InitializeFrom(item)
                '            Me.m_CachedItems.RemoveAt(i)
                '            Exit For
                '        End If
                '    Next
                'End If
                Me.m_AllItems = Nothing
            End SyncLock
        End Sub

        Public Overridable Sub RemoveCached(ByVal item As T)
            SyncLock Me.cacheLock
                If (Me.m_CacheSize = 0) Then Exit Sub
                For i As Integer = 0 To Me.m_CachedItems.Count - 1
                    Dim o As CacheItem = Me.m_CachedItems(i)
                    If (GetID(o.Item) = GetID(item)) Then
                        Me.m_CachedItems.RemoveAt(i)
                        Exit For
                    End If
                Next
                Me.m_AllItems = Nothing
            End SyncLock
        End Sub


        Public Overridable Function LoadAll() As CCollection(Of T)
            SyncLock Me.cacheLock
                If (Me.m_AllItems IsNot Nothing) Then Return Me.m_AllItems
                If (Me.m_CacheSize <> 0) Then
                    Dim cursor As DBObjectCursorBase = Nothing
#If Not DEBUG Then
                    try
#End If
                    cursor = Me.CreateCursor
                    cursor.PageSize = 10000
                    If (TypeOf (cursor) Is DBObjectCursor) Then
                        DirectCast(cursor, DBObjectCursor).Stato.Value = ObjectStatus.OBJECT_VALID
                    End If
                    cursor.IgnoreRights = True
                    While Not cursor.EOF AndAlso (Me.m_CacheSize < 0 OrElse Me.m_CachedItems.Count < Me.m_CacheSize)
                        Dim item1 As T = Me.GetCachedItemById(GetID(cursor.Item))
                        If (item1 Is Nothing) Then Me.AddToCache1(cursor.Item)
                        'Me.UpdateCached(cursor.Item)
                        cursor.MoveNext()
                    End While

#If Not DEBUG Then
                    Catch ex As Exception
                    throw
                    Finally
#End If
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
                    End Try
#End If
                End If

                Me.m_AllItems = New CCollection(Of T)
                For i As Integer = 0 To Me.m_CachedItems.Count - 1
                    Me.m_AllItems.Add(Me.m_CachedItems(i).Item)
                Next

                If (GetType(IComparable).IsAssignableFrom(GetType(T))) Then Me.m_AllItems.Sort()
                Return Me.m_AllItems
            End SyncLock
        End Function



        Protected ReadOnly Property CachedItems As CCollection(Of CacheItem)
            Get
                Return Me.m_CachedItems
            End Get
        End Property

        Protected Friend Sub doItemCreated(ByVal e As ItemEventArgs)
            RaiseEvent ItemCreated(Me, e)
        End Sub

        Protected Friend Sub doItemDeleted(ByVal e As ItemEventArgs)
            RaiseEvent ItemDeleted(Me, e)
        End Sub

        Protected Friend Sub doItemModified(ByVal e As ItemEventArgs)
            RaiseEvent ItemModified(Me, e)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class