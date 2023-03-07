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
using DMD.Net.Mail;
using System.Net.Mail;
using System.IO;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Attachment
        /// </summary>
        [Serializable]
        public class MailAttachment 
            : minidom.Databases.DBObject, ICloneable
        {
            [NonSerialized] private MailApplication m_Application;
            private int m_MessageID;
            [NonSerialized] private MailMessage m_Message;
            private string m_Name;
            private string m_FileName;
            private string m_ContentID;
            private string m_ContentType;
            private string m_ContentDisposition;
            private int m_FileSize;
            private DateTime? m_FileCreationTime;
            private DateTime? m_FileLastEditTime;
            private DateTime? m_FileLastReadTime;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAttachment()
            {
                m_Application = null;
                m_MessageID = 0;
                m_Message = null;
                m_Name = "";
                m_FileName = "";
                m_ContentID = "";
                m_ContentType = "";
                m_ContentDisposition = "";
                m_FileSize = 0;
                m_FileCreationTime = default;
                m_FileLastEditTime = default;
                m_FileLastReadTime = default;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="fileName"></param>
            public MailAttachment(string fileName) 
                : this()
            {
                using (var att = new AttachmentEx(fileName))
                {
                    this.From(att);
                }
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAttachment(Attachment att)
                : this()
            {
                this.From(att);
            }

            /// <summary>
            /// Applicazione
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    return m_Application;
                }
            }

            /// <summary>
            /// Imposta l'applicazione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetApplication(MailApplication value)
            {
                m_Application = value;
            }

            /// <summary>
            /// Data di creazione
            /// </summary>
            public DateTime? FileCreationTime
            {
                get
                {
                    return m_FileCreationTime;
                }

                set
                {
                    var oldValue = m_FileCreationTime;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_FileCreationTime = value;
                    DoChanged("FileCreationTime", value, oldValue);
                }
            }

            /// <summary>
            /// Data dell'ultima modifica
            /// </summary>
            public DateTime? FileLastEditTime
            {
                get
                {
                    return m_FileLastEditTime;
                }

                set
                {
                    var oldValue = m_FileLastEditTime;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_FileLastEditTime = value;
                    DoChanged("FileLastEditTime", value, oldValue);
                }
            }

            /// <summary>
            /// Data dell'ultimo accesso
            /// </summary>
            public DateTime? FileLastReadTime
            {
                get
                {
                    return m_FileLastReadTime;
                }

                set
                {
                    var oldValue = m_FileLastReadTime;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_FileLastReadTime = value;
                    DoChanged("FileLastReadTime", value, oldValue);
                }
            }
             
            /// <summary>
            /// Dimensione dell'allegato in bytes
            /// </summary>
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
            /// Nome del file allegato
            /// </summary>
            public string FileName
            {
                get
                {
                    return m_FileName;
                }

                set
                {
                    string oldValue = m_FileName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FileName = value;
                    DoChanged("FileName", value, oldValue);
                }
            }

            /// <summary>
            /// Disposition
            /// </summary>
            public string ContentDisposition
            {
                get
                {
                    return m_ContentDisposition;
                }

                set
                {
                    string oldValue = m_ContentDisposition;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContentDisposition = value;
                    DoChanged("ContentDisposition", value, oldValue);
                }
            }

            /// <summary>
            /// Content ID
            /// </summary>
            public string ContentId
            {
                get
                {
                    return m_ContentID;
                }

                set
                {
                    string oldValue = m_ContentID;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContentID = value;
                    DoChanged("ContentId", value, oldValue);
                }
            }

            /// <summary>
            /// Content Type
            /// </summary>
            public string ContentType
            {
                get
                {
                    return m_ContentType;
                }

                set
                {
                    string oldValue = m_ContentType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ContentType = value;
                    DoChanged("ContentType", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'allegato
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    string oldValue = m_Name;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// ID del messaggio
            /// </summary>
            public int MessageID
            {
                get
                {
                    return DBUtils.GetID(m_Message, m_MessageID);
                }

                set
                {
                    int oldValue = MessageID;
                    if (oldValue == value)
                        return;
                    m_MessageID = value;
                    m_Message = null;
                    DoChanged("MessageID", value, oldValue);
                }
            }

            /// <summary>
            /// Messaggio
            /// </summary>
            public MailMessage Message
            {
                get
                {
                    if (m_Message is null)
                        m_Message = minidom.Office.Mails.GetItemById(m_MessageID);
                    return m_Message;
                }

                internal set
                {
                    var oldValue = Message;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Message = value;
                    m_MessageID = DBUtils.GetID(value, 0);
                    DoChanged("Message", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il messaggio
            /// </summary>
            /// <param name="message"></param>
            protected internal virtual void SetMessage(MailMessage message)
            {
                m_Message = message;
                m_MessageID = DBUtils.GetID(message, 0);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_FileName;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Name);
            }



            // Protected Sub SaveContentTo(ByVal fileName As String)
            // Dim stream As New System.IO.FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)
            // Dim buffer() As Byte
            // ReDim buffer(1024 - 1)
            // Me.ContentStream.Position = 0
            // While Me.ContentStream.Position < Me.ContentStream.Length
            // Me.ContentStream.Read(buffer, 0, 1024)
            // stream.Write(buffer, 0, 1024)
            // 'Me.ContentStream.Position += 1024
            // End While
            // stream.Dispose()
            // End Sub

            // Protected Sub LoadContentFrom(ByVal fileName As String, ByVal name As String, ByVal mediaType As String)
            // Dim stream As New System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            // Dim outStream As New System.IO.MemoryStream(1024)
            // Dim buffer() As Byte
            // ReDim buffer(1024 - 1)
            // While stream.Position < stream.Length
            // stream.Read(buffer, 0, 1024)
            // outStream.Write(buffer, 0, 1024)
            // stream.Position += 1024
            // End While
            // stream.Dispose()
            // Me.BaseObject = New System.Net.Mail.Attachment(outStream, name, mediaType)
            // outStream.Dispose()
            // End Sub


            // ' This code added by Visual Basic to correctly implement the disposable pattern.
            // Public Overridable Sub Dispose() Implements IDisposable.Dispose
            // If (Me.BaseObject IsNot Nothing) Then Me.BaseObject.Dispose() : Me.BaseObject = Nothing
            // Me.m_Message = Nothing
            // Me.m_Name = vbNullString
            // Me.m_FileName = vbNullString
            // Me.m_ContentID = vbNullString
            // Me.m_NameEncoding = Nothing
            // Me.m_TransferEncoding = Nothing
            // End Sub


            // Private Function GetStr(ByVal enc As System.Text.Encoding) As String
            // If (enc Is Nothing) Then Return ""
            // Return enc.WebName
            // End Function

            // Private Function GetEncoding(ByVal str As String) As System.Text.Encoding
            // str = Trim(str)
            // If (str = "") Then Return Nothing
            // Return System.Text.Encoding.GetEncoding(str)
            // End Function

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_eMailAttachments";
            }

            // Private Function parseTransferEncoding(ByVal str As String) As System.Net.Mime.TransferEncoding
            // Try
            // Return [Enum].Parse(GetType(System.Net.Mime.TransferEncoding), str)
            // Catch ex As Exception
            // Return System.Net.Mime.TransferEncoding.Unknown
            // End Try
            // End Function

            // Private Function formatTransferEncoding(ByVal value As System.Net.Mime.TransferEncoding) As String
            // Try
            // Return [Enum].GetName(GetType(System.Net.Mime.TransferEncoding), value)
            // Catch ex As Exception
            // Return ""
            // End Try
            // End Function

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_MessageID = reader.Read("MessageID",  m_MessageID);
                m_Name = reader.Read("Name",  m_Name);
                m_FileName = reader.Read("FileName",  m_FileName);
                m_ContentID = reader.Read("ContentId",  m_ContentID);
                m_ContentType = reader.Read("ContentType",  m_ContentType);
                m_ContentDisposition = reader.Read("ContentDisposition",  m_ContentDisposition);
                m_FileSize = reader.Read("FileSize",  m_FileSize);
                m_FileCreationTime = reader.Read("FileCreationTime",  m_FileCreationTime);
                m_FileLastEditTime = reader.Read("FileLastEditTime",  m_FileLastEditTime);
                m_FileLastReadTime = reader.Read("FileLastReadTime",  m_FileLastReadTime);
                
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("MessageID", MessageID);
                writer.Write("Name", m_Name);
                writer.Write("FileName", m_FileName);
                writer.Write("ContentId", m_ContentID);
                writer.Write("ContentType", m_ContentType);
                writer.Write("ContentDisposition", m_ContentDisposition);
                writer.Write("FileSize", m_FileSize);
                writer.Write("FileCreationTime", m_FileCreationTime);
                writer.Write("FileLastEditTime", m_FileLastEditTime);
                writer.Write("FileLastReadTime", m_FileLastReadTime);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("MessageID", typeof(int) , 1);
                c = table.Fields.Ensure("Name", typeof(string), 255);
                c = table.Fields.Ensure("FileName", typeof(string), 255);
                c = table.Fields.Ensure("ContentId", typeof(string), 255);
                c = table.Fields.Ensure("ContentType", typeof(string), 255);
                c = table.Fields.Ensure("ContentDisposition", typeof(string), 255);
                c = table.Fields.Ensure("FileSize", typeof(int), 1);
                c = table.Fields.Ensure("FileCreationTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("FileLastEditTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("FileLastReadTime", typeof(DateTime), 1);
                 

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxMessage", new string[] { "MessageID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxName", new string[] { "Name", "FileName" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContext", new string[] { "ContentId", "ContentType", "ContentDisposition" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFile", new string[] { "FileSize", "FileCreationTime", "FileLastEditTime", "FileLastReadTime" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("MessageID", MessageID);
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("FileName", m_FileName);
                writer.WriteAttribute("ContentId", m_ContentID);
                writer.WriteAttribute("ContentType", m_ContentType);
                writer.WriteAttribute("ContentDisposition", m_ContentDisposition);
                writer.WriteAttribute("FileSize", m_FileSize);
                writer.WriteAttribute("FileCreationTime", m_FileCreationTime);
                writer.WriteAttribute("FileLastEditTime", m_FileLastEditTime);
                writer.WriteAttribute("FileLastReadTime", m_FileLastReadTime);
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
                    case "MessageID":
                        {
                            MessageID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FileName":
                        {
                            m_FileName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContentId":
                        {
                            m_ContentID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContentType":
                        {
                            m_ContentType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ContentDisposition":
                        {
                            m_ContentDisposition = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FileSize":
                        {
                            m_FileSize = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "FileCreationTime":
                        {
                            m_FileCreationTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "FileLastEditTime":
                        {
                            m_FileLastEditTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "FileLastReadTime":
                        {
                            m_FileLastReadTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

              
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Attachments;
            }

          

            private string GetFreeFile()
            {
                lock (Sistema.ApplicationContext)
                {
                    m_FileName = @"mail\attachments\malatt" + DMD.Strings.GetRandomString(16) + ".dat";
                    string fileName = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, m_FileName);
                    while (System.IO.File.Exists(fileName))
                        m_FileName = @"mail\attachments\malatt" + DMD.Strings.GetRandomString(16) + ".dat";
                    System.IO.File.WriteAllText(fileName, "");
                    return m_FileName;
                }
            }

            /// <summary>
            /// Inizializza l'oggetto dall'allegato
            /// </summary>
            /// <param name="at"></param>
            internal void From(Attachment at)
            {
                if (at is null)
                    throw new ArgumentNullException("at", "Oggetto nullo");

                if (at.ContentDisposition is object)
                {
                    m_Name = at.ContentDisposition.FileName;
                    m_FileSize = (int)at.ContentDisposition.Size;
                    m_ContentDisposition = at.ContentDisposition.DispositionType;
                }

                if (string.IsNullOrEmpty(m_Name))
                    m_Name = at.Name;
                
                m_ContentID = at.ContentId;
                m_ContentType = at.ContentType.ToString();

                var path = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, @"mail\attachments");
                Sistema.FileSystem.CreateRecursiveFolder(path);
                if (string.IsNullOrEmpty(m_FileName))
                    m_FileName = GetFreeFile();

                string fileName = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, m_FileName);
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    at.ContentStream.Position = 0L;
                    Sistema.FileSystem.CopyStream(at.ContentStream, stream);
                }
            }

            /// <summary>
            /// Clona l'allegato
            /// </summary>
            /// <returns></returns>
            public MailAttachment Clone()
            {
                return (MailAttachment)this._Clone();
            }

            // #Region "Static Methods"

            // Public Shared Function CreateAttachmentFromString(content As String, name As String) As MailAttachment
            // Dim tmp As System.Net.Mail.Attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(content, name)
            // Return New MailAttachment(tmp)
            // End Function

            // Public Shared Function CreateAttachmentFromString(content As String, name As String, contentEncoding As System.Text.Encoding, mediaType As String) As MailAttachment
            // Dim tmp As System.Net.Mail.Attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(content, name, contentEncoding, mediaType)
            // Return New MailAttachment(tmp)
            // End Function

            // Public Shared Function CreateAttachmentFromString(content As String, contentType As System.Net.Mime.ContentType) As MailAttachment
            // Dim tmp As System.Net.Mail.Attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(content, contentType)
            // Return New MailAttachment(tmp)
            // End Function

            // Protected Overrides Sub Finalize()
            // MyBase.Finalize()
            // ' DMDObject.DecreaseCounter(Me)
            // End Sub

            // #End Region


            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is MailAttachment) && this.Equals((MailAttachment)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MailAttachment obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_MessageID, obj.m_MessageID)
                    && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                    && DMD.Strings.EQ(this.m_FileName, obj.m_FileName)
                    && DMD.Strings.EQ(this.m_ContentID, obj.m_ContentID)
                    && DMD.Strings.EQ(this.m_ContentType, obj.m_ContentType)
                    && DMD.Strings.EQ(this.m_ContentDisposition, obj.m_ContentDisposition)
                    && DMD.Integers.EQ(this.m_FileSize, obj.m_FileSize)
                    && DMD.DateUtils.EQ(this.m_FileCreationTime, obj.m_FileCreationTime)
                    && DMD.DateUtils.EQ(this.m_FileLastEditTime, obj.m_FileLastEditTime)
                    && DMD.DateUtils.EQ(this.m_FileLastReadTime, obj.m_FileLastReadTime)
                    ;
            }
        }
    }
}