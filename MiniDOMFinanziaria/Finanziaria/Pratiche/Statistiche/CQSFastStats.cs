using System;
using System.Data;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CQSFastStats
            : IDMDXMLSerializable
        {
            public int NumeroRichieste;
            public int NumeroConsulenzeAccettate;
            public int NumeroConsulenzeRifiutate;
            public int NumeroConsulenzeTotale;
            public int NumeroPraticheSecci;
            public int NumeroPraticheLiquidate;
            public int NumeroPraticheAnnullate;
            public int NumeroPraticheEstinte;
            public int NumeroPraticheTotale;

            public CQSFastStats()
            {
                DMDObject.IncreaseCounter(this);
                Reset();
            }

            public CQSFastStats(CQSFilter filter) : this()
            {
                Execute(filter);
            }

            public void Reset()
            {
                NumeroRichieste = 0;
                NumeroConsulenzeAccettate = 0;
                NumeroConsulenzeRifiutate = 0;
                NumeroConsulenzeTotale = 0;
                NumeroPraticheSecci = 0;
                NumeroPraticheLiquidate = 0;
                NumeroPraticheAnnullate = 0;
                NumeroPraticheEstinte = 0;
                NumeroPraticheTotale = 0;
            }

            public void Execute(CQSFilter filter)
            {
                string dbSQL;
                IDataReader dbRis;
                if (filter is null)
                    throw new ArgumentNullException("filter");
                Reset();
                var stSecci = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTRATTO_FIRMATO);
                var stLiquidata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA);
                var stArchiviata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA);
                var stAnnullata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA);
                if (filter.IDRichiesta != 0)
                {
                    NumeroRichieste = 1;

                    // Me.NumeroConsulenzeTotale = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta)))
                    // Me.NumeroConsulenzeAccettate = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoConsulenza]=" & StatiConsulenza.ACCETTATA & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta)))
                    // Me.NumeroConsulenzeRifiutate = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoConsulenza]=" & StatiConsulenza.RIFIUTATA & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta)))
                    dbSQL = "SELECT Count(*) As [Cnt], [StatoConsulenza] FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiesta]=" + DBUtils.DBNumber(filter.IDRichiesta) + " GROUP BY [StatoConsulenza]";
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        StatiConsulenza idStatoAttuale = (StatiConsulenza)Sistema.Formats.ToInteger(dbRis["StatoConsulenza"]);
                        int cnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                        NumeroConsulenzeTotale += cnt;
                        if (idStatoAttuale == StatiConsulenza.ACCETTATA)
                        {
                            NumeroConsulenzeAccettate += cnt;
                        }
                        else if (idStatoAttuale == StatiConsulenza.RIFIUTATA)
                        {
                            NumeroConsulenzeRifiutate += cnt;
                        }
                    }

                    dbRis.Dispose();

                    // Me.NumeroPraticheTotale = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta))
                    // Me.NumeroPraticheSecci = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " AND [IDStatoAttuale]=" & GetID(stSecci)))
                    // Me.NumeroPraticheLiquidate = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " AND [IDStatoAttuale] In (" & GetID(stLiquidata) & ", " & GetID(stArchiviata) & ")"))
                    // Me.NumeroPraticheAnnullate = Formats.ToInteger(Finanziaria.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " AND [IDStatoAttuale]=" & GetID(stAnnullata)))
                    // Me.NumeroPraticheEstinte = 0

                    dbSQL = "SELECT Count(*) As [Cnt], [IDStatoAttuale] FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + filter.IDRichiesta + " GROUP BY [IDStatoAttuale]";
                    dbRis = Database.ExecuteReader(dbSQL);
                    while (dbRis.Read())
                    {
                        int idStatoAttuale = Sistema.Formats.ToInteger(dbRis["IDStatoAttuale"]);
                        int cnt = Sistema.Formats.ToInteger(dbRis["Cnt"]);
                        NumeroPraticheTotale += cnt;
                        if (idStatoAttuale == DBUtils.GetID(stSecci))
                        {
                            NumeroPraticheSecci += cnt;
                        }
                        else if (idStatoAttuale == DBUtils.GetID(stLiquidata) || idStatoAttuale == DBUtils.GetID(stArchiviata))
                        {
                            NumeroPraticheLiquidate += cnt;
                        }
                        else if (idStatoAttuale == DBUtils.GetID(stAnnullata))
                        {
                            NumeroPraticheAnnullate = cnt;
                        }
                    }

                    dbRis.Dispose();
                }
                else if (filter.IDFonte != 0)
                {
                    dbSQL = "SELECT [ID] FROM [tbl_RichiesteFinanziamenti] WHERE [TipoFonte]=" + DBUtils.DBString(filter.TipoFonte) + " AND [IDFonte]=" + DBUtils.DBNumber(filter.IDFonte) + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                    dbRis = Database.ExecuteReader(dbSQL);
                    string ids = "";
                    while (dbRis.Read())
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += DBUtils.DBNumber(Sistema.Formats.ToInteger(dbRis["ID"]));
                        NumeroRichieste += 1;
                    }

                    dbRis.Dispose();
                    if (!string.IsNullOrEmpty(ids))
                    {
                        NumeroConsulenzeTotale = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiesta] In (" + ids + ")"));
                        NumeroConsulenzeAccettate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [StatoConsulenza]=" + ((int)StatiConsulenza.ACCETTATA).ToString() + " AND [IDRichiesta] In (" + ids + ")"));
                        NumeroConsulenzeRifiutate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [StatoConsulenza]=" + ((int)StatiConsulenza.RIFIUTATA).ToString() + " AND [IDRichiesta] In (" + ids + ")"));
                        NumeroPraticheTotale = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento] In (" + ids + ")"));
                        NumeroPraticheSecci = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + DBUtils.GetID(stSecci) + " AND [IDRichiesta] In (" + ids + ")"));
                        NumeroPraticheLiquidate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento] In (" + DBUtils.GetID(stLiquidata) + ", " + DBUtils.GetID(stArchiviata) + ")" + " AND [IDRichiesta] In (" + ids + ")"));
                        NumeroPraticheAnnullate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + DBUtils.GetID(stAnnullata) + " AND [IDRichiesta] In (" + ids + ")"));
                        NumeroPraticheEstinte = 0;
                    }
                }
                else if (filter.IDConsulenza != 0)
                {
                    NumeroRichieste = 1;
                    NumeroConsulenzeTotale = 1;
                    NumeroPraticheTotale = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDConsulenza] In (" + filter.IDConsulenza + ")"));
                    NumeroPraticheSecci = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + DBUtils.GetID(stSecci) + " AND [IDConsulenza] = " + filter.IDConsulenza));
                    NumeroPraticheLiquidate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento] In (" + DBUtils.GetID(stLiquidata) + ", " + DBUtils.GetID(stArchiviata) + ")" + " AND [IDConsulenza] = " + filter.IDConsulenza));
                    NumeroPraticheAnnullate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + DBUtils.GetID(stAnnullata) + " AND [IDConsulenza] = " + filter.IDConsulenza));
                    NumeroPraticheEstinte = 0;
                }
                else if (filter.IDConsulente != 0)
                {
                    NumeroRichieste = -1;
                    NumeroConsulenzeTotale = -1;
                    NumeroPraticheTotale = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDConsulenza] In (" + filter.IDConsulenza + ")"));
                    NumeroPraticheSecci = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + DBUtils.GetID(stSecci) + " AND [IDConsulente] = " + filter.IDConsulente));
                    NumeroPraticheLiquidate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento] In (" + DBUtils.GetID(stLiquidata) + ", " + DBUtils.GetID(stArchiviata) + ")" + " AND [IDConsulente]  = " + filter.IDConsulente));
                    NumeroPraticheAnnullate = Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDRichiestaFinanziamento]=" + DBUtils.GetID(stAnnullata) + " AND [IDConsulente]  = " + filter.IDConsulente));
                    NumeroPraticheEstinte = 0;
                }
                else
                {
                }
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "NR":
                        {
                            NumeroRichieste = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NCA":
                        {
                            NumeroConsulenzeAccettate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NCR":
                        {
                            NumeroConsulenzeRifiutate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NCT":
                        {
                            NumeroConsulenzeTotale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NPS":
                        {
                            NumeroPraticheSecci = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NPL":
                        {
                            NumeroPraticheLiquidate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NPA":
                        {
                            NumeroPraticheAnnullate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NPE":
                        {
                            NumeroPraticheEstinte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NPT":
                        {
                            NumeroPraticheTotale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("NR", NumeroRichieste);
                writer.WriteAttribute("NCA", NumeroConsulenzeAccettate);
                writer.WriteAttribute("NCR", NumeroConsulenzeRifiutate);
                writer.WriteAttribute("NCT", NumeroConsulenzeTotale);
                writer.WriteAttribute("NPS", NumeroPraticheSecci);
                writer.WriteAttribute("NPL", NumeroPraticheLiquidate);
                writer.WriteAttribute("NPA", NumeroPraticheAnnullate);
                writer.WriteAttribute("NPE", NumeroPraticheEstinte);
                writer.WriteAttribute("NPT", NumeroPraticheTotale);
            }

            ~CQSFastStats()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}