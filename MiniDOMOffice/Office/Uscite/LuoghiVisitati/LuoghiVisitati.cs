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
    public partial class Office
    {


        /// <summary>
        /// Repository di <see cref="LuogoVisitato"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CLuoghiVisitatiClass 
            : CModulesClass<LuogoVisitato>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CLuoghiVisitatiClass() 
                : base("modOfficeLuoghiV", typeof(LuoghiVisitatiCursor))
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeLuoghiV");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeLuoghiV");
            //        ret.Description = "Luoghi Visitati";
            //        ret.DisplayName = "Luoghi Visitati";
            //        ret.Parent = Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    return ret;
            //}
        }

        private static CLuoghiVisitatiClass m_LuoghiVisitati = null;

        /// <summary>
        /// Repository di <see cref="LuogoVisitato"/>
        /// </summary>
        /// <remarks></remarks>
        public static CLuoghiVisitatiClass LuoghiVisitati
        {
            get
            {
                if (m_LuoghiVisitati is null)
                    m_LuoghiVisitati = new CLuoghiVisitatiClass();
                return m_LuoghiVisitati;
            }
        }
    }
}