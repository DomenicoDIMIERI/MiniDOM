using System;
using System.Collections;

namespace minidom.Forms
{





    // --------------------------------
    public class CPraticheDataStatoContattoComparer : IComparer
    {
        public CPraticheDataStatoContattoComparer()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~CPraticheDataStatoContattoComparer()
        {
            DMDObject.DecreaseCounter(this);
        }

        public int Compare(Finanziaria.CPraticaCQSPD a, Finanziaria.CPraticaCQSPD b)
        {
            DateTime? d1 = default;
            DateTime? d2 = default;
            if (a.StatoPreventivo is object)
                d1 = a.StatoPreventivo.Data;
            if (b.StatoPreventivo is object)
                d2 = b.StatoPreventivo.Data;
            return DMD.DateUtils.Compare(d1, d2);
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((Finanziaria.CPraticaCQSPD)x, (Finanziaria.CPraticaCQSPD)y);
        }
    }
}