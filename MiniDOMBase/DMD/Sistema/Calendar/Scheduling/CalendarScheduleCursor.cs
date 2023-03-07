using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="CalendarSchedule"/>
        /// </summary>
        [Serializable]
        public class CalendarScheduleCursor 
            : minidom.Databases.DBObjectCursor<CalendarSchedule>
        {

            private DBCursorField<ScheduleType> m_ScheduleType = new DBCursorField<ScheduleType>("ScheduleType");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<float> m_Intervallo = new DBCursorField<float>("Itnervallo");
            private DBCursorField<int> m_Ripetizioni = new DBCursorField<int>("Ripetizioni");
            private DBCursorStringField m_OwnerType = new DBCursorStringField("OwnerType");
            private DBCursorField<int> m_OwnerID = new DBCursorField<int>("OwnerID");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CalendarScheduleCursor()
            {
            }

            /// <summary>
            /// ScheduleType
            /// </summary>
            public DBCursorField<ScheduleType> ScheduleType
            {
                get
                {
                    return m_ScheduleType;
                }
            }

            /// <summary>
            /// DataInizio
            /// </summary>
            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            /// <summary>
            /// DataFine
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// Intervallo
            /// </summary>
            public DBCursorField<float> Intervallo
            {
                get
                {
                    return m_Intervallo;
                }
            }

            /// <summary>
            /// Ripetizioni
            /// </summary>
            public DBCursorField<int> Ripetizioni
            {
                get
                {
                    return m_Ripetizioni;
                }
            }

            /// <summary>
            /// OwnerType
            /// </summary>
            public DBCursorStringField OwnerType
            {
                get
                {
                    return m_OwnerType;
                }
            }

            /// <summary>
            /// OwnerID
            /// </summary>
            public DBCursorField<int> OwnerID
            {
                get
                {
                    return m_OwnerID;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Calendar.ScheduledTasks; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CalendarSchedules";
            //}
        }
    }
}