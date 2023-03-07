using System;
using System.Collections;
using System.Data;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public class LiquidatoHandler : CQSPDBaseStatsHandler
    {
        private Finanziaria.CStatoPratica statoContatto;
        private Finanziaria.CStatoPratica statoRichiestaDelibera;
        private Finanziaria.CStatoPratica statoLiquidata;
        private Finanziaria.CStatoPratica statoArchiviata;

        public LiquidatoHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return null;
        }

        private int[] GetArray(string ufficiStr)
        {
            var ret = new ArrayList();
            var tmp = Strings.Split(ufficiStr, ",");
            for (int i = 0, loopTo = DMD.Arrays.Len(tmp) - 1; i <= loopTo; i++)
            {
                int id = Sistema.Formats.ToInteger(tmp[i]);
                if (id != 0)
                    ret.Add(id);
            }

            return (int[])ret.ToArray(typeof(int));
        }

        private void AggiungiEstinzioniRinnovabili(string strAnni, string strPO, CKeyCollection<StatLiquidatoItem> items)
        {
            IDataReader dbRis = null;
            try
            {
                string dbSQL = "";
                StatLiquidatoItem item = null;

                // Rinnovabile
                dbSQL += "SELECT Count(*) AS [Cnt], Sum([Rata]*[Durata]) AS [Valore], [IDPuntoOperativo], Year([DataRinnovo]) AS [Anno], Month([DataRinnovo]) AS [Mese] ";
                dbSQL += "FROM [tbl_Estinzioni] ";
                dbSQL += "WHERE (([Stato]=1) AND (([Tipo]) In (1,2)) AND Not ([Rata] Is Null) AND Not ([Durata] Is Null) AND Not ([DataRinnovo]) Is Null) AND [Estingue]=FALSE AND [DataRinnovo]<=" + Databases.DBUtils.DBDate(DMD.DateUtils.GetNextMonthFirstDay(DMD.DateUtils.ToDay())) + " ";
                if (!string.IsNullOrEmpty(strAnni))
                    dbSQL += " AND Year([DataRinnovo]) In (" + strAnni + ") ";
                if (!string.IsNullOrEmpty(strPO))
                    dbSQL += " AND [IDPuntoOperativo] In (" + strPO + ") ";
                dbSQL += "GROUP BY [IDPuntoOperativo], Year([DataRinnovo]), Month([DataRinnovo])";
                dbRis = Finanziaria.Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new StatLiquidatoItem();
                        item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                        item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                        items.Add("K" + item.Anno + "_" + item.Mese, item);
                    }

                    item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                    item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                    // item.RinnovabiliUpfront = Formats.ToValuta(dbRis("Upfront"))
                    // item.RinnovabiliRunning = Formats.ToValuta(dbRis("Running"))
                    item.RinnovabiliCnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                    item.RinnovabiliSum = Sistema.Formats.ToValuta(dbRis["Valore"]);
                    // item.RinnovabiliSconto = Formats.ToValuta(dbRis("Sconto"))
                }
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
                throw;
            }
            finally
            {
                if (dbRis is object)
                {
                    dbRis.Dispose();
                    dbRis = null;
                }
            }
        }

        private string GetStr(int[] arr)
        {
            if (arr is null || arr.Length <= 0)
                return DMD.Strings.vbNullString;
            var ret = new System.Text.StringBuilder();
            for (int i = 0, loopTo = arr.Length - 1; i <= loopTo; i++)
            {
                if (i > 0)
                    ret.Append(",");
                ret.Append(arr[i]);
            }

            return ret.ToString();
        }

        private void AggiungiLiquidato(string strAnni, string strPO, Finanziaria.CPraticheCQSPDCursor cursor, CKeyCollection<StatLiquidatoItem> items)
        {
            IDataReader dbRis = null;
            try
            {
                string dbSQL = "";
                StatLiquidatoItem item = null;

                // dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                dbSQL += "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T2].[Data]) As [Mese], " + "Year([T2].[Data]) As [Anno] " + " FROM (";
                dbSQL += cursor.GetSQL() + ") AS [T1] ";
                dbSQL += " INNER JOIN (";
                dbSQL += "SELECT * FROM [tbl_PraticheSTL]  WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") AND Year([tbl_PraticheSTL].[Data]) In (" + strAnni + ") AND [tbl_PraticheSTL].[IDToStato] In (" + Databases.GetID(statoLiquidata) + ")";
                dbSQL += ") AS T2 ";
                dbSQL += "ON [T1].[ID]=[T2].[IDPratica] ";
                dbSQL += "GROUP BY Year([T2].[Data]), Month([T2].[Data]) ";
                dbRis = Finanziaria.Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new StatLiquidatoItem();
                        item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                        item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                        items.Add("K" + item.Anno + "_" + item.Mese, item);
                    }

                    item.LiquidatoUpfront = Sistema.Formats.ToValuta(dbRis["Upfront"]);
                    item.LiquidatoRunning = Sistema.Formats.ToValuta(dbRis["Running"]);
                    item.LiquidatoCnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                    item.LiquidatoSum = Sistema.Formats.ToValuta(dbRis["Valore"]);
                    item.LiquidatoSconto = Sistema.Formats.ToValuta(dbRis["Sconto"]);
                }
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
                throw;
            }
            finally
            {
                if (dbRis is object)
                {
                    dbRis.Dispose();
                    dbRis = null;
                }
            }
        }

        private void AggiungiDataValuta(string strAnni, string strPO, Finanziaria.CPraticheCQSPDCursor cursor, CKeyCollection<StatLiquidatoItem> items)
        {
            string dbSQL = "";
            StatLiquidatoItem item = null;

            // dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
            dbSQL += "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T1].[DataValuta]) As [Mese], " + "Year([T1].[DataValuta]) As [Anno] " + " FROM (";
            dbSQL += cursor.GetSQL() + ") AS [T1] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") AND Year([T1].[DataValuta]) In (" + strAnni + ") AND [T1].[IDStatoAttuale] In (" + Databases.GetID(statoLiquidata) + ", " + Databases.GetID(statoArchiviata) + ") ";
            dbSQL += "GROUP BY Year([T1].[DataValuta]), Month([T1].[DataValuta]) ";

            using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL))
            {
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new StatLiquidatoItem();
                        item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                        item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                        items.Add("K" + item.Anno + "_" + item.Mese, item);
                    }

                    item.LiquidatoUpfront = Sistema.Formats.ToValuta(dbRis["Upfront"]);
                    item.LiquidatoRunning = Sistema.Formats.ToValuta(dbRis["Running"]);
                    item.LiquidatoCnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                    item.LiquidatoSum = Sistema.Formats.ToValuta(dbRis["Valore"]);
                    item.LiquidatoSconto = Sistema.Formats.ToValuta(dbRis["Sconto"]);
                }
            }
        }

        private void AggiungiCaricato(string strAnni, string strPO, Finanziaria.CPraticheCQSPDCursor cursor, CKeyCollection<StatLiquidatoItem> items)
        {
            string dbSQL = "";
            StatLiquidatoItem item = null;

            // dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
            dbSQL = "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T2].[Data]) As [Mese], " + "Year([T2].[Data]) As [Anno] " + " FROM (";
            dbSQL += cursor.GetSQL() + ") AS [T1] ";
            dbSQL += " INNER JOIN (";
            dbSQL += "SELECT * FROM [tbl_PraticheSTL]  WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") AND Year([tbl_PraticheSTL].[Data]) In (" + strAnni + ") AND [tbl_PraticheSTL].[IDToStato] In (" + Databases.GetID(statoContatto) + ")";
            dbSQL += ") AS T2 ";
            dbSQL += "ON [T1].[ID]=[T2].[IDPratica] ";
            dbSQL += "GROUP BY Year([T2].[Data]), Month([T2].[Data]) ";

            using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL))
            {
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new StatLiquidatoItem();
                        item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                        item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                        items.Add("K" + item.Anno + "_" + item.Mese, item);
                    }

                    item.CaricatoUpfront = Sistema.Formats.ToValuta(dbRis["Upfront"]);
                    item.CaricatoRunning = Sistema.Formats.ToValuta(dbRis["Running"]);
                    item.CaricatoCnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                    item.CaricatoSum = Sistema.Formats.ToValuta(dbRis["Valore"]);
                    item.CaricatoSconto = Sistema.Formats.ToValuta(dbRis["Sconto"]);
                }
            }
        }

        private void AggiungiRichiestaDelibera(string strAnni, string strPO, Finanziaria.CPraticheCQSPDCursor cursor, CKeyCollection<StatLiquidatoItem> items)
        {
            string dbSQL = "";
            StatLiquidatoItem item = null;

            // dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
            dbSQL = "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T2].[Data]) As [Mese], " + "Year([T2].[Data]) As [Anno] " + " FROM (";
            dbSQL += cursor.GetSQL() + ") AS [T1] ";
            dbSQL += " INNER JOIN (";
            dbSQL += "SELECT * FROM [tbl_PraticheSTL]  WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") AND Year([tbl_PraticheSTL].[Data]) In (" + strAnni + ") AND [tbl_PraticheSTL].[IDToStato] In (" + Databases.GetID(statoRichiestaDelibera) + ")";
            dbSQL += ") AS [T2] ";
            dbSQL += "ON [T1].[ID]=[T2].[IDPratica] ";
            dbSQL += "GROUP BY Year([T2].[Data]), Month([T2].[Data]) ";

            using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL))
            {
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new StatLiquidatoItem();
                        item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                        item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                        items.Add("K" + item.Anno + "_" + item.Mese, item);
                    }

                    item.RichiestaDeliberaUpfront = Sistema.Formats.ToValuta(dbRis["Upfront"]);
                    item.RichiestaDeliberaRunning = Sistema.Formats.ToValuta(dbRis["Running"]);
                    item.RichiestaDeliberaCnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                    item.RichiestaDeliberaSum = Sistema.Formats.ToValuta(dbRis["Valore"]);
                    item.RichiestaDeliberaSconto = Sistema.Formats.ToValuta(dbRis["Sconto"]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public string GetStats(object renderer)
        {
            int[] arrPO = null;
            int[] arrAnni = null;
            
            var items = new CKeyCollection<StatLiquidatoItem>();
            string tipoG = this.n2str(renderer, "t", "");
            arrPO = GetArray(this.n2str(renderer, "po", ""));
            arrAnni = GetArray(this.n2str(renderer, "an", ""));
            if (DMD.Arrays.Len(arrPO) <= 0 || DMD.Arrays.Len(arrAnni) <= 0)
                return "";
            statoContatto = Finanziaria.StatiPratica.StatoPraticaCaricata;
            statoRichiestaDelibera = Finanziaria.StatiPratica.StatoRichiestaDelibera;
            statoLiquidata = Finanziaria.StatiPratica.StatoLiquidato;
            statoArchiviata = Finanziaria.StatiPratica.StatoArchiviato;
            string strAnni = GetStr(arrAnni);
            string strPO = GetStr(arrPO);

            // Rinnovabile
            AggiungiEstinzioniRinnovabili(strAnni, strPO, items);


            // Liquidato
            using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
            {
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.Trasferita.Value = false;
                cursor.Flags.Value = Finanziaria.PraticaFlags.HIDDEN;
                cursor.Flags.Operator = OP.OP_NE;
                // cursor.StatoContatto.Inizio = Calendar.MakeDate(anno, 1, 1)
                // cursor.StatoContatto.IDToStato = GetID(statoContatto)
                // cursor.StatoContatto.MacroStato = Nothing
                cursor.IDPuntoOperativo.ValueIn(arrPO);
                // cursor.IDStatoAttuale.ValueIn({GetID(Finanziaria.StatiPratica.StatoLiquidato), GetID(Finanziaria.StatiPratica.StatoArchiviato)})

                switch (tipoG ?? "")
                {
                    case "DataValuta":
                        {
                            AggiungiDataValuta(strAnni, strPO, cursor, items);
                            break;
                        }

                    default:
                        {
                            AggiungiLiquidato(strAnni, strPO, cursor, items);
                            break;
                        }
                }


                // Caricato
                AggiungiCaricato(strAnni, strPO, cursor, items);


                // Richiesta Delibera
                AggiungiRichiestaDelibera(strAnni, strPO, cursor, items);
            }

            items.Sort();
            var ret = new CKeyCollection();
            ret.Add("items", items);
            ret.Add("obiettivi", GetObiettivi(arrPO, Maths.Min(arrAnni), Maths.Max(arrAnni)));
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        private int GetAnnoInizio()
        {
            // Year(Now) - 1
            if (Finanziaria.Pratiche.Module.UserCanDoAction("seestats"))
            {
                DateTime? ret = (DateTime?)Finanziaria.Database.ExecuteScalar("SELECT Min([Data]) FROM [tbl_PraticheSTL] WHERE [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString());
                if (ret.HasValue)
                    return DMD.DateUtils.Year(ret.Value);
                return DMD.DateUtils.Year(DMD.DateUtils.Now());
            }
            else
            {
                return DMD.DateUtils.Year(DMD.DateUtils.Now());
            }
        }

        private double GetPercentage(double val, double sum)
        {
            if (sum == 0d)
                return default;
            try
            {
                return val * 100d / sum;
            }
            catch (Exception ex)
            {
                return 0d;
            }
        }

        private Finanziaria.CObiettivoPratica GetObiettivo(int[] arrPO, int anno, int mese)
        {
            var ret = new Finanziaria.CObiettivoPratica();
            for (int i = 0, loopTo = DMD.Arrays.UBound(arrPO); i <= loopTo; i++)
            {
                var po = Anagrafica.Uffici.GetItemById(arrPO[i]);
                var items = Finanziaria.Obiettivi.GetObiettiviAl(po, DMD.DateUtils.MakeDate(anno, mese, 1));
                if (items.Count > 0)
                {
                    var tmp = items[items.Count - 1]; // Restituiamo l'ultimo obiettivo
                    ret.MontanteLordoLiq = (decimal?)Maths.SumNulls((double?)ret.MontanteLordoLiq, (double?)tmp.MontanteLordoLiq);
                    ret.NumeroPraticheLiq = (int?)Maths.SumNulls(ret.NumeroPraticheLiq, tmp.NumeroPraticheLiq);
                    ret.ValoreSpreadLiq = (decimal?)Maths.SumNulls((double?)ret.ValoreSpreadLiq, (double?)tmp.ValoreSpreadLiq);
                    ret.SpreadLiq = (float?)Maths.AveNulls(ret.SpreadLiq, tmp.SpreadLiq);
                    ret.ValoreUpFront = (decimal?)Maths.SumNulls((double?)ret.ValoreUpFront, (double?)tmp.ValoreUpFront);
                    ret.UpFrontLiq = (float?)Maths.AveNulls(ret.UpFrontLiq, tmp.UpFrontLiq);
                }
                else
                {
                }
            }

            return ret;
        }

        private CCollection<Finanziaria.CObiettivoPratica> GetObiettivi(int[] arrPO, int annoInizio, int annoFine)
        {
            var ret = new CCollection<Finanziaria.CObiettivoPratica>();
            using (var cursor = new Finanziaria.CObiettivoPraticaCursor())
            {
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.IDPuntoOperativo.ValueIn(arrPO);
                cursor.DataInizio.Between(DMD.DateUtils.MakeDate(annoInizio, 1, 1), DMD.DateUtils.MakeDate(annoFine, 12, 31, 23, 59, 59));
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    ret.Add(cursor.Item);
                    cursor.MoveNext();
                }

                return ret;
            }
        }
    }

    public class StatLiquidatoItem : IComparable, IDMDXMLSerializable
    {
        public int Anno;
        public int Mese;
        public int RinnovabiliCnt;
        public decimal RinnovabiliSum;
        public decimal RinnovabiliUpfront;
        public decimal RinnovabiliRunning;
        public decimal RinnovabiliSconto;
        public int CaricatoCnt;
        public decimal CaricatoSum;
        public decimal CaricatoUpfront;
        public decimal CaricatoRunning;
        public decimal CaricatoSconto;
        public int RichiestaDeliberaCnt;
        public decimal RichiestaDeliberaSum;
        public decimal RichiestaDeliberaUpfront;
        public decimal RichiestaDeliberaRunning;
        public decimal RichiestaDeliberaSconto;
        public int LiquidatoCnt;
        public decimal LiquidatoSum;
        public decimal LiquidatoUpfront;
        public decimal LiquidatoRunning;
        public decimal LiquidatoSconto;

        public int CompareTo(object obj)
        {
            StatLiquidatoItem o = (StatLiquidatoItem)obj;
            int ret = -Anno + o.Anno;
            if (ret == 0)
                ret = -Mese + o.Mese;
            return ret;
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Anno":
                    {
                        Anno = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Mese":
                    {
                        Mese = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "RinnovabiliCnt":
                    {
                        RinnovabiliCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "RinnovabiliSum":
                    {
                        RinnovabiliSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RinnovabiliUpfront":
                    {
                        RinnovabiliUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RinnovabiliRunning":
                    {
                        RinnovabiliRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RinnovabiliSconto":
                    {
                        RinnovabiliSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "CaricatoCnt":
                    {
                        CaricatoCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "CaricatoSum":
                    {
                        CaricatoSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "CaricatoUpfront":
                    {
                        CaricatoUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "CaricatoRunning":
                    {
                        CaricatoRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "CaricatoSconto":
                    {
                        CaricatoSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RichiestaDeliberaCnt":
                    {
                        RichiestaDeliberaCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "RichiestaDeliberaSum":
                    {
                        RichiestaDeliberaSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RichiestaDeliberaUpfront":
                    {
                        RichiestaDeliberaUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RichiestaDeliberaRunning":
                    {
                        RichiestaDeliberaRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "RichiestaDeliberaSconto":
                    {
                        RichiestaDeliberaSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "LiquidatoCnt":
                    {
                        LiquidatoCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "LiquidatoSum":
                    {
                        LiquidatoSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "LiquidatoUpfront":
                    {
                        LiquidatoUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "LiquidatoRunning":
                    {
                        LiquidatoRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "LiquidatoSconto":
                    {
                        LiquidatoSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Anno", Anno);
            writer.WriteAttribute("Mese", Mese);
            writer.WriteAttribute("RinnovabiliCnt", RinnovabiliCnt);
            writer.WriteAttribute("RinnovabiliSum", RinnovabiliSum);
            writer.WriteAttribute("RinnovabiliUpfront", RinnovabiliUpfront);
            writer.WriteAttribute("RinnovabiliRunning", RinnovabiliRunning);
            writer.WriteAttribute("RinnovabiliSconto", RinnovabiliSconto);
            writer.WriteAttribute("CaricatoCnt", CaricatoCnt);
            writer.WriteAttribute("CaricatoSum", CaricatoSum);
            writer.WriteAttribute("CaricatoUpfront", CaricatoUpfront);
            writer.WriteAttribute("CaricatoRunning", CaricatoRunning);
            writer.WriteAttribute("CaricatoSconto", CaricatoSconto);
            writer.WriteAttribute("RichiestaDeliberaCnt", RichiestaDeliberaCnt);
            writer.WriteAttribute("RichiestaDeliberaSum", RichiestaDeliberaSum);
            writer.WriteAttribute("RichiestaDeliberaUpfront", RichiestaDeliberaUpfront);
            writer.WriteAttribute("RichiestaDeliberaRunning", RichiestaDeliberaRunning);
            writer.WriteAttribute("RichiestaDeliberaSconto", RichiestaDeliberaSconto);
            writer.WriteAttribute("LiquidatoCnt", LiquidatoCnt);
            writer.WriteAttribute("LiquidatoSum", LiquidatoSum);
            writer.WriteAttribute("LiquidatoUpfront", LiquidatoUpfront);
            writer.WriteAttribute("LiquidatoRunning", LiquidatoRunning);
            writer.WriteAttribute("LiquidatoSconto", LiquidatoSconto);
        }
    }
}