using System;
using System.Collections;
using DMD;
using minidom.Forms;
using minidom.internals;

namespace minidom.internals
{
    public sealed class CSystemUtilsClass
    {
        private PriorityEnum[] priority_values = new PriorityEnum[] { PriorityEnum.PRIORITY_HIGHER, PriorityEnum.PRIORITY_HIGH, PriorityEnum.PRIORITY_NORMAL, PriorityEnum.PRIOTITY_LOW, PriorityEnum.PRIORITY_LOWER };
        private string[] priority_names = new string[] { "Altissima", "Alta", "Normale", "Bassa", "Bassissima" };
        private string[] priority_icons = new string[] { "priority_highest.gif", "priority_high.gif", "priority_normal.gif", "priority_low.gif", "priority_lowest.gif" };

        internal CSystemUtilsClass()
        {
            DMDObject.IncreaseCounter(this);
        }

        public string CreateElenco(string[] values, string selValue)
        {
            return CreateElenco(values, values, selValue);
        }

        public string CreateElenco(string[] names, string[] values, string selValue)
        {
            var ret = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            if (names.Length != values.Length)
                throw new ArgumentException("Gli array names e values devono avere la stessa lunghezza");
            selValue = Strings.Trim(selValue);
            for (int i = 0, loopTo = DMD.Arrays.UBound(names); i <= loopTo; i++)
            {
                t = (Strings.LCase(selValue) ?? "") == (Strings.LCase(values[i]) ?? "");
                t1 = t1 || t;
                ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(values[i]) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(names[i]) + "</option>");
            }

