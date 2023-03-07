using System;

namespace minidom.S300
{
    public class S300EventArgs : EventArgs
    {
        private S300Device m_Device;

        public S300EventArgs()
        {
            m_Device = null;
        }

        public S300EventArgs(S300Device device)
        {
            if (device is null)
                throw new ArgumentNullException("device");
            m_Device = device;
        }

        public S300Device Device
        {
            get
            {
                return m_Device;
            }
        }
    }
}