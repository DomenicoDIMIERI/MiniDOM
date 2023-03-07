using System;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class SogliePremioCollection : CCollection<CSogliaPremio>
        {
            [NonSerialized]
            private CSetPremi m_SetPremi;

            public SogliePremioCollection()
            {
            }

            public SogliePremioCollection(CSetPremi value)
            {
                if (value is null)
                    throw new ArgumentNullException("set premi");
                m_SetPremi = value;
                Load();
            }

            public CSetPremi SetPremi
            {
                get
                {
                    return m_SetPremi;
                }
            }

            public bool RemoveScaglione(decimal soglia)
            {
                int i, j;
                // Se l'elemento è già presente come un punto fisso allora reimposta lo scaglione
                j = -1;
                var loopTo = Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (this[i].Soglia == soglia)
                    {
                        j = i;
                        break;
                    }
                }

                if (j < 0)
                {
                    return false;
                }
                else
                {
                    this[i].Delete();
                    RemoveAt(i);
                    return true;
                }
            }

            public CSogliaPremio SetScaglione(decimal soglia, decimal fisso, double percSuML, double percSuAtt, double percSuNetto)
            {
                int i, j;
                CSogliaPremio item;
                // Se l'elemento è già presente come un punto fisso allora reimposta lo scaglione
                j = -1;
                var loopTo = Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (this[i].Soglia == soglia)
                    {
                        j = i;
                        break;
                    }
                }

                if (j < 0)
                {
                    item = new CSogliaPremio();
                    item.SetSetPremi(SetPremi);
                    Add(item);
                }
                else
                {
                    item = this[j];
                }

                item.Soglia = soglia;
                item.Fisso = fisso;
                item.PercSuML = percSuML;
                item.PercSuProvvAtt = percSuAtt;
                item.PercSuNetto = percSuNetto;
                item.Save();
                Comparer = SetPremi;
                Sort();
                return item;
            }

            protected internal void Load()
            {
                Clear();
                if (DBUtils.GetID(m_SetPremi) == 0)
                    return;
                var cursor = new CSogliePremiCursor();
                cursor.SetPremiID.Value = DBUtils.GetID(m_SetPremi);
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.Soglia.SortOrder = SortEnum.SORT_ASC;
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                cursor.Dispose();
            }
        }
    }
}