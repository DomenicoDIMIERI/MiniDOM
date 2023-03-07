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
using static minidom.Finanziaria;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CAreaManager"/>
        /// </summary>
        [Serializable]
        public sealed class CAreaManagersClass 
            : CModulesClass<Finanziaria.CAreaManager>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CAreaManagersClass() 
                : base("modAreaManager", typeof(Finanziaria.CAreaManagerCursor), -1)
            {
            }


            /// <summary>
            /// Restituisce l'area manager corrispondente alla persona specificata
            /// </summary>
            /// <param name="personID"></param>
            /// <returns></returns>
            public Finanziaria.CAreaManager GetItemByPersona(int personID)
            {
                if (personID == 0)
                    return null;
                foreach (var item in LoadAll())
                {
                    if (item.PersonaID == personID)
                        return item;
                }

                return null;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CAreaManagersClass m_AreaManagers = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CAreaManager"/>
        /// </summary>
        /// <returns></returns>
        public static CAreaManagersClass AreaManagers
        {
            get
            {
                if (m_AreaManagers is null)
                    m_AreaManagers = new CAreaManagersClass();
                return m_AreaManagers;
            }
        }
    }
}