using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{

    [Serializable]
    public class ControlPanelUserInfo 
        : IDMDXMLSerializable
    {

        [NonSerialized] public Sistema.CUser User;
        [NonSerialized] public Sistema.CLoginHistory LastLogin;
        // Public Attivita As CContattoUtente
        // Public DescrizioneAttivita As String

        public ControlPanelUserInfo()
        {
            DMDObject.IncreaseCounter(this);
        }

        public ControlPanelUserInfo(Sistema.CUser user) : this()
        {
            User = user;
        }

        public void Load()
        {
            LastLogin = User.CurrentLogin;
        }

        public void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "User":
                    {
                        User = (Sistema.CUser)fieldValue;
                        break;
                    }

                case "LastLogin":
                    {
                        LastLogin = null;
                        if (fieldValue is Sistema.CLoginHistory)
                            LastLogin = (Sistema.CLoginHistory)fieldValue;
                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteTag("User", User);
            writer.WriteTag("LastLogin", LastLogin);
        }

        ~ControlPanelUserInfo()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}