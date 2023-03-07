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
        /// Cursore di <see cref="MailAddress"/>
        /// </summary>
        [Serializable]
        public class MailAddressCursor 
            : minidom.Databases.DBObjectCursorBase<MailAddress>
        {
            private DBCursorField<int> m_MessageID = new DBCursorField<int>("MessageID");
            private DBCursorStringField m_FieldName = new DBCursorStringField("FieldName");
            private DBCursorStringField m_Address = new DBCursorStringField("Address");
            private DBCursorStringField m_DisplayName = new DBCursorStringField("DisplayName");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAddressCursor()
            {
            }

            /// <summary>
            /// MessageID
            /// </summary>
            public DBCursorField<int> MessageID
            {
                get
                {
                    return m_MessageID;
                }
            }

            /// <summary>
            /// FieldName
            /// </summary>
            public DBCursorStringField FieldName
            {
                get
                {
                    return m_FieldName;
                }
            }

            /// <summary>
            /// Address
            /// </summary>
            public DBCursorStringField Address
            {
                get
                {
                    return m_Address;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails.MailAddressies;
            }

            
        }
    }
}