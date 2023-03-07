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
        [Serializable]
        public class CCollaboratoriCursor 
            : minidom.Databases.DBObjectCursorPO<CCollaboratore>
        {
            private DBCursorField<int> m_PersonaID;
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<int> m_UserID;
            private DBCursorStringField m_NomeUtente = new DBCursorStringField("NomeUtente");
            private DBCursorField<int> m_AttivatoDaID;
            private DBCursorField<DateTime> m_DataAttivazione;
            private DBCursorField<int> m_ReferenteID;
            private DBCursorStringField m_Indirizzo;
            private DBCursorStringField m_NumeroIscrizioneUIF;
            private DBCursorStringField m_NumeroIscrizioneRUI;
            private DBCursorStringField m_NumeroIscrizioneISVAP;
            private bool m_OnlyValid;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCollaboratoriCursor()
            {
                m_PersonaID = new DBCursorField<int>("Persona");
                m_UserID = new DBCursorField<int>("Utente");
                m_AttivatoDaID = new DBCursorField<int>("AttivatoDa");
                m_DataAttivazione = new DBCursorField<DateTime>("DataAttivazione");
                m_ReferenteID = new DBCursorField<int>("Referente");
                m_Indirizzo = new DBCursorStringField("Indirizzo");
                m_NumeroIscrizioneUIF = new DBCursorStringField("NumeroIscrizioneUIF");
                m_NumeroIscrizioneRUI = new DBCursorStringField("NumeroIscrizioneRUI");
                m_NumeroIscrizioneISVAP = new DBCursorStringField("NumeroIscrizioneISVAP");
                m_OnlyValid = false;
            }

            /// <summary>
            /// UserID
            /// </summary>
            public DBCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
                }
            }

            /// <summary>
            /// NomeUtente
            /// </summary>
            public DBCursorStringField NomeUtente
            {
                get
                {
                    return m_NomeUtente;
                }
            }

            /// <summary>
            /// PersonaID
            /// </summary>
            public DBCursorField<int> PersonaID
            {
                get
                {
                    return m_PersonaID;
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
            /// AttivatoDaID
            /// </summary>
            public DBCursorField<int> AttivatoDaID
            {
                get
                {
                    return m_AttivatoDaID;
                }
            }

            /// <summary>
            /// DataAttivazione
            /// </summary>
            public DBCursorField<DateTime> DataAttivazione
            {
                get
                {
                    return m_DataAttivazione;
                }
            }

            /// <summary>
            /// ReferenteID
            /// </summary>
            public DBCursorField<int> ReferenteID
            {
                get
                {
                    return m_ReferenteID;
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
            /// NumeroIscrizioneUIF
            /// </summary>
            public DBCursorStringField NumeroIscrizioneUIF
            {
                get
                {
                    return m_NumeroIscrizioneUIF;
                }
            }

            /// <summary>
            /// NumeroIscrizioneRUI
            /// </summary>
            public DBCursorStringField NumeroIscrizioneRUI
            {
                get
                {
                    return m_NumeroIscrizioneRUI;
                }
            }

            /// <summary>
            /// NumeroIscrizioneISVAP
            /// </summary>
            public DBCursorStringField NumeroIscrizioneISVAP
            {
                get
                {
                    return m_NumeroIscrizioneISVAP;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Collaboratori";
            //}

            /// <summary>
            /// Se true il cursore restituisce solo i collaboratori validi
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

            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Collaboratori;
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CCollaboratore InstantiateNewT(DBReader dbRis)
            {
                return new CCollaboratore();
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("OnlyValid", m_OnlyValid);
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

            public override DBCursorFieldBase GetWherePart()
            {
                var wherePart = base.GetWherePart();
                if (this.m_OnlyValid)
                {
                    var today = DMD.DateUtils.ToDay();
                    wherePart *= Field("DataInizioRapporto").IsNull() + Field("DataInizioRapporto").LE(today);
                    wherePart *= Field("DataFineRapporto").IsNull() + Field("DataFineRapporto").GE(today);
                }
                return wherePart;
            }
        }
    }
}