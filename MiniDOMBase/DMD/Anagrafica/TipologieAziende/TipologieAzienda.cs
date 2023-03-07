using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CTipologiaAzienda" /> 
        /// </summary>
        [Serializable]
        public sealed class CTipologieAziendaClass 
            : CModulesClass<CTipologiaAzienda>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTipologieAziendaClass() 
                : base("modTipologieAzienda", typeof(CTipologiaAziendaCursor), -1)
            {
            }
        }
    }

    public partial class Anagrafica
    {
        

        private static CTipologieAziendaClass m_TipologieAzienda = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CTipologiaAzienda" /> 
        /// </summary>
        public static CTipologieAziendaClass TipologieAzienda
        {
            get
            {
                if (m_TipologieAzienda is null)
                    m_TipologieAzienda = new CTipologieAziendaClass();
                return m_TipologieAzienda;
            }
        }
    }
}