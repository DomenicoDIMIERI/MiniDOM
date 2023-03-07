using System;
using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Comparatore di oggetti di tipo <see cref="CActivePerson"/>
        /// </summary>
        public class ActivePersonDateComparer 
            : IComparer, IComparer<CActivePerson>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public ActivePersonDateComparer()
            {
                //DMDObject.IncreaseCounter(this);
            }

            private int CompareByCategoria(CActivePerson x, CActivePerson y)
            {
                if (string.IsNullOrEmpty(x.Categoria))
                    x.Categoria = "Normale";
                if (string.IsNullOrEmpty(y.Categoria))
                    y.Categoria = "Normale";
                var items = new string[] { "Urgente", "Importante", "Normale", "Poco importante" };
                int ix = DMD.Arrays.IndexOf(items, x.Categoria);
                int iy = DMD.Arrays.IndexOf(items, y.Categoria);
                return ix - iy;
            }

            private int CompareByNominativo(CActivePerson x, CActivePerson y)
            {
                return DMD.Strings.Compare(x.Nominativo, y.Nominativo, true);
            }

            private int CompareByData(CActivePerson x, CActivePerson y)
            {
                int ret = DMD.DateUtils.Compare(DMD.DateUtils.GetDatePart(x.Data), DMD.DateUtils.GetDatePart(y.Data));
                if (ret == 0)
                {
                    DateTime d1 = (DateTime)DMD.DateUtils.GetDatePart(x.Data);
                    DateTime d2 = (DateTime)DMD.DateUtils.GetDatePart(y.Data);
                    int diff1;
                    int diff2;
                    if (x.GiornataIntera)
                    {
                        if (y.GiornataIntera)
                        {
                            // x giornata intera e y giornata intera
                            ret = CompareByCategoria(x, y);
                            if (ret == 0)
                            {
                                if (d1 < d2)
                                {
                                    ret = -1;
                                }
                                else if (d1 > d2)
                                {
                                    ret = 1;
                                }
                                else
                                {
                                    ret = 0;
                                }
                            }
                        }
                        else
                        {
                            // x giornata intera e y non giornata intera
                            diff2 = (int)DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Minute, DMD.DateUtils.Now(), y.Data.Value);
                            if (diff2 <= y.Promemoria)
                            {
                                ret = 1;
                            }
                            else if (d1 < d2)
                            {
                                ret = -1;
                            }
                            else
                            {
                                ret = 1;
                            }
                        }
                    }
                    else if (y.GiornataIntera)
                    {
                        // x non giornata intera e y giornata intera
                        diff1 = (int)DMD.DateUtils.DateDiff(DMD.DateTimeInterval.Minute, DMD.DateUtils.Now(), x.Data.Value);
                        if (diff1 <= x.Promemoria)
                        {
                            ret = -1;
                        }
                        else if (d1 < d2)
                        {
                            ret = -1;
                        }
                        else
                        {
                            ret = 1;
                        }
                    }
                    else if (x.Data < y.Data == true)
                    {
                        ret = -1;
                    }
                    else if (x.Data > y.Data == true)
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret = 0;
                    }
                }

                return ret;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(CActivePerson x, CActivePerson y)
            {
                int ret = CompareByData(x, y);
                if (ret == 0)
                    ret = CompareByCategoria(x, y);
                if (ret == 0)
                    ret = CompareByNominativo(x, y);
                return ret;
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CActivePerson)x, (CActivePerson)y);
            }

            //~ActivePersonDateComparer()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }
}