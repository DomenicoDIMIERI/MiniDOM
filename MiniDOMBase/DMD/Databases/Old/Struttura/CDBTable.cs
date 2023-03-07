using System.Data;
using System;

namespace minidom
{
    public partial class Databases
    {


        /// <summary>
        /// Tabella del database
        /// </summary>
        [Serializable]
        public class CDBTable 
            : CDBEntity
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDBTable()
            {
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="tableName"></param>
            public CDBTable(string tableName) : base(tableName)
            {
                 
            }

            public IDbCommand GetUpdateCommand(CKeyCollection<bool> changedValues)
            {
                // If (Me.m_UpdateCommand IsNot Nothing) Then Me.m_UpdateCommand.Dispose()
                // Me.m_UpdateCommand = Me.GetNewUpdateCommand(changedValues)
                // Return Me.m_UpdateCommand
                return GetNewUpdateCommand(changedValues);
            }

            public IDbCommand GetInsertCommand()
            {
                // If Me.m_InsertCommand Is Nothing Then Me.m_InsertCommand = Me.GetNewInsertCommand
                // Return Me.m_InsertCommand
                return GetNewInsertCommand();
            }

            private IDbCommand GetNewUpdateCommand(CKeyCollection<bool> changedValues)
            {
                string names;
                string tableName = Name;
                string idFieldName = DMD.Strings.vbNullString;
                names = DMD.Strings.vbNullString;
                foreach (CDBEntityField dc in Fields)
                {
                    if (dc.AutoIncrement == false)
                    {
                        if (changedValues[dc.Name])
                        {
                            names = DMD.Strings.Combine(names, "[" + dc.Name + "]=@" + dc.Name, ",");
                        }
                    }
                    else
                    {
                        idFieldName = dc.Name;
                    }
                }

                DMDDBCommand cmd = (DMDDBCommand)Connection.CreateCommand("UPDATE [" + tableName + "] SET " + names + " WHERE [" + idFieldName + "]=@" + idFieldName);
                System.Data.OleDb.OleDbParameter param;
                foreach (CDBEntityField dc in Fields)
                {
                    if (dc.AutoIncrement == false && changedValues[dc.Name])
                    {
                        param = ((System.Data.OleDb.OleDbCommand)cmd.m_cmd).Parameters.Add("@" + dc.Name, DBUtils.GetOleDbDataType(dc.DataType));
                        // param.Direction = ParameterDirection.Output
                        param.IsNullable = dc.AllowDBNull;
                        param.SourceColumn = dc.Name;
                    }
                }

                foreach (CDBEntityField dc in Fields)
                {
                    if (dc.AutoIncrement == true)
                    {
                        param = ((System.Data.OleDb.OleDbCommand)cmd.m_cmd).Parameters.Add("@" + dc.Name, DBUtils.GetOleDbDataType(dc.DataType));
                        // param.Direction = ParameterDirection.Output
                        param.IsNullable = dc.AllowDBNull;
                        param.SourceColumn = dc.Name;
                    }
                }

                return cmd;
            }

            private IDbCommand GetNewInsertCommand()
            {
                // Dim ds As System.Data.DataSet
                // Dim dt As New System.Data.DataTable
                // Dim da As System.Data.IDataAdapter
                string names, values;
                string tableName = Name;
                string idFieldName;

                // da = Me.Connection.CreateAdapter("SELECT * FROM [" & tableName & "] WHERE (0<>0)")
                // ds = New System.Data.DataSet()
                // da.FillSchema(ds, SchemaType.Source)
                // dt = ds.Tables(0)
                names = DMD.Strings.vbNullString;
                values = DMD.Strings.vbNullString;
                // For Each dc As System.Data.DataColumn In dt.Columns
                foreach (CDBEntityField dc in Fields)
                {
                    if (dc.AutoIncrement == false)
                    {
                        names = DMD.Strings.Combine(names, "[" + dc.Name + "]", ",");
                        values = DMD.Strings.Combine(values, "@" + dc.Name, ",");
                    }
                    else
                    {
                        idFieldName = dc.Name;
                    }
                }

                DMDDBCommand cmd = (DMDDBCommand)Connection.CreateCommand("INSERT INTO [" + tableName + "] (" + names + ") VALUES (" + values + ")");
                System.Data.OleDb.OleDbParameter param;
                // For Each dc As System.Data.DataColumn In dt.Columns
                foreach (CDBEntityField dc in Fields)
                {
                    if (dc.AutoIncrement == false)
                    {
                        param = ((System.Data.OleDb.OleDbCommand)cmd.m_cmd).Parameters.Add("@" + dc.Name, DBUtils.GetOleDbDataType(dc.DataType));

                        // If (dc.ColumnName = idFieldName) Then
                        // maxID = Me.ExecuteScalar("SELECT 1 + Max([" & idFieldName & "]) FROM [" & tableName & "]")
                        // param.Value = maxID
                        // Else
                        // param.Value = dr(dc.ColumnName)
                        // End If
                    }
                }

                // dt.Dispose()
                // ds.Dispose()

                return cmd;
            }

            protected override void CreateInternal1()
            {
                Connection.CreateTable(this);
                foreach (var f in this.Fields)
                {
                    f.SetChanged(false);
                    f.SetCreated(true);
                }
            }

            protected override void DropInternal1()
            {
                Connection.DropTable(this);
                Connection.Tables.RemoveByKey(Name);
            }

            protected override void UpdateInternal1()
            {
                Connection.UpdateTable(this);
            }

            protected override void RenameItnernal(string newName)
            {
                var buffer = new System.Text.StringBuilder();
                buffer.Append("ALTER TABLE ");
                buffer.Append(InternalName);
                buffer.Append(" RENAME TO [");
                buffer.Append(newName);
                Connection.ExecuteCommand(buffer.ToString());
            }
        }
    }
}