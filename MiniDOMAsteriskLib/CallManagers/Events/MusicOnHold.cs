
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// Occurs when a channel is placed on hold/unhold and music is played to the caller.
    /// </summary>
    /// <remarks></remarks>
    public class MusicOnHold : AsteriskEvent
    {
        public MusicOnHold() : base("MusicOnHold")
        {
        }

        public MusicOnHold(string[] rows) : base(rows)
        {
        }

        public string Channel
        {
            get
            {
                return GetAttribute("Channel");
            }
        }

        public string State
        {
            get
            {
                return GetAttribute("State");
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