using System;
using System.Data;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom.Forms
{



    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class TabelleAssicurativeModuleHandler : CBaseModuleHandler
    {
        public TabelleAssicurativeModuleHandler() : base(Finanziaria.TabelleAssicurative.Module, ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CTabelleAssicurativeCursor();
        }

        public string GetVincoliPXT(object renderer)
        {
            int id =(int) this.n2int(renderer, "id");
            using (var cursor = new Finanziaria.CProdottoXTabellaAssCursor())
            {
                cursor.ID.Value = (int?)id;
                var item = cursor.Item;
                return DMD.XML.Utils.Serializer.Serialize(item.Vincoli.ToArray(), XMLSerializeMethod.Document);
            }
        }

        public string GetTabellaXProdottoByID(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            var item = Finanziaria.TabelleAssicurative.GetTabellaXProdottoByID(id);
            if (item is null)
            {
                return "";
            }
            else
            {
                return DMD.XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document);
            }
        }

        public string GetArrayNomiTabelle(object renderer)
        {
            var writer = new System.Text.StringBuilder();
            Finanziaria.CTabelleAssicurativeCursor cursor = null;
            try
            {
                cursor = new Finanziaria.CTabelleAssicurativeCursor();
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
                writer.Append("<list>");
                while (!cursor.EOF())
                {
                    writer.Append("<item>");
                    writer.Append("<value>");
                    writer.Append(Databases.GetID(cursor.Item));
                    writer.Append("</value>");
                    writer.Append("<text>");
                    writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                    writer.Append("</text>");
                    writer.Append("<attribute>");
                    if (cursor.Item.IsValid())
                    {
                        writer.Append("1");
                    }
                    else
                    {
                        writer.Append("0");
                    }

                    writer.Append("</attribute>");
                    writer.Append("</item>");
                    cursor.MoveNext();
                }

                writer.Append("</list>");
                return writer.ToString();
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

        public string ExportCoefficienti(object renderer)
        {
            if (!Module.UserCanDoAction("export_coeff"))
                throw new PermissionDeniedException(Module, "export_coeff");
            int idTabella = (int)this.n2int(renderer, "id");
            var tabella = Finanziaria.TabelleAssicurative.GetItemById(idTabella);
            if (tabella is null)
                throw new ArgumentNullException("Tabella");
            string fileName;
            Databases.CXlsDBConnection xlsConn;
            Databases.CDBTable xlsTable;
            int i;
            Sistema.FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL + "temp"));
            fileName = WebSite.Instance.Configuration.PublicURL + "temp/" + Sistema.ASPSecurity.GetRandomKey(12) + ".xls";
            Sistema.FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/xlsfile.xls"), Sistema.ApplicationContext.MapPath(fileName), true);
            xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName));
            xlsConn.OpenDB();
            xlsTable = xlsConn.Tables.Add("Tabella");
            // xlsTable = xlsConn.Tables(0)

            xlsTable.Fields.Add("Anni", typeof(int));
            xlsTable.Fields.Add("Sesso", typeof(string));
            for (i = 24; i <= 120; i += 12)
                xlsTable.Fields.Add("C" + i, typeof(double));
            xlsTable.Update();
            var cmd = xlsTable.GetInsertCommand();
            IDbDataParameter param;
            foreach (Finanziaria.CCoefficienteAssicurativo row in tabella.Coefficienti)
            {
                param = (IDbDataParameter)cmd.Parameters["@Anni"];
                param.Value = xlsConn.ToDB(row.Anni);
                param = (IDbDataParameter)cmd.Parameters["@Sesso"];
                param.Value = xlsConn.ToDB(row.Sesso);
                for (i = 24; i <= 120; i += 12)
                {
                    param = (IDbDataParameter)cmd.Parameters["@C" + i];
                    param.Value = xlsConn.ToDB(row.get_Coefficiente(i));
                }

                cmd.ExecuteNonQuery();
            }

            xlsConn.Dispose();

            // Try
            // minidom.Excel.ExcelUtils.DeleteWorkSheetFromFile(xlsConn.Path, "Tabella0")
            // minidom.Excel.ExcelUtils.FormatStandardTable(xlsConn.Path, "Tabella")
            // Catch ex As Exception

            // End Try

            // Dim token As String = ASPSecurity.CreateToken(fileName, fileName).Token
            WebSite.ASP_Server.Transfer("/minidom/widgets/websvc/download.aspx?fp=" + fileName + "&dn=" + Module.ModuleName + " " + Strings.Replace(Strings.Replace(DMD.Strings.CStr(DMD.DateUtils.Now()), "/", "_"), ":", "_"));
            // WebSite.ServeFile(Server.MapPath(fileName), fileName, ServeFileMode.default)
            return DMD.Strings.vbNullString;
        }

        public string ImportCoefficienti(object renderer)
        {
            if (!Module.UserCanDoAction("import_coeff"))
                throw new PermissionDeniedException(Module, "import_coeff");
            int idTabella = (int)this.n2int(renderer, "id");
            var tabella = Finanziaria.TabelleAssicurative.GetItemById(idTabella);
            if (tabella is null)
                throw new ArgumentNullException("Tabella");
            string fileName;
            Databases.CXlsDBConnection xlsConn;
            Databases.CDBTable xlsTable;
            Databases.DBReader xlsRis;
            int s;
            Finanziaria.CCoefficienteAssicurativo item;
            Finanziaria.CCoefficientiAssicurativiCursor cursor;
            s = (int)this.n2int(renderer, "s");
            if (s == 0)
            {
                return "L'importazione sostituirà completamente tutti i coefficienti della tabella [" + tabella.Nome + "]";
            }
            else
            {
                fileName = Strings.Trim(this.n2str(renderer, "fn", DMD.Strings.vbNullString));
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentNullException("fileName");
                xlsConn = new Databases.CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName));
                xlsConn.OpenDB();
                xlsTable = xlsConn.Tables["Tabella"];
                xlsRis = new Databases.DBReader(xlsTable);
                cursor = new Finanziaria.CCoefficientiAssicurativiCursor();
                cursor.Tabella.Value = idTabella;
                cursor.ID.SortOrder = Databases.SortEnum.SORT_ASC;
                while (xlsRis.Read())
                {
                    if (cursor.EOF())
                    {
                        item = (Finanziaria.CCoefficienteAssicurativo)cursor.Add();
                        item.Tabella = tabella;
                        item.Anni = (int)xlsRis.GetValue<int>("Anni");
                        item.Sesso = xlsRis.GetValue<string>("Sesso");
                        for (int i = 24; i <= 120; i += 12)
                            item.set_Coefficiente(i, xlsRis.GetValue<double>("C" + i));
                        cursor.Update();
                    }
                    else
                    {
                        item = cursor.Item;
                        item.Tabella = tabella;
                        item.Anni = (int)xlsRis.GetValue<int>("Anni");
                        item.Sesso = xlsRis.GetValue<string>("Sesso");
                        for (int i = 24; i <= 120; i += 12)
                            item.set_Coefficiente(i, xlsRis.GetValue<double>("C" + i));
                        cursor.Update();
                        cursor.MoveNext();
                    }
                }

                while (!cursor.EOF())
                {
                    cursor.Item.Delete();
                    cursor.MoveNext();
                }

                xlsConn.Dispose();
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
            }

            return "";
        }
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


}