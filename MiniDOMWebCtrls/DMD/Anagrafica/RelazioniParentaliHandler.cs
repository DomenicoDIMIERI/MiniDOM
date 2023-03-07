
namespace minidom.Forms
{
    public class RelazioniParentaliHandler : CBaseModuleHandler
    {
        public RelazioniParentaliHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CRelazioneParentaleCursor();
        }

        public string GetRelazioni(object renderer)
        {
            int pID = (int)this.n2int(renderer, "pid");
            var items = Anagrafica.RelazioniParentali.GetRelazioni(pID);
            return DMD.XML.Utils.Serializer.Serialize(items);
        }
    }
}