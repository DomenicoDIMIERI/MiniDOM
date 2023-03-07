using System.Collections;
using minidom;

namespace minidom
{
    public partial class Finanziaria
    {

        public class CProfiloComparer
            : IComparer
        {
            public int Compare(object x, object y)
            {
                CProfilo a = (CProfilo)x;
                CProfilo b = (CProfilo)y;
                int ret = DMD.Strings.Compare(a.NomeCessionario, b.NomeCessionario, false);
                if (ret == 0) ret = DMD.Strings.Compare(a.Nome, b.Nome, false);
                if (ret == 0) ret = DMD.Strings.Compare(a.Profilo, b.Profilo, false);
                return ret;
            }
        }
    }
}