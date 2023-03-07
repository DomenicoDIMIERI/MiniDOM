using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Xml.Serialization;

namespace minidom
{
    public partial class Databases
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="minidom.Databases.IDBObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class DBObjectCursor<T> 
            : minidom.Databases.DBObjectCursor
              where T : minidom.Databases.DBObject
        {
             

            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            [XmlIgnore]
            public new T Item
            {
                get
                {
                    return (T)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Inizializza il nuovo oggetto (istanziato dall'ADD)
            /// </summary>
            /// <param name="item"></param>
            protected sealed override void OnInitialize(object item)
            {
                this.OnInitialize((T)item);
            }

            /// <summary>
            /// Inizializza il nuovo oggetto (istanziato dall'ADD)
            /// </summary>
            /// <param name="item"></param>
            protected virtual void OnInitialize(T item)
            {
                base.OnInitialize(item);
            }

            /// <summary>
            /// Aggiunge un nuovo elemento
            /// </summary>
            public new T Add()
            {
                return (T)base.Add();
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                var t = DMD.RunTime.CreateInstance<T>();
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
            /// <param name="reader"></param>
            /// <returns></returns>
            public virtual T InstantiateNewT(DBReader reader)
            {
                return (T)base.InstantiateNew(reader);
            }
        }
    }
}