
namespace minidom.Forms
{
    public class EventHandlersModuleHandler : CBaseModuleHandler
    {
        public EventHandlersModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public EventHandlersModuleHandler(Sistema.CModule module) : this()
        {
            SetModule(module);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Sistema.RegisteredEventHandlerCursor();
        }

        public override object GetInternalItemById(int id)
        {
            foreach (Sistema.RegisteredEventHandler item in Sistema.RegisteredEventHandlers.LoadAll())
            {
                if (Databases.GetID(item) == id)
                    return item;
            }

            return null;
        }
    }
}