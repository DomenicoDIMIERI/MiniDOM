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
        /// Cursore sulla tabella dei comuni
        /// </summary>
        [Serializable]
        public class CComuniCursor 
            : LuogoCursorISTAT<CComune>
        {
            private DBCursorStringField m_SantoPatrono;
            private DBCursorStringField m_GiornoFestivo;
            private DBCursorStringField m_CAP;
            private DBCursorStringField m_Prefisso;
            private DBCursorStringField m_Provincia;
            private DBCursorStringField m_Sigla;
            private DBCursorStringField m_Regione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CComuniCursor()
            {
                m_SantoPatrono = new DBCursorStringField("SantoPatrono");
                m_GiornoFestivo = new DBCursorStringField("GiornoFestivo");
                m_CAP = new DBCursorStringField("CAP");
                m_Prefisso = new DBCursorStringField("Prefisso");
                m_Provincia = new DBCursorStringField("Provincia");
                m_Sigla = new DBCursorStringField("Sigla");
                m_Regione = new DBCursorStringField("Regione");
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
            /// CAP
            /// </summary>
            public DBCursorStringField CAP
            {
                get
                {
                    return m_CAP;
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
            /// Provincia
            /// </summary>
            public DBCursorStringField Provincia
            {
                get
                {
                    return m_Provincia;
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

            /// <summary>
            /// Regione
            /// </summary>
            public DBCursorStringField Regione
            {
                get
                {
                    return m_Regione;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CComune InstantiateNewT(DBReader dbRis)
            {
                return new CComune();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Luoghi_Comuni";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Comuni; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}
        }
    }
}