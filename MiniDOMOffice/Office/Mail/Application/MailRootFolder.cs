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
        /// Root folder di un oggetto <see cref="MailApplication"/>
        /// </summary>
        [Serializable]
        public class MailRootFolder
            : MailFolder
        {
            [NonSerialized] private MailFolder m_Inbox;
            [NonSerialized] private MailFolder m_TrashBin;
            [NonSerialized] private MailFolder m_Drafts;
            [NonSerialized] private MailFolder m_Sent;
            [NonSerialized] private MailFolder m_Spam;
            [NonSerialized] private MailFolder m_Archive;
            [NonSerialized] private MailFolder m_FindFolder;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailRootFolder()
            {
                m_Inbox = null;
                m_TrashBin = null;
                m_Drafts = null;
                m_Sent = null;
                m_Spam = null;
                m_Archive = null;
                m_FindFolder = null;
            }

            /// <summary>
            /// Cartella archivio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Archive
            {
                get
                {
                    if (m_Archive is null)
                        m_Archive = Childs.GetItemByName("/archive");
                    if (m_Archive is null)
                    {
                        m_Archive = Childs.Add("archive");
                        m_Archive.Stato = ObjectStatus.OBJECT_VALID;
                        m_Archive.Save();
                    }

                    return m_Archive;
                }
            }

            /// <summary>
            /// Cartelle ricerche
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder FindFolder
            {
                get
                {
                    if (m_FindFolder is null)
                        m_FindFolder = Childs.GetItemByName("/findfolder");
                    if (m_FindFolder is null)
                    {
                        m_FindFolder = Childs.Add("findfolder");
                        m_FindFolder.Stato = ObjectStatus.OBJECT_VALID;
                        m_FindFolder.Save();
                    }

                    return m_FindFolder;
                }
            }

            /// <summary>
            /// Cartella principale della posta in arrivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Inbox
            {
                get
                {
                    if (m_Inbox is null)
                        m_Inbox = Childs.GetItemByName("/inbox");
                    if (m_Inbox is null)
                    {
                        m_Inbox = Childs.Add("inbox");
                        m_Inbox.Stato = ObjectStatus.OBJECT_VALID;
                        m_Inbox.Save();
                    }

                    return m_Inbox;
                }
            }

            /// <summary>
            /// Cartella principale del cestino
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder TrashBin
            {
                get
                {
                    if (m_TrashBin is null)
                        m_TrashBin = Childs.GetItemByName("/recycler");
                    if (m_TrashBin is null)
                    {
                        m_TrashBin = Childs.Add("recycler");
                        m_TrashBin.Stato = ObjectStatus.OBJECT_VALID;
                        m_TrashBin.Save();
                    }

                    return m_TrashBin;
                }
            }

            /// <summary>
            /// Cartella delle bozze
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Drafts
            {
                get
                {
                    if (m_Drafts is null)
                        m_Drafts = Childs.GetItemByName("/drafts");
                    if (m_Drafts is null)
                    {
                        m_Drafts = Childs.Add("drafts");
                        m_Drafts.Stato = ObjectStatus.OBJECT_VALID;
                        m_Drafts.Save();
                    }

                    return m_Drafts;
                }
            }

            /// <summary>
            /// Cartella delle email inviate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Sent
            {
                get
                {
                    if (m_Sent is null)
                        m_Sent = Childs.GetItemByName("/sent");
                    if (m_Sent is null)
                    {
                        m_Sent = Childs.Add("sent");
                        m_Sent.Stato = ObjectStatus.OBJECT_VALID;
                        m_Sent.Save();
                    }

                    return m_Sent;
                }
            }

            /// <summary>
            /// Cartella delle email indesiderate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Spam
            {
                get
                {
                    if (m_Spam is null)
                        m_Spam = Childs.GetItemByName("/spam");
                    if (m_Spam is null)
                    {
                        m_Spam = Childs.Add("spam");
                        m_Spam.Stato = ObjectStatus.OBJECT_VALID;
                        m_Spam.Save();
                    }

                    return m_Spam;
                }
            }

            /// <summary>
            /// Aggiorna la cartella
            /// </summary>
            /// <param name="folder"></param>
            /// <returns></returns>
            protected internal override MailFolder UpdateFolder(MailFolder folder)
            {
                if (folder is null)
                    throw new ArgumentNullException("folder");
                if (DBUtils.GetID(folder, 0) == 0)
                    return null;

                if (DBUtils.GetID(folder, 0) == DBUtils.GetID(Inbox, 0))
                {
                    m_Inbox = folder;
                }
                else if (DBUtils.GetID(folder, 0) == DBUtils.GetID(m_Archive, 0))
                {
                    m_Archive = folder;
                }
                else if (DBUtils.GetID(folder, 0) == DBUtils.GetID(m_TrashBin, 0))
                {
                    m_TrashBin = folder;
                }
                else if (DBUtils.GetID(folder, 0) == DBUtils.GetID(m_Drafts, 0))
                {
                    m_Drafts = folder;
                }
                else if (DBUtils.GetID(folder, 0) == DBUtils.GetID(m_Sent, 0))
                {
                    m_Sent = folder;
                }
                else if (DBUtils.GetID(folder, 0) == DBUtils.GetID(m_Spam, 0))
                {
                    m_Spam = folder;
                }
                else if (DBUtils.GetID(folder, 0) == DBUtils.GetID(m_FindFolder, 0))
                {
                    m_FindFolder = folder;
                }

                return base.UpdateFolder(folder);
            }
        }
    }
}