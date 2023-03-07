
namespace minidom.Forms
{
    public class CQSPDRichiesteDerogheHandler : CBaseModuleHandler
    {
        public CQSPDRichiesteDerogheHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit)
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Finanziaria.RichiesteDeroghe.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CRichiestaDerogaCursor();
        }

        public string Invia(object renderer)
        {
            if (!Module.UserCanDoAction("inviare"))
                throw new PermissionDeniedException(Module, "inviare");
            Finanziaria.CRichiestaDeroga ric = (Finanziaria.CRichiestaDeroga)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            ric.Invia();
            return DMD.XML.Utils.Serializer.Serialize(ric);
        }

        public string Ricevi(object renderer)
        {
            if (!Module.UserCanDoAction("ricevere"))
                throw new PermissionDeniedException(Module, "ricevere");
            Finanziaria.CRichiestaDeroga ric = (Finanziaria.CRichiestaDeroga)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            ric.Ricevi();
            return DMD.XML.Utils.Serializer.Serialize(ric);
        }
    }
}