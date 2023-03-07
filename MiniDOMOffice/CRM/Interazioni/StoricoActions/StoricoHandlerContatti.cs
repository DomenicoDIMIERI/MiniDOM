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
        /// Aggiunge telefonate, visite, ecc
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerContatti
            : StoricoHandlerBase
        {

            /// <summary>
            /// Aggiunge i contatti utente allo storico
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                int cnt = 0;

                using (var cursor = new CContattoUtenteCursor())
                {
                    if (filter.IDPersona != 0)
                    {
                        // cursor.IDPersona.Value = filter.IDPersona
                        cursor.WhereClauses *= (cursor.Field("IDPersona").EQ(filter.IDPersona) + cursor.Field("IDPerContoDi").EQ(filter.IDPersona));
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        // cursor.NomePersona.Operator = OP.OP_LIKE
                        // cursor.NomePersona.Value = filter.Nominativo & "%"

                        cursor.WhereClauses *= (cursor.Field("NomePersona").IsLike(filter.Nominativo) + cursor.Field("NomePerContoDi").IsLike(filter.Nominativo));
                    }

                    if (filter.IDOperatore != 0)
                        cursor.IDOperatore.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Contenuto))
                    {
                        // cursor.Note.Value = filter.Contenuto
                        // cursor.Note.Operator = OP.OP_LIKE
                    }

                    if (!string.IsNullOrEmpty(filter.Etichetta))
                    {
                        cursor.NomeIndirizzo.Value = filter.Etichetta;
                        cursor.NomeIndirizzo.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        cursor.NumeroOIndirizzo.Value = filter.Numero;
                        cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.TipoOggetto))
                        cursor.ClassName.Value = filter.TipoOggetto;
                    if (filter.Dal.HasValue)
                    {
                        cursor.Data.Value = filter.Dal.Value;
                        cursor.Data.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor.Data.Value1 = filter.Al.Value;
                            cursor.Data.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor.Data.Value = filter.Al.Value;
                        cursor.Data.Operator = OP.OP_LE;
                    }

                    if (DMD.RunTime.TestFlag(filter.Flags, 1) ^ DMD.RunTime.TestFlag(filter.Flags, 2))
                        cursor.Ricevuta.Value = DMD.RunTime.TestFlag(filter.Flags, 1);
                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor.Scopo.Value = filter.Scopo;
                        cursor.Scopo.Operator = OP.OP_LIKE;
                    }

                    if (!string.IsNullOrEmpty(filter.DettaglioEsito))
                    {
                        cursor.DettaglioEsito.Value = filter.DettaglioEsito;
                        cursor.DettaglioEsito.Operator = OP.OP_LIKE;
                    }

                    if (filter.Esito.HasValue)
                        cursor.Esito.Value = (EsitoChiamata?)filter.Esito.Value;
                    if (filter.IDContesto.HasValue)
                    {
                        cursor.Contesto.Value = filter.TipoContesto;
                        cursor.IDContesto.Value = filter.IDContesto;
                    }

                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    // cursor.Data.SortOrder = SortEnum.SORT_DESC
                    cursor.IgnoreRights = filter.IgnoreRights;
                    if (filter.StatoConversazione.HasValue)
                        cursor.StatoConversazione.Value = (StatoConversazione?)filter.StatoConversazione;
                    cursor.Data.SortOrder = SortEnum.SORT_DESC;
                    // If (filter.IDPersona = 0) AndAlso (cursor.Data.IsSet = False) Then
                    // cursor.Data.Value = Calendar.DateAdd(DateTimeInterval.Month, -3, Now)
                    // cursor.Data.Operator = OP.OP_GE
                    // End If

                    // If (filter.IDPersona = 0) AndAlso (cursor.Data.IsSet = False) Then
                    // dbSQL = Strings.Replace(dbSQL, "tbl_Telefonate", "tbl_TelefonateQuick")
                    // 'conn = CRM.StatsDB
                    // End If
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

            private void AddActivities(CCollection<StoricoAction> col, CContattoUtente contatto)
            {
                var action = new StoricoAction();
                action.Data = contatto.Data;
                action.IDOperatore = contatto.IDOperatore;
                action.NomeOperatore = contatto.NomeOperatore;
                action.IDCliente = contatto.IDPersona;
                action.NomeCliente = contatto.NomePersona + ((contatto.IDPerContoDi != 0)? " per conto di " + contatto.NomePerContoDi : "");
                action.Note = contatto.Note;
                action.Scopo = contatto.Scopo;
                action.NumeroOIndirizzo = contatto.NumeroOIndirizzo;
                if (string.IsNullOrEmpty(action.NumeroOIndirizzo) && contatto is CVisita)
                {
                    {
                        var withBlock = ((CVisita)contatto).Luogo;
                        action.NumeroOIndirizzo = DMD.Strings.Combine(withBlock.Nome, withBlock.ToString(), DMD.Strings.vbNewLine);
                    }
                }

                action.Esito = contatto.Esito;
                action.DettaglioEsito = contatto.DettaglioEsito;
                action.Durata = contatto.Durata;
                action.Attesa = contatto.Attesa;
                action.Tag = contatto;
                action.Ricevuta = contatto.Ricevuta;
                action.StatoConversazione = contatto.StatoConversazione;
                action.Attachment = (contatto.Attachments.Count > 0)? contatto.Attachments[0] : null;
                col.Add(action);
            }

            /// <summary>
            /// Prepara i tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CTelefonata", "Telefonata");
                items.Add("CVisita", "Visita");
                items.Add("CAppunto", "Appunto");
                items.Add("SMSMessage", "SMS");
                items.Add("FaxDocument", "Fax");
                items.Add("CEMailMessage", "e-mail");
                items.Add("CTelegramma", "Telegramma");
            }
        }
    }
}