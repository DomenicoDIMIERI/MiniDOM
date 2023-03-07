using DMD;
using DMD.Databases.Collections;
using minidom.repositories;
using System;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Rappresenta una visita effettuata o ricevuta
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CAppunto 
            : CContattoUtente
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAppunto()
            {
            }

            /// <summary>
            /// Nome del tipo di oggetto
            /// </summary>
            /// <returns></returns>
            public override string GetNomeTipoOggetto()
            {
                return "Appunto";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.Appunti;
            }

            /// <summary>
            /// Descrizione attività
            /// </summary>
            public override string DescrizioneAttivita
            {
                get
                {
                    return "Appunto";
                }
            }

            /// <summary>
            /// Azioni proposte
            /// </summary>
            public override CCollection<CAzioneProposta> AzioniProposte
            {
                get
                {
                    return new CCollection<CAzioneProposta>();
                }
            }

           
        }
    }
}