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
        /// Repository di oggetti di tipo <see cref="CBanca"/>
        /// </summary>
        public sealed class CBancheClass 
            : CModulesClass<CBanca>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBancheClass() 
                : base("modBanche", typeof(CBancheCursor), 0)
            {
            }


            /// <summary>
            /// Restituisce la filiale corrispondente all'abi e al cab
            /// </summary>
            /// <param name="abi"></param>
            /// <param name="cab"></param>
            /// <returns></returns>
            public CBanca GetItemByABIeCAB(string abi, string cab)
            {
                abi = DMD.Strings.Trim(abi);
                cab = DMD.Strings.Trim(cab);
                if (string.IsNullOrEmpty(abi) & string.IsNullOrEmpty(cab))
                    return null;

                using (var cursor = new CBancheCursor())
                {
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    cursor.ABI.Value = abi;
                    cursor.CAB.Value = cab;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    return ret = cursor.Item;                    
                }
            }
        }

    }

    public partial class Anagrafica
    {
      
        private static CBancheClass m_Banche = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CBanca"/>
        /// </summary>
        public static CBancheClass Banche
        {
            get
            {
                if (m_Banche is null)
                    m_Banche = new CBancheClass();
                return m_Banche;
            }
        }
    }
}