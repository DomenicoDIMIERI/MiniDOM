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
        /// Repository di oggetti <see cref="CRegisteredCalendarProvider"/>
        /// </summary>
        [Serializable]
        public class CRegisteredCalendarProviderRepository
            : CModulesClass<CRegisteredCalendarProvider>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredCalendarProviderRepository()
                : base("modRegisteredCalendarProvider", typeof(CRegisteredCalendarProviderCursor ), -1)
            {

            }

        }

    }

    public partial class CalendarioClass
    {


        private CRegisteredCalendarProviderRepository m_RegisteredCalendarProviders = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CalendarSchedule"/>
        /// </summary>
        public CRegisteredCalendarProviderRepository RegisteredCalendarProviders
        {
            get
            {
                if (m_RegisteredCalendarProviders is null)
                    m_RegisteredCalendarProviders = new CRegisteredCalendarProviderRepository();
                return m_RegisteredCalendarProviders;
            }
        }
    }

}