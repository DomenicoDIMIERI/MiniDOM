using System;
using DMD;
using DMD.XML;
using minidom.Asterisk;

namespace minidom.CallManagers.Responses
{
    public class ExtensionStateResponse : ActionResponse
    {

        // Private m_ActionID As Integer
        private string m_Exten;
        private string m_Context;
        private string m_Hint;
        private ast_extension_states m_Status;

        public ExtensionStateResponse()
        {
        }

        public ExtensionStateResponse(Action action) : base(action)
        {
        }

        // Public ReadOnly Property ActionID As Integer
        // Get
        // Return Me.m_ActionID
        // End Get
        // End Property

        public string Exten
        {
            get
            {
                return m_Exten;
            }
        }

        public string Context
        {
            get
            {
                return m_Context;
            }
        }

        public string Hint
        {
            get
            {
                return m_Hint;
            }
        }

        public ast_extension_states Status
        {
            get
            {
                return m_Status;
            }
        }

        protected override void ParseRow(RowEntry row)
        {
            switch (Strings.UCase(row.Command) ?? "")
            {
                // Case "ACTIONID" : Me.m_ActionID = CInt(row.Params)
                case "EXTEN":
                    {
                        m_Exten = row.Params;
                        break;
                    }

                case "CONTEXT":
                    {
                        m_Context = row.Params;
                        break;
                    }

                case "HINT":
                    {
                        m_Hint = row.Params;
                        break;
                    }

                case "STATUS":
                    {
                        m_Status = (ast_extension_states)DMD.Integers.ValueOf(row.Params);
                        break;
                    }

                default:
                    {
                        base.ParseRow(row);
                        break;
                    }
            }
        }

        // Public Overrides Function ToString() As String
        // Return MyBase.ToString() & "(Context: " & Me.m_Context & ", Exten: " & Me.m_Exten & ", ActionID: " & Me.m_ActionID & ", Hint: " & Me.m_Hint & ", Status: " & Me.GetStatus(Me.m_Status) & ")"
        // End Function

        private string GetStatus(ast_extension_states s)
        {
            string ret = "";
            foreach (ast_extension_states f in Enum.GetValues(typeof(ast_extension_states)))
            {
                if ((s & f) == f)
                {
                    if (!string.IsNullOrEmpty(ret))
                        ret += ", ";
                    ret += Enum.GetName(typeof(ast_extension_states), f);
                }
            }

            return ((int)s).ToString();
        }
    }
}