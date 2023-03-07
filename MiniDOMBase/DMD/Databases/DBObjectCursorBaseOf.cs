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
        /// Cursore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public abstract class DBObjectCursorBase<T> 
            : DBObjectCursorBase where T : DBObjectBase
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
                var o = RunTime.CreateInstance<T>();
                return o.GetModule();
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public sealed override object InstantiateNew(DBReader reader)
            {
                return this.InstantiateNew(reader);
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