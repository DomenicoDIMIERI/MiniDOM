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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Flag validi per un log
        /// </summary>
        [Flags]
        public enum DeviceLogFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Dispositivo acceso
            /// </summary>
            On = 1,

            /// <summary>
            /// Dispositivo in stato sospeso
            /// </summary>
            Suspended = 2
        }

        /// <summary>
        /// Rappresenta lo stato di un dispositivo in un determinato intervallo di tempo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class DeviceLog
            : minidom.Databases.DBObjectPO
        {
            private int m_IDDevice;
            [NonSerialized] private Dispositivo m_Device;
            private DateTime? m_StartDate;
            private DateTime? m_EndDate;
            private int m_IDUtente;
            [NonSerialized] private CUser m_Utente;
            private string m_NomeUtente;
            private int? m_CPUUsage;
            private int? m_CPUMaximum;
            private double? m_RAMTotal;
            private double? m_RAMAvailable;
            private double? m_RAMMinimum;
            private double? m_DiskTotal;
            private double? m_DiskAvailable;
            private double? m_DiskMinimum;
            private float? m_Temperature;
            private float? m_TemperatureMaximum;
            private int? m_Counter1;
            private int? m_Counter2;
            private int? m_Counter3;
            private int? m_Counter4;
            private int m_NumeroCampioni;
            private GPSRecord m_GPS;
            private CSize m_ScreenSize;
            private int? m_ScreenColors;
            private string m_MACAddress;
            private string m_IPAddress;
            private string m_OSVersion;
            private string m_DettaglioStato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DeviceLog()
            {
                m_IDDevice = 0;
                m_Device = null;
                m_Flags = (int)DeviceLogFlags.None;
                m_StartDate = default;
                m_EndDate = default;
                m_IDUtente = 0;
                m_Utente = null;
                m_NomeUtente = "";
                m_CPUUsage = default;
                m_CPUMaximum = default;
                m_RAMTotal = default;
                m_RAMAvailable = default;
                m_RAMMinimum = default;
                m_DiskTotal = default;
                m_DiskAvailable = default;
                m_DiskMinimum = default;
                m_Temperature = default;
                m_TemperatureMaximum = default;
                m_Counter1 = default;
                m_Counter2 = default;
                m_Counter3 = default;
                m_Counter4 = default;
                m_NumeroCampioni = 0;
                m_GPS = new GPSRecord();
                m_ScreenSize = new CSize();
                m_ScreenColors = default;
                m_MACAddress = "";
                m_IPAddress = "";
                m_OSVersion = "";
                m_DettaglioStato = "";
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo MAC della macchina
            /// </summary>
            /// <returns></returns>
            public string MACAddress
            {
                get
                {
                    return m_MACAddress;
                }

                set
                {
                    string oldValue = m_MACAddress;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MACAddress = value;
                    DoChanged("MACAddress", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo IP della macchina
            /// </summary>
            /// <returns></returns>
            public string IPAddress
            {
                get
                {
                    return m_IPAddress;
                }

                set
                {
                    string oldValue = m_IPAddress;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IPAddress = value;
                    DoChanged("IPAddress", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il software usato sul dispositivo
            /// </summary>
            /// <returns></returns>
            public string OSVersion
            {
                get
                {
                    return m_OSVersion;
                }

                set
                {
                    string oldValue = m_OSVersion;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_OSVersion = value;
                    DoChanged("OSVersion", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del dispositivo a cui fa riferimento il log
            /// </summary>
            /// <returns></returns>
            public int IDDevice
            {
                get
                {
                    return DBUtils.GetID(m_Device, m_IDDevice);
                }

                set
                {
                    int oldValue = IDDevice;
                    if (oldValue == value)
                        return;
                    m_Device = null;
                    m_IDDevice = value;
                    DoChanged("IDDevice", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il dettaglio dello stato
            /// </summary>
            /// <returns></returns>
            public string DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }

                set
                {
                    string oldValue = m_DettaglioStato;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DettaglioStato = value;
                    DoChanged("DettaglioStato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il dispositivo
            /// </summary>
            /// <returns></returns>
            public Dispositivo Device
            {
                get
                {
                    if (m_Device is null)
                        m_Device = minidom.Office.Dispositivi.GetItemById(m_IDDevice);
                    return m_Device;
                }

                set
                {
                    var oldValue = Device;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Device = value;
                    m_IDDevice = DBUtils.GetID(value, 0);
                    DoChanged("Device", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il device
            /// </summary>
            /// <param name="dev"></param>
            protected internal void SetDevice(Dispositivo dev)
            {
                m_Device = dev;
                m_IDDevice = DBUtils.GetID(dev, 0);
            }



            /// <summary>
            /// Restituisce o imposta dei flags che descrivo la categoria
            /// </summary>
            /// <returns></returns>
            public new DeviceLogFlags Flags
            {
                get
                {
                    return (DeviceLogFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio
            /// </summary>
            /// <returns></returns>
            public DateTime? StartDate
            {
                get
                {
                    return m_StartDate;
                }

                set
                {
                    var oldValue = m_StartDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_StartDate = value;
                    DoChanged("StartDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine
            /// </summary>
            /// <returns></returns>
            public DateTime? EndDate
            {
                get
                {
                    return m_EndDate;
                }

                set
                {
                    var oldValue = m_EndDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_EndDate = value;
                    DoChanged("EndDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che sta utilizzando il dispositivo
            /// </summary>
            /// <returns></returns>
            public int IDUtente
            {
                get
                {
                    return DBUtils.GetID(m_Utente, m_IDUtente);
                }

                set
                {
                    int oldValue = IDUtente;
                    if (oldValue == value)
                        return;
                    m_IDUtente = value;
                    m_Utente = null;
                    DoChanged("IDUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che sta utilizzando il dispositivo
            /// </summary>
            /// <returns></returns>
            public Sistema.CUser Utente
            {
                get
                {
                    if (m_Utente is null)
                        m_Utente = Sistema.Users.GetItemById(m_IDUtente);
                    return m_Utente;
                }

                set
                {
                    var oldValue = Utente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDUtente = DBUtils.GetID(value, 0);
                    m_Utente = value;
                    m_NomeUtente = "";
                    if (value is object)
                        m_NomeUtente = value.Nominativo;
                    DoChanged("Utente", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="user"></param>
            protected internal void SetUtente(Sistema.CUser user)
            {
                m_Utente = user;
                m_IDUtente = DBUtils.GetID(user, 0);
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che sta usando il dispositivo
            /// </summary>
            /// <returns></returns>
            public string NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }

                set
                {
                    string oldValue = m_NomeUtente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeUtente = value;
                    DoChanged("NomeUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la percentuale di utlizzo medio della cpu (se presente) del dispositivo
            /// </summary>
            /// <returns></returns>
            public int? CPUUsage
            {
                get
                {
                    return m_CPUUsage;
                }

                set
                {
                    var oldValue = m_CPUUsage;
                    if (oldValue == value == true)
                        return;
                    m_CPUUsage = value;
                    DoChanged("CPUUsage", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la percentuale di utilizzo di picco della cpu (se presente) del dispositivo
            /// </summary>
            /// <returns></returns>
            public int? CPUMaximum
            {
                get
                {
                    return m_CPUMaximum;
                }

                set
                {
                    int oldValue = (int)m_CPUMaximum;
                    if (oldValue == value == true)
                        return;
                    m_CPUMaximum = value;
                    DoChanged("CPUMaximum", value, oldValue);
                }
            }

            /// <summary>
            /// Aggiorna le statistiche per la cpu. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
            /// </summary>
            /// <param name="cpuUsage"></param>
            public void NotifyCPU(int? cpuUsage)
            {
                if (m_NumeroCampioni == 0)
                {
                    m_CPUUsage = cpuUsage;
                    m_CPUMaximum = cpuUsage;
                }
                else if (cpuUsage.HasValue)
                {
                    m_CPUUsage = (int?)Maths.Div(Maths.Sum(Maths.Mul(m_CPUUsage, m_NumeroCampioni), cpuUsage), m_NumeroCampioni + 1);
                    m_CPUMaximum = Maths.Max(m_CPUMaximum, cpuUsage);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità totale di memoria RAM installata
            /// </summary>
            /// <returns></returns>
            public double? RAMTotal
            {
                get
                {
                    return m_RAMTotal;
                }

                set
                {
                    var oldValue = m_RAMTotal;
                    if (oldValue == value == true)
                        return;
                    m_RAMTotal = value;
                    DoChanged("RAMTotal", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità media di memoria disponibile
            /// </summary>
            /// <returns></returns>
            public double? RAMAvailable
            {
                get
                {
                    return m_RAMAvailable;
                }

                set
                {
                    var oldValue = m_RAMAvailable;
                    if (oldValue == value == true)
                        return;
                    m_RAMAvailable = value;
                    DoChanged("RAMAvailable", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità minima di memoria disponibile
            /// </summary>
            /// <returns></returns>
            public double? RAMMinimum
            {
                get
                {
                    return m_RAMMinimum;
                }

                set
                {
                    var oldValue = m_RAMMinimum;
                    if (oldValue == value == true)
                        return;
                    m_RAMMinimum = value;
                    DoChanged("RAMMinimum", value, oldValue);
                }
            }

            /// <summary>
            /// Aggiorna le statistiche per la RAM disponibile. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
            /// </summary>
            /// <param name="ram"></param>
            public void NotifyRAMAvailable(double? ram)
            {
                if (m_NumeroCampioni == 0)
                {
                    m_RAMAvailable = ram;
                    m_RAMMinimum = ram;
                }
                else if (ram.HasValue)
                {
                    m_RAMAvailable = Maths.Div(Maths.Sum(Maths.Mul(m_RAMAvailable, m_NumeroCampioni), ram), m_NumeroCampioni + 1);
                    m_RAMMinimum = Maths.Min(m_RAMMinimum, ram);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità di memoria totale su disco
            /// </summary>
            /// <returns></returns>
            public double? DiskTotal
            {
                get
                {
                    return m_DiskTotal;
                }

                set
                {
                    var oldValue = m_DiskTotal;
                    if (oldValue == value == true)
                        return;
                    m_DiskTotal = value;
                    DoChanged("DiskTotal", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità media di memoria disponibile su disco
            /// </summary>
            /// <returns></returns>
            public double? DiskAvailable
            {
                get
                {
                    return m_DiskAvailable;
                }

                set
                {
                    var oldValue = m_DiskAvailable;
                    if (oldValue == value == true)
                        return;
                    m_DiskAvailable = value;
                    DoChanged("DiskAvailable", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta la quantità minima di memoria disponibile su disco (misura di picco)
            /// </summary>
            /// <returns></returns>
            public double? DiskMinimum
            {
                get
                {
                    return m_DiskMinimum;
                }

                set
                {
                    var oldValue = m_DiskMinimum;
                    if (oldValue == value == true)
                        return;
                    m_DiskMinimum = value;
                    DoChanged("DiskMinimum", value, oldValue);
                }
            }

            /// <summary>
            /// Aggiorna le statistiche per la quantità di memoria disponibile su disco. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
            /// </summary>
            /// <param name="value"></param>
            public void NotifyDiskAvailable(double? value)
            {
                if (m_NumeroCampioni == 0)
                {
                    m_DiskAvailable = value;
                    m_DiskMinimum = value;
                }
                else if (value.HasValue)
                {
                    m_DiskAvailable = Maths.Div(Maths.Sum(Maths.Mul(m_DiskAvailable, m_NumeroCampioni), value), m_NumeroCampioni + 1);
                    m_DiskMinimum = Maths.Min(m_DiskMinimum, value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la temperatura
            /// </summary>
            /// <returns></returns>
            public float? Temperature
            {
                get
                {
                    return m_Temperature;
                }

                set
                {
                    var oldValue = m_Temperature;
                    if (oldValue == value == true)
                        return;
                    m_Temperature = value;
                    DoChanged("Temperature", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la temperatura massima misurata
            /// </summary>
            public float? TemperatureMaximum
            {
                get
                {
                    return m_TemperatureMaximum;
                }

                set
                {
                    var oldValue = m_TemperatureMaximum;
                    if (oldValue == value == true)
                        return;
                    m_TemperatureMaximum = value;
                    DoChanged("TemperatureMaximum", value, oldValue);
                }
            }

            /// <summary>
            /// Aggiorna le statistiche per la temperatura. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
            /// </summary>
            /// <param name="value"></param>
            public void NotifyTemperature(float? value)
            {
                if (m_NumeroCampioni == 0)
                {
                    m_Temperature = value;
                    m_TemperatureMaximum = value;
                }
                else if (value.HasValue)
                {
                    m_Temperature = (float?)Maths.Div(Maths.Sum(Maths.Mul(m_Temperature, m_NumeroCampioni), value), m_NumeroCampioni + 1);
                    m_TemperatureMaximum = Maths.Max(m_TemperatureMaximum, value);
                }
            }

            /// <summary>
            /// Contatore 1
            /// </summary>
            public int Counter1
            {
                get
                {
                    return (int)m_Counter1;
                }

                set
                {
                    int oldValue = (int)m_Counter1;
                    if (oldValue == value)
                        return;
                    m_Counter1 = value;
                    DoChanged("Coutner1", value, oldValue);
                }
            }

            /// <summary>
            /// Contatore 2
            /// </summary>
            public int Counter2
            {
                get
                {
                    return (int)m_Counter2;
                }

                set
                {
                    int oldValue = (int)m_Counter2;
                    if (oldValue == value)
                        return;
                    m_Counter2 = value;
                    DoChanged("Coutner2", value, oldValue);
                }
            }


            /// <summary>
            /// Contatore 3
            /// </summary>
            public int Counter3
            {
                get
                {
                    return (int)m_Counter3;
                }

                set
                {
                    int oldValue = (int)m_Counter3;
                    if (oldValue == value)
                        return;
                    m_Counter3 = value;
                    DoChanged("Coutner3", value, oldValue);
                }
            }

            /// <summary>
            /// Contatore 4
            /// </summary>
            public int Counter4
            {
                get
                {
                    return (int)m_Counter4;
                }

                set
                {
                    int oldValue = (int)m_Counter4;
                    if (oldValue == value)
                        return;
                    m_Counter4 = value;
                    DoChanged("Coutner4", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la posizione GPS
            /// </summary>
            /// <returns></returns>
            public GPSRecord GPS
            {
                get
                {
                    return m_GPS;
                }
            }

            /// <summary>
            /// Dimensioni dello schermo in pixels
            /// </summary>
            public CSize ScreenSize
            {
                get
                {
                    return m_ScreenSize;
                }
            }

            /// <summary>
            /// Profondità di colore in bits
            /// </summary>
            public int? ScreenColors
            {
                get
                {
                    return m_ScreenColors;
                }

                set
                {
                    var oldValue = m_ScreenColors;
                    if (oldValue == value == true)
                        return;
                    m_ScreenColors = value;
                    DoChanged("ScreenColors", value, oldValue);
                }
            }

            /// <summary>
            /// Numero di campioni
            /// </summary>
            public int NumeroCampioni
            {
                get
                {
                    return m_NumeroCampioni;
                }

                set
                {
                    int oldValue = m_NumeroCampioni;
                    if (oldValue == value)
                        return;
                    m_NumeroCampioni = value;
                    DoChanged("NumeroCampioni", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(
                            m_IDDevice , " " ,
                            Sistema.Formats.FormatUserDateTime(m_StartDate) , " " ,
                            Sistema.Formats.FormatUserDateTime(m_EndDate)
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDDevice);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is DeviceLog) && this.Equals((DeviceLog)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(DeviceLog obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDDevice, obj.m_IDDevice)
                    && DMD.DateUtils.EQ(this.m_StartDate, obj.m_StartDate)
                    && DMD.DateUtils.EQ(this.m_EndDate, obj.m_EndDate)
                    && DMD.Integers.EQ(this.m_IDUtente, obj.m_IDUtente)
                    && DMD.Strings.EQ(this.m_NomeUtente, obj.m_NomeUtente)
                    && DMD.Integers.EQ(this.m_CPUUsage, obj.m_CPUUsage)
                    && DMD.Integers.EQ(this.m_CPUMaximum, obj.m_CPUMaximum)
                    && DMD.Doubles.EQ(this.m_RAMTotal, obj.m_RAMTotal)
                    && DMD.Doubles.EQ(this.m_RAMAvailable, obj.m_RAMAvailable)
                    && DMD.Doubles.EQ(this.m_RAMMinimum, obj.m_RAMMinimum)
                    && DMD.Doubles.EQ(this.m_DiskTotal, obj.m_DiskTotal)
                    && DMD.Doubles.EQ(this.m_DiskAvailable, obj.m_DiskAvailable)
                    && DMD.Doubles.EQ(this.m_DiskMinimum, obj.m_DiskMinimum)
                    && DMD.Doubles.EQ(this.m_Temperature, obj.m_Temperature)
                    && DMD.Doubles.EQ(this.m_TemperatureMaximum, obj.m_TemperatureMaximum)
                    && DMD.Integers.EQ(this.m_Counter1, obj.m_Counter1)
                    && DMD.Integers.EQ(this.m_Counter2, obj.m_Counter2)
                    && DMD.Integers.EQ(this.m_Counter3, obj.m_Counter3)
                    && DMD.Integers.EQ(this.m_Counter4, obj.m_Counter4)
                    && DMD.Integers.EQ(this.m_NumeroCampioni, obj.m_NumeroCampioni)
                    && this.m_GPS.Equals(obj.m_GPS)
                    && this.m_ScreenSize.Equals(obj.m_ScreenSize)
                    && DMD.Integers.EQ(this.m_ScreenColors, obj.m_ScreenColors)
                    && DMD.Strings.EQ(this.m_MACAddress, obj.m_MACAddress)
                    && DMD.Strings.EQ(this.m_IPAddress, obj.m_IPAddress)
                    && DMD.Strings.EQ(this.m_OSVersion, obj.m_OSVersion)
                    && DMD.Strings.EQ(this.m_DettaglioStato, obj.m_DettaglioStato)
                    ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.DevicesLog;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeDevLog";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDDevice = reader.Read("IDDevice", m_IDDevice);
                m_StartDate = reader.Read("StartDate", m_StartDate);
                m_EndDate = reader.Read("EndDate", m_EndDate);
                m_IDUtente = reader.Read("IDUtente", m_IDUtente);
                m_NomeUtente = reader.Read("NomeUtente", m_NomeUtente);
                m_CPUUsage = reader.Read("CPUUsage", m_CPUUsage);
                m_CPUMaximum = reader.Read("CPUMaximum", m_CPUMaximum);
                m_RAMTotal = reader.Read("RAMTotal", m_RAMTotal);
                m_RAMAvailable = reader.Read("RAMAvailable", m_RAMAvailable);
                m_RAMMinimum = reader.Read("RAMMinimum", m_RAMMinimum);
                m_DiskTotal = reader.Read("DiskTotal", m_DiskTotal);
                m_DiskAvailable = reader.Read("DiskAvailable", m_DiskAvailable);
                m_DiskMinimum = reader.Read("DiskMinimum", m_DiskMinimum);
                m_Temperature = reader.Read("Temperature", m_Temperature);
                m_TemperatureMaximum = reader.Read("TemperatureMaximum", m_TemperatureMaximum);
                m_Counter1 = reader.Read("Counter1", m_Counter1);
                m_Counter2 = reader.Read("Counter2",  m_Counter2);
                m_Counter3 = reader.Read("Counter3",  m_Counter3);
                m_Counter4 = reader.Read("Counter4",  m_Counter4);
                m_NumeroCampioni = reader.Read("NumeroCampioni",  m_NumeroCampioni);
                m_GPS.Altitudine = reader.Read("GPS_Alt", m_GPS.Altitudine);
                m_GPS.Longitudine = reader.Read("GPS_Lon", m_GPS.Longitudine);
                m_GPS.Latitudine = reader.Read("GPS_Lat", m_GPS.Latitudine);
                m_GPS.Bearing = reader.Read("GPS_Bear", m_GPS.Bearing);
                m_GPS.SetChanged(false);
                m_ScreenSize.Width = reader.Read("Screen_Width", m_ScreenSize.Width);
                m_ScreenSize.Height = reader.Read("Screen_Height", m_ScreenSize.Height);
                m_ScreenColors = reader.Read("ScreenColors", m_ScreenColors);
                m_IPAddress = reader.Read("IPAddress",  m_IPAddress);
                m_MACAddress = reader.Read("MACAddress",  m_MACAddress);
                m_OSVersion = reader.Read("OSVersion",  m_OSVersion);
                m_DettaglioStato = reader.Read("DettaglioStato",  m_DettaglioStato);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDDevice", IDDevice);
                writer.Write("StartDate", m_StartDate);
                writer.Write("EndDate", m_EndDate);
                writer.Write("IDUtente", IDUtente);
                writer.Write("NomeUtente", m_NomeUtente);
                writer.Write("CPUUsage", m_CPUUsage);
                writer.Write("CPUMaximum", m_CPUMaximum);
                writer.Write("RAMTotal", m_RAMTotal);
                writer.Write("RAMAvailable", m_RAMAvailable);
                writer.Write("RAMMinimum", m_RAMMinimum);
                writer.Write("DiskTotal", m_DiskTotal);
                writer.Write("DiskAvailable", m_DiskAvailable);
                writer.Write("DiskMinimum", m_DiskMinimum);
                writer.Write("Temperature", m_Temperature);
                writer.Write("TemperatureMaximum", m_TemperatureMaximum);
                writer.Write("Counter1", m_Counter1);
                writer.Write("Counter2", m_Counter2);
                writer.Write("Counter3", m_Counter3);
                writer.Write("Counter4", m_Counter4);
                writer.Write("NumeroCampioni", m_NumeroCampioni);
                writer.Write("GPS_Alt", m_GPS.Altitudine);
                writer.Write("GPS_Lon", m_GPS.Longitudine);
                writer.Write("GPS_Lat", m_GPS.Latitudine);
                writer.Write("GPS_Bear", m_GPS.Bearing);
                writer.Write("Screen_Width", m_ScreenSize.Width);
                writer.Write("Screen_Height", m_ScreenSize.Height);
                writer.Write("ScreenColors", m_ScreenColors);
                writer.Write("IPAddress", m_IPAddress);
                writer.Write("MACAddress", m_MACAddress);
                writer.Write("OSVersion", m_OSVersion);
                writer.Write("DettaglioStato", m_DettaglioStato);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDDevice", typeof(int), 1);
                c = table.Fields.Ensure("StartDate", typeof(DateTime), 1);
                c = table.Fields.Ensure("EndDate", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDUtente", typeof(int), 1);
                c = table.Fields.Ensure("NomeUtente", typeof(string), 255);
                c = table.Fields.Ensure("CPUUsage", typeof(int), 1);
                c = table.Fields.Ensure("CPUMaximum", typeof(int), 1);
                c = table.Fields.Ensure("RAMTotal", typeof(double), 1);
                c = table.Fields.Ensure("RAMAvailable", typeof(double), 1);
                c = table.Fields.Ensure("RAMMinimum", typeof(double), 1);
                c = table.Fields.Ensure("DiskTotal", typeof(double), 1);
                c = table.Fields.Ensure("DiskAvailable", typeof(double), 1);
                c = table.Fields.Ensure("DiskMinimum", typeof(double), 1);
                c = table.Fields.Ensure("Temperature", typeof(double), 1);
                c = table.Fields.Ensure("TemperatureMaximum", typeof(double), 1);
                c = table.Fields.Ensure("Counter1", typeof(int), 1);
                c = table.Fields.Ensure("Counter2", typeof(int), 1);
                c = table.Fields.Ensure("Counter3", typeof(int), 1);
                c = table.Fields.Ensure("Counter4", typeof(int), 1);
                c = table.Fields.Ensure("NumeroCampioni", typeof(int), 1);
                c = table.Fields.Ensure("GPS_Alt", typeof(double), 1);
                c = table.Fields.Ensure("GPS_Lon", typeof(double), 1);
                c = table.Fields.Ensure("GPS_Lat", typeof(double), 1);
                c = table.Fields.Ensure("GPS_Bear", typeof(double), 1);
                c = table.Fields.Ensure("Screen_Width", typeof(double), 1);
                c = table.Fields.Ensure("Screen_Height", typeof(double), 1);
                c = table.Fields.Ensure("ScreenColors", typeof(int), 1);
                c = table.Fields.Ensure("IPAddress", typeof(string), 255);
                c = table.Fields.Ensure("MACAddress", typeof(string), 255);
                c = table.Fields.Ensure("OSVersion", typeof(string), 255);
                c = table.Fields.Ensure("DettaglioStato", typeof(string), 255);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDevice", new string[] { "IDDevice", "NumeroCampioni"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUtente", new string[] { "IDUtente", "NomeUtente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "StartDate", "EndDate" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCounters1", new string[] { "Counter1", "Counter2", "Counter3", "Counter4" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCounters2", new string[] { "CPUUsage", "CPUMaximum", "RAMTotal", "RAMAvailable", "RAMMinimum" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCounters3", new string[] { "DiskTotal", "DiskAvailable", "DiskMinimum", "Temperature", "TemperatureMaximum" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCounters4", new string[] { "Screen_Width", "Screen_Height", "ScreenColors" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxGPS", new string[] { "GPS_Alt", "GPS_Lon", "GPS_Lat", "GPS_Bear"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCounters4", new string[] { "Screen_Width", "Screen_Height", "ScreenColors" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAddress", new string[] { "IPAddress", "MACAddress", "OSVersion", "DettaglioStato" }, DBFieldConstraintFlags.None);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDDevice", IDDevice);
                writer.WriteAttribute("StartDate", m_StartDate);
                writer.WriteAttribute("EndDate", m_EndDate);
                writer.WriteAttribute("IDUtente", IDUtente);
                writer.WriteAttribute("NomeUtente", m_NomeUtente);
                writer.WriteAttribute("CPUUsage", m_CPUUsage);
                writer.WriteAttribute("CPUMaximum", m_CPUMaximum);
                writer.WriteAttribute("RAMTotal", m_RAMTotal);
                writer.WriteAttribute("RAMAvailable", m_RAMAvailable);
                writer.WriteAttribute("RAMMinimum", m_RAMMinimum);
                writer.WriteAttribute("DiskTotal", m_DiskTotal);
                writer.WriteAttribute("DiskAvailable", m_DiskAvailable);
                writer.WriteAttribute("DiskMinimum", m_DiskMinimum);
                writer.WriteAttribute("Temperature", m_Temperature);
                writer.WriteAttribute("TemperatureMaximum", m_TemperatureMaximum);
                writer.WriteAttribute("Counter1", m_Counter1);
                writer.WriteAttribute("Counter2", m_Counter2);
                writer.WriteAttribute("Counter3", m_Counter3);
                writer.WriteAttribute("Counter4", m_Counter4);
                writer.WriteAttribute("NumeroCampioni", m_NumeroCampioni);
                writer.WriteAttribute("ScreenColors", m_ScreenColors);
                writer.WriteAttribute("IPAddress", m_IPAddress);
                writer.WriteAttribute("MACAddress", m_MACAddress);
                writer.WriteAttribute("OSVersion", m_OSVersion);
                writer.WriteAttribute("DettaglioStato", m_DettaglioStato);
                base.XMLSerialize(writer);
                writer.WriteTag("GPS", m_GPS);
                writer.WriteTag("ScreenSize", m_ScreenSize);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDDevice":
                        {
                            m_IDDevice = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                         
                    case "StartDate":
                        {
                            m_StartDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "EndDate":
                        {
                            m_EndDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }
                         

                    case "IDUtente":
                        {
                            m_IDUtente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeUtente":
                        {
                            m_NomeUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CPUUsage":
                        {
                            m_CPUUsage = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CPUMaximum":
                        {
                            m_CPUMaximum = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RAMTotal":
                        {
                            m_RAMTotal = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RAMAvailable":
                        {
                            m_RAMAvailable = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RAMMinimum":
                        {
                            m_RAMMinimum = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DiskTotal":
                        {
                            m_DiskTotal = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DiskAvailable":
                        {
                            m_DiskAvailable = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DiskMinimum":
                        {
                            m_DiskMinimum = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Temperature":
                        {
                            m_Temperature = (float?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TemperatureMaximum":
                        {
                            m_TemperatureMaximum = (float?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Counter1":
                        {
                            m_Counter1 = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Counter2":
                        {
                            m_Counter2 = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Counter3":
                        {
                            m_Counter3 = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Counter4":
                        {
                            m_Counter4 = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroCampioni":
                        {
                            m_NumeroCampioni = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ScreenColors":
                        {
                            m_ScreenColors = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "GPS":
                        {
                            m_GPS = (GPSRecord)fieldValue;
                            break;
                        }

                    case "ScreenSize":
                        {
                            m_ScreenSize = (CSize)fieldValue;
                            break;
                        }

                    case "IPAddress":
                        {
                            m_IPAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MACAddress":
                        {
                            m_MACAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "OSVersion":
                        {
                            m_OSVersion = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioStato":
                        {
                            m_DettaglioStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}