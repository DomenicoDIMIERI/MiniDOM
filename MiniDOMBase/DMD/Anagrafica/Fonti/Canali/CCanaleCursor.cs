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
        /// Cursore sulla tabella dei canali
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCanaleCursor 
            : minidom.Databases.DBObjectCursor<CCanale>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorField<bool> m_Valid = new DBCursorField<bool>("Valid");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCanaleCursor()
            {
            }

            /// <summary>
            /// Campo Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Campo Tipo
            /// </summary>
            public DBCursorStringField Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            /// <summary>
            /// Campo IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// Campo Valid
            /// </summary>
            public DBCursorField<bool> Valid
            {
                get
                {
                    return m_Valid;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Canali; //.Module;
            }

            ///// <summary>
            ///// Restituisce il nome della tabella
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_ANACanali";
            //}
        }
    }
}