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
        /// Repository di <see cref="RichiestaPermessoFerie"/>
        /// </summary>
        [Serializable]
        public class RichiestaPermessoFerieCursor 
            : minidom.Databases.DBObjectCursorPO<RichiestaPermessoFerie>
        {
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorStringField m_MotivoRichiesta = new DBCursorStringField("MotivoRichiesta");
            private DBCursorStringField m_NoteRichiesta = new DBCursorStringField("NoteRichiesta");
            private DBCursorField<TipoRichiestaPermessoFerie> m_TipoRichiesta = new DBCursorField<TipoRichiestaPermessoFerie>("TipoRichiesta");
            private DBCursorField<int> m_IDRichiedente = new DBCursorField<int>("IDRichiedente");
            private DBCursorStringField m_NomeRichiedente = new DBCursorStringField("NomeRichiedente");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<int> m_IDInCaricoA = new DBCursorField<int>("IDInCaricoA");
            private DBCursorStringField m_NomeInCaricoA = new DBCursorStringField("NomeInCaricoA");
            private DBCursorField<EsitoRichiestaPermessoFerie> m_EsitoRichiesta = new DBCursorField<EsitoRichiestaPermessoFerie>("EsitoRichiesta");
            private DBCursorStringField m_DettaglioEsitoRichiesta = new DBCursorStringField("DettaglioEsitoRichiesta");
            private DBCursorStringField m_NotaPrvSupervisore = new DBCursorStringField("NotaPrvSupervisore");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public RichiestaPermessoFerieCursor()
            {
            }

            /// <summary>
            /// DataRichiesta
            /// </summary>
            public DBCursorField<DateTime> DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
                }
            }

            /// <summary>
            /// DataInizio
            /// </summary>
            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            /// <summary>
            /// NotaPrvSupervisore
            /// </summary>
            public DBCursorStringField NotaPrvSupervisore
            {
                get
                {
                    return m_NotaPrvSupervisore;
                }
            }

            /// <summary>
            /// DataFine
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// MotivoRichiesta
            /// </summary>
            public DBCursorStringField MotivoRichiesta
            {
                get
                {
                    return m_MotivoRichiesta;
                }
            }

            /// <summary>
            /// NoteRichiesta
            /// </summary>
            public DBCursorStringField NoteRichiesta
            {
                get
                {
                    return m_NoteRichiesta;
                }
            }

            /// <summary>
            /// TipoRichiesta
            /// </summary>
            public DBCursorField<TipoRichiestaPermessoFerie> TipoRichiesta
            {
                get
                {
                    return m_TipoRichiesta;
                }
            }

            /// <summary>
            /// IDRichiedente
            /// </summary>
            public DBCursorField<int> IDRichiedente
            {
                get
                {
                    return m_IDRichiedente;
                }
            }

            /// <summary>
            /// NomeRichiedente
            /// </summary>
            public DBCursorStringField NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }
            }

            /// <summary>
            /// DataPresaInCarico
            /// </summary>
            public DBCursorField<DateTime> DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }
            }

            /// <summary>
            /// IDInCaricoA
            /// </summary>
            public DBCursorField<int> IDInCaricoA
            {
                get
                {
                    return m_IDInCaricoA;
                }
            }

            /// <summary>
            /// NomeInCaricoA
            /// </summary>
            public DBCursorStringField NomeInCaricoA
            {
                get
                {
                    return m_NomeInCaricoA;
                }
            }

            /// <summary>
            /// EsitoRichiesta
            /// </summary>
            public DBCursorField<EsitoRichiestaPermessoFerie> EsitoRichiesta
            {
                get
                {
                    return m_EsitoRichiesta;
                }
            }

            /// <summary>
            /// DettaglioEsitoRichiesta
            /// </summary>
            public DBCursorStringField DettaglioEsitoRichiesta
            {
                get
                {
                    return m_DettaglioEsitoRichiesta;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.RichiestePermessiFerie;
            }
        }
    }
}