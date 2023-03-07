using System;
using System.Data;


namespace minidom
{
    public partial class Databases
    {
        public class CDBQueriesCollection : CKeyCollection<CDBQuery>
        {
            private CDBConnection m_Owner;

            public CDBQueriesCollection()
            {
            }

            public CDBQueriesCollection(CDBConnection connection) : this()
            {
                Initialize(connection);
            }

            public CDBConnection Connection
            {
                get
                {
                    return m_Owner;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CDBQuery)value).SetConnection(m_Owner);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Owner is object)
                    ((CDBQuery)newValue).SetConnection(m_Owner);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Inizializza la collezione
            /// </summary>
            /// <param name="conn"></param>
            protected internal void Initialize(CDBConnection conn)
            {
                if (conn is null)
                    throw new ArgumentNullException("conn");
                Clear();
                m_Owner = conn;
                // We only want user tables, not system tables
                var restrictions = new string[] { null, null, null, "View" };
                System.Data.OleDb.OleDbConnection oleDBConn = (System.Data.OleDb.OleDbConnection)conn.GetConnection();
                using (var userTables = oleDBConn.GetSchema("Tables", restrictions))
                {
                    foreach (DataRow dr in userTables.Rows)
                    {
                        // System.Console.WriteLine(userTables.Rows(i)(2).ToString())
                        var table = new CDBQuery();
                        table.Catalog = Sistema.Formats.ToString(dr["TABLE_CATALOG"]);
                        table.Schema = Sistema.Formats.ToString(dr["TABLE_SCHEMA"]);
                        table.Name = Sistema.Formats.ToString(dr["TABLE_NAME"]);
                        table.Type = Sistema.Formats.ToString(dr["TABLE_TYPE"]);
                        table.Guid = Sistema.Formats.ToString(dr["TABLE_GUID"]);
                        table.Description = Sistema.Formats.ToString(dr["DESCRIPTION"]);
                        table.PropID = Sistema.Formats.ToString(dr["TABLE_PROPID"]);
                        table.DateCreated = Sistema.Formats.ToDate(dr["DATE_CREATED"]);
                        table.DateModified = Sistema.Formats.ToDate(dr["DATE_MODIFIED"]);
                        Add(table.Name, table);
                    }
                }
            }

            protected internal virtual void SetOwner(CDBConnection owner)
            {
                m_Owner = owner;
                foreach (CDBQuery table in this)
                    table.SetConnection(owner);
            }
        }
    }
}