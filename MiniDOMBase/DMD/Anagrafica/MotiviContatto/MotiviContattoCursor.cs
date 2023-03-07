using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella del motivi contatto
        /// </summary>
        [Serializable]
        public class MotiviContattoCursor
            : minidom.Databases.DBObjectCursor<MotivoContatto>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<MotivoContattoFlags> m_Flags = new DBCursorField<MotivoContattoFlags>("Flags");
            private DBCursorStringField m_TipoContatto = new DBCursorStringField("TipoContatto");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotiviContattoCursor()
            {
            }

            /// <summary>
            /// TipoContatto
            /// </summary>
            public DBCursorStringField TipoContatto
            {
                get
                {
                    return m_TipoContatto;
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
            /// Flags
            /// </summary>
            public DBCursorField<MotivoContattoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CRMMotiviContatto";
            //}


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.MotiviContatto; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}