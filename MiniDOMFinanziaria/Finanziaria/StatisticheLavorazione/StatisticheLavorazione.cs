using System;
using DMD;
using DMD.XML;
using minidom;
using minidom.internals;
using static minidom.Sistema;
using static minidom.CustomerCalls;

namespace minidom
{
    namespace internals
    {
        [Serializable]
        public class CStatisticheLavorazioneClass
        {
            public Finanziaria.StatisticaLavorazione GetStatistiche(Anagrafica.CPersonaFisica p, DateTime? fromDate, DateTime? toDate)
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                return GetStatistiche(new[] { DBUtils.GetID(p) }, fromDate, toDate)[0];
            }

            public Finanziaria.StatisticaLavorazione[] GetStatistiche(int[] idPersone, DateTime? fromDate, DateTime? toDate)
            {
                var ret = new CKeyCollection<Finanziaria.StatisticaLavorazione>();
                Finanziaria.StatisticaLavorazione item = null;
                foreach (int pid in idPersone)
                {
                    item = new Finanziaria.StatisticaLavorazione();
                    item.IDCliente = pid;
                    ret.Add("K" + pid, item);
                }

                SyncPersone(ret);
                SyncContatti(ret, fromDate, toDate);
                SyncConsulenze(ret, fromDate, toDate);
                SyncPratiche(ret, fromDate, toDate);
                foreach (var currentItem in ret)
                {
                    item = currentItem;
                    item.Update();
                }

                // Correzione
                var retArr = ret.ToArray();
                if (retArr is null)
                    retArr = new Finanziaria.StatisticaLavorazione[] { };
                return retArr;
            }

            private void SyncPersone(CKeyCollection<Finanziaria.StatisticaLavorazione> items)
            {
                var arrID = DMD.Arrays.Empty<int>();
                foreach (Finanziaria.StatisticaLavorazione item in items)
                {
                    int argitem = item.IDCliente;
                    int i = DMD.Arrays.BinarySearch(arrID, argitem);
                    item.IDCliente = argitem;
                    if (i < 0)
                        arrID = DMD.Arrays.InsertSorted(arrID, item.IDCliente);
                }

                if (arrID.Length == 0)
                    return;

                using (var cursor = new Anagrafica.CPersonaCursor())
                {
                    cursor.ID.ValueIn(arrID);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                    cursor.PageSize = 1000;
                    while (!cursor.EOF())
                    {
                        Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)cursor.Item;
                        var item = items.GetItemByKey("K" + DBUtils.GetID(p));
                        item.SetCliente(p);
                        item.IconURL = p.IconURL;
                        item.NomeCliente = p.Nominativo;
                        cursor.MoveNext();
                    }

                }
            }

            private void SyncContatti(CKeyCollection<Finanziaria.StatisticaLavorazione> items, DateTime? fromDate, DateTime? toDate)
            {
                var arrID = DMD.Arrays.Empty<int>();
                foreach (Finanziaria.StatisticaLavorazione item in items)
                {
                    int argitem = item.IDCliente;
                    int i = DMD.Arrays.BinarySearch(arrID, argitem);
                    item.IDCliente = argitem;
                    if (i < 0)
                        arrID = DMD.Arrays.InsertSorted(arrID, item.IDCliente);
                }

                if (arrID.Length == 0)
                    return;

                using (var cursor = new CContattoUtenteCursor())
                {
                    cursor.IDPersona.ValueIn(arrID);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.Data.Between(fromDate, toDate);
                    cursor.PageSize = 1000;
                    while (!cursor.EOF())
                    {
                        var c = cursor.Item;
                        var item = items.GetItemByKey("K" + c.IDPersona);
                        if (c is CVisita)
                        {
                            item.Visite.Add(c);
                        }
                        else
                        {
                            item.Contatti.Add(c);
                        }

                        cursor.MoveNext();
                    }

                }
            }

            private void SyncConsulenze(CKeyCollection<Finanziaria.StatisticaLavorazione> items, DateTime? fromDate, DateTime? toDate)
            {
                var arrID = DMD.Arrays.Empty<int>();
                foreach (Finanziaria.StatisticaLavorazione item in items)
                {
                    int argitem = item.IDCliente;
                    int i = DMD.Arrays.BinarySearch(arrID, argitem);
                    item.IDCliente = argitem;
                    if (i < 0)
                        arrID = DMD.Arrays.InsertSorted(arrID, item.IDCliente);
                }

                if (arrID.Length == 0)
                    return;

                using (var cursor = new Finanziaria.CQSPDConsulenzaCursor())
                {
                    cursor.IDCliente.ValueIn(arrID);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.DataConsulenza.Between(fromDate, toDate);
                    cursor.PageSize = 1000;
                    while (!cursor.EOF())
                    {
                        var c = cursor.Item;
                        var item = items.GetItemByKey("K" + c.IDCliente);
                        item.Consulenze.Add(c);
                        cursor.MoveNext();
                    }

                }
            }

            private void SyncPratiche(CKeyCollection<Finanziaria.StatisticaLavorazione> items, DateTime? fromDate, DateTime? toDate)
            {
                var arrID = DMD.Arrays.Empty<int>();
                foreach (Finanziaria.StatisticaLavorazione item in items)
                {
                    int argitem = item.IDCliente;
                    int i = DMD.Arrays.BinarySearch(arrID, argitem);
                    item.IDCliente = argitem;
                    if (i < 0)
                        arrID = DMD.Arrays.InsertSorted(arrID, item.IDCliente);
                }

                if (arrID.Length == 0)
                    return;

                using (var cursor = new Finanziaria.CPraticheCQSPDCursor())
                {
                    cursor.IDCliente.ValueIn(arrID);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.CreatoIl.Between(fromDate, toDate);
                    cursor.PageSize = 1000;
                    while (!cursor.EOF())
                    {
                        var c = cursor.Item;
                        var item = items.GetItemByKey("K" + c.IDCliente);
                        item.DataConsulenza = DMD.DateUtils.GetDatePart(item.GetDataPrimaConsulenza());
                        DateTime? dCar = c.DataCaricamento;
                        if (DMD.DateUtils.Compare(item.DataConsulenza, dCar) >= 0)
                            item.Pratiche.Add(c);
                        cursor.MoveNext();
                    }

                }
            }
        }
    }

    public partial class Finanziaria
    {
        private static CStatisticheLavorazioneClass m_StatisticheLavorazione = null;

        public static CStatisticheLavorazioneClass StatisticheLavorazione
        {
            get
            {
                if (m_StatisticheLavorazione is null)
                    m_StatisticheLavorazione = new CStatisticheLavorazioneClass();
                return m_StatisticheLavorazione;
            }
        }
    }
}