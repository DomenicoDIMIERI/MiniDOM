using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Compara due oggetti di tipo <see cref="CContattoUtente"/>
        /// </summary>
        public class CContattoComparer 
            : IComparer, IComparer<CContattoUtente>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoComparer()
            {
                
            }
                      
            /// <summary>
            /// Compare i due oggetti
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public int Compare(CContattoUtente a, CContattoUtente b)
            {
                return a.CompareTo(b);
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CContattoUtente)x, (CContattoUtente)y);
            }
        }
    }
}