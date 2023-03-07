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
        /// Provider del calendario
        /// </summary>
        public class CTicketsCalProvider 
            : Sistema.BaseCalendarActivitiesProvider
        {

            /// <summary>
            /// ItemInfo
            /// </summary>
            private class ItemInfo 
                : IComparable, IComparable<ItemInfo>
            {
                /// <summary>
                /// ID del ticket
                /// </summary>
                public int ID;

                /// <summary>
                /// ID del punto operativo
                /// </summary>
                public int PO;

                /// <summary>
                /// Categoria
                /// </summary>
                public string Categoria;

                /// <summary>
                /// Sottocategoria
                /// </summary>
                public string Sottocategoria;

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="c"></param>
                public ItemInfo(CTicket c)
                {
                    //DMDObject.IncreaseCounter(this);
                    ID = DBUtils.GetID(c, 0);
                    PO = c.IDPuntoOperativo;
                    Categoria = c.Categoria;
                    Sottocategoria = c.Sottocategoria;
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

                public int CompareTo(ItemInfo tmp)
                {
                    return DMD.Arrays.Compare(ID, tmp.ID);
                }

                int IComparable.CompareTo(object obj)
                {
                    return this.CompareTo((ItemInfo)obj);
                }


            }

            private object listLock = new object();
            private ItemInfo[] m_Items;
            private readonly TicketStatus[] statiValidi = new[] { TicketStatus.APERTO, TicketStatus.INLAVORAZIONE, TicketStatus.RIAPERTO, TicketStatus.SOSPESO };

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketsCalProvider()
            {
                minidom.Office.Tickets.ItemCreated += handleItem;
                minidom.Office.Tickets.ItemDeleted += handleItem;
                minidom.Office.Tickets.ItemModified += handleItem;
                m_Items = null;
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CTicketsCalProvider()
            {
                minidom.Office.Tickets.ItemCreated -= handleItem;
                minidom.Office.Tickets.ItemDeleted -= handleItem;
                minidom.Office.Tickets.ItemModified -= handleItem;
            }

            private bool IsInList(int id)
            {
                int localBinarySearch() { 
                    var argitem = new ItemInfo(id); 
                    var ret = DMD.Arrays.BinarySearch(m_Items, argitem); 
                    return ret; 
                }

                return DMD.Arrays.Len(m_Items) > 0 && localBinarySearch() >= 0;
            }

            private void AddToList(CTicket c)
            {
                int i = -1;
                var info = new ItemInfo(c);
                if (DMD.Arrays.Len(InnerItems()) > 0)
                    i = DMD.Arrays.BinarySearch(m_Items, info);
                if (i > 0)
                {
                    m_Items[i] = info;
                }
                else
                {
                    m_Items = DMD.Arrays.Append(m_Items, info);
                    Array.Sort(m_Items);
                }
            }

            private void RemoveFromList(int id)
            {
                int i = -1;
                if (DMD.Arrays.Len(InnerItems()) > 0)
                {
                    var argitem = new ItemInfo(id);
                    i = DMD.Arrays.BinarySearch(m_Items, argitem);
                }

                if (i >= 0)
                    m_Items = DMD.Arrays.RemoveAt(m_Items, i);
            }

            private ItemInfo[] InnerItems()
            {
                if (m_Items is null)
                {
                    var tmp = new List<ItemInfo>();
                    using (var cursor1 = new CTicketCursor())
                    {
                        cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor1.IgnoreRights = true;
                        cursor1.StatoSegnalazione.ValueIn(statiValidi);
                        while (cursor1.Read())
                        {
                            tmp.Add(new ItemInfo(cursor1.Item));
                        }
                    }

                    m_Items = tmp.ToArray();
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

            private bool IsVisible(CTicket c)
            {
                return c.Stato == ObjectStatus.OBJECT_VALID && DMD.Arrays.IndexOf(statiValidi, c.StatoSegnalazione) >= 0;
            }

            private void handleItem(object sender, ItemEventArgs e)
            {
                CTicket c = (CTicket)e.Item;
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
                //TODO DeleteActivity
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce le persone con ticket aperti
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            public override CCollection<Sistema.CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0)
            {
                //TODO GetActivePersons
                throw new NotImplementedException();
                //return new CCollection<Sistema.CActivePerson>();
            }

            /// <summary>
            /// Restituisce le persone con ticket aperti
            /// </summary>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                //return new CCollection<Sistema.ICalendarActivity>();
                throw new ArgumentNullException();
                //TODO GetPendingActivities
            }

            /// <summary>
            /// Restituisce le scadenze
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate)
            {
                //TODO GetScadenze
                throw new NotImplementedException();
                //return new CCollection<Sistema.ICalendarActivity>();
            }

            private CCollection<ItemInfo> GetAllowedList()
            {
                var u = Sistema.Users.CurrentUser;
                var ret = new CCollection<ItemInfo>();
                foreach (ItemInfo f in Items)
                {
                    var c = TicketCategories.GetItemByName(f.Categoria, f.Sottocategoria);
                    if (c is object)
                    {
                        if (Tickets.Module.UserCanDoAction("list"))
                        {
                            ret.Add(f);
                        }
                        else if (DMD.Booleans.ValueOf(c.NotifyUsers.Contains(u)))
                        {
                            ret.Add(f);
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'elenco delle cose da fare
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                lock (listLock)
                {
                    var ret = new CCollection<Sistema.ICalendarActivity>();
                    Sistema.CCalendarActivity act;
                    var items = GetAllowedList();
                    if (items.Count > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = DMD.Strings.JoinW("Ci sono ", items.Count.ToString(), " richieste di assistenza");
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Normale";
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/tip.gif";
                        act.Note = "CTicketsShowToDo";
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
                throw new NotImplementedException();
                //TODO SaveActivity
            }

            /// <summary>
            /// Restituisce il nome univoco
            /// </summary>
            public override string UniqueName
            {
                get
                {
                    return "OTICKTCALPROVIDER";
                }
            }
        }
    }
}