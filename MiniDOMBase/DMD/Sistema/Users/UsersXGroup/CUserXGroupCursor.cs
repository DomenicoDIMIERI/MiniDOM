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
        /// Cursore sugli oggetti di tipo <see cref="CUserXGroup"/>
        /// </summary>
        [Serializable]
        public class CUserXGroupCursor
            : minidom.Databases.DBObjectCursorBase<CUserXGroup>
        {
            private DBCursorField<int> m_UserID;
            private DBCursorField<int> m_GroupID;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserXGroupCursor()
            {
                m_UserID = new DBCursorField<int>("User");
                m_GroupID = new DBCursorField<int>("Group");
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Users.UsersXGroupsRepository;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_UsersXGroup";
            }

            /// <summary>
            /// UserID
            /// </summary>
            public DBCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
                }
            }

            /// <summary>
            /// GroupID
            /// </summary>
            public DBCursorField<int> GroupID
            {
                get
                {
                    return m_GroupID;
                }
            }
        }
    }
}