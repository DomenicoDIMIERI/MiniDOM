using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore di oggetti di tipo LuogoIstat
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public abstract class LuogoCursorISTAT<T> 
            : LuogoCursor<T> where T : LuogoISTAT
        {
            private DBCursorStringField m_CodiceISTAT = new DBCursorStringField("Codice_ISTAT");
            private DBCursorStringField m_CodiceCatasto = new DBCursorStringField("Codice_Catasto");

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuogoCursorISTAT()
            {
            }

            /// <summary>
            /// CodiceISTAT
            /// </summary>
            public DBCursorStringField CodiceISTAT
            {
                get
                {
                    return m_CodiceISTAT;
                }
            }

            /// <summary>
            /// CodiceCatasto
            /// </summary>
            public DBCursorStringField CodiceCatasto
            {
                get
                {
                    return m_CodiceCatasto;
                }
            }
        }
    }
}