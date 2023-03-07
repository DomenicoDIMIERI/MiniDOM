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
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
        /// Rappresenta un oggetto indicizzato tbl_Index
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CachedIndexCursor
            : Databases.DBObjectCursorBase<CachedIndex>
        {
            [NonSerialized] private CIndexingService m_Owner;
            private DBCursorField<int> m_ObjectID = new DBCursorField<int>("ObjectID");
            private DBCursorStringField  m_Word = new DBCursorStringField("Word");
            private DBCursorField<int> m_Frequenza = new DBCursorField<int>("Rank");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CachedIndexCursor()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CachedIndexCursor(CIndexingService owner)
            {
                //IncreaseCounter(this);
                if (owner is null)
                    throw new ArgumentNullException("owner");
                this.m_Owner = owner;
            }

            /// <summary>
            /// ObjectID
            /// </summary>
            public DBCursorField<int> ObjectID
            {
                get
                {
                    return this.m_ObjectID;
                }
            }

            /// <summary>
            /// Word
            /// </summary>
            public DBCursorStringField Word
            {
                get
                {
                    return this.m_Word;
                }
            }

            /// <summary>
            /// Frequenza
            /// </summary>
            public DBCursorField<int> Frequenza
            {
                get
                {
                    return this.m_Frequenza;
                }
            }


            /// <summary>
            /// Restituisce la connessione al db del servizio
            /// </summary>
            /// <returns></returns>
            protected override DBConnection GetConnection()
            {
                return this.m_Owner.Database;
            }

            /// <summary>
            /// Nessun repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return null;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return this.m_Owner.IndexTableName;
            }

            
        }


    }
}