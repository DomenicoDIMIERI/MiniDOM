using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
    /// Cursore sulla tabella degli elementi sincronizzati
    /// </summary>
    /// <remarks></remarks>
        public sealed class SyncItemsCursor
            : minidom.Databases.DBObjectCursor<SyncItem>
        {
            private DBCursorStringField m_RemoteSite = new DBCursorStringField("RemoteSite");
            private DBCursorStringField m_ItemType = new DBCursorStringField("ItemType");
            private DBCursorField<int> m_LocalID = new DBCursorField<int>("LocalID");
            private DBCursorField<int> m_RemoteID = new DBCursorField<int>("RemoteID");
            private DBCursorField<DateTime> m_SyncDate = new DBCursorField<DateTime>("SyncDate");

            /// <summary>
            /// Costruttore
            /// </summary>
            public SyncItemsCursor()
            {
            }

            /// <summary>
            /// RemoteSite
            /// </summary>
            public DBCursorStringField RemoteSite
            {
                get
                {
                    return m_RemoteSite;
                }
            }

            /// <summary>
            /// ItemType
            /// </summary>
            public DBCursorStringField ItemType
            {
                get
                {
                    return m_ItemType;
                }
            }

            /// <summary>
            /// LocalID
            /// </summary>
            public DBCursorField<int> LocalID
            {
                get
                {
                    return m_LocalID;
                }
            }

            /// <summary>
            /// RemoteID
            /// </summary>
            public DBCursorField<int> RemoteID
            {
                get
                {
                    return m_RemoteID;
                }
            }

            /// <summary>
            /// SyncDate
            /// </summary>
            public DBCursorField<DateTime> SyncDate
            {
                get
                {
                    return m_SyncDate;
                }
            }
              
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Sincronizzazioni;
            }
        }
    }
}