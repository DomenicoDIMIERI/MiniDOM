using System;
using DMD.XML;
using DMD;

namespace minidom.S300
{
    /// <summary>
    /// Flag per gli oggetti <see cref="S300Clocking"/>
    /// </summary>
    public enum S300ClockingType : int
    {
        /// <summary>
        /// Ingresso
        /// </summary>
        In = 0,

        /// <summary>
        /// Uscita
        /// </summary>
        Out = 1
    }


    /// <summary>
    /// Oggetto che rappresenta una marcatura temporale di un utente
    /// </summary>
    [Serializable]
    public class S300Clocking 
        : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<S300Clocking>
    {
        [NonSerialized] private S300Device m_Device;
        private int m_PersonID;
        [NonSerialized] private S300PersonInfo m_Person;
        private DateTime m_Time;
        private S300ClockingType m_Type;
        private int m_DeviceID;

        // Dim item1 As New ListViewItem("item1", 0)
        // Dim item1 As New ListViewItem(ListView1.Items.Count)
        // item1.SubItems.Add(clocking.PersonID.ToString())
        // item1.SubItems.Add(Encoding.Default.GetString(clocking.Time))
        // item1.SubItems.Add(clocking.Stat.ToString())
        // item1.SubItems.Add(clocking.ID.ToString())

        /// <summary>
        /// Costruttore
        /// </summary>
        public S300Clocking()
        {
            m_Device = null;
            m_PersonID = 0;
            m_Person = null;
            m_Time = default;
            m_Type = S300ClockingType.In;
            m_DeviceID = 0;
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="device"></param>
        /// <param name="info"></param>
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
            return new DateTime(DMD.Integers.ValueOf(ymd[0]), DMD.Integers.ValueOf(ymd[1]), DMD.Integers.ValueOf(ymd[2]), DMD.Integers.ValueOf(hms[0]), DMD.Integers.ValueOf(hms[1]), DMD.Integers.ValueOf(hms[2]));
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

        /// <summary>
        /// Restituisce In per gli ingressi ed out per le uscite
        /// </summary>
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

        /// <summary>
        /// Compara due oggetti
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DMD.Strings.ConcatArray(this.TypeEx, " ", this.m_Time, " ", this.m_PersonID , " on " , this.m_DeviceID);
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.m_Time);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(DMDBaseXMLObject obj)
        {
            return (obj is S300Clocking) && this.Equals((S300Clocking)obj);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(S300Clocking obj)
        {
            return base.Equals(obj)
                && DMD.Integers.EQ(this.m_PersonID, obj.m_PersonID)
                && DMD.DateUtils.EQ(this.m_Time, obj.m_Time)
                && DMD.RunTime.EQ(this.m_Type, obj.m_Type)
                && DMD.Integers.EQ(this.m_DeviceID, obj.m_DeviceID)
                ;
        }


    }
}