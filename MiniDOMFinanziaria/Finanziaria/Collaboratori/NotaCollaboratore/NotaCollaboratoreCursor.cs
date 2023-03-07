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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Cursore sulla tabella degli oggetti <see cref="NotaCollaboratore"/>
        /// </summary>
        [Serializable]
        public class NotaCollaboratoreCursor 
            : minidom.Databases.DBObjectCursor<NotaCollaboratore>
        {
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private DBCursorField<int> m_IDClienteXCollaboratore = new DBCursorField<int>("IDCliXCollab");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Indirizzo = new DBCursorStringField("Indirizzo");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_Esito = new DBCursorField<int>("Esito");
            private DBCursorStringField m_Scopo = new DBCursorStringField("Scopo");
            private DBCursorStringField m_Nota = new DBCursorStringField("Nota");

            /// <summary>
            /// Costruttore
            /// </summary>
            public NotaCollaboratoreCursor()
            {
            }

            /// <summary>
            /// IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// IDCollaboratore
            /// </summary>
            public DBCursorField<int> IDCollaboratore
            {
                get
                {
                    return m_IDCollaboratore;
                }
            }

            /// <summary>
            /// IDClienteXCollaboratore
            /// </summary>
            public DBCursorField<int> IDClienteXCollaboratore
            {
                get
                {
                    return m_IDClienteXCollaboratore;
                }
            }

            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
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
            /// Indirizzo
            /// </summary>
            public DBCursorStringField Indirizzo
            {
                get
                {
                    return m_Indirizzo;
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
                return minidom.Finanziaria.Collaboratori.Note;
            }

            //protected override CDBConnection GetConnection()
            //{
            //    return Database;
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_CQSPDNoteCliXCollab";
            //}
        }
    }
}