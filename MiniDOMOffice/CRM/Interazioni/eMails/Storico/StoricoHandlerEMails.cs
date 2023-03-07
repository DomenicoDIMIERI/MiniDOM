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
        /// Aggiunge le email inviate o ricevute
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerEMails 
            : CustomerCalls.StoricoHandlerBase
        {

            /// <summary>
            /// Aggiunge le email allo storico
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<CustomerCalls.StoricoAction> items, CustomerCalls.CRMFindFilter filter)
            {
                using (var cursor = new MailMessageCursor())
                {
                    int cnt = 0;

                    if (filter.IDOperatore != 0)
                        cursor.ID.Value = 0;
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Contenuto))
                    {
                        cursor.ID.Value = 0;
                    }

                    if (!string.IsNullOrEmpty(filter.Etichetta))
                    {
                        cursor.ID.Value = 0;
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        // cursor.Address.Value = filter.Numero
                        cursor.ID.Value = 0;
                    }

                    if (filter.Dal.HasValue || filter.Al.HasValue)
                    {
                        cursor.DeliveryDate.Between(filter.Dal, filter.Al);
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor.Subject.Value = filter.Scopo;
                        cursor.Subject.Operator = OP.OP_LIKE;
                    }
                    // If (filter.Esito.HasValue) Then cursor.StatoSegnalazione.Value = IIf( filter.Esito.Value, 
                    // If (filter.IDContesto.HasValue) Then
                    // cursor.Contesto.Value = filter.TipoContesto
                    // cursor.IDContesto.Value = filter.IDContesto
                    // End If
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    // cursor.Data.SortOrder = SortEnum.SORT_DESC
                    cursor.IgnoreRights = filter.IgnoreRights;
                    cursor.DeliveryDate.SortOrder = SortEnum.SORT_DESC;
                    if (filter.IDPersona != 0)
                    {
                        // cursor.IDPersona.Value = filter.IDPersona
                        var p = Anagrafica.Persone.GetItemById(filter.IDPersona);
                        var arr = DMD.Arrays.Empty<string>();
                        if (p is object)
                        {
                            foreach (Anagrafica.CContatto c in p.Recapiti)
                            {
                                if (!string.IsNullOrEmpty(c.Valore) && (c.Validated.HasValue == false || c.Validated.Value == true))
                                {
                                    switch (Strings.LCase(c.Tipo) ?? "")
                                    {
                                        case "pec":
                                        case "e-mail":
                                            {
                                                string argitem = Strings.LCase(c.Valore);
                                                if (DMD.Arrays.BinarySearch(arr, argitem) < 0)
                                                {
                                                    arr = DMD.Arrays.InsertSorted(arr, Strings.LCase(c.Valore));
                                                }

                                                break;
                                            }
                                    }
                                }
                            }
                        }

                        if (arr.Length == 0)
                        {
                            cursor.ID.Value = 0;
                        }
                        else
                        {
                            // cursor.Address.ValueIn(arr)
                            cursor.ID.ValueIn(this.GetIDS(arr));
                        }
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        // cursor.NomePersona.Operator = OP.OP_LIKE
                        // cursor.NomePersona.Value = filter.Nominativo & "%"

                        // cursor.NomeCliente.Value = filter.Nominativo & "%"
                        // cursor.NomeCliente.Operator = OP.OP_LIKE
                        cursor.ID.Value = 0;
                    }

                    while (cursor.Read())
                    {
                        cnt += 1;
                        var item = cursor.Item;
                        AddActivities(items, item);
                    }
                }
            }

            private int[] GetIDS(string[] arr)
            {
                var ret = new List<int>(arr.Length + 1);
                using (var cursor = new MailAddressCursor())
                {
                    cursor.Address.ValueIn(arr);
                    while (cursor.Read())
                    {
                        var a = cursor.Item;
                        if (a.MessageID != 0)
                            ret.Add(a.MessageID);
                    }

                }

                return ret.ToArray();
            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, MailMessage res)
            {
                var action = new CustomerCalls.StoricoAction();
                int actionSubID = 0;
                action.Data = res.DeliveryDate;
                // action.IDOperatore = res.
                action.NomeOperatore = res.AccountName;
                // action.IDCliente = res.IDCliente
                action.NomeCliente = ""; // res.To.ToString
                action.Note = res.Body;
                action.Scopo = res.Subject;
                action.NumeroOIndirizzo = res.To.ToString();
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = res;
                action.Ricevuta = false;
                action.ActionSubID = actionSubID;
                actionSubID += 1;
                action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                action.Esito = CustomerCalls.EsitoChiamata.OK;
                action.DettaglioEsito = "";
                col.Add(action);
            }

            /// <summary>
            /// Restituisce true se l'oggetto é supportato
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            protected override bool IsSupportedTipoOggetto(CustomerCalls.CRMFindFilter filter)
            {
                return string.IsNullOrEmpty(filter.TipoOggetto) || filter.TipoOggetto == "MailMessage";
            }

            /// <summary>
            /// Prepara i tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("MailMessage", "e-Mail (App)");
            }
        }
    }
}