
namespace minidom.Forms
{
    public class CCQSPDImportExportHandler : CBaseModuleHandler
    {
        public CCQSPDImportExportHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SExport)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CImportExportCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return Finanziaria.ImportExport.GetItemById(id);
        }

        public string Esporta(object renderer)
        {
            Finanziaria.CImportExport item = (Finanziaria.CImportExport)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            item.Esporta();
            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string ConfermaEsportazione(object renderer)
        {
            Finanziaria.CImportExport item = (Finanziaria.CImportExport)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            item.ConfermaEsportazione(item.MessaggioConferma);
            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string Importa(object renderer)
        {
            Finanziaria.CImportExport item = (Finanziaria.CImportExport)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            item.Importa();
            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string PrendiInCarico(object renderer)
        {
            Finanziaria.CImportExport item = (Finanziaria.CImportExport)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            item.PrendiInCarico();
            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string Sincronizza(object renderer)
        {
            Finanziaria.CImportExport item = (Finanziaria.CImportExport)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            CCollection oggetti = (CCollection)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "oggetti"));
            item.Sincronizza((CKeyCollection)oggetti);
            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string Sollecita(object renderer)
        {
            Finanziaria.CImportExport item = (Finanziaria.CImportExport)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item"));
            item.Sollecita();
            return DMD.XML.Utils.Serializer.Serialize(item);
        }
    }
}