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
        /// Provider delle commissioni per il calendario
        /// </summary>
        public class CommissioniCalProvider 
            : Sistema.BaseCalendarActivitiesProvider
        {
            private class ItemInfo 
                : IComparable, IComparable<ItemInfo>
            {
                /// <summary>
                /// ID della commissione
                /// </summary>
                public int ID;

                /// <summary>
                /// ID del punto operativo associato alla commissione
                /// </summary>
                public int PO;

                /// <summary>
                /// ID dell'operatore associato alla commissione
                /// </summary>
                public int OP;

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="c"></param>
                public ItemInfo(Commissione c)
                {
                    //DMDObject.IncreaseCounter(this);
                    ID = DBUtils.GetID(c, 0);
                    PO = c.IDPuntoOperativo;
                    OP = c.IDAssegnataA;
                }

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="id"></param>
                public ItemInfo(int id)
                {
                    //DMDObject.IncreaseCounter(this);
                    ID = id;
                    PO = 0;
                }

                int IComparable.CompareTo(object obj)
                {
                    return this.CompareTo((ItemInfo)obj);
                }

                public int CompareTo(ItemInfo tmp)
                {
                    return DMD.Arrays.Compare(this.ID, tmp.ID);
                }
            }

            private object listLock = new object();
            private ItemInfo[] m_Items;
            private StatoCommissione[] statiValidi = new StatoCommissione[] { StatoCommissione.NonIniziata, StatoCommissione.Rimandata };

            /// <summary>
            /// Costruttore
            /// </summary>
            public CommissioniCalProvider()
            {
                minidom.Office.Commissioni.ItemCreated += handleItem;
                minidom.Office.Commissioni.ItemDeleted += handleItem;
                minidom.Office.Commissioni.ItemModified += handleItem;
                this.m_Items = null;
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CommissioniCalProvider()
            {
                minidom.Office.Commissioni.ItemCreated -= handleItem;
                minidom.Office.Commissioni.ItemDeleted -= handleItem;
                minidom.Office.Commissioni.ItemModified -= handleItem;
            }

            private bool IsInList(int id)
            {
                int localBinarySearch() { var argitem = new ItemInfo(id); var ret = DMD.Arrays.BinarySearch(this.m_Items, argitem); return ret; }
                return DMD.Arrays.Len(m_Items) > 0 && localBinarySearch() >= 0;
            }

            private void AddToList(Commissione c)
            {
                int i = -1;
                var info = new ItemInfo(c);
                if (DMD.Arrays.Len(InnerItems()) > 0)
                    i = DMD.Arrays.BinarySearch(this.m_Items, info);

                if (i > 0)
                {
                    m_Items[i] = info;
                }
                else
                {
                    this.m_Items = DMD.Arrays.Append(this.m_Items, info);
                    Array.Sort(this.m_Items);
                }
            }

            private void RemoveFromList(int id)
            {
                int i = -1;
                if (DMD.Arrays.Len(InnerItems()) > 0)
                {
                    var argitem = new ItemInfo(id);
                    i = DMD.Arrays.BinarySearch(this.m_Items, argitem);
                }

                if (i >= 0)
                    this.m_Items = DMD.Arrays.RemoveAt(this.m_Items, i);
            }

            private ItemInfo[] InnerItems()
            {
                if (m_Items is null)
                {
                    var tmp = new List<ItemInfo>();
                    using (var cursor1 = new CommissioneCursor())
                    {
                        cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor1.IgnoreRights = true;
                        cursor1.StatoCommissione.ValueIn(statiValidi);
                        while (cursor1.Read())
                        {
                            tmp.Add(new ItemInfo(cursor1.Item));
                        }
                    }
                    m_Items = tmp.ToArray();
                    // If (Me.m_Items IsNot Nothing) Then Array.Sort(Me.m_Items)
                }

                return m_Items;
            }

            private CCollection<ItemInfo> Items
            {
                get
                {
                    lock (listLock)
                        return new CCollection<ItemInfo>(InnerItems());
                }
            }

            private bool IsVisible(Commissione c)
            {
                return c.Stato == ObjectStatus.OBJECT_VALID && DMD.Arrays.IndexOf(statiValidi, c.StatoCommissione) >= 0;
            }

            private void handleItem(object sender, ItemEventArgs e)
            {
                Commissione c = (Commissione)e.Item;
                lock (listLock)
                {
                    if (IsVisible(c))
                    {
                        AddToList(c);
                    }
                    else
                    {
                        RemoveFromList(DBUtils.GetID(c, 0));
                    }
                }
            }

            /// <summary>
            /// Elimina l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public override void DeleteActivity(Sistema.ICalendarActivity item, bool force = false)
            {
                //TODO da fare
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituise le persone attive
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            public override CCollection<Sistema.CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0)
            {
                //TODO da fare
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce l'elenco delle attività pendenti
            /// </summary>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                //TODO da fare
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce l'elenco delle scadenze
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetScadenze(
                                    DateTime? fromDate, 
                                    DateTime? toDate
                                    )
            {
                //TODO da fare
                throw new NotImplementedException();
                //return new CCollection<Sistema.ICalendarActivity>();
            }

            private int ContaCommissioniUfficio()
            {
                if (m_Items is null)
                    return 0;
                var u = Sistema.Users.CurrentUser;
                int cnt = 0;
                foreach (ItemInfo f in m_Items)
                {
                    if (f.PO == 0 || u.Uffici.HasOffice(f.PO))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaCommissioniUtente()
            {
                if (m_Items is null)
                    return 0;
                var u = Sistema.Users.CurrentUser;
                int cnt = 0;
                foreach (ItemInfo f in m_Items)
                {
                    if (f.OP == 0 || f.OP == DBUtils.GetID(u, 0))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            /// <summary>
            /// Restituisce l'elenco delle attività per l'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                lock (listLock)
                {
                    var ret = new CCollection<Sistema.ICalendarActivity>();
                    Sistema.CCalendarActivity act;
                    int cnt = ContaCommissioniUfficio();
                    int cnt1 = ContaCommissioniUtente();
                    if (cnt > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = DMD.Strings.JoinW("Ci sono ", cnt1.ToString(), " commissioni da fare su ", cnt.ToString(), " commissioni per l'ufficio");
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Normale";
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/tuttecommissioni.gif";
                        act.Note = "CommissioniCalToDoList";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Salva l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public override void SaveActivity(Sistema.ICalendarActivity item, bool force = false)
            {
                //TODO da fare
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce il nome univoco del provider
            /// </summary>
            public override string UniqueName
            {
                get
                {
                    return "COMMCALPROV";
                }
            }
        }
    }
}