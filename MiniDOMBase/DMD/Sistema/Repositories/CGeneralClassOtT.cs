using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Gestore delle risorse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class CModulesClass<T> 
            : CModulesClass, System.Collections.Generic.IEnumerable<T>
            where T : Databases.IDBObjectBase
        {


            /// <summary>
            /// Notifica al sistema l'inserimento di un nuovo oggetto
            /// </summary>
            /// <remarks></remarks>
            public new event ItemCreatedEventHandler<T> ItemCreated;



            /// <summary>
            /// Notifica al sistema l'eliminazione di un oggetto
            /// </summary>
            /// <remarks></remarks>
            public new event ItemDeletedEventHandler<T> ItemDeleted;



            /// <summary>
            /// Notifica al sistema che un oggetto è stato modificato
            /// </summary>
            /// <remarks></remarks>
            public new event ItemModifiedEventHandler<T> ItemModified;



            /// <summary>
            /// Costruttore
            /// </summary>
            public CModulesClass()
            {
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            public CModulesClass(string moduleName) 
                : base(moduleName)
            {
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            /// <param name="cursorType"></param>
            public CModulesClass(string moduleName, Type cursorType) 
                : base(moduleName, cursorType)
            {
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            /// <param name="cursorType"></param>
            /// <param name="cacheSize"></param>
            public CModulesClass(string moduleName, Type cursorType, int cacheSize)
                : base(moduleName, cursorType, cacheSize)
            {
                 
            }

                  

            /// <summary>
            /// Restituisce l'elemento in base all'ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual new T GetItemById(int id)
            {
                return (T)base.GetItemById(id);
            }

                
            /// <summary>
            /// Carica in memoria tutti gli elementi gestiti dal modulo (se CacheSize è impostato a -1)
            /// </summary>
            /// <returns></returns>
            public virtual new CCollection<T> LoadAll()
            {
                return new CCollectionWrapper<T>(base.LoadAll());
            }

            /// <summary>
            /// Aggiorna l'oggetto nella cache 
            /// </summary>
            /// <param name="item"></param>
            public sealed override void UpdateCached(object item)
            {
                this.UpdateCached((T)item);
            }

            /// <summary>
            /// Aggiorna l'oggetto nella cache 
            /// </summary>
            /// <param name="item"></param>
            public virtual void UpdateCached(T item)
            {
                base.UpdateCached(item);
            }


            /// <summary>
            /// Restituisce un oggetto IEnumerator che consente di enumerare tutti gli elementi contenuti nel repository
            /// </summary>
            /// <returns></returns>
            public new IEnumerator<T> GetEnumerator() 
            { 
                return (IEnumerator<T>)this._GetEnumerator(); 
            }

            /// <summary>
            /// Genera l'evento
            /// </summary>
            /// <param name="e"></param>
            protected sealed override void OnItemModified(ItemEventArgs e)
            {
                this.OnItemModified(new ItemEventArgs<T>((T)e.Item));
            }

            /// <summary>
            /// Genera l'evento ItemModified
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnItemModified(ItemEventArgs<T> e)
            {
                base.OnItemModified(e);
                this.ItemModified?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento
            /// </summary>
            /// <param name="e"></param>
            protected sealed override void OnItemCreated(ItemEventArgs e)
            {
                this.OnItemCreated(new ItemEventArgs<T>((T)e.Item));
            }

            /// <summary>
            /// Genera l'evento ItemModified
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnItemCreated(ItemEventArgs<T> e)
            {
                base.OnItemCreated(e);
                this.ItemCreated?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento
            /// </summary>
            /// <param name="e"></param>
            protected sealed override void OnItemDeleted(ItemEventArgs e)
            {
                this.OnItemDeleted(new ItemEventArgs<T>((T)e.Item));
            }

            /// <summary>
            /// Genera l'evento ItemModified
            /// </summary>
            /// <param name="e"></param>
            protected virtual void OnItemDeleted(ItemEventArgs<T> e)
            {
                base.OnItemDeleted(e);
                this.ItemDeleted?.Invoke(this, e);
            }

        }
    }
}