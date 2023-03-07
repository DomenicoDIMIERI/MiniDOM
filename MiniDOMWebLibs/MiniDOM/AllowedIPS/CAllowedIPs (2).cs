using System.Collections;

namespace minidom
{
    public partial class WebSite
    {
        public class CAllowedIPs : CCollection<IPADDRESSinfo>
        {
            public readonly object SyncObj = new object();

            public CAllowedIPs()
            {
                base.Sorted = true;
            }

            private new bool Sorted
            {
                get
                {
                    return true;
                }

                set
                {
                }
            }

            private new IComparer Comparer
            {
                get
                {
                    return (IComparer)base.Comparer;
                }

                set
                {
                    base.Comparer = value;
                }
            }

            public void Add(string netAddress)
            {
                Add(new IPADDRESSinfo(netAddress));
            }

            public bool IsIPAllowed(string ip)
            {
                lock (SyncObj)
                {
                    bool allow, negate;
                    if (Count == 0)
                        return true;
                    allow = false;
                    negate = false;
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        var a = this[i];
                        if (a.Match(ip))
                        {
                            allow = allow | a.Allow;
                            negate = negate | a.Negate;
                        }
                    }

                    return allow & !negate;
                }
            }

            public IPADDRESSinfo GetIPAllowInfo(string ip)
            {
                lock (SyncObj)
                {
                    var ret = new IPADDRESSinfo();
                    ret.Allow = false;
                    ret.Negate = false;
                    ret.Interno = false;
                    ret.AssociaUfficio = "";
                    ret.IP = ret.GetBytes(ip);
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        var a = this[i];
                        if (a.Match(ip))
                        {
                            ret.AssociaUfficio = a.AssociaUfficio;
                            ret.Allow = ret.Allow | a.Allow;
                            ret.Negate = ret.Negate | a.Negate;
                        }
                    }

                    return ret;
                }
            }

            public bool IsIPNegated(string ip)
            {
                lock (SyncObj)
                {
                    if (Count == 0)
                        return true;
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        var a = this[i];
                        if (a.Match(ip))
                        {
                            if (a.Negate)
                                return true;
                        }
                    }

                    return false;
                }
            }

            public bool Load()
            {
                lock (SyncObj)
                {
                    Clear();
                    var cursor = new IPADDRESSInfoCursor();
                    cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    base.Sorted = false;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    cursor.Dispose();
                    base.Sorted = true;
                    return true;
                }
            }



            /// <summary>
        /// Restituisce vero se la rimozione dell'indirizzo specificato non causa l'esclusione del client remoto
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool TestRemove(IPADDRESSinfo item)
            {
                bool ret;
                Remove(item);
                ret = IsIPAllowed(Instance.CurrentSession.RemoteIP);
                Add(item);
                return ret;
            }

            /// <summary>
        /// Restituisce vero se la rimozione dell'indirizzo specificato non causa l'esclusione del client remoto
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool TestAdd(IPADDRESSinfo item)
            {
                bool ret;
                Add(item);
                ret = IsIPAllowed(Instance.CurrentSession.RemoteIP);
                Remove(item);
                return ret;
            }

            /// <summary>
        /// Questa funzione consente di verificare se la modifica dei parametri renderà inaccessibile il client remoto
        /// </summary>
        /// <param name="item"></param>
        /// <param name="allow"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool TestAllowNegate(IPADDRESSinfo item, bool allow, bool negate)
            {
                if (allow == negate)
                {
                    return TestRemove(item);
                }
                else
                {
                    bool a = item.Allow;
                    bool n = item.Negate;
                    item.Allow = allow;
                    item.Negate = negate;
                    bool ret = IsIPAllowed(Instance.CurrentSession.RemoteIP);
                    item.Allow = a;
                    item.Negate = n;
                    return ret;
                }
            }

            /// <summary>
        /// Questa funzione modifica i parametri
        /// </summary>
        /// <param name="item"></param>
        /// <param name="allow"></param>
        /// <remarks></remarks>
            public void SetAllowNegate(IPADDRESSinfo item, bool allow, bool negate)
            {
                if (allow == negate)
                {
                    item.Delete();
                    Remove(item);
                }
                else
                {
                    item.Allow = allow;
                    item.Negate = negate;
                    item.Save();
                }
            }
        }
    }
}