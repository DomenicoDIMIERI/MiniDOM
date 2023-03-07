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
        /// Repository di <see cref="MarcaArticolo"/>
        /// </summary>
        [Serializable]
        public class CMarcheArticoliClass 
            : CModulesClass<MarcaArticolo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMarcheArticoliClass() 
                : base("modOfficeMarcheArticoli", typeof(MarcaArticolo), 0)
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
        private static CMarcheArticoliClass m_MarcheArticoli = null;

        /// <summary>
        /// Repository di <see cref="MarcaArticolo"/>
        /// </summary>
        public static CMarcheArticoliClass MarcheArticoli
        {
            get
            {
                if (m_MarcheArticoli is null)
                    m_MarcheArticoli = new CMarcheArticoliClass();
                return m_MarcheArticoli;
            }
        }
    }
}