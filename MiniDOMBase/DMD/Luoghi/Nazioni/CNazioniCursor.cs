using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        [Serializable]
        public class CNazioniCursor
            : LuogoCursorISTAT<CNazione>
        {
            private DBCursorStringField m_SantoPatrono = new DBCursorStringField("SantoPatrono");
            private DBCursorStringField m_GiornoFestivo = new DBCursorStringField("GiornoFestivo");
            private DBCursorStringField m_Prefisso = new DBCursorStringField("Prefisso");
            private DBCursorStringField m_Sigla = new DBCursorStringField("Sigla");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CNazioniCursor()
            {
            }

            /// <summary>
            /// SantoPatrono
            /// </summary>
            public DBCursorStringField SantoPatrono
            {
                get
                {
                    return m_SantoPatrono;
                }
            }

            /// <summary>
            /// GiornoFestivo
            /// </summary>
            public DBCursorStringField GiornoFestivo
            {
                get
                {
                    return m_GiornoFestivo;
                }
            }

            /// <summary>
            /// Prefisso
            /// </summary>
            public DBCursorStringField Prefisso
            {
                get
                {
                    return m_Prefisso;
                }
            }

            /// <summary>
            /// Sigla
            /// </summary>
            public DBCursorStringField Sigla
            {
                get
                {
                    return m_Sigla;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Nazioni; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Luoghi_Nazioni";
            //}
        }
    }
}