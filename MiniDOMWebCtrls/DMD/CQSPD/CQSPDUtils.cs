using System;
using System.Data;
using DMD;
using minidom.Forms;
using minidom.internals;

namespace minidom.internals
{
    public sealed class CCQSPDUtilsClass
    {
        public string CreateElencoAssicurazioni(Finanziaria.CAssicurazione selValue)
        {
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            foreach (Finanziaria.CAssicurazione item in Finanziaria.Assicurazioni.LoadAll())
            {
                t = Databases.GetID(item) == Databases.GetID(selValue);
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(item));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                writer.Append("</option>");
                t1 = t | t1;
            }

            if (selValue is object && t1 == false)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoStatiTeamManager(Finanziaria.StatoTeamManager selValue)
        {
            var values = new Finanziaria.StatoTeamManager[] { Finanziaria.StatoTeamManager.STATO_ATTIVO, Finanziaria.StatoTeamManager.STATO_DISABILITATO, Finanziaria.StatoTeamManager.STATO_INATTIVAZIONE, Finanziaria.StatoTeamManager.STATO_SOSPESO };
            var names = new string[] { "ATTIVO", "DISABILITATO", "IN ATTIVAZIONE", "SOSPESO" };
            var ret = new System.Text.StringBuilder();
            for (int i = 0, loopTo = DMD.Arrays.UBound(names); i <= loopTo; i++)
            {
                ret.Append("<option value=\"");
                ret.Append((int)values[i]);
                ret.Append("\" ");
                if (selValue == values[i])
                    ret.Append("selected");
                ret.Append(">");
                ret.Append(DMD.WebUtils.HtmlEncode(names[i]));
                ret.Append("</option>");
            }

            return ret.ToString();
        }

