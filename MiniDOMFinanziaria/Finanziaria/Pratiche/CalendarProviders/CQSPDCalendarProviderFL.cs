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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        public class CQSPDCalendarProviderFL 
            : Sistema.BaseCalendarActivitiesProvider
        {
            private class ItemInfo : IComparable
            {
                public int ID;
                public int PO;
                public StatoFinestraLavorazione StatoFinestra;
                public StatoOfferteFL StatoContatto;
                public int IDBustaPaga;
                public int IDRichiestaCS;
                public StatoOfferteFL StatoCS;
                public int IDStudioF;
                public StatoOfferteFL StatoStudioF;
                public StatoOfferteFL StatoPratica;

                public ItemInfo(FinestraLavorazione c)
                {
                    //DMDObject.IncreaseCounter(this);
                    ID = DBUtils.GetID(c, 0);
                    PO = c.IDPuntoOperativo;
                    StatoFinestra = c.StatoFinestra;
                    StatoContatto = c.StatoContatto;
                    IDBustaPaga = c.IDBustaPaga;
                    IDRichiestaCS = c.IDRichiestaCertificato;
                    StatoCS = c.StatoRichiestaCertificato;
                    IDStudioF = c.IDStudioDiFattibilita;
                    StatoStudioF = c.StatoStudioFattibilita;
                    StatoPratica = c.GetMaxStatoPratica();
                }

                public ItemInfo(int id)
                {
                    //DMDObject.IncreaseCounter(this);
                    ID = id;
                    PO = 0;
                }

                public int CompareTo(ItemInfo obj)
                {
                    return DMD.Arrays.Compare(this.ID, obj.ID);
                }

                int IComparable.CompareTo(object obj)
                {
                    return CompareTo((ItemInfo)obj);
                }

                //~ItemInfo()
                //{
                //    DMDObject.DecreaseCounter(this);
                //}
            }

            private object listLock = new object();
            private ItemInfo[] m_List;
            private string[] statiValidi = new[] { "Attesa Busta Paga", "Busta Paga Ricevuta", "Proporre Delega", "Rinnovabile" };

            /// <summary>
            /// Costruttore
            /// </summary>
            public CQSPDCalendarProviderFL()
            {
                minidom.Finanziaria.FinestreDiLavorazione.ItemCreated += handleItem;
                minidom.Finanziaria.FinestreDiLavorazione.ItemDeleted += handleItem;
                minidom.Finanziaria.FinestreDiLavorazione.ItemModified += handleItem;
                m_List = null;
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CQSPDCalendarProviderFL()
            {
                minidom.Finanziaria.FinestreDiLavorazione.ItemCreated -= handleItem;
                minidom.Finanziaria.FinestreDiLavorazione.ItemDeleted -= handleItem;
                minidom.Finanziaria.FinestreDiLavorazione.ItemModified -= handleItem;
            }

            private void handleItem(object sender, ItemEventArgs e)
            {
                FinestraLavorazione c = (FinestraLavorazione)e.Item;
                lock (listLock)
                {
                    if (Check(c))
                    {
                        AddToList(c);
                    }
                    else
                    {
                        RemoveFromList(DBUtils.GetID(c, 0));
                    }
                }
            }

            private bool IsInList(int id)
            {
                int localBinarySearch() { var argitem = new ItemInfo(id); var ret = DMD.Arrays.BinarySearch(m_List, argitem); return ret; }

                return DMD.Arrays.Len(m_List) > 0 && localBinarySearch() >= 0;
            }

            private void AddToList(FinestraLavorazione c)
            {
                int i = -1;
                var info = new ItemInfo(c);
                if (DMD.Arrays.Len(m_List) > 0)
                    i = DMD.Arrays.BinarySearch(m_List, info);
                if (i >= 0)
                {
                    m_List[i] = info;
                }
                else
                {
                    m_List = DMD.Arrays.Append(m_List, info);
                    Array.Sort(m_List);
                }
            }

            private void RemoveFromList(int id)
            {
                int i = -1;
                if (DMD.Arrays.Len(m_List) > 0)
                {
                    var argitem = new ItemInfo(id);
                    i = DMD.Arrays.BinarySearch(m_List, argitem);
                }

                if (i >= 0)
                    m_List = DMD.Arrays.RemoveAt(m_List, i);
            }

            private bool Check(FinestraLavorazione p)
            {
                return p.Stato == ObjectStatus.OBJECT_VALID && p.StatoFinestra == StatoFinestraLavorazione.Aperta && p.IDBustaPaga != 0 && p.IDStudioDiFattibilita == 0;
            }

            private CCollection<ItemInfo> List
            {
                get
                {
                    lock (listLock)
                    {
                        if (m_List is null)
                        {
                            var tmp = new ArrayList();
                            tmp.AddRange(GetRinnovabili());
                            tmp.AddRange(GetInLavorazione());
                            m_List = (ItemInfo[])tmp.ToArray(typeof(ItemInfo));
                        }
                        // If (Me.m_List IsNot Nothing) Then Array.Sort(Me.m_List)
                        else
                        {
                            m_List = DMD.Arrays.Empty<ItemInfo>();
                        }

                        return new CCollection<ItemInfo>(m_List);
                    }
                }
            }

            private CCollection<ItemInfo> GetInLavorazione()
            {
                var ret = new CCollection<ItemInfo>();
                var cursor = new FinestraLavorazioneCursor();
                cursor = new FinestraLavorazioneCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta;
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    ret.Add(new ItemInfo(cursor.Item));
                    cursor.MoveNext();
                }

                cursor.Dispose();
                return ret;
            }

            private CCollection<ItemInfo> GetRinnovabili()
            {
                var ret = new CCollection<ItemInfo>();
                var di = DMD.DateUtils.DateAdd(DateTimeInterval.Day, -Configuration.GiorniAnticipoRifin, DMD.DateUtils.ToDay());
                var df = DMD.DateUtils.DateAdd(DateTimeInterval.Day, Configuration.GiorniAnticipoRifin, DMD.DateUtils.ToDay());
                // Inseriamo tutte le finestre di lavorazione precedenti
                var cursor = new FinestraLavorazioneCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta;
                cursor.Flags.Value = FinestraLavorazioneFlags.Rinnovo;
                cursor.Flags.Operator = Databases.OP.OP_ALLBITAND;
                cursor.DataInizioLavorabilita.Between(di, df); // = Calendar.DateAdd(DateTimeInterval.Day, 30, Calendar.ToDay)  'Between(di.Value, df.Value)
                cursor.IgnoreRights = true;
                // cursor.DataInizioLavorabilita.Operator = OP.OP_LE

                while (!cursor.EOF())
                {
                    ret.Add(new ItemInfo(cursor.Item));
                    cursor.MoveNext();
                }

                cursor.Dispose();
                return ret;
            }

            public override CCollection<Sistema.CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0)
            {
                return new CCollection<Sistema.CActivePerson>();
            }

            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                return new CCollection<Sistema.ICalendarActivity>();
            }

            public override CCollection<Sistema.ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate)
            {
                return new CCollection<Sistema.ICalendarActivity>();
            }

            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                var ret = new CCollection<Sistema.ICalendarActivity>();
                Sistema.CCalendarActivity act;
                if (minidom.CustomerCalls.CRM.CRMGroup.Members.Contains(Sistema.Users.CurrentUser))
                {
                    int cnt = ContaRinnovabili();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " clienti che possono rinnovare";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/clientirinnovaili.png";
                        act.Note = "CQSPDCalendarProviderFLRinnovabili";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    cnt = ContaClientiInteressati();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " clienti interessati che devono inviare la busta paga";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/bustapagaricevere.png";
                        act.Note = "CQSPDCalendarProviderFLBustePagaRic";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    cnt = ContaCSDaRichiedere();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " Certificati di Stipendio/CUD da richiedere ";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/bustapagavalutare.png";
                        act.Note = "CQSPDCalendarProviderFLCSRichiedere";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    cnt = ContaStudiDaEffettuare();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " studi di fattibilità da effettuare";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/bustapagavalutare.png";
                        act.Note = "CQSPDCalendarProviderFLSFFare";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    cnt = ContaSFInCorso();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " studi di fattibilità in sospeso";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/sfinsospeso.png";
                        act.Note = "CQSPDCalendarProviderFLSFSospeso";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    cnt = ContaPraticheDaCaricare();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " pratiche da caricare";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/pratichecaricare.png";
                        act.Note = "CQSPDCalendarProviderFLPraticheCAR";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }
                }

                return ret;
            }

            private int ContaSFInCorso()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.StatoFinestra == StatoFinestraLavorazione.Aperta && item.StatoStudioF != 0 && (item.StatoStudioF == StatoOfferteFL.InLavorazione || item.StatoStudioF == StatoOfferteFL.Sconosciuto) && item.StatoPratica == StatoOfferteFL.Sconosciuto && u.Uffici.HasOffice(item.PO))


                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaRinnovabili()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.StatoFinestra == StatoFinestraLavorazione.NonAperta && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaPraticheDaCaricare()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.IDStudioF != 0 && item.StatoStudioF == StatoOfferteFL.Liquidata && item.StatoPratica == StatoOfferteFL.Sconosciuto && item.StatoFinestra == StatoFinestraLavorazione.Aperta && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaBustePagaDaValutare()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.IDBustaPaga != 0 && item.IDStudioF == 0 && item.StatoFinestra == StatoFinestraLavorazione.Aperta && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaClientiInteressati()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.IDBustaPaga == 0 && item.StatoContatto == StatoOfferteFL.Liquidata && item.StatoFinestra == StatoFinestraLavorazione.Aperta && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaCSDaRichiedere()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.IDBustaPaga != 0 && item.IDRichiestaCS == 0 && item.IDStudioF == 0 && item.StatoFinestra == StatoFinestraLavorazione.Aperta && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaStudiDaEffettuare()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.IDRichiestaCS != 0 && item.IDStudioF == 0 && item.StatoFinestra == StatoFinestraLavorazione.Aperta && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaBustePagaDaRicevere()
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.IDBustaPaga == 0 && item.StatoFinestra == StatoFinestraLavorazione.Aperta)
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            public override void SaveActivity(Sistema.ICalendarActivity item, bool force = false)
            {
            }

            public override void DeleteActivity(Sistema.ICalendarActivity item, bool force = false)
            {
            }

            public override string UniqueName
            {
                get
                {
                    return "CQSPDCALPROVFL";
                }
            }
        }
    }
}