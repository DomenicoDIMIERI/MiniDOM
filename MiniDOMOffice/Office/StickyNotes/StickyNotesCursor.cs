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
        /// Cursore di <see cref="StickyNote"/>
        /// </summary>
        [Serializable]
        public class StickyNotesCursor 
            : Databases.DBObjectCursorPO<StickyNote>
        {
            private DBCursorStringField m_Text = new DBCursorStringField("Text");
            private DBCursorField<StickyNoteFlags> m_Flags = new DBCursorField<StickyNoteFlags>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public StickyNotesCursor()
            {

            }

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBCursorStringField Text
            {
                get
                {
                    return m_Text;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<StickyNoteFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Repositoy
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.StickyNotes;
            }
        }
    }
}