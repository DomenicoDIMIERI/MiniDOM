using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Aggiunge gli stati di lavorazione delle pratiche
    /// </summary>
    /// <remarks></remarks>
        public class StoricoHandlerPratiche
            : StoricoHandlerBase
        {
            private CKeyCollection<CPraticaCQSPD> GetPratiche(CRMFindFilter filter)
            {
                var pratiche = new CKeyCollection<CPraticaCQSPD>();
                using (var cursor1 = new CPraticheCQSPDCursor()) { 
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    // cursor1.StatoContatto.MacroStato = Nothing
                    if (filter.IDPersona != 0)
                        cursor1.IDCliente.Value = filter.IDPersona;
                    if (filter.IDPuntoOperativo != 0)
                        cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    // If filter.Scopo <> "" Then cursor1.StatoContatto.Note = filter.Scopo

                    if (filter.Esito.HasValue)
                    {
                        switch (filter.Esito)
                        {
                            case 0: // In Attesa
                                {
                                    break;
                                }

                            case 1: // in corso
                                {
                                    break;
                                }

                            default:
                                {
                                    cursor1.IDStatoAttuale.ValueIn(new[] { DBUtils.GetID(StatiPratica.StatoLiquidato), DBUtils.GetID(StatiPratica.StatoArchiviato), DBUtils.GetID(StatiPratica.StatoAnnullato) });
                                    break;
                                }
                        }
                    }

                    if (filter.Numero != "")
                    {
                        // cursor1.Luogo.Value = filter.Numero & "%"
                        // cursor1.Luogo.Operator = OP.OP_LIKE
                        cursor1.NumeroEsterno.Value = filter.Numero;
                    }

                    if (filter.IDContesto.HasValue)
                    {
                        // cursor1.id.Value = filter.TipoContesto
                        // cursor1.ContextID.Value = filter.IDContesto
                    }
                    // cursor1.DataDecorrenza.SortOrder = SortEnum.SORT_DESC
                    cursor1.IgnoreRights = filter.IgnoreRights;
                    cursor1.StatoGenerico.Inizio = filter.Dal;
                    cursor1.StatoGenerico.Fine = filter.Al;
                    if (filter.IDOperatore != 0)
                        cursor1.StatoGenerico.IDOperatore = filter.IDOperatore;
                    while (!cursor1.EOF())
                    {
                        pratiche.Add("K" + DBUtils.GetID(cursor1.Item), cursor1.Item);
                        cursor1.MoveNext();
                    }
                }
                 

                return pratiche;
            }

            private int[] GetIDPRatiche(CKeyCollection<CPraticaCQSPD> pratiche)
            {
                var ret = DMD.Arrays.Empty<int>();
                foreach (CPraticaCQSPD p in pratiche)
                {
                    int argitem = DBUtils.GetID(p);
                    if (DMD.Arrays.BinarySearch(ret, argitem) < 0)
                    {
                        ret = DMD.Arrays.InsertSorted(ret, DBUtils.GetID(p));
                    }
                }

                return ret;
            }

            private object GetStatiLav(CKeyCollection<CPraticaCQSPD> pratiche, CRMFindFilter filter)
            {
                var statiLav = new CCollection<CStatoLavorazionePratica>();
                var arr = GetIDPRatiche(pratiche);
                if (arr.Length == 0)
                    return statiLav;
                using (var cursor2 = new CStatiLavorazionePraticaCursor())
                {
                    cursor2.IDPratica.ValueIn(arr);
                    cursor2.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor2.IgnoreRights = true;
                    if (filter.IDOperatore != 0)
                        cursor2.IDOperatore.Value = filter.IDOperatore;
                    if (filter.Dal.HasValue || filter.Al.HasValue)
                        cursor2.Data.Between(filter.Dal, filter.Al);
                    while (!cursor2.EOF())
                    {
                        statiLav.Add(cursor2.Item);
                        cursor2.MoveNext();
                    }

                }
                statiLav.Sort();
                return statiLav;
            }

            protected override void AggiungiInternal(CCollection<StoricoAction> items, CRMFindFilter filter)
            {
                var pratiche = GetPratiche(filter);
                if (pratiche.Count == 0)
                    return;
                CCollection<CStatoLavorazionePratica> statiLav = (CCollection<CStatoLavorazionePratica>)GetStatiLav(pratiche, filter);
                foreach (CPraticaCQSPD p in pratiche)
                {
                    var col = new CStatiLavorazionePraticaCollection();
                    foreach (var stLav in statiLav)
                    {
                        if (stLav.IDPratica == DBUtils.GetID(p))
                            col.Add(stLav);
                    }

                    p.SetStatiDiLavorazione(col);
                    AddActivities(items, p);
                }
            }

            private void AddActivities(CCollection<StoricoAction> col, CPraticaCQSPD rich)
            {
                StoricoAction action;
                foreach (CStatoLavorazionePratica stl in rich.StatiDiLavorazione)
                {
                    action = new StoricoAction();
                    action.Data = Sistema.Formats.ToDate(stl.Data);
                    action.IDOperatore = stl.IDOperatore;
                    action.NomeOperatore = stl.NomeOperatore;
                    action.IDCliente = rich.IDCliente;
                    action.NomeCliente = rich.NominativoCliente;
                    string nP = rich.NumeroEsterno;
                    if (string.IsNullOrEmpty(nP))
                        nP = rich.NumeroPratica;
                    action.Note = "Pratica N° <b>" + nP + "</b> -> <i>" + stl.DescrizioneStato + "</i> : " + stl.Note;
                    action.Scopo = stl.DescrizioneStato;
                    action.NumeroOIndirizzo = rich.NumeroEsterno;
                    action.Esito = EsitoChiamata.OK;
                    action.DettaglioEsito = stl.DescrizioneStato;
                    action.Durata = 0;
                    action.Attesa = 0;
                    action.Tag = rich;
                    action.ActionSubID = DBUtils.GetID(stl);
                    action.StatoConversazione = StatoConversazione.CONCLUSO;
                    col.Add(action);
                }
            }

            protected override void FillSupportedTypes(CKeyCollection<string> items)
            {
                items.Add("CPraticaCQSPD", "Pratica");
            }

            public StoricoHandlerPratiche()
            {
            }
        }
    }
}