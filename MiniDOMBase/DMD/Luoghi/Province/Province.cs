using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CProvincia"/>
        /// </summary>
        [Serializable]
        public sealed class CProvinceClass
            : CModulesClass<CProvincia>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProvinceClass() 
                : base("modProvince", typeof(CProvinceCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce la provincia in base alla sigla
            /// </summary>
            /// <param name="sigla"></param>
            /// <returns></returns>
            public CProvincia GetItemBySigla(string sigla)
            {
                sigla = DMD.Strings.Trim(sigla);
                if (string.IsNullOrEmpty(sigla))
                    return null;
                foreach (CProvincia item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Sigla, sigla, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce la provincia in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public CProvincia GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (CProvincia item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, name, true) == 0)
                        return item;
                }

                return null;
            }
        }
    }
}