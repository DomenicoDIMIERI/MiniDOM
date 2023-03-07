﻿
namespace minidom.Forms
{
    public class CartucceTonerHandler : CBaseModuleHandler
    {
        public CartucceTonerHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.CartucceTonerCursor();
        }
    }
}