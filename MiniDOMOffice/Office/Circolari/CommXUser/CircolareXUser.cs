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
        /// Rappresenta un documento o una comunicazione pubblicata sul sito
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CircolareXUser
            : UserAllowNegate<Circolare>
        {
            //TODO aggiornare il corrispondente javascript

            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolareXUser()
            {
            }
   
            /// <summary>
            /// Reposiory
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
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