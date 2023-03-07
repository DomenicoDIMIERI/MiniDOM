using DMD;

namespace minidom.CallManagers.Responses
{
    public class RowEntry
    {
        private string m_Command;
        private string m_Params;

        public RowEntry()
        {
            DMDObject.IncreaseCounter(this);
        }

        public RowEntry(string row) : this()
        {
            Parse(row);
        }

        internal virtual void Parse(string row)
        {
            int p = Strings.InStr(row, ":");
            if (p > 0)
            {
                m_Command = Strings.Left(row, p - 1);
                m_Params = Strings.Mid(row, p + 2);
            }
            else
            {
                m_Command = "";
                m_Params = row;
            }
        }

        ~RowEntry()
        {
            DMDObject.DecreaseCounter(this);
        }

        public string Command
        {
            get
            {
                return m_Command;
            }
        }

        public string Params
        {
            get
            {
                return m_Params;
            }
        }
    }
}