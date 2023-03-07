using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella degli handler di ricerca registrati
        /// </summary>
        [Serializable]
        public class CRegisteredFindHandlerCursor
            : minidom.Databases.DBObjectCursor<CRegisteredFindHandler>
        {

            private DBCursorStringField m_HandlerClass = new DBCursorStringField("HandlerClass");
            private DBCursorStringField m_EditorClass = new DBCursorStringField("EditorClass");
            private DBCursorStringField m_Context = new DBCursorStringField("Context");
            private DBCursorField<int> m_Priority = new DBCursorField<int>("Priority");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRegisteredFindHandlerCursor()
            {
            }

            /// <summary>
            /// EditorClass
            /// </summary>
            public DBCursorStringField EditorClass
            {
                get
                {
                    return m_EditorClass;
                }
            }

            /// <summary>
            /// HandlerClass
            /// </summary>
            public DBCursorStringField HandlerClass
            {
                get
                {
                    return m_HandlerClass;
                }
            }

            /// <summary>
            /// Context
            /// </summary>
            public DBCursorStringField Context
            {
                get
                {
                    return m_Context;
                }
            }

            /// <summary>
            /// Priority
            /// </summary>
            public DBCursorField<int> Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CRegisteredFindHandler InstantiateNewT(DBReader dbRis)
            {
                return new CRegisteredFindHandler();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CRMRegFindHandler";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.RegisteredFindPersonaHandlers;
            }

        }
    }
}