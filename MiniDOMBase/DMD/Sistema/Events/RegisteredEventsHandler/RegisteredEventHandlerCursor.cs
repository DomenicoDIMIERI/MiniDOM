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
        /// Cursore sulla tabella dei gestori di eventi registrati
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class RegisteredEventHandlerCursor
            : minidom.Databases.DBObjectCursorBase<RegisteredEventHandler>
        {
            private DBCursorField<int> m_ModuleID = new DBCursorField<int>("Module");
            private DBCursorStringField m_ModuleName = new DBCursorStringField("ModuleName");
            private DBCursorStringField m_EventName = new DBCursorStringField("EventName");
            private DBCursorStringField m_ClassName = new DBCursorStringField("ClassName");
            private DBCursorField<int> m_Priority = new DBCursorField<int>("Priority");
            private DBCursorField<bool> m_Active = new DBCursorField<bool>("Active");

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegisteredEventHandlerCursor()
            {
            }

            /// <summary>
            /// Active
            /// </summary>
            public DBCursorField<bool> Active
            {
                get
                {
                    return m_Active;
                }
            }

            /// <summary>
            /// ModuleID
            /// </summary>
            public DBCursorField<int> ModuleID
            {
                get
                {
                    return m_ModuleID;
                }
            }

            /// <summary>
            /// EventName
            /// </summary>
            public DBCursorStringField EventName
            {
                get
                {
                    return m_EventName;
                }
            }

            /// <summary>
            /// ClassName
            /// </summary>
            public DBCursorStringField ClassName
            {
                get
                {
                    return m_ClassName;
                }
            }

            /// <summary>
            /// ModuleName
            /// </summary>
            public DBCursorStringField ModuleName
            {
                get
                {
                    return m_ModuleName;
                }
            }

            /// <summary>
            /// Priority
            /// </summary>
            public DBCursorField<int> Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.RegisteredEventHandlers;
            }
             
        }
    }
}