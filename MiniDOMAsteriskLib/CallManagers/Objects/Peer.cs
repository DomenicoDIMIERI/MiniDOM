using System;
using DMD;

namespace minidom.CallManagers
{
    public class Peer : AsteriskObject
    {
        private string m_Name;
        private string m_Status;
        private DateTime? m_LastUpdated;

        public Peer()
        {
            m_Name = "";
            m_Status = "";
            m_LastUpdated = default;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = Strings.Trim(value);
            }
        }

        public string Status
        {
            get
            {
                return m_Status;
            }

            set
            {
                m_Status = Strings.Trim(value);
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return m_LastUpdated;
            }

            set
            {
                m_LastUpdated = value;
            }
        }
    }
}