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
        /// Cursore sulla tabella delle parole indicizzate
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CachedWordCursor
            : Databases.DBObjectCursorBase<CachedWord>
        {
            [NonSerialized] private CIndexingService m_Owner;
            
            private DBCursorStringField m_Word = new DBCursorStringField("");
            private DBCursorField<DateTime> m_LastUsed = new DBCursorField<DateTime>("");
            private DBCursorField<DateTime> m_FirstUsed = new DBCursorField<DateTime>("");
            private DBCursorField<int> m_Frequenza = new DBCursorField<int>("");
            private DBCursorField<int> m_ConteggioUtilizzi = new DBCursorField<int>("");
          
            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CachedWordCursor(CIndexingService owner)
            {
                //IncreaseCounter(this);
                if (owner is null)
                    throw new ArgumentNullException("owner");
                this.m_Owner = owner;
            }

            /// <summary>
            /// Restituisce un riferimento al servizio che gestisce l'oggetto
            /// </summary>
            public CIndexingService Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta il riferimento al servizio che gestisce l'oggetto
            /// </summary>
            /// <param name="owner"></param>
            protected internal void SetOwner(CIndexingService owner)
            {
                m_Owner = owner;
            }

            /// <summary>
            /// Restituisce la parola indicizzata
            /// </summary>
            public DBCursorStringField Word
            {
                get
                {
                    return m_Word;
                }
            }

            /// <summary>
            /// Data dell'ultimo utilizzo della parola
            /// </summary>
            public DBCursorField<DateTime> LastUsed
            {
                get
                {
                    return this.m_LastUsed;
                }
            }

            /// <summary>
            /// Data del primo utilizzo della parola
            /// </summary>
            public DBCursorField<DateTime> FirstUsed
            {
                get
                {
                    return this.m_FirstUsed;
                }
            }

            /// <summary>
            /// Frequenza di utilizzo
            /// </summary>
            public DBCursorField<int> Frequenza
            {
                get
                {
                    return this.m_Frequenza;
                }
            }

            /// <summary>
            /// Conteggio
            /// </summary>
            public DBCursorField<int> ConteggioUtilizzi
            {
                get
                {
                    return this.m_ConteggioUtilizzi;
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_WordStats";
            }

            /// <summary>
            /// Restituisce la connessione del servizio di indicizzazione
            /// </summary>
            /// <returns></returns>
            protected override DBConnection GetConnection()
            {
                return this.Owner.Database;
            }

        }


    }
}