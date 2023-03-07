
namespace minidom.Forms
{



    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class DistributoriModuleHandler : CBaseModuleHandler
    {
        public DistributoriModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CDistributoriCursor();
        }
    }




    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class TipologieAziendaModuleHandler : CBaseModuleHandler
    {
        public TipologieAziendaModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CTipologiaAziendaCursor();
        }
    }



    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class CategorieAziendaModuleHandler : CBaseModuleHandler
    {
        public CategorieAziendaModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CCategorieAziendaCursor();
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class FormeGiuridicheAziendaModuleHandler : CBaseModuleHandler
    {
        public FormeGiuridicheAziendaModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CFormeGiuridicheAziendaCursor();
        }
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */



}