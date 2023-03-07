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
        /// Cursore di <see cref="PersonaPerEMail"/>
        /// </summary>
        [Serializable]
        public class PersonePerEMailCursor 
            : minidom.Databases.DBObjectCursorBase<PersonaPerEMail>
        {
            private int m_IDApplication;
            private DBCursorField<int> m_IDMessaggio = new DBCursorField<int>("IDMessaggio");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<MailFlags> m_Flags = new DBCursorField<MailFlags>("Flags");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorStringField m_Indirizzo = new DBCursorStringField("Indirizzo");
            private DBCursorField<DateTime> m_DataMessaggio = new DBCursorField<DateTime>("DataMessaggio");

            /// <summary>
            /// Costruttore
            /// </summary>
            public PersonePerEMailCursor()
            {
            }

            /// <summary>
            /// IDMessaggio
            /// </summary>
            public DBCursorField<int> IDMessaggio
            {
                get
                {
                    return m_IDMessaggio;
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
            /// Flags
            /// </summary>
            public DBCursorField<MailFlags> Flags
            {
                get
                {
                    return m_Flags;
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
            /// Indirizzo
            /// </summary>
            public DBCursorStringField Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            /// <summary>
            /// DataMessaggio
            /// </summary>
            public DBCursorField<DateTime> DataMessaggio
            {
                get
                {
                    return m_DataMessaggio;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Mails.PersonePerEMail;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("AppID", this.m_IDApplication);
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
                    case "AppID":
                        {
                            m_IDApplication = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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