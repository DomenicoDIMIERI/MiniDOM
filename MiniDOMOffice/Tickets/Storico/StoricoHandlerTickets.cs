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
        /// Aggiunge le richieste di conteggi estintivi
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerTickets 
            : CustomerCalls.StoricoHandlerBase
        {

            /// <summary>
            /// Aggiunge le richieste di supporto allo storico
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<CustomerCalls.StoricoAction> items, CustomerCalls.CRMFindFilter filter)
            {
                using (var cursor = new CTicketCursor())
                {
                    int cnt = 0;

                    if (filter.IDPersona != 0)
                    {
                        // cursor.IDPersona.Value = filter.IDPersona
                        cursor.IDCliente.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        // cursor.NomePersona.Operator = OP.OP_LIKE
                        // cursor.NomePersona.Value = filter.Nominativo & "%"

                        cursor.NomeCliente.Value = filter.Nominativo + "%";
                        cursor.NomeCliente.Operator = OP.OP_LIKE;
                    }

                    if (filter.IDOperatore != 0)
                        cursor.IDInCaricoA.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Contenuto))
                    {
                        cursor.Messaggio.Value = filter.Contenuto;
                        cursor.Messaggio.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Etichetta))
                    {
                        // cursor..Value = filter.Etichetta & "%"
                        // cursor.NomeIndirizzo.Operator = OP.OP_LIKE
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        //TODO controllare conversione da hex
                        int id = DMD.Integers.TryParse("&H" + filter.Numero, 0);
                        if (id > 0) cursor.ID.Value = id;
                        // cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                    }

                    if (filter.Dal.HasValue)
                    {
                        cursor.DataRichiesta.Value = filter.Dal.Value;
                        cursor.DataRichiesta.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor.DataRichiesta.Value1 = filter.Al.Value;
                            cursor.DataRichiesta.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor.DataRichiesta.Value = filter.Al.Value;
                        cursor.DataRichiesta.Operator = OP.OP_LE;
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor.Categoria.Value = filter.Scopo + "%";
                        cursor.Categoria.Operator = OP.OP_LIKE;
                    }
                    // If (filter.Esito.HasValue) Then cursor.StatoSegnalazione.Value = IIf( filter.Esito.Value, 
                    // If (filter.IDContesto.HasValue) Then
                    // cursor.Contesto.Value = filter.TipoContesto
                    // cursor.IDContesto.Value = filter.IDContesto
                    // End If
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    // cursor.Data.SortOrder = SortEnum.SORT_DESC
                    cursor.IgnoreRights = filter.IgnoreRights;
                    // If (filter.StatoConversazione.HasValue) Then cursor.StatoConversazione.Value = filter.StatoConversazione

                    cursor.DataRichiesta.SortOrder = SortEnum.SORT_DESC;
                    while (cursor.Read() && (!filter.nMax.HasValue || cnt < filter.nMax.Value))
                    {
                        cnt += 1;
                        var item = cursor.Item;
                        AddActivities(items, item);                         
                    }
                }
            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, CTicket res)
            {
                var action = new CustomerCalls.StoricoAction();
                int actionSubID = 0;
                action.Data = res.DataRichiesta;
                action.IDOperatore = res.IDApertoDa;
                action.NomeOperatore = res.NomeApertoDa;
                action.IDCliente = res.IDCliente;
                action.NomeCliente = res.NomeCliente;
                action.Note = "Ticket Aperto: " + res.Messaggio;
                action.Scopo = res.Categoria + " / " + res.Sottocategoria;
                action.NumeroOIndirizzo = res.NumberEx;
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = res;
                action.Ricevuta = false;
                action.ActionSubID = actionSubID;
                actionSubID += 1;
                switch (res.StatoSegnalazione)
                {
                    case TicketStatus.RISOLTO:
                    case TicketStatus.NONRISOLVIBILE:
                        {
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            break;
                        }

                    case TicketStatus.INSERITO:
                    case TicketStatus.APERTO:
                        {
                            action.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA;
                            break;
                        }

                    default:
                        {
                            action.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                            break;
                        }
                }

                switch (res.StatoSegnalazione)
                {
                    case TicketStatus.RISOLTO:
                        {
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Risolto";
                            break;
                        }

                    case TicketStatus.NONRISOLVIBILE:
                        {
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Non Risolvibile";
                            break;
                        }

                    case TicketStatus.APERTO:
                        {
                            action.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                            action.DettaglioEsito = "In Attesa";
                            break;
                        }

                    default:
                        {
                            action.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                            action.DettaglioEsito = "In Lavorazione";
                            break;
                        }
                }

                col.Add(action);
                foreach (CTicketAnsware m in res.Messages)
                {
                    action = new CustomerCalls.StoricoAction();
                    action.Data = m.Data;
                    action.IDOperatore = m.IDOperatore;
                    action.NomeOperatore = m.NomeOperatore;
                    action.IDCliente = res.IDCliente;
                    action.NomeCliente = res.NomeCliente;
                    action.Note = "Ticket Aggiornato: " + m.Messaggio;
                    action.Scopo = res.Categoria + " / " + res.Sottocategoria;
                    action.NumeroOIndirizzo = res.NumberEx;
                    action.Esito = CustomerCalls.EsitoChiamata.OK;
                    action.Durata = 0d;
                    action.Attesa = 0d;
                    action.Tag = res;
                    action.Ricevuta = false;
                    action.ActionSubID = actionSubID;
                    actionSubID += 1;
                    switch (res.StatoSegnalazione)
                    {
                        case TicketStatus.RISOLTO:
                        case TicketStatus.NONRISOLVIBILE:
                            {
                                action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                                break;
                            }

                        case TicketStatus.INSERITO:
                        case TicketStatus.APERTO:
                            {
                                action.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA;
                                break;
                            }

                        default:
                            {
                                action.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO;
                                break;
                            }
                    }

                    switch (m.StatoTicket)
                    {
                        case TicketStatus.RISOLTO:
                            {
                                action.Esito = CustomerCalls.EsitoChiamata.OK;
                                action.DettaglioEsito = "Risolto";
                                break;
                            }

                        case TicketStatus.NONRISOLVIBILE:
                            {
                                action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                                action.DettaglioEsito = "Non Risolvibile";
                                break;
                            }

                        case TicketStatus.APERTO:
                            {
                                action.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                                action.DettaglioEsito = "In Attesa";
                                break;
                            }

                        default:
                            {
                                action.Esito = CustomerCalls.EsitoChiamata.ALTRO;
                                action.DettaglioEsito = "In Lavorazione";
                                break;
                            }
                    }

                    col.Add(action);
                }

                // Select Case res.StatoSegnalazione
                // Case TicketStatus.RISOLTO, TicketStatus.NONRISOLVIBILE
                // action = New StoricoAction
                // action.Data = res.DataPresaInCarico
                // action.IDOperatore = res.IDInCaricoA
                // action.NomeOperatore = res.NomeInCaricoA
                // action.IDCliente = res.IDCliente
                // action.NomeCliente = res.NomeCliente
                // action.Note = "Ticket Assegnato: " & res.Messaggio
                // action.Scopo = res.Categoria & " / " & res.Sottocategoria
                // action.NumeroOIndirizzo = res.NumberEx
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Preso in carico"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = res
                // action.StatoConversazione = StatoConversazione.CONCLUSO
                // action.Ricevuta = False
                // action.ActionSubID = actionSubID : actionSubID += 1
                // col.Add(action)

                // action = New StoricoAction
                // action.Data = res.GetDataUltimoAggiornamento
                // action.IDOperatore = res.IDInCaricoA
                // action.NomeOperatore = res.NomeInCaricoA
                // action.IDCliente = res.IDCliente
                // action.NomeCliente = res.NomeCliente
                // action.Note = "Ticket " & CStr(IIf(res.StatoSegnalazione = TicketStatus.RISOLTO, "Risolto", "Non Risolvibile")) & ": " & res.Messaggio
                // action.Scopo = res.Categoria & " / " & res.Sottocategoria
                // action.NumeroOIndirizzo = res.NumberEx
                // action.Esito = IIf(res.StatoSegnalazione = TicketStatus.RISOLTO, EsitoChiamata.OK, EsitoChiamata.NESSUNA_RISPOSTA)
                // action.DettaglioEsito = action.Note
                // action.StatoConversazione = StatoConversazione.CONCLUSO
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = res
                // action.Ricevuta = False
                // action.ActionSubID = actionSubID : actionSubID += 1
                // col.Add(action)

                // Case TicketStatus.INSERITO, TicketStatus.APERTO

                // Case Else
                // action = New StoricoAction
                // action.Data = res.DataPresaInCarico
                // action.IDOperatore = res.IDInCaricoA
                // action.NomeOperatore = res.NomeInCaricoA
                // action.IDCliente = res.IDCliente
                // action.NomeCliente = res.NomeCliente
                // action.Note = "Ticket Assegnato: " & res.Messaggio
                // action.Scopo = res.Categoria & " / " & res.Sottocategoria
                // action.NumeroOIndirizzo = res.NumberEx
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Ticket Preso in carico"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = res
                // action.StatoConversazione = StatoConversazione.INCORSO
                // action.Ricevuta = False
                // action.ActionSubID = actionSubID : actionSubID += 1
                // col.Add(action)
                // End Select


            }

            /// <summary>
            /// Restituisce true se l'oggetto é supportato
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            protected override bool IsSupportedTipoOggetto(CustomerCalls.CRMFindFilter filter)
            {
                return string.IsNullOrEmpty(filter.TipoOggetto) || filter.TipoOggetto == "CTicket";
            }

            /// <summary>
            /// Prepara la collezione dei tipi oggetto
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CTicket", "Ticket");
            }
        }
    }
}