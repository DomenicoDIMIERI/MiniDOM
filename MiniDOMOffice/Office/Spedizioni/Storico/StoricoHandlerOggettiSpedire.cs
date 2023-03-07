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
    public partial class Office
    {

        /// <summary>
        /// Aggiunge gli oggetti da spedire
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerOggettiSpedire 
            : StoricoHandlerBase
        {

            /// <summary>
            /// Aggiunge gli oggetti spediti
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<CustomerCalls.StoricoAction> items, CustomerCalls.CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor1 = new OggettiDaSpedireCursor())
                {
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor1.DataInizioSpedizione.Value = filter.Dal.Value;
                        cursor1.DataInizioSpedizione.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor1.DataInizioSpedizione.Value1 = filter.Al.Value;
                            cursor1.DataInizioSpedizione.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor1.DataInizioSpedizione.Value = filter.Al.Value;
                        cursor1.DataInizioSpedizione.Operator = OP.OP_LE;
                    }

                    if (filter.IDPersona != 0)
                    {
                        cursor1.IDCliente.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        cursor1.NomeCliente.Value = filter.Nominativo + "%";
                        cursor1.NomeCliente.Operator = OP.OP_LIKE;
                    }

                    if (filter.IDOperatore != 0)
                        cursor1.IDRichiestaDa.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor1.CategoriaContenuto.Value = filter.Scopo + "%";
                        cursor1.CategoriaContenuto.Operator = OP.OP_LIKE;
                    }

                    if (filter.StatoConversazione.HasValue)
                    {
                        switch (filter.StatoConversazione)
                        {
                            case 0: // In Attesa
                                {
                                    cursor1.StatoOggetto.ValueIn(new[] { StatoOggettoDaSpedire.InPreparazione, StatoOggettoDaSpedire.ProntoPerLaSpedizione });
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor1.StatoOggetto.ValueIn(new[] { StatoOggettoDaSpedire.Spedito });
                                    break;
                                }

                            default:
                                {
                                    cursor1.StatoOggetto.ValueIn(new[] { StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato, StatoOggettoDaSpedire.ConsegnaFallitaIndirizzoErrato, StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato, StatoOggettoDaSpedire.ConsegnaFallitaRifiutoDestinatario, StatoOggettoDaSpedire.Consegnato, StatoOggettoDaSpedire.SpedizioneAnnullata, StatoOggettoDaSpedire.SpedizioneBocciata, StatoOggettoDaSpedire.SpedizioneRifiutata });
                                    break;
                                }
                        }
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        // cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                        cursor1.INDDEST_ToponimoViaECivico.Value = filter.Numero + "%";
                    }

                    cursor1.DataInizioSpedizione.SortOrder = SortEnum.SORT_DESC;
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    while (cursor1.Read() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        AddActivities(items, cursor1.Item);
                    }
                }
            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, OggettoDaSpedire item)
            {
                var action = new CustomerCalls.StoricoAction();
                action.Data = item.DataRichiesta;
                action.IDOperatore = item.IDRichiestaDa;
                action.NomeOperatore = item.NomeRichiestaDa;
                action.IDCliente = item.IDCliente;
                action.NomeCliente = item.NomeCliente;
                action.Note = "Programmata Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                action.Scopo = item.CategoriaContenuto;
                action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                action.Esito = CustomerCalls.EsitoChiamata.OK;
                action.DettaglioEsito = "Spedizione Programmata";
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = item;
                action.ActionSubID = (int)StatoOggettoDaSpedire.InPreparazione;
                action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                col.Add(action);
                if (item.StatoOggetto > StatoOggettoDaSpedire.ProntoPerLaSpedizione)
                {
                    action = new CustomerCalls.StoricoAction();
                    action.Data = item.DataPresaInCarico;
                    action.IDOperatore = item.IDPresaInCaricoDa;
                    action.NomeOperatore = item.NomePresaInCaricoDa;
                    action.IDCliente = item.IDCliente;
                    action.NomeCliente = item.NomeCliente;
                    action.Note = "Presa in carico della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                    action.Scopo = item.CategoriaContenuto;
                    action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                    action.Esito = CustomerCalls.EsitoChiamata.OK;
                    action.DettaglioEsito = "Spedizione Presa in Carico";
                    action.Durata = 0d;
                    action.Attesa = 0d;
                    action.Tag = item;
                    action.ActionSubID = (int)StatoOggettoDaSpedire.ProntoPerLaSpedizione;
                    action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                    col.Add(action);
                }

                switch (item.StatoOggetto)
                {
                    case StatoOggettoDaSpedire.Spedito:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataInizioSpedizione;
                            action.IDOperatore = item.IDPresaInCaricoDa;
                            action.NomeOperatore = item.NomePresaInCaricoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Inizio della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Inizio Spedizione";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Spedito;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.SpedizioneAnnullata:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Annullamento della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Spediaizone Annullata";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.SpedizioneAnnullata;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.SpedizioneBocciata:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Bocciatura della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Spedizione Bocciata";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.SpedizioneBocciata;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.SpedizioneRifiutata:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Rifiuto del corriere per la Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Spedizione Rifiutata dal Corriere";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.SpedizioneRifiutata;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.Consegnato:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataInizioSpedizione;
                            action.IDOperatore = item.IDPresaInCaricoDa;
                            action.NomeOperatore = item.NomePresaInCaricoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Inizio della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Inizio Spedizione";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Spedito;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "<span class=\"green\">Consegna riuscita</span> di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Consegna Riuscita";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Consegnato;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.ConsegnaFallita:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataInizioSpedizione;
                            action.IDOperatore = item.IDPresaInCaricoDa;
                            action.NomeOperatore = item.NomePresaInCaricoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Inizio della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Inizio Spedizione";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Spedito;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "<span class=\"red\">Consegna fallita</span> di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Consegna Fallita";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.ConsegnaFallita;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.ConsegnaFallitaIndirizzoErrato:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataInizioSpedizione;
                            action.IDOperatore = item.IDPresaInCaricoDa;
                            action.NomeOperatore = item.NomePresaInCaricoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Inizio della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Inizio Spedizione";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Spedito;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "<span class=\"red\">Consegna fallita per indirizzo del destinatario errato</span> di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Consegna Fallita per Indirizzo del destinatario errato";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.ConsegnaFallitaIndirizzoErrato;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataInizioSpedizione;
                            action.IDOperatore = item.IDPresaInCaricoDa;
                            action.NomeOperatore = item.NomePresaInCaricoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Inizio della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Inizio Spedizione";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Spedito;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "<span class=\"red\">Consegna fallita perché il destinatario non è stato trovato</span> di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Consegna Fallita perché il destinatario non è stato trovato";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoOggettoDaSpedire.ConsegnaFallitaRifiutoDestinatario:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataInizioSpedizione;
                            action.IDOperatore = item.IDPresaInCaricoDa;
                            action.NomeOperatore = item.NomePresaInCaricoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "Inizio della Spedizione di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Inizio Spedizione";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.Spedito;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            action = new CustomerCalls.StoricoAction();
                            action.Data = item.DataConferma;
                            action.IDOperatore = item.IDConfermatoDa;
                            action.NomeOperatore = item.NomeConfermatoDa;
                            action.IDCliente = item.IDCliente;
                            action.NomeCliente = item.NomeCliente;
                            action.Note = "<span class=\"red\">Consegna fallita perché rifiutata dal destinatario</span> di <b>" + item.CategoriaContenuto + "</b> a <b>" + item.NomeDestinatario + "</a> presso " + item.IndirizzoDestinatario.ToString();
                            action.Scopo = item.CategoriaContenuto;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(item.IndirizzoDestinatario.ToString(), 255);
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Consegna fallita perché rifiutata dal destinatario";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = item;
                            action.ActionSubID = (int)StatoOggettoDaSpedire.ConsegnaFallitaRifiutoDestinatario;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                        // Debug.Assert(False)
                }
            }

            /// <summary>
            /// Prepara i tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("OggettoDaSpedire", "Spedizione Oggetto");
            }
 
        }
    }
}