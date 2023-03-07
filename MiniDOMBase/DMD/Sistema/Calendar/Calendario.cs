using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;
using minidom.internals;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Calendario
        /// </summary>
        public partial class CalendarioClass
            : CModulesClass<CCalendarActivity>
        {
            
            private CCollection<Sistema.ICalendarProvider> m_Providers;
            private IComparer m_DefaultComparer;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CalendarioClass()
                : base("Calendar", typeof(CCalendarActivityCursor), 0)
            {
                //DMDObject.IncreaseCounter(this);
                //m_FirstDayOfWeek = 0;
                m_Providers = null;
                m_DefaultComparer = null;
                //m_GiorniFestivi = new CKeyCollection<DateTime>();
                //m_GiorniFestivi.Add("Capodanno", DateTime.Parse("1900-01-01"));
                //m_GiorniFestivi.Add("Befana", DateTime.Parse("1900-01-06"));
                //m_GiorniFestivi.Add("Festa della liberazione", DateTime.Parse("1900-04-25"));
                //m_GiorniFestivi.Add("Festa del lavoro", DateTime.Parse("1900-05-01"));
                //m_GiorniFestivi.Add("Festa della Repubblica", DateTime.Parse("1900-06-02"));
                //m_GiorniFestivi.Add("Ognissanti", DateTime.Parse("1900-11-01"));
                //m_GiorniFestivi.Add("Ferragosto", DateTime.Parse("1900-08-15"));
                //m_GiorniFestivi.Add("Festa dell'Immacolata", DateTime.Parse("1900-12-08"));
                //m_GiorniFestivi.Add("Natale", DateTime.Parse("1900-12-25"));
                //m_GiorniFestivi.Add("Santo Stefano", DateTime.Parse("1900-12-26"));
                //GiorniFeriali = new CKeyCollection<int>();
                //GiorniFeriali.Add("Lunedì", 1);
                //GiorniFeriali.Add("Mertedì", 2);
                //GiorniFeriali.Add("Mercoledì", 3);
                //GiorniFeriali.Add("Giovedì", 4);
                //GiorniFeriali.Add("Venerdì", 5);
                //GiorniFeriali.Add("Sabato", 6);
                AddProvider(new CCalendarActivitiesProvider());
            }


            private CRegisteredCalendarProviderRepository m_RegisteredCalendarProviders = null;

            /// <summary>
            /// Repository di oggetti <see cref="CRegisteredCalendarProvider"/>
            /// </summary>
            public CRegisteredCalendarProviderRepository RegisteredCalendarProviders
            {
                get
                {
                    if (this.m_RegisteredCalendarProviders is null)
                        this.m_RegisteredCalendarProviders = new CRegisteredCalendarProviderRepository();
                    return this.m_RegisteredCalendarProviders;
                }
            }



            /// <summary>
            /// Restituisce un oggetto CPersonActivitiesInfo che racchiude le inform
            /// </summary>
            /// <param name="personID"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CCalendarActivityInfo GetPersonInfo(int personID)
            {
                var ret = new Sistema.CCalendarActivityInfo();
                ret.Initialize(personID);
                return ret;
            }

          
            /// <summary>
            /// Notifica una nuova attività
            /// </summary>
            /// <param name="activity"></param>
            /// <param name="user"></param>
            /// <returns></returns>
            public int NotifyPlannedActivity(Sistema.CCalendarActivity activity, Sistema.CUser user)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Pianifica l'attività per l'utente corrente
            /// </summary>
            /// <param name="description"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CCalendarActivity PlanActivity(string description, DateTime fromDate, DateTime toDate)
            {
                var act = new Sistema.CCalendarActivity();
                act.Descrizione = description;
                act.DataInizio = fromDate;
                act.DataFine = toDate;
                act.Stato = ObjectStatus.OBJECT_VALID;
                act.Operatore = Sistema.Users.CurrentUser;
                act.AssegnatoA = Sistema.Users.CurrentUser;
                act.Save();
                return act;
            }

            /// <summary>
            /// Pianifica l'attività per un utente specifico
            /// </summary>
            /// <param name="user"></param>
            /// <param name="description"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CCalendarActivity PlanActivityForUser(Sistema.CUser user, string description, DateTime fromDate, DateTime toDate)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                var act = new Sistema.CCalendarActivity();
                act.Descrizione = description;
                act.DataInizio = fromDate;
                act.DataFine = toDate;
                act.Stato = ObjectStatus.OBJECT_VALID;
                act.Operatore = Sistema.Users.CurrentUser;
                act.AssegnatoA = user;
                act.Save();
                return act;
            }

            /// <summary>
            /// Restituisce vero se gli impegni dell'utente (attività pianificate) intersecano l'intervallo di tempo specificato
            /// </summary>
            /// <param name="user"></param>
            /// <param name="fromTime"></param>
            /// <param name="toTime"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public int CheckIfBusy(Sistema.CUser user, DateTime fromTime, DateTime toTime)
            {
                return CheckIfBusy(DBUtils.GetID(user, 0), fromTime, toTime);
            }

            /// <summary>
            /// Restituisce vero se gli impegni dell'utente (attività pianificate) intersecano l'intervallo di tempo specificato
            /// </summary>
            /// <param name="userID"></param>
            /// <param name="fromTime"></param>
            /// <param name="toTime"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public int CheckIfBusy(int userID, DateTime fromTime, DateTime toTime)
            {
                CCollection<Sistema.ICalendarActivity> items;
                int i;
                int cnt;
                items = GetPendingActivities();
                cnt = 0;
                var loopTo = items.Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (items[i].IDOperatore == userID)
                    {
                        if ((items[i].DataInizio >= fromTime & items[i].DataInizio <= toTime | items[i].DataFine >= fromTime & items[i].DataFine <= toTime) == true)
                        {
                            cnt += 1;
                        }
                    }
                }

                return cnt;
            }

            /// <summary>
            /// Restituisce un array contenente gli operatori predefiniti
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public IEnumerable GetAvailableOperators()
            {
                return Sistema.Users.LoadAll();
            }

            /// <summary>
            /// Restituisce i membri del gruppo
            /// </summary>
            /// <param name="group"></param>
            /// <returns></returns>
            public IEnumerable<CUser> GetAvailableGroupOperators(Sistema.CGroup group)
            {
                return group.Members;
            }

            /// <summary>
            /// Restituisce i membri del gruppo
            /// </summary>
            /// <param name="groupID"></param>
            /// <returns></returns>
            public IEnumerable<CUser> GetAvailableGroupOperators(int groupID)
            {
                return GetAvailableGroupOperators(Sistema.Groups.GetItemById(groupID));
            }

            /// <summary>
            /// Restituisce un array contenente i gruppi disponibili
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public IEnumerable GetAvailableGroups()
            {
                return Sistema.Groups.LoadAll();
            }

            /// <summary>
            /// Restituisce un array contenente i nomi dei tipi di classe che definiscono gli eventi che l'utente specificato può creare
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public Type[] GetAvailableEventTypes()
            {
                // Dim i As Integer
                // Dim prov As ICalendarProvider
                // Dim localActions As System.Type()
                // Dim actions As System.Type()
                // For i = 0 To Me.Providers.Count - 1
                // prov = Me.Providers(i)
                // localActions = prov.GetSupportedTypes
                // 'actions.Merge(localActions)
                // Next
                // Return actions
                throw new NotImplementedException();
            }

        

            // --------------------------------------------------------
            // PROVIDERS

            /// <summary>
            /// Restituisce una collezione di oggetti che implementano l'interfaccia ICalendarProvider
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Sistema.ICalendarProvider> Providers
            {
                get
                {
                    lock (this)
                    {
                        if (m_Providers is null)
                            m_Providers = new CCollection<Sistema.ICalendarProvider>();
                        return new CCollection<Sistema.ICalendarProvider>(m_Providers);
                    }
                }
            }

            /// <summary>
            /// Restituisce una attività in base al suo ID
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CCalendarActivity GetActivityByID(int value)
            {
                if (value == 0)
                    return null;

                using(var cursor = new CCalendarActivityCursor())
                {
                    cursor.ID.Value = value;
                    return cursor.Item;
                }
                 
            }

            /// <summary>
            /// Restituisce il comparer predefinito per gli oggetti di tipo CalendarActivity
            /// </summary>
            public object DefaultComparer
            {
                get
                {
                    if (m_DefaultComparer is null)
                        m_DefaultComparer = new Sistema.CCalendarItemsComparer();
                    return m_DefaultComparer;
                }
            }

            /// <summary>
            /// Restituisce true se è prevista un'esecuzione nell'intervallo specificato
            /// </summary>
            /// <param name="intervalType"></param>
            /// <param name="interval"></param>
            /// <param name="lastRun"></param>
            /// <param name="fromTime"></param>
            /// <param name="toTime"></param>
            /// <returns></returns>
            public bool ShouldExec(
                            DateTimeInterval intervalType, 
                            int interval, 
                            DateTime? lastRun, 
                            double? fromTime, 
                            double? toTime
                            )
            {
                DateTime? f1 = default;
                DateTime? f2 = default;
                if (fromTime.HasValue)
                    f1 = DMD.DateUtils.DateAdd(DateTimeInterval.Hour, fromTime.Value, DMD.DateUtils.ToDay());
                if (toTime.HasValue)
                    f2 = DMD.DateUtils.DateAdd(DateTimeInterval.Hour, toTime.Value, DMD.DateUtils.ToDay());
                return ShouldExec(intervalType, interval, lastRun, f1, f2);
            }


            /// <summary>
            /// Restituisce true se è prevista un'esecuzione nell'intervallo specificato
            /// </summary>
            /// <param name="intervalType"></param>
            /// <param name="interval"></param>
            /// <param name="lastRun"></param>
            /// <param name="fromTime"></param>
            /// <param name="toTime"></param>
            /// <returns></returns>
            public bool ShouldExec(
                        DateTimeInterval intervalType, 
                        int interval, 
                        DateTime? lastRun, 
                        DateTime? fromTime, 
                        DateTime? toTime
                        )
            {
                if (DMD.DateUtils.CheckBetween(DMD.DateUtils.GetTimePart(DMD.DateUtils.Now()), DMD.DateUtils.GetTimePart(fromTime), DMD.DateUtils.GetTimePart(toTime)))
                {
                    if (lastRun.HasValue)
                    {
                        return DMD.Maths.Abs(DMD.DateUtils.DateDiff(intervalType, DMD.DateUtils.Now(), lastRun.Value)) >= interval;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Restituisce una collezione ordinata di oggetti CCalendarActivity che rappresentano le scadenze nell'intervallo specificato
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Sistema.ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate)
            {
                var ret = new CCollection<Sistema.ICalendarActivity>();
                CCollection<Sistema.ICalendarActivity> col;
                ret.Comparer = DefaultComparer;
                ret.Sorted = true;
                var providers = Providers;
                for (int i = 0, loopTo = providers.Count - 1; i <= loopTo; i++)
                {
                    var provider = providers[i];
                    col = provider.GetScadenze(fromDate, toDate);
                    col.Comparer = DefaultComparer;
                    col.Sorted = true;
                    ret.Merge(col);
                }

                return ret;
            }

            /// <summary>
            /// Restituisce le attività pendenti per tutti i provider
            /// </summary>
            /// <returns></returns>
            public CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                CCollection<Sistema.ICalendarActivity> allItems;
                CCollection<Sistema.ICalendarActivity> items;
                var calComparer = new Sistema.CCalendarItemsComparer();
                allItems = new CCollection<Sistema.ICalendarActivity>();
                allItems.Comparer = calComparer;
                allItems.Sorted = true;
                var providers = Providers;
                for (int i = 0, loopTo = providers.Count - 1; i <= loopTo; i++)
                {
                    var provider = providers[i];
                    items = provider.GetPendingActivities();
                    items.Comparer = calComparer;
                    items.Sorted = true;
                    allItems.Merge(items);
                }

                return allItems;
            }

            /// <summary>
            /// Restituisce una collezione di oggetti CActivePerson
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Sistema.CActivePerson> GetActivePersons(
                                string nomeLista, 
                                DateTime? fromDate, 
                                DateTime? toDate, 
                                int ufficio = 0, 
                                int operatore = 0
                                )
            {
                var ret = new CCollection<Sistema.CActivePerson>();
                CCollection<Sistema.CActivePerson> tmp;
                ret.Comparer = DMD.Arrays.DefaultComparer;
                ret.Sorted = true;
                var providers = Providers;
                for (int i = 0, loopTo = providers.Count - 1; i <= loopTo; i++)
                {
                    var provider = providers[i];
                    tmp = provider.GetActivePersons(nomeLista, fromDate, toDate, ufficio, operatore);
                    tmp.Comparer = ret.Comparer;
                    tmp.Sorted = true;
                    ret.Merge(tmp);
                }

                return ret;
            }

         
            /// <summary>
            /// Restituisce un array contenente le collezioni delle persone che compiono gli anni per ogni giorno del mese
            /// </summary>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.CPersonaInfo>[] GetCompleanniPerMese(int year, int month)
            {
                CCollection<Anagrafica.CPersonaInfo>[] ret;
                using (var cursor = new Anagrafica.CPersonaFisicaCursor()) 
                { 
                    int mLen = DMD.DateUtils.GetDaysInMonth(year, month);
                    ret = new CCollection<Anagrafica.CPersonaInfo>[mLen];
                    var arr = new ArrayList();
                    foreach (CUfficio po in Anagrafica.Uffici.GetPuntiOperativiConsentiti())
                        arr.Add(DBUtils.GetID(po));
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Deceduto.Value = false;
                    cursor.WhereClauses *= cursor.Field("DataNascita").Day().Between(1, mLen)
                                           *
                                           cursor.Field("DataNascita").Month().EQ(month);

                    cursor.IgnoreRights = true;
                    cursor.IDPuntoOperativo.ValueIn(arr.ToArray());
                    for (int i = 0, loopTo = DMD.Arrays.UBound(ret); i <= loopTo; i++)
                        ret[i] = new CCollection<Anagrafica.CPersonaInfo>();
                    while (cursor.Read())
                    {
                        ret[DMD.DateUtils.Day(cursor.Item.DataNascita.Value) - 1].Add(new Anagrafica.CPersonaInfo(cursor.Item));
                    }

                    for (int i = 0, loopTo1 = DMD.Arrays.UBound(ret); i <= loopTo1; i++)
                        ret[i].Sort();
                }
                return ret;
            }

            /// <summary>
            /// Restituisce un oggetto IEnumerable che consente di elencare le persone che compiono gli anni alla data specifica
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            public IEnumerable<Anagrafica.CPersonaInfo> GetCompleanniPerGiorno(DateTime d)
            {
                var ret = new CCollection<Anagrafica.CPersonaInfo>();
                using (var cursor = new Anagrafica.CPersonaFisicaCursor()) {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Deceduto.Value = false;
                    cursor.WhereClauses *= cursor.Field("DataNascita").Day().EQ(DMD.DateUtils.Day(d))
                                           *
                                           cursor.Field("DataNascita").Month().EQ(DMD.DateUtils.Month(d));
                    cursor.IgnoreRights = true;
                    var arr = new ArrayList();
                    foreach (CUfficio po in Anagrafica.Uffici.GetPuntiOperativiConsentiti())
                        arr.Add(DBUtils.GetID(po));
                    cursor.IDPuntoOperativo.ValueIn(arr.ToArray());
                    while (cursor.Read())
                    {
                        ret.Add(new Anagrafica.CPersonaInfo(cursor.Item));
                    }
                }

                ret.Sort();
                
                return ret;
            }

            /// <summary>
            /// Restituisce l'elenco delle cose da fare
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                var ret = new CCollection<Sistema.ICalendarActivity>();
                var providers = Providers;
                foreach (Sistema.ICalendarProvider p in providers)
                    ret.AddRange(p.GetToDoList(user));
                ret.Sort();
                return ret;
            }

            /// <summary>
            /// Restituisce il provider per nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Sistema.ICalendarProvider GetProviderByName(string name)
            {
                var providers = Providers;
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (Sistema.ICalendarProvider p in providers)
                {
                    if ((p.UniqueName ?? "") == (name ?? ""))
                        return p;
                }

                return null;
            }

            /// <summary>
            /// Registra il provider
            /// </summary>
            /// <param name="p"></param>
            public void AddProvider(Sistema.ICalendarProvider p)
            {
                lock (this)
                {
                    if (GetProviderByName(p.UniqueName) is object)
                        throw new ArgumentException("Provider già registrato");
                    m_Providers.Add(p);
                }
            }

            /// <summary>
            /// Rimuove il provider
            /// </summary>
            /// <param name="p"></param>
            public void RemoveProvider(Sistema.ICalendarProvider p)
            {
                lock (this)
                {
                    p = GetProviderByName(p.UniqueName);
                    if (p is null)
                        throw new KeyNotFoundException("Provider non trovato");
                    m_Providers.Remove(p);
                }
            }

            //~Calendario()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }

    public partial class Sistema
    {
       
     

        private static CalendarioClass m_DateUtils = null;

        /// <summary>
        /// Calendario
        /// </summary>
        public static CalendarioClass Calendar
        {
            get
            {
                if (m_DateUtils is null)
                    m_DateUtils = new CalendarioClass();
                return m_DateUtils;
            }
        }
    }
}