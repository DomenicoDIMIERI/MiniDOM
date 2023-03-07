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
        /// Collezione di allegati
        /// </summary>
        [Serializable]
        public class CAttachmentsCollection 
            : CCollection<CAttachment>
        {
            [NonSerialized] private object m_Owner;
            // Private m_Contesto As Object
            private string m_ContextType;
            private int m_ContextID;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAttachmentsCollection()
            {
                m_Owner = null;
                m_ContextType = DMD.Strings.vbNullString;
                m_ContextID = 0;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CAttachmentsCollection(object owner) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetOwner(owner);
                Initialize();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="contesto"></param>
            public CAttachmentsCollection(object owner, object contesto) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetOwner(owner);
                SetContesto(contesto);
                Initialize();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="contextType"></param>
            /// <param name="contextID"></param>
            public CAttachmentsCollection(object owner, string contextType, int contextID) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetOwner(owner);
                SetContesto(contextType, contextID);
                Initialize();
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CAttachment value)
            {
                var Item = value;
                if (m_Owner is object)
                    Item.SetOwner(m_Owner);
                if (!string.IsNullOrEmpty(m_ContextType))
                {
                    Item.SetIDContesto(m_ContextID);
                    Item.SetTipoContesto(m_ContextType);
                }

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CAttachment oldValue, CAttachment newValue)
            {
                var Item = (CAttachment)newValue;
                if (m_Owner is object)
                    Item.SetOwner(m_Owner);
                if (!string.IsNullOrEmpty(m_ContextType))
                {
                    Item.SetIDContesto(m_ContextID);
                    Item.SetTipoContesto(m_ContextType);
                }

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Imposta il proprietario
            /// </summary>
            /// <param name="value"></param>
            public void SetOwner(object value)
            {
                m_Owner = value;
                if (value is object)
                {
                    foreach (CAttachment a in this)
                        a.SetOwner(value);
                }
            }

            /// <summary>
            /// Imposta il contesto
            /// </summary>
            /// <param name="value"></param>
            public void SetContesto(object value)
            {
                SetContesto(DMD.RunTime.vbTypeName(value), DBUtils.GetID(value, 0));
            }

            /// <summary>
            /// Imposta il contesto
            /// </summary>
            /// <param name="contextType"></param>
            /// <param name="contextID"></param>
            public void SetContesto(string contextType, int contextID)
            {
                m_ContextType = contextType;
                m_ContextID = contextID;
                foreach (CAttachment a in this)
                {
                    a.SetIDContesto(m_ContextID);
                    a.SetTipoContesto(m_ContextType);
                }
            }

            /// <summary>
            /// Aggiunge un documento
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public CAttachment Add(string url)
            {
                var item = new CAttachment();
                int i;
                i = DMD.Strings.InStrRev(url, "/");
                if (i > 0)
                {
                    item.Testo = DMD.Strings.Mid(url, i + 1);
                }
                else
                {
                    item.Testo = url;
                }

                item.URL = url;
                Add(item);
                return item;
            }

            /// <summary>
            /// Inizializza
            /// </summary>
            /// <returns></returns>
            protected bool Initialize()
            {
                Clear();
                if (DBUtils.GetID(this.m_Owner, 0) == 0)
                    return true;

                using (var cursor = new CAttachmentsCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.OwnerID.Value = DBUtils.GetID(m_Owner, 0);
                    cursor.OwnerType.Value = DMD.RunTime.vbTypeName(m_Owner);
                    cursor.Testo.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    if (!string.IsNullOrEmpty(m_ContextType))
                    {
                        cursor.IDContesto.Value = m_ContextID;
                        cursor.TipoContesto.Value = m_ContextType;
                    }

                    while (cursor.Read())
                    {
                        this.Add(cursor.Item);
                    }
                }

                return true;
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ContextType", m_ContextType);
                writer.WriteAttribute("ContextID", m_ContextID);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "ContextType":
                        {
                            m_ContextType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContextID":
                        {
                            m_ContextID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}