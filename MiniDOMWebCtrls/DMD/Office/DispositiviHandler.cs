using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class DispositiviHandler : CBaseModuleHandler
    {
        public DispositiviHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.DispositivoCursor();
        }

        public override CCollection<ExportableColumnInfo> GetExportableColumnsList()
        {
            var ret = base.GetExportableColumnsList();
            ret.Add(new ExportableColumnInfo("ID", "ID", TypeCode.Int32, true));
            ret.Add(new ExportableColumnInfo("Motivo", "Motivo", TypeCode.String, true));
            return ret;
        }

        public string GetStatoDispositivo(object renderer)
        {
            string sn = this.n2str(renderer, "sn");
            var item = new InfoStatoDispositivo(sn);
            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string GetNomiClassi(object renderer)
        {
            WebSite.ASP_Server.ScriptTimeout = 15;
            string q = Strings.Replace(this.n2str(renderer, "_q", ""), "  ", " ");
            var items = Office.ClassiDispositivi.LoadAll();
            items.Sort();
            var ret = new System.Text.StringBuilder();
            ret.Append("<list>");
            foreach (Office.ClasseDispositivo cls in items)
            {
                if (Strings.InStr(cls.Nome, q) > 0)
                {
                    ret.Append("<item>");
                    ret.Append("<text>");
                    ret.Append(DMD.WebUtils.HtmlEncode(cls.Nome));
                    ret.Append("</text>");
                    ret.Append("<value>");
                    ret.Append(DMD.WebUtils.HtmlEncode(cls.Nome));
                    ret.Append("</value>");
                    ret.Append("<icon>");
                    ret.Append(DMD.WebUtils.HtmlEncode(cls.IconURL));
                    ret.Append("</icon>");
                    ret.Append("</item>");
                }
            }

            ret.Append("</list>");
            return ret.ToString();
        }

        public string GetNomiDispositivi(object renderer)
        {
            WebSite.ASP_Server.ScriptTimeout = 15;
            string q = Strings.Replace(this.n2str(renderer, "_q", ""), "  ", " ");
            var items = Office.Dispositivi.LoadAll();
            items.Sort();
            var ret = new System.Text.StringBuilder();
            ret.Append("<list>");
            foreach (Office.Dispositivo dev in items)
            {
                if (dev.Stato == Databases.ObjectStatus.OBJECT_VALID && Strings.InStr(dev.Nome, q) > 0)
                {
                    ret.Append("<item>");
                    ret.Append("<text>");
                    ret.Append(DMD.WebUtils.HtmlEncode(dev.Nome));
                    ret.Append("</text>");
                    ret.Append("<value>");
                    ret.Append(DMD.WebUtils.HtmlEncode(dev.Nome));
                    ret.Append("</value>");
                    ret.Append("<icon>");
                    if (!string.IsNullOrEmpty(dev.IconURL))
                    {
                        ret.Append(DMD.WebUtils.HtmlEncode(dev.IconURL));
                    }
                    else
                    {
                        var cls = Office.ClassiDispositivi.GetItemByName(dev.Tipo);
                        if (cls is object)
                        {
                            ret.Append(DMD.WebUtils.HtmlEncode(cls.IconURL));
                        }
                    }

                    ret.Append("</icon>");
                    ret.Append("</item>");
                }
            }

            ret.Append("</list>");
            return ret.ToString();
        }

        public string GetElencoDispositivi(object renderer)
        {
            WebSite.ASP_Server.ScriptTimeout = 15;
            string q = Strings.Replace(this.n2str(renderer, "_q", ""), "  ", " ");
            Office.DeviceFlags flags = (Office.DeviceFlags)this.n2int(renderer, "flags");
            var items = Office.Dispositivi.LoadAll();
            items.Sort();
            var ret = new System.Text.StringBuilder();
            ret.Append("<list>");
            foreach (Office.Dispositivo dev in items)
            {
                if (dev.Stato == Databases.ObjectStatus.OBJECT_VALID && Strings.InStr(dev.Nome, q) > 0)
                {
                    var cls = Office.ClassiDispositivi.GetItemByName(dev.Tipo);
                    if (flags != Office.DeviceFlags.None)
                    {
                        if (cls is object && Sistema.TestFlag(cls.Flags, flags))
                        {
                            ret.Append("<item>");
                            ret.Append("<text>");
                            ret.Append(DMD.WebUtils.HtmlEncode(dev.Nome));
                            ret.Append("</text>");
                            ret.Append("<value>");
                            ret.Append(Databases.GetID(dev));
                            ret.Append("</value>");
                            ret.Append("<icon>");
                            if (!string.IsNullOrEmpty(dev.IconURL))
                            {
                                ret.Append(DMD.WebUtils.HtmlEncode(dev.IconURL));
                            }
                            else if (cls is object)
                            {
                                ret.Append(DMD.WebUtils.HtmlEncode(cls.IconURL));
                            }

                            ret.Append("</icon>");
                            ret.Append("</item>");
                        }
                    }
                }
            }

            ret.Append("</list>");
            return ret.ToString();
        }
    }

    public class InfoStatoDispositivo 
        : IDMDXMLSerializable
    {
        public string serialNumber;
        public Sistema.CUser User = null;
        public Office.Uscita Uscita = null;
        public Office.Dispositivo Dispositivo = null;
        public CCollection<Office.Commissione> Commissioni;

        public InfoStatoDispositivo()
        {
            serialNumber = "";
            User = null;
            Uscita = null;
            Dispositivo = null;
            Commissioni = null;
        }

        public InfoStatoDispositivo(string serialNumber)
        {
            Dispositivo = Office.Dispositivi.GetItemBySeriale(serialNumber);
            if (Dispositivo is null)
                throw new ArgumentNullException("Nessun dispositivo associato al seriale: " + serialNumber);
            User = Dispositivo.User;
            if (User is null)
                throw new ArgumentNullException("Nessun utente associato al dispositivo: " + serialNumber);
            Uscita = Office.Uscite.GetUltimaUscita(User);
            Commissioni = Office.Commissioni.GetCommissioniDaFare(User);
        }

        public void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "SN":
                    {
                        serialNumber = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "User":
                    {
                        User = (Sistema.CUser)fieldValue;
                        break;
                    }

                case "Uscita":
                    {
                        Uscita = (Office.Uscita)fieldValue;
                        break;
                    }

                case "Dispositivo":
                    {
                        Dispositivo = (Office.Dispositivo)fieldValue;
                        break;
                    }

                case "Commissioni":
                    {
                        Commissioni = (CCollection<Office.Commissione>)fieldValue;
                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("SN", serialNumber);
            writer.WriteTag("User", User);
            writer.WriteTag("Uscita", Uscita);
            writer.WriteTag("Dispositivo", Dispositivo);
            writer.WriteTag("Commissioni", Commissioni);
        }
    }
}