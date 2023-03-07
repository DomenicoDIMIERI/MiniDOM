using System;
using System.Collections;
using DMD;
using minidom;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CQSPDCalendarProviderStPrat : Sistema.BaseCalendarActivitiesProvider
        {
            private class ItemInfo : IComparable
            {
                public int ID;
                public int PO;
                public int Anno;
                public int Mese;
                public StatoOfferteFL StatoContatto;

                public ItemInfo(CPraticaCQSPD c)
                {
                    DMDObject.IncreaseCounter(this);
                    ID = DBUtils.GetID(c);
                    PO = c.IDPuntoOperativo;
                    string param = DMD.Strings.Trim(c.StatoDiLavorazioneAttuale.Params);
                    try
                    {
                        var n = DMD.Strings.Split(param, "|");
                        Anno = DMD.Integers.ValueOf(n[0]);
                        Mese = DMD.Integers.ValueOf(n[1]);
                    }
                    catch (Exception ex)
                    {
                        Anno = DMD.DateUtils.ToDay().Year;
                        Mese = DMD.DateUtils.ToDay().Month;
                    }
                }

                public ItemInfo(int id)
                {
                    DMDObject.IncreaseCounter(this);
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

                ~ItemInfo()
                {
                    DMDObject.DecreaseCounter(this);
                }
            }

            private object listLock = new object();
            private ItemInfo[] m_List;

            public CQSPDCalendarProviderStPrat()
            {
                Pratiche.ItemCreated += handleItem;
                Pratiche.ItemDeleted += handleItem;
                Pratiche.ItemModified += handleItem;
                m_List = null;
            }

            ~CQSPDCalendarProviderStPrat()
            {
                Pratiche.ItemCreated -= handleItem;
                Pratiche.ItemDeleted -= handleItem;
                Pratiche.ItemModified -= handleItem;
            }

            private void handleItem(object sender, ItemEventArgs e)
            {
                CPraticaCQSPD c = (CPraticaCQSPD)e.Item;
                lock (listLock)
                {
                    if (Check(c))
                    {
                        AddToList(c);
                    }
                    else
                    {
                        RemoveFromList(DBUtils.GetID(c));
                    }
                }
            }

            private bool IsInList(int id)
            {
                int localBinarySearch() { var argitem = new ItemInfo(id); var ret = DMD.Arrays.BinarySearch(m_List, argitem); return ret; }

                return DMD.Arrays.Len(m_List) > 0 && localBinarySearch() >= 0;
            }

            private void AddToList(CPraticaCQSPD c)
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
                    m_List = DMD.Arrays.Append(this.m_List, info);
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

            private bool Check(CPraticaCQSPD p)
            {
                return p.Stato == ObjectStatus.OBJECT_VALID && p.IDStatoAttuale == DBUtils.GetID(StatiPratica.StatoPreventivoAccettato);
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
                            tmp.AddRange(GetPreventiviAccettati());
                            m_List = (ItemInfo[])tmp.ToArray(typeof(ItemInfo));
                            // If (Me.m_List IsNot Nothing) Then Array.Sort(Me.m_List)
                        }

                        return new CCollection<ItemInfo>(m_List);
                    }
                }
            }

            private CCollection<ItemInfo> GetPreventiviAccettati()
            {
                var ret = new CCollection<ItemInfo>();
                int gg = Configuration.GiorniAnticipoRifin;
                var di = DMD.DateUtils.DateAdd(DateTimeInterval.Day, -gg, DMD.DateUtils.ToDay());
                var df = DMD.DateUtils.DateAdd(DateTimeInterval.Day, gg, DMD.DateUtils.ToDay());
                // Inseriamo tutte le finestre di lavorazione precedenti
                using (var cursor = new CPraticheCQSPDCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDStatoAttuale.Value = DBUtils.GetID(StatiPratica.StatoPreventivoAccettato);
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        ret.Add(new ItemInfo(cursor.Item));
                        cursor.MoveNext();
                    }
                }

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
                if (!(GruppoConsulenti.Members.Contains(Sistema.Users.CurrentUser) || GruppoAutorizzatori.Members.Contains(Sistema.Users.CurrentUser) || GruppoSupervisori.Members.Contains(Sistema.Users.CurrentUser) || GruppoReferenti.Members.Contains(Sistema.Users.CurrentUser)))
                    return ret;
                int y = DMD.DateUtils.ToDay().Year;
                int m = DMD.DateUtils.ToDay().Month;
                int cnt = ContaPrecedenti(m, y);
                if (cnt > 0)
                {
                    act = new Sistema.CCalendarActivity();
                    ((Sistema.ICalendarActivity)act).SetProvider(this);
                    act.Descrizione = DMD.Strings.JoinW( "<span class=\"red\">Ci sono " , cnt , " pratic" , (cnt > 1)? "he" : "a", " da caricare!</span>");
                    act.DataInizio = DMD.DateUtils.Now();
                    act.GiornataIntera = true;
                    act.Categoria = "Urgente";
                    act.Priorita = ret.Count;
                    act.Flags = Sistema.CalendarActivityFlags.IsAction;
                    act.IconURL = "/widgets/images/activities/clientirinnovaili.png";
                    act.Note = DMD.Strings.JoinW("CQSPDCalendarProviderStPratR" , DMD.Strings.vbCr , y , "|" , m);
                    act.Stato = ObjectStatus.OBJECT_VALID;
                    ret.Add(act);
                }

                for (int i = 0; i <= 11; i++)
                {
                    cnt = ContaPerMese(m, y);
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " pratic" + Sistema.IIF(cnt > 1, "he deliberabili", "a deliberabile") + " il " + Sistema.Formats.Format(DMD.DateUtils.MakeDate(y, m, 1), "MMMM yyyy");
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Urgente";
                        act.Priorita = ret.Count;
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/clientirinnovaili.png";
                        act.Note = "CQSPDCalendarProviderStPratR" + DMD.Strings.vbCr + y + "|" + m;
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    m += 1;
                    if (m > 12)
                    {
                        m = 1;
                        y += 1;
                    }
                }

                return ret;
            }

            private int ContaPerMese(int mese, int anno)
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if (item.Mese == mese && item.Anno == anno && u.Uffici.HasOffice(item.PO))
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

            private int ContaPrecedenti(int m, int y)
            {
                int cnt = 0;
                var u = Sistema.Users.CurrentUser;
                var items = List;
                foreach (ItemInfo item in items)
                {
                    if ((item.Anno < y || item.Anno == y && item.Mese < m) && u.Uffici.HasOffice(item.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }
        }
    }
}