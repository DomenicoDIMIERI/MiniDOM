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
        /// Circolare per utente
        /// </summary>
        [Serializable]
        public class CircolareXUserCursor 
            : UserAllowNegateCursor<Circolare>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolareXUserCursor()
            {
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Circolari.CircolareXUserRepository;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ComunicazioniXUtente";
            }

            /// <summary>
            /// Nome del campo Utente
            /// </summary>
            /// <returns></returns>
            protected override string GetUserFieldName()
            {
                return "Utente";
            }

            /// <summary>
            /// Nome del campo comunicazione
            /// </summary>
            /// <returns></returns>
            protected override string GetItemFieldName()
            {
                return "Comunicazione";
            }
        }
    }
}