using minidom.repositories;
using System;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Autorizzazioni di una pagina di proprietà per un gruppo utenti
        /// </summary>
        [Serializable]
        public class PropPageGroupAllowNegate 
            : GroupAllowNegate<CRegisteredPropertyPage>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public PropPageGroupAllowNegate()
            {
            }

             
            /// <summary>
            /// Restituisce l'id della colonna pagina di proprietà
            /// </summary>
            /// <returns></returns>
            protected override string GetItemFieldName()
            {
                return "IDPropPage";
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PropPagesXGruppo";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.PropertyPages.GroupAllowNegateRepository;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                minidom.Sistema.PropertyPages.InvalidateGroupAuth();
            }
        }
    }
}