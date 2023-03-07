using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DMD.XML;
using DMD;
using minidom.Internals;

namespace minidom.S300
{
    public enum ClearClockingRecordTypes : int
    {
        All = 0,
        New = 1,
        Count = 2
    }

    public class S300Device
    {

        /// <summary>
        /// Evento generato quando il dispositivo si connette correttamente al PC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public event ConnectedEventHandler Connected;

        public delegate void ConnectedEventHandler(object sender, S300EventArgs e);

        /// <summary>
        /// Evento generato quando il dispositivo si disconnette dal PC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public event DisconnectedEventHandler Disconnected;

        public delegate void DisconnectedEventHandler(object sender, S300EventArgs e);

        /// <summary>
        /// Evento generato quando viene modificata la configurazione di rete del dispositivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public event NetworkConfigurationChangedEventHandler NetworkConfigurationChanged;

        public delegate void NetworkConfigurationChangedEventHandler(object sender, S300NetConfigEventArgs e);

        private int m_IDNumber;
        private string m_Address;
        private bool m_Connected;
        private S300UsersCollection m_Users;

        public S300Device()
        {
            m_IDNumber = 0;
            m_Address = "";
            m_Connected = false;
            m_Users = null;
        }

        internal S300Device(int deviceID, string address) : this()
        {
            address = Strings.Trim(address);
            if (deviceID == 0)
                throw new ArgumentException("deviceID non valido");
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException("address non può essere NULL");
            m_IDNumber = deviceID;
            m_Address = address;
        }

        /// <summary>
        /// Inizializza il collegamento ad un dispositivo collegato su una porta seriale
        /// </summary>
        protected void StartSerialCommunication()
        {
            int comPort = DMD.Integers.CInt(Strings.Mid(m_Address, 4));
            int ret = CKT_DLL.CKT_RegisterSno(m_IDNumber, comPort); // If from com
            m_Connected = ret == 1;
            if (!m_Connected)
                throw new Exception("Errore: " + Marshal.GetLastWin32Error().ToString());
        }

        /// <summary>
        /// Inizializza il collegamento ad un dispositivo collegato in rete
        /// </summary>
        protected void StartNetworkCommunication()
        {
            int ret = CKT_DLL.CKT_RegisterNet(m_IDNumber, m_Address); // If from net
            m_Connected = ret == 1;
            if (!m_Connected)
                throw new Exception("Errore: " + Marshal.GetLastWin32Error().ToString());
        }

        /// <summary>
        /// Inizializza il collegamento ad un dispositivo
        /// </summary>
        public virtual void Start()
        {
            if (IsConnected())
                throw new InvalidOperationException("Dispositivo già connesso");
            if (Strings.UCase(m_Address).StartsWith("COM"))
            {
                StartSerialCommunication();
            }
            else
            {
                StartNetworkCommunication();
            }

            var e = new S300EventArgs(this);
            S300Devices.NotifyConnected(e);
            Connected?.Invoke(this, e);
        }

        /// <summary>
        /// Interrompe il collegamento con il dispositivo remoto
        /// </summary>
        public virtual void Stop()
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
            CKT_DLL.CKT_UnregisterSnoNet(m_IDNumber);
            m_Connected = false;
            var e = new S300EventArgs(this);
            S300Devices.NotifyDisconnected(e);
            Disconnected?.Invoke(this, e);
        }

        /// <summary>
        /// Restituisce l'ID della periferica
        /// </summary>
        /// <returns></returns>
        public int DeviceID
        {
            get
            {
                return m_IDNumber;
            }
        }

        /// <summary>
        /// Restituisce l'indirizzo della periferica
        /// </summary>
        /// <returns></returns>
        public string Address
        {
            get
            {
                return m_Address;
            }
        }

