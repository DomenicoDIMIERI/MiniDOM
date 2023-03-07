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
using static minidom.CustomerCalls;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository su oggetti di tipo <see cref="CCampagnaCRM"/>
        /// </summary>
        [Serializable]
        public sealed class CCampagneCRMClass 
            : CModulesClass<CustomerCalls.CCampagnaCRM>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagneCRMClass() 
                : base("modCRMCampains", typeof(CustomerCalls.CCampagnaCRMCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce la campagna in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public CustomerCalls.CCampagnaCRM GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                var items = LoadAll();
                foreach (CustomerCalls.CCampagnaCRM item in items)
                {
                    if (   
                           item.Stato == ObjectStatus.OBJECT_VALID 
                        && DMD.Strings.Compare(item.Nome, nome, true) == 0
                        )
                        return item;
                }

                return null;
            }
        }
    }

    public partial class CustomerCalls
    {
        private static CCampagneCRMClass m_CampangeCRM = null;

        /// <summary>
        /// Repository su oggetti di tipo <see cref="CCampagnaCRM"/>
        /// </summary>
        public static CCampagneCRMClass CampagneCRM
        {
            get
            {
                if (m_CampangeCRM is null)
                    m_CampangeCRM = new CCampagneCRMClass();
                return m_CampangeCRM;
            }
        }
    }
}