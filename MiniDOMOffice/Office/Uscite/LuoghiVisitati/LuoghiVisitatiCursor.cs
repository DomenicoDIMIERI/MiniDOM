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
        /// Cursore di <see cref="LuogoVisitato"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class LuoghiVisitatiCursor 
            : minidom.Databases.DBObjectCursor<LuogoVisitato>
        {
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorField<int> m_IDUscita = new DBCursorField<int>("IDUscita");
            private DBCursorStringField m_IndirizzoEtichetta = new DBCursorStringField("Indirizzo_Etichetta");
            private DBCursorStringField m_IndirizzoVia = new DBCursorStringField("Indirizzo_Via");
            private DBCursorStringField m_IndirizzoCitta = new DBCursorStringField("Indirizzo_Citta");
            private DBCursorStringField m_IndirizzoProvincia = new DBCursorStringField("Indirizzo_Provincia");
            private DBCursorStringField m_IndirizzoCAP = new DBCursorStringField("Indirizzo_CAP");
            private DBCursorField<DateTime> m_OraArrivo = new DBCursorField<DateTime>("OraArrivo");
            private DBCursorField<DateTime> m_OraPartenza = new DBCursorField<DateTime>("OraPartenza");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<double> m_Lat = new DBCursorField<double>("Lat");
            private DBCursorField<double> m_Lng = new DBCursorField<double>("Lng");
            private DBCursorField<double> m_Alt = new DBCursorField<double>("Alt");
            private DBCursorField<int> m_IDLuogo = new DBCursorField<int>("IDLuogo");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_TipoMateriale = new DBCursorStringField("TipoMateriale");
            private DBCursorField<int> m_ConsegnatiAMano = new DBCursorField<int>("ConsegnatiAMano");
            private DBCursorField<int> m_ConsegnatiPostale = new DBCursorField<int>("ConsegnatiPostale");
            private DBCursorField<int> m_ConsegnatiAuto = new DBCursorField<int>("ConsegnatiAuto");
            private DBCursorField<int> m_ConsegnatiAltro = new DBCursorField<int>("ConsegnatiAltro");
            private DBCursorField<int> m_Progressivo = new DBCursorField<int>("Progressivo");


            /// <summary>
            /// Costruttore
            /// </summary>
            public LuoghiVisitatiCursor()
            {

            }

            /// <summary>
            /// Progressivo
            /// </summary>
            public DBCursorField<int> Progressivo
            {
                get
                {
                    return m_Progressivo;
                }
            }

            /// <summary>
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// IDUscita
            /// </summary>
            public DBCursorField<int> IDUscita
            {
                get
                {
                    return m_IDUscita;
                }
            }

            /// <summary>
            /// Indirizzo_Etichetta
            /// </summary>
            public DBCursorStringField Indirizzo_Etichetta
            {
                get
                {
                    return m_IndirizzoEtichetta;
                }
            }

            /// <summary>
            /// Indirizzo_Via
            /// </summary>
            public DBCursorStringField Indirizzo_Via
            {
                get
                {
                    return m_IndirizzoVia;
                }
            }

            /// <summary>
            /// Indirizzo_Citta
            /// </summary>
            public DBCursorStringField Indirizzo_Citta
            {
                get
                {
                    return m_IndirizzoCitta;
                }
            }

            /// <summary>
            /// Indirizzo_Provincia
            /// </summary>
            public DBCursorStringField Indirizzo_Provincia
            {
                get
                {
                    return m_IndirizzoProvincia;
                }
            }

            /// <summary>
            /// Indirizzo_CAP
            /// </summary>
            public DBCursorStringField Indirizzo_CAP
            {
                get
                {
                    return m_IndirizzoCAP;
                }
            }

            /// <summary>
            /// OraArrivo
            /// </summary>
            public DBCursorField<DateTime> OraArrivo
            {
                get
                {
                    return m_OraArrivo;
                }
            }

            /// <summary>
            /// OraPartenza
            /// </summary>
            public DBCursorField<DateTime> OraPartenza
            {
                get
                {
                    return m_OraPartenza;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.LuoghiVisitati;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeLuoghiV";
            }
             
            /// <summary>
            /// Latitudine
            /// </summary>
            public DBCursorField<double> Lat
            {
                get
                {
                    return m_Lat;
                }
            }

            /// <summary>
            /// Longitudine
            /// </summary>
            public DBCursorField<double> Lng
            {
                get
                {
                    return m_Lng;
                }
            }

            /// <summary>
            /// Altitudine
            /// </summary>
            public DBCursorField<double> Alt
            {
                get
                {
                    return m_Alt;
                }
            }

            /// <summary>
            /// ID del luogo
            /// </summary>
            public DBCursorField<int> IDLuogo
            {
                get
                {
                    return m_IDLuogo;
                }
            }

            /// <summary>
            /// ID della persona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// TipoMateriale
            /// </summary>
            public DBCursorStringField TipoMateriale
            {
                get
                {
                    return m_TipoMateriale;
                }
            }

            /// <summary>
            /// ConsegnatiAMano
            /// </summary>
            public DBCursorField<int> ConsegnatiAMano
            {
                get
                {
                    return m_ConsegnatiAMano;
                }
            }

            /// <summary>
            /// ConsegnatiPostale
            /// </summary>
            public DBCursorField<int> ConsegnatiPostale
            {
                get
                {
                    return m_ConsegnatiPostale;
                }
            }

            /// <summary>
            /// ConsegnatiAuto
            /// </summary>
            public DBCursorField<int> ConsegnatiAuto
            {
                get
                {
                    return m_ConsegnatiAuto;
                }
            }

            /// <summary>
            /// ConsegnatiAltro
            /// </summary>
            public DBCursorField<int> ConsegnatiAltro
            {
                get
                {
                    return m_ConsegnatiAltro;
                }
            }
        }
    }
}