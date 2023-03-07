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
        /// Cursore sulla tabella dei log
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class DeviceLogCursor 
            : minidom.Databases.DBObjectCursorPO<DeviceLog>
        {
            private DBCursorField<int> m_IDDevice = new DBCursorField<int>("IDDevice");
            private DBCursorField<DeviceLogFlags> m_Flags = new DBCursorField<DeviceLogFlags>("Flags");
            private DBCursorField<DateTime> m_StartDate = new DBCursorField<DateTime>("StartDate");
            private DBCursorField<DateTime> m_EndDate = new DBCursorField<DateTime>("EndDate");
            private DBCursorField<int> m_IDUtente = new DBCursorField<int>("IDUtente");
            private DBCursorStringField m_NomeUtente = new DBCursorStringField("NomeUtente");
            private DBCursorField<int> m_CPUUsage = new DBCursorField<int>("CPUUsage");
            private DBCursorField<int> m_CPUMaximum = new DBCursorField<int>("CPUMaximum");
            private DBCursorField<double> m_RAMTotal = new DBCursorField<double>("RAMTotal");
            private DBCursorField<double> m_RAMAvailable = new DBCursorField<double>("RAMAvailable");
            private DBCursorField<double> m_RAMMinimum = new DBCursorField<double>("RAMMinimum");
            private DBCursorField<double> m_DiskTotal = new DBCursorField<double>("DiskTotal");
            private DBCursorField<double> m_DiskAvailable = new DBCursorField<double>("DiskAvailable");
            private DBCursorField<double> m_DiskMinimum = new DBCursorField<double>("DiskMinimum");
            private DBCursorField<float> m_Temperature = new DBCursorField<float>("Temperature");
            private DBCursorField<float> m_TemperatureMaximum = new DBCursorField<float>("TemperatureMaximum");
            private DBCursorField<int> m_Counter1 = new DBCursorField<int>("Counter1");
            private DBCursorField<int> m_Counter2 = new DBCursorField<int>("Counter2");
            private DBCursorField<int> m_Counter3 = new DBCursorField<int>("Counter3");
            private DBCursorField<int> m_Counter4 = new DBCursorField<int>("Counter4");
            private DBCursorField<int> m_NumeroCampioni = new DBCursorField<int>("NumeroCampioni");
            private DBCursorField<int> m_ScreenColors = new DBCursorField<int>("ScreenColors");
            private DBCursorField<double> m_GPS_Alt = new DBCursorField<double>("GPS_Alt");
            private DBCursorField<double> m_GPS_Lat = new DBCursorField<double>("GPS_Lat");
            private DBCursorField<double> m_GPS_Lon = new DBCursorField<double>("GPS_Lon");
            private DBCursorField<double> m_GPS_Bear = new DBCursorField<double>("GPS_Bear");
            private DBCursorField<int> m_Screen_Width = new DBCursorField<int>("Screen_Width");
            private DBCursorField<int> m_Screen_Height = new DBCursorField<int>("Screen_Height");
            private DBCursorStringField m_IPAddress = new DBCursorStringField("IPAddress");
            private DBCursorStringField m_MACAddress = new DBCursorStringField("MACAddress");
            private DBCursorStringField m_OSVersion = new DBCursorStringField("OSVersion");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");

            /// <summary>
            /// Costruttore
            /// </summary>
            public DeviceLogCursor()
            {
            }

            /// <summary>
            /// IPAddress
            /// </summary>
            public DBCursorStringField IPAddress
            {
                get
                {
                    return m_IPAddress;
                }
            }

            /// <summary>
            /// MACAddress
            /// </summary>
            public DBCursorStringField MACAddress
            {
                get
                {
                    return m_MACAddress;
                }
            }

            /// <summary>
            /// OSVersion
            /// </summary>
            public DBCursorStringField OSVersion
            {
                get
                {
                    return m_OSVersion;
                }
            }

            /// <summary>
            /// DettaglioStato
            /// </summary>
            public DBCursorStringField DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }
            }

            /// <summary>
            /// IDDevice
            /// </summary>
            public DBCursorField<int> IDDevice
            {
                get
                {
                    return m_IDDevice;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<DeviceLogFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// StartDate
            /// </summary>
            public DBCursorField<DateTime> StartDate
            {
                get
                {
                    return m_StartDate;
                }
            }

            /// <summary>
            /// EndDate
            /// </summary>
            public DBCursorField<DateTime> EndDate
            {
                get
                {
                    return m_EndDate;
                }
            }

            /// <summary>
            /// IDUtente
            /// </summary>
            public DBCursorField<int> IDUtente
            {
                get
                {
                    return m_IDUtente;
                }
            }

            /// <summary>
            /// NomeUtente
            /// </summary>
            public DBCursorStringField NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }
            }

            /// <summary>
            /// CPUUsage
            /// </summary>
            public DBCursorField<int> CPUUsage
            {
                get
                {
                    return m_CPUUsage;
                }
            }

            /// <summary>
            /// CPUMaximum
            /// </summary>
            public DBCursorField<int> CPUMaximum
            {
                get
                {
                    return m_CPUMaximum;
                }
            }

            /// <summary>
            /// RAMTotal
            /// </summary>
            public DBCursorField<double> RAMTotal
            {
                get
                {
                    return m_RAMTotal;
                }
            }

            /// <summary>
            /// RAMAvailable
            /// </summary>
            public DBCursorField<double> RAMAvailable
            {
                get
                {
                    return m_RAMAvailable;
                }
            }

            /// <summary>
            /// RAMMinimum
            /// </summary>
            public DBCursorField<double> RAMMinimum
            {
                get
                {
                    return m_RAMMinimum;
                }
            }

            /// <summary>
            /// DiskTotal
            /// </summary>
            public DBCursorField<double> DiskTotal
            {
                get
                {
                    return m_DiskTotal;
                }
            }

            /// <summary>
            /// DiskAvailable
            /// </summary>
            public DBCursorField<double> DiskAvailable
            {
                get
                {
                    return m_DiskAvailable;
                }
            }

            /// <summary>
            /// DiskMinimum
            /// </summary>
            public DBCursorField<double> DiskMinimum
            {
                get
                {
                    return m_DiskMinimum;
                }
            }

            /// <summary>
            /// Temperature
            /// </summary>
            public DBCursorField<float> Temperature
            {
                get
                {
                    return m_Temperature;
                }
            }

            /// <summary>
            /// TemperatureMaximum
            /// </summary>
            public DBCursorField<float> TemperatureMaximum
            {
                get
                {
                    return m_TemperatureMaximum;
                }
            }

            /// <summary>
            /// Counter1
            /// </summary>
            public DBCursorField<int> Counter1
            {
                get
                {
                    return m_Counter1;
                }
            }

            /// <summary>
            /// Counter2
            /// </summary>
            public DBCursorField<int> Counter2
            {
                get
                {
                    return m_Counter2;
                }
            }

            /// <summary>
            /// Counter3
            /// </summary>
            public DBCursorField<int> Counter3
            {
                get
                {
                    return m_Counter3;
                }
            }

            /// <summary>
            /// Counter4
            /// </summary>
            public DBCursorField<int> Counter4
            {
                get
                {
                    return m_Counter4;
                }
            }

            /// <summary>
            /// NumeroCampioni
            /// </summary>
            public DBCursorField<int> NumeroCampioni
            {
                get
                {
                    return m_NumeroCampioni;
                }
            }

            /// <summary>
            /// ScreenColors
            /// </summary>
            public DBCursorField<int> ScreenColors
            {
                get
                {
                    return m_ScreenColors;
                }
            }

            /// <summary>
            /// GPS_Alt
            /// </summary>
            public DBCursorField<double> GPS_Alt
            {
                get
                {
                    return m_GPS_Alt;
                }
            }

            /// <summary>
            /// GPS_Lat
            /// </summary>
            public DBCursorField<double> GPS_Lat
            {
                get
                {
                    return m_GPS_Lat;
                }
            }

            /// <summary>
            /// GPS_Lon
            /// </summary>
            public DBCursorField<double> GPS_Lon
            {
                get
                {
                    return m_GPS_Lon;
                }
            }

            /// <summary>
            /// GPS_Bear
            /// </summary>
            public DBCursorField<double> GPS_Bear
            {
                get
                {
                    return m_GPS_Bear;
                }
            }

            /// <summary>
            /// Screen_Width
            /// </summary>
            public DBCursorField<int> Screen_Width
            {
                get
                {
                    return m_Screen_Width;
                }
            }

            /// <summary>
            /// Screen_Height
            /// </summary>
            public DBCursorField<int> Screen_Height
            {
                get
                {
                    return m_Screen_Height;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.DevicesLog;
            }
             
        }
    }
}