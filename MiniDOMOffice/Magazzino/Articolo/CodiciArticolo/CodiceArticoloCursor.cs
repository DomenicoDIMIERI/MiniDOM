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
        /// Cursore di oggetti di tipo <see cref="CodiceArticolo"/>
        /// </summary>
        [Serializable]
        public class CodiceArticoloCursor
            : minidom.Databases.DBObjectCursor<CodiceArticolo>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Valore = new DBCursorStringField("Valore");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDArticolo = new DBCursorField<int>("IDArticolo");
            private DBCursorField<int> m_Ordine = new DBCursorField<int>("Ordine");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CodiceArticoloCursor()
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
            /// Tipo
            /// </summary>
            public DBCursorStringField Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            /// <summary>
            /// Valore
            /// </summary>
            public DBCursorStringField Valore
            {
                get
                {
                    return m_Valore;
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
            /// Ordine
            /// </summary>
            public DBCursorField<int> Ordine
            {
                get
                {
                    return m_Ordine;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.Articoli.Codici;
            }
             
        }
    }
}