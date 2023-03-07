using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
    /// Flags dell'oggetto CCollegamento
    /// </summary>
        public enum CollegamentoFlags : int
        {

            /// <summary>
        /// Nessun flag
        /// </summary>
            None = 0,

            /// <summary>
        /// Esegue il collegamento all'avvio
        /// </summary>
            Autorun = 1,

            /// <summary>
        /// Collegamento nascoto (usato in congiunzione con Autorun consente di lanciare script all'avvio dell'App)
        /// </summary>
            Hidden = 2
        }


        [Serializable]
        public class CCollegamento 
            : Databases.DBObject, IComparable<CCollegamento>, IComparable
        {
            private string m_Nome;
            private string m_Descrizione;
            private string m_Link;
            private string m_Gruppo;
            private string m_Target;
            private string m_IconURL;
            private bool m_EncriptURL;
            private string m_PostedData;
            private int m_IDParent;
            [NonSerialized] private CCollegamento m_Parent;
            [NonSerialized] private CSubCollegamenti m_Childs;
            private string m_CallModule;
            private string m_CallAction;
            [NonSerialized] private LinkUserAllowNegateCollection m_UsersAuth;
            [NonSerialized] private LinkGroupAllowNegateCollection m_GroupAuth;
            private int m_Posizione;
            private CollegamentoFlags m_Flags;
            private bool m_Attivo;

            public CCollegamento()
            {
                m_Nome = "Nuovo collegamento";
                m_Descrizione = "";
                m_Link = "";
                m_Gruppo = "";
                m_Target = "";
                m_IconURL = "";
                m_EncriptURL = false;
                m_IDParent = 0;
                m_Childs = null;
                m_Parent = null;
                m_CallAction = DMD.Strings.vbNullString;
                m_CallModule = DMD.Strings.vbNullString;
                m_Posizione = 0;
                m_Flags = CollegamentoFlags.None;
                m_PostedData = "";
                m_Attivo = true;
            }

            public override CModulesClass GetModule()
            {
                return Collegamenti.Module;
            }

            public string PostedData
            {
                get
                {
                    return m_PostedData;
                }

                set
                {
                    string oldValue = m_PostedData;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_PostedData = value;
                    DoChanged("PostedData", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CollegamentoFlags Flags
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
        /// Restituisce o imposta un valore crescente che forza l'ordinamento (ordinamento per gruppo, posizione, nome)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Posizione
            {
                get
                {
                    return m_Posizione;
                }

                set
                {
                    int oldValue = m_Posizione;
                    if (oldValue == value)
                        return;
                    m_Posizione = value;
                    DoChanged("Posizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il link è attivo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del link (deve essere univoco)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del modulo a cui fa riferimento il link
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string CallModule
            {
                get
                {
                    return m_CallModule;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_CallModule;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_CallModule = value;
                    DoChanged("CallModule", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'azione da eseguire sul modulo a cui fa riferimento il link
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string CallAction
            {
                get
                {
                    return m_CallAction;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_CallAction;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_CallAction = value;
                    DoChanged("CallModule", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica che la url deve essere nascosta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool EncriptURL
            {
                get
                {
                    return m_EncriptURL;
                }

                set
                {
                    if (m_EncriptURL == value)
                        return;
                    m_EncriptURL = value;
                    DoChanged("EncriptURL", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il testo visualizzato per il link
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la url a cui fa riferimento il link
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Link
            {
                get
                {
                    return m_Link;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Link;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Link = value;
                    DoChanged("Link", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del gruppo a cui fa riferimento il link
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Gruppo
            {
                get
                {
                    return m_Gruppo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Gruppo;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Gruppo = value;
                    DoChanged("Gruppo", value, oldValue);
                }
            }

            public string Target
            {
                get
                {
                    return m_Target;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Target;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_Target = value;
                    DoChanged("Target", value, oldValue);
                }
            }

            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            public string GetURL()
            {
                string ret;
                if (m_EncriptURL)
                {
                    // ret = "/download.asp?tk=" & ASPSecurity.FindTokenOrCreate(Me.m_Link, Me.m_Link) & "&dn=" & m_Descrizione
                    ret = m_Link;
                }
                else
                {
                    ret = m_Link;
                }

                return ret;
            }

            public int IDParent
            {
                get
                {
                    return Databases.GetID(m_Parent, m_IDParent);
                }

                set
                {
                    int oldValue = IDParent;
                    if (oldValue == value)
                        return;
                    m_IDParent = value;
                    m_Parent = null;
                    DoChanged("IDParent", value, oldValue);
                }
            }

            public CCollegamento Parent
            {
                get
                {
                    if (m_Parent is null)
                        m_Parent = Collegamenti.GetItemById(m_IDParent);
                    return m_Parent;
                }

                set
                {
                    var oldValue = Parent;
                    if (oldValue == value)
                        return;
                    m_Parent = value;
                    m_IDParent = Databases.GetID(value);
                    DoChanged("Parent", value, oldValue);
                }
            }

            protected internal virtual void SetParent(CCollegamento p)
            {
                m_Parent = p;
                m_IDParent = Databases.GetID(p);
            }

            public CSubCollegamenti Childs
            {
                get
                {
                    lock (this)
                    {
                        if (m_Childs is null)
                            m_Childs = new CSubCollegamenti(this);
                        return m_Childs;
                    }
                }
            }

            public override string GetTableName()
            {
                return "tbl_Collegamenti";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_Link = reader.Read("Link", this.m_Link);
                m_Gruppo = reader.Read("Gruppo", this.m_Gruppo);
                m_Target = reader.Read("Target", this.m_Target);
                m_IconURL = reader.Read("IconURL", this.m_IconURL);
                m_EncriptURL = reader.Read("EncriptURL", this.m_EncriptURL);
                m_IDParent = reader.Read("IDParent", this.m_IDParent);
                m_CallModule = reader.Read("CallModule", this.m_CallModule);
                m_CallAction = reader.Read("CallAction", this.m_CallAction);
                m_Attivo = reader.Read("Attivo", this.m_Attivo);
                m_Posizione = reader.Read("Posizione", this.m_Posizione);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_PostedData = reader.Read("PostedData", this.m_PostedData);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Link", m_Link);
                writer.Write("Gruppo", m_Gruppo);
                writer.Write("Target", m_Target);
                writer.Write("IconURL", m_IconURL);
                writer.Write("EncriptURL", m_EncriptURL);
                writer.Write("IDParent", IDParent);
                writer.Write("CallModule", m_CallModule);
                writer.Write("CallAction", m_CallAction);
                writer.Write("Attivo", m_Attivo);
                writer.Write("Posizione", m_Posizione);
                writer.Write("Flags", m_Flags);
                writer.Write("PostedData", m_PostedData);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Descrizione;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Link", m_Link);
                writer.WriteAttribute("Gruppo", m_Gruppo);
                writer.WriteAttribute("Target", m_Target);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("EncriptURL", m_EncriptURL);
                writer.WriteAttribute("IDParent", IDParent);
                writer.WriteAttribute("CallModule", m_CallModule);
                writer.WriteAttribute("CallAction", m_CallAction);
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("Posizione", m_Posizione);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                base.XMLSerialize(writer);
                writer.WriteTag("PostedData", m_PostedData);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName  )
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Link":
                        {
                            m_Link = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Gruppo":
                        {
                            m_Gruppo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Target":
                        {
                            m_Target = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "EncriptURL":
                        {
                            m_EncriptURL = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDParent":
                        {
                            m_IDParent = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CallModule":
                        {
                            m_CallModule = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CallAction":
                        {
                            m_CallAction = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Posizione":
                        {
                            m_Posizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (CollegamentoFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PostedData":
                        {
                            m_PostedData = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public LinkUserAllowNegate SetUserAllowNegate(Sistema.CUser user, bool allow)
            {
                LinkUserAllowNegate item = null;
                int i = 0;
                while (i < UserAuth.Count && item is null)
                {
                    if (UserAuth[i].UserID == Databases.GetID(user))
                    {
                        item = UserAuth[i];
                    }
                    else
                    {
                        i += 1;
                    }
                }

                if (item is null)
                {
                    item = new LinkUserAllowNegate();
                    UserAuth.Add(item);
                }

                item.Item = this;
                item.User = user;
                item.Allow = allow;
                item.Save();
                return item;
            }

            public LinkGroupAllowNegate SetGroupAllowNegate(Sistema.CGroup group, bool allow)
            {
                LinkGroupAllowNegate item = null;
                int i = 0;
                while (i < GroupAuth.Count && item is null)
                {
                    if (GroupAuth[i].GroupID == Databases.GetID(group))
                    {
                        item = GroupAuth[i];
                    }
                    else
                    {
                        i += 1;
                    }
                }

                if (item is null)
                {
                    item = new LinkGroupAllowNegate();
                    GroupAuth.Add(item);
                }

                item.Item = this;
                item.Group = group;
                item.Allow = allow;
                item.Save();
                return item;
            }


            /// <summary>
        /// Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsVisibleToUser(Sistema.CUser user)
            {
                bool a = false;
                int i = 0;
                LinkUserAllowNegate ua;
                LinkGroupAllowNegate ga;
                while (i < UserAuth.Count)
                {
                    ua = UserAuth[i];
                    if (ua.UserID == Databases.GetID(user))
                    {
                        a = a | ua.Allow;
                    }

                    i += 1;
                }

                i = 0;
                while (i < GroupAuth.Count)
                {
                    ga = GroupAuth[i];
                    foreach (Sistema.CGroup gp in user.Groups)
                    {
                        if (ga.GroupID == Databases.GetID(gp))
                        {
                            a = a | ga.Allow;
                        }
                    }

                    i += 1;
                }

                return a;
            }

            public bool IsVisibleToUser(int userID)
            {
                return IsVisibleToUser(Sistema.Users.GetItemById(userID));
            }

            public LinkUserAllowNegateCollection UserAuth
            {
                get
                {
                    lock (this)
                    {
                        if (m_UsersAuth is null)
                            m_UsersAuth = new LinkUserAllowNegateCollection(this);
                        return m_UsersAuth;
                    }
                }
            }

            public LinkGroupAllowNegateCollection GroupAuth
            {
                get
                {
                    lock (this)
                    {
                        if (m_GroupAuth is null)
                            m_GroupAuth = new LinkGroupAllowNegateCollection(this);
                        return m_GroupAuth;
                    }
                }
            }

            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
                m_Childs = null;
                m_GroupAuth = null;
                m_UsersAuth = null;
            }

            public virtual int CompareTo(CCollegamento other)
            {
                int ret;
                ret = DMD.Strings.Compare(m_Gruppo, other.m_Gruppo, true);
                if (ret == 0)
                    ret = DMD.Arrays.Compare(this.m_Posizione, other.m_Posizione);
                if (ret == 0)
                    ret = DMD.Strings.Compare(this.m_Descrizione, other.m_Descrizione, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CCollegamento)obj);
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                Collegamenti.UpdateCached(this);
            }
        }
    }
}