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
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CalendarSchedule"/>
        /// </summary>
        [Serializable]
        public sealed class CScheduledTasksClass 
            : CModulesClass<Sistema.CalendarSchedule>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CScheduledTasksClass() 
                : base("modScheduledTasks", typeof(Sistema.CalendarScheduleCursor))
            {
            }
        }
    }


    public partial class CalendarioClass
    {

   
        private CScheduledTasksClass m_ScheduldTasks = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CalendarSchedule"/>
        /// </summary>
        public CScheduledTasksClass ScheduledTasks
        {
            get
            {
                if (m_ScheduldTasks is null)
                    m_ScheduldTasks = new CScheduledTasksClass();
                return m_ScheduldTasks;
            }
        }
    }
}