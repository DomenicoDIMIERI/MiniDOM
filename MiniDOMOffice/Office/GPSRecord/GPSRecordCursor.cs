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
        /// Cursore sulla tabella dei luoghi attraversati dall'utente per le commissioni
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class GPSRecordCursor 
            : minidom.Databases.DBObjectCursorBase<GPSRecord>
        {
            private DBCursorField<int> m_IDDispositivo = new DBCursorField<int>("IDDispositivo");
            private DBCursorField<double> m_Latitudine = new DBCursorField<double>("Latitudine");
            private DBCursorField<double> m_Longitudine = new DBCursorField<double>("Longitudine");
            private DBCursorField<double> m_Altitudine = new DBCursorField<double>("Altitudine");
            private DBCursorField<double> m_Bearing = new DBCursorField<double>("Bearing");
            private DBCursorField<DateTime> m_Istante1 = new DBCursorField<DateTime>("Istante1");
            private DBCursorField<DateTime> m_Istante2 = new DBCursorField<DateTime>("Istante2");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");


            /// <summary>
            /// Costruttore
            /// </summary>
            public GPSRecordCursor()
            {

            }

            /// <summary>
            /// IDDispositivo
            /// </summary>
            public DBCursorField<int> IDDispositivo
            {
                get
                {
                    return m_IDDispositivo;
                }
            }

            /// <summary>
            /// Bearing
            /// </summary>
            public DBCursorField<double> Bearing
            {
                get
                {
                    return m_Bearing;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Istante1
            /// </summary>
            public DBCursorField<DateTime> Istante1
            {
                get
                {
                    return m_Istante1;
                }
            }

            /// <summary>
            /// Istante2
            /// </summary>
            public DBCursorField<DateTime> Istante2
            {
                get
                {
                    return m_Istante2;
                }
            }

            /// <summary>
            /// Latitudine
            /// </summary>
            public DBCursorField<double> Latitudine
            {
                get
                {
                    return m_Latitudine;
                }
            }

            /// <summary>
            /// Longitudine
            /// </summary>
            public DBCursorField<double> Longitudine
            {
                get
                {
                    return m_Longitudine;
                }
            }

            /// <summary>
            /// Altitudine
            /// </summary>
            public DBCursorField<double> Altitudine
            {
                get
                {
                    return m_Altitudine;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.GPSRecords;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeGPS";
            }

          
        }
    }
}