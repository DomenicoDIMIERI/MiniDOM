
namespace minidom.Forms
{

    /// <summary>
    /// Handler dei motivi commissione
    /// </summary>
    public class MotiviCommissioniHandler 
        : CBaseModuleHandler
    {
        /// <summary>
        /// Costruttore
        /// </summary>
        public MotiviCommissioniHandler() 
            : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        /// <summary>
        /// Crea il cursore
        /// </summary>
        /// <returns></returns>
        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.MotivoCommissioneCursor();
        }
    }
}