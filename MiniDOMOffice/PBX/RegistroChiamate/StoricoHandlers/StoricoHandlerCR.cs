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
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Handler delle chiamate registrate
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerCR 
            : StoricoHandlerBase
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public StoricoHandlerCR()
            {
            }

            /// <summary>
            /// Aggiunge le chiamate registrate
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(
                                    CCollection<StoricoAction> items, 
                                    CRMFindFilter filter
                                    )
            {
                int cnt = 0;

                using (var cursor = new ChiamataRegistrataCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor.DataInizio.Value = filter.Dal.Value;
                        cursor.DataInizio.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor.DataInizio.Value1 = filter.Al.Value;
                            cursor.DataInizio.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor.DataInizio.Value = filter.Al.Value;
                        cursor.DataInizio.Operator = OP.OP_LE;
                    }

                    if (!string.IsNullOrEmpty(filter.Contenuto))
                    {
                        // cursor..Value = filter.Contenuto & "%"
                        // cursor.Note.Operator = OP.OP_LIKE
                    }
                    // If filter.DettaglioEsito Then
                    // filter.etichetta
                    // If filter.IDOperatore <> 0 Then cursor.IDOperator.Value = filter.IDOperatore
                    // If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                    if (filter.IDPersona != 0)
                    {
                        cursor.IDChiamato.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        cursor.NomeChiamato.Value = filter.Nominativo + "%";
                        cursor.NomeChiamato.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        cursor.ANumero.Value = filter.Numero + "%";
                        cursor.ANumero.Operator = OP.OP_LIKE;
                    }

                    if (filter.StatoConversazione.HasValue)
                    {
                        switch (filter.StatoConversazione)
                        {
                            case 0: // In Attesa
                                {
                                    cursor.StatoChiamata.ValueIn(new object[] { StatoChiamataRegistrata.Sconosciuto, StatoChiamataRegistrata.Composizione });
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor.StatoChiamata.ValueIn(new object[] { StatoChiamataRegistrata.InCorso });
                                    break;
                                }

                            default:
                                {
                                    cursor.StatoChiamata.ValueIn(new object[] { StatoChiamataRegistrata.AgganciatoChiamante, StatoChiamataRegistrata.AgganciatoChiamato, StatoChiamataRegistrata.Errore, StatoChiamataRegistrata.NonRisposto, StatoChiamataRegistrata.Rifiutata });
                                    break;
                                }
                        }
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        // cursor2.ContextType.Value = filter.TipoContesto
                        // cursor2.ContextID.Value = filter.IDContesto
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                    }

                    cursor.DataInizio.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = filter.IgnoreRights;
                    // cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RITIRATA
                    while (cursor.Read() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        // items.Add(cursor2.Item)
                        AddActivities(items, cursor.Item);
                    }
                }
            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, ChiamataRegistrata c)
            {
                var action = new CustomerCalls.StoricoAction();
                action.Data = c.DataInizio;
                action.IDOperatore = DBUtils.GetID(Sistema.Users.KnownUsers.SystemUser, 0);
                action.NomeOperatore = Sistema.Users.KnownUsers.SystemUser.Nominativo;
                action.IDCliente = c.IDChiamato;
                action.NomeCliente = c.NomeChiamato;
                action.Note = "Chiamata da: " + Sistema.Formats.FormatNumber(c.DaNumero) + " a " + Sistema.Formats.FormatNumber(c.ANumero) + DMD.Strings.vbCrLf + "Esito: " + Enum.GetName(typeof(EsitoChiamataRegistrata), c.EsitoChiamata) + "Inizio: " + Sistema.Formats.FormatUserDateTime(c.DataInizio);
                if (c.DataRisposta.HasValue)
                    action.Note += DMD.Strings.vbCrLf + "Risposta: " + Sistema.Formats.FormatUserDateTime(c.DataRisposta);
                if (c.DataRisposta.HasValue && c.DataInizio.HasValue)
                {
                    action.Note += " (Attesa: " + Sistema.Formats.FormatDurata((long?)(c.DataRisposta.Value - c.DataInizio.Value).TotalSeconds) + ")";
                    action.Attesa = 0d;
                }

                if (c.DataFine.HasValue)
                {
                    action.Note += DMD.Strings.vbCrLf + "Fine: " + Sistema.Formats.FormatUserDateTime(c.DataFine);
                    if (c.DataRisposta.HasValue)
                    {
                        action.Durata = (c.DataFine.Value - c.DataInizio.Value).TotalSeconds;
                        action.Attesa = (c.DataRisposta.Value - c.DataInizio.Value).TotalSeconds;
                        action.Note += " (Durata conversazione: " + Sistema.Formats.FormatDurata((long?)(c.DataFine.Value - c.DataRisposta.Value).TotalSeconds) + ")";
                    }
                    else if (c.DataInizio.HasValue)
                    {
                        action.Durata = (c.DataFine.Value - c.DataInizio.Value).TotalSeconds;
                        action.Attesa = action.Durata;
                        action.Note += " (Attesa: " + Sistema.Formats.FormatDurata((long?)action.Durata) + ")";
                    }
                }

                action.Scopo = "";
                action.NumeroOIndirizzo = c.ANumero;
                switch (c.EsitoChiamata)
                {
                    case EsitoChiamataRegistrata.Risposto:
                        {
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            break;
                        }

                    case EsitoChiamataRegistrata.NonRisposto:
                        {
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            break;
                        }
                }

                action.DettaglioEsito = c.EsitoChiamataEx;
                action.Tag = c;
                action.Ricevuta = false;
                switch (c.StatoChiamata)
                {
                    case StatoChiamataRegistrata.Sconosciuto:
                    case StatoChiamataRegistrata.InCorso:
                    case StatoChiamataRegistrata.Composizione:
                        {
                            action.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA;
                            break;
                        }

                    default:
                        {
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            break;
                        }
                }

                col.Add(action);
            }

            /// <summary>
            /// Tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("ChiamataRegistrata", "Chiamata Registrata");
            }
        }
    }
}