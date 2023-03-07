using System;
using System.Collections;

namespace minidom
{
    public partial class Finanziaria
    {

        public class CQSPDCalendarProviderRichAppr 
            : Sistema.BaseCalendarActivitiesProvider
        {
            private class ItemInfo : IComparable
            {
                public int ID;
                public int PO;
                public int IDRichAppr;
                public bool Privilaged;

                public ItemInfo(CRichiestaApprovazione c)
                {
                    DMDObject.IncreaseCounter(this);
                    ID = DBUtils.GetID(c);
                    PO = c.IDPuntoOperativo;
                    IDRichAppr = DBUtils.GetID(c);
                    if (c.MotivoRichiesta is null)
                    {
                        Privilaged = true;
                    }
                    else
                    {
                        Privilaged = c.MotivoRichiesta.Privilegiato;
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

            public CQSPDCalendarProviderRichAppr()
            {
                RichiesteApprovazione.ItemCreated += handleItem;
                RichiesteApprovazione.ItemDeleted += handleItem;
                RichiesteApprovazione.ItemModified += handleItem;
                m_List = null;
            }

            ~CQSPDCalendarProviderRichAppr()
            {
                RichiesteApprovazione.ItemCreated -= handleItem;
                RichiesteApprovazione.ItemDeleted -= handleItem;
                RichiesteApprovazione.ItemModified -= handleItem;
            }

            private void handleItem(object sender, ItemEventArgs e)
            {
                CRichiestaApprovazione c = (CRichiestaApprovazione)e.Item;
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

            private void AddToList(CRichiestaApprovazione c)
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

            private bool Check(CRichiestaApprovazione p)
            {
                if (p.Stato != ObjectStatus.OBJECT_VALID)
                    return false;
                return p.StatoRichiesta == StatoRichiestaApprovazione.PRESAINCARICO | p.StatoRichiesta == StatoRichiestaApprovazione.ATTESA;
            }

            private CCollection<ItemInfo> List
            {
                get
                {
                    lock (listLock)
                    {
                        if (m_List is null)
                        {
                            var cursor = new CRichiestaApprovazioneCursor();
                            var tmp = new ArrayList();
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.StatoRichiesta.ValueIn(new[] { StatoRichiestaApprovazione.ATTESA, StatoRichiestaApprovazione.PRESAINCARICO });
                            cursor.IgnoreRights = true;
                            while (!cursor.EOF())
                            {
                                tmp.Add(new ItemInfo(cursor.Item));
                                cursor.MoveNext();
                            }

                            cursor.Dispose();
                            m_List = (ItemInfo[])tmp.ToArray(typeof(ItemInfo));
                            // If (Me.m_List IsNot Nothing) Then Array.Sort(Me.m_List)
                        }

                        return new CCollection<ItemInfo>(m_List);
                    }
                }
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
                var cnt = default(int);
                if (GruppoAutorizzatori.Members.Contains(Sistema.Users.CurrentUser))
                {
                    cnt = ContaTutto();
                }
                else if (GruppoSupervisori.Members.Contains(Sistema.Users.CurrentUser))
                {
                    cnt = ContaNonProvilegiati();
                }

                if (cnt > 0)
                {
                    act = new Sistema.CCalendarActivity();
                    ((Sistema.ICalendarActivity)act).SetProvider(this);
                    act.Descrizione = "Ci sono " + cnt + " richieste di autorizzazione";
                    act.DataInizio = DMD.DateUtils.Now();
                    act.GiornataIntera = true;
                    act.Categoria = "Urgente";
                    act.Flags = Sistema.CalendarActivityFlags.IsAction;
                    act.IconURL = "/widgets/images/activities/richiesteautorizzazione.png";
                    act.Note = "CQSPDCalendarProviderRICHAUT";
                    act.Stato = ObjectStatus.OBJECT_VALID;
                    ret.Add(act);
                }

                return ret;
            }

            private int ContaTutto()
            {
                return List.Count;
            }

            private int ContaNonProvilegiati()
            {
                int cnt = 0;
                foreach (ItemInfo item in List)
                {
                    if (item.Privilaged == true)
                        cnt += 1;
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
                    return "CQSPDCALPROVRICHA";
                }
            }
        }
    }
}