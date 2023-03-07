using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CRichiesteFinanziamentoCursor : Databases.DBObjectCursorPO<CRichiestaFinanziamento>
        {
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorStringField m_NomeFonte = new DBCursorStringField("NomeFonte");
            private DBCursorField<int> m_IDCanale = new DBCursorField<int>("IDCanale");
            private DBCursorStringField m_NomeCanale = new DBCursorStringField("NomeCanale");
            private DBCursorField<int> m_IDCanale1 = new DBCursorField<int>("IDCanale1");
            private DBCursorStringField m_NomeCanale1 = new DBCursorStringField("NomeCanale1");
            private DBCursorStringField m_Referrer = new DBCursorStringField("Referrer");
            private DBCursorField<decimal> m_ImportoRichiesto = new DBCursorField<decimal>("ImportoRichiesto");
            private DBCursorField<decimal> m_RataMassima = new DBCursorField<decimal>("RataMassima");
            private DBCursorField<int> m_DurataMassima = new DBCursorField<int>("DurataMassima");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<int> m_IDAssegnatoA = new DBCursorField<int>("IDAssegnatoA");
            private DBCursorStringField m_NomeAssegnatoA = new DBCursorStringField("NomeAssegnatoA");
            private DBCursorField<int> m_IDPresaInCaricoDa = new DBCursorField<int>("IDPresaInCaricoDa");
            private DBCursorStringField m_NomePresaInCarocoDa = new DBCursorStringField("NomePresaInCaricoDa");
            private DBCursorStringField m_IDFonteStr = new DBCursorStringField("IDFonteStr");
            private DBCursorStringField m_IDCampagnaStr = new DBCursorStringField("IDCampagnaStr");
            private DBCursorStringField m_IDAnnuncioStr = new DBCursorStringField("IDAnnuncioStr");
            private DBCursorStringField m_IDKeyWordStr = new DBCursorStringField("IDKeyWordStr");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<StatoRichiestaFinanziamento> m_StatoRichiesta = new DBCursorField<StatoRichiestaFinanziamento>("StatoRichiesta");
            private DBCursorField<TipoRichiestaFinanziamento> m_TipoRichiesta = new DBCursorField<TipoRichiestaFinanziamento>("TipoRichiesta");
            private DBCursorField<decimal> m_ImportoRichiesto1 = new DBCursorField<decimal>("ImportoRichiesto1");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorField<double> m_Durata = new DBCursorField<double>("Durata");
            private DBCursorField<RichiestaFinanziamentoFlags> m_Flags = new DBCursorField<RichiestaFinanziamentoFlags>("Flags");
            private DBCursorField<int> m_IDFinestraLavorazione = new DBCursorField<int>("IDFinestraLavorazione");
            private DBCursorStringField m_Scopo = new DBCursorStringField("Scopo");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private DBCursorStringField m_NomeCollaboratore = new DBCursorStringField("NomeCollaboratore");

            public CRichiesteFinanziamentoCursor()
            {
            }

            public DBCursorField<int> IDCollaboratore
            {
                get
                {
                    return m_IDCollaboratore;
                }
            }

            public DBCursorStringField NomeCollaboratore
            {
                get
                {
                    return m_NomeCollaboratore;
                }
            }

            public DBCursorStringField Scopo
            {
                get
                {
                    return m_Scopo;
                }
            }

            public DBCursorField<int> IDFinestraLavorazione
            {
                get
                {
                    return m_IDFinestraLavorazione;
                }
            }

            public DBCursorField<RichiestaFinanziamentoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<double> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            public DBCursorField<TipoRichiestaFinanziamento> TipoRichiesta
            {
                get
                {
                    return m_TipoRichiesta;
                }
            }

            public DBCursorField<StatoRichiestaFinanziamento> StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }
            }

            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            public DBCursorField<int> IDFonte
            {
                get
                {
                    return m_IDFonte;
                }
            }

            public DBCursorStringField NomeFonte
            {
                get
                {
                    return m_NomeFonte;
                }
            }

            public DBCursorField<int> IDCanale
            {
                get
                {
                    return m_IDCanale;
                }
            }

            public DBCursorField<int> IDCanale1
            {
                get
                {
                    return m_IDCanale1;
                }
            }

            public DBCursorStringField NomeCanale
            {
                get
                {
                    return m_NomeCanale;
                }
            }

            public DBCursorStringField NomeCanale1
            {
                get
                {
                    return m_NomeCanale;
                }
            }

            public DBCursorStringField Referrer
            {
                get
                {
                    return m_Referrer;
                }
            }

            public DBCursorField<decimal> ImportoRichiesto
            {
                get
                {
                    return m_ImportoRichiesto;
                }
            }

            public DBCursorField<decimal> ImportoRichiesto1
            {
                get
                {
                    return m_ImportoRichiesto1;
                }
            }

            public DBCursorField<decimal> RataMassima
            {
                get
                {
                    return m_RataMassima;
                }
            }

            public DBCursorField<int> DurataMassima
            {
                get
                {
                    return m_DurataMassima;
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

            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            public DBCursorField<int> IDAssegnatoA
            {
                get
                {
                    return m_IDAssegnatoA;
                }
            }

            public DBCursorStringField NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
                }
            }

            public DBCursorField<int> IDPresaInCaricoDa
            {
                get
                {
                    return m_IDPresaInCaricoDa;
                }
            }

            public DBCursorStringField NomePresaInCarocoDa
            {
                get
                {
                    return m_NomePresaInCarocoDa;
                }
            }

            public DBCursorStringField IDFonteStr
            {
                get
                {
                    return m_IDFonteStr;
                }
            }

            public DBCursorStringField IDCampagnaStr
            {
                get
                {
                    return m_IDCampagnaStr;
                }
            }

            public DBCursorStringField IDAnnuncioStr
            {
                get
                {
                    return m_IDAnnuncioStr;
                }
            }

            public DBCursorStringField IDKeyWordStr
            {
                get
                {
                    return m_IDKeyWordStr;
                }
            }

            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override Sistema.CModule GetModule()
            {
                return RichiesteFinanziamento.Module;
            }

            public override string GetTableName()
            {
                return "tbl_RichiesteFinanziamenti";
            }
        }
    }
}