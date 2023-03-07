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
        /// Cursore sulla tabella delle procedure
        /// </summary>
        [Serializable]
        public class CProcedureCursor 
            : minidom.Databases.DBObjectCursor<CProcedura>
        {
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<ProceduraFlags> m_Flags = new DBCursorField<ProceduraFlags>("Flags");
            private DBCursorField<PriorityEnum> m_Priority = new DBCursorField<PriorityEnum>("Priorita");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProcedureCursor()
            {
            }

            /// <summary>
            /// Priority
            /// </summary>
            public DBCursorField<PriorityEnum> Priority
            {
                get
                {
                    return m_Priority;
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
            /// Flags
            /// </summary>
            public DBCursorField<ProceduraFlags> Flags
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
                return minidom.Sistema.Procedure; //.Module;
            }


            //public override string GetTableName()
            //{
            //    return "tbl_CalendarProcs";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}