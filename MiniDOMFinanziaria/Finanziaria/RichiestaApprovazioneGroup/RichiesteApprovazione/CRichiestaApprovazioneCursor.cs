using System;

namespace minidom
{
    public partial class Finanziaria
    {



        /// <summary>
    /// Cursore sulla tabella degli obiettivi
    /// </summary>
    /// <remarks></remarks>
        public class CRichiestaApprovazioneCursor : Databases.DBObjectCursorPO<CRichiestaApprovazione>
        {
            private DBCursorField<int> m_IDGruppo = new DBCursorField<int>("IDGruppo");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDOggettoApprovabile = new DBCursorField<int>("IDOggettoApprovabile");
            private DBCursorStringField m_TipoOggettoApprovabile = new DBCursorStringField("TipoOggettoApprovabile");
            private DBCursorField<DateTime> m_DataRichiestaApprovazione = new DBCursorField<DateTime>("DataRichiestaApprovazione");
            private DBCursorField<int> m_IDUtenteRichiestaApprovazione = new DBCursorField<int>("IDUtenteRichiestaApprovazione");
            private DBCursorStringField m_NomeUtenteRichiestaApprovazione = new DBCursorStringField("NomeUtenteRichiestaApprovazione");
            private DBCursorField<int> m_IDMotivoRichiesta = new DBCursorField<int>("IDMotivoRichiesta");
            private DBCursorStringField m_NomeMotivoRichiesta = new DBCursorStringField("NomeMotivoRichiesta");
            private DBCursorStringField m_DescrizioneRichiesta = new DBCursorStringField("DescrizioneRichiesta");
            private DBCursorStringField m_ParametriRichiesta = new DBCursorStringField("ParametriRichiesta");
            private DBCursorField<StatoRichiestaApprovazione> m_StatoRichiesta = new DBCursorField<StatoRichiestaApprovazione>("StatoRichiesta");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<int> m_IDPresaInCaricoDa = new DBCursorField<int>("IDPresaInCaricoDa");
            private DBCursorStringField m_NomePresaInCaricoDa = new DBCursorStringField("NomePresaInCaricoDa");
            private DBCursorField<DateTime> m_DataConferma = new DBCursorField<DateTime>("DataConferma");
            private DBCursorField<int> m_IDConfermataDa = new DBCursorField<int>("IDConfermataDa");
            private DBCursorStringField m_NomeConfermataDa = new DBCursorStringField("NomeConfermataDa");
            private DBCursorStringField m_MotivoConferma = new DBCursorStringField("MotivoConferma");
            private DBCursorStringField m_DettaglioConferma = new DBCursorStringField("DettaglioConferma");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomePersona");

            public CRichiestaApprovazioneCursor()
            {
            }

            public DBCursorField<int> IDGruppo
            {
                get
                {
                    return m_IDGruppo;
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

            public DBCursorField<int> IDOggettoApprovabile
            {
                get
                {
                    return m_IDOggettoApprovabile;
                }
            }

            public DBCursorStringField TipoOggettoApprovabile
            {
                get
                {
                    return m_TipoOggettoApprovabile;
                }
            }

            public DBCursorField<DateTime> DataRichiestaApprovazione
            {
                get
                {
                    return m_DataRichiestaApprovazione;
                }
            }

            public DBCursorField<int> IDUtenteRichiestaApprovazione
            {
                get
                {
                    return m_IDUtenteRichiestaApprovazione;
                }
            }

            public DBCursorStringField NomeUtenteRichiestaApprovazione
            {
                get
                {
                    return m_NomeUtenteRichiestaApprovazione;
                }
            }

            public DBCursorField<int> IDMotivoRichiesta
            {
                get
                {
                    return m_IDMotivoRichiesta;
                }
            }

            public DBCursorStringField NomeMotivoRichiesta
            {
                get
                {
                    return m_NomeMotivoRichiesta;
                }
            }

            public DBCursorStringField DescrizioneRichiesta
            {
                get
                {
                    return m_DescrizioneRichiesta;
                }
            }

            public DBCursorStringField ParametriRichiesta
            {
                get
                {
                    return m_ParametriRichiesta;
                }
            }

            public DBCursorField<StatoRichiestaApprovazione> StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }
            }

            public DBCursorField<DateTime> DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }
            }

            public DBCursorField<int> IDPresaInCaricoDa
            {
                get
                {
                    return m_IDPresaInCaricoDa;
                }
            }

            public DBCursorStringField NomePresaInCaricoDa
            {
                get
                {
                    return m_NomePresaInCaricoDa;
                }
            }

            public DBCursorField<DateTime> DataConferma
            {
                get
                {
                    return m_DataConferma;
                }
            }

            public DBCursorField<int> IDConfermataDa
            {
                get
                {
                    return m_IDConfermataDa;
                }
            }

            public DBCursorStringField NomeConfermataDa
            {
                get
                {
                    return m_NomeConfermataDa;
                }
            }

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorStringField MotivoConferma
            {
                get
                {
                    return m_MotivoConferma;
                }
            }

            public DBCursorStringField DettaglioConferma
            {
                get
                {
                    return m_DettaglioConferma;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return RichiesteApprovazione.Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDRichiesteApprovazione";
            }
        }
    }
}