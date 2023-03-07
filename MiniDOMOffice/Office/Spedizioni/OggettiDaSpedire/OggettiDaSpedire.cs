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
        /// Repository di <see cref="OggettoDaSpedire"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class COggettiDaSpedireClass 
            : CModulesClass<OggettoDaSpedire>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public COggettiDaSpedireClass() 
                : base("modOfficeOggettiDaSpedire", typeof(OggettiDaSpedireCursor))
            {
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeOggettiDaSpedire");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeOggettiDaSpedire");
            //        ret.Description = "Oggetti da Spedire";
            //        ret.DisplayName = "Oggetti da Spedire";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    return ret;
            //}
        }
    }

    public partial class Office
    {
        private static COggettiDaSpedireClass m_OggettiDaSpedire = null;

        /// <summary>
        /// Repository di <see cref="OggettoDaSpedire"/>
        /// </summary>
        /// <remarks></remarks>
        public static COggettiDaSpedireClass OggettiDaSpedire
        {
            get
            {
                if (m_OggettiDaSpedire is null)
                    m_OggettiDaSpedire = new COggettiDaSpedireClass();
                return m_OggettiDaSpedire;
            }
        }
    }
}