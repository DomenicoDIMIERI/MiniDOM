
namespace minidom.CallManagers
{
    public class AsteriskObject
    {
        private AsteriskCallManager m_Owner;

        public AsteriskObject()
        {
            DMDObject.IncreaseCounter(this);
            m_Owner = null;
        }

        public AsteriskCallManager Owner
        {
            get
            {
                return m_Owner;
            }
        }

        ~AsteriskObject()
        {
            DMDObject.DecreaseCounter(this);
        }

        protected internal virtual void SetOwner(AsteriskCallManager value)
        {
            m_Owner = value;
        }
    }
}