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
        /// Cursore di oggetti <see cref="OggettoCollegato"/>
        /// </summary>
        [Serializable]
        public class OggettoCollegatoCursor 
            : Databases.DBObjectCursor<OggettoCollegato>
        {
            private DBCursorField<DateTime> m_DataCollegamento = new DBCursorField<DateTime>("DataCollegamento");
            private DBCursorField<DateTime> m_DataScollegamento = new DBCursorField<DateTime>("DataScollegamento");
            private DBCursorField<int> m_IDOggetto1 = new DBCursorField<int>("IDOggetto1");
            private DBCursorField<int> m_IDOggetto2 = new DBCursorField<int>("IDOggetto2");
            private DBCursorField<RelazioniOggettiCollegati> m_Relazione = new DBCursorField<RelazioniOggettiCollegati>("Relazione");

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettoCollegatoCursor()
            {
            }

            /// <summary>
            /// DataCollegamento
            /// </summary>
            public DBCursorField<DateTime> DataCollegamento
            {
                get
                {
                    return m_DataCollegamento;
                }
            }

            /// <summary>
            /// DataScollegamento
            /// </summary>
            public DBCursorField<DateTime> DataScollegamento
            {
                get
                {
                    return m_DataScollegamento;
                }
            }

            /// <summary>
            /// IDOggetto1
            /// </summary>
            public DBCursorField<int> IDOggetto1
            {
                get
                {
                    return m_IDOggetto1;
                }
            }

            /// <summary>
            /// IDOggetto2
            /// </summary>
            public DBCursorField<int> IDOggetto2
            {
                get
                {
                    return m_IDOggetto2;
                }
            }

            /// <summary>
            /// Relazione
            /// </summary>
            public DBCursorField<RelazioniOggettiCollegati> Relazione
            {
                get
                {
                    return m_Relazione;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.OggettiInventariati.OggettiCollegati;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_OfficeOggettiCollegati";
            //}
        }
    }
}