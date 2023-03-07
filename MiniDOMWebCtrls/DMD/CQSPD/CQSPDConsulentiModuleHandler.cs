using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom.Forms
{
    public class CQSPDConsulentiModuleHandler : CBaseModuleHandler
    {
        public CQSPDConsulentiModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CConsulentiPraticaCursor();
        }

        public string GetItemByUser(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            var item = Finanziaria.Consulenti.GetItemByUser(id);
            if (item is null)
            {
                return "";
            }
            else
            {
                return DMD.XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document);
            }
        }

        public string CreateElencoConsulenti(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            bool ov = (bool)this.n2bool(renderer, "ov", true);
            return Utils.CQSPDUtils.CreateElencoConsulenti(id, ov);
        }
    }
}