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
        public enum ImportExportSourceFlags : int
        {
            None = 0,
            CanImport = 1,
            CanExport = 2
        }

        /// <summary>
    /// Rappresenta una sorgente di importazione/esportazione
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CImportExportSource : Databases.DBObject
        {
            private string m_Name;
            private string m_DisplayName;
            private ImportExportSourceFlags m_Flags;
            private string m_Password;
            // Private m_UserName As String
            private string m_RemoteURL;
            private CKeyCollection m_Attributi;
            private CImportExportSourceUserMap m_UserMapping;

            public CImportExportSource()
            {
                m_Name = "";
                m_Flags = ImportExportSourceFlags.None;
                m_Password = "";
                m_RemoteURL = "";
                m_Attributi = null;
                m_UserMapping = null;
                m_DisplayName = "";
                // Me.m_UserName = ""
            }

            public string DisplayName
            {
                get
                {
                    return m_DisplayName;
                }

                set
                {
                    string oldValue = m_DisplayName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DisplayName = value;
                    DoChanged("DisplayName", value, oldValue);
                }
            }

            public Sistema.CUser MapUser(int remoteID)
            {
                foreach (CImportExportSourceUserMatc u in UserMapping)
                {
                    if (u.RemoteUserID == remoteID)
                    {
                        return u.LocalUser;
                    }
                }

                return null;
            }

            public CImportExportSourceUserMap UserMapping
            {
                get
                {
                    lock (this)
                    {
                        if (m_UserMapping is null)
                        {
                            var tmp = Attributi.GetItemByKey("UserMapping");
                            if (!(tmp is CImportExportSourceUserMap))
                            {
                                m_UserMapping = new CImportExportSourceUserMap();
                            }
                            else
                            {
                                m_UserMapping = (CImportExportSourceUserMap)tmp;
                            }

                            m_UserMapping.SetOwner(this);
                            Attributi.SetItemByKey("UserMapping", m_UserMapping);
                        }

                        return m_UserMapping;
                    }
                }
            }

            /// <summary>
        /// Restituisce o imposta un nome descrittivo per l'oggetto
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
                    string oldValue = m_Name;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            public ImportExportSourceFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se questo oggetto è abilitato all'importazione dal sito specificato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CanImport
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, ImportExportSourceFlags.CanImport);
                }

                set
                {
                    if (CanImport == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, ImportExportSourceFlags.CanImport, value);
                    DoChanged("CanImport", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se questo oggetto è abilitato all'esportazione verso il sito remoto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CanExport
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, ImportExportSourceFlags.CanExport);
                }

                set
                {
                    if (CanExport == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, ImportExportSourceFlags.CanExport, value);
                    DoChanged("CanExport", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta una password che deve essere condivisa tra i siti che esportano ed importano
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Password
            {
                get
                {
                    return m_Password;
                }

                set
                {
                    string oldValue = m_Password;
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Password = value;
                    DoChanged("Password", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la url completa che viene chiamata per esportare verso il sito esterno
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string RemoteURL
            {
                get
                {
                    return m_RemoteURL;
                }

                set
                {
                    string oldValue = m_RemoteURL;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RemoteURL = value;
                    DoChanged("RemoteURL", value, oldValue);
                }
            }

            // ''' <summary>
            // ''' Restituisce o imposta il nome utente usato per l'autenticazione con il server remoto
            // ''' </summary>
            // ''' <returns></returns>
            // Public Property UserName As String
            // Get
            // Return Me.m_UserName
            // End Get
            // Set(value As String)
            // Dim oldValue As String = Me.m_UserName
            // value = Strings.Trim(value)
            // If (oldValue = value) Then Return
            // Me.m_UserName = value
            // Me.DoChanged("UserName", value, oldValue)
            // End Set
            // End Property

            public CKeyCollection Attributi
            {
                get
                {
                    if (m_Attributi is null)
                        m_Attributi = new CKeyCollection();
                    return m_Attributi;
                }
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name", m_Name);
                m_DisplayName = reader.Read("DisplayName", m_DisplayName);
                m_Flags = reader.Read("Flags", m_Flags);
                m_Password = reader.Read("Password", m_Password);
                m_RemoteURL = reader.Read("RemoteURL", m_RemoteURL);
                try
                {
                    string argvalue = "";
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Attributi", argvalue));
                }
                catch (Exception ex)
                {
                    m_Attributi = null;
                }

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Name", m_Name);
                writer.Write("DisplayName", m_DisplayName);
                writer.Write("Flags", m_Flags);
                writer.Write("Password", m_Password);
                writer.Write("RemoteURL", m_RemoteURL);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("DisplayName", m_DisplayName);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("Password", m_Password);
                writer.WriteAttribute("RemoteURL", m_RemoteURL);
                base.XMLSerialize(writer);
                writer.WriteTag("Attributi", Attributi);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DisplayName":
                        {
                            m_DisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (ImportExportSourceFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Password":
                        {
                            m_Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RemoteURL":
                        {
                            m_RemoteURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
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
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return ImportExportSources.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDImportExportS";
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                ImportExportSources.UpdateCached(this);
            }

            /// <summary>
        /// Esporta l'anagrafica e la finestra di lavorazione corrente verso il sito remoto
        /// </summary>
        /// <param name="item"></param>
        /// <remarks></remarks>
            protected internal virtual CImportExport Esporta(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (CanExport == false)
                    throw new PermissionDeniedException("Azione Export non consentita per la sorgente: " + Name);
                string tmp = Sistema.RPC.InvokeMethod(RemoteURL + "?_a=Esporta", "persona", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item.PersonaEsportata)), "recapiti", DMD.XML.Utils.Serializer.Serialize(item.PersonaEsportata.Recapiti), "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password));
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(tmp);
            }

            /// <summary>
        /// Esporta l'anagrafica e la finestra di lavorazione corrente verso il sito remoto
        /// </summary>
        /// <param name="item"></param>
        /// <remarks></remarks>
            protected internal virtual CImportExport ConfermaEsportazione(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (CanExport == false)
                    throw new PermissionDeniedException("Azione Export non consentita per la sorgente: " + Name);
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=ConfermaEsportazione", "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }

            /// <summary>
        /// Revoca la richiesta di confronto fatta
        /// </summary>
        /// <param name="item"></param>
        /// <remarks></remarks>
            protected internal virtual CImportExport AnnullaEsportazione(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (CanExport == false)
                    throw new PermissionDeniedException("Azione Export non consentita per la sorgente: " + Name);
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=AnnullaEsportazione", "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }

            /// <summary>
        /// Nel sistema che ha ricevuto la richiesta di valutazione rifiuta il cliente
        /// </summary>
        /// <param name="item"></param>
        /// <remarks></remarks>
            protected internal virtual CImportExport RifiutaCliente(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (CanImport == false)
                    throw new PermissionDeniedException("Azione Import non consentita per la sorgente: " + Name);
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=RifiutaCliente", "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }


            /// <summary>
        /// Importa l'anagrafica e la finestra di lavorazione corrente verso il sito remoto
        /// </summary>
        /// <param name="item"></param>
        /// <remarks></remarks>
            protected internal virtual CImportExport Importa(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (CanImport == false)
                    throw new PermissionDeniedException("Azione Import non consentita per la sorgente: " + Name);
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=Importa", "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }

            /// <summary>
        /// Prende in carico la finestra importata da un sito esterno
        /// </summary>
        /// <param name="item"></param>
        /// <remarks></remarks>
            protected internal virtual CImportExport PrendiInCarico(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (CanImport == false)
                    throw new PermissionDeniedException("Azione PrendiInCarico non consentita per la sorgente " + Name);
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=PrendiInCarico", "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }

            protected internal virtual CImportExport Sincronizza(CImportExport item, CKeyCollection oggetti)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (!CanExport || !CanImport)
                    throw new PermissionDeniedException("La sincronizzazione richiede l'autorizzazione ad Importare e Esportare");
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=Sincronizza", "oggetti", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(oggetti)), "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }

            protected internal virtual CImportExport Sollecita(CImportExport item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                if (!CanExport || !CanImport)
                    throw new PermissionDeniedException("La sollecitazione richiede l'autorizzazione ad Importare e Esportare");
                return (CImportExport)DMD.XML.Utils.Serializer.Deserialize(Sistema.RPC.InvokeMethod(RemoteURL + "?_a=Sollecita", "item", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(item)), "un", Sistema.RPC.str2n(Name), "pwd", Sistema.RPC.str2n(Password)));
            }
        }
    }
}