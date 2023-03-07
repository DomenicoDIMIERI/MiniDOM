using System;

namespace minidom.CallManagers.Events
{
    public class HangupEvent : AsteriskEvent
    {
        public HangupEvent() : base("Hangup")
        {
        }

        public HangupEvent(AsteriskCallManager m, AsteriskEvent e) : base(m, e)
        {
        }

        public HangupEvent(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string Uniqueid
        {
            get
            {
                return GetAttribute("Uniqueid");
            }
        }

        public HangupCauses Cause
        {
            get
            {
                return (HangupCauses)GetAttribute("Cause", 0);
            }
        }

        public string CauseTxt
        {
            get
            {
                return GetAttribute("Cause-txt");
            }
        }

        public string CauseEx
        {
            get
            {
                return Enum.GetName(typeof(HangupCauses), Cause);
            }
        }

        public static string GetCauseName(HangupCauses cause)
        {
            return Enum.GetName(typeof(HangupCauses), cause);
        }
    }
}