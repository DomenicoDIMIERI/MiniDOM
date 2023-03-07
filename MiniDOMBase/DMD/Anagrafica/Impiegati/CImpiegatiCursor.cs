using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella degli impiegati
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CImpiegatiCursor 
            : minidom.Databases.DBObjectCursor<CImpiegato>
        {

            private DBCursorField<int> m_PersonaID;
            private DBCursorStringField m_NomePersona;
            private DBCursorField<int> m_AziendaID;
            private DBCursorStringField m_NomeAzienda;
            private DBCursorField<int> m_IDSede;
            private DBCursorStringField m_NomeSede;
            private DBCursorStringField m_Posizione;
            private DBCursorStringField m_Ufficio;
            private DBCursorField<DateTime> m_DataAssunzione;
            private DBCursorField<DateTime> m_DataLicenziamento;
            private DBCursorField<decimal> m_StipendioNetto;
            private DBCursorField<decimal> m_StipendioLordo;
            private DBCursorField<decimal> m_TFR;
            private DBCursorField<int> m_MensilitaPercepite;
            private DBCursorField<double> m_PercTFRAzienda;
            private DBCursorStringField m_NomeFPC;
            private DBCursorStringField m_TipoContratto;
            private DBCursorStringField m_TipoRapporto;
            private DBCursorField<ImpiegoFlags> m_Flags;
            private DBCursorField<int> m_IDEntePagante;
            private DBCursorStringField m_NomeEntePagante;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CImpiegatiCursor()
            {
                m_PersonaID = new DBCursorField<int>("Persona");
                m_NomePersona = new DBCursorStringField("NomePersona");
                m_AziendaID = new DBCursorField<int>("Azienda");
                m_NomeAzienda = new DBCursorStringField("NomeAzienda");
                m_IDSede = new DBCursorField<int>("IDSede");
                m_NomeSede = new DBCursorStringField("NomeSede");
                m_Posizione = new DBCursorStringField("Posizione");
                m_Ufficio = new DBCursorStringField("Ufficio");
                m_DataAssunzione = new DBCursorField<DateTime>("DataAssunzione");
                m_DataLicenziamento = new DBCursorField<DateTime>("DataLicenziamento");
                m_StipendioNetto = new DBCursorField<decimal>("StipendioNetto");
                m_StipendioLordo = new DBCursorField<decimal>("StipendioLordo");
                m_TFR = new DBCursorField<decimal>("TFR");
                m_MensilitaPercepite = new DBCursorField<int>("MensilitaPercepite");
                m_PercTFRAzienda = new DBCursorField<double>("PercTFRAzienda");
                m_NomeFPC = new DBCursorStringField("NomeFPC");
                m_TipoContratto = new DBCursorStringField("TipoContratto");
                m_TipoRapporto = new DBCursorStringField("TipoRapporto");
                m_Flags = new DBCursorField<ImpiegoFlags>("Flags");
                this.m_IDEntePagante = new DBCursorField<int>("IDEntePagante");
                this.m_NomeEntePagante = new DBCursorStringField("NomeEntePagante");
            }

            /// <summary>
            /// IDEntePagante
            /// </summary>
            public DBCursorField<int> IDEntePagante
            {
                get
                {
                    return this.m_IDEntePagante;
                }
            }

            /// <summary>
            /// NomeEntePagante
            /// </summary>
            public DBCursorStringField NomeEntePagante
            {
                get
                {
                    return this.m_NomeEntePagante;
                }
            }

            /// <summary>
            /// Campo PersonaID
            /// </summary>
            public DBCursorField<int> PersonaID
            {
                get
                {
                    return m_PersonaID;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<ImpiegoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Campo NomePersona
            /// </summary>
            public DBCursorStringField NomePersona
            {
                get
                {
                    return m_NomePersona;
                }
            }

            /// <summary>
            /// Campo AziendaID
            /// </summary>
            public DBCursorField<int> AziendaID
            {
                get
                {
                    return m_AziendaID;
                }
            }

            /// <summary>
            /// Campo NomeAzienda
            /// </summary>
            public DBCursorStringField NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }
            }

            /// <summary>
            /// Campo IDSede
            /// </summary>
            public DBCursorField<int> IDSede
            {
                get
                {
                    return m_IDSede;
                }
            }

            /// <summary>
            /// Campo NomeSede
            /// </summary>
            public DBCursorStringField NomeSede
            {
                get
                {
                    return m_NomeSede;
                }
            }

            /// <summary>
            /// Campo Posizione
            /// </summary>
            public DBCursorStringField Posizione
            {
                get
                {
                    return m_Posizione;
                }
            }

            /// <summary>
            /// Campo Ufficio
            /// </summary>
            public DBCursorStringField Ufficio
            {
                get
                {
                    return m_Ufficio;
                }
            }

            /// <summary>
            /// Campo DataAssunzione
            /// </summary>
            public DBCursorField<DateTime> DataAssunzione
            {
                get
                {
                    return m_DataAssunzione;
                }
            }

            /// <summary>
            /// Campo DataLicenziamento
            /// </summary>
            public DBCursorField<DateTime> DataLicenziamento
            {
                get
                {
                    return m_DataLicenziamento;
                }
            }

            /// <summary>
            /// Campo StipendioNetto
            /// </summary>
            public DBCursorField<decimal> StipendioNetto
            {
                get
                {
                    return m_StipendioNetto;
                }
            }

            /// <summary>
            /// Campo StipendioLordo
            /// </summary>
            public DBCursorField<decimal> StipendioLordo
            {
                get
                {
                    return m_StipendioLordo;
                }
            }

            /// <summary>
            /// Campo TFR
            /// </summary>
            public DBCursorField<decimal> TFR
            {
                get
                {
                    return m_TFR;
                }
            }

            /// <summary>
            /// Campo MensilitaPercepite
            /// </summary>
            public DBCursorField<int> MensilitaPercepite
            {
                get
                {
                    return m_MensilitaPercepite;
                }
            }

            /// <summary>
            /// Campo PercTFRAzienda
            /// </summary>
            public DBCursorField<double> PercTFRAzienda
            {
                get
                {
                    return m_PercTFRAzienda;
                }
            }

            /// <summary>
            /// Campo NomeFPC
            /// </summary>
            public DBCursorStringField NomeFPC
            {
                get
                {
                    return m_NomeFPC;
                }
            }

            /// <summary>
            /// Campo TipoContratto
            /// </summary>
            public DBCursorStringField TipoContratto
            {
                get
                {
                    return m_TipoContratto;
                }
            }

            /// <summary>
            /// Campo TipoRapporto
            /// </summary>
            public DBCursorStringField TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }
            }

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CImpiegato();
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_Impiegati";
            //}

            //protected override Sistema.CModule GetModule()
            //{
            //    return null;
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}