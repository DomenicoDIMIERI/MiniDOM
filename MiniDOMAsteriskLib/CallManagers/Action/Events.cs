using System;
using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{


    /// <summary>
    /// Contol Event Flow: Enable/Disable sending of events to this manager client.
    /// </summary>
    /// <remarks></remarks>
    public class Events : AsyncAction
    {
        private DebugFlags m_EventMask = DebugFlags.On;

        public Events() : base("Events")
        {
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <param name="enabled">[in] Se vero abilita tutti gli eventi</param>
        /// <remarks></remarks>
        public Events(bool enabled) : this()
        {
            m_EventMask = enabled? DebugFlags.On : DebugFlags.Off;
        }

        /// <summary>
        /// Inizializza un nuovo oggetto
        /// </summary>
        /// <param name="flags">[in] flags</param>
        /// <remarks></remarks>
        public Events(DebugFlags flags) : this()
        {
            m_EventMask = flags;
        }

        public DebugFlags EventMask
        {
            get
            {
                return m_EventMask;
            }
        }

        protected override string GetCommandText()
        {
            return base.GetCommandText() + "EVENTMASK: " + GetMask(EventMask) + DMD.Strings.vbCrLf;
        }

        protected string GetMask(DebugFlags flags)
        {
            switch (flags)
            {
                case DebugFlags.On:
                    {
                        return "ON";
                    }

                case DebugFlags.Off:
                    {
                        return "OFF";
                    }

                default:
                    {
                        string ret = "";
                        foreach (DebugFlags f in Enum.GetValues(typeof(DebugFlags)))
                        {
                            if ((flags & f) == f)
                            {
                                if (!string.IsNullOrEmpty(ret))
                                    ret += ",";
                                ret += Enum.GetName(typeof(DebugFlags), f);
                            }
                        }

                        return ret;
                    }
            }
        }

        public override bool IsAsync()
        {
            if (EventMask == DebugFlags.On)
                return true;
            return base.IsAsync();
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new EventsResponse(this);
        }
    }
}