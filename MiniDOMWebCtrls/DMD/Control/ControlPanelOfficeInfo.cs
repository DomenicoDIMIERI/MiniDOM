using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;

namespace minidom.Forms
{
    public class ControlPanelOfficeInfo
        : IDMDXMLSerializable
    {
        public Anagrafica.CUfficio Ufficio;
        public CCollection<ControlPanelUserInfo> Utenti;

        public ControlPanelOfficeInfo()
        {
            DMDObject.IncreaseCounter(this);
        }

        public ControlPanelOfficeInfo(Anagrafica.CUfficio ufficio) : this()
        {
            Ufficio = ufficio;
            Utenti = new CCollection<ControlPanelUserInfo>();
        }

        public void Load(DateTime fromDate)
        {
            Utenti = new CCollection<ControlPanelUserInfo>();
            foreach (Sistema.CUser u in Sistema.Users.LoadAll())
            {
                if (u.IsValid() && u.CurrentLogin.IDUfficio == Databases.GetID(Ufficio))
                {
                    var info = new ControlPanelUserInfo(u);
                    info.Load();
                    Utenti.Add(info);
                }
            }
        }

        public void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Ufficio":
                    {
                        Ufficio = (Anagrafica.CUfficio)fieldValue;
                        break;
                    }

                case "Utenti":
                    {
                        Utenti = new CCollection<ControlPanelUserInfo>();
                        if (fieldValue is IEnumerable)
                        {
                            Utenti.AddRange((IEnumerable)fieldValue);
                        }
                        else if (fieldValue is ControlPanelUserInfo)
                        {
                            Utenti.Add((ControlPanelUserInfo)fieldValue);
                        }

                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteTag("Ufficio", Ufficio);
            writer.WriteTag("Utenti", Utenti);
        }

        ~ControlPanelOfficeInfo()
        {
            DMDObject.DecreaseCounter(this);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }
    }
}