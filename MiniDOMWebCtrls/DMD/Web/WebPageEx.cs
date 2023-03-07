using System;
using System.Web.UI;
using DMD;
using DMD.XML;
using minidom.Forms;

namespace minidom.Web
{

    /// <summary>
    /// Rappresenta una pagina web
    /// </summary>
    public class WebPageEx 
        : minidom.WebSite.WebPageEx
    {
        public const int ADRESSBAR_HEIGHT = 40;
        public const int FOOTER_HEIGHT = 20;
        public const bool ONMAINTENANCE = false;
        public const int SESSIONTIMEOUT = 20;
        private string m_DMDPage = "";
        private CWebControlsCollection m_WebControls;
        private CCollection<string> m_IncludedScripts = new CCollection<string>();
        private CCollection<string> m_IncludedCSS = new CCollection<string>();

        static WebPageEx()
        {
            // minidom.ADV.Initialize()
        }

        /// <summary>
        /// Restituisce vero se la pagina viene generata tramite codice e non tramite HTML
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool CustomRendering()
        {
            return false;
        }

        public WebPageEx()
        {
            SetupInitialScripts();
        }

        public string DMDPage
        {
            get
            {
                if (string.IsNullOrEmpty(m_DMDPage))
                    m_DMDPage = Strings.Trim(Request.Form["_dmdpage"]);
                if (string.IsNullOrEmpty(m_DMDPage))
                    m_DMDPage = Sistema.ASPSecurity.GetRandomKey(25);
                return m_DMDPage;
            }
        }

        protected virtual string GetResourceVersion(string url)
        {
            try
            {
                string path = this.AC.MapPath(url);
                var f = new System.IO.FileInfo(path);
                return DMD.Integers.Hex(f.Length) + "." + f.LastWriteTimeUtc;
            }
            catch (Exception ex)
            {
                return "err";
            }
        }

        protected virtual string AddJSResourceWithVersion(string url)
        {
            string version = GetResourceVersion(url);
            if (Strings.InStr(url, "?") > 0)
            {
                url = url + "&scrv=" + version;
            }
            else
            {
                url = url + "?scrv=" + version;
            }

            IncludedScripts.Add(url);
            return url;
        }

        protected virtual string AddCSSResourceWithVersion(string url)
        {
            string version = GetResourceVersion(url);
            if (Strings.InStr(url, "?") > 0)
            {
                url = url + "&scrv=" + version;
            }
            else
            {
                url = url + "?scrv=" + version;
            }

            IncludedCSS.Add(url);
            return url;
        }

        protected virtual void SetupInitialScripts()
        {
            AddJSResourceWithVersion("/ckeditor/ckeditor.js");
            AddJSResourceWithVersion("/ckeditor/config.js");
            AddJSResourceWithVersion("/minidom/widgets/DMDMiniBase.js");
            AddJSResourceWithVersion("/minidom/widgets/DMDMiniControls.js");
            AddCSSResourceWithVersion("/minidom/widgets/DMDMiniStyle.css");
        }

        protected virtual void IncludeGoogleMapsScript(HtmlTextWriter writer, string apiKey)
        {
            // writer.Write("<script src=""https://maps.googleapis.com/maps/api/js?key=" & apiKey & "&callback=initMap"" async defer></script>")
            // writer.Write("<script  src=""https://maps.googleapis.com/maps/api/js?key=" & apiKey & "&callback=initMap""  type=""text/javascript""></script>")

            writer.Write("<script src=\"https://maps.google.com/maps/api/js?sensor=false&force=lite\" async defer type=\"text/javascript\"></script>");
        }

        protected virtual string GetGoogleMapsApiKey()
        {
            return "";
        }

        /// <summary>
        /// Restituisce la collezione degli script che vengono inclusi nella pagina sul client
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<string> IncludedScripts
        {
            get
            {
                return m_IncludedScripts;
            }
        }

        /// <summary>
        /// Restituisce la collezione dei fogli di stile che vengono inclusi nella pagina sul client
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<string> IncludedCSS
        {
            get
            {
                return m_IncludedCSS;
            }
        }

        protected virtual void CreateHeaderHTML(HtmlTextWriter writer)
        {
            writer.Write("<title>" + this.AC.Title);
            if (this.CurrentModule is object)
            {
                writer.Write(" : " + this.CurrentModule.Description);
            }

            writer.Write("</title>");
            for (int i = 0, loopTo = IncludedScripts.Count - 1; i <= loopTo; i++)
            {
                string script = IncludedScripts[i];
                writer.Write("<script type=\"text/javascript\" src=\"" + script + "\"></script>");
            }

            if (!string.IsNullOrEmpty(GetGoogleMapsApiKey()))
            {
                IncludeGoogleMapsScript(writer, GetGoogleMapsApiKey());
            }

            for (int i = 0, loopTo1 = IncludedCSS.Count - 1; i <= loopTo1; i++)
            {
                string css = IncludedCSS[i];
                writer.Write("<link rel=\"stylesheet\" href=\"" + css + "\" type=\"text/css\">");
            }

            writer.Write("<script language=\"javascript\" type=\"text/javascript\">");
            if (this.AC.IsDebug())
                writer.Write("__DEBUG = true;");
            writer.Write("__BASENAME = '" + this.AC.BaseName + "';");

            // Dim str As String = DMD.XML.Utils.Serializer.Serialize(Sistema.Users.CurrentUser)
            // str = Replace(str, vbCr, "")
            // str = Replace(str, vbLf, "")
            // str = Replace(str, Chr(34), "\" & Chr(34))
            // writer.Write("Sistema_Users_currentUserXML = """ & str & """;")

            // Str = DMD.XML.Utils.Serializer.Serialize(Sistema.Users.CurrentUser.Settings)
            // str = Replace(str, vbCr, "")
            // str = Replace(str, vbLf, "")
            // str = Replace(str, Chr(34), "\" & Chr(34))

            // writer.Write("Sistema_Users_currentUserSettingXML = """ & str & """;")
            writer.Write("</script>");
        }

        public CWebControlsCollection WebControls
        {
            get
            {
                if (m_WebControls is null)
                    m_WebControls = new CWebControlsCollection();
                return m_WebControls;
            }
        }

        protected virtual void GetInnerHTML(HtmlTextWriter writer)
        {
            int i;
            var loopTo = WebControls.Count - 1;
            for (i = 0; i <= loopTo; i++)
                WebControls[i].CreateHTML(writer);
        }


        // Public Overridable Sub GetLoginHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
        // writer.Write("<script language=""javascript"" type=""text/javascript"">")
        // writer.Write(vbNewLine)
        // writer.Write("var _ctrLogin = new CLoginControl(""ctrLogin"");")
        // writer.Write(vbNewLine)
        // writer.Write("Window.addListener(""onload"", new Function(""setTimeout('_ctrLogin.init();', 100)""));")
        // writer.Write(vbNewLine)
        // writer.Write("</script>")
        // End Sub

        protected virtual void CreateBodyHTML(HtmlTextWriter writer)
        {
            writer.Write("<div class=\"mainForm\" id=\"mainForm\"  >");
            writer.Write("<form name=\"frmMain\" id=\"frmMain\" action=\"" + CurrentPage.PageName + "\" method=\"post\" onsubmit=\"return false;\">");
            // If (Not Me.RequiresLogin) OrElse (Me.IsUserLogged) Then
            GetInnerHTML(writer);
            // Else
            // Me.GetLoginHTML(writer)
            // End If

            writer.Write("<input type=\"hidden\" name=\"_m\" id=\"_m\" value=\"" + Databases.GetID(CurrentModule) + "\" />");
            writer.Write("<input type=\"hidden\" name=\"_a\" id=\"_a\" value=\"" + DMD.WebUtils.HtmlEncode(this.n2str("_a")) + "\" />");
            writer.Write("<input type=\"hidden\" name=\"_s\" id=\"_s\" value=''/>");
            writer.Write("</form>");
            writer.Write("</div>");
            // writer.Write("<iframe name=""frmHidden"" id=""frmHidden"" style=""display:none;"" src=""/blank.html""></iframe>")
            // writer.Write("<iframe name=""" & Me.ApplicationContext.BaseName & """ id=""" & Me.ApplicationContext.BaseName & """ style=""display:none;"" src=""/blank.html""></iframe>")
            writer.Write("<script language=\"javascript\" type=\"text/javascript\">");
            writer.Write("var __dmdpage = \"" + DMDPage + "\";" + DMD.Strings.vbNewLine);
            writer.Write("function ShowMessanger() { Messenger.ShowSendMessage(); return false; }");
            if (this.n2str("interface", "0") != "0")
            {
                writer.Write("Cookies.setCookie(\"window_w\", Window.getWidth());");
                writer.Write("Cookies.setCookie(\"window_h\", Window.getHeight());");
            }
            // writer.Write ("if (typeof(window.onload) == ""function"") Window.addListener(""onload"", window.onload);"
            // writer.Write ("window.onload = new Function(""Window.dispatchEvent('onload')"");"
            if (!string.IsNullOrEmpty(Sistema.Formats.ToString(Session["Error_Message"])))
            {
                writer.Write("alert('" + DMD.WebUtils.ToJS(DMD.Strings.CStr(Session["Error_Message"])) + "');");
                Session["Error_Message"] = "";
            }

            writer.Write("</script>");
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Response.Buffer = false;
            if (!CustomRendering())
            {
                for (int i = 0, loopTo = IncludedScripts.Count - 1; i <= loopTo; i++)
                {
                    string item = IncludedScripts[i];
                    ClientScript.RegisterClientScriptInclude(GetType(), "resScript" + i, item);
                }
            }
        }

        protected virtual void CreateHTML(HtmlTextWriter writer)
        {
            writer.Write("<!DOCTYPE html>");
            // writer.Write ("<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""it"" lang=""it"">"
            writer.Write("<html>");
            writer.Write("<head>");
            CreateHeaderHTML(writer);
            writer.Write("</head>");
            writer.Write("<body>");
            CreateBodyHTML(writer);
            writer.Write("</body>");
            writer.Write("</html>");
        }

        protected override void InternalRender(HtmlTextWriter writer)
        {
            if (CustomRendering())
            {
                CreateHTML(writer);
            }
            else
            {
                base.InternalRender(writer);
            }
        }

        public override void Dispose()
        {
            if (m_WebControls is object)
            {
                foreach (WebControl c in m_WebControls)
                {
                    if (c is IDisposable)
                        ((IDisposable)c).Dispose();
                }

                m_WebControls = null;
            }

            m_IncludedScripts = null;
            m_IncludedCSS = null;
            base.Dispose();
        }
    }
}