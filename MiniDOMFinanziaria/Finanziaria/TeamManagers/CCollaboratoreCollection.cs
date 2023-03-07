using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Collezione di collaboratori per un TeamManager
        /// </summary>
        [Serializable]
        public class CCollaboratoreCollection 
            : CCollection<CCollaboratore>
        {
            // Implements IComparer

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCollaboratoreCollection()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tm"></param>
            public CCollaboratoreCollection(CTeamManager tm) : this()
            {
                if (tm is null)
                    throw new ArgumentNullException("tm");
                if (DBUtils.GetID(tm) == 0) return;

                using (var cursor = new CCollaboratoriCursor())
                { 
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.ReferenteID.Value = DBUtils.GetID(tm);
                    cursor.NomePersona.SortOrder = SortEnum.SORT_ASC;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }
                }
            }

            /// <summary>
            /// Restituisce il numero di collaboratori attivi
            /// </summary>
            /// <returns></returns>
            public int ContaValidi()
            {
                int cnt = 0;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var item = this[i];
                    if (item.IsValid())
                        cnt += 1;
                }

                return cnt;
            }

            /// <summary>
            /// Compara due collaboratori
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            protected override int Compare(object a, object b)
            {
                string n1, n2;
                n1 = Strings.LCase(((CCollaboratore)a).NomePersona);
                n2 = Strings.LCase(((CCollaboratore)b).NomePersona);
                return DMD.Strings.Compare(n1, n2, true);
            }
        }
    }
}