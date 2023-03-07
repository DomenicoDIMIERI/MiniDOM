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
        /// Collezione di regole sui messaggi definite per un applicazione
        /// </summary>
        [Serializable]
        public class MailApplicationRules 
            : CCollection<MailRule>
        {
            [NonSerialized] private MailApplication m_Application;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailApplicationRules()
            {
                m_Application = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="app"></param>
            public MailApplicationRules(MailApplication app) : this()
            {
                if (app is null)
                    throw new ArgumentNullException("app");
                m_Application = app;
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, MailRule value)
            {
                if (m_Application is object)
                    value.SetApplication(m_Application);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, MailRule oldValue, MailRule newValue)
            {
                if (m_Application is object)
                    newValue.SetApplication(m_Application);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Imposta l'app
            /// </summary>
            /// <param name="app"></param>
            protected internal void SetApplication(MailApplication app)
            {
                m_Application = app;
                if (app is object)
                {
                    foreach (var rule in this)
                        rule.SetApplication(app);
                }
            }
        }
    }
}