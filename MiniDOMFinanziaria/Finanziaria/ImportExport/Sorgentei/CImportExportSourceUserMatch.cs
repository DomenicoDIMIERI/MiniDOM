using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Flags]
        public enum CImportExportSourceUserMatcF : int
        {
            None = 0,

            /// <summary>
        /// Se impostato l'utente è disabilitato e le operazioni dal server remoto fatte da questo utente
        /// verranno bloccate
        /// </summary>
            Blocca = 1
        }

        /// <summary>
    /// Rappresenta una corrispondenza tra gli utenti dei due sistemi
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CImportExportSourceUserMatc 
            : IDMDXMLSerializable
        {
            public int RemoteUserID;
            public string RemoteUserName;
            public CImportExportSourceUserMatcF Flags;
            private int m_LocalUserID;
            [NonSerialized] private Sistema.CUser m_LocalUser;

            public CImportExportSourceUserMatc()
            {
                DMDObject.IncreaseCounter(this);
                RemoteUserID = 0;
                RemoteUserName = "";
                m_LocalUserID = 0;
                m_LocalUser = null;
                Flags = CImportExportSourceUserMatcF.None;
            }

            public int LocalUserID
            {
                get
                {
                    return DBUtils.GetID(m_LocalUser, m_LocalUserID);
                }

                set
                {
                    m_LocalUserID = value;
                    m_LocalUser = null;
                }
            }

            public Sistema.CUser LocalUser
            {
                get
                {
                    if (m_LocalUser is null)
                        m_LocalUser = Sistema.Users.GetItemById(m_LocalUserID);
                    return m_LocalUser;
                }

                set
                {
                    m_LocalUser = value;
                    m_LocalUserID = DBUtils.GetID(value);
                }
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "RemoteUserID":
                        {
                            RemoteUserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RemoteUserName":
                        {
                            RemoteUserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LocalUserID":
                        {
                            m_LocalUserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            Flags = (CImportExportSourceUserMatcF)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("RemoteUserID", RemoteUserID);
                writer.WriteAttribute("RemoteUserName", RemoteUserName);
                writer.WriteAttribute("LocalUserID", LocalUserID);
                writer.WriteAttribute("Flags", (int?)Flags);
            }

            ~CImportExportSourceUserMatc()
            {
                DMDObject.DecreaseCounter(this);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }
        }
    }
}