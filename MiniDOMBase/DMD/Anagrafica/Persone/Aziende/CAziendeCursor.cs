using System;
using System.Xml.Serialization;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle aziende
        /// </summary>
        [Serializable]
        public class CAziendeCursor 
            : CPersonaCursor
        {
            private DBCursorField<int> m_IDEntePagante = new DBCursorField<int>("IDEntePagante");
            private DBCursorStringField m_NomeEntePagante = new DBCursorStringField("NomeEntePagante");
            private DBCursorField<int> m_ValutazioneGARF = new DBCursorField<int>("GARF");
            private DBCursorStringField m_TipoRapporto = new DBCursorStringField("TipoRapporto");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAziendeCursor()
            {
                this.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                this.TipoPersona.Operator = OP.OP_NE;
            }

            /// <summary>
            /// ValutazioneGARF
            /// </summary>
            public DBCursorField<int> ValutazioneGARF
            {
                get
                {
                    return m_ValutazioneGARF;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CPersona InstantiateNewT(DBReader dbRis)
            {
                return new CAzienda();
            }

            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            [XmlIgnore]
            public new CAzienda Item
            {
                get
                {
                    return (CAzienda)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// IDEntePagante
            /// </summary>
            public DBCursorField<int> IDEntePagante
            {
                get
                {
                    return m_IDEntePagante;
                }
            }

            /// <summary>
            /// NomeEntePagante
            /// </summary>
            public DBCursorStringField NomeEntePagante
            {
                get
                {
                    return m_NomeEntePagante;
                }
            }

            /// <summary>
            /// TipoRapporto
            /// </summary>
            public DBCursorStringField TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Aziende; //.Module;
            }

            /// <summary>
            /// Aggiunge un nuovo oggetto
            /// </summary>
            /// <returns></returns>
            public new CAzienda Add()
            {
                return (CAzienda) base.Add();
            }
        }
    }
}