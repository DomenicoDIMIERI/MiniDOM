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
        /// Cursore sulla tabella dei scansioni
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CartucceTonerCursor 
            : minidom.Databases.DBObjectCursorPO<CartucciaToner>
        {
            private DBCursorField<int> m_IDArticolo = new DBCursorField<int>("IDArticolo");
            private DBCursorStringField m_NomeArticolo = new DBCursorStringField("NomeArticolo");
            private DBCursorStringField m_CodiceArticolo = new DBCursorStringField("CodiceArticolo");
            private DBCursorStringField m_Modello = new DBCursorStringField("Modello");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_IDPostazione = new DBCursorField<int>("IDPostazione");
            private DBCursorStringField m_NomePostazione = new DBCursorStringField("NomePostazione");
            private DBCursorField<DateTime> m_DataAcquisto = new DBCursorField<DateTime>("DataAcquisto");
            private DBCursorField<DateTime> m_DataInstallazione = new DBCursorField<DateTime>("DataInstallazione");
            private DBCursorField<DateTime> m_DataEsaurimento = new DBCursorField<DateTime>("DataEsaurimento");
            private DBCursorField<DateTime> m_DataRimozione = new DBCursorField<DateTime>("DataRimozione");
            private DBCursorField<int> m_StampeDisponibili = new DBCursorField<int>("StampeDisponibili");
            private DBCursorField<int> m_StampeEffettuate = new DBCursorField<int>("StampeEffettuate");
            private DBCursorField<CartucciaTonerFlags> m_Flags = new DBCursorField<CartucciaTonerFlags>("Flags");
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CartucceTonerCursor()
            {
            }

            /// <summary>
            /// IDArticolo
            /// </summary>
            public DBCursorField<int> IDArticolo
            {
                get
                {
                    return m_IDArticolo;
                }
            }

            /// <summary>
            /// NomeArticolo
            /// </summary>
            public DBCursorStringField NomeArticolo
            {
                get
                {
                    return m_NomeArticolo;
                }
            }

            /// <summary>
            /// CodiceArticolo
            /// </summary>
            public DBCursorStringField CodiceArticolo
            {
                get
                {
                    return m_CodiceArticolo;
                }
            }

            /// <summary>
            /// Modello
            /// </summary>
            public DBCursorStringField Modello
            {
                get
                {
                    return m_Modello;
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
            /// IDPostazione
            /// </summary>
            public DBCursorField<int> IDPostazione
            {
                get
                {
                    return m_IDPostazione;
                }
            }

            /// <summary>
            /// NomePostazione
            /// </summary>
            public DBCursorStringField NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }
            }

            /// <summary>
            /// DataAcquisto
            /// </summary>
            public DBCursorField<DateTime> DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }
            }

            /// <summary>
            /// DataInstallazione
            /// </summary>
            public DBCursorField<DateTime> DataInstallazione
            {
                get
                {
                    return m_DataInstallazione;
                }
            }

            /// <summary>
            /// DataEsaurimento
            /// </summary>
            public DBCursorField<DateTime> DataEsaurimento
            {
                get
                {
                    return m_DataEsaurimento;
                }
            }

            /// <summary>
            /// DataRimozione
            /// </summary>
            public DBCursorField<DateTime> DataRimozione
            {
                get
                {
                    return m_DataRimozione;
                }
            }

            /// <summary>
            /// StampeDisponibili
            /// </summary>
            public DBCursorField<int> StampeDisponibili
            {
                get
                {
                    return m_StampeDisponibili;
                }
            }

            /// <summary>
            /// StampeEffettuate
            /// </summary>
            public DBCursorField<int> StampeEffettuate
            {
                get
                {
                    return m_StampeEffettuate;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<CartucciaTonerFlags> Flags
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
                return minidom.Office.CartucceToners;
            }
             
        }
    }
}