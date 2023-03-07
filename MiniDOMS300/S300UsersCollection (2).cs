using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using minidom.S300;

namespace minidom.Internals
{
    public class S300UsersCollection : ReadOnlyCollectionBase
    {
        private S300Device m_Device;

        public S300UsersCollection()
        {
            m_Device = null;
        }

        internal S300UsersCollection(S300Device device) : this()
        {
            Load(device);
        }

        public void EraseAll()
        {
            m_Device.EnsureConnected();
            int ret = CKT_DLL.CKT_DeleteAllPersonInfo(m_Device.DeviceID);
            if (ret == 0)
                throw new S300Exception();
            InnerList.Clear();
        }

        public S300PersonInfo this[int index]
        {
            get
            {
                return (S300PersonInfo)InnerList[index];
            }
        }

        internal void Load(S300Device device)
        {
            InnerList.Clear();
            if (device is null)
                throw new ArgumentNullException("device");
            m_Device = device;
            int pLongRun = 0;
            int devID = m_Device.DeviceID;
            int ret = CKT_DLL.CKT_ListPersonInfoEx(devID, ref pLongRun);
            if (ret == 0)
                throw new Exception("Errore in CKT_ListPersonInfoEx: " + Marshal.GetLastWin32Error());
            Debug.Print(Marshal.GetLastWin32Error().ToString());
            while (true)
            {
                int RecordCount = 0;
                int RetCount = 0;
                var pPersons = IntPtr.Zero;
                ret = CKT_DLL.CKT_ListPersonProgress(pLongRun, ref RecordCount, ref RetCount, ref pPersons);
                Debug.Print(Marshal.GetLastWin32Error().ToString());
                if (ret == 0 && RecordCount > 0)
                {
                    CKT_DLL.PERSONINFO[] arr;
                    arr = new CKT_DLL.PERSONINFO[RecordCount];
                    int argppPersons = (int)pPersons;
                    ret = CKT_DLL.CKT_ListPersonInfo(m_Device.DeviceID, ref RecordCount, ref argppPersons);
                }

                // If (RecordCount > 0) Then
                // ProgressBar2.Maximum = RecordCount
                // End If
                var ptemp = pPersons;
                for (int i = 0, loopTo = RetCount - 1; i <= loopTo; i++) // (i = 0; i < RetCount; i++)
                {
                    var person = new CKT_DLL.PERSONINFO();
                    CKT_DLL.PCopyMemory(ref person, (int)pPersons, Marshal.SizeOf(person));
                    var info = new S300PersonInfo(person);
                    info.SetDevice(m_Device);
                    InnerList.Add(info);
                    pPersons = pPersons + Marshal.SizeOf(person);
                }

                if (ptemp != (IntPtr)0)
                    CKT_DLL.CKT_FreeMemory(ptemp);
                if (ret == 1)
                    return;
            }
        }

        internal void NotifyDelete(S300PersonInfo user)
        {
            InnerList.Remove(user);
        }

        internal void NotifyAdd(S300PersonInfo user)
        {
            InnerList.Add(user);
        }

        public S300PersonInfo GetItemById(int id)
        {
            foreach (S300PersonInfo item in this)
            {
                if (item.PersonID == id)
                    return item;
            }

            return null;
        }

        public S300PersonInfo Create()
        {
            int uid = GetMaxID() + 1;
            var user = new S300PersonInfo(m_Device, 0, "User " + uid);
            InnerList.Add(user);
            return user;
        }

        public int GetMaxID()
        {
            int max = -1;
            foreach (S300PersonInfo user in this)
                max = Math.Max(max, user.PersonID);
            return max;
        }
    }
}