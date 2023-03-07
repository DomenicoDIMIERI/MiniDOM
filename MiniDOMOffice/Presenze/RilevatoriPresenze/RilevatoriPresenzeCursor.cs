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
        /// Cursore di <see cref="RilevatorePresenze"/>
        /// </summary>
        [Serializable]
        public class RilevatoriPresenzeCursor 
            : DeviceCursor<RilevatorePresenze>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Modello = new DBCursorStringField("Modello");

            /// <summary>
            /// Costruttore
            /// </summary>
            public RilevatoriPresenzeCursor()
            {
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Tipo
            /// </summary>
            public DBCursorStringField Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            /// <summary>
            /// Modello
            /// </summary>
            public DBCursorStringField Modello
            {
                get
                {
                    return m_Modello;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.RilevatoriPresenze;
            }
        }
    }
}