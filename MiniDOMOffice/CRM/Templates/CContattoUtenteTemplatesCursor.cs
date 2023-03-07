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
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="CContattoUtenteTemplate"/>
        /// </summary>
        /// <remarks></remarks>
        public class CContattoUtenteTemplateCursor 
            : minidom.Databases.DBObjectCursor<CContattoUtenteTemplate>
        {
            private DBCursorField<TemplateFlags> m_Flags = new DBCursorField<TemplateFlags>("Flags");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Scopo = new DBCursorStringField("Scopo");
            private DBCursorStringField m_TipoContatto = new DBCursorStringField("TipoContatto");
            private DBCursorStringField m_Testo = new DBCursorStringField("Testo");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoUtenteTemplateCursor()
            {
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<TemplateFlags> Flags
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
            /// Scopo
            /// </summary>
            public DBCursorStringField Scopo
            {
                get
                {
                    return m_Scopo;
                }
            }

            /// <summary>
            /// TipoContatto
            /// </summary>
            public DBCursorStringField TipoContatto
            {
                get
                {
                    return m_TipoContatto;
                }
            }

            /// <summary>
            /// Testo
            /// </summary>
            public DBCursorStringField Testo
            {
                get
                {
                    return m_Testo;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.Templates;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CRMTemplates";
            //}
        }
    }
}