
namespace minidom.CallManagers.Events
{

    /// <summary>
    /// Fired when two voice channels are linked together and voice data exchange commences.
    /// </summary>
    /// <remarks>Several Link events may be seen for a single call. This can occur when Asterisk fails to setup a native bridge for the call. As far as I can tell, this is when Asterisk must sit between two telephones and perform CODEC conversion on their behalf.</remarks>
    public class Bridge : Link
    {
        public Bridge() : base()
        {
            SetAttribute("Event", "Bridge");
        }

        public Bridge(string[] rows) : base(rows)
        {
        }
    }
}