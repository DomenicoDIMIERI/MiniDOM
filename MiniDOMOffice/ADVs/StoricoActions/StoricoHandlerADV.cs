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
using static minidom.CustomerCalls;


namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Aggiunge la campagne ADV allo storico del crm
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerADV 
            : StoricoHandlerBase
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public StoricoHandlerADV()
            {
            }


            /// <summary>
            /// Prepara la collezione dei tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CRisultatoCampagna", "Campagna ADV");
            }

            /// <summary>
            /// Aggiungi gli elementi
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor = new ADV.CRisultatoCampagnaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor.DataEsecuzione.Value = filter.Dal.Value;
                        cursor.DataEsecuzione.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor.DataEsecuzione.Value1 = filter.Al.Value;
                            cursor.DataEsecuzione.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor.DataEsecuzione.Value = filter.Al.Value;
                        cursor.DataEsecuzione.Operator = OP.OP_LE;
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
                        cursor.IDDestinatario.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        cursor.NomeDestinatario.Value = filter.Nominativo + "%";
                        cursor.NomeDestinatario.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        cursor.IndirizzoDestinatario.Value = filter.Numero + "%";
                        cursor.IndirizzoDestinatario.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        // cursor2.TipoRichiesta.Value = filter.Scopo & "%"
                        // cursor2.TipoRichiesta.Operator = OP.OP_LIKE
                    }

                    if (filter.StatoConversazione.HasValue)
                    {
                        // Select Case filter.StatoConversazione
                        // Case 0 'In Attesa
                        // cursor.StatoMessaggio.ValueIn(New Object() {ADV.StatoMessaggioCampagna.InPreparazione, ADV.StatoMessaggioCampagna.ProntoPerLaSpedizione})
                        // Case 1 'in corso
                        // cursor.StatoMessaggio.ValueIn(New Object() {ADV.StatoMessaggioCampagna.Inviato})
                        // Case Else
                        // cursor.StatoMessaggio.ValueIn(New Object() {ADV.StatoMessaggioCampagna.Letto, ADV.StatoMessaggioCampagna.RifiutatoDalDestinatario, ADV.StatoMessaggioCampagna.RifiutatoDalVettore})
                        // End Select
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        // cursor2.ContextType.Value = filter.TipoContesto
                        // cursor2.ContextID.Value = filter.IDContesto
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor.NomeCampagna.Value = filter.Scopo + "%";
                        cursor.NomeCampagna.Operator = OP.OP_LIKE;
                    }

                    cursor.DataEsecuzione.SortOrder = SortEnum.SORT_DESC;
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

            private string FormatTipoCampagna(ADV.TipoCampagnaPubblicitaria t)
            {
                switch (t)
                {
                    case ADV.TipoCampagnaPubblicitaria.eMail:
                        {
                            return "e-Mail";
                        }

                    case ADV.TipoCampagnaPubblicitaria.Fax:
                        {
                            return "Fax";
                        }

                    case ADV.TipoCampagnaPubblicitaria.NonImpostato:
                        {
                            return "?";
                        }

                    case ADV.TipoCampagnaPubblicitaria.Quotidiani:
                        {
                            return "Quotidiani";
                        }

                    case ADV.TipoCampagnaPubblicitaria.SMS:
                        {
                            return "SMS";
                        }

                    case ADV.TipoCampagnaPubblicitaria.Web:
                        {
                            return "Web";
                        }

                    default:
                        {
                            return "??";
                        }
                }
            }

            private void AddActivities(CCollection<StoricoAction> col, ADV.CRisultatoCampagna res)
            {
                //TODO AddActivities RisultatoCampagna (creare più elementi in funzione della data di esecuzione, di invio e dell'esito)
                var action = new StoricoAction();
                action.Data = res.DataEsecuzione;
                action.IDOperatore = res.CreatoDaId;
                action.NomeOperatore = res.CreatoDa.Nominativo;
                action.IDCliente = res.IDDestinatario;
                action.NomeCliente = res.NomeDestinatario;
                action.Note = "Campagna ADV <b>" + res.NomeCampagna + "</b> - Eseguita";
                action.Scopo = res.NomeCampagna;
                action.NumeroOIndirizzo = res.IndirizzoDestinatario;
                action.Esito = EsitoChiamata.OK;
                action.DettaglioEsito = "Eseguita";
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = res;
                action.Ricevuta = false;
                switch (res.StatoMessaggio)
                {
                    case ADV.StatoMessaggioCampagna.InPreparazione:
                    case ADV.StatoMessaggioCampagna.ProntoPerLaSpedizione:
                        {
                            action.StatoConversazione = StatoConversazione.INATTESA;
                            break;
                        }

                    default:
                        {
                            action.StatoConversazione = StatoConversazione.CONCLUSO;
                            break;
                        }
                }

                col.Add(action);
            }


            
        }
    }
}