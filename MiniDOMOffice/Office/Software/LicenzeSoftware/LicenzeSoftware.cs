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
        /// Repository di <see cref="LicenzaSoftware"/> 
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CLicenzeSoftwareClass 
            : CModulesClass<LicenzaSoftware>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CLicenzeSoftwareClass() 
                : base("modOfficeLicenzeSoftware", typeof(LicenzeSoftwareCursor), -1)
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeLicenzeSoftware");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeLicenzeSoftware");
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
            /// Restituisce la licenza in base al codice
            /// </summary>
            /// <param name="codiceLicenza"></param>
            /// <returns></returns>
            public LicenzaSoftware GetItemByCodice(string codiceLicenza)
            {
                codiceLicenza = DMD.Strings.Trim(codiceLicenza);
                if (string.IsNullOrEmpty(codiceLicenza))
                    return null;
                foreach (var ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.CodiceLicenza, codiceLicenza, true) == 0)
                        return ret;
                }

                return null;
            }
        }
    }

    public partial class Office
    {
        private static CLicenzeSoftwareClass m_LicenzeSoftware = null;

        /// <summary>
        /// Repository di <see cref="LicenzaSoftware"/> 
        /// </summary>
        /// <remarks></remarks>
        public static CLicenzeSoftwareClass LicenzeSoftware
        {
            get
            {
                if (m_LicenzeSoftware is null)
                    m_LicenzeSoftware = new CLicenzeSoftwareClass();
                return m_LicenzeSoftware;
            }
        }
    }
}