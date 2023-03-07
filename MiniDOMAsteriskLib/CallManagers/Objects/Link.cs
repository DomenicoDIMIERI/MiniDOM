
namespace minidom.CallManagers
{
    public class Link : AsteriskObject
    {
        private ChannelType m_Channel1;
        private ChannelType m_Channel2;

        public Link()
        {
        }

        public ChannelType Channel1
        {
            get
            {
                return m_Channel1;
            }

            set
            {
                m_Channel1 = value;
            }
        }

        public ChannelType Channel2
        {
            get
            {
                return m_Channel2;
            }

            set
            {
                m_Channel2 = value;
            }
        }
    }
}