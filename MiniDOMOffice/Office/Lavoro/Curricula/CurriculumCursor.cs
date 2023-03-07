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
        /// Cursore di <see cref="Curriculum"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CurriculumCursor 
            : minidom.Databases.DBObjectCursorPO<Curriculum>
        {
            private DBCursorField<DateTime> m_DataPresentazione = new DBCursorField<DateTime>("DataPresentazione");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_IDAllegato = new DBCursorField<int>("IDAllegato");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CurriculumCursor()
            {
            }

            /// <summary>
            /// DataPresentazione
            /// </summary>
            public DBCursorField<DateTime> DataPresentazione
            {
                get
                {
                    return m_DataPresentazione;
                }
            }

            /// <summary>
            /// IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// NomePersona
            /// </summary>
            public DBCursorStringField NomePersona
            {
                get
                {
                    return m_NomePersona;
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
            /// IDAllegato
            /// </summary>
            public DBCursorField<int> IDAllegato
            {
                get
                {
                    return m_IDAllegato;
                }
            }
  
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Curricula;
            }
        }
    }
}