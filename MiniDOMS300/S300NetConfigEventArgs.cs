
namespace minidom.S300
{
    public class S300NetConfigEventArgs : S300EventArgs
    {
        private CKT_DLL.NETINFO m_Config;

        public S300NetConfigEventArgs()
        {
            m_Config = default;
        }

        public S300NetConfigEventArgs(S300Device device, CKT_DLL.NETINFO config) : base(device)
        {
            // If (config Is Nothing) Then Throw New ArgumentNullException("config")
            m_Config = config;
        }

        public CKT_DLL.NETINFO Config
        {
            get
            {
                return m_Config;
            }
        }
    }
}