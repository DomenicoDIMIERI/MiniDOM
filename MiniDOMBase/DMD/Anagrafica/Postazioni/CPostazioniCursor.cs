using System;
using DMD.XML;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Collections;
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
        public class CPostazioniCursor
            : minidom.Databases.DBObjectCursorPO<CPostazione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<FlagsPostazioneLavoro> m_Flags = new DBCursorField<FlagsPostazioneLavoro>("Flags");
            private DBCursorField<int> m_IDUtentePrincipale = new DBCursorField<int>("IDUtentePrincipale");
            private DBCursorStringField m_NomeUtentePrincipale = new DBCursorStringField("NomeUtentePrincipale");
            private DBCursorStringField m_NomeReparto = new DBCursorStringField("NomeReparto");
            private DBCursorStringField m_InternoTelefonico = new DBCursorStringField("InternoTelefonico");
            private DBCursorStringField m_SistemaOperativo = new DBCursorStringField("SistemaOperativo");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_SottoCategoria = new DBCursorStringField("SottoCategoria");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPostazioniCursor()
            {
            }

            /// <summary>
            /// Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// SottoCategoria
            /// </summary>
            public DBCursorStringField SottoCategoria
            {
                get
                {
                    return m_SottoCategoria;
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
            /// Flags
            /// </summary>
            public DBCursorField<FlagsPostazioneLavoro> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// IDUtentePrincipale
            /// </summary>
            public DBCursorField<int> IDUtentePrincipale
            {
                get
                {
                    return m_IDUtentePrincipale;
                }
            }

            /// <summary>
            /// NomeUtentePrincipale
            /// </summary>
            public DBCursorStringField NomeUtentePrincipale
            {
                get
                {
                    return m_NomeUtentePrincipale;
                }
            }

            /// <summary>
            /// NomeReparto
            /// </summary>
            public DBCursorStringField NomeReparto
            {
                get
                {
                    return m_NomeReparto;
                }
            }

            /// <summary>
            /// InternoTelefonico
            /// </summary>
            public DBCursorStringField InternoTelefonico
            {
                get
                {
                    return m_InternoTelefonico;
                }
            }

            /// <summary>
            /// SistemaOperativo
            /// </summary>
            public DBCursorStringField SistemaOperativo
            {
                get
                {
                    return m_SistemaOperativo;
                }
            }

            /// <summary>
            /// Note
            /// </summary>
            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CPostazione InstantiateNewT(DBReader dbRis)
            {
                return new CPostazione();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_PostazioniLavoro";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Postazioni; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}