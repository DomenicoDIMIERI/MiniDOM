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
using static minidom.Contabilita;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Interfaccia implementata dalle classi utilizzabili per generare una bolletta a partire da un'utenza
        /// </summary>
        /// <remarks></remarks>
        public interface IUtenzaBollettaHandler
        {

            /// <summary>
            /// Genera la prossima bolletta
            /// </summary>
            /// <param name="utenza"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            DocumentoContabile GenerateNext(Utenza utenza);
        }
    }
}