using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.CustomerCalls;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Aggiunge le elaborazioni
    /// </summary>
    /// <remarks></remarks>
        public class StoricoElaborazioni 
            : StoricoHandlerBase
        {
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                AggiungiEInternal(items, filter);
                AggiungiIInternal(items, filter);
            }

            private void AggiungiEInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                var cursor = new CImportExportCursor();


                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Esportazione.Value = true;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                if (filter.Dal.HasValue)
                {
                    if (filter.Al.HasValue)
                    {
                        cursor.DataEsportazione.Between(filter.Dal, filter.Al);
                    }
                    else
                    {
                        cursor.DataEsportazione.Value = filter.Dal;
                        cursor.DataEsportazione.Operator = Databases.OP.OP_GE;
                    }
                }
                else if (filter.Al.HasValue)
                {
                    cursor.DataEsportazione.Value = filter.Dal;
                    cursor.DataEsportazione.Operator = Databases.OP.OP_LE;
                }

                if (filter.IDPersona != 0)
                {
                    cursor.IDPersonaEsportata.Value = filter.IDPersona;
                }
                else if (filter.Nominativo != "")
                {
                    cursor.NomePersonaEsportata.Value = filter.Nominativo + "%";
                    cursor.NomePersonaEsportata.Operator = Databases.OP.OP_LIKE;
                }

                if (filter.IDOperatore != 0)
                    cursor.IDEsportataDa.Value = filter.IDOperatore;
                if (filter.IDPuntoOperativo != 0)
                    cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
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
                                cursor.StatoRemoto.Value = StatoEsportazione.NonEsportato;
                                break;
                            }

                        case 1: // in corso
                            {
                                cursor.StatoRemoto.Value = StatoEsportazione.Esportato;
                                cursor.StatoConferma.Value = StatoConfermaEsportazione.Inviato;
                                break;
                            }

                        default:
                            {
                                cursor.StatoConferma.ValueIn(new[] { StatoConfermaEsportazione.Confermato, StatoConfermaEsportazione.Revocato, StatoConfermaEsportazione.Rifiutata });
                                break;
                            }
                    }
                }

                if (filter.Numero != "")
                {
                    // cursor.sou.Value = filter.Numero & "%"
                    // cursor.MezzoDiInvio.Operator = OP.OP_LIKE
                }

                if (filter.IDContesto.HasValue)
                {
                    // cursor1.id.Value = filter.TipoContesto
                    // cursor1.ContextID.Value = filter.IDContesto
                }

                cursor.DataEsportazione.SortOrder = SortEnum.SORT_DESC;
                cursor.IgnoreRights = filter.IgnoreRights;
                while (!cursor.EOF() && (!filter.nMax.HasValue || cnt < filter.nMax))
                {
                    cnt += 1;
                    AddEActivities(items, cursor.Item);
                    cursor.MoveNext();
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Dispose();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }

            private void AggiungiIInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                var cursor = new CImportExportCursor();


                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Esportazione.Value = false;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                if (filter.Dal.HasValue)
                {
                    if (filter.Al.HasValue)
                    {
                        cursor.DataEsportazione.Between(filter.Dal, filter.Al);
                    }
                    else
                    {
                        cursor.DataEsportazione.Value = filter.Dal;
                        cursor.DataEsportazione.Operator = Databases.OP.OP_GE;
                    }
                }
                else if (filter.Al.HasValue)
                {
                    cursor.DataEsportazione.Value = filter.Dal;
                    cursor.DataEsportazione.Operator = Databases.OP.OP_LE;
                }

                if (filter.IDPersona != 0)
                {
                    cursor.IDPersonaImportata.Value = filter.IDPersona;
                }
                else if (filter.Nominativo != "")
                {
                    cursor.NomePersonaImportata.Value = filter.Nominativo + "%";
                    cursor.NomePersonaImportata.Operator = Databases.OP.OP_LIKE;
                }

                if (filter.IDOperatore != 0)
                    cursor.IDOperatoreConferma.Value = filter.IDOperatore;
                if (filter.IDPuntoOperativo != 0)
                    cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
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
                                cursor.StatoRemoto.Value = StatoEsportazione.NonEsportato;
                                break;
                            }

                        case 1: // in corso
                            {
                                cursor.StatoRemoto.Value = StatoEsportazione.Esportato;
                                cursor.StatoConferma.Value = StatoConfermaEsportazione.Inviato;
                                break;
                            }

                        default:
                            {
                                cursor.StatoConferma.ValueIn(new[] { StatoConfermaEsportazione.Confermato, StatoConfermaEsportazione.Revocato, StatoConfermaEsportazione.Rifiutata });
                                break;
                            }
                    }
                }

                if (filter.Numero != "")
                {
                    // cursor.sou.Value = filter.Numero & "%"
                    // cursor.MezzoDiInvio.Operator = OP.OP_LIKE
                }

                if (filter.IDContesto.HasValue)
                {
                    // cursor1.id.Value = filter.TipoContesto
                    // cursor1.ContextID.Value = filter.IDContesto
                }

                cursor.DataEsportazione.SortOrder = SortEnum.SORT_DESC;
                cursor.IgnoreRights = filter.IgnoreRights;
                while (!cursor.EOF() && (!filter.nMax.HasValue || cnt < filter.nMax))
                {
                    cnt += 1;
                    AddIActivities(items, cursor.Item);
                    cursor.MoveNext();
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor.Dispose();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }

            private void AddEActivities(CCollection<StoricoAction> col, CImportExport exp)
            {
                var action = new StoricoAction();
                action.Data = exp.DataEsportazione;
                action.IDOperatore = exp.IDEsportataDa;
                action.NomeOperatore = exp.NomeEsportataDa;
                action.IDCliente = exp.IDPersonaEsportata;
                action.NomeCliente = exp.NomePersonaEsportata;
                action.Note = "Richiesta Confronto - Inviata<br/><br/>" + exp.MessaggioEsportazione + "<br/><hr/><br/>" + exp.MessaggioImportazione;
                action.Scopo = "Richiesta Confronto";
                action.NumeroOIndirizzo = exp.Source.Name;
                if (exp.StatoRemoto == StatoEsportazione.Errore)
                {
                    action.Esito = EsitoChiamata.OK;
                }
                else
                {
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA;
                }

                action.DettaglioEsito = exp.DettaglioStatoRemoto;
                action.Durata = 0;
                action.Attesa = 0;
                action.Tag = exp;
                action.ActionSubID = 0;
                action.Ricevuta = false;
                action.StatoConversazione = (exp.StatoConferma == StatoConfermaEsportazione.Inviato)? StatoConversazione.INCORSO : StatoConversazione.CONCLUSO;
                col.Add(action);
                if (exp.StatoConferma != StatoConfermaEsportazione.Inviato)
                {
                    action = new StoricoAction();
                    action.Data = exp.DataEsportazioneOk;
                    action.IDOperatore = exp.IDOperatoreConferma;
                    action.NomeOperatore = exp.NomeOperatoreConferma;
                    action.IDCliente = exp.IDPersonaEsportata;
                    action.NomeCliente = exp.NomePersonaEsportata;
                    switch (exp.StatoConferma)
                    {
                        case StatoConfermaEsportazione.Confermato:
                            {
                                action.Note = "Invio Forzato da Locale<br/><br/>" + exp.MessaggioConferma;
                                action.Scopo = "Invio Forzato";
                                break;
                            }

                        case StatoConfermaEsportazione.Revocato:
                            {
                                action.Note = "Elaborazione Annullata da Locale<br/><br/>" + exp.MessaggioConferma;
                                action.Scopo = "Elaborazione Annullata";
                                break;
                            }

                        case StatoConfermaEsportazione.Rifiutata:
                            {
                                action.Note = "Elaborazione Rifiutata da Remoto<br/><br/>" + exp.MessaggioConferma;
                                action.Scopo = "Elaborazione Rifiutata";
                                break;
                            }
                    }

                    action.NumeroOIndirizzo = exp.Source.Name;
                    if (exp.StatoRemoto == StatoEsportazione.Errore)
                    {
                        action.Esito = EsitoChiamata.OK;
                    }
                    else
                    {
                        action.Esito = EsitoChiamata.NESSUNA_RISPOSTA;
                    }

                    action.DettaglioEsito = exp.DettaglioStatoRemoto;
                    action.Durata = 0;
                    action.Attesa = 0;
                    action.Tag = exp;
                    action.ActionSubID = 1;
                    action.Ricevuta = false;
                    action.StatoConversazione = StatoConversazione.CONCLUSO;
                    col.Add(action);
                }
            }

            private void AddIActivities(CCollection<StoricoAction> col, CImportExport exp)
            {
                var action = new StoricoAction();
                action.Data = exp.DataEsportazione;
                action.IDOperatore = exp.IDEsportataDa;
                action.NomeOperatore = exp.NomeEsportataDa;
                action.IDCliente = exp.IDPersonaImportata;
                action.NomeCliente = exp.NomePersonaImportata;
                action.Note = "Richiesta Confronto Ricevuta<br/><br/>" + exp.MessaggioEsportazione + "<br/><hr/><br/>" + exp.MessaggioImportazione;
                action.Scopo = "Richiesta Confronto";
                action.NumeroOIndirizzo = exp.Source.Name;
                if (exp.StatoRemoto == StatoEsportazione.Errore)
                {
                    action.Esito = EsitoChiamata.OK;
                }
                else
                {
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA;
                }

                action.DettaglioEsito = exp.DettaglioStatoRemoto;
                action.Durata = 0;
                action.Attesa = 0;
                action.Tag = exp;
                action.ActionSubID = 0;
                action.Ricevuta = true;
                action.StatoConversazione = (exp.StatoConferma == StatoConfermaEsportazione.Inviato)? StatoConversazione.INCORSO : StatoConversazione.CONCLUSO;
                col.Add(action);
                if (exp.StatoConferma != StatoConfermaEsportazione.Inviato)
                {
                    action = new StoricoAction();
                    action.Data = exp.DataEsportazioneOk;
                    action.IDOperatore = exp.IDOperatoreConferma;
                    action.NomeOperatore = exp.NomeOperatoreConferma;
                    action.IDCliente = exp.IDPersonaEsportata;
                    action.NomeCliente = exp.NomePersonaEsportata;
                    switch (exp.StatoConferma)
                    {
                        case StatoConfermaEsportazione.Confermato:
                            {
                                action.Note = "Richiesta Confronto Confermata da Remoto<br/><br/>" + exp.MessaggioConferma;
                                action.Scopo = "Richiesta Confronto";
                                break;
                            }

                        case StatoConfermaEsportazione.Revocato:
                            {
                                action.Note = "Richiesta Confronto Annullata da Remoto<br/><br/>" + exp.MessaggioConferma;
                                action.Scopo = "Richiesta Confronto";
                                break;
                            }

                        case StatoConfermaEsportazione.Rifiutata:
                            {
                                action.Note = "Richiesta Confronto Rifiutata<br/><br/>" + exp.MessaggioConferma;
                                action.Scopo = "Richiesta Confronto";
                                break;
                            }
                    }

                    action.NumeroOIndirizzo = exp.Source.Name;
                    if (exp.StatoRemoto == StatoEsportazione.Errore)
                    {
                        action.Esito = EsitoChiamata.OK;
                    }
                    else
                    {
                        action.Esito = EsitoChiamata.NESSUNA_RISPOSTA;
                    }

                    action.DettaglioEsito = exp.DettaglioStatoRemoto;
                    action.Durata = 0;
                    action.Attesa = 0;
                    action.Tag = exp;
                    action.ActionSubID = 1;
                    action.Ricevuta = false;
                    action.StatoConversazione = StatoConversazione.CONCLUSO;
                    col.Add(action);
                }
            }

            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CImportExport", "Importazioni/Esportazioni");
            }

            public StoricoElaborazioni()
            {
            }
        }
    }
}