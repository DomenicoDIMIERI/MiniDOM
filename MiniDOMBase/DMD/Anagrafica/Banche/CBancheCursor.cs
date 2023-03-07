using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle banche
        /// </summary>
        [Serializable]
        public class CBancheCursor 
            : Databases.DBObjectCursorPO<CBanca>
        {
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_Filiale = new DBCursorStringField("Filiale");
            private DBCursorStringField m_Indirizzo_Citta = new DBCursorStringField("Indirizzo_Citta");
            private DBCursorStringField m_Indirizzo_Provincia = new DBCursorStringField("Indirizzo_Provincia");
            private DBCursorStringField m_Indirizzo_Via = new DBCursorStringField("Indirizzo_Via");
            private DBCursorStringField m_Indirizzo_CAP = new DBCursorStringField("Indirizzo_CAP");
            private DBCursorStringField m_ABI = new DBCursorStringField("ABI");
            private DBCursorStringField m_CAB = new DBCursorStringField("CAB");
            private DBCursorStringField m_SWIFT = new DBCursorStringField("SWIFT");
            private DBCursorField<DateTime> m_DataApertura = new DBCursorField<DateTime>("DataApertura");
            private DBCursorField<DateTime> m_DataChiusura = new DBCursorField<DateTime>("DataChiusura");
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");
            private bool m_OnlyValid = false;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBancheCursor()
            {
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il cursore deve restituire solo gli oggetti validi alla data corrente
            /// </summary>
            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    if (m_OnlyValid == value)
                        return;
                    m_OnlyValid = value;
                    Reset1();
                }
            }

            /// <summary>
            /// DataApertura
            /// </summary>
            public DBCursorField<DateTime> DataApertura
            {
                get
                {
                    return m_DataApertura;
                }
            }

            /// <summary>
            /// DataChiusura
            /// </summary>
            public DBCursorField<DateTime> DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }
            }

            /// <summary>
            /// Attiva
            /// </summary>
            public DBCursorField<bool> Attiva
            {
                get
                {
                    return m_Attivo;
                }
            }

            /// <summary>
            /// Indirizzo_Citta
            /// </summary>
            public DBCursorStringField Indirizzo_Citta
            {
                get
                {
                    return m_Indirizzo_Citta;
                }
            }

            /// <summary>
            /// Indirizzo_Provincia
            /// </summary>
            public DBCursorStringField Indirizzo_Provincia
            {
                get
                {
                    return m_Indirizzo_Provincia;
                }
            }

            /// <summary>
            /// Indirizzo_Via
            /// </summary>
            public DBCursorStringField Indirizzo_Via
            {
                get
                {
                    return m_Indirizzo_Via;
                }
            }

            /// <summary>
            /// Indirizzo_CAP
            /// </summary>
            public DBCursorStringField Indirizzo_CAP
            {
                get
                {
                    return m_Indirizzo_CAP;
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
            /// Filiale
            /// </summary>
            public DBCursorStringField Filiale
            {
                get
                {
                    return m_Filiale;
                }
            }

            // Public ReadOnly Property Indirizzo As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_Indirizzo
            // End Get
            // End Property

            /// <summary>
            /// ABI
            /// </summary>
            public DBCursorStringField ABI
            {
                get
                {
                    return m_ABI;
                }
            }

            /// <summary>
            /// CAB
            /// </summary>
            public DBCursorStringField CAB
            {
                get
                {
                    return m_CAB;
                }
            }

            /// <summary>
            /// SWIFT
            /// </summary>
            public DBCursorStringField SWIFT
            {
                get
                {
                    return m_SWIFT;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Restituisce un riferimento al repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Banche; //.Module;
            }

            ///// <summary>
            ///// Restituisce il nome del discriminante nel repository
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_Banche";
            //}

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OnlyValid", m_OnlyValid);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OnlyValid":
                        {
                            m_OnlyValid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            
            /// <summary>
            /// Aggiunge le clausole aggiuntive
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();

                if (this.OnlyValid)
                {
                    ret *= (this.Field("DataApertura").IsNull() + this.Field("DataApertura").LE(DMD.DateUtils.ToDay()));
                    ret *= (this.Field("DataChiusura").IsNull() + this.Field("DataChiusura").GE(DMD.DateUtils.ToDay()));
                }

                return ret;
            }
             
        }
    }
}