using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CStatsItem : IComparable
        {
            public Anagrafica.CFonte Fonte;
            public int Visualizzazioni;
            public DateTime? PrimaVisualizzazione;
            public DateTime? UltimaVisualizzazione;
            public int VisualizzazioniPerGiorno;
            public int RichiesteGenerate;
            public int PraticheGenerate;
            public int PratichePerfezionate;

            public CStatsItem()
            {
            }

            public CStatsItem(Anagrafica.CFonte fonte)
            {
                Fonte = fonte;
            }

            private int CompareTo(CStatsItem obj)
            {
                int ret = obj.PraticheGenerate - PraticheGenerate;
                if (ret == 0)
                    ret = obj.RichiesteGenerate - RichiesteGenerate;
                if (ret == 0)
                    ret = obj.Visualizzazioni - Visualizzazioni;
                if (ret == 0)
                    ret = DMD.Strings.Compare(Fonte.Nome, obj.Fonte.Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CStatsItem)obj);
            }
        }
    }
}