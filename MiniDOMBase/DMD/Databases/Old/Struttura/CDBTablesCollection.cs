using System;
using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Databases
    {

        /// <summary>
        /// Collezione delle tabelle
        /// </summary>
        [Serializable]
        public class CDBTablesCollection 
            : IEnumerable<CDBTable>
        {

            private List<CDBTable> lst = new List<CDBTable>();
            private Dictionary<string, CDBTable> dic = new Dictionary<string, CDBTable>();
            [NonSerialized] private CDBConnection m_Owner;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CDBTablesCollection()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="connection"></param>
            public CDBTablesCollection(CDBConnection connection) : this()
            {
                m_Owner = connection;
            }

            public int Count
            {
                get
                {
                    return this.lst.Count;
                }
            }

            /// <summary>
            /// Riferimento alla connessione
            /// </summary>
            public CDBConnection Connection
            {
                get
                {
                    return m_Owner;
                }
            }

            public CDBTable this[int index]
            {
                get
                {
                    return this.lst[index];
                }
            }

            public CDBTable this[string tableName]
            {
                get
                {
                    return this.dic[DMD.Strings.LCase(DMD.Strings.Trim(tableName))];
                }
            }

          

            /// <summary>
            /// Aggiunge la tabella 
            /// </summary>
            /// <param name="table"></param>
            public void Add(CDBTable table)
            {
                var tableName = DMD.Strings.LCase(DMD.Strings.Trim(table.Name));
                this.dic.Add(tableName, table);
                this.lst.Add(table);
                if (this.m_Owner is object)
                    table.SetConnection(this.m_Owner);
            }

            /// <summary>
            /// Aggiunge una nuova tabella con nome
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public CDBTable Add(string tableName)
            {
                var item = new CDBTable(tableName);
                Add(item);
                return item;
            }

            /// <summary>
            /// Restituisce la tabella in base al nome
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public CDBTable GetItemByKey(string tableName)
            {
                tableName = DMD.Strings.LCase(DMD.Strings.Trim(tableName));
                CDBTable ret = null;
                this.dic.TryGetValue(tableName, out ret);
                return ret;
            }

            /// <summary>
            /// Restituisce vero se la collezione contiene la tabella con nome
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public bool ContainsKey(string tableName)
            {
                return this.dic.ContainsKey(DMD.Strings.LCase(DMD.Strings.Trim(tableName)));
            }

            /// <summary>
            /// Restituisce l'enumeratore
            /// </summary>
            /// <returns></returns>
            public IEnumerator<CDBTable> GetEnumerator()
            {
                return this.lst.GetEnumerator();
            }

            /// <summary>
            /// Restituisce o crea la tabella se non esiste
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public CDBTable Ensure(string tableName)
            {
                var ret = this.GetItemByKey(tableName);
                if (ret is null) ret = this.Add(tableName);
                return ret;
            }

            /// <summary>
            /// Rimuove la tabella
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public CDBTable RemoveByKey(string tableName)
            {
                var table = this.GetItemByKey(tableName);
                this.lst.Remove(table);
                this.dic.Remove(DMD.Strings.LCase(DMD.Strings.Trim(tableName)));
                return table;
            }

            /// <summary>
            /// Rimuove la tabella
            /// </summary>
            /// <param name="table"></param>
            /// <returns></returns>
            public void Remove(CDBTable table)
            {
                this.dic.Remove(DMD.Strings.LCase(DMD.Strings.Trim(table.Name)));
                this.lst.Remove(table);
            }

            /// <summary>
            /// Imposta il proprietario
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void SetOwner(CDBConnection owner)
            {
                m_Owner = owner;
                foreach (CDBTable table in this)
                    table.SetConnection(owner);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}