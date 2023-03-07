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
using static minidom.Contabilita;


namespace minidom
{
    public partial class Contabilita
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="VoceDiPagamento"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class VoceDiPagamentoCursor 
            : Databases.DBObjectCursorPO<VoceDiPagamento>
        {
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<decimal> m_Quantita = new DBCursorField<decimal>("Quantita");
            private DBCursorStringField m_NomeValuta = new DBCursorStringField("NomeValuta");
            private DBCursorField<DateTime> m_DataOperazione = new DBCursorField<DateTime>("DataOperazione");
            private DBCursorField<DateTime> m_DataEffettiva = new DBCursorField<DateTime>("DataEffettiva");
            private DBCursorStringField m_SourceType = new DBCursorStringField("SourceType");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorStringField m_SourceParams = new DBCursorStringField("SourceParams");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDCCOrigine = new DBCursorField<int>("IDCCOrigine");
            private DBCursorStringField m_NomeCCOrigine = new DBCursorStringField("NomeCCOrigine");
            private DBCursorField<int> m_IDCCDestinazione = new DBCursorField<int>("IDCCDestinazione");
            private DBCursorStringField m_NomeCCDestinazione = new DBCursorStringField("NomeCCDestinazione");
            private DBCursorStringField m_TipoMetodoDiPagamento = new DBCursorStringField("TipoMetodoDiPagamento");
            private DBCursorField<int> m_IDMetodoDiPagamento = new DBCursorField<int>("IDMetodotoDiPagamento");
            private DBCursorStringField m_NomeMetodoDiPagamento = new DBCursorStringField("NomeMetodoDiPagamento");

            /// <summary>
            /// Costruttore
            /// </summary>
            public VoceDiPagamentoCursor()
            {
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
            /// Quantita
            /// </summary>
            public DBCursorField<decimal> Quantita
            {
                get
                {
                    return m_Quantita;
                }
            }

            /// <summary>
            /// NomeValuta
            /// </summary>
            public DBCursorStringField NomeValuta
            {
                get
                {
                    return m_NomeValuta;
                }
            }

            /// <summary>
            /// DataOperazione
            /// </summary>
            public DBCursorField<DateTime> DataOperazione
            {
                get
                {
                    return m_DataOperazione;
                }
            }

            /// <summary>
            /// DataEffettiva
            /// </summary>
            public DBCursorField<DateTime> DataEffettiva
            {
                get
                {
                    return m_DataEffettiva;
                }
            }

            /// <summary>
            /// SourceType
            /// </summary>
            public DBCursorStringField SourceType
            {
                get
                {
                    return m_SourceType;
                }
            }

            /// <summary>
            /// SourceID
            /// </summary>
            public DBCursorField<int> SourceID
            {
                get
                {
                    return m_SourceID;
                }
            }

            /// <summary>
            /// SourceParams
            /// </summary>
            public DBCursorStringField SourceParams
            {
                get
                {
                    return m_SourceParams;
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

            /// <summary>
            /// IDCCOrigine
            /// </summary>
            public DBCursorField<int> IDCCOrigine
            {
                get
                {
                    return m_IDCCOrigine;
                }
            }

            /// <summary>
            /// NomeCCOrigine
            /// </summary>
            public DBCursorStringField NomeCCOrigine
            {
                get
                {
                    return m_NomeCCOrigine;
                }
            }

            /// <summary>
            /// IDCCDestinazione
            /// </summary>
            public DBCursorField<int> IDCCDestinazione
            {
                get
                {
                    return m_IDCCDestinazione;
                }
            }

            /// <summary>
            /// NomeCCDestinazione
            /// </summary>
            public DBCursorStringField NomeCCDestinazione
            {
                get
                {
                    return m_NomeCCDestinazione;
                }
            }

            /// <summary>
            /// TipoMetodoDiPagamento
            /// </summary>
            public DBCursorStringField TipoMetodoDiPagamento
            {
                get
                {
                    return m_TipoMetodoDiPagamento;
                }
            }

            /// <summary>
            /// IDMetodoDiPagamento
            /// </summary>
            public DBCursorField<int> IDMetodoDiPagamento
            {
                get
                {
                    return m_IDMetodoDiPagamento;
                }
            }

            /// <summary>
            /// NomeMetodoDiPagamento
            /// </summary>
            public DBCursorStringField NomeMetodoDiPagamento
            {
                get
                {
                    return m_NomeMetodoDiPagamento;
                }
            }

        

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Contabilita.VociDiPagamento;
            }
        }
    }
}