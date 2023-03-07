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
        /// Cursore di oggetti <see cref="CircolareXGroup"/>
        /// </summary>
        [Serializable]
        public class CircolareXGroupCollection
            : CGroupAllowNegateCollection<Circolare, CircolareXGroup, CircolareXGroupCursor>
        {
           //TODO aggiornare il corrispondente javascript

            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolareXGroupCollection()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CircolareXGroupCollection(Circolare owner)
                : base(owner)
            {

            }

        }
    }
}