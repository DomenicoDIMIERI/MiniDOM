using System;
using System.Collections;
using System.Runtime.InteropServices;
using minidom.S300;

namespace minidom.Internals
{
    public class S300FingerPrintsCollection : ReadOnlyCollectionBase
    {
        private S300PersonInfo m_User;

        public S300FingerPrintsCollection()
        {
            m_User = null;
        }

        internal S300FingerPrintsCollection(S300PersonInfo user) : this()
        {
            Load(user);
        }

        public S300FingerPrint this[int fpid]
        {
            get
            {
                return (S300FingerPrint)InnerList[fpid];
            }

            set
            {
                if (fpid < 0)
                    throw new ArgumentOutOfRangeException("L'indice dell'impronta non può essere < 0");
                if (value is null)
                    throw new ArgumentNullException("Impossibile assegnare il valore NULL");
                if (m_User is null)
                    throw new ArgumentNullException("Utente non impostato");
                int devID = m_User.Device.DeviceID;
                int ret = CKT_DLL.CKT_PutFPTemplate(devID, m_User.PersonID, fpid, value.m_Data, value.m_Data.Length);
                if (ret == 1)
                {
                    value.m_FPID = fpid;
                    value.m_User = m_User;
                    if (fpid < Count)
                    {
                        InnerList[fpid] = value;
                    }
                    else
                    {
                        InnerList.Add(value);
                    }
                }
                else
                {
                    throw new S300Exception((S300ExceptionCodes)ret);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        internal void Load(S300PersonInfo user)
        {
            InnerList.Clear();
            if (user is null)
                throw new ArgumentNullException("user");
            m_User = user;
            int devID = user.Device.DeviceID;
            for (int fpid = 0; fpid <= 10; fpid++)
            {
                int fpDataLen = 0;
                var fpDataPtr = IntPtr.Zero;
                int ret = CKT_DLL.CKT_GetFPTemplate(devID, user.PersonID, fpid, ref fpDataPtr, ref fpDataLen);
                if (ret == 1 && fpDataLen > 0)
                {
                    var fp = new S300FingerPrint();
                    fp.m_User = user;
                    fp.m_FPID = fpid;
                    fp.m_Data = (byte[])Array.CreateInstance(typeof(byte), fpDataLen);
                    Marshal.Copy(fpDataPtr, fp.m_Data, 0, fpDataLen);
                    // ret = CKT_DLL.CKT_GetFPTemplate(devID, user.PersonID, fpid, fp.m_Data, fpDataLen)
                    ret = CKT_DLL.CKT_FreeMemory(fpDataPtr);
                    InnerList.Add(fp);
                }
            }
        }

        internal void NotifyDelete(S300FingerPrint fp)
        {
            InnerList.Remove(fp);
        }

        internal void NotifyAdd(S300FingerPrint fp)
        {
            InnerList.Add(fp);
        }
    }
}