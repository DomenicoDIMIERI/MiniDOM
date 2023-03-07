using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class WebSite
    {
        [Serializable]
        public sealed class CUserAgentsClass : Sistema.CModulesClass<CUserAgent>
        {
            internal CUserAgentsClass() : base("modUserAgents", typeof(CUserAgentsCursor), -1)
            {
            }

            public CUserAgent GetItemByString(string userAgent)
            {
                userAgent = Strings.Trim(userAgent);
                if (string.IsNullOrEmpty(userAgent))
                    return null;
                var items = LoadAll();
                foreach (var item in items)
                {
                    if (DMD.Strings.Compare(item.UserAgent, userAgent) == 0)
                        return item;
                }

                return null;
            }
        }

        private static CUserAgentsClass m_UserAgents = null;

        public static CUserAgentsClass UserAgents
        {
            get
            {
                if (m_UserAgents is null)
                    m_UserAgents = new CUserAgentsClass();
                return m_UserAgents;
            }
        }
    }
}