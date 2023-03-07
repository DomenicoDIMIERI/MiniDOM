using DMD;
using System;

namespace minidom.S300
{

    /// <summary>
    /// Evento generico del modulo S300
    /// </summary>
    [Serializable]
    public class S300EventArgs 
        : DMDEventArgs
    {
        [NonSerialized] private S300Device m_Device;

        /// <summary>
        /// Costruttore
        /// </summary>
        public S300EventArgs()
        {
            m_Device = null;
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="device"></param>
        public S300EventArgs(S300Device device)
        {
            if (device is null)
                throw new ArgumentNullException("device");
            m_Device = device;
        }

        /// <summary>
        /// Periferica su cui é stato generato l'evento
        /// </summary>
        public S300Device Device
        {
            get
            {
                return m_Device;
            }
        }
    }
}