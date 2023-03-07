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
        /// Azione che consente di prendere in carico un appuntamento
        /// </summary>
        public class PrendiInCaricoRicontatto 
            : AzioneEseguibile
        {

            /// <summary>
            /// Descrizione dell'azione
            /// </summary>
            public override string Description
            {
                get
                {
                    return "Prende in carico la richiesta di finanziamento e programma un ricontatto nel CRM";
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
                if (notifica.SourceName != "CRicontatto")
                    throw new ArgumentException("L'azione non è definita sul tipo [" + notifica.SourceName + "]");
                var richFin = Anagrafica.Ricontatti.GetItemById(notifica.SourceID);
                if (richFin is null)
                    throw new ArgumentNullException("Richiesta di finaniamento");
                return null;
            }

            /// <summary>
            /// Nome univoco dell'azione
            /// </summary>
            public override string Name
            {
                get
                {
                    return "RICONTTPRENDIINCARICO";
                }
            }

            /// <summary>
            /// Rendering dell'azione
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