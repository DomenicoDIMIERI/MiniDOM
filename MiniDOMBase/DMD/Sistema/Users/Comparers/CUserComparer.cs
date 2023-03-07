using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Oggetto IComparer
        /// </summary>
        public class CUserComparer 
            : IComparer, IComparer<CUser>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserComparer()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="user1"></param>
            /// <param name="user2"></param>
            /// <returns></returns>
            public int Compare(CUser user1, CUser user2)
            {
                return user1.CompareTo(user2);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            int IComparer.Compare(object a, object b)
            {
                return Compare((CUser)a, (CUser)b);
            }
        }
    }
}