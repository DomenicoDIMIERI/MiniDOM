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
        /// Repository di oggetti di tipo <see cref="NotificaUtenteXCircolare"/>
        /// </summary>
        [Serializable]
        public class NotificaUtenteXCircolareRepository
            : CModulesClass <NotificaUtenteXCircolare>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public NotificaUtenteXCircolareRepository()
                : base("modOfficeCircolariResults", typeof(NotificaUtenteXCircolareCursor), 0)
            {

            }

        }

        public partial class CircolariRepository
        {

            private NotificaUtenteXCircolareRepository _Results = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="NotificaUtenteXCircolare"/>
            /// </summary>
            public NotificaUtenteXCircolareRepository Results
            {
                get
                {
                    if (this._Results is null)
                        this._Results = new NotificaUtenteXCircolareRepository();
                    return this._Results;
                }
            }
        }

    }

    
}