
namespace minidom.Forms
{
    public class CAmministrazioniModuleHandler : CPersoneGiuridicheModuleHandler
    {
        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CPersonaCursor();
            ret.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_GIURIDICA;
            return ret;
        }
    }
}