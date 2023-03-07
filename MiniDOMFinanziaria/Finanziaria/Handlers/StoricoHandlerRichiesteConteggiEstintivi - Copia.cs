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
    /// Aggiunge le commissioni
    /// </summary>
    /// <remarks></remarks>
        public class StoricoHandlerRichiesteConteggiEstintivi : StoricoHandlerBase
        {
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor1 = new CRichiestaConteggioCursor())
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor1.InviatoIl.Value = filter.Dal.Value;
                        cursor1.InviatoIl.Operator = Databases.OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor1.InviatoIl.Value1 = filter.Al.Value;
                            cursor1.InviatoIl.Operator = Databases.OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor1.InviatoIl.Value = filter.Al.Value;
                        cursor1.InviatoIl.Operator = Databases.OP.OP_LE;
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
                        cursor1.PresaInCaricoDaID.Value = filter.IDOperatore;
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
                                    cursor1.PresaInCaricoDaID.Value = 0;
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor1.PresaInCaricoDaID.Value = 0;
                                    break;
                                }

                            default:
                                {
                                    cursor1.PresaInCaricoDaID.Value = 0;
                                    cursor1.PresaInCaricoDaID.Operator = Databases.OP.OP_NE;
                                    break;
                                }
                        }
                    }

                    if (filter.Numero != "")
                    {
                        cursor1.MezzoDiInvio.Value = filter.Numero + "%";
                        cursor1.MezzoDiInvio.Operator = Databases.OP.OP_LIKE;
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        // cursor1.id.Value = filter.TipoContesto
                        // cursor1.ContextID.Value = filter.IDContesto
                    }

                    cursor1.InviatoIl.SortOrder = SortEnum.SORT_DESC;
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    while (!cursor1.EOF() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        AddActivities(items, cursor1.Item);
                        cursor1.MoveNext();
                    }
                    
                }



            }

            private void AddActivities(CCollection<StoricoAction> col, CRichiestaConteggio rich)
            {
                var action = new StoricoAction();
                action.Data = rich.InviatoIl;
                action.IDOperatore = rich.RicevutoDaID;
                action.NomeOperatore = rich.RicevutoDaNome;
                action.IDCliente = rich.IDCliente;
                action.NomeCliente = rich.NomeCliente;
                action.Note = "Segnalazione Stay: " + Sistema.Formats.FormatValuta(rich.ImportoRata) + " x " + Sistema.Formats.FormatInteger(rich.DurataMesi) + ", TAN: " + Sistema.Formats.FormatPercentage(rich.TAN) + " %, TAEG: " + Sistema.Formats.FormatPercentage(rich.TAEG) + ".<br/>Richiedente " + rich.NomeIstituto + ".<br/>Segnalata da " + rich.InviatoDaNome;
                action.Scopo = "Segnalazione Stay";
                action.NumeroOIndirizzo = rich.MezzoDiInvio;
                action.Esito = EsitoChiamata.OK;
                action.DettaglioEsito = "Segnalata";
                action.Durata = 0;
                action.Attesa = 0;
                action.Tag = rich;
                action.ActionSubID = 0;
                action.StatoConversazione = (rich.PresaInCaricoDaID == 0)? StatoConversazione.INCORSO : StatoConversazione.CONCLUSO;
                col.Add(action);

                // action = New StoricoAction
                // action.Data = rich.DataSegnalazione
                // action.IDOperatore = rich.RicevutoDaID
                // action.NomeOperatore = rich.RicevutoDaNome
                // action.IDCliente = rich.IDCliente
                // action.NomeCliente = rich.NomeCliente
                // action.Note = "Richiesta CE da parte di " & rich.NomeIstituto & " ricevuta da " & rich.RicevutoDaNome
                // action.Scopo = "Richiesta CE"
                // action.NumeroOIndirizzo = rich.MezzoDiInvio
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Ricevuta"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = rich
                // action.ActionSubID = 1
                // action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
                // col.Add(action)

                // If (rich.DataPresaInCarico.HasValue AndAlso rich.PresaInCaricoDaID <> 0) Then
                // action = New StoricoAction
                // action.Data = rich.DataPresaInCarico
                // action.IDOperatore = rich.PresaInCaricoDaID
                // action.NomeOperatore = rich.PresaInCaricoDaNome
                // action.IDCliente = rich.IDCliente
                // action.NomeCliente = rich.NomeCliente
                // action.Note = "Richiesta CE da parte di " & rich.NomeIstituto & " presa in carico da " & rich.PresaInCaricoDaNome
                // action.Scopo = "Richiesta CE"
                // action.NumeroOIndirizzo = rich.MezzoDiInvio
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Presa in carico"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = rich
                // action.ActionSubID = 2
                // action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
                // col.Add(action)
                // End If

                // If (rich.DataEvasione.HasValue AndAlso rich.DataEvasione.Value <= Calendar.ToMorrow) Then
                // action = New StoricoAction
                // action.Data = rich.DataEvasione
                // action.IDOperatore = 0 'rich.PresaInCaricoDaID
                // action.NomeOperatore = "" ' rich.PresaInCaricoDaNome
                // action.IDCliente = rich.IDCliente
                // action.NomeCliente = rich.NomeCliente
                // action.Note = "Richiesta CE da parte di " & rich.NomeIstituto & " evasa da " & rich.PresaInCaricoDaNome
                // action.Scopo = "Richiesta CE"
                // action.NumeroOIndirizzo = rich.MezzoDiInvio
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Evasa"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = rich
                // action.ActionSubID = 3
                // action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
                // col.Add(action)
                // End If

            }

            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CRichiestaConteggio", "Richiesta Conteggio Estintivo");
            }

            public StoricoHandlerRichiesteConteggiEstintivi()
            {
            }
        }
    }
}