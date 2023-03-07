using System;
using System.Data;
using System.Globalization;
using DMD;
using minidom.Internals;

namespace minidom
{
    namespace Internals
    {
        [Serializable]
        public sealed class CVisitedPagesClass 
            : Sistema.CModulesClass<WebSite.VisitedPage>
        {
            internal CVisitedPagesClass() : base("modVisitedPages", typeof(WebSite.VisitedPagesCursor), 0)
            {
            }

            public string ChangeQueryString(string qs, string keyName, string keyValue)
            {
                string[] items;
                string iName;
                int i, p;
                bool trovato;
                trovato = false;
                items = Strings.Split(qs, "&");
                var loopTo = DMD.Arrays.UBound(items);
                for (i = DMD.Arrays.LBound(items); i <= loopTo; i++)
                {
                    p = Strings.InStr(items[i], "=");
                    if (p > 0)
                    {
                        iName = Strings.Trim(Strings.Left(items[i], p - 1));
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.LCase(iName) ?? "", Strings.LCase(Strings.Trim(keyName)) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        {
                            trovato = true;
                            items[i] = keyName + "=" + keyValue;
                            break;
                        }
                    }
                }

                if (!trovato)
                {
                    Array.Resize(ref items, 1 + DMD.Arrays.UBound(items) + 1);
                    items[DMD.Arrays.UBound(items)] = keyName + "=" + keyValue;
                }

                return Strings.Join(items, "&");
            }

            public string GetQueryStringValue(string qs, string keyName)
            {
                string[] items;
                string iName, retVal;
                int i, p;
                bool trovato;
                trovato = false;
                items = Strings.Split(qs, "&");
                retVal = "";
                var loopTo = DMD.Arrays.UBound(items);
                for (i = DMD.Arrays.LBound(items); i <= loopTo; i++)
                {
                    p = Strings.InStr(items[i], "=");
                    if (p > 0)
                    {
                        iName = Strings.Trim(Strings.Left(items[i], p - 1));
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.LCase(iName) ?? "", Strings.LCase(Strings.Trim(keyName)) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        {
                            trovato = true;
                            retVal = Strings.Mid(items[i], p + 1);
                            break;
                        }
                    }
                }

                return retVal;
            }

            public int CountPageVisits(string pageURL)
            {
                IDataReader dbRis;
                int cnt;
                // Lanciamo la query per recuperare il numero di visite alla pagina specificata
                dbRis = Databases.APPConn.ExecuteReader("SELECT [visite] FROM [TabContatore] WHERE [pagina]=" + Databases.DBUtils.DBString(pageURL) + "");
                if (dbRis.Read() == false)
                {
                    cnt = 0;
                }
                else
                {
                    cnt = Sistema.Formats.ToInteger(dbRis["visite"]);
                }

                dbRis.Dispose();
                dbRis = null;
                return cnt;
            }


            /// <summary>
        /// Restituisce il numero di visite effettuate alla pagina corrente
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public int CountThisPageVisits()
            {
                return CountPageVisits(WebSite.ASP_Request.ServerVariables["SCRIPT_NAME"]);
            }

            /// <summary>
        /// Incrementa di 1 il numero di visite alla pagina e restituisce il valore aggiornato
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IncreasePageVisits(string pageURL)
            {
                string dbSQL;
                int cnt;
                // Incrementiamo il numero di visite.
                // Risolve potenziali problemi dovuti alla sincronizzazione 
                cnt = CountPageVisits(pageURL);
                if (cnt == 0)
                {
                    dbSQL = "INSERT INTO [TabContatore] ([pagina], [visite]) VALUES (" + Databases.DBUtils.DBString(pageURL) + ", 1)";
                    cnt = 1;
                }
                else
                {
                    cnt = cnt + 1;
                    dbSQL = "UPDATE [TabContatore] SET [visite]=" + cnt + " WHERE [pagina] = " + Databases.DBUtils.DBString(pageURL) + "";
                }

                Databases.LOGConn.ExecuteCommand(dbSQL);
                return cnt;
            }
        }
    }

    public partial class WebSite
    {
        private static CVisitedPagesClass m_VisitedPages = null;

        public static CVisitedPagesClass VisitedPages
        {
            get
            {
                if (m_VisitedPages is null)
                    m_VisitedPages = new CVisitedPagesClass();
                return m_VisitedPages;
            }
        }
    }
}