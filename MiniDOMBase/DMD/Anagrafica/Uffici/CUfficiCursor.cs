using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Cursore sulla tabella degli uffici
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUfficiCursor 
            : minidom.Databases.DBObjectCursor<CUfficio>
        {
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorStringField m_NomeAzienda = new DBCursorStringField("NomeAzienda");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<bool> m_Attivo = new DBCursorField<bool>("Attivo");
            private DBCursorField<DateTime> m_DataApertura = new DBCursorField<DateTime>("DataApertura");
            private DBCursorField<DateTime> m_DataChiusura = new DBCursorField<DateTime>("DataChiusura");
            private DBCursorStringField m_CodiceFiscale = new DBCursorStringField("CodiceFiscale");
            private DBCursorField<UfficioFlags> m_Flags = new DBCursorField<UfficioFlags>("Flags");
            private bool m_OnlyValid = false;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUfficiCursor()
            {
            }

            /// <summary>
            /// OnlyValid
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
            /// Flags
            /// </summary>
            public DBCursorField<UfficioFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Attivo
            /// </summary>
            public DBCursorField<bool> Attivo
            {
                get
                {
                    return m_Attivo;
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
            /// CodiceFiscale
            /// </summary>
            public DBCursorStringField CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }
            }

            /// <summary>
            /// IDAzienda
            /// </summary>
            public DBCursorField<int> IDAzienda
            {
                get
                {
                    return m_IDAzienda;
                }
            }

            /// <summary>
            /// NomeAzienda
            /// </summary>
            public DBCursorStringField NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
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
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CUfficio InstantiateNewT(DBReader dbRis)
            {
                return new CUfficio();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_AziendaUffici";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Uffici; //.Module;
            }

            /// <summary>
            /// Vincoli aggiuntivi
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var ret = base.GetWherePart();

                if (this.OnlyValid)
                {
                    var oggi = DMD.DateUtils.ToDay();
                    ret *= Field("DataApertura").IsNull() + Field("DataApertura").LE(oggi);
                    ret *= Field("DataChiusura").IsNull() + Field("DataChiusura").GT(oggi);
                    ret *= Field("Attivo").IsNull() + Field("Attivo").EQ(true);
                }

                return ret;
            }
        }
    }
}