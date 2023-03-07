using System;


namespace minidom
{
    public partial class Databases
    {
        public class CXlsDBConnection 
            : COleDBConnection
        {

            /// <summary>
        /// Restituisce o imposta la versione predefinita
        /// </summary>
        /// <returns></returns>
            public static string DefauleVersion { get; set; } = ""; // Auto

            private string m_Version = "";
            private bool m_UseHeaders;

            public CXlsDBConnection()
            {
                m_UseHeaders = true;
            }

            public CXlsDBConnection(string fileName) : this(fileName, DefauleVersion, true)
            {
            }

            public CXlsDBConnection(string fileName, string version) : this(fileName, version, true)
            {
            }

            public CXlsDBConnection(string fileName, bool useHaders) : this(fileName, DefauleVersion, useHaders)
            {
            }

            public CXlsDBConnection(string fileName, string version, bool useHeaders) : this()
            {
                m_Version = DMD.Strings.Trim(version);
                if (string.IsNullOrEmpty(m_Version))
                    m_Version = DefauleVersion;
                m_UseHeaders = useHeaders;
                Path = fileName;
            }

            public string DriverVerion
            {
                get
                {
                    return m_Version;
                }
            }

            public bool UseHaders
            {
                get
                {
                    return m_UseHeaders;
                }
            }

            public override string GetSqlDataType(CDBEntityField field)
            {
                if (ReferenceEquals(field.DataType, typeof(string)))
                {
                    return Sistema.IIF(field.MaxLength > 0, "TEXT(255)", "MEMO");
                }
                else
                {
                    return base.GetSqlDataType(field);
                }
            }

            public override string GetFriendlyName(string name)
            {
                name = base.GetFriendlyName(name);
                if (DMD.Strings.Right(name, 1) == "$" | DMD.Strings.Right(name, 1) == "_")
                {
                    return DMD.Strings.Left(name, DMD.Strings.Len(name) - 1);
                }
                else
                {
                    return name;
                }
            }

            public override string GetInternalTableName(CDBEntity table)
            {
                if (table.IsHidden)
                    return "[" + table.Name + "_]";
                return "[" + table.Name + "$]";
            }

            public override string GetConnectionString()
            {
                const string excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"{3};HDR=YES;{4}\"";
                if (string.IsNullOrEmpty(m_Version))
                {
                    // Autoidentify
                    switch (DMD.Strings.LCase(DMD.Strings.Trim(Sistema.FileSystem.GetExtensionName(Path))) ?? "")
                    {
                        case "xls":  // For Excel Below 2007 Format
                            {
                                // Return String.Format(excelObject, "Jet", "4.0", Me.Path, "Excel 8.0", "")
                                return string.Format(excelObject, "ACE", "12.0", Path, "Excel 8.0", "");
                            }

                        case "xlsx": // For Excel 2007 File  Format
                            {
                                return string.Format(excelObject, "ACE", "12.0", Path, "Excel 12.0", "");
                            }

                        default:
                            {
                                throw new ArgumentOutOfRangeException();
                                 
                            }
                    }
                }
                else if (m_Version == "12.0")
                {
                    return string.Format(excelObject, "ACE", m_Version, Path, "Excel 12.0", "");
                }
                else
                {
                    return string.Format(excelObject, "Jet", "4.0", Path, "Excel 8.0", "");
                }
            }

            protected override CDBTable[] GetTablesArray()
            {
                var ret = base.GetTablesArray();
                foreach (var table in ret)
                {
                    if (table.Name.StartsWith("'") && table.Name.EndsWith("'"))
                    {
                        table.Name = table.Name.Substring(1, table.Name.Length - 2);
                    }

                    table.IsHidden = DMD.Strings.Right(table.Name, 1) == "_";
                    if (table.Name.EndsWith("$"))
                    {
                        if (table.Name.StartsWith("'"))
                        {
                            table.Name = table.Name.Substring(1, table.Name.Length - 3);
                        }
                        else
                        {
                            table.Name = table.Name.Substring(0, table.Name.Length - 1);
                        }
                    }
                    else
                    {
                        table.Name = table.Name.Substring(0, table.Name.Length - 1);
                    }
                }

                return ret;
            }

            // Protected Friend Overrides Sub CreateTable(table As CDBTable)
            // Dim sql As String
            // Dim t As Boolean = False
            // sql = vbNullString
            // sql &= "CREATE TABLE [" & table.Name & "] ("
            // For Each field As CDBEntityField In table.Fields
            // If (t) Then sql &= ","
            // sql &= "[" & field.Name & "] " & Me.GetSqlDataType(field)
            // t = True
            // Next
            // sql &= ")"
            // Me.ExecuteCommand(sql)
            // End Sub

        }
    }
}