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
        /// Collezione di elementi del backup
        /// </summary>
        [Serializable]
        public class CBackupItems 
            : CCollection<CBackupItem>
        {
            [NonSerialized] private CBackup m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBackupItems()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CBackupItems(CBackup owner) : this()
            {
                Load(owner);
            }

            /// <summary>
            /// Restituisce un riferimento al proprietario
            /// </summary>
            public CBackup Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta il proprietario
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void SetOwner(CBackup owner)
            {
                m_Owner = owner;
                if (owner is object)
                {
                    foreach (CBackupItem item in this)
                        item.SetOwner(m_Owner);
                }
            }

            /// <summary>
            /// OnInsertComplete
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsertComplete(int index, CBackupItem value)
            {
                if (m_Owner is object)
                    value.SetOwner(m_Owner);

                base.OnInsertComplete(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CBackupItem oldValue, CBackupItem newValue)
            {
                if (m_Owner is object)
                    newValue.SetOwner(m_Owner);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="owner"></param>
            protected virtual void Load(CBackup owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_Owner = owner;
                string fName = m_Owner.FileName;
                if (FileSystem.FileExists(fName))
                {
                    string text = FileSystem.GetTextFileContents(fName);
                    CCollection<CBackupItem> items = (CCollection<CBackupItem>)DMD.XML.Utils.Serializer.Deserialize(text);
                    foreach (CBackupItem item in items)
                        Add(item);
                }
            }

            /// <summary>
            /// Salva
            /// </summary>
            public void Save()
            {
                if (Owner is null)
                    throw new ArgumentNullException("owner");
                string fName = m_Owner.FileName;
                string text = DMD.XML.Utils.Serializer.Serialize(this);
                FileSystem.SetTextFileContents(fName, text);
            }
        }
    }
}