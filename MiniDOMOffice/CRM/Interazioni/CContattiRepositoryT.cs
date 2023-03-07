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
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CContattoUtente"/>
        /// </summary>
        [Serializable]
        public abstract class CContattiRepository<T>
            : CContattiRepository 
            where T : CContattoUtente
        {
          

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="moduleName"></param>
            /// <param name="cursorType"></param>
            /// <param name="cacheSize"></param>
            public CContattiRepository(string moduleName, System.Type cursorType, int cacheSize) 
                : base(moduleName, cursorType, cacheSize)
            {
                 
            }
             
            /// <summary>
            /// Restituisce l'elemento in base all'id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual new T GetItemById(int id)
            {
                return (T)base.GetItemById(id);
            }
             
            /// <summary>
            /// Restituisce l'ultimo contatto da o verso la persona
            /// </summary>
            /// <param name="pID"></param>
            /// <returns></returns>
            public new T GetLastRunning(int pID)
            {
                return (T) base.GetLastRunning(pID);
            }

           
             

            /// <summary>
            /// Restituisce la collezione dei contatti in attesa
            /// </summary>
            public new List<T> InAttesa
            {
                get
                {
                    lock (inAttesaLock)
                    {
                        var ret = new List<T>(InnerList.Count);
                        foreach(var item in base.InnerList.Values)
                        {
                            ret.Add((T)item);
                        }
                        return ret;
                    }                         
                }
            }

            /// <summary>
            /// Aggiunge il contatto in attesa
            /// </summary>
            /// <param name="item"></param>
            public virtual void SetInAttesa(T item)
            {
                base.SetInAttesa(item);
            }

            private new void SetInAttesa(CContattoUtente item)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Rimuove il contatto in attesa
            /// </summary>
            /// <param name="item"></param>
            public virtual void SetFineAttesa(T item)
            {
                base.SetFineAttesa(item);
            }

            private new void SetFineAttesa(CContattoUtente item)
            {
                throw new NotSupportedException();
            }


        }
    }
     
}