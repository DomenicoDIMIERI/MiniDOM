Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Text.RegularExpressions
Imports minidom
Imports minidom.Sistema
Imports minidom.WebSite
Imports minidom.Databases
Imports minidom.Anagrafica
Imports DMD.XLS

Partial Class WebSite

    Public Class WebServiceEx
        Inherits WebPageEx

        Public Event MessageLogged(ByVal sender As Object, ByVal message As String)
        'Private m_FunName As String
        ''Public Shared WebServicesStats As New CKeyCollection(Of StatsItem)
        'Private m_SInfo As StatsItem
        Private m_Support As ModuleSupportFlags

        Public Sub New()
        End Sub

        Public Sub New(ByVal support As ModuleSupportFlags)
            Me.New
            Me.m_Support = support

        End Sub

        'Protected Overrides Sub StartStatsInfo()
        '    Me.m_FunName = Trim(Me.CurrentPage.PageName & "." & Me.GetParameter("_m", "")) & "." & Trim(Me.GetParameter("_a", ""))
        '    m_SInfo = Me.AC.GetCurrentSessionInfo.BeginWebService(Me.m_FunName)
        '    'Me.m_StartTime = Now
        '    'SyncLock pagesLock
        '    '    Dim item As StatsItem = WebServicesStats.GetItemByKey(Me.m_FunName)
        '    '    If (item Is Nothing) Then
        '    '        item = New StatsItem
        '    '        item.Name = Me.m_FunName
        '    '        WebServicesStats.Add(Me.m_FunName, item)
        '    '    End If
        '    '    item.ActiveCount += 1
        '    '    item.Count += 1
        '    'End SyncLock

        'End Sub

        Protected Overrides Sub OnPreInit(e As EventArgs)
            'Me.Response.ContentEncoding = System.Text.Encoding.ASCII
            MyBase.OnPreInit(e)
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            MyBase.OnLoad(e)
        End Sub

        'Protected Overrides Sub EndStatsInfo()
        '    'Dim item As StatsItem
        '    'SyncLock pagesLock
        '    '    item = WebServicesStats.GetItemByKey(Me.m_FunName)
        '    'End SyncLock
        '    'If (item IsNot Nothing) Then
        '    '    Dim exeTime As Double = (Now - Me.m_StartTime).TotalMilliseconds
        '    '    item.ActiveCount -= 1
        '    '    item.MaxExecTime = Math.Max(item.MaxExecTime, exeTime)
        '    '    item.ExecTime += exeTime
        '    'End If
        '    Me.AC.GetCurrentSessionInfo.EndWebService(Me.m_SInfo)
        'End Sub

        Public Overridable Function CreateCursor() As DBObjectCursorBase
            If (Me.CurrentModule Is Nothing) Then Return Nothing
            Return Me.CurrentModule.CreateCursor()
        End Function

        Public Overridable Function GetInternalItemById(id As Integer) As Object
            Using cursor As DBObjectCursorBase = Me.CreateCursor
                cursor.ID.Value = id
                cursor.PageSize = 1
                cursor.IgnoreRights = True
                Return cursor.Item
            End Using
        End Function

        ''' <summary>
        ''' Restituisce vero se l'utente corrente può eseguire la configurazione del modulo 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function CanConfigure() As Boolean
            Return Me.CurrentModule.UserCanDoAction("configure")
        End Function


        Public Overridable Function CanLavorare(ByVal item As Object) As Boolean
            Return Me.CurrentModule.UserCanDoAction("lavorare")
        End Function

        Public Overridable Function CanCreate(ByVal item As Object) As Boolean
            Return Me.CurrentModule.UserCanDoAction("create")
        End Function

        Public Overridable Function CanTransfer(ByVal item As Object) As Boolean
            Dim ret As Boolean
            ret = Me.CurrentModule.UserCanDoAction("transfer")
            If (TypeOf (item) Is IDBObject) Then
                ret = Me.CurrentModule.UserCanDoAction("transfer_own") And (DirectCast(item, IDBObject).CreatoDaId = GetID(Sistema.Users.CurrentUser))
            End If
            If (ret = False) Then
                If ((TypeOf (item) Is IDBPOObject) AndAlso Me.CurrentModule.UserCanDoAction("transfer_office")) Then
                    Dim po As CUfficio = DirectCast(item, IDBPOObject).PuntoOperativo
                    ret = (po Is Nothing) OrElse
                           (Sistema.Users.CurrentUser.Uffici.HasOffice(po))
                End If
            End If
            Return ret
        End Function
        Protected Overrides Sub OnUnload(e As EventArgs)
            MyBase.OnUnload(e)
            'SyncLock FunctionStats
            '    Dim item As StatsItem = FunctionStats.GetItemByKey(Me.m_FunName)
            '    If (item IsNot Nothing) Then
            '        Dim exeTime As Double = (Now - Me.m_StartTime).TotalMilliseconds
            '        item.ActiveCount -= 1
            '        item.MaxExecTime = Math.Max(item.MaxExecTime, exeTime)
            '        item.ExecTime += exeTime
            '    End If
            'End SyncLock

        End Sub



        'I webservice hanno tempi di esecuzione più brevi
        Protected Overrides Function GetDefaultTimeOut() As Integer
            Dim ret As Integer = WebSite.Instance.Configuration.ShortTimeOut
            If (ret <= 1) Then ret = 30
            Return ret
        End Function



        Protected Overrides Sub ReadCookies()
            'MyBase.ReadCookies()

        End Sub

        Protected Overrides Sub WriteCookies()
            'MyBase.WriteCookies()
        End Sub


        Protected Overrides Sub SecurityCheckMaintenance()
            If WebSite.Instance.IsMaintenance() OrElse minidom.Sistema.FileSystem.FileExists(Server.MapPath("/maintenance.html")) Then
                Throw New InvalidOperationException("Sistema in manutenzione")
            End If
        End Sub

        'Protected Overrides Sub InternalRender(writer As HtmlTextWriter)
        '    Dim ret As New System.Text.StringBuilder
        '    Try
        '        ret.Append(Me.ExecuteAction(Me.RequestedAction, Me))
        '        Me.SanitarizeResponse(ret)
        '        writer.Write("00")
        '        writer.Write(ret.ToString)
        '    Catch ex As Exception
        '        If (TypeOf (ex) Is System.Reflection.TargetInvocationException) AndAlso (ex.InnerException IsNot Nothing) Then ex = ex.InnerException
        '        ret.Append(TypeName(ex))
        '        ret.Append(vbNewLine)
        '        ret.Append(ex.Message)
        '        Me.SanitarizeResponse(ret) '& vbNewLine & ex.StackTrace)
        '        writer.Write("01")
        '        writer.Write(ret.ToString)
        '    End Try
        'End Sub

        Protected Overrides Sub InternalRender(writer As HtmlTextWriter)
            Dim ret As New System.Text.StringBuilder
            If Me.AC.IsDebug Then
                ret.Append(Me.ExecuteAction(Me.RequestedAction, Me))
                Me.SanitarizeResponse(ret)
                writer.Write("00")
                writer.Write(ret.ToString)
            Else
                Try
                    ret.Append(Me.ExecuteAction(Me.RequestedAction, Me))
                    Me.SanitarizeResponse(ret)
                    writer.Write("00")
                    writer.Write(ret.ToString)
                Catch ex As Exception
                    If (TypeOf (ex) Is System.Reflection.TargetInvocationException) AndAlso (ex.InnerException IsNot Nothing) Then ex = ex.InnerException
                    ret.Append(TypeName(ex))
                    ret.Append(vbNewLine)
                    ret.Append(ex.Message)
                    Me.SanitarizeResponse(ret) '& vbNewLine & ex.StackTrace)
                    writer.Write("01")
                    writer.Write(ret.ToString)
                End Try
            End If
        End Sub

        Protected Overridable Function SanitarizeResponse(ByVal text As System.Text.StringBuilder) As System.Text.StringBuilder
            text.Replace(ChrW(65535), "?")
            Return text
        End Function

        '''' <summary>
        '''' Metodo richiamato per i WebServices
        '''' </summary>
        '''' <param name="actionName"></param>
        '''' <param name="context"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Protected Overridable Function ExecuteAction(ByVal actionName As String, ByVal context As Object) As String
        '    If (Me.CurrentModule Is Nothing) Then Throw New ArgumentNullException("Modulo")
        '    Me.ValidateActionBeforeRun(actionName, context)
        '    Return Me.CurrentModule.ExecuteAction(actionName, context)
        'End Function


        Protected Overridable Sub CheckActionToLog(ByVal actionName As String)
            'Me.ApplicationContext.Log(Formats.FormatUserDateTime(Calendar.Now()) & " " & Request.ServerVariables("REMOTE_ADDR") & " " & Users.CurrentUser.Nominativo & " - websvcf." & actionName & "?" & Me.ClientQueryString)
        End Sub



        Protected Overridable Function ExecuteAction(ByVal actionName As String, ByVal context As Object) As String
            'Verifica se l'azione richiede di essere tracciata
            Me.CheckActionToLog(actionName)

            'Esegue l'azione
            actionName = Trim(actionName)
            Select Case LCase(actionName)
                Case "" : Throw New InvalidOperationException("Azione non valida")
                Case Else
                    Dim m As System.Reflection.MethodInfo
                    m = Me.GetType.GetMethod(actionName, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance, Nothing, New System.Type() {}, Nothing)
                    If (m Is Nothing) Then
                        Me.ApplicationContext.Log(Formats.FormatUserDateTime(DateUtils.Now()) & " - AZIONE NON VALIDA - websvcf." & actionName & "?" & Me.ClientQueryString)
                        Throw New NotImplementedException("Servizio non implementato [" & actionName & "]")
                    End If
                    Return m.Invoke(Me, Nothing)
            End Select
        End Function



        Public Overrides Function IsLogEnabled() As Boolean
            Return False  ' MyBase.IsLogEnabled()
        End Function

        Public Function GetWebServicesStats() As String
            Dim col As New CCollection(Of StatsItem)
            'SyncLock Me.AC.applicationLock
            '    For Each Session As CSessionInfo In Me.AC.GetAllSessions
            '        For Each info As StatsItem In Session.WebServicesInfo
            '            col.Add(info)
            '        Next
            '    Next
            'End SyncLock
            Dim items As StatsItem() = col.ToArray
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.Serialize(items)
            Else
                Return ""
            End If
        End Function

        Public Function GetPageStats() As String
            Dim col As New CCollection(Of StatsItem)
            'SyncLock Me.AC.applicationLock
            '    For Each Session As CSessionInfo In Me.AC.GetAllSessions
            '        For Each info As StatsItem In Session.PagesInfo
            '            col.Add(info)
            '        Next
            '    Next
            'End SyncLock
            Dim items As StatsItem() = col.ToArray
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.Serialize(items)
            Else
                Return ""
            End If
        End Function


#Region "Module"


        Public Overridable Function GetItemById() As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter("ID", ""))
            Dim ret As Object = Me.GetInternalItemById(itemID)
            If ret Is Nothing Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(ret)
            End If
        End Function

        Public Overridable Function GetItemsById() As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim ret As New CCollection
            Try
                Dim ids As String = RPC.n2str(Me.GetParameter("ids", ""))
                Dim strids As String() = Sistema.Strings.Split(ids, ",")
                Dim items() As Integer = {}
                If (Arrays.Len(strids)) > 0 Then
                    For Each str As String In strids
                        Dim id As Integer = Formats.ToInteger(str)
                        If (id <> 0) Then items = Arrays.Append(items, id)
                    Next
                End If
                If (Arrays.Len(items) > 0) Then
                    cursor = Me.CreateCursor
                    cursor.ID.ValueIn(items)
                    While Not cursor.EOF
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                End If

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Overridable Function GetInternalItemById1(ByVal id As Integer) As Object
            If (id = 0) Then Return Nothing
            Dim cursor As DBObjectCursorBase = Me.CreateCursor
            Dim tn As String = DBUtils.GetTableName(cursor)
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As Object = Nothing
            Dim conn As CDBConnection = cursor.Connection
            Try
                'Dim t As Date = Calendar.Now
                dbRis = conn.ExecuteReader("SELECT * FROM [" & tn & "] WHERE [ID]=" & id)
                'Dim t1 As Date = Calendar.Now
                'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> ExecuteReader : " & ((t1 - t).TotalMilliseconds / 1000) & " s")
                If dbRis.Read Then
                    'Dim t3 As Date = Calendar.Now
                    'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> Read : " & ((t3 - t1).TotalMilliseconds / 1000) & " s")
                    Dim reader As New DBReader(dbRis)
                    'Dim t4 As Date = Calendar.Now
                    'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> new DBReader : " & ((t4 - t3).TotalMilliseconds / 1000) & " s")
                    ret = cursor.InstantiateNew(reader)
                    conn.Load(ret, reader)
                    reader.Dispose()
                    dbRis = Nothing
                    'Dim t5 As Date = Calendar.Now
                    'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> new Load : " & ((t5 - t4).TotalMilliseconds / 1000) & " s")
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                cursor.Dispose()
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try


        End Function

#Region "List"

        Public Overridable Function list() As String
            Return vbNullString
        End Function

#End Region

#Region "Create"

        Public Overridable ReadOnly Property SupportsCreate As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SCreate) = ModuleSupportFlags.SCreate
            End Get
        End Property

        Public Overridable ReadOnly Property SupportsDuplicate As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SDuplicate) = ModuleSupportFlags.SDuplicate
            End Get
        End Property



        'Public Overridable Function create() As String
        '    Dim cursor As DBObjectCursorBase
        '    Dim item As DBObjectBase
        '    Dim fromID As Integer
        '    If (Me.CanCreate = False) Then Throw New PermissionDeniedException(Me.CurrentModule, "create")
        '    fromID = Formats.ToInteger(RPC.n2int(Me.GetParameter("ID", "")))
        '    cursor = Me.CreateCursor
        '    item = cursor.Add
        '    If (fromID <> 0) AndAlso (TypeOf (item) Is ISupportInitializeFrom) Then DirectCast(item, ISupportInitializeFrom).InitializeFrom(Me.GetInternalItemById(fromID))
        '    'e = New Sistema.SystemEvent
        '    'item.OnCreate", "Sub", e)
        '    item.Save()
        '    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '    Return Me.InternalEdit(item)
        'End Function

#End Region

#Region "Delete"

        Public Overridable ReadOnly Property SupportsDelete As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SDelete) = ModuleSupportFlags.SDelete
            End Get
        End Property

        Public Overridable Function CanDelete(ByVal item As Object) As Boolean
            Dim ret As Boolean
            ret = Me.CurrentModule.UserCanDoAction("delete")
            If (ret = False) AndAlso (TypeOf (item) Is IDBObject) Then
                With DirectCast(item, IDBObject)
                    ret = Me.CurrentModule.UserCanDoAction("delete_own") And (.CreatoDaId = GetID(Users.CurrentUser))
                End With
            End If
            If (ret = False) AndAlso (TypeOf (item) Is IDBPOObject) Then
                With DirectCast(item, IDBPOObject)
                    If Me.CurrentModule.UserCanDoAction("delete_office") Then
                        ret = (.PuntoOperativo Is Nothing) OrElse (Users.CurrentUser.Uffici.HasOffice(.PuntoOperativo))
                    End If
                End With
            End If
            Return ret
        End Function


        Public Overridable Function delete() As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter("ID", ""))
            Dim item As DBObjectBase = Me.GetInternalItemById(itemID)
            If Me.CanDelete(item) Then
                Dim cursor As DBObjectCursorBase = Me.CreateCursor
                If TypeOf (item) Is IDBObject Then
                    DirectCast(item, IDBObject).Stato = ObjectStatus.OBJECT_DELETED
                    item.Save()
                Else
                    item.Delete()
                End If
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                Return vbNullString
            Else
                Throw New PermissionDeniedException
                Return vbNullString
            End If
        End Function

#End Region

#Region "Edit"

        Public Overridable ReadOnly Property SupportsEdit As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SEdit) = ModuleSupportFlags.SEdit
            End Get
        End Property

        Public Overridable ReadOnly Property SupportsAnnotations As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SAnnotations) = ModuleSupportFlags.SAnnotations
            End Get
        End Property

        Public Overridable Function CanEdit(ByVal item As Object) As Boolean
            Dim ret As Boolean
            ret = Me.CurrentModule.UserCanDoAction("edit")
            If (ret = False) AndAlso (TypeOf (item) Is IDBObject) Then
                With DirectCast(item, IDBObject)
                    ret = Me.CurrentModule.UserCanDoAction("edit_own") And (.CreatoDaId = GetID(Users.CurrentUser))
                End With
            End If
            If (ret = False) AndAlso (TypeOf (item) Is IDBPOObject) Then
                With DirectCast(item, IDBPOObject)
                    If Me.CurrentModule.UserCanDoAction("edit_office") Then
                        ret = (.PuntoOperativo Is Nothing) OrElse (Users.CurrentUser.Uffici.HasOffice(.PuntoOperativo))
                    End If
                End With
            End If
            Return ret
        End Function

        Public Overridable Function CanList() As Boolean
            Dim ret As Boolean
            ret = Me.CurrentModule.UserCanDoAction("list") OrElse Me.CurrentModule.UserCanDoAction("edit_office") OrElse Me.CurrentModule.UserCanDoAction("edit_own")
            Return ret
        End Function

        Public Overridable Function CanList(ByVal item As Object) As Boolean
            Dim ret As Boolean
            ret = Me.CurrentModule.UserCanDoAction("list")
            If (ret = False AndAlso TypeOf (item) Is IDBObject) Then
                ret = Me.CurrentModule.UserCanDoAction("edit_own") AndAlso DirectCast(item, IDBObject).CreatoDaId = GetID(Sistema.Users.CurrentUser)
            End If
            If (ret = False AndAlso TypeOf (item) Is IDBPOObject) Then
                ret = Me.CurrentModule.UserCanDoAction("edit_office") AndAlso Anagrafica.Uffici.GetPuntiOperativiConsentiti.GetItemById(DirectCast(item, IDBPOObject).IDPuntoOperativo) IsNot Nothing
            End If
            Return ret
        End Function


        Public Overridable Function edit() As String
            Dim itemID As Integer = Formats.ToInteger(RPC.n2int(Me.GetParameter("ID", "")))
            Dim itemObj As Object = Me.GetInternalItemById(itemID)
            If Not Me.CanEdit(itemObj) Then
                Throw New PermissionDeniedException(Me.CurrentModule, "edit")
            End If
            'If itemObj Is Nothing Then
            '    Throw New ArgumentNullException("Non trovo alcun elemeneto corrispondente all'ID: " & itemID)
            'End If
            'Return Me.InternalEdit(itemObj)
            'Dim editor As BaseModuleEditor = Me.GetEditor
            'With editor
            '    .UseLocal = True
            '    .Width = Me.m_.Width
            '    .Height = Me.m_.Height
            '    .Name = "ctrlEditor"
            '    .Visible = False
            '    .DoLayout()
            'End With
            'Dim writer As New HTMLWriter
            'editor.CreateHTML(writer)

            Return Me.InternalEdit(itemObj)
        End Function


        Public Overridable Function CanPrint(ByVal item As Object) As Boolean
            Return Me.CurrentModule.UserCanDoAction("print")
        End Function

        Public Overridable Function print() As String
            Dim html As String = Me.ExecuteAction("edit", Me)
            html &= "<script type=""text-javascript"">" & vbNewLine & "Window.addListener(""onload"", new Function('window.print()'));" & vbNewLine & "</script>"
            Return html
        End Function


        Protected Overridable Function InternalEdit(value As DBObjectBase) As String
            'Dim tabControl As CTabEditor
            'Dim writer As New HTMLWriter
            'Dim itemID As Integer
            'Dim message As String = vbNullString

            'tabControl = Me.CreateTabEditor(value)
            'itemID = GetID(value)

            'If Not Me.CanEdit(value) Then
            '    message = "Diritti insufficienti"
            'Else
            '    tabControl.Item = value

            '    Select Case Me.GetParameter("_s")
            '        Case "1", "2", "3", "4", "5"
            '            Me.OnBeforeSave(value)

            '            If tabControl.Save Then
            '                If TypeOf (value) Is IDBObject Then
            '                    If DirectCast(value, IDBObject).Stato = ObjectStatus.OBJECT_TEMP Then
            '                        DirectCast(value, IDBObject).Stato = ObjectStatus.OBJECT_VALID
            '                    End If
            '                End If
            '                value.Save()
            '            Else
            '                message = "Errore sconosciuto"
            '            End If

            '            Me.OnAfterSave(value)
            '    End Select
            'End If

            'tabControl.CreateHTML(writer)
            'writer.WriteRowData("<input type=""hidden"" name=""ID"" id=""ID"" value=""" & itemID & """ />")
            'If GetParameter("strsrc") = "" Then writer.WriteRowData("<input type=""hidden"" name=""strsrc"" id=""strsrc"" value=""" & Me.GetParameter("strsrc") & """ />")

            'If message <> "" Then
            '    writer.WriteRowData("<script language=""javascript"" type=""text/javascript"">" & vbNewLine)
            '    writer.WriteRowData("alert(""" & message & """);")
            '    writer.WriteRowData("</script>")
            'End If

            'Dim ret As String = writer.ToString
            'writer.Dispose()
            'Return ret

            Dim ret As String = "" 'Me.list()
            ret &= "<script type=""text/javascript"">" & vbNewLine
            ret &= "Window.addListener(""onload"", new Function('setTimeout(SystemUtils.EditModuleItem(" & GetID(Me.CurrentModule) & ", " & GetID(value) & "), 500)'));"
            ret &= "</script>"

            Return ret
        End Function

        Protected Overridable Sub OnBeforeSave(ByVal item As Object)

        End Sub

        Protected Overridable Sub OnAfterSave(ByVal item As Object)

        End Sub

#End Region

#Region "Print"

        Public Overridable ReadOnly Property SupportsPrint As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SPrint) = ModuleSupportFlags.SPrint
            End Get
        End Property

#End Region

#Region "Export"

        ''' <summary>
        ''' Quando sottoposto ad override in una classe derivata restituisce un valore booleano che indica se il modulo supporta l'esportazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property SupportsExport As Boolean
            Get
                Return ((Me.m_Support And ModuleSupportFlags.SExport) = ModuleSupportFlags.SExport) Or (Me.GetExportableColumnsList.Count > 0)
            End Get
        End Property

        Public Overridable Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As New CCollection(Of ExportableColumnInfo) '= Me.CurrentModuleHandler.GetExportableColumnList()
            'ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            Return ret
        End Function

        Public Function GetExportableColumns() As String
            Dim ret As CCollection(Of ExportableColumnInfo) = Me.GetExportableColumnsList
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            Else
                Return vbNullString
            End If
        End Function

        Protected Overridable Function GetExportedColumns() As CCollection(Of ExportableColumnInfo)
            Dim text As String = RPC.n2str(Me.GetParameter("listCols", ""))

            Dim ret As CCollection(Of ExportableColumnInfo)
            If (text = "") Then
                ret = Me.GetExportableColumnsList
            Else
                ret = New CCollection(Of ExportableColumnInfo)
                Dim items() As Object = XML.Utils.Serializer.Deserialize(text)
                For Each item As ExportableColumnInfo In items
                    ret.Add(item)
                Next
            End If
            Return ret
        End Function

        Protected Overridable Function GetColumnValue(item As Object, ByVal key As String) As Object
            Dim m As System.Reflection.PropertyInfo = item.GetType().GetProperty(key, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            Return m.GetValue(item, New Object() {})
        End Function

        Public Function CanExport() As Boolean
            Return Me.CurrentModule.UserCanDoAction("export")
        End Function

        Public Overridable Function ExportList() As String
            If Not Me.CanExport Then Throw New PermissionDeniedException(Me.CurrentModule, "export")

            Dim listFormat As String = Sistema.Strings.LCase(Sistema.Strings.Trim(RPC.n2str(GetParameter("listFormat", "xls"))))
            Select Case listFormat
                Case "xls" : Return Me.ExportXlsFormat()
                Case "xlsx" : Return Me.ExportXLSXFormat()
                'Case "xls" : Return Me.ExportFakeXLSFormat()
                Case "mdb" : Return Me.ExportMdbFormat()
                Case "csv" : Return Me.ExportCSVFormat()
                Case "txt" : Return Me.ExportTxtFormat()
                Case Else
                    Throw New NotSupportedException("Il formato " & listFormat & " non è supportato")
            End Select
        End Function


        Protected Overridable Function ExportXLSXFormat() As String
            Using cursor As DBObjectCursorBase = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
                cursor.Reset1()
                cursor.PageSize = 10000
                'cursor.Count()

                Using wb = New DMD.XLS.XLWorkbook()
                    Dim xlsTable = wb.Worksheets.Add(Me.CurrentModule.ModuleName)
                    Dim cols = Me.GetExportedColumns()

                    Dim dic As New Dictionary(Of String, Integer)
                    Dim dic1 As New Dictionary(Of String, ExportableColumnInfo)
                    For Each col As ExportableColumnInfo In cols
                        If (col.Selected) Then
                            Dim xlsCol = XLSUtils.EnsureXlsColumn(xlsTable, col.Key)
                            dic.Add(col.Key, xlsCol.ColumnNumber())
                            dic1.Add(col.Key, col)
                        End If
                    Next
                    XLSUtils.ApplyDefaultColumnsStyle(xlsTable, XLColor.AliceBlue, True)

                    Dim r As Integer = 1
                    While Not cursor.EOF
                        r += 1
                        For Each key As String In dic.Keys
                            XLSUtils.SetCellValue(xlsTable, r, dic(key), Me.GetColumnValue(cursor.Item, key), dic1(key).TipoValore)
                        Next
                        cursor.MoveNext()
                    End While

                    Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                    Dim fileName As String = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xlsx"
                    wb.SaveAs(Me.AC.MapPath(fileName))

                    Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(DateUtils.Now(), "/", "_"), ":", "_")

                    Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

                    Return url
                End Using
            End Using
        End Function


        Protected Overridable Function ExportXlsFormat() As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim fileName As String = ""
            Dim xlsConn As CXlsDBConnection = Nothing
            Dim xlsTable As CDBTable = Nothing
            Dim cols As CCollection(Of ExportableColumnInfo)
            Dim cmd As System.Data.IDbCommand = Nothing
            Dim param As System.Data.IDbDataParameter = Nothing

            'If (Me.UseLocal) Then
            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
                cursor.Reset1()
                cursor.PageSize = 10000
                'cursor.Count()

                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"

                Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/xlsfile.xls"), Sistema.ApplicationContext.MapPath(fileName), True)
                xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                xlsConn.OpenDB()

                xlsTable = xlsConn.Tables.Add("Tabella")
                'xlsTable = xlsConn.Tables(0)

                cols = Me.GetExportedColumns()
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        xlsTable.Fields.Add(col.Key, col.TipoValore)
                    End If
                Next
                xlsTable.Update()

                cmd = xlsTable.GetInsertCommand
                While Not cursor.EOF
                    ' cmd.Parameters.Clear()

                    For Each col As ExportableColumnInfo In cols
                        If (col.Selected) Then
                            param = cmd.Parameters("@" & col.Key)
                            param.Value = xlsConn.ToDB(Me.GetColumnValue(cursor.Item, col.Key))
                        End If
                    Next
                    cmd.ExecuteNonQuery()
                    cursor.MoveNext()
                End While
                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (xlsConn IsNot Nothing) Then xlsConn.Dispose() : xlsConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

                Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

                Return url
            Catch ex As Exception
                Throw
            Finally
                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (xlsConn IsNot Nothing) Then xlsConn.Dispose() : xlsConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Protected Overridable Function ExportMdbFormat() As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim fileName As String = ""
            Dim outConn As CMdbDBConnection = Nothing
            Dim outTable As CDBTable = Nothing
            Dim cols As CCollection(Of ExportableColumnInfo)
            Dim cmd As System.Data.IDbCommand = Nothing
            Dim param As System.Data.IDbDataParameter = Nothing
            Try

                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
                cursor.Reset1()

                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".mdb"

                Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/template.mdb"), Sistema.ApplicationContext.MapPath(fileName), True)
                outConn = New CMdbDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                outConn.OpenDB()

                outTable = outConn.Tables.Add(Me.CurrentModule.DisplayName)
                'xlsTable = xlsConn.Tables(0)

                cols = Me.GetExportedColumns()
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        outTable.Fields.Add(col.Key, col.TipoValore)
                    End If
                Next
                outTable.Update()

                cmd = outTable.GetInsertCommand
                While Not cursor.EOF
                    For Each col As ExportableColumnInfo In cols
                        If (col.Selected) Then
                            param = cmd.Parameters("@" & col.Key)
                            param.Value = outConn.ToDB(Me.GetColumnValue(cursor.Item, col.Key))
                        End If
                    Next
                    cmd.ExecuteNonQuery()
                    cursor.MoveNext()
                End While

                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (outConn IsNot Nothing) Then outConn.Dispose() : outConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


                Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

                Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))


                Return url
            Catch ex As Exception
                Throw
            Finally
                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (outConn IsNot Nothing) Then outConn.Dispose() : outConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Protected Sub writeCSV(ByVal buffer As System.Text.StringBuilder, ByVal text As String)
            buffer.Append(Chr(34))
            buffer.Append(Replace(text, Chr(34), Chr(34)))
            buffer.Append(Chr(34))
        End Sub

        'Protected Overridable Function ExportFakeXLSFormat() As String
        '    Dim cursor As DBObjectCursorBase
        '    Dim fileName As String
        '    Dim cols As CCollection(Of ExportableColumnInfo)

        '    'If (Me.UseLocal) Then
        '    cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
        '    cursor.Reset1()

        '    FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
        '    fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"


        '    Dim buffer As New System.Text.StringBuilder


        '    cols = Me.GetExportedColumns()
        '    Dim i As Integer = 0
        '    For Each col As ExportableColumnInfo In cols
        '        If (col.Selected) Then
        '            If (i > 0) Then buffer.Append(";")
        '            Me.writeCSV(buffer, col.Key)
        '            i += 1
        '        End If
        '    Next
        '    buffer.Append(vbCrLf)

        '    While Not cursor.EOF
        '        i = 0
        '        For Each col As ExportableColumnInfo In cols
        '            If (col.Selected) Then
        '                If (i > 0) Then buffer.Append(";")
        '                Me.writeCSV(buffer, Formats.ToString(Me.GetColumnValue(cursor.Item, col.Key)))
        '                i += 1
        '            End If
        '        Next
        '        cursor.MoveNext()
        '        buffer.Append(vbCrLf)
        '    End While
        '    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

        '    System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
        '    Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

        '    Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

        '    Return url
        'End Function

        Protected Overridable Function ExportCSVFormat() As String
            Dim cursor As DBObjectCursorBase
            Dim fileName As String
            Dim cols As CCollection(Of ExportableColumnInfo)

            'If (Me.UseLocal) Then
            cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
            cursor.Reset1()

            Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".csv"


            Dim buffer As New System.Text.StringBuilder


            cols = Me.GetExportedColumns()
            Dim i As Integer = 0
            For Each col As ExportableColumnInfo In cols
                If (col.Selected) Then
                    If (i > 0) Then buffer.Append(";")
                    Me.writeCSV(buffer, col.Key)
                    i += 1
                End If
            Next
            buffer.Append(vbCrLf)

            While Not cursor.EOF
                i = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(";")
                        Me.writeCSV(buffer, Formats.ToString(Me.GetColumnValue(cursor.Item, col.Key)))
                        i += 1
                    End If
                Next
                cursor.MoveNext()
                buffer.Append(vbCrLf)
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

            Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

            Return url
        End Function

        Protected Overridable Function ExportTxtFormat() As String
            Dim cursor As DBObjectCursorBase
            Dim fileName As String
            Dim cols As CCollection(Of ExportableColumnInfo)

            'If (Me.UseLocal) Then
            cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
            cursor.Reset1()

            Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".txt"


            Dim buffer As New System.Text.StringBuilder


            cols = Me.GetExportedColumns()
            Dim i As Integer = 0
            For Each col As ExportableColumnInfo In cols
                If (col.Selected) Then
                    If (i > 0) Then buffer.Append(vbTab)
                    buffer.Append(col.Key)
                    i += 1
                End If
            Next
            buffer.Append(vbCrLf)

            While Not cursor.EOF
                i = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(vbTab)
                        buffer.Append(Formats.ToString(Me.GetColumnValue(cursor.Item, col.Key)))
                        i += 1
                    End If
                Next
                cursor.MoveNext()
                buffer.Append(vbCrLf)
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

            Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

            Return url
        End Function
#End Region

#Region "Import"

        ''' <summary>
        ''' Quando sottoposto ad override in una classe derivata restituisce un valore booleano che indica se il modulo supporta l'importazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property SupportsImport As Boolean
            Get
                Return ((Me.m_Support And ModuleSupportFlags.SImport) = ModuleSupportFlags.SImport) Or (Me.GetExportableColumnsList.Count > 0)
            End Get
        End Property

        Protected Overridable Sub SetColumnValue(item As Object, ByVal key As String, ByVal value As Object)
            Select Case key
                Case "ID"
                Case Else
                    Dim m As System.Reflection.PropertyInfo = item.GetType.GetProperty(key, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                    If m.CanWrite = False Then
                        Throw New InvalidConstraintException("Proprietà in sola lettura: " & key)
                    End If
                    Try
                        If TypeOf (value) Is DBNull Then
                            m.SetValue(item, Nothing, New Object() {})
                        Else
                            m.SetValue(item, Types.CastTo(value, m.PropertyType), New Object() {})
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
            End Select

        End Sub

        Protected Overridable Function FindExportedItem(ByVal cursor As DBObjectCursorBase, ByVal info As DBReader) As Object
            Dim ret As Object
            Dim id As Integer = info.GetValue(Of Integer)("ID", 0)
            If (id = 0) Then Return Nothing
            cursor.Reset1()
            If (TypeOf (cursor) Is DBObjectCursor) Then DirectCast(cursor, DBObjectCursor).Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.ID.Value = id
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function CanImport() As Boolean
            Return Me.CurrentModule.UserCanDoAction("import")
        End Function

        Public Overridable Function ImportList() As String
            If Not Me.CanImport Then Throw New PermissionDeniedException(Me.CurrentModule, "import")

            Dim fileName As String
            Dim xlsConn As CXlsDBConnection
            Dim xlsTable As CDBTable
            Dim xlsRis As DBReader
            Dim cols As CCollection(Of ExportableColumnInfo)
            Dim item As DBObjectBase
            Dim cursor As DBObjectCursorBase
            Dim s As Integer

            s = RPC.n2int(Me.GetParameter("s", 0))
            If (s = 0) Then
                Return "L'importazione modificherà i record esistenti ed aggiungerà eventuali record non trovati."
            Else
                fileName = Trim(RPC.n2str(Me.GetParameter("fn", vbNullString)))
                If (fileName = vbNullString) Then Throw New ArgumentNullException("fileName")
                xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                xlsConn.OpenDB()

                xlsTable = xlsConn.Tables("Tabella")
                xlsRis = New DBReader(xlsTable)
                cols = Me.GetExportedColumns()
                cursor = Me.CreateCursor
                While xlsRis.Read
                    item = Me.FindExportedItem(cursor, xlsRis)
                    If (item Is Nothing) Then item = cursor.Add
                    If (TypeOf (item) Is IDBObject) Then DirectCast(item, IDBObject).Stato = ObjectStatus.OBJECT_VALID
                    For Each column As ExportableColumnInfo In cols
                        If xlsRis.Schema.Fields.ContainsKey(column.Key) Then
                            Me.SetColumnValue(item, column.Key, xlsRis.GetValue(column.Key))
                        End If
                    Next
                    item.Save()
                End While
                xlsConn.Dispose()
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Me.CurrentModule.DispatchEvent(New EventDescription("list_imported", "Dati importati dal file " & fileName, Nothing))
            End If

            Return ""
        End Function

#End Region

        Protected Overridable Sub OnMessageLogged(ByVal message As String)
            RaiseEvent MessageLogged(Me, message)
        End Sub

        Public Overridable Function LoadAll() As String
            Dim ret As New CCollection
            Dim cursor As DBObjectCursorBase = Me.CreateCursor
#If Not DEBUG Then
        Try
#End If
            If (TypeOf (cursor) Is DBObjectCursor) Then
                With DirectCast(cursor, DBObjectCursor)
                    .Stato.Value = ObjectStatus.OBJECT_VALID
                End With
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
        Catch ex As Exception
            Throw
        Finally
#End If
            cursor.Dispose()
#If Not DEBUG Then
        End Try
#End If
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

#End Region
    End Class



End Class