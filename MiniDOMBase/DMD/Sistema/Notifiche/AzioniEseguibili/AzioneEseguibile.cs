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
        /// Rappresenta un'azione possibile su una notifica di sistema
        /// </summary>
        /// <remarks></remarks>
        public abstract class AzioneEseguibile
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public AzioneEseguibile()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Nome dell'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract string Name { get; }

            /// <summary>
            /// Descrizione dell'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract string Description { get; }


            /// <summary>
            /// Esegue l'azione
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public AzioneEseguita Execute(Notifica notifica, string parameters)
            {
                if (notifica is null)
                    throw new ArgumentNullException("notifica");
                var ret = new AzioneEseguita();
                ret.Notifica = notifica;
                ret.Azione = this;
                ret.DataEsecuzione = DMD.DateUtils.Now();
                ret.ActionParameters = parameters;
                ret.ActionResults = this.ExecuteInternal(notifica, parameters);
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.Save();
                notifica.StatoNotifica = StatoNotifica.LETTA;
                notifica.DataLettura = DMD.DateUtils.Now();
                notifica.Save();
                return ret;
            }

            /// <summary>
            /// Esegue l'azione
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
            protected abstract string ExecuteInternal(Notifica notifica, string parameters);

            /// <summary>
            /// Metodo eventualmente richiamato dall'interfaccia dell'applicazione per generare l'interfaccia utente relativa all'azione
            /// </summary>
            /// <param name="notifica"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract object Render(Notifica notifica, object context);

            
        }
    }
}