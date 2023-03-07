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
    public partial class Databases
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="IDBPOObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public abstract class DBObjectCursorPO<T> 
            : DBObjectCursorPO
            where T : IDBPOObject
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectCursorPO()
            {
            }

             
            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            public new T Item
            {
                get
                {
                    return (T) base.Item;
                }
                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge un nuovo elemento
            /// </summary>
            /// <returns></returns>
            public new T Add()
            {
                return (T)base.Add();
            }

            ///// <summary>
            ///// Istanzia un nuovo oggetto gestito dal modulo
            ///// </summary>
            ///// <param name="reader"></param>
            ///// <returns></returns>
            //public override object InstantiateNew(DBReader reader)
            //{
            //    return DMD.RunTime.CreateInstance<T>();
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                DBObjectPO t = (DBObjectPO) (object)DMD.RunTime.CreateInstance<T>();
                return t.GetModule();
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public sealed override object InstantiateNew(DBReader reader)
            {
                return this.InstantiateNewT(reader);
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="reader"></param>
            /// <returns></returns>
            public virtual T InstantiateNewT(DBReader reader)
            {
                return (T) base.InstantiateNew(reader);
            }

            /// <summary>
            /// OnInitialize
            /// </summary>
            /// <param name="item"></param>
            protected sealed override void OnInitialize(object item)
            {
                this.OnInitialize((T)item);
            }

            /// <summary>
            /// Inizializza il nuovo oggetto
            /// </summary>
            /// <param name="item"></param>
            protected virtual void OnInitialize(T item)
            {
                base.OnInitialize(item);
            }
        }
    }
}