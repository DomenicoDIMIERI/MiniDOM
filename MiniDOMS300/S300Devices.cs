using System;
using System.Collections;
using DMD.XML;

namespace minidom.S300
{
    public sealed class S300Devices
    {


        /// <summary>
        /// Evento generato quando il dispositivo si connette correttamente al PC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public event DeviceConnectedEventHandler DeviceConnected;

        public delegate void DeviceConnectedEventHandler(object sender, S300EventArgs e);

        /// <summary>
        /// Evento generato quando il dispositivo si disconnette dal PC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static event DeviceDisconnectedEventHandler DeviceDisconnected;

        public delegate void DeviceDisconnectedEventHandler(object sender, S300EventArgs e);

        /// <summary>
        /// Evento generato quando viene modificata la configurazione di rete del dispositivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static event DeviceNetworkConfigurationChangedEventHandler DeviceNetworkConfigurationChanged;

        public delegate void DeviceNetworkConfigurationChangedEventHandler(object sender, S300NetConfigEventArgs e);

        private static ArrayList m_Devices = new ArrayList();

        private S300Devices()
        {
        }

        /// <summary>
        /// Inizializza la libreria
        /// </summary>
        public static void Initialize()
        {
        }

        /// <summary>
        /// Finalizza la libreria (nessun metodo di questa libreria deve essere chiamato dopo Terminate)
        /// </summary>
        public static void Terminate()
        {
            CKT_DLL.CKT_Disconnect();
        }

        /// <summary>
        /// Restituisce un array contenente l'ID di tutti i dispositivi connessi
        /// </summary>
        /// <returns></returns>
        public static int[] GetConnectedDevicesIDs()
        {
            var SnoPtr = IntPtr.Zero;
            int count = CKT_DLL.CKT_ReportConnections(ref SnoPtr);
            var ret = DMD.Arrays.Empty<int>();
            if (count > 0)
            {
                ret = new int[count];
                for (int i = 0, loopTo = count - 1; i <= loopTo; i++) // (i = 1; i <= count; i++)
                {
                    int Sno = 0;
                    CKT_DLL.PCopyMemory(ref Sno, new IntPtr(SnoPtr.ToInt32() + i * 4), 4);
                    ret[i] = Sno;
                }

                CKT_DLL.CKT_FreeMemory(SnoPtr);
            }

            return ret;
        }

        /// <summary>
        /// Metodo richiamato quando un dispositivo si connette
        /// </summary>
        /// <param name="e"></param>
        internal static void NotifyConnected(S300EventArgs e)
        {
            DeviceDisconnected?.Invoke(e.Device, e);
        }

        /// <summary>
        /// Metodo richiamato quando un dispositivo si disconnette
        /// </summary>
        /// <param name="e"></param>
        internal static void NotifyDisconnected(S300EventArgs e)
        {
            DeviceDisconnected?.Invoke(e.Device, e);
        }

        /// <summary>
        /// Metodo richiamato quando viene modificata la configurazione di rete di un dispositivo
        /// </summary>
        /// <param name="e"></param>
        internal static void NotifyNetworkConfigurationChanged(S300NetConfigEventArgs e)
        {
            DeviceNetworkConfigurationChanged?.Invoke(e.Device, e);
        }

        /// <summary>
        /// Registra la periferica nel sistema
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static S300Device RegisterDevice(int id, string address)
        {
            if (id == 0)
                return null;
            address = DMD.Strings.Trim(address);
            if (string.IsNullOrEmpty(address))
                return null;
            S300Device dev = null;
            foreach (S300Device currentDev in m_Devices)
            {
                dev = currentDev;
                if (dev.DeviceID == id && string.Compare(dev.Address, address, true) == 0)
                {
                    return dev;
                }
            }

            dev = new S300Device(id, address);
            m_Devices.Add(dev);
            return dev;
        }

        /// <summary>
        /// Elimina la registrazione della periferica nel sistema
        /// </summary>
        /// <param name="device"></param>
        public static void UnregisterDevice(S300Device device)
        {
            if (device is null)
                throw new ArgumentNullException("device");
            if (device.IsConnected())
                throw new InvalidOperationException("La periferica deve essere disconnessa prima di chiamare il metodo UnregisterDevice");
            m_Devices.Remove(device);
        }
    }
}