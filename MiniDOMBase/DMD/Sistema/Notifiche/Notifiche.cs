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

namespace minidom.repositories 
{
    /// <summary>
    /// Repository di oggetti di tipo <see cref="Notifica"/>
    /// </summary>
    /// <remarks></remarks>
    public sealed partial class CNotificheClass
        : CModulesClass<Notifica>
    {

        /// <summary>
        /// Evento generato quando ci sono nuove notifiche per l'utente corrente
        /// </summary>
        /// <remarks></remarks>
        public event NuoveNotificheEventHandler NuoveNotifiche;

        
        /// <summary>
        /// Costruttore
        /// </summary>
        public CNotificheClass() 
            : base("modSYSNotifiche", typeof(NotificaCursor), 0)
        {
        }


        [NonSerialized] private AzioniRegistrateRepository m_AzioniRegistrateRepository = null;

        /// <summary>
        /// Repository di oggetti <see cref="AzioneRegistrata"/>
        /// </summary>
        public AzioniRegistrateRepository AzioniRegistrateRepository
        {
            get
            {
                if (this.m_AzioniRegistrateRepository is null)
                    this.m_AzioniRegistrateRepository = new AzioniRegistrateRepository();
                return this.m_AzioniRegistrateRepository;
            }
        }

        [NonSerialized] private AzioniEseguiteRepository m_AzioniEseguiteRepository = null;

        /// <summary>
        /// Repository di oggetti <see cref="AzioneEseguita"/>
        /// </summary>
        public AzioniEseguiteRepository AzioniEseguiteRepository
        {
            get
            {
                if (this.m_AzioniEseguiteRepository is null)
                    this.m_AzioniEseguiteRepository = new AzioniEseguiteRepository();
                return this.m_AzioniEseguiteRepository;
            }
        }


        /// <summary>
        /// Annulla tutte le notifiche pendenti impostate dall'oggetto specificato
        /// </summary>
        /// <param name="toDate">[in] Data fino alla quale annullare le notifiche pendenti. Se NULL vengono annullate tutte le date</param>
        /// <param name="source">[in] Oggetto che ha generato le notifiche da annullare. Se NULL vengono annullate tutte le notifiche</param>
        /// <param name="categoria">[in] Categoria della notifica</param>
        /// <remarks></remarks>
        public void CancelPendingAlertsBySource(DateTime? toDate, object source, string categoria)
        {
            CancelPendingAlertsBySource(Users.CurrentUser, toDate, source, categoria);
        }



        /// <summary>
        /// Annulla tutte le notifiche pendenti impostate dall'oggetto specificato
        /// </summary>
        /// <param name="user">[in] Utente per cui annullare le notifiche</param>
        /// <param name="toDate">[in] Data fino a cui annullare le notifiche (se NULL annulla tutte le notifiche)</param>
        /// <param name="source">[in] Oggetto che ha generato le notifiche. Se NULL annulla tutte le notifiche</param>
        /// <remarks></remarks>
        public void CancelPendingAlertsBySource(CUser user, DateTime? toDate, object source, string categoria)
        {
            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA;
                cursor.StatoNotifica.Operator = OP.OP_LE;
                if (!string.IsNullOrEmpty(categoria))
                    cursor.Categoria.Value = categoria;
                cursor.IgnoreRights = true;
                if (toDate.HasValue)
                {
                    cursor.Data.Value = DMD.DateUtils.DateAdd(DateTimeInterval.Day, 1d, DMD.DateUtils.GetDatePart(toDate));
                    cursor.Data.Operator = OP.OP_LT;
                }

                if (user is object)
                    cursor.TargetID.Value = DBUtils.GetID(user, 0);
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                while (cursor.Read())
                {
                    cursor.Item.StatoNotifica = StatoNotifica.ANNULLATA;
                    cursor.Item.Save();
                }

            }
        }

        /// <summary>
        /// Programma un promemoria per l'utente corrente
        /// </summary>
        /// <param name="descrione"></param>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Notifica ProgramAlert(string descrione, DateTime data, object source, string categoria)
        {
            return ProgramAlert(Users.CurrentUser, descrione, data, source, categoria);
        }

        /// <summary>
        /// Programma un promemoria per l'utente specificato
        /// </summary>
        /// <param name="user">[in] Utente per cui programmare il promemoria</param>
        /// <param name="descrione"></param>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Notifica ProgramAlert(CUser user, string descrione, DateTime data, object source, string categoria)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            if (source is null)
                throw new ArgumentNullException("source");
            var ret = new Notifica();
            if (DBUtils.GetID(user, 0) == DBUtils.GetID(Users.KnownUsers.GuestUser, 0))
                throw new ArgumentException("Impossibile programmare una notifica per l'utente Guest");
            if (DBUtils.GetID(user, 0) == DBUtils.GetID(Users.KnownUsers.SystemUser, 0))
                throw new ArgumentException("Impossibile programmare una notifica per l'utente SYSTEM");

