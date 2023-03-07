
namespace minidom.CallManagers.Events
{
    public class MessageWaiting : AsteriskEvent
    {
        public MessageWaiting() : base("MessageWaiting")
        {
        }

        public MessageWaiting(string[] rows) : base(rows)
        {
        }

        public string Mailbox
        {
            get
            {
                return GetAttribute("Mailbox");
            }
        }

        public int Waiting
        {
            get
            {
                return GetAttribute("Waiting", 0);
            }
        }

        public int New
        {
            get
            {
                return GetAttribute("New", 0);
            }
        }

        public int Old
        {
            get
            {
                return GetAttribute("Old", 0);
            }
        }
    }
}