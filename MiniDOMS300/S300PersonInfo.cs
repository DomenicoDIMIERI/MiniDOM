using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using minidom.Internals;

namespace minidom.S300
{
    public enum S300UserType : int
    {
        NormalUser = 0,
        Administrator = 1
    }

    /// <summary>
    /// Classe che rappresenta una persona censita sul dispositivo
    /// </summary>
    public class S300PersonInfo
    {
        private S300Device m_Device;
        private int m_PersonID;
        private string m_Password;
        private int m_CardNo;
        private string m_Name;
        private int m_Dept; // ²¿ÃÅ
        private int m_Group; // ²¿ÃÅ
        private int m_KQOption; // ¿¼ÇÚÄ£Ê½
        private int m_FPMark;
        private int m_Other; // ÌØÊâÐÅÏ¢ =0 ÆÕÍ¨ÈËÔ±, =1 ¹ÜÀíÔ±
        private S300FingerPrintsCollection m_FingerPrints;

        public S300PersonInfo()
        {
            m_Device = null;
            m_PersonID = 0;
            m_Password = "";
            m_CardNo = 0;
            m_Name = "";
            m_Dept = 0;
            m_Group = 0;
            m_KQOption = 0;
            m_FPMark = 0;
            m_Other = 0;
            m_FingerPrints = null;
        }

        internal S300PersonInfo(CKT_DLL.PERSONINFO info) : this()
        {
            m_PersonID = info.PersonID;
            m_Password = System.Text.Encoding.ASCII.GetString(info.Password);
            m_CardNo = info.CardNo;
            m_Name = System.Text.Encoding.ASCII.GetString(info.Name);
            m_Dept = info.Dept;
            m_Group = info.Group;
            m_KQOption = info.KQOption;
            m_FPMark = info.FPMark;
            m_Other = info.Other;
        }

        internal S300PersonInfo(S300Device device, int uid, string userName) : this()
        {
            if (device is null)
                throw new ArgumentNullException("device");
            m_Device = device;
            m_PersonID = uid;
            m_Name = userName;
        }

        public int PersonID
        {
            get
            {
                return m_PersonID;
            }

            set
            {
                if (m_PersonID != 0)
                    throw new InvalidOperationException("Impossibile modificare l'ID della persona dopo averla salvata");
                m_PersonID = value;
            }
        }

        public string Password
        {
            get
            {
                return m_Password;
            }

            set
            {
                m_Password = value;
            }
        }

        public int CardNo
        {
            get
            {
                return m_CardNo;
            }

            set
            {
                m_CardNo = value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public int Department // ²¿ÃÅ
        {
            get
            {
                return m_Dept;
            }

            set
            {
                m_Dept = value;
            }
        }

        public int Group // ²¿ÃÅ
        {
            get
            {
                return m_Group;
            }

            set
            {
                m_Group = value;
            }
        }

        public int KQOption // modalità di frequenza
        {
            get
            {
                return m_KQOption;
            }

            set
            {
                m_KQOption = value;
            }
        }

        public int FPMark
        {
            get
            {
                return m_FPMark;
            }

            set
            {
                m_FPMark = value;
            }
        }

        public S300UserType UserType // ÌØÊâÐÅÏ¢ =0 ÆÕÍ¨ÈËÔ±, =1 ¹ÜÀíÔ±
        {
            get
            {
                return (S300UserType)m_Other;
            }

            set
            {
                m_Other = (int)value;
            }
        }

        public S300Device Device
        {
            get
            {
                return m_Device;
            }
        }

        protected internal void SetDevice(S300Device device)
        {
            m_Device = device;
        }

        /// <summary>
        /// Salva le modifiche fatte sulla periferica
        /// </summary>
        public void Save()
        {
            if (Device is null)
                throw new ArgumentNullException("Device è NULL");
            if (!Device.IsConnected())
                throw new ArgumentNullException("Dispositivo non connesso");
            var person = new CKT_DLL.PERSONINFO();
            person.CardNo = CardNo;
            person.Name = System.Text.Encoding.ASCII.GetBytes(Name);
            Array.Resize(ref person.Name, 12);
            person.Password = System.Text.Encoding.ASCII.GetBytes(Password);
            Array.Resize(ref person.Password, 8);
            person.PersonID = m_PersonID;
            person.Dept = m_Dept;
            person.Group = m_Group;
            person.KQOption = m_KQOption;
            person.FPMark = m_FPMark;
            person.Other = m_Other;
            int mpiRet = CKT_DLL.CKT_ModifyPersonInfo(Device.DeviceID, ref person);
            if (mpiRet == CKT_DLL.CKT_RESULT_ADDOK)
            {
                return; // ("Edit OK!")
            }
            else if (mpiRet == CKT_DLL.CKT_ERROR_MEMORYFULL)
            {
                throw new OutOfMemoryException("Errore in CKT_ModifyPersonInfo: Memoria piena"); // MessageBox.Show("MEMORY FUL")
            }
            else
            {
                // MessageBox.Show("Edit ERROR!")
                throw new OutOfMemoryException("Errore in CKT_ModifyPersonInfo: " + Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Elimina l'utente sul dispositivo
        /// </summary>
        public void Delete()
        {
            // SyncLock Me.Device
            if (Device is null)
                throw new ArgumentNullException("Device è NULL");
            if (!Device.IsConnected())
                throw new ArgumentNullException("Dispositivo non connesso");
            int dpiRet = 0;
            dpiRet = CKT_DLL.CKT_DeletePersonInfo(Device.DeviceID, PersonID, PersonID); // &H4
            if (dpiRet == CKT_DLL.CKT_RESULT_OK)
            {
                Device.Users.NotifyDelete(this);
                return; // ("Delete OK!")
            }
            else if (dpiRet == CKT_DLL.CKT_ERROR_NOTHISPERSON)
            {
                throw new KeyNotFoundException("Nessun utente con questo ID sul dispositivo");
            }
            else
            {
                throw new Exception("Errore in CKT_DeletePersonInfo: " + Marshal.GetLastWin32Error());
            }
            // End SyncLock
        }

        /// <summary>
        /// Restituisce la collezione delle impronte digitali registrate per questo utente
        /// </summary>
        /// <returns></returns>
        public S300FingerPrintsCollection FingerPrints
        {
            get
            {
                lock (this)
                {
                    if (m_FingerPrints is null)
                        m_FingerPrints = new S300FingerPrintsCollection(this);
                    return m_FingerPrints;
                }
            }
        }
    }
}