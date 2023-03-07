using System;
using DMD;
using DMD.XML;
using minidom;
using minidom.Internals;
using static minidom.Sistema;


namespace minidom
{
    public partial class WebSite
    {
        /// <summary>
    /// Gestione del collegamenti in prima pagina
    /// </summary>
    /// <remarks></remarks>
        public sealed class CCollegamentiClass : Sistema.CModulesClass<CCollegamento>
        {
            internal CCollegamentiClass() : base("modLinks", typeof(CCollegamentiCursor), -1)
            {
            }


            /// <summary>
        /// Restituisce i link visibili all'utente
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CCollegamento> GetUserLinks(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                var ret = new CCollection<CCollegamento>();
                if (Databases.GetID(user) == 0)
                    return ret;
                var items = LoadAll();
                foreach (CCollegamento item in items)
                {
                    if (item.Stato == Databases.ObjectStatus.OBJECT_VALID && item.IsVisibleToUser(user))
                        ret.Add(item);
                }

                ret.Sort();
                return ret;
            }
        }

        private static CCollegamentiClass m_Collegamenti = null;

        public static CCollegamentiClass Collegamenti
        {
            get
            {
                if (m_Collegamenti is null)
                    m_Collegamenti = new CCollegamentiClass();
                return m_Collegamenti;
            }
        }
    }
}