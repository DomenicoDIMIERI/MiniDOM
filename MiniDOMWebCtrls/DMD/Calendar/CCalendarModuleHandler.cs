using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CCalendarModuleHandler 
        : CBaseModuleHandler
    {
        public CCalendarModuleHandler()
        {
        }

        public string getPersonInfo(object renderer)
        {
            int pid;
            Sistema.CCalendarActivityInfo ret;
            pid = (int)this.n2int(renderer, "pid");
            ret = minidom.Sistema.Calendar.GetPersonInfo(pid);
            return DMD.XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return null;
        }

        public string GetAvailableGroups(object renderer)
        {
            CCollection<Sistema.CGroup> col = (CCollection<Sistema.CGroup>)minidom.Sistema.Calendar.GetAvailableGroups();
            if (col.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(col.ToArray(), XMLSerializeMethod.Document);
            }
            else
            {
                return DMD.Strings.vbNullString;
            }
        }

        public string GetAvailableGroupOperators(object renderer)
        {
            int gid = (int) this.n2int(renderer, "gid");
            CCollection<Sistema.CUser> col = (CCollection<Sistema.CUser>)minidom.Sistema.Calendar.GetAvailableGroupOperators(gid);
            if (col.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(col.ToArray(), XMLSerializeMethod.Document);
            }
            else
            {
                return DMD.Strings.vbNullString;
            }
        }

        public string GetCompleanniPerMese(object renderer)
        {
            int year = (int)this.n2int(renderer, "y");
            int month = (int)this.n2int(renderer, "m");
            var compleanni = minidom.Sistema.Calendar.GetCompleanniPerMese(year, month);
            return DMD.XML.Utils.Serializer.Serialize(compleanni, XMLSerializeMethod.Document);
        }

        public string GetCompleanniPerGiorno(object renderer)
        {
            DateTime d = (DateTime)this.n2date(renderer, "d");
            var compleanni = minidom.Sistema.Calendar.GetCompleanniPerGiorno(d);
            return DMD.XML.Utils.Serializer.Serialize(compleanni, XMLSerializeMethod.Document);
        }

        public string GetToDoList(object renderer)
        {
            int uid = (int)this.n2int(renderer, "uid");
            var user = Sistema.Users.GetItemById(uid);
            var items = new CCollection(minidom.Sistema.Calendar.GetToDoList(user));
            return DMD.XML.Utils.Serializer.Serialize(items);
        }
    }
}