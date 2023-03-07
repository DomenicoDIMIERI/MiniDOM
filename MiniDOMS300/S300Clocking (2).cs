using System;
using DMD.XML;
using DMD;

namespace minidom.S300
{
    public enum S300ClockingType : int
    {
        In = 0,
        Out = 1
    }


    /// <summary>
    /// Oggetto che rappresenta una marcatura temporale di un utente
    /// </summary>
    public class S300Clocking : IComparable
    {
        private S300Device m_Device;
        private int m_PersonID;
        private S300PersonInfo m_Person;
        private DateTime m_Time;
        private S300ClockingType m_Type;
        private int m_DeviceID;

        // Dim item1 As New ListViewItem("item1", 0)
        // Dim item1 As New ListViewItem(ListView1.Items.Count)
        // item1.SubItems.Add(clocking.PersonID.ToString())
        // item1.SubItems.Add(Encoding.Default.GetString(clocking.Time))
        // item1.SubItems.Add(clocking.Stat.ToString())
        // item1.SubItems.Add(clocking.ID.ToString())

        public S300Clocking()
        {
            m_Device = null;
            m_PersonID = 0;
            m_Person = null;
            m_Time = default;
            m_Type = S300ClockingType.In;
            m_DeviceID = 0;
        }

        internal S300Clocking(S300Device device, CKT_DLL.CLOCKINGRECORD info) : this()
        {
            if (device is null)
                throw new ArgumentNullException("device");
            m_Device = device;
            m_DeviceID = info.ID;
            m_PersonID = info.PersonID;
            m_Type = ((info.Stat & 1) == 1)? S300ClockingType.Out : S300ClockingType.In;
            m_Time = ParseDate(System.Text.Encoding.ASCII.GetString(info.Time));
        }

        private DateTime ParseDate(string value)
        {
            var dt = Strings.Split(Strings.Trim(value), " ");
            if (dt is null || dt.Length != 2)
                throw new FormatException("Date format exception: " + value);
            var ymd = Strings.Split(dt[0], "-");
            var hms = Strings.Split(dt[1], ":");
            return new DateTime(DMD.Integers.CInt(ymd[0]), DMD.Integers.CInt(ymd[1]), DMD.Integers.CInt(ymd[2]), DMD.Integers.CInt(hms[0]), DMD.Integers.CInt(hms[1]), DMD.Integers.CInt(hms[2]));
        }

        /// <summary>
        /// Restituisce l'ID del dispositivo su cui è registrata la marcatura
        /// </summary>
        /// <returns></returns>
        public int DeviceID
        {
            get
            {
                return m_DeviceID;
            }
        }

        /// <summary>
        /// Restituisce il dispositivo su cui è registrata la marcatura
        /// </summary>
        /// <returns></returns>
        public S300Device Device
        {
            get
            {
                return m_Device;
            }
        }

        /// <summary>
        /// Restituisce l'ID della persona che ha effettuato la marcatura
        /// </summary>
        /// <returns></returns>
        public int PersonID
        {
            get
            {
                return m_PersonID;
            }
        }

        /// <summary>
        /// Restituisce la persona che ha effettuato la marcatura
        /// </summary>
        /// <returns></returns>
        public S300PersonInfo Person
        {
            get
            {
                if (m_Person is null)
                    m_Person = Device.Users.GetItemById(m_PersonID);
                return m_Person;
            }
        }

        /// <summary>
        /// Restituisce il tipo di marcatura Ingresso/Uscita
        /// </summary>
        /// <returns></returns>
        public S300ClockingType Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// Restituisce o imposta la data e l'ora della marcatura
        /// </summary>
        /// <returns></returns>
        public DateTime Time
        {
            get
            {
                return m_Time;
            }
        }

        public string TypeEx
        {
            get
            {
                switch (m_Type)
                {
                    case S300ClockingType.In:
                        {
                            return "In";
                        }

                    case S300ClockingType.Out:
                        {
                            return "Out";
                        }

                    default:
                        {
                            return "?";
                        }
                }
            }
        }

        public int CompareTo(S300Clocking obj)
        {
            int ret = Time.CompareTo(obj.Time);
            if (ret == 0)
            {
                int o1 = (Type == S300ClockingType.In)? 0 : 1;
                int o2 = (obj.Type == S300ClockingType.In)? 0 : 1;
                ret = o1.CompareTo(o2);
            }

            if (ret == 0)
                ret = PersonID.CompareTo(obj.PersonID);
            if (ret == 0)
                ret = DeviceID.CompareTo(obj.DeviceID);
            return ret;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((S300Clocking)obj);
        }
    }
}