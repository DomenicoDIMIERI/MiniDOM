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
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CModule" />
        /// </summary>
        [Serializable]
        public sealed partial class CModulesModeClass
            : CModulesClass<Sistema.CModule>
        {
             
           
            /// <summary>
            /// Costruttore
            /// </summary>
            public CModulesModeClass() 
                : base("modModuli", typeof(Sistema.CModulesCursor), -1)
            {
            }
               

            /// <summary>
            /// Restituisce i moduli visibili all'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Sistema.CModule> GetUserModules(Sistema.CUser user)
            {
                var ret = new CCollection<Sistema.CModule>();
                foreach (Sistema.CModule m in LoadAll())
                {
                    if (m.Stato == ObjectStatus.OBJECT_VALID && m.Visible && m.ParentID == 0 && m.IsVisibleToUser(user))
                    {
                        ret.Add(m);
                    }
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
            /// Restituisce il modulo in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Sistema.CModule GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Sistema.CModule m in LoadAll())
                {
                    if (DMD.Strings.Compare(m.ModuleName, value, true) == 0)
                        return m;
                }

                return null;
            }
        }
    
    }

    public partial class Sistema
    {
        private static CModulesModeClass m_Modules = null;

        /// <summary>
        /// Repository dei moduli
        /// </summary>
        public static CModulesModeClass Modules
        {
            get
            {
                if (m_Modules is null)
                    m_Modules = new CModulesModeClass();
                return m_Modules;
            }
        }
    }
}