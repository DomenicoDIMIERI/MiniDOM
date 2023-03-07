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
using static minidom.CustomerCalls;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CContattoUtenteTemplate"/>
        /// </summary>
        [Serializable]
        public sealed class CContattoUtenteTemplateRepository 
            : CModulesClass<CustomerCalls.CContattoUtenteTemplate>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoUtenteTemplateRepository() 
                : base("modCRMTemplates", typeof(CustomerCalls.CContattoUtenteTemplateCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce il modello in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CustomerCalls.CContattoUtenteTemplate GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                var items = LoadAll();
                foreach (CustomerCalls.CContattoUtenteTemplate item in items)
                {
                    if (
                        item.Attivo 
                        && 
                        DMD.Strings.Compare(item.Nome, value, true) == 0
                        )
                        return item;
                }

                foreach (CustomerCalls.CContattoUtenteTemplate item in items)
                {
                    if (DMD.Strings.Compare(item.Nome, value, true) == 0)
                        return item;
                }

                return null;
            }
        }
    }

    public partial class CustomerCalls
    {
        private static CContattoUtenteTemplateRepository m_Templates = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CContattoUtenteTemplate"/>
        /// </summary>
        public static CContattoUtenteTemplateRepository Templates
        {
            get
            {
                if (m_Templates is null)
                    m_Templates = new CContattoUtenteTemplateRepository();
                return m_Templates;
            }
        }
    }
}