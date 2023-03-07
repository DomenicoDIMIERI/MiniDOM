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
        /// Cursore di <see cref="Candidatura"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CandidaturaCursor 
            : minidom.Databases.DBObjectCursorPO<Candidatura>
        {
            private DBCursorField<DateTime> m_DataCandidatura = new DBCursorField<DateTime>("DataCandidatura");
            private DBCursorField<int> m_IDCandidato = new DBCursorField<int>("IDCandidato");
            private DBCursorStringField m_NomeCandidato = new DBCursorStringField("NomeCandidato");
            private DBCursorField<int> m_IDCurriculum = new DBCursorField<int>("IDCurriculum");
            private DBCursorField<int> m_IDOfferta = new DBCursorField<int>("IDOfferta");
            private DBCursorStringField m_NomeOfferta = new DBCursorStringField("NomeOfferta");
            private DBCursorField<int> m_IDCanale = new DBCursorField<int>("IDCanale");
            private DBCursorStringField m_NomeCanale = new DBCursorStringField("NomeCanale");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorStringField m_NomeFonte = new DBCursorStringField("NomeFonte");
            private DBCursorField<DateTime> m_DataNascita = new DBCursorField<DateTime>("DataNascita");
            private DBCursorStringField m_NatoA_Citta = new DBCursorStringField("NatoA_Comune");
            private DBCursorStringField m_NatoA_Provincia = new DBCursorStringField("NatoA_Provincia");
            private DBCursorStringField m_ResidenteA_Citta = new DBCursorStringField("ResidenteA_Citta");
            private DBCursorStringField m_ResidenteA_CAP = new DBCursorStringField("ResidenteA_CAP");
            private DBCursorStringField m_ResidenteA_Provincia = new DBCursorStringField("ResidenteA_Provincia");
            private DBCursorStringField m_ResidenteA_Via = new DBCursorStringField("ResidenteA_Via");
            private DBCursorStringField m_ResidenteA_Civico = new DBCursorStringField("ResidenteA_Civico");
            private DBCursorStringField m_Telefono = new DBCursorStringField("Telefono");
            private DBCursorStringField m_eMail = new DBCursorStringField("eMail");
            private DBCursorField<int> m_Valutazione = new DBCursorField<int>("Valutazione");
            private DBCursorField<int> m_ValutatoDaID = new DBCursorField<int>("ValutatoDaID");
            private DBCursorStringField m_ValutatoDaNome = new DBCursorStringField("ValutatoDaNome");
            private DBCursorField<DateTime> m_ValutatoIl = new DBCursorField<DateTime>("ValutatoIl");
            private DBCursorStringField m_MotivoValutazione = new DBCursorStringField("MotivoValutazione");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CandidaturaCursor()
            {
            }

            /// <summary>
            /// DataNascita
            /// </summary>
            public DBCursorField<DateTime> DataNascita
            {
                get
                {
                    return m_DataNascita;
                }
            }

            /// <summary>
            /// NatoA_Citta
            /// </summary>
            public DBCursorStringField NatoA_Citta
            {
                get
                {
                    return m_NatoA_Citta;
                }
            }

            /// <summary>
            /// NatoA_Provincia
            /// </summary>
            public DBCursorStringField NatoA_Provincia
            {
                get
                {
                    return m_NatoA_Provincia;
                }
            }

            /// <summary>
            /// ResidenteA_Citta
            /// </summary>
            public DBCursorStringField ResidenteA_Citta
            {
                get
                {
                    return m_ResidenteA_Citta;
                }
            }

            /// <summary>
            /// ResidenteA_CAP
            /// </summary>
            public DBCursorStringField ResidenteA_CAP
            {
                get
                {
                    return m_ResidenteA_CAP;
                }
            }

            /// <summary>
            /// ResidenteA_Provincia
            /// </summary>
            public DBCursorStringField ResidenteA_Provincia
            {
                get
                {
                    return m_ResidenteA_Provincia;
                }
            }

            /// <summary>
            /// ResidenteA_Via
            /// </summary>
            public DBCursorStringField ResidenteA_Via
            {
                get
                {
                    return m_ResidenteA_Via;
                }
            }

            /// <summary>
            /// ResidenteA_Civico
            /// </summary>
            public DBCursorStringField ResidenteA_Civico
            {
                get
                {
                    return m_ResidenteA_Civico;
                }
            }

            /// <summary>
            /// Telefono
            /// </summary>
            public DBCursorStringField Telefono
            {
                get
                {
                    return m_Telefono;
                }
            }

            /// <summary>
            /// eMail
            /// </summary>
            public DBCursorStringField eMail
            {
                get
                {
                    return m_eMail;
                }
            }

            /// <summary>
            /// Valutazione
            /// </summary>
            public DBCursorField<int> Valutazione
            {
                get
                {
                    return m_Valutazione;
                }
            }

            /// <summary>
            /// ValutatoDaID
            /// </summary>
            public DBCursorField<int> ValutatoDaID
            {
                get
                {
                    return m_ValutatoDaID;
                }
            }

            /// <summary>
            /// ValutatoDaNome
            /// </summary>
            public DBCursorStringField ValutatoDaNome
            {
                get
                {
                    return m_ValutatoDaNome;
                }
            }

            /// <summary>
            /// ValutatoIl
            /// </summary>
            public DBCursorField<DateTime> ValutatoIl
            {
                get
                {
                    return m_ValutatoIl;
                }
            }

            /// <summary>
            /// MotivoValutazione
            /// </summary>
            public DBCursorStringField MotivoValutazione
            {
                get
                {
                    return m_MotivoValutazione;
                }
            }

            /// <summary>
            /// IDCanale
            /// </summary>
            public DBCursorField<int> IDCanale
            {
                get
                {
                    return m_IDCanale;
                }
            }

            /// <summary>
            /// NomeCanale
            /// </summary>
            public DBCursorStringField NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }
            }

            /// <summary>
            /// TipoFonte
            /// </summary>
            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }
            }

            /// <summary>
            /// IDFonte
            /// </summary>
            public DBCursorField<int> IDFonte
            {
                get
                {
                    return m_IDFonte;
                }
            }

            /// <summary>
            /// NomeFonte
            /// </summary>
            public DBCursorStringField NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }
            }

            /// <summary>
            /// DataCandidatura
            /// </summary>
            public DBCursorField<DateTime> DataCandidatura
            {
                get
                {
                    return m_DataCandidatura;
                }
            }

            /// <summary>
            /// IDCandidato
            /// </summary>
            public DBCursorField<int> IDCandidato
            {
                get
                {
                    return m_IDCandidato;
                }
            }

            /// <summary>
            /// NomeCandidato
            /// </summary>
            public DBCursorStringField NomeCandidato
            {
                get
                {
                    return m_NomeCandidato;
                }
            }

            /// <summary>
            /// IDOfferta
            /// </summary>
            public DBCursorField<int> IDOfferta
            {
                get
                {
                    return m_IDOfferta;
                }
            }

            /// <summary>
            /// NomeOfferta
            /// </summary>
            public DBCursorStringField NomeOfferta
            {
                get
                {
                    return m_NomeOfferta;
                }
            }

            /// <summary>
            /// IDCurriculum
            /// </summary>
            public DBCursorField<int> IDCurriculum
            {
                get
                {
                    return m_IDCurriculum;
                }
            }

            /// <summary>
            /// Restituisce l'elenco dei campi da inserire nel cursore
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldsCollection GetCursorFields()
            {
                var ret = base.GetCursorFields();
                ret.Remove(this.m_Telefono);
                return ret;
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Candidature;
            }

            /// <summary>
            /// Restituisce la clausola where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var ret = base.GetWherePart();
                if (m_Telefono.IsSet())
                {
                    //TODO usare il campo intero (specializzare il campo con il nuovo nome)
                    ret *= (this.Field("Telefono1").EQ(this.m_Telefono.Value))
                           + (this.Field("Telefono2").EQ(this.m_Telefono.Value))
                            ;
                }

                return ret;
            }
        }
    }
}