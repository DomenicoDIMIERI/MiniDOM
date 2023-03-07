using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CBaseModuleHandler 
        : IModuleHandler
    {
        private Sistema.CModule m_Module;
        private ModuleSupportFlags m_Support;

        public event MessageLoggedEventHandler MessageLogged;

        public delegate void MessageLoggedEventHandler(object sender, string message);

        public CBaseModuleHandler()
        {
            DMDObject.IncreaseCounter(this);
            m_Support = ModuleSupportFlags.SNone;
            m_Module = null;
        }

        public CBaseModuleHandler(ModuleSupportFlags support) : this()
        {
            m_Support = support;
            m_Module = null;
        }

        public CBaseModuleHandler(Sistema.CModule module, ModuleSupportFlags support = ModuleSupportFlags.SNone, bool useLocal = true) : this(support)
        {
            if (module is null)
                throw new ArgumentNullException("module");
            SetModule(module);
        }

        public string GetParameter1(object renderer, string paramName, string defValue)
        {
            // Return Sistema.ApplicationContext.GetParameter(paramName, defValue)
            WebSite.WebPageEx page = (WebSite.WebPageEx)renderer;
            return page.GetParameter1(paramName, defValue);
        }

        public T GetParameter1<T>(object renderer, string paramName, object defValue) where T : struct
        {
            // Return Sistema.ApplicationContext.renderer(Of T)(paramName, defValue)
            WebSite.WebPageEx page = (WebSite.WebPageEx)renderer;
            return (T)page.GetParameter1<T>(paramName, defValue);
        }

        public virtual string ExecuteAction(object renderer, string actionName)
        {
            switch (Strings.LCase(actionName) ?? "")
            {
                case "getitembyid":
                    {
                        return GetItemById(renderer);
                    }

                case "edit":
                    {
                        return edit(renderer);
                    }

                case "list":
                    {
                        return list(renderer);
                    }
                // Case "create" : Return Me.create(renderer)
                case "delete":
                    {
                        return delete(renderer);
                    }

                case "importlist":
                    {
                        return ImportList(renderer);
                    }

                case "exportlist":
                    {
                        return ExportList(renderer);
                    }

                case "getexportablecolumns":
                    {
                        return GetExportableColumns(renderer);
                    }

                case "print":
                    {
                        return print(renderer);
                    }

                default:
                    {
                        MethodInfo m;
                        m = GetType().GetMethod(actionName, BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(object) }, null);
                        if (m is null)
                        {
                            throw new MissingMethodException("Il modulo [" + Module.ModuleName + "] non implementa l'azione [" + actionName + "]");
                            return DMD.Strings.vbNullString;
                        }

                        var ret = m.Invoke(this, new[] { renderer });
                        if (ret is Sistema.MethodResults)
                        {
                            return DMD.XML.Utils.Serializer.Serialize(ret);
                        }
                        else
                        {
                            return DMD.Strings.CStr(ret);
                        }

                        break;
                    }
            }
        }

        public void SetModule(Sistema.CModule value)
        {
            m_Module = value;
        }

        public Sistema.CModule Module
        {
            get
            {
                return m_Module;
            }
        }

        public virtual Databases.DBObjectCursorBase CreateCursor()
        {
            return null;
        }


        public string n2str(object renderer, string paramName, string defValue = "")
        {
            WebSite.WebPageEx page = (WebSite.WebPageEx)renderer;
            NameValueCollection col = null;
            if (page.Request.QueryString.HasKeys())
            {
                col = page.Request.QueryString;
            }
            else
            {
                col = page.Request.Form;
            }

            string ret = null;
            foreach (string k in col.Keys)
            {
                if (DMD.Strings.EQ(k, paramName, true))
                {
                    ret = col[k];
                }
            }

            return DMD.WebUtils.URLDecode(ret);
        }

        public bool? n2bool(object renderer, string paramName, bool? defValue = default)
        {
            WebSite.WebPageEx page = (WebSite.WebPageEx)renderer;
            NameValueCollection col = null;
            if (page.Request.QueryString.HasKeys())
            {
                col = page.Request.QueryString;
            }
            else
            {
                col = page.Request.Form;
            }

            string ret = null;
            foreach (string k in col.Keys)
            {
                if (DMD.Strings.EQ(k, paramName, true))
                {
                    ret = col[k];
                }
            }

            return DMD.Booleans.TryParse( DMD.WebUtils.URLDecode(ret), defValue);
        }

        public int? n2int(object renderer, string paramName, int? defValue = default)
        {
            WebSite.WebPageEx page = (WebSite.WebPageEx)renderer;
            NameValueCollection col = null;
            if (page.Request.QueryString.HasKeys())
            {
                col = page.Request.QueryString;
            }
            else
            {
                col = page.Request.Form;
            }

            string ret = null;
            foreach (string k in col.Keys)
            {
                if (DMD.Strings.EQ(k, paramName, true))
                {
                    ret = col[k];
                }
            }

            return DMD.Integers.TryParse(DMD.WebUtils.URLDecode(ret), defValue);
        }

        public DateTime? n2date(object renderer, string paramName, DateTime? defValue = default)
        {
            WebSite.WebPageEx page = (WebSite.WebPageEx)renderer;
            NameValueCollection col = null;
            if (page.Request.QueryString.HasKeys())
            {
                col = page.Request.QueryString;
            }
            else
            {
                col = page.Request.Form;
            }

            string ret = null;
            foreach (string k in col.Keys)
            {
                if (DMD.Strings.EQ(k, paramName, true))
                {
                    ret = col[k];
                }
            }

            return DMD.DateUtils.TryParse(DMD.WebUtils.URLDecode(ret), defValue);
        }


        // Public MustOverride Function GetGC As CModulesClass(Of 

        public virtual string GetItemById(object renderer)
        {
            int itemID = (int)this.n2int("ID", "");
            var ret = GetInternalItemById(itemID);
            if (ret is null)
            {
                return "";
            }
            else
            {
                return DMD.XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document);
            }
        }

        public virtual string GetItemsById(object renderer)
        {
            Databases.DBObjectCursorBase cursor = null;
            var ret = new CCollection();
            try
            {
                string ids = this.n2str(renderer, "ids");
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

        public virtual object GetInternalItemById(int id)
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
        public virtual string list(object renderer)
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

        /// <summary>
        /// Restituisce vero se l'utente corrente può eseguire la configurazione del modulo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool CanConfigure()
        {
            return Module.UserCanDoAction("configure");
        }

        public virtual bool CanCreate(object item)
        {
            return Module.UserCanDoAction("create");
        }

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
            ret = Module.UserCanDoAction("delete");
            if (ret == false && item is Databases.IDBObject)
            {
                {
                    var withBlock = (Databases.IDBObject)item;
                    ret = Module.UserCanDoAction("delete_own") & withBlock.CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
                }
            }

            if (ret == false && item is Databases.IDBPOObject)
            {
                {
                    var withBlock1 = (Databases.IDBPOObject)item;
                    if (Module.UserCanDoAction("delete_office"))
                    {
                        ret = withBlock1.PuntoOperativo is null || Sistema.Users.CurrentUser.Uffici.HasOffice(withBlock1.PuntoOperativo);
                    }
                }
            }

            return ret;
        }

        public virtual string delete(object renderer)
        {
            int itemID = (int) this.n2int(renderer, "ID");
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
            ret = Module.UserCanDoAction("edit");
            if (ret == false && item is Databases.IDBObject)
            {
                {
                    var withBlock = (Databases.IDBObject)item;
                    ret = Module.UserCanDoAction("edit_own") & withBlock.CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
                }
            }

            if (ret == false && item is Databases.IDBPOObject)
            {
                {
                    var withBlock1 = (Databases.IDBPOObject)item;
                    if (Module.UserCanDoAction("edit_office"))
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
            ret = Module.UserCanDoAction("list") || Module.UserCanDoAction("edit_office") || Module.UserCanDoAction("edit_own");
            return ret;
        }

        public virtual bool CanList(object item)
        {
            bool ret;
            ret = Module.UserCanDoAction("list");
            if (ret == false && item is Databases.IDBObject)
            {
                ret = Module.UserCanDoAction("edit_own") && ((Databases.IDBObject)item).CreatoDaId == Databases.GetID(Sistema.Users.CurrentUser);
            }

            if (ret == false && item is Databases.IDBPOObject)
            {
                ret = Module.UserCanDoAction("edit_office") && Anagrafica.Uffici.GetPuntiOperativiConsentiti().GetItemById(((Databases.IDBPOObject)item).IDPuntoOperativo) is object;
            }

            return ret;
        }

        public virtual string edit(object renderer)
        {
            int itemID = (int) this.n2int(renderer, "ID");
            var itemObj = GetInternalItemById(itemID);
            if (!CanEdit(itemObj))
            {
                throw new PermissionDeniedException(Module, "edit");
            }
            // If itemObj Is Nothing Then
            // Throw New ArgumentNullException("Non trovo alcun elemeneto corrispondente all'ID: " & itemID)
            // End If
            // Return Me.InternalEdit(itemObj)
            // Dim editor As BaseModuleEditor = Me.GetEditor
            // With editor
            // .UseLocal = True
            // .Width = Me.m_Renderer.Width
            // .Height = Me.m_Renderer.Height
            // .Name = "ctrlEditor"
            // .Visible = False
            // .DoLayout()
            // End With
            // Dim writer As New HTMLWriter
            // editor.CreateHTML(writer)

            return InternalEdit(renderer, (Databases.DBObjectBase)itemObj);
        }

        public virtual bool CanPrint(object item)
        {
            return Module.UserCanDoAction("print");
        }

        public virtual string print(object renderer)
        {
            string html = ExecuteAction(renderer, "edit");
            html += "<script type=\"text-javascript\">" + DMD.Strings.vbNewLine + "Window.addListener(\"onload\", new Function('window.print()'));" + DMD.Strings.vbNewLine + "</script>";
            return html;
        }

        protected virtual string InternalEdit(object renderer, Databases.DBObjectBase value)
        {
            // Dim tabControl As CTabEditor
            // Dim writer As New HTMLWriter
            // Dim itemID As Integer
            // Dim message As String = vbNullString

            // tabControl = Me.CreateTabEditor(renderer, value)
            // itemID = GetID(value)

            // If Not Me.CanEdit(value) Then
            // message = "Diritti insufficienti"
            // Else
            // tabControl.Item = value

            // Select Case Me.GetParameter(renderer, "_s")
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
            // If GetParameter(renderer, "strsrc") = "" Then writer.WriteRowData("<input type=""hidden"" name=""strsrc"" id=""strsrc"" value=""" & Me.GetParameter(renderer, "strsrc") & """ />")

            // If message <> "" Then
            // writer.WriteRowData("<script language=""javascript"" type=""text/javascript"">" & vbNewLine)
            // writer.WriteRowData("alert(""" & message & """);")
            // writer.WriteRowData("</script>")
            // End If

            // Dim ret As String = writer.ToString
            // writer.Dispose()
            // Return ret

            string ret = ""; // Me.list(renderer)
            ret += "<script type=\"text/javascript\">" + DMD.Strings.vbNewLine;
            ret += "Window.addListener(\"onload\", new Function('setTimeout(SystemUtils.EditModuleItem(" + Databases.GetID(Module) + ", " + Databases.GetID(value) + "), 500)'));";
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
            var ret = new CCollection<ExportableColumnInfo>();
            // ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            return ret;
        }

        public string GetExportableColumns(object renderer)
        {
            var ret = GetExportableColumnsList();
            if (ret.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(ret.ToArray(), XMLSerializeMethod.Document);
            }
            else
            {
                return DMD.Strings.vbNullString;
            }
        }

        protected virtual CCollection<ExportableColumnInfo> GetExportedColumns(object renderer)
        {
            string text = this.n2str(renderer, "listCols");
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

        protected virtual object GetColumnValue(object renderer, object item, string key)
        {
            var m = item.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
            return m.GetValue(item, new object[] { });
        }

        public bool CanExport()
        {
            return Module.UserCanDoAction("export");
        }

        public virtual string ExportList(object renderer)
        {
            if (!CanExport())
                throw new PermissionDeniedException(Module, "export");
            string listFormat = DMD.Strings.LCase(DMD.Strings.Trim(this.n2str(renderer, "listFormat", "xls")));
            switch (listFormat ?? "")
            {
                case "xls":
                    {
                        return ExportXlsFormat(renderer);
                    }
                // Case "xls" : Return Me.ExportFakeXLSFormat(renderer)
                case "mdb":
                    {
                        return ExportMdbFormat(renderer);
                    }

                case "csv":
                    {
                        return ExportCSVFormat(renderer);
                    }

                case "txt":
                    {
                        return ExportTxtFormat(renderer);
                    }

                default:
                    {
                        throw new NotSupportedException("Il formato " + listFormat + " non è supportato");
                        break;
                    }
            }
        }

        protected virtual string ExportXlsFormat(object renderer)
        {
            string fileName = "";
            Databases.CDBTable xlsTable = null;
            CCollection<ExportableColumnInfo> cols;
            IDbDataParameter param = null;

            using (var cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "cursor", "")))
            {
                cursor.Reset1();
                cursor.PageSize = 10000;
                // cursor.Count()

                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL + "temp"));
                fileName = WebSite.Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".xls";
                Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/xlsfile.xls"), Sistema.ApplicationContext.MapPath(fileName), true);

                using (var xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName)))
                {
                    xlsConn.OpenDB();
                    xlsTable = xlsConn.Tables.Add("Tabella");
                    // xlsTable = xlsConn.Tables(0)

                    cols = GetExportedColumns(renderer);
                    foreach (ExportableColumnInfo col in cols)
                    {
                        if (col.Selected)
                        {
                            xlsTable.Fields.Add(col.Key, col.TipoValore);
                        }
                    }

                    xlsTable.Update();

                    using (var cmd = xlsTable.GetInsertCommand())
                    {
                        while (!cursor.EOF())
                        {
                            // cmd.Parameters.Clear()

                            foreach (ExportableColumnInfo col in cols)
                            {
                                if (col.Selected)
                                {
                                    param = (IDbDataParameter)cmd.Parameters["@" + col.Key];
                                    param.Value = xlsConn.ToDB(GetColumnValue(renderer, cursor.Item, col.Key));
                                }
                            }

                            cmd.ExecuteNonQuery();
                            cursor.MoveNext();
                        }
                    }
                }
            }

            string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + Module.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
            Module.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
            return url;
        }

        protected virtual string ExportMdbFormat(object renderer)
        {
            string fileName = "";
            Databases.CDBTable outTable = null;
            CCollection<ExportableColumnInfo> cols;
            IDbDataParameter param = null;

            using (var cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "cursor", "")))
            {
                cursor.Reset1();
                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL + "temp"));
                fileName = WebSite.Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".mdb";
                Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/template.mdb"), Sistema.ApplicationContext.MapPath(fileName), true);

                using (var outConn = new Databases.CMdbDBConnection(Sistema.ApplicationContext.MapPath(fileName)))
                {
                    outConn.OpenDB();
                    outTable = outConn.Tables.Add(Module.DisplayName);
                    // xlsTable = xlsConn.Tables(0)

                    cols = GetExportedColumns(renderer);
                    foreach (ExportableColumnInfo col in cols)
                    {
                        if (col.Selected)
                        {
                            outTable.Fields.Add(col.Key, col.TipoValore);
                        }
                    }

                    outTable.Update();
                    using (var cmd = outTable.GetInsertCommand())
                    {
                        while (!cursor.EOF())
                        {
                            foreach (ExportableColumnInfo col in cols)
                            {
                                if (col.Selected)
                                {
                                    param = (IDbDataParameter)cmd.Parameters["@" + col.Key];
                                    param.Value = outConn.ToDB(GetColumnValue(renderer, cursor.Item, col.Key));
                                }
                            }

                            cmd.ExecuteNonQuery();
                            cursor.MoveNext();
                        }

                    }

                }

            }

            string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + Module.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
            Module.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
            return url;

        }

        protected void writeCSV(System.Text.StringBuilder buffer, string text)
        {
            buffer.Append('"');
            buffer.Append(Strings.Replace(text, DMD.Strings.CStr('"'), DMD.Strings.CStr('"')));
            buffer.Append('"');
        }

            
        protected virtual string ExportCSVFormat(object renderer)
        {
            var buffer = new System.Text.StringBuilder();
            string fileName;
            CCollection<ExportableColumnInfo> cols;

            // If (Me.UseLocal) Then
            using (var cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "cursor", "")))
            {
                cursor.Reset1();
                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL + "temp"));
                fileName = WebSite.Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".csv";
                cols = GetExportedColumns(renderer);
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
                            writeCSV(buffer, Sistema.Formats.ToString(GetColumnValue(renderer, cursor.Item, col.Key)));
                            i += 1;
                        }
                    }

                    cursor.MoveNext();
                    buffer.Append(DMD.Strings.vbCrLf);
                }

            }

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString());
            string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + Module.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
            Module.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
            return url;
        }

        protected virtual string ExportTxtFormat(object renderer)
        {
            var buffer = new System.Text.StringBuilder();
            string fileName;
            CCollection<ExportableColumnInfo> cols;

            // If (Me.UseLocal) Then
            using (var cursor = (Databases.DBObjectCursorBase)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "cursor", "")))
            {
                cursor.Reset1();
                Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL + "temp"));
                fileName = WebSite.Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".txt";
                cols = GetExportedColumns(renderer);
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
                            buffer.Append(Sistema.Formats.ToString(GetColumnValue(renderer, cursor.Item, col.Key)));
                            i += 1;
                        }
                    }

                    cursor.MoveNext();
                    buffer.Append(DMD.Strings.vbCrLf);
                }

            }

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString());
            string url = "/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + Module.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_");
            Module.DispatchEvent(new Sistema.EventDescription("list_exported", "Dati esportati nel file " + fileName, fileName));
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

        protected virtual void SetColumnValue(object renderer, object item, string key, object value)
        {
            switch (key ?? "")
            {
                case "ID":
                    {
                        break;
                    }

                default:
                    {
                        var m = item.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
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
            return Module.UserCanDoAction("import");
        }

        public virtual string ImportList(object renderer)
        {
            if (!CanImport())
                throw new PermissionDeniedException(Module, "import");
            string fileName;
            Databases.CDBTable xlsTable;
            CCollection<ExportableColumnInfo> cols;
            Databases.DBObjectBase item;
            
            int s;
            s = (int)this.n2int(renderer, "s");
            if (s == 0)
            {
                return "L'importazione modificherà i record esistenti ed aggiungerà eventuali record non trovati.";
            }
            else
            {
                fileName = Strings.Trim(this.n2str(renderer, "fn"));
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentNullException("fileName");
                using (var xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName)))
                {
                    xlsConn.OpenDB();
                    xlsTable = xlsConn.Tables["Tabella"];
                    using (var xlsRis = new Databases.DBReader(xlsTable))
                    {
                        cols = GetExportedColumns(renderer);
                        using (var cursor = CreateCursor())
                        {
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
                                        SetColumnValue(renderer, item, column.Key, xlsRis.GetValue(column.Key));
                                    }
                                }

                                item.Save();
                            }

                        }
                    }
                }

                Module.DispatchEvent(new Sistema.EventDescription("list_imported", "Dati importati dal file " + fileName, null));
            }

            return "";
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        protected virtual void OnMessageLogged(string message)
        {
            MessageLogged?.Invoke(this, message);
        }

        public virtual string LoadAll(object renderer)
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

        ~CBaseModuleHandler()
        {
            DMDObject.DecreaseCounter(this);
        }

        public virtual object NewItem(object param)
        {
            using (var cursor = CreateCursor())
            {
                return cursor.Add();
            }
        }

        public CCollection<ExportableColumnInfo> GetExportableColumnList()
        {
            var o = NewItem(null);
            var props = DMD.RunTime.GetBrowsablePropertyDecriptors(o.GetType());
            var ret = new CCollection<ExportableColumnInfo>();
            foreach (PropertyDescriptor prop in props)
            {
                var col = new ExportableColumnInfo(prop.Name, prop.DisplayName, Type.GetTypeCode(prop.PropertyType), true);
                ret.Add(col);
            }

            return ret;
        }
    }
}