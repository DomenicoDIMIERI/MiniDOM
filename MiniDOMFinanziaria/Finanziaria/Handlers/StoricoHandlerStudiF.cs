using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Aggiunge gli studi di fattibilità
    /// </summary>
    /// <remarks></remarks>
        public class StoricoHandlerStudiF 
            : StoricoHandlerBase
        {
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor1 = new CQSPDConsulenzaCursor())
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor1.DataConsulenza.Value = filter.Dal.Value;
                        cursor1.DataConsulenza.Operator = Databases.OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor1.DataConsulenza.Value1 = filter.Al.Value;
                            cursor1.DataConsulenza.Operator = Databases.OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor1.DataConsulenza.Value = filter.Al.Value;
                        cursor1.DataConsulenza.Operator = Databases.OP.OP_LE;
                    }

                    if (filter.IDPersona != 0)
                    {
                        cursor1.IDCliente.Value = filter.IDPersona;
                    }
                    else if (filter.Nominativo != "")
                    {
                        cursor1.NomeCliente.Value = filter.Nominativo + "%";
                        cursor1.NomeCliente.Operator = Databases.OP.OP_LIKE;
                    }

                    if (filter.IDOperatore != 0)
                        cursor1.IDInseritoDa.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.Scopo != "")
                    {
                        // cursor1..Value = filter.Scopo & "%"
                        // cursor1.Motivo.Operator = OP.OP_LIKE
                    }

                    if (filter.StatoConversazione.HasValue)
                    {
                        switch (filter.StatoConversazione)
                        {
                            case 0: // In Attesa
                                {
                                    cursor1.IDInseritoDa.Value = 0;
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor1.IDInseritoDa.Value = 0;
                                    break;
                                }

                            default:
                                {
                                    cursor1.IDInseritoDa.Value = 0;
                                    cursor1.IDInseritoDa.Operator = Databases.OP.OP_NE;
                                    break;
                                }
                        }
                    }

                    if (filter.Numero != "")
                    {
                        // cursor1.num.Value = filter.Numero & "%"
                        // cursor1.NomeFonte.Operator = OP.OP_LIKE
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        // cursor1.id.Value = filter.TipoContesto
                        // cursor1.ContextID.Value = filter.IDContesto
                    }

                    cursor1.DataConsulenza.SortOrder = SortEnum.SORT_DESC;
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    while (!cursor1.EOF() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        AddActivities(items, cursor1.Item);
                        cursor1.MoveNext();
                    }
                }

 
            }

            private void AddActivities(CCollection<StoricoAction> col, CQSPDConsulenza rich)
            {
                StoricoAction action = default;
                if (col.Count > 0)
                {
                    var lastAction = col[col.Count - 1];
                    DateTime dc = (DateTime)DMD.DateUtils.GetDatePart(Sistema.Formats.ToDate(rich.DataConsulenza));
                    if (lastAction.ActionType == "CQSPDConsulenza" && DMD.DateUtils.GetDatePart(lastAction.Data) == dc)
                    {
                        lastAction.Note += "<br/>" + DMD.Strings.vbNewLine;
                        lastAction.Note += "Studio di Fattibilità: <b>" + Sistema.RPC.FormatID(DBUtils.GetID(rich)) + "</b>, Note: " + rich.Descrizione;
                        return;
                    }
                }

                action = new StoricoAction();
                // action.ActionType = "CQSPDConsulenza"
                action.Data = Sistema.Formats.ToDate(rich.DataConsulenza);
                action.IDOperatore = rich.IDInseritoDa;
                if (rich.InseritoDa is object)
                    action.NomeOperatore = rich.InseritoDa.Nominativo;
                action.IDCliente = rich.IDCliente;
                action.NomeCliente = rich.NomeCliente;
                action.Note = "Studio di Fattibilità: <b>" + Sistema.RPC.FormatID(DBUtils.GetID(rich)) + "</b>, Note: " + rich.Descrizione;
                action.Scopo = "Studio di Fattibilità";
                action.NumeroOIndirizzo = Sistema.RPC.FormatID(DBUtils.GetID(rich));
                action.Esito = EsitoChiamata.OK;
                action.DettaglioEsito = "";
                action.Durata = rich.Durata;
                action.Attesa = 0;
                action.Tag = rich;
                action.ActionSubID = 0;
                action.StatoConversazione = StatoConversazione.CONCLUSO;
                col.Add(action);
            }

            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CQSPDConsulenza", "Studo di fattibilità");
            }

            public StoricoHandlerStudiF()
            {
            }
        }
    }
}