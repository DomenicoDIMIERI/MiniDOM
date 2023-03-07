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

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="CRegisteredPropertyPage"/>
        /// </summary>
        [Serializable]
        public sealed class CPropertyPagesClass 
            : CModulesClass<CRegisteredPropertyPage>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPropertyPagesClass() 
                : base("modPropertyPages", typeof(CRegisteredPropertyPageCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce un array contenente i tipi delle classi di tipo PropertyPage registrate per il tipo specificato
            /// </summary>
            /// <param name="typeName"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CRegisteredPropertyPage> GetRegisteredPropertyPages(string typeName, string context)
            {
                return GetRegisteredPropertyPages(Users.CurrentUser, typeName, context);
            }

            /// <summary>
            /// Restituisce una collezione di pagine di proprietà visibili all'utente specificato per il tipo e nel contesto specificato
            /// </summary>
            /// <param name="typeName"></param>
            /// <param name="context"></param>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CRegisteredPropertyPage> GetRegisteredPropertyPages(CUser user, string typeName, string context)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                typeName = DMD.Strings.Trim(typeName);
                if (string.IsNullOrEmpty(typeName))
                    throw new ArgumentNullException("typeName");
                context = DMD.Strings.Trim(context);
                var ret = new CCollection<CRegisteredPropertyPage>();
                var items = LoadAll();
                lock (this)
                {
                    foreach (CRegisteredPropertyPage rp in items)
                    {
                        if (rp.IsVisibleToUser(user) && DMD.Strings.Compare(rp.ClassName, typeName, true) == 0 && (string.IsNullOrEmpty(context) || (rp.Context ?? "") == (context ?? "")))
                        {
                            // If rp.TabPageType IsNot Nothing Then
                            ret.Add(rp);
                            // Else
                            // Throw New ArgumentException("La pagina di proprietà [" & rp.TabPageClass & "] definita per il tipo [" & rp.ClassName & "] non è definita")
                            // End If
                        }
                    }
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
            /// Restituisce l'id della pagina associata
            /// </summary>
            /// <param name="typeName"></param>
            /// <param name="pageName"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public int GetPropertyID(string typeName, string pageName, string context)
            {
                return DBUtils.GetID(GetPropertyPage(typeName, pageName, context), 0);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="typeName"></param>
            /// <param name="pageName"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public CRegisteredPropertyPage GetPropertyPage(string typeName, string pageName, string context)
            {
                var items = LoadAll();
                foreach (CRegisteredPropertyPage p in items)
                {
                    if (
                           (p.ClassName ?? "") == (typeName ?? "") 
                        && (p.TabPageClass ?? "") == (pageName ?? "")
                        && (p.Context ?? "") == (context ?? "")
                        )
                        return p;
                }

                return null;
            }

            /// <summary>
            /// Registra la pagina di proprietà
            /// </summary>
            /// <param name="typeName"></param>
            /// <param name="pageName"></param>
            /// <param name="context"></param>
            /// <param name="priority"></param>
            /// <returns></returns>
            public CRegisteredPropertyPage RegisterPropertyPage(string typeName, string pageName, string context, int priority)
            {
                CRegisteredPropertyPage item;
                item = GetPropertyPage(typeName, pageName, context);
                if (item is null)
                {
                    item = new CRegisteredPropertyPage();
                    item.ClassName = typeName;
                    item.TabPageClass = pageName;
                    item.Context = context;
                    AddToCache(item);
                }
                item.Priority = priority;
                item.Save();
                return item;
            }

            /// <summary>
            /// Comparer di oggetti <see cref="CRegisteredPropertyPage"/>
            /// </summary>
            private class RPPriorityComparer 
                : IComparer, IComparer<CRegisteredPropertyPage>
            {
                
                /// <summary>
                /// Compara due oggetti
                /// </summary>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <returns></returns>
                public int Compare(CRegisteredPropertyPage a, CRegisteredPropertyPage b)
                {
                    return a.Priority - b.Priority;
                }

                int IComparer.Compare(object x, object y)
                {
                    return Compare((CRegisteredPropertyPage)x, (CRegisteredPropertyPage)y);
                }
            }

            /// <summary>
            /// Restituisce un array contenente i tipi delle classi di tipo PropertyPage registrate per il tipo specificato
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Type[] GetRegisteredPropertyPages(string typeName)
            {
                var ret = new ArrayList();
                typeName = DMD.Strings.Trim(typeName);
                foreach (CRegisteredPropertyPage rp in LoadAll())
                {
                    if (DMD.Strings.Compare(rp.ClassName, typeName, true) == 0)
                    {
                        if (rp.TabPageType is object)
                        {
                            ret.Add(rp.TabPageType);
                        }
                        else
                        {
                            throw new ArgumentException("La pagina di proprietà [" + rp.TabPageClass + "] definita per il tipo [" + rp.ClassName + "] non è definita");
                        }
                    }
                }

                return (Type[])ret.ToArray(typeof(Type));
            }

            /// <summary>
            /// Restituisce un array contenente i nomi delle classi di tipo PropertyPage registrate per il tipo specificato
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public string[] GetRegisteredPropertyPageNames(string typeName)
            {
                var ret = new ArrayList();
                typeName = DMD.Strings.Trim(typeName);
                foreach (CRegisteredPropertyPage rp in LoadAll())
                {
                    if (DMD.Strings.Compare(rp.ClassName, typeName, true) == 0)
                    {
                        if (!string.IsNullOrEmpty(rp.TabPageClass))
                            ret.Add(rp.TabPageClass);
                    }
                }

                return (string[])ret.ToArray(typeof(string));
            }

            /// <summary>
            /// Restituisce un array contenente i nomi delle classi di tipo PropertyPage registrate per il tipo specificato
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Type[] GetRegisteredPropertyPages(Type type)
            {
                return GetRegisteredPropertyPages(type.Name);
            }

            /// <summary>
            /// Invalida tutte le autorizzazioni utente
            /// </summary>
            internal void InvalidateUserAuth()
            {
                foreach (var o in this.LoadAll())
                    o.InvalidateUserAuth();
            }

            /// <summary>
            /// Invalida tutte le autorizzazione dei gruppi
            /// </summary>
            internal void InvalidateGroupAuth()
            {
                foreach (var o in this.LoadAll())
                    o.InvalidateGroupAuth();
            }

            [NonSerialized] private PropPageGroupAllowNegateRepository m_GroupAllowNegateRepository = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="PropPageGroupAllowNegate"/>
            /// </summary>
            public PropPageGroupAllowNegateRepository GroupAllowNegateRepository
            {
                get
                {
                    if (this.m_GroupAllowNegateRepository is null)
                        this.m_GroupAllowNegateRepository = new PropPageGroupAllowNegateRepository();
                    return this.m_GroupAllowNegateRepository;
                }
            }

            [NonSerialized] private PropPageUserAllowNegateRepository m_UserAllowNegateRepository = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="PropPageUserAllowNegate"/>
            /// </summary>
            public PropPageUserAllowNegateRepository UserAllowNegateRepository
            {
                get
                {
                    if (this.m_UserAllowNegateRepository is null)
                        this.m_UserAllowNegateRepository = new PropPageUserAllowNegateRepository();
                    return this.m_UserAllowNegateRepository;
                }
            }
        }

        
    }


    public partial class Sistema
    {

        private static CPropertyPagesClass m_PropertyPages = null;

        /// <summary>
        /// Repository di oggetti <see cref="CRegisteredPropertyPage"/>
        /// </summary>
        public static CPropertyPagesClass PropertyPages
        {
            get
            {
                if (m_PropertyPages is null) m_PropertyPages = new CPropertyPagesClass();
                return m_PropertyPages;
            }
        }

    }

}