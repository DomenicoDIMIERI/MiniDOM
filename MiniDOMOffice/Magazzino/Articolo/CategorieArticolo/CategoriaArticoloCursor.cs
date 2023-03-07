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
using static minidom.Store;


namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Cursore di oggetti <see cref="CategoriaArticolo"/>
        /// </summary>
        [Serializable]
        public class CategoriaArticoloCursor 
            : Databases.DBObjectCursor<CategoriaArticolo>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorField<int> m_IDParent = new DBCursorField<int>("IDParent");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CategoriaArticoloCursor()
            {
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
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
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
            /// IDParent
            /// </summary>
            public DBCursorField<int> IDParent
            {
                get
                {
                    return m_IDParent;
                }
            }
         
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.CategorieArticoli;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_OfficeCategorieArticoli";
            //}
        }
    }
}