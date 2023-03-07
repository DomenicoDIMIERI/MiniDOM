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
        public class CircolareXGroup 
            : GroupAllowNegate<Circolare>
        {
            //TODO aggiornare il corrispondente javascript


            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolareXGroup()
            {
                 
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Circolari.CircolareXGroupRepository;
            }


            // Public Function GetLink() As String
            // Return WebSite.Configuration.URL & "/?_m=" & GetID(Comunicazioni.Module) & "&_a=get&ID=" & GetID(Me)
            // End Function

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