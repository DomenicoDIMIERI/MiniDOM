using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Collezione di <see cref="MailAddress"/>
        /// </summary>
        [Serializable]
        public class MailAddressCollection 
            : CCollection<MailAddress>
        {
            [NonSerialized] private MailMessage m_Message;
            private string m_FieldName;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailAddressCollection()
            {
                m_Message = null;
                m_FieldName = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="msg"></param>
            /// <param name="fieldName"></param>
            public MailAddressCollection(MailMessage msg, string fieldName) : this()
            {
                m_Message = msg;
                m_FieldName = fieldName;
                foreach (MailAddress m in msg.GetOriginalAdressies())
                {
                    if ((m.FieldName ?? "") == (fieldName ?? ""))
                    {
                        Add(m);
                    }
                }
            }

            /// <summary>
            /// Restituisce un riferimento all'applicazione
            /// </summary>
            public MailApplication Application
            {
                get
                {
                    if (m_Message is null)
                        return null;
                    return m_Message.Application;
                }
            }

            /// <summary>
            /// Restituisce un riferimento al messaggio
            /// </summary>
            public MailMessage Message
            {
                get
                {
                    return m_Message;
                }
            }

            /// <summary>
            /// Imposta il messaggio
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetMessage(MailMessage value)
            {
                m_Message = value;
                if (value is object)
                {
                    foreach (MailAddress m in this)
                    {
                        m.SetApplication(value.Application);
                        m.SetMessage(value);
                    }
                }
            }

            /// <summary>
            /// Imposta il nome del file
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetFieldName(string value)
            {
                m_FieldName = value;
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, MailAddress value)
            {
                if (m_Message is object)
                {
                    value.SetApplication(Application);
                    value.SetMessage(m_Message);
                    value.FieldName = m_FieldName;
                }

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, MailAddress oldValue, MailAddress newValue)
            {
                if (m_Message is object)
                {
                    newValue.SetApplication(Application);
                    newValue.SetMessage(m_Message);
                    newValue.FieldName = m_FieldName;
                }

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder(200 * this.Count);
                foreach (var a in this)
                {
                    if (ret.Length > 0)
                        ret.Append(", ");
                    ret.Append(a.ToString());
                }

                return ret.ToString();
            }
        }
    }
}