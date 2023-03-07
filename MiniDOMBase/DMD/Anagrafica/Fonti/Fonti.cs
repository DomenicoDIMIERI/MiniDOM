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
        /// Repository di oggetti di tipo <see cref="CFonte"/>
        /// </summary>
        [Serializable]
        public sealed class CFontiClass
            : CModulesClass<Anagrafica.CFonte>
        {
            
            private List<IFonteProvider> m_Providers = new List<IFonteProvider>();

            // Public Shared Event FonteMerged(ByVal e As FonteMergedEventArgs)

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFontiClass() 
                : base("modFonti", typeof(Anagrafica.CFontiCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce la fonte in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFonte GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;

                foreach (var item in LoadAll())
                {
                    if (DMD.Strings.EQ(item.Nome, value, true))
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce la fonte associata all'annuncio
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFonte GetItemByIDAnnuncio(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                
                foreach (Anagrafica.CFonte item in LoadAll())
                {
                    if (DMD.Strings.EQ(item.IDAnnuncio, value, true))
                        return item;
                }

                return null;
                // End SyncLock
            }

            /// <summary>
            /// Restituisce la fonte in base all'ID
            /// </summary>
            /// <param name="providerName"></param>
            /// <param name="tipo"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public IFonte GetItemById(string providerName, string tipo, int id)
            {
                // SyncLock Me.lockObject
                if (id == 0)
                    return null;

                var p = GetProviderByName(providerName);
                if (p is null)
                    return null;
                return p.GetItemById(tipo, id);
                // End SyncLock
            }

            /// <summary>
            /// Restituisce la fonte in base al nome
            /// </summary>
            /// <param name="providerName"></param>
            /// <param name="tipo"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFonte GetItemByName(string providerName, string tipo, string value)
            {
                // SyncLock Me.lockObject
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                var p = GetProviderByName(providerName);
                if (p is null)
                    return null;
                return p.GetItemByName(tipo, value);
                // End SyncLock
            }

            /// <summary>
            /// Restituisce le fonti supportate dal provider
            /// </summary>
            /// <param name="providerName"></param>
            /// <param name="tipo"></param>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            public IFonte[] GetItemsAsArray(string providerName, string tipo, bool onlyValid = true)
            {
                // SyncLock Me.lockObject
                var provider = GetProviderByName(providerName);
                if (provider is null)
                    return null;
                return provider.GetItemsAsArray(tipo, onlyValid);
                // End SyncLock
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */

            /// <summary>
            /// Restituisce le statistiche esterne
            /// </summary>
            /// <param name="entryPage"></param>
            /// <param name="di"></param>
            /// <param name="df"></param>
            /// <returns></returns>
            public CKeyCollection<Anagrafica.CFonteExternalStats> GetStatisticheEsterne(string entryPage, DateTime? di, DateTime? df)
            {
                // SyncLock Me.lockObject
                var ret = new CKeyCollection<Anagrafica.CFonteExternalStats>();
                entryPage = DMD.Strings.Trim(entryPage);
                if (string.IsNullOrEmpty(entryPage))
                    throw new ArgumentOutOfRangeException("entryPage");
                string text = Sistema.RPC.InvokeMethod(entryPage, "di", Sistema.RPC.date2n(di), "df", Sistema.RPC.date2n(df));
                if (!string.IsNullOrEmpty(text))
                {
                    CCollection items = (CCollection)DMD.XML.Utils.Serializer.Deserialize(text);
                    for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                    {
                        Anagrafica.CFonteExternalStats item = (Anagrafica.CFonteExternalStats)items[i];
                        ret.Add(item.IDAnnuncio, item);
                    }
                }

                return ret;
                // End SyncLock
            }

            /// <summary>
            /// Registra un provider di fonti
            /// </summary>
            /// <param name="provider"></param>
            public void RegisterProvider(Anagrafica.IFonteProvider provider)
            {
                lock (this)
                    m_Providers.Add(provider);
            }

            /// <summary>
            /// Rimuove un provider di fonti
            /// </summary>
            /// <param name="provider"></param>
            public void UnregisterProvider(Anagrafica.IFonteProvider provider)
            {
                lock (this)
                    m_Providers.Remove(provider);
            }

            /// <summary>
            /// Restituisce l'elenco dei provider delle fonti
            /// </summary>
            public List<IFonteProvider> Providers
            {
                get
                {
                    lock (this)
                    {
                        var ret = new List<Anagrafica.IFonteProvider>(this.m_Providers);
                        return ret;
                    }
                }
            }

            /// <summary>
            /// Restituisce il provider in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Anagrafica.IFonteProvider GetProviderByName(string name)
            {
                name = DMD.Strings.Trim(name);
                for (int i = 0, loopTo = m_Providers.Count - 1; i <= loopTo; i++)
                {
                    Anagrafica.IFonteProvider p = (Anagrafica.IFonteProvider)m_Providers[i];
                    var items = p.GetSupportedNames();
                    for (int j = 0, loopTo1 = DMD.Arrays.UBound(items); j <= loopTo1; j++)
                    {
                        if ((items[j] ?? "") == (name ?? ""))
                            return p;
                    }
                }

                return null;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CFontiClass m_Fonti = null;

        /// <summary>
        /// Repository di oggetti <see cref="CFonte"/>
        /// </summary>
        public static CFontiClass Fonti
        {
            get
            {
                if (m_Fonti is null)
                    m_Fonti = new CFontiClass();
                return m_Fonti;
            }
        }
    }
}