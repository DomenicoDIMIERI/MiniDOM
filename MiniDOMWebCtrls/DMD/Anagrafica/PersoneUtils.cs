using System.Collections;
using System.Linq;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.Databases;
using static minidom.Anagrafica;
using minidom.Forms;
using internals;

namespace internals
{

    public sealed class CPersoneUtilsClass
    {
        public CPersoneUtilsClass()
        {
            DMDObject.IncreaseCounter(this);
        }

        public string CreateElencoUfficiConsentiti(Anagrafica.CUfficio selItem)
        {
            var items = Anagrafica.Uffici.GetPuntiOperativiConsentiti();
            var ret = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            {
                var u = items[i];
                if (u.Attivo)
                {
                    t1 = minidom.Databases.GetID(u) == minidom.Databases.GetID(selItem);
                    t = t || t1;
                    ret.Append("<option value=\"" + minidom.Databases.GetID(u) + "\" " + Sistema.IIF(t1, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(u.Nome) + "</option>");
                }
            }

            if (t == false && selItem is object)
            {
                ret.Append("<option value=\"" + minidom.Databases.GetID(selItem) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selItem.Nome) + "</option>");
            }

            return ret.ToString();
        }

        public string CreateElencoTipoCanale(string selValue, bool onlyValid = true)
        {
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            var items = Anagrafica.Canali.GetTipiCanale(onlyValid);
            selValue = Strings.Trim(selValue);
            for (int i = 0, loopTo = DMD.Arrays.Len(items) - 1; i <= loopTo; i++)
            {
                string str = items[i];
                t = DMD.Strings.Compare(str, selValue, true) == 0;
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(str));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(str));
                writer.Append("</option>");
            }

