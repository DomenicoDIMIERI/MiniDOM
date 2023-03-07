
namespace minidom.Forms
{
    public class CQSPDRichiesteConteggiHandler : CBaseModuleHandler
    {
        public CQSPDRichiesteConteggiHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CRichiestaConteggioCursor();
        }

        public string Segnala(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            Finanziaria.CRichiestaConteggio ric = (Finanziaria.CRichiestaConteggio)GetInternalItemById(id);
            if (!Module.UserCanDoAction("segnalare"))
                throw new PermissionDeniedException(Module, "segnalare");
            ric.Segnala();
            return "";
        }

        public string PrendiInCarico(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            Finanziaria.CRichiestaConteggio ric = (Finanziaria.CRichiestaConteggio)GetInternalItemById(id);
            if (!Module.UserCanDoAction("prendereincarico"))
                throw new PermissionDeniedException(Module, "prendereincarico");
            ric.PrendiInCarico();
            return "";
        }
    }
}