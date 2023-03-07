using System.Windows.Forms;
using DMD.XML;

namespace minidom.PBX
{
    public class PagineBiancheAnalizer
        : WebPageAnalizer
    {
        public class Item
        {
            public string Number;
            public string Name;
            public string Link;
            public string Insegna;
            public CCollection<ItemAddress> Addresses = new CCollection<ItemAddress>();

            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append(Number + ") " + Name + " (" + Insegna + ")");
                foreach (ItemAddress a in Addresses)
                {
                    ret.Append(DMD.Strings.vbCrLf);
                    ret.Append(a.ToString());
                }

                ret.Append(DMD.Strings.vbCrLf);
                return ret.ToString();
            }
        }

        public class ItemAddress
        {
            public string StreetAddress;
            public string PostalCode;
            public string Locality;
            public string Latitude;
            public string Longitude;

            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append(StreetAddress);
                ret.Append(" - ");
                ret.Append(PostalCode);
                ret.Append(" ");
                ret.Append(Locality);
                return ret.ToString();
            }
        }

        private CCollection<Item> m_Results = new CCollection<Item>();

        public PagineBiancheAnalizer()
        {
        }

        public CCollection<Item> Results
        {
            get
            {
                return m_Results;
            }
        }

        public void CercaInfoNumero(string numero)
        {
            m_Results = new CCollection<Item>();
            Analize("http://www.paginebianche.it/ricerca-da-numero?qs=" + DMD.Strings.Replace(numero, " ", ""));
        }

        private System.Xml.XmlElement GetChildOfType(System.Xml.XmlElement node, string tagName)
        {
            var items = node.GetElementsByTagName(tagName);
            if (items is null || items.Count == 0)
            {
                return null;
            }
            else
            {
                return (System.Xml.XmlElement)items[0];
            }
        }

        protected override void OnDataReady(DataEventArgs e)
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(e.Data);
            m_Results.Clear();
            int i = 1;
            var node = doc.GetElementById("co_" + i);
            while (node is object)
            {
                var o = new Item();
                o.Number = i.ToString();
                var a = GetChildOfType(node, "a");
                if (a is null)
                    return;
                o.Name = a.InnerText;
                o.Link = a.GetAttribute("href");
                int j = 1;
                var node1 = doc.GetElementById("addr_" + j);
                while (node1 is object)
                {
                    var oa = new ItemAddress();
                    foreach (HtmlElement e1 in node1.ChildNodes)
                    {
                        switch (DMD.Strings.Trim(e1.GetAttribute("itemprop")) ?? "")
                        {
                            case "streetAddress":
                                {
                                    oa.StreetAddress = e1.InnerText;
                                    break;
                                }

                            case "postalCode":
                                {
                                    oa.PostalCode = e1.InnerText;
                                    break;
                                }

                            case "addressLocality":
                                {
                                    oa.Locality = e1.InnerText;
                                    break;
                                }
                        }
                    }

                    o.Addresses.Add(oa);
                    j += 1;
                    node1 = doc.GetElementById("addr_" + j);
                }

                m_Results.Add(o);
                i += 1;
                node = doc.GetElementById("co_" + i);
            }

            base.OnDataReady(e);
        }

        public override string ToString()
        {
            var ret = new System.Text.StringBuilder();
            foreach (Item o in Results)
            {
                if (ret.Length > 0)
                    ret.Append(DMD.Strings.vbCrLf);
                ret.Append(o.ToString());
            }

            return ret.ToString();
        }

        public override void Cancel()
        {
            m_Results = new CCollection<Item>();
            base.Cancel();
        }
    }
}