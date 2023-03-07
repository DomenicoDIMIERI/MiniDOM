using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public partial class Utils
    {
        public sealed class CLuoghiUtilsClass
        {
            internal CLuoghiUtilsClass()
            {
            }

            public string CreateElencoPuntiOperativi(string selItem)
            {
                var writer = new System.Text.StringBuilder();
                int i;
                CCollection<Anagrafica.CUfficio> Uffici;
                Anagrafica.CUfficio ufficio;
                Uffici = Anagrafica.Uffici.GetPuntiOperativi();
                var loopTo = Uffici.Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    ufficio = Uffici[i];
                    writer.Append("<option value=\"");
                    writer.Append(Databases.GetID(ufficio));
                    writer.Append("\" ");
                    if (Databases.GetID(ufficio) == DMD.Doubles.CDbl(selItem))
                        writer.Append("selected");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(ufficio.Nome));
                    writer.Append("</option>");
                }

                return writer.ToString();
            }

            public string CreateElencoComuni(string provincia, string comune)
            {
                var ret = new System.Text.StringBuilder();
                bool t1 = false;
                bool t = false;
                comune = DMD.Strings.Trim(comune);
                provincia = DMD.Strings.Trim(provincia);
                foreach (Anagrafica.CComune p in Anagrafica.Luoghi.Comuni.LoadAll())
                {
                    if (p.Stato != Databases.ObjectStatus.OBJECT_VALID)
                        continue;
                    if (
                        DMD.Strings.Compare(p.Provincia, provincia, true) != 0 
                        && 
                        DMD.Strings.Compare(p.Sigla, provincia, true) != 0
                        )
                        continue;

                    t = DMD.Strings.Compare(p.Nome, comune, true) == 0;
                    t1 = t1 || t;
                    ret.Append("<option value=\"");
                    ret.Append(DMD.WebUtils.HtmlEncode(p.Nome));
                    ret.Append("\" ");
                    if (t)
                        ret.Append("selected");
                    ret.Append(">");
                    ret.Append(DMD.WebUtils.HtmlEncode(p.Nome));
                    ret.Append("</option>");
                }

                if (!string.IsNullOrEmpty(comune) && !t1)
                {
                    ret.Append("<option value=\"");
                    ret.Append(DMD.WebUtils.HtmlEncode(comune));
                    ret.Append("\" selected style=\"color:red\">");
                    ret.Append(DMD.WebUtils.HtmlEncode(comune));
                    ret.Append("</option>");
                }

                return ret.ToString();
            }

            public string CreateElencoNomiProvince(string selValue)
            {
                var ret = new System.Text.StringBuilder();
                bool t1 = false;
                bool t = false;
                selValue = DMD.Strings.Trim(selValue);
                foreach (Anagrafica.CProvincia p in Anagrafica.Luoghi.Province.LoadAll())
                {
                    if (p.Stato != Databases.ObjectStatus.OBJECT_VALID)
                        continue;
                    t = DMD.Strings.Compare(p.Nome, selValue, true) == 0;
                    t1 = t1 || t;
                    ret.Append("<option value=\"");
                    ret.Append(DMD.WebUtils.HtmlEncode(p.Nome));
                    ret.Append("\" ");
                    if (t)
                        ret.Append("selected");
                    ret.Append(">");
                    ret.Append(DMD.WebUtils.HtmlEncode(p.Nome));
                    ret.Append("</option>");
                }

                if (!string.IsNullOrEmpty(selValue) && !t1)
                {
                    ret.Append("<option value=\"");
                    ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                    ret.Append("\" selected style=\"color:red\">");
                    ret.Append(DMD.WebUtils.HtmlEncode(selValue));
                    ret.Append("</option>");
                }

                return ret.ToString();
            }

            public string CreateElencoSigleProvince(string selValue)
            {
                var ret = new System.Text.StringBuilder();
                bool t1 = false;
                bool t = false;
                selValue = DMD.Strings.Trim(selValue);
                foreach (Anagrafica.CProvincia p in Anagrafica.Luoghi.Province.LoadAll())
                {
                    if (p.Stato != Databases.ObjectStatus.OBJECT_VALID)
                        continue;
                    t = DMD.Strings.Compare(p.Sigla, selValue, true) == 0;
                    t1 = t1 || t;
                    ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(p.Sigla) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(p.Sigla) + "</option>");
                }

                if (!string.IsNullOrEmpty(selValue) && !t1)
                {
                    ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(selValue) + "\" " + Sistema.IIF(t, "selected", "") + " style=\"color:red\">" + DMD.WebUtils.HtmlEncode(selValue) + "</option>");
                }

                return ret.ToString();
            }

            public string CreateElencoNomiRegioni(string selValue)
            {
                var ret = new System.Text.StringBuilder();
                bool t1 = false;
                bool t = false;
                selValue = DMD.Strings.Trim(selValue);
                foreach (Anagrafica.CRegione p in Anagrafica.Luoghi.Regioni.LoadAll())
                {
                    if (p.Stato != Databases.ObjectStatus.OBJECT_VALID)
                        continue;
                    t = DMD.Strings.Compare(p.Nome, selValue, true) == 0;
                    t1 = t1 || t;
                    ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(p.Nome) + "\" " + Sistema.IIF(t, "selected", "") + ">" + DMD.WebUtils.HtmlEncode(p.Nome) + "</option>");
                }

                if (!string.IsNullOrEmpty(selValue) && !t1)
                {
                    ret.Append("<option value=\"" + DMD.WebUtils.HtmlEncode(selValue) + "\" " + Sistema.IIF(t, "selected", "") + " style=\"color:red\">" + DMD.WebUtils.HtmlEncode(selValue) + "</option>");
                }

                return ret.ToString();
            }
        }

        private static CLuoghiUtilsClass m_LuoghiUtils = null;

        public static CLuoghiUtilsClass LuoghiUtils
        {
            get
            {
                if (m_LuoghiUtils is null)
                    m_LuoghiUtils = new CLuoghiUtilsClass();
                return m_LuoghiUtils;
            }
        }
    }
}