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
        /// Provider di eventi del calendario
        /// </summary>
        public class CRMCalProvider 
            : Sistema.BaseCalendarActivitiesProvider
        {

            /// <summary>
            /// Informazioni sui fax in attesa
            /// </summary>
            private class FaxInfo 
                : IComparable, IComparable<FaxInfo>
            {
                /// <summary>
                /// ID del fax
                /// </summary>
                public int ID;

                /// <summary>
                /// ID del punto operativo
                /// </summary>
                public int PO;

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="c"></param>
                public FaxInfo(FaxDocument c)
                {
                    ID = DBUtils.GetID(c, 0);
                    PO = c.IDPuntoOperativo;
                }

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="id"></param>
                public FaxInfo(int id)
                {
                    ID = id;
                    PO = 0;
                }

                /// <summary>
                /// Compara due oggetti
                /// </summary>
                /// <param name="tmp"></param>
                /// <returns></returns>
                public int CompareTo(FaxInfo tmp)
                {
                    return DMD.Arrays.Compare(this.ID, tmp.ID);
                }

                int IComparable.CompareTo(object obj)
                {
                    return this.CompareTo((FaxInfo)obj);
                }
            }

            private object listLock = new object();
            private FaxInfo[] m_FaxList;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMCalProvider()
            {
                minidom.CustomerCalls.CRM.ItemModified += handleContatto;
                minidom.CustomerCalls.CRM.ItemCreated += handleContatto;
                minidom.CustomerCalls.CRM.ItemDeleted += handleContatto;
                m_FaxList = null;
            }

            private bool IsInFaxList(int id)
            {
                int localBinarySearch() { var argitem = new FaxInfo(id); var ret = DMD.Arrays.BinarySearch(m_FaxList, argitem); return ret; }

                return DMD.Arrays.Len(m_FaxList) > 0 && localBinarySearch() >= 0;
            }

            private void AddToFaxList(FaxDocument c)
            {
                int i = -1;
                var info = new FaxInfo(c);
                if (DMD.Arrays.Len(m_FaxList) > 0)
                    i = DMD.Arrays.BinarySearch(m_FaxList, info);

                if (i >= 0)
                {
                    m_FaxList[i] = info;
                }
                else
                {
                    m_FaxList = DMD.Arrays.Append(m_FaxList, info);
                    DMD.Arrays.Sort(m_FaxList);
                }
            }

            private void RemoveFromFaxList(int id)
            {
                int i = -1;
                if (DMD.Arrays.Len(m_FaxList) > 0)
                {
                    var argitem = new FaxInfo(id);
                    i = DMD.Arrays.BinarySearch(m_FaxList, argitem);
                }

                if (i >= 0)
                    m_FaxList = DMD.Arrays.RemoveAt(m_FaxList, i);
            }

            private CCollection<FaxInfo> FaxList
            {
                get
                {
                    lock (listLock)
                    {
                        if (m_FaxList is null)
                        {
                            var tmp = new List<FaxInfo>();
                            using (var cursor1 = new CContattoUtenteCursor())
                            {
                                cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                                cursor1.IDPersona.Value = 0;
                                cursor1.ClassName.Value = "FaxDocument";
                                cursor1.IgnoreRights = true;
                                cursor1.ID.SortOrder = SortEnum.SORT_ASC;
                                while (cursor1.Read())
                                {
                                    tmp.Add(new FaxInfo((FaxDocument)cursor1.Item));
                                }

                            }
                            m_FaxList = tmp.ToArray();
                            // If (Me.m_FaxList IsNot Nothing) Then Array.Sort(Me.m_FaxList)
                        }

                        var ret = new CCollection<FaxInfo>();
                        if (m_FaxList is object)
                            ret.AddRange(m_FaxList);
                        return ret;
                    }
                }
            }

            private void handleContatto(object sender, ItemEventArgs<CContattoUtente> e)
            {
                var c = e.Item;
                if (c is FaxDocument)
                {
                    lock (listLock)
                    {
                        if (c.Stato == ObjectStatus.OBJECT_VALID && c.IDPersona == 0)
                        {
                            AddToFaxList((FaxDocument)c);
                        }
                        else
                        {
                            RemoveFromFaxList(DBUtils.GetID(c, 0));
                        }
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
                return new CCollection<Sistema.CActivePerson>();
            }

            /// <summary>
            /// Restituisce le attività in sospeso
            /// </summary>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                //TODO GetPendingActivities
                return new CCollection<Sistema.ICalendarActivity>();
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
                return new CCollection<Sistema.ICalendarActivity>();
            }

            private CCollection<FaxInfo> GetAllowedFaxList()
            {
                lock(this.listLock)
                {
                    var u = Sistema.Users.CurrentUser;
                    var ret = new CCollection<FaxInfo>();
                    foreach (FaxInfo f in FaxList)
                    {
                        if (f.PO == 0 || u.Uffici.HasOffice(f.PO))
                        {
                            ret.Add(f);
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce l'elenco delle cose da fare
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                var ret = new CCollection<Sistema.ICalendarActivity>();
                Sistema.CCalendarActivity act;
                if (CRM.CRMGroup.Members.Contains(Sistema.Users.CurrentUser))
                {
                    var items = GetAllowedFaxList();
                    if (items.Count > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + items.Count + " fax da associare";
                        act.DataInizio = DMD.DateUtils.Now();
                        act.GiornataIntera = true;
                        act.Categoria = "Normale";
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/faxdaassociare.png";
                        act.Note = "CRMShowFaxNonAssociati";
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        ret.Add(act);
                    }
                }

                return ret;
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
            /// Restituisce il nome univoco dell'handler
            /// </summary>
            public override string UniqueName
            {
                get
                {
                    return "CRMCALPROVIDER";
                }
            }
        }
    }
}