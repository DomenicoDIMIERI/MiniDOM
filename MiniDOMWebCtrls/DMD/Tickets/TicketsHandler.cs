
namespace minidom.Forms
{
    public class TicketsHandler : CBaseModuleHandler
    {
        public TicketsHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }
    }
}