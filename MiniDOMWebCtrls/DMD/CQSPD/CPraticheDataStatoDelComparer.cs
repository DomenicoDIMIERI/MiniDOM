using System.Collections;

namespace minidom.Forms
{
    public class CPraticheDataStatoDelComparer : IComparer
    {
        public CPraticheDataStatoDelComparer()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~CPraticheDataStatoDelComparer()
        {
            DMDObject.DecreaseCounter(this);
        }

        public int Compare(Finanziaria.CPraticaCQSPD a, Finanziaria.CPraticaCQSPD b)
        {
            if (a.StatoDeliberata.Data < b.StatoDeliberata.Data == true)
                return -1;
            if (a.StatoDeliberata.Data > b.StatoDeliberata.Data == true)
                return 1;
            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((Finanziaria.CPraticaCQSPD)x, (Finanziaria.CPraticaCQSPD)y);
        }
    }
}