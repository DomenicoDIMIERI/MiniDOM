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
        /// Azione definita su tutte le notifiche
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class EliminaNotificaAction 
            : AzioneEseguibile
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public EliminaNotificaAction()
            {

            }

            /// <summary>
            /// Descrizione dell'azione
            /// </summary>
            public override string Description
            {
                get
                {
                    return "Elimina la notifica";
                }
            }

            /// <summary>
            /// Esegue l'azione
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
            protected override string ExecuteInternal(Notifica notifica, string parameters)
            {
                notifica.DataLettura = DMD.DateUtils.Now();
                notifica.StatoNotifica = StatoNotifica.LETTA;
                return DMD.Strings.vbNullString;
            }

            /// <summary>
            /// Restituisce il nome dell'azione
            /// </summary>
            public override string Name
            {
                get
                {
                    return "ELIMINANOTIFICA";
                }
            }

            /// <summary>
            /// Crea l'interfaccia
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public override object Render(Notifica notifica, object context)
            {
                return null;
            }
        }
    }
}