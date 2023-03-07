using System.Diagnostics;
using DMD;

namespace minidom.CallManagers
{
    public class ChannelType : IDObject
    {
        private string m_State;
        private string m_Channel;
        private string m_CallerID;
        private string m_CallerIDNum;
        private string m_CallerIDName;

        public ChannelType()
        {
            // DMDObject.IncreaseCounter(Me)
        }

        public ChannelType(Events.Newchannel e) : this()
        {
            m_State = e.State;
            m_Channel = e.Channel;
            m_CallerID = e.CallerID;
            m_CallerIDNum = e.CallerIDNum;
            m_CallerIDName = e.CallerIDName;
            UniqueID = e.UniqueID;
        }

        public string State
        {
            get
            {
                return m_State;
            }

            set
            {
                m_State = Strings.Trim(value);
            }
        }

        public string Channel
        {
            get
            {
                return m_Channel;
            }

            set
            {
                m_Channel = Strings.Trim(value);
            }
        }

        public string CallerID
        {
            get
            {
                return m_CallerID;
            }

            set
            {
                m_CallerID = Strings.Trim(value);
            }
        }

        public string CallerIDNum
        {
            get
            {
                return m_CallerIDNum;
            }

            set
            {
                m_CallerIDNum = Strings.Trim(value);
            }
        }

        public string CallerIDName
        {
            get
            {
                return m_CallerIDName;
            }

            set
            {
                m_CallerIDName = Strings.Trim(value);
            }
        }

        public void Hangup()
        {
            Debug.Print("Canale chiuso: " + Channel);
        }

        ~ChannelType()
        {
            // DMDObject.DecreaseCounter(Me)
        }
    }
}