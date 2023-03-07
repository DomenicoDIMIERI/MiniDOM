using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class ChatStats : IDMDXMLSerializable
    {
        public CCollection<Messenger.CChatUser> OnlineUsers;
        public CCollection<Messenger.CMessage> Messages;

        public ChatStats()
        {
            DMDObject.IncreaseCounter(this);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "OnlineUsers":
                    {
                        OnlineUsers.AddRange((IEnumerable)fieldValue);
                        break;
                    }

                case "Messages":
                    {
                        Messages.AddRange((IEnumerable)fieldValue);
                        break;
                    }
            }
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            writer.WriteTag("OnlineUsers", OnlineUsers.ToArray());
            writer.WriteTag("Messages", Messages.ToArray());
        }

        ~ChatStats()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}