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
        /// Collezione di <see cref="MailAttachment"/>
        /// </summary>
        [Serializable]
        public class MailAttachmentCollection
            : CCollection<MailAttachment>
        {

            [NonSerialized] private MailMessage m_Message;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAttachmentCollection()
            {
                m_Message = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="message"></param>
            protected internal MailAttachmentCollection(MailMessage message) // , ByVal under As System.Net.Mail.AttachmentCollection)
            {
                if (message is null)
                    throw new ArgumentNullException("message");
                this.Load(message);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, MailAttachment value)
            {
                if (m_Message is object)
                    value.SetMessage(m_Message);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, MailAttachment oldValue, MailAttachment newValue)
            {
                if (m_Message is object)
                    newValue.SetMessage(m_Message);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Imposta l'oggetto
            /// </summary>
            /// <param name="message"></param>
            protected internal void SetOwner(MailMessage message)
            {
                m_Message = message;
                if (message is object)
                {
                    foreach (var a in this)
                        a.SetMessage(message);
                }
            }

            /// <summary>
            /// Carica 
            /// </summary>
            protected internal void Load(MailMessage message)
            {
                if (message is null)
                    throw new ArgumentNullException("message");

                this.Clear();
                this.SetOwner(message);
                using (var cursor = new MailAttachmentCursor())
                { 
                    cursor.MessageID.Value = DBUtils.GetID(message, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.ID.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                 
            }
        }
    }
}