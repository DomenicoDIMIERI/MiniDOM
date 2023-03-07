
namespace minidom.CallManagers.Events
{
    public class AlarmClear : AsteriskEvent
    {
        public AlarmClear() : base("AlarmClear")
        {
        }

        public AlarmClear(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }
    }
}