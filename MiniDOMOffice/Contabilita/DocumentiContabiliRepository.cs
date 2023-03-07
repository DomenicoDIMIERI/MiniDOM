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
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="DocumentoContabile"/>
        /// </summary>
        [Serializable]
        public class DocumentiContabiliClass 
            : CModulesClass<DocumentoContabile>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentiContabiliClass() 
                : base("modOfficeDocumentiContabili", typeof(DocumentoContabileCursor), 0)
            {
            }
        }
    }

    public partial class Contabilita
    {
        private static DocumentiContabiliClass m_DocumentiContabili = null;

        /// <summary>
        /// Repository di oggetti <see cref="DocumentoContabile"/>
        /// </summary>
        public static DocumentiContabiliClass DocumentiContabili
        {
            get
            {
                if (m_DocumentiContabili is null)
                    m_DocumentiContabili = new DocumentiContabiliClass();
                return m_DocumentiContabili;
            }
        }
    }
}