            if (!t1 && !string.IsNullOrEmpty(selValue))
            {
                ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(selValue) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue) + "</option>");
            }

            return ret.ToString();
        }

        public string CreateElenco<E>(E[] values, E selValue)
        {
            var arr = new ArrayList();
            foreach (E i in values)
                arr.Add(i.ToString());
            string[] names = (string[])arr.ToArray(typeof(string));
            return CreateElenco(names, values, selValue);
        }

        public string CreateElenco<E>(string[] names, E[] values, object selValue)
        {
            if (typeof(E).IsEnum)
            {
                return CreateElencoInt(names, values, selValue);
            }
            else if (typeof(E).IsAssignableFrom(typeof(Databases.DBObjectBase)))
            {
                return CreateElencoObj(names, values, (Databases.IDBObjectBase)selValue);
            }
            else
            {
                var ret = new System.Text.StringBuilder();
                bool t = false;
                bool t1 = false;
                if (names.Length != values.Length)
                    throw new ArgumentException("Gli array names e values devono avere la stessa lunghezza");
                for (int i = 0, loopTo = DMD.Arrays.UBound(names); i <= loopTo; i++)
                {
                    t = selValue is object && selValue.Equals(values[i]);
                    t1 = t1 || t;
                    ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(values[i].ToString()) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(names[i]) + "</option>");
                }

                if (!t1 && selValue is object)
                {
                    ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(selValue.ToString()) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue.ToString()) + "</option>");
                }

                return ret.ToString();
            }
        }

        private string CreateElencoInt<E>(string[] names, E[] values, object selValue)
        {
            var ret = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            if (names.Length != values.Length)
                throw new ArgumentException("Gli array names e values devono avere la stessa lunghezza");
            for (int i = 0, loopTo = DMD.Arrays.UBound(names); i <= loopTo; i++)
            {
                t = selValue is object && selValue.Equals(values[i]);
                t1 = t1 || t;
                ret.Append("<option value=\"" + Convert.ToInt32(values[i]) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(names[i]) + "</option>");
            }

            if (!t1 && selValue is object)
            {
                ret.Append("<option value=\"" + Convert.ToInt32(selValue) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue.ToString()) + "</option>");
            }

            return ret.ToString();
        }

        private string CreateElencoObj(Array items, Databases.IDBObjectBase selValue)
        {
            var ret = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                var item = items.GetValue(i); // {i, 0})
                t = Databases.GetID((Databases.IDBObjectBase)item) == selValue.ID;
                t1 = t1 || t;
                ret.Append("<option value=\"" + Databases.GetID((Databases.IDBObjectBase)item) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(item.ToString()) + "</option>");
            }

            if (!t1 && selValue is object)
            {
                ret.Append("<option value=\"" + Databases.GetID(selValue) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue.ToString()) + "</option>");
            }

            return ret.ToString();
        }

        private string CreateElencoObj(string[] names, Array items, Databases.IDBObjectBase selValue)
        {
            var ret = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                var item = items.GetValue(i);
                t = Databases.GetID((Databases.IDBObjectBase)item) == selValue.ID;
                t1 = t1 || t;
                ret.Append("<option value=\"" + Databases.GetID((Databases.IDBObjectBase)item) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(names[i]) + "</option>");
            }

            if (!t1 && selValue is object)
            {
                ret.Append("<option value=\"" + Databases.GetID(selValue) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue.ToString()) + "</option>");
            }

            return ret.ToString();
        }

        public string FormatField(Databases.CCursorField<DateTime> field)
        {
            if (field is null)
                return DMD.Strings.vbNullString;
            switch (field.Operator)
            {
                case OP.OP_BETWEEN:
                    {
                        return Sistema.Formats.FormatUserDate(field.Value) + "..." + Sistema.Formats.FormatUserDate(field.Value1);
                    }

                case OP.OP_EQ:
                    {
                        return Sistema.Formats.FormatUserDate(field.Value);
                    }

                case OP.OP_GE:
                    {
                        return Sistema.Formats.FormatUserDate(field.Value) + "...";
                    }

                case OP.OP_LE:
                    {
                        return "..." + Sistema.Formats.FormatUserDate(field.Value);
                    }

                default:
                    {
                        throw new NotSupportedException();
                        break;
                    }
            }
        }

        public string FormatField(Databases.CCursorField<double> field)
        {
            if (field is null)
                return DMD.Strings.vbNullString;
            switch (field.Operator)
            {
                case OP.OP_BETWEEN:
                    {
                        return Sistema.Formats.FormatNumber(field.Value, 3) + "..." + Sistema.Formats.FormatNumber(field.Value1, 3);
                    }

                case OP.OP_EQ:
                    {
                        return Sistema.Formats.FormatNumber(field.Value, 3);
                    }

                case OP.OP_GE:
                    {
                        return Sistema.Formats.FormatNumber(field.Value, 3) + "...";
                    }

                case OP.OP_LE:
                    {
                        return "..." + Sistema.Formats.FormatNumber(field.Value, 3);
                    }

                default:
                    {
                        throw new NotSupportedException();
                        break;
                    }
            }
        }

        public string FormatField(Databases.CCursorField<decimal> field)
        {
            if (field is null)
                return DMD.Strings.vbNullString;
            switch (field.Operator)
            {
                case OP.OP_BETWEEN:
                    {
                        return Sistema.Formats.FormatValuta(field.Value) + "..." + Sistema.Formats.FormatValuta(field.Value1);
                    }

                case OP.OP_EQ:
                    {
                        return Sistema.Formats.FormatValuta(field.Value);
                    }

                case OP.OP_GE:
                    {
                        return Sistema.Formats.FormatValuta(field.Value) + "...";
                    }

                case OP.OP_LE:
                    {
                        return "..." + Sistema.Formats.FormatValuta(field.Value);
                    }

                default:
                    {
                        throw new NotSupportedException();
                        break;
                    }
            }
        }

        public string FormatField(Databases.CCursorField<int> field)
        {
            if (field is null)
                return DMD.Strings.vbNullString;
            switch (field.Operator)
            {
                case OP.OP_BETWEEN:
                    {
                        return Sistema.Formats.FormatInteger(field.Value) + "..." + Sistema.Formats.FormatInteger(field.Value1);
                    }

                case OP.OP_EQ:
                    {
                        return Sistema.Formats.FormatInteger(field.Value);
                    }

                case OP.OP_GE:
                    {
                        return Sistema.Formats.FormatInteger(field.Value) + "...";
                    }

                case OP.OP_LE:
                    {
                        return "..." + Sistema.Formats.FormatInteger(field.Value);
                    }

                default:
                    {
                        throw new NotSupportedException();
                        break;
                    }
            }
        }

        public void ParseCursorField(Databases.CCursorField<DateTime> field, string text)
        {
            text = Strings.Trim(text);
            field.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            var items = Strings.Split(Strings.Trim(text), "...");
            if (DMD.Arrays.UBound(items) > 0)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    field.Operator = OP.OP_LE;
                    field.Value = Sistema.Formats.ParseDate(items[1]);
                }
                else if (string.IsNullOrEmpty(items[1]))
                {
                    field.Operator = OP.OP_GE;
                    field.Value = Sistema.Formats.ParseDate(items[0]);
                }
                else
                {
                    field.Operator = OP.OP_BETWEEN;
                    field.Value = Sistema.Formats.ParseDate(items[0]);
                    field.Value1 = Sistema.Formats.ParseDate(items[1]);
                }
            }
            else
            {
                field.Value = Sistema.Formats.ParseDate(items[0]);
                field.Operator = OP.OP_EQ;
            }
        }

        public void ParseCursorField(Databases.CCursorField<double> field, string text)
        {
            text = Strings.Trim(text);
            field.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            var items = Strings.Split(Strings.Trim(text), "...");
            if (DMD.Arrays.UBound(items) > 0)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    field.Operator = OP.OP_LE;
                    field.Value = Sistema.Formats.ParseDouble(items[1]);
                }
                else if (string.IsNullOrEmpty(items[1]))
                {
                    field.Operator = OP.OP_GE;
                    field.Value = Sistema.Formats.ParseDouble(items[0]);
                }
                else
                {
                    field.Operator = OP.OP_BETWEEN;
                    field.Value = Sistema.Formats.ParseDouble(items[0]);
                    field.Value1 = Sistema.Formats.ParseDouble(items[1]);
                }
            }
            else
            {
                field.Value = Sistema.Formats.ParseDouble(items[0]);
                field.Operator = OP.OP_EQ;
            }
        }

        public void ParseCursorField(Databases.CCursorField<decimal> field, string text)
        {
            text = Strings.Trim(text);
            field.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            var items = Strings.Split(Strings.Trim(text), "...");
            if (DMD.Arrays.UBound(items) > 0)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    field.Operator = OP.OP_LE;
                    field.Value = (decimal?)Sistema.Formats.ParseDouble(items[1]);
                }
                else if (string.IsNullOrEmpty(items[1]))
                {
                    field.Operator = OP.OP_GE;
                    field.Value = (decimal?)Sistema.Formats.ParseDouble(items[0]);
                }
                else
                {
                    field.Operator = OP.OP_BETWEEN;
                    field.Value = (decimal?)Sistema.Formats.ParseDouble(items[0]);
                    field.Value1 = (decimal?)Sistema.Formats.ParseDouble(items[1]);
                }
            }
            else
            {
                field.Value = (decimal?)Sistema.Formats.ParseDouble(items[0]);
                field.Operator = OP.OP_EQ;
            }
        }

        public void ParseCursorField(Databases.CCursorField<int> field, string text)
        {
            string[] items;
            text = Strings.Trim(text);
            field.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            if (Strings.InStr(text, DMD.Strings.vbTab) > 0)
            {
                var arr = new ArrayList();
                items = Strings.Split(text, DMD.Strings.vbTab);
                if (DMD.Arrays.Len(items) > 0)
                {
                    for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                    {
                        if (!string.IsNullOrEmpty(Strings.Trim(items[i])))
                            arr.Add(Sistema.Formats.ToInteger(items[i]));
                    }
                }

                if (arr.Count > 0)
                    field.ValueIn(arr.ToArray());
            }
            else
            {
                items = Strings.Split(Strings.Trim(text), "...");
                if (DMD.Arrays.UBound(items) > 0)
                {
                    if (string.IsNullOrEmpty(items[0]))
                    {
                        field.Operator = OP.OP_LE;
                        field.Value = Sistema.Formats.ParseInteger(items[1]);
                    }
                    else if (string.IsNullOrEmpty(items[1]))
                    {
                        field.Operator = OP.OP_GE;
                        field.Value = Sistema.Formats.ParseInteger(items[0]);
                    }
                    else
                    {
                        field.Operator = OP.OP_BETWEEN;
                        field.Value = Sistema.Formats.ParseInteger(items[0]);
                        field.Value1 = Sistema.Formats.ParseInteger(items[1]);
                    }
                }
                else
                {
                    field.Value = Sistema.Formats.ParseInteger(items[0]);
                    field.Operator = OP.OP_EQ;
                }
            }
        }

        public string CreateElencoUtenti(Sistema.CUser selValue, bool onlyValid = true)
        {
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            var col = new CCollection<Sistema.CUser>();
            string lastName = DMD.Strings.vbNullString;
            foreach (Sistema.CUser user in Sistema.Users.LoadAll())
            {
                if (user.Visible && (onlyValid == false || user.UserStato == Sistema.UserStatus.USER_ENABLED))
                {
                    col.Add(user);
                }
            }

            col.Sort();
            foreach (Sistema.CUser user in col)
            {
                t = Databases.GetID(user) == Databases.GetID(selValue);
                t1 = t1 || t;
                if ((lastName ?? "") == (user.Nominativo ?? ""))
                {
                    writer.Append("<option value=\"");
                    writer.Append(Databases.GetID(user));
                    writer.Append("\" ");
                    if (t)
                        writer.Append("selected");
                    writer.Append("");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(user.Nominativo));
                    writer.Append(" (");
                    writer.Append(user.UserName);
                    writer.Append(")");
                    writer.Append("</option>");
                }
                else
                {
                    writer.Append("<option value=\"");
                    writer.Append(Databases.GetID(user));
                    writer.Append("\" ");
                    if (t)
                        writer.Append("selected");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(user.Nominativo));
                    writer.Append("</option>");
                }
            }

            if (selValue is object && t1 == false)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.UserName));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public void SendPasswordsToEMail(string eMail)
        {
            eMail = Strings.Trim(eMail);
            if (string.IsNullOrEmpty(eMail))
                throw new ArgumentNullException("eMail");
            var cursor = new Sistema.CUserCursor();
            var writer = new System.Text.StringBuilder();
            try
            {
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.eMail.Value = eMail;
                cursor.UserName.SortOrder = Databases.SortEnum.SORT_ASC;
                cursor.IgnoreRights = true;
                if (cursor.Count() == 0)
                    throw new ArgumentException("Non esiste alcun account associato a questo indirizzo e-mail");
                writer.Append("<html>");
                writer.Append("<body>");
                writer.Append("<h2>Procedura di recupero dati degli account</h2>");
                writer.Append("&Egrave; stata effettuata una richiesta per recuperare gli account dell'utente sul sito " + "<a href=\"" + Sistema.ApplicationContext.BaseURL + "\">" + Sistema.ApplicationContext.Title + "</a><br/>");
                writer.Append("<br/>");
                writer.Append("<table style=\"border-collapse:collapse;\" border=\"1\" cellsapcing=\"0\" cellpadding=\"0\">");
                writer.Append("<tr>");
                writer.Append("<th style=\"width:120px;\">Username</th>");
                writer.Append("<th style=\"width:120px;\">Password</th>");
                writer.Append("</tr>");
                while (!cursor.EOF())
                {
                    writer.Append("<tr>");
                    writer.Append("<td style=\"width:120px;\">" + DMD.WebUtils.HtmlEncode(cursor.Item.UserName) + "</td>");
                    writer.Append("<td>" + DMD.WebUtils.HtmlEncode(Sistema.Formats.ToString(Databases.APPConn.ExecuteScalar("SELECT [UsrPwd] FROM [tbl_Users] WHERE [ID]=" + Databases.GetID(cursor.Item)))) + "</td>");
                    writer.Append("</tr>");
                    cursor.MoveNext();
                }

                cursor.Dispose();
                cursor = null;
                writer.Append("</table>");
                writer.Append("<table>");
                writer.Append("<tr>");
                writer.Append("   <td>Data: " + DMD.DateUtils.Now() + "</td>");
                writer.Append("</tr>");
                writer.Append("<tr>");
                writer.Append("   <td>Sessione: " + WebSite.ASP_Session.SessionID + "</td>");
                writer.Append("</tr>");
                writer.Append("<tr>");
                writer.Append("   <td>IP Remoto: " + WebSite.Instance.CurrentSession.RemoteIP + ":" + WebSite.Instance.CurrentSession.RemotePort + "</td>");
                writer.Append("</tr>");
                writer.Append("</table>");
                writer.Append("<BR/>");
                writer.Append("</body>");
                writer.Append("</html>");
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

            System.Net.Mail.MailMessage m = Sistema.EMailer.PrepareMessage(Sistema.EMailer.Config.SMTPUserName, eMail, "", "", Sistema.ApplicationContext.Title + " - Procedura di recupero password", writer.ToString(), "", true);
            Sistema.EMailer.SendMessageAsync((Net.Mail.MailMessageEx)m, true);

            // writer.Dispose()
        }

        public void SendUserPassword(Sistema.CUser user)
        {
            var writer = new System.Text.StringBuilder();
            if (user is null)
                throw new ArgumentNullException("User");
            writer.Append("<html>");
            writer.Append("<body>");
            writer.Append("<h2>Sistema di recupero password</h2>");
            writer.Append("Si prega di non rispondere a questa e-mail.<br/>");
            writer.Append("&Egrave; stata effettuata una richiesta per recuperare la password associata all'utente: <b>" + user.Nominativo + "</b>");
            writer.Append("<table>");
            writer.Append("<tr>");
            writer.Append("<th>UserName</th>");
            writer.Append("<th>Password</th>");
            writer.Append("</tr>");
            writer.Append("<tr>");
            writer.Append("<td>" + DMD.WebUtils.HtmlEncode(user.UserName) + "</td>");
            writer.Append("<td>" + DMD.WebUtils.HtmlEncode(Sistema.Formats.ToString(Databases.APPConn.ExecuteScalar("SELECT [UsrPwd] FROM [tbl_Users] WHERE [ID]=" + Databases.GetID(user)))) + "</td>");
            writer.Append("</tr>");
            writer.Append("</table>");
            writer.Append("<br/>");
            writer.Append("<hr/>");
            writer.Append("<table>");
            writer.Append("<tr>");
            writer.Append("<td>Data</td>");
            writer.Append("<td>" + DMD.DateUtils.Now() + "</td>");
            writer.Append("</tr>");
            writer.Append("<tr>");
            writer.Append("<td>Sessione</td>");
            writer.Append("<td>" + WebSite.Instance.CurrentSession.SessionID + "</td>");
            writer.Append("</tr>");
            writer.Append("<tr>");
            writer.Append("<td>IP Remoto</td>");
            writer.Append("<td>" + WebSite.Instance.CurrentSession.RemoteIP + ":" + WebSite.Instance.CurrentSession.RemotePort + "</td>");
            writer.Append("</tr>");
            writer.Append("<tr>");
            writer.Append("<td>Sistema</td>");
            writer.Append("<td><a href=\"" + Sistema.ApplicationContext.BaseURL + "\">" + Sistema.ApplicationContext.Title + "</a></td>");
            writer.Append("</tr>");
            writer.Append("</table>");
            writer.Append("</body>");
            writer.Append("</html>");
            System.Net.Mail.MailMessage m = Sistema.EMailer.PrepareMessage(Sistema.EMailer.Config.SMTPUserName, user.eMail, "", "", Sistema.ApplicationContext.Title + ": Procedura di recupero password per l'utente " + user.Nominativo, writer.ToString(), "", true);
            Sistema.EMailer.SendMessageAsync((Net.Mail.MailMessageEx)m, true);

            // writer.Dispose()
        }

        public string CreateElencoPossibiliGenitori(int objId, int selValue)
        {
            var cursor = new Sistema.CModulesCursor();
            var writer = new System.Text.StringBuilder();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.ID.Value = objId;
            cursor.ID.Operator = OP.OP_NE;
            cursor.DisplayName.SortOrder = Databases.SortEnum.SORT_ASC;
            while (!cursor.EOF())
            {
                var item = cursor.Item;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(item));
                writer.Append("\" ");
                if (Databases.GetID(item) == selValue)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(item.DisplayName));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            string ret = writer.ToString();
            // writer.Dispose()
            return ret;
        }

        public string CreateElencoStatoUtente(Sistema.UserStatus value)
        {
            var values = new Sistema.UserStatus[] { Sistema.UserStatus.USER_DISABLED, Sistema.UserStatus.USER_DELETED, Sistema.UserStatus.USER_NEW, Sistema.UserStatus.USER_SUSPENDED, Sistema.UserStatus.USER_ENABLED };
            var names = new[] { "Disabilitati", "Eliminati", "Da Attivare", "Sospesi", "Abilitati" };
            int i;
            var writer = new System.Text.StringBuilder();
            writer.Append("<option value=\"-1\">Tutto</option>");
            var loopTo = DMD.Arrays.UBound(values);
            for (i = 0; i <= loopTo; i++)
            {
                writer.Append("<option value=\"");
                writer.Append((int)values[i]);
                writer.Append("\" ");
                if (values[i] == value)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(names[i]));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoGruppo(int value)
        {
            Sistema.CGroupCursor cursor;
            var writer = new System.Text.StringBuilder();
            cursor = new Sistema.CGroupCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.GroupName.SortOrder = Databases.SortEnum.SORT_ASC;
            writer.Append("<option></option>");
            while (!cursor.EOF())
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == value)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.GroupName));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            return writer.ToString();
        }

        /// <summary>
        /// Crea la sequenza di &gt;option&lt; relativa ai periodo supportati dal calendario
        /// </summary>
        /// <param name="selValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string CreateElencoPeriodi(string selValue)
        {
            var items = DMD.DateUtils.GetSupportedPeriods();
            return CreateElenco(items, selValue);
        }

        public string CreateElencoPriority(PriorityEnum currValue)
        {
            int i;
            var writer = new System.Text.StringBuilder();
            var loopTo = DMD.Arrays.UBound(priority_values);
            for (i = DMD.Arrays.LBound(priority_values); i <= loopTo; i++)
            {
                if (priority_values[i] == currValue)
                {
                    writer.Append("<option value=\"");
                    writer.Append((int)priority_values[i]);
                    writer.Append("\" selected=\"selected\">");
                    writer.Append(priority_names[i]);
                    writer.Append("</option>");
                }
                else
                {
                    writer.Append("<option value=\"");
                    writer.Append((int)priority_values[i]);
                    writer.Append("\">");
                    writer.Append(priority_names[i]);
                    writer.Append("</option>");
                }
            }

            return writer.ToString();
        }

        // ---------------------------------------------------------
        public string GetPriorityText(PriorityEnum value)
        {
            string ret;
            switch (value)
            {
                case (PriorityEnum)(-1):
                    {
                        ret = "Alta";
                        break;
                    }

                case 0:
                    {
                        ret = "Normale";
                        break;
                    }

                case (PriorityEnum)1:
                    {
                        ret = "Bassa";
                        break;
                    }

                default:
                    {
                        if ((int)value <= -2)
                        {
                            ret = "Altissima";
                        }
                        else
                        {
                            ret = "Bassissima";
                        }

                        break;
                    }
            }

            return ret;
        }

        public string GetPriorityIcon(PriorityEnum currValue)
        {
            int i;
            var loopTo = DMD.Arrays.UBound(priority_values);
            for (i = 0; i <= loopTo; i++)
            {
                if (priority_values[i] == currValue)
                {
                    return "/minidom/widgets/images/" + priority_icons[i];
                }
            }

            return DMD.Strings.vbNullString;
        }

        public string GetFriendlyContextName(string value)
        {
            var values = new[] { "CPraticaCQSPD" };
            var names = new[] { "Pratica" };
            int p = DMD.Strings.InStr(value, "(");
            string tipoContesto = DMD.Strings.Trim(DMD.Strings.Left(value, p - 1));
            string idContesto = DMD.Strings.Mid(value, p + 1, DMD.Strings.Len(value) - 1 - p);
            int i = DMD.Arrays.IndexOf(values, tipoContesto);
            if (i >= 0)
                tipoContesto = names[i];
            return tipoContesto + " (" + idContesto + ")";
        }

        public string CreateElencoTipoContestoAnnotazione(object obj, string selValue)
        {
            var tipi = Sistema.Annotazioni.GetTipiContestoPerOggetto(obj);
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            selValue = DMD.Strings.Trim(selValue);
            foreach (string tipo in tipi)
            {
                t = (tipo ?? "") == (selValue ?? "");
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(tipo));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(minidom.Forms.Utils.SystemUtils.GetFriendlyContextName(tipo)));
                writer.Append("</option>");
            }

            if (!string.IsNullOrEmpty(selValue) && !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("\" selected style=\"colo:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(minidom.Forms.Utils.SystemUtils.GetFriendlyContextName(selValue)));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        ~CSystemUtilsClass()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}

namespace minidom.Forms
{
    public partial class Utils
    {
        private static CSystemUtilsClass m_SystemUtils = null;

        public static CSystemUtilsClass SystemUtils
        {
            get
            {
                if (m_SystemUtils is null)
                    m_SystemUtils = new CSystemUtilsClass();
                return m_SystemUtils;
            }
        }
    }
}