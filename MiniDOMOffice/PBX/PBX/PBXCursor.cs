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
        /// Cursore di <see cref="PBX"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class PBXCursor 
             : minidom.Databases.DBObjectCursorPO<PBX>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Versione = new DBCursorStringField("Versione");
            private DBCursorField<DateTime> m_DataInstallazione = new DBCursorField<DateTime>("DataInstallazione");
            private DBCursorField<DateTime> m_DataDismissione = new DBCursorField<DateTime>("DataDismissione");
            private DBCursorField<PBXFlags> m_Flags = new DBCursorField<PBXFlags>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public PBXCursor()
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
            /// DataInstallazione
            /// </summary>
            public DBCursorField<DateTime> DataInstallazione
            {
                get
                {
                    return m_DataInstallazione;
                }
            }

            /// <summary>
            /// DataDismissione
            /// </summary>
            public DBCursorField<DateTime> DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<PBXFlags> Flags
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
                return minidom.Office.PBXs;
            }
             
        }
    }
}