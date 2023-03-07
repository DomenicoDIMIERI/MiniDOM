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
        /// Repository di <see cref="PBX"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CPBXsClass 
            : CModulesClass<PBX>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CPBXsClass() 
                : base("modOfficePBX", typeof(minidom.Office.PBX), -1)
            {
            }

            
            //public override void Initialize()
            //{
            //    base.Initialize();
            //    var table = Database.Tables.GetItemByKey("tbl_OfficePBX");
            //    if (table is null)
            //    {
            //    }
            //}
        }
    }

    public partial class Office
    {
        private static CPBXsClass m_PBXs = null;

        /// <summary>
        /// Repository di <see cref="PBX"/>
        /// </summary>
        /// <remarks></remarks>
        public static CPBXsClass PBXs
        {
            get
            {
                if (m_PBXs is null)
                    m_PBXs = new CPBXsClass();
                return m_PBXs;
            }
        }
    }
}