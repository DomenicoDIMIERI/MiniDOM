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
    public partial class CustomerCalls
    {

        /// <summary>
        /// Passaggi di stato
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerTasks 
            : StoricoHandlerBase
        {

            /// <summary>
            /// Aggiunge i passaggi di stato alla lista
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor = new Anagrafica.TaskLavorazioneCursor())
                {

                    if (filter.IDPersona != 0)
                    {
                        cursor.IDCliente.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        cursor.NomeCliente.Operator = OP.OP_LIKE;
                        cursor.NomeCliente.Value = filter.Nominativo + "%";
                    }

                    if (filter.IDOperatore != 0)
                        cursor.IDAssegnatoA.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Contenuto))
                    {
                        // cursor.Note.Value = filter.Contenuto
                        // cursor.Note.Operator = OP.OP_LIKE
                    }

                    if (!string.IsNullOrEmpty(filter.Etichetta))
                    {
                        // cursor..Value = filter.Etichetta & "%"
                        // cursor.NomeIndirizzo.Operator = OP.OP_LIKE
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        // cursor.NumeroOIndirizz.Value = filter.Numero & "%"
                        // cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                    }

                   
                    if (filter.Dal.HasValue)
                    {
                        cursor.DataAssegnazione.Value = filter.Dal.Value;
                        cursor.DataAssegnazione.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor.DataAssegnazione.Value1 = filter.Al.Value;
                            cursor.DataAssegnazione.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor.DataAssegnazione.Value = filter.Al.Value;
                        cursor.DataAssegnazione.Operator = OP.OP_LE;
                    }

                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        // cursor.Scopo.Value = filter.Scopo & "%"
                        // cursor.Scopo.Operator = OP.OP_LIKE
                    }

                    if (!string.IsNullOrEmpty(filter.DettaglioEsito))
                    {
                        // cursor.DettaglioEsito.Value = filter.DettaglioEsito & "%"
                        // cursor.DettaglioEsito.Operator = OP.OP_LIKE
                    }
                    // If (filter.Esito.HasValue) Then cursor.Esito.Value = filter.Esito.Value
                    if (filter.IDContesto.HasValue)
                    {
                        // cursor.Contesto.Value = filter.TipoContesto
                        // cursor.IDContesto.Value = filter.IDContesto
                    }

                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    // cursor.Data.SortOrder = SortEnum.SORT_DESC
                    cursor.IgnoreRights = filter.IgnoreRights;
                    // If (filter.StatoConversazione.HasValue) Then cursor.StatoConversazione.Value = filter.StatoConversazione

                    cursor.DataAssegnazione.SortOrder = SortEnum.SORT_DESC;
                    // If (filter.IDPersona = 0) AndAlso (cursor.Data.IsSet = False) Then
                    // cursor.Data.Value = Calendar.DateAdd(DateTimeInterval.Month, -3, Now)
                    // cursor.Data.Operator = OP.OP_GE
                    // End If

                    // If (filter.IDPersona = 0) AndAlso (cursor.Data.IsSet = False) Then
                    // dbSQL = Strings.Replace(dbSQL, "tbl_Telefonate", "tbl_TelefonateQuick")
                    // 'conn = CRM.StatsDB
                    // End If
                    if (!string.IsNullOrEmpty(filter.TipoOggetto) && filter.TipoOggetto != "TaskLavorazione")
                        cursor.WhereClauses *= DBCursorField.False;


                    while (cursor.Read() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        var item = cursor.Item;
                        AddActivities(items, item);
                    }
                    // While Not cursor.EOF AndAlso cnt < filter.nMax
                    // cnt += 1
                    // 'items.Add(cursor.Item)
                    // Me.AddActivities(items, cursor.Item)
                    // cursor.MoveNext()
                    // End While
                }
            }

            private void AddActivities(CCollection<StoricoAction> col, Anagrafica.TaskLavorazione task)
            {
                var action = new StoricoAction();
                action.Data = task.DataAssegnazione;
                action.IDOperatore = task.IDAssegnatoA;
                action.NomeOperatore = "";
                if (task.AssegnatoDa is object)
                    action.NomeOperatore = task.AssegnatoDa.Nominativo;
                action.IDCliente = task.IDCliente;
                action.NomeCliente = task.NomeCliente;
                action.Note = "Passaggio di Stato: ";
                if (task.StatoAttuale is object)
                    action.Note = action.Note + DMD.Strings.Combine(task.StatoAttuale.Descrizione, task.StatoAttuale.Descrizione2, " - ");
                action.Note = DMD.Strings.Combine(action.Note, task.Note, DMD.Strings.vbCrLf);
                action.Scopo = "";
                action.NumeroOIndirizzo = task.StatoAttuale.Nome;
                action.Esito = EsitoChiamata.OK;
                action.DettaglioEsito = task.StatoAttuale.Descrizione;
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = task;
                action.Ricevuta = false;
                action.StatoConversazione = StatoConversazione.INCORSO;
                action.Attachment = null;
                col.Add(action);
            }

            /// <summary>
            /// Prepara i tipi di oggetto supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("TaskLavorazione", "Lavorazione Cliente");
            }
        }
    }
}