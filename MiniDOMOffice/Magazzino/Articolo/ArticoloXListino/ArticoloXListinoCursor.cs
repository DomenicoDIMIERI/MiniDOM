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
        /// Cursore di oggetti di tipo <see cref="ArticoloXListino"/>
        /// </summary>
        [Serializable]
        public class ArticoloXListinoCursor 
            : minidom.Databases.DBObjectCursor<ArticoloXListino>
        {
            private DBCursorField<TipoPrezzoListino> m_TipoPrezzo = new DBCursorField<TipoPrezzoListino>("TipoPrezzo");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDArticolo = new DBCursorField<int>("IDArticolo");
            private DBCursorField<int> m_IDListino = new DBCursorField<int>("IDListino");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloXListinoCursor()
            {
            }

            /// <summary>
            /// TipoPrezzo
            /// </summary>
            public DBCursorField<TipoPrezzoListino> TipoPrezzo
            {
                get
                {
                    return m_TipoPrezzo;
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
            /// IDArticolo
            /// </summary>
            public DBCursorField<int> IDArticolo
            {
                get
                {
                    return m_IDArticolo;
                }
            }

            /// <summary>
            /// IDListino
            /// </summary>
            public DBCursorField<int> IDListino
            {
                get
                {
                    return m_IDListino;
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.Articoli.Listini;
            }

         
        }
    }
}