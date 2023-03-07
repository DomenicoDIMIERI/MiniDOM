using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CPraticheCollection : CCollection<CPraticaCQSPD>
        {
            public CPraticheCollection()
            {
            }

            public CPraticheCollection(CTeamManager tm) : this()
            {
                if (tm is null)
                    throw new ArgumentNullException("tm");
                if (DBUtils.GetID(tm) != 0)
                {
                    var cursor = new CPraticheCQSPDCursor();
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCommerciale.Value = DBUtils.GetID(tm);
                    cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }

            public int ContaValide()
            {
                int cnt = 0;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var Item = this[i];
                    if (Item.StatoAttuale.MacroStato.HasValue)
                    {
                        switch (Item.StatoAttuale.MacroStato)


                        {
                            case StatoPraticaEnum.STATO_ANNULLATA:
                            case StatoPraticaEnum.STATO_RICHIESTADELIBERA:
                            case StatoPraticaEnum.STATO_DELIBERATA:
                            case StatoPraticaEnum.STATO_LIQUIDATA:
                                {
                                    cnt += 1;
                                    break;
                                }
                        }
                    }
                }

                return cnt;
            }

            public int ContaInLavorazione()
            {
                int cnt = 0;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var Item = this[i];
                    if (Item.StatoAttuale.MacroStato.HasValue)
                    {
                        switch (Item.StatoAttuale.MacroStato)
                        {
                            case StatoPraticaEnum.STATO_RICHIESTADELIBERA:
                            case StatoPraticaEnum.STATO_DELIBERATA:
                                {
                                    cnt += 1;
                                    break;
                                }
                        }
                    }
                }

                return cnt;
            }

            public int ContaNonPagate()
            {
                return -1;
            }

            /// <summary>
        /// Crea una nuova pratica
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CPraticaCQSPD Add()
            {
                var item = new CPraticaCQSPD();
                Add(item);
                return item;
            }
        }
    }
}