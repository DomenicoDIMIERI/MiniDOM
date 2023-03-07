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
        /// Collezione di <see cref="MailFolder"/>
        /// </summary>
        [Serializable]
        public class MailFoldersCollection
            : CCollection<MailFolder>
        {
            [NonSerialized] private MailApplication m_Application;
            [NonSerialized] private CUser m_User;
            [NonSerialized] private MailFolder m_Parent;


            /// <summary>
            /// Costruttore
            /// </summary>
            public MailFoldersCollection()
            {
                m_Application = null;
                m_User = null;
                m_Parent = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="parent"></param>
            public MailFoldersCollection(MailFolder parent) : this()
            {
                if (parent is null)
                    throw new ArgumentNullException("parent");
                Load(parent);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="user"></param>
            public MailFoldersCollection(Sistema.CUser user) : this()
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                Load(user);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, MailFolder value)
            {
                if (m_Application is object)
                    value.SetApplication(m_Application);
                if (m_User is object)
                    value.SetUtente(m_User);
                if (m_Parent is object)
                    value.SetParent(m_Parent);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, MailFolder oldValue, MailFolder newValue)
            {
                if (m_Application is object)
                    newValue.SetApplication(m_Application);
                if (m_User is object)
                    newValue.SetUtente(m_User);
                if (m_Parent is object)
                    newValue.SetParent(m_Parent);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica 
            /// </summary>
            /// <param name="parent"></param>
            protected internal void Load(MailFolder parent)
            {
                this.m_Parent = parent;
                this.m_User = parent.Utente;
                this.SetApplication(parent.Application);
                this.Clear();
                using (var cursor = new MailFolderCursor())
                {
                    cursor.ParentID.Value = DBUtils.GetID(m_Parent, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.ID.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// Carica
            /// </summary>
            /// <param name="user"></param>
            protected internal void Load(CUser user)
            {
                this.Clear();
                this.m_User = user;
                using (var cursor = new MailFolderCursor())
                { 
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.ParentID.Value = 0;
                    cursor.ParentID.IncludeNulls = true;
                    cursor.IDutente.Value = DBUtils.GetID(user, 0);
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// Restituisce la sottocartella con nome
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public MailFolder GetItemByName(string path)
            {
                path = Strings.Replace(Strings.Trim(path), "/", @"\");
                if (string.IsNullOrEmpty(path))
                    return null;
                int p = Strings.InStr(path, @"\");
                string name = "";
                if (p > 0)
                {
                    name = Strings.Trim(Strings.Left(path, p - 1));
                    path = Strings.Trim(Strings.Mid(path, p + 1));
                }
                else
                {
                    name = path;
                }

                if (string.IsNullOrEmpty(name))
                    return m_Application.Root.Childs.GetItemByName(path);
                if (name == "..")
                {
                    if (m_Parent is null)
                        return null;
                    return m_Parent.Childs.GetItemByName(path);
                }

                MailFolder ret = null;
                foreach (MailFolder f in this)
                {
                    if ((f.Name ?? "") == (name ?? ""))
                        return f;
                }

                if (ret is null)
                    return null;
                if (string.IsNullOrEmpty(path))
                    return ret;
                return ret.Childs.GetItemByName(path);
            }

            /// <summary>
            /// Aggiunge una nuova cartella con nome
            /// </summary>
            /// <param name="folderName"></param>
            /// <returns></returns>
            public MailFolder Add(string folderName)
            {
                folderName = DMD.Strings.Trim(folderName);
                if (folderName.IndexOf("/") >= 0)
                    throw new ArgumentException("Il percorso non può contenere caratteri speciali");
                var f = new MailFolder();
                f.Name = folderName;
                f.Stato = ObjectStatus.OBJECT_VALID;
                Add(f);
                f.Save();
                return f;
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="user"></param>
            protected internal void SetUser(Sistema.CUser user)
            {
                m_User = user;
                if (user is object)
                {
                    foreach (MailFolder f in this)
                        f.SetUtente(user);
                }
            }

            /// <summary>
            /// Imposta il genitore
            /// </summary>
            /// <param name="p"></param>
            protected internal void SetParent(MailFolder p)
            {
                m_Parent = p;
                if (p is object)
                {
                    foreach (MailFolder f in this)
                        f.SetParent(p);
                }
            }

            /// <summary>
            /// Imposta l'app
            /// </summary>
            /// <param name="a"></param>
            protected internal void SetApplication(MailApplication a)
            {
                m_Application = a;
                if (a is object)
                {
                    foreach (MailFolder f in this)
                        f.SetApplication(a);
                }
            }
        }
    }
}