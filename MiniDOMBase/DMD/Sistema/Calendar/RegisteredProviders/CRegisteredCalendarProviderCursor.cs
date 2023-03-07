using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore di oggetti <see cref="CRegisteredCalendarProvider"/>
        /// </summary>
        [Serializable]
        public class CRegisteredCalendarProviderCursor
            : minidom.Databases.DBObjectCursorBase<CRegisteredCalendarProvider>
        {

            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_TypeName = new DBCursorStringField("ClassName");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredCalendarProviderCursor()
            {
                
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Calendar.RegisteredCalendarProviders;
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return this.m_Nome;
                }
            }

            /// <summary>
            /// TypeName
            /// </summary>
            public DBCursorStringField TypeName
            {
                get
                {
                    return this.m_TypeName;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return this.m_Flags;
                }
            }

        }
    }
}