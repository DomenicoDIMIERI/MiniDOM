using System;
using System.Data;
using System.Runtime.InteropServices;

namespace minidom
{
    public partial class Databases
    {
        public class XMLRemoteConnection
            : CDBConnection
        {
            private string m_Entry;

            public XMLRemoteConnection()
            {
                m_Entry = "";
            }

            public XMLRemoteConnection(string entry)
            {
                m_Entry = entry;
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

            public override object GetItemById(Sistema.CModule m, int id)
            {
                if (id == 0)
                    return null;
                if (m is null)
                    throw new ArgumentNullException("m");
                if (string.IsNullOrEmpty(m.ModuleName))
                    throw new ArgumentNullException("module name");
                string tmp = Sistema.RPC.InvokeMethod(m_Entry + "/websvc/" + m.ModuleName + ".aspx?_a=GetItemById", "id", Sistema.RPC.int2n(id));
                object ret = null;
                if (!string.IsNullOrEmpty(tmp))
                    ret = DMD.XML.Utils.Serializer.Deserialize(tmp);
                return ret;
            }

            protected internal override void CreateField(CDBEntityField field)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateInternal()
            {
                throw new NotImplementedException();
            }

            protected internal override void CreateTable(CDBTable table)
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            protected internal override void DropFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropTable(CDBTable table)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropTableConstraint(CDBTableConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void DropView(CDBQuery view)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateField(CDBEntityField field)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateFieldConstraint(CDBFieldConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateTable(CDBTable table)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateTableConstraint(CDBTableConstraint c)
            {
                throw new NotImplementedException();
            }

            protected internal override void UpdateView(CDBQuery vuew)
            {
                throw new NotImplementedException();
            }

            public override IDbConnection CreateConnection()
            {
                throw new NotImplementedException();
            }

            public override IDbCommand GetInsertCommand(object obj, string idFieldName, DataRow dr, [Optional, DefaultParameterValue(0)] ref int maxID)
            {
                throw new NotImplementedException();
            }

            public override string GetSqlDataType(CDBEntityField field)
            {
                throw new NotImplementedException();
            }

            public override IDbCommand GetUpdateCommand(object obj, string idFieldName, int idValue, DataRow dr, CKeyCollection<bool> changedValues)
            {
                throw new NotImplementedException();
            }

            protected override CDBTable[] GetTablesArray()
            {
                throw new NotImplementedException();
            }

            protected internal override IDataAdapter CreateAdapterInternal(string selectCommand)
            {
                throw new NotImplementedException();
            }

            protected internal override IDbCommand CreateCommandInternal(string sql)
            {
                throw new NotImplementedException();
            }

            public override void SaveObject(object obj, bool force)
            {
                lock (this)
                {
                    DBObjectBase o = (DBObjectBase)obj;
                    bool isNew = GetID(o) == 0;
                    string value = DMD.XML.Utils.Serializer.Serialize(o);
                    string Type = Sistema.Types.vbTypeName(o);
                    string ret = Sistema.RPC.InvokeMethod(m_Entry + "/websvc/modSistema.aspx?_a=SaveObject", "type", Sistema.RPC.str2n(Type), "value", Sistema.RPC.str2n(value));
                    int tmp = Sistema.RPC.ParseID(DMD.Strings.Left(ret, 8));
                    if (tmp != 0)
                    {
                        if (isNew)
                        {
                            DBUtils.SetID(o, tmp);
                        }
                    }
                    else
                    {
                        throw new Exception(ret);
                    }

                    o.SetChanged(false);
                }
            }

            public override void DeleteObject(object obj, bool force)
            {
                lock (this)
                {
                }
            }

            public override bool IsRemote()
            {
                return true;
            }

            public override string InvokeMethodArray(Sistema.CModule module, string methodName, object[] args)
            {
                if (module is null)
                    throw new ArgumentNullException("module");
                if (string.IsNullOrEmpty(module.ModuleName))
                    throw new ArgumentNullException("module name");
                return Sistema.RPC.InvokeMethod1(m_Entry + "/websvc/" + module.ModuleName + ".aspx?_a=" + methodName, args);
            }

            public override Sistema.AsyncState InvokeMethodArrayAsync(Sistema.CModule module, string methodName, object handler, object[] args)
            {
                if (module is null)
                    throw new ArgumentNullException("module");
                if (string.IsNullOrEmpty(module.ModuleName))
                    throw new ArgumentNullException("module name");
                return Sistema.RPC.InvokeMethodArrayAsync(m_Entry + "/websvc/" + module.ModuleName + ".aspx?_a=" + methodName, (Sistema.IRPCCallHandler)handler, args);
            }

            protected internal override void CompactInternal()
            {
                throw new NotImplementedException();
            }
        }
    }
}