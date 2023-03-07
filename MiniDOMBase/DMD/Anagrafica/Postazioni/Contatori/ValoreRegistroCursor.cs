using System;
using DMD.XML;
using DMD;
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
        /// Cursore sulla tabella delle postazioni di lavoro
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ValoreRegistroCursor 
            : minidom.Databases.DBObjectCursorPO<ValoreRegistroContatore>
        {
            private DBCursorField<int> m_IDPostazione = new DBCursorField<int>("IDPostazione");
            private DBCursorStringField m_NomePostazione = new DBCursorStringField("NomePostazione");
            private DBCursorField<DateTime> m_DataRegistrazione = new DBCursorField<DateTime>("DataRegistrazione");
            private DBCursorStringField m_NomeRegistro = new DBCursorStringField("NomeRegistro");
            private DBCursorField<double> m_ValoreRegistro = new DBCursorField<double>("ValoreRegistro");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ValoreRegistroCursor()
            {
            }

            /// <summary>
            /// IDPostazione
            /// </summary>
            public DBCursorField<int> IDPostazione
            {
                get
                {
                    return m_IDPostazione;
                }
            }

            /// <summary>
            /// NomePostazione
            /// </summary>
            public DBCursorStringField NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }
            }

            /// <summary>
            /// DataRegistrazione
            /// </summary>
            public DBCursorField<DateTime> DataRegistrazione
            {
                get
                {
                    return m_DataRegistrazione;
                }
            }

            /// <summary>
            /// NomeRegistro
            /// </summary>
            public DBCursorStringField NomeRegistro
            {
                get
                {
                    return m_NomeRegistro;
                }
            }

            /// <summary>
            /// ValoreRegistro
            /// </summary>
            public DBCursorField<double> ValoreRegistro
            {
                get
                {
                    return m_ValoreRegistro;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public override ValoreRegistroContatore InstantiateNewT(DBReader reader)
            {
                return new ValoreRegistroContatore();
            }
 
            //public override string GetTableName()
            //{
            //    return "tbl_PostazoniRegistri";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Postazioni.ValoriRegistri;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}