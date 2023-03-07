using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione degli uffici a cui appartiene un utente
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUserUffici 
            : CCollection<CUfficio>
        {
            [NonSerialized] private Sistema.CUser m_User;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUserUffici()
            {
                m_User = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="user"></param>
            public CUserUffici(Sistema.CUser user) : this()
            {
                Load(user);
            }

            /// <summary>
            /// Restituisce un riferimento all'utente
            /// </summary>
            public Sistema.CUser User
            {
                get
                {
                    return m_User;
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CUfficio oldValue, CUfficio newValue)
            {
                //if (this.m_User is object)
                //    newValue.SetUser(this.m_User);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CUfficio value)
            {
                //if (value is object)
                //    value.SetUser(this.m_User);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// Inserisce l'ufficio
            /// </summary>
            /// <param name="index"></param>
            /// <param name="u"></param>
            private new void Insert(int index, CUfficio u)
            {
                lock (this)
                    base.Insert(index, u);
            }

            /// <summary>
            /// Aggiunge l'ufficio
            /// </summary>
            /// <param name="u"></param>
            public new void Add(CUfficio u)
            {
                if (u is null)
                    throw new ArgumentNullException("ufficio");
                //var db = Uffici.GetConnection();
                //if (db.IsRemote())
                //{
                //    int uID = DBUtils.GetID(u);
                //    if (GetItemById(uID) is object)
                //        throw new Exception("La collezione contiene già l'ufficio specificato");
                //    string tmp = db.InvokeMethod(Uffici.Module, "AddUfficio", "uid", Sistema.RPC.int2n(DBUtils.GetID(m_User)), "oid", uID);
                //    if (!string.IsNullOrEmpty(tmp))
                //        throw new Exception(tmp);
                //}
                //else
                //{
                    lock (m_User)
                    {
                        int i = IndexOf(u);
                        if (i >= 0)
                            throw new ArgumentException("L'ufficio è già associato all'utente");
                        lock (Uffici)
                        {
                            CUtenteXUfficio item = null;
                            foreach (var uxu in minidom.Anagrafica.Uffici.UfficiConsentiti)
                            {
                                if (
                                       uxu.IDUfficio == DBUtils.GetID(u, 0) 
                                    && uxu.IDUtente == DBUtils.GetID(m_User, 0)
                                    )
                                {
                                    item = uxu;
                                    break;
                                }
                            }

                            if (item is null)
                            {
                                item = new CUtenteXUfficio();
                                item.Ufficio = u;
                                item.Utente = m_User;
                                item.Save();
                                //Uffici.UfficiConsentiti.Add(item);
                            }

                            base.Add(u);
                        }
                    }
                //}
            }

            /// <summary>
            /// Rimuove l'ufficio
            /// </summary>
            /// <param name="u"></param>
            public new void Remove(CUfficio u)
            {
                lock (m_User)
                {
                    int i = IndexOf(u);
                    if (i < 0)
                        throw new ArgumentException("L'ufficio non è associato all'utente");
                    lock (Uffici)
                    {
                        CUtenteXUfficio item = null;
                        foreach (var uxu in Uffici.UfficiConsentiti)
                        {
                            if (uxu.IDUfficio == DBUtils.GetID(u, 0) && uxu.IDUtente == DBUtils.GetID(m_User, 0))
                            {
                                item = uxu;
                                break;
                            }
                        }

                        if (item is object)
                        {
                            item.Delete();
                            //Uffici.UfficiConsentiti.Remove(item);
                        }
                    }

                    RemoveAt(i);
                }
            }

            /// <summary>
            /// Restituisce l'indice dell'ufficio
            /// </summary>
            /// <param name="u"></param>
            /// <returns></returns>
            public new int IndexOf(CUfficio u)
            {
                lock (m_User)
                {
                    if (u is null)
                        throw new ArgumentNullException("u");
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        if (DBUtils.GetID(this[i]) == DBUtils.GetID(u))
                            return i;
                    }

                    return -1;
                }
            }

            /// <summary>
            /// Carica gli uffici
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            protected internal bool Load(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                lock (user)
                {
                    lock (Uffici)
                    {
                        Clear();
                        m_User = user;
                        if (DBUtils.GetID(user, 0) == 0)
                            return true;
                        
                        foreach(var item in  minidom.Anagrafica.Uffici.UfficiConsentiti)
                        { 
                            if (  
                                   item.IDUtente == DBUtils.GetID(user, 0) 
                                && item.Ufficio is object 
                                && item.Ufficio.Stato == ObjectStatus.OBJECT_VALID
                                )
                            {
                                base.Add(item.Ufficio);
                            }
                        }

                        return true;
                    }
                }
            }

            /// <summary>
            /// Restituisce vero se l'utente corrente condivide almeno un ufficio con l'utente specificato
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool SameOffice(Sistema.CUser user)
            {
                if (ReferenceEquals(user, m_User))
                    return true;
                if (user is null)
                    throw new ArgumentNullException("user");
                lock (m_User)
                {
                    lock (user)
                    {
                        for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                        {
                            var u = this[i];
                            for (int j = 0, loopTo1 = user.Uffici.Count - 1; j <= loopTo1; j++)
                            {
                                var u1 = user.Uffici[j];
                                if (DBUtils.GetID(u) == DBUtils.GetID(u1))
                                    return true;
                            }
                        }

                        return false;
                    }
                }
            }

            /// <summary>
            /// Restituisce true se l'ufficio appartiene alla collezione
            /// </summary>
            /// <param name="office"></param>
            /// <returns></returns>
            public bool HasOffice(CUfficio office)
            {
                return HasOffice(DBUtils.GetID(office, 0));
            }

            /// <summary>
            /// Restituisce true se l'ufficio appartiene alla collezione
            /// </summary>
            /// <param name="officeID"></param>
            /// <returns></returns>
            public bool HasOffice(int officeID)
            {
                lock (m_User)
                {
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        var u = this[i];
                        if (DBUtils.GetID(u, 0) == officeID)
                            return true;
                    }

                    return false;
                }
            }

            /// <summary>
            /// Restituisce true se l'ufficio appartiene alla collezione
            /// </summary>
            /// <param name="u"></param>
            /// <returns></returns>
            public new bool Contains(CUfficio u)
            {
                return IndexOf(u) >= 0;
            }

            /// <summary>
            /// Imposta l'utente
            /// </summary>
            /// <param name="user"></param>
            protected internal virtual void SetUser(Sistema.CUser user)
            {
                m_User = user;
            }
        }
    }
}