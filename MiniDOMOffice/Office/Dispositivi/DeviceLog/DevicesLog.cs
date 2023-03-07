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
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="DeviceLog"/>
        /// </summary>
        [Serializable]
        public sealed class CDevicesLogClass 
            : CModulesClass<DeviceLog>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDevicesLogClass() 
                : base("modOfficeDevLog", typeof(DeviceLogCursor), 50)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeDevLog");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeDevLog");
            //        ret.Description = "Log Dispositivi";
            //        ret.DisplayName = "Log Dispositivi";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!minidom.Office.Database.Tables.ContainsKey("tbl_OfficeDevLog"))
            //    {
            //        var table = minidom.Office.Database.Tables.Add("tbl_OfficeDevLog");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("IDDevice", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Flags", TypeCode.Int32);
            //        field = table.Fields.Add("Params", TypeCode.String);
            //        field.MaxLength = 0;
            //        field = table.Fields.Add("StartDate", TypeCode.DateTime);
            //        field = table.Fields.Add("EndDate", TypeCode.DateTime);
            //        field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32);
            //        field = table.Fields.Add("NomePuntoOperativo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("CreatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("CreatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("ModificatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("ModificatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("Stato", TypeCode.Int32);
            //        table.Create();
            //    }

            //    return ret;
            //}


            /// <summary>
            /// Restituisce l'ultimo log registrato per la periferica
            /// </summary>
            /// <param name="dev"></param>
            /// <returns></returns>
            public DeviceLog GetLastLog(Dispositivo dev)
            {
                DeviceLog log = null;
                lock (cacheLock)
                {
                    if (dev is null)
                        throw new ArgumentNullException("dev");
                    foreach (var o in CachedItems)
                    {
                        log = (DeviceLog) o.Item;
                        if (log.IDDevice == DBUtils.GetID(dev, 0))
                        {
                            log.SetDevice(dev);
                            return log;
                        }
                    }
                }


                using (var cursor = new DeviceLogCursor())
                {
                    cursor.IDDevice.Value = DBUtils.GetID(dev, 0);
                    cursor.ID.SortOrder = SortEnum.SORT_DESC;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    log = cursor.Item;
                    if (log is object)
                    {
                        log.SetDevice(dev);
                        AddToCache(log);
                    }
                }

                return log;
            }
        }
    }

    public partial class Office
    {
        private static CDevicesLogClass m_DevicesLog = null;

        /// <summary>
        /// Repository di <see cref="DeviceLog"/>
        /// </summary>
        public static CDevicesLogClass DevicesLog
        {
            get
            {
                if (m_DevicesLog is null)
                    m_DevicesLog = new CDevicesLogClass();
                return m_DevicesLog;
            }
        }
    }
}