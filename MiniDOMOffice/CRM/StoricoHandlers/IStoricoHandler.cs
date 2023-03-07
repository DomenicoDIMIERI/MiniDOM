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

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Interfaccia implementata dagli oggetti che si installano nel CRM per essere visualizzati nello storico del cliente
        /// </summary>
        /// <remarks></remarks>
        public interface IStoricoHandler
        {

            /// <summary>
            /// Metodo richiamato per aggiungere gli elementi allo storico
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            /// <remarks></remarks>
            void Aggiungi(CCollection<StoricoAction> items, CRMFindFilter filter);

            /// <summary>
            /// Restituisce la collezione dei tipi gestiti.
            /// Le chiavi della collezione indicano il nome della classe mentre il valore indica il nome visibile
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            CKeyCollection<string> GetHandledTypes();
        }
    }
}