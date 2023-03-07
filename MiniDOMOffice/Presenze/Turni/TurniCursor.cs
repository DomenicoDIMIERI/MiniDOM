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
        /// Cursore di <see cref="Turno"/>
        /// </summary>
        [Serializable]
        public class TurniCursor 
            : Databases.DBObjectCursor<Turno>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<DateTime> m_OraIngresso = new DBCursorField<DateTime>("OraIngresso");
            private DBCursorField<DateTime> m_OraUscita = new DBCursorField<DateTime>("OraUscita");
            private DBCursorField<int> m_TolleranzaIngressoAnticipato = new DBCursorField<int>("TolleranzaIngressoAnticipato");
            private DBCursorField<int> m_TolleranzaIngressoRitardato = new DBCursorField<int>("TolleranzaIngressoRitardato");
            private DBCursorField<int> m_TolleranzaUscitaAnticipata = new DBCursorField<int>("TolleranzaUscitaAnticipata");
            private DBCursorField<int> m_TolleranzaUscitaRitardata = new DBCursorField<int>("TolleranzaUscitaRitardata");
            private DBCursorField<DateTime> m_ValidoDal = new DBCursorField<DateTime>("ValidoDal");
            private DBCursorField<DateTime> m_ValidoAl = new DBCursorField<DateTime>("ValidoAl");
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");
            private DBCursorField<TurnoFlagGiorni> m_GiorniDellaSettimana = new DBCursorField<TurnoFlagGiorni>("GiorniDellaSettimana");
            private DBCursorField<int> m_Periodicita = new DBCursorField<int>("Periodicita");

            /// <summary>
            /// Costruttore
            /// </summary>
            public TurniCursor()
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
            /// OraIngresso
            /// </summary>
            public DBCursorField<DateTime> OraIngresso
            {
                get
                {
                    return m_OraIngresso;
                }
            }

            /// <summary>
            /// OraUscita
            /// </summary>
            public DBCursorField<DateTime> OraUscita
            {
                get
                {
                    return m_OraUscita;
                }
            }

            /// <summary>
            /// TolleranzaIngressoAnticipato
            /// </summary>
            public DBCursorField<int> TolleranzaIngressoAnticipato
            {
                get
                {
                    return m_TolleranzaIngressoAnticipato;
                }
            }

            /// <summary>
            /// TolleranzaIngressoRitardato
            /// </summary>
            public DBCursorField<int> TolleranzaIngressoRitardato
            {
                get
                {
                    return m_TolleranzaIngressoRitardato;
                }
            }

            /// <summary>
            /// TolleranzaUscitaAnticipata
            /// </summary>
            public DBCursorField<int> TolleranzaUscitaAnticipata
            {
                get
                {
                    return m_TolleranzaUscitaAnticipata;
                }
            }

            /// <summary>
            /// TolleranzaUscitaRitardata
            /// </summary>
            public DBCursorField<int> TolleranzaUscitaRitardata
            {
                get
                {
                    return m_TolleranzaUscitaRitardata;
                }
            }

            /// <summary>
            /// ValidoDal
            /// </summary>
            public DBCursorField<DateTime> ValidoDal
            {
                get
                {
                    return m_ValidoDal;
                }
            }

            /// <summary>
            /// ValidoAl
            /// </summary>
            public DBCursorField<DateTime> ValidoAl
            {
                get
                {
                    return m_ValidoAl;
                }
            }

            /// <summary>
            /// Periodicita
            /// </summary>
            public DBCursorField<int> Periodicita
            {
                get
                {
                    return m_Periodicita;
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
            /// GiorniDellaSettimana
            /// </summary>
            public DBCursorField<TurnoFlagGiorni> GiorniDellaSettimana
            {
                get
                {
                    return m_GiorniDellaSettimana;
                }
            }
              
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Turni;
            }
        }
    }
}