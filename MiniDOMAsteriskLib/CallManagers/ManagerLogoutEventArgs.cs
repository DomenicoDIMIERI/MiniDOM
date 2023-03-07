using System;

namespace minidom.CallManagers
{

    /// <summary>
    /// Informazioni sull'accesso ad un callmanager
    /// </summary>
    /// <remarks></remarks>
    public class ManagerLogoutEventArgs : EventArgs
    {
        private string m_UserName;
        private string m_Status;

        public ManagerLogoutEventArgs()
        {
            DMDObject.IncreaseCounter(this);
            m_UserName = "";
        }

        public ManagerLogoutEventArgs(string userName, string status) : this()
        {
            m_UserName = userName;
            m_Status = status;
        }

        ~ManagerLogoutEventArgs()
        {
            DMDObject.DecreaseCounter(this);
        }

        public string UserName
        {
            get
            {
                return m_UserName;
            }
        }

        public string Status
        {
            get
            {
                return m_Status;
            }
        }
    }
}