            // ret.PuntoOperativo = po
            ret.Target = user;
            ret.Descrizione = descrione;
            ret.Data = data;
            ret.Categoria = categoria;
            ret.SourceName = DMD.RunTime.vbTypeName(source);
            ret.SourceID = DBUtils.GetID(source, 0);
            ret.StatoNotifica = StatoNotifica.NON_CONSEGNATA;
            ret.Stato = ObjectStatus.OBJECT_VALID;
            ret.Save();
            return ret;
        }

        /// <summary>
        /// Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notifiche)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<Notifica> GetPendingAlertsBySource(object source)
        {
            return GetPendingAlertsBySource(Users.CurrentUser, source);
        }

        /// <summary>
        /// Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notifiche)</param>
        /// <param name="contesto"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<Notifica> GetPendingAlertsBySource(object source, string contesto)
        {
            return GetPendingAlertsBySource(Users.CurrentUser, source, contesto);
        }

        /// <summary>
        /// Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="userID">[in] Utente di cui recuperare le notifiche</param>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<Notifica> GetPendingAlertsBySource(int userID, object source)
        {
            var ret = new CCollection<Notifica>();
            if (userID == 0 || userID == DBUtils.GetID(Users.KnownUsers.GuestUser, 0))
                return ret;

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA;
                cursor.StatoNotifica.Operator = OP.OP_LE;
                cursor.Data.Value = DMD.DateUtils.ToMorrow();
                cursor.Data.Operator = OP.OP_LT;
                cursor.TargetID.Value = userID;
                cursor.IgnoreRights = true;
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }

            }
            return ret;
        }

        /// <summary>
        /// Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="user">[in] Utente di cui recuperare le notifiche</param>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<Notifica> GetPendingAlertsBySource(CUser user, object source)
        {

            var ret = new CCollection<Notifica>();
            if (user is null)
                throw new ArgumentNullException("user");
            if (DBUtils.GetID(user, 0) == DBUtils.GetID(Users.KnownUsers.GuestUser, 0))
                return ret;


            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA;
                cursor.StatoNotifica.Operator = OP.OP_LE;
                cursor.Data.Value = DMD.DateUtils.ToMorrow();
                cursor.Data.Operator = OP.OP_LT;
                cursor.TargetID.Value = DBUtils.GetID(user, 0);
                cursor.IgnoreRights = true;
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                while (cursor.Read())
                {
                    var n = cursor.Item;
                    n.SetTarget(user);
                    ret.Add(n);
                }

            }

            return ret;
        }

        /// <summary>
        /// Restituisce la collezione delle notifiche non consegnate all'utente specificato
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public CCollection<Notifica> GetNotificheNonConsegnate(int userID, object source)
        {
            var ret = new CCollection<Notifica>();
            if (userID == 0 || userID == DBUtils.GetID(Users.KnownUsers.GuestUser, 0))
                return ret;
            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoNotifica.Value = StatoNotifica.NON_CONSEGNATA;
                cursor.TargetID.Value = userID;
                cursor.IgnoreRights = true;
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }

                return ret;
            }

        }

        /// <summary>
        /// Restituisce la collezione delle notifiche non consegnate all'utente corrente e generate dalla sorgente
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CCollection<Notifica> GetNotificheNonConsegnate(string source)
        {
            return GetNotificheNonConsegnate(DBUtils.GetID(Users.CurrentUser, 0), source);
        }

        /// <summary>
        /// Restituisce la collezione delle notifiche non consegnate all'utente corrente e generate dalla sorgente
        /// </summary>
        /// <param name="user"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public int CountPendingAlertsBySource(CUser user, object source)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            return CountPendingAlertsBySource(DBUtils.GetID(user, 0), source);
        }

        /// <summary>
        /// Restituisce il numero delle notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int CountPendingAlertsBySource(object source)
        {
            return CountPendingAlertsBySource(Users.CurrentUser, source);
        }

        /// <summary>
        /// Restituisce il numero delle notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int CountPendingAlertsBySource(int userID, object source)
        {
            if (userID == 0 || userID == DBUtils.GetID(Users.KnownUsers.GuestUser, 0))
                return 0;
            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA;
                cursor.StatoNotifica.Operator = OP.OP_LE;
                cursor.Data.Value = DMD.DateUtils.ToMorrow();
                cursor.Data.Operator = OP.OP_LT;
                cursor.TargetID.Value = userID;
                cursor.IgnoreRights = true;
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                return (int) cursor.Count();
            }

        }

        /// <summary>
        /// Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        /// </summary>
        /// <param name="user">[in] Utente di cui recuperare le notifiche</param>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        /// <param name="contesto"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<Notifica> GetPendingAlertsBySource(CUser user, object source, string contesto)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            var ret = new CCollection<Notifica>();
            if (DBUtils.GetID(user, 0) == 0 || DBUtils.GetID(user, 0) == DBUtils.GetID(Users.KnownUsers.GuestUser, 0))
                return ret;
            contesto = DMD.Strings.Trim(contesto);

            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA;
                cursor.StatoNotifica.Operator = OP.OP_LE;
                cursor.Data.Value = DMD.DateUtils.ToMorrow();
                cursor.Data.Operator = OP.OP_LT;
                cursor.TargetID.Value = DBUtils.GetID(user, 0);
                cursor.Context.Value = contesto;
                cursor.IgnoreRights = true;
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }

            }
            return ret;
        }

        /// <summary>
        /// Restituisce la collezione di tutte le notifiche "attive" e non programmate per l'utente
        /// </summary>
        /// <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        /// <param name="contesto"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<Notifica> GetAlertsBySource(object source, string contesto)
        {
            var ret = new CCollection<Notifica>();
            contesto = DMD.Strings.Trim(contesto);
            using (var cursor = new NotificaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.Context.Value = contesto;
                if (source is object)
                {
                    cursor.SourceName.Value = DMD.RunTime.vbTypeName(source);
                    cursor.SourceID.Value = DBUtils.GetID(source, 0);
                }

                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }

            }

            return ret;
        }
    }

}

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Firma dei gestori dell'evento NuoveNotifiche
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NuoveNotificheEventHandler(object sender, EventArgs e);

        private static CNotificheClass m_Notifiche = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="Notifica"/>
        /// </summary>
        public static CNotificheClass Notifiche
        {
            get
            {
                if (m_Notifiche is null)
                    m_Notifiche = new CNotificheClass();
                return m_Notifiche;
            }
        }
    }
}