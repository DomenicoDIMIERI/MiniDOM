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
        /// Interfaccia implementata dai provider di attività del calendario
        /// </summary>
        /// <remarks></remarks>
        public interface ICalendarProvider
        {

            /// <summary>
            /// Crea un nuovo oggetto
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            object InstantiateNewItem();

            /// <summary>
            /// Restituisce la stringa che il provider identifica come stringa di creazione
            /// </summary>
            /// <returns></returns>
            string GetCreateCommand();

            /// <summary>
            /// Restituisce un testo breve che descrive il tipo di provider
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            string GetShortDescription();

            /// <summary>
            /// Restituisce un array di tipi di oggetto supportati da questo provider
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            Type[] GetSupportedTypes();

            /// <summary>
            /// Restituisce le persone con eventi gestiti da questo provider nell'intervallo di date specificato
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <param name="nomeLista"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            CCollection<CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0);

            /// <summary>
            /// Restituisce le scadenza gestite da questo provider
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            CCollection<ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate);

            /// <summary>
            /// Restituisce le aziende pendenti
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            CCollection<ICalendarActivity> GetPendingActivities();

            /// <summary>
            /// Restituisce l'elenco delle cose da fare per l'utente specificato
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            CCollection<ICalendarActivity> GetToDoList(CUser user);

            /// <summary>
            /// Restituisce il nome che identifica univocamente il provider all'interno del sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string UniqueName { get; }

            /// <summary>
            /// Salva l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            void SaveActivity(ICalendarActivity item, bool force = false);

            /// <summary>
            /// Elimina l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            void DeleteActivity(ICalendarActivity item, bool force = false);
        }
    }
}