            if (!string.IsNullOrEmpty(selValue) && !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoCanali(string tipo, Anagrafica.CCanale selValue, bool onlyValid = true)
        {
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            var cursor = new Anagrafica.CCanaleCursor();
            cursor.Stato.Value = minidom.Databases.ObjectStatus.OBJECT_VALID;
            if (onlyValid)
                cursor.Valid.Value = true;
            cursor.Nome.SortOrder = minidom.Databases.SortEnum.SORT_ASC;
            cursor.Tipo.Value = Strings.Trim(tipo);
            cursor.IgnoreRights = true;
            while (!cursor.EOF())
            {
                var item = cursor.Item;
                t = minidom.Databases.GetID(item) == minidom.Databases.GetID(selValue);
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(minidom.Databases.GetID(item));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(" imgattr=\"");
                writer.Append(DMD.WebUtils.URLEncode(item.IconURL));
                writer.Append("\">");
                writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            cursor.Dispose();
            if (selValue is object && !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(minidom.Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\" imgattr=\"");
                writer.Append(DMD.WebUtils.URLEncode(selValue.IconURL));
                writer.Append("\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoFonti(string tipoFonte, IFonte fonte, bool onlyValid = true)
        {
            var provider = Anagrafica.Fonti.GetProviderByName(tipoFonte);
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            if (provider is object)
            {
                var items = provider.GetItemsAsArray(tipoFonte, onlyValid);
                if (items is object)
                {
                    for (int i = 0, loopTo = items.Count() - 1; i <= loopTo; i++)
                    {
                        var item = items[i];
                        t = minidom.Databases.GetID((minidom.Databases.IDBObjectBase)item) == minidom.Databases.GetID((minidom.Databases.IDBObjectBase)fonte);
                        t1 = t1 || t;
                        writer.Append("<option value=\"");
                        writer.Append(minidom.Databases.GetID((minidom.Databases.IDBObjectBase)item));
                        writer.Append("\" ");
                        if (t)
                            writer.Append("selected");
                        writer.Append(" imgattr=\"");
                        writer.Append(DMD.WebUtils.URLEncode(item.IconURL));
                        writer.Append("\">");
                        writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                        writer.Append("</option>");
                    }
                }
            }

            if (fonte is object && !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(minidom.Databases.GetID((minidom.Databases.IDBObjectBase)fonte));
                writer.Append("\" selected style=\"color:red;\" imgattr=\"");
                writer.Append(DMD.WebUtils.URLEncode(fonte.IconURL));
                writer.Append("\">");
                writer.Append(DMD.WebUtils.HtmlEncode(fonte.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoTipoFonte(string selValue)
        {
            var col = new ArrayList();
            var fontiP = Anagrafica.Fonti.Providers;
            for (int i = 0, loopTo = fontiP.Count - 1; i <= loopTo; i++)
            {
                var p = fontiP[i];
                var names = p.GetSupportedNames();
                for (int j = 0, loopTo1 = DMD.Arrays.UBound(names); j <= loopTo1; j++)
                    col.Add(names[j]);
            }

            col.Sort();
            var buffer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            selValue = Strings.Trim(selValue);
            foreach (string n in col)
            {
                t = (n ?? "") == (selValue ?? "");
                t1 = t1 || t;
                buffer.Append("<option value=\"");
                buffer.Append(DMD.WebUtils.HtmlEncode(n));
                buffer.Append("\" ");
                buffer.Append(Sistema.IIF(t, "selected", ""));
                buffer.Append(">");
                buffer.Append(DMD.WebUtils.HtmlEncode(n));
                buffer.Append("</option>");
            }

            if (!string.IsNullOrEmpty(selValue) && !t1)
            {
                buffer.Append("<option value=\"");
                buffer.Append(DMD.WebUtils.HtmlEncode(selValue));
                buffer.Append("\" selected style=\"color:red;\">");
                buffer.Append(DMD.WebUtils.HtmlEncode(selValue));
                buffer.Append("</option>");
            }

            return buffer.ToString();
        }

        public string CreateElencoEntiPaganti(Anagrafica.CAzienda selValue)
        {
            var cursor = new Anagrafica.CAziendeCursor();
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            cursor.FormaGiuridica.Value = "Ente";
            cursor.Stato.Value = minidom.Databases.ObjectStatus.OBJECT_VALID;
            cursor.Cognome.SortOrder = minidom.Databases.SortEnum.SORT_ASC;
            while (!cursor.EOF())
            {
                t = minidom.Databases.GetID(selValue) == minidom.Databases.GetID(cursor.Item);
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(minidom.Databases.GetID(cursor.Item));
                writer.Append("\" ");
                writer.Append(Sistema.IIF(t, "selected", ""));
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nominativo));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            cursor.Dispose();
            if (selValue is object && t1 == false)
            {
                writer.Append("<option value=\"");
                writer.Append(minidom.Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nominativo));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoListeRicontatto(string selValue)
        {
            var liste = Anagrafica.ListeRicontatto.GetListeRicontatto();
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            for (int i = 0, loopTo = liste.Count - 1; i <= loopTo; i++)
            {
                var l = liste[i];
                string str = l.Name;
                if (!string.IsNullOrEmpty(str))
                {
                    t = (str ?? "") == (selValue ?? "");
                    t1 = t1 || t;
                    writer.Append("<option value=\"");
                    writer.Append(DMD.WebUtils.HtmlEncode(str));
                    writer.Append("\" ");
                    writer.Append(Sistema.IIF(t, "selected", ""));
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(str));
                    writer.Append("</option>");
                    if (!string.IsNullOrEmpty(selValue) & !t1)
                    {
                        writer.Append("<option value=\"");
                        writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                        writer.Append("\" selected style=\"color:red;\">");
                        writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                        writer.Append("</option>");
                    }
                }
            }

            return writer.ToString();
        }

        public string CreateElencoNomeTelefonoHTML(string selValue)
        {
            var items = new string[] { "Telefono", "Telefono (casa)", "Telefono (lavoro)", "Cellulare", "Cellulare (casa)", "Cellulare (lavoro)", "Fax", "Fax (casa)", "Fax (lavoro)" };
            bool t = false;
            var writer = new System.Text.StringBuilder();
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                t = (items[i] ?? "") == (selValue ?? "");
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("\" ");
                writer.Append(Sistema.IIF(t, "selected", ""));
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("</option>");
            }

            if (!string.IsNullOrEmpty(DMD.Strings.Trim(selValue)) & !t)
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoNomeWebHTML(string selValue)
        {
            var items = new string[] { "e-mail", "web site" };
            bool t = false;
            var writer = new System.Text.StringBuilder();
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                t = (items[i] ?? "") == (selValue ?? "");
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("</option>");
            }

            if (!string.IsNullOrEmpty(DMD.Strings.Trim(selValue)) & !t)
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        // Public Sub UpdateContattoTelefonico(ByVal name As String, ByVal contatto As CContatto)
        // contatto.Nome = Sistema.ApplicationContext.GetParameter(renderer, "txtNTel" & name, vbNullString)
        // contatto.Valore = Formats.ParsePhoneNumber(Sistema.ApplicationContext.GetParameter(renderer, "txtVTel" & name, vbNullString))
        // contatto.Validated = Sistema.ApplicationContext.GetParameter(renderer, "txtCTel" & name, "0") <> "0"
        // End Sub

        // Public Sub UpdateContattoWeb(ByVal name As String, ByVal contatto As CContatto)
        // contatto.Nome = Sistema.ApplicationContext.GetParameter(renderer, "txtNWeb" & name, vbNullString)
        // contatto.Valore = Sistema.ApplicationContext.GetParameter(renderer, "txtVWeb" & name, vbNullString)
        // contatto.Validated = Sistema.ApplicationContext.GetParameter(renderer, "txtCWeb" & name, "0") <> "0"
        // End Sub

        public string CreateElencoTipoAzienda(string selValue)
        {
            var ret = new System.Text.StringBuilder();
            bool t = false;

            using (var cursor = new Anagrafica.CTipologiaAziendaCursor())
            {
                bool t1;
                cursor.Stato.Value = minidom.Databases.ObjectStatus.OBJECT_VALID;
                selValue = Strings.Trim(selValue);
                t = false;
                while (!cursor.EOF())
                {
                    t1 = DMD.Strings.Compare(selValue, cursor.Item.Nome, true) == 0;
                    t = t | t1;

                    ret.Append("<option value=\""); ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome)); ret.Append("\" ");
                    ret.Append(t1 ? "selected" : "");
                    ret.Append(">");
                    ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                    ret.Append("</option>");

                    cursor.MoveNext();
                }

            }

            if (!string.IsNullOrEmpty(selValue) && !t)
            {
                ret.Append("<option value=\"");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("\" selected>");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("</option>");
            }

            return ret.ToString();
        }

        public string CreateElencoCategorieAzienda(string selValue)
        {
            var ret = new System.Text.StringBuilder();
            bool t = false;
            using (var cursor = new Anagrafica.CCategorieAziendaCursor())
            {
                bool t1;
                cursor.Stato.Value = minidom.Databases.ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
                selValue = Strings.Trim(selValue);
                t = false;
                while (!cursor.EOF())
                {
                    t1 = DMD.Strings.Compare(selValue, cursor.Item.Nome, true) == 0;
                    t = t | t1;
                    ret.Append("<option value=\"");
                    ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                    ret.Append("\" ");
                    ret.Append(t1 ? "selected" : "");
                    ret.Append(">");
                    ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                    ret.Append("</option>");
                    cursor.MoveNext();
                }

            }

            if (!string.IsNullOrEmpty(selValue) && !t)
            {
                ret.Append("<option value=\"");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("\" selected>");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("</option>");
            }

            return ret.ToString();

        }

        public string CreateElencoFormeGiuridicheAzienda(string selValue)
        {
            selValue = Strings.Trim(selValue);
            var ret = new System.Text.StringBuilder();
            bool t = false;

            using (var cursor = new Anagrafica.CFormeGiuridicheAziendaCursor())
            {
                cursor.Stato.Value = minidom.Databases.ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = true;
                cursor.Nome.SortOrder = minidom.Databases.SortEnum.SORT_ASC;
                bool t1 = false;
                while (!cursor.EOF())
                {
                    t1 = DMD.Strings.Compare(selValue, cursor.Item.Nome, true) == 0;
                    t = t | t1;
                    ret.Append("<option value=\"");
                    ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                    ret.Append("\" ");
                    if (t1) ret.Append("selected");
                    ret.Append(">");
                    ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                    ret.Append("</option>");
                    cursor.MoveNext();
                }

            }

            if (!string.IsNullOrEmpty(selValue) && !t)
            {
                ret.Append("<option value=\"");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("\" selected>");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("</option>");
            }

            return ret.ToString();
        }

        public string CreateElencoSesso(string value)
        {
            var items = new string[] { "M", "F" };
            var writer = new System.Text.StringBuilder();
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("\" ");
                if ((items[i] ?? "") == (value ?? ""))
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoStatiCivili(string selValue)
        {
            var items = new string[] { "Single", "Sposato", "Divorziato", "Vedovo" };
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
            {
                t = (selValue ?? "") == (items[i] ?? "");
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(items[i]));
                writer.Append("</option>");
            }

            if (!t1 && !string.IsNullOrEmpty(selValue))
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        // Public Function CreateElencoProfessioni(ByVal value As String) As String
        // Dim dbRis As System.Data.IDataReader = Nothing
        // Try
        // dbRis = APPConn.ExecuteReader("SELECT [Professione] FROM [tbl_PersoneProfessioni] ORDER BY [Professione] ASC")
        // Dim writer As New System.Text.StringBuilder
        // While dbRis.Read
        // Dim p As String = Formats.ToString(dbRis("Professione"))
        // writer.Append("<option value=""")
        // writer.Append(WebUtils.HtmlEncode(p))
        // writer.Append(""" ")
        // If (p = value) Then writer.Append("selected")
        // writer.Append(">")
        // writer.Append(WebUtils.HtmlEncode(p))
        // writer.Append("</option>")
        // End While

        // Return writer.ToString
        // Catch ex As Exception
        // Sistema.Events.NotifyUnhandledException(ex)
        // Throw
        // Finally
        // If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
        // End Try
        // End Function

        // Public Function CreateElencoTipoRapporto(ByVal selValue As String) As String
        // 'Dim items() As String = {"", "Ferrovia", "Medico", "Parapubblico", "Pensionato", "Pubblico", "Statale", "Privato"}
        // Dim t1 = False, t As Boolean = False
        // Dim writer As New System.Text.StringBuilder
        // Dim cursor As New CTipoRapportoCursor
        // cursor.Descrizione.SortOrder = SortEnum.SORT_ASC
        // cursor.IgnoreRights = True
        // 'For i As Integer = 0 To UBound(items)
        // While Not cursor.EOF
        // Dim item As CTipoRapporto = cursor.Item
        // t = (item.Descrizione = selValue) Or (item.IdTipoRapporto = selValue)
        // t1 = t1 OrElse t
        // writer.Append("<option value=""")
        // writer.Append(WebUtils.HtmlEncode(item.IdTipoRapporto))
        // writer.Append(""" ")
        // If (t) Then writer.Append("selected")
        // writer.Append(">")
        // writer.Append(WebUtils.HtmlEncode(item.ToString))
        // writer.Append("</option>")
        // cursor.MoveNext()
        // End While
        // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

        // If ((Strings.Trim(selValue) <> "") AndAlso Not t1) Then
        // writer.Append("<option value=""")
        // writer.Append(WebUtils.HtmlEncode(selValue))
        // writer.Append(""" selected>")
        // writer.Append(WebUtils.HtmlEncode(selValue))
        // writer.Append("</option>")
        // End If
        // Return writer.ToString
        // End Function

        // Public Function CreateElencoPosizioni(ByVal selValue As String) As String
        // Dim items() As String = {"Impiegato", "Pensionato", "Operaio", "Dirigente", "Amministratore", "Responsabile", "Disoccupato", "Carabiniere", "Poliziotto", "Militare", "Docente"}
        // selValue = Trim(selValue)
        // Array.Sort(items)
        // Dim t = False, t1 As Boolean = False
        // Dim writer As New System.Text.StringBuilder
        // For i As Integer = 0 To UBound(items)
        // t = (items(i) = selValue)
        // t1 = t1 OrElse t
        // writer.Append("<option value=""")
        // writer.Append(WebUtils.HtmlEncode(items(i)))
        // writer.Append(""" ")
        // If (t) Then writer.Append("selected")
        // writer.Append(">")
        // writer.Append(WebUtils.HtmlEncode(items(i)))
        // writer.Append("</option>")
        // Next
        // If ((selValue <> vbNullString) And Not t1) Then
        // writer.Append("<option value=""")
        // writer.Append(WebUtils.HtmlEncode(selValue))
        // writer.Append(""" selected style=""color:Red;"">")
        // writer.Append(WebUtils.HtmlEncode(selValue))
        // writer.Append("</option>")
        // End If
        // Return writer.ToString
        // End Function

        // Public Function CreateElencoTitoli(ByVal value As String) As String
        // Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader("SELECT [Titolo] FROM [tbl_PersoneTitoli] ORDER BY [Titolo] ASC")
        // Dim writer As New System.Text.StringBuilder
        // While dbRis.Read
        // Dim t As String = Formats.ToString(dbRis("Titolo"))
        // writer.Append("<option value=""")
        // writer.Append(WebUtils.HtmlEncode(t))
        // writer.Append(""" ")
        // If (t = value) Then writer.Append("selected")
        // writer.Append(">")
        // writer.Append(WebUtils.HtmlEncode(t))
        // writer.Append("</option>")
        // End While
        // dbRis.Dispose() : dbRis = Nothing
        // Return writer.ToString
        // End Function


        public string CreateElencoUffici(Anagrafica.CUfficio selValue)
        {
            var items = Anagrafica.Uffici.GetPuntiOperativiConsentiti();
            var writer = new System.Text.StringBuilder();
            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            {
                var item = items[i];
                if (item.Attivo)
                {
                    writer.Append("<option value=\"");
                    writer.Append(minidom.Databases.GetID(item));
                    writer.Append("\" ");
                    if (minidom.Databases.GetID(selValue) == minidom.Databases.GetID(item))
                        writer.Append("selected");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                    writer.Append("</option>");
                }
            }

            return writer.ToString();
        }

        //public string CreateElencoOperatoriPerUfficio(Anagrafica.CUfficio ufficio, Sistema.CUser selValue, bool onlyValid = true)
        //{
        //    var writer = new System.Text.StringBuilder();
        //    if (ufficio is null)
        //    {
        //        return SystemUtils.CreateElencoUtenti(selValue, onlyValid);
        //    }
        //    else
        //    {
        //        for (int i = 0, loopTo = ufficio.Utenti.Count - 1; i <= loopTo; i++)
        //        {
        //            var item = ufficio.Utenti[i];
        //            if (!onlyValid || item.UserStato == Sistema.UserStatus.USER_ENABLED)
        //            {
        //                writer.Append("<option value=\"");
        //                writer.Append(minidom.Databases.GetID(item));
        //                writer.Append("\" ");
        //                if (minidom.Databases.GetID(selValue) == minidom.Databases.GetID(item))
        //                    writer.Append("selected");
        //                writer.Append(">");
        //                writer.Append(DMD.WebUtils.HtmlEncode(item.Nominativo));
        //                writer.Append("</option>");
        //            }
        //        }
        //    }

        //    return writer.ToString();
        //}

        public string[] GetCategorieRicontatto()
        {
            return new[] { "Urgente", "Importante", "Normale", "Poco importante" };
        }

        public string ColorFromCategoria(string value)
        {
            switch (value ?? "")
            {
                case "Urgente":
                    {
                        return "red";
                    }

                case "Importante":
                    {
                        return "orange";
                    }

                case "Normale":
                    {
                        return "white";
                    }

                case "Poco importante":
                    {
                        return "#e0e0e0";
                    }

                default:
                    {
                        return "white";
                    }
            }
        }

        /// <summary>
        /// Distruttore
        /// </summary>
        ~CPersoneUtilsClass()
        {
            DMDObject.DecreaseCounter(this);
        }
    }

}

namespace minidom.Forms
{
    public partial class Utils
    {

        private static CPersoneUtilsClass m_PersoneUtils = null;

        public static CPersoneUtilsClass PersoneUtils
        {
            get
            {
                if (m_PersoneUtils is null)
                    m_PersoneUtils = new CPersoneUtilsClass();
                return m_PersoneUtils;
            }
        }
    }
}