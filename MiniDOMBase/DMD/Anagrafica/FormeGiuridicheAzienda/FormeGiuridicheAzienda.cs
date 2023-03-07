using System;
using System.Collections;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Anagrafica;
using static minidom.Sistema;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CFormaGiuridicaAzienda"/>
        /// </summary>
        [Serializable]
        public sealed class CFormeGiuridicheAziendaClass
            : CModulesClass<Anagrafica.CFormaGiuridicaAzienda>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFormeGiuridicheAziendaClass() 
                : base("modFormeGiuridicheAzienda", typeof(Anagrafica.CFormeGiuridicheAziendaCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CFormaGiuridicaAzienda GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;

                var items = this.LoadAll();
                foreach (var f in items)
                {
                    if (
                           f.Stato == ObjectStatus.OBJECT_VALID 
                        && DMD.Strings.EQ(f.Nome, value, true)
                        )
                        return f;
                }

                return null;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CFormeGiuridicheAziendaClass m_FormeGiuridicheAzienda = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CFormaGiuridicaAzienda"/>
        /// </summary>
        public static CFormeGiuridicheAziendaClass FormeGiuridicheAzienda
        {
            get
            {
                if (m_FormeGiuridicheAzienda is null)
                    m_FormeGiuridicheAzienda = new CFormeGiuridicheAziendaClass();
                return m_FormeGiuridicheAzienda;
            }
        }
    }
}