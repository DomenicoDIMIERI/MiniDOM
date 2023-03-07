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
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="StickyNote"/>
        /// </summary>
        [Serializable]
        public sealed class CStickyNotesClass 
            : CModulesClass<StickyNote>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CStickyNotesClass() 
                : base("modOfficeStickyNotes", typeof(StickyNotesCursor))
            {
            }
        }
    }

    public partial class Office
    {
        private static CStickyNotesClass m_StickyNotes = null;

        /// <summary>
        /// Repository di <see cref="StickyNote"/>
        /// </summary>
        public static CStickyNotesClass StickyNotes
        {
            get
            {
                if (m_StickyNotes is null)
                    m_StickyNotes = new CStickyNotesClass();
                return m_StickyNotes;
            }
        }
    }
}