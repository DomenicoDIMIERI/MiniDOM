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
        /// Repository di oggetti <see cref="CContinente"/>
        /// </summary>
        [Serializable]
        public sealed class CContinentiClass
            : CModulesClass<CContinente>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContinentiClass() 
                : base("modContinenti", typeof(CContinentiCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public CContinente GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                foreach (CContinente item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, name, true) == 0)
                        return item;
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
            /// Cerca tutte le Continenti in base al filtro
            /// </summary>
            /// <param name="value"></param>
            /// <param name="strict"></param>
            /// <returns></returns>
            public CCollection<Luogo> Find(string value, bool strict = false)
            {
                var col = new CCollection<Luogo>();
                string citta = Luoghi.GetComune(value);
                string provincia = Luoghi.GetProvincia(value);
                foreach (CContinente n in LoadAll())
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