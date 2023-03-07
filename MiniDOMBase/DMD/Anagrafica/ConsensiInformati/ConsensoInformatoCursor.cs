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
        /// Cursore sulla tabella dei consensi informati
        /// </summary>
        [Serializable]
        public class ConsensoInformatoCursor 
            : minidom.Databases.DBObjectCursorBase<ConsensoInformato>
        {
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<DateTime> m_DataConsenso = new DBCursorField<DateTime>("DataConsenso");
            private DBCursorField<bool> m_Consenso = new DBCursorField<bool>("Consenso");
            private DBCursorField<bool> m_Richiesto = new DBCursorField<bool>("Richiesto");
            private DBCursorStringField m_NomeDocumento = new DBCursorStringField("NomeDocumento");
            private DBCursorStringField m_DescrizioneDocumento = new DBCursorStringField("DescrizioneDocumento");
            private DBCursorStringField m_LinkDocumentoVisionato = new DBCursorStringField("LinkVisionato");
            private DBCursorStringField m_LinkDocumentoFirmato = new DBCursorStringField("LinkFirmato");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ConsensoInformatoCursor()
            {
            }

            /// <summary>
            /// Campo IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// Campo NomePersona
            /// </summary>
            public DBCursorStringField NomePersona
            {
                get
                {
                    return m_NomePersona;
                }
            }

            /// <summary>
            /// Campo Consenso
            /// </summary>
            public DBCursorField<bool> Consenso
            {
                get
                {
                    return m_Consenso;
                }
            }

            /// <summary>
            /// Campo Richiesto
            /// </summary>
            public DBCursorField<bool> Richiesto
            {
                get
                {
                    return m_Richiesto;
                }
            }

            /// <summary>
            /// Campo DataConsenso
            /// </summary>
            public DBCursorField<DateTime> DataConsenso
            {
                get
                {
                    return m_DataConsenso;
                }
            }

            /// <summary>
            /// Campo NomeDocumento
            /// </summary>
            public DBCursorStringField NomeDocumento
            {
                get
                {
                    return m_NomeDocumento;
                }
            }

            /// <summary>
            /// Campo DescrizioneDocumento
            /// </summary>
            public DBCursorStringField DescrizioneDocumento
            {
                get
                {
                    return m_DescrizioneDocumento;
                }
            }

            /// <summary>
            /// Campo LinkDocumentoVisionato
            /// </summary>
            public DBCursorStringField LinkDocumentoVisionato
            {
                get
                {
                    return m_LinkDocumentoVisionato;
                }
            }

            /// <summary>
            /// Campo LinkDocumentoFirmato
            /// </summary>
            public DBCursorStringField LinkDocumentoFirmato
            {
                get
                {
                    return m_LinkDocumentoFirmato;
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
                return minidom.Anagrafica.ConsensiInformati; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_PersoneConsensi";
            //}
        }
    }
}