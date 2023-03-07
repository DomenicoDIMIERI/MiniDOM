using DMD;

namespace minidom.Forms
{
    public partial class Utils
    {
        public sealed class CTicketUtilsClass
        {
            internal CTicketUtilsClass()
            {
            }

            public string CreateElencoCategorie(string selValue)
            {
                selValue = Strings.Trim(selValue + "");
                string dbSQL = "SELECT [Categoria] FROM [tbl_SupportTicketsCat] WHERE [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + " GROUP BY [Categoria] ORDER BY [Categoria] ASC";
                var dbRis = Office.Database.ExecuteReader(dbSQL);
                var html = new System.Text.StringBuilder();
                bool t, t1;
                t1 = false;
                while (dbRis.Read())
                {
                    string cat = Sistema.Formats.ToString(dbRis["Categoria"]);
                    t = (cat ?? "") == (selValue ?? "");
                    t1 = t1 || t;
                    if (!string.IsNullOrEmpty(cat))
                        html.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(cat) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(cat) + "</option>");
                }

                dbRis.Dispose();
                if (!string.IsNullOrEmpty(selValue) && !t1)
                    html.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(selValue) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue) + "</option>");
                return html.ToString();
            }

            public string CreateElencoSottocategoria(string categoria, string selValue)
            {
                categoria = Strings.Trim(categoria + "");
                selValue = Strings.Trim(selValue + "");
                string dbSQL;
                if (string.IsNullOrEmpty(categoria))
                {
                    dbSQL = "SELECT [Sottocategoria] FROM [tbl_SupportTicketsCat] WHERE ([Categoria]='' Or [Categoria] Is NULL) AND [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + " GROUP BY [Sottocategoria] ORDER BY [Sottocategoria] ASC";
                }
                else
                {
                    dbSQL = "SELECT [Sottocategoria] FROM [tbl_SupportTicketsCat] WHERE [Categoria]=" + Databases.DBUtils.DBString(categoria) + " AND [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + " GROUP BY [Sottocategoria] ORDER BY [Sottocategoria] ASC";
                }

                var dbRis = Office.Database.ExecuteReader(dbSQL);
                var html = new System.Text.StringBuilder();
                bool t, t1;
                t1 = false;
                while (dbRis.Read())
                {
                    string cat = Sistema.Formats.ToString(dbRis["Sottocategoria"]);
                    t = (cat ?? "") == (selValue ?? "");
                    t1 = t1 || t;
                    if (!string.IsNullOrEmpty(cat))
                        html.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(cat) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(cat) + "</option>");
                }

                dbRis.Dispose();
                if (!string.IsNullOrEmpty(selValue) && !t1)
                    html.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(selValue) + "\" selected style=\"color:red;\">" + DMD.WebUtils.HtmlEncode(selValue) + "</option>");
                return html.ToString();
            }
        }

        private static CTicketUtilsClass m_TicketUtils = null;

        public static CTicketUtilsClass TicketUtils
        {
            get
            {
                if (m_TicketUtils is null)
                    m_TicketUtils = new CTicketUtilsClass();
                return m_TicketUtils;
            }
        }
    }
}