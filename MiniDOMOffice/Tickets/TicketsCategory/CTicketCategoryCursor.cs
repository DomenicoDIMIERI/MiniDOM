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
        /// Cursore di <see cref="CTicketCategory"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTicketCategoryCursor 
            : Databases.DBObjectCursor<CTicketCategory>
        {
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_Sottocategoria = new DBCursorStringField("Sottocategoria");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketCategoryCursor()
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
            /// Sottocategoria
            /// </summary>
            public DBCursorStringField Sottocategoria
            {
                get
                {
                    return m_Sottocategoria;
                }
            }
 
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.TicketCategories;
            }
        }
    }
}