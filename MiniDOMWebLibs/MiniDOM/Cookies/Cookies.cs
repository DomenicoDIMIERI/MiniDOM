using System;
using System.Diagnostics;
using System.Globalization;
using DMD;

namespace minidom
{
    public partial class WebSite
    {
        public sealed class CCookiesClass
        {
            internal CCookiesClass()
            {
            }

            public string GetCookie(string name, string defValue = DMD.Strings.vbNullString)
            {
                var ret = ASP_Request.Cookies[name];
                if (ret is null)
                    return defValue;
                try
                {
                    return ret.Value;
                }
                catch (Exception )
                {
                    return defValue;
                }
            }

            public bool IsCookieSet(string name)
            {
                var ret = ASP_Request.Cookies[name];
                return ret is object;
            }

            public bool CanWriteCookies()
            {
                return CultureInfo.CurrentCulture.CompareInfo.Compare(GetCookie("__DMDWLIBFSE", ""), "YES", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;
            }

            public void SetCanWriteCookies(bool value)
            {
                if (value)
                {
                    _SetCookie("__DMDWLIBFSE", "YES");
                }
                else
                {
                    _SetCookie("__DMDWLIBFSE", "", DMD.DateUtils.Now());
                }
            }

            private void _SetCookie(string cookieName, string value, DateTime? expireDate = default)
            {
                System.Web.HttpCookie c;
                if (IsCookieSet(cookieName))
                {
                    c = ASP_Response.Cookies[cookieName];
                }
                else
                {
                    c = new System.Web.HttpCookie(cookieName, value);
                    ASP_Response.Cookies.Add(c);
                }

                if (expireDate.HasValue)
                {
                    c.Expires = expireDate.Value;
                }
                else
                {
                    c.Expires = DMD.DateUtils.MakeDate(2500, 1, 1);
                }
            }

            public void SetCookie(string cookieName, string value, DateTime? expireDate = default)
            {
                if (!CanWriteCookies())
                {
                    Debug.Print("Permesso Negato per il cookie: " + cookieName + ", " + value);
                    return;
                }
                else
                {
                }

                System.Web.HttpCookie c;
                if (IsCookieSet(cookieName))
                {
                    c = ASP_Response.Cookies[cookieName];
                }
                else
                {
                    c = new System.Web.HttpCookie(cookieName, value);
                    ASP_Response.Cookies.Add(c);
                }

                if (expireDate.HasValue)
                {
                    c.Expires = expireDate.Value;
                }
                else
                {
                    c.Expires = DMD.DateUtils.MakeDate(2500, 1, 1);
                }
            }
        }

        public static readonly CCookiesClass Cookies = new CCookiesClass();
    }
}