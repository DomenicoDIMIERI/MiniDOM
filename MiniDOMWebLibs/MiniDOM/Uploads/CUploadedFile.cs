using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
        /// Rappresenta le informazioni registrate nel DB relativamente ad un upload
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUploadedFile : Databases.DBObjectBase
        {
            private string m_Key;
            private int m_UserID;
            [NonSerialized]   private Sistema.CUser m_User;
            private string m_SourceFile;
            private string m_TargetURL;
            private DateTime m_UploadTime;
            private int m_FileSize;
            private byte[] m_FileContent;

            public CUploadedFile()
            {
                m_Key = "";
                m_User = Sistema.Users.CurrentUser;
                m_UserID = Databases.GetID(m_User);
                m_SourceFile = "";
                m_TargetURL = "";
                m_UploadTime = default;
                m_FileSize = 0;
                m_FileContent = null;
            }

            public override CModulesClass GetModule()
            {
                return Uploads.Module;
            }

            /// <summary>
        /// Restituisce o imposta la chiave che identifica univocamente l'upload
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Key
            {
                get
                {
                    return m_Key;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Key;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Key = value;
                    DoChanged("Key", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha caricato il file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int UserID
            {
                get
                {
                    return Databases.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_UserID = value;
                    m_User = null;
                    DoChanged("UserID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha caricato il file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    if (ReferenceEquals(m_User, value))
                        return;
                    m_User = value;
                    m_UserID = Databases.GetID(value);
                    DoChanged("User", value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il percorso sul PC remoto da cui è stato caricato il file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string SourceFile
            {
                get
                {
                    return m_SourceFile;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SourceFile;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_SourceFile = value;
                    DoChanged("SourceFile", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la URL da cui è possibile scaricare il file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Questo campo è vuoto se si tratta di un Upload effettuato direttamente nel DB. In tal caso fare riferimento al campo FileContent</remarks>
            public string TargetURL
            {
                get
                {
                    return m_TargetURL;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TargetURL;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_TargetURL = value;
                    DoChanged("TargetURL", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data e l'ora del completamento dell'upload
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime UploadTime
            {
                get
                {
                    return m_UploadTime;
                }

                set
                {
                    var oldValue = m_UploadTime;
                    if (oldValue == value)
                        return;
                    m_UploadTime = value;
                    DoChanged("UploadTime", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta le dimensioni in bytes del file caricato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int FileSize
            {
                get
                {
                    return m_FileSize;
                }

                set
                {
                    int oldValue = m_FileSize;
                    if (oldValue == value)
                        return;
                    m_FileSize = value;
                    DoChanged("FileSize", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il contenuto del file se questo è stato salvato nel database. In tal caso il camppo TargetURL è nullo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public byte[] FileContent
            {
                get
                {
                    return m_FileContent;
                }

                set
                {
                    m_FileContent = value;
                    DoChanged("FileContent", value);
                }
            }

            public override string ToString()
            {
                return m_TargetURL;
            }

            public override string GetTableName()
            {
                return "tbl_Uploads";
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                m_Key = reader.Read("Key", this. m_Key);
                m_UserID = reader.Read("UserID", this.m_UserID);
                m_SourceFile = reader.Read("SourceFile", this.m_SourceFile);
                m_TargetURL = reader.Read("TargetURL", this.m_TargetURL);
                m_UploadTime = reader.Read("UploadTime", this.m_UploadTime);
                m_FileSize = reader.Read("FileSize", this.m_FileSize);
                m_FileContent = reader.Read("FileContent", this.m_FileContent);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("Key", m_Key);
                writer.Write("UserID", UserID);
                writer.Write("SourceFile", m_SourceFile);
                writer.Write("TargetURL", m_TargetURL);
                writer.Write("UploadTime", m_UploadTime);
                writer.Write("FileSize", m_FileSize);
                writer.Write("FileContent", m_FileContent);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Key", m_Key);
                writer.WriteAttribute("UserID", UserID);
                writer.WriteAttribute("SourceFile", m_SourceFile);
                writer.WriteAttribute("TargetURL", m_TargetURL);
                writer.WriteAttribute("UploadTime", m_UploadTime);
                writer.WriteAttribute("FileSize", m_FileSize);
                base.XMLSerialize(writer);
                writer.WriteTag("FileContent", m_FileContent);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "Key", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Key = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "UserID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "SourceFile", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_SourceFile = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "TargetURL", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_TargetURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "UploadTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_UploadTime = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "FileSize", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_FileSize = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "FileContent", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_FileContent = (byte[])fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }
        }
    }
}