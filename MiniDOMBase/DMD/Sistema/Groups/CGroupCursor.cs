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
        /// Cursore sulla tabella dei gruppi
        /// </summary>
        [Serializable]
        public class CGroupCursor 
            : minidom.Databases.DBObjectCursor<CGroup>
        {
            private DBCursorStringField m_GroupName;
            private DBCursorStringField m_Description;
            private DBCursorField<int> m_Flags;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupCursor()
            {
                m_GroupName = new DBCursorStringField("GroupName");
                m_Description = new DBCursorStringField("Description");
                m_Flags = new DBCursorField<int>("Flags");
            }

            /// <summary>
            /// GroupName
            /// </summary>
            public DBCursorStringField GroupName
            {
                get
                {
                    return m_GroupName;
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
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CGroup InstantiateNewT(DBReader dbRis)
            {
                return new CGroup();
            }

            /// <summary>
            /// Inizializza l'oggetto aggiunto
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(CGroup item)
            {
                item.SetGroupName(minidom.Sistema.Groups.GetFirstAvailableGroupName("Gruppo"));

                base.OnInitialize(item);
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Gruppi";
            //}

            // Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
            // Dim col As CCursorFieldsCollection
            // col = MyBase.GetCursorFields
            // col.Add(m_GroupName)
            // col.Add(m_Description)
            // Return col
            // End Function

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Groups; //.Module;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}
        }
    }
}