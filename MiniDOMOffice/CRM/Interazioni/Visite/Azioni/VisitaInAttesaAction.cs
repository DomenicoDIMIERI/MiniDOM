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
    public partial class CustomerCalls
    {

        /// <summary>
        /// Azione che consente di prendere in carico una visita in attesa
        /// </summary>
        public class VisitaInAttesaAction 
            : AzioneEseguibile
        {

            /// <summary>
            /// Descrizione dell'azione
            /// </summary>
            public override string Description
            {
                get
                {
                    return "Cliente in attesa";
                }
            }

            /// <summary>
            /// Esegue l'azione
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
            protected override string ExecuteInternal(Sistema.Notifica notifica, string parameters)
            {
                if (notifica.SourceName != "CVisita")
                    throw new ArgumentException("L'azione non è definita sul tipo [" + notifica.SourceName + "]");
                var visita = Visite.GetItemById(notifica.SourceID);
                if (visita is null)
                    throw new ArgumentNullException("Visita");
                return null;
            }

            /// <summary>
            /// Nome univoco dell'azione
            /// </summary>
            public override string Name
            {
                get
                {
                    return "VISITAINATTESAACT";
                }
            }

            /// <summary>
            /// Rendering
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public override object Render(Sistema.Notifica notifica, object context)
            {
                return null;
            }
        }
    }
}