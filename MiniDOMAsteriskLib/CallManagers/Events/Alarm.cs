
namespace minidom.CallManagers.Events
{
    public class AlarmType : AsteriskEvent
    {
        public AlarmType() : base("Alarm")
        {
        }

        public AlarmType(string[] rows) : base(rows)
        {
        }

        /// <summary>
        /// (Red|Yellow|Blue|No|Unknown) Alarm|Recovering|Loopback|Not Open|None
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Alarm
        {
            get
            {
                return GetAttribute("Alarm");
            }
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