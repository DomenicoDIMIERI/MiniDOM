
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// Fired when the "RELOAD" console command is executed.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Reload : AsteriskEvent
    {
        public Reload() : base("Reload")
        {
        }

        public Reload(string[] rows) : base(rows)
        {
        }

        public string Message
        {
            get
            {
                return GetAttribute("Message");
            }
        }
    }
}