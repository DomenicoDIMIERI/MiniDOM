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
        /// Cursore sulla tabella dei motivi appuntamento
        /// </summary>
        [Serializable]
        public class MotiviAppuntamentoCursor 
            : minidom.Databases.DBObjectCursor<MotivoAppuntamento>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<MotivoAppuntamentoFlags> m_Flags = new DBCursorField<MotivoAppuntamentoFlags>("Flags");
            private DBCursorStringField m_TipoAppuntamento = new DBCursorStringField("TipoAppuntamento");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotiviAppuntamentoCursor()
            {
            }

            /// <summary>
            /// TipoAppuntamento
            /// </summary>
            public DBCursorStringField TipoAppuntamento
            {
                get
                {
                    return m_TipoAppuntamento;
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
            public DBCursorField<MotivoAppuntamentoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CRMMotiviAppuntamento";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.MotiviAppuntamento; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}