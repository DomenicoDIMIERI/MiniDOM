using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class ClientiLavoratiStats 
            : IDMDXMLSerializable
        {
            
            public CCollection<ClientiLavoratiStatsItem> items;

            public ClientiLavoratiStats()
            {
                DMDObject.IncreaseCounter(this);
                items = new CCollection<ClientiLavoratiStatsItem>();
            }

            public void Apply(ClientiLavoratiFilter filter)
            {
                var col = new CKeyCollection<ClientiLavoratiStatsItem>();
                // Dim t1, t2 As Double
                // t1 = Timer
                // Me.AggiungiVisite(filter, col)
                // t2 = Timer : Debug.Print("AggiungiVisite " & (t2 - t1)) : t1 = t2
                // Me.AggiungiRichieste(filter, col)
                // t2 = Timer : Debug.Print("AggiungiRichieste " & (t2 - t1)) : t1 = t2
                // Me.AggiungiConsulenze(filter, col)
                // t2 = Timer : Debug.Print("AggiungiConsulenze " & (t2 - t1)) : t1 = t2
                // Me.AggiungiPratiche(filter, col)
                // t2 = Timer : Debug.Print("AggiungiPratiche " & (t2 - t1)) : t1 = t2
                // 'For Each item As ClientiLavoratiStatsItem In items


                // Next
                // Me.items = New CCollection(Of ClientiLavoratiStatsItem)(col)

                if (filter is null)
                    throw new ArgumentNullException("filter");
                var cursor = new ClientiLavoratiStatsItemCursor();
                if (filter.IDPuntoOperativo != 0)
                    cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                if (filter.DataInizio.HasValue)
                {
                    cursor.DataInizioLavorazione.IncludeNulls = true;
                    cursor.DataInizioLavorazione.Value = filter.DataInizio.Value;
                    cursor.DataInizioLavorazione.Operator = Databases.OP.OP_GE;
                    if (filter.DataFine.HasValue)
                    {
                        cursor.DataInizioLavorazione.Value1 = filter.DataFine.Value;
                        cursor.DataInizioLavorazione.Operator = Databases.OP.OP_BETWEEN;
                    }
                }

                while (!cursor.EOF())
                {
                    var item = cursor.Item;
                    var oItem = col.GetItemByKey("K" + item.IDCliente);
                    if (oItem is object)
                    {
                        oItem.MergeWith(item);
                    }
                    else
                    {
                        col.Add("K" + item.IDCliente, item);
                    }

                    cursor.MoveNext();
                }

                cursor.Dispose();
                items = new CCollection<ClientiLavoratiStatsItem>(col);
            }
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "items":
                        {
                            items.Clear();
                            items.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteTag("items", items);
            }

            ~ClientiLavoratiStats()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}