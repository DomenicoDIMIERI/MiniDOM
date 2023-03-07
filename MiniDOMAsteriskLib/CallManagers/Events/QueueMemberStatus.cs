
namespace minidom.CallManagers.Events
{
    public class QueueMemberStatus : AsteriskEvent
    {
        public QueueMemberStatus() : base("QueueMemberStatus")
        {
        }

        public QueueMemberStatus(string[] rows) : base(rows)
        {
        }

        public string Queue
        {
            get
            {
                return GetAttribute("Queue");
            }
        }

        public string Location
        {
            get
            {
                return GetAttribute("Location");
            }
        }

        public string MemberName
        {
            get
            {
                return GetAttribute("MemberName");
            }
        }

        public string Membership
        {
            get
            {
                return GetAttribute("Membership");
            }
        }

        public int Penalty
        {
            get
            {
                return GetAttribute("Penalty", 0);
            }
        }

        public int CallsTaken
        {
            get
            {
                return GetAttribute("CallsTaken", 0);
            }
        }

        public int LastCall
        {
            get
            {
                return GetAttribute("LastCall", 0);
            }
        }

        public QueueStatusFlags Status
        {
            get
            {
                return (QueueStatusFlags)GetAttribute("Status", 0);
            }
        }

        public int Paused
        {
            get
            {
                return GetAttribute("Paused", 0);
            }
        }

        // As far as I know Possible values are:
        // /*! Device is valid but channel didn't know state */
        // define AST_DEVICE_UNKNOWN 0
        // /*! Device is not used */
        // define AST_DEVICE_NOT_INUSE 1
        // /*! Device is in use */
        // define AST_DEVICE_INUSE 2
        // /*! Device is busy */
        // define AST_DEVICE_BUSY 3
        // /*! Device is invalid */
        // define AST_DEVICE_INVALID 4
        // /*! Device is unavailable */
        // define AST_DEVICE_UNAVAILABLE 5
        // /*! Device is ringing */
        // define AST_DEVICE_RINGING 6
        // /*! Device is ringing *and* in use */
        // define AST_DEVICE_RINGINUSE 7
        // /*! Device is on hold */
        // define AST_DEVICE_ONHOLD 8

    }
}