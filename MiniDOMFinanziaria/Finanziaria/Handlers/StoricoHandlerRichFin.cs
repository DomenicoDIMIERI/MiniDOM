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
    /// Aggiunge le richieste di finanziamento
    /// </summary>
    /// <remarks></remarks>
        public class StoricoHandlerRichFin : StoricoHandlerBase
        {
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor1 = new CRichiesteFinanziamentoCursor())
                {


                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor1.Data.Value = filter.Dal.Value;
                        cursor1.Data.Operator = Databases.OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor1.Data.Value1 = filter.Al.Value;
                            cursor1.Data.Operator = Databases.OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor1.Data.Value = filter.Al.Value;
                        cursor1.Data.Operator = Databases.OP.OP_LE;
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
                        cursor1.CreatoDa.Value = filter.IDOperatore;
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
                                    cursor1.CreatoDa.Value = 0;
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor1.CreatoDa.Value = 0;
                                    break;
                                }

                            default:
                                {
                                    cursor1.CreatoDa.Value = 0;
                                    cursor1.CreatoDa.Operator = Databases.OP.OP_NE;
                                    break;
                                }
                        }
                    }

                    if (filter.Numero != "")
                    {
                        cursor1.NomeFonte.Value = filter.Numero + "%";
                        cursor1.NomeFonte.Operator = Databases.OP.OP_LIKE;
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        // cursor1.id.Value = filter.TipoContesto
                        // cursor1.ContextID.Value = filter.IDContesto
                    }

                    cursor1.Data.SortOrder = SortEnum.SORT_DESC;
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    while (!cursor1.EOF() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        AddActivities(items, cursor1.Item);
                        cursor1.MoveNext();
                    }
                }
            }

            private string formatImporto(CRichiestaFinanziamento rich)
            {
                switch (rich.TipoRichiesta)
                {
                    case TipoRichiestaFinanziamento.ALMENO:
                        {
                            return "Almeno " + Sistema.Formats.FormatValuta(rich.ImportoRichiesto);
                        }

                    case TipoRichiestaFinanziamento.MASSIMO_POSSIBILE:
                        {
                            return "Massimo possibile";
                        }

                    case TipoRichiestaFinanziamento.NONSPECIFICATO:
                        {
                            return "Non specificato";
                        }

                    case TipoRichiestaFinanziamento.TRA:
                        {
                            return "Tra " + Sistema.Formats.FormatValuta(rich.ImportoRichiesto) + " e " + Sistema.Formats.FormatValuta(rich.ImportoRichiesto1);
                        }

                    case TipoRichiestaFinanziamento.UGUALEA:
                        {
                            return Sistema.Formats.FormatValuta(rich.ImportoRichiesto);
                        }

                    default:
                        {
                            return "?";
                        }
                }
            }

            private void AddActivities(CCollection<StoricoAction> col, CRichiestaFinanziamento rich)
            {
                StoricoAction action;
                if (rich.ImportoRichiesto.HasValue)
                {
                    action = new StoricoAction();
                    action.Data = Sistema.Formats.ToDate(rich.Data);
                    action.IDOperatore = rich.CreatoDaId;
                    action.NomeOperatore = rich.NomeAssegnatoA;
                    action.IDCliente = rich.IDCliente;
                    action.NomeCliente = rich.NomeCliente;
                    action.Note = "Importo Richiesto: <b>" + formatImporto(rich) + "</b>";
                    if (!string.IsNullOrEmpty(rich.Scopo))
                        action.Note = DMD.Strings.Combine(action.Note, "Scopo: " + rich.Scopo, ", ");
                    // If (rich.Note <> "") Then action.Note = Strings.Combine(action.Note, "Note: " & rich.Note, ", ")
                    action.Scopo = rich.Scopo;
                    action.NumeroOIndirizzo = "";
                    action.Esito = EsitoChiamata.OK;
                    action.DettaglioEsito = "";
                    action.Durata = rich.Durata;
                    action.Attesa = 0;
                    action.Tag = rich;
                    action.ActionSubID = 0;
                    action.StatoConversazione = StatoConversazione.CONCLUSO;
                    col.Add(action);
                }
            }

            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CRichiestaFinanziamento", "Richiesta di Finanziamento");
            }

            public StoricoHandlerRichFin()
            {
            }
        }
    }
}