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
        /// Cursore di oggetti <see cref="DocumentoCaricato"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class DocumentiCaricatiCursor
            : minidom.Databases.DBObjectCursor<DocumentoCaricato>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_TemplatePath = new DBCursorStringField("Template");
            private DBCursorField<bool> m_Uploadable = new DBCursorField<bool>("Uploadable");
            private DBCursorField<bool> m_ValiditaLimitata = new DBCursorField<bool>("ValiditaLimitata");
            private DBCursorField<bool> m_LegatoAlContesto = new DBCursorField<bool>("LegatoAlContesto");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Categoria = new DBCursorStringField("cat");
            private DBCursorStringField m_SottoCategoria = new DBCursorStringField("sotto_cat");

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentiCaricatiCursor()
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
            /// TemplatePath
            /// </summary>
            public DBCursorStringField TemplatePath
            {
                get
                {
                    return m_TemplatePath;
                }
            }

            /// <summary>
            /// Uploadable
            /// </summary>
            public DBCursorField<bool> Uploadable
            {
                get
                {
                    return m_Uploadable;
                }
            }

            /// <summary>
            /// ValiditaLimitata
            /// </summary>
            public DBCursorField<bool> ValiditaLimitata
            {
                get
                {
                    return m_ValiditaLimitata;
                }
            }

            /// <summary>
            /// LegatoAlContesto
            /// </summary>
            public DBCursorField<bool> LegatoAlContesto
            {
                get
                {
                    return m_LegatoAlContesto;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.GDE;
            }
             
        }
    }
}