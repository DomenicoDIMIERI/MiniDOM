using System;
using DMD;
using DMD.Databases;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom
{

  
    namespace repositories
    {



        /// <summary>
        /// Repository di oggetti di tipo <see cref="CAzienda"/>
        /// </summary>
        [Serializable]
        public sealed class CAziendeClass 
            : CModulesClass<Anagrafica.CAzienda>
        {
            /// <summary>
            /// Evento generato quando viene modificata l'azienda principale
            /// </summary>
            public event AziendaPrincipaleChangedEventHandler AziendaPrincipaleChanged;

           
            [NonSerialized]
            private Anagrafica.CAziendaPrincipale m_AziendaPrincipale = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAziendeClass() 
                : base("modAziende", typeof(Anagrafica.CAziendeCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base all'ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public override Anagrafica.CAzienda GetItemById(int id)
            {
                return base.GetItemById(id);
            }

            /// <summary>
            /// Restituisce l'elemento in base alla partita iva
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CAzienda GetAziendaByPIVA(string value)
            {
                value = Sistema.Formats.ParsePartitaIVA(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                lock (this)
                {
                    foreach (CacheItem item in CachedItems)
                    {
                        Anagrafica.CAzienda azienda = (Anagrafica.CAzienda)item.Item;
                        if ((azienda.PartitaIVA ?? "") == (value ?? ""))
                            return azienda;
                    }
                }

                using (var cursor = new Anagrafica.CAziendeCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.PageSize = 1;
                    cursor.PartitaIVA.Value = value;
                    return cursor.Item;
                }
                  
            }

            /// <summary>
            /// Restituisce l'elemento corrispondente alla ragione sociale
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CAzienda GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                lock (this)
                {
                    foreach (CacheItem item in CachedItems)
                    {
                        Anagrafica.CAzienda azienda = (Anagrafica.CAzienda)item.Item;
                        if ((azienda.RagioneSociale ?? "") == (value ?? ""))
                            return azienda;
                    }
                }

                using (var cursor = new Anagrafica.CAziendeCursor())
                {
                    cursor.Cognome.Value = value;
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    return cursor.Item;
                }
                    
            }

            /// <summary>
            /// Restituisce l'elemento in base al codice fiscale
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CAzienda GetItemByCF(string value)
            {
                value = Sistema.Formats.ParseCodiceFiscale(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                lock (this)
                {
                    foreach (CacheItem item in CachedItems)
                    {
                        Anagrafica.CAzienda azienda = (Anagrafica.CAzienda)item.Item;
                        if ((azienda.CodiceFiscale ?? "") == (value ?? ""))
                            return azienda;
                    }
                }

                using (var cursor = new Anagrafica.CAziendeCursor())
                {
                    cursor.CodiceFiscale.Value = value;
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azienda principale
            /// </summary>
            public int IDAziendaPrincipale
            {
                get
                {
                    return Sistema.ApplicationContext.IDAziendaPrincipale;
                }

                set
                {
                    Sistema.ApplicationContext.IDAziendaPrincipale = value;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azienda principale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CAziendaPrincipale AziendaPrincipale
            {
                get
                {
                    if (m_AziendaPrincipale is null)
                    {
                        int idAzienda = Sistema.ApplicationContext.IDAziendaPrincipale;
                        var a = Anagrafica.Aziende.GetItemById(idAzienda);
                        if (a is object)
                        {
                            m_AziendaPrincipale = new Anagrafica.CAziendaPrincipale(a);
                        }
                        Sistema.ApplicationContext.IDAziendaPrincipale = DBUtils.GetID(a, 0);
                    }

                    return m_AziendaPrincipale;
                }

                set
                {
                    var oldValue = AziendaPrincipale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AziendaPrincipale = value;
                    Sistema.ApplicationContext.IDAziendaPrincipale = DBUtils.GetID(value, 0);
                    OnAziendaPrincipaleChanged(new ItemEventArgs(value));
                }
            }

            /// <summary>
            /// Imposta l'azienda principale
            /// </summary>
            /// <param name="azienda"></param>
            public void SetAziendaPrincipale(Anagrafica.CAzienda azienda)
            {
                if (azienda is Anagrafica.CAziendaPrincipale)
                {
                    m_AziendaPrincipale = (Anagrafica.CAziendaPrincipale)azienda;
                }
                else
                {
                    m_AziendaPrincipale = new Anagrafica.CAziendaPrincipale(azienda);
                }
            }

            /// <summary>
            /// Genera l'evento AziendaPrincipaleChanged
            /// </summary>
            /// <param name="e"></param>
            private void OnAziendaPrincipaleChanged(ItemEventArgs e)
            {
                AziendaPrincipaleChanged?.Invoke(this, e);
            }
        }
    }

    public partial class Anagrafica
    {

        /// <summary>
        /// Delegato dell'evento AziendaPrincipaleChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AziendaPrincipaleChangedEventHandler(object sender, ItemEventArgs e);


        private static CAziendeClass m_Aziende = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CAzienda"/>
        /// </summary>
        public static CAziendeClass Aziende
        {
            get
            {
                if (m_Aziende is null)
                    m_Aziende = new CAziendeClass();
                return m_Aziende;
            }
        }
    }
}