using DMD.XML;

namespace minidom.CallManagers.Events
{
    public class ExtensionStatus : AsteriskEvent
    {
        public ExtensionStatus() : base("ExtensionStatus")
        {
        }

        public ExtensionStatus(string[] rows) : base(rows)
        {
        }

        public string Exten
        {
            get
            {
                return GetAttribute("Exten");
            }
        }

        public string Context
        {
            get
            {
                return GetAttribute("Context");
            }
        }

        public int Status
        {
            get
            {
                return DMD.Integers.ValueOf(GetAttribute("Status"));
            }
        }
    }
}