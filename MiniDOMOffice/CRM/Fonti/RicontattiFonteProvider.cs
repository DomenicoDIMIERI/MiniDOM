using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.Anagrafica;
using DMD.Databases.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Provider delle fonti di tipo Utente
        /// </summary>
        public sealed class RicontattiFonteProvider 
            : Anagrafica.IFonteProvider
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public RicontattiFonteProvider()
            {
                 
            }
 
            /// <summary>
            /// Restituisce l'elemento in base all'id
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public IFonte GetItemById(string nome, int id)
            {
                return Sistema.Users.GetItemById(id);
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public IFonte GetItemByName(string nome, string name)
            {
                return Sistema.Users.GetItemByName(name);
            }

            /// <summary>
            /// Restituisce tutti gli elementi per nome
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            public IFonte[] GetItemsAsArray(string nome, bool onlyValid = true)
            {
                var grp = Sistema.Groups.GetItemByName("CRM");
                var ret = new List<IFonte>();
                foreach (Sistema.CUser user in grp.Members)
                {
                    if (user.Visible && !onlyValid | user.UserStato == Sistema.UserStatus.USER_ENABLED)
                    {
                        ret.Add(user);
                    }
                }

                ret.Sort();
                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce il tipo di fonte
            /// </summary>
            /// <returns></returns>
            public string[] GetSupportedNames()
            {
                return new string[] { "Ricontatto" };
            }
        }
    }
}