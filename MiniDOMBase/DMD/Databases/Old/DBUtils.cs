using System;
using System.Collections;
using System.Diagnostics;

using DMD;
using minidom.internals;
using static minidom.Databases;

namespace minidom
{
    namespace internals
    {
        public sealed class CDBUtilsClass
        {
            public event ConnectionOpenedEventHandler ConnectionOpened;

            public delegate void ConnectionOpenedEventHandler(object sender, DBEventArgs e);

            public event ConnectionClosedEventHandler ConnectionClosed;

            public delegate void ConnectionClosedEventHandler(object sender, DBEventArgs e);

            public event ConnectionErrorEventHandler ConnectionError;

            public delegate void ConnectionErrorEventHandler(object sender, DBEventArgs e);

            public event CursorOpenedEventHandler CursorOpened;

            public delegate void CursorOpenedEventHandler(object sender, Databases.DBCursorEventArgs e);

            public event CursorClosedEventHandler CursorClosed;

            public delegate void CursorClosedEventHandler(object sender, Databases.DBCursorEventArgs e);

            private object lockObject = new object();
            private CCollection<Databases.CDBConnection> m_OpenedConnections = new CCollection<Databases.CDBConnection>();
            private static bool m_StopStatistics = true;
            private CCollection<StatsItem> m_PendingQueries = new CCollection<StatsItem>();

            public CDBUtilsClass()
            {
                DMDObject.IncreaseCounter(this);
            }

            public bool StopStatistics
            {
                get
                {
                    return m_StopStatistics;
                }

                set
                {
                    m_StopStatistics = value;
                }
            }

            public CCollection<Databases.CDBConnection> GetAllOpenedConnections()
            {
                lock (lockObject)
                    return new CCollection<Databases.CDBConnection>(m_OpenedConnections);
            }

            internal void doConnectionOpened(Databases.DBEventArgs e)
            {
                lock (lockObject)
                    m_OpenedConnections.Add(e.Connection);
                ConnectionOpened?.Invoke(null, e);
            }

            internal void doConnectionClosed(Databases.DBEventArgs e)
            {
                lock (lockObject)
                    m_OpenedConnections.Remove(e.Connection);
                ConnectionClosed?.Invoke(null, e);
            }

            internal void doConnectionError(Databases.DBEventArgs e)
            {
                ConnectionError?.Invoke(null, e);
            }

            internal void doCursorOpened(Databases.DBCursorEventArgs e)
            {
                CursorOpened?.Invoke(null, e);
            }

            public void SetConnection(Databases.DBObjectCursorBase cursor, Databases.CDBConnection conn)
            {
                cursor.SetConnection(conn);
            }

            internal void doCursorClosed(Databases.DBCursorEventArgs e)
            {
                CursorClosed?.Invoke(null, e);
            }

            public void PrepareDBSchemaFor(object obj)
            {
                var conn = GetConnection(obj);
                this.PrepareDBSchemaFor(obj, conn);
            }

            public void PrepareDBSchemaFor(object obj, Databases.CDBConnection conn)
            {
                ((DBObjectBase)obj).PrepareDBSchema(conn);
            }


            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public string MakeArrayStr(int[] items, string separator = ",")
            {
                var ret = new System.Text.StringBuilder();
                for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                {
                    if (i > 0)
                        ret.Append(separator);
                    ret.Append(DBNumber(items[i]));
                }

                return ret.ToString();
            }

            public string MakeArrayStr(double[] items, string separator = ",")
            {
                var ret = new System.Text.StringBuilder();
                for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                {
                    if (i > 0)
                        ret.Append(separator);
                    ret.Append(DBNumber(items[i]));
                }

                return ret.ToString();
            }

            public string MakeArrayStr(string[] items, string separator = ",")
            {
                var ret = new System.Text.StringBuilder();
                for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                {
                    if (i > 0)
                        ret.Append(separator);
                    ret.Append(DBString(items[i]));
                }

                return ret.ToString();
            }

