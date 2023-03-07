
namespace minidom.CallManagers.Events
{
    public class ShutdownType : AsteriskEvent
    {
        public ShutdownType() : base("Shutdown")
        {
        }

        public ShutdownType(string[] rows) : base(rows)
        {
        }

        public string Shutdown
        {
            get
            {
                return GetAttribute("Shutdown");
            }
        }

        public string Restart
        {
            get
            {
                return GetAttribute("Restart");
            }
        }
    }
}