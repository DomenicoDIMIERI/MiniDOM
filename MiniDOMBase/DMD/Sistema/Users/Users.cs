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
using System.Globalization;
using static DMD.Anagrafica;

namespace minidom
{
  namespace repositories
  {

    /// <summary>
    /// Repository degli utenti
    /// </summary>
    [Serializable]
    public sealed partial class CUsersClass
        : CModulesClass<Sistema.CUser>
    {

      /// <summary>
      /// Evento generato quando un utente effettua l'accesso correttamente
      /// </summary>
      public event UserLoggedInEventHandler UserLoggedIn;

      /// <summary>
      /// Evento generato quando un utente effettua l'accesso correttamente
      /// </summary>
      /// <param name="e"></param>
      public delegate void UserLoggedInEventHandler(UserLoginEventArgs e);

      /// <summary>
      /// Evento generato quando un utente si disconnette o viene disconnesso
      /// </summary>
      public event UserLoggedOutEventHandler UserLoggedOut;

      /// <summary>
      /// Evento generato quando un utente si disconnette o viene disconnesso
      /// </summary>
      /// <param name="e"></param>
      public delegate void UserLoggedOutEventHandler(UserLogoutEventArgs e);

      ///// <summary>
      ///// Evento generato quando un utente viene creato
      ///// </summary>
      //public event UserCreatedEventHandler UserCreated;

      ///// <summary>
      ///// Evento generato quando un utente viene creato
      ///// </summary>
      ///// <param name="e"></param>
      //public delegate void UserCreatedEventHandler(UserEventArgs e);

      //public event UserDeletedEventHandler UserDeleted;

      //public delegate void UserDeletedEventHandler(UserEventArgs e);

      /// <summary>
      /// Evento generato quando la password di un utente viene modificata
      /// </summary>
      public event UserPasswordChangedEventHandler UserPasswordChanged;

      /// <summary>
      /// Evento generato quando la password di un utente viene modificata
      /// </summary>
      /// <param name="e"></param>
      public delegate void UserPasswordChangedEventHandler(UserEventArgs e);

      /// <summary>
      /// Evento generato quando viene tentato un accesso non autorizzato
      /// </summary>
      /// <remarks></remarks>
      public event LoginErrorEventHandler LoginError;

      /// <summary>
      /// Evento generato quando viene tentato un accesso non autorizzato
      /// </summary>
      /// <param name="e"></param>
      /// <remarks></remarks>
      public delegate void LoginErrorEventHandler(UserLoginException e);

      /// <summary>
      /// Restituisce o imposta la lunghezza minima ammessa per i nomi utente
      /// </summary>
      public int MINUSERNAMELEN
      {
        get
        {
          return minidom.Sistema.ApplicationContext.Settings.GetValueInt("Sistema.Users.MINUSERNAMELEN", 3);
        }
        set
        {
          if (value < 1) throw new ArgumentOutOfRangeException("I nomi utente non possono essere vuoti");
          Sistema.ApplicationContext.Settings.SetValueInt("Sistema.Users.MINUSERNAMELEN", value);
        }
      }

      /// <summary>
      /// Restituisce o imposta la dimensione minima della password
      /// </summary>
      public int PWDMINLEN
      {
        get
        {
          return minidom.Sistema.ApplicationContext.Settings.GetValueInt("Sistema.Users.PWDMINLEN", 6);
        }
        set
        {
          Sistema.ApplicationContext.Settings.SetValueInt("Sistema.Users.PWDMINLEN", value);
        }
      }

      /// <summary>
      /// Restituisce o imposta la durata massima di una password
      /// </summary>
      public int DEFAULT_PASSWORD_INTERVAL
      {
        get
        {
          return minidom.Sistema.ApplicationContext.Settings.GetValueInt("Sistema.Users.DEFAULT_PASSWORD_INTERVAL", 180);
        }
        set
        {
          Sistema.ApplicationContext.Settings.SetValueInt("Sistema.Users.DEFAULT_PASSWORD_INTERVAL", value);
        }
      }

      /// <summary>
      /// Restituisce o imposta il pattern che deve verufucare la password
      /// </summary>
      public string PWDPATTERN
      {
        get
        {
          return minidom.Sistema.ApplicationContext.Settings.GetValueString("Sistema.Users.PWDPATTERN", "");
        }
        set
        {
          Sistema.ApplicationContext.Settings.SetValueString("Sistema.Users.PWDPATTERN", value);
        }
      }

      // Private m_Users As CKeyCollection(Of CUser)

      /// <summary>
      /// Costruttore
      /// </summary>
      public CUsersClass()
          : base("modUsers", typeof(Sistema.CUserCursor), -1)
      {
      }

      /// <summary>
      /// Inizializza il modulo
      /// </summary>
      public override void Initialize()
      {
        base.Initialize();
        minidom.Anagrafica.PersonaCreated += HandlePeronaModified;
        minidom.Anagrafica.PersonaDeleted += HandlePeronaModified;
        minidom.Anagrafica.PersonaModified += HandlePeronaModified;
        minidom.Anagrafica.PersonaMerged += HandlePeronaMerged;
        minidom.Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
      }

      /// <summary>
      /// Termina il modulo
      /// </summary>
      public override void Terminate()
      {
        minidom.Anagrafica.PersonaCreated -= HandlePeronaModified;
        minidom.Anagrafica.PersonaDeleted -= HandlePeronaModified;
        minidom.Anagrafica.PersonaModified -= HandlePeronaModified;
        minidom.Anagrafica.PersonaMerged -= HandlePeronaMerged;
        minidom.Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
        base.Terminate();
      }

      private CLoginHistoryRepository m_LoginHistories = null;

      /// <summary>
      /// Repository di oggetti di tipo <see cref="CLoginHistory"/>
      /// </summary>
      public CLoginHistoryRepository LoginHistories
      {
        get
        {
          if (m_LoginHistories is null) m_LoginHistories = new CLoginHistoryRepository();
          return m_LoginHistories;
        }
      }

      /// <summary>
      /// Gestisce l'evento PersonaMoficied
      /// </summary>
      /// <param name="e"></param>
      private void HandlePeronaModified(PersonaEventArgs e)
      {
        lock (this.cacheLock)
        {


        }
      }

      /// <summary>
      /// Gestisce l'evento PersonaUnmerged
      /// </summary>
      /// <param name="e"></param>
      private void HandlePeronaMerged(MergePersonaEventArgs e)
      {
        lock (this.cacheLock)
        {
          var mi = e.MI;
          var persona1 = mi.Persona1;
          var persona2 = mi.Persona2;
          CMergePersonaRecord rec;

          //dbSQL = DMD.Strings.JoinW("SELECT [ID] FROM [tbl_Users] WHERE [Persona]=", DBUtils.GetID(persona1).ToString());
          //dbRis = Databases.APPConn.ExecuteReader(dbSQL);
          using (var cursor = new CUserCursor())
          {
            cursor.IDPersona.Value = mi.IDPersona2;
            cursor.IgnoreRights = true;
            while (cursor.Read())
            {
              rec = new CMergePersonaRecord();
              rec.NomeTabella = "tbl_Users";
              rec.FieldName = "Persona";
              rec.RecordID = DBUtils.GetID(cursor.Item, 0);
              mi.TabelleModificate.Add(rec);

              cursor.Item.Persona = mi.Persona1;
              cursor.Item.Save();
            }
          }


        }
      }

      /// <summary>
      /// Gestisce l'evento PersonaUnmerged
      /// </summary>
      /// <param name="e"></param>
      private void HandlePeronaUnMerged(MergePersonaEventArgs e)
      {
        lock (this.cacheLock)
        {
          var mi = e.MI;
#if (!DEBUG)
                try {
#endif
          // Tabella Login
          var items = mi.GetAffectedRecorsIDs("tbl_Users", "Persona");
          //if (!string.IsNullOrEmpty(items))
          //    Databases.APPConn.ExecuteCommand(DMD.Strings.JoinW("UPDATE [tbl_Users] SET [Persona]=", DBUtils.GetID(persona1).ToString(), " WHERE [ID] In (", items, ")"));
          using (var cursor = new CUserCursor())
          {
            cursor.ID.ValueIn(items);
            cursor.IgnoreRights = true;
            while (cursor.Read())
            {
              cursor.Item.PersonaID = mi.IDPersona2;
              cursor.Item.Save();
            }
          }
#if (!DEBUG)
                } catch (Exception ex) { Sistema.Events.NotifyUnhandledException(ex); }
#endif
        }
      }

      /// <summary>
      /// Genera l'evento PasswordChanged
      /// </summary>
      /// <param name="e"></param>
      internal void notifyPasswordChanged(UserEventArgs e)
      {
        UserPasswordChanged?.Invoke(e);
      }

      /// <summary>
      /// Forza il sistema ad agire in modalità supervisore
      /// </summary>
      public void Trap()
      {
        if (
               Sistema.ApplicationContext.CurrentUser is object
            && Sistema.ApplicationContext.CurrentUser.UserName == "SYSTEM"
            )
          return;

        Sistema.ApplicationContext.CurrentUser = KnownUsers.SystemUser;
      }

      /// <summary>
      /// Esce dalla modalità supervisore
      /// </summary>
      public void UnTrap()
      {
        if (Sistema.ApplicationContext.CurrentUser is null || Sistema.ApplicationContext.CurrentUser.UserName != "SYSTEM")
          return;
        Sistema.ApplicationContext.CurrentUser = null;
      }

      /// <summary>
      /// Conta il numero di utenti correntemente connessi
      /// </summary>
      /// <returns></returns>
      public int CountLoggedUsers()
      {
        int cnt = 0;
        foreach (Sistema.CUser user in Sistema.Users.LoadAll())
        {
          if (user.IsLogged())
            cnt += 1;
        }

        return cnt;
      }

      /// <summary>
      /// Elimina lo storico di tutti gli utenti
      /// </summary>
      public void DeleteUsersHistory()
      {
        var tbl = this.Database.AllEntities["tbl_LoginHistory"];
        this.Database.DeleteAll(tbl);
      }

      /// <summary>
      /// Disconnette tutti gli utenti
      /// </summary>
      public void DisconnectAll()
      {
        foreach (Sistema.CUser user in Sistema.Users.LoadAll())
        {
          if (user.IsLogged())
            user.LogOut(LogOutMethods.LOGOUT_REMOTEDISCONNECT);
        }

      }

      /// <summary>
      /// Crea un nuovo utente con il nome specificato
      /// </summary>
      /// <param name="userName"></param>
      /// <returns></returns>
      public Sistema.CUser CreateUser(string userName)
      {
        lock (this.cacheLock)
        {
          var item = this.GetItemByName(userName);
          if (item is object)
            throw new ArgumentException("Esiste già un utente con il nome: " + userName);

          item = new Sistema.CUser();
          item.SetUserName(userName);
          item.Stato = ObjectStatus.OBJECT_VALID;
          item.UserStato = UserStatus.USER_NEW;
          item.Save();

          return item;
        }
      }

      public bool IsUserLogged(CUser user)
      {
        if (user is null) throw new ArgumentNullException("user");
        if (user.ID == 0) return false;
        switch (Strings.LCase(Strings.Trim(user.UserName)) ?? "")
        {
          case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "system", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
          case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "guest", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
            {
              return false;
            }

          default:
            {
              for (int i = 0, loopTo = m_Info.Count - 1; i <= loopTo; i++)
              {
                var info = m_Info[i];
                if (Databases.GetID(info.CurrentUser) == userID)
                  return true;
              }

              return false;
            }
        }
      }

      /// <summary>
      /// Crea un nuovo utente con il nome che inizia con la stringa specificata
      /// </summary>
      /// <param name="baseName">[in] Nome utente da creare. Se l'utente esiste già il programma aggiunge un numero in modo che il nome utente sia univoco</param>
      /// <returns></returns>
      public Sistema.CUser CreateNewUser(string baseName)
      {
        lock (this.cacheLock)
        {
          var item = new Sistema.CUser();
          item.SetUserName(this.GetAvailableUserName(baseName));
          item.Stato = ObjectStatus.OBJECT_VALID;
          item.UserStato = UserStatus.USER_NEW;
          item.Save();
          return item;
        }
      }

      /// <summary>
      /// Restitusice il primo nome disponibile
      /// </summary>
      /// <returns></returns>
      public string GetFirstAvailableUserName()
      {
        return GetAvailableUserName("Utente");
      }

      /// <summary>
      /// Restituisce il primo nome disponibile
      /// </summary>
      /// <param name="baseName"></param>
      /// <returns></returns>
      public string GetAvailableUserName(string baseName)
      {
        baseName = DMD.Strings.Trim(baseName);
        string nome = baseName;
        int i;
        bool t = GetItemByName(nome) is null;
        i = 1;
        while (!t)
        {
          nome = baseName + " (" + i + ")";
          t = GetItemByName(nome) is null;
          i = i + 1;
        }

        return nome;
      }

      /// <summary>
      /// Rimuove eventuali caratteri non validi
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public string GetValidUserName(string value)
      {
        const string chars = "/-#'\"@?,;: \r\n\t\f";

        value = DMD.Strings.Trim(value);

        var ret = new System.Text.StringBuilder();
        foreach (var ch in value)
        {
          if (chars.IndexOf(ch) >= 0)
          {
            ret.Append("_");
          }
          else
          {
            ret.Append(ch);
          }
        }
        return ret.ToString();
      }

      /// <summary>
      /// Formatta il codice di errore
      /// </summary>
      /// <param name="code"></param>
      /// <returns></returns>
      public string TranslateLoginErrorCode(int code)
      {
        string ret;
        switch (code)
        {
          case 0:
            {
              ret = "Nessun errore";
              break;
            }

          case -1:
            {
              ret = "Nome utente non valido";
              break;
            }

          case -2:
            {
              ret = "Password non valida";
              break;
            }

          case -3:
            {
              ret = "Nome utente non esiste";
              break;
            }

          case -4:
            {
              ret = "Password non corrispondente al nome utente";
              break;
            }

          case -5:
            {
              ret = "L'utente è stato disabilitato";
              break;
            }

          case -6:
            {
              ret = "L'utente è stato eliminato";
              break;
            }

          case -7:
            {
              ret = "L'utente è in attesa di essere attivato";
              break;
            }

          case -8:
            {
              ret = "L'utente non ha ancora confermato la sua iscrizione al sito";
              break;
            }

          case -9:
            {
              ret = "L'utente è stato sospeso";
              break;
            }

          default:
            {
              ret = "Errore generico sconosciuto";
              break;
            }
        }

        return ret;
      }

      /// <summary>
      /// Genera l'evento LoginError
      /// </summary>
      /// <param name="e"></param>
      internal void OnLoginError(UserLoginException e)
      {
        LoginError?.Invoke(e);

        if (e is BadPasswordException)
        {
          Module.DispatchEvent(new Sistema.EventDescription("LogIn_Error", "Tentativo di accesso non autorizzato: " + e.UserName, new string[] { e.Message, ((BadPasswordException)e).BadPassword }));
        }
        else
        {
          Module.DispatchEvent(new Sistema.EventDescription("LogIn_Error", "Tentativo di accesso non autorizzato: " + e.UserName, e.Message));
        }
      }

      //internal void OnUserCreated(UserEventArgs e)
      //{
      //    Module.DispatchEvent(new Sistema.EventDescription("Delete", "[" + e.User.UserName + "] è stato creato", e));
      //    UserCreated?.Invoke(e);
      //}

      //internal void OnUserDeleted(UserEventArgs e)
      //{
      //    Module.DispatchEvent(new Sistema.EventDescription("Delete", "[" + e.User.UserName + "] è stato eliminato", e));
      //    UserDeleted?.Invoke(e);
      //}

      /// <summary>
      /// Genera l'evento UserLogged
      /// </summary>
      /// <param name="e"></param>
      internal void OnUserLoggedIn(UserLoginEventArgs e)
      {
        UserLoggedIn?.Invoke(e);
        Module.DispatchEvent(new Sistema.EventDescription("LogIn", "[" + e.User.UserName + "] ha effettuato l'accesso", e));
      }

      /// <summary>
      /// Genera l'evento UserLogout
      /// </summary>
      /// <param name="e"></param>
      internal void OnUserLoggedOut(UserLogoutEventArgs e)
      {
        UserLoggedOut?.Invoke(e);
        Module.DispatchEvent(new Sistema.EventDescription("LogOut", "[" + e.User.UserName + "] ha effettuato il logout", e));
      }


      // Effettua il LogIn dell'utente corrente e restituisce un codice
      // numerico che indica il tipo di errore:
      // 0  	Utente loggato correttamente
      // -1	Nome utente non valido
      // -2	Password non valida
      // -3	Nome utente non esiste
      // -4 	Password non valida per il nome utente
      // -5   L'utente è stato disabilitato
      // -6   L'utente è stato eliminato
      // -7   L'utente è in attesa di essere attivato
      // -8   L'utente non ha ancora confermato la sua iscrizione al sito
      // -9   L'utente è stato sospeso
      // -10  L'account utente è scaduto 
      // -255 Errore generico sconosciuto

      /// <summary>
      /// Effettua il login
      /// </summary>
      /// <param name="userName"></param>
      /// <param name="password"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      public Sistema.CUser LogIn(string userName, string password, CKeyCollection parameters)
      {
        Sistema.CUser user;
        Exception e;
        userName = DMD.Strings.Trim(userName);
        user = GetItemByName(userName);
        if (user is null || user.Stato != ObjectStatus.OBJECT_VALID)
        {
          e = new UserNotFoundException(userName);
          Sistema.Users.OnLoginError((UserLoginException)e);
          throw e;
        }

        if (!user.CheckPassword(password))
        {
          e = new BadPasswordException(userName, "Password non valida", password);
          Sistema.Users.OnLoginError((UserLoginException)e);
          throw e;
        }

        switch (user.UserStato)
        {
          case Sistema.UserStatus.USER_DISABLED:
          case Sistema.UserStatus.USER_DELETED:
          case Sistema.UserStatus.USER_NEW:
          case Sistema.UserStatus.USER_SUSPENDED:
            {
              e = new UserNotEnabledException(userName, user.UserStato);
              Sistema.Users.OnLoginError((UserLoginException)e);
              throw e;
            }

          default:
            {
              if (user.IsExpired())
              {
                e = new UserExpiredException(userName);
                Sistema.Users.OnLoginError((UserLoginException)e);
                throw e;
              }
              else if (DMD.RunTime.TestFlag(user.Flags, Sistema.UserFlags.ForceChangePassword))
              {
                e = new UserForcePwdPasswordException(userName);
                Sistema.Users.OnLoginError((UserLoginException)e);
                throw e;
              }
              else if (user.PasswordExpire.HasValue && DMD.DateUtils.Compare(user.PasswordExpire, DMD.DateUtils.Now()) < 0)
              {
                e = new PasswordExpiredException(userName);
                Sistema.Users.OnLoginError((UserLoginException)e);
                throw e;
              }
              else
              {
                user.LogIn(password, parameters);
                OnUserLoggedIn(new UserLoginEventArgs(user));
              }

              break;
            }
        }

        return user;
      }

      /// <summary>
      /// Effettua il logout
      /// </summary>
      /// <param name="user"></param>
      /// <param name="logoutMethod"></param>
      public void LogOut(
                      Sistema.CUser user,
                      Sistema.LogOutMethods logoutMethod = Sistema.LogOutMethods.LOGOUT_LOGOUT
                      )
      {
        if (DBUtils.GetID(user, 0) == 0)
          return;

        if (
            DBUtils.GetID(user, 0) != DBUtils.GetID(Sistema.Users.CurrentUser, 0)
            &&
            !ReferenceEquals(Sistema.Users.CurrentUser, KnownUsers.SystemUser)
            )
        {
          // Events.DispatchEvent(Sistema.Users.Module, "LogOut_Error", user.UserName, "L'utente [" & Users.CurrentUser.UserName & "] non è autorizzato a disconnettere [" & user.UserName & "]")
          throw new InvalidOperationException("Solo l'utente di sistema può disconnettere altri utenti");
        }

        user.LogOut(logoutMethod);
      }

      /// <summary>
      /// Restituisce un oggetto CCurrentUser che rappresenta l'utente corrente
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
      public Sistema.CUser CurrentUser
      {
        get
        {
          if (Sistema.ApplicationContext is null)
            return null;
          return Sistema.ApplicationContext.CurrentUser;
        }

        set
        {
          Sistema.ApplicationContext.CurrentUser = value;
        }
      }

      /// <summary>
      /// Restituisce l'utente in base al nome
      /// </summary>
      /// <param name="userName"></param>
      /// <returns></returns>
      public Sistema.CUser GetItemByName(string userName)
      {
        userName = DMD.Strings.Trim(userName);
        if (string.IsNullOrEmpty(userName))
          return null;
        var items = LoadAll();
        foreach (Sistema.CUser u in items)
        {
          if (DMD.Strings.Compare(u.UserName, userName, true) == 0)
            return u;
        }

        return null;
      }

      /// <summary>
      /// Restituisce l'utente in base al nominativo
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public Sistema.CUser GetItemDisplayByName(string value)
      {
        value = DMD.Strings.LCase(DMD.Strings.Trim(value));
        if (string.IsNullOrEmpty(value))
          return null;
        var items = LoadAll();
        foreach (Sistema.CUser u in items)
        {
          if (DMD.Strings.Compare(u.Nominativo, value, true) == 0)
            return u;
        }

        return null;
      }

      /// <summary>
      /// Restituisce un oggetto CLoginHistory in base al suo ID
      /// </summary>
      /// <param name="ID"></param>
      /// <returns></returns>
      /// <remarks></remarks>
      public Sistema.CLoginHistory GetLoginHistoryById(int ID)
      {
        using (var cursor = new Sistema.CLoginHistoryCursor())
        {
          cursor.PageSize = 1;
          cursor.IgnoreRights = true;
          cursor.ID.Value = ID;
          return cursor.Item;
        }
      }

      /// <summary>
      /// Restituisce una stringa che descrive lo stato di abilitazione dell'utente
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public string GetStatusText(Sistema.UserStatus value)
      {
        string ret;
        switch (value)
        {
          // Case UserStatus.USER_TEMP : ret = "Temporaneo"
          case Sistema.UserStatus.USER_ENABLED:
            {
              ret = "Abilitato";
              break;
            }

          case Sistema.UserStatus.USER_DISABLED:
            {
              ret = "Disabilitato";
              break;
            }

          case Sistema.UserStatus.USER_DELETED:
            {
              ret = "Eliminato";
              break;
            }

          case Sistema.UserStatus.USER_NEW:
            {
              ret = "Da Confermare";
              break;
            }

          case Sistema.UserStatus.USER_SUSPENDED:
            {
              ret = "Sospeso";
              break;
            }

          default:
            {
              ret = "Unknown";
              break;
            }
        }

        return ret;
      }

      /// <summary>
      /// Restituisce una stringa che descrive il codice di logout
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public string GetLogoutMethodText(Sistema.LogOutMethods value)
      {
        string ret;
        switch (value)
        {
          // Case LOGOUT_UNKNOWN : ret = "Unknown"
          case Sistema.LogOutMethods.LOGOUT_LOGOUT:
            {
              ret = "Logout";
              break;
            }

          case Sistema.LogOutMethods.LOGOUT_TIMEOUT:
            {
              ret = "Timeout";
              break;
            }

          case Sistema.LogOutMethods.LOGOUT_REMOTEDISCONNECT:
            {
              ret = "Remote Disconnect";
              break;
            }

          default:
            {
              ret = "Unknown";
              break;
            }
        }

        return ret;
      }

      /// <summary>
      /// Aggionra le informazioni di sessione dell'utente correntemente loggato
      /// </summary>
      public void RefreshSessionTimes()
      {
        //var db = this.Database;

        //var u = Sistema.Users.CurrentUser;
        //u.SetIsLogged(true);
        //u.Save();
        //var li = u.CurrentLogin;

        //dbSQL = "UPDATE [tbl_LoginHistory] SET [LogOutTime]=" + Databases.DBUtils.DBDate(DMD.DateUtils.Now()) + " WHERE [ID]=" + uID + ";";
        //    Databases.LOGConn.ExecuteCommand(dbSQL);

        //TODO

      }



      /// <summary>
      /// Ricarica le variabili di sistema
      /// </summary>
      /// <remarks></remarks>
      public void Reset()
      {
        KnownUsers.Reset();
      }

      /// <summary>
      /// Restituisce una collezione contenente tutti gli utenti 
      /// </summary>
      /// <returns></returns>
      public CCollection<Sistema.CUser> GetUtentiAsCollection()
      {
        return LoadAll();
      }

      private CKnownUsersClass m_KnownUsers = null;

      /// <summary>
      /// Utenti predefiniti
      /// </summary>
      public CKnownUsersClass KnownUsers
      {
        get
        {
          lock (this)
          {
            if (m_KnownUsers is null)
              m_KnownUsers = new CKnownUsersClass();
            return m_KnownUsers;
          }
        }
      }

      [NonSerialized] private CUserAuthorizationsRepository m_UserAuthorizations = null;

      /// <summary>
      /// Respository di oggetti di tipo <see cref="CUserAuthorization"/>
      /// </summary>
      public CUserAuthorizationsRepository UserAuthorizations
      {
        get
        {
          if (this.m_UserAuthorizations is null) this.m_UserAuthorizations = new CUserAuthorizationsRepository();
          return this.m_UserAuthorizations;
        }
      }


    }
  }

  public partial class Sistema
  {
    private static CUsersClass m_Users = null;


    /// <summary>
    /// Repository degli utenti
    /// </summary>
    public static CUsersClass Users
    {
      get
      {
        if (m_Users is null) m_Users = new CUsersClass();
        return m_Users;
      }
    }
  }
}