            public string MakeArrayStr(bool[] items, string separator = ",")
            {
                var ret = new System.Text.StringBuilder();
                for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                {
                    if (i > 0)
                        ret.Append(separator);
                    ret.Append(DBBool(items[i]));
                }

                return ret.ToString();
            }

            public void PrepareSchema(object obj)
            {
                var con = GetConnection(obj);
                this.PrepareDBSchemaFor(obj, con);
            }

            public void PrepareSchema(object obj, CDBConnection conn)
            {
                ((DBObjectBase)obj).PrepareDBSchema(conn);
            }

            public string MakeArrayStr(DateTime[] items, string separator = ",")
            {
                var ret = new System.Text.StringBuilder();
                for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                {
                    if (i > 0)
                        ret.Append(separator);
                    ret.Append(DBDate(items[i]));
                }

                return ret.ToString();
            }

            /// <summary>
        /// Restituisce il valore dell'oggetto o DBNull se l'oggetto è vuoto
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public object ToDB<T>(T? value) where T : struct
            {
                if (value.HasValue)
                    return value.Value;
                return DBNull.Value;
            }

            /// <summary>
        /// Restituisce il valore della stringa o DBNull se la stringa è nulla
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public object ToDB(string value)
            {
                return value;
            }

            public T? FromDB<T>(object value) where T : struct
            {
                if (value is DBNull)
                    return default;
                return (T)value;
            }

            public string FromDB(object value)
            {
                if (value is DBNull)
                    return null;
                return DMD.Strings.CStr(value);
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            /// <summary>
        /// Restituisce la data nel formato MM/DD/YYYY HH.NN.SS
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DBDate(object value)
            {
                if (value is DBNull || value is null)
                {
                    return "NULL";
                }
                else if (value is DateTime? && ((DateTime?)value).HasValue == false)
                {
                    return "NULL";
                }
                else if (value is NDate && ((NDate)value).HasValue() == false)
                {
                    return "NULL";
                }
                else
                {
                    DateTime d = DMD.DateUtils.CDate(value);
                    var ret = new System.Text.StringBuilder();
                    ret.Append("#");
                    ret.Append(DMD.DateUtils.Month(d));
                    ret.Append("/");
                    ret.Append(DMD.DateUtils.Day(d));
                    ret.Append("/");
                    ret.Append(DMD.DateUtils.Year(d));
                    ret.Append(" ");
                    ret.Append(DMD.DateUtils.Hour(d));
                    ret.Append(":");
                    ret.Append(DMD.DateUtils.Minute(d));
                    ret.Append(":");
                    ret.Append(DMD.DateUtils.Second(d));
                    ret.Append("#");
                    return ret.ToString();
                }
            }

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /// <summary>
        /// Restituisce il formato SQL del numero
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DBNumber(object value)
            {
                if (value is DBNull || value is null) return "NULL";
                return DMD.Longs.FormatUSA(value); //.Trim(DMD.Strings.Replace(DMD.Strings.CStr(value), ",", "."));

            }



            // ''' <summary>
            // ''' Funzione utilizzata dagli oggetti per salvare i dati in una tabella
            // ''' </summary>
            // ''' <param name="dbConn"></param>
            // ''' <param name="obj"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public  Function SaveToDatabase( _
            // ByVal dbConn As System.Data.IDbConnection, _
            // ByVal obj As Object, _
            // Optional ByVal idField As String = "ID" _
            // ) As Integer
            // If TypeOf (obj) Is IDBObjectBase Then
            // Return SaveToDatabaseObj(dbConn, obj, idField)
            // ElseIf TypeOf (obj) Is IDBMinObject Then
            // Return DirectCast(obj, IDBMinObject).SaveToDatabase(dbConn)
            // Else
            // Throw New ArgumentException("L'oggetto non implementa alcuna interfaccia valida")
            // Return False
            // End If
            // End Function








            public string PrepareWhereClause1(string fieldName, string searchString, string matchExact)
            {
                // Dim ret As String
                int i, j;
                string[] operands;
                string[] operators;
                int status;
                string ch;
                searchString = DMD.Strings.Trim(searchString);
                i = 1;
                operands = new string[1];
                operators = new string[1];
                status = 0;
                operands[0] = "";
                operators[0] = "";
                while (i <= DMD.Strings.Len(searchString))
                {
                    ch = DMD.Strings.Mid(searchString, i, 1);
                    switch (status)
                    {
                        case 0:
                            {
                                switch (ch ?? "")
                                {
                                    case DMD.Strings.vbDoubleQuote: // Inizio stringa
                                        {
                                            status = 1;
                                            if (i > 1)
                                            {
                                                operators[DMD.Arrays.UBound(operators)] = "+";
                                                Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                                Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                                operands[DMD.Arrays.UBound(operands)] = "";
                                            }

                                            break;
                                        }

                                    case "&":    // Operatore logico AND
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "*";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = "";
                                            break;
                                        }

                                    case "|":
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = "";
                                            break;
                                        }

                                    case " ":
                                        {
                                            status = 2;
                                            break;
                                        }

                                    default:
                                        {
                                            operands[DMD.Arrays.UBound(operands)] = operands[DMD.Arrays.UBound(operands)] + ch;
                                            break;
                                        }
                                }

                                break;
                            }

                        case 1:
                            {
                                if (ch == DMD.Strings.CStr('"'))
                                {
                                    status = 0;
                                }
                                else
                                {
                                    operands[DMD.Arrays.UBound(operands)] = operands[DMD.Arrays.UBound(operands)] + ch;
                                }

                                break;
                            }

                        case 2:
                            {
                                switch (ch ?? "")
                                {
                                    case " ":
                                        {
                                            break;
                                        }

                                    case "|":
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            status = 3;
                                            break;
                                        }

                                    case "&":
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "*";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            status = 3;
                                            break;
                                        }

                                    case DMD.Strings.vbDoubleQuote:
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            status = 1;
                                            break;
                                        }

                                    default:
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = ch;
                                            status = 0;
                                            break;
                                        }
                                }

