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
        /// Cursore di oggetti <see cref="PercorsoDefinito"/>
        /// </summary>
        [Serializable]
        public class PercorsiDefinitiCursor 
            : Databases.DBObjectCursorPO<PercorsoDefinito>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_NomeGruppo = new DBCursorStringField("NomeGruppo");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");
            private DBCursorField<float> m_DistanzaStimataMin = new DBCursorField<float>("DistanzaStimataMin");
            private DBCursorField<float> m_DistanzaStimataMax = new DBCursorField<float>("DistanzaStimataMax");
            private DBCursorField<int> m_TempoStimatoMin = new DBCursorField<int>("TempoStimatoMin");
            private DBCursorField<int> m_TempoStimatoMax = new DBCursorField<int>("TempoStimatoMax");

            /// <summary>
            /// Costruttore
            /// </summary>
            public PercorsiDefinitiCursor()
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
            /// NomeGruppo
            /// </summary>
            public DBCursorStringField NomeGruppo
            {
                get
                {
                    return m_NomeGruppo;
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
            /// Attivo
            /// </summary>
            public DBCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
                }
            }

            /// <summary>
            /// DistanzaStimataMin
            /// </summary>
            public DBCursorField<float> DistanzaStimataMin
            {
                get
                {
                    return m_DistanzaStimataMin;
                }
            }

            /// <summary>
            /// DistanzaStimataMax
            /// </summary>
            public DBCursorField<float> DistanzaStimataMax
            {
                get
                {
                    return m_DistanzaStimataMax;
                }
            }

            /// <summary>
            /// TempoStimatoMin
            /// </summary>
            public DBCursorField<int> TempoStimatoMin
            {
                get
                {
                    return m_TempoStimatoMin;
                }
            }

            /// <summary>
            /// TempoStimatoMax
            /// </summary>
            public DBCursorField<int> TempoStimatoMax
            {
                get
                {
                    return m_TempoStimatoMax;
                }
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.PercorsiDefiniti;
            }
        }
    }
}