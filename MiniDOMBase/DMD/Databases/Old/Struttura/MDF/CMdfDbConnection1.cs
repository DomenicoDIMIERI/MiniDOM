using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;


namespace minidom
{
    public partial class Databases
    {
        public class CMdfDbConnection 
            : CDBConnection
        {

            // Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\temp\WK_FINSAB2_data.mdf;Integrated Security=True;Connect Timeout=30
            private bool m_UseLocalDB = true;
            private string m_DataSource = @".\SQLEXPRESS";
            private int m_ConnectionTimeout = 30;
            private bool m_IntegratedSecurity = true;


            protected internal override void CompactInternal()
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// Restituisce o imposta il nome dell'istanza di SQL Server a cui connettersi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DataSouce
            {
                get
                {
                    return m_DataSource;
                }

                set
                {
                    m_DataSource = value;
                }
            }

            protected override void ChangeDatabasePasswordInternal(string newPassword, string oldPassword)
            {
                var builder = new System.Text.StringBuilder();
                builder.Append("ALTER ");
                builder.Append("DATABASE ");
                builder.Append("PASSWORD ");
                builder.Append(str2db(newPassword));
                builder.Append(" ");
                builder.Append(str2db(oldPassword));
                ExecuteCommand(builder.ToString());
            }

            /// <summary>
        /// Restituisce o imposta il valore in secondi del timeout per il tentativo di connessione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ConnectionTimeOut
            {
                get
                {
                    return m_ConnectionTimeout;
                }

                set
                {
                    m_ConnectionTimeout = value;
                }
            }

            public bool IntegratedSecurity
            {
                get
                {
                    return m_IntegratedSecurity;
                }

                set
                {
                    m_IntegratedSecurity = value;
                }
            }

            public bool UseLocalDB
            {
                get
                {
                    return m_UseLocalDB;
                }

                set
                {
                    m_UseLocalDB = value;
                }
            }

            public virtual string GetConnectionString()
            {
                var str = new System.Text.StringBuilder();
                if (m_UseLocalDB)
                {
                    str.Append(@"Server=(LocalDB)\v11.0;");
                }
                else
                {
                    str.Append("Data Source=" + m_DataSource + ";");
                    str.Append("Database='" + Sistema.FileSystem.GetBaseName(Path) + "';");
                    str.Append("Connect Timeout=" + m_ConnectionTimeout + ";");
                }

                str.Append("AttachDbFilename=" + Path + ";");
                str.Append("Integrated Security=" + Sistema.IIF(m_IntegratedSecurity, "True", "False") + ";");
                return str.ToString();
            }

            protected internal override IDataAdapter CreateAdapterInternal(string selectCommand)
            {
                return new SqlDataAdapter(selectCommand, (SqlConnection)GetConnection());
            }

            protected internal override IDbCommand CreateCommandInternal(string sql)
            {
                var cmd = GetConnection().CreateCommand();
                cmd.CommandText = sql;
                return cmd;
            }

            public override IDbConnection CreateConnection()
            {
                var cn = new SqlConnection();
                cn.ConnectionString = GetConnectionString();
                return cn;
            }

            protected internal override void CreateField(CDBEntityField field)
            {
                string dbSQL;
                dbSQL = "ALTER TABLE [" + field.Owner.Name + "] ADD COLUMN [" + field.Name + "] " + GetSqlDataType(field);
                ExecuteCommand(dbSQL);
            }

            protected internal override void CreateFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateTable(CDBTable table)
            {
                string sql;
                bool t = false;
                sql = DMD.Strings.vbNullString;
                sql += "CREATE TABLE [" + table.Name + "] (";
                foreach (CDBEntityField field in table.Fields)
                {
                    if (t)
                        sql += ",";
                    sql += "[" + field.Name + "] " + GetSqlDataType(field);
                    t = true;
                }

                sql += ")";
                ExecuteCommand(sql);
            }

            protected internal override void CreateTableConstraint(CDBTableConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateView(CDBQuery view)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropField(CDBEntityField field)
            {
                var dbSQL = new System.Text.StringBuilder();
                dbSQL.Append("ALTER TABLE [");
                dbSQL.Append(field.Owner.Name);
                dbSQL.Append("] DROP COLUMN [");
                dbSQL.Append(field.Name);
                dbSQL.Append("]");
                ExecuteCommand(dbSQL.ToString());
            }

            protected internal override void DropFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropTable(CDBTable table)
            {
                string dbSQL;
                dbSQL = "DROP TABLE [" + table.Name + "]";
                ExecuteCommand(dbSQL);
            }

            protected internal override void DropTableConstraint(CDBTableConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropView(CDBQuery view)
            {
                throw new NotImplementedException();
            }

            public override IDbCommand GetInsertCommand(object obj, string idFieldName, DataRow dr, [Optional, DefaultParameterValue(0)] ref int maxID)
            {
                string tableName = GetSaveTableName(obj);
                var table = Tables[tableName];
                SqlCommand cmd = (SqlCommand)table.GetInsertCommand();
                SqlParameter param;
                foreach (CDBEntityField dc in table.Fields) // dt.Columns
                {
                    if (dc.AutoIncrement == false)
                    {
                        param = cmd.Parameters["@" + dc.Name];
                        if ((dc.Name ?? "") == (idFieldName ?? ""))
                        {
                            maxID = DMD.Integers.CInt(ExecuteScalar("SELECT Max([" + idFieldName + "]) FROM [" + tableName + "]"));
                            param.Value = maxID + 1;
                        }
                        else
                        {
                            param.Value = dr[dc.Name];
                        }
                    }
                }

                return cmd;
            }

            public override string GetSqlDataType(CDBEntityField field)
            {
                switch (Sistema.Types.GetTypeCode(field.DataType))
                {
                    case TypeCode.Boolean:
                        {
                            return "BIT";
                        }

                    case TypeCode.Byte:
                        {
                            return "TINYINT";
                        }

                    case TypeCode.Char:
                        {
                            return "CHAR";
                        }

                    case TypeCode.DateTime:
                        {
                            return "DATE";
                        }
                    // Case TypeCode.DBNull : Return "
                    case TypeCode.Decimal:
                        {
                            return "DECIMAL";
                        }

                    case TypeCode.Double:
                        {
                            return "REAL";
                        }
                    // Case TypeCode.Empty : Return Nothing
                    case TypeCode.Int16:
                        {
                            return "SHORT";
                        }

                    case TypeCode.Int32:
                        {
                            return Sistema.IIF(field.AutoIncrement, "Autonumber", "INT");
                        }

                    case TypeCode.Int64:
                        {
                            return "LONG";
                        }
                    // Case TypeCode.Object : Return value
                    case TypeCode.SByte:
                        {
                            return "TINYINT";
                        }

                    case TypeCode.Single:
                        {
                            return "SINGLE";
                        }

                    case TypeCode.String:
                        {
                            if (field.MaxLength == 0)
                            {
                                return "MEMO";
                            }
                            else
                            {
                                return "TEXT(" + field.MaxLength + ")";
                            }

                             
                        }

                    case TypeCode.UInt16:
                        {
                            return "SHORT";
                        }

                    case TypeCode.UInt32:
                        {
                            return "INT";
                        }

                    case TypeCode.UInt64:
                        {
                            return "LONG";
                        }

                    default:
                        {
                            throw new NotSupportedException();
                             
                        }
                }
            }

            protected override CDBTable[] GetTablesArray()
            {
                // We only want user tables, not system tables
                var restrictions = new string[] { DMD.Strings.vbNullString, DMD.Strings.vbNullString, DMD.Strings.vbNullString, "Table" };
                SqlConnection oleDBConn = (SqlConnection)GetConnection();
                var userTables = oleDBConn.GetSchema("Tables"); // , restrictions)
                var ret = new CCollection<CDBTable>();
                foreach (DataRow dr in userTables.Rows)
                {
                    // System.Console.WriteLine(userTables.Rows(i)(2).ToString())
                    var table = new CDBTable();
                    table.Catalog = Sistema.Formats.ToString(GetRowField(dr, "TABLE_CATALOG"));
                    table.Schema = Sistema.Formats.ToString(GetRowField(dr, "TABLE_SCHEMA"));
                    table.Name = Sistema.Formats.ToString(GetRowField(dr, "TABLE_NAME"));
                    table.Type = Sistema.Formats.ToString(GetRowField(dr, "TABLE_TYPE"));
                    table.Guid = Sistema.Formats.ToString(GetRowField(dr, "TABLE_GUID"));
                    table.Description = Sistema.Formats.ToString(GetRowField(dr, "DESCRIPTION"));
                    table.PropID = Sistema.Formats.ToString(GetRowField(dr, "TABLE_PROPID"));
                    table.DateCreated = Sistema.Formats.ToDate(GetRowField(dr, "DATE_CREATED"));
                    table.DateModified = Sistema.Formats.ToDate(GetRowField(dr, "DATE_MODIFIED"));
                    table.SetChanged(false);
                    table.SetCreated(true);
                    table.SetConnection(this);
                    ret.Add(table);
                }

                return ret.ToArray();
            }

            private object GetRowField(DataRow dr, string fieldName)
            {
            
                    return dr[fieldName];
                 
            }

            public override IDbCommand GetUpdateCommand(object obj, string idFieldName, int idValue, DataRow dr, CKeyCollection<bool> changedValues)
            {
                string tableName = GetSaveTableName(obj);
                var table = Tables[tableName];
                SqlCommand cmd = (SqlCommand)table.GetUpdateCommand(changedValues);
                foreach (CDBEntityField dc in table.Fields)
                {
                    if (dc.AutoIncrement || changedValues[dc.Name])
                    {
                        SqlParameter param;
                        param = cmd.Parameters["@" + dc.Name];
                        param.Value = dr[dc.Name];
                    }
                }

                return cmd;
            }

            protected internal override void UpdateField(CDBEntityField field)
            {
                string dbSQL;
                dbSQL = "ALTER TABLE [" + field.Owner.Name + "] CHANGE [" + field.Name + "] " + GetSqlDataType(field);
                ExecuteCommand(dbSQL);
            }

            protected internal override void UpdateFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateTable(CDBTable table)
            {
                foreach (CDBEntityField field in table.Fields)
                {
                    if (field.IsCreated() == false)
                    {
                        field.Create();
                    }
                    else if (field.IsChanged())
                    {
                        field.Update();
                    }
                }
            }

            protected internal override void UpdateTableConstraint(CDBTableConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateView(CDBQuery vuew)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateInternal()
            {
                throw new NotImplementedException();
            }

            public override string InvokeMethodArray(Sistema.CModule module, string methodName, object[] args)
            {
                throw new NotImplementedException();
            }

            public override Sistema.AsyncState InvokeMethodArrayAsync(Sistema.CModule module, string methodName, object handler, object[] args)
            {
                throw new NotImplementedException();
            }
        }
    }
}