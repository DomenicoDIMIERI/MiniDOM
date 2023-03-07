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
        /// Repository di <see cref="CTicketCategory"/>
        /// </summary>
        [Serializable]
        public class CTicketCategoriesClass 
            : CModulesClass<CTicketCategory>
        {
            [NonSerialized]
            private CKeyCollection<CCollection<CTicketCategory>> m_UserAllowedCategories;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketCategoriesClass() 
                : base("modTicketsCategories", typeof(minidom.Office.CTicketCategoryCursor), -1)
            {
                m_UserAllowedCategories = null;
            }

            

            /// <summary>
            /// Restituisce l'elenco delle sottocategorie di una categoria
            /// </summary>
            /// <param name="cat"></param>
            /// <returns></returns>
            public string[] GetArraySottocategorie(string cat)
            {
                cat = Strings.Trim(cat);
                var dic = new Dictionary<string, string>();
                foreach (var c in LoadAll())
                {
                    if (
                        string.IsNullOrEmpty(cat) || DMD.Strings.EQ(c.Categoria, cat, true) 
                        && !dic.ContainsKey(c.Sottocategoria)
                        )
                        dic.Add(c.Sottocategoria, c.Sottocategoria);
                }

                return new List<string>(dic.Values).ToArray();
            }

            /// <summary>
            /// Restituisce l'oggetto in base a categoria e sottocategoria
            /// </summary>
            /// <param name="categoria"></param>
            /// <param name="sottocategoria"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CTicketCategory GetItemByName(string categoria, string sottocategoria)
            {
                categoria = DMD.Strings.Trim(categoria);
                sottocategoria = DMD.Strings.Trim(sottocategoria);
                foreach (var item in LoadAll())
                {
                    if ((item.Categoria ?? "") == (categoria ?? "") && (item.Sottocategoria ?? "") == (sottocategoria ?? ""))
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce gli utenti a cui vengono inviati le notifiche per la categoria e la sottocategoria.
            /// Agli utenti di { categoria, sottocategoria } vengono aggiunti anche gli utenti di { categoria, NULL }
            /// </summary>
            /// <param name="categoria"></param>
            /// <param name="sottocategoria"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CUser> GetTargetUsers(string categoria, string sottocategoria)
            {
                var targetUser = new CKeyCollection<CUser>();
                var cat = minidom.Office.TicketCategories.GetItemByName(categoria, "");
                if (cat is object)
                {
                    foreach (var user in cat.NotifyUsers)
                    {
                        if (!targetUser.ContainsKey(user.UserName))
                            targetUser.Add(user.UserName, user);
                    }
                }

                cat = minidom.Office.TicketCategories.GetItemByName(categoria, sottocategoria);
                if (cat is object)
                {
                    foreach (var user in cat.NotifyUsers)
                    {
                        if (!targetUser.ContainsKey(user.UserName))
                            targetUser.Add(user.UserName, user);
                    }
                }

                return new CCollection<Sistema.CUser>(targetUser);
            }

            /// <summary>
            /// Restituisce l'elenco degli utenti a cui inviare le notifiche per la categoria e la sottocategoria.
            /// </summary>
            /// <param name="categoria"></param>
            /// <param name="sottocategoria"></param>
            /// <returns></returns>
            public CCollection<Sistema.CUser> GetNotifiedUsers(string categoria, string sottocategoria)
            {
                var targetUser = new CKeyCollection<Sistema.CUser>();
                foreach (Sistema.CUser user in minidom.Office.Tickets.GruppoResponsabili.Members)
                {
                    if (!targetUser.ContainsKey(user.UserName))
                        targetUser.Add(user.UserName, user);
                }

                var cat = minidom.Office.TicketCategories.GetItemByName(categoria, "");
                if (cat is object)
                {
                    foreach (Sistema.CUser user in cat.NotifyUsers)
                    {
                        if (!targetUser.ContainsKey(user.UserName))
                            targetUser.Add(user.UserName, user);
                    }
                }

                cat = minidom.Office.TicketCategories.GetItemByName(categoria, sottocategoria);
                if (cat is object)
                {
                    foreach (Sistema.CUser user in cat.NotifyUsers)
                    {
                        if (!targetUser.ContainsKey(user.UserName))
                            targetUser.Add(user.UserName, user);
                    }
                }

                foreach (Sistema.CUser user in minidom.Office.Tickets.GruppoEsclusi.Members)
                {
                    if (targetUser.ContainsKey(user.UserName))
                        targetUser.RemoveByKey(user.UserName);
                }

                return new CCollection<Sistema.CUser>(targetUser);
            }

            private void BuildUserCat()
            {
                lock (this)
                {
                    if (m_UserAllowedCategories is null)
                    {
                        m_UserAllowedCategories = new CKeyCollection<CCollection<minidom.Office.CTicketCategory>>();
                        foreach (minidom.Office.CTicketCategory cat in LoadAll())
                        {
                            foreach (Sistema.CUser user in cat.NotifyUsers)
                            {
                                var col = m_UserAllowedCategories.GetItemByKey(user.UserName);
                                if (col is null)
                                {
                                    col = new CCollection<minidom.Office.CTicketCategory>();
                                    m_UserAllowedCategories.Add(user.UserName, col);
                                }

                                col.Add(cat);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce le categorie associate all'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CCollection<CTicketCategory> GetUserAllowedCategories(Sistema.CUser user)
            {
                lock (this)
                {
                    BuildUserCat();
                    CCollection<minidom.Office.CTicketCategory> ret;
                    ret = m_UserAllowedCategories.GetItemByKey(user.UserName);
                    if (ret is null)
                        ret = new CCollection<minidom.Office.CTicketCategory>();
                    return ret;
                }
            }

            /// <summary>
            /// Invalida le categoria associate all'utente
            /// </summary>
            protected internal void InvalidateUserAllowedCategories()
            {
                lock (this)
                    m_UserAllowedCategories = null;
            }
        }
    }

    public partial class Office
    {
        private static CTicketCategoriesClass m_TicketCategories = null;

        /// <summary>
        /// Repository di <see cref="CTicketCategory"/>
        /// </summary>
        /// <returns></returns>
        public static CTicketCategoriesClass TicketCategories
        {
            get
            {
                if (m_TicketCategories is null)
                    m_TicketCategories = new CTicketCategoriesClass();
                return m_TicketCategories;
            }
        }
    }
}