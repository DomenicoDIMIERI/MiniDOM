using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{

    /// <summary>
    /// The "QueueStatus" request returns statistical information about calls delivered to the existing queues, as well as the corresponding service level.
    /// The response consists of zero or more "Event: QueueParams" stanzas, one per queue. Each is followed by zero or more "Event: QueueMember" stanzas, one per agent assigned to that queue, and zero or more "Event: QueueEntry" stanzas, one for each call waiting in the queue. The whole lot ends with an "Event: QueueStatusComplete" stanza.
    /// Parameters: ActionID, Queue, Member
    /// The Queue parameter allows to select one queue to view its status, as with the Member parameter, that allow to select one queue member.
    /// You can use the Member parameter with something like 'cnlohewfuy4r89734yc8yc48' to not receive any information about members, just info on the Queue.
    /// If the parameters are not set all Queues and all Member will be shown.
    /// </summary>
    /// <remarks></remarks>
    public class QueueStatus : AsyncAction
    {
        public QueueStatus() : base("QueueStatus")
        {
        }

        protected override string GetCommandText()
        {
            string ret = base.GetCommandText();
            return ret;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new QueueStatusResponse(this);
        }
    }
}