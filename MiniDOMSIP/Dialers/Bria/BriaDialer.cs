using System.IO;
using DMD.XML;

namespace minidom.PBX
{
    public class BriaDialer : DialerBaseClass
    {
        private string m_Path;

        public BriaDialer()
        {
            m_Path = GetBriaPath();
        }

        public override void Dial(string number)
        {
            if (!IsInstalled())
                return;
            DMD.IOUtils.Shell(m_Path + " -dial=sip:" + number);
        }

        public override bool IsInstalled()
        {
            return !string.IsNullOrEmpty(m_Path);
        }

        private static string GetBriaPath()
        {
            string p = Path.Combine(GetProgramFilesFolder(), @"CounterPath\Bria\bria.exe");
            if (File.Exists(p))
                return p;
            p = Path.Combine(GetProgramFilesFolder() + " (x86)", @"CounterPath\Bria\bria.exe");
            if (File.Exists(p))
                return p;
            return DMD.Strings.vbNullString;
        }

        public override string Name
        {
            get
            {
                return "CounterPath Bria";
            }
        }

        public override void HangUp()
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is C3CXDialer))
                return false;
            return base.Equals(obj) && (((BriaDialer)obj).m_Path ?? "") == (m_Path ?? "");
        }
    }
}