        /// <summary>
        /// Restituisce vero se il dispositivo è correttamente connesso al pc
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return m_Connected;
        }

        /// <summary>
        /// Restituisce la configurazione di rete del dispositivo
        /// </summary>
        /// <returns></returns>
        public CKT_DLL.NETINFO GetNetworkConfiguration()
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
            var devnetinfo = new CKT_DLL.NETINFO();
            int ret = CKT_DLL.CKT_GetDeviceNetInfo(m_IDNumber, ref devnetinfo);
            if (ret == 0)
                throw new Exception(Marshal.GetLastWin32Error().ToString());
            // msg = "IP: " & devnetinfo.IP(0) & "." & devnetinfo.IP(1) & "." & devnetinfo.IP(2) & "." & devnetinfo.IP(3) & vbLf
            // msg = msg & "Mask: " & devnetinfo.Mask(0) & "." & devnetinfo.Mask(1) & "." & devnetinfo.Mask(2) & "." & devnetinfo.Mask(3) & vbLf
            // msg = msg & "Gate: " & devnetinfo.Gateway(0) & "." & devnetinfo.Gateway(1) & "." & devnetinfo.Gateway(2) & "." & devnetinfo.Gateway(3) & vbLf
            // msg = msg & "Server: " & devnetinfo.ServerIP(0) & "." & devnetinfo.ServerIP(1) & "." & devnetinfo.ServerIP(2) & "." & devnetinfo.ServerIP(3) & vbLf
            // msg = msg & "MAC: " & devnetinfo.MAC(0) & "." & devnetinfo.MAC(1) & "." & devnetinfo.MAC(2) & "." & devnetinfo.MAC(3) & "." & devnetinfo.MAC(4) & "." & devnetinfo.MAC(5) & vbLf
            // MessageBox.Show(msg)
            // End If
            return devnetinfo;
        }

        /// <summary>
        /// Imposta i parametri di rete del dispositivo
        /// </summary>
        /// <param name="config"></param>
        public void SetNetworkConfiguration(CKT_DLL.NETINFO config)
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
            int ret;
            ret = CKT_DLL.CKT_SetDeviceIPAddr(m_IDNumber, config.IP);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceIPAddr: " + Marshal.GetLastWin32Error().ToString());
            ret = CKT_DLL.CKT_SetDeviceMask(m_IDNumber, config.Mask);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceMask: " + Marshal.GetLastWin32Error().ToString());
            ret = CKT_DLL.CKT_SetDeviceGateway(m_IDNumber, config.Gateway);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceGateway: " + Marshal.GetLastWin32Error().ToString());
            ret = CKT_DLL.CKT_SetDeviceServerIPAddr(m_IDNumber, config.ServerIP);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceServerIPAddr: " + Marshal.GetLastWin32Error().ToString());
            ret = CKT_DLL.CKT_SetDeviceMAC(m_IDNumber, config.MAC);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceMAC: " + Marshal.GetLastWin32Error().ToString());
            config = GetNetworkConfiguration();
            OnNetworkConfigurationChanged(new S300NetConfigEventArgs(this, config));
        }

        protected virtual void OnNetworkConfigurationChanged(S300NetConfigEventArgs e)
        {
            S300Devices.NotifyNetworkConfigurationChanged(e);
            NetworkConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Restituisce la data e l'ora di sistema sul dispositivo remoto
        /// </summary>
        /// <returns></returns>
        public DateTime GetDeviceTime()
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
            var devclock = new CKT_DLL.DATETIMEINFO();
            int ret = CKT_DLL.CKT_GetDeviceClock(m_IDNumber, ref devclock);
            if (ret == 0)
                throw new Exception("Errore in CKT_GetDeviceClock: " + Marshal.GetLastWin32Error());
            return new DateTime(devclock.Year_Renamed, devclock.Month_Renamed, devclock.Day_Renamed, devclock.Hour_Renamed, devclock.Minute_Renamed, devclock.Second_Renamed);
        }

        /// <summary>
        /// Imposta la data e l'ora di sistema sul dispositivo remoto
        /// </summary>
        /// <param name="[date]"></param>
        public void SetDeviceTime(DateTime date)
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
            int ret = CKT_DLL.CKT_SetDeviceDate(m_IDNumber, (short)date.Year, (byte)date.Month, (byte)date.Day);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceDate: " + Marshal.GetLastWin32Error());
            CKT_DLL.Sleep(300);
            ret = CKT_DLL.CKT_SetDeviceTime(m_IDNumber, (byte)date.Hour, (byte)date.Minute, (byte)date.Second);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceDate: " + Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Restituisce la versione del software installato sul dispositivo remoto
        /// </summary>
        /// <returns></returns>
        public Version GetDeviceSoftwareVersion()
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
            var devnfo = new CKT_DLL.DEVICEINFO();
            int ret = CKT_DLL.CKT_GetDeviceInfo(m_IDNumber, ref devnfo);
            if (ret == 0)
                throw new Exception("Errore in CKT_GetDeviceInfo: " + Marshal.GetLastWin32Error());
            return new Version(devnfo.MajorVersion, devnfo.MinorVersion);
        }

        /// <summary>
        /// Restituisce la versione del software installato sul dispositivo remoto
        /// </summary>
        /// <returns></returns>
        public int GetDeviceID()
        {
            EnsureConnected();
            var devnfo = new CKT_DLL.DEVICEINFO();
            int ret = CKT_DLL.CKT_GetDeviceInfo(m_IDNumber, ref devnfo);
            if (ret == 0)
                throw new Exception("Errore in CKT_GetDeviceInfo: " + Marshal.GetLastWin32Error());
            return devnfo.ID;
        }

        internal void EnsureConnected()
        {
            if (!IsConnected())
                throw new InvalidOperationException("Dispositivo non connesso");
        }

        /// <summary>
        /// Restituisce i contatori relative alle strutture dati definite sul dispositivo
        /// </summary>
        /// <returns></returns>
        public S300CountsInfo GetCounts()
        {
            EnsureConnected();
            int pc = default, fc = default, cc = default;
            int ret = CKT_DLL.CKT_GetCounts(m_IDNumber, ref pc, ref fc, ref cc);
            if (ret == 0)
                throw new Exception("Errore in CKT_GetCounts: " + Marshal.GetLastWin32Error());
            var info = new S300CountsInfo();
            info.PersonsCount = pc;
            info.FingerPrintsCount = fc;
            info.ClockingsCounts = cc;
            return info;
        }

        /// <summary>
        /// Restituisce la configurazione del dispositivo
        /// </summary>
        /// <returns></returns>
        public S300Config GetConfiguration()
        {
            EnsureConnected();
            var config = new S300Config();
            var devnfo = new CKT_DLL.DEVICEINFO();
            int ret = CKT_DLL.CKT_GetDeviceInfo(m_IDNumber, ref devnfo);
            if (ret == 0)
                throw new Exception("Errore in CKT_GetDeviceInfo: " + Marshal.GetLastWin32Error());
            config.RingAllow = devnfo.RingAllow != 0;
            config.RealtimeMode = devnfo.RealTimeAllow != 0;
            config.AutoUpdateFingerprint = devnfo.AutoUpdateAllow != 0;
            config.SpeakerVolume = devnfo.SpeakerVolume;
            config.LockDelayTime = devnfo.LockDelayTime;
            config.AdminPassword = ToAnsiString(devnfo.AdminPassword);
            config.FixedWiegandAreaCode = devnfo.FixWGHead;
            config.WiegandOption = (WGOptions)devnfo.WGOption;
            config.MinDelayInOut = devnfo.KQRepeatTime;
            return config;
        }

        private string ToAnsiString(byte[] arr)
        {
            string ret = System.Text.Encoding.ASCII.GetString(arr);
            int p = Strings.InStr(ret, Constants.vbNullChar);
            if (p > 0)
                ret = Strings.Left(ret, p - 1);
            return ret;
        }

        /// <summary>
        /// Imposta la configurazione
        /// </summary>
        /// <param name="value"></param>
        public void SetConfiguration(S300Config value)
        {
            EnsureConnected();
            int ret;
            ret = CKT_DLL.CKT_SetRingAllow(m_IDNumber, DMD.Integers.CInt(Interaction.IIf(value.RingAllow, 1, 0)));
            if (ret == 0)
                throw new Exception("Errore in CKT_SetRingAllow: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetRealtimeMode(m_IDNumber, DMD.Integers.CInt(Interaction.IIf(value.RealtimeMode, 1, 0)));
            if (ret == 0)
                throw new Exception("Errore in CKT_SetRealtimeMode: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetAutoUpdate(m_IDNumber, DMD.Integers.CInt(Interaction.IIf(value.AutoUpdateFingerprint, 1, 0)));
            if (ret == 0)
                throw new Exception("Errore in CKT_SetAutoUpdate: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetSpeakerVolume(m_IDNumber, value.SpeakerVolume);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetSpeakerVolume: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetDoor(m_IDNumber, value.LockDelayTime);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDoor: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetFixWGHead(m_IDNumber, value.FixedWiegandAreaCode);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetFixWGHead: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetWG(m_IDNumber, (int)value.WiegandOption);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetWG: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetRepeatKQ(m_IDNumber, value.MinDelayInOut);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetRepeatKQ: " + Marshal.GetLastWin32Error());
            ret = CKT_DLL.CKT_SetDeviceAdminPassword(m_IDNumber, value.AdminPassword);
            if (ret == 0)
                throw new Exception("Errore in CKT_SetDeviceAdminPassword: " + Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Forza l'apertura del relè
        /// </summary>
        public void ForceOpenLock()
        {
            EnsureConnected();
            int ret = CKT_DLL.CKT_ForceOpenLock(m_IDNumber);
            if (ret == 0)
                throw new Exception("Errore in CKT_ForceOpenLock: " + Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Restituisce l'elenco degli utenti definiti sul dispositivo
        /// </summary>
        /// <returns></returns>
        public S300UsersCollection Users
        {
            get
            {
                EnsureConnected();
                if (m_Users is null)
                    m_Users = new S300UsersCollection(this);
                return m_Users;
            }
        }

        /// <summary>
        /// Scarica le mercature dal dispositivo
        /// </summary>
        /// <returns></returns>
        public S300Clocking[] GetAllClockings()
        {
            EnsureConnected();
            var arr = Array.Empty<S300Clocking>();
            int cnt = 0;
            var pLongRun = IntPtr.Zero;
            // If CKT_GetClockingNewRecordEx(IDNumber, pLongRun) Then 'IF GET NewRecord
            if (CKT_DLL.CKT_GetClockingRecordEx(DeviceID, ref pLongRun) != 0) // If GET Record
            {
                while (true)
                {
                    int RecordCount = 0;
                    int RetCount = 0;
                    var pClockings = IntPtr.Zero;
                    int Ret = CKT_DLL.CKT_GetClockingRecordProgress((int)pLongRun, ref RecordCount, ref RetCount, ref pClockings);
                    if (RecordCount > 0)
                    {
                        Array.Resize(ref arr, RecordCount);
                        // ProgressBar1.Maximum = RecordCount
                    }

                    if (Ret == 0)
                    {
                        Debug.Print("Error cond 1");
                        break;
                    }

                    if (Ret != 0)
                    {
                        var ptemp = pClockings;
                        for (int i = 1, loopTo = RetCount; i <= loopTo; i++) // (i = 1; i <= RetCount; i++)
                        {
                            var clocking = new CKT_DLL.CLOCKINGRECORD();
                            clocking.Time = (byte[])Array.CreateInstance(typeof(byte), 20); // New Byte[20]
                            CKT_DLL.PCopyMemory(ref clocking, (int)pClockings, CKT_DLL.CLOCKINGRECORDSIZE);
                            pClockings = pClockings + CKT_DLL.CLOCKINGRECORDSIZE;
                            arr[cnt] = new S300Clocking(this, clocking);
                            cnt += 1;
                        }

                        if (ptemp != (IntPtr)0)
                            CKT_DLL.CKT_FreeMemory(ptemp);
                    }

                    if (Ret == 1)
                    {
                        Debug.Print("Error cond 2");
                        break;
                    }
                }
            }
            else
            {
                Debug.Print("Error cond 3");
            }

            if (cnt > 0)
            {
                Array.Resize(ref arr, cnt);
            }
            else
            {
                arr = Array.Empty<S300Clocking>();
            }

            return arr;
        }

        /// <summary>
        /// Scarica le mercature dal dispositivo
        /// </summary>
        /// <returns></returns>
        public S300Clocking[] GetNewClockings()
        {
            EnsureConnected();
            var arr = Array.Empty<S300Clocking>();
            int cnt = 0;
            var pLongRun = IntPtr.Zero;
            // If CKT_GetClockingNewRecordEx(IDNumber, pLongRun) Then 'IF GET NewRecord
            if (CKT_DLL.CKT_GetClockingNewRecordEx(DeviceID, ref pLongRun) != 0) // If GET Record
            {
                while (true)
                {
                    int RecordCount = 0;
                    int RetCount = 0;
                    var pClockings = IntPtr.Zero;
                    int Ret = CKT_DLL.CKT_GetClockingRecordProgress((int)pLongRun, ref RecordCount, ref RetCount, ref pClockings);
                    if (RecordCount > 0)
                    {
                        Array.Resize(ref arr, RecordCount);
                        // ProgressBar1.Maximum = RecordCount
                    }

                    if (Ret == 0)
                    {
                        break;
                    }

                    if (Ret != 0)
                    {
                        var ptemp = pClockings;
                        for (int i = 1, loopTo = RetCount; i <= loopTo; i++) // (i = 1; i <= RetCount; i++)
                        {
                            var clocking = new CKT_DLL.CLOCKINGRECORD();
                            clocking.Time = (byte[])Array.CreateInstance(typeof(byte), 20); // New Byte[20]
                            CKT_DLL.PCopyMemory(ref clocking, (int)pClockings, CKT_DLL.CLOCKINGRECORDSIZE);
                            pClockings = pClockings + CKT_DLL.CLOCKINGRECORDSIZE;
                            arr[cnt] = new S300Clocking(this, clocking);
                            cnt += 1;
                        }

                        if (ptemp != (IntPtr)0)
                            CKT_DLL.CKT_FreeMemory(ptemp);
                    }

                    if (Ret == 1)
                    {
                        break;
                    }
                }
            }

            return arr;
        }

        /// <summary>
        /// Elimina tutte le marcature
        /// </summary>
        public void DeleteAllClockings()
        {
            EnsureConnected();
            int ret = CKT_DLL.CKT_ClearClockingRecord(DeviceID, (int)ClearClockingRecordTypes.All, 0);
            if (ret == 0)
                throw new S300Exception();
            // MessageBox.Show("CKT_ClearClockingRecord OK!")
            // Else
            // MessageBox.Show("Í¨Ñ¶Ê§°Ü")
            // End If
        }

        /// <summary>
        /// Elimina le prime N marcature
        /// </summary>
        /// <param name="n"></param>
        public void DeleteFirstNClockings(int n)
        {
            EnsureConnected();
            int ret = CKT_DLL.CKT_ClearClockingRecord(DeviceID, (int)ClearClockingRecordTypes.Count, n);
            if (ret == 0)
                throw new S300Exception();
            // MessageBox.Show("CKT_ClearClockingRecord OK!")
            // Else
            // MessageBox.Show("Í¨Ñ¶Ê§°Ü")
            // End If
        }

        /// <summary>
        /// Resetta il dispositivo riportandolo alle impostazioni di fabbrica
        /// </summary>
        public void FactoryReset()
        {
            EnsureConnected();
            int ret = CKT_DLL.CKT_ResetDevice(DeviceID);
            if (ret != 0)
                throw new S300Exception();
            // MessageBox.Show("CKT_ResetDevice OK!")
            // End If
        }

        /// <summary>
        /// Restituisce un array di strutture contenenti gli orari per la sirena
        /// </summary>
        /// <returns></returns>
        public CKT_DLL.RINGTIME[] GetRingTimes()
        {
            EnsureConnected();
            CKT_DLL.RINGTIME[] tarr = (CKT_DLL.RINGTIME[])Array.CreateInstance(typeof(CKT_DLL.RINGTIME), 50);
            int ret = CKT_DLL.CKT_GetHitRingInfo(DeviceID, tarr);
            if (ret == 0)
                throw new S300Exception();
            var arr = new ArrayList();
            for (int i = 0, loopTo = tarr.Length - 1; i <= loopTo; i++)
            {
                if (tarr[i].week != 0)
                    arr.Add(tarr[i]);
            }

            return (CKT_DLL.RINGTIME[])arr.ToArray(typeof(CKT_DLL.RINGTIME));
        }

        /// <summary>
        /// Imposta gli orari
        /// </summary>
        /// <param name="arr"></param>
        public void SetRingTimes(CKT_DLL.RINGTIME[] arr)
        {
            EnsureConnected();
            for (int i = 0, loopTo = arr.Length - 1; i <= loopTo; i++)
            {
                int ret = CKT_DLL.CKT_SetHitRingInfo(DeviceID, i, ref arr[i]);
                if (ret == 0)
                    throw new S300Exception();
            }
        }
    }
}