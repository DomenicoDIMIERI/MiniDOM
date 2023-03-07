
namespace minidom.Forms
{
    public class CControlPanelHandler : CBaseModuleHandler
    {
        public CControlPanelHandler()
        {
        }

        public string Load(object renderer)
        {
            var ret = new ControlPanelInfo();
            var fromDate = this.n2date(renderer, "fd");
            if (fromDate.HasValue == false)
                fromDate = DMD.DateUtils.ToDay();
            ret.Load(fromDate.Value);
            string tmp = DMD.XML.Utils.Serializer.Serialize(ret);
            return tmp;
        }

        public string Update(object renderer)
        {
            var ret = new ControlPanelInfo();
            var fromDate = this.n2date(renderer, "fd");
            if (fromDate.HasValue == false)
                fromDate = DMD.DateUtils.ToDay();
            ret.Update(fromDate.Value);
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }
    }
}