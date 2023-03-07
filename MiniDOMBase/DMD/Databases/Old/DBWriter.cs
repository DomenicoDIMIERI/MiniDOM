using System;
using System.Data;



namespace minidom
{
    public partial class Databases
    {
        public sealed class DBWriter : IDisposable
        {
            private CDBConnection m_Connection;
            private DataRow m_dbRis;
            private CKeyCollection m_OldValues = new CKeyCollection();
            private string m_Prefix = DMD.Strings.vbNullString;
            private CDBEntity m_Schema;

            public DBWriter(CDBEntity schema, DataRow dbRis, string prefix = DMD.Strings.vbNullString)
            {
                DMDObject.IncreaseCounter(this);
                if (schema is null)
                    throw new ArgumentNullException("schema");
                if (dbRis is null)
                    throw new ArgumentNullException("dbRis");
                m_Connection = schema.Connection;
                m_Schema = schema;
                m_dbRis = dbRis;
                m_Prefix = DMD.Strings.Trim(prefix);
                if (Schema.TrackChanges)
                {
                    foreach (DataColumn c in m_dbRis.Table.Columns)
                        m_OldValues.Add(prefix + c.ColumnName, m_dbRis[prefix + c.ColumnName]);
                }
            }

            public CDBConnection Connection
            {
                get
                {
                    return m_Connection;
                }
            }

            public CDBEntity Schema
            {
                get
                {
                    return m_Schema;
                }
            }

            public string Prefix
            {
                get
                {
                    return m_Prefix;
                }

                set
                {
                    m_Prefix = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
        /// Informa l'oggetto che il record è stato salvato
        /// </summary>
        /// <remarks></remarks>
            public void Update(int id)
            {
                if (id == 0)
                    throw new ArgumentOutOfRangeException("id");
                if (Schema is object && Schema.TrackChanges)
                {
                    var data = DMD.DateUtils.Now();
                    var changesFields = GetChangedFields();
                    var cmd = LOGConn.Tables["tbl_ChangesHistory"].GetInsertCommand();
                    int userID = GetID(Sistema.Users.CurrentUser);
                    if (CheckEquality(m_dbRis["ID"], id) == false)
                    {
                        ((IDbDataParameter)cmd.Parameters["@Utente"]).Value = userID;
                        ((IDbDataParameter)cmd.Parameters["@DataModifica"]).Value = data;
                        ((IDbDataParameter)cmd.Parameters["@Tabella"]).Value = m_Schema.Name;
                        ((IDbDataParameter)cmd.Parameters["@IDOggetto"]).Value = id;
                        ((IDbDataParameter)cmd.Parameters["@NomeCampo"]).Value = "ID";
                        ((IDbDataParameter)cmd.Parameters["@OldValue"]).Value = m_dbRis["ID"];
                        ((IDbDataParameter)cmd.Parameters["@NewValue"]).Value = id;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (PropertyChangedEventArgs p in changesFields)
                    {
                        ((IDbDataParameter)cmd.Parameters["@Utente"]).Value = userID;
                        ((IDbDataParameter)cmd.Parameters["@DataModifica"]).Value = data;
                        ((IDbDataParameter)cmd.Parameters["@Tabella"]).Value = m_Schema.Name;
                        ((IDbDataParameter)cmd.Parameters["@IDOggetto"]).Value = id;
                        ((IDbDataParameter)cmd.Parameters["@NomeCampo"]).Value = p.PropertyName;
                        ((IDbDataParameter)cmd.Parameters["@OldValue"]).Value = "" + Sistema.Formats.ToString(p.OldValue);
                        ((IDbDataParameter)cmd.Parameters["@NewValue"]).Value = "" + Sistema.Formats.ToString(p.NewValue);
                        cmd.ExecuteNonQuery();
                    }

                    m_OldValues.Clear();
                    foreach (DataColumn c in m_dbRis.Table.Columns)
                        m_OldValues.Add(Prefix + c.ColumnName, m_dbRis[Prefix + c.ColumnName]);
                }
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public void Write(string fieldName, string value)
            {
                if (m_Schema is object)
                {
                    var f = m_Schema.Fields.GetItemByKey(fieldName);
                    if (f is null)
                        throw new MissingFieldException(m_Schema.Name, fieldName);
                    if (string.IsNullOrEmpty(value))
                    {
                        if (f.AllowDBNull == false)
                            throw new InvalidConstraintException("Il campo [" + m_Schema.Name + "." + fieldName + "] non può essere NULL");
                        m_dbRis[m_Prefix + fieldName] = DBNull.Value;
                    }
                    else if (f.MaxLength > 0 && DMD.Strings.Len(value) > f.MaxLength)
                    {
                        throw new ArgumentOutOfRangeException("Il campo [" + m_Schema.Name + "." + fieldName + "] non può superare " + f.MaxLength + " caratteri");
                    }
                }

                m_dbRis[m_Prefix + fieldName] = value;
            }

            public void Write<T>(string fieldName, T value)
            {
                m_dbRis[m_Prefix + fieldName] = value;
            }

            public void Write(string fieldName, int[] value)
            {
                if (value is object)
                {
                    m_dbRis[m_Prefix + fieldName] = DMD.Arrays.IntegersToBytes(value);
                }
                else
                {
                    m_dbRis[m_Prefix + fieldName] = DBNull.Value;
                }
            }

            public void Write<T>(string fieldName, T? value) where T : struct
            {
                if (value.HasValue)
                {
                    m_dbRis[m_Prefix + fieldName] = value;
                }
                else
                {
                    m_dbRis[m_Prefix + fieldName] = DBNull.Value;
                }
            }

            public void Write(string fieldName, Array value)
            {
                if (value is null)
                {
                    m_dbRis[m_Prefix + fieldName] = DBNull.Value;
                }
                else
                {
                    m_dbRis[m_Prefix + fieldName] = value;
                }
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            public CCollection<PropertyChangedEventArgs> GetChangedFields()
            {
                var ret = new CCollection<PropertyChangedEventArgs>();
                if (Schema is object && Schema.TrackChanges)
                {
                    foreach (DataColumn c in m_dbRis.Table.Columns)
                    {
                        var value = m_dbRis[m_Prefix + c.ColumnName];
                        var oldValue = m_OldValues[m_Prefix + c.ColumnName];
                        if (CheckEquality(value, oldValue) == false)
                            ret.Add(new PropertyChangedEventArgs(m_Prefix + c.ColumnName, value, oldValue));
                    }
                }

                return ret;
            }

            protected bool CheckEquality(object a, object b)
            {
                if (a is DBNull)
                {
                    if (b is DBNull)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (b is DBNull)
                {
                    return false;
                }
                else if (a is ValueType)
                {
                    if (b is ValueType)
                    {
                        return a.Equals(b);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (b is ValueType)
                {
                    return false;
                }
                else if (a is string && b is string)
                {
                    return DMD.Strings.Compare(DMD.Strings.CStr(a), DMD.Strings.CStr(b), false) == 0;
                }
                else
                {
                    return false;
                }
            }

            // Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            public void Dispose()
            {
                m_Connection = null;
                m_dbRis = null;
                m_OldValues = null;
                m_Prefix = DMD.Strings.vbNullString;
                m_Schema = null;
            }

            ~DBWriter()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}