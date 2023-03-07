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
        /// Cursore sulla tabella delle azioni definite per un modulo
        /// </summary>
        [Serializable]
        public class CModuleActionsCursor 
            : minidom.Databases.DBObjectCursorBase<CModuleAction>
        {
            private DBCursorStringField m_ModuleName;
            private DBCursorStringField m_AuthorizationName;
            private DBCursorStringField m_AuthorizationDescription;
            private DBCursorField<bool> m_Visible;
            private DBCursorStringField m_ClassHandler;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModuleActionsCursor()
            {
                m_ModuleName = new DBCursorStringField("Modulo");
                m_AuthorizationName = new DBCursorStringField("AuthorizationName");
                m_AuthorizationDescription = new DBCursorStringField("AuthorizationDescription");
                m_Visible = new DBCursorField<bool>("Visible");
                m_ClassHandler = new DBCursorStringField("ClassHandler");
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
            /// AuthorizationName
            /// </summary>
            public DBCursorStringField AuthorizationName
            {
                get
                {
                    return m_AuthorizationName;
                }
            }

            /// <summary>
            /// AuthorizationDescription
            /// </summary>
            public DBCursorStringField AuthorizationDescription
            {
                get
                {
                    return m_AuthorizationDescription;
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
            public override CModuleAction InstantiateNewT(DBReader dbRis)
            {
                return new CModuleAction();
            }

            // Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
            // Dim col As CCursorFieldsCollection
            // col = MyBase.GetCursorFields
            // Call col.Add(m_ModuleName)
            // Call col.Add(m_AuthorizationName)
            // Call col.Add(m_AuthorizationDescription)
            // Call col.Add(m_Visible)
            // Call col.Add(m_ClassHandler)
            // Return col
            // End Function

            ///// <summary>
            ///// Discriminator
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_DefinedAuthorizations";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules.DefinedActions;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}