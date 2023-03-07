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

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CSettingRecord"/>
        /// </summary>
        [Serializable]
        public class CSettingRecordsRepository
            : CModulesClass<CSettingRecord>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSettingRecordsRepository()
                : base("modSettingRecords", typeof(CSettingRecordCursor), -1)
            {

            }
        }
    }
    
    public partial class Sistema
    {

        private static CSettingRecordsRepository m_Settings = null;

        /// <summary>
        /// Repository di oggetti <see cref="CSettingRecord"/>
        /// </summary>
        public static CSettingRecordsRepository Settings
        {
            get
            {
                if (m_Settings is null) m_Settings = new CSettingRecordsRepository();
                return m_Settings;
            }
        }
    }
}