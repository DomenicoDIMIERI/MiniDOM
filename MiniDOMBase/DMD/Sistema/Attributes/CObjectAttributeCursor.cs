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
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella degli attributi
        /// </summary>
        [Serializable]
        public class CObjectAttributeCursor 
            : minidom.Databases.DBObjectCursor<CObjectAttribute>
        {
            private DBCursorField<int> m_ObjectID;
            private DBCursorStringField m_ObjectType;
            private DBCursorStringField m_AttributeName;
            private DBCursorField<int> m_AttributeType;
            private DBCursorStringField m_AttributeValue;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CObjectAttributeCursor()
            {
                m_ObjectID = new DBCursorField<int>("IDObject");
                m_ObjectType = new DBCursorStringField("ObjectType");
                m_AttributeName = new DBCursorStringField("AttributeName");
                m_AttributeType = new DBCursorField<int>("AttributeType");
                m_AttributeValue = new DBCursorStringField("AttributeValue");
            }

            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Attributi; // Sistema.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_ObjectAttributes";
            //}

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CObjectAttribute();
            //}

            /// <summary>
            /// Campo ObjectID
            /// </summary>
            public DBCursorField<int> ObjectID
            {
                get
                {
                    return m_ObjectID;
                }
            }

            /// <summary>
            /// Campo ObjectType
            /// </summary>
            public DBCursorStringField ObjectType
            {
                get
                {
                    return m_ObjectType;
                }
            }

            /// <summary>
            /// Campo AttributeName
            /// </summary>
            public DBCursorStringField AttributeName
            {
                get
                {
                    return m_AttributeName;
                }
            }

            /// <summary>
            /// Campo AttributeType
            /// </summary>
            public DBCursorField<int> AttributeType
            {
                get
                {
                    return m_AttributeType;
                }
            }

            /// <summary>
            /// Campo AttributeValue
            /// </summary>
            public DBCursorStringField AttributeValue
            {
                get
                {
                    return m_AttributeValue;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}