using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Compare di oggetti <see cref="CUser"/>
        /// </summary>
        public class CUsersComparer 
            : IComparer, IComparer<CUser>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUsersComparer()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Compare due oggetti
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public int Compare(CUser a, CUser b)
            {
                return DMD.Strings.Compare(a.Nominativo, b.Nominativo, true);
            }

            int IComparer.Compare(object x, object y)
            {
                return this.Compare((CUser)x, (CUser)y);
            }
        }
    }
}