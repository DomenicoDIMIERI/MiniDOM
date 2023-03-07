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
    public partial class Sistema
    {
 

        /// <summary>
        /// Interfaccia implementata dagli handler degli eventi di sistema
        /// </summary>
        /// <remarks></remarks>
        public interface IEventHandler
        {

            /// <summary>
            /// Metodo richiamato dal sistema per notificare l'evento all'handler
            /// </summary>
            /// <param name="e">[in] Descrittore dell'evento</param>
            void NotifyEvent(EventDescription e);
        }
    }
}