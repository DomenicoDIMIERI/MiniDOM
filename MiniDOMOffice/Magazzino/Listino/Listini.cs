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
        /// Repository di <see cref="Listino"/>
        /// </summary>
        [Serializable]
        public class CListiniClass 
            : CModulesClass<Listino>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CListiniClass() 
                : base("modOfficeListini", typeof(Listino), 0)
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
        }
    }

    public partial class Store
    {
        private static CListiniClass m_Listini = null;

        /// <summary>
        /// Repository di <see cref="Listino"/>
        /// </summary>
        public static CListiniClass Listini
        {
            get
            {
                if (m_Listini is null)
                    m_Listini = new CListiniClass();
                return m_Listini;
            }
        }
    }
}