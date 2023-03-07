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
        /// Repository di <see cref="Software"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CSotfwaresClass 
            : CModulesClass<Software>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CSotfwaresClass() 
                : base("modOfficeSoftware", typeof(SoftwareCursor), -1)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeSoftware");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeSoftware");
            //        ret.Description = "Software";
            //        ret.DisplayName = "Software";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    return ret;
            //}

            /// <summary>
            /// Restituisce l'oggetto in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <param name="version"></param>
            /// <returns></returns>
            public Software GetItemByNameAndVersione(string name, string version)
            {
                name = DMD.Strings.Trim(name);
                version = DMD.Strings.Trim(version);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (var ret in LoadAll())
                {
                    if (
                        DMD.Strings.Compare(ret.Nome, name, true) == 0 
                        && 
                        DMD.Strings.Compare(ret.Versione, version, true) == 0)
                        return ret;
                }

                return null;
            }
        }
    }

    public partial class Office
    {
        private static CSotfwaresClass m_Software = null;

        /// <summary>
        /// Repository di <see cref="Software"/>
        /// </summary>
        /// <remarks></remarks>
        public static CSotfwaresClass Softwares
        {
            get
            {
                if (m_Software is null)
                    m_Software = new CSotfwaresClass();
                return m_Software;
            }
        }
    }
}