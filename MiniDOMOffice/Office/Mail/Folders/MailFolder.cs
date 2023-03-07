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
        /// Folder
        /// </summary>
        [Serializable]
        public class MailFolder 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<MailFolder>
        {
            private string m_Name;
            private int m_ParentID;
            [NonSerialized] private MailFolder m_Parent;
            [NonSerialized] private MailFolderChilds m_Childs;
            private int m_TotalMessages;
            private int m_TotalUnread;
            [NonSerialized] private CUser m_Utente;
            private int m_IDUtente;
            private int m_ApplicationID;
            [NonSerialized] private MailApplication m_Application;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailFolder()
            {
                m_Name = "";
                m_ParentID = 0;
                m_Parent = null;
                m_Childs = null;
                m_TotalMessages = 0;
                m_TotalUnread = 0;
                m_Flags = 0;
                m_ApplicationID = 0;
                m_Application = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="name"></param>
            public MailFolder(string name) 
                : this()
            {
                m_Name = DMD.Strings.Trim(name);
            }

            /// <summary>
            /// ID dell'applicazione
            /// </summary>
            public int ApplicationID
            {
                get
                {
                    return DBUtils.GetID(m_Application, m_ApplicationID);
                }

                set
                {
                    int oldValue = ApplicationID;
                    if (oldValue == value)
                        return;
                    m_ApplicationID = value;
                    m_Application = null;
                    DoChanged("ApplicationID", value, oldValue);
                }
            }

            /// <summary>
            /// Applicazione
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    return this.GetApplication();
                }

                set
                {
                    var oldValue = Application;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Application = value;
                    m_ApplicationID = DBUtils.GetID(value, 0);
                    DoChanged("Application", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce un riferimento all'app
            /// </summary>
            /// <returns></returns>
            protected virtual MailApplication GetApplication()
            {
                if (this.m_Parent is object) return this.m_Parent.Application;

                if (this.m_Application is null)
                    this.m_Application = Mails.Applications.GetItemById(m_ApplicationID);

                return this.m_Application;
            }

            /// <summary>
            /// Imposta l'app
            /// </summary>
            /// <param name="app"></param>
            protected internal void SetApplication(MailApplication app)
            {
                m_Application = app;
                m_ApplicationID = DBUtils.GetID(app, 0);
            }

            /// <summary>
            /// Restituisce o imposta il proprietario della cartella
            /// </summary>
            /// <returns></returns>
            public Sistema.CUser Utente
            {
                get
                {
                    if (m_Utente is null)
                        m_Utente = Sistema.Users.GetItemById(m_IDUtente);
                    return m_Utente;
                }

                set
                {
                    var oldValue = Utente;
                    if (oldValue == value)
                        return;
                    m_Utente = value;
                    m_IDUtente = DBUtils.GetID(value, 0);
                    DoChanged("Utente", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'utente
            /// </summary>
            public int IDUtente
            {
                get
                {
                    return DBUtils.GetID(m_Utente, m_IDUtente);
                }

                set
                {
                    int oldValue = IDUtente;
                    if (oldValue == value)
                        return;
                    m_IDUtente = value;
                    m_Utente = null;
                    DoChanged("IDUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUtente(Sistema.CUser value)
            {
                m_Utente = value;
                m_IDUtente = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta il numero totale di messaggio memorizzati nella cartella (questo valore viene aggiornato dall'applicazione e potrebbe essere non sincronizzato)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int TotalMessages
            {
                get
                {
                    return m_TotalMessages;
                }

                set
                {
                    int oldValue = m_TotalMessages;
                    if (oldValue == value)
                        return;
                    m_TotalMessages = value;
                    DoChanged("TotalMessages", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero totale di messaggi non letti (aggiornato dall'applicazione)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int TotalUnread
            {
                get
                {
                    return m_TotalUnread;
                }

                set
                {
                    int oldValue = m_TotalUnread;
                    if (oldValue == value)
                        return;
                    m_TotalUnread = value;
                    DoChanged("TotalUnread", value, oldValue);
                }
            }
              
            /// <summary>
            /// Restituisce o imposta il nome della cartella
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del contenitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ParentID
            {
                get
                {
                    return DBUtils.GetID(m_Parent, m_ParentID);
                }

                set
                {
                    int oldValue = ParentID;
                    if (oldValue == value)
                        return;
                    m_ParentID = value;
                    m_Parent = null;
                    DoChanged("ParentID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il contenitore
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolder Parent
            {
                get
                {
                    if (m_Parent is null)
                        m_Parent = Mails.Folders.GetItemById(m_ParentID);
                    return m_Parent;
                }

                set
                {
                    var oldValue = Parent;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Parent = value;
                    m_ParentID = DBUtils.GetID(value, 0);
                    DoChanged("Parent", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il folder 
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetParent(MailFolder value)
            {
                m_Parent = value;
                m_ParentID = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce la collezione delle sottocartelle
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public MailFolderChilds Childs
            {
                get
                {
                    if (m_Childs is null)
                        m_Childs = new MailFolderChilds(this);
                    return m_Childs;
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Name);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Mails.Folders;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_eMailFolders";
            }

            /// <summary>
            /// Aggiorna i conteggi
            /// </summary>
            public void AggiornaConteggi()
            {
                m_TotalMessages = 0;
                m_TotalUnread = 0;

                if (DBUtils.GetID(this, 0) == 0)
                    return;

                using (var cursor = new MailMessageCursor())
                { 
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.FolderID.Value = DBUtils.GetID(this);
                    cursor.PageSize = 1;
                    this.m_TotalMessages = (int) cursor.Count();
                    cursor.Reset1();
                    cursor.ReadDate.Value = default;
                    this.m_TotalUnread = (int) cursor.Count();
                }

                this.Save(true);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name", m_Name);
                m_ParentID = reader.Read("ParentID", m_ParentID);
                m_TotalMessages = reader.Read("TotalMessages", m_TotalMessages);
                m_TotalUnread = reader.Read("TotalUnread", m_TotalUnread);
                m_IDUtente = reader.Read("IDUtente", m_IDUtente);
                m_ApplicationID = reader.Read("ApplicationID", m_ApplicationID);
                 
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Name", m_Name);
                writer.Write("ParentID", ParentID);
                writer.Write("TotalMessages", m_TotalMessages);
                writer.Write("TotalUnread", m_TotalUnread);
                writer.Write("IDUtente", IDUtente);
                writer.Write("ApplicationID", ApplicationID);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c= table.Fields.Ensure("Name", typeof(string), 255);
                c = table.Fields.Ensure("ParentID", typeof(int), 1);
                c = table.Fields.Ensure("TotalMessages", typeof(int), 1);
                c = table.Fields.Ensure("TotalUnread", typeof(int), 1);
                c = table.Fields.Ensure("IDUtente", typeof(int), 1);
                c = table.Fields.Ensure("ApplicationID", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Name", "ParentID", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxStats", new string[] { "TotalMessages", "TotalUnread" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxApp", new string[] { "IDUtente", "ApplicationID" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("ParentID", ParentID);
                writer.WriteAttribute("TotalMessages", m_TotalMessages);
                writer.WriteAttribute("TotalUnread", m_TotalUnread);
                writer.WriteAttribute("IDUtente", IDUtente);
                writer.WriteAttribute("ApplicationID", ApplicationID);
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
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ParentID":
                        {
                            m_ParentID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TotalMessages":
                        {
                            m_TotalMessages = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TotalUnread":
                        {
                            m_TotalUnread = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                  
                    case "IDUtente":
                        {
                            m_IDUtente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ApplicationID":
                        {
                            m_ApplicationID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            private int getPOS(MailRootFolder root, MailFolder o)
            {
                if (root is null)
                    return -1;
                if (DBUtils.GetID(root) == DBUtils.GetID(o))
                    return -10;
                if (DBUtils.GetID(root.Inbox) == DBUtils.GetID(o))
                    return -9;
                if (DBUtils.GetID(root.Sent) == DBUtils.GetID(o))
                    return -8;
                if (DBUtils.GetID(root.Drafts) == DBUtils.GetID(o))
                    return -7;
                if (DBUtils.GetID(root.Spam) == DBUtils.GetID(o))
                    return -6;
                if (DBUtils.GetID(root.TrashBin) == DBUtils.GetID(o))
                    return -5;
                if (DBUtils.GetID(root.Archive) == DBUtils.GetID(o))
                    return -4;
                if (DBUtils.GetID(root.FindFolder) == DBUtils.GetID(o))
                    return -3;
                return -1;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public int CompareTo(MailFolder o)
            {
                MailRootFolder root = null;
                if (Application is object)
                    root = Application.Root;
                int a1 = getPOS(root, this);
                int a2 = getPOS(root, o);
                int ret = a1.CompareTo(a2);
                if (ret == 0)
                    ret = DMD.Strings.Compare(Name, o.Name, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((MailFolder)obj);
            }

            /// <summary>
            /// Salva
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (this.Parent is object)
                    this.Parent.UpdateFolder(this);
                if (Application is object)
                    Application.UpdateFolder(this);
            }

            /// <summary>
            /// Aggiorna la cartella "figlio"
            /// </summary>
            /// <param name="folder"></param>
            /// <returns></returns>
            protected internal virtual MailFolder UpdateFolder(MailFolder folder)
            {
                if (folder is null)
                    throw new ArgumentNullException("folder");
                if (DBUtils.GetID(folder, 0) == 0)
                    return null;

                var o = this.Childs.GetItemById(DBUtils.GetID(folder, 0));
                if (o is null)
                {
                    foreach (var f in Childs)
                    {
                        o = f.UpdateFolder(folder);
                        if (o is object)
                            return o;
                    }
                }
                else
                {
                    int i = Childs.IndexOf(o);
                    Childs[i] = folder;
                }

                return o;
            }
        }
    }
}