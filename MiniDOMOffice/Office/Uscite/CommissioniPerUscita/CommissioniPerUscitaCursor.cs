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
        /// Cursore di <see cref="CommissionePerUscita"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CommissioniPerUscitaCursor
            : Databases.DBObjectCursor<CommissionePerUscita>
        {
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<int> m_IDUscita = new DBCursorField<int>("IDUscita");
            private DBCursorField<int> m_IDCommissione = new DBCursorField<int>("IDCommissione");
            private DBCursorStringField m_DescrizioneEsito = new DBCursorStringField("DescrizioneEsito");
            private DBCursorField<DateTime> m_OraInizio = new DBCursorField<DateTime>("OraInizio");
            private DBCursorField<DateTime> m_OraFine = new DBCursorField<DateTime>("OraFine");
            private DBCursorField<double> m_DistanzaPercorsa = new DBCursorField<double>("DistanzaPercorsa");
            // Private m_Luogo As New DBCursorFieldObj(Of String)("Luogo")
            private DBCursorField<StatoCommissione> m_StatoCommissione = new DBCursorField<StatoCommissione>("StatoCommissione");

            // Public ReadOnly Property Luogo As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Luogo
            // End Get
            // End Property

            /// <summary>
            /// Costruttore
            /// </summary>
            public CommissioniPerUscitaCursor()
            {

            }

            /// <summary>
            /// StatoCommissione
            /// </summary>
            public DBCursorField<StatoCommissione> StatoCommissione
            {
                get
                {
                    return m_StatoCommissione;
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
            /// OraInizio
            /// </summary>
            public DBCursorField<DateTime> OraInizio
            {
                get
                {
                    return m_OraInizio;
                }
            }

            /// <summary>
            /// OraFine
            /// </summary>
            public DBCursorField<DateTime> OraFine
            {
                get
                {
                    return m_OraFine;
                }
            }

            /// <summary>
            /// DistanzaPercorsa
            /// </summary>
            public DBCursorField<double> DistanzaPercorsa
            {
                get
                {
                    return m_DistanzaPercorsa;
                }
            }

            /// <summary>
            /// IDCommissione
            /// </summary>
            public DBCursorField<int> IDCommissione
            {
                get
                {
                    return m_IDCommissione;
                }
            }

            /// <summary>
            /// DescrizioneEsito
            /// </summary>
            public DBCursorStringField DescrizioneEsito
            {
                get
                {
                    return m_DescrizioneEsito;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Uscite.CommissioniPerUscite;
            }

            /// <summary>
            /// Inizializza i parametri
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(CommissionePerUscita item)
            {
                base.OnInitialize(item);
                item.Operatore = Sistema.Users.CurrentUser;
            }
 
        }
    }
}