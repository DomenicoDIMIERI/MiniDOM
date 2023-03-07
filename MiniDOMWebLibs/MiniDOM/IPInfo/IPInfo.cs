using System;
using DMD;
using DMD.XML;
using minidom;
using minidom.Internals;
using static minidom.Sistema;


namespace minidom
{
    public partial class WebSite
    {
        public sealed class CIPInfoClass : Sistema.CModulesClass<CIPInfo>
        {
            internal CIPInfoClass() : base("modIPInfo", typeof(CIPInfoCursor), -1)
            {
            }

            public CIPInfo GetItemByIP(string ip)
            {
                ip = Strings.Trim(ip);
                if (string.IsNullOrEmpty(ip))
                    return null;
                var items = LoadAll();
                foreach (CIPInfo item in items)
                {
                    if (DMD.Strings.Compare(item.IP, ip, true) == 0)
                        return item;
                }

                return null;
            }
        }

        private static CIPInfoClass m_IPInfo = null;

        public static CIPInfoClass IPInfo
        {
            get
            {
                if (m_IPInfo is null)
                    m_IPInfo = new CIPInfoClass();
                return m_IPInfo;
            }
        }
    }
}