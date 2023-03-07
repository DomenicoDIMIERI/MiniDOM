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
        /// Provider di oggetti di tipo <see cref="EstrattoContributivo"/>
        /// </summary>
        public class EstrattiContributiviCalProvider 
            : Sistema.BaseCalendarActivitiesProvider
        {
            private class ItemInfo
                : IComparable, IComparable<ItemInfo>
            {
                /// <summary>
                /// ID dell'estratto contributizo
                /// </summary>
                public int ID;

                /// <summary>
                /// ID del punto operativo
                /// </summary>
                public int PO;

                /// <summary>
                /// ID dell'operatore
                /// </summary>
                public int OP;

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="c"></param>
                public ItemInfo(EstrattoContributivo c)
                {
                    //DMDObject.IncreaseCounter(this);
                    ID = DBUtils.GetID(c, 0);
                    PO = c.IDPuntoOperativo;
                    OP = c.IDAssegnatoA;
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

                /// <summary>
                /// Compara due oggetti
                /// </summary>
                /// <param name="tmp"></param>
                /// <returns></returns>
                int CompareTo(ItemInfo tmp)
                {
                    return DMD.Arrays.Compare(this.ID, tmp.ID);
                }

                int IComparable.CompareTo(object obj)
                {
                    return this.CompareTo((ItemInfo)obj);
                }

                //~ItemInfo()
                //{
                //    DMDObject.DecreaseCounter(this);
                //}
            }

            private object listLock = new object();
            private ItemInfo[] m_Items;
            private StatoEstrattoContributivo[] statiValidi = new StatoEstrattoContributivo[] { StatoEstrattoContributivo.Richiesto, StatoEstrattoContributivo.Assegnato, StatoEstrattoContributivo.Sospeso };

            /// <summary>
            /// Costruttore
            /// </summary>
            public EstrattiContributiviCalProvider()
            {
                minidom.Office.EstrattiContributivi.ItemCreated += handleItem;
                minidom.Office.EstrattiContributivi.ItemDeleted += handleItem;
                minidom.Office.EstrattiContributivi.ItemModified += handleItem;
                m_Items = null;
            }

            ~EstrattiContributiviCalProvider()
            {
                minidom.Office.EstrattiContributivi.ItemCreated -= handleItem;
                minidom.Office.EstrattiContributivi.ItemDeleted -= handleItem;
                minidom.Office.EstrattiContributivi.ItemModified -= handleItem;
            }

            private bool IsInList(int id)
            {
                int localBinarySearch() { var argitem = new ItemInfo(id); var ret = DMD.Arrays.BinarySearch(m_Items, argitem); return ret; }

                return DMD.Arrays.Len(m_Items) > 0 && localBinarySearch() >= 0;
            }

            private void AddToList(EstrattoContributivo c)
            {
                int i = -1;
                var info = new ItemInfo(c);
                if (DMD.Arrays.Len(InnerItems()) > 0)
                    i = DMD.Arrays.BinarySearch(this.m_Items, info);

                if (i > 0)
                {
                    this.m_Items[i] = info;
                }
                else
                {
                    this.m_Items = DMD.Arrays.Append(this.m_Items, info);
                    Array.Sort(m_Items);
                }
            }

            private void RemoveFromList(int id)
            {
                int i = -1;
                if (DMD.Arrays.Len(InnerItems()) > 0)
                {
                    var argitem = new ItemInfo(id);
                    i = DMD.Arrays.BinarySearch(InnerItems(), argitem);
                }

                if (i >= 0)
                    this.m_Items = DMD.Arrays.RemoveAt(this.m_Items, i);
            }

            private ItemInfo[] InnerItems()
            {
                if (m_Items is null)
                {
                    var tmp = new ArrayList();

                    using (var cursor1 = new EstrattiContributiviCursor())
                    {
                        cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor1.IgnoreRights = true;
                        cursor1.StatoRichiesta.ValueIn(statiValidi);
                        while (cursor1.Read())
                        {
                            tmp.Add(new ItemInfo(cursor1.Item));
                        }

                    }

                    if (tmp.Count > 0)
                    {
                        m_Items = (ItemInfo[])tmp.ToArray(typeof(ItemInfo));
                    }
                    else
                    {
                        m_Items = DMD.Arrays.Empty<ItemInfo>();
                    }
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

            private bool IsVisible(EstrattoContributivo c)
            {
                return c.Stato == ObjectStatus.OBJECT_VALID && DMD.Arrays.IndexOf(statiValidi, c.StatoRichiesta) >= 0;
            }

            private void handleItem(object sender, ItemEventArgs e)
            {
                EstrattoContributivo c = (EstrattoContributivo)e.Item;
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
                //TODO non implementato DeleteActivity
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce le persone attive
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
            /// Restituisce le operazioni non complete
            /// </summary>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                //TODO GetPendingActivities
                throw new NotImplementedException();
                //return new CCollection<Sistema.ICalendarActivity>();
            }

            /// <summary>
            /// Restituisce le scadenze previste
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

            private int Conta()
            {
                // Dim u As CUser = Sistema.Users.CurrentUser
                // Dim cnt As Integer = 0
                // For Each f As ItemInfo In Me.Items
                // If f.PO = 0 OrElse u.Uffici.HasOffice(f.PO) Then
                // cnt += 1
                // End If
                // Next
                // Return cnt
                return InnerItems().Length;
            }

            /// <summary>
            /// Restituisce la collezione delle attività da fare
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                lock (listLock)
                {
                    var ret = new CCollection<Sistema.ICalendarActivity>();
                    Sistema.CCalendarActivity act;
                    int cnt = Conta();
                    if (cnt > 0 && EstrattiContributivi.GruppoResponsabili.Members.Contains(Sistema.Users.CurrentUser))
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + cnt + " richiesta di estratti contributivi da evadere";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Normale";
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/estratticontributivi.gif";
                        act.Note = "EstrattiContributiviCalToDoList";
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
                //TODO SaveActivity
                throw new NotImplementedException();
            }

            /// <summary>
            /// Restituisce il nome univoco del provider
            /// </summary>
            public override string UniqueName
            {
                get
                {
                    return "ESTRCONTRCALPROV";
                }
            }
        }
    }
}