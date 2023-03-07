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
using static minidom.Store;


namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Cursore di <see cref="Magazzino"/>
        /// </summary>
        [Serializable]
        public class MagazzinoCursor
            : Databases.DBObjectCursor<Magazzino>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MagazzinoCursor()
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
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.Magazzini;
            }

             
        }
    }
}