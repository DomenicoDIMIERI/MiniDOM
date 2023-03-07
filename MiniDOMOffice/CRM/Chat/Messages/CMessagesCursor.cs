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
    public partial class Messenger
    {

        /// <summary>
        /// Cursore su <see cref="CMessage"/>
        /// </summary>
        [Serializable]
        public class CMessagesCursor 
            : minidom.Databases.DBObjectCursor<CMessage>
        {

            // Private m_Time As New DBCursorField(Of Date)("Time")
            private DBCursorField<DateTime> m_Time = new DBCursorField<DateTime>("Time");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorStringField m_SourceName = new DBCursorStringField("SourceName");
            private DBCursorStringField m_SourceDescription = new DBCursorStringField("SourceDescription");
            private DBCursorField<int> m_TargetID = new DBCursorField<int>("TargetID");
            private DBCursorStringField m_TargetName = new DBCursorStringField("TargetName");
            private DBCursorStringField m_Message = new DBCursorStringField("Message");
            private DBCursorField<DateTime> m_DeliveryTime = new DBCursorField<DateTime>("DeliveryTime");
            private DBCursorField<DateTime> m_ReadTime = new DBCursorField<DateTime>("ReadTime");
            private DBCursorField<int> m_SourceSession = new DBCursorField<int>("SourceSession");
            private DBCursorField<int> m_TargetSession = new DBCursorField<int>("TargetSession");
            private DBCursorField<int> m_IDStanza = new DBCursorField<int>("IDStanza");
            private DBCursorStringField m_NomeStanza = new DBCursorStringField("Stanza");
            private DBCursorField<StatoMessaggio> m_StatoMessaggio = new DBCursorField<StatoMessaggio>("StatoMessaggio");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMessagesCursor()
            {
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Messenger.Messages;
            }

            /// <summary>
            /// Time
            /// </summary>
            public DBCursorField<DateTime> Time
            {
                get
                {
                    return m_Time;
                }
            }

            /// <summary>
            /// SourceID
            /// </summary>
            public DBCursorField<int> SourceID
            {
                get
                {
                    return m_SourceID;
                }
            }

            /// <summary>
            /// SourceName
            /// </summary>
            public DBCursorStringField SourceName
            {
                get
                {
                    return m_SourceName;
                }
            }

            /// <summary>
            /// SourceDescription
            /// </summary>
            public DBCursorStringField SourceDescription
            {
                get
                {
                    return m_SourceDescription;
                }
            }

            /// <summary>
            /// TargetID
            /// </summary>
            public DBCursorField<int> TargetID
            {
                get
                {
                    return m_TargetID;
                }
            }

            /// <summary>
            /// TargetName
            /// </summary>
            public DBCursorStringField TargetName
            {
                get
                {
                    return m_TargetName;
                }
            }

            /// <summary>
            /// Message
            /// </summary>
            public DBCursorStringField Message
            {
                get
                {
                    return m_Message;
                }
            }

            /// <summary>
            /// DeliveryTime
            /// </summary>
            public DBCursorField<DateTime> DeliveryTime
            {
                get
                {
                    return m_DeliveryTime;
                }
            }

            /// <summary>
            /// ReadTime
            /// </summary>
            public DBCursorField<DateTime> ReadTime
            {
                get
                {
                    return m_ReadTime;
                }
            }

            /// <summary>
            /// SourceSession
            /// </summary>
            public DBCursorField<int> SourceSession
            {
                get
                {
                    return m_SourceSession;
                }
            }

            /// <summary>
            /// TargetSession
            /// </summary>
            public DBCursorField<int> TargetSession
            {
                get
                {
                    return m_TargetSession;
                }
            }

            /// <summary>
            /// IDStanza
            /// </summary>
            public DBCursorField<int> IDStanza
            {
                get
                {
                    return m_IDStanza;
                }
            }

            /// <summary>
            /// NomeStanza
            /// </summary>
            public DBCursorStringField NomeStanza
            {
                get
                {
                    return m_NomeStanza;
                }
            }

            /// <summary>
            /// StatoMessaggio
            /// </summary>
            public DBCursorField<StatoMessaggio> StatoMessaggio
            {
                get
                {
                    return m_StatoMessaggio;
                }
            }

            /// <summary>
            /// Limita il cursore
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var tmpSQL = DBCursorFieldBase.True;
                if (!this.Module.UserCanDoAction("list"))
                {
                    if (Module.UserCanDoAction("list_own"))
                    {
                        var uid = DBUtils.GetID(minidom.Sistema.Users.CurrentUser, 0);
                        tmpSQL *= (Field("TargetID").EQ(uid) + Field("SourceID").EQ(uid));
                    }
                    else 
                        tmpSQL = DBCursorFieldBase.False;
                }

                return tmpSQL;
            }
        }
    }
}