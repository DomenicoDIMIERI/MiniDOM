using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CGroup"/>
        /// </summary>
        [Serializable]
        public sealed partial class CGroupsClass 
            : CModulesClass<CGroup>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGroupsClass() 
                : base("modGruppi", typeof(CGroupCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Sistema.CGroup GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Sistema.CGroup grp in LoadAll())
                {
                    if (DMD.Strings.Compare(grp.GroupName, value, true) == 0)
                        return grp;
                }

                return null;
            }

            private bool ContainsKey(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return false;
                foreach (Sistema.CGroup item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.GroupName, value, true) == 0)
                        return true;
                }

                return false;
            }

            /// <summary>
            /// Restituisce il primo nome disponibile
            /// </summary>
            /// <param name="baseName"></param>
            /// <returns></returns>
            public string GetFirstAvailableGroupName(string baseName)
            {
                lock (this)
                {
                    baseName = DMD.Strings.Trim(baseName);
                    string nome = baseName;
                    int i;
                    bool t;
                    t = ContainsKey(nome);
                    i = 0;
                    while (t)
                    {
                        i = i + 1;
                        nome = baseName + " (" + i + ")";
                        t = ContainsKey(nome);
                    }

                    return nome;
                }
            }

            /// <summary>
            /// Restituisce il primo nome disponibile
            /// </summary>
            /// <returns></returns>
            public string GetFirstAvailableGroupName()
            {
                return GetFirstAvailableGroupName("Gruppo");
            }

             
        }
    }

    public partial class Sistema
    {
        private static CGroupsClass m_Groups = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CGroup"/>
        /// </summary>
        public static CGroupsClass Groups
        {
            get
            {
                if (m_Groups is null)
                    m_Groups = new CGroupsClass();
                return m_Groups;
            }
        }
    }
}