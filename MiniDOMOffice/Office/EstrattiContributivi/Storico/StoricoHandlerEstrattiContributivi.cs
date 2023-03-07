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
        /// Costruttore
        /// </summary>
        public StoricoHandlerEstrattiContributivi()
        {
        }

        /// <summary>
        /// Aggiunge gli estratti contributivi
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerEstrattiContributivi 
            : CustomerCalls.StoricoHandlerBase
        {


            /// <summary>
            /// Aggiunge gli estratti contributivi allo storico
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<CustomerCalls.StoricoAction> items, CustomerCalls.CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor1 = new EstrattiContributiviCursor())
                {
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor1.DataRichiesta.Value = filter.Dal.Value;
                        cursor1.DataRichiesta.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor1.DataRichiesta.Value1 = filter.Al.Value;
                            cursor1.DataRichiesta.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor1.DataRichiesta.Value = filter.Al.Value;
                        cursor1.DataRichiesta.Operator = OP.OP_LE;
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
                        cursor1.IDRichiedente.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        // cursor1..Value = filter.Scopo & "%"
                        // cursor1.Motivo.Operator = OP.OP_LIKE
                        cursor1.ID.Value = 0;
                    }

                    if (filter.StatoConversazione.HasValue)
                    {
                        switch (filter.StatoConversazione)
                        {
                            case 0: // In Attesa
                                {
                                    cursor1.StatoRichiesta.ValueIn(new[] { StatoEstrattoContributivo.DaRichiedere, StatoEstrattoContributivo.Sospeso });
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor1.StatoRichiesta.ValueIn(new[] { StatoEstrattoContributivo.Richiesto, StatoEstrattoContributivo.Sospeso });
                                    break;
                                }

                            default:
                                {
                                    cursor1.StatoRichiesta.ValueIn(new[] { StatoEstrattoContributivo.Errore, StatoEstrattoContributivo.Evaso });
                                    break;
                                }
                        }
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        // cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                        cursor1.ID.Value = 0;
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        cursor1.SourceType.Value = filter.TipoContesto;
                        cursor1.SourceID.Value = filter.IDContesto;
                    }

                    cursor1.DataRichiesta.SortOrder = SortEnum.SORT_DESC;
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    while (cursor1.Read() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        AddActivities(items, cursor1.Item);
                    }
                }

            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, EstrattoContributivo commissione)
            {
                var action = new CustomerCalls.StoricoAction();
                action.Data = commissione.DataRichiesta;
                action.IDOperatore = commissione.IDRichiedente;
                action.NomeOperatore = commissione.NomeRichiedente;
                action.IDCliente = commissione.IDCliente;
                action.NomeCliente = commissione.NomeCliente;
                action.Note = "Richiesta Etratto Contributivo -> Registrata da " + commissione.NomeRichiedente + "</b>";
                action.Scopo = "";
                action.NumeroOIndirizzo = "";
                action.Esito = CustomerCalls.EsitoChiamata.OK;
                action.DettaglioEsito = "Richiesta Registrata";
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = commissione;
                action.ActionSubID = (int)StatoEstrattoContributivo.Richiesto;
                action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                col.Add(action);
                if (commissione.StatoRichiesta >= StatoEstrattoContributivo.Assegnato)
                {
                    action = new CustomerCalls.StoricoAction();
                    action.Data = commissione.DataAssegnazione;
                    action.IDOperatore = commissione.IDAssegnatoA;
                    action.NomeOperatore = commissione.NomeAssegnatoA;
                    action.IDCliente = commissione.IDCliente;
                    action.NomeCliente = commissione.NomeCliente;
                    action.Note = "Richiesta Etratto Contributivo -> Presa in carico da " + commissione.NomeAssegnatoA + "</b>";
                    action.Scopo = "";
                    action.NumeroOIndirizzo = "";
                    action.Esito = CustomerCalls.EsitoChiamata.OK;
                    action.DettaglioEsito = "Richiesta Presa In Carico";
                    action.Durata = 0d;
                    action.Attesa = 0d;
                    action.Tag = commissione;
                    action.ActionSubID = (int)StatoEstrattoContributivo.Assegnato;
                    action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                    col.Add(action);
                }

                switch (commissione.StatoRichiesta)
                {
                    case StatoEstrattoContributivo.Evaso:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = commissione.DataCompletamento;
                            action.IDOperatore = commissione.IDAssegnatoA;
                            action.NomeOperatore = commissione.NomeAssegnatoA;
                            action.IDCliente = commissione.IDCliente;
                            action.NomeCliente = commissione.NomeCliente;
                            action.Note = "Richiesta Etratto Contributivo -> Richiesta Evasa da " + commissione.NomeAssegnatoA + "</b>";
                            action.Scopo = "";
                            action.NumeroOIndirizzo = "";
                            action.Esito = CustomerCalls.EsitoChiamata.OK;
                            action.DettaglioEsito = "Richiesta Evasa";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = commissione;
                            action.ActionSubID = (int)StatoEstrattoContributivo.Evaso;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoEstrattoContributivo.Errore:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = commissione.DataCompletamento;
                            action.IDOperatore = commissione.IDAssegnatoA;
                            action.NomeOperatore = commissione.NomeAssegnatoA;
                            action.IDCliente = commissione.IDCliente;
                            action.NomeCliente = commissione.NomeCliente;
                            action.Note = "Richiesta Etratto Contributivo -> Richiesta Rigettata da " + commissione.NomeAssegnatoA + "</b>";
                            action.Scopo = "";
                            action.NumeroOIndirizzo = "";
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Richiesta Presa In Carico";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = commissione;
                            action.ActionSubID = (int)StatoEstrattoContributivo.Errore;
                            action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                            col.Add(action);
                            break;
                        }

                    case StatoEstrattoContributivo.Sospeso:
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = commissione.DataCompletamento;
                            action.IDOperatore = commissione.IDAssegnatoA;
                            action.NomeOperatore = commissione.NomeAssegnatoA;
                            action.IDCliente = commissione.IDCliente;
                            action.NomeCliente = commissione.NomeCliente;
                            action.Note = "Richiesta Etratto Contributivo -> Richiesta Sospesa da " + commissione.NomeAssegnatoA + "</b>";
                            action.Scopo = "";
                            action.NumeroOIndirizzo = "";
                            action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                            action.DettaglioEsito = "Richiesta Sospesa";
                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = commissione;
                            action.ActionSubID = (int)StatoEstrattoContributivo.Sospeso;
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
                items.Add("EstrattoContributivo", "Estratto Contributivo");
            }

          
        }
    }
}