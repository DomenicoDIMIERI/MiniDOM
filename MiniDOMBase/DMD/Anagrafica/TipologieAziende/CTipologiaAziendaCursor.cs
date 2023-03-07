using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella degli oggetti di tipo <see cref="CTipologiaAzienda"/>
        /// </summary>
        public class CTipologiaAziendaCursor 
            : minidom.Databases.DBObjectCursor<CTipologiaAzienda>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<bool> m_RichiedeValutazione = new DBCursorField<bool>("RichVal");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTipologiaAziendaCursor()
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
            /// RichiedeValutazione
            /// </summary>
            public DBCursorField<bool> RichiedeValutazione
            {
                get
                {
                    return m_RichiedeValutazione;
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

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.TipologieAzienda; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_TipologieAzienda";
            //}
        }
    }
}