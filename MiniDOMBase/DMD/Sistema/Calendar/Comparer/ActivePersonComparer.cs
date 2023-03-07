using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Comparer di oggetti di tipo <see cref="CActivePerson"/>
        /// </summary>
        public class ActivePersonComparer 
            : IComparer, IComparer<CActivePerson>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public ActivePersonComparer()
            {
                //DMDObject.IncreaseCounter(this);
            }

            //~ActivePersonComparer()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}


            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(CActivePerson x, CActivePerson y)
            {
                int ret = 0;
                if (x.PersonID != y.PersonID)
                {
                    // Dim la As String = ""
                    // Dim lb As String = ""
                    // If (x.Ricontatto IsNot Nothing) Then la = x.Ricontatto.NomeLista
                    // If (y.Ricontatto IsNot Nothing) Then lb = y.Ricontatto.NomeLista
                    // ret = DMD.Strings.Compare(la, lb)
                    if (ret == 0)
                        ret = DMD.Strings.Compare(x.Nominativo, y.Nominativo, true);
                    return ret;
                }

                ret = -DMD.DateUtils.Compare(x.Data, y.Data);
                if (ret == 0)
                    ret = DMD.Strings.Compare(x.Nominativo, y.Nominativo, true);
                return ret;
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CActivePerson)x, (CActivePerson)y);
            }
        }
    }
}