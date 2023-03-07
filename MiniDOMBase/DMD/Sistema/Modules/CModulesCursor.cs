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
        /// Cursore sulla tabella dei moduli registrati
        /// </summary>
        [Serializable]
        public class CModulesCursor 
            : minidom.Databases.DBObjectCursor<CModule>
        {
            private DBCursorField<int> m_ParentID;
            private DBCursorField<int> m_Posizione;
            private DBCursorStringField m_ModuleName;
            private DBCursorStringField m_ClassHandler;
            private DBCursorStringField m_ConfigClass;
            private DBCursorStringField m_DisplayName;
            private DBCursorStringField m_ModulePath;
            private DBCursorStringField m_IconPath;
            private DBCursorStringField m_Description;
            private DBCursorField<bool> m_Builtin;
            private DBCursorField<bool> m_Visible;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModulesCursor()
            {
                m_ParentID = new DBCursorField<int>("Parent");
                m_Posizione = new DBCursorField<int>("Posizione");
                m_ModuleName = new DBCursorStringField("ModuleName");
                m_ClassHandler = new DBCursorStringField("ClassHandler");
                m_ConfigClass = new DBCursorStringField("ConfigClass");
                m_DisplayName = new DBCursorStringField("DisplayName");
                m_ModulePath = new DBCursorStringField("ModulePath");
                m_IconPath = new DBCursorStringField("IconPath");
                m_Description = new DBCursorStringField("Description");
                m_Builtin = new DBCursorField<bool>("Builtin");
                m_Visible = new DBCursorField<bool>("Visible");
            }

            /// <summary>
            /// ParentID
            /// </summary>
            public DBCursorField<int> ParentID
            {
                get
                {
                    return m_ParentID;
                }
            }


            /// <summary>
            /// Posizione
            /// </summary>
            public DBCursorField<int> Posizione
            {
                get
                {
                    return m_Posizione;
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
            /// ClassHandler
            /// </summary>
            public DBCursorStringField ClassHandler
            {
                get
                {
                    return m_ClassHandler;
                }
            }

            /// <summary>
            /// ConfigClass
            /// </summary>
            public DBCursorStringField ConfigClass
            {
                get
                {
                    return m_ConfigClass;
                }
            }

            /// <summary>
            /// DisplayName
            /// </summary>
            public DBCursorStringField DisplayName
            {
                get
                {
                    return m_DisplayName;
                }
            }

            /// <summary>
            /// ModulePath
            /// </summary>
            public DBCursorStringField ModulePath
            {
                get
                {
                    return m_ModulePath;
                }
            }

            /// <summary>
            /// IconPath
            /// </summary>
            public DBCursorStringField IconPath
            {
                get
                {
                    return m_IconPath;
                }
            }

            /// <summary>
            /// Description
            /// </summary>
            public DBCursorStringField Description
            {
                get
                {
                    return m_Description;
                }
            }

            /// <summary>
            /// Builtin
            /// </summary>
            public DBCursorField<bool> Builtin
            {
                get
                {
                    return m_Builtin;
                }
            }

            /// <summary>
            /// Visible
            /// </summary>
            public DBCursorField<bool> Visible
            {
                get
                {
                    return m_Visible;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CModule InstantiateNewT(DBReader dbRis)
            {
                return new CModule();
            }

            ///// <summary>
            ///// Discriminator
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_Modules";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}