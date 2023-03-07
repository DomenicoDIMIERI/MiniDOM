using System;
using System.Collections.Generic;
using DMD;
using DMD.Databases;
using DMD.XML;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle fonti
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CFontiCursor 
            : minidom.Databases.DBObjectCursor<CFonte>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<bool> m_Attiva = new DBCursorField<bool>("Attiva");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorStringField m_IDCampagna = new DBCursorStringField("IDCampagna");
            private DBCursorStringField m_IDAnnuncio = new DBCursorStringField("IDAnnuncio");
            private DBCursorStringField m_IDKeyWord = new DBCursorStringField("IDKeyWord");
            private DBCursorStringField m_Siti = new DBCursorStringField("Siti");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private bool m_OnlyValid;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFontiCursor()
            {
                m_OnlyValid = false;
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
            /// Attiva
            /// </summary>
            public DBCursorField<bool> Attiva
            {
                get
                {
                    return m_Attiva;
                }
            }

            /// <summary>
            /// IDCampagna
            /// </summary>
            public DBCursorStringField IDCampagna
            {
                get
                {
                    return m_IDCampagna;
                }
            }

            /// <summary>
            /// IDAnnuncio
            /// </summary>
            public DBCursorStringField IDAnnuncio
            {
                get
                {
                    return m_IDAnnuncio;
                }
            }

            /// <summary>
            /// IDKeyWord
            /// </summary>
            public DBCursorStringField IDKeyWord
            {
                get
                {
                    return m_IDKeyWord;
                }
            }

            /// <summary>
            /// Siti
            /// </summary>
            public DBCursorStringField Siti
            {
                get
                {
                    return m_Siti;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return this.m_Flags;
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il cursore deve restituire solo gli oggetti validi alla data odierna
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
            /// Aggiunge i vincoli aggiontivi 
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();
                if (this.OnlyValid)
                {
                    var oggi = DMD.DateUtils.Now();
                    ret *= this.Field("DataInizio").IsNull() + this.Field("DataInizio").LE(oggi);
                    ret *= this.Field("DataFine").IsNull() + this.Field("DataFine").GT(oggi);
                }
                return ret;
            }


       

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Fonti; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_FontiContatto";
            //}

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
        }
    }
}