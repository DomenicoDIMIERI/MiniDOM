using System.IO;
using DMD.XML;

namespace minidom.PBX
{

    /// <summary>
    /// Dialler per usare il software client 3CX
    /// </summary>
    public class C3CXDialer 
        : DialerBaseClass
    {
        private string m_Path;

        /// <summary>
        /// Costruttore
        /// </summary>
        public C3CXDialer()
        {
            m_Path = Get3CXPath();
        }

        /// <summary>
        /// Prepara il numero da comporre
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string PrepareNumber(string number)
        {
            return DMD.Strings.Trim(number);
        }

        /// <summary>
        /// Compone il numero
        /// </summary>
        /// <param name="number"></param>
        public override void Dial(string number)
        {
            if (!IsInstalled())
                return;
            DMD.IOUtils.Shell(m_Path + " dial:" + PrepareNumber(number));
        }

        /// <summary>
        /// Restituisce true se il programma é installato
        /// </summary>
        /// <returns></returns>
        public override bool IsInstalled()
        {
            return !string.IsNullOrEmpty(m_Path);
        }

        private static string Get3CXPath()
        {
            string p = Path.Combine(GetProgramFilesFolder(), @"3CXPhone\3CXPhone.exe");
            if (File.Exists(p))
                return p;
            p = Path.Combine(GetProgramFilesFolder() + " (x86)", @"3CXPhone\3CXPhone.exe");
            if (File.Exists(p))
                return p;
            return DMD.Strings.vbNullString;
        }

        /// <summary>
        /// Nome univoco del dialler
        /// </summary>
        public override string Name
        {
            get
            {
                return "3CX VoIP Phone";
            }
        }

        /// <summary>
        /// Aggancia
        /// </summary>
        public override void HangUp()
        {
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is C3CXDialer))
                return false;
            return base.Equals(obj) && (((C3CXDialer)obj).m_Path ?? "") == (m_Path ?? "");
        }
         
    }
}