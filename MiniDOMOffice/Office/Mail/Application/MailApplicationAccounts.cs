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
        /// Collezione di account appartenenti ad un'app
        /// </summary>
        [Serializable]
        public class MailApplicationAccounts 
            : CCollection<MailAccount>
        {
            [NonSerialized] private MailApplication m_Application;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailApplicationAccounts()
            {
                m_Application = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="app"></param>
            public MailApplicationAccounts(MailApplication app) 
                : this()
            {
                if (app is null)
                    throw new ArgumentNullException("app");
                Load(app);
            }

            /// <summary>
            /// Carica
            /// </summary>
            /// <param name="app"></param>
            protected internal void Load(MailApplication app)
            {
                if (app is null)
                    throw new ArgumentNullException("app");

                this.Clear();
                
                this.m_Application = app;

                if (DBUtils.GetID(app, 0) == 0)
                    return;

                using (var cursor = new MailAccountCursor())
                {
                    cursor.ApplicationID.Value = DBUtils.GetID(app, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }
        }
    }
}