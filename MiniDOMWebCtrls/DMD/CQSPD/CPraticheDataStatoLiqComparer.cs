using System.Collections;

namespace minidom.Forms
{
    public class CPraticheDataStatoLiqComparer : IComparer
    {
        public CPraticheDataStatoLiqComparer()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~CPraticheDataStatoLiqComparer()
        {
            DMDObject.DecreaseCounter(this);
        }

        public int Compare(Finanziaria.CPraticaCQSPD a, Finanziaria.CPraticaCQSPD b)
        {
            if (a.StatoLiquidata.Data < b.StatoLiquidata.Data == true)
                return -1;
            if (a.StatoLiquidata.Data > b.StatoLiquidata.Data == true)
                return 1;
            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((Finanziaria.CPraticaCQSPD)x, (Finanziaria.CPraticaCQSPD)y);
        }
    }
}