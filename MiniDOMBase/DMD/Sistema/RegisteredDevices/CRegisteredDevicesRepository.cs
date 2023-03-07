using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CRegisteredDevice"/>
        /// </summary>
        [Serializable]
        public sealed class CRegisteredDevicesRepository
            : CModulesClass<CRegisteredDevice>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredDevicesRepository() 
                : base("modRegisteredDevices", typeof(CRegisteredDeviceCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce la collezione delle periferiche registrate in base alla classe 
            /// </summary>
            /// <param name="className"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CRegisteredDevice> GetDevicesByClassName(string className)
            {
                className = Strings.Trim(className);
                if (string.IsNullOrEmpty(className))
                    throw new ArgumentNullException("className");
                var ret = new CCollection<CRegisteredDevice>();
                foreach(var item in this.LoadAll())
                {
                    if (item.ClassName == className)
                        ret.Add(item);
                }
                return ret;
            }

            /// <summary>
            /// Restituisce la collezione delle periferiche gestite dal driver 
            /// </summary>
            /// <param name="driverName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CRegisteredDevice> GetDevicesByDriver(string driverName)
            {
                driverName = Strings.Trim(driverName);
                if (string.IsNullOrEmpty(driverName))
                    throw new ArgumentNullException("driverName");
                var ret = new CCollection<CRegisteredDevice>();
                foreach (var item in this.LoadAll())
                {
                    if (item.DriverName == driverName)
                        ret.Add(item);
                }
                return ret;
            }

            /// <summary>
            /// Restituisce la collezione delle periferiche installate all'indirizzo specificato
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CRegisteredDevice> GetDevicesByAddress(string address)
            {
                address = Strings.Trim(address);
                if (string.IsNullOrEmpty(address))
                    throw new ArgumentNullException("address");
                var ret = new CCollection<CRegisteredDevice>();
                foreach (var item in this.LoadAll())
                {
                    if (item.Address == address)
                        ret.Add(item);
                }
                return ret;
            }
        }

        
    }


    public partial class Sistema
    {

        private static CRegisteredDevicesRepository m_RegisteredDevices = null;

        /// <summary>
        /// Repository di oggetti <see cref="CRegisteredDevice"/>
        /// </summary>
        public static CRegisteredDevicesRepository RegisteredDevices
        {
            get
            {
                if (m_RegisteredDevices is null) m_RegisteredDevices = new CRegisteredDevicesRepository();
                return m_RegisteredDevices;
            }
        }

    }

}