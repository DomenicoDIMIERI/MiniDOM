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
        /// Aggiunge le richieste di
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerRichiesteCERQ 
            : CustomerCalls.StoricoHandlerBase
        {

            /// <summary>
            /// Aggiunge le richieste
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<CustomerCalls.StoricoAction> items, CustomerCalls.CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor2 = new RichiestaCERQCursor())
                {
                    cursor2.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor2.DataEffettiva.Value = filter.Dal.Value;
                        cursor2.DataEffettiva.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor2.DataEffettiva.Value1 = filter.Al.Value;
                            cursor2.DataEffettiva.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor2.DataEffettiva.Value = filter.Al.Value;
                        cursor2.DataEffettiva.Operator = OP.OP_LE;
                    }

                    if (!string.IsNullOrEmpty(filter.Contenuto))
                    {
                        cursor2.Note.Value = filter.Contenuto + "%";
                        cursor2.Note.Operator = OP.OP_LIKE;
                    }
                    // If filter.DettaglioEsito Then
                    // filter.etichetta
                    if (filter.IDOperatore != 0)
                        cursor2.IDOperatore.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor2.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.IDPersona != 0)
                    {
                        cursor2.IDCliente.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        cursor2.NomeCliente.Value = filter.Nominativo + "%";
                        cursor2.NomeCliente.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        cursor2.RichiestaAIndirizzo.Value = filter.Numero + "%";
                        cursor2.RichiestaAIndirizzo.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor2.TipoRichiesta.Value = filter.Scopo + "%";
                        cursor2.TipoRichiesta.Operator = OP.OP_LIKE;
                    }
                    // If filter.StatoConversazione.HasValue Then
                    // Select Case filter.StatoConversazione
                    // Case 0 'In Attesa
                    // cursor2.StatoOperazione.Value = StatoRichiestaCERQ.DA_RICHIEDERE
                    // Case 1 'in corso
                    // cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RICHIESTA
                    // Case Else
                    // cursor2.StatoOperazione.ValueIn(New Object() {StatoRichiestaCERQ.ANNULLATA, StatoRichiestaCERQ.RIFIUTATA, StatoRichiestaCERQ.RITIRATA})
                    // End Select
                    // End If
                    if (filter.IDContesto.HasValue)
                    {
                        cursor2.ContextType.Value = filter.TipoContesto;
                        cursor2.ContextID.Value = filter.IDContesto;
                    }

                    cursor2.DataEffettiva.SortOrder = SortEnum.SORT_DESC;
                    cursor2.IgnoreRights = filter.IgnoreRights;
                    // cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RITIRATA
                    while (cursor2.Read() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        // items.Add(cursor2.Item)
                        AddActivities(items, cursor2.Item);
                    }
                }
            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, RichiestaCERQ richiesta)
            {
                var action = new CustomerCalls.StoricoAction();
                action.Data = richiesta.Data;
                action.IDOperatore = richiesta.IDOperatore;
                action.NomeOperatore = richiesta.NomeOperatore;
                action.IDCliente = richiesta.IDCliente;
                action.NomeCliente = richiesta.NomeCliente;
                action.Note = "Richiesta Programmata. Motivo: <b>" + richiesta.TipoRichiesta + "</b> presso <b>" + richiesta.NomeAmministrazione + "</b>" + DMD.Strings.vbNewLine;
                action.Scopo = richiesta.TipoRichiesta;
                action.NumeroOIndirizzo = richiesta.RichiestaAMezzo + ": " + richiesta.RichiestaAIndirizzo;
                action.Esito = CustomerCalls.EsitoChiamata.OK;
                action.DettaglioEsito = "Richiesta Programmata";
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = richiesta;
                action.ActionSubID = (int)StatoRichiestaCERQ.DA_RICHIEDERE;
                action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                col.Add(action);
                if (richiesta.StatoOperazione >= StatoRichiestaCERQ.RICHIESTA)
                {
                    action = new CustomerCalls.StoricoAction();
                    action.Data = richiesta.DataEffettiva;
                    action.IDOperatore = richiesta.IDOperatore;
                    action.NomeOperatore = richiesta.NomeOperatore;
                    action.IDCliente = richiesta.IDCliente;
                    action.NomeCliente = richiesta.NomeCliente;
                    action.Note = "Richiesta Effettuata. Motivo: <b>" + richiesta.TipoRichiesta + "</b> presso <b>" + richiesta.NomeAmministrazione + "</b>" + DMD.Strings.vbNewLine;
                    action.Scopo = richiesta.TipoRichiesta;
                    action.NumeroOIndirizzo = richiesta.RichiestaAMezzo + ": " + richiesta.RichiestaAIndirizzo;
                    action.Esito = CustomerCalls.EsitoChiamata.OK;
                    action.DettaglioEsito = "Richiesta Effettuata";
                    action.Durata = 0d;
                    action.Attesa = 0d;
                    action.Tag = richiesta;
                    action.ActionSubID = (int)StatoRichiestaCERQ.RICHIESTA;
                    action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                    col.Add(action);
                }

                switch (richiesta.StatoOperazione)
                {
                    case StatoRichiestaCERQ.RITIRATA:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = richiesta.DataEffettiva;
                            action.IDOperatore = richiesta.IDOperatoreEffettivo;
                            action.NomeOperatore = richiesta.NomeOperatoreEffettivo;
                            action.IDCliente = richiesta.IDCliente;
                            action.NomeCliente = richiesta.NomeCliente;
                            action.Note = "Richiesta Ritirata. Motivo: <b>" + richiesta.TipoRichiesta + "</b> presso <b>" + richiesta.NomeAmministrazione + "</b>" + DMD.Strings.vbNewLine;
                            action.Scopo = richiesta.TipoRichiesta;
                            action.NumeroOIndirizzo = richiesta.RichiestaAMezzo + ": " + richiesta.RichiestaAIndirizzo;
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Richiesta Ritirata";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = richiesta;
                            action.ActionSubID = (int)StatoRichiestaCERQ.RITIRATA;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoRichiestaCERQ.ANNULLATA:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = richiesta.DataEffettiva;
                            action.IDOperatore = richiesta.IDOperatoreEffettivo;
                            action.NomeOperatore = richiesta.NomeOperatoreEffettivo;
                            action.IDCliente = richiesta.IDCliente;
                            action.NomeCliente = richiesta.NomeCliente;
                            action.Note = "Richiesta Annullata. Motivo: <b>" + richiesta.TipoRichiesta + "</b> presso <b>" + richiesta.NomeAmministrazione + "</b>" + DMD.Strings.vbNewLine;
                            action.Scopo = richiesta.TipoRichiesta;
                            action.NumeroOIndirizzo = richiesta.RichiestaAMezzo + ": " + richiesta.RichiestaAIndirizzo;
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Richiesta Annullata";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = richiesta;
                            action.ActionSubID = (int)StatoRichiestaCERQ.ANNULLATA;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoRichiestaCERQ.RIFIUTATA:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = richiesta.DataEffettiva;
                            action.IDOperatore = richiesta.IDOperatoreEffettivo;
                            action.NomeOperatore = richiesta.NomeOperatoreEffettivo;
                            action.IDCliente = richiesta.IDCliente;
                            action.NomeCliente = richiesta.NomeCliente;
                            action.Note = "Richiesta Rifiutata. Motivo: <b>" + richiesta.TipoRichiesta + "</b> presso <b>" + richiesta.NomeAmministrazione + "</b>" + DMD.Strings.vbNewLine;
                            action.Scopo = richiesta.TipoRichiesta;
                            action.NumeroOIndirizzo = richiesta.RichiestaAMezzo + ": " + richiesta.RichiestaAIndirizzo;
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Richiesta Rifiutata";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = richiesta;
                            action.ActionSubID = (int)StatoRichiestaCERQ.RIFIUTATA;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoRichiestaCERQ.DA_RICHIEDERE:
                        {
                            break;
                        }

                    case StatoRichiestaCERQ.RICHIESTA:
                        {
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }

            /// <summary>
            /// Aggiunge i tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("RichiestaCERQ", "Richiesta");
            }
        }
    }
}