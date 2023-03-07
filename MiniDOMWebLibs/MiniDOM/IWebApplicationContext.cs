using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
        /// Interfaccia implementata da un applicazione web
        /// </summary>
        public interface IWebApplicationContext
            : IApplicationContext
        {

            /// <summary>
            /// Restituisce o imposta la configurazione dell'app
            /// </summary>
            SiteConfig WebSiteConfig { get; set; }

            /// <summary>
            /// Salva la configurazione dell'app
            /// </summary>
            /// <param name="value"></param>
            void SaveConfiguration(SiteConfig value);

            /// <summary>
            /// Carica la configurazione dell'app
            /// </summary>
            /// <returns></returns>
            SiteConfig LoadConfiguration();


        }
    }
}