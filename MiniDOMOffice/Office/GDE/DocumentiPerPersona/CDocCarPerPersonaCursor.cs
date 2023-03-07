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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Cursore di <see cref="CDocCarPerPersona"/>
        /// </summary>
        [Serializable]
        public class CDocCarPerPersonaCursor 
            : minidom.Databases.DBObjectCursor<CDocCarPerPersona>
        {
            private DBCursorField<int> m_IDDocumento = new DBCursorField<int>("IDDocumento");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorField<int> m_IDAttachment = new DBCursorField<int>("IDAttachment");
            private DBCursorField<DateTime> m_DataCaricamento = new DBCursorField<DateTime>("DataCaricamento");
            private DBCursorField<int> m_IDOperatoreCaricamento = new DBCursorField<int>("IDOperatoreCaricamento");
            private DBCursorStringField m_NomeOperatoreCaricamento = new DBCursorStringField("NomeOperatoreCaricamento");
            private DBCursorStringField m_Annotazioni = new DBCursorStringField("Annotazioni");
            private DBCursorField<DateTime> m_DataRilascio = new DBCursorField<DateTime>("DataRilascio");
            private DBCursorField<DateTime> m_DataScadenza = new DBCursorField<DateTime>("DataScadenza");
            private DBCursorField<int> m_IDRilasciatoDa = new DBCursorField<int>("IDRilasciatoDa");
            private DBCursorStringField m_NomeRilasciatoDa = new DBCursorStringField("NomeRilasciatoDa");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDocCarPerPersonaCursor()
            {
            }

            /// <summary>
            /// IDDocumento
            /// </summary>
            public DBCursorField<int> IDDocumento
            {
                get
                {
                    return m_IDDocumento;
                }
            }

            /// <summary>
            /// IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// IDAttachment
            /// </summary>
            public DBCursorField<int> IDAttachment
            {
                get
                {
                    return m_IDAttachment;
                }
            }

            /// <summary>
            /// DataCaricamento
            /// </summary>
            public DBCursorField<DateTime> DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }
            }

            /// <summary>
            /// IDOperatoreCaricamento
            /// </summary>
            public DBCursorField<int> IDOperatoreCaricamento
            {
                get
                {
                    return m_IDOperatoreCaricamento;
                }
            }

            /// <summary>
            /// NomeOperatoreCaricamento
            /// </summary>
            public DBCursorStringField NomeOperatoreCaricamento
            {
                get
                {
                    return m_NomeOperatoreCaricamento;
                }
            }

            /// <summary>
            /// Annotazioni
            /// </summary>
            public DBCursorStringField Annotazioni
            {
                get
                {
                    return m_Annotazioni;
                }
            }

            /// <summary>
            /// DataRilascio
            /// </summary>
            public DBCursorField<DateTime> DataRilascio
            {
                get
                {
                    return m_DataRilascio;
                }
            }

            /// <summary>
            /// DataScadenza
            /// </summary>
            public DBCursorField<DateTime> DataScadenza
            {
                get
                {
                    return m_DataScadenza;
                }
            }

            /// <summary>
            /// IDRilasciatoDa
            /// </summary>
            public DBCursorField<int> IDRilasciatoDa
            {
                get
                {
                    return m_IDRilasciatoDa;
                }
            }

            /// <summary>
            /// NomeRilasciatoDa
            /// </summary>
            public DBCursorStringField NomeRilasciatoDa
            {
                get
                {
                    return m_NomeRilasciatoDa;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.DocumentiCaricati.DocumentiPerPersona;
            }
             
        }

    }
}