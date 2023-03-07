
namespace minidom.Forms
{
    public class LuoghiVisitatiHandler : CBaseModuleHandler
    {
        public LuoghiVisitatiHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.LuoghiVisitatiCursor();
        }

        public string GetLuoghiVisitati(object renderer)
        {
            int lid = (int)this.n2int(renderer, "lid");
            var uscita = Office.Uscite.GetItemById(lid);
            return DMD.XML.Utils.Serializer.Serialize(uscita.LuoghiVisitati);
        }
    }
}