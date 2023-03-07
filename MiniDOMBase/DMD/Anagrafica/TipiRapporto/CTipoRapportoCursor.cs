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
        /// Cursore di oggetti di tipo <see cref="CTipoRapporto"/>
        /// </summary>
        [Serializable]
        public class CTipoRapportoCursor
            : minidom.Databases.DBObjectCursorBase<CTipoRapporto>
        {
            private DBCursorStringField m_Descrizione = new DBCursorStringField("descrizione");
            private DBCursorStringField m_IdTipoRapporto = new DBCursorStringField("IdTipoRapporto");
            private DBCursorField<TipoRapportoFlags> m_Flags = new DBCursorField<TipoRapportoFlags>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTipoRapportoCursor()
            {
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<TipoRapportoFlags> Flags
            {
                get
                {
                    return m_Flags;
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
            /// IdTipoRapporto
            /// </summary>
            public DBCursorStringField IdTipoRapporto
            {
                get
                {
                    return m_IdTipoRapporto;
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
                return minidom.Anagrafica.TipiRapporto; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "Tiporapporto";
            //}
        }
    }
}