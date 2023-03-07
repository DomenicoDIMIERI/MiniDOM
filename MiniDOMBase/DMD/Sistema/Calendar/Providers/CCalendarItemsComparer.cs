using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Comparer di oggetti <see cref="ICalendarActivity"/>
        /// </summary>
        public class CCalendarItemsComparer 
            : IComparer, IComparer<ICalendarActivity>
        {

            /// <summary>
            /// Istanza predefinita
            /// </summary>
            public static readonly CCalendarItemsComparer Instance = new CCalendarItemsComparer();

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCalendarItemsComparer()
            {
                //DMDObject.IncreaseCounter(this);
            }

            //~CCalendarItemsComparer()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public int Compare(ICalendarActivity a, ICalendarActivity b)
            {
                if (a.DataInizio < b.DataInizio)
                    return -1;
                if (a.DataInizio > b.DataInizio)
                    return 1;
                return 0;
            }

            int IComparer.Compare(object x, object y)
            {
                return this.Compare((ICalendarActivity)x, (ICalendarActivity)y);                 
            }
        }
    }
}