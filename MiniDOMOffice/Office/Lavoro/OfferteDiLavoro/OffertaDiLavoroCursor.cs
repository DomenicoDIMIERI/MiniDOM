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
        /// Cursore di <see cref="OffertaDiLavoro"/>
        /// </summary>
        /// <remarks></remarks>
        public class OffertaDiLavoroCursor 
            : minidom.Databases.DBObjectCursorPO<OffertaDiLavoro>
        {
            private bool m_OnlyValid;
            private DBCursorField<DateTime> m_DataInserzione = new DBCursorField<DateTime>("DataInserzione");
            private DBCursorField<DateTime> m_ValidaDal = new DBCursorField<DateTime>("ValidaDal");
            private DBCursorField<DateTime> m_ValidaAl = new DBCursorField<DateTime>("ValidaAl");
            private DBCursorField<bool> m_Attiva = new DBCursorField<bool>("Attiva");
            private DBCursorStringField m_NomeOfferta = new DBCursorStringField("NomeOfferta");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");

            /// <summary>
            /// Costruttore
            /// </summary>
            public OffertaDiLavoroCursor()
            {
                m_OnlyValid = false;
            }

            /// <summary>
            /// DataInserzione
            /// </summary>
            public DBCursorField<DateTime> DataInserzione
            {
                get
                {
                    return m_DataInserzione;
                }
            }

            /// <summary>
            /// ValidaDal
            /// </summary>
            public DBCursorField<DateTime> ValidaDal
            {
                get
                {
                    return m_ValidaDal;
                }
            }

            /// <summary>
            /// ValidaAl
            /// </summary>
            public DBCursorField<DateTime> ValidaAl
            {
                get
                {
                    return m_ValidaAl;
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

    
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.OfferteDiLavoro;
            }
        }
    }
}