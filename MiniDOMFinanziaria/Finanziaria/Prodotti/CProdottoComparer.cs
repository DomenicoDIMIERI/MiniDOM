using System.Collections;
using minidom;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CProdottoComparer 
            : IComparer
        {
            public CProdottoComparer()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CProdottoComparer()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int Compare(object x, object y)
            {
                CCQSPDProdotto a = (CCQSPDProdotto)x;
                CCQSPDProdotto b = (CCQSPDProdotto)y;
                int ret = DMD.Strings.Compare(a.Descrizione, b.Descrizione, false);
                return ret;
            }
        }
    }
}