                                break;
                            }

                        case 3:
                            {
                                switch (ch ?? "")
                                {
                                    case " ":
                                        {
                                            break;
                                        }

                                    default:
                                        {
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = ch;
                                            status = 0;
                                            break;
                                        }
                                }

                                break;
                            }
                    }

                    i = i + 1;
                }

                var loopTo = DMD.Arrays.UBound(operands);
                for (i = 0; i <= loopTo; i++)
                {
                    if (DMD.Booleans.CBool(matchExact))
                    {
                        operands[i] = DMD.Strings.JoinW("[", fieldName, "]='", DMD.Strings.Replace(operands[i], "'", "''"), "'");
                    }
                    else
                    {
                        operands[i] = DMD.Strings.JoinW("[", fieldName, "] Like '%", DMD.Strings.Replace(operands[i], "'", "''"), "%'");
                    }
                }

                i = DMD.Arrays.UBound(operands);
                for (j = DMD.Arrays.UBound(operators) - 1; j >= 0; j -= 1)
                {
                    switch (operators[j] ?? "")
                    {
                        case "+":
                            {
                                if (DMD.Arrays.UBound(operands) >= i)
                                {
                                    operands[i - 1] = DMD.Strings.JoinW("(", operands[i - 1], ") Or (", operands[i], ")");
                                    i = i - 1;
                                }
                                else
                                {
                                    operands[i - 1] = operands[i];
                                }

                                break;
                            }

                        case "*":
                            {
                                if (DMD.Arrays.UBound(operands) >= i)
                                {
                                    operands[i - 1] = DMD.Strings.JoinW("(", operands[i - 1], ") And (", operands[i], ")");
                                    i = i - 1;
                                }
                                else
                                {
                                    operands[i - 1] = operands[i];
                                }

                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }

                return operands[0];
            }

            public void CreateTable(Databases.CDBConnection db, string dbSQL)
            {
                db.ExecuteCommand(dbSQL);
            }

            public void CreateIndex(Databases.CDBConnection db, string indexName, string tblName, string[] columnNames)
            {
                // Try
                var tmp = new System.Text.StringBuilder();
                int i;
                tmp.Append("create index [");
                tmp.Append(indexName);
                tmp.Append("] on [");
                tmp.Append(tblName);
                tmp.Append("] (");
                i = 0;
                foreach (string cn in columnNames)
                {
                    if (i > 0)
                        tmp.Append(",");
                    tmp.Append("[");
                    tmp.Append(cn);
                    tmp.Append("]");
                    i += 1;
                }

                tmp.Append(")");
                db.ExecuteCommand(tmp.ToString());
                // Catch ex As Exception
                // Me.Log("CreateIndex -> " & ex.Message & vbCrLf & ex.StackTrace)
                // End Try
            }

            public void DropIndex(Databases.CDBConnection db, string tblName, string indexName)
            {
                // Try
                var tbl = db.Tables[tblName];
                var idx = tbl.Constraints[indexName];
                idx.Drop();
                // tbl.Constraints.Remove(idx)
                // Catch ex As Exception
                // Me.Log("DropIndex -> " & ex.Message & vbCrLf & ex.StackTrace)
                // End Try
            }

            public void ForceReopen()
            {
                CCollection col = GetAllOpenedConnections();
                foreach (Databases.CDBConnection d in col)
                {
                    try
                    {
                        d.CloseDB();
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.Message);
                    }

                    d.OpenDB();
                }
            }

            public void ResetID(object item)
            {
                ((Databases.IDBObjectBase)item).ResetID();
            }

            public void SetID(object item, int id)
            {
                ((Databases.IDBObjectBase)item).SetID(id);
            }



            /// <summary>
        /// Restituisce una stringa utilizzabile in una istruzione SQL (sostituisce ' con '')
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DBString(object value)
            {
                if (value is DBNull || value is null)
                {
                    return "NULL";
                }
                else
                {
                    return DMD.Strings.JoinW("'", DMD.Strings.Replace(DMD.Strings.CStr(value), "'", "''"), "'");
                }
            }



            /// <summary>
        /// Restituisce il formato SQL del valore booleano
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string DBBool(object value)
            {
                if (value is DBNull)
                    return "NULL";
                return (DMD.Booleans.CBool(value))? "True" : "False";
            }

            public Databases.CDBConnection OpenDatabase(string dbType, string dbPath, string userName = DMD.Strings.vbNullString, string password = DMD.Strings.vbNullString)
            {
                Databases.CDBConnection ret;
                switch (DMD.Strings.LCase(DMD.Strings.Trim(dbType)) ?? "")
                {
                    case "mdb":
                        {
                            ret = new Databases.COleDBConnection();
                            break;
                        }

                    case "xls":
                        {
                            ret = new Databases.CXlsDBConnection();
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException();
                            break;
                        }
                }

                ret.Path = dbPath;
                ret.SetCredentials(userName, password);
                ret.OpenDB();
                return ret;
            }

            public void CompactDB(string dbPath)
            {
                // 'jro = Nothing
                // Dim jro As JRO.JetEngine
                // Dim tmp As String = Sistema.FileSystem.GetFolderName(dbPath)
                // tmp & = "\tmpDB.tmp"
                // Sistema.FileSystem.DeleteFile(tmp, True)

                // jro = New JRO.JetEngine()

                // jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbPath, "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" 
                // & tmp &
                // ";Jet OLEDB:Engine Type=5")
                // Sistema.FileSystem.DeleteFile(dbPath, True)
                // Sistema.FileSystem.MoveFile(tmp, dbPath)

            }

            public void CompactDB(string dbPath, string target)
            {
                // Dim jro As JRO.JetEngine
                // jro = New JRO.JetEngine()

                // jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbPath, "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" '
                // & target 
                // & 
                // ";Jet OLEDB:Engine Type=5")
            }

            public string PrepareWhereClause(string fieldName, string searchString, bool matchExact)
            {
                int i, j;
                string[] operands;
                string[] operators;
                int status;
                string ch;
                // Dim ret As String

                searchString = DMD.Strings.Trim(searchString);
                i = 1;
                operands = new string[1];
                operators = new string[1];
                status = 0;
                operands[0] = "";
                operators[0] = "";
                while (i <= DMD.Strings.Len(searchString))
                {
                    ch = DMD.Strings.Mid(searchString, i, 1);
                    switch (status)
                    {
                        case 0:
                            {
                                switch (ch ?? "")
                                {
                                    case DMD.Strings.vbDoubleQuote: // Inizio stringa
                                        {
                                            status = 1;
                                            if (i > 1)
                                            {
                                                operators[DMD.Arrays.UBound(operators)] = "+";
                                                Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                                Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                                operands[DMD.Arrays.UBound(operands)] = "";
                                            }

                                            break;
                                        }

                                    case "&":    // Operatore logico AND
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "*";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = "";
                                            break;
                                        }

                                    case "|":
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = "";
                                            break;
                                        }

                                    case " ":
                                        {
                                            status = 2;
                                            break;
                                        }

                                    default:
                                        {
                                            operands[DMD.Arrays.UBound(operands)] = operands[DMD.Arrays.UBound(operands)] + ch;
                                            break;
                                        }
                                }

                                break;
                            }

                        case 1:
                            {
                                if (ch == DMD.Strings.CStr('"'))
                                {
                                    status = 0;
                                }
                                else
                                {
                                    operands[DMD.Arrays.UBound(operands)] = operands[DMD.Arrays.UBound(operands)] + ch;
                                }

                                break;
                            }

                        case 2:
                            {
                                switch (ch ?? "")
                                {
                                    case " ":
                                        {
                                            break;
                                        }

                                    case "|":
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            status = 3;
                                            break;
                                        }

                                    case "&":
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "*";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            status = 3;
                                            break;
                                        }

                                    case DMD.Strings.vbDoubleQuote:
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            status = 1;
                                            break;
                                        }

                                    default:
                                        {
                                            operators[DMD.Arrays.UBound(operators)] = "+";
                                            Array.Resize(ref operators, 1 + DMD.Arrays.UBound(operators) + 1);
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = ch;
                                            status = 0;
                                            break;
                                        }
                                }

                                break;
                            }

                        case 3:
                            {
                                switch (ch ?? "")
                                {
                                    case " ":
                                        {
                                            break;
                                        }

                                    default:
                                        {
                                            Array.Resize(ref operands, 1 + DMD.Arrays.UBound(operands) + 1);
                                            operands[DMD.Arrays.UBound(operands)] = ch;
                                            status = 0;
                                            break;
                                        }
                                }

                                break;
                            }
                    }

                    i = i + 1;
                }

                var loopTo = DMD.Arrays.UBound(operands);
                for (i = 0; i <= loopTo; i++)
                {
                    if (matchExact)
                    {
                        operands[i] = DMD.Strings.JoinW("[", fieldName, "]='", DMD.Strings.Replace(operands[i], "'", "''"), "'");
                    }
                    else
                    {
                        operands[i] = DMD.Strings.JoinW("[", fieldName, "] Like '%", DMD.Strings.Replace(operands[i], "'", "''"), "%'");
                    }
                }

                i = DMD.Arrays.UBound(operands);
                for (j = DMD.Arrays.UBound(operators) - 1; j >= 0; j -= 1)
                {
                    switch (operators[j] ?? "")
                    {
                        case "+":
                            {
                                if (DMD.Arrays.UBound(operands) >= i)
                                {
                                    operands[i - 1] = DMD.Strings.JoinW("(", operands[i - 1], ") Or (", operands[i], ")");
                                    i = i - 1;
                                }
                                else
                                {
                                    operands[i - 1] = operands[i];
                                }

                                break;
                            }

                        case "*":
                            {
                                if (DMD.Arrays.UBound(operands) >= i)
                                {
                                    operands[i - 1] = DMD.Strings.JoinW("(", operands[i - 1], ") And (", operands[i], ")");
                                    i = i - 1;
                                }
                                else
                                {
                                    operands[i - 1] = operands[i];
                                }

                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }

                return operands[0];
            }

            public bool IsChanged(object value)
            {
                if (value is IEnumerable)
                {
                    foreach (object v in (IEnumerable)value)
                    {
                        if (IsChanged(v))
                            return true;
                    }

                    return false;
                }
                else if (value is Databases.DBObjectBase)
                {
                    return ((Databases.DBObjectBase)value).IsChanged();
                }
                else
                {
                    throw new ArgumentException("value non implementa alcuna interfaccia valida");
                }
            }

            public void SetChanged(object obj, bool value)
            {
                if (obj is IEnumerable)
                {
                    foreach (object v in (IEnumerable)obj)
                        Databases.DBUtils.SetChanged(v, value);
                }
                else if (obj is Databases.DBObjectBase)
                {
                    ((Databases.DBObjectBase)obj).SetChanged(value);
                }
                else
                {
                    throw new ArgumentException("obj non implementa alcuna interfaccia valida");
                }
            }

            public int GetID(Databases.IDBObject obj)
            {
                return obj.ID;
            }


            /// <summary>
        /// Restituisce un valore che indica se si tratta di un tipo data, stringa, numerico o altro
        /// </summary>
        /// <param name="rdt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Databases.DBTypesEnum GetDataTypeFamily(Databases.adDataTypeEnum rdt)
            {
                switch (rdt)
                {
                    case Databases.adDataTypeEnum.adTinyInt:
                    case Databases.adDataTypeEnum.adSmallInt:
                    case Databases.adDataTypeEnum.adInteger:
                    case Databases.adDataTypeEnum.adBigInt:
                    case Databases.adDataTypeEnum.adUnsignedTinyInt:
                    case Databases.adDataTypeEnum.adUnsignedSmallInt:
                    case Databases.adDataTypeEnum.adUnsignedInt:
                    case Databases.adDataTypeEnum.adUnsignedBigInt:
                    case Databases.adDataTypeEnum.adSingle:
                    case Databases.adDataTypeEnum.adDouble:
                    case Databases.adDataTypeEnum.adCurrency:
                    case Databases.adDataTypeEnum.adDecimal:
                    case Databases.adDataTypeEnum.adNumeric:
                        {
                            return Databases.DBTypesEnum.DBNUMERIC_TYPE;
                        }

                    case Databases.adDataTypeEnum.adDate:
                    case Databases.adDataTypeEnum.adDBDate:
                    case Databases.adDataTypeEnum.adDBTime:
                    case Databases.adDataTypeEnum.adDBTimeStamp:
                        {
                            return Databases.DBTypesEnum.DBDATETIME_TYPE;
                        }

                    case Databases.adDataTypeEnum.adBoolean:
                        {
                            return Databases.DBTypesEnum.DBBOOLEAN_TYPE;
                        }

                    case Databases.adDataTypeEnum.adBSTR:
                    case Databases.adDataTypeEnum.adChar:
                    case Databases.adDataTypeEnum.adVarChar:
                    case Databases.adDataTypeEnum.adLongVarChar:
                    case Databases.adDataTypeEnum.adWChar:
                    case Databases.adDataTypeEnum.adVarWChar:
                    case Databases.adDataTypeEnum.adLongVarWChar:
                        {
                            return Databases.DBTypesEnum.DBTEXT_TYPE;
                        }

                    case Databases.adDataTypeEnum.adBinary:
                    case Databases.adDataTypeEnum.adVarBinary:
                    case Databases.adDataTypeEnum.adLongVarBinary:
                    case Databases.adDataTypeEnum.adArray:
                        {
                            return Databases.DBTypesEnum.DBBINARY_TYPE;
                        }

                    default:
                        {
                            return Databases.DBTypesEnum.DBUNSUPPORTED_TYPE;
                        }
                }
            }

            public Databases.adDataTypeEnum GetADOType(Type t)
            {
                if (ReferenceEquals(t, typeof(string)))
                    return Databases.adDataTypeEnum.adWChar;
                if (ReferenceEquals(t, typeof(bool)))
                    return Databases.adDataTypeEnum.adBoolean;
                if (ReferenceEquals(t, typeof(byte)))
                    return Databases.adDataTypeEnum.adUnsignedTinyInt;
                if (ReferenceEquals(t, typeof(sbyte)))
                    return Databases.adDataTypeEnum.adTinyInt;
                if (ReferenceEquals(t, typeof(short)))
                    return Databases.adDataTypeEnum.adSmallInt;
                if (ReferenceEquals(t, typeof(ushort)))
                    return Databases.adDataTypeEnum.adUnsignedSmallInt;
                if (ReferenceEquals(t, typeof(int)))
                    return Databases.adDataTypeEnum.adInteger;
                if (ReferenceEquals(t, typeof(uint)))
                    return Databases.adDataTypeEnum.adUnsignedInt;
                if (ReferenceEquals(t, typeof(long)))
                    return Databases.adDataTypeEnum.adBigInt;
                if (ReferenceEquals(t, typeof(ulong)))
                    return Databases.adDataTypeEnum.adUnsignedBigInt;
                if (ReferenceEquals(t, typeof(float)))
                    return Databases.adDataTypeEnum.adSingle;
                if (ReferenceEquals(t, typeof(double)))
                    return Databases.adDataTypeEnum.adDouble;
                if (ReferenceEquals(t, typeof(DateTime)))
                    return Databases.adDataTypeEnum.adDate;
                if (ReferenceEquals(t, typeof(decimal)))
                    return Databases.adDataTypeEnum.adCurrency;
                if (t.IsArray)
                    return Databases.adDataTypeEnum.adBinary;
                if (t.IsEnum)
                    return Databases.adDataTypeEnum.adInteger;
                // Throw New NotSupportedException("GetADOType non riconosce il tipo " & t.FullName)
                return Databases.adDataTypeEnum.adBinary; // adDataTypeEnum.adVariant
            }

            public System.Data.OleDb.OleDbType GetOleDbDataType(Type t)
            {
                if (ReferenceEquals(t, typeof(string)))
                    return System.Data.OleDb.OleDbType.WChar;
                if (ReferenceEquals(t, typeof(bool)))
                    return System.Data.OleDb.OleDbType.Boolean;
                if (ReferenceEquals(t, typeof(byte)))
                    return System.Data.OleDb.OleDbType.UnsignedTinyInt;
                if (ReferenceEquals(t, typeof(sbyte)))
                    return System.Data.OleDb.OleDbType.TinyInt;
                if (ReferenceEquals(t, typeof(short)))
                    return System.Data.OleDb.OleDbType.SmallInt;
                if (ReferenceEquals(t, typeof(ushort)))
                    return System.Data.OleDb.OleDbType.UnsignedSmallInt;
                if (ReferenceEquals(t, typeof(int)))
                    return System.Data.OleDb.OleDbType.Integer;
                if (ReferenceEquals(t, typeof(uint)))
                    return System.Data.OleDb.OleDbType.UnsignedInt;
                if (ReferenceEquals(t, typeof(long)))
                    return System.Data.OleDb.OleDbType.BigInt;
                if (ReferenceEquals(t, typeof(ulong)))
                    return System.Data.OleDb.OleDbType.UnsignedBigInt;
                if (ReferenceEquals(t, typeof(float)))
                    return System.Data.OleDb.OleDbType.Single;
                if (ReferenceEquals(t, typeof(double)))
                    return System.Data.OleDb.OleDbType.Double;
                if (ReferenceEquals(t, typeof(DateTime)))
                    return System.Data.OleDb.OleDbType.Date;
                if (ReferenceEquals(t, typeof(decimal)))
                    return System.Data.OleDb.OleDbType.Currency;
                if (t.IsArray)
                    return System.Data.OleDb.OleDbType.Binary;
                if (t.IsEnum)
                    return System.Data.OleDb.OleDbType.Integer;
                // Throw New NotSupportedException("GetADOType non riconosce il tipo " & t.FullName)
                return System.Data.OleDb.OleDbType.Binary; // OleDb.OleDbType.Variant
            }







            // -----------------------------------

            /// <summary>
        /// Restituisce la proprietà dell'oggetto prima delle modifiche
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public object GetOriginalValue(Databases.DBObjectBase obj, string fieldName)
            {
                return obj.GetOriginalFieldValue(fieldName);
            }

            /// <summary>
        /// Restituisce la proprietà dell'oggetto prima delle modifiche
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public T? GetOriginalValue<T>(Databases.DBObjectBase obj, string fieldName) where T : struct
            {
                var ret = GetOriginalValue(obj, fieldName);
                // If (ret Is Nothing) Then Return Nothing
                return (T?)ret;
            }

            public string GetTableName(object obj)
            {
                if (obj is Databases.DBObjectCursorBase)
                {
                    return ((Databases.DBObjectCursorBase)obj).GetTableName();
                }
                else
                {
                    return ((Databases.IDBObjectBase)obj).GetTableName();
                }
            }

            public Databases.CDBConnection GetConnection(object obj)
            {
                return ((Databases.DBObjectBase)obj).GetConnection();
            }

            public CCollection<StatsItem> PendingQueries
            {
                get
                {
                    return m_PendingQueries;
                }
            }

            public string ToDBDateStr(DateTime? d)
            {
                if (d.HasValue == false)
                    return DMD.Strings.vbNullString;
                var ret = new System.Text.StringBuilder();
                ret.Append(DMD.Strings.Right("0000" + d.Value.Year, 4));
                ret.Append(DMD.Strings.Right("00" + d.Value.Month, 2));
                ret.Append(DMD.Strings.Right("00" + d.Value.Day, 2));
                ret.Append(DMD.Strings.Right("00" + d.Value.Hour, 2));
                ret.Append(DMD.Strings.Right("00" + d.Value.Minute, 2));
                ret.Append(DMD.Strings.Right("00" + d.Value.Second, 2));
                return ret.ToString();
            }

            public DateTime? FromDBDateStr(string str)
            {
                str = DMD.Strings.Trim(str);
                if (string.IsNullOrEmpty(str))
                    return default;
                int year = DMD.Integers.CInt(DMD.Strings.Mid(str, 1, 4));
                int month = DMD.Integers.CInt(DMD.Strings.Mid(str, 5, 2));
                int day = DMD.Integers.CInt(DMD.Strings.Mid(str, 7, 2));
                int hour = DMD.Integers.CInt(DMD.Strings.Mid(str, 9, 2));
                int minute = DMD.Integers.CInt(DMD.Strings.Mid(str, 11, 2));
                int second = DMD.Integers.CInt(DMD.Strings.Mid(str, 13, 2));
                return DMD.DateUtils.MakeDate(year, month, day, hour, minute, second);
            }

            ~CDBUtilsClass()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }

    public partial class Databases
    {
        private static CDBUtilsClass m_Utils = null;

        public static CDBUtilsClass DBUtils
        {
            get
            {
                if (m_Utils is null)
                    m_Utils = new CDBUtilsClass();
                return m_Utils;
            }
        }
    }
}