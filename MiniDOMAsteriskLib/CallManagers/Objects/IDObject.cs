using DMD;

namespace minidom.CallManagers
{
    public class IDObject : AsteriskObject
    {
        private string m_UniqueID;

        public IDObject()
        {
            m_UniqueID = "";
        }

        public string UniqueID
        {
            get
            {
                return m_UniqueID;
            }

            set
            {
                m_UniqueID = Strings.Trim(value);
            }
        }
    }
}