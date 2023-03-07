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
        /// Repository di oggetti di tipo <see cref="CRegione"/>
        /// </summary>
        [Serializable]
        public sealed class CRegioniClass 
            : CModulesClass<CRegione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegioniClass() 
                : base("modRegioni", typeof(CRegioniCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public CRegione GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (CRegione item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, nome, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce l'elemento in base alla sigla
            /// </summary>
            /// <param name="sigla"></param>
            /// <returns></returns>
            public CRegione GetItemBySigla(string sigla)
            {
                sigla = DMD.Strings.Trim(sigla);
                if (string.IsNullOrEmpty(sigla))
                    return null;
                foreach (CRegione item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Sigla, sigla, true) == 0)
                        return item;
                }

                return null;
            }
        }
    }
}