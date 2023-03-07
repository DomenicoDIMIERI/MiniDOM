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
        /// Cursore sulla tabella delle richieste di conteggi / quote
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RichiestaCERQCursor 
            : Databases.DBObjectCursorPO<RichiestaCERQ>
        {
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_TipoRichiesta = new DBCursorStringField("TipoRichiesta");
            private DBCursorField<int> m_IDAmministrazione = new DBCursorField<int>("IDAmministrazione");
            private DBCursorStringField m_NomeAmministrazione = new DBCursorStringField("NomeAmministrazione");
            private DBCursorStringField m_RichiestaAMezzo = new DBCursorStringField("RichiestaAMezzo");
            private DBCursorStringField m_RichiestaAIndirizzo = new DBCursorStringField("RichiestaAIndirizzo");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<StatoRichiestaCERQ> m_StatoOperazione = new DBCursorField<StatoRichiestaCERQ>("StatoOperazione");
            private DBCursorField<DateTime> m_DataPrevista = new DBCursorField<DateTime>("DataPrevista");
            private DBCursorField<DateTime> m_DataEffettiva = new DBCursorField<DateTime>("DataEffettiva");
            private DBCursorField<int> m_IDOperatoreEffettivo = new DBCursorField<int>("IDOperatoreEffettivo");
            private DBCursorStringField m_NomeOperatoreEffettivo = new DBCursorStringField("NomeOperatoreEffettivo");
            private DBCursorField<int> m_IDCommissione = new DBCursorField<int>("IDCommissione");
            private DBCursorStringField m_ContextType = new DBCursorStringField("ContextType");
            private DBCursorField<int> m_ContextID = new DBCursorField<int>("ContextID");
            private DBCursorField<int> m_IDAziendaRichiedente = new DBCursorField<int>("IDAziendaRichiedente");
            private DBCursorStringField m_NomeAziendaRichiedente = new DBCursorStringField("NomeAziendaRichiedente");
            private DBCursorField<int> m_IDDocumentoProdotto = new DBCursorField<int>("IDOggettoProdotto");
            private DBCursorStringField m_TipoDocumentoProdotto = new DBCursorStringField("TipoOggettoProdotto");

            public RichiestaCERQCursor()
            {
            }

            public DBCursorStringField NomeAziendaRichiedente
            {
                get
                {
                    return m_NomeAziendaRichiedente;
                }
            }

            public DBCursorField<int> IDAziendaRichiedente
            {
                get
                {
                    return m_IDAziendaRichiedente;
                }
            }

            public DBCursorStringField ContextType
            {
                get
                {
                    return m_ContextType;
                }
            }

            public DBCursorField<int> ContextID
            {
                get
                {
                    return m_ContextID;
                }
            }

            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            public DBCursorStringField TipoRichiesta
            {
                get
                {
                    return m_TipoRichiesta;
                }
            }

            public DBCursorField<int> IDAmministrazione
            {
                get
                {
                    return m_IDAmministrazione;
                }
            }

            public DBCursorStringField NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }
            }

            public DBCursorStringField RichiestaAMezzo
            {
                get
                {
                    return m_RichiestaAMezzo;
                }
            }

            public DBCursorStringField RichiestaAIndirizzo
            {
                get
                {
                    return m_RichiestaAIndirizzo;
                }
            }

            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            public DBCursorField<StatoRichiestaCERQ> StatoOperazione
            {
                get
                {
                    return m_StatoOperazione;
                }
            }

            public DBCursorField<DateTime> DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }
            }

            public DBCursorField<DateTime> DataEffettiva
            {
                get
                {
                    return m_DataEffettiva;
                }
            }

            public DBCursorField<int> IDOperatoreEffettivo
            {
                get
                {
                    return m_IDOperatoreEffettivo;
                }
            }

            public DBCursorStringField NomeOperatoreEffettivo
            {
                get
                {
                    return m_NomeOperatoreEffettivo;
                }
            }

            public DBCursorField<int> IDCommissione
            {
                get
                {
                    return m_IDCommissione;
                }
            }

            public DBCursorField<int> IDDocumentoProdotto
            {
                get
                {
                    return m_IDDocumentoProdotto;
                }
            }

            public DBCursorStringField TipoDocumentoProdotto
            {
                get
                {
                    return m_TipoDocumentoProdotto;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return RichiesteCERQ.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDRichCERQ";
            }

            public override object Add()
            {
                RichiestaCERQ ret = (RichiestaCERQ)base.Add();
                ret.Data = DMD.DateUtils.Now();
                ret.Operatore = Sistema.Users.CurrentUser;
                ret.AziendaRichiedente = Anagrafica.Aziende.AziendaPrincipale;
                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}