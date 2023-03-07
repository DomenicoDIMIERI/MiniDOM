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
        /// Pagina di proprietà assegnata o negata ad un utente specifico
        /// </summary>
        [Serializable]
        public class PropPageUserAllowNegate 
            : UserAllowNegate<CRegisteredPropertyPage>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public PropPageUserAllowNegate()
            {
            }

            
            /// <summary>
            /// ID della pagina
            /// </summary>
            /// <returns></returns>
            protected override string GetItemFieldName()
            {
                return "IDPropPage";
            }

            /// <summary>
            /// ID dell'utente
            /// </summary>
            /// <returns></returns>
            protected override string GetUserFieldName()
            {
                return "IDUser";
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PropPageXUtente";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.PropertyPages.UserAllowNegateRepository;
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                minidom.Sistema.PropertyPages.InvalidateUserAuth();
            }
        }
    }
}