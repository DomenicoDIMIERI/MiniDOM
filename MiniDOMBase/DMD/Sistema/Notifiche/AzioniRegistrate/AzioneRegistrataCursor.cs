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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella delle azioni registrate
        /// </summary>
        [Serializable]
        public class AzioneRegistrataCursor 
            : minidom.Databases.DBObjectCursor<AzioneRegistrata>
        {
            private DBCursorField<int> m_Priorita = new DBCursorField<int>("Priorita");
            private DBCursorStringField m_Description = new DBCursorStringField("Description");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_SourceType = new DBCursorStringField("SourceType");
            private DBCursorStringField m_ActionType = new DBCursorStringField("ActionType");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");

            /// <summary>
            /// Costruttore
            /// </summary>
            public AzioneRegistrataCursor()
            {
            }

            /// <summary>
            /// Attivo
            /// </summary>
            public DBCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
                }
            }

            /// <summary>
            /// Priorita
            /// </summary>
            public DBCursorField<int> Priorita
            {
                get
                {
                    return m_Priorita;
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
            /// Description
            /// </summary>
            public DBCursorStringField Description
            {
                get
                {
                    return m_Description;
                }
            }

            /// <summary>
            /// Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// SourceName
            /// </summary>
            public DBCursorStringField SourceName
            {
                get
                {
                    return m_SourceType;
                }
            }

            /// <summary>
            /// ActionType
            /// </summary>
            public DBCursorStringField ActionType
            {
                get
                {
                    return m_ActionType;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Notifiche.AzioniRegistrateRepository;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_SYSNotifyAct";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Notifiche.Database;
            //}
        }
    }
}