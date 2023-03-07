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
using static minidom.Store;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CategoriaArticolo"/>
        /// </summary>
        [Serializable]
        public class CCategorieArticoliClass
            : CModulesClass<CategoriaArticolo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCategorieArticoliClass() 
                : base("modOfficeCategorieArticoli", typeof(CategoriaArticoloCursor), -1)
            {
            }

            //protected override Sistema.CModule CreateModuleInfo()
            //{
            //    var m = base.CreateModuleInfo();
            //    m.Parent = minidom.Office.Module;
            //    m.Visible = true;
            //    m.Save();
            //    return m;
            //}

            /// <summary>
            /// Restituisce la categoria in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public CategoriaArticolo GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                var items = LoadAll();
                foreach (var c in items)
                {
                    if (DMD.Strings.Compare(c.Nome, value, true) == 0)
                        return c;
                }

                return null;
            }
        }
    }

    public partial class Store
    {
        private static CCategorieArticoliClass m_CategorieArticoli = null;

        /// <summary>
        /// Repository di oggetti <see cref="CategoriaArticolo"/>
        /// </summary>
        /// <returns></returns>
        public static CCategorieArticoliClass CategorieArticoli
        {
            get
            {
                if (m_CategorieArticoli is null)
                    m_CategorieArticoli = new CCategorieArticoliClass();
                return m_CategorieArticoli;
            }
        }
    }
}