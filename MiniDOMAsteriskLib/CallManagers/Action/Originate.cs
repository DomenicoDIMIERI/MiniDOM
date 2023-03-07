using DMD;
using minidom.CallManagers.Responses;

namespace minidom.CallManagers.Actions
{
    public class Originate : AsyncAction
    {
        private string m_Channel;
        private string m_Context;
        private string m_Exten;
        private int? m_Priority;
        private int m_Timeout;
        private string m_CallerID;
        private string m_Variable;
        private string m_Account;
        private string m_Application;
        private string m_Data;
        private bool m_Async;

        public Originate() : base("Originate")
        {
            m_Channel = "";
            m_Context = "";
            m_Exten = "";
            m_Priority = default;
            m_Timeout = 30000;
            m_CallerID = "";
            m_Variable = "";
            m_Account = "";
            m_Application = "";
            m_Data = "";
            m_Async = false;
        }

        public Originate(string channel, string callerID, string account, string variable, string context, string exten, int priority, string application, string data, string actionID, bool async = false, int timeout = 30000) : this()
        {
            m_Channel = channel;
            m_Context = context;
            m_Priority = priority;
            m_Timeout = timeout;
            m_CallerID = callerID;
            m_Variable = variable;
            m_Account = account;
            m_Application = application;
            m_Data = data;
            m_Async = async;
            m_Exten = exten;
        }

        public Originate(string channel, string callerID, string account, string variable, string application, string data, string actionID, bool async = false, int timeout = 30000) : this()
        {
            m_Channel = channel;
            m_Context = Context;
            m_Priority = Priority;
            m_Timeout = timeout;
            m_CallerID = callerID;
            m_Variable = variable;
            m_Account = account;
            m_Application = application;
            m_Data = data;
            m_Async = async;
        }

        /// <summary>
        /// Channel on which to originate the call (The same as you specify in the Dial application command)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Channel
        {
            get
            {
                return m_Channel;
            }

            set
            {
                m_Channel = value;
            }
        }

        /// <summary>
        /// Context to use on connect (must use Exten and Priority with it)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Context
        {
            get
            {
                return m_Context;
            }

            set
            {
                m_Context = value;
            }
        }

        /// <summary>
        /// Priority to use on connect (must use Context and Exten with it)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int? Priority
        {
            get
            {
                return m_Priority;
            }

            set
            {
                m_Priority = value;
            }
        }

        /// <summary>
        /// Timeout (in milliseconds) for the originating connection to happen(defaults to 30000 milliseconds)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Timeout
        {
            get
            {
                return m_Timeout;
            }

            set
            {
                m_Timeout = value;
            }
        }

        /// <summary>
        /// CallerID to use for the call
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string CallerID
        {
            get
            {
                return m_CallerID;
            }

            set
            {
                m_CallerID = value;
            }
        }

        /// <summary>
        /// Channels variables to set (max 32). Variables will be set for both channels (local and connected).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Variable
        {
            get
            {
                return m_Variable;
            }

            set
            {
                m_Variable = value;
            }
        }

        /// <summary>
        /// Account code for the call
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Account
        {
            get
            {
                return m_Account;
            }

            set
            {
                m_Account = value;
            }
        }

        /// <summary>
        /// Application to use on connect (use Data for parameters)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Application
        {
            get
            {
                return m_Application;
            }

            set
            {
                m_Application = value;
            }
        }

        /// <summary>
        /// Data if Application parameter is used
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Data
        {
            get
            {
                return m_Data;
            }

            set
            {
                m_Data = value;
            }
        }


        /// <summary>
        /// For the origination to be asynchronous (allows multiple calls to be generated without waiting for a response)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Async
        {
            get
            {
                return m_Async;
            }

            set
            {
                m_Async = value;
            }
        }


        /// <summary>
        /// Extension to use on connect (must use Context and Priority with it)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Exten
        {
            get
            {
                return m_Exten;
            }

            set
            {
                m_Exten = value;
            }
        }

        protected override string GetCommandText()
        {
            string cmd = base.GetCommandText();
            cmd += "Channel: " + Channel + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(Context) || !string.IsNullOrEmpty(Exten) || Priority.HasValue)
            {
                cmd += "Context: " + Context + DMD.Strings.vbCrLf;
                cmd += "Exten: " + Exten + DMD.Strings.vbCrLf;
                cmd += "Priority: " + Priority + DMD.Strings.vbCrLf;
            }

            cmd += "Timeout: " + Timeout + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(CallerID))
                cmd += "Callerid: " + CallerID + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(Variable))
                cmd += "Variable: " + Variable + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(Account))
                cmd += "Account: " + Account + DMD.Strings.vbCrLf;
            if (!string.IsNullOrEmpty(Application) || !string.IsNullOrEmpty(Data))
            {
                cmd += "Application: " + Application + DMD.Strings.vbCrLf;
                cmd += "Data: " + Data + DMD.Strings.vbCrLf;
            }

            if (m_Async)
                cmd += "Async: yes" + DMD.Strings.vbCrLf;
            return cmd;
        }

        protected override ActionResponse AllocateResponse(string[] rows)
        {
            return new OriginateResponse(this);
        }
    }
}