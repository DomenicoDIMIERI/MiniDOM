using System;
using System.Data;

namespace minidom
{
    public partial class Databases
    {
        public class CDBConstraintsCollection : CKeyCollection<CDBTableConstraint>
        {
            private CDBEntity m_Owner;

            public CDBConstraintsCollection()
            {
            }

            public CDBConstraintsCollection(CDBEntity owner) : this()
            {
                Initialize(owner);
            }

            public CDBEntity Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            protected internal void SetOwner(CDBEntity value)
            {
                m_Owner = value;
                foreach (CDBTableConstraint f in this)
                    f.SetOwner(value);
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Owner is object)
                    ((CDBTableConstraint)value).SetOwner(m_Owner);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Owner is object)
                    ((CDBTableConstraint)newValue).SetOwner(m_Owner);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal void Initialize(CDBEntity owner)
            {
                Clear();
                if (owner is null)
                    throw new ArgumentNullException("owner");
                m_Owner = owner;
                if (owner.Connection is null || owner.IsCreated() == false)
                    return;
                // Dim ds As System.Data.DataSet
                // Dim dt As New System.Data.DataTable
                // Dim da As System.Data.IDataAdapter

                // da = owner.Connection.CreateAdapter("SELECT * FROM [" & owner.Connection.GetTableName(owner.Name) & "] WHERE (0<>0)")
                // ds = New System.Data.DataSet()
                // da.FillSchema(ds, SchemaType.Source)
                // dt = ds.Tables(0)
                // For Each dc As System.Data.Constraint In dt.Constraints
                // Dim c As New CDBConstraint
                // c.Name = dc.ConstraintName
                // MyBase.Add(c.Name, c)
                // Next

                // dt.Dispose()
                // ds.Dispose()
                var restrictions = new string[] { null, null, null, null, m_Owner.Name };
                using (var indexDetails = ((System.Data.OleDb.OleDbConnection)m_Owner.Connection.GetConnection()).GetSchema("INDEXES", restrictions))
                {
                    foreach (DataRow row in indexDetails.Rows)
                    {
                        CDBTableConstraint c;
                        int i = IndexOfKey(Sistema.Formats.ToString(row["INDEX_NAME"]));
                        if (i >= 0)
                        {
                            c = this[i];
                        }
                        else
                        {
                            c = new CDBTableConstraint();
                            c.Name = Sistema.Formats.ToString(row["INDEX_NAME"]);
                            Add(c.Name, c);
                        }

                        c.Columns.Add(m_Owner.Fields[Sistema.Formats.ToString(row["COLUMN_NAME"])]);
                    }

                    // Dim columnsIWant = From row In indexDetails.AsEnumerable()
                    // Select
                    // TableName = row.Field(Of String)("TABLE_NAME"),
                    // IndexName = row.Field(Of String)("INDEX_NAME"),
                    // ColumnOrdinal = row.Field(Of Int64)("ORDINAL_POSITION"),
                    // ColumnName = row.Field(Of String)("COLUMN_NAME")
                    // Order By TableName, IndexName, ColumnOrdinal, ColumnName
                }
            }
        }
    }
}