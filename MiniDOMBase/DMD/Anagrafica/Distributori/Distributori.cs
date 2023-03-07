using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using DMD.Databases.Collections;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CDistributore"/>
        /// </summary>
        [Serializable]
        public sealed class CDistributoriClass 
            : CModulesClass<Anagrafica.CDistributore>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CDistributoriClass()
                : base("modDistributori", typeof(Anagrafica.CDistributoriCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CDistributore GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Anagrafica.CDistributore item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, value, true) == 0)
                        return item;
                }

                return null;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CDistributoriClass m_Distributori = null;

        /// <summary>
        /// Repository di oggetti <see cref="CDistributore"/>
        /// </summary>
        public static CDistributoriClass Distributori
        {
            get
            {
                if (m_Distributori is null)
                    m_Distributori = new CDistributoriClass();
                return m_Distributori;
            }
        }
    }
}