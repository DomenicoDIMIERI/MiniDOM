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
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Classe base per i gestori dello storico
        /// </summary>
        /// <remarks></remarks>
        public abstract class StoricoHandlerBase
            : IStoricoHandler
        {
            private CKeyCollection<string> m_SupportedTypes;

            /// <summary>
            /// Costruttore
            /// </summary>
            public StoricoHandlerBase()
            {
                this.m_SupportedTypes = null;
            }

            /// <summary>
            /// Aggiunge gli elementi ottenuti da questo oggetto tramite l'applicazione del filtro filter
            /// alla collezione items
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            public void Aggiungi(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                if (!IsSupportedTipoOggetto(filter))
                    return;
                AggiungiInternal(items, filter);
            }

            /// <summary>
            /// Aggiunge gli elementi ottenuti da questo oggetto tramite l'applicazione del filtro filter
            /// alla collezione items
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected abstract void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter);


            /// <summary>
            /// Funzione che determina se il tipo oggetto specificato nel filtro è supportato da questo gestore
            /// </summary>
            /// <param name="filter">[in] Filtro</param>
            /// <returns></returns>
            /// <remarks></remarks>
            protected virtual bool IsSupportedTipoOggetto(CRMFindFilter filter)
            {
                return string.IsNullOrEmpty(filter.TipoOggetto) || GetHandledTypes().ContainsKey(filter.TipoOggetto);
            }

            /// <summary>
            /// Restituisce la collezione dei tipi supportati.
            /// L'oggetto CKeyCollection contiene i nomi dei tipi come chiave e la loro descrizione come stringa
            /// </summary>
            /// <returns></returns>
            public CKeyCollection<string> GetHandledTypes()
            {
                lock (this)
                {
                    if (m_SupportedTypes is null)
                    {
                        m_SupportedTypes = new CKeyCollection<string>();
                        FillSupportedTypes(m_SupportedTypes);
                    }

                    return m_SupportedTypes;
                }
            }

            /// <summary>
            /// Prepara la collezione dei tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected abstract void FillSupportedTypes(CKeyCollection<string> items);

             
        }
    }
}