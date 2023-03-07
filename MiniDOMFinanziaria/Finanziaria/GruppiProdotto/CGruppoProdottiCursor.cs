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
        /// Cursore sugli oggetti di tipo <see cref="CGruppoProdotti"/>
        /// </summary>
        [Serializable]
        public class CGruppoProdottiCursor
            : minidom.Databases.DBObjectCursor<CGruppoProdotti>
        {
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_CessionarioID = new DBCursorField<int>("Cessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorStringField m_TipoRapportoID = new DBCursorStringField("TipoRapporto");
            private DBCursorStringField m_TipoContrattoID = new DBCursorStringField("TipoContratto");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<bool> m_Visible = new DBCursorField<bool>("Visible");
            private DBCursorField<int> m_IDStatoIniziale = new DBCursorField<int>("IDStatoIniziale");
            private bool m_OnlyValid;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CGruppoProdottiCursor()
            {
                m_OnlyValid = false;
            }
             
            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il cursore deve restituire solo gli oggetti validi
            /// </summary>
            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    m_OnlyValid = value;
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
            /// Visible
            /// </summary>
            public DBCursorField<bool> Visible
            {
                get
                {
                    return m_Visible;
                }
            }

            /// <summary>
            /// NomeCessionario
            /// </summary>
            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            /// <summary>
            /// CessionarioID
            /// </summary>
            public DBCursorField<int> CessionarioID
            {
                get
                {
                    return m_CessionarioID;
                }
            }

            /// <summary>
            /// TipoRapportoID
            /// </summary>
            public DBCursorStringField TipoRapportoID
            {
                get
                {
                    return m_TipoRapportoID;
                }
            }

            /// <summary>
            /// TipoContrattoID
            /// </summary>
            public DBCursorStringField TipoContrattoID
            {
                get
                {
                    return m_TipoContrattoID;
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
            /// DataFine
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.GruppiProdotto;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_GruppiProdotti";
            //}

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CGruppoProdotti();
            //}

            /// <summary>
            /// Inizializza l'oggetto
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(CGruppoProdotti item)
            {
                item.Descrizione = "Nuovo gruppo prodotti";
                base.OnInitialize(item);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OnlyValid", m_OnlyValid);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName)
                {
                    case "OnlyValid": this.m_OnlyValid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }    
                
            }
        }
    }
}