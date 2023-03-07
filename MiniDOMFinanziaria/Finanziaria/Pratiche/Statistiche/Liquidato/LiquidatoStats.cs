using System.Data;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Rappresenta le statistiche sul liquidato
    /// </summary>
    /// <remarks></remarks>
        public class LiquidatoStats : IDMDXMLSerializable
        {
            public CKeyCollection<LiquidatoStatsItem> items;
            public CKeyCollection<string> charts;

            public LiquidatoStats()
            {
                DMDObject.IncreaseCounter(this);
            }

            public virtual void Apply(LiquidatoFilter filter)
            {
                this.items.Clear();
                if (filter.Anni.Count == 0 || filter.PuntiOperativi.Count == 0)
                    return;
                var items = new CKeyCollection<LiquidatoStatsItem>();
                var arrPO = filter.PuntiOperativi.ToArray();
                var arrAnni = filter.Anni.ToArray();
                int lAnno = -1;
                string strAnni = "";
                for (int i = 0, loopTo = DMD.Arrays.UBound(arrAnni); i <= loopTo; i++)
                {
                    if (i > 0)
                        strAnni += ",";
                    strAnni += arrAnni[i].ToString();
                }

                var statoArchiviata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA);
                var statoContatto = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO_ACCETTATO);
                var statoLiquidata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA);
                var statoRichiestaDelibera = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_RICHIESTADELIBERA);
                IDataReader dbRis;
                LiquidatoStatsItem item;
                string dbSQL;

                // Liquidato
                var cursor = new CPraticheCQSPDCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.Trasferita.Value = false;
                cursor.Flags.Value = PraticaFlags.HIDDEN;
                cursor.Flags.Operator = Databases.OP.OP_NE;
                // cursor.StatoContatto.Inizio = Calendar.MakeDate(anno, 1, 1)
                // cursor.StatoContatto.IDToStato = GetID(statoContatto)
                // cursor.StatoContatto.MacroStato = Nothing
                cursor.IDPuntoOperativo.ValueIn(arrPO);

                // Dim strSql As String


                // dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                dbSQL = "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T2].[Data]) As [Mese], " + "Year([T2].[Data]) As [Anno] " + " FROM (";
                dbSQL += cursor.GetSQL() + ") AS [T1] ";
                dbSQL += " INNER JOIN (";
                dbSQL += "SELECT * FROM [tbl_PraticheSTL]  WHERE Year([tbl_PraticheSTL].[Data]) In (" + strAnni + ") AND [tbl_PraticheSTL].[IDToStato] In (" + DBUtils.GetID(statoLiquidata) + ")";
                dbSQL += ") AS T2 ";
                dbSQL += "ON [T1].[ID]=[T2].[IDPratica] ";
                dbSQL += "GROUP BY Year([T2].[Data]), Month([T2].[Data]) ";
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    item = new LiquidatoStatsItem();
                    item.Anno = Sistema.Formats.ToInteger(dbRis["Anno"]);
                    item.Mese = Sistema.Formats.ToInteger(dbRis["Mese"]);
                    item.LiquidatoUpfront = Sistema.Formats.ToValuta(dbRis["Upfront"]);
                    item.LiquidatoRunning = Sistema.Formats.ToValuta(dbRis["Running"]);
                    item.LiquidatoCnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                    item.LiquidatoSum = Sistema.Formats.ToValuta(dbRis["Valore"]);
                    item.LiquidatoSconto = Sistema.Formats.ToValuta(dbRis["Sconto"]);
                    items.Add("K" + item.Anno + "_" + item.Mese, item);
                }

                // Caricato
                dbSQL = "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T2].[Data]) As [Mese], " + "Year([T2].[Data]) As [Anno] " + " FROM (";
                dbSQL += cursor.GetSQL() + ") AS [T1] ";
                dbSQL += " INNER JOIN (";
                dbSQL += "SELECT * FROM [tbl_PraticheSTL]  WHERE Year([tbl_PraticheSTL].[Data]) In (" + strAnni + ") AND [tbl_PraticheSTL].[IDToStato] In (" + DBUtils.GetID(statoContatto) + ")";
                dbSQL += ") AS T2 ";
                dbSQL += "ON [T1].[ID]=[T2].[IDPratica] ";
                dbSQL += "GROUP BY Year([T2].[Data]), Month([T2].[Data]) ";
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new LiquidatoStatsItem();
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

                // Richiesta Delibera
                // dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                // dbSQL &= cursor.GetSQL & ") AS [T1] "
                dbSQL = "SELECT Count(*) As [Cnt], " + "SUM([T1].[MontanteLordo]) As [Valore], " + "SUM([T1].[Running]) As [Running], " + "SUM([T1].[UpFront]) As [UpFront], " + "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " + "Month([T2].[Data]) As [Mese], " + "Year([T2].[Data]) As [Anno] " + " FROM (";
                dbSQL += cursor.GetSQL() + ") AS [T1] ";
                dbSQL += " INNER JOIN (";
                dbSQL += "SELECT * FROM [tbl_PraticheSTL]  WHERE Year([tbl_PraticheSTL].[Data]) In (" + strAnni + ") AND [tbl_PraticheSTL].[IDToStato] In (" + DBUtils.GetID(statoRichiestaDelibera) + ")";
                dbSQL += ") AS [T2] ";
                dbSQL += "ON [T1].[ID]=[T2].[IDPratica] ";
                dbSQL += "GROUP BY Year([T2].[Data]), Month([T2].[Data]) ";
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    item = items.GetItemByKey("K" + Sistema.Formats.ToInteger(dbRis["Anno"]) + "_" + Sistema.Formats.ToInteger(dbRis["Mese"]));
                    if (item is null)
                    {
                        item = new LiquidatoStatsItem();
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

                this.items.AddRange(items);
                this.items.Sort();
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "items":
                        {
                            items.Clear();
                            CKeyCollection tmp = (CKeyCollection)fieldValue;
                            foreach (string k in tmp.Keys)
                                items.Add(k, (LiquidatoStatsItem)tmp[k]);
                            break;
                        }

                    case "charts":
                        {
                            charts.Clear();
                            CKeyCollection tmp = (CKeyCollection)fieldValue;
                            foreach (string k in tmp.Keys)
                                charts.Add(k, DMD.Strings.CStr(tmp[k]));
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteTag("items", items);
                writer.WriteTag("charts", charts);
            }

            ~LiquidatoStats()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}