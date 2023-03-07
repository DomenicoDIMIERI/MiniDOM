

namespace minidom
{
    public partial class Databases
    {
        public class CCvsDBConnection 
            : CXlsDBConnection
        {
            public CCvsDBConnection() : base()
            {
            }

            public CCvsDBConnection(string fileName, bool useHeaders = true) : base(fileName, "", useHeaders)
            {
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
                return DMD.Strings.Replace(table.Name, "#c", ".csv");
            }

            public override string GetConnectionString()
            {
                const string excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"{3};HDR=YES;{4}\"";
                return string.Format(excelObject, "Jet", "4.0", Path, "text", "FMT=Delimited");
            }

            protected override CDBTable[] GetTablesArray()
            {
                var ret = base.GetTablesArray();
                for (int i = 0, loopTo = DMD.Arrays.UBound(ret); i <= loopTo; i++)
                {
                    var table = ret[i];
                    table.IsHidden = DMD.Strings.Right(table.Name, 1) == "_";
                    table.Name = DMD.Strings.Left(table.Name, DMD.Strings.Len(table.Name) - 1);
                }

                return ret;
            }
        }
    }
}