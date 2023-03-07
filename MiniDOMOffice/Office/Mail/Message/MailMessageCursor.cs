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
        /// Cursore di <see cref="MailMessage"/>
        /// </summary>
        [Serializable]
        public class MailMessageCursor 
            : Databases.DBObjectCursorPO<MailMessage>
        {
            private DBCursorField<int> m_ApplicationID = new DBCursorField<int>("ApplicationID");
            private DBCursorField<int> m_FolderID = new DBCursorField<int>("FolderID");
            private DBCursorField<int> m_AccountID = new DBCursorField<int>("AccountID");
            private DBCursorStringField m_Bcc = new DBCursorStringField("Bcc");
            private DBCursorStringField m_Cc = new DBCursorStringField("Cc");
            private DBCursorStringField m_From = new DBCursorStringField("From");
            private DBCursorStringField m_DeliveredTo = new DBCursorStringField("DeliveredTo");
            private DBCursorStringField m_ReplyTo = new DBCursorStringField("ReplayTo");
            private DBCursorStringField m_Subject = new DBCursorStringField("Subject");
            private DBCursorStringField m_To = new DBCursorStringField("To");
            private DBCursorStringField m_Body = new DBCursorStringField("Body");
            private DBCursorField<bool> m_IsBodyHtml = new DBCursorField<bool>("BodyHtml");
            private DBCursorField<DateTime> m_DeliveryDate = new DBCursorField<DateTime>("DeliveryDate");
            private DBCursorField<DateTime> m_DownloadDate = new DBCursorField<DateTime>("DownloadDate");
            private DBCursorField<DateTime> m_ReadDate = new DBCursorField<DateTime>("ReadDate");
            private DBCursorField<MailFlags> m_Flags = new DBCursorField<MailFlags>("Falgs");
            private DBCursorStringField m_MessageID = new DBCursorStringField("MessageId");
            private bool m_OnlyHeaders = false;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailMessageCursor()
            {
            }

            /// <summary>
            /// Se vero il cursore non carica l'intero messaggio, ma solo gli headers
            /// </summary>
            public bool OnlyHeaders
            {
                get
                {
                    return m_OnlyHeaders;
                }

                set
                {
                    m_OnlyHeaders = value;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<MailFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// DeliveredTo
            /// </summary>
            public DBCursorStringField DeliveredTo
            {
                get
                {
                    return m_DeliveredTo;
                }
            }

            /// <summary>
            /// MessageID
            /// </summary>
            public DBCursorStringField MessageID
            {
                get
                {
                    return m_MessageID;
                }
            }

            /// <summary>
            /// AccountID
            /// </summary>
            public DBCursorField<int> AccountID
            {
                get
                {
                    return m_AccountID;
                }
            }

            /// <summary>
            /// ApplicationID
            /// </summary>
            public DBCursorField<int> ApplicationID
            {
                get
                {
                    return m_ApplicationID;
                }
            }

            /// <summary>
            /// FolderID
            /// </summary>
            public DBCursorField<int> FolderID
            {
                get
                {
                    return m_FolderID;
                }
            }

            /// <summary>
            /// Bcc
            /// </summary>
            public DBCursorStringField Bcc
            {
                get
                {
                    return m_Bcc;
                }
            }

            /// <summary>
            /// Cc
            /// </summary>
            public DBCursorStringField Cc
            {
                get
                {
                    return m_Cc;
                }
            }

            /// <summary>
            /// From
            /// </summary>
            public DBCursorStringField From
            {
                get
                {
                    return m_From;
                }
            }

            /// <summary>
            /// ReplyTo
            /// </summary>
            public DBCursorStringField ReplyTo
            {
                get
                {
                    return m_ReplyTo;
                }
            }

            /// <summary>
            /// Subject
            /// </summary>
            public DBCursorStringField Subject
            {
                get
                {
                    return m_Subject;
                }
            }

            /// <summary>
            /// To
            /// </summary>
            public DBCursorStringField To
            {
                get
                {
                    return m_To;
                }
            }

            /// <summary>
            /// Body
            /// </summary>
            public DBCursorStringField Body
            {
                get
                {
                    return m_Body;
                }
            }

            /// <summary>
            /// IsBodyHtml
            /// </summary>
            public DBCursorField<bool> IsBodyHtml
            {
                get
                {
                    return m_IsBodyHtml;
                }
            }

            /// <summary>
            /// DeliveryDate
            /// </summary>
            public DBCursorField<DateTime> DeliveryDate
            {
                get
                {
                    return m_DeliveryDate;
                }
            }

            /// <summary>
            /// ReadDate
            /// </summary>
            public DBCursorField<DateTime> ReadDate
            {
                get
                {
                    return m_ReadDate;
                }
            }

            /// <summary>
            /// DownloadDate
            /// </summary>
            public DBCursorField<DateTime> DownloadDate
            {
                get
                {
                    return m_DownloadDate;
                }
            }
             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails;
            } 


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OnlyHeaders", m_OnlyHeaders);
                writer.SetSetting("OnlyHeaders", m_OnlyHeaders);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OnlyHeaders": this.m_OnlyHeaders = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true; break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            /// <summary>
            /// Sincronizza gli oggetti dei messaggi nella pagina corrente del cursore
            /// </summary>
            protected override void SyncPage()
            {
                base.SyncPage();
                var msgsid = DMD.Arrays.Empty<int>();
                var items = GetItemsArray();
                foreach (MailMessage m in (IEnumerable)items)
                {
                    if (m is object && m.m_Attachements is null)
                    {
                        msgsid = DMD.Arrays.InsertSorted(msgsid, DBUtils.GetID(m, 0));
                    }
                }

                if (msgsid.Length > 0)
                {
                    var col = new CCollection<MailAttachment>();

                    using (var cursor = new MailAttachmentCursor())
                    {
                        cursor.MessageID.ValueIn(msgsid);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        cursor.ID.SortOrder = SortEnum.SORT_ASC;
                        while (cursor.Read())
                        {
                            col.Add(cursor.Item);
                        }

                    }

                    foreach (var o in (IEnumerable)items)
                    {
                        var m = o as MailMessage;
                        if (m is object && m.m_Attachements is null)
                        {
                            var atts = new MailAttachmentCollection();
                            atts.SetOwner(m);
                            foreach (MailAttachment a in col)
                            {
                                if (a.MessageID == DBUtils.GetID(m, 0))
                                {
                                    atts.Add(a);
                                }
                            }

                            m.m_Attachements = atts;
                        }
                    }
                }


                // -----------------
                msgsid = DMD.Arrays.Empty<int>();
                foreach (MailMessage m in (IEnumerable)items)
                {
                    if (m is object && m.m_OriginalAddresses is null)
                    {
                        msgsid = DMD.Arrays.InsertSorted(msgsid, DBUtils.GetID(m, 0));
                    }
                }

                if (msgsid.Length > 0)
                {
                    var col1 = new CCollection<MailAddress>();
                    using (var cursor1 = new MailAddressCursor())
                    {
                        cursor1.IgnoreRights = true;
                        cursor1.MessageID.ValueIn(msgsid);
                        cursor1.ID.SortOrder = SortEnum.SORT_ASC;
                        while (cursor1.Read())
                        {
                            var add = cursor1.Item;
                            col1.Add(add);
                        }

                    }

                    foreach (MailMessage m in (IEnumerable)items)
                    {
                        if (m is object && m.m_OriginalAddresses is null)
                        {
                            var lst = new List<MailAddress>();
                            foreach (MailAddress add in col1)
                            {
                                if (add.MessageID == DBUtils.GetID(m, 0))
                                {
                                    add.SetApplication(m.Application);
                                    add.SetMessage(m);
                                    lst.Add(add);
                                }
                            }

                            m.SetOriginalAddressList(lst);
                        }
                    }
                }
            }
        }
    }
}