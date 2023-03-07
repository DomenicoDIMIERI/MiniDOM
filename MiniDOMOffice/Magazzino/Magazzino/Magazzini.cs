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
        /// Repository di oggetti <see cref="Magazzino"/>
        /// </summary>
        [Serializable]
        public class CMagazziniClass 
            : CModulesClass<Magazzino>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMagazziniClass() 
                : base("modOfficeMagazzini", typeof(MagazzinoCursor), 0)
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
        private static CMagazziniClass m_Magazzini = null;

        /// <summary>
        /// Repository di oggetti <see cref="Magazzino"/>
        /// </summary>
        public static CMagazziniClass Magazzini
        {
            get
            {
                if (m_Magazzini is null)
                    m_Magazzini = new CMagazziniClass();
                return m_Magazzini;
            }
        }
    }
}