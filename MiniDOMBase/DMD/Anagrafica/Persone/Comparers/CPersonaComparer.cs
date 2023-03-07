using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Oggetto comparer tra due persone
        /// </summary>
        public class CPersonaComparer
            : IComparer, IComparer<CPersona>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonaComparer()
            {
                ////DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CPersonaComparer()
            {
                ////DMDObject.DecreaseCounter(this);
            }

            /// <summary>
            /// Compare due persone per nominativo
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public virtual int Compare(CPersona a, CPersona b)
            {
                return DMD.Strings.Compare(a.Nominativo, b.Nominativo, true);
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CPersona)x, (CPersona)y);
            }
        }
    }
}