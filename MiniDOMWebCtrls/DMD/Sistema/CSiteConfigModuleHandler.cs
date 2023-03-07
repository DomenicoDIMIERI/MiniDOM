using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public class CSiteConfigModuleHandler : CBaseModuleHandler
    {
        public CSiteConfigModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return null;
        }

        public override object GetInternalItemById(int id)
        {
            return WebSite.Instance.Configuration;
        }

        public override string list(object renderer)
        {
            // Return Me.InternalEdit(renderer, WebSite.Instance.Configuration)
            string ret = ""; // Me.list(renderer)
            ret += "<script type=\"text/javascript\">" + DMD.Strings.vbNewLine;
            ret += "Window.addListener(\"onload\", new Function('setTimeout(SystemUtils.EditItem(WebSite.getConfiguration()), 500)'));";
            ret += "</script>";
            return ret;
        }

        public string CreateBackup(object renderer)
        {
            var fd = this.n2date(renderer, "fd");
            var bk = Sistema.Backups.Create(fd, Sistema.Backups.Configuration.CompressionMethod, Sistema.Backups.Configuration.CompressionLevel);
            return bk.Name;
        }

        public string RestoreBackup(object renderer)
        {
            Sistema.Backups.Restore((int) this.n2int(renderer, "id"));
            return DMD.Strings.vbNullString;
        }
    }
}