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
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella dei <see cref="CRegisteredDevice"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CRegisteredDeviceCursor 
            : minidom.Databases.DBObjectCursor<CRegisteredDevice>
        {
            private DBCursorStringField m_ClassName;
            private DBCursorStringField m_DriverName;
            private DBCursorStringField m_DeviceName;
            private DBCursorStringField m_Address;
            private DBCursorField<int> m_Flags;
            private DBCursorField<RegisteredDeviceCaps> m_Capabilities;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredDeviceCursor()
            {
                this.m_ClassName= new DBCursorStringField ("ClassName");
                this.m_DriverName = new DBCursorStringField("DriverName");
                this.m_DeviceName = new DBCursorStringField("DeviceName");
                this.m_Address = new DBCursorStringField("Address");
                this.m_Flags = new DBCursorField<int>("Flags");
                this.m_Capabilities = new DBCursorField<RegisteredDeviceCaps>("Capabilities");
            }

            /// <summary>
            /// Capabilities
            /// </summary>
            public DBCursorField<RegisteredDeviceCaps> Capabilities
            {
                get
                {
                    return this.m_Capabilities;
                }
            }

            /// <summary>
            /// ClassName
            /// </summary>
            public DBCursorStringField ClassName
            {
                get
                {
                    return this.m_ClassName;
                }
            }

            /// <summary>
            /// DriverName
            /// </summary>
            public DBCursorStringField DriverName
            {
                get
                {
                    return this.m_DriverName;
                }
            }

            /// <summary>
            /// DeviceName
            /// </summary>
            public DBCursorStringField DeviceName
            {
                get
                {
                    return this.m_DeviceName;
                }
            }

            /// <summary>
            /// Address
            /// </summary>
            public DBCursorStringField Address
            {
                get
                {
                    return this.m_Address;
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


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.RegisteredDevices; //.Module;
            }



            
        }
    }
}