
namespace minidom.Forms
{
    public class GruppiConsulenzeHandler : CBaseModuleHandler
    {
        public GruppiConsulenzeHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CQSPDStudiDiFattibilitaCursor();
        }

        public string GetStudiDiFattibilitaByPersona(object renderer)
        {
            int idPersona = (int)this.n2int(renderer, "pid");
            var items = Finanziaria.StudiDiFattibilita.GetStudiDiFattibilitaByPersona(idPersona);
            return DMD.XML.Utils.Serializer.Serialize(items);
        }

        public string GetUltimoStudioDiFattibilita(object renderer)
        {
            int idPersona = (int)this.n2int(renderer, "pid");
            var s = Finanziaria.StudiDiFattibilita.GetUltimoStudioDiFattibilita(idPersona);
            if (s is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(s);
        }

        public string GetSoluzioniXStudioDiFattibilita(object renderer)
        {
            int gid = (int)this.n2int(renderer, "gid");
            var g = Finanziaria.StudiDiFattibilita.GetItemById(gid);
            return DMD.XML.Utils.Serializer.Serialize(g.Proposte);
        }

        public string GetStudiDiFattibilitaByRichiesta(object renderer)
        {
            int idRichiesta = (int)this.n2int(renderer, "rid");
            var items = Finanziaria.StudiDiFattibilita.GetStudiDiFattibilitaByRichiesta(idRichiesta);
            return DMD.XML.Utils.Serializer.Serialize(items);
        }
    }
}