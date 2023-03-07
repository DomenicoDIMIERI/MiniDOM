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
        /// Cursore sulla tabella delle pagine registrate
        /// </summary>
        [Serializable]
        public class CRegisteredPropertyPageCursor 
            : minidom.Databases.DBObjectCursorBase<CRegisteredPropertyPage>
        {
            private DBCursorStringField m_ClassName;
            private DBCursorStringField m_TabPageClass;
            private DBCursorStringField m_Context;
            private DBCursorField<int> m_Priority;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredPropertyPageCursor()
            {
                m_ClassName = new DBCursorStringField("ClassName");
                m_TabPageClass = new DBCursorStringField("TabPageClass");
                m_Context = new DBCursorStringField("Context");
                m_Priority = new DBCursorField<int>("Priority");
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
            /// TabPageClass
            /// </summary>
            public DBCursorStringField TabPageClass
            {
                get
                {
                    return m_TabPageClass;
                }
            }

            /// <summary>
            /// Context
            /// </summary>
            public DBCursorStringField Context
            {
                get
                {
                    return m_Context;
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
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CRegisteredPropertyPage InstantiateNewT(DBReader dbRis)
            {
                return new CRegisteredPropertyPage();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_RegisteredTabPages";
            //}

            // Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
            // Dim col As CCursorFieldsCollection
            // col = MyBase.GetCursorFields
            // col.Add(m_ClassName)
            // col.Add(m_TabPageClass)
            // col.Add(m_Context)
            // col.Add(m_Priority)
            // Return col
            // End Function

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.PropertyPages;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}