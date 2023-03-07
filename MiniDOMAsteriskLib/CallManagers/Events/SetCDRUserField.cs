
namespace minidom.CallManagers.Events
{
    public class SetCDRUserField : AsteriskEvent
    {
        public SetCDRUserField() : base("SetCDRUserField")
        {
        }

        public SetCDRUserField(string[] rows) : base(rows)
        {
        }
    }
}