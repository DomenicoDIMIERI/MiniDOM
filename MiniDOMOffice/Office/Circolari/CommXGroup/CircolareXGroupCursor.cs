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
        public class CircolareXGroupCursor 
            : GroupAllowNegateCursor<CircolareXGroup>
        {
           //TODO aggiornare il corrispondente javascript

            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolareXGroupCursor()
            {
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Circolari.CircolareXGroupRepository;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ComunicazioniXGruppo";
            }

            /// <summary>
            /// nome del campo group
            /// </summary>
            /// <returns></returns>
            protected override string GetGroupFieldName()
            {
                return "Group";
            }

            /// <summary>
            /// Nome del campo item
            /// </summary>
            /// <returns></returns>
            protected override string GetItemFieldName()
            {
                return "Comunicazione";
            }

        }
    }
}