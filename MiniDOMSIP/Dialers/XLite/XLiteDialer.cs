using System.IO;
using DMD.XML;

namespace minidom.PBX
{
    public class XLiteDialer : DialerBaseClass
    {
        private string m_Path;

        public XLiteDialer()
        {
            m_Path = Get3CXPath();
        }

        public override void Dial(string number)
        {
            if (!IsInstalled())
                return;
            DMD.IOUtils.Shell(m_Path + " -call?to=" + number);
        }

        public override bool IsInstalled()
        {
            return !string.IsNullOrEmpty(m_Path);
        }

        private static string Get3CXPath()
        {
            string p = Path.Combine(GetProgramFilesFolder(), @"CounterPath\X-Lite\X-Lite.exe");
            if (File.Exists(p))
                return p;
            p = Path.Combine(GetProgramFilesFolder() + " (x86)", @"CounterPath\X-Lite\X-Lite.exe");
            if (File.Exists(p))
                return p;
            return DMD.Strings.vbNullString;
        }

        public override string Name
        {
            get
            {
                return "CounterPath X-Lite";
            }
        }

        public override void HangUp()
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is C3CXDialer))
                return false;
            return base.Equals(obj) && (((XLiteDialer)obj).m_Path ?? "") == (m_Path ?? "");
        }
    }
}