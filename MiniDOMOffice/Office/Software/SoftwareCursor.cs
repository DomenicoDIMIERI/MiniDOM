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
        /// Cursore di <see cref="Software"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class SoftwareCursor : Databases.DBObjectCursor<Software>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Versione = new DBCursorStringField("Versione");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorStringField m_Classe = new DBCursorStringField("Classe");
            private DBCursorStringField m_Autore = new DBCursorStringField("Autore");
            private DBCursorField<DateTime> m_DataPubblicazione = new DBCursorField<DateTime>("DataPubblicazione");
            private DBCursorField<DateTime> m_DataRitiro = new DBCursorField<DateTime>("DataRitiro");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public SoftwareCursor()
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
            /// Versione
            /// </summary>
            public DBCursorStringField Versione
            {
                get
                {
                    return m_Versione;
                }
            }

            /// <summary>
            /// Autore
            /// </summary>
            public DBCursorStringField Autore
            {
                get
                {
                    return m_Autore;
                }
            }

            /// <summary>
            /// Classe
            /// </summary>
            public DBCursorStringField Classe
            {
                get
                {
                    return m_Classe;
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
            /// DataPubblicazione
            /// </summary>
            public DBCursorField<DateTime> DataPubblicazione
            {
                get
                {
                    return m_DataPubblicazione;
                }
            }

            /// <summary>
            /// DataRitiro
            /// </summary>
            public DBCursorField<DateTime> DataRitiro
            {
                get
                {
                    return m_DataRitiro;
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
                return minidom.Office.Softwares;
            }
             
        }
    }
}