using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;
using System.Data;
using static minidom.Anagrafica;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Stati utente
        /// </summary>
        public enum UserStatus : int
        {
            // USER_TEMP = 0         'Utente non valido

            /// <summary>
            /// Utente abilitato
            /// </summary>
            USER_ENABLED = 1,

            /// <summary>
            /// Utente disabilitato
            /// </summary>
            USER_DISABLED = 2,

            /// <summary>
            /// Utente eliminato
            /// </summary>
            USER_DELETED = 3,

            /// <summary>
            /// Utente in attesa di essere confermato
            /// </summary>
            USER_NEW = 4,

            /// <summary>
            /// Utente sospeso temporaneamente
            /// </summary>
            USER_SUSPENDED = 5    
        }

        /// <summary>
        /// Flag 
        /// </summary>
        [Flags]
        public enum UserFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Utente nascosto
            /// </summary>
            Hidden = 1,

            /// <summary>
            /// Forza il cambiamento della password al prossimo accesso
            /// </summary>
            ForceChangePassword = 2
        }

        /// <summary>
        /// Evento generato quando la password dell'utente viene modificata
        /// </summary>
        public delegate void PasswordChangedEventHandler(object sender, UserEventArgs e);

        /// <summary>
        /// Utente
        /// </summary>
        [Serializable]
        public sealed class CUser 
            : minidom.Databases.DBObjectPO, ISupportsSingleNotes, IComparable, IComparable<CUser>, IFonte, ISettingsOwner, ICloneable, ISupportInitializeFrom
        {

            /// <summary>
            /// Evento generato quando la password dell'utente viene modificata
            /// </summary>
            public event PasswordChangedEventHandler PasswordChanged;

            

            private string m_UserName; // UserName
            private string m_Nominativo; // [TEXT] Nominativo
            private string m_IconURL;
            private string m_email; // e-mail dell'utente
                                    // Private m_Password		'[Password
            private DateTime? m_ExpireDate; // [DATE] Data di scadenza dell'account
            private DateTime? m_ExpireLogin; // [date] Data di scadenza dell'account
            private int m_ExpireDays; // [int]  Numero di giorni di cui prolungare la data di scadenza dell'account
                                      // Private m_IsLogged As Boolean '[BOOL] Vero se l'utente è loggato
            [NonSerialized]
            private CSettings m_Settings; // [CUserSettings] Collezione di parametri aggiuntivi
            private string m_HomePage; // [TEXT] Indirizzo della pagina iniziale per l'utente
            private int m_PersonaID; // [INT]  ID della persona collegata all'utenza   
            [NonSerialized] private CPersona m_Persona; // [CPersona] Persona o azienda collegata all'utenza
            private string m_Note; // Note
            [NonSerialized] private CUserUffici m_Uffici;
            [NonSerialized] private CUserGroups m_Groups;
            private UserStatus m_UserStato;
            private string m_ClientCertificate;   // Percorso del certificato utente
            [NonSerialized] private CUserAuthorizationCollection m_Authorizations;
            [NonSerialized] private CModuleXUserCollection m_Modules;
            private DateTime? m_PasswordExpire;           // Data in cui la password scade
           
            /// <summary>
            /// Costruttore
            /// </summary>
            public CUser()
            {
                m_UserName = "";
                m_Nominativo = "";
                m_ExpireDate = default;
                m_ExpireLogin = default;
                m_ExpireDays = 0;
                // Me.m_IsLogged = False
                m_HomePage = "";
                // Me.m_Visible = True
                m_Flags = (int)UserFlags.None;
                m_PersonaID = 0;
                m_Settings = null;
                m_Persona = null;
                m_Uffici = null;
                m_UserStato = UserStatus.USER_NEW;
                m_email = "";
                m_Note = "";
                m_IconURL = "";
                m_ClientCertificate = "";
                m_PasswordExpire = default;
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="userName"></param>
            public CUser(string userName) : this()
            {
                SetUserName(userName);
                m_Nominativo = userName;
            }
             

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Users; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta la data oltre la quale é necessario resettare la password
            /// </summary>
            /// <returns></returns>
            public DateTime? PasswordExpire
            {
                get
                {
                    return m_PasswordExpire;
                }

                set
                {
                    var oldValue = m_PasswordExpire;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_PasswordExpire = value;
                    DoChanged("PasswordExpire", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione delle azioni consentite o negate esplicitamente all'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUserAuthorizationCollection Authorizations
            {
                get
                {
                    if (m_Authorizations is null)
                        m_Authorizations = new CUserAuthorizationCollection(this);
                    return m_Authorizations;
                }
            }

            /// <summary>
            /// Restituisce la collezione dei moduli esplicitamente autorizzati o negati per l'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CModuleXUserCollection Modules
            {
                get
                {
                    if (m_Modules is null)
                        m_Modules = new CModuleXUserCollection(this);
                    return m_Modules;
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso del certificato che identifica l'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ClientCertificate
            {
                get
                {
                    return m_ClientCertificate;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ClientCertificate;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ClientCertificate = value;
                    DoChanged("ClientCertificate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato dell'utente
            /// </summary>
            public UserStatus UserStato
            {
                get
                {
                    return m_UserStato;
                }

                set
                {
                    var oldValue = m_UserStato;
                    if (oldValue == value)
                        return;
                    m_UserStato = value;
                    DoChanged("UserStato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'avatar dell'utente
            /// </summary>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Controlla che la password sia valida
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool CheckPassword(string value)
            {
                var db = this.GetConnection();
                var tbl = db.AllEntities[this.getTableName()];
                using (var cursor = new DBWriter(tbl))
                {
                    cursor.MoveToObject(this);
                    var dic = cursor.GetCurrentRowAsDictionary();
                    return ((string)dic["UsrPwd"] == value);
                }
                //    bool ret;
                //ret = false;
                //dbRis = Databases.APPConn.ExecuteReader("SELECT [UsrPwd] FROM [tbl_Users] WHERE [ID]=" + ID);
                //if (dbRis.Read())
                //    ret = (Formats.ToString(dbRis["UsrPwd"]) ?? "") == (value ?? "");
                //dbRis.Dispose();
                //return ret;
            }

            /// <summary>
            /// Restituisce la collezione degli uffici abilitato per l'utente
            /// </summary>
            public Anagrafica.CUserUffici Uffici
            {
                get
                {
                    lock (this)
                    {
                        if (m_Uffici is null)
                            m_Uffici = new Anagrafica.CUserUffici(this);
                        return m_Uffici;
                    }
                }
            }

            //protected override void OnCreate(SystemEvent e)
            //{
            //    // Me.m_UserName = Sistema.Users.GetFirstAvailableUserName
            //    base.OnCreate(e);
            //    Users.OnUserCreated(new UserEventArgs(this));
            //}

            //protected override void OnDelete(SystemEvent e)
            //{
            //    base.OnDelete(e);
            //    Users.OnUserDeleted(new UserEventArgs(this));
            //}


            /// <summary>
            /// Restituisce il nome dell'utente corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UserName
            {
                get
                {
                    return m_UserName;
                }
            }

            /// <summary>
            /// Imposta il nome utente
            /// </summary>
            /// <param name="value"></param>
            public void SetUserName(string value)
            {
                m_UserName = DMD.Strings.Trim(value);
            }

            /// <summary>
            /// Restituisce o imposta il nome visualizzato per l'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nominativo
            {
                get
                {
                    return m_Nominativo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nominativo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nominativo = value;
                    DoChanged("Nominativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di scadenza della password
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? ExpireDate
            {
                get
                {
                    return m_ExpireDate;
                }

                set
                {
                    var oldValue = m_ExpireDate;
                    if (oldValue == value == true)
                        return;
                    m_ExpireDate = value;
                    DoChanged("ExpireDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di scadenza dell'account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? ExpireLogin
            {
                get
                {
                    return m_ExpireLogin;
                }

                set
                {
                    var oldValue = m_ExpireLogin;
                    if (oldValue == value == true)
                        return;
                    m_ExpireLogin = value;
                    DoChanged("ExpireLogin", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di giorni di cui prolungare la data di scadenza dell'account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ExpireDays
            {
                get
                {
                    return m_ExpireDays;
                }

                set
                {
                    int oldValue = m_ExpireDays;
                    if (oldValue == value)
                        return;
                    m_ExpireDays = value;
                    DoChanged("ExpireDays", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'e-mail principale dell'account
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string eMail
            {
                get
                {
                    return m_email;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_email;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_email = value;
                    DoChanged("eMail", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione lunga per l'utente
            /// </summary>
            public string Notes
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la pagina principale per l'utente
            /// </summary>
            public string HomePage
            {
                get
                {
                    return m_HomePage;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_HomePage;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_HomePage = value;
                    DoChanged("HomePage", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se l'utente è visibile
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Visible
            {
                get
                {
                    return !DMD.RunTime.TestFlag(this.Flags, UserFlags.Hidden); // Me.m_Visible
                }

                set
                {
                    if (Visible == value)
                        return;
                    // Me.m_Visible = value
                    m_Flags = (int) DMD.RunTime.SetFlag(this.Flags, UserFlags.Hidden, !value);
                    DoChanged("Visible", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta i flag
            /// </summary>
            public new UserFlags Flags
            {
                get
                {
                    return (UserFlags)base.Flags;
                }

                set
                {
                    base.Flags = (int)value;
                }
            }

            /// <summary>
            /// Restituisce la collezione delle impostazioni utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CSettings Settings
            {
                get
                {
                    lock (this)
                    {
                        if (m_Settings is null)
                            m_Settings = new CSettings(this);
                        return m_Settings;
                    }
                }
            }

            void ISettingsOwner.NotifySettingsChanged(SettingsChangedEventArgs e)
            {
                this.SetChanged(true);
            }

            CSettings ISettingsOwner.Settings
            {
                get
                {
                    return this.Settings;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona a cui é associato l'utente
            /// </summary>
            public int PersonaID
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_PersonaID);
                }

                set
                {
                    int oldValue = PersonaID;
                    if (oldValue == value)
                        return;
                    m_PersonaID = value;
                    m_Persona = null;
                    DoChanged("PersonaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona associata all'utente
            /// </summary>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_PersonaID);
                    return m_Persona;
                }

                set
                {
                    var oldValue = Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_PersonaID = DBUtils.GetID(value, 0);
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_UserName;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_UserName);
            }



            /// <summary>
            /// Rinomina l'utente
            /// </summary>
            /// <param name="newUserName"></param>
            /// <returns>
            /// 0 - Nessun errore
            /// 1 - Nuovo nome utente non valido
            /// 2 - Il nome utente esiste già 
            /// 3 - Errore durante il salvataggio
            /// 255 - Errore generico
            /// </returns>
            public void Rename(string newUserName)
            {
                switch (DMD.Strings.UCase(UserName) ?? "")
                {
                    case "ADMIN":
                    case "SYSTEM":
                    case "GUEST":
                        {
                            throw new InvalidOperationException("Impossibile rinominare l'account di sistema: " + UserName);
                             
                        }
                }

                
                newUserName = minidom.Sistema.Users.GetValidUserName(newUserName);
                if ((DMD.Strings.LCase(UserName) ?? "") == (DMD.Strings.LCase(newUserName) ?? ""))
                    return;

                if (DMD.Strings.Len(newUserName) < Users.MINUSERNAMELEN)
                    throw new DMDException("Il nome utente non rispetta i criteri definiti");
                
                var item = minidom.Sistema.Users.GetItemByName(newUserName);
                if (item is object && item.Stato == ObjectStatus.OBJECT_VALID)
                    throw new DMDException("L'utete esiste già");

                this.SetUserName(newUserName);
                this.Save(true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CUser)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CUser obj)
            {
                int ret = DMD.Strings.Compare(Nominativo, obj.Nominativo, true);
                if (ret == 0) ret = DMD.Strings.Compare(UserName, obj.UserName, true);
                return ret;
            }

            /// <summary>
            /// Restituisce l'accesso della sessione corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CLoginHistory CurrentLogin
            {
                get
                {
                    return ApplicationContext.CurrentLogin;
                }
            }

            /// <summary>
            /// Restituisce le informazioni sull'accesso più recente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CLoginHistory GetLastLogin()
            {
                using (var cursor = new CLoginHistoryCursor())
                {
                    cursor.UserID.Value = DBUtils.GetID(this, 0);
                    cursor.LoginTime.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    cursor.LoginTime.Value = DMD.DateUtils.DateAdd(DateTimeInterval.Hour, -1, DateTime.Now);
                    cursor.LoginTime.Operator = OP.OP_LT;
                    // If (Not cursor.EOF) Then cursor.MoveNext()
                    return cursor.Item;
                }
            }

            private bool validatePassword(string value)
            {
                if (DMD.Strings.Len(value) < minidom.Sistema.Users.PWDMINLEN)
                    return false;
                var pattern = minidom.Sistema.Users.PWDPATTERN;
                if (!string.IsNullOrEmpty(pattern))
                {
                    var reg = new System.Text.RegularExpressions.Regex(pattern);
                    return reg.IsMatch(value);
                }
                else
                {
                    return true;
                }
            }

            /// <summary>
            /// Modifica la password dell'utente
            /// </summary>
            /// <param name="oldPassword"></param>
            /// <param name="newPassword"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public void ChangePassword(string oldPassword, string newPassword)
            {
                if (!validatePassword(newPassword))
                    throw new BadPasswordException("La nuova password non rispetta i criteri di sicurezza richiesti");

                if (!this.CheckPassword(oldPassword))
                    throw new BadPasswordException("La vecchia password non corrisponde a quella memorizzata");

                this.m_PasswordExpire = DMD.DateUtils.DateAdd("d", minidom.Sistema.Users.DEFAULT_PASSWORD_INTERVAL, DMD.DateUtils.Now());

                var db = this.GetConnection();
                var tbl = db.AllEntities[this.getTableName()];
                using(var writer = new DBWriter (tbl))
                {
                    writer.MoveToObject(this);
                    writer.Write("UsrPwd", newPassword);
                    writer.Write("PasswordExpire", this.m_PasswordExpire);
                    writer.Update();
                }
                      
                var e = new UserEventArgs(this);
                this.OnPasswordChanged(e);
                var repo = minidom.Sistema.Users;
                repo.notifyPasswordChanged(e);
                repo.DispatchEvent(new EventDescription("password_changed", "Password modificata per l'utente: " + UserName, e));
            }

            /// <summary>
            /// Genera l'evento PasswordChanged
            /// </summary>
            /// <param name="e"></param>
            private void OnPasswordChanged(UserEventArgs e)
            {
                PasswordChanged?.Invoke(this, e);
            }

            /// <summary>
            /// Forza la password dell'utente
            /// </summary>
            /// <param name="newPassword"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public void ForcePassword(string newPassword)
            {
                this.m_PasswordExpire = DMD.DateUtils.DateAdd(DateTimeInterval.Day, minidom.Sistema.Users.DEFAULT_PASSWORD_INTERVAL, DMD.DateUtils.Now());
                var tbl = this.GetConnection().AllEntities[this.getTableName()];
                using(var writer = new DBWriter(tbl))
                {
                    writer.MoveToObject(this);
                    writer.Write("UsrPwd", newPassword);
                    writer.Write("PasswordExpire", this.m_PasswordExpire);
                    writer.Update();
                }
                 
                var e = new UserEventArgs(this);
                this.OnPasswordChanged(e);
                var repo = minidom.Sistema.Users;
                repo.notifyPasswordChanged(e);
                repo.DispatchEvent(new EventDescription("password_changed", "Password forzata per l'utente: " + UserName, e));
            }

            /// <summary>
            /// Restituisce un array contenente tutti i gruppi a cui appartiene l'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUserGroups Groups
            {
                get
                {
                    if (m_Groups is null)
                    {
                        m_Groups = new CUserGroups();
                        m_Groups.Load(this);
                    }

                    return m_Groups;
                }
            }

            /// <summary>
            /// Restituisce true se l'utente appartiene al gruppo Administrators
            /// </summary>
            /// <returns></returns>
            public bool IsAdministrator()
            {
                return IsInGroup("Administrators");
            }

            /// <summary>
            /// Restituisce true se l'utente è presente nel gruppo
            /// </summary>
            /// <param name="groupName"></param>
            /// <returns></returns>
            public bool IsInGroup(string groupName)
            {
                return DMD.Booleans.ValueOf(Groups.Contains(groupName));
            }

            /// <summary>
            /// Restituisce vero se esiste un utente loggato nella sessione corrente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsLogged()
            {
                return ApplicationContext.IsUserLogged(this);
            }

            /// <summary>
            /// Restituisce vero se l'account è scaduto
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsExpired()
            {
                if (ExpireLogin.HasValue)
                {
                    return DMD.DateUtils.Now() >= DMD.DateUtils.DateAdd("d", ExpireDays, ExpireLogin.Value);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Restituisce vero se esiste un utente loggato nella sessione corrente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsImpersonating()
            {
                return false;
            }

            /// <summary>
            /// Effettua il login
            /// </summary>
            /// <param name="password"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
            internal CLoginHistory LogIn(string password, CKeyCollection parameters)
            {
                if (DMD.Strings.LCase(UserName) == "system" | DMD.Strings.LCase(UserName) == "guest")
                    throw new InvalidConstraintException("L'utente [" + UserName + "] è un utente di sistema");

                //string dbSQL = "UPDATE [tbl_Users] SET [IsLogged]=TRUE WHERE [ID]=" + DBUtils.GetID(this) + ";";
                //db.ExecuteCommand(dbSQL);
                // Me.m_IsLogged = True
                var lastLogin = new CLoginHistory(this, parameters);
                lastLogin.Save();

                //this.IsLogged 

                ApplicationContext.CurrentUser = this;
                ApplicationContext.CurrentLogin = lastLogin;
                return lastLogin;
            }



            /// <summary>
            /// Effettua la disconnessione dell'utente corrente
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool LogOut(LogOutMethods method)
            {
                var currLogin = ApplicationContext.CurrentLogin;
                if (currLogin is object)
                {
                    currLogin.SetLogoutMethod(method);
                    currLogin.SetLogOutTime(DMD.DateUtils.Now());
                    currLogin.Save(true);
                }

                ApplicationContext.CurrentLogin = null;
                ApplicationContext.CurrentUser = Users.KnownUsers.GuestUser;
                minidom.Sistema.Users.OnUserLoggedOut(new UserLogoutEventArgs(this, method));
                return true;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Users";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_UserName = reader.Read("UserName", this.m_UserName);
                this.m_ExpireDate = reader.Read("ExpireDate", this.m_ExpireDate);
                this.m_ExpireLogin = reader.Read("ExpireLogin", this.m_ExpireLogin);
                this.m_ExpireDays = reader.Read("ExpireDays", this.m_ExpireDays);
                this.m_Nominativo = reader.Read("Nominativo", this.m_Nominativo);
                this.m_email = reader.Read("email", this.m_email);
                this.m_Note = reader.Read("Notes", this.m_Note);
                this.m_PersonaID = reader.Read("Persona", this.m_PersonaID);
                this.m_HomePage = reader.Read("HomePage", this.m_HomePage);
                // Me.m_Visible = reader.Read("Visible", Me.m_Visible)
                this.m_UserStato = reader.Read("UserStato", this.m_UserStato);
                this.m_IconURL = reader.Read("IconURL", this.m_IconURL);
                this.m_ClientCertificate = reader.Read("ClientCert", this.m_ClientCertificate);
                this.m_PasswordExpire = reader.Read("PasswordExpire", this.m_PasswordExpire);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("UserName", m_UserName);
                writer.Write("ExpireDate", m_ExpireDate);
                writer.Write("ExpireLogin", m_ExpireLogin);
                writer.Write("ExpireDays", m_ExpireDays);
                writer.Write("Nominativo", m_Nominativo);
                writer.Write("email", m_email);
                writer.Write("Notes", m_Note);
                writer.Write("Persona", DBUtils.GetID(m_Persona, m_PersonaID));
                writer.Write("HomePage", m_HomePage);
                // writer.Write("Visible", Me.m_Visible)
                writer.Write("UserStato", m_UserStato);
                writer.Write("IconURL", m_IconURL);
                writer.Write("ClientCert", m_ClientCertificate);
                writer.Write("PasswordExpire", m_PasswordExpire);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("UserName", typeof(string), 255);
                c = table.Fields.Ensure("ExpireDate", typeof(DateTime), 1);
                c = table.Fields.Ensure("ExpireLogin", typeof(DateTime), 1);
                c = table.Fields.Ensure("ExpireDays", typeof(int), 1);
                c = table.Fields.Ensure("Nominativo", typeof(string), 255);
                c = table.Fields.Ensure("email", typeof(string), 255);
                c = table.Fields.Ensure("Notes", typeof(string), 0);
                c = table.Fields.Ensure("Persona", typeof(int), 1);
                c = table.Fields.Ensure("HomePage", typeof(string), 255);
                c = table.Fields.Ensure("UserStato", typeof(int), 1);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("ClientCert", typeof(string), 0);
                c = table.Fields.Ensure("PasswordExpire", typeof(DateTime), 1);

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxUser", new string[] { "UserName", "Nominativo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxExpireInfo", new string[] { "ExpireDate" , "ExpireLogin", "ExpireDays", "PasswordExpire" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzi", new string[] { "email", "HomePage" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "Persona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUserStato", new string[] { "UserStato", "Flags" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Notes", typeof(string), 0);
                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
                //c = table.Fields.Ensure("ClientCert", typeof(string), 0);


            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("UserName", m_UserName);
                writer.WriteAttribute("Nominativo", m_Nominativo);
                writer.WriteAttribute("email", m_email);
                writer.WriteAttribute("ExpireDate", m_ExpireDate);
                writer.WriteAttribute("ExpireLogin", m_ExpireLogin);
                writer.WriteAttribute("ExpireDays", m_ExpireDays);
                writer.WriteAttribute("HomePage", m_HomePage);
                writer.WriteAttribute("PersonaID", PersonaID);
                // writer.WriteAttribute("Visible", Me.m_Visible)
                writer.WriteAttribute("UserStato", m_UserStato);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("ClientCert", m_ClientCertificate);
                writer.WriteAttribute("PasswordExpire", m_PasswordExpire);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
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
                    case "UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nominativo":
                        {
                            m_Nominativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "email":
                        {
                            m_email = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ExpireDate":
                        {
                            m_ExpireDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ExpireLogin":
                        {
                            m_ExpireLogin = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ExpireDays":
                        {
                            m_ExpireDays = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "HomePage":
                        {
                            m_HomePage = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PersonaID":
                        {
                            m_PersonaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    
                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UserStato":
                        {
                            m_UserStato = (UserStatus)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PasswordExpire":
                        {
                            m_PasswordExpire = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IsLogged":
                        {
                            break;
                        }

                    case "ClientCert":
                        {
                            m_ClientCertificate = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            string IFonte.Nome
            {
                get
                {
                    return m_Nominativo;
                }
            }

            string IFonte.IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// OnBeforeDelete
            /// </summary>
            /// <param name="e"></param>
            protected override void OnBeforeDelete(DMDEventArgs e)
            {
                switch (DMD.Strings.UCase(UserName) ?? "")
                {
                    case "ADMIN":
                    case "SYSTEM":
                    case "GUEST":
                        {
                            throw new InvalidOperationException("Impossibile eliminare l'account di sistema: " + UserName);
                            break;
                        }
                }

                base.OnBeforeDelete(e);
            }

            /// <summary>
            /// OnBeforeSave
            /// </summary>
            /// <param name="e"></param>
            protected override void OnBeforeSave(DMDEventArgs e)
            {
                switch (DMD.Strings.UCase(UserName) ?? "")
                {
                    case "ADMIN":
                    case "SYSTEM":
                    case "GUEST":
                        {
                            if (Stato != ObjectStatus.OBJECT_VALID)
                            {
                                throw new InvalidOperationException("Impossibile eliminare l'account di sistema: " + UserName);
                            }

                            break;
                        }
                }

                base.OnBeforeSave(e);
            }

            //protected override void OnAfterSave(SystemEvent e)
            //{
            //    base.OnAfterSave(e);
            //    Users.UpdateCached(this);
            //}

            /// <summary>
            /// Restituisce true se l'utente è valido
            /// </summary>
            /// <returns></returns>

            public bool IsValid()
            {
                return this.Stato == ObjectStatus.OBJECT_VALID 
                       && this.UserStato == UserStatus.USER_ENABLED;
            }

            /// <summary>
            /// Inizializza l'oggetto
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public CUser InitializeFrom(CUser obj)
            {
                //base.InitializeFrom(value);
                //m_Uffici = null;
                //m_Authorizations = null;
                //m_Settings = null;
                //m_Groups = null;
                this.m_UserName = obj.m_UserName;
                this.m_Nominativo = obj.m_Nominativo;
                this.m_IconURL = obj.m_IconURL;
                this.m_email = obj.m_email;
                this.m_ExpireDate = obj.m_ExpireDate;
                this.m_ExpireLogin = obj.m_ExpireLogin;
                this.m_ExpireDays = obj.m_ExpireDays;
                this.m_HomePage = obj.m_HomePage;
                this.PersonaID = obj.PersonaID;
                this.m_Flags = obj.m_Flags;
                this.m_Note = obj.m_Note;
                this.m_UserStato = obj.m_UserStato;
                this.m_ClientCertificate = obj.m_ClientCertificate;
                this.m_PasswordExpire = obj.m_PasswordExpire;
                //private CUserSettings m_Settings; // [CUserSettings] Collezione di parametri aggiuntivi
                ;

                return this;
            }

            void ISupportInitializeFrom.InitializeFrom(object value)
            {
                this.InitializeFrom((CUser)value);
            }

            /// <summary>
            /// Crea una copia identica dell'oggetto
            /// </summary>
            /// <returns></returns>
            public CUser Clone()
            {
                return (CUser)MemberwiseClone();
            }

            object ICloneable.Clone() { return this.Clone();  }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CUser) && this.Equals((CUser)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CUser obj)
            {
                return base.Equals(obj)
                                && DMD.Strings.EQ(this.m_UserName, obj.m_UserName)
                                && DMD.Strings.EQ(this.m_Nominativo, obj.m_Nominativo)
                                && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                                && DMD.Strings.EQ(this.m_email, obj.m_email)
                                && DMD.DateUtils.EQ(this.m_ExpireDate, obj.m_ExpireDate)
                                && DMD.DateUtils.EQ(this.m_ExpireLogin, obj.m_ExpireLogin)
                                && DMD.Integers.EQ(this.m_ExpireDays, obj.m_ExpireDays)
                                && DMD.Strings.EQ(this.m_HomePage, obj.m_HomePage)
                                && DMD.Integers.EQ(this.PersonaID, obj.PersonaID)
                                && DMD.Integers.EQ(this.m_Flags, obj.m_Flags)
                                && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                                && DMD.Integers.EQ((int)this.m_UserStato, (int)obj.m_UserStato)
                                && DMD.Strings.EQ(this.m_ClientCertificate, obj.m_ClientCertificate)
                                && DMD.DateUtils.EQ(this.m_PasswordExpire, obj.m_PasswordExpire)
            //private CUserSettings m_Settings; // [CUserSettings] Collezione di parametri aggiuntivi
            ;

        }

            CSetting ISettingsOwner.CreateSetting(object args)
            {
                var ret = new CSetting();
                return ret;
            }
        }
    }
}