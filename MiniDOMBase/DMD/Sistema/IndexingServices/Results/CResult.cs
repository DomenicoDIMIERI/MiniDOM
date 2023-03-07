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
using minidom.internals;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Risultato del servizio di indicizzazione
        /// </summary>
        [Serializable]
        public class CResult
             : IComparable, IComparable<CResult>
        {
            public int OwnerID;
            public int Count;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CResult()
            {
                this.OwnerID = 0;
                this.Count = 0;
            }


            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="ownerID"></param>
            public CResult(int ownerID)
            {
                OwnerID = ownerID;
                Count = 1;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CResult obj)
            {
                return obj.Count - Count;
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CResult)obj);
            }
        }


    }
}