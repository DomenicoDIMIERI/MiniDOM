using System;
using DMD;
using DMD.Databases;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle forme giuridiche aziendali
        /// </summary>
        [Serializable]
        public class CFormeGiuridicheAziendaCursor
            : minidom.Databases.DBObjectCursor<CFormaGiuridicaAzienda>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFormeGiuridicheAziendaCursor()
            {
            }

            /// <summary>
            /// Campo Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return this.m_Nome;
                }
            }

            /// <summary>
            /// Campo Nome
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return this.m_Flags;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            //protected override Sistema.CModule GetModule()
            //{
            //    return FormeGiuridicheAzienda.Module;
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_FormeGiuridicheAzienda";
            //}
        }
    }
}