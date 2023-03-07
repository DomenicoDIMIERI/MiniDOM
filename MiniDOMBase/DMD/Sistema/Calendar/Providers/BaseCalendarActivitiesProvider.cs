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


namespace minidom
{


    

    public partial class Sistema
    {

        /// <summary>
        /// Classe base del provider di attività del calendario
        /// Classe base che implementa i metodi dell'interfaccia ICalendarProvider
        /// </summary>
        /// <remarks></remarks>
        public abstract class BaseCalendarActivitiesProvider 
            : ICalendarProvider
        {

            private CModulesClass m_Module;

            /// <summary>
            /// Costruttore
            /// </summary>
            public BaseCalendarActivitiesProvider()
            {
                //DMDObject.IncreaseCounter(this);
                m_Module = null;
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            public CModulesClass Module
            {
                get
                {
                    if (m_Module is null)
                        m_Module = GetModule();
                    return m_Module;
                }
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            protected virtual CModulesClass GetModule()
            {
                return null;
            }

            /// <summary>
            /// Istanzia un nuovo oggetto gestito
            /// </summary>
            /// <returns></returns>
            public virtual object InstantiateNewItem()
            {
                return new CCalendarActivity();
            }

            /// <summary>
            /// Restituisce il comando di creazione
            /// </summary>
            /// <returns></returns>
            public virtual string GetCreateCommand()
            {
                string GetCreateCommandRet = default;
                GetCreateCommandRet = "/calendar/activities/?_a=create";
                return GetCreateCommandRet;
            }

            /// <summary>
            /// Restituisce la descrizione del provider
            /// </summary>
            /// <returns></returns>
            public virtual string GetShortDescription()
            {
                return "Attività";
            }

            /// <summary>
            /// Restituisce i tipi gestiti dal provider
            /// </summary>
            /// <returns></returns>
            public virtual Type[] GetSupportedTypes()
            {
                return new[] { typeof(CCalendarActivity) };
            }

            /// <summary>
            /// Restituisce una collezione di oggetti CActivePerson contenente tutte le persone con attività in sospes o nelle date indicate. La collezione è ordinata in funzione della priorità o del ritardo delle operazioni
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract CCollection<CActivePerson> GetActivePersons(
                            string nomeLista, 
                            DateTime? fromDate, 
                            DateTime? toDate, 
                            int ufficio = 0, 
                            int operatore = 0
                            );

            /// <summary>
            /// Restituisce le scadenze nell'intevallo compreso tra fromDate e toDate (incluse)
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public abstract CCollection<ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate);

            /// <summary>
            /// Restituisce le scadenza pendenti (fino ad oggi)
            /// </summary>
            /// <returns></returns>
            public abstract CCollection<ICalendarActivity> GetPendingActivities();

            /// <summary>
            /// Restituisce l'elenco delle cose da fare per l'utente specificato
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract CCollection<ICalendarActivity> GetToDoList(CUser user);

            /// <summary>
            /// Restituisce il nome che identifica univocamente il provider all'interno del sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract string UniqueName { get; }

            /// <summary>
            /// Elimina l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public abstract void DeleteActivity(ICalendarActivity item, bool force = false);

            /// <summary>
            /// Salva l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public abstract void SaveActivity(ICalendarActivity item, bool force = false);

            ///// <summary>
            ///// Distruttore
            ///// </summary>
            //~BaseCalendarActivitiesProvider()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }
}