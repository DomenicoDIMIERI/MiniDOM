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
        /// Aggiunge le commissioni
        /// </summary>
        /// <remarks></remarks>
        public class StoricoHandlerCommissioni 
            : CustomerCalls.StoricoHandlerBase
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public StoricoHandlerCommissioni()
            {
            }

            /// <summary>
            /// Aggiunge le commissioni allo storico
            /// </summary>
            /// <param name="items"></param>
            /// <param name="filter"></param>
            protected override void AggiungiInternal(CCollection<CustomerCalls.StoricoAction> items, CustomerCalls.CRMFindFilter filter)
            {
                int cnt = 0;
                using (var cursor1 = new CommissioneCursor())
                {
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    if (filter.Dal.HasValue)
                    {
                        cursor1.OraRientro.Value = filter.Dal.Value;
                        cursor1.OraRientro.Operator = OP.OP_GT;
                        if (filter.Al.HasValue)
                        {
                            cursor1.OraRientro.Value1 = filter.Al.Value;
                            cursor1.OraRientro.Operator = OP.OP_BETWEEN;
                        }
                    }
                    else if (filter.Al.HasValue)
                    {
                        cursor1.OraRientro.Value = filter.Al.Value;
                        cursor1.OraRientro.Operator = OP.OP_LE;
                    }

                    if (filter.IDPersona != 0)
                    {
                        cursor1.IDPersonaIncontrata.Value = filter.IDPersona;
                    }
                    else if (!string.IsNullOrEmpty(filter.Nominativo))
                    {
                        cursor1.NomePersonaIncontrata.Value = filter.Nominativo;
                        cursor1.NomePersonaIncontrata.Operator = OP.OP_LIKE;
                    }

                    if (filter.IDOperatore != 0)
                        cursor1.IDOperatore.Value = filter.IDOperatore;
                    if (filter.IDPuntoOperativo != 0)
                        cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (!string.IsNullOrEmpty(filter.Scopo))
                    {
                        cursor1.Motivo.Value = filter.Scopo ;
                        cursor1.Motivo.Operator = OP.OP_LIKE;
                    }

                    if (filter.StatoConversazione.HasValue)
                    {
                        switch (filter.StatoConversazione)
                        {
                            case 0: // In Attesa
                                {
                                    cursor1.StatoCommissione.Value = StatoCommissione.NonIniziata;
                                    break;
                                }

                            case 1: // in corso
                                {
                                    cursor1.StatoCommissione.ValueIn(new object[] { StatoCommissione.Iniziata, StatoCommissione.Rimandata });
                                    break;
                                }

                            default:
                                {
                                    cursor1.StatoCommissione.ValueIn(new object[] { StatoCommissione.Annullata, StatoCommissione.Completata });
                                    break;
                                }
                        }
                    }

                    if (!string.IsNullOrEmpty(filter.Numero))
                    {
                        // cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                        cursor1.NomeAzienda.Value = filter.Numero  ;
                        cursor1.NomeAzienda.Operator = OP.OP_LIKE;
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        cursor1.ContextType.Value = filter.TipoContesto;
                        cursor1.ContextID.Value = filter.IDContesto;
                    }

                    cursor1.OraRientro.SortOrder = SortEnum.SORT_DESC;
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    while (cursor1.Read() && (!filter.nMax.HasValue || cnt < filter.nMax))
                    {
                        cnt += 1;
                        AddActivities(items, cursor1.Item);
                    }
                }
            }

            private void AddActivities(CCollection<CustomerCalls.StoricoAction> col, Commissione commissione)
            {
                var action = new CustomerCalls.StoricoAction();
                action.Data = commissione.AssegnataIl;
                action.IDOperatore = commissione.IDAssegnataDa;
                action.NomeOperatore = commissione.NomeAssegnataDa;
                action.IDCliente = commissione.IDPersonaIncontrata;
                action.NomeCliente = commissione.NomePersonaIncontrata;
                action.Note = "Commissione Programmata. Motivo: <b>" + commissione.Motivo + "</b>";
                switch (Strings.LCase(commissione.Presso) ?? "")
                {
                    case "residenza":
                    case "domicilio":
                    case "posto di lavoro":
                        {
                            action.Note += " presso <b>" + commissione.Presso + "</b>" + DMD.Strings.vbNewLine;
                            break;
                        }

                    default:
                        {
                            if (commissione.Azienda is object)
                            {
                                action.Note += " presso <b>" + commissione.NomeAzienda + "</b>" + DMD.Strings.vbNewLine;
                            }
                            else if (commissione.Luoghi.Count > 0)
                            {
                                action.Note += " presso <b>" + commissione.Luoghi[0].ToString() + "</b>" + DMD.Strings.vbNewLine;
                            }
                            else
                            {
                                action.Note += " presso <b><i>Indirizzo non specificato</i></b>" + DMD.Strings.vbNewLine;
                            }

                            break;
                        }
                }

                action.Note += " Assegnata a <b>" + commissione.NomeAssegnataA + "</b> per il <b>" + Sistema.Formats.FormatUserDate(commissione.DataPrevista) + "</b>";
                action.Scopo = commissione.Motivo;
                action.NumeroOIndirizzo = DMD.Strings.TrimTo(commissione.Luoghi.ToString(), 255);
                action.Esito = CustomerCalls.EsitoChiamata.OK;
                action.DettaglioEsito = "Commissione Assegnata";
                action.Durata = 0d;
                action.Attesa = 0d;
                action.Tag = commissione;
                action.ActionSubID = (int)StatoCommissione.NonIniziata;
                action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                col.Add(action);
                foreach (var u in commissione.Uscite)
                {
                    if (
                           u.Stato == ObjectStatus.OBJECT_VALID
                        && u.Uscita is object 
                        && u.Uscita.Stato == ObjectStatus.OBJECT_VALID
                        )
                    {
                        var uscita = u.Uscita;
                        action = new CustomerCalls.StoricoAction();
                        action.Data = uscita.OraUscita;
                        action.IDOperatore = uscita.IDOperatore;
                        action.NomeOperatore = uscita.NomeOperatore;
                        action.IDCliente = commissione.IDPersonaIncontrata;
                        action.NomeCliente = commissione.NomePersonaIncontrata;
                        action.Note = "Uscita Iniziata -> Commissione: <b>" + commissione.Motivo + "</b>"; // presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
                        action.Scopo = commissione.Motivo;
                        action.NumeroOIndirizzo = DMD.Strings.TrimTo(commissione.Luoghi.ToString(), 255);
                        action.Esito = CustomerCalls.EsitoChiamata.OK;
                        action.DettaglioEsito = "Commissione Iniziata";
                        action.Durata = 0d;
                        action.Attesa = 0d;
                        action.Tag = commissione;
                        action.ActionSubID = (int)StatoCommissione.Iniziata;
                        action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                        col.Add(action);
                        if (uscita.OraRientro.HasValue)
                        {
                            action = new CustomerCalls.StoricoAction();
                            action.Data = uscita.OraRientro;
                            action.IDOperatore = uscita.IDOperatore;
                            action.NomeOperatore = uscita.NomeOperatore;
                            action.IDCliente = commissione.IDPersonaIncontrata;
                            action.NomeCliente = commissione.NomePersonaIncontrata;
                            action.Note = "Uscita Terminata -> Commissione: <b>" + commissione.Motivo + "</b>"; // presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
                            action.Scopo = commissione.Motivo;
                            action.NumeroOIndirizzo = DMD.Strings.TrimTo(commissione.Luoghi.ToString(), 255);
                            switch (u.StatoCommissione)
                            {
                                case StatoCommissione.Annullata:
                                    {
                                        action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                                        action.DettaglioEsito = "Commissione Annullata: " + u.DescrizioneEsito;
                                        action.ActionSubID = (int)StatoCommissione.Annullata;
                                        action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                                        break;
                                    }

                                case StatoCommissione.Completata:
                                    {
                                        action.Esito = CustomerCalls.EsitoChiamata.OK;
                                        action.DettaglioEsito = "Commissione Completata: " + u.DescrizioneEsito;
                                        action.ActionSubID = (int)StatoCommissione.Completata;
                                        action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                                        break;
                                    }

                                case StatoCommissione.Rimandata:
                                    {
                                        action.Esito = CustomerCalls.EsitoChiamata.NESSUNA_RISPOSTA;
                                        action.DettaglioEsito = "Commissione Completata: " + u.DescrizioneEsito;
                                        action.ActionSubID = (int)StatoCommissione.Rimandata;
                                        action.StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO;
                                        break;
                                    }
                            }

                            action.Durata = 0d;
                            action.Attesa = 0d;
                            action.Tag = commissione;
                            col.Add(action);
                        }
                    }
                }

                // If (commissione.StatoCommissione >= StatoCommissione.Iniziata) Then
                // action = New StoricoAction
                // action.Data = commissione.OraUscita
                // action.IDOperatore = commissione.IDOperatore
                // action.NomeOperatore = commissione.NomeOperatore
                // action.IDCliente = commissione.IDPersonaIncontrata
                // action.NomeCliente = commissione.NomePersonaIncontrata
                // action.Note = "Commissione Iniziata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
                // action.Scopo = commissione.Motivo
                // action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Commissione Iniziata"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = commissione
                // action.ActionSubID = StatoCommissione.Iniziata
                // action.StatoConversazione = StatoConversazione.CONCLUSO
                // col.Add(action)
                // End If

                // Select Case (commissione.StatoCommissione)
                // Case StatoCommissione.Completata
                // action = New StoricoAction
                // action.Data = commissione.OraRientro
                // action.IDOperatore = commissione.IDOperatore
                // action.NomeOperatore = commissione.NomeOperatore
                // action.IDCliente = commissione.IDPersonaIncontrata
                // action.NomeCliente = commissione.NomePersonaIncontrata
                // action.Note = "Commissione Completata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
                // action.Scopo = commissione.Motivo
                // action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                // action.Esito = EsitoChiamata.OK
                // action.DettaglioEsito = "Commissione Completata"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = commissione
                // action.ActionSubID = StatoCommissione.Completata
                // action.StatoConversazione = StatoConversazione.CONCLUSO
                // col.Add(action)
                // Case StatoCommissione.Annullata
                // action = New StoricoAction
                // action.Data = IIf(commissione.OraRientro.HasValue, commissione.OraRientro, IIf(commissione.OraUscita.HasValue, commissione.OraUscita, commissione.AssegnataIl))
                // action.IDOperatore = commissione.IDOperatore
                // action.NomeOperatore = commissione.NomeOperatore
                // action.IDCliente = commissione.IDPersonaIncontrata
                // action.NomeCliente = commissione.NomePersonaIncontrata
                // action.Note = "Commissione Annullata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
                // action.Scopo = commissione.Motivo
                // action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                // action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                // action.DettaglioEsito = "Commissione Annullata"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = commissione
                // action.ActionSubID = StatoCommissione.Annullata
                // action.StatoConversazione = StatoConversazione.CONCLUSO
                // col.Add(action)
                // Case StatoCommissione.Rimandata
                // action = New StoricoAction
                // action.Data = IIf(commissione.OraRientro.HasValue, commissione.OraRientro, IIf(commissione.OraUscita.HasValue, commissione.OraUscita, commissione.AssegnataIl))
                // action.IDOperatore = commissione.IDOperatore
                // action.NomeOperatore = commissione.NomeOperatore
                // action.IDCliente = commissione.IDPersonaIncontrata
                // action.NomeCliente = commissione.NomePersonaIncontrata
                // action.Note = "Commissione Rimandata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
                // action.Scopo = commissione.Motivo
                // action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                // action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                // action.DettaglioEsito = "Commissione Rimandata"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = commissione
                // action.ActionSubID = StatoCommissione.Rimandata
                // action.StatoConversazione = StatoConversazione.CONCLUSO
                // col.Add(action)
                // Case StatoCommissione.NonIniziata

                // Case StatoCommissione.Iniziata
                // action = New StoricoAction
                // action.Data = IIf(commissione.OraUscita.HasValue, commissione.OraUscita, commissione.AssegnataIl)
                // action.IDOperatore = commissione.IDOperatore
                // action.NomeOperatore = commissione.NomeOperatore
                // action.IDCliente = commissione.IDPersonaIncontrata
                // action.NomeCliente = commissione.NomePersonaIncontrata
                // action.Note = "Commissione In Corso. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
                // action.Scopo = commissione.Motivo
                // action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                // action.Esito = EsitoChiamata.ALTRO
                // action.DettaglioEsito = "Commissione In Corso"
                // action.Durata = 0
                // action.Attesa = 0
                // action.Tag = commissione
                // action.ActionSubID = StatoCommissione.Iniziata
                // action.StatoConversazione = StatoConversazione.INCORSO
                // col.Add(action)
                // Case Else
                // Debug.Assert(False)
                // End Select

            }

            /// <summary>
            /// Prepara i tipi supportati
            /// </summary>
            /// <param name="items"></param>
            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("Commissione", "Commissione");
            }

           
        }
    }
}