using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace minidom
{
    public partial class Databases
    {
        
        [Serializable]
        public class CDBEntityFields 
            : IEnumerable<CDBEntityField>
        {

            private List<CDBEntityField> lst = new List<CDBEntityField>();
            private Dictionary<string, CDBEntityField> dic = new Dictionary<string, CDBEntityField>();
            [NonSerialized] private CDBEntity m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDBEntityFields()
            {
                 
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="entity"></param>
            public CDBEntityFields(CDBEntity entity) : this()
            {
                GetFields(entity);
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
                foreach (CDBEntityField f in this)
                    f.SetOwner(value);
            }

            public CDBEntityField this[int index]
            {
                get
                {
                    return this.lst[index];
                }
            }

            public CDBEntityField this[string fieldName]
            {
                get
                {
                    return this.dic[DMD.Strings.LCase(DMD.Strings.Trim(fieldName))];
                }
            }

           
            protected internal void GetFields(CDBEntity entity)
            {
                if (entity is null)
                    throw new ArgumentNullException("entity");
                
                lst.Clear();
                dic.Clear();
                
                m_Owner = entity;
                
                if (entity.Connection is null || !entity.IsCreated())
                    return;
                
                var da = entity.Connection.CreateAdapter("SELECT * FROM " + entity.InternalName + " WHERE (0<>0)");
                using (var ds = new DataSet())
                {
                    da.FillSchema(ds, SchemaType.Source);
                    if (ds.Tables.Count > 0)
                    {
                        using (var dt = ds.Tables[0])
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                var field = new CDBEntityField();
                                field.DataType = dc.DataType;
                                field.Name = dc.ColumnName;
                                field.MaxLength = dc.MaxLength;
                                field.AutoIncrement = dc.AutoIncrement;
                                field.AutoIncrementSeed = (int)dc.AutoIncrementSeed;
                                field.AutoIncrementStep = (int)dc.AutoIncrementStep;
                                field.AllowDBNull = dc.AllowDBNull;
                                field.Caption = dc.Caption;
                                field.ColumnMapping = ((int)dc.ColumnMapping).ToString();
                                field.DefaultValue = dc.DefaultValue;
                                field.Expression = dc.Expression;
                                field.Namespace = dc.Namespace;
                                field.SetOrdinal(dc.Ordinal);
                                field.Prefix = dc.Prefix;
                                field.ReadOnly = dc.ReadOnly;
                                field.Unique = dc.Unique;
                                field.SetChanged(false);
                                field.SetCreated(true);
                                Add(field);
                            }

                        }
                    }

                }
            }

            /// <summary>
            /// Rimuove la tabella
            /// </summary>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            public CDBEntityField RemoveByKey(string fieldName)
            {
                var table = this.GetItemByKey(fieldName);
                this.lst.Remove(table);
                this.dic.Remove(DMD.Strings.LCase(DMD.Strings.Trim(fieldName)));
                return table;
            }

            /// <summary>
            /// Rimuove la tabella
            /// </summary>
            /// <param name="field"></param>
            /// <returns></returns>
            public void Remove(CDBEntityField field)
            {
                this.dic.Remove(DMD.Strings.LCase(DMD.Strings.Trim(field.Name)));
                this.lst.Remove(field);
            }

            public CDBEntityField GetItemByKey(string fieldName)
            {
                CDBEntityField ret = null;
                this.dic.TryGetValue(DMD.Strings.LCase(DMD.Strings.Trim(fieldName)), out ret);
                return ret;
            }

            public new void Add(CDBEntityField item)
            {
                this.dic.Add(DMD.Strings.LCase(DMD.Strings.Trim(item.Name)), item);
                this.lst.Add(item);
                if (this.m_Owner is object)
                {
                    item.SetOwner(this.m_Owner);
                    item.SetConnection(this.m_Owner.Connection);
                }
            }

            public new CDBEntityField Add(string fieldName, Type fieldType)
            {
                var field = new CDBEntityField(fieldName, fieldType);
                Add(field);
                return field;
            }

            public new CDBEntityField Add(string fieldName, TypeCode fieldType)
            {
                var field = new CDBEntityField(fieldName, fieldType);
                Add(field);
                return field;
            }

            public CDBEntityField Alter(string fieldName, Type valueType)
            {
                var col = GetItemByKey(fieldName);
                if (col is null)
                    col = Add(fieldName, valueType);
                col.DataType = valueType;
                return col;
            }

            public CDBEntityField Alter(string fieldName, Type valueType, int maxLen)
            {
                var col = GetItemByKey(fieldName);
                if (col is null)
                    col = Add(fieldName, valueType);
                col.DataType = valueType;
                col.MaxLength = maxLen;
                return col;
            }

            public CDBEntityField Ensure(string fieldName, Type valueType, int maxLen )
            {
                return this.Alter(fieldName, valueType, maxLen);
            }

            public IEnumerator<CDBEntityField> GetEnumerator()
            {
                return this.lst.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.lst.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this.lst.Count;
                }
            }

            /// <summary>
            /// Restituisce vero se la collezione contiene la tabella con nome
            /// </summary>
            /// <param name="fieldName"></param>
            /// <returns></returns>
            public bool ContainsKey(string fieldName)
            {
                return this.dic.ContainsKey(DMD.Strings.LCase(DMD.Strings.Trim(fieldName)));
            }

        }
    }
}