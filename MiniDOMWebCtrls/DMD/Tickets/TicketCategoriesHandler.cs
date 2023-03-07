using minidom.diallib;

namespace minidom.Forms
{
    public class TicketCategoriesHandler : CBaseModuleHandler
    {
        public TicketCategoriesHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.CTicketCategoryCursor();
        }
    }

    public class ChiamateRegistrateHandler : CBaseModuleHandler
    {
        public ChiamateRegistrateHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.ChiamataRegistrataCursor();
        }
    }

    public class DialTPConfigHandler : CBaseModuleHandler
    {
        public DialTPConfigHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new DMDSIPConfigCursor();
        }
    }
}