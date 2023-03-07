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
        /// Cursore di <see cref="ClasseDispositivo"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ClassiDispositivoCursor 
            : minidom.Databases.DBObjectCursor<ClasseDispositivo>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<DeviceFlags> m_Flags = new DBCursorField<DeviceFlags>("Flags");
            private DBCursorStringField m_IconUrl = new DBCursorStringField("IconUrl");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ClassiDispositivoCursor()
            {
            }

            /// <summary>
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconUrl;
                }
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<DeviceFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.ClassiDispositivi;
            }

         
        }
    }
}