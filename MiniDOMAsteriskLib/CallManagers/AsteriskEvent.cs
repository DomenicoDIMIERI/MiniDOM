using System;
using System.Linq;
using DMD;
using DMD.XML;

namespace minidom.CallManagers
{

    /// <summary>
    /// Rappresenta un evento generico di Asterisk CLI
    /// </summary>
    /// <remarks></remarks>
    public class AsteriskEvent : EventArgs
    {
        private DateTime m_Data;
        private AsteriskCallManager m_Manager;
        private System.Collections.Specialized.NameValueCollection m_Attributes; // (Of String)

        public AsteriskEvent()
        {
            DMDObject.IncreaseCounter(this);
            m_Data = DMD.DateUtils.Now();
            m_Attributes = new System.Collections.Specialized.NameValueCollection(); // (Of String)
        }

        public AsteriskEvent(string[] rows) : this()
        {
            Parse(rows);
        }

        public AsteriskEvent(string eventName) : this()
        {
            m_Manager = null;
            SetAttribute("Event", Strings.Trim(eventName));
        }

        public AsteriskEvent(AsteriskCallManager manager, string eventName) : this()
        {
            m_Manager = manager;
            SetAttribute("Event", Strings.Trim(eventName));
        }

        public AsteriskEvent(AsteriskCallManager manager, string[] rows) : this()
        {
            m_Manager = manager;
            Parse(rows);
        }

        public AsteriskEvent(AsteriskCallManager manager, AsteriskEvent e) : this()
        {
            m_Data = e.m_Data;
            m_Manager = manager;
            m_Attributes = e.m_Attributes;
        }

        public AsteriskCallManager Manager
        {
            get
            {
                return m_Manager;
            }
        }

        /// <summary>
        /// Restituisce la data e l'ora dell'evento
        /// </summary>
        /// <returns></returns>
        public DateTime Data
        {
            get
            {
                return m_Data;
            }
        }

        public string GetAttribute(string attrName)
        {
            attrName = Strings.UCase(Strings.Trim(attrName));
            return m_Attributes.Get(attrName); // .GetItemByKey(attrName)
        }

        private bool ContainsKey(string key)
        {
            if (m_Attributes.Get(key) is null)
                return m_Attributes.AllKeys.Contains(key);
            return true;
        }

        public int GetAttribute(string attrName, int defValue)
        {
            attrName = Strings.UCase(Strings.Trim(attrName));
            if (ContainsKey(attrName) == false)
                return defValue;
            return DMD.Integers.ValueOf(m_Attributes.Get(attrName));
        }

        public float GetAttribute(string attrName, float defValue)
        {
            attrName = Strings.UCase(Strings.Trim(attrName));
            if (ContainsKey(attrName) == false)
                return defValue;
            return DMD.Floats.CSng(m_Attributes.Get(attrName));
        }

        public void SetAttribute(string attrName, string attrValue)
        {
            attrName = Strings.UCase(Strings.Trim(attrName));
            // If Not Me.AttrMe.m_Attributes.ContainsKey(attrName) Then
            // Me.m_Attributes(attrName) = attrValue
            // Else
            // Me.m_Attributes.Add(attrName, attrValue)
            // End If
            m_Attributes.Set(attrName, attrValue);
        }

        public void RemoveAttribute(string attrName)
        {
            attrName = Strings.UCase(Strings.Trim(attrName));
            m_Attributes.Remove(attrName);
            // Me.m_Attributes.RemoveByKey(attrName)
        }

        /// <summary>
        /// Restituisce il nome dell'evento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string EventName
        {
            get
            {
                return GetAttribute("Event");
            }
        }


        /// <summary>
        /// Se presente l'evento è parte della risposta all'azione con l'ID specificato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ActionID
        {
            get
            {
                return GetAttribute("ActionID");
            }
        }

        protected internal void Parse(string[] rows)
        {
            for (int i = 0, loopTo = DMD.Arrays.UBound(rows); i <= loopTo; i++)
            {
                var row = new Responses.RowEntry(rows[i]);
                ParseRow(row);
            }
        }

        protected virtual void ParseRow(Responses.RowEntry row)
        {
            // Select Case UCase(row.Command)
            // Case "EVENT" : Me.m_EventName = row.Params
            // Case "PRIVILEGE" : Me.m_Privilege = row.Params
            // Case "ACTIONID" : Me.m_ActionID = row.Params
            // Case Else : Debug.Print("Unsupported: " & row.Command)
            // End Select
            SetAttribute(row.Command, row.Params);
        }

        public override string ToString()
        {
            var ret = new System.Text.StringBuilder();
            foreach (string k in m_Attributes.Keys)
                ret.Append(k + ": " + m_Attributes[k] + DMD.Strings.vbCrLf);
            return ret.ToString();
        }

        ~AsteriskEvent()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}