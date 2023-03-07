using System.Collections;

namespace minidom.Forms
{
    public class CPraticheDataStatoRichDelComparer : IComparer
    {
        public CPraticheDataStatoRichDelComparer()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~CPraticheDataStatoRichDelComparer()
        {
            DMDObject.DecreaseCounter(this);
        }

        public int Compare(Finanziaria.CPraticaCQSPD a, Finanziaria.CPraticaCQSPD b)
        {
            if (a.StatoRichiestaDelibera.Data < b.StatoRichiestaDelibera.Data == true)
                return -1;
            if (a.StatoRichiestaDelibera.Data > b.StatoRichiestaDelibera.Data == true)
                return 1;
            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((Finanziaria.CPraticaCQSPD)x, (Finanziaria.CPraticaCQSPD)y);
        }
    }
}