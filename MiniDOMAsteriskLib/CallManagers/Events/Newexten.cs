
namespace minidom.CallManagers.Events
{
    public class Newexten : AsteriskEvent
    {
        public Newexten() : base("Newexten")
        {
        }

        public Newexten(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Context
        {
            get
            {
                return GetAttribute("Context");
            }
        }

        public string Extension
        {
            get
            {
                return GetAttribute("Extension");
            }
        }

        public int Priority
        {
            get
            {
                return GetAttribute("Priority", 0);
            }
        }

        public string Application
        {
            get
            {
                return GetAttribute("Application");
            }
        }

        public string AppData
        {
            get
            {
                return GetAttribute("AppData");
            }
        }

        public string UniqueID
        {
            get
            {
                return GetAttribute("UniqueID");
            }
        }
    }
}