
namespace minidom.Forms
{
    public class ValutazioniAziendeHandler : CBaseModuleHandler
    {
        public ValutazioniAziendeHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CQSPDValutazioneAziendaCursor();
        }
    }
}