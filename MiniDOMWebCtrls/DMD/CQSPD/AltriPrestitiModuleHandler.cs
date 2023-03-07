
namespace minidom.Forms
{
    public class AltriPrestitiModuleHandler : CBaseModuleHandler
    {
        public AltriPrestitiModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CAltriPrestitiCursor();
        }

        // Public Overrides Function GetEditor() As Object
        // Return New AltriPrestitiEditor
        // End Function

    }
}