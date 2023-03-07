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
using static minidom.Finanziaria;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="CEstinzione"/>
        /// </summary>
        [Serializable]
        public class CEstinzioniCursor
            : minidom.Databases.DBObjectCursorPO<CEstinzione>
        {
            private DBCursorField<TipoEstinzione> m_Tipo = new DBCursorField<TipoEstinzione>("Tipo");  // [INT]      Un valore intero che indica la tipologia di estinzione
            private DBCursorField<int> m_IDIstituto = new DBCursorField<int>("IDIstituto");   // [INT]      ID dell'istituto con cui il cliente ha stipulato il contratto da estinguere
            private DBCursorStringField m_NomeIstituto = new DBCursorStringField("NomeIstituto"); // [TEXT]     Nome dell'istituto
            private DBCursorStringField m_NomeFiliale = new DBCursorStringField("NomeFiliale"); // [TEXT]     Nome dell'istituto
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio"); // [Date]     Data di inizio del prestito
            private DBCursorField<DateTime> m_DataEstinzione = new DBCursorField<DateTime>("DataEstinzione"); // [Date]     Data di scadenza del prestito
            private DBCursorField<DateTime> m_Scadenza = new DBCursorField<DateTime>("Scadenza"); // [Date]     Data di scadenza del prestito
            private DBCursorField<decimal> m_Rata = new DBCursorField<decimal>("Rata"); // [Double]   
            private DBCursorField<int> m_Durata = new DBCursorField<int>("Durata"); // [INT]      Durata in numero di rate
            private DBCursorField<double> m_TAN = new DBCursorField<double>("TAN"); // [Double]    
            private DBCursorField<double> m_TAEG = new DBCursorField<double>("TAEG"); // [Double]    
            private DBCursorField<bool> m_Estinta = new DBCursorField<bool>("Estingue"); // [Boolean]  Se vero indica che estingue questo prestito
            private DBCursorField<int> m_IDPratica = new DBCursorField<int>("IDPratica");
            private DBCursorField<DateTime> m_DecorrenzaPratica = new DBCursorField<DateTime>("DecorrenzaPratica");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<int> m_IDEstintoDa = new DBCursorField<int>("IDEstintoDa");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");
            private DBCursorStringField m_SourceType = new DBCursorStringField("SourceType");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorStringField m_Numero = new DBCursorStringField("Numero");
            private DBCursorStringField m_NomeAgenzia = new DBCursorStringField("NomeAgenzia");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<DateTime> m_DataRinnovo = new DBCursorField<DateTime>("DataRinnovo");
            private DBCursorField<DateTime> m_DataRicontatto = new DBCursorField<DateTime>("DataRicontatto");
            private DBCursorField<bool> m_Validato = new DBCursorField<bool>("Validato");
            private DBCursorField<DateTime> m_ValidatoIl = new DBCursorField<DateTime>("ValidatoIl");
            private DBCursorField<int> m_IDValidatoDa = new DBCursorField<int>("IDValidatoDa");
            private DBCursorStringField m_NomeValidatoDa = new DBCursorStringField("NomeValidatoDa");
            private DBCursorStringField m_NomeSorgenteValidazione = new DBCursorStringField("NomeSorgenteValidazione");
            private DBCursorStringField m_TipoSorgenteValidazione = new DBCursorStringField("TipoSorgenteValidazione");
            private DBCursorField<int> m_IDSorgenteValidazione = new DBCursorField<int>("IDSorgenteValidazione");
            private DBCursorField<int> m_IDClienteXCollaboratore = new DBCursorField<int>("IDClienteXCollaboratore");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEstinzioniCursor()
            {
            }

            /// <summary>
            /// IDClienteXCollaboratore
            /// </summary>
            public DBCursorField<int> IDClienteXCollaboratore
            {
                get
                {
                    return m_IDClienteXCollaboratore;
                }
            }

            /// <summary>
            /// Validato
            /// </summary>
            public DBCursorField<bool> Validato
            {
                get
                {
                    return m_Validato;
                }
            }

            /// <summary>
            /// ValidatoIl
            /// </summary>
            public DBCursorField<DateTime> ValidatoIl
            {
                get
                {
                    return m_ValidatoIl;
                }
            }

            /// <summary>
            /// IDValidatoDa
            /// </summary>
            public DBCursorField<int> IDValidatoDa
            {
                get
                {
                    return m_IDValidatoDa;
                }
            }

            /// <summary>
            /// NomeValidatoDa
            /// </summary>
            public DBCursorStringField NomeValidatoDa
            {
                get
                {
                    return m_NomeValidatoDa;
                }
            }

            /// <summary>
            /// NomeSorgenteValidazione
            /// </summary>
            public DBCursorStringField NomeSorgenteValidazione
            {
                get
                {
                    return m_NomeSorgenteValidazione;
                }
            }

            /// <summary>
            /// TipoSorgenteValidazione
            /// </summary>
            public DBCursorStringField TipoSorgenteValidazione
            {
                get
                {
                    return m_TipoSorgenteValidazione;
                }
            }

            /// <summary>
            /// IDSorgenteValidazione
            /// </summary>
            public DBCursorField<int> IDSorgenteValidazione
            {
                get
                {
                    return m_IDSorgenteValidazione;
                }
            }

            /// <summary>
            /// DataRinnovo
            /// </summary>
            public DBCursorField<DateTime> DataRinnovo
            {
                get
                {
                    return m_DataRinnovo;
                }
            }

            /// <summary>
            /// DataRicontatto
            /// </summary>
            public DBCursorField<DateTime> DataRicontatto
            {
                get
                {
                    return m_DataRicontatto;
                }
            }

            /// <summary>
            /// DataEstinzione
            /// </summary>
            public DBCursorField<DateTime> DataEstinzione
            {
                get
                {
                    return m_DataEstinzione;
                }
            }

            /// <summary>
            /// Note
            /// </summary>
            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            /// <summary>
            /// Numero
            /// </summary>
            public DBCursorStringField Numero
            {
                get
                {
                    return m_Numero;
                }
            }

            /// <summary>
            /// NomeAgenzia
            /// </summary>
            public DBCursorStringField NomeAgenzia
            {
                get
                {
                    return m_NomeAgenzia;
                }
            }

            /// <summary>
            /// SourceType
            /// </summary>
            public DBCursorStringField SourceType
            {
                get
                {
                    return m_SourceType;
                }
            }

            /// <summary>
            /// SourceID
            /// </summary>
            public DBCursorField<int> SourceID
            {
                get
                {
                    return m_SourceID;
                }
            }


            /// <summary>
            /// DettaglioStato
            /// </summary>
            public DBCursorStringField DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }
            }

            /// <summary>
            /// IDEstintoDa
            /// </summary>
            public DBCursorField<int> IDEstintoDa
            {
                get
                {
                    return m_IDEstintoDa;
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
            /// DecorrenzaPratica
            /// </summary>
            public DBCursorField<DateTime> DecorrenzaPratica
            {
                get
                {
                    return m_DecorrenzaPratica;
                }
            }

            /// <summary>
            /// IDPratica
            /// </summary>
            public DBCursorField<int> IDPratica
            {
                get
                {
                    return m_IDPratica;
                }
            }

            /// <summary>
            /// Tipo
            /// </summary>
            public DBCursorField<TipoEstinzione> Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            /// <summary>
            /// IDIstituto
            /// </summary>
            public DBCursorField<int> IDIstituto
            {
                get
                {
                    return m_IDIstituto;
                }
            }

            /// <summary>
            /// NomeIstituto
            /// </summary>
            public DBCursorStringField NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }
            }

            /// <summary>
            /// NomeFiliale
            /// </summary>
            public DBCursorStringField NomeFiliale
            {
                get
                {
                    return m_NomeFiliale;
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
            /// Scadenza
            /// </summary>
            public DBCursorField<DateTime> Scadenza
            {
                get
                {
                    return m_Scadenza;
                }
            }

            /// <summary>
            /// Rata
            /// </summary>
            public DBCursorField<decimal> Rata
            {
                get
                {
                    return m_Rata;
                }
            }

            /// <summary>
            /// Durata
            /// </summary>
            public DBCursorField<int> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            /// <summary>
            /// TAN
            /// </summary>
            public DBCursorField<double> TAN
            {
                get
                {
                    return m_TAN;
                }
            }

            /// <summary>
            /// TAEG
            /// </summary>
            public DBCursorField<double> TAEG
            {
                get
                {
                    return m_TAEG;
                }
            }

            /// <summary>
            /// Estinta
            /// </summary>
            public DBCursorField<bool> Estinta
            {
                get
                {
                    return m_Estinta;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CEstinzione InstantiateNewT(DBReader dbRis)
            {
                return new CEstinzione();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Estinzioni";
            //}

            /// <summary>
            /// Respository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Estinzioni;
            }
        }
    }
}