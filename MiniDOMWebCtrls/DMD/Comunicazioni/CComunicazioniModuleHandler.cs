using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;

namespace minidom.Forms
{
    public class CComunicazioniModuleHandler
        : CBaseModuleHandler<Circolare>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CComunicazioniModuleHandler() 
            : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        /// <summary>
        /// Crea il cursore
        /// </summary>
        /// <returns></returns>
        public override  DBObjectCursorBase CreateCursor()
        {
            return new  CircolariCursor();
        }

        // Public Function read(ByVal renderer As Object) As String
        // Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "ID"))
        // Dim comunicazione As Circolare = Comunicazioni.GetItemById(id)
        // If Not Me.CanRead(comunicazione) Then Throw New PermissionDeniedException

        // Dim writer As New HTMLWriter
        // writer.WriteRowData("<div style=""padding:3px;width:" & CStr(DirectCast(renderer, IModuleRenderer).Width) & "px;height:60px;overflow:hidden;white-space:nowrap;background-color:#e0e0e0;"">")
        // writer.WriteRowData("<table cellspacing=""0"" cellpadding=""0"" style=""width:" & CStr(DirectCast(renderer, IModuleRenderer).Width - 20) & "px;border-collapse:collapse;"">")
        // writer.WriteRowData("<tr>")
        // writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Oggetto:</td>")
        // writer.WriteRowData("<td>" & WebUtils.HtmlEncode(comunicazione.Descrizione) & "</td>")
        // writer.WriteRowData("</tr>")
        // writer.WriteRowData("<tr>")
        // writer.WriteRowData("<td colspan=""2"">")

        // writer.WriteRowData("<table cellspacing=""0"" cellpadding=""0"" style=""border-collapse:collapse;"">")
        // writer.WriteRowData("<tr>")
        // writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Data:</td>")
        // writer.WriteRowData("<td style=""min-width:100px;"">" & Formats.FormatUserDate(comunicazione.DataInizio) & CStr(IIf(comunicazione.DataFine.HasValue, " - " & Formats.FormatUserDate(comunicazione.DataFine), "")) & "</td>")
        // writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Categoria:</td>")
        // writer.WriteRowData("<td style=""min-width:100px;"">" & WebUtils.HtmlEncode(comunicazione.Categoria) & "</td>")
        // writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Autore:</td>")
        // writer.WriteRowData("<td style=""min-width:100px;"">" & WebUtils.HtmlEncode(comunicazione.CreatoDa.Nominativo) & "</td>")
        // writer.WriteRowData("</tr>")
        // writer.WriteRowData("</table>")

        // writer.WriteRowData("</td>")
        // writer.WriteRowData("</tr>")

        // writer.WriteRowData("<tr>")
        // writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Allegati</td>")
        // writer.WriteRowData("<td><a href=""" & comunicazione.URL & """ target=""_blank"">" & comunicazione.URL & "</a></td>")
        // writer.WriteRowData("</tr>")
        // writer.WriteRowData("<tr>")
        // writer.WriteRowData("<td colspan=""2"" style=""text-align:right;""><input type=""button"" name=""btnClose"" onclick=""Window.Close(); return false;"" value=""Chiudi"" /></td>")
        // writer.WriteRowData("</tr>")
        // writer.WriteRowData("</table>")
        // writer.WriteRowData("</div>")
        // writer.WriteRowData("<div style=""padding:5px;width:" & CStr(DirectCast(renderer, IModuleRenderer).Width - 20) & "px;height:" & CStr(DirectCast(renderer, IModuleRenderer).Height - 80) & "px;overflow:auto;white-space:normal;"">")
        // writer.WriteRowData(comunicazione.Note)
        // writer.WriteRowData("</div>")
        // Dim ret As String = writer.ToString
        // writer.Dispose()
        // Return ret
        // End Function

        /// <summary>
        /// Restituisce true se l'utente corrente può visualizzare l'oggetto
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanRead(Circolare item)
        {
            return Office.Circolari.Module.UserCanDoAction("read");
        }

