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
        /// Cursore sulla tabella dei cessionari
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCessionariCursor 
            : Databases.DBObjectCursor<CCQSPDCessionarioClass>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_ImagePath = new DBCursorStringField("ImagePath"); // Percorso dell'immagine
            private DBCursorField<bool> m_Visibile = new DBCursorField<bool>("Visibile"); // Se vero il cessionario viene mostrato negli elenchi 
            private DBCursorStringField m_Preventivatore = new DBCursorStringField("Preventivatore"); // Percorso dello script di backdoor alla pagina di preventivazione
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<CessionarioFlags> m_Flags = new DBCursorField<CessionarioFlags>("Flags");
            private bool m_OnlyValid = false;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCessionariCursor()
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
                    m_OnlyValid = value;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<CessionarioFlags> Flags
            {
                get
                {
                    return m_Flags;
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
            /// ImagePath
            /// </summary>
            public DBCursorStringField ImagePath
            {
                get
                {
                    return m_ImagePath;
                }
            }

            /// <summary>
            /// Visibile
            /// </summary>
            public DBCursorField<bool> Visibile
            {
                get
                {
                    return m_Visibile;
                }
            }

            /// <summary>
            /// Preventivatore
            /// </summary>
            public DBCursorStringField Preventivatore
            {
                get
                {
                    return m_Preventivatore;
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
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CCQSPDCessionarioClass InstantiateNewT(DBReader dbRis)
            {
                return new CCQSPDCessionarioClass();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Cessionari";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Cessionari;
            }

            /// <summary>
            /// Where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var wherePart = base.GetWherePart();
                if (this.m_OnlyValid)
                {
                    var today = DMD.DateUtils.ToDay();
                    wherePart *= DBCursorField.Field("DataInizio").IsNull() + DBCursorField.Field("DataInizio").LE(today);
                    wherePart *= DBCursorField.Field("DataFine").IsNull() + DBCursorField.Field("DataFine").GE(today);
                }

                return wherePart;                 
            }



            //protected override Databases.CDBConnection GetConnection()
            //{
            //    return Database;
            //}

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_OnlyValid", m_OnlyValid);
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
                    case "m_OnlyValid": this.m_OnlyValid = (bool) DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }
        }


    }
}