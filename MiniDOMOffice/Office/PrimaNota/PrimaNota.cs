using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="RigaPrimaNota"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CPrimaNotaClass 
            : CModulesClass<RigaPrimaNota>
        {

            /// <summary>
            /// Evento generato quando la cassa del punto operativo scende sotto una soglia specifica
            /// </summary>
            public event SottoSogliaEventHandler SottoSoglia;

         
            /// <summary>
            /// Costruttore
            /// </summary>
            public CPrimaNotaClass() 
                : base("modOfficePrimaNota", typeof(RigaPrimaNotaCursor))
            {
            }

            /// <summary>
            /// Verifica per ogni punto operativo che la giacenza in cassa sia non inferiore al limite.
            /// In caso la giacenza sia inferiore viene generato l'evento sottosoglia
            /// </summary>
            /// <remarks></remarks>
            public void CheckSottoSoglia()
            {
                foreach(var u in Anagrafica.Aziende.AziendaPrincipale.Uffici )
                {
                    if (u.Attivo && u.Stato == ObjectStatus.OBJECT_VALID )
                        CheckSottoSoglia(u);
                }
                //using (var cursor = new Anagrafica.CUfficiCursor())
                //{
                //    cursor.IgnoreRights = true;
                //    cursor.IDAzienda.Value = DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale, 0);
                //    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                //    cursor.OnlyValid = true;
                //    while (cursor.Read())
                //    {
                //        CheckSottoSoglia(cursor.Item);
                
                //    }
                //}
            }

            /// <summary>
            /// Verifica per l'ufficio specificato la giacenza in cassa sia non inferiore al limite.
            /// In caso la giacenza sia inferiore viene generato l'evento sottosoglia
            /// </summary>
            /// <remarks></remarks>
            public void CheckSottoSoglia(CUfficio ufficio)
            {
                double limite = minidom.Office.PrimaNota.Module.Settings.GetValueDouble("Limite", 30.0d);
                decimal giacenzaCassa = (decimal)minidom.Office.PrimaNota.GetGiacenzaCassa(ufficio);
                if ((double)giacenzaCassa <= limite)
                    OnSottoSoglia(new PrimaNotaSottoSogliaEventArgs(ufficio, (decimal)limite));
            }

            /// <summary>
            /// Genera l'evento SottoSoglia
            /// </summary>
            /// <param name="e"></param>
            internal void OnSottoSoglia(minidom.Office.PrimaNotaSottoSogliaEventArgs e)
            {
                SottoSoglia?.Invoke(this, e);
                this.DispatchEvent(new EventDescription("sottosoglia", "Giacenza in cassa inferiore a " + Sistema.Formats.FormatValuta(e.Soglia) + " per l'ufficio di " + e.Ufficio.Nome, e));
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficePrimaNota");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficePrimaNota");
            //        ret.Description = "Prima Nota";
            //        ret.DisplayName = "Prima Nota";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!minidom.Office.Database.Tables.ContainsKey("tbl_OfficePrimaNota"))
            //    {
            //        var table = minidom.Office.Database.Tables.Add("tbl_OfficePrimaNota");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("Data", TypeCode.DateTime);
            //        field = table.Fields.Add("DescrizioneMovimento", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Entrate", TypeCode.Decimal);
            //        field = table.Fields.Add("Uscite", TypeCode.Decimal);
            //        field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32);
            //        field = table.Fields.Add("NomePuntoOperativo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("CreatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("CreatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("ModificatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("ModificatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("Stato", TypeCode.Int32);
            //        table.Create();
            //    }

            //    return ret;
            //}

            /// <summary>
            /// Restituisce la giacenza per il punto operativo
            /// </summary>
            /// <param name="ufficio"></param>
            /// <returns></returns>
            public double GetGiacenzaCassa(Anagrafica.CUfficio ufficio)
            {
                var info = GetInfoPrimaNota(ufficio, default, default);
                decimal sumEI = info["sumEI"];
                decimal sumEF = info["sumEF"];
                decimal sumUI = info["sumUI"];
                decimal sumUF = info["sumUF"];
                // text += ", Riporto: " + Formats.FormatValuta(sumEI - sumUI) + ", Giacenza in cassa: <b>" + Formats.FormatValuta(sumEF - sumUF) + "</b>";
                return (double)(sumEF - sumUF);
            }

            /// <summary>
            /// Restituisce la giacenza per il punto operativo alla data
            /// </summary>
            /// <param name="ufficio"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public double GetGiacenzaCassa(CUfficio ufficio, DateTime toDate)
            {
                var info = GetInfoPrimaNota(ufficio, default, toDate);
                decimal sumEI = info["sumEI"];
                decimal sumEF = info["sumEF"];
                decimal sumUI = info["sumUI"];
                decimal sumUF = info["sumUF"];
                // text += ", Riporto: " + Formats.FormatValuta(sumEI - sumUI) + ", Giacenza in cassa: <b>" + Formats.FormatValuta(sumEF - sumUF) + "</b>";
                return (double)(sumEF - sumUF);
            }

            /// <summary>
            /// Restituisce 
            /// </summary>
            /// <param name="ufficio"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public CKeyCollection<decimal> GetInfoPrimaNota(
                                        Anagrafica.CUfficio ufficio, 
                                        DateTime? fromDate, 
                                        DateTime? toDate
                                        )
            {
                int po = DBUtils.GetID(ufficio, 0);
                int sumEI = 0;
                int sumUI = 0;
                decimal sumEF = 0m;
                decimal sumUF = 0m;
                
                DBCursorFieldBase wherePart =  DBCursorField.Field("Stato").EQ (ObjectStatus.OBJECT_VALID);
                if (!Module.UserCanDoAction("list"))
                {
                    var lst = new List<int>();
                    if (Module.UserCanDoAction("list_office"))
                    {
                        foreach (var u in Sistema.Users.CurrentUser.Uffici)
                        {
                            lst.Add(DBUtils.GetID(u, 0));
                        }
                    }

                    DBCursorFieldBase tmp = DBCursorFieldBase.False;
                    if (lst.Count > 0)
                    {
                        lst.Add(0);
                        tmp += DBCursorField.Field("IDPuntoOperativo").In(lst.ToArray());
                    }

                    if (Module.UserCanDoAction("list_own"))
                    {
                        tmp += DBCursorField.Field("CreatoDa").EQ(DBUtils.GetID(Sistema.Users.CurrentUser, 0));
                    }

                    wherePart *= tmp;
                }

                if (po != 0)
                {
                    wherePart *= DBCursorField.Field("IDPuntoOperativo").EQ(po);
                }

                if (fromDate.HasValue)
                {
                    using (var dbRis = this.Database.ExecuteReader(
                                        DBUtils.SELECT(
                                                    DBProjection.Field("Entrate").Sum().As("sumEI"),
                                                    DBProjection.Field("Uscite").Sum().As("sumUI")
                                                    )
                                                    .FROM("tbl_OfficePrimaNota")
                                                    .WHERE(wherePart * DBCursorField.Field("Data").LT(fromDate))
                                                ))
                    {
                        if (dbRis.Read())
                        {
                            sumEI = (int)Sistema.Formats.ToValuta(dbRis["sumEI"]);
                            sumUI = (int)Sistema.Formats.ToValuta(dbRis["sumUI"]);
                        }
                    }
                }

                if (toDate.HasValue)
                {
                    if (fromDate.HasValue)
                        toDate = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 24 * 3600 - 1, DMD.DateUtils.GetDatePart(toDate));

                    using (var dbRis = this.Database.ExecuteReader(
                                        DBUtils.SELECT(
                                                    DBProjection.Field("Entrate").Sum().As("sumEF"),
                                                    DBProjection.Field("Uscite").Sum().As("sumUF")
                                                    )
                                                    .FROM("tbl_OfficePrimaNota")
                                                    .WHERE(wherePart * DBCursorField.Field("Data").LE(toDate))
                                                ))
                    {
                        if (dbRis.Read())
                        {
                            sumEI = (int)Sistema.Formats.ToValuta(dbRis["sumEF"]);
                            sumUI = (int)Sistema.Formats.ToValuta(dbRis["sumUF"]);
                        }
                    }
                      
                }
                else
                {
                    using (var dbRis = this.Database.ExecuteReader(
                                        DBUtils.SELECT(
                                                    DBProjection.Field("Entrate").Sum().As("sumEF"),
                                                    DBProjection.Field("Uscite").Sum().As("sumUF")
                                                    )
                                                    .FROM("tbl_OfficePrimaNota")
                                                    .WHERE(wherePart)
                                                ))
                    {
                        if (dbRis.Read())
                        {
                            sumEI = (int)Sistema.Formats.ToValuta(dbRis["sumEF"]);
                            sumUI = (int)Sistema.Formats.ToValuta(dbRis["sumUF"]);
                        }
                    }
                }


                var ret = new CKeyCollection<decimal>();
                ret.Add("sumEI", sumEI);
                ret.Add("sumUI", sumUI);
                ret.Add("sumEF", sumEF);
                ret.Add("sumUF", sumUF);
                return ret;
            }
        }
    }

    public partial class Office
    {
        private static CPrimaNotaClass m_PrimaNota = null;

        /// <summary>
        /// Repository di <see cref="RigaPrimaNota"/>
        /// </summary>
        /// <remarks></remarks>
        public static CPrimaNotaClass PrimaNota
        {
            get
            {
                if (m_PrimaNota is null)
                    m_PrimaNota = new CPrimaNotaClass();
                return m_PrimaNota;
            }
        }
    }
}