        public string CreateElencoAreaManagers(Finanziaria.CAreaManager selValue, bool onlyValid)
        {
            var ret = new System.Text.StringBuilder();
            bool t, t1;
            var cursor = new Finanziaria.CAreaManagerCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.OnlyValid = onlyValid;
            cursor.Nominativo.SortOrder = Databases.SortEnum.SORT_ASC;
            t1 = false;
            while (!cursor.EOF())
            {
                t = Databases.GetID(cursor.Item) == Databases.GetID(selValue);
                t1 = t1 | t;
                ret.Append("<option value=\"");
                ret.Append(Databases.GetID(cursor.Item));
                ret.Append("\" ");
                if (t)
                    ret.Append("selected");
                ret.Append(">");
                ret.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nominativo));
                ret.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue is object && t1 == false)
            {
                ret.Append("<option value=\"");
                ret.Append(Databases.GetID(selValue));
                ret.Append("\" selected style=\"color:red;\">");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue.Nominativo));
                ret.Append("</option>");
            }

            return ret.ToString();
        }

        public string CreateElencoTipiRapportoTM(string selValue)
        {
            var names = new string[] { "Agente", "Mediatore", "Segnalatore" };
            var ret = new System.Text.StringBuilder();
            bool t, t1;
            t1 = false;
            selValue = Strings.Trim(selValue);
            for (int i = 0, loopTo = DMD.Arrays.UBound(names); i <= loopTo; i++)
            {
                t = (Strings.LCase(selValue) ?? "") == (Strings.LCase(names[i]) ?? "");
                t1 = t1 | t;
                ret.Append("<option value=\"");
                ret.Append(DMD.WebUtils.HtmlEncode(names[i]));
                ret.Append("\" ");
                if (t)
                    ret.Append("selected");
                ret.Append(">");
                ret.Append(DMD.WebUtils.HtmlEncode(names[i]));
                ret.Append("</option>");
            }

            if (!string.IsNullOrEmpty(selValue) && t1 == false)
            {
                ret.Append("<option value=\"");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("\" selected style=\"color:red;\">");
                ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                ret.Append("</option>");
            }

            return ret.ToString();
        }

        public CCollection<Sistema.CUser> GetOperatoriAutorizz()
        {
            var col = new CCollection<Sistema.CUser>();
            col.AddRange(Finanziaria.GruppoAutorizzatori.Members);
            foreach (Sistema.CUser u in Finanziaria.GruppoSupervisori.Members)
            {
                if (col.GetItemById(Databases.GetID(u)) is null)
                    col.Add(u);
            }

            col.Sort();
            return col;
        }

        public string CreateElencoOpAutorizz(Sistema.CUser selValue)
        {
            var col = GetOperatoriAutorizz();
            var writer = new System.Text.StringBuilder();
            bool t = false;
            bool t1 = false;
            for (int i = 0, loopTo = col.Count - 1; i <= loopTo; i++)
            {
                var user = col[i];
                t = Databases.GetID(user) == Databases.GetID(selValue);
                t1 = t1 | t;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(user));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(user.Nominativo));
                writer.Append("</option>");
            }

            if (selValue is object & !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nominativo));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoCollaboratori(Finanziaria.CCollaboratore selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CCollaboratoriCursor();
            var writer = new System.Text.StringBuilder();
            bool t, t1;
            cursor.NomePersona.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.OnlyValid = onlyValid;
            t = false;
            t1 = false;
            while (!cursor.EOF())
            {
                t = Databases.GetID(cursor.Item) == Databases.GetID(selValue);
                t1 = t1 | t;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.NomePersona));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue is object && !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.NomePersona));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoConsulenti(Finanziaria.CConsulentePratica selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CConsulentiPraticaCursor();
            var writer = new System.Text.StringBuilder();
            bool t, t1;
            cursor.OnlyValid = onlyValid;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            t1 = false;
            while (!cursor.EOF())
            {
                t = Databases.GetID(cursor.Item) == Databases.GetID(selValue);
                t1 = t1 | t;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue is object & !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoConsulenti(int selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CConsulentiPraticaCursor();
            var writer = new System.Text.StringBuilder();
            bool t, t1;
            cursor.OnlyValid = onlyValid;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            t1 = false;
            while (!cursor.EOF())
            {
                t = Databases.GetID(cursor.Item) == selValue;
                t1 = t1 | t;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue != 0 & !t1)
            {
                writer.Append("<option value=\"");
                writer.Append(selValue);
                writer.Append("\" selected style=\"color:red;\">ID: ");
                writer.Append(selValue);
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoTabelleSpese(Finanziaria.CCQSPDCessionarioClass cessionario, Finanziaria.CTabellaSpese selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CTabellaSpeseCursor();
            var writer = new System.Text.StringBuilder();
            bool t;
            cursor.OnlyValid = onlyValid;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            if (cessionario is object)
                cursor.CessionarioID.Value = Databases.GetID(cessionario);
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            while (!cursor.EOF())
            {
                t = t | Databases.GetID(cursor.Item) == Databases.GetID(selValue);
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == Databases.GetID(selValue))
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue is object & !t)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoTabelleTEG(Finanziaria.CCQSPDCessionarioClass cessionario, Finanziaria.CTabellaTEGMax selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CTabelleTEGMaxCursor();
            var writer = new System.Text.StringBuilder();
            bool t;
            // cursor.OnlyValid = onlyValid
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            if (cessionario is object)
                cursor.IDCessionario.Value = Databases.GetID(cessionario);
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            while (!cursor.EOF())
            {
                t = t | Databases.GetID(cursor.Item) == Databases.GetID(selValue);
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == Databases.GetID(selValue))
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue is object & !t)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoTabelleAssicurative(Finanziaria.CTabellaAssicurativa selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CTabelleAssicurativeCursor();
            var writer = new System.Text.StringBuilder();
            bool t;
            cursor.OnlyValid = onlyValid;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            while (!cursor.EOF())
            {
                t = t | Databases.GetID(cursor.Item) == Databases.GetID(selValue);
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == Databases.GetID(selValue))
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (selValue is object & !t)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(selValue));
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoTipoEstinzione(string selValue)
        {
            return DMD.Strings.vbNullString;
        }

        public string CreateElencoUnusedTblFin(int idProdotto)
        {
            // Dim dbRis As System.Data.IDataReader = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And Not [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
            var dbRis = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And Not [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And ([Prodotto]=" + idProdotto + ") GROYP BY [Tabella]) ORDER BY [Nome] ASC");
            var writer = new System.Text.StringBuilder();
            while (dbRis.Read())
            {
                writer.Append("<option value=\"");
                writer.Append(Sistema.Formats.ToInteger(dbRis["ID"]));
                writer.Append("\">");
                writer.Append(Sistema.Formats.ToString(dbRis["Nome"]));
                writer.Append(" (");
                writer.Append(Sistema.Formats.ToString(dbRis["NomeCessionario"]));
                writer.Append(")</option>");
            }

            dbRis.Dispose();
            dbRis = null;
            return writer.ToString();
        }

        public string CreateElencoUsedTblFin(int idProdotto)
        {
            // Dim dbRis As System.Data.IDataReader = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
            var dbRis = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And ([Prodotto]=" + idProdotto + ") GROYP BY [Tabella]) ORDER BY [Nome] ASC");
            var writer = new System.Text.StringBuilder();
            while (dbRis.Read())
            {
                writer.Append("<option value=\"");
                writer.Append(Sistema.Formats.ToInteger(dbRis["ID"]));
                writer.Append("\">");
                writer.Append(Sistema.Formats.ToString(dbRis["Nome"]));
                writer.Append(" (");
                writer.Append(Sistema.Formats.ToString(dbRis["NomeCessionario"]));
                writer.Append(")</option>");
            }

            dbRis.Dispose();
            dbRis = null;
            return writer.ToString();
        }

        public string CreateElencoNumeroMensilita(int value)
        {
            var writer = new System.Text.StringBuilder();
            writer.Append("<option></option>");
            for (int i = 1; i <= 15; i++)
            {
                if (Sistema.Formats.ToInteger(value) == i)
                {
                    writer.Append("<option value=\"");
                    writer.Append(i);
                    writer.Append("\" selected>");
                    writer.Append(i);
                    writer.Append("</option>");
                }
                else
                {
                    writer.Append("<option value=\"");
                    writer.Append(i);
                    writer.Append("\">");
                    writer.Append(i);
                    writer.Append("</option>");
                }
            }

            return writer.ToString();
        }

        public string CreateElencoPreventivatoriDisponibili(int value)
        {
            CCollection<Finanziaria.CProfilo> arrPreventivatori;
            var writer = new System.Text.StringBuilder();
            bool t;
            arrPreventivatori = Finanziaria.Profili.GetPreventivatoriUtenteOffline();
            t = false;
            for (int i = 0, loopTo = arrPreventivatori.Count - 1; i <= loopTo; i++)
            {
                var item = arrPreventivatori[i];
                t = t | Databases.GetID(item) == value;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(item));
                writer.Append("\" ");
                if (Databases.GetID(item) == value)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(item.ProfiloVisibile));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public decimal SommaRiga(decimal[,] matrice, int r, int startColumn, int numColumns)
        {
            decimal sum = 0m;
            for (int c = startColumn, loopTo = startColumn + numColumns - 1; c <= loopTo; c++)
                sum = sum + matrice[r, c];
            return sum;
        }

        public int SommaRiga(int[,] matrice, int r, int startColumn, int numColumns)
        {
            int sum = 0;
            for (int c = startColumn, loopTo = startColumn + numColumns - 1; c <= loopTo; c++)
                sum = sum + matrice[r, c];
            return sum;
        }

        public double SommaRiga(double[,] matrice, int r, int startColumn, int numColumns)
        {
            double sum = 0d;
            for (int c = startColumn, loopTo = startColumn + numColumns - 1; c <= loopTo; c++)
                sum = sum + matrice[r, c];
            return sum;
        }

        public void SortByTotale(decimal[,] m, int numRows, int numCols, int[] indexes)
        {
            decimal[] sums;
            int r, c;
            sums = new decimal[numRows];
            var loopTo = numRows - 1;
            for (r = 0; r <= loopTo; r++)
                sums[r] = SommaRiga(m, r, 0, numCols);
            var loopTo1 = numRows;
            for (r = 0; r <= loopTo1; r++)
            {
                var loopTo2 = numRows;
                for (c = r + 1; c <= loopTo2; c++)
                {
                    if (sums[indexes[c]] > sums[indexes[r]])
                    {
                        int tmp;
                        tmp = indexes[c];
                        indexes[c] = indexes[r];
                        indexes[r] = tmp;
                    }
                }
            }
        }

        public string CreateElencoPadri(Finanziaria.CCQSPDCessionarioClass cessionario, Finanziaria.CProfilo selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CProfiliCursor();
            var writer = new System.Text.StringBuilder();
            bool t;
            int idCessionario = Databases.GetID(cessionario);
            int selValueID = Databases.GetID(selValue);
            cursor.IDCessionario.Value = idCessionario;
            cursor.ID.Value = selValueID;
            cursor.ID.Operator = OP.OP_NE;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.ProfiloVisibile.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.OnlyValid = onlyValid;
            t = false;
            while (!cursor.EOF())
            {
                t = t | Databases.GetID(cursor.Item) == selValueID;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == selValueID)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.ProfiloVisibile));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (t == false & selValue is object)
            {
                writer.Append("<option value=\"");
                writer.Append(selValueID);
                writer.Append("\" selected style=\"color:red;\">");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.ProfiloVisibile));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoStatiDiPartenza(Finanziaria.CStatoPratica selValue)
        {
            CCollection<Finanziaria.CStatoPratica> items;
            int cID;
            int i;
            var writer = new System.Text.StringBuilder();
            bool t;
            int selValueID = Databases.GetID(selValue);
            items = Finanziaria.StatiPratica.GetStatiAttivi();
            t = false;
            var loopTo = items.Count - 1;
            for (i = 0; i <= loopTo; i++)
            {
                cID = Databases.GetID(items[i], 0);
                {
                    var withBlock = items[i];
                    t = t | cID == selValueID;
                    writer.Append("<option value=\"");
                    writer.Append(cID);
                    writer.Append("\" ");
                    if (cID == selValueID)
                        writer.Append("selected");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(withBlock.Nome));
                    writer.Append("</option>");
                }
            }

            if (!t & selValue is object)
            {
                writer.Append("<option value=\"");
                writer.Append(selValueID);
                writer.Append("\" style=\"color:red;\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(selValue.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoPeriodi(string periodo)
        {
            var items = new[] { "Oggi", "Ieri", "Questa settimana", "Questo mese", "Il mese scorso", "Quest'anno", "L'anno scorso", "Tra" };
            int i;
            var writer = new System.Text.StringBuilder();
            var loopTo = DMD.Arrays.UBound(items);
            for (i = 0; i <= loopTo; i++)
            {
                if ((items[i] ?? "") == (periodo ?? ""))
                {
                    writer.Append("<option value=\"");
                    writer.Append(items[i]);
                    writer.Append("\" selected>");
                    writer.Append(items[i]);
                    writer.Append("</option>");
                }
                else
                {
                    writer.Append("<option value=\"");
                    writer.Append(items[i]);
                    writer.Append("\">");
                    writer.Append(items[i]);
                    writer.Append("</option>");
                }
            }

            return writer.ToString();
        }

        public string CreateElencoTipoIData(string tipoIntervallo)
        {
            var values = DMD.DateUtils.GetSupportedPeriods(); // {"", "Oggi", "Ieri", "Questa settimana", "Questo mese", "Il mese scorso", "Questo anno", "L'anno scorso", "Tra"}
            var writer = new System.Text.StringBuilder();
            int i;
            tipoIntervallo = Strings.LCase(Strings.Trim(tipoIntervallo));
            var loopTo = DMD.Arrays.UBound(values);
            for (i = 0; i <= loopTo; i++)
            {
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(values[i]));
                writer.Append("\" ");
                if ((tipoIntervallo ?? "") == (Strings.LCase(values[i]) ?? ""))
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(values[i]));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoDataDi(int value)
        {
            var stati = new int[] { 0, 1, 2, 3, 4, 5 };
            var valori = new string[] { "Qualsiasi", "Inserimento", "Caricamento", "Perfezionamento", "Annullamento", "Trasferimento" };
            int i;
            var writer = new System.Text.StringBuilder();
            var loopTo = DMD.Arrays.UBound(valori);
            for (i = DMD.Arrays.LBound(valori); i <= loopTo; i++)
            {
                writer.Append("<option value=\"");
                writer.Append(stati[i]);
                writer.Append("\" ");
                if (value == stati[i])
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(valori[i]));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoProfili(string nomeProfilo)
        {
            CCollection<Finanziaria.CProfilo> items;
            var writer = new System.Text.StringBuilder();
            int i;
            items = Finanziaria.Profili.GetPreventivatoriUtente();
            var loopTo = items.Count - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if ((items[i].Profilo ?? "") == (nomeProfilo ?? ""))
                {
                    writer.Append("<option value=\"");
                    writer.Append(items[i].Profilo);
                    writer.Append("\" selected>");
                    writer.Append(items[i].Profilo);
                    writer.Append("</option>");
                }
                else
                {
                    writer.Append("<option value=\"");
                    writer.Append(items[i].Profilo);
                    writer.Append("\">");
                    writer.Append(items[i].Profilo);
                    writer.Append("</option>");
                }
            }

            return writer.ToString();
        }

        public string CreateElencoCessionari(Finanziaria.CCQSPDCessionarioClass cessionario, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CCessionariCursor();
            bool t;
            var writer = new System.Text.StringBuilder();
            int idCessionario;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.Visibile.Value = true;
            cursor.OnlyValid = onlyValid;
            idCessionario = Databases.GetID(cessionario);
            t = false;
            while (!cursor.EOF())
            {
                t = t | Databases.GetID(cursor.Item) == idCessionario;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == idCessionario)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (t == false & cessionario is object)
            {
                writer.Append("<option value=\"");
                writer.Append(idCessionario);
                writer.Append("\" style=\"color:red\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(cessionario.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoGruppiProdotto(Finanziaria.CCQSPDCessionarioClass cessionario, Finanziaria.CGruppoProdotti selItem, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CGruppoProdottiCursor();
            var writer = new System.Text.StringBuilder();
            bool t;
            int idCessionario = Databases.GetID(cessionario);
            int selItemID = Databases.GetID(selItem);
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.OnlyValid = onlyValid;
            cursor.CessionarioID.Value = idCessionario;
            cursor.Descrizione.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            while (!cursor.EOF())
            {
                t = t | Databases.GetID(cursor.Item) == selItemID;
                writer.Append("<option value=\"");
                writer.Append(cursor.Item.ID);
                writer.Append("\" ");
                if (Databases.GetID(cursor.Item) == selItemID)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Descrizione));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (t == false & selItem is object)
            {
                writer.Append("<option value=\"");
                writer.Append(selItemID);
                writer.Append("\" style=\"color:red\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(selItem.Descrizione));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoTipoContratto(string selValue)
        {
            string ret;
            bool t;
            var items = Finanziaria.TipiContratto.LoadAll();
            items.Sort();
            ret = "";
            t = false;
            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            {
                var item = items[i];
                if (item.Attivo == false)
                    continue;
                string tc = item.IdTipoContratto;
                t = t | (tc ?? "") == (selValue ?? "");
                ret = ret + "<option value=\"" + DMD.WebUtils.HtmlEncode(tc) + "\" " + Sistema.IIF((tc ?? "") == (selValue ?? ""), "selected", "") + ">" + DMD.WebUtils.HtmlEncode(item.Descrizione + " (" + tc + ")") + "</option>";
            }

            if (t == false & !string.IsNullOrEmpty(selValue))
            {
                ret = ret + "<option value=\"" + DMD.WebUtils.HtmlEncode(selValue) + "\" style=\"color:red\" selected>INVALID: ID=" + DMD.WebUtils.HtmlEncode(selValue) + "</option>";
            }

            return ret;
        }

        // Public Function CreateElencoProfiliEsterni(ByVal cessionario As CCQSPDCessionarioClass, ByVal selValue As CProfilo, Optional ByVal onlyValid As Boolean = True) As String
        // Dim writer As New System.Text.StringBuilder
        // Dim t As Boolean
        // Dim items As CCollection(Of CProfilo)
        // Dim idProfilo As Integer = GetID(selValue)
        // items = minidom.Finanziaria.Pratiche.GetArrayProfiliEsterni(cessionario)
        // t = False
        // For i As Integer = 0 To items.Count - 1
        // Dim item As CProfilo = items(i)
        // If (onlyValid = False) Or (item.IsValid) Then
        // t = t Or (idProfilo = GetID(item))
        // writer.Append("<option value=""")
        // writer.Append(GetID(item))
        // writer.Append(""" ")
        // If (idProfilo = GetID(item)) Then writer.Append("selected")
        // writer.Append(">")
        // writer.Append(WebUtils.HtmlEncode(item.ProfiloVisibile))
        // writer.Append("</option>")
        // End If
        // Next
        // If (t = False) And (selValue IsNot Nothing) Then
        // writer.Append("<option value=""")
        // writer.Append(idProfilo)
        // writer.Append(""" style=""color:red"" selected>")
        // writer.Append(selValue.ProfiloVisibile)
        // writer.Append("</option>")
        // End If

        // Return writer.ToString
        // End Function

        public string CreateElencoProdotti(Finanziaria.CProfilo profilo, Finanziaria.CCQSPDProdotto prodotto, bool onlyValid = true)
        {
            CCollection<Finanziaria.CCQSPDProdotto> prodotti;
            var t = default(bool);
            var writer = new System.Text.StringBuilder();
            int selValue = Databases.GetID(prodotto);
            if (profilo is object)
            {
                prodotti = profilo.ProdottiXProfiloRelations.GetProdotti();
                t = false;
                for (int i = 0, loopTo = prodotti.Count - 1; i <= loopTo; i++)
                {
                    var item = prodotti[i];
                    if (item.IsValid() | onlyValid == false)
                    {
                        t = t || Databases.GetID(item) == selValue;
                        writer.Append("<option value=\"");
                        writer.Append(Databases.GetID(item));
                        writer.Append("\" ");
                        if (Databases.GetID(item) == selValue)
                            writer.Append("selected");
                        writer.Append(">");
                        writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                        writer.Append("</option>");
                    }
                }
            }

            if (t == false && prodotto is object)
            {
                writer.Append("<option value=\"");
                writer.Append(selValue);
                writer.Append("\" style=\"color:red\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(prodotto.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoProdotti(int idProdotto, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CProdottiCursor();
            var writer = new System.Text.StringBuilder();
            bool t, t1;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.OnlyValid = onlyValid;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.Visible.Value = true;
            t = false;
            t1 = false;
            while (!cursor.EOF())
            {
                string strStyle;
                if (cursor.Item.IsValid())
                {
                    strStyle = "display:block";
                }
                else
                {
                    strStyle = "color:red;";
                }

                t = Databases.GetID(cursor.Item) == idProdotto;
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(cursor.Item));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(" valid=\"");
                if (cursor.Item.IsValid())
                {
                    writer.Append("1");
                }
                else
                {
                    writer.Append("0");
                }

                writer.Append("\" style=\"");
                writer.Append(strStyle);
                writer.Append("\">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append(" (");
                writer.Append(cursor.Item.NomeCessionario);
                writer.Append(")");
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (t1 == false && idProdotto != 0)
            {
                var prodotto = Finanziaria.Prodotti.GetItemById(idProdotto);
                string nomeProdotto = "INVALID: ID=" + idProdotto;
                if (prodotto is object)
                    nomeProdotto = prodotto.Descrizione + " (" + prodotto.NomeCessionario + ")";
                writer.Append("<option value=\"");
                writer.Append(idProdotto);
                writer.Append("\" style=\"color:red\" selected>");
                writer.Append(DMD.WebUtils.HtmlEncode(nomeProdotto));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoNomiProdotto(string selValue, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CProdottiCursor();
            var writer = new System.Text.StringBuilder();
            bool t, t1;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.OnlyValid = onlyValid;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            t = false;
            t1 = false;
            while (!cursor.EOF())
            {
                t = (cursor.Item.Nome ?? "") == (selValue ?? "");
                t1 = t1 || t;
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append("\" ");
                if (t)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nome));
                writer.Append(" (");
                writer.Append(cursor.Item.NomeCessionario);
                writer.Append(")");
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (t == false && !string.IsNullOrEmpty(selValue))
            {
                writer.Append("<option value=\"");
                writer.Append(selValue);
                writer.Append("\" style=\"color:red\" selected>INVALID: Nome=");
                writer.Append(selValue);
                writer.Append("</option>");
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            return writer.ToString();
        }

        public string CreateElencoDurata(int tVal)
        {
            var writer = new System.Text.StringBuilder();
            for (int i = 12; i <= 120; i += 12)
            {
                if (tVal == i)
                {
                    writer.Append("<option value=\"");
                    writer.Append(i);
                    writer.Append("\" style=\"text-align:right;\" selected>");
                    writer.Append(i);
                    writer.Append("</option>");
                }
                else
                {
                    writer.Append("<option value=\"");
                    writer.Append(i);
                    writer.Append("\" style=\"text-align:right;\">");
                    writer.Append(i);
                    writer.Append("</option>");
                }
            }

            return writer.ToString();
        }

        public string CreateElencoOperatori(Sistema.CUser operatore, bool onlyValid = true)
        {
            Sistema.CModule module;
            Sistema.CGroup grp;
            string ret;
            bool t, t1;
            int idOperatore = Databases.GetID(operatore);
            module = Finanziaria.Pratiche.Module;
            ret = "";
            t = false;
            if (module.UserCanDoAction("list") || module.UserCanDoAction("list_office"))
            {
                // Otteniamo il gruppo Finanziaria
                grp = Finanziaria.GruppoOperatori;
                if (grp is object)
                {
                    for (int i = 0, loopTo = grp.Members.Count - 1; i <= loopTo; i++)
                    {
                        var usr = grp.Members[i];
                        t1 = idOperatore == Databases.GetID(usr);
                        t = t || t1;
                        ret = ret + "<option value=\"" + Databases.GetID(usr) + "\" " + Sistema.IIF(t1, "selected", "") + ">" + DMD.WebUtils.HtmlEncode("" + usr.Nominativo) + "</option>";
                    }
                }
            }

            if (module.UserCanDoAction("list_own"))
            {
                t1 = idOperatore == Databases.GetID(Sistema.Users.CurrentUser);
                t = t || t1;
                ret = ret + "<option value=\"" + Databases.GetID(Sistema.Users.CurrentUser) + "\" " + Sistema.IIF(t1, "selected", "") + ">" + DMD.WebUtils.HtmlEncode("" + Sistema.Users.CurrentUser.Nominativo) + "</option>";
            }

            if (!t && operatore is object)
            {
                ret = ret + "<option value=\"" + idOperatore + "\" selected style=\"color:red;\">" + operatore.Nominativo + "</option>";
            }

            return ret;
        }

        public string CreateElencoCommerciali(Finanziaria.CTeamManager commerciale, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CTeamManagersCursor();
            var writer = new System.Text.StringBuilder();
            int iID;
            bool t1, t;
            var items = new CCollection<Finanziaria.CTeamManager>();
            int idCommerciale = Databases.GetID(commerciale);
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            // cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
            cursor.OnlyValid = onlyValid;
            t = false;
            t1 = false;
            while (!cursor.EOF())
            {
                if (cursor.Item is object)
                    items.Add(cursor.Item);
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            // items.Comparer = New CTeamManager
            items.Sort();
            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            {
                var item = items[i];
                iID = Databases.GetID(item);
                t = t || iID == idCommerciale;
                writer.Append("<option value=\"");
                writer.Append(iID);
                writer.Append("\" ");
                if (iID == idCommerciale)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(item.Nominativo));
                writer.Append("</option>");
            }

            if (t == false && commerciale is object)
            {
                writer.Append("<option value=\"");
                writer.Append(idCommerciale);
                writer.Append("\" selected style=\"color:red\">");
                writer.Append(DMD.WebUtils.HtmlEncode(commerciale.Nominativo));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoCommerciali(Finanziaria.CAreaManager areaManager, Finanziaria.CTeamManager commerciale, bool onlyValid = true)
        {
            var cursor = new Finanziaria.CTeamManagersCursor();
            var writer = new System.Text.StringBuilder();
            int iID;
            bool t;
            int idAreaManager = Databases.GetID(areaManager);
            int commercialeID = Databases.GetID(commerciale);
            cursor.IDReferente.Value = idAreaManager;
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nominativo.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.OnlyValid = onlyValid;
            t = false;
            while (!cursor.EOF())
            {
                iID = Databases.GetID(cursor.Item);
                t = t || iID == commercialeID;
                writer.Append("<option value=\"");
                writer.Append(iID);
                writer.Append("\" ");
                if (iID == commercialeID)
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(cursor.Item.Nominativo));
                writer.Append("</option>");
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            if (t == false && commerciale is object)
            {
                writer.Append("<option value=\"");
                writer.Append(commercialeID);
                writer.Append("\" selected style=\"color:red\">");
                writer.Append(DMD.WebUtils.HtmlEncode(commerciale.Nominativo));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoStatoPratica(Finanziaria.StatoPraticaEnum? value)
        {
            var stati = DMD.Arrays.Empty<Finanziaria.StatoPraticaEnum>();
            var valori = DMD.Arrays.Empty<string>();
            foreach (Finanziaria.StatoPraticaEnum s in Enum.GetValues(typeof(Finanziaria.StatoPraticaEnum)))
            {
                stati = DMD.Arrays.Append(stati, s);
                valori = DMD.Arrays.Append(valori, Finanziaria.StatiPratica.FormatMacroStato(s));
            }

            return minidom.Forms.Utils.SystemUtils.CreateElenco(valori, stati, (object)value);
        }

        public string CreateElencoUfficiStr(string value)
        {
            Sistema.CModule module;
            bool canList, canListOwn;
            IDataReader dbRis;
            string dbSQL;
            module = Finanziaria.Pratiche.Module;
            canList = module.UserCanDoAction("list");
            canListOwn = module.UserCanDoAction("list_own");
            value = Strings.Trim(value);
            if (canList)
            {
                dbSQL = "SELECT [ID] As [a], [Nome] As [b] FROM [tbl_AziendaUffici] ORDER BY [Nome] ASC";
            }
            else if (canListOwn)
            {
                dbSQL = "SELECT [tbl_UtentiXUfficio].[Ufficio] As [a], [tbl_AziendaUffici].[Nome] As [b] FROM [tbl_UtentiXUfficio] INNER JOIN [tbl_AziendaUffici] ON [tbl_UtentiXUfficio].[Ufficio]=[tbl_AziendaUffici].[ID] WHERE ([tbl_UtentiXUfficio].[Utente]=" + Sistema.Users.CurrentUser.ID + ")";
            }
            else
            {
                dbSQL = "SELECT [ID] As [a], [Nome] As [b] FROM [tbl_AziendaUffici] WHERE (0<>0)";
            }

            dbRis = Databases.APPConn.ExecuteReader(dbSQL);
            var writer = new System.Text.StringBuilder();
            while (dbRis.Read())
            {
                string b = Strings.Trim(Sistema.Formats.ToString(dbRis["b"]));
                writer.Append("<option value=\"");
                writer.Append(DMD.WebUtils.HtmlEncode(b));
                writer.Append("\" ");
                if ((b ?? "") == (value ?? ""))
                    writer.Append("selected");
                writer.Append(">");
                writer.Append(DMD.WebUtils.HtmlEncode(b));
                writer.Append("</option>");
            }

            dbRis.Dispose();
            dbRis = null;
            return writer.ToString();
        }

        public string CreateElencoUffici(Anagrafica.CUfficio ufficio)
        {
            var writer = new System.Text.StringBuilder();
            bool t1, t;
            var items = Anagrafica.Uffici.GetPuntiOperativiConsentiti();
            t1 = false;
            t = false;
            for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
            {
                var item = items[i];
                if (item.Attivo && item.Stato == Databases.ObjectStatus.OBJECT_VALID)
                {
                    t = Databases.GetID(ufficio) == Databases.GetID(item);
                    t1 = t1 | t;
                    writer.Append("<option value=\"");
                    writer.Append(Databases.GetID(item));
                    writer.Append("\" ");
                    if (t)
                        writer.Append("selected");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                    writer.Append("</option>");
                }
            }

            if (t1 == false && ufficio is object)
            {
                writer.Append("<option value=\"");
                writer.Append(Databases.GetID(ufficio));
                writer.Append("\" selected style=\"color:red\">");
                writer.Append(DMD.WebUtils.HtmlEncode(ufficio.Nome));
                writer.Append("</option>");
            }

            return writer.ToString();
        }

        public string CreateElencoConsulenze(int idCliente, int id)
        {
            var cursor = new Finanziaria.CQSPDConsulenzaCursor();
            try
            {
                string html = "";
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.DataConsulenza.SortOrder = Databases.SortEnum.SORT_DESC;
                cursor.IDCliente.Value = idCliente;
                while (!cursor.EOF())
                {
                    var c = cursor.Item;
                    html += "<option value=\"" + Databases.GetID(c) + "\" " + Sistema.IIF(Databases.GetID(c) == id, "selected", "") + ">" + c.DataConsulenza + ", Op: " + c.NomeConsulente + " (" + Finanziaria.Consulenze.FormatStato(c.StatoConsulenza) + ")</option>";
                    cursor.MoveNext();
                }

                return html;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cursor.Dispose();
            }
        }

        public string CreateElencoRichiesteDiFinanziamento(int idCliente, int id)
        {
            var cursor = new Finanziaria.CRichiesteFinanziamentoCursor();
            try
            {
                string html = "";
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                // if (this.getItem().StatoContatto() != null && this.getItem().StatoContatto().getData() != null) {
                // //    cursor.Data().setValue(Calendar.GetLastSecond(this.getItem().StatoContatto().getData()));
                // //} else {
                // //    cursor.Data().setValue(Calendar.GetLastSecond(this.getItem().getCreatoIl()));
                // //}
                cursor.IDCliente.Value = idCliente;
                cursor.Data.Operator = OP.OP_LE;
                cursor.Data.SortOrder = Databases.SortEnum.SORT_DESC;
                while (!cursor.EOF())
                {
                    var c = cursor.Item;
                    string str;
                    str = Sistema.Formats.FormatUserDate(c.Data);
                    switch (c.TipoRichiesta)
                    {
                        case Finanziaria.TipoRichiestaFinanziamento.ALMENO:
                            {
                                str += ", Almeno " + Sistema.Formats.FormatValuta(c.ImportoRichiesto);
                                break;
                            }

                        case Finanziaria.TipoRichiestaFinanziamento.MASSIMO_POSSIBILE:
                            {
                                str += ", Massimo";
                                break;
                            }

                        case Finanziaria.TipoRichiestaFinanziamento.TRA:
                            {
                                str += ", Tra " + Sistema.Formats.FormatValuta(c.ImportoRichiesto) + " e " + Sistema.Formats.FormatValuta(c.ImportoRichiesto1);
                                break;
                            }

                        case Finanziaria.TipoRichiestaFinanziamento.UGUALEA:
                            {
                                str += ", Uguale a " + Sistema.Formats.FormatValuta(c.ImportoRichiesto);
                                break;
                            }
                    }

                    html += "<option value=\"" + Databases.GetID(c) + "\" " + Sistema.IIF(Databases.GetID(c) == id, "selected", "") + ">" + str + "</option>";
                    cursor.MoveNext();
                }

                return html;
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

        public string CreateElencoStatiRichiestaSconto(Finanziaria.StatoRichiestaApprovazione? selValue)
        {
            var items = new Finanziaria.StatoRichiestaApprovazione[] { Finanziaria.StatoRichiestaApprovazione.ATTESA, Finanziaria.StatoRichiestaApprovazione.PRESAINCARICO, Finanziaria.StatoRichiestaApprovazione.APPROVATA, Finanziaria.StatoRichiestaApprovazione.ATTESA };
            var names = new string[] { "Attesa Valutazione", "In Valutazione", "Approvata", "Negata" };
            return minidom.Forms.Utils.SystemUtils.CreateElenco(names, items, selValue);
        }
    }
}

namespace minidom.Forms
{
    public partial class Utils
    {
        private static CCQSPDUtilsClass m_CQSPDUtils = null;

        public static CCQSPDUtilsClass CQSPDUtils
        {
            get
            {
                if (m_CQSPDUtils is null)
                    m_CQSPDUtils = new CCQSPDUtilsClass();
                return m_CQSPDUtils;
            }
        }
    }
}