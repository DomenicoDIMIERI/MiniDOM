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
        /// Cursore su una tabella
        /// </summary>
        [Serializable]
        public abstract class DBObjectCursorBase 
            : DMD.Databases.DBCursorBase // IComparer, IDisposable,  IDMDXMLSerializable
        {
            [NonSerialized] private CModulesClass m_Module;
            [NonSerialized] private string m_tableName;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectCursorBase()
            {
                this.m_Module = null;
                this.m_tableName = null;
            }

           
            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            /// <returns></returns>
            protected abstract CModulesClass GetModule();

            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            public CModulesClass Module
            {
                get
                {
                    if (this.m_Module is null) this.m_Module = this.GetModule();
                    return m_Module;
                }
            }

            /// <summary>
            /// Restituisce un riferimento alla connessione
            /// </summary>
            /// <returns></returns>
            protected override DBConnection GetConnection()
            {
                DBConnection c = null;
                if (c is null)
                {
                    var m = this.Module;
                    c = (m is object) ? m.Database : minidom.Sistema.ApplicationContext.APPConn;
                }
                return c;
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public override object InstantiateNew(DBReader reader)
            {
                var m = this.Module;
                return m.InstantiateNewItem(reader);
            }

            /// <summary>
            /// Restituisce il discriminante nel repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                if (string.IsNullOrEmpty(this.m_tableName))
                {
                    var o = this.InstantiateNew(null);
                    this.m_tableName = DBUtils.GetTableName(o);
                    //if (o is IDisposable)
                    //    (o as IDisposable).Dispose();
                }
                return this.m_tableName;
            }

            /// <summary>
            /// Restituise le clausole where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var ret = base.GetWherePart();
                ret = ret.And(this.GetWherePartLimit());
                return ret;
            }

            /// <summary>
            /// Restituisce i campi aggiuntivi per GetWherePart
            /// </summary>
            /// <returns></returns>
            protected virtual DBCursorFieldBase GetWherePartLimit()
            {
                return DBCursorFieldBase.True;
            }
        }
    }
}