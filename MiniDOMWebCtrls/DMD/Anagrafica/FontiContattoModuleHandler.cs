using System;

namespace minidom.Forms
{
    public class FontiContattoModuleHandler : CBaseModuleHandler
    {
        public FontiContattoModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CFontiCursor();
        }

        public override CCollection<ExportableColumnInfo> GetExportableColumnsList()
        {
            var ret = base.GetExportableColumnsList();
            ret.Add(new ExportableColumnInfo("ID", "ID", TypeCode.Int32, true));
            ret.Add(new ExportableColumnInfo("Nome", "Nome", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Tipo", "Tipo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("IDCampagna", "IDCampagna", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("IDAnnuncio", "IDAnnuncio", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("IDKeyWord", "IDKeyword", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("DataInizio", "DataInizio", TypeCode.DateTime, true));
            ret.Add(new ExportableColumnInfo("DataFine", "DataFine", TypeCode.DateTime, true));
            ret.Add(new ExportableColumnInfo("Attiva", "Attiva", TypeCode.Boolean, true));
            return ret;
        }
    }
}