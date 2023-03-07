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
    public partial class Anagrafica
    {



        /// <summary>
        /// Cursore di oggetti di tipo <see cref="CUtenteXUfficio"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUtentiXUfficioCursor 
            : minidom.Databases.DBObjectCursorBase<CUtenteXUfficio>
        {
            private DBCursorField<int> m_IDUtente;
            private DBCursorField<int> m_IDUfficio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUtentiXUfficioCursor()
            {
                m_IDUtente = new DBCursorField<int>("Utente");
                m_IDUfficio = new DBCursorField<int>("Ufficio");
            }

            /// <summary>
            /// ID utente
            /// </summary>
            public DBCursorField<int> IDUtente
            {
                get
                {
                    return m_IDUtente;
                }
            }

            /// <summary>
            /// ID ufficio
            /// </summary>
            public DBCursorField<int> IDUfficio
            {
                get
                {
                    return m_IDUfficio;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CUtenteXUfficio InstantiateNewT(DBReader dbRis)
            {
                return new CUtenteXUfficio();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_UtentiXUfficio";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Uffici.UfficiConsentiti;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}
        }
    }
}