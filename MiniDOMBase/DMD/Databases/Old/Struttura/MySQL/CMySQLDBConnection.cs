using System;
using System.Data;
using System.Runtime.InteropServices;


namespace minidom
{
    public partial class Databases
    {

        /// <summary>
        /// Driver MySQL
        /// </summary>
        public class CMySQLDBConnection 
            : CDBConnection
        {
            private string m_CharSet;


            /// <summary>
            /// Costruttore
            /// </summary>
            protected internal override void CompactInternal()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMySQLDBConnection()
            {
                m_CharSet = "utf8";
            }

            /// <summary>
            /// Restituisce o imposta la codifica predefinita
            /// </summary>
            public string CharSet
            {
                get
                {
                    return m_CharSet;
                }

                set
                {
                    m_CharSet = value;
                }
            }

            /// <summary>
            /// Cambia la password di accesso al database
            /// </summary>
            /// <param name="newPassword"></param>
            /// <param name="oldPassword"></param>
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
            /// Crea il database
            /// </summary>
            protected internal override void CreateInternal()
            {
                string cmd;
                cmd = "CREATE DATABASE " + Path + " DEFAULT CHARACTER SET " + m_CharSet + " COLLATE utf8_unicode_ci;";
            }

            /// <summary>
            /// Restituisce la stringa di connessione
            /// </summary>
            /// <returns></returns>
            public virtual string GetConnectionString()
            {
                // Dim ret As New System.Text.StringBuilder
                // ret.Append("Provider=" & Me.m_Provider & ";")
                // ret.Append("Data Source=" & Me.Path & ";")
                // If (Me.m_PersistSecurityInfo) Then ret.Append("Persist Security Info=" & Me.m_PersistSecurityInfo & ";")
                // Select Case Me.m_LockingMode
                // Case OleDbLockMode.RowLevelLock : ret.Append("Jet OLEDB:Database Locking Mode=1")
                // Case OleDbLockMode.TableLevelLock : ret.Append("Jet OLEDB:Database Locking Mode=0")
                // Case Else
                // End Select
                // Return ret.ToString
                throw new NotImplementedException();
            }

            /// <summary>
            /// Crea la connessione
            /// </summary>
            /// <returns></returns>
            public override IDbConnection CreateConnection()
            {
                // Dim builder As New OleDb.OleDbConnectionStringBuilder(GetConnectionString())
                // builder.AsynchronousProcessin= True


                var cn = new System.Data.OleDb.OleDbConnection();
                cn.ConnectionString = GetConnectionString();
                return cn;
            }

            protected internal override IDataAdapter CreateAdapterInternal(string selectCommand)
            {
                return new System.Data.OleDb.OleDbDataAdapter(selectCommand, (System.Data.OleDb.OleDbConnection)GetConnection());
            }

            protected internal override IDbCommand CreateCommandInternal(string sql)
            {
                var cmd = GetConnection().CreateCommand();
                cmd.CommandText = sql;
                return cmd;
            }

            protected override CDBTable[] GetTablesArray()
            {
                // We only want user tables, not system tables
                var restrictions = new string[] { DMD.Strings.vbNullString, DMD.Strings.vbNullString, DMD.Strings.vbNullString, "Table" };
                System.Data.OleDb.OleDbConnection oleDBConn = (System.Data.OleDb.OleDbConnection)GetConnection();
                var userTables = oleDBConn.GetSchema("Tables", restrictions);
                var ret = new CCollection<CDBTable>();
                foreach (DataRow dr in userTables.Rows)
                {
                    // System.Console.WriteLine(userTables.Rows(i)(2).ToString())
                    var table = new CDBTable();
                    table.Catalog = Sistema.Formats.ToString(dr["TABLE_CATALOG"]);
                    table.Schema = Sistema.Formats.ToString(dr["TABLE_SCHEMA"]);
                    table.Name = Sistema.Formats.ToString(dr["TABLE_NAME"]);
                    table.Type = Sistema.Formats.ToString(dr["TABLE_TYPE"]);
                    table.Guid = Sistema.Formats.ToString(dr["TABLE_GUID"]);
                    table.Description = Sistema.Formats.ToString(dr["DESCRIPTION"]);
                    table.PropID = Sistema.Formats.ToString(dr["TABLE_PROPID"]);
                    table.DateCreated = Sistema.Formats.ToDate(dr["DATE_CREATED"]);
                    table.DateModified = Sistema.Formats.ToDate(dr["DATE_MODIFIED"]);
                    table.SetChanged(false);
                    table.SetCreated(true);
                    table.SetConnection(this);
                    ret.Add(table);
                }

                return ret.ToArray();
            }

            public override IDbCommand GetUpdateCommand(object obj, string idFieldName, int idValue, DataRow dr, CKeyCollection<bool> changedValues)
            {
                string tableName = GetSaveTableName(obj);
                var table = Tables[tableName];
                DMDDBCommand cmd = (DMDDBCommand)table.GetUpdateCommand(changedValues);
                foreach (CDBEntityField dc in table.Fields)
                {
                    if (dc.AutoIncrement || changedValues[dc.Name])
                    {
                        System.Data.OleDb.OleDbParameter param;
                        param = (System.Data.OleDb.OleDbParameter)cmd.Parameters["@" + dc.Name];
                        param.Value = dr[dc.Name];
                    }
                }

                return cmd;
            }

            public override IDbCommand GetInsertCommand(object obj, string idFieldName, DataRow dr, [Optional, DefaultParameterValue(0)] ref int maxID)
            {
                // Dim ds As System.Data.DataSet
                // Dim dt As New System.Data.DataTable
                // Dim da As System.Data.IDataAdapter
                // Dim names, values As String
                string tableName = GetSaveTableName(obj);
                var table = Tables[tableName];
                DMDDBCommand cmd = (DMDDBCommand)table.GetInsertCommand();

                // da = Me.CreateAdapter("SELECT * FROM [" & tableName & "] WHERE (0<>0)")
                // ds = New System.Data.DataSet()
                // da.FillSchema(ds, SchemaType.Source)
                // dt = ds.Tables(0)
                // names = vbNullString
                // values = vbNullString
                // For Each dc As System.Data.DataColumn In dt.Columns
                // If dc.AutoIncrement = False Then
                // names = DMD.Strings.Combine(names, "[" & dc.ColumnName & "]", ",")
                // values = DMD.Strings.Combine(values, "@" & dc.ColumnName, ",")
                // End If
                // Next

                // Dim cmd As System.Data.OleDb.OleDbCommand = Me.CreateCommand("INSERT INTO [" & tableName & "] (" & names & ") VALUES (" & values & ")")
                System.Data.OleDb.OleDbParameter param;
                // For Each dc As System.Data.DataColumn In table.Fields ' dt.Columns
                foreach (CDBEntityField dc in table.Fields) // dt.Columns
                {
                    if (dc.AutoIncrement == false)
                    {
                        param = (System.Data.OleDb.OleDbParameter)cmd.Parameters["@" + dc.Name];
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


                // dt.Dispose()
                // ds.Dispose()

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
                            return Sistema.IIF(field.AutoIncrement, "COUNTER", "INT");
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

                    case TypeCode.Object:
                        {
                            return "IMAGE";
                        }

                    default:
                        {
                            throw new NotSupportedException();
                             
                        }
                }
            }

            public override string GetInternalTableName(CDBEntity table)
            {
                return "[" + table.Name + "]";
            }

            public override string GetFriendlyName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (DMD.Strings.Left(name, 1) == "[" & DMD.Strings.Right(name, 1) == "]")
                {
                    name = DMD.Strings.Mid(name, 2, DMD.Strings.Len(name) - 2);
                }

                return name;
            }

            protected internal override void CreateField(CDBEntityField field)
            {
                string dbSQL;
                dbSQL = "ALTER TABLE [" + field.Owner.Name + "] ADD COLUMN [" + field.Name + "] " + GetSqlDataType(field);
                ExecuteCommand(dbSQL);
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

            protected internal override void CreateView(CDBQuery view)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropField(CDBEntityField field)
            {
                string dbSQL;
                dbSQL = "ALTER TABLE [" + field.Owner.Name + "] DROP COLUMN [" + field.Name + "]";
                ExecuteCommand(dbSQL);
            }

            protected internal override void DropTable(CDBTable table)
            {
                string dbSQL;
                dbSQL = "DROP TABLE [" + table.Name + "]";
                ExecuteCommand(dbSQL);
            }

            protected internal override void DropView(CDBQuery view)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateField(CDBEntityField field)
            {
                string dbSQL;
                dbSQL = "ALTER TABLE [" + field.Owner.Name + "] CHANGE [" + field.Name + "] " + GetSqlDataType(field);
                ExecuteCommand(dbSQL);
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

            protected internal override void UpdateView(CDBQuery vuew)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateTableConstraint(CDBTableConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropTableConstraint(CDBTableConstraint c)
            {
                string dbSQL = "DROP INDEX [" + c.Name + "] ON [" + c.Owner.Name + "] ";
                ExecuteCommand(dbSQL);
            }

            protected internal override void UpdateTableConstraint(CDBTableConstraint c)
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