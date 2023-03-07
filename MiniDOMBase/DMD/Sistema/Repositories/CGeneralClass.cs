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
using minidom.internals;

namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Handler degli eventi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ModuleEventHandler(object sender, EventDescription e);

        /// <summary>
        /// Gestore delle risorse
        /// </summary>
        [Serializable]
        public abstract class CModulesClass 
            : IEnumerable
        {


            /// <summary>
            /// Notifica al sistema l'inserimento di un nuovo oggetto
            /// </summary>
            /// <remarks></remarks>
            public event ItemCreatedEventHandler ItemCreated;

            

            /// <summary>
            /// Notifica al sistema l'eliminazione di un oggetto
            /// </summary>
            /// <remarks></remarks>
            public event ItemDeletedEventHandler ItemDeleted;

            

            /// <summary>
            /// Notifica al sistema che un oggetto è stato modificato
            /// </summary>
            /// <remarks></remarks>
            public event ItemModifiedEventHandler ItemModified;

            /// <summary>
            /// Evento genirico sul modulo
            /// </summary>
            public event ModuleEventHandler ModuleEvent;

            private string m_ModuleName;
            private Type m_CursorType;
            [NonSerialized] private CModule m_Module;
            [NonSerialized] private List<CacheItem> m_CachedItems;
            private int m_CacheSize;
            [NonSerialized] private CCollection m_AllItems;
            private bool m_isInit;
            [NonSerialized] private DBConnection m_Conn = null;

            [NonSerialized] private CIndexingService m_Index = null;

            /// <summary>
            /// Oggetto utilizzabile per la sincronizzazione del singolo modulo
            /// </summary>
            /// <remarks></remarks>
            [NonSerialized] public readonly object cacheLock = new object();


            /// <summary>
            /// Elemento nella cache
            /// </summary>
            [Serializable]
            protected class CacheItem
            {
                /// <summary>
                /// Elmento
                /// </summary>
                public object Item;

                /// <summary>
                /// Vita
                /// </summary>
                public int Live;

                /// <summary>
                /// Costruttore
                /// </summary>
                public CacheItem()
                {
                    ////DMDObject.IncreaseCounter(this);
                }

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="item"></param>
                public CacheItem(object item)
                {
                    ////DMDObject.IncreaseCounter(this);
                    Item = item;
                    Live = 1;
                }
 
            }


            /// <summary>
            /// Costruttore
            /// </summary>
            public CModulesClass()
            {
                ////DMDObject.IncreaseCounter(this);
                m_ModuleName = "";
                m_CursorType = null;
                m_Module = null;
                m_CacheSize = 0;
                m_CachedItems = new List<CacheItem>();
                m_AllItems = null;
                m_isInit = false;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            public CModulesClass(string moduleName) 
                : this()
            {
                moduleName = DMD.Strings.Trim(moduleName);
                if (DMD.Strings.Left(moduleName, 3) != "mod")
                    throw new ArgumentNullException("moduleName");
                if (string.IsNullOrEmpty(moduleName))
                    throw new ArgumentNullException("moduleName");
                m_ModuleName = moduleName;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            /// <param name="cursorType"></param>
            public CModulesClass(string moduleName, Type cursorType) 
                : this()
            {
                moduleName = DMD.Strings.Trim(moduleName);
                if (DMD.Strings.Left(moduleName, 3) != "mod")
                    throw new ArgumentNullException("moduleName");
                if (string.IsNullOrEmpty(moduleName))
                    throw new ArgumentNullException("moduleName");
                if (cursorType is null)
                    throw new ArgumentNullException("cursorType");
                m_ModuleName = moduleName;
                m_CursorType = cursorType;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            /// <param name="cursorType"></param>
            /// <param name="cacheSize"></param>
            public CModulesClass(string moduleName, Type cursorType, int cacheSize)
                : this()
            {
                moduleName = DMD.Strings.Trim(moduleName);
                if (DMD.Strings.Left(moduleName, 3) != "mod")
                    throw new ArgumentNullException("moduleName");
                if (string.IsNullOrEmpty(moduleName))
                    throw new ArgumentNullException("moduleName");
                if (cursorType is null)
                    throw new ArgumentNullException("cursorType");
                m_ModuleName = moduleName;
                m_CursorType = cursorType;
                m_Module = null;
                m_CacheSize = cacheSize;
            }

            /// <summary>
            /// Restituisce un riferimento al modulo
            /// </summary>
            /// <returns></returns>
            public CModule Module
            {
                get
                {
                    lock (this)
                    {
                        if (this.m_Module is null) this.m_Module = Modules.GetItemByName(m_ModuleName);
                        if (this.m_Module is null)
                        {
                            this.m_Module = CreateModuleInfo();
                            this.InitializeStandardActions();
                        }
                        return this.m_Module;
                    }
                }
            }

            /// <summary>
            /// Accede al sistema di indicizzazione
            /// </summary>
            public CIndexingService Index
            {
                get
                {
                    lock (this)
                    {
                        if (m_Index is null && !string.IsNullOrEmpty(this.m_ModuleName))
                        {
                            var db = this.Database;
                            var tblWordStats = this.m_ModuleName + "_IdxWStats";
                            var tblWordIndexes = this.m_ModuleName + "_IdxWIndex";
                            m_Index = new CIndexingService(db, tblWordStats, tblWordIndexes);
                            m_Index.Database = db;
                            m_Index.WordIndexFolder = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, this.m_ModuleName + "\\wordindex\\");
                            Sistema.FileSystem.CreateRecursiveFolder(m_Index.WordIndexFolder);
                        }

                        return m_Index;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la connessione al DB
            /// </summary>
            public DBConnection Database
            {
                get
                {
                    if (m_Conn is null)
                        return minidom.Sistema.ApplicationContext.APPConn;
                    return m_Conn;
                }

                set
                {
                    m_Conn = value;
                }
            }

            /// <summary>
            /// Inizializza le risorse necessarie al modulo
            /// </summary>
            public virtual void Initialize()
            {
                if (m_isInit)
                    return;
                var m = this.Module;
                m_isInit = true;
            }

            /// <summary>
            /// Crea un nuovo oggetto gestito dal modulo
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public virtual object InstantiateNewItem(object args)
            {
                using(var c = this.CreateCursor())
                {
                    return c.Add();
                }
            }

            /// <summary>
            /// Inizializza le azioni standard del modulo
            /// </summary>
            public virtual void InitializeStandardActions()
            {
                var m = this.Module;

                m.DefinedActions.EnsureAction("create");
                m.DefinedActions.EnsureAction("list");
                m.DefinedActions.EnsureAction("edit");
                m.DefinedActions.EnsureAction("delete");
                using (var c = this.CreateCursor())
                {
                    if (c is minidom.Databases.DBObjectCursor)
                    {
                        m.DefinedActions.EnsureAction("list_own");
                        m.DefinedActions.EnsureAction("edit_own");
                        m.DefinedActions.EnsureAction("delete_own");
                        m.DefinedActions.EnsureAction("list_assigned");
                        m.DefinedActions.EnsureAction("edit_assigned");
                        m.DefinedActions.EnsureAction("delete_assigned");
                    }
                    if (c is minidom.Databases.DBObjectCursorPO)
                    {
                        m.DefinedActions.EnsureAction("list_office");
                        m.DefinedActions.EnsureAction("edit_office");
                        m.DefinedActions.EnsureAction("delete_office");
                    }
                }

            }


            /// <summary>
            /// Rilascia le risorse necessarie al modulo
            /// </summary>
            public virtual void Terminate()
            {
                m_isInit = false;
            }

            /// <summary>
            /// Crea il modulo e salva le informazioni nel DB
            /// </summary>
            /// <returns></returns>
            protected virtual CModule CreateModuleInfo()
            {
                var ret = new CModule(m_ModuleName);
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.Description = m_ModuleName;
                ret.DisplayName = m_ModuleName;
                ret.ClassHandler = "CBaseModuleHandler";
                ret.Save();
                return ret;
            }

            /// <summary>
            /// Crea un'istanza del cursore
            /// </summary>
            /// <returns></returns>
            public virtual Databases.DBObjectCursorBase CreateCursor()
            {
                return (Databases.DBObjectCursorBase)DMD.RunTime.CreateInstance(this.m_CursorType);
            }

            /// <summary>
            /// Restituisce l'elemento in base all'ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual object GetItemById(int id)
            {
                lock (cacheLock)
                {
                    if (id == 0)
                        return null;
                    var ret = GetCachedItemById(id);
                    if (ret is null)
                    {
                        ret = GetNonCachedItemById(id);
                        if (ret is object)
                            AddToCache(ret);
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce l'elemento in base all'ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            protected virtual object GetCachedItemById(int id)
            {
                if (id == 0)
                    return null;
                for (int i = 0, loopTo = m_CachedItems.Count - 1; i <= loopTo; i++)
                {
                    var o = m_CachedItems[i];
                    if (DBUtils.GetID(o.Item, 0) == id)
                        return o.Item;
                }

                return null;
            }

            ///// <summary>
            ///// Restituisce un riferimento al database
            ///// </summary>
            ///// <returns></returns>
            //public virtual Databases.CDBConnection GetConnection()
            //{
            //    lock (this)
            //    {
            //        if (m_Conn is null)
            //        {
            //            var cursor = CreateCursor();
            //            if (cursor is object)
            //            {
            //                m_Conn = cursor.GetConnection();
            //                cursor.Dispose();
            //            }
            //        }

            //        return m_Conn;
            //    }
            //}

            /// <summary>
            /// Carica l'elemento dalla base di dati
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            protected virtual object GetNonCachedItemById(int id)
            {
                if (id == 0)
                    return null;
                using (var cursor = this.CreateCursor())
                {
                    cursor.ID.Value = id;
                    cursor.IgnoreRights = true;
                    return cursor.Item;
                }
            }
                 

            /// <summary>
            /// Aggiunge l'elemento alla cache
            /// </summary>
            /// <param name="item"></param>
            protected virtual void AddToCache(object item)
            {
                if (m_CacheSize == 0)
                    return;
                m_CachedItems.Add(new CacheItem(item));
                if (m_CacheSize > 0)
                {
                    while (m_CachedItems.Count > m_CacheSize)
                        m_CachedItems.RemoveAt(0);
                }

                m_AllItems = null;
            }

            /// <summary>
            /// Aggiunge l'elemento alla cache
            /// </summary>
            /// <param name="item"></param>
            protected virtual void AddToCache1(object item)
            {
                if (m_CacheSize == 0)
                    return;
                m_CachedItems.Add(new CacheItem(item));
                if (m_CacheSize > 0)
                {
                    while (m_CachedItems.Count > m_CacheSize)
                        m_CachedItems.RemoveAt(0);
                }
            }

            /// <summary>
            /// Restituisce true se l'utente corrente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public virtual bool UserCanDoAction(CModuleAction action)
            {
                return action.UserCanDoAction(minidom.Sistema.Users.CurrentUser);
            }

            /// <summary>
            /// Restituisce true se l'utente corrente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            public virtual bool UserCanDoAction(string actionName)
            {
                var a = this.Module.DefinedActions.GetItemByKey(actionName);
                if (a is null)
                    return false; // Throw New ArgumentOutOfRangeException("Il modulo non implementa l'azione [" & actionName & "]")
                return this.UserCanDoAction(a);
            }

            /// <summary>
            /// Restituisce true se l'utente corrente è abilitato ad eseguire l'azione
            /// </summary>
            /// <param name="actionID"></param>
            /// <returns></returns>
            public virtual bool UserCanDoAction(int actionID)
            {
                return this.UserCanDoAction(this.Module.DefinedActions.GetItemById(actionID));
            }


            /// <summary>
            /// Aggiorna la cache sulla base dello stato dell'oggetto
            /// </summary>
            /// <param name="item"></param>
            /// <remarks></remarks>
            public virtual void UpdateCached(object item)
            {
                lock (cacheLock)
                {
                    if (m_CacheSize == 0)
                        return;
                    for (int i = 0, loopTo = m_CachedItems.Count - 1; i <= loopTo; i++)
                    {
                        var o = m_CachedItems[i];
                        if (DBUtils.GetID(o.Item, 0) == DBUtils.GetID(item, 0))
                        {
                            //var oldItem = o.Item;
                            //oldItem.InitializeFrom(item);
                            //item = oldItem;
                            m_CachedItems.RemoveAt(i);
                            break;
                        }
                    }

                    if (item is Databases.IDBObject)
                    {
                        if (((Databases.IDBObject)item).Stato == ObjectStatus.OBJECT_VALID)
                        {
                            m_CachedItems.Add(new CacheItem(item));
                        }
                    }
                    else if (DBUtils.GetID(item, 0) != 0)
                    {
                        m_CachedItems.Add(new CacheItem(item));
                    }

                    if (m_CacheSize > 0)
                    {
                        while (m_CachedItems.Count > m_CacheSize)
                            m_CachedItems.RemoveAt(0);
                    }

                    // If (Me.m_AllItems IsNot Nothing) Then
                    // For i As Integer = 0 To Me.m_AllItems.Count - 1
                    // If (GetID(Me.m_AllItems(i)) = GetID(item)) Then
                    // Dim oldItem As T = Me.m_AllItems(i)
                    // oldItem.InitializeFrom(item)
                    // Me.m_CachedItems.RemoveAt(i)
                    // Exit For
                    // End If
                    // Next
                    // End If
                    m_AllItems = null;
                }
            }

            /// <summary>
            /// Rimuove l'elemento dalla cache
            /// </summary>
            /// <param name="item"></param>
            public virtual void RemoveCached(object item)
            {
                lock (cacheLock)
                {
                    if (m_CacheSize == 0)
                        return;
                    for (int i = 0, loopTo = m_CachedItems.Count - 1; i <= loopTo; i++)
                    {
                        var o = m_CachedItems[i];
                        if (DBUtils.GetID(o.Item, 0) == DBUtils.GetID(item, 0))
                        {
                            m_CachedItems.RemoveAt(i);
                            break;
                        }
                    }

                    m_AllItems = null;
                }
            }

            /// <summary>
            /// Carica in memoria tutti gli elementi gestiti dal modulo (se CacheSize è impostato a -1)
            /// </summary>
            /// <returns></returns>
            public virtual CCollection LoadAll()
            {
                lock (cacheLock)
                {
                    if (m_AllItems is object)
                        return m_AllItems;
                    if (m_CacheSize != 0)
                    {
                        using (var cursor = CreateCursor())
                        {
                            if (cursor is Databases.DBObjectCursor)
                            {
                                ((Databases.DBObjectCursor)cursor).Stato.Value = ObjectStatus.OBJECT_VALID;
                            }
                            cursor.IgnoreRights = true;
                            while (
                                      cursor.Read() 
                                   && (m_CacheSize < 0 || m_CachedItems.Count < m_CacheSize)
                                   )
                            {
                                var item1 = GetCachedItemById(DBUtils.GetID(cursor.Item, 0));
                                if (item1 is null)
                                    AddToCache1(cursor.Item);
                                // Me.UpdateCached(cursor.Item)
                                 
                            }

                        }
                    }

                    m_AllItems = new CCollection();
                    for (int i = 0, loopTo = m_CachedItems.Count - 1; i <= loopTo; i++)
                        m_AllItems.Add(m_CachedItems[i].Item);
                    //if (typeof(IComparable).IsAssignableFrom(typeof(T)))
                    //    m_AllItems.Sort();
                    return m_AllItems;
                }
            }

            /// <summary>
            /// Accede alla cache
            /// </summary>
            protected List<CacheItem> CachedItems
            {
                get
                {
                    return this.m_CachedItems;
                }
            }

            /// <summary>
            /// Genera l'evento ItemCreated
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnItemCreated(ItemEventArgs e)
            {
                this.UpdateCached(e.Item);
                ItemCreated?.Invoke(this, e);
            }

            /// <summary>
            /// Chiama il metodo OnItemCreated
            /// </summary>
            /// <param name="e"></param>
            internal void doOnItemCreated(ItemEventArgs e)
            {
                this.OnItemCreated(e);
            }

            /// <summary>
            /// Genera l'evento ItemDeleted
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnItemDeleted(ItemEventArgs e)
            {
                this.UpdateCached(e.Item);
                ItemDeleted?.Invoke(this, e);
            }

            /// <summary>
            /// Chiama il metodo OnItemDeleted
            /// </summary>
            /// <param name="e"></param>
            internal void doOnItemDeleted(ItemEventArgs e)
            {
                this.OnItemDeleted(e);
            }

            /// <summary>
            /// Genera l'evento ItemModified
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnItemModified(ItemEventArgs e)
            {
                this.UpdateCached(e.Item);
                ItemModified?.Invoke(this, e);
            }

            /// <summary>
            /// Chiama il metodo OnItemModified
            /// </summary>
            /// <param name="e"></param>
            internal void doOnItemModified(ItemEventArgs e)
            {
                this.OnItemModified(e);
            }

            /// <summary>
            /// Notifica un evento relativo al modulo
            /// </summary>
            /// <param name="e"></param>
            public virtual void DispatchEvent(EventDescription e)
            {
                e.SetModule(this.Module);
                this.ModuleEvent?.Invoke(this, e);
                minidom.Sistema.Events.DispatchEvent(e);
            }

            /// <summary>
            /// Restituisce un oggetto IEnumerator che consente di enumerare tutti gli oggetti
            /// gestiti dal repository
            /// </summary>
            /// <returns></returns>
            public IEnumerator GetEnumerator()
            {
                return this._GetEnumerator();
            }

            /// <summary>
            /// Restituisce un oggetto IEnumerator che consente di enumerare tutti gli oggetti
            /// gestiti dal repository
            /// </summary>
            /// <returns></returns>
            protected virtual IEnumerator _GetEnumerator()
            {
                if (this.m_CacheSize == -1)
                {
                    return this.LoadAll().GetEnumerator();
                }
                else
                {
                    return new MyCursorEnumerator(this);
                }
            }

            /// <summary>
            /// Enumeratore basato su cursore
            /// </summary>
            protected class MyCursorEnumerator
                : IEnumerator<object>
            {
                /// <summary>
                /// Owner
                /// </summary>
                public CModulesClass owner;

                /// <summary>
                /// Cursore
                /// </summary>
                public minidom.Databases.DBObjectCursorBase cursor;

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="owner"></param>
                public MyCursorEnumerator(CModulesClass owner)
                {
                    if (owner is null)
                        throw new ArgumentNullException("owner");
                    this.cursor = owner.CreateCursor();
                    if (this.cursor is minidom.Databases.DBObjectCursor)
                    {
                        ((minidom.Databases.DBObjectCursor)cursor).Stato.Value = ObjectStatus.OBJECT_VALID;
                    }
                }

                /// <summary>
                /// Elemento corrente
                /// </summary>
                public object Current
                {
                    get
                    {
                        return this.cursor.Item;
                    }
                }

                /// <summary>
                /// Rilascia le risorese
                /// </summary>
                public void Dispose()
                {
                    this.cursor.Dispose();
                    this.cursor = null;
                }

                /// <summary>
                /// Sposta all'elemento successivo
                /// </summary>
                /// <returns></returns>
                public bool MoveNext()
                {
                    return this.cursor.Read();
                }

                /// <summary>
                /// Resetta il cursore
                /// </summary>
                public void Reset()
                {
                    this.cursor.Clear();
                }
            }

        }
    }
}