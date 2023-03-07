using System.Globalization;
using DMD;

namespace minidom
{
    public partial class WebSite
    {
        public class CCurrentPage : VisitedPage
        {

            // Private m_LogTime As New StatsItem
            private bool m_Unhautorized = false;

            public CCurrentPage()
            {
            }



            /// <summary>
        /// Incrementa di 1 il numero di visite alla pagina e restituisce il valore aggiornato
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IncreaseThisPageVisits()
            {
                return VisitedPages.IncreasePageVisits(PageName);
            }

            public void SaveLog()
            {
                // Dim posted As String = Me.PostedData
                // Dim getted As String = Me.QueryString
                Save(true);
            }

            /// <summary>
        /// Metodo che indica l'inizio di una richiesta al sito
        /// </summary>
        /// <remarks></remarks>
            public void StartExecution()
            {
            }

            /// <summary>
        /// Metodo che indica la fine dell'esecuzione della pagina
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        /// <remarks></remarks>
            public void EndExecution(string statusCode, string statusDescription)
            {
                if (m_Unhautorized)
                    return;
                StatusCode = statusCode;
                StatusDescription = statusDescription;
                ExecTime = (float)((DMD.DateUtils.Now() - Data).TotalMilliseconds / 1000d); // Me.m_LogTime.ExecTime

                // Me.SaveLog()
            }

            /// <summary>
        /// Metodo che indica alla pagina corrente che subito dopo verrà invocato il metodo Server.TransferTo
        /// </summary>
        /// <param name="url"></param>
        /// <remarks></remarks>
            public void TransferredTo(string url)
            {
                if (m_Unhautorized)
                    return;
                StatusCode = "200";
                StatusDescription = "Esecuzione trasferita a \"" + url + "\" ";
                SaveLog();
            }

            public void NotifyUnhautorized(string msg)
            {
                m_Unhautorized = true;
                StatusCode = "200.403";
                StatusDescription = msg;
                ExecTime = (float)((DMD.DateUtils.Now() - Data).TotalMilliseconds / 1000d); // Me.m_LogTime.ExecTime
                Save(true);
                SetChanged(false);
            }

            internal void Initialize(System.Web.UI.Page page)
            {
                Data = DMD.DateUtils.Now();
                // Me.m_LogTime.LastRun = Me.Data

                SetSession(Instance.CurrentSession);
                SetUser(Sistema.Users.CurrentUser);
                Secure = CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.UCase(page.Request.ServerVariables["HTTPS"]), "ON", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;
                PageName = page.Request.ServerVariables["SCRIPT_NAME"];
                Referrer = page.Request.ServerVariables["HTTP_REFERER"];
                QueryString = page.Request.QueryString.ToString();
                // Me.PostedData = Request.Form.ToString 
            }
        }
    }
}