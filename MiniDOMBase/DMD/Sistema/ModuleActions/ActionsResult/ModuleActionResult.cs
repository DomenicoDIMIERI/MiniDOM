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
        /// Racchiude informazioni sullo stato dell'esecuzione di un'azione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public abstract class ModuleActionResult 
            : Databases.DBObjectBase
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public ModuleActionResult()
            {
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            //public override CModule GetModule()
            //{
            //    return null; // TODO
            //}
            

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SYSModActRes";
            }
        }
    }
}