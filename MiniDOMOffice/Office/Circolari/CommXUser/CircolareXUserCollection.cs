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
    public partial class Office
    {

        /// <summary>
        /// Collezione di autorizzazioni utente per le circolari
        /// </summary>
        [Serializable]
        public class CircolareXUserCollection
            : CUserAllowNegateCollection<Circolare, CircolareXUser, CircolareXUserCursor>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolareXUserCollection()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CircolareXUserCollection(Circolare owner)
                : base(owner)
            {

            }
             
        }
    }
}