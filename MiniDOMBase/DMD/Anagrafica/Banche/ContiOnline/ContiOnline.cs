using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using DMD.Databases.Collections;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="ContoOnline"/>
        /// </summary>
        [Serializable]
        public class CContiOnlineClass
            : CModulesClass<Anagrafica.ContoOnline>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContiOnlineClass()
                : base("modContiOnline", typeof(Anagrafica.ContoOnlineCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce l'elemento per nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.ContoOnline GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
               
                using(var cursor = new Anagrafica.ContoOnlineCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Name.Value = value;
                    return cursor.Item;
                }
                 
            }

            /// <summary>
            /// Restituisce l'elemento in base all'account
            /// </summary>
            /// <param name="sito"></param>
            /// <param name="account"></param>
            /// <returns></returns>
            public Anagrafica.ContoOnline GetItemByAccount(string sito, string account)
            {
                sito = DMD.Strings.Trim(sito);
                account = DMD.Strings.Trim(account);
                if (string.IsNullOrEmpty(sito) || string.IsNullOrEmpty(account))
                    return null;

                using (var cursor = new ContoOnlineCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Sito.Value = sito;
                    cursor.Account.Value = account;
                    cursor.DataInizio.SortOrder = SortEnum.SORT_DESC;
                    return cursor.Item;
                }
            }
        }
    }

    public partial class Anagrafica
    {
        private static CContiOnlineClass m_ContiOnline = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="ContoOnline"/>
        /// </summary>
        public static CContiOnlineClass ContiOnline
        {
            get
            {
                if (m_ContiOnline is null)
                    m_ContiOnline = new CContiOnlineClass();
                return m_ContiOnline;
            }
        }
    }
}