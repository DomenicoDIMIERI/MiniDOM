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
using static minidom.Office;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="Visura"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CVisureClass
            : CModulesClass<Visura>
        {



            /// <summary>
            /// Evento generato quando viene memorizzata una nuova richiesta
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event ItemEventHandler<Visura> RichiestaCreata;

            
            /// <summary>
            /// Evento generato quando viene effettuata una nuova richiesta ad un cessionario
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event ItemEventHandler<Visura> RichiestaEffettuata;
             
            /// <summary>
            /// Evento generato quando viene ritirata un richiesta ad un cessionario
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event ItemEventHandler<Visura> RichiestaRitirata;
 
            /// <summary>
            /// Evento generato quando viene rifiutata un richiesta da un cessionario
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event ItemEventHandler<Visura> RichiestaRifiutata;
             
            /// <summary>
            /// Evento generato quando viene annullata una richiesta da un utente
            /// </summary>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event ItemEventHandler<Visura> RichiestaAnnullata;
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public CVisureClass() 
                : base("modVisure", typeof(VisureCursor))
            {
            } 

            /// <summary>
            /// Genera l'evento RichestaCreata
            /// </summary>
            /// <param name="e"></param>
            internal void doRichiestaCreata(ItemEventArgs<Visura> e)
            {
                RichiestaCreata?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento RichestaEffettuata
            /// </summary>
            /// <param name="e"></param>
            internal void doRichiestaEffettuata(ItemEventArgs<Visura> e)
            {
                RichiestaEffettuata?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento RichestaRifiutata
            /// </summary>
            /// <param name="e"></param>
            internal void doRichiestaRifiutata(ItemEventArgs<Visura> e)
            {
                RichiestaRifiutata?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento RichestaAnnullata
            /// </summary>
            /// <param name="e"></param>
            internal void doRichiestaAnnullata(ItemEventArgs<Visura> e)
            {
                RichiestaAnnullata?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento RichestaRitirata
            /// </summary>
            /// <param name="e"></param>
            internal void doRichiestaRitirata(ItemEventArgs<Visura> e)
            {
                RichiestaRitirata?.Invoke(this, e);
            }
        }
    }

    public partial class Office
    {
       
        private static CVisureClass m_Visure = null;

        /// <summary>
        /// Repository di <see cref="Visura"/>
        /// </summary>
        public static CVisureClass Visure
        {
            get
            {
                if (m_Visure is null)
                    m_Visure = new CVisureClass();
                return m_Visure;
            }
        }
    }
}