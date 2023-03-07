using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CListaRicontatti"/>
        /// </summary>
        [Serializable]
        public sealed class CListeRicontattoClass 
            : CModulesClass<Anagrafica.CListaRicontatti>
        {
            
            /// <summary>
            /// Repository degli elementi della lista di ricontatto
            /// </summary>
            private CListeRicontattoItemsClass m_Items;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CListeRicontattoClass() 
                : base("modListeRicontatto", typeof(Anagrafica.CListaRicontattiCursor), -1)
            {
                m_Items = null;                 
            }

            

            /// <summary>
            /// Restituisce la lista in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public Anagrafica.CListaRicontatti GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (Anagrafica.CListaRicontatti item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Name, nome, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce la collezione di tutte le liste di ricontatto
            /// </summary>
            /// <returns></returns>
            public CCollection<Anagrafica.CListaRicontatti> GetListeRicontatto()
            {
                return this.LoadAll();
            }

            /// <summary>
            /// Restituisce il repository degli elementi delle liste di ricontatto
            /// </summary>
            public CListeRicontattoItemsClass Items
            {
                get
                {
                    lock (this)
                    {
                        if (m_Items is null)
                            m_Items = new CListeRicontattoItemsClass();
                        return m_Items;
                    }
                }
            }
        }
    }

    public partial class Anagrafica
    {
        private static CListeRicontattoClass m_ListeRicontatto = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CListaRicontatti"/>
        /// </summary>
        public static CListeRicontattoClass ListeRicontatto
        {
            get
            {
                if (m_ListeRicontatto is null)
                    m_ListeRicontatto = new CListeRicontattoClass();
                return m_ListeRicontatto;
            }
        }
    }
}