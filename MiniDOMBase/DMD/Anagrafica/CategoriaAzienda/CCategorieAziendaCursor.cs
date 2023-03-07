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
        /// Cursore sulla tabella delle categorie azienda
        /// </summary>
        [Serializable]
        public class CCategorieAziendaCursor 
            : Databases.DBObjectCursor<CCategoriaAzienda>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCategorieAziendaCursor()
            {
            }

            /// <summary>
            /// Campo nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return this.m_Flags;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.CategorieAzienda; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CategorieAzienda";
            //}
        }
    }
}