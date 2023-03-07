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
        /// Cursore di <see cref="MarcaturaIngressoUscita"/>
        /// </summary>
        [Serializable] 
        public class MarcatureIngressoUscitaCursor 
            : minidom.Databases.DBObjectCursorPO<MarcaturaIngressoUscita>
        {
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<int> m_IDDispositivo = new DBCursorField<int>("IDDispositivo");
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<TipoMarcaturaIO> m_Operazione = new DBCursorField<TipoMarcaturaIO>("Operazione");
            private DBCursorField<int> m_IDReparto = new DBCursorField<int>("IDReparto");
            private DBCursorStringField m_NomeReparto = new DBCursorStringField("NomeReparto");
            private DBCursorField<MetodoRiconoscimento> m_MetodiRiconoscimentoUsati = new DBCursorField<MetodoRiconoscimento>("MetodiRiconoscimento");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MarcatureIngressoUscitaCursor()
            {
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
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            /// <summary>
            /// IDDispositivo
            /// </summary>
            public DBCursorField<int> IDDispositivo
            {
                get
                {
                    return m_IDDispositivo;
                }
            }

            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            /// <summary>
            /// Operazione
            /// </summary>
            public DBCursorField<TipoMarcaturaIO> Operazione
            {
                get
                {
                    return m_Operazione;
                }
            }

            /// <summary>
            /// IDReparto
            /// </summary>
            public DBCursorField<int> IDReparto
            {
                get
                {
                    return m_IDReparto;
                }
            }

            /// <summary>
            /// NomeReparto
            /// </summary>
            public DBCursorStringField NomeReparto
            {
                get
                {
                    return m_NomeReparto;
                }
            }

            /// <summary>
            /// MetodiRiconoscimentoUsati
            /// </summary>
            public DBCursorField<MetodoRiconoscimento> MetodiRiconoscimentoUsati
            {
                get
                {
                    return m_MetodiRiconoscimentoUsati;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Marcature;
            }
        }
    }
}