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
        /// Cursore di <see cref="AttributoOggetto"/>
        /// </summary>
        [Serializable]
        public class AttributoOggettoCursor 
            : minidom.Databases.DBObjectCursor<AttributoOggetto>
        {
            private DBCursorField<int> m_IDOggetto = new DBCursorField<int>("IDOggetto");
            private DBCursorStringField m_NomeAttributo = new DBCursorStringField("NomeAttributo");
            private DBCursorStringField m_ValoreAttributoFormattato = new DBCursorStringField("ValoreAttributo");
            private DBCursorField<TypeCode> m_TipoAttributo = new DBCursorField<TypeCode>("TipoAttributo");
            private DBCursorStringField m_UnitaDiMisura = new DBCursorStringField("UnitaDiMisura");

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributoOggettoCursor()
            {
            }

            /// <summary>
            /// IDOggetto
            /// </summary>
            public DBCursorField<int> IDOggetto
            {
                get
                {
                    return m_IDOggetto;
                }
            }

            /// <summary>
            /// NomeAttributo
            /// </summary>
            public DBCursorStringField NomeAttributo
            {
                get
                {
                    return m_NomeAttributo;
                }
            }

            /// <summary>
            /// ValoreAttributoFormattato
            /// </summary>
            public DBCursorStringField ValoreAttributoFormattato
            {
                get
                {
                    return m_ValoreAttributoFormattato;
                }
            }

            /// <summary>
            /// TipoAttributo
            /// </summary>
            public DBCursorField<TypeCode> TipoAttributo
            {
                get
                {
                    return m_TipoAttributo;
                }
            }

            /// <summary>
            /// UnitaDiMisura
            /// </summary>
            public DBCursorStringField UnitaDiMisura
            {
                get
                {
                    return m_UnitaDiMisura;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.OggettiInventariati.Attributi;
            }

           
        }
    }
}