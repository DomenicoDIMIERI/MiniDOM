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
        /// Cursore sulla tabella dei veicoli
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class VeicoliCursor 
            : minidom.Databases.DBObjectCursorPO<Veicolo>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Modello = new DBCursorStringField("Modello");
            private DBCursorStringField m_Seriale = new DBCursorStringField("Seriale");
            private DBCursorStringField m_Alimentazione = new DBCursorStringField("Alimentazione");
            private DBCursorField<double> m_KmALitro = new DBCursorField<double>("KmALitro");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorField<DateTime> m_DataAcquisto = new DBCursorField<DateTime>("DataAcquisto");
            private DBCursorField<DateTime> m_DataDismissione = new DBCursorField<DateTime>("DataDismissione");
            private DBCursorField<StatoVeicolo> m_StatoVeicolo = new DBCursorField<StatoVeicolo>("StatoVeicolo");
            private DBCursorStringField m_Targa = new DBCursorStringField("Targa");
            private DBCursorField<DateTime> m_DataImmatricolazione = new DBCursorField<DateTime>("DataImmatricolazione");
            private DBCursorField<double> m_ConsumoUrbano = new DBCursorField<double>("ConsumoUrbano");
            private DBCursorField<double> m_ConsumoExtraUrbano = new DBCursorField<double>("ConsumoExtraUrbano");
            private DBCursorField<double> m_ConsumoCombinato = new DBCursorField<double>("ConsumoCombinato");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<VeicoloFlags> m_Flags = new DBCursorField<VeicoloFlags>("Flags");
            private DBCursorField<int> m_IDProprietario = new DBCursorField<int>("IDProprietario");
            private DBCursorStringField m_NomeProprietario = new DBCursorStringField("NomeProprietario");

            /// <summary>
            /// Costruttore
            /// </summary>
            public VeicoliCursor()
            {
            }

            /// <summary>
            /// IDProprietario
            /// </summary>
            public DBCursorField<int> IDProprietario
            {
                get
                {
                    return m_IDProprietario;
                }
            }

            /// <summary>
            /// NomeProprietario
            /// </summary>
            public DBCursorStringField NomeProprietario
            {
                get
                {
                    return m_NomeProprietario;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<VeicoloFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// KmALitro
            /// </summary>
            public DBCursorField<double> KmALitro
            {
                get
                {
                    return m_KmALitro;
                }
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
            /// Tipo
            /// </summary>
            public DBCursorStringField Tipo
            {
                get
                {
                    return m_Tipo;
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
            /// Seriale
            /// </summary>
            public DBCursorStringField Seriale
            {
                get
                {
                    return m_Seriale;
                }
            }

            /// <summary>
            /// Alimentazione
            /// </summary>
            public DBCursorStringField Alimentazione
            {
                get
                {
                    return m_Alimentazione;
                }
            }

            /// <summary>
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
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
            /// DataDismissione
            /// </summary>
            public DBCursorField<DateTime> DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }
            }

            /// <summary>
            /// StatoVeicolo
            /// </summary>
            public DBCursorField<StatoVeicolo> StatoVeicolo
            {
                get
                {
                    return m_StatoVeicolo;
                }
            }

            /// <summary>
            /// Targa
            /// </summary>
            public DBCursorStringField Targa
            {
                get
                {
                    return m_Targa;
                }
            }

            /// <summary>
            /// DataImmatricolazione
            /// </summary>
            public DBCursorField<DateTime> DataImmatricolazione
            {
                get
                {
                    return m_DataImmatricolazione;
                }
            }

            /// <summary>
            /// ConsumoUrbano
            /// </summary>
            public DBCursorField<double> ConsumoUrbano
            {
                get
                {
                    return m_ConsumoUrbano;
                }
            }

            /// <summary>
            /// ConsumoExtraUrbano
            /// </summary>
            public DBCursorField<double> ConsumoExtraUrbano
            {
                get
                {
                    return m_ConsumoExtraUrbano;
                }
            }

            /// <summary>
            /// ConsumoCombinato
            /// </summary>
            public DBCursorField<double> ConsumoCombinato
            {
                get
                {
                    return m_ConsumoCombinato;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Veicoli;
            }

          
        }
    }
}