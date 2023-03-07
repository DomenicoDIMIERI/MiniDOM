using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Anagrafica;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Rappresenta la descrizione di un repository
        /// </summary>
        [Serializable]
        public sealed class CModule 
            : Databases.DBObject, IComparable, ISettingsOwner, IComparable<CModule>
        {
            private string m_DisplayName;
            private string m_ModuleName; // Nome del modulo
            private string m_ModulePath; // Percorso del modulo		
            private string m_Description; // Descrizione del modulo
            private bool m_IsBuiltIn; // Vero se il modulo appartiene alla libreria standard
            private bool m_Visible; // Se vero il modulo è visibile
            private int m_Posizione;
            private bool m_ShowInMainPage; // Se vero il modulo deve essere visualizzato in prima pagina
            private string m_ClassHandler; // Nome della classe che verrà utilizzata per eseguire il modulo
                                           // Private m_Handler As IModuleHandler = Nothing
            private bool m_EncriptURL; // Se vero la URL viene nascota da un collegamento valido solo all'interno della sessione corrente
            private string m_Target; // Finstra di destinazione del collegamento
            [NonSerialized] private CModulesCollection m_Childs;  // Collezione dei moduli contenuti
            private string m_IconPath; // Percorso di una icona che rappresenta il modulo
            private int m_ParentID;
            [NonSerialized] private CModule m_Parent;
            [NonSerialized] private CModuleActions m_DefinedActions;
            [NonSerialized] private CSettings m_Settings;
            [NonSerialized] private IModuleHandler m_handler = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CModule()
            {
                m_DisplayName = "";
                m_ModuleName = "";
                m_ModulePath = "";
                m_Description = "";
                m_IsBuiltIn = false;
                m_Visible = true;
                m_Posizione = 0;
                m_ShowInMainPage = false;
                m_ClassHandler = "";
                // Me.m_Handler = Nothing
                m_EncriptURL = true;
                m_Target = "";
                m_Childs = null;
                m_IconPath = "";
                m_ParentID = 0;
                m_Parent = null;
                // Me.m_Configuration = Nothing
                m_DefinedActions = null;
                m_Settings = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            public CModule(string moduleName) : this()
            {
                moduleName = DMD.Strings.Trim(moduleName);
                if (string.IsNullOrEmpty(moduleName))
                    throw new ArgumentNullException("moduleName");
                m_ModuleName = moduleName;
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Modules; //.Module;
            }

            /// <summary>
            /// Crea il cursore per il repository
            /// </summary>
            /// <returns></returns>
            public Databases.DBObjectCursorBase CreateCursor()
            {
                var h = (IModuleHandler)CreateHandler(this);
                if (h is object)
                {
                    return h.CreateCursor();
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Restituisce le impostazioni aggiuntive del modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CSettings Settings
            {
                get
                {
                    if (m_Settings is null)
                        m_Settings = new CSettings(this);
                    return m_Settings;
                }
            }

            void ISettingsOwner.NotifySettingsChanged(SettingsChangedEventArgs e)
            {
                //e.Setting.Save();
                this.SetChanged(true);
            }

            CSetting ISettingsOwner.CreateSetting(object args)
            {
                var ret = new CSetting();
                return ret;
            }

            /// <summary>
            /// Restituisce il nome del modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ModuleName
            {
                get
                {
                    return m_ModuleName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ModuleName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ModuleName = value;
                    DoChanged("ModuleName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore intero che indica la precedenza nell'ordinamento rispetto al contenitore
            /// </summary>
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
            /// Restituisce il percorso gestitoto dal modulo
            /// </summary>
            public string ModulePath
            {
                get
                {
                    return m_ModulePath;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ModulePath;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ModulePath = value;
                    DoChanged("ModulePath", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione estesa del modulo
            /// </summary>
            public string Description
            {
                get
                {
                    return m_Description;
                }

                set
                {
                    string oldValue = m_Description;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Description = value;
                    DoChanged("Description", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se il modulo é un modulo di sistema
            /// </summary>
            /// <returns></returns>
            public bool IsBuiltIn()
            {
                return m_IsBuiltIn;
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il modulo é visibile all'utente
            /// </summary>
            public bool Visible
            {
                get
                {
                    return m_Visible;
                }

                set
                {
                    if (m_Visible == value)
                        return;
                    m_Visible = value;
                    DoChanged("Visible", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la URL dell'icona associata al modulo
            /// </summary>
            public string IconPath
            {
                get
                {
                    return m_IconPath;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IconPath;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconPath = value;
                    DoChanged("IconPath", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome user-friendly del modulo
            /// </summary>
            public string DisplayName
            {
                get
                {
                    return m_DisplayName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DisplayName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DisplayName = value;
                    DoChanged("DisplayName", value, oldValue);
                }
            }

            

            /// <summary>
            /// Restituisce o imposta l'ID del modulo contenitore
            /// </summary>
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
                    if (m_Parent is object)
                        m_Parent.ResetChilds();
                    m_Parent = minidom.Sistema.Modules.GetItemById(m_ParentID);
                    if (m_Parent is object)
                        m_Parent.ResetChilds();
                    DoChanged("ParentID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un riferimento al modulo contenitore
            /// </summary>
            public CModule Parent
            {
                get
                {
                    if (m_Parent is null)
                        m_Parent = minidom.Sistema.Modules.GetItemById(m_ParentID);
                    return m_Parent;
                }

                set
                {
                    var oldValue = Parent;
                    if (oldValue == value)
                        return;
                    if (m_Parent is object)
                        m_Parent.ResetChilds();
                    m_Parent = value;
                    m_ParentID = DBUtils.GetID(value, this.m_ParentID);
                    if (m_Parent is object)
                        m_Parent.ResetChilds();
                    DoChanged("Parent", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il riferimento al modulo contenitore
            /// </summary>
            /// <param name="value"></param>
            internal void SetParent(CModule value)
            {
                m_Parent = value;
                m_ParentID = DBUtils.GetID(value, this.m_ParentID);
            }


            /// <summary>
            /// Restituisce la collezione delle azioni definite per il modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModuleActions DefinedActions
            {
                get
                {
                    lock (Modules.actionsLock)
                    {
                        if (m_DefinedActions is null)
                        {
                            m_DefinedActions = new CModuleActions();
                            m_DefinedActions.Load(this);
                        }

                        return m_DefinedActions;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della classe che gestisce il modulo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ClassHandler
            {
                get
                {
                    return m_ClassHandler;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ClassHandler;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ClassHandler = DMD.Strings.Trim(value);
                    // Me.m_Handler = Nothing
                    DoChanged("ClassHandler", value, oldValue);
                }
            }

            

            /// <summary>
            /// Restituisce la collezione dei moduli "figlio"
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModulesCollection Childs
            {
                get
                {
                    lock (this)
                    {
                        if (m_Childs is null)
                            m_Childs = new CModulesCollection(this);
                        return m_Childs;
                    }
                }
            }

            /// <summary>
            /// Restituisce true se l'utente corrente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public bool UserCanDoAction(CModuleAction action)
            {
                return action.UserCanDoAction(minidom.Sistema.Users.CurrentUser);
            }

            /// <summary>
            /// Restituisce true se l'utente corrente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            public bool UserCanDoAction(string actionName)
            {
                var a = DefinedActions.GetItemByKey(actionName);
                if (a is null)
                    return false; // Throw New ArgumentOutOfRangeException("Il modulo non implementa l'azione [" & actionName & "]")
                return UserCanDoAction(a);
            }

            /// <summary>
            /// Restituisce true se l'utente corrente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="actionID"></param>
            /// <returns></returns>
            public bool UserCanDoAction(int actionID)
            {
                return UserCanDoAction(DefinedActions.GetItemById(actionID));
            }

            /// <summary>
            /// Restituisce true se il modulo è visibile all'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public bool IsVisibleToUser(CUser user)
            {
                // Return Me.IsVisibleToUser(GetID(user))
                if (user is null)
                    throw new ArgumentNullException("user");
                bool a = false;
                bool n = false;
                user.Modules.GetAllowNegate(this, ref a, ref n);
                foreach (CGroup grp in user.Groups)
                    grp.Modules.GetAllowNegate(this, ref a, ref n);
                return a && !n;
            }

            /// <summary>
            /// Crea un oggetto che gestisce le azioni sul modulo
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public object CreateHandler(object context)
            {
                // If (Me.m_Handler Is Nothing AndAlso Me.m_ClassHandler <> vbNullString) Then
                // Me.m_Handler = Types.CreateInstance(Me.m_ClassHandler)
                // Me.m_Handler.SetModule(Me)
                // End If
                // If (m_Handler IsNot Nothing) Then Me.m_Handler.Context = context
                // Return Me.m_Handler
                if (m_handler is null && !string.IsNullOrEmpty(m_ClassHandler))
                {
                    m_handler = (IModuleHandler)minidom.Sistema.ApplicationContext.CreateInstance(m_ClassHandler);
                    m_handler.SetModule(this);
                }

                return m_handler;
            }

            /// <summary>
            /// Esegue l'azione nel contesto predefinito
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            public string ExecuteAction(string actionName)
            {
                return ExecuteAction(actionName, null);
            }

            // Public Function ExecuteAction1(ByVal actionName As String) As MethodResults
            // Return Me.ExecuteAction1(actionName, Nothing)
            // End Function

            /// <summary>
            /// Esegue l'azione nel contesto specifico
            /// </summary>
            /// <param name="actionName"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public string ExecuteAction(string actionName, object context)
            {
                if (string.IsNullOrEmpty(actionName))
                    actionName = "list";
                IModuleHandler handler = (IModuleHandler)CreateHandler(context);
                return handler.ExecuteAction(context, actionName);
            }

            // Public Function ExecuteAction1(ByVal actionName As String, ByVal context As Object) As MethodResults
            // If actionName = "" Then actionName = "list"
            // Dim handler As IModuleHandler = Me.CreateHandler(context)
            // Return handler.ExecuteAction1(context, actionName)
            // End Function


            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Modules";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_ModuleName = reader.Read("ModuleName", this.m_ModuleName);
                m_ModulePath = reader.Read("ModulePath", this.m_ModulePath);
                m_DisplayName = reader.Read("DisplayName", this.m_DisplayName);
                m_Description = reader.Read("Description", this.m_Description);
                m_IsBuiltIn = reader.Read("BuiltIn", this.m_IsBuiltIn);
                m_Visible = reader.Read("Visible", this.m_Visible);
                m_IconPath = reader.Read("IconPath", this.m_IconPath);
                m_ParentID = reader.Read("Parent", this.m_ParentID);
                m_ShowInMainPage = reader.Read("ShowInMainPage", this.m_ShowInMainPage);
                m_ClassHandler = reader.Read("ClassHandler", this.m_ClassHandler);
                m_EncriptURL = reader.Read("EncriptURL", this.m_EncriptURL);
                m_Target = reader.Read("Target", this.m_Target);
                m_Posizione = reader.Read("Posizione", this.m_Posizione);
                // m_ConfigClass = Formats.ToString(dbRis("ConfigClass"))
                string tmp = reader.Read("Settings", "");
                if (!string.IsNullOrEmpty(tmp)) {
                    this.m_Settings = DMD.XML.Utils.Deserialize<CSettings>(tmp);
                    this.m_Settings.SetOwner(this);
                }
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                // dbRis("ConfigClass", Me.m_ConfigClass)
                writer.Write("ModuleName", m_ModuleName);
                writer.Write("ModulePath", m_ModulePath);
                writer.Write("Description", m_Description);
                writer.Write("BuiltIn", m_IsBuiltIn);
                writer.Write("Visible", m_Visible);
                writer.Write("IconPath", m_IconPath);
                writer.Write("Parent", DBUtils.GetID(m_Parent, m_ParentID));
                writer.Write("Posizione", m_Posizione);
                writer.Write("DisplayName", m_DisplayName);
                writer.Write("ShowInMainPage", m_ShowInMainPage);
                writer.Write("ClassHandler", m_ClassHandler);
                writer.Write("EncriptURL", m_EncriptURL);
                writer.Write("Target", m_Target);
                writer.Write("Settings", DMD.XML.Utils.Serialize(this.Settings));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi della tabella
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("ModuleName", typeof(string), 255);
                c = table.Fields.Ensure("ModulePath", typeof(string), 255);
                c = table.Fields.Ensure("Description", typeof(string), 0);
                c = table.Fields.Ensure("BuiltIn", typeof(bool), 0);
                c = table.Fields.Ensure("Visible", typeof(bool), 0);
                c = table.Fields.Ensure("IconPath", typeof(string), 255);
                c = table.Fields.Ensure("Parent", typeof(int), 0);
                c = table.Fields.Ensure("Posizione", typeof(int), 0);
                c = table.Fields.Ensure("DisplayName", typeof(string), 255);
                c = table.Fields.Ensure("ShowInMainPage", typeof(bool), 0);
                c = table.Fields.Ensure("ClassHandler", typeof(string), 255);
                c = table.Fields.Ensure("EncriptURL", typeof(string), 255);
                c = table.Fields.Ensure("Target", typeof(string), 255);
                c = table.Fields.Ensure("Settings", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli sulla tabella
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object) 
                    c.Drop();
                
                c = table.Constraints.Ensure("idxModuleName", new string[] { "ModuleName", "Stato" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxParent", new string[] { "Parent", "Posizione" }, DBFieldConstraintFlags.Unique);
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_IsBuiltIn", m_IsBuiltIn);
                writer.WriteAttribute("m_Visible", m_Visible);
                writer.WriteAttribute("m_Posizione", m_Posizione);
                writer.WriteAttribute("m_ShowInMainPage", m_ShowInMainPage);
                writer.WriteAttribute("m_ClassHandler", m_ClassHandler);
                writer.WriteAttribute("m_EncriptURL", m_EncriptURL);
                writer.WriteAttribute("m_Target", m_Target);
                writer.WriteAttribute("m_IconPath", m_IconPath);
                writer.WriteAttribute("m_DisplayName", m_DisplayName);
                writer.WriteAttribute("m_ModuleName", m_ModuleName);
                writer.WriteAttribute("m_ModulePath", m_ModulePath);
                writer.WriteAttribute("m_ParentID", ParentID);
                base.XMLSerialize(writer);
                writer.WriteTag("Settings", this.Settings);
                writer.WriteTag("m_Description", m_Description);
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
                    case "m_DisplayName":
                        {
                            m_DisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_ModuleName":
                        {
                            m_ModuleName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_ModulePath":
                        {
                            m_ModulePath = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Description":
                        {
                            m_Description = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_IsBuiltIn":
                        {
                            m_IsBuiltIn = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_Visible":
                        {
                            m_Visible = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_Posizione":
                        {
                            m_Posizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_ShowInMainPage":
                        {
                            m_ShowInMainPage = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_ClassHandler":
                        {
                            m_ClassHandler = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // ret &= "<m_Handler
                    case "m_EncriptURL":
                        {
                            m_EncriptURL = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_Target":
                        {
                            m_Target = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Childs":
                        {
                            break;
                        }
                    // Me.Childs.Clear
                    case "m_IconPath":
                        {
                            m_IconPath = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_ParentID":
                        {
                            m_ParentID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    case "Settings":
                        {
                            m_Settings = (CSettings)fieldValue ;
                            m_Settings.SetOwner(this);
                            break;
                        }
                    // ret &= "<m_ConfigClass
                    // ret &= "<m_Configuration
                    case "m_DefinedActions":
                        {
                            if (!(fieldValue is string))
                            {
                                if (!Arrays.IsArray(fieldValue))
                                    fieldValue = new[] { fieldValue };
                                DefinedActions.Clear();
                                DefinedActions.AddRange((IEnumerable)fieldValue);
                            }

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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_ModuleName;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_ModuleName, this.ParentID, this.m_ClassHandler);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CModule) && this.Equals((CModule)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CModule obj)
            {
                return     DMD.Strings.EQ(this.m_DisplayName, obj.m_DisplayName)
                        && DMD.Strings.EQ(this.m_ModuleName, obj.m_ModuleName)
                        && DMD.Strings.EQ(this.m_ModulePath, obj.m_ModulePath)
                        && DMD.Strings.EQ(this.m_Description, obj.m_Description)
                        && DMD.Booleans.EQ(this.m_IsBuiltIn, obj.m_IsBuiltIn)
                        && DMD.Booleans.EQ(this.m_Visible, obj.m_Visible)
                        && DMD.Integers.EQ(this.m_Posizione, obj.m_Posizione)
                        && DMD.Booleans.EQ(this.m_ShowInMainPage, obj.m_ShowInMainPage)
                        && DMD.Strings.EQ(this.m_ClassHandler, obj.m_ClassHandler)
                        && DMD.Booleans.EQ(this.m_EncriptURL, obj.m_EncriptURL)
                        && DMD.Strings.EQ(this.m_Target, obj.m_Target)
                        && DMD.Strings.EQ(this.m_IconPath, obj.m_IconPath)
                        && DMD.Integers.EQ(this.ParentID, obj.ParentID);
            
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public int CompareTo(CModule b)
            {
                int ret = Arrays.Compare(this.m_Posizione , b.m_Posizione);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_DisplayName, b.m_DisplayName, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CModule)obj);
            }

            CSettings ISettingsOwner.Settings
            {
                get
                {
                    return this.Settings;
                }
            }

            internal void ResetChilds()
            {
                lock (this)
                    m_Childs = null;
            }

            
        }
    }
}