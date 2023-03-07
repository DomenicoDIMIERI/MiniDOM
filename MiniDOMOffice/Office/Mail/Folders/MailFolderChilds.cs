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
        /// Collezione di cartelle appartenenti ad una cartella "genitore"
        /// </summary>
        [Serializable]
        public class MailFolderChilds
            : MailFoldersCollection
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public MailFolderChilds()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="parent"></param>
            public MailFolderChilds(MailFolder parent) : this()
            {
                if (parent is null)
                    throw new ArgumentNullException("parent");
                Load(parent);
            }
        }
    }
}