        public string Notify(object renderer)
        {
            int id = (int)this.n2int(renderer, "ID");
            string via = this.n2str(renderer, "via");
            var c = Office.Circolari.GetItemById(id);
            if (!CanEdit(c))
                throw new PermissionDeniedException(Module, "edit");
            var ret = Office.Circolari.Notify(c, via);
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string notifyViaEMail(object renderer)
        {
            int id;
            Office.Circolare item;
            id = (int)this.n2int(renderer, "ID");
            item = Office.Circolari.GetItemById(id);
            // item.NotifyViaEMail()
            return "";
        }

        public string getAffectedUsers(object renderer)
        {
            int id;
            Office.Circolare item;
            CCollection<Sistema.CUser> items;
            int i;
            var writer = new System.Text.StringBuilder();
            id = (int)this.n2int(renderer, "ID");
            item = Office.Circolari.GetItemById(id);
            items = item.GetAffectedUsers();
            writer.Append("<list>");
            var loopTo = items.Count - 1;
            for (i = 0; i <= loopTo; i++)
            {
                {
                    var withBlock = items[i];
                    if (!string.IsNullOrEmpty(withBlock.eMail))
                    {
                        writer.Append("<text>");
                        writer.Append(DMD.WebUtils.HtmlEncode(withBlock.Nominativo));
                        writer.Append(" &lt;");
                        writer.Append(withBlock.eMail);
                        writer.Append("&gt;");
                        writer.Append("</text>");
                        writer.Append("<value>");
                        writer.Append(Databases.GetID(items[i], 0));
                        writer.Append("</value>");
                    }
                }
            }

            writer.Append("</list>");
            return writer.ToString();
        }

        public string notifyTo(object renderer)
        {
            //string notifyToRet = default;
            //int id;
            //Office.Circolare item;
            //string[] tgt;
            //int i;
            //string ret;
            //id = (int)this.n2int(renderer, "ID", ""));
            //tgt = Strings.Split(this.n2str(renderer, "tgt", "")), ";");
            //item = Office.Comunicazioni.GetItemById(id);
            //ret = "";
            //var loopTo = DMD.Arrays.UBound(tgt);
            //for (i = 0; i <= loopTo; i++)
            //{
            //    // item.NotifyViaEMailTo(Formats.ToInteger(tgt(i)))
            //    if (Information.Err().Number != 0)
            //    {
            //        ret = ret + "0x" + DMD.Integers.Hex(Information.Err().Number) + " " + Information.Err().Description + DMD.Strings.vbCrLf;
            //    }
            //}

            //notifyToRet = ret;
            //return notifyToRet;
            throw new NotImplementedException();
        }

        public string SetUserAllowNegate(object renderer)
        {
            int itemID = (int)this.n2int(renderer, "cid");
            int userID = (int)this.n2int(renderer, "uid");
            bool allow = (bool)this.n2bool(renderer, "a");
            Office.Circolare item = (Office.Circolare)GetInternalItemById(itemID);
            if (CanEdit(item) == false)
            {
                throw new PermissionDeniedException();
            }

            item.SetUserAllowNegate(userID, allow);
            return DMD.Strings.vbNullString;
        }

        public string SetGroupAllowNegate(object renderer)
        {
            int itemID = (int)this.n2int(renderer, "cid");
            int groupID = (int)this.n2int(renderer, "gid");
            bool allow = (bool)this.n2bool(renderer, "a");
            Office.Circolare item = (Office.Circolare)GetInternalItemById(itemID);
            if (CanEdit(item) == false)
            {
                throw new PermissionDeniedException();
            }

            item.SetGroupAllowNegate(groupID, allow);
            return DMD.Strings.vbNullString;
        }

        public string setRead(object renderer)
        {
            int cid = (int)this.n2int(renderer, "cid");
            var cursor = new Office.NotificaUtenteXCircolareCursor();
            Office.NotificaUtenteXCircolare alert;
            cursor.ID.Value = cid;
            alert = cursor.Item;
            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            alert.StatoComunicazione = Office.StatoNotificaUtenteXCircolare.LETTO;
            alert.DataLettura = DMD.DateUtils.Now();
            alert.Save();
            return "";
        }
    }
}