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
        /// Repository di oggetti <see cref="CNazione"/>
        /// </summary>
        [Serializable]
        public sealed class CNazioniClass
            : CModulesClass<CNazione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CNazioniClass() 
                : base("modNazioni", typeof(CNazioniCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public CNazione GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (CNazione item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, name, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce l'elemento in base al codice catastale
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public CNazione GetItemByCodiceCatastale(string code)
            {
                code = DMD.Strings.Trim(code);
                if (string.IsNullOrEmpty(code))
                    return null;
                foreach (CNazione c in LoadAll())
                {
                    if (DMD.Strings.Compare(c.CodiceCatasto, code, true) == 0)
                        return c;
                }

                return null;
            }

            private bool Find_Compare(string a, string b, bool strict)
            {
                if (strict)
                    return DMD.Strings.InStr(a, b, true) > 0;
                a = DMD.Strings.OnlyCharsAndNumbers(a);
                b = DMD.Strings.OnlyCharsAndNumbers(b);
                return DMD.Strings.InStr(a, b, true) > 0;
            }

            /// <summary>
            /// Cerca tutte le nazioni in base al filtro
            /// </summary>
            /// <param name="value"></param>
            /// <param name="strict"></param>
            /// <returns></returns>
            public CCollection<Luogo> Find(string value, bool strict = false)
            {
                var col = new CCollection<Luogo>();
                string citta = Luoghi.GetComune(value);
                string provincia = Luoghi.GetProvincia(value);
                foreach (CNazione n in LoadAll())
                {
                    if (Find_Compare(n.Nome, citta, strict))
                    {
                        col.Add(n);
                    }
                }

                return col;
            }
        }
    }
}