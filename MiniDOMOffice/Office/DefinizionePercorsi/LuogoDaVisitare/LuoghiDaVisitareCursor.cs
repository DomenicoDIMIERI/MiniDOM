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
        /// Cursor di <see cref="LuogoDaVisitare"/>
        /// </summary>
        [Serializable]
        public class LuoghiDaVisitareCursor 
            : minidom.Databases.DBObjectCursor<LuogoDaVisitare>
        {
            private DBCursorStringField m_Etichetta = new DBCursorStringField("Etichetta");
            private DBCursorField<int> m_IDPercorso = new DBCursorField<int>("IDPercorso");
            private DBCursorStringField m_Indirizzo_CAP = new DBCursorStringField("Indirizzo_CAP");
            private DBCursorStringField m_Indirizzo_Via = new DBCursorStringField("Indirizzo_Via");
            private DBCursorStringField m_Indirizzo_Civico = new DBCursorStringField("Indirizzo_Civico");
            private DBCursorStringField m_Indirizzo_Citta = new DBCursorStringField("Indirizzo_Citta");
            private DBCursorStringField m_Indirizzo_Provincia = new DBCursorStringField("Indirizzo_Provincia");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<double> m_Latitudine = new DBCursorField<double>("Lat");
            private DBCursorField<double> m_Longitudine = new DBCursorField<double>("Lng");
            private DBCursorField<double> m_Altitudine = new DBCursorField<double>("Alt");

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuoghiDaVisitareCursor()
            {
            }

            /// <summary>
            /// Altitudine
            /// </summary>
            public DBCursorField<double> Altitudine
            {
                get
                {
                    return m_Altitudine;
                }
            }

            /// <summary>
            /// Longitudine
            /// </summary>
            public DBCursorField<double> Longitudine
            {
                get
                {
                    return m_Longitudine;
                }
            }

            /// <summary>
            /// Latidudine
            /// </summary>
            public DBCursorField<double> Latidudine
            {
                get
                {
                    return m_Latitudine;
                }
            }

            /// <summary>
            /// Etichetta
            /// </summary>
            public DBCursorStringField Etichetta
            {
                get
                {
                    return m_Etichetta;
                }
            }

            /// <summary>
            /// IDPercorso
            /// </summary>
            public DBCursorField<int> IDPercorso
            {
                get
                {
                    return m_IDPercorso;
                }
            }

            /// <summary>
            /// Indirizzo_CAP
            /// </summary>
            public DBCursorStringField Indirizzo_CAP
            {
                get
                {
                    return m_Indirizzo_CAP;
                }
            }

            /// <summary>
            /// Indirizzo_Via
            /// </summary>
            public DBCursorStringField Indirizzo_Via
            {
                get
                {
                    return m_Indirizzo_Via;
                }
            }

            /// <summary>
            /// Indirizzo_Civico
            /// </summary>
            public DBCursorStringField Indirizzo_Civico
            {
                get
                {
                    return m_Indirizzo_Civico;
                }
            }

            /// <summary>
            /// Indirizzo_Citta
            /// </summary>
            public DBCursorStringField Indirizzo_Citta
            {
                get
                {
                    return m_Indirizzo_Citta;
                }
            }

            /// <summary>
            /// Indirizzo_Provincia
            /// </summary>
            public DBCursorStringField Indirizzo_Provincia
            {
                get
                {
                    return m_Indirizzo_Provincia;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.PercorsiDefiniti.LuoghiDaVisitare;
            }
        }
    }
}