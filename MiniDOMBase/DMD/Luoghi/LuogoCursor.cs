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
        /// Costruttore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public abstract class LuogoCursor<T> 
            : Databases.DBObjectCursorPO<T> where T : Luogo
        {
            
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_NumeroAbitanti = new DBCursorField<int>("NumeroAbitanti");
            private DBCursorStringField m_NomeAbitanti = new DBCursorStringField("NomeAbitanti");

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuogoCursor()
            {
            }

            /// <summary>
            /// NumeroAbitanti
            /// </summary>
            public DBCursorField<int> NumeroAbitanti
            {
                get
                {
                    return m_NumeroAbitanti;
                }
            }

            /// <summary>
            /// NomeAbitanti
            /// </summary>
            public DBCursorStringField NomeAbitanti
            {
                get
                {
                    return m_NomeAbitanti;
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
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            
        }
    }
}