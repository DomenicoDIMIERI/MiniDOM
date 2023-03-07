using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using DMD.XLS;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {
        public class WebServiceEx : WebPageEx
        {
            public event MessageLoggedEventHandler MessageLogged;

            public delegate void MessageLoggedEventHandler(object sender, string message);
            // Private m_FunName As String
            // 'Public Shared WebServicesStats As New CKeyCollection(Of StatsItem)
            // Private m_SInfo As StatsItem
            private ModuleSupportFlags m_Support;

            public WebServiceEx()
            {
            }

            public WebServiceEx(ModuleSupportFlags support) : this()
            {
                m_Support = support;
            }

            // Protected Overrides Sub StartStatsInfo()
            // Me.m_FunName = Trim(Me.CurrentPage.PageName & "." & Me.GetParameter("_m", "")) & "." & Trim(Me.GetParameter("_a", ""))
            // m_SInfo = Me.AC.GetCurrentSessionInfo.BeginWebService(Me.m_FunName)
            // 'Me.m_StartTime = Now
            // 'SyncLock pagesLock
            // '    Dim item As StatsItem = WebServicesStats.GetItemByKey(Me.m_FunName)
            // '    If (item Is Nothing) Then
            // '        item = New StatsItem
            // '        item.Name = Me.m_FunName
            // '        WebServicesStats.Add(Me.m_FunName, item)
            // '    End If
            // '    item.ActiveCount += 1
            // '    item.Count += 1
            // 'End SyncLock

            // End Sub

            protected override void OnPreInit(EventArgs e)
            {
                // Me.Response.ContentEncoding = System.Text.Encoding.ASCII
                base.OnPreInit(e);
            }

            protected override void OnLoad(EventArgs e)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.OnLoad(e);
            }

            // Protected Overrides Sub EndStatsInfo()
            // 'Dim item As StatsItem
            // 'SyncLock pagesLock
            // '    item = WebServicesStats.GetItemByKey(Me.m_FunName)
            // 'End SyncLock
            // 'If (item IsNot Nothing) Then
            // '    Dim exeTime As Double = (Now - Me.m_StartTime).TotalMilliseconds
            // '    item.ActiveCount -= 1
            // '    item.MaxExecTime = Math.Max(item.MaxExecTime, exeTime)
            // '    item.ExecTime += exeTime
            // 'End If
            // Me.AC.GetCurrentSessionInfo.EndWebService(Me.m_SInfo)
            // End Sub

            public virtual Databases.DBObjectCursorBase CreateCursor()
            {
                if (CurrentModule is null)
                    return null;
                return CurrentModule.CreateCursor();
            }

            public virtual object GetInternalItemById(int id)
            {
                using (var cursor = CreateCursor())
                {
                    cursor.ID.Value = id;
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }
            }

            /// <summary>
        /// Restituisce vero se l'utente corrente può eseguire la configurazione del modulo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool CanConfigure()
            {
                return CurrentModule.UserCanDoAction("configure");
            }

            public virtual bool CanLavorare(object item)
            {
                return CurrentModule.UserCanDoAction("lavorare");
            }

            public virtual bool CanCreate(object item)
            {
                return CurrentModule.UserCanDoAction("create");
            }

            public virtual bool CanTransfer(object item)
            {
                bool ret;
                ret = CurrentModule.UserCanDoAction("transfer");
                if (item is Databases.IDBObject)
                {
                    ret = CurrentModule.UserCanDoAction("transfer_own") & ((Databases.IDBObject)item).CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
                }

                if (ret == false)
                {
                    if (item is Databases.IDBPOObject && CurrentModule.UserCanDoAction("transfer_office"))
                    {
                        var po = ((Databases.IDBPOObject)item).PuntoOperativo;
                        ret = po is null || Sistema.Users.CurrentUser.Uffici.HasOffice(po);
                    }
                }

                return ret;
            }

            protected override void OnUnload(EventArgs e)
            {
                base.OnUnload(e);
                // SyncLock FunctionStats
                // Dim item As StatsItem = FunctionStats.GetItemByKey(Me.m_FunName)
                // If (item IsNot Nothing) Then
                // Dim exeTime As Double = (Now - Me.m_StartTime).TotalMilliseconds
                // item.ActiveCount -= 1
                // item.MaxExecTime = Math.Max(item.MaxExecTime, exeTime)
                // item.ExecTime += exeTime
                // End If
                // End SyncLock

            }



            // I webservice hanno tempi di esecuzione più brevi
            protected override int GetDefaultTimeOut()
            {
                int ret = Instance.Configuration.ShortTimeOut;
                if (ret <= 1)
                    ret = 30;
                return ret;
            }

            protected override void ReadCookies()
            {
                // MyBase.ReadCookies()

            }

            protected override void WriteCookies()
            {
                // MyBase.WriteCookies()
            }

            protected override void SecurityCheckMaintenance()
            {
                if (Instance.IsMaintenance() || Sistema.FileSystem.FileExists(Server.MapPath("/maintenance.html")))
                {
                    throw new InvalidOperationException("Sistema in manutenzione");
                }
            }

            // Protected Overrides Sub InternalRender(writer As HtmlTextWriter)
            // Dim ret As New System.Text.StringBuilder
            // Try
            // ret.Append(Me.ExecuteAction(Me.RequestedAction, Me))
            // Me.SanitarizeResponse(ret)
            // writer.Write("00")
            // writer.Write(ret.ToString)
            // Catch ex As Exception
            // If (TypeOf (ex) Is System.Reflection.TargetInvocationException) AndAlso (ex.InnerException IsNot Nothing) Then ex = ex.InnerException
            // ret.Append(TypeName(ex))
            // ret.Append(vbNewLine)
            // ret.Append(ex.Message)
            // Me.SanitarizeResponse(ret) '& vbNewLine & ex.StackTrace)
            // writer.Write("01")
            // writer.Write(ret.ToString)
            // End Try
            // End Sub

            protected override void InternalRender(HtmlTextWriter writer)
            {
                var ret = new System.Text.StringBuilder();
                if (AC.IsDebug())
                {
                    ret.Append(ExecuteAction(RequestedAction, this));
                    SanitarizeResponse(ret);
                    writer.Write("00");
                    writer.Write(ret.ToString());
                }
                else
                {
                    try
                    {
                        ret.Append(ExecuteAction(RequestedAction, this));
                        SanitarizeResponse(ret);
                        writer.Write("00");
                        writer.Write(ret.ToString());
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Reflection.TargetInvocationException && ex.InnerException is object)
                            ex = ex.InnerException;
                        ret.Append(DMD.RunTime.vbTypeName(ex));
                        ret.Append(DMD.Strings.vbNewLine);
                        ret.Append(ex.Message);
                        SanitarizeResponse(ret); // & vbNewLine & ex.StackTrace)
                        writer.Write("01");
                        writer.Write(ret.ToString());
                    }
                }
            }

            protected virtual System.Text.StringBuilder SanitarizeResponse(System.Text.StringBuilder text)
            {
                text.Replace(DMD.Strings.CStr('\uffff'), "?");
                return text;
            }

            // ''' <summary>
            // ''' Metodo richiamato per i WebServices
            // ''' </summary>
            // ''' <param name="actionName"></param>
            // ''' <param name="context"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Protected Overridable Function ExecuteAction(ByVal actionName As String, ByVal context As Object) As String
            // If (Me.CurrentModule Is Nothing) Then Throw New ArgumentNullException("Modulo")
            // Me.ValidateActionBeforeRun(actionName, context)
            // Return Me.CurrentModule.ExecuteAction(actionName, context)
            // End Function


            protected virtual void CheckActionToLog(string actionName)
            {
                // Me.ApplicationContext.Log(Formats.FormatUserDateTime(Calendar.Now()) & " " & Request.ServerVariables("REMOTE_ADDR") & " " & Users.CurrentUser.Nominativo & " - websvcf." & actionName & "?" & Me.ClientQueryString)
            }

            protected virtual string ExecuteAction(string actionName, object context)
            {
                // Verifica se l'azione richiede di essere tracciata
                CheckActionToLog(actionName);

                // Esegue l'azione
                actionName = Strings.Trim(actionName);
                switch (Strings.LCase(actionName) ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            throw new InvalidOperationException("Azione non valida");
                            break;
                        }

                    default:
                        {
                            System.Reflection.MethodInfo m;
                            m = GetType().GetMethod(actionName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new Type[] { }, null);
                            if (m is null)
                            {
                                ApplicationContext.Log(Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " - AZIONE NON VALIDA - websvcf." + actionName + "?" + ClientQueryString);
                                throw new NotImplementedException("Servizio non implementato [" + actionName + "]");
                            }

                            return DMD.Strings.CStr(m.Invoke(this, null));
                        }
                }
            }

            public override bool IsLogEnabled()
            {
                return false;  // MyBase.IsLogEnabled()
            }

            public string GetWebServicesStats()
            {
                var col = new CCollection<StatsItem>();
                // SyncLock Me.AC.applicationLock
                // For Each Session As CSessionInfo In Me.AC.GetAllSessions
                // For Each info As StatsItem In Session.WebServicesInfo
                // col.Add(info)
                // Next
                // Next
                // End SyncLock
                var items = col.ToArray();
                if (DMD.Arrays.Len(items) > 0)
                {
                    return DMD.XML.Utils.Serializer.Serialize(items);
                }
                else
                {
                    return "";
                }
            }

            public string GetPageStats()
            {
                var col = new CCollection<StatsItem>();
                // SyncLock Me.AC.applicationLock
                // For Each Session As CSessionInfo In Me.AC.GetAllSessions
                // For Each info As StatsItem In Session.PagesInfo
                // col.Add(info)
                // Next
                // Next
                // End SyncLock
                var items = col.ToArray();
                if (DMD.Arrays.Len(items) > 0)
                {
                    return DMD.XML.Utils.Serializer.Serialize(items);
                }
                else
                {
                    return "";
                }
            }


            /* TODO ERROR: Skipped RegionDirectiveTrivia */

            public virtual string GetItemById()
            {
                int itemID = (int)Sistema.RPC.n2int(GetParameter("ID", ""));
                var ret = GetInternalItemById(itemID);
                if (ret is null)
                {
                    return "";
                }
                else
                {
                    return DMD.XML.Utils.Serializer.Serialize(ret);
                }
            }

            public virtual string GetItemsById()
            {
                Databases.DBObjectCursorBase cursor = null;
                var ret = new CCollection();
                try
                {
                    string ids = Sistema.RPC.n2str(GetParameter("ids", ""));
                    var strids = DMD.Strings.Split(ids, ",");
                    var items = DMD.Arrays.Empty<int>();
                    if (DMD.Arrays.Len(strids) > 0)
                    {
                        foreach (string str in strids)
                        {
                            int id = Sistema.Formats.ToInteger(str);
                            if (id != 0)
                                items = DMD.Arrays.Append(items, id);
                        }
                    }

                    if (DMD.Arrays.Len(items) > 0)
                    {
                        cursor = CreateCursor();
                        cursor.ID.ValueIn(items);
                        while (!cursor.EOF())
                        {
                            ret.Add(cursor.Item);
                            cursor.MoveNext();
                        }
                    }

                    return DMD.XML.Utils.Serializer.Serialize(ret);
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }

            public virtual object GetInternalItemById1(int id)
            {
                if (id == 0)
                    return null;
                var cursor = CreateCursor();
                string tn = Databases.DBUtils.GetTableName(cursor);
                IDataReader dbRis = null;
                object ret = null;
                var conn = cursor.Connection;
                try
                {
                    // Dim t As Date = Calendar.Now
                    dbRis = conn.ExecuteReader("SELECT * FROM [" + tn + "] WHERE [ID]=" + id);
                    // Dim t1 As Date = Calendar.Now
                    // Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> ExecuteReader : " & ((t1 - t).TotalMilliseconds / 1000) & " s")
                    if (dbRis.Read())
                    {
                        // Dim t3 As Date = Calendar.Now
                        // Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> Read : " & ((t3 - t1).TotalMilliseconds / 1000) & " s")
                        var reader = new Databases.DBReader(dbRis);
                        // Dim t4 As Date = Calendar.Now
                        // Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> new DBReader : " & ((t4 - t3).TotalMilliseconds / 1000) & " s")
                        ret = cursor.InstantiateNew(reader);
                        conn.Load(ret, reader);
                        reader.Dispose();
                        dbRis = null;
                        // Dim t5 As Date = Calendar.Now
                        // Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> new Load : " & ((t5 - t4).TotalMilliseconds / 1000) & " s")
                    }

                    return ret;
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    cursor.Dispose();
                    if (dbRis is object)
                        dbRis.Dispose();
                }
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public virtual string list()
            {
                return DMD.Strings.vbNullString;
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public virtual bool SupportsCreate
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SCreate) == ModuleSupportFlags.SCreate;
                }
            }

            public virtual bool SupportsDuplicate
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SDuplicate) == ModuleSupportFlags.SDuplicate;
                }
            }



            // Public Overridable Function create() As String
            // Dim cursor As DBObjectCursorBase
            // Dim item As DBObjectBase
            // Dim fromID As Integer
            // If (Me.CanCreate = False) Then Throw New PermissionDeniedException(Me.CurrentModule, "create")
            // fromID = Formats.ToInteger(RPC.n2int(Me.GetParameter("ID", "")))
            // cursor = Me.CreateCursor
            // item = cursor.Add
            // If (fromID <> 0) AndAlso (TypeOf (item) Is ISupportInitializeFrom) Then DirectCast(item, ISupportInitializeFrom).InitializeFrom(Me.GetInternalItemById(fromID))
            // 'e = New Sistema.SystemEvent
            // 'item.OnCreate", "Sub", e)
            // item.Save()
            // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            // Return Me.InternalEdit(item)
            // End Function

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public virtual bool SupportsDelete
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SDelete) == ModuleSupportFlags.SDelete;
                }
            }

            public virtual bool CanDelete(object item)
            {
                bool ret;
                ret = CurrentModule.UserCanDoAction("delete");
                if (ret == false && item is Databases.IDBObject)
                {
                    {
                        var withBlock = (Databases.IDBObject)item;
                        ret = CurrentModule.UserCanDoAction("delete_own") & withBlock.CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
                    }
                }

                if (ret == false && item is Databases.IDBPOObject)
                {
                    {
                        var withBlock1 = (Databases.IDBPOObject)item;
                        if (CurrentModule.UserCanDoAction("delete_office"))
                        {
                            ret = withBlock1.PuntoOperativo is null || Sistema.Users.CurrentUser.Uffici.HasOffice(withBlock1.PuntoOperativo);
                        }
                    }
                }

                return ret;
            }

            public virtual string delete()
            {
                int itemID = (int)Sistema.RPC.n2int(GetParameter("ID", ""));
                Databases.DBObjectBase item = (Databases.DBObjectBase)GetInternalItemById(itemID);
                if (CanDelete(item))
                {
                    var cursor = CreateCursor();
                    if (item is Databases.IDBObject)
                    {
                        ((Databases.IDBObject)item).Stato = Databases.ObjectStatus.OBJECT_DELETED;
                        item.Save();
                    }
                    else
                    {
                        item.Delete();
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }

                    return DMD.Strings.vbNullString;
                }
                else
                {
                    throw new PermissionDeniedException();
                    return DMD.Strings.vbNullString;
                }
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public virtual bool SupportsEdit
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SEdit) == ModuleSupportFlags.SEdit;
                }
            }

            public virtual bool SupportsAnnotations
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SAnnotations) == ModuleSupportFlags.SAnnotations;
                }
            }

            public virtual bool CanEdit(object item)
            {
                bool ret;
                ret = CurrentModule.UserCanDoAction("edit");
                if (ret == false && item is Databases.IDBObject)
                {
                    {
                        var withBlock = (Databases.IDBObject)item;
                        ret = CurrentModule.UserCanDoAction("edit_own") & withBlock.CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
                    }
                }

                if (ret == false && item is Databases.IDBPOObject)
                {
                    {
                        var withBlock1 = (Databases.IDBPOObject)item;
                        if (CurrentModule.UserCanDoAction("edit_office"))
                        {
                            ret = withBlock1.PuntoOperativo is null || Sistema.Users.CurrentUser.Uffici.HasOffice(withBlock1.PuntoOperativo);
                        }
                    }
                }

                return ret;
            }

            public virtual bool CanList()
            {
                bool ret;
                ret = CurrentModule.UserCanDoAction("list") || CurrentModule.UserCanDoAction("edit_office") || CurrentModule.UserCanDoAction("edit_own");
                return ret;
            }

            public virtual bool CanList(object item)
            {
                bool ret;
                ret = CurrentModule.UserCanDoAction("list");
                if (ret == false && item is Databases.IDBObject)
                {
                    ret = CurrentModule.UserCanDoAction("edit_own") && ((Databases.IDBObject)item).CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
                }

                if (ret == false && item is Databases.IDBPOObject)
                {
                    ret = CurrentModule.UserCanDoAction("edit_office") && Anagrafica.Uffici.GetPuntiOperativiConsentiti().GetItemById(((Databases.IDBPOObject)item).IDPuntoOperativo) is object;
                }

                return ret;
            }

            public virtual string edit()
            {
                int itemID = Sistema.Formats.ToInteger(Sistema.RPC.n2int(GetParameter("ID", "")));
                var itemObj = GetInternalItemById(itemID);
                if (!CanEdit(itemObj))
                {
                    throw new PermissionDeniedException(CurrentModule, "edit");
                }
                // If itemObj Is Nothing Then
                // Throw New ArgumentNullException("Non trovo alcun elemeneto corrispondente all'ID: " & itemID)
                // End If
                // Return Me.InternalEdit(itemObj)
                // Dim editor As BaseModuleEditor = Me.GetEditor
                // With editor
                // .UseLocal = True
                // .Width = Me.m_.Width
                // .Height = Me.m_.Height
                // .Name = "ctrlEditor"
                // .Visible = False
                // .DoLayout()
                // End With
                // Dim writer As New HTMLWriter
                // editor.CreateHTML(writer)

                return InternalEdit((Databases.DBObjectBase)itemObj);
            }

            public virtual bool CanPrint(object item)
            {
                return CurrentModule.UserCanDoAction("print");
            }

            public virtual string print()
            {
                string html = ExecuteAction("edit", this);
                html += "<script type=\"text-javascript\">" + DMD.Strings.vbNewLine + "Window.addListener(\"onload\", new Function('window.print()'));" + DMD.Strings.vbNewLine + "</script>";
                return html;
            }

            protected virtual string InternalEdit(Databases.DBObjectBase value)
            {
                // Dim tabControl As CTabEditor
                // Dim writer As New HTMLWriter
                // Dim itemID As Integer
                // Dim message As String = vbNullString

                // tabControl = Me.CreateTabEditor(value)
                // itemID = GetID(value)

                // If Not Me.CanEdit(value) Then
                // message = "Diritti insufficienti"
                // Else
                // tabControl.Item = value

                // Select Case Me.GetParameter("_s")
                // Case "1", "2", "3", "4", "5"
                // Me.OnBeforeSave(value)

                // If tabControl.Save Then
                // If TypeOf (value) Is IDBObject Then
                // If DirectCast(value, IDBObject).Stato = ObjectStatus.OBJECT_TEMP Then
                // DirectCast(value, IDBObject).Stato = ObjectStatus.OBJECT_VALID
                // End If
                // End If
                // value.Save()
                // Else
                // message = "Errore sconosciuto"
                // End If

                // Me.OnAfterSave(value)
                // End Select
                // End If

                // tabControl.CreateHTML(writer)
                // writer.WriteRowData("<input type=""hidden"" name=""ID"" id=""ID"" value=""" & itemID & """ />")
                // If GetParameter("strsrc") = "" Then writer.WriteRowData("<input type=""hidden"" name=""strsrc"" id=""strsrc"" value=""" & Me.GetParameter("strsrc") & """ />")

                // If message <> "" Then
                // writer.WriteRowData("<script language=""javascript"" type=""text/javascript"">" & vbNewLine)
                // writer.WriteRowData("alert(""" & message & """);")
                // writer.WriteRowData("</script>")
                // End If

                // Dim ret As String = writer.ToString
                // writer.Dispose()
                // Return ret

                string ret = ""; // Me.list()
                ret += "<script type=\"text/javascript\">" + DMD.Strings.vbNewLine;
                ret += "Window.addListener(\"onload\", new Function('setTimeout(SystemUtils.EditModuleItem(" + Databases.GetID(CurrentModule) + ", " + Databases.GetID(value) + "), 500)'));";
                ret += "</script>";
                return ret;
            }

            protected virtual void OnBeforeSave(object item)
            {
            }

            protected virtual void OnAfterSave(object item)
            {
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public virtual bool SupportsPrint
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SPrint) == ModuleSupportFlags.SPrint;
                }
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            /// <summary>
        /// Quando sottoposto ad override in una classe derivata restituisce un valore booleano che indica se il modulo supporta l'esportazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool SupportsExport
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SExport) == ModuleSupportFlags.SExport | this.GetExportableColumnsList().Count > 0;
                }
            }

            public virtual CCollection<ExportableColumnInfo> GetExportableColumnsList()
            {
                var ret = new CCollection<ExportableColumnInfo>(); // = Me.CurrentModuleHandler.GetExportableColumnList()
                                                                   // ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
                return ret;
            }

            public string GetExportableColumns()
            {
                var ret = GetExportableColumnsList();
                if (ret.Count > 0)
                {
                    return DMD.XML.Utils.Serializer.Serialize(ret.ToArray());
                }
                else
                {
                    return DMD.Strings.vbNullString;
                }
            }

            protected virtual CCollection<ExportableColumnInfo> GetExportedColumns()
            {
                string text = Sistema.RPC.n2str(GetParameter("listCols", ""));
                CCollection<ExportableColumnInfo> ret;
                if (string.IsNullOrEmpty(text))
                {
                    ret = GetExportableColumnsList();
                }
                else
                {
                    ret = new CCollection<ExportableColumnInfo>();
                    object[] items = (object[])DMD.XML.Utils.Serializer.Deserialize(text);
                    foreach (ExportableColumnInfo item in items)
                        ret.Add(item);
                }

                return ret;
            }

            protected virtual object GetColumnValue(object item, string key)
            {
                var m = item.GetType().GetProperty(key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                return m.GetValue(item, new object[] { });
            }

            public bool CanExport()
            {
                return CurrentModule.UserCanDoAction("export");
            }

            public virtual string ExportList()
            {
                if (!CanExport())
                    throw new PermissionDeniedException(CurrentModule, "export");
                string listFormat = DMD.Strings.LCase(DMD.Strings.Trim(Sistema.RPC.n2str(GetParameter("listFormat", "xls"))));
                switch (listFormat ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "xls", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            return ExportXlsFormat();
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "xlsx", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            return ExportXLSXFormat();
                        }
                    // Case "xls" : Return Me.ExportFakeXLSFormat()
                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "mdb", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            return ExportMdbFormat();
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "csv", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            return ExportCSVFormat();
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "txt", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            return ExportTxtFormat();
                        }

                    default:
                        {
                            throw new NotSupportedException("Il formato " + listFormat + " non è supportato");
                            break;
                        }
                }
            }

            protected virtual string ExportXLSXFormat()
            {
                using (Databases.DBObjectCursorBase cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.n2str(GetParameter("cursor", ""))))
                {
                    cursor.Reset1();
                    cursor.PageSize = 10000;
                    // cursor.Count()

                    using (var wb = new XLWorkbook())
                    {
                        var xlsTable = wb.Worksheets.Add(CurrentModule.ModuleName);
                        var cols = GetExportedColumns();
                        var dic = new Dictionary<string, int>();
                        var dic1 = new Dictionary<string, ExportableColumnInfo>();
                        foreach (ExportableColumnInfo col in cols)
                        {
                            if (col.Selected)
                            {
                                var xlsCol = XLSUtils.EnsureXlsColumn(xlsTable, col.Key);
                                dic.Add(col.Key, xlsCol.ColumnNumber());
                                dic1.Add(col.Key, col);
                            }
                        }

                        XLSUtils.ApplyDefaultColumnsStyle(xlsTable, XLColor.AliceBlue, true);
                        int r = 1;
                        while (!cursor.EOF())
                        {
                            r += 1;
                            foreach (string key in dic.Keys)
                                XLSUtils.SetCellValue(xlsTable, r, dic[key], GetColumnValue(cursor.Item, key), dic1[key].TipoValore);
                            cursor.MoveNext();
                        }

                        Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(Instance.Configuration.PublicURL + "temp"));
                        string fileName = Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".xlsx";
                        wb.SaveAs(AC.MapPath(fileName));
                        string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + CurrentModule.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
                        CurrentModule.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
                        return url;
                    }
                }
            }

            protected virtual string ExportXlsFormat()
            {
                Databases.DBObjectCursorBase cursor = null;
                string fileName = "";
                Databases.CXlsDBConnection xlsConn = null;
                Databases.CDBTable xlsTable = null;
                CCollection<ExportableColumnInfo> cols;
                IDbCommand cmd = null;
                IDbDataParameter param = null;

                // If (Me.UseLocal) Then
                try
                {
                    cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.n2str(GetParameter("cursor", "")));
                    cursor.Reset1();
                    cursor.PageSize = 10000;
                    // cursor.Count()

                    Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(Instance.Configuration.PublicURL + "temp"));
                    fileName = Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".xls";
                    Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/xlsfile.xls"), Sistema.ApplicationContext.MapPath(fileName), true);
                    xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName));
                    xlsConn.OpenDB();
                    xlsTable = xlsConn.Tables.Add("Tabella");
                    // xlsTable = xlsConn.Tables(0)

                    cols = GetExportedColumns();
                    foreach (ExportableColumnInfo col in cols)
                    {
                        if (col.Selected)
                        {
                            xlsTable.Fields.Add(col.Key, col.TipoValore);
                        }
                    }

                    xlsTable.Update();
                    cmd = xlsTable.GetInsertCommand();
                    while (!cursor.EOF())
                    {
                        // cmd.Parameters.Clear()

                        foreach (ExportableColumnInfo col in cols)
                        {
                            if (col.Selected)
                            {
                                param = (IDbDataParameter)cmd.Parameters["@" + col.Key];
                                param.Value = xlsConn.ToDB(GetColumnValue(cursor.Item, col.Key));
                            }
                        }

                        cmd.ExecuteNonQuery();
                        cursor.MoveNext();
                    }

                    if (cmd is object)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }

                    if (xlsConn is object)
                    {
                        xlsConn.Dispose();
                        xlsConn = null;
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }

                    string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + CurrentModule.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
                    CurrentModule.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
                    return url;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (cmd is object)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }

                    if (xlsConn is object)
                    {
                        xlsConn.Dispose();
                        xlsConn = null;
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }

            protected virtual string ExportMdbFormat()
            {
                Databases.DBObjectCursorBase cursor = null;
                string fileName = "";
                Databases.CMdbDBConnection outConn = null;
                Databases.CDBTable outTable = null;
                CCollection<ExportableColumnInfo> cols;
                IDbCommand cmd = null;
                IDbDataParameter param = null;
                try
                {
                    cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.n2str(GetParameter("cursor", "")));
                    cursor.Reset1();
                    Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(Instance.Configuration.PublicURL + "temp"));
                    fileName = Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".mdb";
                    Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/template.mdb"), Sistema.ApplicationContext.MapPath(fileName), true);
                    outConn = new Databases.CMdbDBConnection(Sistema.ApplicationContext.MapPath(fileName));
                    outConn.OpenDB();
                    outTable = outConn.Tables.Add(CurrentModule.DisplayName);
                    // xlsTable = xlsConn.Tables(0)

                    cols = GetExportedColumns();
                    foreach (ExportableColumnInfo col in cols)
                    {
                        if (col.Selected)
                        {
                            outTable.Fields.Add(col.Key, col.TipoValore);
                        }
                    }

                    outTable.Update();
                    cmd = outTable.GetInsertCommand();
                    while (!cursor.EOF())
                    {
                        foreach (ExportableColumnInfo col in cols)
                        {
                            if (col.Selected)
                            {
                                param = (IDbDataParameter)cmd.Parameters["@" + col.Key];
                                param.Value = outConn.ToDB(GetColumnValue(cursor.Item, col.Key));
                            }
                        }

                        cmd.ExecuteNonQuery();
                        cursor.MoveNext();
                    }

                    if (cmd is object)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }

                    if (outConn is object)
                    {
                        outConn.Dispose();
                        outConn = null;
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }

                    string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + CurrentModule.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
                    CurrentModule.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
                    return url;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (cmd is object)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }

                    if (outConn is object)
                    {
                        outConn.Dispose();
                        outConn = null;
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }

            protected void writeCSV(System.Text.StringBuilder buffer, string text)
            {
                buffer.Append('"');
                buffer.Append(Strings.Replace(text, DMD.Strings.CStr('"'), DMD.Strings.CStr('"')));
                buffer.Append('"');
            }

            // Protected Overridable Function ExportFakeXLSFormat() As String
            // Dim cursor As DBObjectCursorBase
            // Dim fileName As String
            // Dim cols As CCollection(Of ExportableColumnInfo)

            // 'If (Me.UseLocal) Then
            // cursor = DMD.XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter("cursor", "")))
            // cursor.Reset1()

            // FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            // fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"


            // Dim buffer As New System.Text.StringBuilder


            // cols = Me.GetExportedColumns()
            // Dim i As Integer = 0
            // For Each col As ExportableColumnInfo In cols
            // If (col.Selected) Then
            // If (i > 0) Then buffer.Append(";")
            // Me.writeCSV(buffer, col.Key)
            // i += 1
            // End If
            // Next
            // buffer.Append(vbCrLf)

            // While Not cursor.EOF
            // i = 0
            // For Each col As ExportableColumnInfo In cols
            // If (col.Selected) Then
            // If (i > 0) Then buffer.Append(";")
            // Me.writeCSV(buffer, Formats.ToString(Me.GetColumnValue(cursor.Item, col.Key)))
            // i += 1
            // End If
            // Next
            // cursor.MoveNext()
            // buffer.Append(vbCrLf)
            // End While
            // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            // System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            // Dim url As String = "/minidom/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.CurrentModule.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

            // Me.CurrentModule.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

            // Return url
            // End Function

            protected virtual string ExportCSVFormat()
            {
                Databases.DBObjectCursorBase cursor;
                string fileName;
                CCollection<ExportableColumnInfo> cols;

                // If (Me.UseLocal) Then
                cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.n2str(GetParameter("cursor", "")));
                cursor.Reset1();
                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(Instance.Configuration.PublicURL + "temp"));
                fileName = Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".csv";
                var buffer = new System.Text.StringBuilder();
                cols = GetExportedColumns();
                int i = 0;
                foreach (ExportableColumnInfo col in cols)
                {
                    if (col.Selected)
                    {
                        if (i > 0)
                            buffer.Append(";");
                        writeCSV(buffer, col.Key);
                        i += 1;
                    }
                }

                buffer.Append(DMD.Strings.vbCrLf);
                while (!cursor.EOF())
                {
                    i = 0;
                    foreach (ExportableColumnInfo col in cols)
                    {
                        if (col.Selected)
                        {
                            if (i > 0)
                                buffer.Append(";");
                            writeCSV(buffer, Sistema.Formats.ToString(GetColumnValue(cursor.Item, col.Key)));
                            i += 1;
                        }
                    }

                    cursor.MoveNext();
                    buffer.Append(DMD.Strings.vbCrLf);
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString());
                string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + CurrentModule.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
                CurrentModule.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
                return url;
            }

            protected virtual string ExportTxtFormat()
            {
                Databases.DBObjectCursorBase cursor;
                string fileName;
                CCollection<ExportableColumnInfo> cols;

                // If (Me.UseLocal) Then
                cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.n2str(GetParameter("cursor", "")));
                cursor.Reset1();
                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(Instance.Configuration.PublicURL + "temp"));
                fileName = Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".txt";
                var buffer = new System.Text.StringBuilder();
                cols = GetExportedColumns();
                int i = 0;
                foreach (ExportableColumnInfo col in cols)
                {
                    if (col.Selected)
                    {
                        if (i > 0)
                            buffer.Append(DMD.Strings.vbTab);
                        buffer.Append(col.Key);
                        i += 1;
                    }
                }

                buffer.Append(DMD.Strings.vbCrLf);
                while (!cursor.EOF())
                {
                    i = 0;
                    foreach (ExportableColumnInfo col in cols)
                    {
                        if (col.Selected)
                        {
                            if (i > 0)
                                buffer.Append(DMD.Strings.vbTab);
                            buffer.Append(Sistema.Formats.ToString(GetColumnValue(cursor.Item, col.Key)));
                            i += 1;
                        }
                    }

                    cursor.MoveNext();
                    buffer.Append(DMD.Strings.vbCrLf);
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString());
                string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + CurrentModule.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
                CurrentModule.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
                return url;
            }
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            /// <summary>
        /// Quando sottoposto ad override in una classe derivata restituisce un valore booleano che indica se il modulo supporta l'importazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public virtual bool SupportsImport
            {
                get
                {
                    return (m_Support & ModuleSupportFlags.SImport) == ModuleSupportFlags.SImport | this.GetExportableColumnsList().Count > 0;
                }
            }

            protected virtual void SetColumnValue(object item, string key, object value)
            {
                switch (key ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "ID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            break;
                        }

                    default:
                        {
                            var m = item.GetType().GetProperty(key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            if (m.CanWrite == false)
                            {
                                throw new InvalidConstraintException("Proprietà in sola lettura: " + key);
                            }

                            try
                            {
                                if (value is DBNull)
                                {
                                    m.SetValue(item, null, new object[] { });
                                }
                                else
                                {
                                    m.SetValue(item, Sistema.Types.CastTo(value, m.PropertyType), new object[] { });
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            break;
                        }
                }
            }

            protected virtual object FindExportedItem(Databases.DBObjectCursorBase cursor, Databases.DBReader info)
            {
                object ret;
                int id = info.GetValue<int>("ID", 0);
                if (id == 0)
                    return null;
                cursor.Reset1();
                if (cursor is Databases.DBObjectCursor)
                    ((Databases.DBObjectCursor)cursor).Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.ID.Value = id;
                ret = cursor.Item;
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }

            public bool CanImport()
            {
                return CurrentModule.UserCanDoAction("import");
            }

            public virtual string ImportList()
            {
                if (!CanImport())
                    throw new PermissionDeniedException(CurrentModule, "import");
                string fileName;
                Databases.CXlsDBConnection xlsConn;
                Databases.CDBTable xlsTable;
                Databases.DBReader xlsRis;
                CCollection<ExportableColumnInfo> cols;
                Databases.DBObjectBase item;
                Databases.DBObjectCursorBase cursor;
                int s;
                s = (int)Sistema.RPC.n2int(GetParameter("s", 0.ToString()));
                if (s == 0)
                {
                    return "L'importazione modificherà i record esistenti ed aggiungerà eventuali record non trovati.";
                }
                else
                {
                    fileName = Strings.Trim(Sistema.RPC.n2str(GetParameter("fn", DMD.Strings.vbNullString)));
                    if (string.IsNullOrEmpty(fileName))
                        throw new ArgumentNullException("fileName");
                    xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName));
                    xlsConn.OpenDB();
                    xlsTable = xlsConn.Tables["Tabella"];
                    xlsRis = new Databases.DBReader(xlsTable);
                    cols = GetExportedColumns();
                    cursor = CreateCursor();
                    while (xlsRis.Read())
                    {
                        item = (Databases.DBObjectBase)FindExportedItem(cursor, xlsRis);
                        if (item is null)
                            item = (Databases.DBObjectBase)cursor.Add();
                        if (item is Databases.IDBObject)
                            ((Databases.IDBObject)item).Stato = Databases.ObjectStatus.OBJECT_VALID;
                        foreach (ExportableColumnInfo column in cols)
                        {
                            if (xlsRis.Schema.Fields.ContainsKey(column.Key))
                            {
                                SetColumnValue(item, column.Key, xlsRis.GetValue(column.Key));
                            }
                        }

                        item.Save();
                    }

                    xlsConn.Dispose();
                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }

                    CurrentModule.DispatchEvent(new Sistema.EventDescription("list_imported", "Dati importati dal file " + fileName, null));
                }

                return "";
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            protected virtual void OnMessageLogged(string message)
            {
                MessageLogged?.Invoke(this, message);
            }

            public virtual string LoadAll()
            {
                var ret = new CCollection();
                var cursor = CreateCursor();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (cursor is Databases.DBObjectCursor)
                {
                    {
                        var withBlock = (Databases.DBObjectCursor)cursor;
                        withBlock.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                    }
                }

                while (!cursor.EOF())
                {
                    ret.Add(cursor.Item);
                    cursor.MoveNext();
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Dispose();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                return DMD.XML.Utils.Serializer.Serialize(ret);
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        }
    }
}