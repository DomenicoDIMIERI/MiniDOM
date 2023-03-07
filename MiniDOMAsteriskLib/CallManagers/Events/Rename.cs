
namespace minidom.CallManagers.Events
{
    public class Rename : AsteriskEvent
    {
        public Rename() : base("Rename")
        {
        }

        public Rename(string[] rows) : base(rows)
        {
        }

        public string Oldname
        {
            get
            {
                return GetAttribute("Oldname");
            }
        }

        public string Newname
        {
            get
            {
                return GetAttribute("Newname");
            }
        }

        public string CallerID
        {
            get
            {
                return GetAttribute("CallerID");
            }
        }
    }
}