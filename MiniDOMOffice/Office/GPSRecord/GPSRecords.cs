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
        /// Repository di <see cref="GPSRecord"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class GPSRecordsClass
            : CModulesClass<GPSRecord>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public GPSRecordsClass() 
                : base("modOfficeGPSRecords", typeof(GPSRecordCursor), 0)
            {
            }

            // Protected Overrides Function InitializeModule() As CModule
            // Return MyBase.InitializeModule()
            // Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeGPSRecords")
            // If (ret Is Nothing) Then
            // ret = New CModule("modOfficeGPSRecords")
            // ret.Description = "GPS Records"
            // ret.DisplayName = "GPS Records"
            // ret.Parent = Office.Module
            // ret.Stato = ObjectStatus.OBJECT_VALID
            // ret.Save()
            // ret.InitializeStandardActions()
            // End If
            // If Not Office.Database.Tables.ContainsKey("tbl_OfficeLuoghiV") Then
            // Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeLuoghiV")
            // Dim field As CDBEntityField
            // field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
            // field = table.Fields.Add("UserID", TypeCode.Int32)
            // field = table.Fields.Add("IDDispositivo", TypeCode.Int32)
            // field = table.Fields.Add("ContextType", TypeCode.String) : field.MaxLength = 255
            // field = table.Fields.Add("ContextID", TypeCode.Int32)
            // field = table.Fields.Add("Istante1", TypeCode.DateTime)
            // field = table.Fields.Add("Istante2", TypeCode.DateTime)
            // field = table.Fields.Add("Latitudine", TypeCode.Double)
            // field = table.Fields.Add("Longitudine", TypeCode.Double)
            // field = table.Fields.Add("Altitudine", TypeCode.Double)
            // table.Create()
            // End If
            // Return ret
            // End Function

            /// <summary>
            /// Restituisce le posizioni registrata per il dispositivo nell'intervallo specificato
            /// </summary>
            /// <param name="dispositivo"></param>
            /// <param name="daIstante"></param>
            /// <param name="aIstante"></param>
            /// <returns></returns>
            public CCollection<GPSRecord> GetPosizioniDispositivo(
                                                        Dispositivo dispositivo, 
                                                        DateTime? daIstante, 
                                                        DateTime? aIstante
                                                        )
            {
                if (dispositivo is null)
                    throw new ArgumentNullException("dispositivo");

                var ret = new CCollection<minidom.Office.GPSRecord>();

                using (var cursor = new GPSRecordCursor())
                {
                    cursor.IDDispositivo.Value = DBUtils.GetID(dispositivo, 0);
                    cursor.Istante1.SortOrder = SortEnum.SORT_ASC;
                    if (daIstante.HasValue)
                    {
                        cursor.Istante1.Value = daIstante.Value;
                        if (aIstante.HasValue)
                        {
                            cursor.Istante1.Value1 = aIstante.Value;
                            cursor.Istante1.Operator = OP.OP_BETWEEN;
                        }
                        else
                        {
                            cursor.Istante1.Operator = OP.OP_GE;
                        }
                    }
                    else if (aIstante.HasValue)
                    {
                        cursor.Istante1.Value = aIstante.Value;
                        cursor.Istante1.Operator = OP.OP_LT;
                    }

                    while (cursor.Read())
                    {
                        var rec = cursor.Item;
                        rec.SetDispositivo(dispositivo);
                        ret.Add(rec);
                    }

                }
                return ret;
            }
        }
    }

    public partial class Office
    {
        private static GPSRecordsClass m_GPSRecords = null;

        /// <summary>
        /// Repository di <see cref="GPSRecord"/>
        /// </summary>
        /// <remarks></remarks>
        public static GPSRecordsClass GPSRecords
        {
            get
            {
                if (m_GPSRecords is null)
                    m_GPSRecords = new GPSRecordsClass();
                return m_GPSRecords;
            }
        }
    }
}